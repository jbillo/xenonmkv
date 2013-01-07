import os
import sys
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
        self.log.debug("Starting process with arguments: {0} ".format(' '.join(cmd)))
        return subprocess.Popen(cmd, stdout=subprocess.PIPE,
                                stderr=subprocess.PIPE, env=env)

    def start_output(self, cmd):
        env = dict(os.environ)
        if not "LD_LIBRARY_PATH" in env:
            env["LD_LIBRARY_PATH"] = ""
        env["LD_LIBRARY_PATH"] += os.pathsep + os.pathsep.join(
            self.args.library_paths)
        self.log.debug("Starting process and waiting with arguments: {0}".format(
                       ' '.join(cmd)))

        process_out = sys.stdout
        process_err = sys.stderr
        fnull = None

        # Suppress tool output in quiet mode
        if self.args.quiet:
            fnull = open(os.devnull, "w")
            process_out = fnull
            process_err = fnull

        try:
            p = subprocess.call(cmd, env=env, stdout=process_out, stderr=process_err)
        except KeyboardInterrupt:
            print
            self.log.critical("Tool execution cancelled; exiting")
            if fnull:
                fnull.close()
            sys.exit(1)

        if fnull:
            fnull.close()

        return p
