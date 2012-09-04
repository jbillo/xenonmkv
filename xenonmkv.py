#!/usr/bin/env python

# XenonMKV
# Jake Billo, jake@jakebillo.com
# https://github.com/jbillo/xenonmkv

import sys
import os

# Check for Python version before running argparse import
if sys.version_info[0] == 2 and sys.version_info[1] < 7:
	print "You need to be running at least Python 2.7 to use this application."
	print "Try running /usr/bin/python2.7 %s" % sys.argv[0]
	sys.exit(1)

import argparse
import subprocess
import logging
import traceback

from mkv_info_parser import MKVInfoParser
from file_utils import FileUtils
from track import *
from decoder import AudioDecoder
from encoder import FAACEncoder
from mp4box import MP4Box
from process_handler import ProcessHandler
from mkvfile import MKVFile

def cleanup_temp_files(args, log):
	if not args.preserve_temp_files:
		t = MKVTrack(log)
		f_utils = FileUtils(log, args)		
		f_utils.delete_temp_files(args.scratch_dir, t.get_possible_extensions())

# Log an exception with the stack trace in debug mode, and exit if it's a critical log type
def log_exception(log, source, e, log_type="critical"):
	getattr(log, log_type)(source + ": " + e.message)
	log.debug(traceback.format_exc())
	if log_type == "critical":
		sys.exit(1)

