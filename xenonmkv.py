#!/usr/bin/python

# XenonMKV
# Jake Billo, jake@jakebillo.com
# https://github.com/jbillo/xenonmkv

import argparse
import sys
import os
import subprocess
import logging
import fractions

from reference_frame import ReferenceFrameValidator
from mkv_info_parser import MKVInfoParser
from file_utils import FileUtils
from track import *
from decoder import AudioDecoder
from encoder import FAACEncoder

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
		try: 
			result = subprocess.check_output(["mkvinfo", self.get_path()])
		except subprocess.CalledProcessError:
			log.critical("Error occurred while obtaining MKV information for %s - please make sure the file exists and is readable" % self.get_path())
			sys.exit(1)

		log.debug("mkvinfo finished; attempting to parse output")
		self.parse_mkvinfo(result)
		
	# Open the mediainfo process to obtain detailed info on the file.		
	def get_mediainfo(self, track_type):
		if track_type == "video":
			parameters = "Video;%ID%,%Height%,%Width%,%Format_Settings_RefFrames%,%Language%,%FrameRate%,%CodecID%,%DisplayAspectRatio%,~"
		else:
			parameters = "Audio;%ID%,%CodecID%,%Language%,%Channels%,~"
			
		log.debug("Executing 'mediainfo %s %s'" % (parameters, self.get_path()))
		result = subprocess.check_output(["mediainfo", "--Inform=" + parameters, self.get_path()])
		log.debug("mediainfo finished; attempting to parse output for %s settings" % track_type)
		return self.parse_mediainfo(result)		

	def parse_mediainfo(self, result):
		output = []
		result = result.replace("\n", "")
		
		# Obtain multiple tracks if they are present
		lines = result.split("~")
		lines = lines[0:-1] # remove last tilde separator character
		
		for line in lines:
			# remove last \n element from array that will always be present
			values = line.split(",")
			# print values
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

		mediainfo_video_output = self.get_mediainfo("video")
		mediainfo_audio_output = self.get_mediainfo("audio")

		# For ease of use, throw these values into a dictionary with the key being the track ID.
		mediainfo = {}

		for mediainfo_track in mediainfo_video_output:
			mediainfo[int(mediainfo_track[0])] = mediainfo_track[1:]
		for mediainfo_track in mediainfo_audio_output:
			mediainfo[int(mediainfo_track[0])] = mediainfo_track[1:]

		# Create a new parser that can be used for all tracks
		info_parser = MKVInfoParser(log)

		while track_detect_string in track_info:
			track = MKVTrack(log)
			if track_detect_string in track_info:
				track_info = track_info[track_info.index(track_detect_string) + len(track_detect_string):]
			else:
				break

			# Get track type and number out of this block
			track_type = info_parser.parse_track_type(track_info)
			track_number = info_parser.parse_track_number(track_info)

			# Set individual track properties for the object by track ID
			if track_type in ("video", "audio"):
				mediainfo_track = mediainfo[track_number]

			if track_type == "video":
				track = VideoTrack(log)
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
					log.debug("Reference frame value '%s' in track %i could not be parsed; assuming 0 reference frames" % (mediainfo_track[2], track.number))
					
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
				track = AudioTrack(log)
				track.number = track_number
				track.default = info_parser.parse_track_is_default(track_info)

				track.codec_id = mediainfo_track[0]
				track.language = mediainfo_track[1]
				track.channels = int(mediainfo_track[2])

				# Indicate if the audio track needs a recode. By default, it does.
				# Check that the audio type is AAC, and if the number of channels in the file
				# is less than or equal to what was specified on the command line, no recode is necessary
				if track.codec_id == "A_AAC":
					# Check on the number of channels in the file versus the argument passed.
					if track.channels <= args.channels:
						log.debug("Audio track %i will not need to be re-encoded (%s channels specified, %i channels in file)" % (track.number, args.channels, track.channels))
						track.needs_recode = False

				log.debug("Audio track %i has codec %s and language %s" % (track.number, track.codec_id, track.language))
				log.debug("Audio track %i has %i channel(s)" % (track.number, track.channels))
				
			else:
				# Unrecognized track type. Don't completely abort processing, but do log it.
				# Do not proceed to add this to the global tracks list.
				log.debug("Unrecognized track type '%s' in %i; skipping" % (track_type, track_number)) 
				continue

			log.debug("All properties set for %s track %i" % (track_type, track.number))
			track.track_type = track_type
			self.tracks[track.number] = track

		# All tracks detected here
		log.debug("All tracks detected from mkvinfo output; total number is %i" % len(self.tracks))

	def reference_frames_exceeded(self, video_track):
		return ReferenceFrameValidator.validate(video_track.height, video_track.width, video_track.reference_frames)

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
				
		# Check again if we have tracks specified here. If not, nothing was hinted as default
		# in the MKV file and we should really pick the first audio and first video track.
		if not self.video_track_id:
			log.debug("No default video track was specified in '%s'; using first available" % self.path)
			for track_id in self.tracks:
				track = self.tracks[track_id]
				if track.track_type == "video":
					self.video_track_id = track.number
					log.debug("First available video track in file is %i" % self.video_track_id)
					break
					
		if not self.audio_track_id:
			log.debug("No default audio track was specified in '%s'; using first available" % self.path)
			for track_id in self.tracks:
				track = self.tracks[track_id]
				if track.track_type == "audio":
					self.audio_track_id = track.number
					log.debug("First available audio track in file is %i" % self.audio_track_id)
					break
					
		# If we still don't have a video and audio track specified, it's time to throw an error.
		if not self.video_track_id or not self.audio_track_id:
			raise Exception("Could not select an audio and video track for MKV file '%s'" % self.path)

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
			
		log.debug("Using video track from MKV file with ID %i" % self.video_track_id)
		log.debug("Using audio track from MKV file with ID %i" % self.audio_track_id)

		try:
			temp_video_file = "temp_video" + self.tracks[self.video_track_id].get_filename_extension()
			temp_audio_file = "temp_audio" + self.tracks[self.audio_track_id].get_filename_extension()
		except UnsupportedCodecError:
			log.critical("The codec used for the video or audio track is unsupported")
			return None, None

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

		if process.returncode != 0:
			log.critical("An error occurred while extracting tracks from %s - please make sure this file exists and is readable" % self.get_path())
			sys.exit(1)

		temp_video_file = os.getcwd() + "/" + temp_video_file
		temp_audio_file = os.getcwd() + "/" + temp_audio_file

		os.chdir(prev_dir)
		log.debug("mkvextract finished; attempting to parse output")
		return (temp_video_file, temp_audio_file)

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
parser.add_argument('-c', '--channels', help='Specify the maximum number of channels that are acceptable in the output file. Certain devices (Xbox) will not play audio with more than two channels. If the audio needs to be re-encoded at all, it will be downmixed to two channels only. Possible values for this option are 2 (stereo); 4 (surround); 5.1 or 6 (full 5.1); 7.1 or 8 (full 7.1 audio). For more details, view the README file.', default="6")
parser.add_argument('-fq', '--faac-quality', help='Quality setting for FAAC when encoding WAV files to AAC. Defaults to 150 (see http://wiki.hydrogenaudio.org/index.php?title=FAAC)', default=150)
parser.add_argument('-rp', '--resume-previous', help='Resume a previous run (do not recreate files if they already exist). Useful for debugging quickly if a conversion has already partially succeeded.', action='store_true')
parser.add_argument('-n', '--name', help='Specify a name for the final MP4 container. Defaults to the original file name.', default="")
parser.add_argument('-st', '--select-tracks', help="If there are multiple tracks in the MKV file, prompt to select which ones will be used. By default, the last video and tracks flagged as 'default' in the MKV file will be used.", action='store_true')
parser.add_argument('-preserve', '--preserve-temp-files', help="Preserve temporary files on the filesystem rather than deleting them at the end of each run.", action='store_true')
parser.add_argument("-p", '--profile', help="Select a standardized device profile for encoding. Current profile options are: xbox360, playbook", default="")
parser.add_argument("-eS", "--error-filesize", help="Stop processing this file if it is over 4GiB. Files of this size will not be processed correctly by some devices such as the Xbox 360, and they will not save correctly to FAT32-formatted storage. By default, you will only see a warning message, and processing will continue.", action="store_true")

