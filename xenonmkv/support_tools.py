import sys
import platform
import os
import webbrowser

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
			"direct_download" : "http://www.bunkus.org/videotools/mkvtoolnix/win32/mkvtoolnix-unicode-5.8.0-setup.exe",
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
		return getattr(self, "find_" + app)()
	
		print "On %s running %s" % (self.ostype, app)
		pass
		
	def offer_windows_install(self, app):
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
				pass
			elif response in ('w', 'website'):
				webbrowser.open(properties["website"])
				return False
			else:
				# Return false as the user didn't choose to install the support tool.
				return False
		except KeyboardInterrupt:
			# Print \n before critical log
			print
			self.log.critical("Installation prompt cancelled; exiting")
			sys.exit(1)
		
	def find_mkvinfo(self):
		if self.ostype == "windows":
			# MKVToolnix on Windows adds itself to the path
			return self.offer_windows_install("mkvinfo")
		pass
	