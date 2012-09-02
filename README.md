# About XenonMKV

XenonMKV is a video format conversion tool that takes MKV files and outputs them as MP4 files. It does not re-encode video, and only decodes and encodes audio as necessary.

You'll find this tool useful to converting videos for devices that support H.264 video and AAC audio, but do not understand the MKV container. Originally, XenonMKV was meant for Xbox 360 consoles, but I'm finding now that this tool is much more useful for my <a href="http://blackberry.com/playbook">BlackBerry PlayBook</a>.

# System Requirements

XenonMKV was built and tested on a standard Ubuntu 12.04 LTS installation, but most of the utilities and requirements here are available for most popular *nix distributions.

## Packages to Install

You will need some supporting packages. Instructions below work on Ubuntu 12.04.

### Install All Dependencies
		sudo apt-get install mediainfo mkvtoolnix mplayer faac gpac

### Individual Package Requirements

* mediainfo 
		sudo apt-get install mediainfo
	http://mediainfo.sourceforge.net/en/Download/Ubuntu
* mkvtoolnix
		sudo apt-get install mkvtoolnix
	http://www.bunkus.org/videotools/mkvtoolnix/downloads.html)
* mplayer
		sudo apt-get install mplayer
	http://www.mplayerhq.hu/design7/news.html
* faac 
		sudo apt-get install faac
	http://www.audiocoding.com/downloads.html
* MP4Box 
		sudo apt-get install gpac
	https://sourceforge.net/projects/gpac/

# Suggested Applications

* vlc

	VLC is highly useful for investigating video files in a GUI. You can see the expected number of audio and video tracks and generally confirm that your video output works as expected.
		sudo apt-get install vlc

# Usage

Run xenonmkv.py --help to see all command line arguments. 

# Suggestions/Caveats

* If your files to convert aren't too large, distributions that mount /tmp as tmpfs (planned for Fedora 18, Ubuntu 12.10, Debian Wheezy) can show a significant speedup if you use "--scratch-dir /tmp". Right now for future proofing, the scratch directory is set to /var/tmp.
* Use -vv to find display debug information and output exactly what's going on during the processing stages.
* Native multiple file support will be coming (eg: convert an entire directory of MKVs) but you can do something like this in the meantime to queue up a list:

		cd ~/mymkvdir
		for i in `ls *.mkv`; do /path/to/xenonmkv.py $i --destination ~/mymp4dir; done

* Performance on an Intel Core i5-2500K CPU at 3.3GHz, with a 1TB Western Digital Black SATA hard drive:

	A 442MB source MKV file with h.264 video and 6-channel AC3 audio is converted into a PlayBook-compatible MP4 (same video, 2-channel AAC audio, quality q=150) in 40.6 seconds. This does not have any enhancements such as a tmpfs mount.

	You could probably get much better performance with a solid state drive, and obviously processor speed will have an impact here.



