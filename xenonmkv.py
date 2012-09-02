#!/usr/bin/python

# XenonMKV for PlayBook/revision 2.0

import argparse
import sys
import os
import subprocess
import logging
import fractions

import reference_frame

class MKVInfoParser:
	@staticmethod
	def _parse_to_newline(output, detect, what="value"):
		if detect in output:
			output = output[output.index(detect) + len(detect):]
			if "\n" in output:
				output = output[0:output.index("\n")]
			log.debug("Detected %s '%s' from mkvinfo output" % (what, output))
			return output
		else:
			raise Exception("Could not parse %s from mkvinfo" % what)
		
	@staticmethod
	def parse_track_number(output):
		return int(MKVInfoParser._parse_to_newline(output, "|  + Track number: ", "track number"))

	@staticmethod
	def parse_track_type(output):
		return MKVInfoParser._parse_to_newline(output, "|  + Track type: ", "track type")

	@staticmethod
	def parse_track_is_default(output):
		try:
			result = MKVInfoParser._parse_to_newline(output, "|  + Default flag: ", "default flag")		
			return "1" in result
		except:
			# Could not determine whether track was default, so set it to default
			# Usually this means that there is only one video and one audio track
			return True


class MKVFile():
	path = ""
	tracks = {}
	duration = 0

	video_track_id = audio_track_id = 0

	def __init__(self, path):
		self.path = path
	
	def get_path(self):
		return self.path

	# Open the mkvinfo process and parse its output.
	def get_mkvinfo(self):
		track_count = 0
		log.debug("Executing 'mkvinfo %s'" % self.get_path())
		result = subprocess.check_output(["mkvinfo", self.get_path()])
		log.debug("mkvinfo finished; attempting to parse output")
		self.parse_mkvinfo(result)

	# Open the mediainfo process to obtain detailed info on the file.
	def get_mediainfo_video(self):
		parameters = "Video;%ID%,%Height%,%Width%,%Format_Settings_RefFrames%,%Language%,%FrameRate%,%CodecID%,%DisplayAspectRatio%,\n"
		log.debug("Executing 'mediainfo %s %s'" % (parameters, self.get_path()))
		result = subprocess.check_output(["mediainfo", "--Inform=" + parameters, self.get_path()])
		log.debug("mediainfo finished; attempting to parse output for video settings")
		return self.parse_mediainfo(result)

	def get_mediainfo_audio(self):
		parameters = "Audio;%ID%,%CodecID%,%Language%,%Channels%,\n"
		log.debug("Executing 'mediainfo %s %s'" % (parameters, self.get_path()))
		result = subprocess.check_output(["mediainfo", "--Inform=" + parameters, self.get_path()])
		log.debug("mediainfo finished; attempting to parse output for audio settings")
		return self.parse_mediainfo(result)

	def parse_mediainfo(self, result):
		output = []
		lines = result.split("\n")
		for line in lines:
			# remove last \n element from array that will always be present
			values = result.split(",")[0:-1] 
			output.append(values)

		return output

	# Return a float value specifying the display aspect ratio.
	def parse_display_aspect_ratio(self, dar_string):
		log.debug("Attempting to parse display aspect ratio '%s'" % dar_string)
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
				raise Exception("Could not parse display aspect ratio of %s" % dar_string)
		else:
			return float(dar_string.strip())
			 
	# Calculate the pixel aspect ratio of the track based on the height, width, and display A/R
	def calc_pixel_aspect_ratio(self, track):
		t_height = track.height * track.display_ar
		t_width = track.width
		gcd = fractions.gcd(t_height, t_width)
		log.debug("GCD of %i height, %i width is %i" % (t_height, t_width, gcd))

		if gcd == 0:
			# Pixel aspect ratio should be 1:1
			t_height = 1
			t_width = 1
			gcd = 1
		else:
			# We can do division on integers here because the denominator is common
			t_height = t_height / gcd
			t_width = t_width / gcd
		
		# If height and width are extraordinarily large, bring them down by a multiple of 10 simultaneously
		while t_height > 1000 or t_width > 1000:
			t_height = t_height / 10
			t_width = t_width / 10

		log.debug("Calculated pixel aspect ratio is %i:%i (%f)" % (t_height, t_width, t_height / t_width))
		
		if not args.no_round_par:
			if t_height / t_width > 0.98 and t_height / t_width < 1:
				log.debug("Rounding pixel aspect ratio up to 1:1")
				t_height = t_width = 1
			elif t_height / t_width < 1.01 and t_height / t_width > 1:
				log.debug("Rounding pixel aspect ratio down to 1:1")
				t_height = t_width = 1
		
		return str(t_height) + ":" + str(t_width)
		
	# Return the fixed duration of the MKV file.
	def get_duration(self):
		return self.duration

	# Parse the 'duration' line in the mkvinfo output to estimate a duration for the file.
	def parse_audio_duration(self, output):
		audio_int = 0

		duration_detect_string = "| + Duration: "
		audio_duration = output[output.index(duration_detect_string) + len(duration_detect_string):]

		if not "s" in audio_duration:
			raise Exception("Could not parse MKV duration - no 's' specified")

		audio_duration = audio_duration[0:audio_duration.index("s")]

		log.debug("Audio duration detected as %s seconds" % audio_duration)

		# Check if there is a decimal value; if so, add one second
		if "." in audio_duration:
			audio_duration = audio_duration[0:audio_duration.index(".")]
			audio_int += 1
	
		audio_int += int(audio_duration)
		log.debug("Audio duration for MKV file may have been rounded: using %i seconds" % audio_int)

		return audio_int

	# Parse the output from mkvinfo for the file.
	def parse_mkvinfo(self, result):
		track_detect_string = "| + A track"
		
		if not track_detect_string in result:
			raise Exception("mkvinfo: output did not contain any tracks")

		track_info = result
		self.duration = self.parse_audio_duration(track_info)

		# Multiple track support:
		# Extract mediainfo profile for all tracks in file, then cross-reference them
		# with the output from mkvinfo. This prevents running mediainfo multiple times.

		mediainfo_video_output = self.get_mediainfo_video()
		mediainfo_audio_output = self.get_mediainfo_audio()

		# For ease of use, throw these values into a dictionary with the key being the track ID.
		mediainfo = {}

		for mediainfo_track in mediainfo_video_output:
			mediainfo[int(mediainfo_track[0])] = mediainfo_track[1:]
		for mediainfo_track in mediainfo_audio_output:
			mediainfo[int(mediainfo_track[0])] = mediainfo_track[1:]

		while track_detect_string in track_info:
			track = MKVTrack()
			if track_detect_string in track_info:
				track_info = track_info[track_info.index(track_detect_string) + len(track_detect_string):]
			else:
				break

			# Get track type and number out of this block
			track_type = MKVInfoParser.parse_track_type(track_info)
			track_number = MKVInfoParser.parse_track_number(track_info)

			# Set individual track properties for the object by track ID
			mediainfo_track = mediainfo[track_number]

			if track_type == "video":
				track = VideoTrack()
				track.number = track_number
				track.default = MKVInfoParser.parse_track_is_default(track_info)

				track.height = int(mediainfo_track[0])
				track.width = int(mediainfo_track[1])
				track.reference_frames = int(mediainfo_track[2])
				track.language = mediainfo_track[3]
				track.frame_rate = float(mediainfo_track[4])
				track.codec_id = mediainfo_track[5]
				track.display_ar = self.parse_display_aspect_ratio(mediainfo_track[6])
				track.pixel_ar = self.calc_pixel_aspect_ratio(track)

				log.debug("Video track %i has dimensions %ix%i with %i reference frames" % (track.number, track.width, track.height, track.reference_frames))
				log.debug("Video track %i has %f FPS and codec %s" % (track.number, track.frame_rate, track.codec_id))
				log.debug("Video track %i has display aspect ratio %f" % (track.number, track.display_ar))

				if self.reference_frames_exceeded(track):
					log.warning("Video track %i contains too many reference frames to play properly on low-powered devices" % track.number)
					if not args.ignore_reference_frames:
						sys.exit(1)
				else:
					log.debug("Video track %i has a reasonable number of reference frames, and should be compatible with low-powered devices" % track.number)

	
			elif track_type == "audio":
				track = AudioTrack()
				track.number = track_number
				track.default = MKVInfoParser.parse_track_is_default(track_info)

				track.codec_id = mediainfo_track[0]
				track.language = mediainfo_track[1]
				track.channels = int(mediainfo_track[2])

				# Preemptively indicate if the audio track needs a recode
				# If we have an AAC file with two channels, nothing needs to happen
				track.needs_recode = (track.codec_id != "A_AAC" or track.channels != 2)

				log.debug("Audio track %i has codec %s and language %s" % (track.number, track.codec_id, track.language))
				log.debug("Audio track %i has %i channel(s)" % (track.number, track.channels))
			
			else:
				raise Exception("Unrecognized track type %s" % track_type)

			log.debug("All properties set for %s track %i" % (track_type, track.number))
			track.track_type = track_type
			self.tracks[track.number] = track

		# All tracks detected here
		log.debug("All tracks detected from mkvinfo output; total number is %i" % len(self.tracks))

	def reference_frames_exceeded(self, video_track):
		return reference_frame.ReferenceFrameValidator.validate(video_track.height, video_track.width, video_track.reference_frames)

	def has_multiple_av_tracks(self):
		video_tracks = audio_tracks = 0
		for track_id in self.tracks:
			track = self.tracks[track_id]
			if track.track_type == "video":
				video_tracks += 1
			elif track.track_type == "audio":
				audio_tracks += 1

		return (video_tracks > 1 or audio_tracks > 1)

	def set_default_av_tracks(self):
		for track_id in self.tracks:
			track = self.tracks[track_id]
			if track.track_type == "video" and track.default:
				self.video_track_id = track.number
			elif track.track_type == "audio" and track.default:
				self.audio_track_id = track.number

	def get_audio_track(self):
		return self.tracks[self.audio_track_id]

	def get_video_track(self):
		return self.tracks[self.video_track_id]

	def extract_mkv(self):
		log.debug("Executing mkvextract on %s'" % self.get_path())
		prev_dir = os.getcwd()

		if args.scratch_dir != ".":
			log.debug("Using %s as scratch directory for MKV extraction" % args.scratch_dir)
			os.chdir(args.scratch_dir)

		temp_video_file = "temp_video" + self.tracks[self.video_track_id].get_filename_extension()
		temp_audio_file = "temp_audio" + self.tracks[self.audio_track_id].get_filename_extension()

		if args.resume_previous and os.path.isfile(temp_video_file) and os.path.isfile(temp_audio_file):
			log.debug("Temporary video and audio files already exist; cancelling extract")
			temp_video_file = os.getcwd() + "/" + temp_video_file
			temp_audio_file = os.getcwd() + "/" + temp_audio_file
			os.chdir(prev_dir)
			return (temp_video_file, temp_audio_file)
	
		# Remove any existing files with the same names
		if os.path.isfile(temp_video_file):
			log.debug("Deleting temporary video file %s" % os.getcwd() + "/" + temp_video_file)
			os.unlink(temp_video_file)

		if os.path.isfile(temp_audio_file):
			log.debug("Deleting temporary audio file %s" % os.getcwd() + "/" + temp_audio_file)
			os.unlink(temp_audio_file)

		video_output = str(self.video_track_id) + ":" + temp_video_file
		audio_output = str(self.audio_track_id) + ":" + temp_audio_file

		cmd = ["mkvextract", "tracks", self.get_path(), video_output, audio_output]
		process = subprocess.Popen(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE)

		while True:
			out = process.stdout.read(1)
			if out == '' and process.poll() != None:
				break
			if out != '' and not args.quiet:
				sys.stdout.write(out)
				sys.stdout.flush()

		temp_video_file = os.getcwd() + "/" + temp_video_file
		temp_audio_file = os.getcwd() + "/" + temp_audio_file

		os.chdir(prev_dir)
		log.debug("mkvextract finished; attempting to parse output")
		return (temp_video_file, temp_audio_file)

