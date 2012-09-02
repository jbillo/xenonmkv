import os

class FileUtils:
	log = None

	def __init__(self, log):
		self.log = log
		
	def check_dest_dir(self, destination):
		if not os.path.isdir(destination):
			log.critical("Destination directory %s does not exist" % destination)
			sys.exit(1)
			
	def check_source_file(self, source_file):
		if not os.path.isfile(source_file):
			log.critical("Source file %s does not exist" % source_file)
			sys.exit(1)

	def hex_edit_video_file(self, path):
		with open(path, 'r+b') as f:
			f.seek(7)
			f.write("\x29")
		# File is automatically closed		

	def delete_temp_files(self, scratch_dir, extensions):
		for extension in extensions:
			temp_video = scratch_dir + "temp_video" + extension
			temp_audio = scratch_dir + "temp_audio" + extension
			if os.path.isfile(temp_video):
				self.log.debug("Cleaning up: deleting %s" % temp_video)
				os.unlink(temp_video)
			if os.path.isfile(temp_audio):
				self.log.debug("Cleaning up: deleting %s" % temp_audio)
				os.unlink(temp_audio)

		# Check for audiodump.aac and audiodump.wav
		other_files = ('audiodump.aac', 'audiodump.wav')
		for name in other_files:
			if os.path.isfile(scratch_dir + name):
				self.log.debug("Cleaning up: deleting %s" % scratch_dir + name)
				os.unlink(scratch_dir + name)


