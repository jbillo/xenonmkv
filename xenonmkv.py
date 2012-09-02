#!/usr/bin/python

# XenonMKV for PlayBook/revision 2.0

import argparse
import sys
import os
import subprocess
import logging

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


class MKVFile():
	path = ""
	tracks = []
	duration = 0

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
		parameters = "Video;%Height%,%Width%,%Format_Settings_RefFrames%,%Language%,%FrameRate%,%CodecID%"
		log.debug("Executing 'mediainfo %s %s'" % (parameters, self.get_path()))
		result = subprocess.check_output(["mediainfo", "--Inform=" + parameters, self.get_path()])
		log.debug("mediainfo finished; attempting to parse output for video settings")
		return self.parse_mediainfo(result)

	def get_mediainfo_audio(self):
		parameters = "Video;%Height%,%Width%,%Format_Settings_RefFrames%"
		log.debug("Executing 'mediainfo %s %s'" % (parameters, self.get_path()))
		result = subprocess.check_output(["mediainfo", "--Inform=" + parameters, self.get_path()])
		log.debug("mediainfo finished; attempting to parse output for audio settings")
		return self.parse_mediainfo(result)

	def parse_mediainfo(self, result):
		output = result.split(",")

		return output

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

		while track_detect_string in track_info:
			track = MKVTrack()
			if track_detect_string in track_info:
				track_info = track_info[track_info.index(track_detect_string) + len(track_detect_string):]
			else:
				break

			# Get track type and number out of this block
			track_type = MKVInfoParser.parse_track_type(track_info)
			track_number = MKVInfoParser.parse_track_number(track_info)

			if track_type == "video":
				track = VideoTrack()
				track.number = track_number

				# height, width, reference_frames are returned in mediainfo
				mediainfo = self.get_mediainfo_video()
				track.height = int(mediainfo[0])
				track.width = int(mediainfo[1])
				track.reference_frames = int(mediainfo[2])
				track.language = mediainfo[3]
				track.frame_rate = float(mediainfo[4])
				track.codec_id = mediainfo[5]

				log.debug("Video track has dimensions %ix%i with %i reference frames" % (track.width, track.height, track.reference_frames))
				log.debug("Video track has %f FPS and codec %s" % (track.frame_rate, track.codec_id))

				if self.reference_frames_exceeded(track):
					raise Exception("Video file contains too many reference frames to play properly on low-power devices")
				
				
					

			elif track_type == "audio":
				track = AudioTrack()
				track.number = track_number

				mediainfo = self.get_mediainfo_audio()
			
			else:
				raise Exception("Unrecognized track type %s" % track_type)


	def reference_frames_exceeded(self, video_track):
		return reference_frame.ReferenceFrameValidator.validate(video_track.height, video_track.width, video_track.reference_frames)

class MKVTrack():
	number = 0
	uid = track_type = language = codec_id = ""

	def set_number(self, number):
		self.number = number

	def set_uid(self, uid):
		self.uid = uid

	def set_type(self, track_type):
		self.track_type = track_type

	def get_number(self):
		return self.number

	def get_uid(self):
		return self.uid

	def get_type(self):
		return self.track_type

class VideoTrack(MKVTrack):
	height = width = reference_frames = 0
	frame_rate = 0.0

class AudioTrack(MKVTrack):
	length = 0

parser = argparse.ArgumentParser(description='Parse command line arguments for XenonMKV.')
parser.add_argument('source_file', help='Path to the source MKV file')
parser.add_argument('-d', '--destination', help='Directory to output the destination .mp4 file (default: current directory)', default='.')
parser.add_argument('-v', '--verbose', help='Verbose and debug output', action='store_true')

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
if args.verbose:
	log.setLevel(logging.DEBUG)
	log.debug("Using verbose mode for output")
else:
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



