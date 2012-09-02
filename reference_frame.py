#!/usr/bin/python

class ReferenceFrameValidator:
	@staticmethod
	def validate(height, width, reference_frames):
		max_frames_1280 = []
		max_frames_1920 = []

		max_frames_1280.append([720, 9])
		max_frames_1280.append([648, 10])
		max_frames_1280.append([588, 11])
		max_frames_1280.append([540, 12])
		max_frames_1280.append([498, 13])
		max_frames_1280.append([462, 14])
		max_frames_1280.append([432, 15])
		max_frames_1280.append([405, 16])

		max_frames_1920.append([1088, 4])
		max_frames_1920.append([864, 5])
		max_frames_1920.append([720, 6])

		if height == 0 or width == 0 or reference_frames == 0:
			return False

		if width == 1280:
			return ReferenceFrameValidator.recurse_width(max_frames_1280, 0, height, reference_frames)
		elif width == 1920:
			return ReferenceFrameValidator.recurse_width(max_frames_1920, 0, height, reference_frames)
		else:
			# Does not exceed reference frame table
			return False

	@staticmethod
	def recurse_width(frame_list, index, height, reference_frames):
		if index > len(frame_list):
			# Gone over the length of the frame list. Does not exceed max values.
			return False

		current_test = frame_list[index]
		if height < current_test[0]:
			return recurse_width(frame_list, index + 1, height, reference_frames)
		elif height == current_test[0]:
			return reference_frames > current_test[1]
		else: # height > current_test[0]:
			# One past the current test - drop back
			current_test = frame_list[index - 1]
			return reference_frames > current_test[1]
