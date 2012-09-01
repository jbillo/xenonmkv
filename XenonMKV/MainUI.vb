Imports System.Threading
Imports System.Text
Imports System.Globalization
Imports System.IO

Public Class MainUI

#Region "Variable Definitions"
#Region "Processing Variables"
    Dim outputPath As String = AppPath
    Dim progValue As Integer = 0
    Dim logArrayList As New ArrayList
    Dim filesToConvert As New List(Of ConvertItem)

    Dim syncedString As String = ""
#End Region

#Region "Callback MethodInvoker Variables"
    Dim CallFormThreadAdd As New MethodInvoker(AddressOf FormThreadAdd)
    Dim CallProgBarUpdate As New MethodInvoker(AddressOf FormThreadProgUpdate)
    Dim CallProgBarMax As New MethodInvoker(AddressOf FormThreadProgMax)
    Dim CallProgBar100 As New MethodInvoker(AddressOf FormThreadProg100)
    Dim CallProgBar200 As New MethodInvoker(AddressOf FormThreadProg200)
    Dim CallEnableRecode As New MethodInvoker(AddressOf FormThreadEnableRecode)
    Dim CallEnableMKVInfo As New MethodInvoker(AddressOf FormThreadEnableMKVInfo)
    Dim CallDisableCancel As New MethodInvoker(AddressOf FormThreadDisableCancel)
#End Region
#End Region

#Region "Form Thread Native Operations"

    Private Sub FormThreadAdd()
        While logArrayList.Count > 0
            Dim s As String = logArrayList(0) ' catching threading error
            If s <> "" Then
                Dim i As Integer = StripImageTag(s)
                s = RegularExpressions.Regex.Replace(s, "\[(A|E|W|I|S)\]", "")
                Log.Items.Add(s, i)

                Log.Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
            End If
            Try
                logArrayList.RemoveAt(0)
            Catch ex As Exception
                logArrayList.Clear()
            End Try
        End While
        If ScrollLog.Checked Then
            Log.SelectedItems.Clear()
            Log.Items(Log.Items.Count - 1).Selected = True
            Log.Items(Log.Items.Count - 1).Focused = True
            Log.Items(Log.Items.Count - 1).EnsureVisible()
        End If
    End Sub

    Private Sub FormThreadProgUpdate()
        If progValue > ProgressBar.Maximum Then
            ' More accurate estimate of progress, especially for Nero
            If progValue > curItem.AudLength Then
                ProgressBar.Maximum = progValue * 1.5
            Else
                ProgressBar.Maximum = curItem.AudLength
            End If
        End If
        Try
            ProgressBar.Value = progValue
        Catch ex As Exception
            ProgressBar.Maximum = progValue * 2
        End Try
    End Sub

    Private Sub FormThreadProgMax()
        ProgressBar.Value = 0
        ProgressBar.Maximum = curItem.AudLength
    End Sub

    Private Sub FormThreadProg100()
        ProgressBar.Value = 0
        ProgressBar.Maximum = 100
    End Sub

    Private Sub FormThreadProg200()
        ProgressBar.Value = 0
        ProgressBar.Maximum = 200
    End Sub

    Private Sub FormThreadEnableRecode()
        StartRecode.Enabled = True
    End Sub

    Private Sub FormThreadEnableMKVInfo()
        MKVInformationToolStripMenuItem.Enabled = True
    End Sub

    Private Sub FormThreadDisableCancel()
        CancelProcess.Enabled = False
    End Sub

    Private Sub AddToLog(ByVal text As String, Optional ByVal requireVerbose As Boolean = False)
        text = "[" & My.Computer.Clock.LocalTime.ToShortDateString & " " & _
            My.Computer.Clock.LocalTime.ToShortTimeString & "] " & text

        If Not requireVerbose Then
            logArrayList.Add(text)
            BeginInvoke(CallFormThreadAdd)
        End If

        ' Don't log all data if appropriate checkbox not selected
        If requireVerbose And Not IsVerbose Then
            Exit Sub
        End If

        Try
            Dim w As New IO.StreamWriter(New IO.FileStream(AppPath & "\log.txt", FileMode.Append))
            w.WriteLine(text)
            w.Close()
        Catch ex As Exception
        End Try

        If UIEvents.FromCommandLine Then
            Console.WriteLine(text)
        End If
    End Sub

#End Region

#Region "Invocations"

#Region "Progress Bar Invocations"
    Private Sub ProgBarUpdate(ByVal value As Integer)
        progValue = value
        BeginInvoke(CallProgBarUpdate)
    End Sub

    Private Sub ProgBarMax100()
        BeginInvoke(CallProgBar100)
    End Sub

    Private Sub ProgBarMax200()
        BeginInvoke(CallProgBar200)
    End Sub

    Private Sub ProgBarMaxDuration()
        BeginInvoke(CallProgBarMax)
    End Sub
#End Region

#Region "Button and Menu Invocations"
    Private Sub UICallback_EnableRecode()
        BeginInvoke(CallEnableRecode)
    End Sub

    Private Sub UICallback_EnableMKVInfo()
        BeginInvoke(CallEnableMKVInfo)
    End Sub

    Private Sub UICallback_DisableCancel()
        BeginInvoke(CallDisableCancel)
    End Sub
#End Region

#End Region

