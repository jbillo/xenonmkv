Module RetrieveTools

    Dim downloadMessage As String = "You can download it from " & NERO_AAC_URL & ". Once you've opened the ZIP, place the EXE files from win32\ in the " & vbNewLine & _
        AppPath & "\tools\ folder. " & vbNewLine & vbNewLine & _
        "(To copy this message, press Ctrl+C, then open Notepad and click Edit > Paste.)"

    Public Function CheckNeroAAC() As Boolean
        If My.Computer.FileSystem.FileExists(AppPath & "\tools\NeroAacEnc.exe") Then
            Return True
        Else
            Dim resp As Integer = MsgBox("You'll need the Nero AAC encoder in order to properly parse files. Due to licensing restrictions, you must download it yourself. " & vbNewLine & _
                "To download the file and automatically install the tool, click OK. To exit the program, click Cancel.", MsgBoxStyle.OkCancel + MsgBoxStyle.Question, _
                "Download and install Nero AAC Encoder?")
            If resp = MsgBoxResult.Ok Then
                RetrieveNeroAAC()
                Return True
            Else
                MsgBox(downloadMessage, MsgBoxStyle.Information, "Nero AAC Encoder Download Information")
                End
            End If
        End If
    End Function


    Public Sub RetrieveNeroAAC()
        Try
            My.Computer.Network.DownloadFile(NERO_AAC_URL, AppPath & "\tools\neroaac.zip", "", "", True, 30, True)
        Catch downloadEx As Exception
            MsgBox("There was an error retrieving the Nero AAC encoder. " & vbNewLine & _
                        downloadMessage, MsgBoxStyle.Exclamation, "Nero AAC Encoder Download Information")
            End
        End Try
        UnzipFile(AppPath & "\tools\neroaac.zip", AppPath & "\tools\", "win32/neroAacEnc.exe", True)
        If My.Computer.FileSystem.FileExists(AppPath & "\tools\neroAacEnc.exe") Then
            Try
                My.Computer.FileSystem.DeleteFile(AppPath & "\tools\neroaac.zip")
            Catch ex As Exception
                ' ignore this
            End Try
            Exit Sub
        Else
            MsgBox("There was an error retrieving the Nero AAC encoder. " & vbNewLine & _
            downloadMessage, MsgBoxStyle.Exclamation, "Nero AAC Encoder Download Information")
            End
        End If
    End Sub

End Module
