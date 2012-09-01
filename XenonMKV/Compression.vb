Imports ICSharpCode.SharpZipLib.Zip
Imports System.IO

Module Compression

    Public Function UnzipFile(ByVal filename As String, ByVal workingDir As String, Optional ByVal specificFile As String = "", Optional ByVal flattenDirs As Boolean = False) As Boolean
        Directory.SetCurrentDirectory(workingDir)

        If Not My.Computer.FileSystem.FileExists(filename) Then
            Return False
        End If

        Dim fileStream As New IO.FileStream(filename, FileMode.Open)
        Dim zInputStream As New ZipInputStream(fileStream)
        Dim zEntry As ZipEntry

        While True
            zEntry = zInputStream.GetNextEntry
            If zEntry Is Nothing Then
                Exit While
            End If

            If specificFile <> "" And zEntry.Name <> specificFile Then
                Continue While
            End If

            Dim dirName As String = Path.GetDirectoryName(zEntry.Name)
            Dim file As String = Path.GetFileName(zEntry.Name)

            If dirName.Length <> 0 And flattenDirs = False Then
                Directory.CreateDirectory(dirName)
            End If

            If file <> "" Then
                Dim tempName As String
                If flattenDirs = True Then
                    tempName = file
                Else
                    tempName = zEntry.Name
                End If
                Dim streamWriter As New FileStream(tempName, FileMode.Create)
                Dim size As Integer = 2048
                Dim data(2048) As Byte
                While True
                    size = zInputStream.Read(data, 0, data.Length)
                    If (size > 0) Then
                        streamWriter.Write(data, 0, size)
                    Else
                        Exit While
                    End If
                End While
                streamWriter.Close()
            End If
        End While
        zInputStream.Close()
        fileStream.Close()

        Return True
    End Function

End Module
