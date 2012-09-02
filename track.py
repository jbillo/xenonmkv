class MKVTrack():
	number = 0
	uid = track_type = language = codec_id = ""
	default = True

	codec_table = {}
	codec_table["A_AC3"] = ".ac3"
	codec_table["A_AAC"] = ".aac"
	codec_table["A_AAC/MPEG2/LC"] = ".aac"
	codec_table["A_DTS"] = ".dts"
	codec_table["A_MP3"] = ".mp3"
	codec_table["A_MPEG/L3"] = ".mp3"
	codec_table["A_MS/ACM"] = ".mp3" # because people like to run anime through this thing
	codec_table["A_VORBIS"] = ".ogg"
	codec_table["V_MS/VFW/FOURCC"] = ".avi"
	codec_table["V_MPEG4/ISO/AVC"] = ".h264"
	codec_table["V_MPEG2"] = ".mpg"

	def get_filename_extension(self):
		if self.codec_id in self.codec_table:
			return self.codec_table[self.codec_id]
			
		# Catch particular conditions where people will try and convert odd files
		if self.codec_id == "DIV3":
			# MP4Box will not touch DIV3 content
			log.critical("The video track selected uses the DIV3 codec, which is not supported in a MP4 container.")
			raise UnsupportedCodecError("DIV3 codec used in selected video track is not supported")

		# Otherwise, set defaults
		if self.codec_id.startswith("A_"):
			log.warning("Returning default .aac extension for audio codec %s" % self.codec_id)
			return ".aac"

		if self.codec_id.startswith("V_"):
			log.warning("Returning default .avi extension for video codec %s" % self.codec_id)
			return ".avi"

		raise Exception("Could not detect appropriate extension for codec %s" % self.codec_id)

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
