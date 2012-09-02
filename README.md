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
	http://mediainfo.sourceforge.net/en/Download/Ubuntu

		sudo apt-get install mediainfo
	
* mkvtoolnix
	http://www.bunkus.org/videotools/mkvtoolnix/downloads.html

		sudo apt-get install mkvtoolnix

* mplayer
	http://www.mplayerhq.hu/design7/news.html

		sudo apt-get install mplayer

* faac 
	http://www.audiocoding.com/downloads.html

		sudo apt-get install faac

* MP4Box 
	https://sourceforge.net/projects/gpac/

		sudo apt-get install gpac


# Suggested Applications

* vlc

	VLC is highly useful for investigating video files in a GUI. You can see the expected number of audio and video tracks and generally confirm that your video output works as expected.
		sudo apt-get install vlc

# Usage

Basic usage with default settings:

	xenonmkv.py /path/to/file.mkv

To see all command line arguments:

	xenonmkv.py --help
	
For a quiet run (batch processing or in a cronjob):
	
	xenonmkv.py /path/to/file.mkv -q

# Suggestions/Caveats

* If your files to convert aren't too large, distributions that mount /tmp as tmpfs (planned for Fedora 18, Ubuntu 12.10, Debian Wheezy) can show a significant speedup if you use "--scratch-dir /tmp". Right now for future proofing, the scratch directory is set to /var/tmp.
* Use -vv to find display debug information and output exactly what's going on during the processing stages.
* Native multiple file support will be coming (eg: convert an entire directory of MKVs) but you can do something like this in the meantime to queue up a list:

		cd ~/mymkvdir
		for i in `ls *.mkv`; do /path/to/xenonmkv.py $i --destination ~/mymp4dir; done

* Performance on an Intel Core i5-2500K CPU at 3.3GHz, with a 1TB Western Digital Black SATA hard drive:

	A 442MB source MKV file with h.264 video and 6-channel AC3 audio is converted into a PlayBook-compatible MP4 (same video, 2-channel AAC audio, quality q=150) in 40.6 seconds. This does not have any enhancements such as a tmpfs mount.

	You could probably get much better performance with a solid state drive, and obviously processor speed will have an impact here.

# Audio Downmixing/Re-Encoding

By default, XenonMKV tries not to have to resample, downmix or re-encode any part of the content provided. However, chances are your source files will contain AC3, DTS or MP3 audio that needs to be re-encoded. In this case, the original source audio will always be downmixed to a two channel AAC file before it is repackaged. 

If the audio track in your MKV file is already AAC, the next thing to consider is your playback device. The Xbox 360 will not play audio in an MP4 container unless it is 2-channel stereo, which is a highly stupid limitation. Other devices, like the PlayBook, will happily parse up to 5.1 channel audio. By using either the "--channels" or "--profile" settings, you can tell XenonMKV how many channels of audio are acceptable from an AAC source before it will aggressively re-encode and downmix to 2-channel stereo. 

In short, if you plan to play MP4s on your Xbox 360, definitely use the "--profile xbox360" setting to make sure that no more than two channels make it into the output file. 
