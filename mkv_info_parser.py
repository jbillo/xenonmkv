class MKVInfoParser:
	log = None

	def __init__(self, log):
		self.log = log

	def _parse_to_newline(self, output, detect, what="value"):
		if detect in output:
			output = output[output.index(detect) + len(detect):]
			if "\n" in output:
				output = output[0:output.index("\n")]
			self.log.debug("Detected %s '%s' from mkvinfo output" % (what, output))
			return output
		else:
			raise Exception("Could not parse %s from mkvinfo" % what)
		
	def parse_track_number(self, output):
		return int(self._parse_to_newline(output, "|  + Track number: ", "track number"))

	def parse_track_type(self, output):
		return self._parse_to_newline(output, "|  + Track type: ", "track type")

	def parse_track_is_default(self, output):
		try:
			result = self._parse_to_newline(output, "|  + Default flag: ", "default flag")		
			return "1" in result
		except:
			# Could not determine whether track was default, so set it to default
			# Usually this means that there is only one video and one audio track
			return True
