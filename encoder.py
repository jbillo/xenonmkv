import os
import sys

from process_handler import ProcessHandler

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

		cmd = [self.args.tool_paths["faac"], "-q", str(self.args.faac_quality), self.file_path]
		self.log.debug("Executing '%s'" % ' '.join(cmd))
		ph = ProcessHandler(self.args)
		process = ph.start_process(cmd)

		"""FIXME: On Windows, this causes issues. Technically if Linux/OSX didn't have any content
		in stderr, faac could run into the same error.
		
		The problem comes down to the fact that stderr.read(1) is blocking. If there is no content
		output to stderr, the call will never return and proceed to the next phase.
		
		An answer to http://stackoverflow.com/questions/375427/non-blocking-read-on-a-subprocess-pipe-in-python
		seems to have the method - combining a Queue and threading in order to allow non-blocking reads.
		
		This should probably all be integrated into the ProcessHandler class.
		"""
		while True:
			err = process.stderr.read(1)
			if err == '' and process.poll() != None:
				break
			if err != '' and not self.args.quiet:
				sys.stderr.write(err)
				sys.stderr.flush()

		os.chdir(prev_dir)
		self.log.debug("FAAC encoding to AAC audio file complete")
