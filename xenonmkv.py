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
	tracks = []
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
		parameters = "Audio;%ID%,%CodecID%,%Language%,\n"
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

				log.debug("Audio track %i has codec %s and language %s" % (track.number, track.codec_id, track.language))
			
			else:
				raise Exception("Unrecognized track type %s" % track_type)

			log.debug("All properties set for %s track %i" % (track_type, track.number))
			self.tracks.append(track)

		# All tracks detected here
		log.debug("All tracks detected from mkvinfo output; total number is %i" % len(self.tracks))

	def reference_frames_exceeded(self, video_track):
		return reference_frame.ReferenceFrameValidator.validate(video_track.height, video_track.width, video_track.reference_frames)

	def has_multiple_av_tracks(self):
		video_tracks = audio_tracks = 0
		for track in self.tracks:
			if track.track_type == "video":
				video_tracks += 1
			elif track.track_type == "audio":
				audio_tracks += 1

		return (video_tracks > 1 or audio_tracks > 1)

	def set_default_av_tracks(self):
		for track in self.tracks:
			if track.track_type == "video" and track.default:
				self.video_track_id = track.number
			elif track.track_type == "audio" and track.default:
				self.audio_track_id = track.number

class MKVTrack():
	number = 0
	uid = track_type = language = codec_id = ""
	default = True

class VideoTrack(MKVTrack):
	height = width = reference_frames = 0
	frame_rate = display_ar = 0.0
	pixel_ar = ""

class AudioTrack(MKVTrack):
	length = 0

parser = argparse.ArgumentParser(description='Parse command line arguments for XenonMKV.')
parser.add_argument('source_file', help='Path to the source MKV file')
parser.add_argument('-d', '--destination', help='Directory to output the destination .mp4 file (default: current directory)', default='.')
parser.add_argument('-v', '--verbose', help='Verbose output', action='store_true')
parser.add_argument('-vv', '--very-verbose', help='Verbose and debug output', action='store_true')
parser.add_argument('-nrp', '--no-round-par', help='When processing video, do not round pixel aspect ratio from 0.98 to 1.01 to 1:1.', action='store_true')
parser.add_argument('-irf', '--ignore-reference-frames', help='If the source video has too many reference frames to play on low-powered devices (Xbox, PlayBook), continue converting anyway', action='store_true')

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
if args.very_verbose:
	log.setLevel(logging.DEBUG)
	log.debug("Using debug/very verbose mode output")
elif args.verbose:
	log.setLevel(logging.INFO)

log.debug("Starting XenonMKV")

# Always ensure destination path ends with a slash
if not destination.endswith('/'):
	destination += '/'

# Check if source file exists
if not os.path.isfile(source_file):
	print "Error: File " + source_file + " does not exist"
	sys.exit(1)

# Check if destination directory exists
if not os.path.isdir(destination):
	print "Error: Destination " + destination + " does not exist"
	sys.exit(1)

log.info("Loading source file %s " % source_file)

to_convert = MKVFile(source_file)
to_convert.get_mkvinfo()

# Check for multiple tracks
if to_convert.has_multiple_av_tracks():
	# Handle this scenario: either prompt the user to select specific tracks,
	# or automatically detect tracks based on MKV defaults
	log.debug("Source file %s has multiple A/V tracks" % source_file)
else:
	# Pick appropriate audio/video tracks
	log.debug("Source file %s has 1 audio and 1 video track; using these" % source_file)
	to_convert.set_default_av_tracks()