class MKVTrack():
	number = 0
	uid = track_type = language = codec_id = ""
	default = True

	codec_table = {}
	codec_table["A_AC3"] = ".ac3"
	codec_table["A_AAC"] = ".aac"
	codec_table["A_DTS"] = ".dts"
	codec_table["A_MP3"] = ".mp3"
	codec_table["A_MPEG/L3"] = ".mp3"
	codec_table["A_MS/ACM"] = ".mp3" # because people like to run anime through this thing
	codec_table["A_VORBIS"] = ".ogg"
	codec_table["V_MS/VFW/FOURCC"] = ".avi"
	codec_table["V_MPEG4/ISO/AVC"] = ".h264"
	codec_table["V_MPEG2"] = ".mpg"

	def get_filename_extension(self):
		if self.codec_id in self.codec_table:
			return self.codec_table[self.codec_id]

		# Otherwise, set defaults
		if self.codec_id.startswith("A_"):
			log.warning("Returning default .aac extension for audio codec %s" % self.codec_id)
			return ".aac"

		if self.codec_id.startswith("V_"):
			log.warning("Returning default .avi extension for video codec %s" % self.codec_id)
			return ".avi"

		raise Exception("Could not detect appropriate extension for codec %s" % self.codec_id)

	def get_possible_extensions(self):
		extensions = set()
		for key in self.codec_table:
			extensions.add(self.codec_table[key])

		return extensions

