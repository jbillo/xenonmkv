#!/usr/bin/env python

import subprocess
import os


def main():
    # Run tests against XenonMKV to confirm that everything
    # is operational after code changes
    passes = 0
    tests = [
        "file_not_exists",
        "destdir_not_exists",
        "invalid_mkv_file",
        "mkv_file_no_usable_tracks",
        "mkv_file_no_video_track",
        "mkv_file_no_audio_track",
        "invalid_codec_wvc1",
    ]

    for test in tests:
        result = globals()["test_" + test]()
        if result:
            passes += 1
            pf = "Pass"
        else:
            pf = "Fail"
        print "Test " + test + ": " + pf

    print "%i/%i tests passed" % (passes, len(tests))


def call(args):
    try:
        cmd = ["python", os.path.join(os.getcwd(), "xenonmkv.py")]
        cmd.extend(args)
        output = subprocess.check_output(cmd, stderr=subprocess.STDOUT)
    except subprocess.CalledProcessError as e:
        output = e.output

    return output


def test_file_not_exists():
    # Pass a file path that does not exist to XenonMKV.
    # Expected result: output string contains 'CRITICAL' and 'does not exist'
    output = call(["/tmp/file_does_not_exist.mkv"])
    return "[CRITICAL]" in output and "does not exist" in output


def test_destdir_not_exists():
    # Pass an output directory that does not exist.
    # Expected result: output string contains
    # 'CRITICAL', 'directory' and 'does not exist'
    # This is a different result from the original source file not existing
    output = call(["tests/invalid.mkv", "-d", "/tmp/dir/does/not/exist"])
    return in_output(["[CRITICAL]", "directory", "does not exist"], output)


def test_invalid_mkv_file():
    # Pass an invalid file that contains a string of text
    # and is not a valid MKV container.
    # Expected result: output contains 'CRITICAL' and
    # prompts for a valid MKV file
    output = call([os.path.join("tests", "invalid.mkv")])
    return in_output(["[CRITICAL]", "valid MKV file"], output)


def test_mkv_file_no_usable_tracks():
    # Pass an MKV container that contains only 'subtitle' tracks.
    # Expected result: output contains 'CRITICAL' and
    # specifically mentions no usable tracks.
    output = call([os.path.join("tests", "subtitles_only.mkv")])
    return in_output(["[CRITICAL]", "No", "track found"], output)


def test_mkv_file_no_video_track():
    # Pass a file that contains one audio track, but no video tracks.
    # Expected result: output contains 'CRITICAL' and
    # specifically mentions lack of a video track.
    output = call([os.path.join("tests", "no_video_track.mkv")])
    return in_output(["[CRITICAL]", "No video track found"], output)


def test_mkv_file_no_audio_track():
    # Pass a file that contains one video track, but no audio tracks.
    # Expected result: output contains 'CRITICAL'
    # and specifically mentions lack of an audio track.
    output = call([os.path.join("tests", "no_audio_track.mkv")])
    return in_output(["[CRITICAL]", "No audio track found"], output)


def test_invalid_codec_wvc1():
    # Pass a file that contains a WVC1 video track.
    # Expected result: output contains 'CRITICAL' and
    # specifically mentions that WVC1 is not supported
    output = call([os.path.join("tests", "invalid_codec_wvc1.mkv")])
    return in_output(["[CRITICAL]",
                     "WVC1 codec in selected video track is not supported"], output)


def in_output(strings, output):
    if "Exception" in output:
        print ("An exception was thrown in the output, failing test "
               "automatically. Details: ")
        print output
        return False

    for item in strings:
        if item not in output:
            print ("One or more required items were not found in the output. "
                   "Details:")
            print output
            return False

    return True

if __name__ == "__main__":
    main()
