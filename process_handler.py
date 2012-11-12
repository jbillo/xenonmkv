import os
import subprocess


class ProcessHandler:
    args = log = None

    def __init__(self, args, log):
        self.args = args
        self.log = log

    def start_process(self, cmd):
        env = dict(os.environ)
        if not "LD_LIBRARY_PATH" in env:
            env["LD_LIBRARY_PATH"] = ""
        env["LD_LIBRARY_PATH"] += os.pathsep + os.pathsep.join(
            self.args.library_paths)
        self.log.debug("Starting process with arguments: %s " %
            ' '.join(cmd))
        return subprocess.Popen(cmd, stdout=subprocess.PIPE,
            stderr=subprocess.PIPE, env=env)
