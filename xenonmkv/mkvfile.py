import subprocess
import fractions
import os

import xenonmkv.text

from xenonmkv.reference_frame import ReferenceFrameValidator
from xenonmkv.mkv_info_parser import MKVInfoParser
from xenonmkv.process_handler import ProcessHandler
from xenonmkv.track import MKVTrack, AudioTrack, VideoTrack, UnsupportedCodecError


class MKVFile():
    path = ""
    tracks = {}
    duration = 0

    video_track_id = audio_track_id = 0

    log = args = None

    def __init__(self, path, log, args):
        self.path = path
        self.log = log
        self.args = args

    def get_path(self):
        return self.path

    # Open the mkvinfo process and parse its output.
    def get_mkvinfo(self):
        mkvinfo_args = [self.args.tool_paths["mkvinfo"], "--ui-language", "en_US", self.get_path()]
        self.log.debug("Executing 'mkvinfo {0}'".format(' '.join(mkvinfo_args)))
        try:
            # Force mkvinfo to output details in English; corrected version of pull request 
            # <https://github.com/jbillo/xenonmkv/pull/14>
            result = subprocess.check_output(mkvinfo_args)
        except subprocess.CalledProcessError as e:
            self.log.debug("mkvinfo process error: {0}".format(e.output))
            raise Exception("Error occurred while obtaining MKV information "
                            "for {0} - please make sure the file exists, is readable and "
                            "a valid MKV file".format(self.get_path()))

        self.log.debug("mkvinfo finished; attempting to parse output")
        try:
            self.parse_mkvinfo(result)
        except:
            # Punt back exception from inner function to the main application
            raise

    # Open the mediainfo process to obtain detailed info on the file.
    def get_mediainfo(self, track_type):
        if track_type == "video":
            parameters = ("Video;%ID%,%Height%,%Width%,"
                          "%Format_Settings_RefFrames%,%Language%,%FrameRate%,%CodecID%,"
                          "%DisplayAspectRatio%,%FrameRate_Original%,~")
        else:
            parameters = "Audio;%ID%,%CodecID%,%Language%,%Channels%,~"

        subprocess_args = [self.args.tool_paths["mediainfo"],
                           "--Inform=" + parameters, self.get_path()]
        self.log.debug("Executing '{0}'".format(' '.join(subprocess_args)))
        result = subprocess.check_output(subprocess_args)
        self.log.debug("mediainfo finished; attempting to parse output "
                       "for {0} settings".format(track_type))
        return self.parse_mediainfo(result)

    def parse_mediainfo(self, result):
        output = []
        result = result.replace(os.linesep, "")

        # Obtain multiple tracks if they are present
        lines = result.split("~")
        lines = lines[0:-1]  # remove last tilde separator character

        for line in lines:
            # remove last element from array that will always be present
            values = line.split(",")
            # print values
            output.append(values)

        return output

    # Return a float value specifying the display aspect ratio.
    def parse_display_aspect_ratio(self, dar_string):
        self.log.debug("Attempting to parse display aspect ratio '{0}'".format(
                       dar_string))
        if "16/9" in dar_string:
            return 1.778
        elif "4/3" in dar_string:
            return 1.333
        elif "/" in dar_string:
            # Halfass some math and try to get an approximate number.
            try:
                numerator = int(dar_string[0:dar_string.index("/")])
                denominator = int(dar_string[dar_string.index("/") + 1:])
                return numerator / denominator
            except:
                # Couldn't divide
                raise Exception("Could not parse display aspect ratio of {0}".format(
                                dar_string))
        else:
            return float(dar_string.strip())

    # Calculate the pixel aspect ratio of the track
    # based on the height, width, and display A/R
    def calc_pixel_aspect_ratio(self, track):
        t_height = track.height * track.display_ar
        t_width = track.width
        gcd = fractions.gcd(t_height, t_width)
        self.log.debug("GCD of {0} height, {1} width is {2:f}".format(
                       t_height, t_width, gcd))

        if gcd == 0:
            # Pixel aspect ratio should be 1:1
            t_height = 1
            t_width = 1
            gcd = 1
        else:
            # We can do division on integers here
            # because the denominator is common
            t_height = t_height / gcd
            t_width = t_width / gcd

        # If height and width are extraordinarily large,
        # bring them down by a multiple of 10 simultaneously
        while t_height > 1000 or t_width > 1000:
            t_height = t_height / 10
            t_width = t_width / 10

        self.log.debug("Calculated pixel aspect ratio is {0:f}:{1:f} ({2:f})".format(
                       t_height, t_width, t_height / t_width))

        if not self.args.no_round_par:
            if t_height / t_width > 0.98 and t_height / t_width < 1:
                self.log.debug("Rounding pixel aspect ratio up to 1:1")
                t_height = t_width = 1
            elif t_height / t_width < 1.01 and t_height / t_width > 1:
                self.log.debug("Rounding pixel aspect ratio down to 1:1")
                t_height = t_width = 1

        return str(t_height) + ":" + str(t_width)

    # Return the fixed duration of the MKV file.
    def get_duration(self):
        return self.duration

    # Parse the 'duration' line in the mkvinfo output
    # to estimate a duration for the file.
    def parse_audio_duration(self, output):
        audio_int = 0

        duration_detect_string = "| + Duration: "
        audio_duration = output[output.index(duration_detect_string) +
                                len(duration_detect_string):]

        if not "s" in audio_duration:
            raise Exception("Could not parse MKV duration - no 's' seconds specified")

        audio_duration = audio_duration[0:audio_duration.index("s")]

        self.log.debug("Audio duration detected as {0} seconds".format(
                       audio_duration))

        # Check if there is a decimal value; if so, add one second
        if "." in audio_duration:
            audio_duration = audio_duration[0:audio_duration.index(".")]
            audio_int += 1

        audio_int += int(audio_duration)
        self.log.debug("Audio duration for MKV file may have been rounded: "
                       "using {0} seconds".format(audio_int))

        return audio_int

    # Parse the output from mkvinfo for the file.
    def parse_mkvinfo(self, result):
        track_detect_string = "| + A track"

        if not track_detect_string in result:
            raise Exception("mkvinfo: output did not contain any tracks")

        track_info = result
        self.duration = self.parse_audio_duration(track_info)

        # Multiple track support:
        # Extract mediainfo profile for all tracks in file,
        # then cross-reference them with the output from mkvinfo.
        # This prevents running mediainfo multiple times.
        mediainfo_video_output = self.get_mediainfo("video")
        mediainfo_audio_output = self.get_mediainfo("audio")

        # For ease of use, throw these values into a dictionary
        # with the key being the track ID.
        mediainfo = {}

        for mediainfo_track in mediainfo_video_output:
            mediainfo[int(mediainfo_track[0])] = mediainfo_track[1:]
        for mediainfo_track in mediainfo_audio_output:
            mediainfo[int(mediainfo_track[0])] = mediainfo_track[1:]

        # Create a new parser that can be used for all tracks
        info_parser = MKVInfoParser(self.log)
        has_audio = has_video = False

        while track_detect_string in track_info:
            track = MKVTrack(self.log)
            if track_detect_string in track_info:
                track_info = track_info[track_info.index(track_detect_string) +
                                        len(track_detect_string):]
            else:
                break

            # Get track type and number out of this block
            track_type = info_parser.parse_track_type(track_info)
            track_number, track_mkvtoolnix_id = (
                info_parser.parse_track_number(track_info)
            )
            self.log.debug("Track {0} will use ID {1} when taking actions "
                           "with the mkvtoolnix suite".format(
                           track_number, track_mkvtoolnix_id))

            # Set individual track properties for the object by track ID
            if track_type in ("video", "audio"):
                mediainfo_track = mediainfo[track_number]

            if track_type == "video":
                has_video = True
                track = VideoTrack(self.log)
                track.number = track_number
                track.default = info_parser.parse_track_is_default(track_info)

                track.height = int(mediainfo_track[0])
                track.width = int(mediainfo_track[1])

                try:
                    # Possible condition: no reference frames detected
                    # If so, just set to zero and log a debug message
                    track.reference_frames = int(mediainfo_track[2])
                except ValueError:
                    track.reference_frames = 0
                    self.log.debug("Reference frame value '{0}' in track {1} "
                                   "could not be parsed; assuming 0 reference "
                                   "frames".format(mediainfo_track[2], track.number))

                track.language = mediainfo_track[3].lower()
                if mediainfo_track[4]:
                    track.frame_rate = float(mediainfo_track[4])
                elif mediainfo_track[7]:
                    self.log.debug("Using original track frame rate as "
                                   "container frame rate was not set")
                    track.frame_rate = float(mediainfo_track[7])
                else:
                    raise ValueError("Could not read FPS information "
                                     "from mediainfo output")

                track.codec_id = mediainfo_track[5]
                track.display_ar = self.parse_display_aspect_ratio(
                    mediainfo_track[6])
                track.pixel_ar = self.calc_pixel_aspect_ratio(track)

                self.log.debug("Video track {0} has dimensions {1}x{2} with "
                               "{3} reference frames".format(
                               track.number, track.width,
                               track.height, track.reference_frames))
                self.log.debug("Video track {0} has {1:f} FPS and codec {2}".format(
                               track.number, track.frame_rate, track.codec_id))
                self.log.debug("Video track {0} has display aspect ratio {1:f}".format(
                               track.number, track.display_ar))

                if self.reference_frames_exceeded(track):
                    self.log.warning("Video track {0} contains too many "
                                     "reference frames to play properly on low-powered "
                                     "devices. See {1} for details".format(
                                        track.number, xenonmkv.text.REFERENCE_FRAMES_INFO)
                                    )
                    if not self.args.ignore_reference_frames:
                        raise Exception("Video track {0} has too many "
                                        "reference frames".format(track.number))
                else:
                    self.log.debug("Video track {0} has a reasonable number "
                                   "of reference frames, and should be compatible with "
                                   "low-powered devices".format(track.number))

            elif track_type == "audio":
                has_audio = True
                track = AudioTrack(self.log)
                track.number = track_number
                track.default = info_parser.parse_track_is_default(track_info)

                track.codec_id = mediainfo_track[0]
                track.language = mediainfo_track[1].lower()
                track.channels = int(mediainfo_track[2])

                # Indicate if the audio track needs a recode.
                # By default, it does.

                # Check that the audio type is AAC, and if the number of
                # channels in the file is less than or equal to what was
                # specified on the command line, no recode is necessary.
                if track.codec_id == "A_AAC":
                    # Check on the number of channels in the file
                    # versus the argument passed.
                    if track.channels <= self.args.channels:
                        # Reasonable message to log at info level
                        self.log.info("Audio track {0} will not need to be "
                                      "re-encoded ({1} channels specified, {2} channels "
                                      "in file)".format(track.number,
                                      self.args.channels, track.channels))
                        track.needs_recode = False

                self.log.debug("Audio track {0} has codec {1} and language {2}".format(
                               track.number, track.codec_id, track.language))
                self.log.debug("Audio track {0} has {1} channel(s)".format(
                               track.number, track.channels))

            else:
                # Unrecognized track type. Don't completely abort processing,
                # but do log it.
                # Do not proceed to add this to the global tracks list.
                self.log.debug("Unrecognized track type '{0}' in {1}; skipping".format(
                               track_type, track_number))
                continue

            # Add general properties to track
            track.mkvtoolnix_id = track_mkvtoolnix_id

            self.log.debug("All properties set for {0} track {1}".format(
                           track_type, track.number))
            track.track_type = track_type
            self.tracks[track.number] = track

        # All tracks detected here
        self.log.debug("All tracks detected from mkvinfo output; "
                       "total number is {0}".format(len(self.tracks)))

        # Make sure that there is at least one audio and one video track
        # and throw an exception if not
        if not has_video:
            raise Exception("No video track found in MKV file {0}".format(self.path))
        elif not has_audio:
            raise Exception("No audio track found in MKV file {0}".format(self.path))

    def reference_frames_exceeded(self, video_track):
        return ReferenceFrameValidator.validate(
            video_track.height, video_track.width,
            video_track.reference_frames
        )

    def has_multiple_av_tracks(self):
        video_tracks = audio_tracks = 0
        for track_id in self.tracks:
            track = self.tracks[track_id]
            if track.track_type == "video":
                video_tracks += 1
            elif track.track_type == "audio":
                audio_tracks += 1

        return (video_tracks > 1 or audio_tracks > 1)

    def set_video_track(self, track_id):
        if (self.tracks[track_id] and
                self.tracks[track_id].track_type == "video"):
            self.video_track_id = track_id
        else:
            raise Exception("Video track with ID {0} was not found "
                            "in file".format(track_id))

    def set_audio_track(self, track_id):
        if (self.tracks[track_id] and
                self.tracks[track_id].track_type == "audio"):
            self.audio_track_id = track_id
        else:
            raise Exception("Audio track with ID {0} was not found "
                            "in file".format(track_id))

    def set_default_av_tracks(self, preferred_language=None):
        """
        Set default audio and video tracks somewhat intelligently.
        Here is the flow:
         * Always take user preference set as video/audio_track_id property
         * Check if the user has a language preference; pick first track
           with that language
         * Select MKV tracks flagged as 'default' (may not be user preference)
         * If none are default, take first available track
           (also fallback for 1A/1V track)
        """
        # TODO: Accept language preference for audio tracks
        # (eg: always take 'en' track if possible)

        if preferred_language:
            # Completely ignore 'default' track setting
            for track_id in self.tracks:
                track = self.tracks[track_id]
                if (track.track_type == "video" and
                        not self.video_track_id and
                        track.language == preferred_language):
                    self.video_track_id = track.number

                elif (track.track_type == "audio" and
                        not self.audio_track_id and
                        track.language == preferred_language):
                    self.audio_track_id = track.number

        # If a track was selected, all subsequent tests
        # will skip that type of track (video/audio_track_id)

        # Select first track flagged as 'default' in MKV file
        for track_id in self.tracks:
            track = self.tracks[track_id]
            if (track.track_type == "video" and
                    track.default and
                    not self.video_track_id):
                self.video_track_id = track.number

            elif (track.track_type == "audio" and
                    track.default and
                    not self.audio_track_id):
                self.audio_track_id = track.number

        # Check again if we have tracks specified here.
        # If not, nothing was hinted as default
        # in the MKV file and we should really pick the
        # first audio and first video track.
        if not self.video_track_id:
            self.log.debug("No default video track was specified in '{0}'; "
                           "using first available".format(self.path))
            for track_id in self.tracks:
                track = self.tracks[track_id]
                if track.track_type == "video":
                    self.video_track_id = track.number
                    self.log.debug(
                        "First available video track in file is {0}".format(
                        self.video_track_id))
                    break

        if not self.audio_track_id:
            self.log.debug("No default audio track was specified in '{0}'; "
                           "using first available".format(self.path))
            for track_id in self.tracks:
                track = self.tracks[track_id]
                if track.track_type == "audio":
                    self.audio_track_id = track.number
                    self.log.debug("First available audio track in file is {0}".format(
                                   self.audio_track_id))
                    break

        # If we still don't have a video and audio track specified,
        # it's time to throw an error.
        if not self.video_track_id or not self.audio_track_id:
            raise Exception("Could not select an audio and video track "
                            "for MKV file '{0}'".format(self.path))

    def get_audio_track(self):
        return self.tracks[self.audio_track_id]

    def get_video_track(self):
        return self.tracks[self.video_track_id]

    def audio_track_list(self):
        track_list = {}
        for track in self.tracks:
            track = self.tracks[track]
            if track.track_type != "audio":
                continue

            track_list[track.number] = (
                "{0} - {1} [{2} channels]".format(
                track.number, track.language, track.channels)
            )

        return track_list

    def video_track_list(self):
        track_list = {}
        for track in self.tracks:
            track = self.tracks[track]
            if track.track_type != "video":
                continue

            track_list[track.number] = (
                "{0} - {1} [{2}x{3}]".format(
                track.number, track.language, track.width, track.height)
            )

        return track_list

    def extract_mkv(self):
        self.log.debug("Executing mkvextract on '{0}'".format(self.get_path()))
        prev_dir = os.getcwd()

        if self.args.scratch_dir != ".":
            self.log.debug("Using {0} as scratch directory for "
                           "MKV extraction".format(self.args.scratch_dir))
            os.chdir(self.args.scratch_dir)

        mkvtoolnix_video_id = self.tracks[self.video_track_id].mkvtoolnix_id
        mkvtoolnix_audio_id = self.tracks[self.audio_track_id].mkvtoolnix_id

        self.log.debug("Using video track from MKV file with ID {0} "
                       "(mkvtoolnix ID {1})".format(
                       self.video_track_id, mkvtoolnix_video_id))
        self.log.debug("Using audio track from MKV file with ID {0} "
                       "(mkvtoolnix ID {1})".format(
                       self.audio_track_id, mkvtoolnix_audio_id))

        try:
            temp_video_file = ("temp_video" +
                               self.tracks[self.video_track_id].get_filename_extension())
            temp_audio_file = ("temp_audio" +
                               self.tracks[self.audio_track_id].get_filename_extension())
        except UnsupportedCodecError:
            # Send back to main application
            raise

        if (self.args.resume_previous and
                os.path.isfile(temp_video_file) and
                os.path.isfile(temp_audio_file)):

            self.log.debug("Temporary video and audio files already exist; "
                           "cancelling extract")
            temp_video_file = os.path.join(os.getcwd(), temp_video_file)
            temp_audio_file = os.path.join(os.getcwd(), temp_audio_file)
            os.chdir(prev_dir)
            return (temp_video_file, temp_audio_file)

        # Remove any existing files with the same names
        if os.path.isfile(temp_video_file):
            self.log.debug("Deleting temporary video file {0}".format(
                           os.path.join(os.getcwd(), temp_video_file)))
            os.unlink(temp_video_file)
        if os.path.isfile(temp_audio_file):
            self.log.debug("Deleting temporary audio file {0}".format(
                           os.path.join(os.getcwd(), temp_audio_file)))
            os.unlink(temp_audio_file)

        video_output = str(mkvtoolnix_video_id) + ":" + temp_video_file
        audio_output = str(mkvtoolnix_audio_id) + ":" + temp_audio_file

        cmd = [self.args.tool_paths["mkvextract"], "tracks",
               self.get_path(), video_output, audio_output]
        ph = ProcessHandler(self.args, self.log)
        process = ph.start_output(cmd)

        if process != 0:
            raise Exception("An error occurred while extracting tracks from {0}"
                            " - please make sure this file exists and is readable".format(
                            self.get_path()))

        temp_video_file = os.path.join(os.getcwd(), temp_video_file)
        temp_audio_file = os.path.join(os.getcwd(), temp_audio_file)

        os.chdir(prev_dir)
        self.log.debug("mkvextract finished; attempting to parse output")

        if not temp_video_file or not temp_audio_file:
            raise Exception("Audio or video file missing from "
                            "mkvextract output")

        return (temp_video_file, temp_audio_file)