def main():
	# Main program begins
	log = logging.getLogger("xenonmkv")
	console_handler = logging.StreamHandler()
	formatter = logging.Formatter('%(asctime)s - %(name)s [%(levelname)s] %(message)s')
	console_handler.setFormatter(formatter)
	log.addHandler(console_handler)

	dependencies = ('mkvinfo', 'mediainfo', 'mkvextract', 'mplayer', 'faac', 'MP4Box')

	parser = argparse.ArgumentParser(description='Parse command line arguments for XenonMKV.')
	parser.add_argument('source_file', help='Path to the source MKV file')
	parser.add_argument('-d', '--destination', help='Directory to output the destination .mp4 file (default: current directory)', default='.')
	parser.add_argument('-sd', '--scratch-dir', help='Specify a scratch directory where temporary files should be stored (default: /var/tmp/)', default='/var/tmp/')
	parser.add_argument('-cfg', '--config-file', help='(Not yet implemented) Provide a configuration file that contains default arguments or settings for the application', default='')
	parser.add_argument("-p", '--profile', help="Select a standardized device profile for encoding. Current profile options are: xbox360, playbook", default="")

	output_group = parser.add_argument_group("Output options")
	output_group.add_argument('-q', '--quiet', help='Do not display output or progress from dependent tools', action='store_true')
	output_group.add_argument('-v', '--verbose', help='Verbose output', action='store_true')
	output_group.add_argument('-vv', '--debug', help='Highly verbose debug output', action='store_true')
	output_group.add_argument('-pf', '--print-file', help='Output filenames before and after converting', action='store_true')

	video_group = parser.add_argument_group("Video options", "Options for processing video.")
	video_group.add_argument('-nrp', '--no-round-par', help='When processing video, do not round pixel aspect ratio from 0.98 to 1.01 to 1:1.', action='store_true')
	video_group.add_argument('-irf', '--ignore-reference-frames', help='If the source video has too many reference frames to play on low-powered devices (Xbox, PlayBook), continue converting anyway', action='store_true')

	audio_group = parser.add_argument_group("Audio options", "Select custom audio decoding and encoding options.")
	audio_group.add_argument('-c', '--channels', help='Specify the maximum number of channels that are acceptable in the output file. Certain devices (Xbox) will not play audio with more than two channels. If the audio needs to be re-encoded at all, it will be downmixed to two channels only. Possible values for this option are 2 (stereo); 4 (surround); 5.1 or 6 (full 5.1); 7.1 or 8 (full 7.1 audio). For more details, view the README file.', default="6")
	audio_group.add_argument('-fq', '--faac-quality', help='Quality setting for FAAC when encoding WAV files to AAC. Defaults to 150 (see http://wiki.hydrogenaudio.org/index.php?title=FAAC)', default=150)

	proc_group = parser.add_argument_group("File and processing options", "These options determine how XenonMKV processes files and their contents.")
	proc_group.add_argument('-st', '--select-tracks', help="(Not yet implemented) If there are multiple tracks in the MKV file, prompt to select which ones will be used. By default, the last video and tracks flagged as 'default' in the MKV file will be used.", action='store_true')
	proc_group.add_argument('-vt', '--video-track', help="Use the specified video track. If not present in the file, the default track will be used.", type=int)
	proc_group.add_argument('-at', '--audio-track', help="Use the specified audio track. If not present in the file, the default track will be used.", type=int)
	proc_group.add_argument('-rp', '--resume-previous', help='Resume a previous run (do not recreate files if they already exist). Useful for debugging quickly if a conversion has already partially succeeded.', action='store_true')
	proc_group.add_argument('-n', '--name', help='Specify a name for the final MP4 container. Defaults to the original file name.', default="")
	proc_group.add_argument('-preserve', '--preserve-temp-files', help="Preserve temporary files on the filesystem rather than deleting them at the end of each run.", action='store_true')
	proc_group.add_argument("-eS", "--error-filesize", help="Stop processing this file if it is over 4GiB. Files of this size will not be processed correctly by some devices such as the Xbox 360, and they will not save correctly to FAT32-formatted storage. By default, you will only see a warning message, and processing will continue.", action="store_true")
	proc_group.add_argument('--mp4box-retries', help="Set the number of retry attempts for MP4Box to attempt to create a file (default: 3)", default=3, type=int)

	dep_group = parser.add_argument_group("Custom paths", "Set custom paths for the utilities used by XenonMKV.")
	for dependency in dependencies:
		dep_group.add_argument("--" + dependency.lower() + "-path", help="Set a custom complete path for the " + dependency + " tool. Any library under that path will also be loaded.")

	if len(sys.argv) < 2:
		parser.print_help()
		sys.exit(1)

	args = parser.parse_args()

	# Depending on the arguments, set the logging level appropriately.
	if args.quiet:
		log.setLevel(logging.ERROR)
	elif args.debug:
		log.setLevel(logging.DEBUG)
		log.debug("Using debug/highly verbose mode output")
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
	if os.sep not in args.source_file:
		log.debug("Ensuring that we have a complete path to %s" % args.source_file)
		args.source_file = os.path.join(os.getcwd(), args.source_file)
		log.debug("%s will be used to reference the original MKV file" % args.source_file)
	
	# Always ensure destination path ends with a slash
	if not args.destination.endswith(os.sep):
		args.destination += os.sep

	if not args.scratch_dir.endswith(os.sep):
		args.scratch_dir += os.sep

	# Initialize file utilities
	f_utils = FileUtils(log, args)

	# Check if all dependent applications are installed and available in PATH, or if they are specified
	# If so, store them in args.tool_paths so all classes have access to them as needed
	(args.tool_paths, args.library_paths) = f_utils.check_dependencies(dependencies)

	# Check if source file exists and is an appropriate size
	try: 
		f_utils.check_source_file(args.source_file)
	except IOError as e:
		log_exception(log, "check_source_file", e)
		
	source_basename = os.path.basename(args.source_file)
	source_noext = source_basename[0:source_basename.rindex(".")]

	if not args.name:
		args.name = source_noext
		log.debug("Using '%s' as final container name" % args.name)	

	# Check if destination directory exists
	try:
		f_utils.check_dest_dir(args.destination)
	except IOError as e:
		log_exception(log, "check_dest_dir", e)

	log.info("Loading source file %s " % args.source_file)

	if args.print_file:
		print "Processing: %s" % args.source_file

	try:
		to_convert = MKVFile(args.source_file, log, args)
		to_convert.get_mkvinfo()
	except Exception as e:
		cleanup_temp_files(args, log)
		log_exception(log, "get_mkvinfo", e)

	# If the user knows which A/V tracks they want, set them. MKVFile will not overwrite them.
	try: 
		if args.video_track:
			to_convert.set_video_track(args.video_track)
	except Exception as e:
		log_exception(log, "set_video_track", e, "warning")
	
	try:
		if args.audio_track:
			to_convert.set_audio_track(args.audio_track)	
	except Exception as e:
		log_exception(log, "set_audio_track", e, "warning")

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
	try:
		(video_file, audio_file) = to_convert.extract_mkv()
	except Exception as e:
		cleanup_temp_files(args, log)
		log_exception(log, "extract_mkv", e)

	# If needed, hex edit the video file to make it compliant with a lower h264 profile level
	if video_file.endswith(".h264"):
		f_utils.hex_edit_video_file(video_file)

	# Detect which audio codec is in place and dump audio to WAV accordingly
	if to_convert.get_audio_track().needs_recode:
		log.debug("Audio track %s needs to be re-encoded" % audio_file)
		audio_dec = AudioDecoder(audio_file, log, args)
		audio_dec.decode()

		# Once audio has been decoded to a WAV, use the FAAC application to encode it to .aac
		faac_enc = FAACEncoder(os.path.join(args.scratch_dir, "audiodump.wav"), log, args)
		faac_enc.encode()
		encoded_audio = os.path.join(args.scratch_dir, "audiodump.aac")
	else:
		# Bypass this whole encoding shenanigans and just reference the already-valid audio file
		encoded_audio = audio_file

	# Now, throw things back together into a .mp4 container with MP4Box.
	video_track = to_convert.get_video_track()
	mp4box = MP4Box(video_file, encoded_audio, video_track.frame_rate, video_track.pixel_ar, args, log)
	try:
		mp4box.package()
	except Exception as e:
		cleanup_temp_files(args, log)
		log_exception(log, "package", e)

	# Move the file to the destination directory with the original name
	dest_path = os.path.join(args.destination, source_noext + ".mp4")
	os.rename(os.path.join(args.scratch_dir, "output.mp4"), dest_path)

	log.info("Processing of %s complete; file saved as %s" % (args.source_file, dest_path))

	# Delete temporary files if possible
	cleanup_temp_files(args, log)

	log.debug("XenonMKV completed processing")
	if args.print_file:
		print "Completed: %s" % dest_path	
	


if __name__ == "__main__":
	main()