if len(sys.argv) < 2:
	parser.print_help()
	sys.exit(1)

log = logging.getLogger("xenonmkv")
console_handler = logging.StreamHandler()
formatter = logging.Formatter('%(asctime)s - %(name)s [%(levelname)s] %(message)s')
console_handler.setFormatter(formatter)
log.addHandler(console_handler)

args = parser.parse_args()

if args.quiet:
	log.setLevel(logging.ERROR)
elif args.very_verbose:
	log.setLevel(logging.DEBUG)
	log.debug("Using debug/very verbose mode output")
elif args.verbose:
	log.setLevel(logging.INFO)
	
# Check for 5.1/7.1 audio with the channels setting
if args.channels == "5.1":
	args.channels = "6"
elif args.channels == "7.1":
	args.channels = "8"
if args.channels not in ('2', '4', '6', '8'):
	log.warning("An invalid number of channels was specified. Falling back to 2-channel stereo audio.")
	args.channels = "2"
	
if args.profile:
	if args.profile == "xbox360":
		args.channels = "2"
		args.error_filesize = True
	elif args.profile == "playbook":
		args.channels = "6"
		args.error_filesize = False
	else:
		log.warning("Unrecognized device profile %s" % args.profile)
		args.profile = ""

log.debug("Starting XenonMKV")

