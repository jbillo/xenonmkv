Imports System.Diagnostics
Imports Microsoft.Win32

Module UIEvents

    Public PreviewMediaTT As New ToolTip
    Dim BrowseOutputFolderTT As New ToolTip
    Dim BrowseInputSourceTT As New ToolTip
    Dim ViewOutputFolderTT As New ToolTip

    Dim i18nStrings As New Hashtable

    Public SaveSettingsOnExit As Boolean = True
    Public FromCommandLine As Boolean = False

    Public Sub ApplicationStartup()
        If Not ValidateLocalAppPath() Then End
        DeleteLogIfExists()

        ' Check for command line parameters. If so, we can run this application minimized and exit on completion.
        If My.Application.CommandLineArgs.Count > 0 Then
            FromCommandLine = True
            SaveSettingsOnExit = False
        End If

        If Not CheckNeroAAC() Then End

        ' Ensure the MP4 playback feature is enabled
        If My.Settings.MP4PlaybackEnabled Then
            MainUI.EnableMP4PlaybackInMediaPlayerToolStripMenuItem.Checked = True
        End If

        ' Check if the user has chosen not to move output files
        If My.Settings.MoveOutputFile Then
            MainUI.MoveOutputToolStripMenuItem.Checked = My.Settings.MoveOutputFile
        End If

        ' Check registry for MP4 settings
        Dim key As RegistryKey
        key = Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\Multimedia\WMPlayer\Extensions\.mp4")
        If key Is Nothing Then
        Else
            MainUI.EnableMP4PlaybackInMediaPlayerToolStripMenuItem.Checked = True
            My.Settings.MP4PlaybackEnabled = True
            My.Settings.Save()
            key.Close()
        End If

        ' Check if the default output folder is set
        If My.Settings.DefaultOutputFolder <> "" Then
            MainUI.OutputFolder.Text = My.Settings.DefaultOutputFolder
        End If

        ' Set up tooltip text for certain controls
        PreviewMediaTT.SetToolTip(MainUI.PreviewMedia, GetMsg("ToolTip_PreviewMedia_1"))
        BrowseOutputFolderTT.SetToolTip(MainUI.BrowseOutputFolder, GetMsg("ToolTip_BrowseOutputFolder"))
        BrowseInputSourceTT.SetToolTip(MainUI.BrowseSingleFile, GetMsg("ToolTip_BrowseInputSource"))
        ViewOutputFolderTT.SetToolTip(MainUI.ViewOutputFolder, GetMsg("ToolTip_ViewOutputFolder"))

        ' Run i18n
        i18nItUp()

        'Check if UNC paths are enabled and check the box appropriately
        If My.Settings.AllowUNCPathInput Then
            MainUI.AllowNetworkPathToolStripMenuItem.Checked = True
        End If

        If My.Settings.DeleteTempFiles Then
            MainUI.DeleteTemporaryFilesToolStripMenuItem.Checked = True
        Else
            MainUI.DeleteTemporaryFilesToolStripMenuItem.Checked = False
        End If

        ' Window size and position from saved settings
        If My.Settings.MainUIHeight > 0 Then
            MainUI.Height = My.Settings.MainUIHeight
        End If

        If My.Settings.MainUIWidth > 0 Then
            MainUI.Width = My.Settings.MainUIWidth
        End If

        If My.Settings.MainUILeft > -1 Then
            MainUI.Left = My.Settings.MainUILeft
        End If

        If My.Settings.MainUITop > -1 Then
            MainUI.Top = My.Settings.MainUITop
        End If

        MainUI.WindowState = My.Settings.MainUIWindowState
    End Sub

    Public Function GetMsg(ByVal messageIdentifier As String, Optional ByVal messageReplace As String() = Nothing) As String
        If Not My.Computer.FileSystem.FileExists(AppPath & "\i18n\" & My.Settings.i18n) Then
            My.Settings.i18n = DEFAULT_LANGUAGE
            My.Settings.Save()
        End If

        Dim i As Integer, s As String = ""

        If i18nStrings.Count = 0 Then
            Dim msgDoc As New Xml.XmlDocument()
            msgDoc.Load(AppPath & "\i18n\" & My.Settings.i18n)
            Dim stringList As Xml.XmlNodeList = msgDoc.GetElementsByTagName("string")

            For i = 0 To stringList.Count - 1
                i18nStrings.Add(stringList.Item(i).Attributes(0).Value, stringList.Item(i).InnerText)
            Next
        End If

        s = i18nStrings(messageIdentifier)

        If s = "" Then
            Return "!" & messageIdentifier & "!"
        End If

        s = s.Replace("{\n}", vbNewLine)

        If messageReplace Is Nothing Then
            Return s
        End If

        For i = 0 To messageReplace.Length - 1
            s = s.Replace("{" & i & "}", messageReplace(i))
        Next

        Return s
    End Function

    Public Sub DetermineFileSplitState(ByVal filename As String)
        Dim addedFileSize As Long = Math.Floor(My.Computer.FileSystem.GetFileInfo(filename).Length / 1024 / 1024) + 1

        If My.Settings.SplitFiles Then
            If My.Settings.SplitFileThreshold = 0 Then
                My.Settings.SplitFileThreshold = DEFAULT_SPLIT_MB
            End If
            If addedFileSize >= My.Settings.SplitFileThreshold Then
                'MainUI.AddToLog(GetMsg("DetermineFileSplit_Required"))
            End If
        End If
    End Sub

    Public Function StripImageTag(ByVal s As String) As Integer
        If s.Contains("[A]") Then
            s.Replace("[A]", "")
            Return IMG_ACCEPT
        ElseIf s.Contains("[E]") Then
            s.Replace("[E]", "")
            Return IMG_ERROR
        ElseIf s.Contains("[W]") Then
            s.Replace("[W]", "")
            Return IMG_WAIT
        ElseIf s.Contains("[I]") Then
            s.Replace("[I]", "")
            Return IMG_INFO
        ElseIf s.Contains("[S]") Then
            s.Replace("[S]", "")
            Return IMG_STOP
        End If

        Return IMG_INFO
    End Function

    Public Sub i18nItUp()
        With MainUI
            .ConvertSingle.Text = GetMsg("UI_ConvertSingle")
            .ConvertFolder.Text = GetMsg("UI_ConvertFolder")

            .ToolsToolStripMenuItem.Text = GetMsg("UI_ToolsMenu")
            .EnableMP4PlaybackInMediaPlayerToolStripMenuItem.Text = GetMsg("UI_Tools_EnableMP4Playback")
            .OptionsToolStripMenuItem.Text = GetMsg("UI_Tools_Options")
            .MKVInformationToolStripMenuItem.Text = GetMsg("UI_Tools_MKVInformation")

            .HelpToolStripMenuItem.Text = GetMsg("UI_HelpMenu")
            .WebsiteToolStripMenuItem.Text = GetMsg("UI_Help_Website")
            .ForumToolStripMenuItem.Text = GetMsg("UI_Help_Forum")
            .AboutToolStripMenuItem.Text = GetMsg("UI_Help_About")

            .IsTalkative.Text = GetMsg("UI_ShowToolOutput")
            .ScrollLog.Text = GetMsg("UI_ScrollLog")

            .CancelProcess.Text = GetMsg("UI_CancelProcessButton")
            .StartRecode.Text = GetMsg("UI_StartRecodeButton")

            .OutputFolderBrowser.Description = GetMsg("UI_OutputFolderBrowser_Desc")
            .InputFolderBrowser.Description = GetMsg("UI_InputFolderBrowser_Desc")
            .OpenFile.Filter = GetMsg("UI_InputFileBrowser_Filter")

            .Text = GetMsg("UI_Title", New String() {BUILD_VERSION, BUILD_CODENAME})
        End With
    End Sub

    Public Function ConvertSecondsToTime(ByVal s As Long, Optional ByVal forceHours As Boolean = False) As String
        Dim t As New TimeSpan(0, 0, s)
        Dim result As String = ""

        If t.Hours > 0 Then
            If t.Hours < 10 Then
                result = "0" & t.Hours & ":"
            Else
                result = t.Hours & ":"
            End If
        ElseIf forceHours Then
            result = "00:"
        End If

        If t.Minutes > 0 Then
            If t.Minutes < 10 Then
                result = result & "0" & t.Minutes & ":"
            Else
                result = result & t.Minutes & ":"
            End If
        Else
            result = result & "00:"
        End If

        If t.Seconds > 0 Then
            If t.Seconds < 10 Then
                result = result & "0" & t.Seconds
            Else
                result = result & t.Seconds
            End If
        Else
            result = result & "00"
        End If

        Return result
    End Function

    Public Function ConvertTimeToSeconds(ByVal s As String) As Long
        Dim t As TimeSpan
        If s.StartsWith("00:") And s.Length = 5 Then
            s = "00:" & s ' force this to be treated as h:m:s
        End If

        TimeSpan.TryParse(s, t)

        Return Math.Floor(t.TotalSeconds)
    End Function

    Public Sub ProcessCommandLineArgs()
        ' Check if we're running from the command line.
        If UIEvents.FromCommandLine Then
            MainUI.WindowState = FormWindowState.Minimized
            Console.WriteLine(GetMsg("CmdLine_Init"))
            Dim i As Integer
            For i = 0 To My.Application.CommandLineArgs.Count - 1
                Dim s As String = My.Application.CommandLineArgs(i).Trim

                Select Case s
                    Case "-help", "--help"
                        Console.Write(GetMsg("CmdLine_Usage"))
                        End
                    Case "-inputfile"
                        i += 1
                        MainUI.ConvertSingle.Checked = True
                        MainUI.SingleFileName.Text = My.Application.CommandLineArgs(i)
                    Case "-inputfolder"
                        i += 1
                        MainUI.ConvertFolder.Checked = True
                        MainUI.SingleFileName.Text = My.Application.CommandLineArgs(i)
                    Case "-outputfolder"
                        i += 1
                        MainUI.OutputFolder.Text = My.Application.CommandLineArgs(i)
                End Select
            Next

            MainUI.StartRecode_Click(MainUI, Nothing)
        End If
    End Sub

End Module