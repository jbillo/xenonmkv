import os
import sys
from xenonmkv.support_tools import SupportTools


class FileUtils:
    log = args = None
    FOUR_GIGS = (4 * 1024 * 1024 * 1024)
    app_path = os.path.dirname(os.path.realpath(sys.argv[0]))
    tools_path = os.path.join(app_path, "tools")

    def __init__(self, log, args):
        self.log = log
        self.args = args

    def scan_app_paths(self, ospath, app):
        if sys.platform.startswith("win32"):
            filename_ext = ".exe"
        else:
            filename_ext = ""

        filename = app + filename_ext
        for path in ospath:
            # Detect if the file exists in the OS path,
            # or one level deep with the app name.
            if os.path.isfile(os.path.join(path, filename)):
                return os.path.join(path, filename)
            elif os.path.isfile(os.path.join(path, app, filename)):
                return os.path.join(path, app, filename)
            else:
                pass

        return False

    # Check if all dependent applications are installed on the system
    # and present in PATH
    # TODO: Allow custom path to be specified for each of these
    # if it is in a config file
    def check_dependencies(self, dependency_list):
        dependency_paths = {}
        library_paths = []

        # Add Python default path and OS environment variable if available
        ospath = set(os.defpath.split(os.pathsep))

        if "PATH" in os.environ:
            for path in os.environ["PATH"].split(os.pathsep):
                ospath.add(path)
        else:
            self.log.warning("No PATH environment variable defined. "
                             "This could cause issues locating certain tools.")

        # Add own internal tool path
        ospath.add(self.tools_path)

        # If on OS X, append various paths under /Applications/
        # to search for system wide tools.
        if sys.platform.startswith("darwin"):
            ospath.add("/Applications/Mkvtoolnix.app/Contents/MacOS")
            ospath.add("/Applications/Osmo4.app/Contents/MacOS")

        # If on Windows, try adding various paths under ProgramFiles.
        if sys.platform.startswith("win32"):
            # GPAC/MP4box
            ospath.add(os.path.join(os.environ['PROGRAMFILES'], "GPAC"))
            if "PROGRAMFILES(X86)" in os.environ:
                ospath.add(
                    os.path.join(os.environ['PROGRAMFILES(X86)'], "GPAC")
                )

        for app in dependency_list:
            app_present = False
            # Check if the custom app parameter is set
            if app.lower() + "_path" in self.args:
                custom_path = getattr(self.args, app.lower() + "_path")
                if custom_path and os.path.isfile(custom_path):
                    self.log.debug("Using custom path for dependent "
                                   "application {0}: {1}".format(app, custom_path))
                    dependency_paths[app.lower()] = custom_path
                    # Because some applications have libraries in their own
                    # directory, add the base path of custom_path
                    # to a variable that can later be used to set
                    # LD_LIBRARY_PATH.
                    library_paths.append(os.path.dirname(custom_path))

                    # Proceed to next application and do not let this one get
                    # overwritten with the default later on.
                    continue
                elif not custom_path:
                    # Fall through and set default version, but do not warn
                    # since the dependent application is not set
                    pass
                else:
                    self.log.error("Dependent application {0} does not exist "
                                   "as {1}; looking for default version".format(
                                   app, custom_path))

            app_present = self.scan_app_paths(ospath, app)

            if app_present:
                self.log.debug("Found tool {0} at {1} by scanning common paths".format(
                    app, app_present))
                dependency_paths[app.lower()] = app_present
                continue

            """
            At this point we haven't specified a custom app location,
            nor found it in PATH.
            This problem is likely to come up with clean installations,
            or if someone tries to run on Windows.

            What we can do is the following:
            * Windows/OSX: Offer to download and extract the specific
              applications - either locally or system wide.
            * Linux: Hint for package manager names, perhaps tailored to
              RPM/DEB/source distributions.
            """

            # If we're at this point and the --quiet option is set,
            # raise an exception
            if self.args.quiet:
                self.log.warning("Cannot prompt to install dependent tool {0}".format(
                                 app))
                raise IOError("Dependent application '{0}' was not found in "
                              "PATH or the {1} directory, and quiet mode does not allow "
                              "user input. Please make sure {0} is installed.".format(
                              app, self.tools_path))

            support_tools = SupportTools(self.log)
            tool_install_result = support_tools.find_tool(app)

            # tool_install_result can be distinctly "None" or "False"
            if tool_install_result is False:
                # User opted not to install this support tool explicitly
                self.log.error("Dependent application '{0}' was not found in "
                               "PATH or the {1} directory. Please install it.".format(
                               app, self.tools_path))
                sys.exit(1)
            elif not tool_install_result:
                # Some issue occurred with installing the support tool
                raise IOError("Cannot install application '{0}' as one or more errors "
                              "occurred.".format(app))

            # Rebuild ospath with any new entries that may have been added
            # as a result of the app install
            if "PATH" in os.environ:
                for path in os.environ["PATH"].split(os.pathsep):
                    ospath.add(path)

            # Once more, check if the app is actually in the path
            # (depending on installer, could have been installed system-wide
            # or just to tools directory)
            app_present = self.scan_app_paths(ospath, app)

            if app_present:
                self.log.debug("Found tool {0} at {1} by rescanning "
                               "common paths".format(app, app_present))
                dependency_paths[app.lower()] = app_present
                continue
            else:
                raise IOError("Dependent application '{0}' was not found in "
                              "PATH or the {1} directory. Please make sure it is "
                              "installed.".format(app, self.tools_path))

        return (dependency_paths, library_paths)

    def check_dest_dir(self, destination):
        if not os.path.isdir(destination):
            raise IOError("Destination directory {0} does not exist".format(destination))

    def check_source_file(self, source_file):
        if not os.path.isfile(source_file):
            raise IOError("Source file {0} does not exist".format(source_file))

        filesize = os.path.getsize(source_file)
        if (filesize >= self.FOUR_GIGS):
            self.log.warning("File size of {0} is {1}, which is over 4GiB. "
                             "This file may not play on certain devices, such as the Xbox 360,"
                             " and cannot be copied to a FAT32-formatted storage medium.".format(
                                source_file, filesize)
                             )
            if self.args.error_filesize:
                raise IOError("Cancelling processing as file size limit "
                              "of 4GiB is exceeded")

    def hex_edit_video_file(self, path):
        with open(path, 'r+b') as f:
            f.seek(7)
            f.write("\x29")
        # File is automatically closed when using 'with'

    def delete_temp_files(self, scratch_dir, extensions):
        for extension in extensions:
            temp_video = os.path.join(scratch_dir, "temp_video{0}".format(extension))
            temp_audio = os.path.join(scratch_dir, "temp_audio{0}".format(extension))
            if os.path.isfile(temp_video):
                self.log.debug("Cleaning up: deleting {0}".format(temp_video))
                os.unlink(temp_video)
            if os.path.isfile(temp_audio):
                self.log.debug("Cleaning up: deleting {0}".format(temp_audio))
                os.unlink(temp_audio)

        # Check for audiodump.aac and audiodump.wav
        other_files = ('audiodump.aac', 'audiodump.wav', 'output.mp4')
        for name in other_files:
            cleanup_file = os.path.join(scratch_dir, name)
            if os.path.isfile(cleanup_file):
                self.log.debug("Cleaning up: deleting {0}".format(cleanup_file))
                os.unlink(cleanup_file)
