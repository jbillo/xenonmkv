class ReferenceFrameValidator:
    @staticmethod
    def validate(height, width, reference_frames):
        max_frames_1280 = [
            [720, 9],
            [648, 10],
            [588, 11],
            [540, 12],
            [498, 13],
            [462, 14],
            [432, 15],
            [405, 16]
        ]

        max_frames_1920 = [
            [1088, 4],
            [864, 5],
            [720, 6]
        ]

        if height == 0 or width == 0 or reference_frames == 0:
            return False

        if width == 1280:
            return ReferenceFrameValidator.recurse_width(
                max_frames_1280, 0, height, reference_frames)
        elif width == 1920:
            return ReferenceFrameValidator.recurse_width(
                max_frames_1920, 0, height, reference_frames)
        else:
            # Does not exceed reference frame table
            return False

    @staticmethod
    def recurse_width(frame_list, index, height, reference_frames):
        if index > len(frame_list):
            # Gone over the length of the frame list.
            # Does not exceed max values.
            return False

        current_test = frame_list[index]
        if height < current_test[0]:
            return ReferenceFrameValidator.recurse_width(
                frame_list, index + 1, height, reference_frames)
        elif height == current_test[0]:
            return reference_frames > current_test[1]
        else:  # height > current_test[0]:
            # Selected element one past the current test - drop back
            current_test = frame_list[index - 1]
            return reference_frames > current_test[1]
