Public Class SetAspectRatio

    Public AR As Double
    Public AutoAR As Double

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        If Choice_Auto.Checked = True Then
            AR = AutoAR
        Else 'choice_custom
            ' convert AR to double
            Dim num As Integer, den As Integer
            Try
                If CustomAR.Text.Contains("/") Then
                    num = CustomAR.Text.Substring(0, CustomAR.Text.IndexOf("/"))
                    den = CustomAR.Text.Substring(CustomAR.Text.IndexOf("/") + 1)
                    AR = num / den
                ElseIf CustomAR.Text.Contains(":") Then
                    num = CustomAR.Text.Substring(0, CustomAR.Text.IndexOf(":"))
                    den = CustomAR.Text.Substring(CustomAR.Text.IndexOf(":") + 1)
                    AR = num / den
                Else
                    AR = Double.Parse(CustomAR.Text)
                End If
            Catch ex As Exception
                MsgBox(GetMsg("UI_CustomAR_Fail", New String() {CustomAR.Text}), MsgBoxStyle.Exclamation, GetMsg("UI_CustomAR_FailTitle"))
                Return
            End Try
        End If

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub CustomAR_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomAR.SelectedIndexChanged
        Me.Choice_Auto.Checked = False
        Me.Choice_Custom.Checked = True
    End Sub
End Class