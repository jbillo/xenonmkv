# System Requirements

XenonMKV was built and tested on a standard Ubuntu 12.04 LTS installation, but most of the utilities and requirements here are available for most popular *nix distributions.

## Packages to Install

* mediainfo (sudo apt-get install mediainfo, http://mediainfo.sourceforge.net/en/Download/Ubuntu)
* mkvtoolnix (sudo apt-get install mkvtoolnix, http://www.bunkus.org/videotools/mkvtoolnix/downloads.html)
* mplayer (sudo apt-get install mplayer)
* MP4Box (sudo apt-get install gpac)

## Other Dependencies

* Nero AAC Codec (http://www.nero.com/eng/downloads-nerodigital-nero-aac-codec.php)

	This can be directly obtained at http://ftp6.nero.com/tools/NeroAACCodec-1.5.1.zip. For now, the contents of the "linux" directory in the ZIP file should be placed in /usr/local/bin or another path-accessible location, and made executable. I plan to add a feature to directly allow the AAC codec to be downloaded on first run.

# Suggested Applications

* vlc

	VLC is highly useful for investigating video files in a GUI. You can see the expected number of audio and video tracks.
