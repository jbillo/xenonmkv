#!/usr/bin/python

# XenonMKV for PlayBook/revision 2.0

import argparse
import sys
import os
import subprocess
import logging

class MKVFile():
	path = ""
	tracks = []
	duration = 0

	def __init__(self, path):
		self.path = path
	
	def get_path(self):
		return self.path

	def get_mkvinfo(self):
		track_count = 0
		result = subprocess.check_output(["mkvinfo", self.get_path()])
		self.parse_mkvinfo(result)

	def get_duration(self):
		return self.duration

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

	def parse_mkvinfo(self, result):
		track_detect_string = "| + A track"
		
		if not track_detect_string in result:
			raise Exception("mkvinfo: output did not contain any tracks")

		track_info = result
		self.duration = self.parse_audio_duration(track_info)



		# Split output into new line array
		#lines = result.split("\n")

"""
		for line in range(0, len(lines)):
			# We need a numerical index so we can skip forward here
			line = lines[line]
			if line == track_detect_string:
				# Create new track object
				track_count += 1
				track = MKVTrack()
"""


class MKVTrack():
	number = 0
	uid = track_type = ""

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

to_convert = MKVFile(source_file)
to_convert.get_mkvinfo()



