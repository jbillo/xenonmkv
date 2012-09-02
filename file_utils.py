#!/usr/bin/python

class FileUtils:
	def hex_edit_video_file(self, path):
		with open(path, 'r+b') as f:
			f.seek(7)
			f.write("\x29")
		# File is automatically closed		


	def delete_temp_files(self, scratch_dir):
		# Get possible file extensions 
		t = MKVTrack()
		extensions = t.get_possible_extensions()

		for extension in extensions:
			temp_video = scratch_dir + "temp_video" + extension
			temp_audio = scratch_dir + "temp_audio" + extension
			if os.path.isfile(temp_video):
				log.debug("Cleaning up: deleting %s" % temp_video)
				os.unlink(temp_video)
			if os.path.isfile(temp_audio):
				log.debug("Cleaning up: deleting %s" % temp_audio)
				os.unlink(temp_audio)

		# Check for audiodump.aac and audiodump.wav
		other_files = ('audiodump.aac', 'audiodump.wav')
		for name in other_files:
			if os.path.isfile(scratch_dir + name):
				log.debug("Cleaning up: deleting %s" % scratch_dir + name)
				os.unlink(scratch_dir + name)


