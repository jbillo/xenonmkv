Imports System.Text.RegularExpressions

Public Class OptionsDialog

    Private StartupMode As Boolean = False

    Private Sub OptionsDialog_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If My.Settings.CustomScratchFolder = "" Then
            My.Settings.Save()
            Exit Sub
        End If

        ' Reworked: Check if the folder is valid.
        If My.Computer.FileSystem.DirectoryExists(My.Settings.CustomScratchFolder) Then
            ' Make sure it's not a UNC path.
            If My.Settings.CustomScratchFolder.StartsWith("\\") Then
                MsgBox(GetMsg("UI_Options_ScratchFolderNoUNC"), MsgBoxStyle.Exclamation)
                My.Settings.CustomScratchFolder = ""
                My.Settings.Save()
                Exit Sub
            End If

            ' Make sure it's not the root of a drive.
            If Regex.IsMatch(My.Settings.CustomScratchFolder, "^[A-Z]\:\\?$", RegexOptions.IgnoreCase) Then
                ' Redirect this directory
                My.Settings.CustomScratchFolder = My.Settings.CustomScratchFolder.Substring(0, 1) & ":\temp\"
            End If
        Else
            MsgBox(GetMsg("UI_Options_ScratchFolderNotExist"), MsgBoxStyle.Exclamation)
            My.Settings.CustomScratchFolder = ""
        End If

        My.Settings.Save()
    End Sub

    Private Sub OptionsDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        StartupMode = True

        ' Azid - normalize AC3 audio
        AzidNormalizeAudio.Checked = My.Settings.AzidNormalizeAudio

        ' Azid - dynamic range compression
        Dim i As Integer
        i = AzidDRC.FindStringExact(My.Settings.AzidDRC)
        If i = -1 Then
            AzidDRC.SelectedIndex = 0
        Else
            AzidDRC.SelectedIndex = i
        End If

        ' Azid - filter rear channels
        AzidFilterRearChannels.Checked = My.Settings.AzidFilterRearChannels

        ' Azid - dialog normalization
        AzidDialogNormalization.Checked = My.Settings.AzidDialogNormalization

        ' Azid - stereo downmix
        i = AzidStereoDownmix.FindStringExact(My.Settings.AzidStereoDownmix)
        If i = -1 Then
            AzidStereoDownmix.SelectedIndex = 0
        Else
            AzidStereoDownmix.SelectedIndex = i
        End If

        ' Split files
        SplitFile_ThresholdValue.Text = My.Settings.SplitFileThreshold
        If My.Settings.SplitFiles = False Then
            If My.Settings.CutoffTimePrompt Then
                SplitFile_Time.Checked = True
            Else
                SplitFile_None.Checked = True
            End If
        Else
            If My.Settings.SplitFileThreshold > 0 And My.Settings.SplitFileThreshold <> DEFAULT_SPLIT_MB Then
                SplitFile_Threshold.Checked = True
                SwapSplitFileThreshold(True)
            Else
                SplitFile_Default.Checked = True
                SwapSplitFileThreshold(False)
            End If
        End If

        ' Incompatible files
        If My.Settings.HaltOnIncompatible Then
            HaltOnIncompatible.Checked = True
        Else
            ContinueOnIncompatible.Checked = True
        End If

        ' Output extension
        If My.Settings.OutputExtension = "" Then
            My.Settings.OutputExtension = DEFAULT_OUTPUT_EXTENSION
        End If

        If My.Settings.OutputExtension <> DEFAULT_OUTPUT_EXTENSION Then
            RenameOutputExtension.Text = My.Settings.OutputExtension
            RenameOutputCheck.Checked = True
        Else
            RenameOutputCheck.Checked = False
        End If

        ' Uppercase file name
        UppercaseOutputCheck.Checked = My.Settings.OutputUpperCase

        ' Scratch folder
        If Not My.Computer.FileSystem.DirectoryExists(My.Settings.CustomScratchFolder) Or _
            My.Settings.CustomScratchFolder = "\temp\" Then

            My.Settings.CustomScratchFolder = ""
            My.Settings.Save()
        End If
        ScratchFolder.Text = My.Settings.CustomScratchFolder

        ' i18n
        For Each foundFile As String In _
            My.Computer.FileSystem.GetFiles(AppPath & "\i18n\", FileIO.SearchOption.SearchTopLevelOnly, "*.xml")
            ' Add file to list
            Dim s As String = My.Computer.FileSystem.GetName(foundFile).Replace(".xml", "")
            s = UCase(s.Substring(0, 1)) & s.Substring(1)
            LanguageSelector.Items.Add(s)
        Next

        i = LanguageSelector.FindString(My.Settings.i18n.Replace(".xml", ""))
        If i > -1 Then
            LanguageSelector.SelectedIndex = i
        End If

        ' Auto select tracks
        AutoPickTracks.Checked = My.Settings.AutoPickTracks

        ' Default track language
        AlwaysUseLanguageList.Text = My.Settings.AlwaysUseLanguage

        ' Round to 1:1 PAR
        RoundPAR.Checked = My.Settings.RoundPAR

        'Nero AAC quality type
        If My.Settings.NeroEncodeOption = "-br" Or My.Settings.NeroEncodeOption = "-cbr" Then
            If My.Settings.NeroEncodeOption = "-br" Then
                Quality_BR.Checked = True
            Else
                Quality_CBR.Checked = True
            End If
            Quality_Q_Panel.Enabled = False
            Quality_BR_Panel.Enabled = True
            Quality_BR_Value.Text = My.Settings.NeroEncodeValue
        Else
            Quality_Q.Checked = True
            Quality_Q_Panel.Enabled = True
            Quality_BR_Panel.Enabled = False
            NeroQualityBar.Value = (My.Settings.NeroEncodeValue * 10)
            NeroQualityValue.Text = My.Settings.NeroEncodeValue
        End If

        'MP4Box build selector
        If My.Settings.MP4BoxPath = Constants.MP4BOX_DEFAULT_PATH Then
            MP4BoxDefaultBuild.Checked = True
        ElseIf My.Settings.MP4BoxPath = Constants.MP4BOX_APR13_PATH Then
            MP4BoxApril13Build.Checked = True
        ElseIf My.Settings.MP4BoxPath = Constants.MP4BOX_044_PATH Then
            MP4Box044_2007.Checked = True
        Else
            My.Settings.MP4BoxPath = Constants.MP4BOX_DEFAULT_PATH
            MP4BoxDefaultBuild.Checked = True
            My.Settings.Save()
        End If

        'MP4box output direct or move?
        If My.Settings.MP4BoxOutputDirect = True Then
            MP4BoxOutputDirect.Checked = True
        Else
            MP4BoxOutputMove.Checked = True
        End If

        'MKVextract safe sync
        MKVExtractUseSync.Checked = My.Settings.MKVExtractSync

        ' Custom aspect ratio selector
        ProvideCustomAR.Checked = My.Settings.ProvideCustomAR

        StartupMode = False
    End Sub

    Private Sub AzidNormalizeAudio_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AzidNormalizeAudio.CheckedChanged
        If Not StartupMode Then
            My.Settings.AzidNormalizeAudio = AzidNormalizeAudio.Checked
            My.Settings.Save()
        End If
    End Sub

    Private Sub SaveOptions_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveOptions.Click
        My.Settings.i18n = LanguageSelector.SelectedItem.ToString & ".xml"

        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub SwapSplitFileThreshold(ByVal isCustom As Boolean)
        If isCustom Then
            SplitFile_ThresholdValue.Enabled = True
            SplitFile_ThresholdValue.Text = My.Settings.SplitFileThreshold
        Else
            SplitFile_ThresholdValue.Enabled = False
        End If
    End Sub

    Private Sub SplitFile_Threshold_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitFile_Threshold.CheckedChanged
        If Not StartupMode Then
            SwapSplitFileThreshold(True)
            If SplitFile_Threshold.Checked Then
                Try
                    My.Settings.SplitFiles = True
                    My.Settings.CutoffTimePrompt = False
                    My.Settings.SplitFileThreshold = CInt(SplitFile_ThresholdValue.Text)
                Catch ice As InvalidCastException
                    My.Settings.SplitFileThreshold = DEFAULT_SPLIT_MB
                End Try
            End If
        End If
    End Sub

    Private Sub SplitFile_Default_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitFile_Default.CheckedChanged
        If Not StartupMode Then
            SwapSplitFileThreshold(False)
            If SplitFile_Default.Checked Then
                My.Settings.SplitFiles = True
                My.Settings.CutoffTimePrompt = False
                My.Settings.SplitFileThreshold = DEFAULT_SPLIT_MB
            End If
        End If
    End Sub

    Private Sub SplitFile_None_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitFile_None.CheckedChanged
        If Not StartupMode Then
            SwapSplitFileThreshold(False)
            If SplitFile_None.Checked Then
                My.Settings.SplitFiles = False
                My.Settings.CutoffTimePrompt = False
            End If
        End If
    End Sub

    Private Sub SplitFile_ThresholdValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitFile_ThresholdValue.TextChanged
        SplitFile_ThresholdValue.Text = Regex.Replace(SplitFile_ThresholdValue.Text, "[^0-9]*", "", RegexOptions.IgnoreCase)
        If SplitFile_Threshold.Checked Then
            Try
                My.Settings.CutoffTimePrompt = False
                My.Settings.SplitFiles = True
                My.Settings.SplitFileThreshold = CInt(SplitFile_ThresholdValue.Text)
            Catch ice As InvalidCastException
                My.Settings.SplitFileThreshold = DEFAULT_SPLIT_MB
            End Try
        End If
    End Sub

    Private Sub SplitFile_Threshold_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SplitFile_Threshold.Click
        SplitFile_ThresholdValue.Focus()
    End Sub

    Private Sub RenameOutputCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RenameOutputCheck.CheckedChanged
        If Not StartupMode Then
            If RenameOutputCheck.Checked Then
                RenameOutputExtension.Text = My.Settings.OutputExtension
                RenameOutputExtension.Enabled = True
                RenameOutputExtension.Focus()
                My.Settings.OutputExtension = RenameOutputExtension.Text
            Else
                RenameOutputExtension.Text = DEFAULT_OUTPUT_EXTENSION
                RenameOutputExtension.Enabled = False
                My.Settings.OutputExtension = DEFAULT_OUTPUT_EXTENSION
            End If
        End If
    End Sub

    Private Sub RenameOutputExtension_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RenameOutputExtension.TextChanged
        If Not StartupMode Then
            RenameOutputExtension.Text = Regex.Replace(RenameOutputExtension.Text, "[^a-z|0-9]*", "", RegexOptions.IgnoreCase)
            RenameOutputExtension.Text = RenameOutputExtension.Text.ToLower

            If RenameOutputCheck.Checked Then
                If RenameOutputExtension.Text <> "" Then
                    My.Settings.OutputExtension = RenameOutputExtension.Text
                Else
                    My.Settings.OutputExtension = DEFAULT_OUTPUT_EXTENSION
                End If
            End If
        End If
    End Sub

    Private Sub ScratchFolder_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScratchFolder.TextChanged
        If Not StartupMode Then
            My.Settings.CustomScratchFolder = ScratchFolder.Text
            If My.Computer.FileSystem.DirectoryExists(My.Settings.CustomScratchFolder) Then
                My.Settings.Save()
            End If
        End If
    End Sub

    Private Sub BrowseScratchLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BrowseScratchLocation.Click
        If ScratchFolderBrowser.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If ScratchFolderBrowser.SelectedPath.Contains("\\") Then
                MsgBox(GetMsg("ScratchFolder_Network"), MsgBoxStyle.Exclamation, GetMsg("ScratchFolder_Network_Title"))
            ElseIf Not My.Computer.FileSystem.GetDirectoryInfo(ScratchFolderBrowser.SelectedPath).Exists Then
                MsgBox(GetMsg("ScratchFolder_NotExist"), MsgBoxStyle.Exclamation, GetMsg("ScratchFolder_NotExist_Title"))
            Else
                If ScratchFolderBrowser.SelectedPath.Length < 4 Then
                    ' assuming X:\; append "temp" on the end of this
                    If ScratchFolderBrowser.SelectedPath.EndsWith("\") Then
                        ScratchFolderBrowser.SelectedPath = ScratchFolderBrowser.SelectedPath & "temp\"
                    Else
                        ScratchFolderBrowser.SelectedPath = ScratchFolderBrowser.SelectedPath & "\temp\"
                    End If
                End If

                Try
                    Dim t As String = ScratchFolderBrowser.SelectedPath & "\" & FSHelper.GetCleanTempDir(ScratchFolderBrowser.SelectedPath)
                    My.Computer.FileSystem.CreateDirectory(t)
                    My.Computer.FileSystem.DeleteDirectory(t, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    ScratchFolder.Text = ScratchFolderBrowser.SelectedPath
                    My.Settings.CustomScratchFolder = ScratchFolder.Text
                Catch ex As Exception
                    MsgBox(GetMsg("ScratchFolder_NotWritable"), MsgBoxStyle.Exclamation, GetMsg("ScratchFolder_NotWritable_Title"))
                End Try
                End If
        End If
    End Sub

    Private Sub AutoPickTracks_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoPickTracks.CheckedChanged
        If Not StartupMode Then
            My.Settings.AutoPickTracks = AutoPickTracks.Checked
            My.Settings.Save()
        End If
    End Sub

    Private Sub AlwaysUseLanguageList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AlwaysUseLanguageList.SelectedIndexChanged
        If Not StartupMode Then
            My.Settings.AlwaysUseLanguage = AlwaysUseLanguageList.Text
            My.Settings.Save()
        End If
    End Sub

    Private Sub AlwaysUseLanguageList_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AlwaysUseLanguageList.TextChanged
        If Not StartupMode Then
            My.Settings.AlwaysUseLanguage = AlwaysUseLanguageList.Text
            My.Settings.Save()
        End If
    End Sub

    Private Sub NeroQualityBar_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NeroQualityBar.Scroll
        NeroQualityValue.Text = NeroQualityBar.Value * 0.1
        My.Settings.NeroEncodeValue = CSng(NeroQualityValue.Text)
        My.Settings.Save()
    End Sub

    Private Sub SplitFile_Time_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitFile_Time.CheckedChanged
        If Not StartupMode Then
            If SplitFile_Time.Checked Then
                My.Settings.SplitFiles = False
                My.Settings.CutoffTimePrompt = True
                My.Settings.SplitFileThreshold = DEFAULT_SPLIT_MB
            Else
                My.Settings.CutoffTimePrompt = False
            End If

            My.Settings.Save()
        End If
    End Sub

    Private Sub AzidDRC_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AzidDRC.SelectedIndexChanged
        If Not StartupMode Then
            My.Settings.AzidDRC = AzidDRC.SelectedItem.ToString
        End If
    End Sub

    Private Sub AzidDRCAbout_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles AzidDRCAbout.LinkClicked
        MsgBox(GetMsg("Info_AzidDRC"))
    End Sub

    Private Sub AzidFilterRearChannels_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AzidFilterRearChannels.CheckedChanged
        If Not StartupMode Then
            My.Settings.AzidFilterRearChannels = AzidFilterRearChannels.Checked
            My.Settings.Save()
        End If
    End Sub

    Private Sub AzidRearFilterAbout_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles AzidRearFilterAbout.LinkClicked
        MsgBox(GetMsg("Info_AzidRearFilter"))
    End Sub

    Private Sub AzidDialogNormalization_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AzidDialogNormalization.CheckedChanged
        If Not StartupMode Then
            My.Settings.AzidDialogNormalization = AzidDialogNormalization.Checked
            My.Settings.Save()
        End If
    End Sub

    Private Sub AzidDialogNormalizationInfo_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles AzidDialogNormalizationInfo.LinkClicked
        MsgBox(GetMsg("Info_AzidDialogNormalization"))
    End Sub

    Private Sub AzidStereoDownmix_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AzidStereoDownmix.SelectedIndexChanged
        If Not StartupMode Then
            My.Settings.AzidStereoDownmix = AzidStereoDownmix.SelectedItem.ToString
        End If
    End Sub

    Private Sub AzidStereoDownmixInfo_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles AzidStereoDownmixInfo.LinkClicked
        MsgBox(GetMsg("Info_AzidStereoDownmix"))
    End Sub

    Private Sub CheckerHaltProcess_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HaltOnIncompatible.CheckedChanged
        If Not StartupMode And HaltOnIncompatible.Checked Then
            My.Settings.HaltOnIncompatible = True
        End If
    End Sub

    Private Sub CheckerContinueProcess_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ContinueOnIncompatible.CheckedChanged
        If Not StartupMode And ContinueOnIncompatible.Checked Then
            My.Settings.HaltOnIncompatible = False
        End If
    End Sub

    Private Sub RoundPAR_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RoundPAR.CheckedChanged
        If Not StartupMode Then
            My.Settings.RoundPAR = RoundPAR.Checked
        End If
    End Sub

    Private Sub MP4BoxDefaultBuild_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MP4BoxDefaultBuild.CheckedChanged
        If Not StartupMode And MP4BoxDefaultBuild.Checked Then
            My.Settings.MP4BoxPath = Constants.MP4BOX_DEFAULT_PATH
        End If
    End Sub

    Private Sub MP4BoxApril13Build_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MP4BoxApril13Build.CheckedChanged
        If Not StartupMode And MP4BoxApril13Build.Checked Then
            My.Settings.MP4BoxPath = Constants.MP4BOX_APR13_PATH
        End If
    End Sub

    Private Sub MP4Box044_2007_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MP4Box044_2007.CheckedChanged
        If Not StartupMode And MP4Box044_2007.Checked Then
            My.Settings.MP4BoxPath = Constants.MP4BOX_044_PATH
        End If
    End Sub

    Private Sub MP4BoxOutputMove_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MP4BoxOutputMove.CheckedChanged
        If Not StartupMode And MP4BoxOutputMove.Checked Then
            My.Settings.MP4BoxOutputDirect = False
        End If
    End Sub

    Private Sub MP4BoxOutputDirect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MP4BoxOutputDirect.CheckedChanged
        If Not StartupMode And MP4BoxOutputDirect.Checked Then
            My.Settings.MP4BoxOutputDirect = True
        End If
    End Sub

    Private Sub MKVExtractUseSync_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MKVExtractUseSync.CheckedChanged
        If Not StartupMode Then
            My.Settings.MKVExtractSync = MKVExtractUseSync.Checked
        End If
    End Sub

    Private Sub ProvideCustomAR_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProvideCustomAR.CheckedChanged
        If Not StartupMode Then
            My.Settings.ProvideCustomAR = ProvideCustomAR.Checked
        End If
    End Sub

    Private Sub Quality_Q_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Quality_Q.CheckedChanged
        If Not StartupMode Then
            My.Settings.NeroEncodeOption = "-q"
            My.Settings.NeroEncodeValue = NeroQualityValue.Text
            Quality_BR_Panel.Enabled = False
            Quality_Q_Panel.Enabled = True
        End If
    End Sub

    Private Sub Quality_BR_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Quality_BR.CheckedChanged
        If Not StartupMode Then
            My.Settings.NeroEncodeOption = "-br"
            Quality_BR_Panel.Enabled = True
            Quality_Q_Panel.Enabled = False
            If My.Settings.NeroEncodeValue < 1.1 Then
                Quality_BR_Value.Text = "192"
            End If
        End If
    End Sub

    Private Sub Quality_CBR_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Quality_CBR.CheckedChanged
        If Not StartupMode Then
            My.Settings.NeroEncodeOption = "-cbr"
            Quality_BR_Panel.Enabled = True
            Quality_Q_Panel.Enabled = False
            If My.Settings.NeroEncodeValue < 1.1 Then
                Quality_BR_Value.Text = "192"
            End If
        End If
    End Sub

    Private Sub Quality_BR_Value_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Quality_BR_Value.TextChanged
        If Not IsNumeric(Quality_BR_Value.Text) And Quality_BR_Value.Text <> "" Then
            MsgBox(GetMsg("UI_Options_TargetBitrateNonNumeric"), MsgBoxStyle.Exclamation, GetMsg("UI_Options_TargetBitrateNonNumericTitle"))
            Quality_BR_Value.Text = ""
        Else
            My.Settings.NeroEncodeValue = CSng(Quality_BR_Value.Text)
            If Quality_CBR.Checked Then
                My.Settings.NeroEncodeOption = "-cbr"
            Else
                My.Settings.NeroEncodeOption = "-br"
            End If
        End If
    End Sub


    Private Sub UppercaseOutputCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UppercaseOutputCheck.CheckedChanged
        If Not StartupMode Then
            My.Settings.OutputUpperCase = UppercaseOutputCheck.Checked
        End If
    End Sub

    Private Sub ResetMainUIWindowMetrics_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResetMainUIWindowMetrics.Click
        My.Settings.MainUIHeight = 0
        My.Settings.MainUIWidth = 0
        My.Settings.MainUITop = -1
        My.Settings.MainUILeft = -1
        My.Settings.MainUIWindowState = 0

        My.Settings.Save()
        MsgBox(GetMsg("UI_Options_RestartApp"), MsgBoxStyle.Information)
        UIEvents.SaveSettingsOnExit = False
        End
    End Sub
End Class