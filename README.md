# About XenonMKV
XenonMKV is a video format conversion tool that takes MKV files and outputs them as MP4 files. It does not re-encode video, and only decodes and encodes audio as necessary.

You'll find this tool useful to converting videos for devices that support H.264 video and AAC audio, but do not understand the MKV container. Originally, XenonMKV was meant for Xbox 360 consoles, but I'm finding now that this tool is much more useful for my <a href="http://blackberry.com/playbook">BlackBerry PlayBook</a>.

# System Requirements
XenonMKV was built and tested on a standard Ubuntu 12.04 LTS installation (x86_64), but most of the utilities and requirements here are available for most popular *nix distributions. You will need at least Python 2.7 for the argparse library, and ideally Python 2.7.3 or later in the 2.x series.

## Packages to Install
You will need some supporting packages. Instructions below work on Ubuntu 12.04.

### Install All Dependencies
	sudo apt-get install mediainfo mkvtoolnix mplayer faac gpac

### Individual Package Requirements
*	mediainfo (<http://mediainfo.sourceforge.net/en/Download/Ubuntu>)

		sudo apt-get install mediainfo	
	Alternatively, add the official mediainfo PPA and install the package, which the developer suggests might be a good idea:
	
		sudo add-apt-repository ppa:shiki/mediainfo
		sudo apt-get update
		sudo apt-get install mediainfo
*	mkvtoolnix (<http://www.bunkus.org/videotools/mkvtoolnix/downloads.html>)

		sudo apt-get install mkvtoolnix
*	mplayer (<http://www.mplayerhq.hu/design7/news.html>)

		sudo apt-get install mplayer
*	faac (<http://www.audiocoding.com/downloads.html>)

		sudo apt-get install faac
* 	MP4Box (<https://sourceforge.net/projects/gpac/>)

		sudo apt-get install gpac
		
	To compile your own gpac version, follow these instructions: <http://gpac.wp.mines-telecom.fr/2011/04/20/compiling-gpac-on-ubuntu/>
	Then, run the following commands for great success (the SpiderMonkey JS library appears to be broken currently):
	
		./configure --enable-debug --use-js=no
		make
		
	You probably still won't be able to run it, though, unless you run `sudo make install`. I prefer not to overwrite system libraries, so I copy the application directly into /opt/gpac/0.5.0:
	
		sudo mkdir -p /opt/gpac/0.5.0
		sudo cp -R bin/gcc/* /opt/gpac/0.5.0
		
	Then to run:
	
		LD_LIBRARY_PATH="$LD_LIBRARY_PATH;/opt/gpac/0.5.0" /opt/gpac/0.5.0/MP4Box

## Ubuntu 10.04
As I still have a few systems around running Ubuntu 10.04, here are the changes required to make XenonMKV functional:

*	Install Python 2.7, either from source or add the appropriate PPA:

		sudo add-apt-repository ppa:fkrull/deadsnakes
		sudo apt-get update
		sudo apt-get install python2.7	
*	Install mediainfo by adding the PPA referenced above as it is not in the 10.04 package repository.
*	Run the application directly referencing Python 2.7:

		/usr/bin/python2.7 /path/to/xenonmkv.py [arguments]

# Suggested Applications
*	vlc

	VLC is highly useful for investigating video files in a GUI. You can see the expected number of audio and video tracks and generally confirm that your video output works as expected.
	
		sudo apt-get install vlc

# Usage
Basic usage with default settings:

	xenonmkv.py /path/to/file.mkv
	
To ensure your Xbox 360 will play the resulting file, at a possible expense of audio quality:

	xenonmkv.py /path/to/file.mkv --profile xbox360

To see all command line arguments:

	xenonmkv.py --help
	
For a quiet run (batch processing or in a cronjob):
	
	xenonmkv.py /path/to/file.mkv -q

# Suggestions/Caveats
*	If your files to convert aren't too large, distributions that mount `/tmp` as tmpfs (planned for Fedora 18, Ubuntu 12.10, Debian Wheezy) can show a significant speedup if you use `--scratch-dir /tmp`. Right now for future proofing, the scratch directory is set to `/var/tmp`.
*	Use `-vv` to find display debug information and output exactly what's going on during the processing stages.
*	Native multiple file support will be coming (eg: convert an entire directory of MKVs) but you can do something like this in the meantime to queue up a list:

		cd ~/mymkvdir
		for i in `ls *.mkv`; do /path/to/xenonmkv.py $i --destination ~/mymp4dir; done

*	Performance on an Intel Core i5-2500K CPU at 3.3GHz, with a 1TB Western Digital Black SATA hard drive:
	A 442MB source MKV file with h.264 video and 6-channel AC3 audio is converted into a PlayBook-compatible MP4 (same video, 2-channel AAC audio, quality q=150) in 40.6 seconds. This does not have any enhancements such as a tmpfs mount.
	You could probably get much better performance with a solid state drive, and obviously processor speed will have an impact here.

# Audio Downmixing/Re-Encoding
By default, XenonMKV tries not to have to resample, downmix or re-encode any part of the content provided. However, chances are your source files will contain AC3, DTS or MP3 audio that needs to be re-encoded. In this case, the original source audio will always be downmixed to a two channel AAC file before it is repackaged. 

If the audio track in your MKV file is already AAC, the next thing to consider is your playback device. The Xbox 360 will not play audio in an MP4 container unless it is 2-channel stereo, which is a highly stupid limitation. Other devices, like the PlayBook, will happily parse up to 5.1 channel audio. By using either the `--channels` or `--profile` settings, you can tell XenonMKV how many channels of audio are acceptable from an AAC source before it will aggressively re-encode and downmix to 2-channel stereo. 

In short, if you plan to play MP4s on your Xbox 360, definitely use the `--profile xbox360` setting to make sure that no more than two channels make it into the output file. If your device is more reasonable, the default settings should be fine. More profiles will be added as users confirm their own device capabilities.

# Known Issues
## MP4Box crash with backtrace
Certain video files, when MP4Box loads them to rejoin into an MP4 container, will throw a glibc error beginning with:
	
	*** glibc detected *** MP4Box: free(): invalid next size (fast): 0x0000000000cc8400 ***
	======= Backtrace: =========
	/lib/x86_64-linux-gnu/libc.so.6(+0x7e626)[0x7f0b09a77626]
	/usr/lib/nvidia-current/tls/libnvidia-tls.so.295.40(+0x1c01)[0x7f0b084c7c01]
			
This occurs with both nVidia proprietary and Nouveau open source drivers. The message above is displayed when using the "current" or "current-updates" versions (295.40, 295.49). When using the 173, 173-updates (173.14.35) or Nouveau open-source driver, the free() error is the same, but the backtrace is different:

	*** glibc detected *** MP4Box: free(): invalid next size (fast): 0x00000000022d2420 ***
	======= Backtrace: =========
	/lib/x86_64-linux-gnu/libc.so.6(+0x7e626)[0x7f5e9820a626]
	/usr/lib/x86_64-linux-gnu/libgpac.so.1(minf_del+0x3f)[0x7f5e9867c69f]
	/usr/lib/x86_64-linux-gnu/libgpac.so.1(mdia_del+0x25)[0x7f5e9867c395]
	/usr/lib/x86_64-linux-gnu/libgpac.so.1(trak_del+0x33)[0x7f5e98680c03]
	/usr/lib/x86_64-linux-gnu/libgpac.so.1(gf_isom_box_array_del+0x37)[0x7f5e98692c87]
	/usr/lib/x86_64-linux-gnu/libgpac.so.1(moov_del+0x58)[0x7f5e9867c9c8]
	/usr/lib/x86_64-linux-gnu/libgpac.so.1(gf_isom_box_array_del+0x37)[0x7f5e98692c87]
	/usr/lib/x86_64-linux-gnu/libgpac.so.1(gf_isom_delete_movie+0x3a)[0x7f5e9869ad5a]
	/usr/lib/x86_64-linux-gnu/libgpac.so.1(gf_isom_close+0x39)[0x7f5e9869c779]
	MP4Box[0x40d30c]
	/lib/x86_64-linux-gnu/libc.so.6(__libc_start_main+0xed)[0x7f5e981ad76d]
	MP4Box[0x4085c1]

Sometimes this is an intermittent error, so MP4Box has a 3-retry mechanism that starts with the temporary files. At this time I'm not sure whether it's system memory or an issue with MP4Box/gpac. There were similar crashing issues in the Windows version, which is why multiple versions of MP4Box were bundled and used for fallback.
	
If you do see this issue in your own testing, please report it and include a link to the file that causes the problem if possible. You may also be able to get the file to convert by using different command line options, such as `--resume-previous --preserve-temp-files` or including or excluding `-vv`.

Relevant system information:

	$ uname -a
	Linux ubuntu 3.2.0-29-generic #46-Ubuntu SMP Fri Jul 27 17:03:23 UTC 2012 x86_64 x86_64 x86_64 GNU/Linux
	
My MP4Box version is the default from the 'gpac' Ubuntu 12.04 package, which is `0.4.5+svn3462~dfsg0-1`:

	$ MP4Box -version
	MP4Box - GPAC version 0.4.6-DEV-rev
	GPAC Copyright: (c) Jean Le Feuvre 2000-2005
	(c) ENST 2005-200X
	GPAC Configuration: --build=x86_64-linux-gnu --prefix=/usr --includedir=${prefix}/include --mandir=${prefix}/share/man --infodir=${prefix}/share/info --sysconfdir=/etc --localstatedir=/var --libdir=${prefix}/lib/x86_64-linux-gnu --libexecdir=${prefix}/lib/x86_64-linux-gnu --disable-maintainer-mode --disable-dependency-tracking --prefix=/usr --mandir=${prefix}/share/man --libdir=lib/x86_64-linux-gnu --extra-cflags='-Wall -fPIC -DPIC -I/usr/include/mozjs -DXP_UNIX' --enable-joystick --enable-debug --disable-ssl
	Features: GPAC_HAS_JPEG GPAC_HAS_PNG 

A fresh compile of the 0.5.0 version available at <https://sourceforge.net/projects/gpac/> also appears to trigger this bug:

	MP4Box - GPAC version 0.5.0-rev4065
	GPAC Copyright: (c) Jean Le Feuvre 2000-2005
		(c) ENST 2005-200X
	GPAC Configuration:  --enable-debug
	Features: GPAC_DISABLE_3D 

The version included with Ubuntu 10.04 (`0.4.5-0.3ubuntu6`) does not appear to have issues.

# TODO
*	Add a ConfigParser instance, allowing default options to be read and stored in persistent files: <http://docs.python.org/library/configparser.html>
*	~~Add support for custom tool locations (eg: use newer MP4Box build in a custom directory, rather than the version defined in PATH)~~
*	~~Add support for picking a specific track from a multiple-track MKV file, rather than forcing defaults~~
*	Add way for user to pick tracks at runtime (get `--select-tracks` functional; this will not be a default option