#Region "UI Code"

    Private Sub ViewOutputFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewOutputFolder.Click
        If My.Computer.FileSystem.DirectoryExists(OutputFolder.Text) Then
            System.Diagnostics.Process.Start(OutputFolder.Text)
        End If
    End Sub

    Private Sub CheckForUpdateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckForUpdateToolStripMenuItem.Click
        Dim localFile As String = AppPath & "\update.txt"
        Try
            My.Computer.Network.DownloadFile(HELP_UPDATE, localFile, "", "", True, 30, True)
        Catch ex As Exception ' this could be a 404
            MsgBox(GetMsg("Update_Failed"), MsgBoxStyle.Exclamation, GetMsg("Update_Failed_Title"))
            Return
        End Try

        If My.Computer.FileSystem.FileExists(localFile) Then
            Dim s As String = My.Computer.FileSystem.ReadAllText(localFile)
            Dim i As Integer
            Int32.TryParse(s, i)
            If i = 0 Then
                ' parse failed
                MsgBox(GetMsg("Update_Failed"), MsgBoxStyle.Exclamation, GetMsg("Update_Failed_Title"))
            ElseIf i > BUILD_REVISION Then
                ' new update available, launch web browser
                ' check if OK with user first
                If MsgBox(GetMsg("Update_Available", New String() {i}), _
                    MsgBoxStyle.YesNo + MsgBoxStyle.Question, _
                    GetMsg("Update_Available_Title")) = MsgBoxResult.Yes Then
                    System.Diagnostics.Process.Start(HELP_DOWNLOAD)
                End If
            ElseIf i <= BUILD_REVISION Then
                ' update not available
                MsgBox(GetMsg("Update_NotAvailable", New String() {i, BUILD_REVISION}), MsgBoxStyle.Information, GetMsg("Update_NotAvailable_Title"))
            End If

            DeleteSingleFile(localFile)
        End If
    End Sub

    Public Sub StartRecode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartRecode.Click
        StartRecodeProcess(ConvertSingle.Checked, SingleFileName.Text, OutputFolder.Text)
    End Sub

    Private Sub IsTalkative_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles IsTalkative.CheckedChanged
        IsVerbose = IsTalkative.Checked
    End Sub

    Private Sub MainUI_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        SafeAbortProcess()
        SafeAbortIOThreads()

        If UIEvents.SaveSettingsOnExit Then
            If Me.WindowState = FormWindowState.Normal Then
                My.Settings.MainUIWidth = Me.Width
                My.Settings.MainUIHeight = Me.Height
                My.Settings.MainUITop = Me.Top
                My.Settings.MainUILeft = Me.Left
            End If

            My.Settings.MainUIWindowState = Me.WindowState

            My.Settings.Save()
        End If
    End Sub

    Private Sub MainUI_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ApplicationStartup()
        AddToLog(GetMsg("Application_Loaded", New String() {BUILD_VERSION, BUILD_CODENAME}))

        UIEvents.ProcessCommandLineArgs()
        ValidateWarningPanel()
    End Sub

    Private Sub BrowseSingleFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrowseSingleFile.Click
        If ConvertSingle.Checked Then
            OpenFile.ShowDialog()
        Else
            If InputFolderBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
                SingleFileName.Text = InputFolderBrowser.SelectedPath
                ValidateWarningPanel()
            End If
        End If
    End Sub

    Private Sub OpenFile_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFile.FileOk
        If OpenFile.FileName = "" Then Exit Sub

        SingleFileName.Text = OpenFile.FileName

        ' Log an event: retrieve file size and determine whether it will be split
        DetermineFileSplitState(SingleFileName.Text)

        ValidateWarningPanel()
    End Sub

    Private Sub ValidateWarningPanel()
        Dim outDrive As String = ""
        If OutputFolder.Text <> "" Then
            outDrive = OutputFolder.Text.Substring(0, 1)
        End If

        WarningPanel.Visible = False

        If (SingleFileName.Text.Contains("\\") Or OutputFolder.Text.Contains("\\")) And _
            Not My.Settings.AllowUNCPathInput Then

            WarningPanel.Visible = True
            WarningText.Text = GetMsg("Error_OutputFolderNetwork")
        ElseIf Not My.Computer.FileSystem.DirectoryExists(OutputFolder.Text) Then
            WarningPanel.Visible = True
            WarningText.Text = GetMsg("Error_OutputFolderInvalid")
        ElseIf ConvertSingle.Checked And Not My.Computer.FileSystem.FileExists(SingleFileName.Text) Then
            WarningPanel.Visible = True
            WarningText.Text = GetMsg("Error_InputFileInvalid")
        ElseIf ConvertFolder.Checked And Not My.Computer.FileSystem.DirectoryExists(SingleFileName.Text) Then
            WarningPanel.Visible = True
            WarningText.Text = GetMsg("Error_InputFolderInvalid")
        ElseIf ConvertSingle.Checked Then
            If Not My.Computer.FileSystem.GetDriveInfo(outDrive).AvailableFreeSpace > _
            My.Computer.FileSystem.GetFileInfo(SingleFileName.Text).Length * 2 _
            And outDrive <> "" Then
                ' Not enough free space on the drive
                WarningPanel.Visible = True
                WarningText.Text = GetMsg("Error_FreeSpaceOutput", New String() {outDrive.ToUpper})
            End If

            If My.Settings.CustomScratchFolder = "" Then
            Else
                If Not EnoughScratchFolderSpace(SingleFileName.Text) Then
                    WarningPanel.Visible = True
                    WarningText.Text = GetMsg("Error_FreeSpaceScratch", New String() {My.Settings.CustomScratchFolder.Substring(0, 1)})
                End If
            End If
        End If

        StartRecode.Enabled = Not WarningPanel.Visible
        If StartRecode.Enabled Then
            My.Settings.DefaultOutputFolder = OutputFolder.Text
            My.Settings.Save()
        End If
    End Sub

    Private Sub BrowseOutputFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrowseOutputFolder.Click
        If OutputFolderBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            OutputFolder.Text = OutputFolderBrowser.SelectedPath
        End If

        ValidateWarningPanel()
    End Sub

    Private Sub ConvertFolder_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertFolder.CheckedChanged
        If ConvertFolder.Checked Then
            SourceType.Text = GetMsg("UI_SourceFolder")
            PreviewMediaTT.SetToolTip(PreviewMedia, GetMsg("ToolTip_PreviewMedia_M"))
        End If

        ValidateWarningPanel()
    End Sub

    Private Sub ConversionSingle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ConvertSingle.CheckedChanged
        If ConvertSingle.Checked Then
            SourceType.Text = GetMsg("UI_SourceSingle")
            PreviewMediaTT.SetToolTip(PreviewMedia, GetMsg("ToolTip_PreviewMedia_1"))
        End If

        ValidateWarningPanel()
    End Sub

    Private Sub OutputFolder_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OutputFolder.TextChanged
        ValidateWarningPanel()
    End Sub

    Private Sub WebsiteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WebsiteToolStripMenuItem.Click
        System.Diagnostics.Process.Start(HELP_WEBSITE)
    End Sub

    Private Sub ForumToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ForumToolStripMenuItem.Click
        System.Diagnostics.Process.Start(HELP_FORUM)
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub

    Private Sub EnableMP4PlaybackInMediaPlayerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EnableMP4PlaybackInMediaPlayerToolStripMenuItem.Click
        ' This is a hack and a half, but should be good enough unless I get some communist that purposely made an x86 directory on a 32bit box.

        If My.Settings.MP4PlaybackEnabled Then
            If MsgBox(GetMsg("UI_MP4SupportAgain"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, GetMsg("UI_MP4SupportAgain_Title")) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        EnableMP4Support_Regedit()

        MsgBox(GetMsg("UI_MP4SupportSuccess"), MsgBoxStyle.Information, GetMsg("UI_MP4SupportSuccess_Title"))
        EnableMP4PlaybackInMediaPlayerToolStripMenuItem.Checked = True
    End Sub

    Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
        OptionsDialog.Show()
    End Sub

    Private Sub SkipToAudioDecodeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Debug_SkipExtract = True
        StartRecode_Click(sender, e)
    End Sub

    Private Sub TrayIcon_BalloonTipClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles TrayIcon.BalloonTipClicked
        Me.Visible = True
        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
    End Sub

    Private Sub TrayIcon_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TrayIcon.MouseDoubleClick
        Me.Visible = True
        Me.ShowInTaskbar = True
        Me.WindowState = FormWindowState.Normal
        Me.Focus()
    End Sub

    Private Sub MainUI_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Visible = False
            Me.ShowInTaskbar = False
        End If
    End Sub

    Private Sub tmrTooltip_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrTooltip.Tick
        If Me.WindowState <> FormWindowState.Minimized Then
            Me.TrayIcon.Visible = False
        End If
        tmrTooltip.Enabled = False
    End Sub

    Private Sub HideShowXenonMKVToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideShowXenonMKVToolStripMenuItem1.Click
        If Me.WindowState = FormWindowState.Minimized Then
            Me.WindowState = FormWindowState.Normal
            Me.ShowInTaskbar = True
            Me.Visible = True
        Else
            Me.WindowState = FormWindowState.Minimized
            Me.ShowInTaskbar = False
            Me.Visible = False
        End If
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub

    Private Sub CancelProcess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelProcess.Click
        SafeAbortProcess()
        SafeAbortIOThreads()
        ProgBarUpdate(0)

        AddToLog(GetMsg("CancelProcess"))

        CancelProcess.Enabled = False
        StartRecode.Enabled = True
        MKVInformationToolStripMenuItem.Enabled = True
        filesToConvert.Clear()

        NextTask = TASK_ABORT
    End Sub

    Private Sub DeleteTemporaryFilesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteTemporaryFilesToolStripMenuItem.Click
        If My.Settings.DeleteTempFiles Then
            DeleteTemporaryFilesToolStripMenuItem.Checked = False
            My.Settings.DeleteTempFiles = False
        Else
            DeleteTemporaryFilesToolStripMenuItem.Checked = True
            My.Settings.DeleteTempFiles = True
        End If

        My.Settings.Save()
    End Sub

    Private Sub PreviewMedia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PreviewMedia.Click
        If My.Computer.FileSystem.DirectoryExists(SingleFileName.Text) Or _
            My.Computer.FileSystem.FileExists(SingleFileName.Text) Then
            System.Diagnostics.Process.Start(SingleFileName.Text)
        End If
    End Sub

    Private Sub MKVInformationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MKVInformationToolStripMenuItem.Click
        If Me.SingleFileName.Text = "" Then
            MsgBox(GetMsg("UI_Tools_MKVInformation_Error"), MsgBoxStyle.Exclamation, GetMsg("UI_Tools_MKVInformation"))
        ElseIf Not My.Computer.FileSystem.FileExists(SingleFileName.Text) Then
            MsgBox(GetMsg("UI_Tools_MKVInformation_Error"), MsgBoxStyle.Exclamation, GetMsg("UI_Tools_MKVInformation"))
        Else
            process = New Process
            With process.StartInfo
                .FileName = AppPath & "\tools\mkvinfo.exe"
                .Arguments = """" & Me.SingleFileName.Text & """"
                .RedirectStandardOutput = True
                .CreateNoWindow = True
                .UseShellExecute = False
                .WorkingDirectory = AppPath
            End With

            StartProcess(process)
            OutputText = ""
            outputThread = New Thread(AddressOf ToolOutputToFile)
            outputThread.IsBackground = False
            outputThread.Start()

            process.WaitForExit()

            ' When complete
            Try
                System.Diagnostics.Process.Start(AppPath & "\temp.txt")
            Catch ex As Exception
            End Try

        End If
    End Sub

    Private Sub AllowNetworkPathToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllowNetworkPathToolStripMenuItem.Click
        AllowNetworkPathToolStripMenuItem.Checked = Not AllowNetworkPathToolStripMenuItem.Checked
        My.Settings.AllowUNCPathInput = AllowNetworkPathToolStripMenuItem.Checked
        My.Settings.Save()
    End Sub

    Private Sub MoveOutputToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MoveOutputToolStripMenuItem.Click
        MoveOutputToolStripMenuItem.Checked = Not MoveOutputToolStripMenuItem.Checked
        My.Settings.MoveOutputFile = MoveOutputToolStripMenuItem.Checked
        My.Settings.Save()
    End Sub

    Private Sub btnOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOptions.Click
        OptionsDialog.Show()
    End Sub
#End Region

#Region "Processing Code"

#Region "Accessors"
#End Region

#Region "Tool Output Parsing"
    Private Sub ToolOutputCapture()
        syncedString = ""

        Dim line As String = process.StandardOutput.ReadLine()
        Try
            Do While Not line Is Nothing
                If line.Length > 0 Then
                    syncedString = syncedString & line

                    If Not line.Contains(vbNewLine) Then
                        syncedString = syncedString & vbNewLine
                    End If
                End If
                line = process.StandardOutput.ReadLine()
            Loop
        Catch ex As Exception
            AddToLog("Exception fired from ToolOutputCapture: " & ex.Message, True)
        End Try
    End Sub


    Private Sub ToolOutputToFile()
        Dim fw As New IO.StreamWriter(New IO.FileStream(AppPath & "\temp.txt", FileMode.Create))
        Dim line As String = process.StandardOutput.ReadLine()
        Try
            Do While line.Length >= 0
                If line.Length > 0 Then
                    fw.WriteLine(line)
                    AddToLog(line, True)
                End If
                line = process.StandardOutput.ReadLine()
            Loop
        Catch ex As Exception
            AddToLog("Exception fired from ToolOutputToFile: " & ex.Message, True)
        End Try

        fw.Flush()
        fw.Close()
    End Sub

    Private Sub ToolOutputDefault()
        Dim line As String = process.StandardOutput.ReadLine()
        Try
            Do While Not line Is Nothing
                If line.Length > 0 Then
                    OutputText += line & vbNewLine
                    AddToLog(line, True)
                End If
                line = process.StandardOutput.ReadLine()
            Loop
            Try
                If process.HasExited Then
                    ' Fire the next event
                    TaskInProgress = False
                    FireNextEvent()
                End If
            Catch pex As Exception
                AddToLog(line, True)
            End Try
        Catch ex As Exception
            Try
                If process.HasExited Then
                    ' Fire the next event
                    TaskInProgress = False
                    FireNextEvent()
                End If
            Catch pex As Exception
                AddToLog(line, True)
            End Try
        End Try
    End Sub

    Private Sub ToolStdErrLine()
        Dim line As String = process.StandardError.ReadLine()
        Try
            Do While line.Length >= 0
                If line.Length > 0 Then
                    OutputText += line & vbNewLine
                    AddToLog(line, True)
                End If
                line = process.StandardError.ReadLine()
            Loop
        Catch ex As Exception
            Try
                If process.HasExited Then
                    ' Fire the next event
                    TaskInProgress = False
                    FireNextEvent()
                End If
            Catch pex As Exception
            End Try
        End Try
    End Sub

    Private Sub ToolStdErrProgress()
        Dim line As String = process.StandardError.ReadLine
        Dim prevLine As String = ""
        Try
            Do While line.Length >= 0
                If line.Length > 0 Then
                    ' Add output to log
                    If line <> prevLine Then
                        AddToLog(line, True)
                        prevLine = line
                    End If

                    If line.Contains("Processed") Then
                        'Using NeroAACEnc
                        line = line.Substring(line.IndexOf(" ") + 1)
                        line = line.Substring(0, line.IndexOf(" "))
                        ProgBarUpdate(CInt(line))
                    ElseIf line.Contains("Frame:") Then
                        ' Using Azid
                        line = line.Substring(line.LastIndexOf("[") + 1)
                        line = line.Substring(0, line.LastIndexOf("."))
                        line = line.Trim()
                        ProgBarUpdate(CInt(line))
                    ElseIf line.Contains("time=") Then
                        ' Using ffmpeg
                        Dim tempPos As String = "time="
                        line = line.Substring(line.IndexOf(tempPos) + tempPos.Length)
                        line = line.Substring(0, line.IndexOf("."))
                        ProgBarUpdate(CInt(line))
                    ElseIf line.Contains("% Frs:") Then
                        ' Using valdec
                        line = line.Substring(0, line.IndexOf("%") - 1)
                        ProgBarUpdate(CInt(line))
                    ElseIf line.Contains("Didn't get subframe DSYNC") And Not MainCycle.flag_DTSFailed Then
                        ' Oops! Log the error 
                        MainCycle.flag_DTSFailed = True
                        AddToLog(GetMsg("Error_DTSDecodeSync"))
                    End If
                End If
                line = process.StandardError.ReadLine
            Loop
        Catch ex As Exception
            Try
                If process.HasExited Then
                    TaskInProgress = False
                    FireNextEvent()
                End If
            Catch pex As Exception
            End Try
        End Try
    End Sub

    Private Sub ToolStdErrChar()
        Dim c As String = Chr(process.StandardError.Read())
        Try
            While c <> ""
                OutputText = OutputText & c
                c = Chr(process.StandardError.Read())
            End While
        Catch ex As Exception
            Try
                If process.HasExited Then
                    If OutputText.Contains("Getting ""Nero Audio Decoder 2"" instance failed.") Then
                        AddToLog(GetMsg("Error_EAC3To_Nero7"))
                        NextTask = TASK_ERROR
                    End If
                    TaskInProgress = False
                    FireNextEvent()
                End If
            Catch pex As Exception
            End Try
        End Try
    End Sub

    Private Sub ToolOutputProgress()
        While process.StartTime = Nothing

        End While

        Dim line As String = process.StandardOutput.ReadLine()
        Dim prevLine As String = ""
        Try
            Do While line.Length >= 0
                If line.Length > 0 Then
                    ' Add output to lines
                    If line <> prevLine Then
                        AddToLog(line, True)
                        prevLine = line
                    End If

                    If line.Contains("progress: ") Then
                        'Using mkvextract or mkvmerge, both of these utils have this seting
                        line = line.Substring(line.IndexOf(": ") + 2)
                        line = line.Substring(0, line.IndexOf("%"))
                        ProgBarUpdate(CInt(line))
                    ElseIf line.Contains("The current packet's timecode is smaller than that of the previous packet") Then
                        'Using mkvmerge, check for synchronization and threading
                        If Not My.Settings.MKVExtractSync And Not flag_needSafeSync Then
                            'The user may not be aware of this setting
                            flag_needSafeSync = True
                            AddToLog(GetMsg("MKVMerge_SafeSync"))
                        End If
                    ElseIf line.Contains("A:") Then
                        'Using mplayer
                        line = line.Substring(line.IndexOf(":") + 1)
                        line = line.Substring(0, line.IndexOf("."))
                        line = line.Trim()
                        ProgBarUpdate(CInt(line))
                    ElseIf line.Contains("Importing AVC-H264") Or line.Contains("ISO File Writing") Then
                        ' Using MP4box
                        line = line.Substring(line.IndexOf("(" + 1))
                        line = line.Substring(0, line.IndexOf("/"))
                        line = line.Trim
                        ProgBarUpdate(CInt(line))
                    ElseIf line.Contains("Audio: no sound") Or _
                     line.Contains("For Avi files, try to force non-interleaved mode with the -ni option") Then
                        'mplayer ruined itself: set up for the fallback task
                        AddToLog(GetMsg("Error_mplayer_AudioFallback"))
                        NextTask = TASK_MPLAYER_FALLBACK1
                        process.Kill()
                    End If
                End If
                line = process.StandardOutput.ReadLine()
            Loop
        Catch ex As Exception
            Try
                If process.HasExited Then
                    ' Fire the next event
                    TaskInProgress = False
                    FireNextEvent()
                End If
            Catch pex As Exception
            End Try
        End Try
    End Sub

    Private Sub ToolOutputChar()
        Dim c As String
        Dim fw As IO.StreamWriter
        If logToolOutput Then
            fw = New IO.StreamWriter(New IO.FileStream(AppPath & "\log_output.txt", FileMode.Create))
        End If

        Try
            c = Chr(process.StandardOutput.Read())
            While c <> ""

                If logToolOutput Then
                    fw.Write(c) ' quit bothering me here
                End If

                OutputText = OutputText & c
                If OutputText.Contains("/100)") Then
                    ' Retrieve the first batch

                    If OutputText.Contains("| (") And OutputText.Contains("/") Then

                        Dim stringStart As Integer = OutputText.IndexOf("| (") + 3
                        OutputText = OutputText.Substring(stringStart) ' fixed for transition period

                        Dim stringEnd As Integer = OutputText.IndexOf("/")
                        OutputText = OutputText.Substring(0, stringEnd)

                        ProgBarUpdate(CInt(OutputText))

                        ' Reset the first p and continue with the fine work
                        AddToLog("Utility progress: " & OutputText & "/100", True)
                        OutputText = ""
                    End If

                    ' Otherwise, sort of ignore this... and keep on going.
                ElseIf OutputText.Contains("--") Then
                    ' Do nothing!
                End If
                c = Chr(process.StandardOutput.Read())
            End While
        Catch ex As Exception
            If process.HasExited Then
                TaskInProgress = False
                fw.Close()
                FireNextEvent()
            End If
        End Try
    End Sub
#End Region

    Private Sub StartRecodeProcess(ByVal convertSingleFile As Boolean, ByVal filePath As String, ByVal initialOutputPath As String)
        ' Initial output path
        outputPath = initialOutputPath
        scratchFolder = My.Settings.CustomScratchFolder

        ' Check regex - people may still have historically bad \temp\ settings
        If Not System.Text.RegularExpressions.Regex.IsMatch(scratchFolder, "^[A-Z]\:\\", RegularExpressions.RegexOptions.IgnoreCase) Then
            My.Settings.CustomScratchFolder = ""
            scratchFolder = ""
            My.Settings.Save()
        End If

        If Not My.Computer.FileSystem.DirectoryExists(scratchFolder) Then
            Try
                My.Computer.FileSystem.CreateDirectory(outputPath & "\temp\")
            Catch ex As Exception ' ignore this, the directory may already exist
            End Try

            scratchFolder = outputPath & "\temp\"

            'Replace any dual slashes in the path with single ones
            outputPath = outputPath.Replace("\\", "\")
            scratchFolder = scratchFolder.Replace("\\", "\")

            AddToLog(GetMsg("Error_InvalidScratch", New String() {scratchFolder}))
        End If

        ' Check if we're outputting the file name in uppercase - if so apply this to the extension
        If My.Settings.OutputUpperCase Then
            My.Settings.OutputExtension = My.Settings.OutputExtension.ToUpper
            My.Settings.Save()
        End If

        ' Enable cancel button
        CancelProcess.Enabled = True

        ' Add file or folder?
        If convertSingleFile Then
            filesToConvert.Clear()
            filesToConvert.Add(New ConvertItem(filePath))
            AddToLog(GetMsg("AddFile_Single", New String() {filePath}))
        Else
            filesToConvert.Clear()
            For Each foundFile As String In _
             My.Computer.FileSystem.GetFiles(filePath, FileIO.SearchOption.SearchTopLevelOnly, "*.mkv")
                ' Add file to list
                filesToConvert.Add(New ConvertItem(foundFile))
                AddToLog(GetMsg("AddFile_FromFolder", New String() {foundFile}))
            Next
        End If

        StartSingleProcess()
    End Sub

    Private Sub StartSingleProcess()
        ClearTrackList()
        curItem = New ConvertItem()
        curItem.EAC3ToHasRun = False
        MainCycle.ResetFlags()

        ' Get current filename
        If filesToConvert.Count > 0 Then
            Dim temp As ConvertItem = filesToConvert.Item(0)

            curItem.FileName = temp.FileName
            curItem.PartNumber = temp.PartNumber
            curItem.OriginalFileName = temp.OriginalFileName
            curItem.EAC3ToHasRun = temp.EAC3ToHasRun
            curItem.DeleteOnFinish = temp.DeleteOnFinish
            curItem.IsSplit = temp.IsSplit
            curItem.VidTrack = temp.VidTrack
            curItem.VidCodec = temp.VidCodec
            curItem.AudCodec = temp.AudCodec
            curItem.AudTrack = temp.AudTrack

            If outputPath.EndsWith("\") Then
                ' Remove the last slash; this utility adds one properly
                outputPath = outputPath.Substring(0, outputPath.Length - 1)
            End If

            If scratchFolder.EndsWith("\") Then
                ' Remove last slash
                scratchFolder = scratchFolder.Substring(0, scratchFolder.Length - 1)
            End If

            outputPath = outputPath & "\"
            scratchFolder = scratchFolder & "\"

            If Not My.Computer.FileSystem.DirectoryExists(scratchFolder) Then
                Try
                    My.Computer.FileSystem.CreateDirectory(scratchFolder)
                Catch ex As Exception
                    AddToLog(GetMsg("Error_CreateTempOutputFolder", New String() {scratchFolder}))
                    NextTask = TASK_ERROR
                    FireNextEvent()
                    Exit Sub
                End Try
            End If

            If My.Computer.FileSystem.FileExists(curItem.FileName) Then
                AddToLog(GetMsg("StartConvert_File", New String() {curItem.FileName}))
                StartRecode.Enabled = False
                MKVInformationToolStripMenuItem.Enabled = False
                If (curItem.PartNumber > 0) Then
                    NextTask = TASK_GETMKV
                Else
                    NextTask = TASK_SPLIT
                End If

                FireNextEvent()
            Else
                AddToLog(GetMsg("Error_FileNotFound", New String() {curItem.FileName}))
                filesToConvert.RemoveAt(0)
                StartSingleProcess()
            End If
        End If
    End Sub

    Private Sub FireNextEvent()
        Try
            If Not MainCycle.process Is Nothing Then
                MainCycle.process.Close()
            End If
        Catch ex As NullReferenceException
            ' Completely ignore this nonsense.
        End Try

        Try
            If flag_Aborted = True Then
                NextTask = TASK_ABORT
            End If

            MainCycle.TaskInProgress = True
            Select Case NextTask
                Case 0
                    logToolOutput = False
                    ' Check if the final file exists:
                    Dim outputFile As String
                    If My.Settings.MP4BoxOutputDirect Then
                        outputFile = outputPath & GetFinalFileName(curItem) & "." & My.Settings.OutputExtension
                    Else
                        outputFile = scratchFolder & "output.mp4"
                    End If

                    ' Check if output file exists
                    If Not My.Computer.FileSystem.FileExists(outputFile) Then
                        ' Check if we used mp4box 0.4.4 this run:
                        If flag_mp4boxFailed Or My.Settings.MP4BoxPath = Constants.MP4BOX_044_PATH Then
                            ' We used mp4box 0.4.4, assume we can't deal with this.
                            AddToLog(GetMsg("Error_MP4Box_Fail2"))
                            ' Fall through to FinishCycle
                        Else
                            ' Drop back to mp4box build 0.4.4
                            flag_mp4boxFailed = True
                            AddToLog(GetMsg("Error_MP4Box_Fail"))
                            My.Settings.MP4BoxPath = Constants.MP4BOX_044_PATH
                            My.Settings.Save()
                            NextTask = TASK_MP4BOX
                            FireNextEvent()
                            Exit Sub
                        End If
                    Else
                        ' Check if we directly output or not. If not, rename/move the file.
                        If Not My.Settings.MP4BoxOutputDirect Then
                            Try
                                DeleteSingleFile(outputPath & GetFinalFileName(curItem) & "." & My.Settings.OutputExtension)
                                My.Computer.FileSystem.MoveFile(scratchFolder & "output.mp4", _
                                    outputPath & GetFinalFileName(curItem) & "." & My.Settings.OutputExtension)
                                AddToLog(GetMsg("Finish_ConversionMoveOK"))
                            Catch e As Exception
                                AddToLog("[E]Exception encountered moving final file: " & e.Message)
                            End Try
                        End If
                    End If

                    FinishCycle()
                Case TASK_SPLIT
                    NextTask = TASK_GETMKV
                    AddToLog(GetMsg("MKVSplit_Start"))
                    If Not Debug_SkipExtract Then
                        StartCheckAndSplit() ' this call threads
                    Else
                        FireNextEvent()
                    End If
                Case TASK_GETMKV
                    NextTask = TASK_PARSEMKV
                    GetMKVTrackInfo(curItem.FileName) ' this call threads
                Case TASK_PARSEMKV
                    NextTask = TASK_EXTRACTMKV
                    ParseMKVTrackInfo(curItem.FileName) ' this call does not thread
                    If curItem.VidTrack <> -1 And curItem.AudTrack <> -1 Then
                        AddToLog(GetMsg("MKVParse_TrackDetails", New String() _
                            {curItem.VidTrack, curItem.VidCodec, curItem.AudTrack, curItem.AudCodec}))
                    End If

                    If My.Settings.CutoffTimePrompt And curItem.IsSplit = False Then
                        VideoCutoffSplit()
                        FireNextEvent()
                    Else
                        FireNextEvent()
                    End If
                Case TASK_EXTRACTMKV
                    NextTask = TASK_HEXEDIT
                    ProgBarMax100()
                    ProgBarUpdate(0)
                    If Not Debug_SkipExtract Then
                        StartMKVExtract(curItem.FileName) ' this call threads
                    Else
                        FireNextEvent()
                    End If
                Case TASK_HEXEDIT
                    ' Determine which audio type was used.
                    ' if it's AC3, then use Azid. Otherwise, mplayer or valdec.
                    If curItem.AudCodec = "" Then
                        NextTask = TASK_ERROR
                        AddToLog(GetMsg("Error_NoAudioCodec"))
                    ElseIf (curItem.AudCodec = ".ac3") Then
                        NextTask = TASK_AZID ' skip audiodump
                    ElseIf curItem.AudCodec = ".dts" Then
                        ' use valdec (rev 16+)
                        NextTask = TASK_VALDEC ' skip audiodump
                    Else
                        NextTask = TASK_MPLAYER ' now skipping audiodump
                    End If
                    ProgBarUpdate(0)
                    StartBastardHexEdit(scratchFolder & "video" & curItem.VidCodec) ' this call does not thread, but fires
                Case TASK_AUDIODUMP
                    ' Skip this task, it's not used anymore
                Case TASK_MPLAYER
                    NextTask = TASK_NEROAAC
                    ProgBarMaxDuration()
                    StartMPlayerEnc() ' this call threads
                Case TASK_AZID
                    NextTask = TASK_NEROAAC
                    StartAzid() ' this call threads
                Case TASK_FFMPEG
                    ProgBarMaxDuration()
                    NextTask = TASK_NEROAAC
                    StartFFmpeg() ' this call threads
                Case TASK_VALDEC
                    ProgBarMax100()
                    NextTask = TASK_NEROAAC
                    StartValdec() ' this call threads
                Case TASK_MPLAYER_FALLBACK1
                    If curItem.EAC3ToHasRun Then
                        AddToLog(GetMsg("Error_FFmpeg_Fail"))
                        NextTask = TASK_ERROR
                        FireNextEvent()
                    Else
                        ' Start ffmpeg instead of eac3to as it's probably better equipped to handle things
                        ProgBarMaxDuration()
                        NextTask = TASK_NEROAAC
                        curItem.EAC3ToHasRun = True
                        StartFFmpeg() ' this call threads
                    End If
                Case TASK_NEROAAC
                    ' Check if the audio file is zero bytes: if so, just fail now
                    If Not My.Computer.FileSystem.FileExists(scratchFolder & "audiodump.wav") Or _
                     My.Computer.FileSystem.GetFileInfo(scratchFolder & "audiodump.wav").Length = 0 Then
                        AddToLog(GetMsg("Error_AudioFallback"))
                        ' Start FFmpeg
                        NextTask = TASK_NEROAAC
                        ProgBarMaxDuration()
                        StartFFmpeg()
                    Else
                        NextTask = TASK_MP4BOX
                        ProgBarMaxDuration()
                        ProgBarUpdate(0)
                        StartNeroAACEnc() ' this call threads
                    End If
                Case TASK_MP4BOX
                    ProgBarUpdate(0)
                    ProgBarMax100()
                    NextTask = 0 ' skip to last task
                    StartMP4Box() ' this call threads
                Case TASK_ERROR
                    AddToLog(GetMsg("Error_GeneralConversion"))
                    ProgBarUpdate(0)
                    ErrorOut()
                Case TASK_ABORT
                    logArrayList.Clear()
                    Exit Sub
            End Select
        Catch ex As Exception
            AddToLog("[E]" & NextTask & " - Exception fired: " & ex.Message)
        End Try
    End Sub

    Private Sub VideoCutoffSplit()
        Dim st As New SelectTime()
        If st.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ' grab time and split
            Dim splitTime As Long = st.selectedTime

            If st.DontSplit.Checked Then
                ' Don't modify the process; just change the setting
                My.Settings.CutoffTimePrompt = False
                My.Settings.SplitFiles = False
                My.Settings.SplitFileThreshold = DEFAULT_SPLIT_MB
                My.Settings.Save()
                Exit Sub
            End If

            st.Close()
            st.Dispose()

            If splitTime > curItem.AudLength Or splitTime < 1 Then
                NextTask = TASK_ERROR
                AddToLog(GetMsg("MKVSplit_Cutoff_Cancel"))
                FireNextEvent()
                Exit Sub
            End If

            ' Perform the split process

            AddToLog(GetMsg("MKVSplit_Cutoff_Start", New String() {splitTime}))

            process = New Process
            With process.StartInfo
                .FileName = AppPath & "\tools\mkvmerge.exe"
                .WorkingDirectory = scratchFolder
                .Arguments = "-o """ & scratchFolder & "part.mkv"" -S -B """ & curItem.FileName & """ --split " & ConvertSecondsToTime(splitTime, True)
                .UseShellExecute = False
                .CreateNoWindow = True
                .RedirectStandardOutput = True
            End With

            curItem.OriginalFileName = My.Computer.FileSystem.GetName(curItem.FileName)
            curItem.FileName = scratchFolder & "part-001.mkv"
            curItem.IsSplit = True
            curItem.PartNumber = -1

            ' Start the process
            StartProcess(process)

            '' Start the output thread to monitor state changes and progress: this will thread so don't push it
            outputThread = New Thread(AddressOf ToolOutputProgress)
            outputThread.IsBackground = True
            outputThread.Start()

            process.WaitForExit()

        Else
            NextTask = TASK_ERROR
            AddToLog(GetMsg("MKVSplit_Cutoff_Cancel"))
        End If
    End Sub

    Private Sub StartCheckAndSplit()
        ' First, remove any temporary audio/video processing files.
        DeleteTempConversionFiles(curItem, scratchFolder)

        ' Determine if a split even needs to occur based on user settings.
        If My.Settings.SplitFiles = False Then
            ' Determine if the user has cut it off.
            AddToLog(GetMsg("MKVSplit_UserDeny", New String() {My.Computer.FileSystem.GetName(curItem.FileName)}))
            FireNextEvent()
            Exit Sub
        End If

        ' Before even splitting the file, run the synchronized process to determine reference frames.
        ' Check this file for Xbox 360 compatibility:
        If ReferenceFramesExceeded(GetMediaInfo(curItem.FileName), GetMKVInfo(curItem.FileName)) Then
            If My.Settings.HaltOnIncompatible = True Then
                AddToLog(GetMsg("Error_ReferenceFramesExceeded"))
                NextTask = TASK_ERROR
                FireNextEvent()
                Return
            Else
                AddToLog(GetMsg("Warning_ReferenceFramesExceeded"))
            End If
        End If

        ' Determine exactly when we call a split, since we know it's occurring.
        Dim splitThreshold As Long = My.Settings.SplitFileThreshold
        If splitThreshold <= 0 Then
            splitThreshold = DEFAULT_SPLIT_MB
        End If

        ' splitTH is currently in MB: multiply by 1024 twice
        splitThreshold = splitThreshold * 1024 ' splitTH is now in KB
        splitThreshold = splitThreshold * 1024 ' splitTH is now in bytes

        ' Determine current file mode. Are we converting a folder or single file?
        ' converting single file: create N conversion items if >4GB
        Dim fileSize As Long = My.Computer.FileSystem.GetFileInfo(curItem.FileName).Length
        AddToLog(GetMsg("MKVSplit_FileSize", New String() _
         {My.Computer.FileSystem.GetName(curItem.FileName), Math.Floor(fileSize / 1024 / 1024) + 1}))

        Dim oldFilePath As String = curItem.FileName

        If fileSize > splitThreshold Then
            ' split required
            ' Lucky Build Number 8: Determine how many splits are required.
            Dim i As Integer, numPieces As Integer = 1, splitSize As Long
            For i = 2 To My.Settings.ArbitraryMaxSplits
                splitSize = fileSize \ i
                If splitSize < splitThreshold Then
                    numPieces = i
                    Exit For
                End If
            Next

            Dim safeSplitSize As Long = Math.Floor(splitSize / 1024 / 1024) + 1 ' 1 extra MB should do the trick for a buffer zone

            process = New Process
            With process.StartInfo
                .FileName = AppPath & "\tools\mkvmerge.exe"
                .WorkingDirectory = scratchFolder
                .Arguments = "-o """ & scratchFolder & "part.mkv"" -S -B """ & curItem.FileName & """ --split " & safeSplitSize & "M"
                If My.Settings.MKVExtractSync Then
                    .Arguments = .Arguments & " --sync"
                End If
                .UseShellExecute = False
                .CreateNoWindow = True
                AddToLog("Launching: " & .FileName & " " & .Arguments, True)
                .RedirectStandardOutput = True
            End With

            ' We know how many pieces will be generated and what their filenames will be.
            ' Replace the current piece position and filename with the appropriate one.

            Dim pieceGUID As String = System.Guid.NewGuid.ToString

            curItem.OriginalFileName = My.Computer.FileSystem.GetName(curItem.FileName)
            ' curItem.OriginalFileName = curItem.OriginalFileName.Substring(0, curItem.OriginalFileName.LastIndexOf("."))
            curItem.FileName = scratchFolder & "part-001.mkv"
            curItem.PartNumber = 1
            curItem.IsSplit = True
            curItem.GUID = pieceGUID

            ' Create a new ArrayList and set up the objects. 
            Dim tmpNewFileList As New List(Of ConvertItem)
            For i = 1 To numPieces
                Dim padNum As String
                If i < 10 Then
                    padNum = "00" & i
                ElseIf i < 100 Then
                    padNum = "0" & i
                Else
                    padNum = i
                End If
                Dim ci As New ConvertItem(scratchFolder & "part-" & padNum & ".mkv", True)
                ci.GUID = pieceGUID
                ci.PartNumber = i
                ci.OriginalFileName = curItem.OriginalFileName
                tmpNewFileList.Add(ci)
            Next

            ' Remove the first item from the ArrayList.
            filesToConvert.RemoveAt(0)

            ' Merge the current ArrayList with the existing one.
            tmpNewFileList.AddRange(filesToConvert)
            filesToConvert = tmpNewFileList

            ' Set progress bar up for percentage
            ProgBarMax100()

            ' Start the process
            StartProcess(process)

            ' Start the output thread to monitor state changes and progress: this will thread so don't push it
            outputThread = New Thread(AddressOf ToolOutputProgress)
            outputThread.IsBackground = True
            outputThread.Start()

            AddToLog(GetMsg("MKVSplit_SplitFilePieces", New String() {numPieces, Math.Floor(splitSize / 1024 / 1024)}))
        Else
            AddToLog(GetMsg("MKVSplit_NoSplitThreshold", New String() {My.Settings.SplitFileThreshold}))
        End If

        curItem.EAC3ToHasRun = False

        If fileSize <= splitThreshold Then
            ' The call does not thread if there is no split.
            ' Fire next event.
            FireNextEvent()
        End If
    End Sub

    Private Sub GetMKVTrackInfo(ByVal mkvFile As String)
        Dim toolPath As String = AppPath & "\tools\mkvinfo.exe"

        process = New Process
        With process.StartInfo
            .FileName = toolPath
            .Arguments = """" & mkvFile & """"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardOutput = True
            .WorkingDirectory = scratchFolder
        End With

        StartProcess(process)
        outputThread = New Thread(AddressOf ToolOutputDefault)
        outputThread.IsBackground = True
        outputThread.Start()

        AddToLog(GetMsg("MKVInfo_Start"))
    End Sub

    Private Sub ParseMKVTrackInfo(ByVal mkvfile As String)
        ' Take the output text and determine which track is the video and 
        ' which track is the audio.

        Dim detectString As String = "| + A track"

        Dim trackInfo As String = OutputText
        Dim totalTrackInfo As String = OutputText
        If trackInfo.Contains(detectString) Then

            ' This has been moved out because output from this tool is not consistent
            GetAudioDuration(totalTrackInfo)

            ' Now retrieve all the track info segments and look for the first video and audio segment.
            While trackInfo.Contains(detectString)
                If trackInfo.Contains(detectString) Then
                    trackInfo = trackInfo.Substring(trackInfo.IndexOf(detectString) + detectString.Length)
                Else
                    Exit While
                End If

                Dim mediaInfo As String = GetMediaInfo(mkvfile)

                Dim tempTrackInfo As String = trackInfo
                Dim trackType As String = GetTrackType(tempTrackInfo)
                Dim trackNumber As Integer = GetTrackNumber(tempTrackInfo)

                ' Check this file for Xbox 360 compatibility:
                If ReferenceFramesExceeded(mediaInfo, tempTrackInfo) Then
                    If My.Settings.HaltOnIncompatible = True Then
                        AddToLog(GetMsg("Error_ReferenceFramesExceeded"))
                        NextTask = TASK_ERROR
                        Return
                    Else
                        AddToLog(GetMsg("Warning_ReferenceFramesExceeded"))
                    End If
                End If

                If trackType = "video" Then
                    Dim vt As New VideoTrack
                    vt.ID = trackNumber
                    vt.Type = trackType
                    vt.Language = GetLanguage(tempTrackInfo)
                    Double.TryParse(GetVideoFPS(tempTrackInfo), vt.FPS)
                    If vt.FPS = 0 Then vt.FPS = Constants.DEFAULT_FPS
                    vt.CodecID = GetCodecID(tempTrackInfo)
                    vt.DefaultTrack = GetTrackIsDefault(tempTrackInfo)
                    vt.PixelAspectRatio = GetPixelAspectRatio(tempTrackInfo, mkvfile)
                    'war59312's hack (Replace FPS value with mediainfo's FPS Value instead of mkvinfo's FPS value for the video track FPS)
                    vt.FPS = GetMediaInfoFPS(tempTrackInfo, mkvfile)
                    AddToLog(GetMsg("MKVParse_VideoPAR", New String() {vt.PixelAspectRatio}))
                    TrackList.Add(vt)
                ElseIf trackType = "audio" Then
                    Dim at As New AudioTrack
                    at.ID = trackNumber
                    at.Type = trackType
                    at.CodecID = GetCodecID(tempTrackInfo)
                    at.Language = GetLanguage(tempTrackInfo)
                    ' Double.TryParse(GetVideoFPS(tempTrackInfo), at.FPS)
                    ' If at.FPS = 0 Then at.FPS = Constants.DEFAULT_FPS
                    at.DefaultTrack = GetTrackIsDefault(tempTrackInfo)
                    TrackList.Add(at)
                End If
            End While

            AddToLog(GetMsg("MKVParse_AllTracksAdded"))

            Debug.WriteLine(curItem.VidTrack)

            If curItem.VidTrack > -1 And curItem.AudTrack > -1 Then
                ' The current item is part of a multi-part set. Inherit its PAR and FPS only.
                AddToLog(GetMsg("MKVParse_MultiMKVSet"))
                curItem.FPS = GetTrackByID(curItem.VidTrack).FPS
                Dim tempVidTrack As VideoTrack = TryCast(GetTrackByID(curItem.VidTrack), VideoTrack)
                If tempVidTrack Is Nothing Then
                Else
                    curItem.PAR = tempVidTrack.PixelAspectRatio
                End If
                Exit Sub
            ElseIf My.Settings.AutoPickTracks Then
                ' Automatically picking tracks as per user preference.
                AddToLog(GetMsg("MKVParse_AutoTrackSel"))
                AutoPickTracks()
                Exit Sub
            End If

            Dim multiTracks As Boolean = MultiAVTracks()

            If multiTracks Then
                AddToLog(GetMsg("MKVParse_MultiAVTracks"))
                Dim st As New SelectTrack
                If st.ShowDialog() = Windows.Forms.DialogResult.OK Then
                    ' Add these tracks to all others in the current filesplit set.
                    Dim i As Integer, curGUID As String = curItem.GUID
                    For i = 0 To filesToConvert.Count - 1
                        If filesToConvert(i).GUID = curGUID Then
                            filesToConvert(i).VidTrack = curItem.VidTrack
                            filesToConvert(i).AudTrack = curItem.AudTrack
                            filesToConvert(i).AudCodec = curItem.AudCodec
                            filesToConvert(i).VidCodec = curItem.VidCodec
                        End If
                    Next
                Else
                    AddToLog(GetMsg("Error_TrackSelCancel"))
                End If
                st.Dispose()
            Else
                AddToLog(GetMsg("MKVParse_SingleAVTrack"))
                Dim i As Integer
                For i = 0 To TrackList.Count - 1
                    If TrackList.Item(i).Type = "video" Then
                        Dim tempVideoTrack As VideoTrack = TryCast(TrackList.Item(i), VideoTrack)
                        curItem.VidTrack = tempVideoTrack.ID
                        curItem.FPS = tempVideoTrack.FPS
                        curItem.VidCodec = tempVideoTrack.CodecID
                        curItem.PAR = tempVideoTrack.PixelAspectRatio
                    ElseIf TrackList.Item(i).Type = "audio" Then
                        curItem.AudTrack = TrackList.Item(i).ID
                        curItem.AudCodec = TrackList.Item(i).CodecID
                    End If
                Next
            End If

            If curItem.VidTrack = -1 And curItem.AudTrack = -1 Then
                NextTask = TASK_ERROR
                Return
            End If

            If (curItem.VidTrack < 0 Or curItem.AudTrack < 0) Then
                AddToLog(GetMsg("Error_NoAVID", New String() {curItem.AudTrack, curItem.VidTrack}))
                NextTask = TASK_ERROR
            End If

        Else
            AddToLog(GetMsg("Error_NoAVID", New String() {curItem.AudTrack, curItem.VidTrack}))
            NextTask = TASK_ERROR
        End If

    End Sub

    Private Sub GetAudioDuration(ByVal s As String)
        ' This is under "Segment information"
        Dim d As String = "| + Duration:"

        s = s.Substring(s.IndexOf(d) + d.Length)

        ' Retrieve the length of the MKV file audio and video
        If s.IndexOf(".") > 0 Then
            curItem.AudLength = CInt(s.Substring(0, s.IndexOf("."))) ' returns integer of audio duration in seconds
            curItem.AudLength += 1 ' assuming that the decimal part is not zero; this number is really only needed for progress calculations
        Else
            curItem.AudLength = CInt(s)
        End If

        AddToLog(GetMsg("MKVParse_Length", New String() {curItem.AudLength}))
    End Sub

    Public Function GetMediaInfo(ByVal mkvFile As String) As String
        process = New Process
        With process.StartInfo
            .FileName = AppPath & "\tools\mediainfo\mediainfo.exe"
            .Arguments = """" & mkvFile & """"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardOutput = True
        End With

        StartProcess(process)

        outputThread = New Thread(AddressOf ToolOutputCapture)
        outputThread.IsBackground = True
        outputThread.Start()

        process.WaitForExit()

        Return syncedString
    End Function

    Private Function GetMKVInfo(ByVal mkvFile As String) As String
        process = New Process
        With process.StartInfo
            .FileName = AppPath & "\tools\mkvinfo.exe"
            .Arguments = """" & mkvFile & """"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardOutput = True
        End With

        StartProcess(process)

        outputThread = New Thread(AddressOf ToolOutputCapture)
        outputThread.IsBackground = True
        outputThread.Start()

        process.WaitForExit()

        Return syncedString
    End Function

#Region "Video Dimension Getters"
    Private Function GetVideoWidth(ByVal mkvInfoOutput As String) As Integer
        Dim s As String
        Dim t As String = "+ Pixel width: "
        If mkvInfoOutput.Contains(t) Then
            s = mkvInfoOutput.Substring(mkvInfoOutput.IndexOf(t) + t.Length)
            s = s.Substring(0, s.IndexOf(vbNewLine))
            Return CInt(s)
        End If

        Return 0
    End Function

    Private Function GetVideoHeight(ByVal mkvInfoOutput As String) As Integer
        Dim s As String
        Dim t As String = "+ Pixel height: "
        If mkvInfoOutput.Contains(t) Then
            s = mkvInfoOutput.Substring(mkvInfoOutput.IndexOf(t) + t.Length)
            s = s.Substring(0, s.IndexOf(vbNewLine))
            Return CInt(s)
        End If

        Return 0
    End Function
#End Region

    Private Function ReferenceFramesExceeded(ByVal mediaInfo As String, ByVal mkvInfoOutput As String) As Boolean
        Dim width As Integer, height As Integer, refFrames As Integer

        width = GetVideoWidth(mkvInfoOutput)
        height = GetVideoHeight(mkvInfoOutput)

        ' Using the custom table at: http://www.avsforum.com/avs-vb/showthread.php?t=972503
        ' we can determine the maximum number of reference frames for the frame.

        refFrames = GetReferenceFrames(mediaInfo)

        ' The frame table function errs on the side of caution: if you have a weird resolution,
        ' it will just continue without issue.
        Return ReferenceFrameTable(width, height, refFrames)

    End Function

    Private Function GetPixelAspectRatio(ByVal s As String, ByVal mkvfile As String) As String
        ' This function is rather crafty because it requires a separate call to mediainfo to find out the appropriate A/R.
        Dim orig As String = s
        Dim t As String
        Dim height As Integer, width As Integer, dar As Double
        width = GetVideoWidth(s)
        height = GetVideoHeight(s)

        AddToLog(GetMsg("MKVParse_VideoDimensions", New String() {width, height}))

        ' Now we have the height and width: call mediainfo.exe to see what we can get
        process = New Process
        With process.StartInfo
            .FileName = AppPath & "\tools\mediainfo\mediainfo.exe"
            .Arguments = """" & mkvfile & """"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardOutput = True
        End With

        StartProcess(process)

        outputThread = New Thread(AddressOf ToolOutputCapture)
        outputThread.IsBackground = True
        outputThread.Start()

        process.WaitForExit()

        ' Once the process is done, snag the aspect ratio from the output
        t = "Display Aspect ratio"
        s = syncedString

        If s.Contains(t) Then

            s = s.Substring(s.IndexOf(t) + t.Length)
            s = s.Substring(s.IndexOf(": ") + 2)
            s = s.Substring(0, s.IndexOf(vbNewLine))

            AddToLog(GetMsg("MKVParse_VideoDAR", New String() {s}))

            If s.Contains("16/9") Then
                s = "1.778"
            ElseIf s.Contains("4/3") Then
                s = "1.333"
            ElseIf s.Contains("/") Then
                Dim j, k As String
                j = s.Substring(0, s.IndexOf("/"))
                k = s.Substring(j.Length)
                Try
                    s = Math.Round(Int32.Parse(j) / Int32.Parse(k), 3).ToString
                Catch ex As Exception
                    AddToLog(GetMsg("Error_ParseAR", New String() {s}))
                    NextTask = TASK_ERROR
                End Try
            End If

            s = s.Trim()

            If Not Double.TryParse(s, dar) Then
                ' Display aspect ratio conversion failed; log this and return 1:1 default
                AddToLog(GetMsg("Error_ConvertAR", New String() {s}))
                Return "1:1"
            End If
        End If

        If My.Settings.ProvideCustomAR Then
            Dim setAR As New SetAspectRatio
            setAR.Description.Text = GetMsg("UI_ProvideARDesc", New String() {dar})
            setAR.AutoAR = dar
            If setAR.ShowDialog() = Windows.Forms.DialogResult.OK Then
                If dar <> setAR.AR Then
                    AddToLog(GetMsg("MKVParse_CustomARSet", New String() {setAR.AR}))
                End If
                dar = Math.Round(setAR.AR, 3)
            End If
            ' Otherwise, continue with default settings
        End If

        ' Now, take the height and width, and get a value
        Dim tempHeight As Long = height * dar
        Dim tempWidth As Long = width
        Dim theGCD As Long = Gcd(tempHeight, tempWidth)

        If theGCD = 0 Then
            ' Just make PAR 1:1
            tempHeight = 1
            tempWidth = 1
            theGCD = 1
        Else
            tempHeight = tempHeight / theGCD
            tempWidth = tempWidth / theGCD
        End If

        While tempHeight > 1000 Or tempWidth > 1000
            tempHeight = tempHeight / 10
            tempWidth = tempWidth / 10
        End While

        ' Round pixel aspect ratio to 1:1 if setting is changed
        If My.Settings.RoundPAR Then
            If (tempHeight / tempWidth) > 0.98 And (tempHeight / tempWidth) < 1 Then
                AddToLog(GetMsg("MKVParse_VideoPARRoundUp", New String() {tempHeight & ":" & tempWidth}))
                tempHeight = 1
                tempWidth = 1
            ElseIf (tempHeight / tempWidth) < 1.01 And (tempHeight / tempWidth) > 1 Then
                AddToLog(GetMsg("MKVParse_VideoPARRoundDown", New String() {tempHeight & ":" & tempWidth}))
                tempHeight = 1
                tempWidth = 1
            End If
        End If

        Return (tempHeight) & ":" & (tempWidth)

    End Function

    'war59312's hack (Use mediainfo to determine the FPS Value for the current video track)
    Private Function GetMediaInfoFPS(ByVal s As String, ByVal mkvfile As String) As String

        'I'm looking only for the FPS value, so let's name what I am looking for as frameRate
        Dim frameRate As String

        'MediaInfo names the FPS value as "Frame rate" in its Output
        frameRate = "Frame rate"

        'Not sure what this does at the moment, only that it's requried
        s = syncedString

        'Find the first instance of frameRate in MediaInfo's Output
        s = s.Substring(s.IndexOf(frameRate) + frameRate.Length)

        'MediaInfo uses ":" to separate the FPS name and its value, We only need the value that follows the first ":"
        s = s.Substring(s.IndexOf(": ") + 2)

        'Store the entire FPS value as a string, this includes the "fps" text at the end
        s = s.Substring(0, s.IndexOf(vbNewLine))

        'frameRate was stored as a string, but we need FPS as a double so let's store the correct FPS as fps
        Dim fps As Double

        'Drop the "fps" ending text part off the double, we need only the numbers
        fps = s.Replace(" fps", "")

        'Returns the correct FPS as a double from medainfo for later use with MP4BoX
        Return fps

    End Function

    Private Function GetCodecID(ByVal s As String) As String
        Dim t As String = "|  + Codec ID: "
        If s.Contains(t) Then
            s = s.Substring(s.IndexOf(t) + t.Length)
            If s.IndexOf(vbNewLine) > 0 Then
                s = s.Substring(0, s.IndexOf(vbNewLine))
            Else
                s = s.Substring(0, 5)
            End If
            AddToLog(GetMsg("MKVParse_CodecID", New String() {s}))
            Return ExtractCodec(s)
        Else
            Return 0
        End If
    End Function

    Public Function ExtractCodec(ByVal s As String) As String
        If (s.Contains("A_AC3")) Then
            Return ".ac3"
        ElseIf s.Contains("A_AAC") Then
            Return ".aac"
        ElseIf s.Contains("A_DTS") Then
            Return ".dts"
        ElseIf s.Contains("A_MP3") Or s.Contains("A_MPEG/L3") Or s.Contains("A_MS/ACM") Then
            ' Treating A_MS/ACM as MP3 for random anime
            Return ".mp3"
        ElseIf s.Contains("A_VORBIS") Then
            Return ".ogg"
        ElseIf s.Contains("V_MS/VFW/FOURCC") Then
            Return ".avi"
        ElseIf s.Contains("V_MPEG4/ISO/AVC") Then
            Return ".h264"
        ElseIf s.Contains("V_MPEG2") Then
            Return ".mpg"
        ElseIf s.Contains("V_") Then
            Return ".avi" ' default video
        Else
            AddToLog(GetMsg("Error_UnknownCodec", New String() {s}))
            NextTask = TASK_ERROR
            Return ".ac3"
        End If
    End Function

    Private Sub StartProcess(ByRef p As Process)
        ' Log the execution path to the file.
        AddToLog("[W]" & p.StartInfo.FileName & " " & p.StartInfo.Arguments, True)

        ' Check if the process actually exists and log if it doesn't.
        If Not My.Computer.FileSystem.FileExists(p.StartInfo.FileName) Then
            AddToLog(GetMsg("Error_ProcessStartFNF", New String() {p.StartInfo.FileName}))
            NextTask = TASK_ERROR
            Exit Sub
        End If

        p.Start()
    End Sub

    Private Sub StartMKVExtract(ByVal filename As String)
        process = New Process
        With process.StartInfo
            .FileName = AppPath & "\tools\mkvextract.exe"
            .Arguments = "tracks """ & filename & """ " & curItem.VidTrack & ":video" & curItem.VidCodec & " " & _
                curItem.AudTrack & ":audio" & curItem.AudCodec
            .CreateNoWindow = True
            .RedirectStandardOutput = True
            .UseShellExecute = False
            .WorkingDirectory = scratchFolder
        End With

        AddToLog(GetMsg("MKVExtract_Start"))
        StartProcess(process)

        OutputText = ""
        outputThread = New Thread(AddressOf ToolOutputProgress)
        outputThread.IsBackground = True
        outputThread.Start()
    End Sub

    Private Sub StartBastardHexEdit(ByVal filename As String)
        ' Bastard hex edit byte 7 of this file and change it
        ' from [67 64 00] 33 to [67 64 00] 29 (5.1 to 4.1 audio) - if it's a H264 file.

        If curItem.VidCodec <> ".h264" Then
            FireNextEvent()
            Exit Sub
        End If

        Try
            Dim w As New IO.BinaryWriter(New IO.FileStream(filename, IO.FileMode.Open))
            w.Seek(7, IO.SeekOrigin.Begin) ' 8th position
            w.Write(Byte.Parse("41")) ' 29 in hex
            w.Close()

            AddToLog(GetMsg("HexEdit_Success"))
            TaskInProgress = False
        Catch ex_fnf As FileNotFoundException
            AddToLog(GetMsg("Error_HexEditFNF"))
            NextTask = TASK_ERROR
            TaskInProgress = False
        Catch ex_ro As UnauthorizedAccessException
            AddToLog(GetMsg("Error_HexEditAccess"))
            NextTask = TASK_ERROR
            TaskInProgress = False
        Catch ex_2 As IO.IOException
            AddToLog(GetMsg("Error_HexEditIO"))
            AddToLog(GetMsg("Error_ExceptionFollows") & ex_2.Message)
            NextTask = TASK_ERROR
            TaskInProgress = False
        Catch ex As Exception
            ' Could not find the file since something broke. :(
            AddToLog(ex.GetType.ToString & " / " & ex.Message)
            AddToLog(GetMsg("Error_HexEditFail"))
            If Not Debug_SkipExtract Then
                NextTask = TASK_ERROR
                TaskInProgress = False
            End If
        End Try

        FireNextEvent()
    End Sub

    Private Sub StartAzid()
        process = New Process
        With process.StartInfo
            .WorkingDirectory = scratchFolder
            .FileName = AppPath & "\tools\azid\azid.exe"

            ' Add Azid settings: Normalize
            If My.Settings.AzidNormalizeAudio Then
                .Arguments = "-a "
            Else
                .Arguments = ""
            End If

            ' DRC
            .Arguments = .Arguments & "-c " & My.Settings.AzidDRC & " "

            ' Filter
            If My.Settings.AzidFilterRearChannels Then
                .Arguments = .Arguments & "-f1 "
            End If

            ' Stereo downmix
            If My.Settings.AzidStereoDownmix <> "" Then
                .Arguments = .Arguments & "-s " & My.Settings.AzidStereoDownmix & " "
            End If

            .Arguments = .Arguments & "audio.ac3 audiodump.wav"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardError = True
        End With

        If My.Settings.AzidNormalizeAudio Then
            ProgBarMax200()
        Else
            ProgBarMax100()
        End If

        StartProcess(process)

        OutputText = ""
        outputThread = New Thread(AddressOf ToolStdErrProgress)
        outputThread.IsBackground = True
        outputThread.Start()

        AddToLog(GetMsg("Azid_Start"))
    End Sub

    Private Sub StartFFmpeg()
        process = New Process
        With process.StartInfo
            .WorkingDirectory = scratchFolder
            .FileName = AppPath & "\tools\FFmpeg\ffmpeg.exe"
            .Arguments = "-async 1 -i audio" & curItem.AudCodec & " -r " & curItem.FPS & " -y -vn -ac 2 audiodump.wav"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardError = True
        End With

        StartProcess(process)
        errorThread = New Thread(AddressOf ToolStdErrProgress)
        errorThread.IsBackground = True
        errorThread.Start()

        AddToLog(GetMsg("FFmpeg_Start"))
    End Sub

    Private Sub StartValdec()
        process = New Process
        With process.StartInfo
            .WorkingDirectory = scratchFolder
            .FileName = AppPath & "\tools\ac3filter\valdec.exe"
            .Arguments = "audio" & curItem.AudCodec & " -spk:2 -normalize+ -wav audiodump.wav"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardError = True
        End With

        StartProcess(process)
        errorThread = New Thread(AddressOf ToolStdErrProgress)
        errorThread.IsBackground = True
        errorThread.Start()

        AddToLog(GetMsg("Valdec_Start"))
    End Sub

    Private Sub StartMPlayerEnc()
        process = New Process
        With process.StartInfo
            .FileName = AppPath & "\tools\mplayer\mplayer.exe"
            .Arguments = "audio" & curItem.AudCodec & " " & _
            "-vc null -vo null -channels 2 -ao pcm:fast"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardOutput = True
            .WorkingDirectory = scratchFolder
        End With

        StartProcess(process)
        outputThread = New Thread(AddressOf ToolOutputProgress)
        outputThread.IsBackground = True
        outputThread.Start()

        AddToLog(GetMsg("mplayer_Start"))
    End Sub

    Private Sub StartNeroAACEnc()
        ' Delete the original audio file here since it's no longer needed
        If curItem.AudCodec <> "" Then
            DeleteSingleFile(scratchFolder & "audio" & curItem.AudCodec, True)
        End If

        process = New Process
        With process.StartInfo
            .FileName = AppPath & "\tools\neroAACEnc.exe"

            If My.Settings.NeroEncodeValue < 0 Then
                If My.Settings.NeroEncodeOption = "-q" Then
                    My.Settings.NeroEncodeValue = 0.5
                Else
                    My.Settings.NeroEncodeValue = 192
                End If
            End If

            If My.Settings.NeroEncodeValue < 80000 And My.Settings.NeroEncodeOption <> "-q" Then ' it's in kbit and not bits
                My.Settings.NeroEncodeValue *= 1024 ' convert from bits to bytes with proper 1024 factor
            End If

            .Arguments = "-lc " & My.Settings.NeroEncodeOption & " " & My.Settings.NeroEncodeValue & " "
            .Arguments = .Arguments & "-if audiodump.wav -of audio.m4a"

            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardError = True
            .WorkingDirectory = scratchFolder

            AddToLog(GetMsg("NeroAAC_Params", New String() {My.Settings.NeroEncodeOption, My.Settings.NeroEncodeValue}))

            ' Don't save any of these settings once the arguments have been inited.
            My.Settings.Reload()
        End With

        StartProcess(process)
        OutputText = ""
        outputThread = New Thread(AddressOf ToolStdErrProgress)
        outputThread.IsBackground = True
        outputThread.Start()

        AddToLog(GetMsg("NeroAAC_Start"))

    End Sub

    Public Sub StartMP4Box()
        ProgBarMax100()

        ' Delete the audiodump.wav file here to free up disk space since it's no longer needed
        DeleteSingleFile(scratchFolder & "audiodump.wav", True)

        ' If there's already an output.mp4, delete it
        DeleteSingleFile(scratchFolder & "output.mp4")

        ' B21: Check if 'video' still exists properly
        If Not My.Computer.FileSystem.FileExists(scratchFolder & "\video" & curItem.VidCodec) Then
            AddToLog(GetMsg("Error_MP4Box_VideoFNE"))
            NextTask = TASK_ERROR
        End If

        Dim name As String = GetFinalFileName(curItem)
        Dim tmpFolder As String = scratchFolder.Substring(0, scratchFolder.Length - 1)
        If tmpFolder.Length < 3 Then
            tmpFolder = scratchFolder
        End If

        logToolOutput = True

        Dim outputFile As String
        If My.Settings.MP4BoxOutputDirect Then
            If My.Settings.OutputUpperCase Then
                name = name.ToUpper
                My.Settings.OutputExtension = My.Settings.OutputExtension.ToUpper
            End If
            outputFile = outputPath & name & "." & My.Settings.OutputExtension
        Else
            outputFile = scratchFolder & "output.mp4"
        End If

        process = New Process
        With process.StartInfo
            .WorkingDirectory = scratchFolder
            .FileName = AppPath & My.Settings.MP4BoxPath

            Dim decimalPoint As String = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator
            Dim fpsDouble As Double = Double.Parse(curItem.FPS)
            Dim fpsInt As Integer = Math.Floor(fpsDouble)
            Dim fpsString As String = fpsDouble
            ' Dim fpsString As String = fpsInt & decimalPoint
            ' fpsString += curItem.FPS.Substring(curItem.FPS.IndexOf(".") + 1)
            .Arguments = """" & outputFile & _
            """ -add video" & curItem.VidCodec & " -fps " & fpsString & " -par 1=" & curItem.PAR & " -add audio.m4a " & _
            "-tmp """ & tmpFolder & """ -itags name=""" & name & """"
            .UseShellExecute = False
            .CreateNoWindow = True
            .RedirectStandardOutput = True
        End With

        DeleteSingleFile(outputFile) ' remove output file if it already exists

        StartProcess(process)
        OutputText = ""
        outputThread = New Thread(AddressOf ToolOutputChar)
        outputThread.IsBackground = True
        outputThread.Start()

        AddToLog(GetMsg("MP4Box_Start"))

        If My.Settings.MP4BoxPath = Constants.MP4BOX_DEFAULT_PATH Then
            AddToLog(GetMsg("MP4Box_Start_default"))
        ElseIf My.Settings.MP4BoxPath = Constants.MP4BOX_APR13_PATH Then
            AddToLog(GetMsg("MP4Box_Start_20080413"))
        Else
            AddToLog(GetMsg("MP4Box_Start_044"))
        End If
    End Sub

    Private Sub FinishCycle()
        ' Move the MP4 out to the proper output directory and rename to the selected filename
        Dim moveSuccess As Boolean = True
        Dim s As String = ""

        AddToLog(GetMsg("Finish_ConversionComplete"))

        ProgBarUpdate(0)

        ' Always delete temporary conversion files
        DeleteTempConversionFiles(curItem, scratchFolder)

        ' Check if this file was a single or split
        If curItem.IsSplit Then
            If My.Settings.DeleteTempFiles Then
                DeleteSingleFile(curItem.FileName)
            End If
        End If

        ' Check whether there are still more files to parse
        filesToConvert.RemoveAt(0)
        If filesToConvert.Count > 0 Then
            StartSingleProcess()
        Else
            FinallyFinished()
            Exit Sub
        End If
    End Sub

    Private Sub FinallyFinished()
        ' Enter final finishing code here
        FSHelper.NukeAndPave(scratchFolder)

        ' Also, perhaps we can pop up an alert here:
        Me.TrayIcon.ShowBalloonTip(1500, GetMsg("UI_Tray_Title"), GetMsg("UI_Tray_Success"), ToolTipIcon.Info)

        UICallback_EnableRecode()
        UICallback_EnableMKVInfo()
        UICallback_DisableCancel()

        SafeAbortIOThreads()
        SafeAbortProcess()

        If UIEvents.FromCommandLine Then
            End
        End If
    End Sub

    Public Function SafeAbortProcess()
        Try
            process.Kill()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function SafeAbortIOThreads()
        Try
            outputThread.Interrupt()
            outputThread.Abort()
        Catch ex As Exception
            Return False
        End Try

        Try
            errorThread.Interrupt()
            errorThread.Abort()
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

    Private Sub ErrorOut()
        ' Check whether there are still more files to parse
        filesToConvert.RemoveAt(0)
        If filesToConvert.Count > 0 Then
            AddToLog(GetMsg("Application_NextItem"))
            StartSingleProcess()
        Else
            AddToLog(GetMsg("Application_NoMoreItems"))
            FinallyFinished()
            Exit Sub
        End If
    End Sub
#End Region
End Class
