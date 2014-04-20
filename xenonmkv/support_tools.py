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
                "downloads.html",
            "windows_direct_download":
                "http://www.bunkus.org/videotools/mkvtoolnix/win32/"
                "mkvtoolnix-unicode-5.8.0-setup.exe",
            "mac_direct_download":
                "http://www.bunkus.org/videotools/mkvtoolnix/macos/"
                "Mkvtoolnix-5.7.0_2012-07-11-cd22.dmg",
            "apt_package":
                "mkvtoolnix",
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
                "MediaInfo_CLI_0.7.60_Windows_x64.zip",
            "apt_package":
                "mediainfo",
        },
        "mplayer": {
            "friendly_name": "mplayer",
            "website":
                "http://oss.netfarm.it/mplayer-win32.php",
            "mac_website":
                "http://www.mplayerosx.ch/#downloads",
            "windows_direct_download":
                "http://jakebillo.com/xenonmkv/MPlayer-p4-svn-34401.zip",
            "mac_direct_download":
                "http://mplayerosxext.googlecode.com/files/MPlayer-OSX-Extended_rev14.zip",
            "apt_package":
                "mplayer",
        },
        "faac": {
            "friendly_name": "FAAC",
            "website": "http://www.rarewares.org/aac-encoders.php",
            "windows_direct_download":
                "http://www.rarewares.org/files/aac/faac-1.28-mod.zip",
            "mac_direct_download":
                "http://jakebillo.com/xenonmkv/faac_osx_1.28.zip",
            "apt_package":
                "faac",
        },
        "MP4Box": {
            "friendly_name": "MP4Box (GPAC)",
            "website": "http://gpac.wp.mines-telecom.fr/downloads/",
            "windows_direct_download":
                "http://download.tsi.telecom-paristech.fr/gpac/release/"
                "0.5.0/GPAC.Framework.Setup-0.5.0.exe",
            "mac_direct_download":
                "http://download.tsi.telecom-paristech.fr/gpac/release/0.5.0/GPAC-0.5.0-OSX-64bits.dmg",
            "apt_package":
                "gpac",
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
                "for {0}".format(path))
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
        if os.path.isfile(destination_path):
            self.log.debug("File {0} already exists; skipping download step".format(
                destination_path))
            return destination_path

        self.log.debug("Opening URL {0}".format(url))
        try:
            u = urllib2.urlopen(url)
        except urllib2.URLError as ue:
            self.log.error("Could not open URL {0} ({1})".format(url, ue))
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

    def sudo_call(self, cmd):
        print("This tool installer requires 'sudo' access. "
            "You may be prompted for your account password to continue.")
        cmd = "sudo {0}".format(cmd)
        return subprocess.call([cmd], shell=True)

    def install_file(self, app, path):
        # Check if we need to run aptitude - Linux/Ubuntu for now
        if not path:
            if not "apt_package" in self.tools[app]:
                self.log.warning("No apt package was found for app {0}; "
                                 "cannot install automatically".format(app))
                return False

            apt_package = self.tools[app]["apt_package"]
            self.log.debug("Attempting to install app {0} with apt (package: {1})".format(app, apt_package))
            result = self.sudo_call("apt-get -y install {0}".format(apt_package))
            return (result == 0)

        file_extension = os.path.splitext(path)[1].lower()
        output_dir = os.path.join(os.path.dirname(path), app)

        if file_extension == ".zip":
            self.log.debug("Attempting to extract ZIP {0}".format(path))
            # Extract .zip to appropriate directory
            zip_file = zipfile.ZipFile(path, 'r')
            if not os.path.isdir(output_dir):
                os.mkdir(output_dir)

            self.log.debug("Extracting all contents from {0} to {1}".format(path, output_dir))
            zip_file.extractall(output_dir)

            # Special case: create symlink on OS X for mplayer under tools path
            if app == "mplayer" and self.ostype == "mac":
                os.symlink(os.path.join(output_dir,
                    "MPlayer OSX Extended.app/Contents/MacOS/MPlayer OSX Extended"),
                os.path.join(output_dir, "mplayer"))

            # Special case: check for .xenonmkv file in output directory
            if (app == "faac" and self.ostype == "mac" and
                os.path.isfile(os.path.join(output_dir, "faac", ".xenonmkv"))):
                # Copy contents to /usr/local for proper execution
                self.sudo_call("cp -R {0}/faac/* /usr/local/".format(output_dir))

            return True
        elif file_extension == ".exe":
            # Assume it is a Windows install; just run the application
            self.write_uac_regkey(path)
            self.log.debug("Running executable {0}".format(path))
            try:
                subprocess.check_call(path, shell=True)
            except subprocess.CalledProcessError:
                # installation failed, return false
                self.log.error("Installation of %s failed or was cancelled" %
                    self.tools[app]["friendly_name"])
                return False

            return True
        elif file_extension == ".dmg":
            # This will always be on OS X so we don't need to check for OS type
            # Mount DMG with hdiutil
            subprocess.call(["hdiutil", "attach", path,
                "-mountpoint", "/Volumes/" + app])
            if app == "mkvinfo":
                # Copy Mkvtoolnix.app to /Applications
                self.sudo_call("cp -R /Volumes/%s/Mkvtoolnix.app "
                    "/Applications/" % app)
            elif app == "mediainfo":
                self.sudo_call("installer -pkg \"/Volumes/%s/MediaInfo CLI.pkg\" -target /" % app)
            elif app == "MP4Box":
                self.sudo_call("cp -R /Volumes/{0}/Osmo4.app /Applications/".format(app))
                self.sudo_call("chmod -R g+rx,o+rx /Applications/Osmo4.app")

            # All done - unmount the volume
            subprocess.call(["diskutil", "unmount", "force",
                "/Volumes/" + app])
            return True
        else:
            # File extension unknown
            return False

    def try_download(self, download_property, properties):
        if not download_property in properties:
            self.log.error("XenonMKV does not have a direct download available"
                " for the {0} installer".format(properties["friendly_name"]))
            return None

        if self.win_64 and download_property + "_64" in properties:
            url = properties[download_property + "_64"]
        else:
            url = properties[download_property]

        self.log.info("Attempting to download installation package "
            "from {0}".format(url))
        file_path = self.download_file(url)

        if not file_path:
            self.log.error("Could not download {0} package "
                "automatically".format(properties["friendly_name"]))
            return None
        else:
            self.log.debug("Package {0} downloaded successfully".format(properties["friendly_name"]))

        return file_path

    def offer_install(self, app):
        properties = self.tools[app]
        website = properties["website"]
        if ("{0}_website".format(self.ostype)) in properties:
            website = properties["{0}_website".format(self.ostype)]

        # TODO: change this to use string .format style
        print ("It appears that your system is missing %s, which is required "
            "for XenonMKV to work. You can either download it from its website "
            "at %s, or a copy can be installed automatically." % (
            properties["friendly_name"],
            website,
        ))

        print """Options:

[Y] Yes: Download and install {0} automatically (default)
[W] Website: Open the website for {0} in the default browser.
    Once you have downloaded and installed {0}, run XenonMKV again to continue.
[N] No: Do not install {0}. XenonMKV will exit.
        """.format(properties["friendly_name"])

        try:
            response = raw_input("Install {0} automatically (default=Yes) "
                "[Y/w/n]? ".format(properties["friendly_name"]))
            response = response.strip().lower()
            if response == '' or response.startswith('y'):
                download_property = self.ostype + "_direct_download"

                # Check if we need to download a file
                if self.ostype == "linux" and self.distro[0] == "Ubuntu":
                    # skip download and just try apt-get install
                    file_path = ""
                else:
                    download_result = self.try_download(download_property, properties)
                    if not download_result:
                        return download_result
                    file_path = download_result

                # Install application according to OS mechanism
                install_result = self.install_file(app, file_path)
                if not install_result:
                    self.log.error("Could not install {0} package "
                        "automatically".format(properties["friendly_name"]))
                else:
                    self.log.info("Package {0} installed successfully".format(properties["friendly_name"]))

                return install_result
            elif response.startswith('w'):
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
