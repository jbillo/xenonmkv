# About XenonMKV
XenonMKV is a video container conversion tool that takes MKV files and outputs them as MP4 files. It does not re-encode video, and only decodes and encodes audio as necessary.

You'll find this tool useful for converting videos for devices that support H.264 video and AAC audio, but do not understand the MKV container. Other uses include converting videos with AC3 or DTS audio to AAC audio (2 channel or 5.1 channels).

Originally, XenonMKV was meant for Xbox 360 consoles, but I'm finding now that this tool is much more useful for my [BlackBerry PlayBook](http://blackberry.com/playbook). You may also find it useful for Roku devices or to pre-convert videos to reduce the likelihood that Plex will need to re-encode them.

## Disclosure ##
Note: During early development of this application, I worked for [BlackBerry](http://blackberry.com). The opinions expressed here are my own and don’t necessarily represent those of my previous or current employer(s). All code is developed on my own time without use of company resources.

# System Requirements
XenonMKV was built and tested on a standard Ubuntu 12.04 LTS installation (x86_64), but most of the utilities and requirements here are possible to run on most popular \*nix distributions. I've also given the suite of tools a cursory run on Ubuntu 13.10 and they seem to work. Once 14.04 is released in final form I plan to retest.

Windows 7 (64-bit) and Mac OS X 10.8 are also supported, but may work on different versions of OS X and Windows.

You will need at least Python 2.7 for the argparse library, and ideally 
Python 2.7.3 or later in the 2.x series.

## Ubuntu 12.04
You will need some supporting packages. These will be installed automatically
if they are not found on your system, or they can be installed beforehand.

On desktop installations, please ensure that 'Update Manager' is closed before installing dependendencies, or
you will receive a nasty message in the form of:

```
E: Could not get lock /var/lib/dpkg/lock - open (11: Resource temporarily unavailable)
E: Unable to lock the administration directory (/var/lib/dpkg/), is another process using it?
```

### Install All Dependencies (at once)
    sudo apt-get install mediainfo mkvtoolnix mplayer faac gpac

At the current state of development, on Ubuntu you do not need to install
any Python packages from requirements.txt as the dependent tools are installed with 'apt'. 
If this changes in the future, requirements can be installed by running:

    sudo apt-get install python-setuptools && sudo easy_install -U pip
    pip install -r requirements.txt

### Individual Package Details
*    [mediainfo](http://mediainfo.sourceforge.net/en/Download/Ubuntu)

        sudo apt-get install mediainfo
    Alternatively, add the official mediainfo PPA and install the package,
    which the developer suggests might be a good idea:

        sudo add-apt-repository ppa:shiki/mediainfo
        sudo apt-get update
        sudo apt-get install mediainfo
*    [mkvtoolnix](http://www.bunkus.org/videotools/mkvtoolnix/downloads.html)

        sudo apt-get install mkvtoolnix
*    [mplayer](http://www.mplayerhq.hu/design7/news.html)

        sudo apt-get install mplayer
*    [faac](http://www.audiocoding.com/downloads.html)

        sudo apt-get install faac
*    [MP4Box](https://sourceforge.net/projects/gpac/)

        sudo apt-get install gpac

    To compile your own gpac version, follow [these instructions](http://gpac.wp.mines-telecom.fr/2011/04/20/compiling-gpac-on-ubuntu/).
    Then, run the following commands for success (the SpiderMonkey JS library isn't needed):

        ./configure --enable-debug --use-js=no
        make

    By default MP4Box will be installed in `/usr/local/{bin,lib,man,share}` as necessary.
    If your PATH variable includes `/usr/local/bin` first, your custom compiled version will be executed
    instead of the system version.

    To put MP4Box into a specific path on compile, use the `--prefix`
    option with `./configure` -
    [thanks Romain](https://github.com/jbillo/xenonmkv/issues/2#issuecomment-10257505):

        ./configure --enable-debug --use-js=no --prefix=/opt/gpac

    Run `sudo make install` to finalize.
    Then to run, specify LD_LIBRARY_PATH if it is not already in your LD
    configuration. On Ubuntu /usr/local/lib is already there, so just run:

        /usr/local/bin/MP4Box

    For a custom prefix:

        LD_LIBRARY_PATH="$LD_LIBRARY_PATH;/opt/gpac/lib" /opt/gpac/bin/MP4Box

## Ubuntu 10.04
Not currently still tested against, but here are the historical changes required to make XenonMKV functional:

* Install Python 2.7, either from source or add the appropriate PPA. If you need to upgrade your system from 10.04 to 12.04 later, make sure to purge this PPA first.

        sudo add-apt-repository ppa:fkrull/deadsnakes
        sudo apt-get update
        sudo apt-get install python2.7

* Install mediainfo by adding the `ppa:shiki/mediainfo` PPA as the *mediainfo* package is not in the 10.04 repository.
* Perform *install dependencies* step from the 12.04 instructions
* Run the application directly referencing Python 2.7:

        /usr/bin/python2.7 /path/to/xenonmkv.py [arguments]

## Other Linux Distributions
Install the packages mentioned above, either from source or your distribution's package manager.

## Windows
Eventually I will package the application as an .exe file for convenience with
Windows installations. XenonMKV has been tested on Windows 7 and 8.1, 64-bit versions.

As long as you have all the dependent tools installed, you should only need to
have Python installed and add the Python directory to your PATH environment
variable.

### Basic Use
* Install Python 2.7
([32-bit](http://python.org/ftp/python/2.7.3/python-2.7.3.msi),
[64-bit](http://python.org/ftp/python/2.7.3/python-2.7.3.amd64.msi))
depending on your architecture.
* Add the Python27 directory to your PATH environment variable:
    * Hit *Win* + *Break* to bring up Computer Properties
    * Click *Advanced System Settings*
    * Click *Environment Variables*
    * With PATH selected, click *Edit* and add `;C:\Python27`
      (replace C:\Python27 with your installation directory)
    * Click *OK* all the way out and restart any `cmd` instances
    * Confirm the setting was applied by entering `echo %PATH% | find "Python"`
      (you should see your PATH variable)
* Run XenonMKV from the command line:
        python xenonmkv.py [arguments]

### Development on Windows
To get things up and running for a development environment on Windows,
you can perform the following steps:

* Install [`setuptools`](http://pypi.python.org/pypi/setuptools#downloads)
for your appropriate OS. Downloading and running
<http://peak.telecommunity.com/dist/ez_setup.py> may be the best option.
* Install [MinGW](http://sourceforge.net/projects/mingw/files/) for
compiling Python packages, with the C and C++ compiler options
* Add the Python27\Scripts and MinGW directories to your PATH environment
  variable:
    * Hit *Win* + *Break* to bring up Computer Properties
    * Click *Advanced System Settings*
    * Click *Environment Variables*
    * With PATH selected, click *Edit* and add
    `;C:\Python27;C:\Python27\Scripts;C:\MinGW`
    (replace C:\Python27 with your installation directory)
    * Click *OK* all the way out and restart any `cmd` instances
    * Confirm the setting was applied by entering
      `echo %PATH% | find "Python" | find "MinGW"`
      (you should see your PATH variable)
* Create or edit the *distutils.cfg* file under `C:\Python27\Lib\distutils` and enter the following contents:

        [build]
        compiler=mingw32

* As per <http://bugs.python.org/issue12641> and
<http://stackoverflow.com/questions/6034390/compiling-with-cython-and-mingw-produces-gcc-error-unrecognized-command-line-o>,
edit `C:\Python27\Lib\distutils\cygwinccompiler.py` and remove all
`-mno-cygwin` references beginning on line 322.
The definition should look like:

```python
        self.set_executables(compiler='gcc -O -Wall',
                             compiler_so='gcc -mdll -O -Wall',
                             compiler_cxx='g++ -O -Wall',
                             linker_exe='gcc',
                             linker_so='%s %s %s'
                                        % (self.linker_dll, shared_option,
                                           entry_point))
```

* Use `easy_install` to get `pip` added to your system,
which will let you pull the necessary dependencies:

        easy_install pip

* Install all necessary dependencies:

        pip install -r requirements.txt

## OS X
XenonMKV has been tested on OS X 10.8. For best results, use the packages offered by the installer. You will need at least OS X 10.6 and a 64-bit capable machine for some of the dependent applications.

# Suggested Applications and Optional Tools
*    vlc

    VLC is highly useful for investigating video files in a GUI. You can see
    the number of audio and video tracks in an MKV, and confirm that your MP4
    output works as expected.

        sudo apt-get install vlc

*    mkvtoolnix-gui

    The GUI version of mkvtoolnix is useful for constructing smaller MKV files
    for test cases, extracting specific content or modifying tracks.

        sudo apt-get install mkvtoolnix-gui

* [Nero AAC Codec / Encoder](http://www.nero.com/enu/company/about-nero/nero-aac-codec.php)
    
    (Windows and Linux platforms only) Instead of using `faac`, future versions of XenonMKV can use an 
    installation of the Nero AAC Encoder. This tool can provide better sound quality when the
    source audio file must be transcoded to two channels. It is distributed under a proprietary
    license and is restricted to personal, non-commercial use.

    When implemented, provide the --neroaacenc-path parameter, or ensure that `neroAacEnc`/`neroAacEnc.exe`
    is in your PATH variable. It will be preferred over `faac` if available.

# Usage
Basic usage with default settings:

    xenonmkv.py /path/to/file.mkv

To ensure your Xbox 360 console will play the resulting file, at a possible expense
of audio quality:

    xenonmkv.py /path/to/file.mkv --profile xbox360

To see all command line arguments:

    xenonmkv.py --help

For a quiet run (batch processing or in a cronjob):

    xenonmkv.py /path/to/file.mkv -q

The -q option ensures you will never be prompted for input and would be useful
for integration with software like SABnzbd+.

If you're reporting an issue, please run XenonMKV in debug/very verbose mode:

    xenonmkv.py /path/to/file.mkv -vv

For the latest release of XenonMKV, I've included a really crummy script that handles batch
encoding of MKV files on Linux, since I always screw up the parameters passed to `find`. Use:

    batch.py source_directory <xenonmkv_parameters>

# Suggestions/Caveats
* If your MKV files aren't too large, distributions that mount `/tmp`
  as tmpfs (planned for Fedora 18, Ubuntu 12.10, Debian Wheezy) can show a
  significant speedup if you use `--scratch-dir /tmp`. Right now for future
  proofing, the scratch directory is set to `/var/tmp`.
* Use `-vv` to find display debug information and output exactly what's going
  on during the processing stages.
* Native multiple file support (eg: convert an entire
  directory of MKVs) is not inherently in this version, but you can do
  something like this in the meantime to queue up a list:

        cd ~/mymkvdir
        for i in `ls *.mkv`; do /path/to/xenonmkv.py $i --destination ~/mymp4dir; done

* Performance on an Intel Core i5-2500K CPU at 3.3GHz, with a 1TB Western
  Digital Black SATA hard drive: A 442MB source MKV file with h.264 video and
  6-channel AC3 audio is converted into a PlayBook-compatible MP4
  (same video, 2-channel AAC audio, quality q=150) in 40.6 seconds.
  This does not have any enhancements such as a tmpfs mount.
  You could probably get much better performance with a solid state drive,
  and obviously processor speed will have an impact here.

# Audio Downmixing/Re-Encoding
By default, XenonMKV tries not to resample, downmix or re-encode any part of
the content provided. However, chances are your source files will contain AC3,
DTS or MP3 audio that needs to be re-encoded. In this case, the original
source audio will always be downmixed to a two channel AAC file before
it is repackaged.

If the audio track in your MKV file is already AAC, the next thing to
consider is your playback device. The Xbox 360 will not play audio in an MP4
container unless it is 2-channel stereo, which is a highly stupid limitation.
Other devices, like the PlayBook, will happily parse up to 5.1 channel audio.
By using either the `--channels` or `--profile` settings, you can tell
XenonMKV how many channels of audio are acceptable from an AAC source before
it will aggressively re-encode and downmix to 2-channel stereo.

In short, if you plan to play MP4s on your Xbox 360, definitely use the
`--profile xbox360` setting to make sure that no more than two channels make
it into the output file. If your device is more reasonable, the default
settings should be fine. More profiles will be added as users confirm their
own device capabilities.

# Known Issues
## MP4Box crash with backtrace
Certain video files, when MP4Box loads them to rejoin into an MP4 container,
will throw a glibc error beginning with:

    *** glibc detected *** MP4Box: free(): invalid next size (fast): 0x0000000000cc8400 ***
    ======= Backtrace: =========
    /lib/x86_64-linux-gnu/libc.so.6(+0x7e626)[0x7f0b09a77626]
    /usr/lib/nvidia-current/tls/libnvidia-tls.so.295.40(+0x1c01)[0x7f0b084c7c01]

This occurs with both nVidia proprietary and Nouveau open source drivers.
The message above is displayed when using the "current" or "current-updates"
versions (295.40, 295.49). When using the 173, 173-updates (173.14.35) or
Nouveau open-source driver, the free() error is the same, but the backtrace
is different:

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

Sometimes this is an intermittent error, so MP4Box has a 3-retry mechanism
that starts with the temporary files. At this time I'm not sure whether it's
system memory or an issue with MP4Box/gpac. There were similar crashing issues
in the original Windows version, which is why multiple versions of MP4Box were
bundled and used for fallback.

If you do see this issue in your own testing, please report it and include a
link to the file that causes the problem if possible. You may also be able to
get the file to convert by using different command line options, such as
`--resume-previous --preserve-temp-files` or including or excluding `-vv`.

Relevant system information:

    $ uname -a
    Linux ubuntu 3.2.0-29-generic #46-Ubuntu SMP Fri Jul 27 17:03:23
    UTC 2012 x86_64 x86_64 x86_64 GNU/Linux

My MP4Box version is the default from the 'gpac' Ubuntu 12.04 package,
which is `0.4.5+svn3462~dfsg0-1`:

    $ MP4Box -version
    MP4Box - GPAC version 0.4.6-DEV-rev
    GPAC Copyright: (c) Jean Le Feuvre 2000-2005
    (c) ENST 2005-200X
    GPAC Configuration: --build=x86_64-linux-gnu --prefix=/usr --includedir=${prefix}/include --mandir=${prefix}/share/man --infodir=${prefix}/share/info --sysconfdir=/etc --localstatedir=/var --libdir=${prefix}/lib/x86_64-linux-gnu --libexecdir=${prefix}/lib/x86_64-linux-gnu --disable-maintainer-mode --disable-dependency-tracking --prefix=/usr --mandir=${prefix}/share/man --libdir=lib/x86_64-linux-gnu --extra-cflags='-Wall -fPIC -DPIC -I/usr/include/mozjs -DXP_UNIX' --enable-joystick --enable-debug --disable-ssl
    Features: GPAC_HAS_JPEG GPAC_HAS_PNG

A fresh compile of the 0.5.0 version available at
<https://sourceforge.net/projects/gpac/> also appears to trigger this bug:

    MP4Box - GPAC version 0.5.0-rev4065
    GPAC Copyright: (c) Jean Le Feuvre 2000-2005
        (c) ENST 2005-200X
    GPAC Configuration:  --enable-debug
    Features: GPAC_DISABLE_3D

The version included with Ubuntu 10.04 (`0.4.5-0.3ubuntu6`) does not appear
to have issues with the same files that fail on 0.4.6 and newer.

# Contributing
Please submit all pull requests to the 'development' branch. I'd like to avoid merging contributions directly to master. 
It is also much easier for me to deal with PRs when your forked repository remains publicly viewable and accessible, until I've had a chance to assess it. In other words, please leave your forks and branches open until a merge/no-merge decision is made.
