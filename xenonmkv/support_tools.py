import sys
import platform
import os
import webbrowser
import urllib2
import subprocess
import zipfile


class SupportTools():
    ostype = None
    distro = None
    win_64 = None
    log = None
    app_path = os.path.dirname(os.path.realpath(sys.argv[0]))

    tools = {
        "mkvinfo": {
            "friendly_name": "MKVToolnix",
            "website":
                "http://www.bunkus.org/videotools/mkvtoolnix/"
                "downloads.html#windows",
            "windows_direct_download":
                "http://www.bunkus.org/videotools/mkvtoolnix/win32/"
                "mkvtoolnix-unicode-5.8.0-setup.exe",
            "mac_direct_download":
                "http://www.bunkus.org/videotools/mkvtoolnix/macos/"
                "Mkvtoolnix-5.7.0_2012-07-11-cd22.dmg",
        },
        "mediainfo": {
            "friendly_name": "MediaInfo",
            "website":
                "http://mediainfo.sourceforge.net/en/Download",
            "mac_direct_download":
                "http://mediaarea.net/download/binary/mediainfo/0.7.60/"
                "MediaInfo_CLI_0.7.60_Mac_i386%2Bx86_64.dmg",
            "windows_direct_download":
                "http://mediaarea.net/download/binary/mediainfo/0.7.60/"
                "MediaInfo_CLI_0.7.60_Windows_i386.zip",
            "windows_direct_download_64":
                "http://mediaarea.net/download/binary/mediainfo/0.7.60/"
                "MediaInfo_CLI_0.7.60_Windows_x64.zip"
        },
        "mplayer": {
            "friendly_name": "mplayer",
            "website":
                "http://oss.netfarm.it/mplayer-win32.php",
            "windows_direct_download":
                "https://github.com/downloads/jbillo/"
                "xenonmkv/MPlayer-p4-svn-34401.zip"
        },
        "faac": {
            "friendly_name": "FAAC",
            "website": "http://www.rarewares.org/aac-encoders.php",
            "windows_direct_download":
                "http://www.rarewares.org/files/aac/faac-1.28-mod.zip",
        },
        "MP4Box": {
            "friendly_name": "MP4Box (GPAC)",
            "website": "http://gpac.wp.mines-telecom.fr/downloads/",
            "windows_direct_download":
                "http://download.tsi.telecom-paristech.fr/gpac/release/"
                "0.5.0/GPAC.Framework.Setup-0.5.0.exe"
        }
    }

    def __init__(self, log):
        self.log = log

        # Set internal variables based on OS
        if sys.platform.startswith("linux"):
            self.ostype = "linux"
            self.distro = platform.linux_distribution()
        elif sys.platform.startswith("win32"):
            self.ostype = "windows"
            self.win_64 = 'PROGRAMFILES(X86)' in os.environ
        elif sys.platform.startswith("darwin"):
            self.ostype = "mac"
        else:
            # Won't be able to support a lot here
            self.ostype = "unknown"

    def write_uac_regkey(self, path):
        # Write a UAC registry key to force certain installers
        # to request elevated permissions.
        try:
            import _winreg
            regkey = _winreg.CreateKey(_winreg.HKEY_CURRENT_USER,
                "Software\\Microsoft\\Windows NT\\CurrentVersion\\"
                "AppCompatFlags\\Layers")
            _winreg.SetValueEx(regkey, path, 0, _winreg.REG_SZ, "RUNASADMIN")
            _winreg.CloseKey(regkey)
        except:
            self.log.warning("Could not write UAC/elevation registry key "
                "for %s" % path)
            return False

        return True

    def find_tool(self, app):
        try:
            return self.offer_install(app)  # getattr(self, "find_" + app)()
        except AttributeError:
            # Haven't written a check for this app yet
            return None

    def download_file(self, url, destination_dir=None):
        if not destination_dir or not os.path.isdir(destination_dir):
            destination_dir = os.path.join(self.app_path, "tools")

        file_name = url.split("/")[-1]
        destination_path = os.path.join(destination_dir, file_name)

        # Debugging: if the file exists already,
        # return true and don't download again
        # This should be removed for non-debug copies
        if os.path.isfile(destination_path):
            self.log.debug("File %s already exists; skipping download step" %
                destination_path)
            return destination_path

        self.log.debug("Opening URL %s" % url)
        try:
            u = urllib2.urlopen(url)
        except urllib2.URLError as ue:
            self.log.error("Could not open URL %s (%s)" % (url, ue))
            return False

        f = open(destination_path, 'wb')
        meta = u.info()
        file_size = int(meta.getheaders("Content-Length")[0])

        file_size_dl = 0
        block_sz = 8192
        while True:
            dl_buffer = u.read(block_sz)
            if not dl_buffer:
                break

            file_size_dl += len(dl_buffer)
            f.write(dl_buffer)
            status = (r"%10d    bytes [%3.2f%%]" %
                (file_size_dl, file_size_dl * 100. / file_size))
            status = status + chr(8) * (len(status) + 1)
            print status,

        f.close()
        # Output a new line to avoid interfering with subsequent output
        print
        return destination_path

    def install_file(self, app, path):
        file_extension = os.path.splitext(path)[1].lower()
        output_dir = os.path.join(os.path.dirname(path), app)

        if file_extension == ".zip":
            self.log.debug("Attempting to extract ZIP %s" % path)
            # Extract .zip to appropriate directory
            zip_file = zipfile.ZipFile(path, 'r')
            if not os.path.isdir(output_dir):
                os.mkdir(output_dir)

            self.log.debug("Extracting all contents from %s to %s"
                % (path, output_dir))
            zip_file.extractall(output_dir)
            return True
        elif file_extension == ".exe":
            # Assume it is a Windows install; just run the application
            self.write_uac_regkey(path)
            self.log.debug("Running executable %s" % path)
            try:
                subprocess.check_call(path, shell=True)
            except subprocess.CalledProcessError:
                # installation failed, return false
                self.log.error("Installation of %s failed or was cancelled" %
                    self.tools[app]["friendly_name"])
                return False

            return True
        elif file_extension == ".dmg":
            # Mount DMG with hdiutil
            subprocess.call(["hdiutil", "attach", path,
                "-mountpoint", "/Volumes/" + app])
            if app == "mkvinfo":
                # Copy Mkvtoolnix.app to /Applications
                subprocess.call(["sudo cp -R /Volumes/%s/Mkvtoolnix.app "
                    "/Applications/" % app], shell=True)
            elif app == "mediainfo":
                # TODO: Run package installer for MediaInfo CLI
                #subprocess.call
                pass

            # All done - unmount the volume
            subprocess.call(["diskutil", "unmount", "force",
                "/Volumes/" + app])
            return True
        elif file_extension == ".7z":
            self.log.debug("Attempting to extract LZMA/.7z file %s" % path)
            try:
                from xenonmkv.util import sevenzfile
            except ImportError:
                # TODO: Check if we have a 7zip executable around to use
                # Could not find py7zlib; error out and let user know
                # it needs to be installed
                self.log.error("Could not import py7zlib/pylzma to extract "
                    ".7z file. %s will need to be installed manually." % app)
                return False

            # We have py7zlib imported
            self.log.debug("sevenzfile library imported; proceeding to"
                "extract %s" % path)

            sz_file = sevenzfile.SevenZFile(path)
            sz_file.extractall(output_dir)

            return True
        else:
            # File extension unknown
            return False

    def offer_install(self, app):
        properties = self.tools[app]
        print ("It appears that your system is missing %s, which is required "
            "for XenonMKV to work. You can either download it from its website "
            "at %s, or a copy can be installed automatically." % (
            properties["friendly_name"],
            properties["website"],
        ))

        print """Options:

[Y] Yes: Download and install %s automatically (default)
[W] Website: Open the website for %s in the default browser.
    Once you have downloaded and installed %s, run XenonMKV again to continue.
[N] No: Do not install %s. XenonMKV will exit.
        """ % (properties["friendly_name"], properties["friendly_name"],
        properties["friendly_name"], properties["friendly_name"])

        try:
            response = raw_input("Install %s automatically (default=Yes) "
                "[Y/w/n]? " % properties["friendly_name"])
            response = response.strip().lower()
            if response in ('', 'y', 'yes'):
                # TODO: Download app and run installer/decompress
                download_property = self.ostype + "_direct_download"
                if self.win_64 and download_property + "_64" in properties:
                    url = properties[download_property + "_64"]
                else:
                    url = properties[download_property]
                self.log.info("Attempting to download installation package "
                    "from %s" % url)
                file_path = self.download_file(url)

                if not file_path:
                    self.log.error("Could not download %s package "
                        "automatically" % properties["friendly_name"])
                    return None
                else:
                    self.log.debug("Package %s downloaded successfully" %
                        properties["friendly_name"])

                # Install application according to OS mechanism
                install_result = self.install_file(app, file_path)
                if not install_result:
                    self.log.error("Could not install %s package "
                        "automatically" % properties["friendly_name"])
                else:
                    self.log.info("Package %s installed successfully" %
                        properties["friendly_name"])

                return install_result
            elif response in ('w', 'website'):
                webbrowser.open(properties["website"])
                return False
            else:
                # Return false as the user didn't choose to install
                # the support tool.
                return False
        except KeyboardInterrupt:
            # Print newline before critical log line
            print
            self.log.critical("Installation prompt cancelled; exiting")
            sys.exit(1)
