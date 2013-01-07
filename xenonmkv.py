#!/usr/bin/env python

# XenonMKV: wrapper utility for MKV to MP4 container conversions
# Jake Billo, jake@jakebillo.com
# https://github.com/jbillo/xenonmkv

import sys
import os
import ConfigParser


def prompt_install_python27():
    print """You need to be running at least Python 2.7 to use this application.
 Install Python 2.7 using your package manager or download for
 your OS at http://www.python.org/getit/"""
    sys.exit(1)


# Check for Python version before running argparse import
if sys.version_info[0] == 2 and sys.version_info[1] < 7:
    # Are we running Python <2.4?
    if sys.version_info[1] < 4:
        prompt_install_python27()

    if os.path.isfile("/usr/bin/python2.7"):
        import subprocess
        subprocess.call(["/usr/bin/python2.7"] + sys.argv)
        sys.exit(0)
    else:
        prompt_install_python27()

import shutil
import argparse
import logging
import traceback

from xenonmkv.file_utils import FileUtils
from xenonmkv.decoder import AudioDecoder
from xenonmkv.encoder import AACEncoder
from xenonmkv.mp4box import MP4Box
from xenonmkv.mkvfile import MKVFile
from xenonmkv.track import MKVTrack

log = args = None


def cleanup_temp_files():
    global args, log
    if not args.preserve_temp_files:
        t = MKVTrack(log)
        f_utils = FileUtils(log, args)
        f_utils.delete_temp_files(args.scratch_dir, t.get_possible_extensions())


def log_exception(source, e, log_type="critical"):
    """
    Log an exception with the stack trace in debug mode,
    and exit if it's a critical log type
    """

    global log
    getattr(log, log_type)("{0}: {1}".format(str(source), str(e.message)))
    log.debug(traceback.format_exc())
    if log_type == "critical":
        sys.exit(1)


def try_set_video_track(to_convert):
    global args, log
    try:
        if args.video_track:
            to_convert.set_video_track(args.video_track)
    except Exception as e:
        log_exception("set_video_track", e, "warning")


def try_set_audio_track(to_convert):
    global args, log
    try:
        if args.audio_track:
            to_convert.set_audio_track(args.audio_track)
    except Exception as e:
        log_exception("set_audio_track", e, "warning")


def select_track(track_type, tracks):
    track_values = []

    for track in tracks:
        track_values.append(str(track))

    track_type = track_type.lower()
    title = track_type[0:1].upper() + track_type[1:]

    try:
        try_track = None
        # Output type of track list and prompt to pick one
        print "== {0} Tracks ==".format(title)
        for track in tracks:
            print tracks[track]
        while not try_track:
            try_track = raw_input("Select a {0} track or Ctrl+C to exit: ".format(
                                  track_type))
            if try_track in track_values:
                return int(try_track)

            log.error("Track '{0}' not in {1} track list, "
                      "please pick a valid track.".format(try_track, track_type))
            try_track = None

    except KeyboardInterrupt:
        # Output a new line before the critical message
        print
        log.critical("Track selection cancelled; exiting")
        sys.exit(1)


def parse_config_file(args):
    available_args = vars(args).keys()
    output = [("debug", "Parsing configuration settings from file {0}".format(
              args.config_file))]

    # Specifically remove config settings that shouldn't be touched
    # For example, do not allow config_file to be overwritten again
    try:
        available_args.remove("config_file")
    except:
        pass

    config = ConfigParser.SafeConfigParser()
    config.read(args.config_file)
    for item in config.items("xenonmkv"):
        if item[0] in available_args:
            output.append(("debug", "Applying setting {0} with value '{1}' "
                          "from configuration file".format(item[0], item[1])))
            setattr(args, item[0], item[1])
        else:
            output.append(("warning", "Found unrecognized or invalid setting {0} in "
                          "configuration file".format(item[0])))

    return output


