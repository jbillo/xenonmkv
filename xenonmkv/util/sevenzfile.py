# Adapted from http://stackoverflow.com/questions/10701528/example-of-how-to-use-pylzma

import py7zlib
import os


class SevenZFile(object):
    archive = None

    def __init__(self, path):
        fp = open(path, 'rb')
        self.archive = py7zlib.Archive7z(fp)

    def extractall(self, path):
        for name in self.archive.getnames():
            outfilename = os.path.join(path, name)
            outdir = os.path.dirname(outfilename)
            if not os.path.exists(outdir):
                os.makedirs(outdir)
            outfile = open(outfilename, 'wb')
            outfile.write(self.archive.getmember(name).read())
            outfile.close()