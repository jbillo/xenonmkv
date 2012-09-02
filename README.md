# About XenonMKV

XenonMKV is a video format conversion tool that takes MKV files and outputs them as MP4 files. It does not re-encode video, and only decodes and encodes audio as necessary.

You'll find this tool useful to converting videos for devices that support H.264 video and AAC audio, but do not understand the MKV container. Originally, XenonMKV was meant for Xbox 360 consoles, but I'm finding now that this tool is much more useful for my <a href="http://blackberry.com/playbook">BlackBerry PlayBook</a>.

# System Requirements

XenonMKV was built and tested on a standard Ubuntu 12.04 LTS installation, but most of the utilities and requirements here are available for most popular *nix distributions.

## Packages to Install

* mediainfo (sudo apt-get install mediainfo, http://mediainfo.sourceforge.net/en/Download/Ubuntu)
* mkvtoolnix (sudo apt-get install mkvtoolnix, http://www.bunkus.org/videotools/mkvtoolnix/downloads.html)
* mplayer (sudo apt-get install mplayer)
* faac (sudo apt-get install faac)
* MP4Box (sudo apt-get install gpac)

# Suggested Applications

* vlc

	VLC is highly useful for investigating video files in a GUI. You can see the expected number of audio and video tracks.

# Usage

Run xenonmkv.py --help to see all command line arguments. 

# Suggestions/Caveats

* If your files to convert aren't too large, distributions that mount /tmp as tmpfs (planned for Fedora 18, Ubuntu 12.10, Debian Wheezy) can show a significant speedup if you use "--scratch-dir /tmp". Right now for future proofing, the scratch directory is set to /var/tmp.