class VideoTrack(MKVTrack):
	height = width = reference_frames = 0
	frame_rate = display_ar = 0.0
	pixel_ar = ""

class AudioTrack(MKVTrack):
	length = channels = 0
	needs_recode = True

class AudioDecoder():
	extension = ""
	file_path = ""
	decoder = ""

	def __init__(self, file_path):
		self.file_path = file_path
		self.extension = file_path[file_path.rindex("."):]

	def detect_decoder(self):
		# New decoders can be added here when necessary
		if self.extension == ".ac3":
			self.decoder = "mplayer"
		elif self.extension == ".dts":
			self.decoder = "mplayer"
		else:
			self.decoder = "mplayer"	

	def decode(self):
		if not self.decoder:
			self.detect_decoder()

		prev_dir = os.getcwd()
		os.chdir(args.scratch_dir)

		# Based on the decoder, perform the appropriate operation
		if self.decoder == "mplayer":
			self.decode_mplayer()
		
		os.chdir(prev_dir)

	def decode_mplayer(self):
		log.debug("Starting decoding file to WAV with mplayer")
		# Check for existing audiodump.wav (already changed to temp directory)
		if os.path.isfile("audiodump.wav"):
			if args.resume_previous:
				log.debug("audiodump.wav already exists in scratch directory; cancelling decode")
				return True

			log.debug("Deleting temporary mplayer output file %s/audiodump.wav" % os.getcwd())
			os.unlink("audiodump.wav")

		cmd = ["mplayer", self.file_path, "-benchmark", "-vc", "null", "-vo", "null", "-channels", "2", "-ao", "pcm:fast"]
		process = subprocess.Popen(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE)

		while True:
			out = process.stdout.read(1)
			if out == '' and process.poll() != None:
				break
			if out != '' and not args.quiet:
				sys.stdout.write(out)
				sys.stdout.flush()

		log.debug("mplayer decoding complete")


