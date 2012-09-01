Public Class SelectTime

    Public selectedTime As Long = 0

    Public Sub ResetControls()
        VideoCutoffTracker.Minimum = 0
        VideoCutoffTracker.Maximum = curItem.AudLength
        VideoCutoffTracker.Value = 30

        selectedTime = VideoCutoffTracker.Value
        VideoCutoffTime.Text = ConvertSecondsToTime(VideoCutoffTracker.Value)

        If selectedTime < 30 Then
            TimeOK.Visible = False
            OKCutoff.Enabled = False
        End If
    End Sub

    Private Sub SelectTime_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ResetControls()
    End Sub

    Private Sub VideoCutoffTracker_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VideoCutoffTracker.Scroll
        selectedTime = VideoCutoffTracker.Value
        VideoCutoffTime.Text = ConvertSecondsToTime(VideoCutoffTracker.Value)

        If selectedTime < 30 Then
            TimeOK.Visible = False
            OKCutoff.Enabled = False
        Else
            TimeOK.Visible = True
            OKCutoff.Enabled = True
        End If
    End Sub

    Private Sub VideoCutoffTime_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VideoCutoffTime.TextChanged
        If System.Text.RegularExpressions.Regex.IsMatch(VideoCutoffTime.Text, "^([0-9][0-9]?\:)?[0-9]?[0-9]([0-9]*)\:[0-9][0-9]$") Then
            selectedTime = ConvertTimeToSeconds(VideoCutoffTime.Text)
            If selectedTime < 30 Then
                TimeOK.Visible = False
                OKCutoff.Enabled = False
                Exit Sub
            End If
            TimeOK.Visible = True
            OKCutoff.Enabled = True
        Else
            TimeOK.Visible = False
            OKCutoff.Enabled = False
        End If
    End Sub

    Private Sub DontSplit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DontSplit.CheckedChanged
        If DontSplit.Checked Then
            OKCutoff.Text = GetMsg("UI_TimeSplit_OKNoSplit")
        Else
            OKCutoff.Text = GetMsg("UI_TimeSplit_OK")
        End If
    End Sub
End Class