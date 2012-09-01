#!/usr/bin/python

# XenonMKV for PlayBook/revision 2.0

import argparse
import sys
import os

parser = argparse.ArgumentParser(description='Parse command line arguments for XenonMKV.')
parser.add_argument('source_file', help='Path to the source MKV file')
parser.add_argument('-d', '--destination', help='Directory to output the destination .mp4 file (default: current directory)', default='.')

if len(sys.argv) < 2:
	parser.print_help()
	sys.exit(1)

args = parser.parse_args()
source_file = args.source_file
destination = args.destination

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



