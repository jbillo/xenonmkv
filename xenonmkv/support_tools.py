import sys
import platform
import os
import webbrowser
import urllib2
import subprocess

class SupportTools():
	ostype = None
	distro = None
	bits = None
	log = None
	app_path = os.path.dirname(os.path.realpath(sys.argv[0]))

	tools = {
		"mkvinfo" : {
			"friendly_name" : "MKVToolnix",
			"website" : "http://www.bunkus.org/videotools/mkvtoolnix/downloads.html#windows",
			"windows_direct_download" : "http://www.bunkus.org/videotools/mkvtoolnix/win32/mkvtoolnix-unicode-5.8.0-setup.exe",
			"mac_direct_download" : "http://www.bunkus.org/videotools/mkvtoolnix/macos/Mkvtoolnix-5.7.0_2012-07-11-cd22.dmg",
		},
		"mediainfo" : {
		    "friendly_name" : "MediaInfo",
		    "website" : "http://mediainfo.sourceforge.net/en/Download",
		    "mac_direct_download" : "http://mediaarea.net/download/binary/mediainfo/0.7.60/MediaInfo_CLI_0.7.60_Mac_i386%2Bx86_64.dmg",
		    "windows_direct_download" : "http://mediaarea.net/download/binary/mediainfo/0.7.60/MediaInfo_CLI_0.7.60_Windows_i386.zip",
		    "windows_direct_download_64" : "http://mediaarea.net/download/binary/mediainfo/0.7.60/MediaInfo_CLI_0.7.60_Windows_x64.zip"
		}
	}

	def __init__(self, log):
		self.log = log

		# Set internal variables based on OS
		if sys.platform.startswith("linux"):
			self.ostype = "linux"
			self.distro = platform.linux_distribution()
		elif sys.platform.startswith("win32"):
			self.ostype = "windows"
		elif sys.platform.startswith("darwin"):
			self.ostype = "mac"
		else:
			# Won't be able to support a lot here
			self.ostype = "unknown"

	def find_tool(self, app):
		try:
			return self.offer_install(app) # getattr(self, "find_" + app)()
		except AttributeError:
			# Haven't written a check for this app yet
			return None

	def download_file(self, url, destination_dir=None):
	    if not destination_dir or not os.path.isdir(destination_dir):
	        destination_dir = os.path.join(self.app_path, "tools")

		u = urllib2.urlopen(url)
		file_name = url.split("/")[-1]
		destination_path = os.path.join(destination_dir, file_name)

		# Debugging: if the file exists already, return true and don't download again
		# This should be removed for non-debug copies
		if os.path.isfile(destination_path):
		    return destination_path

		f = open(destination_path, 'wb')
		meta = u.info()
		file_size = int(meta.getheaders("Content-Length")[0])

		file_size_dl = 0
		block_sz = 8192
		while True:
			buffer = u.read(block_sz)
			if not buffer:
				break

			file_size_dl += len(buffer)
			f.write(buffer)
			status = r"%10d	 [%3.2f%%]" % (file_size_dl, file_size_dl * 100. / file_size)
			status = status + chr(8)*(len(status)+1)
			print status,

		f.close()
		return destination_path

	def install_file(self, app, path):
	    # Check OS
	    file_extension = os.path.splitext(path)[1].lower()
	    if self.ostype == "mac":
	        print "You may need to provide your account password to install this application."
	        # Check file extension
	        if file_extension == ".dmg":
	            # Mount DMG with hdiutil
	            subprocess.call(["hdiutil", "attach", path, "-mountpoint", "/Volumes/" + app])
	            if app == "mkvinfo":
	                # Copy Mkvtoolnix.app to /Applications
	                subprocess.call(["sudo cp -R /Volumes/" + app + "/Mkvtoolnix.app /Applications/"], shell=True)
	            elif app == "mediainfo":
	                # Run package installer for MediaInfo CLI
	                #subprocess.call
	                pass

	            # All done - unmount the volume
	            subprocess.call(["diskutil", "unmount", "force", "/Volumes/" + app])
	            return True

	def offer_install(self, app):
		properties = self.tools[app]
		print """It appears that your system is missing %s, which is required for XenonMKV to work. You can either download it from its website at %s, or a copy can be installed automatically.
		""" % (
			properties["friendly_name"],
			properties["website"],
		)

		print """Options:

[Y] Yes: Download and install %s automatically (default)
[W] Website: Open the website for %s in the default browser.
	Once you have downloaded and installed %s, run XenonMKV again to continue.
[N] No: Do not install %s. XenonMKV will exit.
		""" % (properties["friendly_name"], properties["friendly_name"], properties["friendly_name"], properties["friendly_name"])

		try:
			response = raw_input("Install %s automatically (default=Yes) [Y/w/n]? " % properties["friendly_name"])
			response = response.strip().lower()
			if response in ('', 'y', 'yes'):
				# TODO: Download app and run installer/decompress
				url = properties[self.ostype + "_direct_download"]
				print "Downloading installation package from %s" % url
				file_path = self.download_file(url)

				if not file_path:
				    self.log.error("Could not download %s package automatically" % properties["friendly_name"])
				    return None

				# Install application according to OS mechanism
				return self.install_file(app, file_path)
			elif response in ('w', 'website'):
				webbrowser.open(properties["website"])
				return False
			else:
				# Return false as the user didn't choose to install the support tool.
				return False
		except KeyboardInterrupt:
			# Print \n before critical log line
			print
			self.log.critical("Installation prompt cancelled; exiting")
			sys.exit(1)

	def find_mkvinfo(self):
		if self.ostype == "windows":
			# MKVToolnix on Windows adds itself to the path
			return self.offer_install("mkvinfo")
		elif self.ostype == "mac":
			return self.offer_install("mkvinfo")

		return None