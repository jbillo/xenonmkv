class UnsupportedCodecError(Exception):
    pass


class MKVTrack():
    number = 0
    mkvtoolnix_id = 0
    uid = track_type = language = codec_id = ""
    default = True
    log = None

    codec_table = {
        "A_AC3":            ".ac3",
        "A_EAC3":           ".eac3",  # mplayer should play EAC3 correctly
        "A_AAC":            ".aac",
        "A_AAC/MPEG2/LC":   ".aac",   # low complexity AAC
        "A_DTS":            ".dts",
        "A_MP3":            ".mp3",
        "A_MPEG/L3":        ".mp3",
        "A_MS/ACM":         ".mp3",   # MS/ACM: anime tends to have this identifier
        "A_VORBIS":         ".ogg",
        "V_MS/VFW/FOURCC":  ".avi",   # Video for Windows / AVI container
        "V_MPEG4/ISO/AVC":  ".h264",
        "V_MPEG2":          ".mpg",
    }

    blacklisted_codecs = (
        "DIV3",
        "WVC1",
        "V_MS/VFW/FOURCC / WVC1",
    )

    def __init__(self, log):
        self.log = log

    def get_filename_extension(self):
        if self.codec_id in self.codec_table:
            return self.codec_table[self.codec_id]

        # Catch conditions where people will try and convert odd files
        if self.codec_id in self.blacklisted_codecs:
            # MP4Box will not touch this content
            raise UnsupportedCodecError("{0} codec in selected video track "
                                        "is not supported in an MP4 container".format(
                                        self.codec_id))

        # Otherwise, set defaults
        if self.codec_id.startswith("A_"):
            self.log.warning("Returning default .aac extension for audio "
                             "codec {0}".format(self.codec_id))
            return ".aac"

        if self.codec_id.startswith("V_"):
            self.log.warning("Returning default .avi extension for video "
                             "codec {0}".format(self.codec_id))
            return ".avi"

        raise Exception("Could not detect appropriate extension "
                        "for codec {0}".format(self.codec_id))

    def get_possible_extensions(self):
        extensions = set()
        for key in self.codec_table:
            extensions.add(self.codec_table[key])

        return extensions


class VideoTrack(MKVTrack):
    height = width = reference_frames = 0
    frame_rate = display_ar = 0.0
    pixel_ar = ""


class AudioTrack(MKVTrack):
    length = channels = 0
    needs_recode = True
