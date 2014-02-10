# Reference Frames and You

**TL;DR:** Your file contains complex video content and may not play correctly on certain devices. These are typically phones or tablets with lower video decoding power than a desktop or laptop computer. By default, XenonMKV will not process these files to save you the aggravation of dealing with a choppy or unplayable video.

## Technical Details

XenonMKV re-encodes audio only when necessary, and does not touch the quality or format of the source video. Certain video tracks contain more reference frames than the H.264 Level 4.1 standard (http://en.wikipedia.org/wiki/H.264/MPEG-4_AVC) and therefore will play poorly on certain hardware devices. The Xbox 360 is particularly sensitive to these restrictions. 