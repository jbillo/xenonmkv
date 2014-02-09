#!/usr/bin/env python

import subprocess, sys

if len(sys.argv) < 2 or sys.argv[1].lower() == '-h' or sys.argv[1].lower() == '--help':
	print "Usage: {0} source_directory <xenonmkv_parameters>".format(sys.argv[0])
	print "See xenonmkv.py --help for additional information"
	sys.exit(1)

result = subprocess.check_output(['/usr/bin/find', sys.argv[1], '-name', '*.mkv'])

lines = result.split('\n')
print "Processing the following files in a batch:"
if not lines[-1]:
	lines = lines[0:-1]

for line in lines:
	print line

print

for line in lines:
	if line:
		subprocess.check_output(['./xenonmkv.py'] + sys.argv[2:] + [line])