class FAACEncoder():
	file_path = ""

	def __init__(self, file_path):
		self.file_path = file_path

	def encode(self):
		# Start encoding
		log.debug("Starting AAC encoding with FAAC")
		prev_dir = os.getcwd()
		os.chdir(args.scratch_dir)

		if args.resume_previous and os.path.isfile("audiodump.aac"):
			os.chdir(prev_dir)
			log.debug("audiodump.aac already exists in scratch directory; cancelling encode")
			return True

		cmd = ["faac", "-q", str(args.faac_quality), self.file_path]
		
		process = subprocess.Popen(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE)

		while True:
			err = process.stderr.read(1)
			if err == '' and process.poll() != None:
				break
			if err != '' and not args.quiet:
				sys.stderr.write(err)
				sys.stderr.flush()

		os.chdir(prev_dir)
		log.debug("FAAC complete")

class MP4Box():
	video_path = audio_path = video_fps = video_pixel_ar = ""

	def __init__(self, video_path, audio_path, video_fps, video_pixel_ar):
		self.video_path = video_path
		self.audio_path = audio_path
		self.video_fps = str(video_fps)
		self.video_pixel_ar = video_pixel_ar

	def package(self):
		prev_dir = os.getcwd()
		os.chdir(args.scratch_dir)

		cmd = ["MP4Box", "output.mp4", "-add", self.video_path, "-fps", self.video_fps, "-par", "1=" + self.video_pixel_ar, "-add", self.audio_path, "-tmp", args.scratch_dir, "-itags", "name=" + args.name]

		process = subprocess.Popen(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE)

		while True:
			out = process.stdout.read(1)
			if out == '' and process.poll() != None:
				break
			if out != '' and not args.quiet:
				sys.stdout.write(out)
				sys.stdout.flush()

		log.debug("MP4Box process complete")

		# When complete, change back to original directory
		os.chdir(prev_dir)


