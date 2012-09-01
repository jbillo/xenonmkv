' String extraction functions abstracted out.
Module StringExtract
    Function GetTrackNumber(ByVal s As String) As Integer
        Dim t As String = "|  + Track number: "
        If s.Contains(t) Then
            s = s.Substring(s.IndexOf(t) + t.Length)
            If s.IndexOf(vbNewLine) > 0 Then
                Return CInt(s.Substring(0, s.IndexOf(vbNewLine)))
            Else
                Return CInt(s)
            End If
        Else
            Return 0
        End If
    End Function

    Function GetTrackIsDefault(ByVal s As String) As Boolean
        Dim t As String = "|  + Default flag: "
        If s.Contains(t) Then
            s = s.Substring(s.IndexOf(t) + t.Length)
            If s.IndexOf(vbNewLine) > 0 Then
                s = s.Substring(0, s.IndexOf(vbNewLine))
            Else
                s = s.Substring(s, 1)
            End If
            Return s.Contains("1")
        Else
            Return False
        End If
    End Function

    Function GetLanguage(ByVal s As String) As String
        Dim t As String = "|  + Language: "
        If s.Contains(t) Then
            s = s.Substring(s.IndexOf(t) + t.Length)
            If s.IndexOf(vbNewLine) > 0 Then
                Return s.Substring(0, s.IndexOf(vbNewLine))
            Else
                Return s.Substring(0, 3)
            End If
        Else
            Return ""
        End If
    End Function


    Function GetVideoFPS(ByVal s As String) As String
        Dim t As String = "|  + Default duration: "
        If s.Contains(t) Then
            s = s.Substring(s.IndexOf(t) + t.Length)
            s = s.Substring(s.IndexOf("(") + 1)
            If s.IndexOf(" ") > 0 Then
                Return s.Substring(0, s.IndexOf(" "))
            Else
                Return s.Substring(0, 4)
            End If
        Else
            Return 0
        End If
    End Function

    Function GetReferenceFrames(ByVal mediaInfo As String) As Integer
        Dim t As String = "Codec_Settings_RefFrames"
        If mediaInfo.Contains(t) Then
            Dim s As String = mediaInfo.Substring(mediaInfo.IndexOf(t) + t.Length)
            s = s.Substring(s.IndexOf(": ") + 2)
            Dim i As Integer = s.IndexOf(vbNewLine)
            If i = -1 Then
                i = 2 ' pick default value in case vbNewLine isn't in string
            End If
            s = s.Substring(0, i) ' this will either be a 2 digit number or 1 number followed by a letter
            If IsNumeric(s) Then
                Return CInt(s)
            Else
                Return CInt(s.Substring(0, 1))
            End If
        End If

        Return 0
    End Function

    Function GetTrackType(ByVal s As String) As String
        Dim t As String = "|  + Track type: "
        If s.Contains(t) Then
            s = s.Substring(s.IndexOf(t) + t.Length)
            If s.IndexOf(vbNewLine) > 0 Then
                Return s.Substring(0, s.IndexOf(vbNewLine))
            Else
                Return s.Substring(0, 5)
            End If
        Else
            Return 0
        End If
    End Function
End Module
