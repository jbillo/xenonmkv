import os
import sys

class FileUtils:
	log = args = None
	FOUR_GIGS = (4*1024*1024*1024)

	def __init__(self, log, args):
		self.log = log
		self.args = args
		
	# Check if all dependent applications are installed on the system and present in PATH
	# TODO: Allow custom path to be specified for each of these if it is in a config file
	def check_dependencies(self, dependency_list):
		dependency_paths = {}
		library_paths = []
		
		ospath = os.defpath.split(os.pathsep)
		for app in dependency_list:
			app_present = False
			# Check if the custom app parameter is set
			if app.lower() + "_path" in self.args:
				custom_path = getattr(self.args, app.lower() + "_path")
				if custom_path and os.path.isfile(custom_path):
					self.log.debug("Using custom path for dependent application %s: %s" % (app, custom_path))
					dependency_paths[app.lower()] = custom_path
					# Because some applications have libraries in their own directory, add the base path of custom_path
					# to a variable that can later be used to set LD_LIBRARY_PATH.
					library_paths.append(os.path.dirname(custom_path))
					
					# Proceed to next application and do not let this one get overwritten with the default later on.
					continue
				elif not custom_path:
					# Fall through and set default version, but do not warn since the dependent application is not set
					pass
				else:
					self.log.error("Dependent application %s does not exist as %s; looking for default version" % (app, custom_path))
	
			for path in ospath:
				app_path = os.path.join(path, app)
				if os.path.isfile(app_path):
					self.log.debug("Found dependent application %s in %s" % (app, path))
					dependency_paths[app.lower()] = app_path
					app_present = True
					break
					
			if not app_present:
				raise IOError("Dependent application '%s' was not found in PATH. Please make sure it is installed." % app)
		
		return (dependency_paths, library_paths)
		
	def check_dest_dir(self, destination):
		if not os.path.isdir(destination):
			raise IOError("Destination directory %s does not exist" % destination)
			
	def check_source_file(self, source_file):
		if not os.path.isfile(source_file):
			raise IOError("Source file %s does not exist" % source_file)
			
		filesize = os.path.getsize(source_file)
		if (filesize >= self.FOUR_GIGS):
			self.log.warning("File size of %s is %i, which is over 4GiB. This file may not play on certain devices and cannot be copied to a FAT32-formatted storage medium." % (source_file, filesize))
			if self.args.error_filesize:
				raise IOError("Cancelling processing as file size limit of 4GiB is exceeded")

	def hex_edit_video_file(self, path):
		with open(path, 'r+b') as f:
			f.seek(7)
			f.write("\x29")
		# File is automatically closed when using 'with'		

	def delete_temp_files(self, scratch_dir, extensions):
		for extension in extensions:
			temp_video = os.path.join(scratch_dir, "temp_video" + extension)
			temp_audio = os.path.join(scratch_dir, "temp_audio" + extension)
			if os.path.isfile(temp_video):
				self.log.debug("Cleaning up: deleting %s" % temp_video)
				os.unlink(temp_video)
			if os.path.isfile(temp_audio):
				self.log.debug("Cleaning up: deleting %s" % temp_audio)
				os.unlink(temp_audio)

		# Check for audiodump.aac and audiodump.wav
		other_files = ('audiodump.aac', 'audiodump.wav', 'output.mp4')
		for name in other_files:
			if os.path.isfile(os.path.join(scratch_dir, name)):
				self.log.debug("Cleaning up: deleting %s" % os.path.join(scratch_dir, name))
				os.unlink(os.path.join(scratch_dir, name))