def hex_edit_video_file(path):
	with open(path, 'r+b') as f:
		f.seek(7)
		f.write("\x29")
	# File is automatically closed		


def delete_temp_files(scratch_dir):
	# Get possible file extensions 
	t = MKVTrack()
	extensions = t.get_possible_extensions()

	for extension in extensions:
		temp_video = scratch_dir + "temp_video" + extension
		temp_audio = scratch_dir + "temp_audio" + extension
		if os.path.isfile(temp_video):
			log.debug("Cleaning up: deleting %s" % temp_video)
			os.unlink(temp_video)
		if os.path.isfile(temp_audio):
			log.debug("Cleaning up: deleting %s" % temp_audio)
			os.unlink(temp_audio)

	# Check for audiodump.aac and audiodump.wav
	other_files = ('audiodump.aac', 'audiodump.wav')
	for name in other_files:
		if os.path.isfile(scratch_dir + name):
			log.debug("Cleaning up: deleting %s" % scratch_dir + name)
			os.unlink(scratch_dir + name)

# Main program begins

parser = argparse.ArgumentParser(description='Parse command line arguments for XenonMKV.')
parser.add_argument('source_file', help='Path to the source MKV file')
parser.add_argument('-d', '--destination', help='Directory to output the destination .mp4 file (default: current directory)', default='.')
parser.add_argument('-q', '--quiet', help='Do not display output or progress from dependent tools', action='store_true')
parser.add_argument('-v', '--verbose', help='Verbose output', action='store_true')
parser.add_argument('-vv', '--very-verbose', help='Verbose and debug output', action='store_true')
parser.add_argument('-nrp', '--no-round-par', help='When processing video, do not round pixel aspect ratio from 0.98 to 1.01 to 1:1.', action='store_true')
parser.add_argument('-irf', '--ignore-reference-frames', help='If the source video has too many reference frames to play on low-powered devices (Xbox, PlayBook), continue converting anyway', action='store_true')
parser.add_argument('-sd', '--scratch-dir', help='Specify a scratch directory where temporary files should be stored (default: /var/tmp/)', default='/var/tmp/')
parser.add_argument('-fq', '--faac-quality', help='Quality setting for FAAC when encoding WAV files to AAC. Defaults to 150 (see http://wiki.hydrogenaudio.org/index.php?title=FAAC)', default=150)
parser.add_argument('-rp', '--resume-previous', help='Resume a previous run (do not recreate files if they already exist). Useful for debugging quickly if a conversion has already partially succeeded.', action='store_true')
parser.add_argument('-n', '--name', help='Specify a name for the final MP4 container. Defaults to the original file name.', default="")
parser.add_argument('-st', '--select-tracks', help="If there are multiple tracks in the MKV file, prompt to select which ones will be used. By default, the last video and tracks flagged as 'default' in the MKV file will be used.", action='store_true')

