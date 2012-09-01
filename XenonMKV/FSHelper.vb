Module FSHelper

    Public filesToDelete As New ArrayList
    Public MoveFinalFileException As New Exception("Could not move final file from the scratch directory.")

    Public Function DeleteTempConversionFiles(ByVal curItem As ConvertItem, ByVal scratchFolder As String) As Boolean
        If Not My.Settings.DeleteTempFiles Then
            Return True
        End If

        DeleteSingleFile(scratchFolder & "audiodump.wav")
        DeleteSingleFile(scratchFolder & "audio.m4a")
        DeleteSingleFile(scratchFolder & "audio.aac")
        If curItem.VidCodec <> "" Then
            DeleteSingleFile(scratchFolder & "video" & curItem.VidCodec)
        End If
        If curItem.AudCodec <> "" Then
            DeleteSingleFile(scratchFolder & "audio" & curItem.AudCodec)
        End If
        If Not My.Computer.FileSystem.DirectoryExists(scratchFolder) Then
            My.Computer.FileSystem.CreateDirectory(scratchFolder)
        End If

        Return True
    End Function

    Public Function DeleteSingleFile(ByVal filename As String, Optional ByVal isTempFile As Boolean = False) As Boolean
        Try

            If isTempFile Then
                If Not My.Settings.DeleteTempFiles Then
                    Return True
                End If
            End If

			If System.IO.File.Exists(filename) Then
				My.Computer.FileSystem.DeleteFile(filename)
			End If

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub EnableMP4Support_Regedit()
        Dim isx64System As Boolean = False
        Dim pfPath As String = My.Computer.FileSystem.SpecialDirectories.ProgramFiles
        pfPath = pfPath & " (x86)"
        If My.Computer.FileSystem.DirectoryExists(pfPath) Then
            isx64System = True
        End If

        Dim regPath As String = AppPath & "\tools\" & REGEDIT_MP4_ENABLE_BASE
        If isx64System Then
            regPath = regPath & "-x64"
        End If
        regPath = regPath & ".reg"

        Shell("regedit /s """ & regPath & """", AppWinStyle.NormalFocus, True)

        My.Settings.MP4PlaybackEnabled = True
        My.Settings.Save()
    End Sub


    Public Sub DeleteLogIfExists()
        If My.Computer.FileSystem.FileExists(AppPath & "\log.txt") Then
            Try
                My.Computer.FileSystem.DeleteFile(AppPath & "\log.txt")
            Catch ex As Exception
                ' ignore!
            End Try
        End If
    End Sub

    Public Function MoveFinalFile(ByVal curItem As ConvertItem, ByVal outputPath As String) As String
        Dim s As String = GetFinalFileName(curItem)
        Dim out As String = outputPath & s & "." & My.Settings.OutputExtension

        Try
            DeleteSingleFile(out)

            ' Delete only the files that were created
            DeleteTempConversionFiles(curItem, scratchFolder)
            If curItem.IsSplit Then
                DeleteSingleFile(curItem.FileName)
            End If
            Return s
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function GetFinalFileName(ByVal curItem As ConvertItem) As String
        Dim s As String = My.Computer.FileSystem.GetName(curItem.FileName)
        ' Fix for split: original file name
        If curItem.OriginalFileName <> "" Then
            s = My.Computer.FileSystem.GetName(curItem.OriginalFileName)
        End If

        s = s.Substring(0, s.LastIndexOf("."))

        If (curItem.PartNumber > 0) Then
            s = s & "-" & curItem.PartNumber
        End If

        If My.Settings.OutputUpperCase Then
            s = s.ToUpper
        End If

        Return s
    End Function

    Public Function GetCleanTempDir(ByVal baseDir As String) As String
        Dim i As Integer
        For i = 1 To Integer.MaxValue
            If Not My.Computer.FileSystem.DirectoryExists(baseDir & "\temp_" & i) Then
                Return "\temp_" & i
            End If
        Next

        Return "\"
    End Function

    Public Function EnoughScratchFolderSpace(ByVal filename As String) As Boolean
        Dim sd As String = My.Settings.CustomScratchFolder.Substring(0, 1) & ":\"
        Dim fs As ULong = My.Computer.FileSystem.GetDriveInfo(sd).AvailableFreeSpace
        Dim fileSize As ULong = My.Computer.FileSystem.GetFileInfo(filename).Length

        Return Not ((fileSize * 3) > fs)
    End Function

    Public Function NukeAndPave(ByVal folder As String)
        ' Delete all partial MKV's in here
        For Each foundFile As String In _
            My.Computer.FileSystem.GetFiles(folder, FileIO.SearchOption.SearchTopLevelOnly, "part-*.mkv")
            DeleteSingleFile(foundFile, True)
        Next

        Return True
    End Function

End Module
