class UnsupportedCodecError(Exception):
	pass

class MKVTrack():
	number = 0
	mkvtoolnix_id = 0
	uid = track_type = language = codec_id = ""
	default = True
	log = None

	codec_table = {}
	codec_table["A_AC3"] = ".ac3"
	codec_table["A_EAC3"] = ".eac3" # mplayer should play this with the correct codecs
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
	
	def __init__(self, log):
		self.log = log

	def get_filename_extension(self):
		if self.codec_id in self.codec_table:
			return self.codec_table[self.codec_id]
			
		# Catch particular conditions where people will try and convert odd files
		if self.codec_id in ('DIV3', 'WVC1'):
			# MP4Box will not touch this content
			raise UnsupportedCodecError(self.codec_id + " codec in selected video track is not supported in an MP4 container")
					
		# Otherwise, set defaults
		if self.codec_id.startswith("A_"):
			self.log.warning("Returning default .aac extension for audio codec %s" % self.codec_id)
			return ".aac"

		if self.codec_id.startswith("V_"):
			self.log.warning("Returning default .avi extension for video codec %s" % self.codec_id)
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
	

