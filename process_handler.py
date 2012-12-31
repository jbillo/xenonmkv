import os
import sys
import subprocess

from Queue import Queue, Empty
from threading import Thread

ON_POSIX = 'posix' in sys.builtin_module_names

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

    def read_pipe(self, pipe, q):
        print "Reading pipe"
        q.put(pipe.read(1))
        print "Completed call to queue"

    def start_output(self, cmd):
        env = dict(os.environ)
        if not "LD_LIBRARY_PATH" in env:
            env["LD_LIBRARY_PATH"] = ""
        env["LD_LIBRARY_PATH"] += os.pathsep + os.pathsep.join(
            self.args.library_paths)
        self.log.debug("Starting process and capturing output with arguments: %s " %
            ' '.join(cmd))

        #p = subprocess.Popen(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE, bufsize=1,
        #@    close_fds=ON_POSIX, env=env)

        p = subprocess.Popen(cmd, env=env)
        while p.poll() is not None:
            pass

        # stderr_queue = Queue()
        # stdout_queue = Queue()
        # stderr_thread = Thread(target=self.read_pipe, args=(p.stderr, stderr_queue))
        # stdout_thread = Thread(target=self.read_pipe, args=(p.stdout, stdout_queue))

        # stderr_thread.start()
        # stdout_thread.start()

        # while True:
        #     try:
        #         if not self.args.quiet:
        #             sys.stderr.write(stderr_queue.get_nowait())
        #             sys.stderr.flush()
        #     except Empty:
        #         # stderr queue doesn't have any content
        #         pass

        #     try:
        #         if not self.args.quiet:
        #             sys.stdout.write(stdout_queue.get_nowait())
        #             sys.stdout.flush()
        #     except Empty:
        #         # stdout queue doesn't have any content
        #         pass

        #     # not None = process has terminated
        #     if p.poll() is not None:
        #         break

        # stderr_thread = stdout_thread = None
        return p