def main():
    # Main program begins
    global args, log, app_path
    log = logging.getLogger("xenonmkv")
    console_handler = logging.StreamHandler()
    formatter = logging.Formatter(
        '%(asctime)s - %(name)s [%(levelname)s] %(message)s'
    )
    console_handler.setFormatter(formatter)
    log.addHandler(console_handler)

    dependencies = ('mkvinfo', 'mediainfo', 'mkvextract',
                    'mplayer', 'faac', 'MP4Box')

    parser = argparse.ArgumentParser(description='Parse command line arguments '
                                     'for XenonMKV.')
    parser.add_argument('source_file', help='Path to the source MKV file')
    parser.add_argument('-d', '--destination',
                        help="""Directory to output the destination .mp4 file
                        (default: current directory)""",
                        default='.')
    parser.add_argument('-sd', '--scratch-dir',
                        help="""Specify a scratch directory where temporary files should
                         be stored""",
                        default=None)
    parser.add_argument('-cfg', '--config-file',
                        help="""Provide a configuration file that
                         contains default arguments or settings for the application""",
                        default='')
    parser.add_argument("-p", '--profile',
                        help="""Select a standardized device profile for encoding.
                         Current profile options are: xbox360, playbook""",
                        default="")

    output_group = parser.add_argument_group("Output options")
    output_group.add_argument('-q', '--quiet',
                              help="""Do not display output or progress from tools,
                               or prompt for input""",
                              action='store_true')
    output_group.add_argument('-v', '--verbose', help='Verbose output',
                              action='store_true')
    output_group.add_argument('-vv', '--debug',
                              help='Highly verbose debug output',
                              action='store_true')
    output_group.add_argument('-pf', '--print-file',
                              help='Output filenames before and after converting',
                              action='store_true')

    video_group = parser.add_argument_group("Video options",
                                            "Options for processing video.")
    video_group.add_argument('-nrp', '--no-round-par',
                             help="""When processing video, do not round pixel aspect
                             ratio from 0.98 to 1.01 to 1:1.""",
                             action='store_true')
    video_group.add_argument('-irf', '--ignore-reference-frames',
                             help="""If the source video has too many reference frames
                              to play on low-powered devices (Xbox, PlayBook), continue
                              converting anyway""",
                             action='store_true')

    audio_group = parser.add_argument_group("Audio options",
                                            "Select custom audio decoding and "
                                            "encoding options.")
    audio_group.add_argument('-c', '--channels',
                             help="""Specify the maximum number of channels that are
                              acceptable in the output file. Certain devices (Xbox) will
                              not play audio with more than two channels. If the audio
                              needs to be re-encoded at all, it will be downmixed to two
                              channels only. Possible values for this option are 2
                              (stereo); 4 (surround); 5.1 or 6 (full 5.1); 7.1 or 8
                              (full 7.1 audio).
                              For more details, view the README file.""",
                             default=6)
    audio_group.add_argument('-fq', '--faac-quality',
                             help="""Quality setting for FAAC when encoding WAV files
                              to AAC. Defaults to 150 (see
                              http://wiki.hydrogenaudio.org/index.php?title=FAAC)""",
                             default=150)

    track_group = parser.add_argument_group("Track options",
                                            "These options determine how multiple tracks "
                                            "in MKV files are selected.")
    track_group.add_argument('-st', '--select-tracks',
                             help="""If there are multiple tracks in the MKV file, prompt
                             to select which ones will be used. By default, the last video
                              and audio tracks flagged as 'default' in the MKV file will
                             be used. This option requires interactive user input, so do
                             not use it in batch processing or scripts.""",
                             action='store_true')
    track_group.add_argument('-vt', '--video-track',
                             help="""Use the specified video track. If not present in
                             the file, the default track will be used.""",
                             type=int)
    track_group.add_argument('-at', '--audio-track',
                             help="""Use the specified audio track. If not present in
                             the file, the default track will be used.""",
                             type=int)
    track_group.add_argument('-lang', '--preferred-language',
                             help="""Provide a preferred language code in ISO 639-1 format
                              ('en' for English, 'fr' for French, etc.)
                             When picking tracks, this language will be preferred.""")

    proc_group = parser.add_argument_group("File and processing options",
                                           """These options determine how XenonMKV
                                           processes files and their contents.""")
    proc_group.add_argument('-rp', '--resume-previous',
                            help="""Resume a previous run (do not recreate files
                            if they already exist). Useful for debugging quickly if a
                            conversion has already partially succeeded.""",
                            action='store_true')
    proc_group.add_argument('-n', '--name',
                            help="""Specify a name for the final MP4 container.
                            Defaults to the original file name.""",
                            default="")
    proc_group.add_argument('-preserve', '--preserve-temp-files',
                            help="""Preserve temporary files on the filesystem rather
                             than deleting them at the end of each run.""",
                            action='store_true', default=False)
    proc_group.add_argument("-eS", "--error-filesize",
                            help="""Stop processing this file if it is over 4GiB.
                             Files of this size will not be processed correctly by some
                             devices such as the Xbox 360, and they will not save
                             correctly to FAT32-formatted storage. By default, you will
                             only see a warning message, and processing will continue.""",
                            action="store_true")
    proc_group.add_argument('--mp4box-retries',
                            help="""Set the number of retry attempts for MP4Box to attempt
                             to create a file (default: 3)""", default=3, type=int)

    dep_group = parser.add_argument_group("Custom paths",
                                          "Set custom paths for the utilities used by "
                                          "XenonMKV.")
    for dependency in dependencies:
        dep_group.add_argument("--{0}-path".format(dependency.lower()),
                               help="""Set a custom complete path for the {0} tool.
                                Any library under that path will also be
                                loaded.""".format(dependency))

    if len(sys.argv) < 2:
        parser.print_help()
        sys.exit(1)

    args = parser.parse_args()
    config_file_output = False

    # If a configuration file was specified, attempt to read it.
    if args.config_file and os.path.isfile(args.config_file):
        config_file_output = parse_config_file(args)

    # Depending on the arguments, set the logging level appropriately.
    if args.quiet:
        log.setLevel(logging.ERROR)
    elif args.debug:
        log.setLevel(logging.DEBUG)
        log.debug("Using debug/highly verbose mode output")
    elif args.verbose:
        log.setLevel(logging.INFO)

    # If we parsed a configuration file, run through all logging output
    if config_file_output:
        for level, message in config_file_output:
            getattr(log, level)(message)

    # Pick temporary/scratch directory
    if not args.scratch_dir:
        if "TEMP" in os.environ:
            args.scratch_dir = os.environ["TEMP"]
        elif os.path.isdir("/var/tmp"):
            args.scratch_dir = "/var/tmp"
        else:
            args.scratch_dir = os.curdir

    # Apply selected profile
    if args.profile:
        if args.profile == "xbox360":
            args.channels = 2
            args.error_filesize = True
        elif args.profile == "playbook":
            args.channels = 6
            args.error_filesize = False
        else:
            log.warning("Unrecognized device profile {0}".format(args.profile))
            args.profile = ""

    # Check for 5.1/7.1 audio with the channels setting
    if args.channels == "5.1":
        args.channels = 6
    elif args.channels == "7.1":
        args.channels = 8
    if args.channels not in ('2', '4', '6', '8', 2, 4, 6, 8):
        log.warning("An invalid number of channels was specified. "
                    "Falling back to 2-channel stereo audio.")
        args.channels = 2

    # Enforce channels as integer for comparison purposes later on
    args.channels = int(args.channels)

    # Ensure preferred language, if present, is lowercased and 2 characters
    if args.preferred_language:
        args.preferred_language = args.preferred_language.lower()
        if len(args.preferred_language) < 2:
            log.warning("Could not set preferred language code '{0}'".format(
                        args.preferred_language))
            args.preferred_language = None
        elif len(args.preferred_language) > 2:
            args.preferred_language = args.preferred_language[0:2]
            log.warning("Preferred language code truncated to '{0}'".format(
                        args.preferred_language))

    # Make sure user is not prompted for input if quiet option is used
    if args.quiet and args.select_tracks:
        log.warning("Cannot use interactive track selection in quiet mode. "
                    "Tracks will be automatically selected.")
        args.select_tracks = False

    log.debug("Starting XenonMKV")

    # Check if we have a full file path or are just specifying a file
    if os.sep not in args.source_file:
        log.debug("Ensuring that we have a complete path to {0}".format(
                  args.source_file))
        args.source_file = os.path.join(os.getcwd(), args.source_file)
        log.debug("{0} will be used to reference the original MKV file".format(
                  args.source_file))

    # Always ensure destination path ends with a slash
    if not args.destination.endswith(os.sep):
        args.destination += os.sep

    if not args.scratch_dir.endswith(os.sep):
        args.scratch_dir += os.sep

    # Initialize file utilities
    f_utils = FileUtils(log, args)

    # Check if all dependent applications are installed and available in PATH,
    # or if they are specified.
    # If so, store them in args.tool_paths so all classes
    # have access to them as needed
    (args.tool_paths, args.library_paths) = f_utils.check_dependencies(
        dependencies
    )

    # Check if source file exists and is an appropriate size
    try:
        f_utils.check_source_file(args.source_file)
    except IOError as e:
        log_exception("check_source_file", e)

    source_basename = os.path.basename(args.source_file)
    source_noext = source_basename[0:source_basename.rindex(".")]

    if not args.name:
        args.name = source_noext
        log.debug("Using '{0}' as final container name".format(args.name))

    # Check if destination directory exists
    try:
        f_utils.check_dest_dir(args.destination)
    except IOError as e:
        log_exception("check_dest_dir", e)

    log.info("Loading source file {0}".format(args.source_file))

    if args.print_file:
        print "Processing: {0}".format(args.source_file)

    try:
        to_convert = MKVFile(args.source_file, log, args)
        to_convert.get_mkvinfo()
    except Exception as e:
        if not args.preserve_temp_files:
            cleanup_temp_files()
        log_exception("get_mkvinfo", e)

    # If the user knows which A/V tracks they want, set them.
    # MKVFile will not overwrite them.
    try_set_video_track(to_convert)
    try_set_audio_track(to_convert)

    try:
        # Check for multiple tracks
        if to_convert.has_multiple_av_tracks():
            log.debug("Source file {0} has multiple audio or "
                      "video tracks".format(args.source_file))

            # First, pick default tracks,
            # which can be overridden in select_tracks
            to_convert.set_default_av_tracks()

            if args.select_tracks:
                video_tracks = to_convert.video_track_list()
                audio_tracks = to_convert.audio_track_list()
                if len(video_tracks) > 1:
                    args.video_track = select_track("video", video_tracks)
                    try_set_video_track(to_convert)
                if len(audio_tracks) > 1:
                    args.audio_track = select_track("audio", audio_tracks)
                    try_set_audio_track(to_convert)
            else:
                log.debug("Selected default audio and video tracks")

        else:
            # Pick default (or only) audio/video tracks
            log.debug("Source file {0} has 1 audio and 1 video track; "
                      "using these".format(args.source_file))
            to_convert.set_default_av_tracks()
    except Exception as e:
        if not args.preserve_temp_files:
            cleanup_temp_files()
        log_exception("set_default_av_tracks", e)

    # Next phase: Extract MKV files to scratch directory
    try:
        (video_file, audio_file) = to_convert.extract_mkv()
    except Exception as e:
        if not args.preserve_temp_files:
            cleanup_temp_files()
        log_exception("extract_mkv", e)

    # If needed, hex edit the video file to make it compliant
    # with a lower h264 profile level
    if video_file.endswith(".h264"):
        f_utils.hex_edit_video_file(video_file)

    # Detect which audio codec is in place and dump audio to WAV accordingly
    if to_convert.get_audio_track().needs_recode:
        log.debug("Audio track {0} needs to be re-encoded".format(audio_file))
        audio_dec = AudioDecoder(audio_file, log, args)
        audio_dec.decode()

        # Once audio has been decoded to a WAV,
        # use the appropriate AAC encoder to transform it to .aac
        enc = AACEncoder(
            os.path.join(args.scratch_dir, "audiodump.wav"), log, args)
        enc.encode()
        encoded_audio = os.path.join(args.scratch_dir, "audiodump.aac")
    else:
        # The audio track does not need to be re-encoded.
        # Reference the already-valid audio file and put it into the MP4 container.
        encoded_audio = audio_file

    # Now, throw things back together into a .mp4 container with MP4Box.
    video_track = to_convert.get_video_track()
    mp4box = MP4Box(video_file, encoded_audio, video_track.frame_rate,
                    video_track.pixel_ar, args, log)
    try:
        mp4box.package()
    except Exception as e:
        if not args.preserve_temp_files:
            cleanup_temp_files()
        log_exception("package", e)

    # Move the file to the destination directory with the original name
    dest_path = os.path.join(args.destination, source_noext + ".mp4")
    shutil.move(os.path.join(args.scratch_dir, "output.mp4"), dest_path)

    log.info("Processing of {0} complete; file saved as {1}".format(
             args.source_file, dest_path))

    # Delete temporary files if possible
    if not args.preserve_temp_files:
        cleanup_temp_files()

    log.debug("XenonMKV completed processing")
    if args.print_file:
        print "Completed: {0}".format(dest_path)


if __name__ == "__main__":
    main()
