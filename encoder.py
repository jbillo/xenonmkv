import os

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
            self.log.debug("audiodump.aac already exists in scratch "
                "directory; cancelling encode")
            return True

        cmd = [self.args.tool_paths["faac"], "-q",
            str(self.args.faac_quality), self.file_path]
        ph = ProcessHandler(self.args, self.log)

        ph.start_output(cmd)

        os.chdir(prev_dir)
        self.log.debug("FAAC encoding to AAC audio file complete")
