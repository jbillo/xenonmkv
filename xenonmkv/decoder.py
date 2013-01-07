import os
from xenonmkv.process_handler import ProcessHandler


class AudioDecoder():
    extension = ""
    file_path = ""
    decoder = ""
    log = None

    def __init__(self, file_path, log, args):
        self.file_path = file_path
        self.extension = file_path[file_path.rindex("."):]
        self.log = log
        self.args = args

    def detect_decoder(self):
        # New decoders can be added here when necessary
        if self.extension in (".ac3", ".dts"):
            self.decoder = "mplayer"
        else:
            self.decoder = "mplayer"

    def decode(self):
        if not self.decoder:
            self.detect_decoder()

        prev_dir = os.getcwd()
        os.chdir(self.args.scratch_dir)

        # Based on the decoder, perform the appropriate operation
        # so self.decoder = mplayer will call self.decode_mplayer()
        getattr(self, "decode_%s" % self.decoder)()

        os.chdir(prev_dir)

    def decode_mplayer(self):
        self.log.debug("Starting decoding file to WAV with mplayer")
        # Check for existing audiodump.wav (already changed to temp directory)
        audiodump_file = os.path.join(os.getcwd(), "audiodump.wav")
        if os.path.isfile(audiodump_file):
            if self.args.resume_previous:
                self.log.debug("audiodump.wav already exists in scratch "
                               "directory; cancelling decode")
                return True

            self.log.debug("Deleting temporary mplayer output file {0}".format(
                           audiodump_file))
            os.unlink(audiodump_file)

        cmd = [self.args.tool_paths["mplayer"], self.file_path, "-benchmark",
               "-vc", "null", "-vo", "null", "-channels", "2", "-noautosub",
               "-ao", "pcm:fast"]
        ph = ProcessHandler(self.args, self.log)
        ph.start_output(cmd)

        self.log.debug("mplayer decoding to PCM WAV file complete")