# Check if we have a full file path or are just specifying a file
if "/" not in args.source_file:
	log.debug("Ensuring that we have a complete path to %s" % args.source_file)
	args.source_file = os.getcwd() + "/" + args.source_file
	log.debug("%s will be used to reference the original MKV file" % args.source_file)
	
# Always ensure destination path ends with a slash
if not args.destination.endswith('/'):
	args.destination += '/'

if not args.scratch_dir.endswith('/'):
	args.scratch_dir += '/'

# Initialize file utilities
f_utils = FileUtils(log, args)

# Check if source file exists and is an appropriate size
f_utils.check_source_file(args.source_file)

source_basename = os.path.basename(args.source_file)
source_noext = source_basename[0:source_basename.rindex(".")]

if not args.name:
	args.name = source_noext
	log.debug("Using '%s' as final container name" % args.name)	

# Check if destination directory exists
f_utils.check_dest_dir(args.destination)

log.info("Loading source file %s " % args.source_file)

to_convert = MKVFile(args.source_file)
to_convert.get_mkvinfo()

# Check for multiple tracks
if to_convert.has_multiple_av_tracks():
	log.debug("Source file %s has multiple audio and/or video tracks" % args.source_file)
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
	log.debug("Source file %s has 1 audio and 1 video track; using these" % args.source_file)
	to_convert.set_default_av_tracks()

# Next phase: Extract MKV files to scratch directory
(video_file, audio_file) = to_convert.extract_mkv()

if not video_file or not audio_file:
	log.critical("One or more errors occurred during MKV extraction")
	sys.exit(1)

# If needed, hex edit the video file to make it compliant with a lower h264 profile level
if video_file.endswith(".h264"):
	f_utils.hex_edit_video_file(video_file)

# Detect which audio codec is in place and dump audio to WAV accordingly
if to_convert.get_audio_track().needs_recode:
	log.debug("Audio track %s needs to be re-encoded" % audio_file)
	audio_dec = AudioDecoder(audio_file, log, args)
	audio_dec.decode()

	# Once audio has been decoded to a WAV, use the FAAC application to encode it to .aac
	faac_enc = FAACEncoder(args.scratch_dir + "audiodump.wav", log, args)
	faac_enc.encode()
	encoded_audio = args.scratch_dir + "audiodump.aac"
else:
	# Bypass this whole encoding shenanigans and just reference the already-valid audio file
	encoded_audio = audio_file

# Now, throw things back together into a .mp4 container with MP4Box.
video_track = to_convert.get_video_track()
mp4box = MP4Box(video_file, encoded_audio, video_track.frame_rate, video_track.pixel_ar)
mp4box.package()

# Move the file to the destination directory with the original name
dest_path = args.destination + source_noext + ".mp4"
os.rename(args.scratch_dir + "output.mp4", dest_path)

log.info("Processing of %s complete; file saved as %s" % (args.source_file, dest_path))

# Delete temporary files if possible
if not args.preserve_temp_files:
	t = MKVTrack(log)
	f_utils.delete_temp_files(args.scratch_dir, t.get_possible_extensions())

log.debug("XenonMKV run complete")

