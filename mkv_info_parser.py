import os


class MKVInfoParser:
    log = None

    def __init__(self, log):
        self.log = log

    def _parse_to_newline(self, output, detect, what="value"):
        if detect in output:
            output = output[output.index(detect) + len(detect):]
            if os.linesep in output:
                output = output[0:output.index(os.linesep)]

            # Remove trailing characters from output
            # (seems to be an issue on Windows, likely with CR characters)
            output = output.strip()
            self.log.debug("Detected {0} '{1}' from mkvinfo output".format(what, output))
            return output
        else:
            raise Exception("Could not parse {0} from mkvinfo".format(what))

    def parse_track_number(self, output):
        result = self._parse_to_newline(output,
                                        "|  + Track number: ", "track number")
        # mkvinfo 5.8.0 on Windows apparently adds more content to the string
        # use ID number for mkvmerge/mkvextract

        # We need to return a tuple: track_id, mkvtoolnix_track_id
        if "(" in result:
            track_id = int(result[0:result.index(" (")].strip())
        else:
            # Older versions of mkvinfo don't need a specific ID
            return int(result), int(result)

        detect_mkvextract = "mkvextract: "
        if detect_mkvextract in result:
            result = result[result.index(detect_mkvextract) +
                            len(detect_mkvextract):]
        if ")" in result:
            result = result[0:result.index(")")]

        return track_id, int(result)

    def parse_track_type(self, output):
        return self._parse_to_newline(output, "|  + Track type: ", "track type")

    def parse_track_is_default(self, output):
        try:
            result = self._parse_to_newline(output,
                                            "|  + Default flag: ", "default flag")
            return "1" in result
        except:
            # Could not determine whether track was default,
            # so set it to default
            # Usually this means that there is only
            # one video and one audio track
            return True
