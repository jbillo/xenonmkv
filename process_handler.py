import os
import subprocess

class ProcessHandler:
	args = None

	def __init__(self, args):
		self.args = args

	def start_process(self, cmd):
		env = dict(os.environ)
		if not "LD_LIBRARY_PATH" in env:
			env["LD_LIBRARY_PATH"] = ""
		env["LD_LIBRARY_PATH"] += os.pathsep + os.pathsep.join(self.args.library_paths)	
		return subprocess.Popen(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE, env=env)
