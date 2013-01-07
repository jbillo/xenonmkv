import os

from xenonmkv.process_handler import ProcessHandler


class AACEncoder():
    file_path = ""
    log = args = None
    encoder = ""

    def __init__(self, file_path, log, args):
        self.file_path = file_path
        self.log = log
        self.args = args

    def detect_encoder(self):
        # New encoders can be added here when necessary
        self.encoder = "faac"

    def encode(self):
        if not self.encoder:
            self.detect_encoder()

        prev_dir = os.getcwd()
        os.chdir(self.args.scratch_dir)

        # Based on the encoder, perform the appropriate operation
        # so self.encoder = faac will call self.encode_faac()
        getattr(self, "encode_%s" % self.encoder)()

        os.chdir(prev_dir)

        self.log.debug("Encoding to AAC audio file complete")

    def encode_faac(self):
        # Start encoding
        self.log.debug("Using FAAC to encode AAC audio file")

        if self.args.resume_previous and os.path.isfile("audiodump.aac"):
            self.log.debug("audiodump.aac already exists in scratch "
                           "directory; cancelling encode")
            return True

        cmd = [self.args.tool_paths["faac"], "-q",
               str(self.args.faac_quality), self.file_path]
        ph = ProcessHandler(self.args, self.log)

        return ph.start_output(cmd)
