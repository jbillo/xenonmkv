import os
import subprocess
import sys

class FAACEncoder():
	file_path = ""
	log = args = None

	def __init__(self, file_path, log, args):
		self.file_path = file_path
		self.log = log
		self.args = args

	def encode(self):
		# Start encoding
		self.log.debug("Starting AAC encoding with FAAC")
		prev_dir = os.getcwd()
		os.chdir(self.args.scratch_dir)

		if self.args.resume_previous and os.path.isfile("audiodump.aac"):
			os.chdir(prev_dir)
			self.log.debug("audiodump.aac already exists in scratch directory; cancelling encode")
			return True

		cmd = ["faac", "-q", str(self.args.faac_quality), self.file_path]
		
		process = subprocess.Popen(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE)

		while True:
			err = process.stderr.read(1)
			if err == '' and process.poll() != None:
				break
			if err != '' and not self.args.quiet:
				sys.stderr.write(err)
				sys.stderr.flush()

		os.chdir(prev_dir)
		self.log.debug("FAAC encoding to AAC audio file complete")
