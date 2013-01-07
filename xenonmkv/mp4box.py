import os
from xenonmkv.process_handler import ProcessHandler


class MP4Box():
    video_path = audio_path = video_fps = video_pixel_ar = ""

    args = log = None

    def __init__(self, video_path, audio_path, video_fps,
                 video_pixel_ar, args, log):
        self.video_path = video_path
        self.audio_path = audio_path
        self.video_fps = str(video_fps)
        self.video_pixel_ar = video_pixel_ar
        self.args = args
        self.log = log

    def package(self):
        prev_dir = os.getcwd()
        os.chdir(self.args.scratch_dir)

        # Make sure there is no 'output.mp4' in the scratch directory
        # MP4Box has a tendency to add tracks
        output_file = os.path.join(os.getcwd(), "output.mp4")
        if os.path.isfile(output_file):
            os.unlink(output_file)

        run_attempts = 1

        while run_attempts <= self.args.mp4box_retries:
            cmd = [self.args.tool_paths["mp4box"], "output.mp4",
                   # Always create new file with mp4box/GPAC
                   # https://github.com/jbillo/xenonmkv/issues/2
                   "-new",
                   "-add", self.video_path, "-fps", self.video_fps,
                   "-par", "1=" + self.video_pixel_ar,
                   "-add", self.audio_path, "-tmp", self.args.scratch_dir,
                   "-itags", "name=" + self.args.name]

            ph = ProcessHandler(self.args, self.log)
            process = ph.start_output(cmd)

            if process != 0:
                # Destroy temporary file
                # so it does not have multiple tracks imported
                os.unlink(output_file)
                self.log.warning("An error occurred while creating "
                                 "an MP4 file with MP4Box; {0} retries left".format(
                                 self.args.mp4box_retries - run_attempts))
                run_attempts += 1
                # Continue retrying to create the file
            else:
                # File was created successfully; exit retry loop
                break

        if run_attempts > self.args.mp4box_retries:
            # Delete the temporary file so that nobody gets tempted to use it
            if os.path.isfile(output_file):
                try:
                    os.unlink(output_file)
                except:
                    # Don't really care, just as long as the file is gone.
                    pass

            raise Exception("MP4Box could not create file after {0} retries; "
                            "giving up.".format(self.args.mp4box_retries))

        self.log.debug("MP4Box process complete")

        # When complete, change back to original directory
        os.chdir(prev_dir)