if len(sys.argv) < 2:
	parser.print_help()
	sys.exit(1)

log = logging.getLogger("xenonmkv")
console_handler = logging.StreamHandler()
formatter = logging.Formatter('%(asctime)s - %(name)s [%(levelname)s] %(message)s')
console_handler.setFormatter(formatter)
log.addHandler(console_handler)

args = parser.parse_args()
source_file = args.source_file
destination = args.destination
if args.quiet:
	log.setLevel(logging.ERROR)
elif args.very_verbose:
	log.setLevel(logging.DEBUG)
	log.debug("Using debug/very verbose mode output")
elif args.verbose:
	log.setLevel(logging.INFO)

log.debug("Starting XenonMKV")

# Always ensure destination path ends with a slash
if not destination.endswith('/'):
	destination += '/'

if not args.scratch_dir.endswith('/'):
	args.scratch_dir += '/'

# Check if source file exists
if not os.path.isfile(source_file):
	log.critical("Source file %s does not exist" % source_file)
	sys.exit(1)

source_basename = os.path.basename(source_file)
source_noext = source_basename[0:source_basename.rindex(".")]

if not args.name:
	args.name = source_noext
	log.debug("Using '%s' as final container name" % args.name)	

# Check if destination directory exists
if not os.path.isdir(destination):
	log.critical("Destination directory %s does not exist" % destination)
	sys.exit(1)

log.info("Loading source file %s " % source_file)

to_convert = MKVFile(source_file)
to_convert.get_mkvinfo()

# Check for multiple tracks
if to_convert.has_multiple_av_tracks():
	log.debug("Source file %s has multiple A/V tracks" % source_file)
	if args.select_tracks:
		# TODO: Add selector for tracks
		# Handle this scenario: prompt the user to select specific tracks,
		# or read command line arguments, or something.
		log.warning("Multiple track selection support is not yet implemented. Using default tracks")
		to_convert.set_default_av_tracks()
	else:
		log.debug("Selecting default audio and video tracks")
		to_convert.set_default_av_tracks()
else:
	# Pick default (or only) audio/video tracks
	log.debug("Source file %s has 1 audio and 1 video track; using these" % source_file)
	to_convert.set_default_av_tracks()

# Next phase: Extract MKV files to scratch directory
(video_file, audio_file) = to_convert.extract_mkv()

# If needed, hex edit the video file to make it compliant with a lower h264 profile level
if video_file.endswith(".h264"):
	hex_edit_video_file(video_file)

# Detect which audio codec is in place and dump audio to WAV accordingly
if to_convert.get_audio_track().needs_recode:
	log.debug("Audio track %s needs to be re-encoded to a 2-channel AAC file" % audio_file)
	audio_dec = AudioDecoder(audio_file)
	audio_dec.decode()

	# Once audio has been decoded to a WAV, use the FAAC application to encode it to .aac
	faac_enc = FAACEncoder(args.scratch_dir + "/audiodump.wav")
	faac_enc.encode()
	encoded_audio = args.scratch_dir + "/audiodump.aac"
else:
	# Bypass this whole encoding shenanigans and just reference the already-valid audio file
	encoded_audio = audio_file

# Now, throw things back together into a .mp4 container with MP4Box.
video_track = to_convert.get_video_track()
mp4box = MP4Box(video_file, encoded_audio, video_track.frame_rate, video_track.pixel_ar)
mp4box.package()

# Move the file to the destination directory with the original name
dest_path = destination + source_noext + ".mp4"
os.rename(args.scratch_dir + "/output.mp4", dest_path)

log.info("Processing of %s complete; file saved as %s" % (source_file, dest_path))

# Delete temporary files if possible
delete_temp_files(args.scratch_dir)

log.debug("XenonMKV run complete")

