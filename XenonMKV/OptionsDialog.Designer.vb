<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OptionsDialog))
        Me.SaveOptions = New System.Windows.Forms.Button()
        Me.ScratchFolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.LastStepsTab = New System.Windows.Forms.TabPage()
        Me.MP4BoxOutputMethodGroup = New System.Windows.Forms.GroupBox()
        Me.MP4BoxOutputDirect = New System.Windows.Forms.RadioButton()
        Me.MP4BoxOutputMove = New System.Windows.Forms.RadioButton()
        Me.MP4Box_Latest_Group = New System.Windows.Forms.GroupBox()
        Me.MP4Box044_2007 = New System.Windows.Forms.RadioButton()
        Me.MP4BoxApril13Build = New System.Windows.Forms.RadioButton()
        Me.MP4BoxDefaultBuild = New System.Windows.Forms.RadioButton()
        Me.MP4Box_NewestBuild_Info = New System.Windows.Forms.Label()
        Me.RenameOutputGroup = New System.Windows.Forms.GroupBox()
        Me.UppercaseOutputCheck = New System.Windows.Forms.CheckBox()
        Me.RenameOutputExtension = New System.Windows.Forms.TextBox()
        Me.RenameOutputCheck = New System.Windows.Forms.CheckBox()
        Me.RenameOutputDesc = New System.Windows.Forms.Label()
        Me.AudioTab = New System.Windows.Forms.TabPage()
        Me.AzidFrame = New System.Windows.Forms.GroupBox()
        Me.AzidStereoDownmixInfo = New System.Windows.Forms.LinkLabel()
        Me.AzidStereoDownmix = New System.Windows.Forms.ComboBox()
        Me.AzidStereoDownmixPreamble = New System.Windows.Forms.Label()
        Me.AzidDialogNormalizationInfo = New System.Windows.Forms.LinkLabel()
        Me.AzidDialogNormalization = New System.Windows.Forms.CheckBox()
        Me.AzidRearFilterAbout = New System.Windows.Forms.LinkLabel()
        Me.AzidFilterRearChannels = New System.Windows.Forms.CheckBox()
        Me.AzidDRCAbout = New System.Windows.Forms.LinkLabel()
        Me.AzidDRC = New System.Windows.Forms.ComboBox()
        Me.AzidDRCPreamble = New System.Windows.Forms.Label()
        Me.AzidNormalizeAudio = New System.Windows.Forms.CheckBox()
        Me.TrackTab = New System.Windows.Forms.TabPage()
        Me.VideoTrackGroup = New System.Windows.Forms.GroupBox()
        Me.ProvideCustomAR = New System.Windows.Forms.CheckBox()
        Me.RoundPAR = New System.Windows.Forms.CheckBox()
        Me.TrackOptionsBox = New System.Windows.Forms.GroupBox()
        Me.MKVExtractUseSync = New System.Windows.Forms.CheckBox()
        Me.AutoPickTracks = New System.Windows.Forms.CheckBox()
        Me.AudioTrackBox = New System.Windows.Forms.GroupBox()
        Me.AlwaysUseLanguageList = New System.Windows.Forms.ComboBox()
        Me.AudioTrackLanguageInfo = New System.Windows.Forms.Label()
        Me.FilesAndFoldersTab = New System.Windows.Forms.TabPage()
        Me.XB360RefFramesBox = New System.Windows.Forms.GroupBox()
        Me.ContinueOnIncompatible = New System.Windows.Forms.RadioButton()
        Me.HaltOnIncompatible = New System.Windows.Forms.RadioButton()
        Me.XB360RefFrameInfo = New System.Windows.Forms.Label()
        Me.ScratchDiskGroup = New System.Windows.Forms.GroupBox()
        Me.BrowseScratchLocation = New System.Windows.Forms.Button()
        Me.ScratchFolder = New System.Windows.Forms.TextBox()
        Me.ScratchFolderDesc = New System.Windows.Forms.Label()
        Me.FileSplitGroup = New System.Windows.Forms.GroupBox()
        Me.SplitFile_Time = New System.Windows.Forms.RadioButton()
        Me.SplitFile_ThresholdValue = New System.Windows.Forms.TextBox()
        Me.SplitFile_Threshold = New System.Windows.Forms.RadioButton()
        Me.SplitFile_None = New System.Windows.Forms.RadioButton()
        Me.SplitFile_Default = New System.Windows.Forms.RadioButton()
        Me.FileSplittingInfo = New System.Windows.Forms.Label()
        Me.UITab = New System.Windows.Forms.TabPage()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ResetMainUIWindowMetrics = New System.Windows.Forms.Button()
        Me.LanguageFrame = New System.Windows.Forms.GroupBox()
        Me.LanguageRestartInfo = New System.Windows.Forms.Label()
        Me.LanguageSelector = New System.Windows.Forms.ComboBox()
        Me.OptionTabContainer = New System.Windows.Forms.TabControl()
        Me.NeroAACTab = New System.Windows.Forms.TabPage()
        Me.NeroAACEncoderBox = New System.Windows.Forms.GroupBox()
        Me.Quality_CBR = New System.Windows.Forms.RadioButton()
        Me.Quality_BR_Panel = New System.Windows.Forms.Panel()
        Me.TargetBitrateKbps = New System.Windows.Forms.Label()
        Me.Quality_BR_Value = New System.Windows.Forms.ComboBox()
        Me.TargetBitrateInfo = New System.Windows.Forms.Label()
        Me.Quality_Q_Panel = New System.Windows.Forms.Panel()
        Me.NeroEncoderInfo = New System.Windows.Forms.Label()
        Me.NeroQualityValue = New System.Windows.Forms.Label()
        Me.NeroQualityBar = New System.Windows.Forms.TrackBar()
        Me.Quality_BR = New System.Windows.Forms.RadioButton()
        Me.Quality_Q = New System.Windows.Forms.RadioButton()
        Me.LastStepsTab.SuspendLayout()
        Me.MP4BoxOutputMethodGroup.SuspendLayout()
        Me.MP4Box_Latest_Group.SuspendLayout()
        Me.RenameOutputGroup.SuspendLayout()
        Me.AudioTab.SuspendLayout()
        Me.AzidFrame.SuspendLayout()
        Me.TrackTab.SuspendLayout()
        Me.VideoTrackGroup.SuspendLayout()
        Me.TrackOptionsBox.SuspendLayout()
        Me.AudioTrackBox.SuspendLayout()
        Me.FilesAndFoldersTab.SuspendLayout()
        Me.XB360RefFramesBox.SuspendLayout()
        Me.ScratchDiskGroup.SuspendLayout()
        Me.FileSplitGroup.SuspendLayout()
        Me.UITab.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.LanguageFrame.SuspendLayout()
        Me.OptionTabContainer.SuspendLayout()
        Me.NeroAACTab.SuspendLayout()
        Me.NeroAACEncoderBox.SuspendLayout()
        Me.Quality_BR_Panel.SuspendLayout()
        Me.Quality_Q_Panel.SuspendLayout()
        CType(Me.NeroQualityBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SaveOptions
        '
        Me.SaveOptions.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveOptions.Location = New System.Drawing.Point(313, 414)
        Me.SaveOptions.Name = "SaveOptions"
        Me.SaveOptions.Size = New System.Drawing.Size(121, 28)
        Me.SaveOptions.TabIndex = 1
        Me.SaveOptions.Text = "Save Settings"
        Me.SaveOptions.UseVisualStyleBackColor = True
        '
        'ScratchFolderBrowser
        '
        Me.ScratchFolderBrowser.Description = "Select a scratch location for temporary files."
        '
        'LastStepsTab
        '
        Me.LastStepsTab.Controls.Add(Me.MP4BoxOutputMethodGroup)
        Me.LastStepsTab.Controls.Add(Me.MP4Box_Latest_Group)
        Me.LastStepsTab.Controls.Add(Me.RenameOutputGroup)
        Me.LastStepsTab.Location = New System.Drawing.Point(4, 22)
        Me.LastStepsTab.Name = "LastStepsTab"
        Me.LastStepsTab.Padding = New System.Windows.Forms.Padding(3)
        Me.LastStepsTab.Size = New System.Drawing.Size(426, 386)
        Me.LastStepsTab.TabIndex = 1
        Me.LastStepsTab.Text = "Last Steps"
        Me.LastStepsTab.UseVisualStyleBackColor = True
        '
        'MP4BoxOutputMethodGroup
        '
        Me.MP4BoxOutputMethodGroup.Controls.Add(Me.MP4BoxOutputDirect)
        Me.MP4BoxOutputMethodGroup.Controls.Add(Me.MP4BoxOutputMove)
        Me.MP4BoxOutputMethodGroup.Location = New System.Drawing.Point(8, 120)
        Me.MP4BoxOutputMethodGroup.Name = "MP4BoxOutputMethodGroup"
        Me.MP4BoxOutputMethodGroup.Size = New System.Drawing.Size(412, 67)
        Me.MP4BoxOutputMethodGroup.TabIndex = 4
        Me.MP4BoxOutputMethodGroup.TabStop = False
        Me.MP4BoxOutputMethodGroup.Text = "MP4Box Output Method"
        '
        'MP4BoxOutputDirect
        '
        Me.MP4BoxOutputDirect.AutoSize = True
        Me.MP4BoxOutputDirect.Location = New System.Drawing.Point(6, 43)
        Me.MP4BoxOutputDirect.Name = "MP4BoxOutputDirect"
        Me.MP4BoxOutputDirect.Size = New System.Drawing.Size(279, 17)
        Me.MP4BoxOutputDirect.TabIndex = 1
        Me.MP4BoxOutputDirect.TabStop = True
        Me.MP4BoxOutputDirect.Text = "mp4box should output directly to my destination folder."
        Me.MP4BoxOutputDirect.UseVisualStyleBackColor = True
        '
        'MP4BoxOutputMove
        '
        Me.MP4BoxOutputMove.AutoSize = True
        Me.MP4BoxOutputMove.Location = New System.Drawing.Point(6, 20)
        Me.MP4BoxOutputMove.Name = "MP4BoxOutputMove"
        Me.MP4BoxOutputMove.Size = New System.Drawing.Size(379, 17)
        Me.MP4BoxOutputMove.TabIndex = 0
        Me.MP4BoxOutputMove.TabStop = True
        Me.MP4BoxOutputMove.Text = "mp4box should output to my scratch folder, then move the finished product."
        Me.MP4BoxOutputMove.UseVisualStyleBackColor = True
        '
        'MP4Box_Latest_Group
        '
        Me.MP4Box_Latest_Group.Controls.Add(Me.MP4Box044_2007)
        Me.MP4Box_Latest_Group.Controls.Add(Me.MP4BoxApril13Build)
        Me.MP4Box_Latest_Group.Controls.Add(Me.MP4BoxDefaultBuild)
        Me.MP4Box_Latest_Group.Controls.Add(Me.MP4Box_NewestBuild_Info)
        Me.MP4Box_Latest_Group.Location = New System.Drawing.Point(8, 6)
        Me.MP4Box_Latest_Group.Name = "MP4Box_Latest_Group"
        Me.MP4Box_Latest_Group.Size = New System.Drawing.Size(412, 108)
        Me.MP4Box_Latest_Group.TabIndex = 3
        Me.MP4Box_Latest_Group.TabStop = False
        Me.MP4Box_Latest_Group.Text = "MP4 Creation"
        '
        'MP4Box044_2007
        '
        Me.MP4Box044_2007.AutoSize = True
        Me.MP4Box044_2007.Location = New System.Drawing.Point(6, 85)
        Me.MP4Box044_2007.Name = "MP4Box044_2007"
        Me.MP4Box044_2007.Size = New System.Drawing.Size(246, 17)
        Me.MP4Box044_2007.TabIndex = 3
        Me.MP4Box044_2007.Text = "Use 0.4.4: June 3, 2007 build (stable fallback)"
        Me.MP4Box044_2007.UseVisualStyleBackColor = True
        '
        'MP4BoxApril13Build
        '
        Me.MP4BoxApril13Build.AutoSize = True
        Me.MP4BoxApril13Build.Location = New System.Drawing.Point(6, 61)
        Me.MP4BoxApril13Build.Name = "MP4BoxApril13Build"
        Me.MP4BoxApril13Build.Size = New System.Drawing.Size(171, 17)
        Me.MP4BoxApril13Build.TabIndex = 2
        Me.MP4BoxApril13Build.TabStop = True
        Me.MP4BoxApril13Build.Text = "Use 0.4.5: April 13, 2008 build"
        Me.MP4BoxApril13Build.UseVisualStyleBackColor = True
        '
        'MP4BoxDefaultBuild
        '
        Me.MP4BoxDefaultBuild.AutoSize = True
        Me.MP4BoxDefaultBuild.Location = New System.Drawing.Point(6, 37)
        Me.MP4BoxDefaultBuild.Name = "MP4BoxDefaultBuild"
        Me.MP4BoxDefaultBuild.Size = New System.Drawing.Size(212, 17)
        Me.MP4BoxDefaultBuild.TabIndex = 1
        Me.MP4BoxDefaultBuild.TabStop = True
        Me.MP4BoxDefaultBuild.Text = "Use 0.4.6: Oct 18, 2010 build (default)"
        Me.MP4BoxDefaultBuild.UseVisualStyleBackColor = True
        '
        'MP4Box_NewestBuild_Info
        '
        Me.MP4Box_NewestBuild_Info.Location = New System.Drawing.Point(6, 17)
        Me.MP4Box_NewestBuild_Info.Name = "MP4Box_NewestBuild_Info"
        Me.MP4Box_NewestBuild_Info.Size = New System.Drawing.Size(398, 17)
        Me.MP4Box_NewestBuild_Info.TabIndex = 0
        Me.MP4Box_NewestBuild_Info.Text = "If MP4box.exe crashes during conversion, you can select a different build:"
        '
        'RenameOutputGroup
        '
        Me.RenameOutputGroup.Controls.Add(Me.UppercaseOutputCheck)
        Me.RenameOutputGroup.Controls.Add(Me.RenameOutputExtension)
        Me.RenameOutputGroup.Controls.Add(Me.RenameOutputCheck)
        Me.RenameOutputGroup.Controls.Add(Me.RenameOutputDesc)
        Me.RenameOutputGroup.Location = New System.Drawing.Point(8, 193)
        Me.RenameOutputGroup.Name = "RenameOutputGroup"
        Me.RenameOutputGroup.Size = New System.Drawing.Size(412, 117)
        Me.RenameOutputGroup.TabIndex = 0
        Me.RenameOutputGroup.TabStop = False
        Me.RenameOutputGroup.Text = "Rename Files"
        '
        'UppercaseOutputCheck
        '
        Me.UppercaseOutputCheck.AutoSize = True
        Me.UppercaseOutputCheck.Location = New System.Drawing.Point(10, 89)
        Me.UppercaseOutputCheck.Name = "UppercaseOutputCheck"
        Me.UppercaseOutputCheck.Size = New System.Drawing.Size(354, 17)
        Me.UppercaseOutputCheck.TabIndex = 3
        Me.UppercaseOutputCheck.Text = "Use upper case for output file name and extension (some LaCie disks)"
        Me.UppercaseOutputCheck.UseVisualStyleBackColor = True
        '
        'RenameOutputExtension
        '
        Me.RenameOutputExtension.Enabled = False
        Me.RenameOutputExtension.Location = New System.Drawing.Point(247, 63)
        Me.RenameOutputExtension.MaxLength = 16
        Me.RenameOutputExtension.Name = "RenameOutputExtension"
        Me.RenameOutputExtension.Size = New System.Drawing.Size(100, 21)
        Me.RenameOutputExtension.TabIndex = 2
        '
        'RenameOutputCheck
        '
        Me.RenameOutputCheck.AutoSize = True
        Me.RenameOutputCheck.Location = New System.Drawing.Point(10, 65)
        Me.RenameOutputCheck.Name = "RenameOutputCheck"
        Me.RenameOutputCheck.Size = New System.Drawing.Size(234, 17)
        Me.RenameOutputCheck.TabIndex = 1
        Me.RenameOutputCheck.Text = "Rename file extensions after processing to  ."
        Me.RenameOutputCheck.UseVisualStyleBackColor = True
        '
        'RenameOutputDesc
        '
        Me.RenameOutputDesc.Location = New System.Drawing.Point(7, 21)
        Me.RenameOutputDesc.Name = "RenameOutputDesc"
        Me.RenameOutputDesc.Size = New System.Drawing.Size(397, 40)
        Me.RenameOutputDesc.TabIndex = 0
        Me.RenameOutputDesc.Text = resources.GetString("RenameOutputDesc.Text")
        '
        'AudioTab
        '
        Me.AudioTab.Controls.Add(Me.AzidFrame)
        Me.AudioTab.Location = New System.Drawing.Point(4, 22)
        Me.AudioTab.Name = "AudioTab"
        Me.AudioTab.Padding = New System.Windows.Forms.Padding(3)
        Me.AudioTab.Size = New System.Drawing.Size(426, 386)
        Me.AudioTab.TabIndex = 0
        Me.AudioTab.Text = "AC3 Decoder"
        Me.AudioTab.UseVisualStyleBackColor = True
        '
        'AzidFrame
        '
        Me.AzidFrame.Controls.Add(Me.AzidStereoDownmixInfo)
        Me.AzidFrame.Controls.Add(Me.AzidStereoDownmix)
        Me.AzidFrame.Controls.Add(Me.AzidStereoDownmixPreamble)
        Me.AzidFrame.Controls.Add(Me.AzidDialogNormalizationInfo)
        Me.AzidFrame.Controls.Add(Me.AzidDialogNormalization)
        Me.AzidFrame.Controls.Add(Me.AzidRearFilterAbout)
        Me.AzidFrame.Controls.Add(Me.AzidFilterRearChannels)
        Me.AzidFrame.Controls.Add(Me.AzidDRCAbout)
        Me.AzidFrame.Controls.Add(Me.AzidDRC)
        Me.AzidFrame.Controls.Add(Me.AzidDRCPreamble)
        Me.AzidFrame.Controls.Add(Me.AzidNormalizeAudio)
        Me.AzidFrame.Location = New System.Drawing.Point(6, 6)
        Me.AzidFrame.Name = "AzidFrame"
        Me.AzidFrame.Size = New System.Drawing.Size(412, 150)
        Me.AzidFrame.TabIndex = 0
        Me.AzidFrame.TabStop = False
        Me.AzidFrame.Text = "Azid AC3 Decoder"
        '
        'AzidStereoDownmixInfo
        '
        Me.AzidStereoDownmixInfo.AutoSize = True
        Me.AzidStereoDownmixInfo.Location = New System.Drawing.Point(353, 122)
        Me.AzidStereoDownmixInfo.Name = "AzidStereoDownmixInfo"
        Me.AzidStereoDownmixInfo.Size = New System.Drawing.Size(51, 13)
        Me.AzidStereoDownmixInfo.TabIndex = 10
        Me.AzidStereoDownmixInfo.TabStop = True
        Me.AzidStereoDownmixInfo.Text = "Details..."
        '
        'AzidStereoDownmix
        '
        Me.AzidStereoDownmix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AzidStereoDownmix.FormattingEnabled = True
        Me.AzidStereoDownmix.Items.AddRange(New Object() {"dpl", "dplii", "stereo", "mono"})
        Me.AzidStereoDownmix.Location = New System.Drawing.Point(132, 119)
        Me.AzidStereoDownmix.Name = "AzidStereoDownmix"
        Me.AzidStereoDownmix.Size = New System.Drawing.Size(215, 21)
        Me.AzidStereoDownmix.TabIndex = 9
        '
        'AzidStereoDownmixPreamble
        '
        Me.AzidStereoDownmixPreamble.AutoSize = True
        Me.AzidStereoDownmixPreamble.Location = New System.Drawing.Point(6, 122)
        Me.AzidStereoDownmixPreamble.Name = "AzidStereoDownmixPreamble"
        Me.AzidStereoDownmixPreamble.Size = New System.Drawing.Size(120, 13)
        Me.AzidStereoDownmixPreamble.TabIndex = 8
        Me.AzidStereoDownmixPreamble.Text = "Stereo downmix mode: "
        '
        'AzidDialogNormalizationInfo
        '
        Me.AzidDialogNormalizationInfo.AutoSize = True
        Me.AzidDialogNormalizationInfo.Location = New System.Drawing.Point(353, 97)
        Me.AzidDialogNormalizationInfo.Name = "AzidDialogNormalizationInfo"
        Me.AzidDialogNormalizationInfo.Size = New System.Drawing.Size(51, 13)
        Me.AzidDialogNormalizationInfo.TabIndex = 7
        Me.AzidDialogNormalizationInfo.TabStop = True
        Me.AzidDialogNormalizationInfo.Text = "Details..."
        '
        'AzidDialogNormalization
        '
        Me.AzidDialogNormalization.AutoSize = True
        Me.AzidDialogNormalization.Location = New System.Drawing.Point(6, 96)
        Me.AzidDialogNormalization.Name = "AzidDialogNormalization"
        Me.AzidDialogNormalization.Size = New System.Drawing.Size(277, 17)
        Me.AzidDialogNormalization.TabIndex = 6
        Me.AzidDialogNormalization.Text = "Use dialog normalization reduction (loud commercials)"
        Me.AzidDialogNormalization.UseVisualStyleBackColor = True
        '
        'AzidRearFilterAbout
        '
        Me.AzidRearFilterAbout.AutoSize = True
        Me.AzidRearFilterAbout.Location = New System.Drawing.Point(353, 74)
        Me.AzidRearFilterAbout.Name = "AzidRearFilterAbout"
        Me.AzidRearFilterAbout.Size = New System.Drawing.Size(51, 13)
        Me.AzidRearFilterAbout.TabIndex = 5
        Me.AzidRearFilterAbout.TabStop = True
        Me.AzidRearFilterAbout.Text = "Details..."
        '
        'AzidFilterRearChannels
        '
        Me.AzidFilterRearChannels.AutoSize = True
        Me.AzidFilterRearChannels.Location = New System.Drawing.Point(6, 73)
        Me.AzidFilterRearChannels.Name = "AzidFilterRearChannels"
        Me.AzidFilterRearChannels.Size = New System.Drawing.Size(272, 17)
        Me.AzidFilterRearChannels.TabIndex = 4
        Me.AzidFilterRearChannels.Text = "Filter rear channels  (-3dB, 7kHz) for proper downmix"
        Me.AzidFilterRearChannels.UseVisualStyleBackColor = True
        '
        'AzidDRCAbout
        '
        Me.AzidDRCAbout.AutoSize = True
        Me.AzidDRCAbout.Location = New System.Drawing.Point(353, 47)
        Me.AzidDRCAbout.Name = "AzidDRCAbout"
        Me.AzidDRCAbout.Size = New System.Drawing.Size(51, 13)
        Me.AzidDRCAbout.TabIndex = 3
        Me.AzidDRCAbout.TabStop = True
        Me.AzidDRCAbout.Text = "Details..."
        '
        'AzidDRC
        '
        Me.AzidDRC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AzidDRC.FormattingEnabled = True
        Me.AzidDRC.Items.AddRange(New Object() {"none", "normal", "light", "heavy", "inverse"})
        Me.AzidDRC.Location = New System.Drawing.Point(153, 43)
        Me.AzidDRC.Name = "AzidDRC"
        Me.AzidDRC.Size = New System.Drawing.Size(194, 21)
        Me.AzidDRC.TabIndex = 2
        '
        'AzidDRCPreamble
        '
        Me.AzidDRCPreamble.AutoSize = True
        Me.AzidDRCPreamble.Location = New System.Drawing.Point(6, 47)
        Me.AzidDRCPreamble.Name = "AzidDRCPreamble"
        Me.AzidDRCPreamble.Size = New System.Drawing.Size(144, 13)
        Me.AzidDRCPreamble.TabIndex = 1
        Me.AzidDRCPreamble.Text = "Dynamic range compression:"
        '
        'AzidNormalizeAudio
        '
        Me.AzidNormalizeAudio.AutoSize = True
        Me.AzidNormalizeAudio.Location = New System.Drawing.Point(6, 20)
        Me.AzidNormalizeAudio.Name = "AzidNormalizeAudio"
        Me.AzidNormalizeAudio.Size = New System.Drawing.Size(393, 17)
        Me.AzidNormalizeAudio.TabIndex = 0
        Me.AzidNormalizeAudio.Text = "When converting audio, normalize to +0dB (sounds louder, slower conversion)"
        Me.AzidNormalizeAudio.UseVisualStyleBackColor = True
        '
        'TrackTab
        '
        Me.TrackTab.Controls.Add(Me.VideoTrackGroup)
        Me.TrackTab.Controls.Add(Me.TrackOptionsBox)
        Me.TrackTab.Controls.Add(Me.AudioTrackBox)
        Me.TrackTab.Location = New System.Drawing.Point(4, 22)
        Me.TrackTab.Name = "TrackTab"
        Me.TrackTab.Padding = New System.Windows.Forms.Padding(3)
        Me.TrackTab.Size = New System.Drawing.Size(426, 386)
        Me.TrackTab.TabIndex = 4
        Me.TrackTab.Text = "Tracks"
        Me.TrackTab.UseVisualStyleBackColor = True
        '
        'VideoTrackGroup
        '
        Me.VideoTrackGroup.Controls.Add(Me.ProvideCustomAR)
        Me.VideoTrackGroup.Controls.Add(Me.RoundPAR)
        Me.VideoTrackGroup.Location = New System.Drawing.Point(6, 55)
        Me.VideoTrackGroup.Name = "VideoTrackGroup"
        Me.VideoTrackGroup.Size = New System.Drawing.Size(412, 73)
        Me.VideoTrackGroup.TabIndex = 2
        Me.VideoTrackGroup.TabStop = False
        Me.VideoTrackGroup.Text = "Video Track"
        '
        'ProvideCustomAR
        '
        Me.ProvideCustomAR.AutoSize = True
        Me.ProvideCustomAR.Location = New System.Drawing.Point(7, 43)
        Me.ProvideCustomAR.Name = "ProvideCustomAR"
        Me.ProvideCustomAR.Size = New System.Drawing.Size(358, 17)
        Me.ProvideCustomAR.TabIndex = 1
        Me.ProvideCustomAR.Text = "Prompt me to provide a custom display aspect ratio (DAR) for each file."
        Me.ProvideCustomAR.UseVisualStyleBackColor = True
        '
        'RoundPAR
        '
        Me.RoundPAR.AutoSize = True
        Me.RoundPAR.Location = New System.Drawing.Point(7, 20)
        Me.RoundPAR.Name = "RoundPAR"
        Me.RoundPAR.Size = New System.Drawing.Size(263, 17)
        Me.RoundPAR.TabIndex = 0
        Me.RoundPAR.Text = "Round pixel aspect ratios to 1:1 if within tolerance."
        Me.RoundPAR.UseVisualStyleBackColor = True
        '
        'TrackOptionsBox
        '
        Me.TrackOptionsBox.Controls.Add(Me.MKVExtractUseSync)
        Me.TrackOptionsBox.Controls.Add(Me.AutoPickTracks)
        Me.TrackOptionsBox.Location = New System.Drawing.Point(6, 134)
        Me.TrackOptionsBox.Name = "TrackOptionsBox"
        Me.TrackOptionsBox.Size = New System.Drawing.Size(412, 69)
        Me.TrackOptionsBox.TabIndex = 1
        Me.TrackOptionsBox.TabStop = False
        Me.TrackOptionsBox.Text = "Track Options"
        '
        'MKVExtractUseSync
        '
        Me.MKVExtractUseSync.AutoSize = True
        Me.MKVExtractUseSync.Location = New System.Drawing.Point(7, 43)
        Me.MKVExtractUseSync.Name = "MKVExtractUseSync"
        Me.MKVExtractUseSync.Size = New System.Drawing.Size(309, 17)
        Me.MKVExtractUseSync.TabIndex = 1
        Me.MKVExtractUseSync.Text = "Use safe synchronization (--sync) when splitting MKV tracks."
        Me.MKVExtractUseSync.UseVisualStyleBackColor = True
        '
        'AutoPickTracks
        '
        Me.AutoPickTracks.AutoSize = True
        Me.AutoPickTracks.Location = New System.Drawing.Point(7, 20)
        Me.AutoPickTracks.Name = "AutoPickTracks"
        Me.AutoPickTracks.Size = New System.Drawing.Size(372, 17)
        Me.AutoPickTracks.TabIndex = 0
        Me.AutoPickTracks.Text = "Don't ask me about tracks. Choose the best available track automatically."
        Me.AutoPickTracks.UseVisualStyleBackColor = True
        '
        'AudioTrackBox
        '
        Me.AudioTrackBox.Controls.Add(Me.AlwaysUseLanguageList)
        Me.AudioTrackBox.Controls.Add(Me.AudioTrackLanguageInfo)
        Me.AudioTrackBox.Location = New System.Drawing.Point(6, 6)
        Me.AudioTrackBox.Name = "AudioTrackBox"
        Me.AudioTrackBox.Size = New System.Drawing.Size(412, 43)
        Me.AudioTrackBox.TabIndex = 0
        Me.AudioTrackBox.TabStop = False
        Me.AudioTrackBox.Text = "Audio Track"
        '
        'AlwaysUseLanguageList
        '
        Me.AlwaysUseLanguageList.FormattingEnabled = True
        Me.AlwaysUseLanguageList.Items.AddRange(New Object() {"", "eng", "fre", "ita", "deu", "spa"})
        Me.AlwaysUseLanguageList.Location = New System.Drawing.Point(191, 14)
        Me.AlwaysUseLanguageList.Name = "AlwaysUseLanguageList"
        Me.AlwaysUseLanguageList.Size = New System.Drawing.Size(121, 21)
        Me.AlwaysUseLanguageList.TabIndex = 1
        '
        'AudioTrackLanguageInfo
        '
        Me.AudioTrackLanguageInfo.AutoSize = True
        Me.AudioTrackLanguageInfo.Location = New System.Drawing.Point(6, 17)
        Me.AudioTrackLanguageInfo.Name = "AudioTrackLanguageInfo"
        Me.AudioTrackLanguageInfo.Size = New System.Drawing.Size(177, 13)
        Me.AudioTrackLanguageInfo.TabIndex = 0
        Me.AudioTrackLanguageInfo.Text = "Always use the selected language: "
        '
        'FilesAndFoldersTab
        '
        Me.FilesAndFoldersTab.Controls.Add(Me.XB360RefFramesBox)
        Me.FilesAndFoldersTab.Controls.Add(Me.ScratchDiskGroup)
        Me.FilesAndFoldersTab.Controls.Add(Me.FileSplitGroup)
        Me.FilesAndFoldersTab.Location = New System.Drawing.Point(4, 22)
        Me.FilesAndFoldersTab.Name = "FilesAndFoldersTab"
        Me.FilesAndFoldersTab.Padding = New System.Windows.Forms.Padding(3)
        Me.FilesAndFoldersTab.Size = New System.Drawing.Size(426, 386)
        Me.FilesAndFoldersTab.TabIndex = 2
        Me.FilesAndFoldersTab.Text = "Files and Folders"
        Me.FilesAndFoldersTab.UseVisualStyleBackColor = True
        '
        'XB360RefFramesBox
        '
        Me.XB360RefFramesBox.Controls.Add(Me.ContinueOnIncompatible)
        Me.XB360RefFramesBox.Controls.Add(Me.HaltOnIncompatible)
        Me.XB360RefFramesBox.Controls.Add(Me.XB360RefFrameInfo)
        Me.XB360RefFramesBox.Location = New System.Drawing.Point(6, 271)
        Me.XB360RefFramesBox.Name = "XB360RefFramesBox"
        Me.XB360RefFramesBox.Size = New System.Drawing.Size(412, 110)
        Me.XB360RefFramesBox.TabIndex = 4
        Me.XB360RefFramesBox.TabStop = False
        Me.XB360RefFramesBox.Text = "Xbox 360 File Compatibility"
        '
        'ContinueOnIncompatible
        '
        Me.ContinueOnIncompatible.AutoSize = True
        Me.ContinueOnIncompatible.Location = New System.Drawing.Point(10, 76)
        Me.ContinueOnIncompatible.Name = "ContinueOnIncompatible"
        Me.ContinueOnIncompatible.Size = New System.Drawing.Size(291, 17)
        Me.ContinueOnIncompatible.TabIndex = 2
        Me.ContinueOnIncompatible.TabStop = True
        Me.ContinueOnIncompatible.Text = "Warn me about these files but continue to process them."
        Me.ContinueOnIncompatible.UseVisualStyleBackColor = True
        '
        'HaltOnIncompatible
        '
        Me.HaltOnIncompatible.AutoSize = True
        Me.HaltOnIncompatible.Location = New System.Drawing.Point(10, 53)
        Me.HaltOnIncompatible.Name = "HaltOnIncompatible"
        Me.HaltOnIncompatible.Size = New System.Drawing.Size(363, 17)
        Me.HaltOnIncompatible.TabIndex = 1
        Me.HaltOnIncompatible.TabStop = True
        Me.HaltOnIncompatible.Text = "Don't process these files. If processing a folder, continue to the next file."
        Me.HaltOnIncompatible.UseVisualStyleBackColor = True
        '
        'XB360RefFrameInfo
        '
        Me.XB360RefFrameInfo.Location = New System.Drawing.Point(7, 21)
        Me.XB360RefFrameInfo.Name = "XB360RefFrameInfo"
        Me.XB360RefFrameInfo.Size = New System.Drawing.Size(389, 28)
        Me.XB360RefFrameInfo.TabIndex = 0
        Me.XB360RefFrameInfo.Text = "Some files will not play properly on the Xbox 360 due to their reference frame co" & _
            "unt. These files will need to be re-encoded outside of XenonMKV. "
        '
        'ScratchDiskGroup
        '
        Me.ScratchDiskGroup.Controls.Add(Me.BrowseScratchLocation)
        Me.ScratchDiskGroup.Controls.Add(Me.ScratchFolder)
        Me.ScratchDiskGroup.Controls.Add(Me.ScratchFolderDesc)
        Me.ScratchDiskGroup.Location = New System.Drawing.Point(6, 174)
        Me.ScratchDiskGroup.Name = "ScratchDiskGroup"
        Me.ScratchDiskGroup.Size = New System.Drawing.Size(412, 91)
        Me.ScratchDiskGroup.TabIndex = 3
        Me.ScratchDiskGroup.TabStop = False
        Me.ScratchDiskGroup.Text = "Scratch Location"
        '
        'BrowseScratchLocation
        '
        Me.BrowseScratchLocation.Image = Global.XenonMKV.My.Resources.Resources.folder_explore
        Me.BrowseScratchLocation.Location = New System.Drawing.Point(383, 63)
        Me.BrowseScratchLocation.Name = "BrowseScratchLocation"
        Me.BrowseScratchLocation.Size = New System.Drawing.Size(23, 23)
        Me.BrowseScratchLocation.TabIndex = 3
        Me.BrowseScratchLocation.UseVisualStyleBackColor = True
        '
        'ScratchFolder
        '
        Me.ScratchFolder.Location = New System.Drawing.Point(8, 65)
        Me.ScratchFolder.Name = "ScratchFolder"
        Me.ScratchFolder.Size = New System.Drawing.Size(367, 21)
        Me.ScratchFolder.TabIndex = 2
        '
        'ScratchFolderDesc
        '
        Me.ScratchFolderDesc.Location = New System.Drawing.Point(7, 21)
        Me.ScratchFolderDesc.Name = "ScratchFolderDesc"
        Me.ScratchFolderDesc.Size = New System.Drawing.Size(399, 41)
        Me.ScratchFolderDesc.TabIndex = 0
        Me.ScratchFolderDesc.Text = resources.GetString("ScratchFolderDesc.Text")
        '
        'FileSplitGroup
        '
        Me.FileSplitGroup.Controls.Add(Me.SplitFile_Time)
        Me.FileSplitGroup.Controls.Add(Me.SplitFile_ThresholdValue)
        Me.FileSplitGroup.Controls.Add(Me.SplitFile_Threshold)
        Me.FileSplitGroup.Controls.Add(Me.SplitFile_None)
        Me.FileSplitGroup.Controls.Add(Me.SplitFile_Default)
        Me.FileSplitGroup.Controls.Add(Me.FileSplittingInfo)
        Me.FileSplitGroup.Location = New System.Drawing.Point(6, 6)
        Me.FileSplitGroup.Name = "FileSplitGroup"
        Me.FileSplitGroup.Size = New System.Drawing.Size(412, 162)
        Me.FileSplitGroup.TabIndex = 2
        Me.FileSplitGroup.TabStop = False
        Me.FileSplitGroup.Text = "File Splitting"
        '
        'SplitFile_Time
        '
        Me.SplitFile_Time.AutoSize = True
        Me.SplitFile_Time.Location = New System.Drawing.Point(6, 112)
        Me.SplitFile_Time.Name = "SplitFile_Time"
        Me.SplitFile_Time.Size = New System.Drawing.Size(337, 17)
        Me.SplitFile_Time.TabIndex = 5
        Me.SplitFile_Time.TabStop = True
        Me.SplitFile_Time.Text = "Don't split files automatically. Let me choose a time to cut them off."
        Me.SplitFile_Time.UseVisualStyleBackColor = True
        '
        'SplitFile_ThresholdValue
        '
        Me.SplitFile_ThresholdValue.Location = New System.Drawing.Point(121, 134)
        Me.SplitFile_ThresholdValue.MaxLength = 16
        Me.SplitFile_ThresholdValue.Name = "SplitFile_ThresholdValue"
        Me.SplitFile_ThresholdValue.Size = New System.Drawing.Size(58, 21)
        Me.SplitFile_ThresholdValue.TabIndex = 4
        '
        'SplitFile_Threshold
        '
        Me.SplitFile_Threshold.AutoSize = True
        Me.SplitFile_Threshold.Location = New System.Drawing.Point(6, 135)
        Me.SplitFile_Threshold.Name = "SplitFile_Threshold"
        Me.SplitFile_Threshold.Size = New System.Drawing.Size(234, 17)
        Me.SplitFile_Threshold.TabIndex = 3
        Me.SplitFile_Threshold.TabStop = True
        Me.SplitFile_Threshold.Text = "Only split files over                       MiB in size."
        Me.SplitFile_Threshold.UseVisualStyleBackColor = True
        '
        'SplitFile_None
        '
        Me.SplitFile_None.AutoSize = True
        Me.SplitFile_None.Location = New System.Drawing.Point(6, 89)
        Me.SplitFile_None.Name = "SplitFile_None"
        Me.SplitFile_None.Size = New System.Drawing.Size(224, 17)
        Me.SplitFile_None.TabIndex = 2
        Me.SplitFile_None.TabStop = True
        Me.SplitFile_None.Text = "Don't split files, even if they are over 4GiB."
        Me.SplitFile_None.UseVisualStyleBackColor = True
        '
        'SplitFile_Default
        '
        Me.SplitFile_Default.AutoSize = True
        Me.SplitFile_Default.Location = New System.Drawing.Point(6, 66)
        Me.SplitFile_Default.Name = "SplitFile_Default"
        Me.SplitFile_Default.Size = New System.Drawing.Size(364, 17)
        Me.SplitFile_Default.TabIndex = 1
        Me.SplitFile_Default.TabStop = True
        Me.SplitFile_Default.Text = "Split files over 4096MB into equal chunks that will play on the Xbox 360."
        Me.SplitFile_Default.UseVisualStyleBackColor = True
        '
        'FileSplittingInfo
        '
        Me.FileSplittingInfo.Location = New System.Drawing.Point(7, 21)
        Me.FileSplittingInfo.Name = "FileSplittingInfo"
        Me.FileSplittingInfo.Size = New System.Drawing.Size(399, 42)
        Me.FileSplittingInfo.TabIndex = 0
        Me.FileSplittingInfo.Text = resources.GetString("FileSplittingInfo.Text")
        '
        'UITab
        '
        Me.UITab.Controls.Add(Me.GroupBox1)
        Me.UITab.Controls.Add(Me.LanguageFrame)
        Me.UITab.Location = New System.Drawing.Point(4, 22)
        Me.UITab.Name = "UITab"
        Me.UITab.Padding = New System.Windows.Forms.Padding(3)
        Me.UITab.Size = New System.Drawing.Size(426, 386)
        Me.UITab.TabIndex = 3
        Me.UITab.Text = "Interface"
        Me.UITab.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ResetMainUIWindowMetrics)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 58)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(410, 54)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Window Size and Position"
        '
        'ResetMainUIWindowMetrics
        '
        Me.ResetMainUIWindowMetrics.Location = New System.Drawing.Point(6, 20)
        Me.ResetMainUIWindowMetrics.Name = "ResetMainUIWindowMetrics"
        Me.ResetMainUIWindowMetrics.Size = New System.Drawing.Size(201, 23)
        Me.ResetMainUIWindowMetrics.TabIndex = 0
        Me.ResetMainUIWindowMetrics.Text = "Reset Window Size and Position"
        Me.ResetMainUIWindowMetrics.UseVisualStyleBackColor = True
        '
        'LanguageFrame
        '
        Me.LanguageFrame.Controls.Add(Me.LanguageRestartInfo)
        Me.LanguageFrame.Controls.Add(Me.LanguageSelector)
        Me.LanguageFrame.Location = New System.Drawing.Point(8, 6)
        Me.LanguageFrame.Name = "LanguageFrame"
        Me.LanguageFrame.Size = New System.Drawing.Size(410, 46)
        Me.LanguageFrame.TabIndex = 0
        Me.LanguageFrame.TabStop = False
        Me.LanguageFrame.Text = "Language"
        '
        'LanguageRestartInfo
        '
        Me.LanguageRestartInfo.AutoSize = True
        Me.LanguageRestartInfo.Location = New System.Drawing.Point(152, 22)
        Me.LanguageRestartInfo.Name = "LanguageRestartInfo"
        Me.LanguageRestartInfo.Size = New System.Drawing.Size(240, 13)
        Me.LanguageRestartInfo.TabIndex = 1
        Me.LanguageRestartInfo.Text = "Save and restart XenonMKV to view the change."
        '
        'LanguageSelector
        '
        Me.LanguageSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LanguageSelector.FormattingEnabled = True
        Me.LanguageSelector.Location = New System.Drawing.Point(6, 19)
        Me.LanguageSelector.Name = "LanguageSelector"
        Me.LanguageSelector.Size = New System.Drawing.Size(140, 21)
        Me.LanguageSelector.TabIndex = 0
        '
        'OptionTabContainer
        '
        Me.OptionTabContainer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.OptionTabContainer.Controls.Add(Me.FilesAndFoldersTab)
        Me.OptionTabContainer.Controls.Add(Me.TrackTab)
        Me.OptionTabContainer.Controls.Add(Me.AudioTab)
        Me.OptionTabContainer.Controls.Add(Me.NeroAACTab)
        Me.OptionTabContainer.Controls.Add(Me.LastStepsTab)
        Me.OptionTabContainer.Controls.Add(Me.UITab)
        Me.OptionTabContainer.Location = New System.Drawing.Point(0, 0)
        Me.OptionTabContainer.Name = "OptionTabContainer"
        Me.OptionTabContainer.SelectedIndex = 0
        Me.OptionTabContainer.Size = New System.Drawing.Size(434, 412)
        Me.OptionTabContainer.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.OptionTabContainer.TabIndex = 0
        '
        'NeroAACTab
        '
        Me.NeroAACTab.Controls.Add(Me.NeroAACEncoderBox)
        Me.NeroAACTab.Location = New System.Drawing.Point(4, 22)
        Me.NeroAACTab.Name = "NeroAACTab"
        Me.NeroAACTab.Padding = New System.Windows.Forms.Padding(3)
        Me.NeroAACTab.Size = New System.Drawing.Size(426, 386)
        Me.NeroAACTab.TabIndex = 5
        Me.NeroAACTab.Text = "Nero AAC"
        Me.NeroAACTab.UseVisualStyleBackColor = True
        '
        'NeroAACEncoderBox
        '
        Me.NeroAACEncoderBox.Controls.Add(Me.Quality_CBR)
        Me.NeroAACEncoderBox.Controls.Add(Me.Quality_BR_Panel)
        Me.NeroAACEncoderBox.Controls.Add(Me.Quality_Q_Panel)
        Me.NeroAACEncoderBox.Controls.Add(Me.Quality_BR)
        Me.NeroAACEncoderBox.Controls.Add(Me.Quality_Q)
        Me.NeroAACEncoderBox.Location = New System.Drawing.Point(6, 6)
        Me.NeroAACEncoderBox.Name = "NeroAACEncoderBox"
        Me.NeroAACEncoderBox.Size = New System.Drawing.Size(412, 374)
        Me.NeroAACEncoderBox.TabIndex = 2
        Me.NeroAACEncoderBox.TabStop = False
        Me.NeroAACEncoderBox.Text = "Nero AAC Encoder"
        '
        'Quality_CBR
        '
        Me.Quality_CBR.AutoSize = True
        Me.Quality_CBR.Location = New System.Drawing.Point(12, 194)
        Me.Quality_CBR.Name = "Quality_CBR"
        Me.Quality_CBR.Size = New System.Drawing.Size(195, 17)
        Me.Quality_CBR.TabIndex = 11
        Me.Quality_CBR.TabStop = True
        Me.Quality_CBR.Text = "Target bitrate, streaming mode (-cbr)"
        Me.Quality_CBR.UseVisualStyleBackColor = True
        '
        'Quality_BR_Panel
        '
        Me.Quality_BR_Panel.Controls.Add(Me.TargetBitrateKbps)
        Me.Quality_BR_Panel.Controls.Add(Me.Quality_BR_Value)
        Me.Quality_BR_Panel.Controls.Add(Me.TargetBitrateInfo)
        Me.Quality_BR_Panel.Location = New System.Drawing.Point(30, 217)
        Me.Quality_BR_Panel.Name = "Quality_BR_Panel"
        Me.Quality_BR_Panel.Size = New System.Drawing.Size(376, 59)
        Me.Quality_BR_Panel.TabIndex = 10
        '
        'TargetBitrateKbps
        '
        Me.TargetBitrateKbps.AutoSize = True
        Me.TargetBitrateKbps.Location = New System.Drawing.Point(84, 30)
        Me.TargetBitrateKbps.Name = "TargetBitrateKbps"
        Me.TargetBitrateKbps.Size = New System.Drawing.Size(29, 13)
        Me.TargetBitrateKbps.TabIndex = 2
        Me.TargetBitrateKbps.Text = "kbps"
        '
        'Quality_BR_Value
        '
        Me.Quality_BR_Value.FormattingEnabled = True
        Me.Quality_BR_Value.Items.AddRange(New Object() {"80", "96", "128", "160", "192", "224", "256", "320"})
        Me.Quality_BR_Value.Location = New System.Drawing.Point(12, 27)
        Me.Quality_BR_Value.Name = "Quality_BR_Value"
        Me.Quality_BR_Value.Size = New System.Drawing.Size(66, 21)
        Me.Quality_BR_Value.TabIndex = 1
        '
        'TargetBitrateInfo
        '
        Me.TargetBitrateInfo.AutoSize = True
        Me.TargetBitrateInfo.Location = New System.Drawing.Point(9, 10)
        Me.TargetBitrateInfo.Name = "TargetBitrateInfo"
        Me.TargetBitrateInfo.Size = New System.Drawing.Size(358, 13)
        Me.TargetBitrateInfo.TabIndex = 0
        Me.TargetBitrateInfo.Text = "In target bitrate mode, you specify the bitrate (kbps) for the conversion."
        '
        'Quality_Q_Panel
        '
        Me.Quality_Q_Panel.Controls.Add(Me.NeroEncoderInfo)
        Me.Quality_Q_Panel.Controls.Add(Me.NeroQualityValue)
        Me.Quality_Q_Panel.Controls.Add(Me.NeroQualityBar)
        Me.Quality_Q_Panel.Location = New System.Drawing.Point(30, 42)
        Me.Quality_Q_Panel.Name = "Quality_Q_Panel"
        Me.Quality_Q_Panel.Size = New System.Drawing.Size(376, 117)
        Me.Quality_Q_Panel.TabIndex = 9
        '
        'NeroEncoderInfo
        '
        Me.NeroEncoderInfo.Location = New System.Drawing.Point(9, 9)
        Me.NeroEncoderInfo.Name = "NeroEncoderInfo"
        Me.NeroEncoderInfo.Size = New System.Drawing.Size(354, 33)
        Me.NeroEncoderInfo.TabIndex = 8
        Me.NeroEncoderInfo.Text = "In target quality mode, larger values provide better quality, but increase the ou" & _
            "tput filesize. The default is 0.5."
        '
        'NeroQualityValue
        '
        Me.NeroQualityValue.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NeroQualityValue.Location = New System.Drawing.Point(12, 93)
        Me.NeroQualityValue.Name = "NeroQualityValue"
        Me.NeroQualityValue.Size = New System.Drawing.Size(351, 15)
        Me.NeroQualityValue.TabIndex = 7
        Me.NeroQualityValue.Text = "0.5"
        Me.NeroQualityValue.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'NeroQualityBar
        '
        Me.NeroQualityBar.LargeChange = 3
        Me.NeroQualityBar.Location = New System.Drawing.Point(12, 45)
        Me.NeroQualityBar.Minimum = 1
        Me.NeroQualityBar.Name = "NeroQualityBar"
        Me.NeroQualityBar.Size = New System.Drawing.Size(351, 45)
        Me.NeroQualityBar.TabIndex = 6
        Me.NeroQualityBar.Value = 5
        '
        'Quality_BR
        '
        Me.Quality_BR.AutoSize = True
        Me.Quality_BR.Location = New System.Drawing.Point(12, 171)
        Me.Quality_BR.Name = "Quality_BR"
        Me.Quality_BR.Size = New System.Drawing.Size(138, 17)
        Me.Quality_BR.TabIndex = 8
        Me.Quality_BR.TabStop = True
        Me.Quality_BR.Text = "Target bitrate mode (-br)"
        Me.Quality_BR.UseVisualStyleBackColor = True
        '
        'Quality_Q
        '
        Me.Quality_Q.AutoSize = True
        Me.Quality_Q.Location = New System.Drawing.Point(9, 19)
        Me.Quality_Q.Name = "Quality_Q"
        Me.Quality_Q.Size = New System.Drawing.Size(177, 17)
        Me.Quality_Q.TabIndex = 7
        Me.Quality_Q.TabStop = True
        Me.Quality_Q.Text = "Target quality mode (-q) (default)"
        Me.Quality_Q.UseVisualStyleBackColor = True
        '
        'OptionsDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(434, 444)
        Me.Controls.Add(Me.SaveOptions)
        Me.Controls.Add(Me.OptionTabContainer)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "OptionsDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XenonMKV Options"
        Me.LastStepsTab.ResumeLayout(False)
        Me.MP4BoxOutputMethodGroup.ResumeLayout(False)
        Me.MP4BoxOutputMethodGroup.PerformLayout()
        Me.MP4Box_Latest_Group.ResumeLayout(False)
        Me.MP4Box_Latest_Group.PerformLayout()
        Me.RenameOutputGroup.ResumeLayout(False)
        Me.RenameOutputGroup.PerformLayout()
        Me.AudioTab.ResumeLayout(False)
        Me.AzidFrame.ResumeLayout(False)
        Me.AzidFrame.PerformLayout()
        Me.TrackTab.ResumeLayout(False)
        Me.VideoTrackGroup.ResumeLayout(False)
        Me.VideoTrackGroup.PerformLayout()
        Me.TrackOptionsBox.ResumeLayout(False)
        Me.TrackOptionsBox.PerformLayout()
        Me.AudioTrackBox.ResumeLayout(False)
        Me.AudioTrackBox.PerformLayout()
        Me.FilesAndFoldersTab.ResumeLayout(False)
        Me.XB360RefFramesBox.ResumeLayout(False)
        Me.XB360RefFramesBox.PerformLayout()
        Me.ScratchDiskGroup.ResumeLayout(False)
        Me.ScratchDiskGroup.PerformLayout()
        Me.FileSplitGroup.ResumeLayout(False)
        Me.FileSplitGroup.PerformLayout()
        Me.UITab.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.LanguageFrame.ResumeLayout(False)
        Me.LanguageFrame.PerformLayout()
        Me.OptionTabContainer.ResumeLayout(False)
        Me.NeroAACTab.ResumeLayout(False)
        Me.NeroAACEncoderBox.ResumeLayout(False)
        Me.NeroAACEncoderBox.PerformLayout()
        Me.Quality_BR_Panel.ResumeLayout(False)
        Me.Quality_BR_Panel.PerformLayout()
        Me.Quality_Q_Panel.ResumeLayout(False)
        Me.Quality_Q_Panel.PerformLayout()
        CType(Me.NeroQualityBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SaveOptions As System.Windows.Forms.Button
    Friend WithEvents ScratchFolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents LastStepsTab As System.Windows.Forms.TabPage
    Friend WithEvents RenameOutputGroup As System.Windows.Forms.GroupBox
    Friend WithEvents RenameOutputExtension As System.Windows.Forms.TextBox
    Friend WithEvents RenameOutputCheck As System.Windows.Forms.CheckBox
    Friend WithEvents RenameOutputDesc As System.Windows.Forms.Label
    Friend WithEvents AudioTab As System.Windows.Forms.TabPage
    Friend WithEvents AzidFrame As System.Windows.Forms.GroupBox
    Friend WithEvents AzidNormalizeAudio As System.Windows.Forms.CheckBox
    Friend WithEvents TrackTab As System.Windows.Forms.TabPage
    Friend WithEvents TrackOptionsBox As System.Windows.Forms.GroupBox
    Friend WithEvents AutoPickTracks As System.Windows.Forms.CheckBox
    Friend WithEvents AudioTrackBox As System.Windows.Forms.GroupBox
    Friend WithEvents AlwaysUseLanguageList As System.Windows.Forms.ComboBox
    Friend WithEvents AudioTrackLanguageInfo As System.Windows.Forms.Label
    Friend WithEvents FilesAndFoldersTab As System.Windows.Forms.TabPage
    Friend WithEvents ScratchDiskGroup As System.Windows.Forms.GroupBox
    Friend WithEvents BrowseScratchLocation As System.Windows.Forms.Button
    Friend WithEvents ScratchFolder As System.Windows.Forms.TextBox
    Friend WithEvents ScratchFolderDesc As System.Windows.Forms.Label
    Friend WithEvents FileSplitGroup As System.Windows.Forms.GroupBox
    Friend WithEvents SplitFile_Time As System.Windows.Forms.RadioButton
    Friend WithEvents SplitFile_ThresholdValue As System.Windows.Forms.TextBox
    Friend WithEvents SplitFile_Threshold As System.Windows.Forms.RadioButton
    Friend WithEvents SplitFile_None As System.Windows.Forms.RadioButton
    Friend WithEvents SplitFile_Default As System.Windows.Forms.RadioButton
    Friend WithEvents FileSplittingInfo As System.Windows.Forms.Label
    Friend WithEvents UITab As System.Windows.Forms.TabPage
    Friend WithEvents LanguageFrame As System.Windows.Forms.GroupBox
    Friend WithEvents LanguageRestartInfo As System.Windows.Forms.Label
    Friend WithEvents LanguageSelector As System.Windows.Forms.ComboBox
    Friend WithEvents OptionTabContainer As System.Windows.Forms.TabControl
    Friend WithEvents AzidDRC As System.Windows.Forms.ComboBox
    Friend WithEvents AzidDRCPreamble As System.Windows.Forms.Label
    Friend WithEvents AzidDRCAbout As System.Windows.Forms.LinkLabel
    Friend WithEvents AzidFilterRearChannels As System.Windows.Forms.CheckBox
    Friend WithEvents AzidRearFilterAbout As System.Windows.Forms.LinkLabel
    Friend WithEvents AzidDialogNormalizationInfo As System.Windows.Forms.LinkLabel
    Friend WithEvents AzidDialogNormalization As System.Windows.Forms.CheckBox
    Friend WithEvents AzidStereoDownmixInfo As System.Windows.Forms.LinkLabel
    Friend WithEvents AzidStereoDownmix As System.Windows.Forms.ComboBox
    Friend WithEvents AzidStereoDownmixPreamble As System.Windows.Forms.Label
    Friend WithEvents XB360RefFramesBox As System.Windows.Forms.GroupBox
    Friend WithEvents ContinueOnIncompatible As System.Windows.Forms.RadioButton
    Friend WithEvents HaltOnIncompatible As System.Windows.Forms.RadioButton
    Friend WithEvents XB360RefFrameInfo As System.Windows.Forms.Label
    Friend WithEvents VideoTrackGroup As System.Windows.Forms.GroupBox
    Friend WithEvents RoundPAR As System.Windows.Forms.CheckBox
    Friend WithEvents MP4Box_Latest_Group As System.Windows.Forms.GroupBox
    Friend WithEvents MP4Box_NewestBuild_Info As System.Windows.Forms.Label
    Friend WithEvents MP4BoxApril13Build As System.Windows.Forms.RadioButton
    Friend WithEvents MP4BoxDefaultBuild As System.Windows.Forms.RadioButton
    Friend WithEvents MP4Box044_2007 As System.Windows.Forms.RadioButton
    Friend WithEvents MP4BoxOutputMethodGroup As System.Windows.Forms.GroupBox
    Friend WithEvents MP4BoxOutputDirect As System.Windows.Forms.RadioButton
    Friend WithEvents MP4BoxOutputMove As System.Windows.Forms.RadioButton
    Friend WithEvents MKVExtractUseSync As System.Windows.Forms.CheckBox
    Friend WithEvents ProvideCustomAR As System.Windows.Forms.CheckBox
    Friend WithEvents NeroAACTab As System.Windows.Forms.TabPage
    Friend WithEvents NeroAACEncoderBox As System.Windows.Forms.GroupBox
    Friend WithEvents Quality_BR_Panel As System.Windows.Forms.Panel
    Friend WithEvents Quality_Q_Panel As System.Windows.Forms.Panel
    Friend WithEvents NeroEncoderInfo As System.Windows.Forms.Label
    Friend WithEvents NeroQualityValue As System.Windows.Forms.Label
    Friend WithEvents NeroQualityBar As System.Windows.Forms.TrackBar
    Friend WithEvents Quality_BR As System.Windows.Forms.RadioButton
    Friend WithEvents Quality_Q As System.Windows.Forms.RadioButton
    Friend WithEvents TargetBitrateInfo As System.Windows.Forms.Label
    Friend WithEvents Quality_CBR As System.Windows.Forms.RadioButton
    Friend WithEvents TargetBitrateKbps As System.Windows.Forms.Label
    Friend WithEvents Quality_BR_Value As System.Windows.Forms.ComboBox
    Friend WithEvents UppercaseOutputCheck As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ResetMainUIWindowMetrics As System.Windows.Forms.Button
End Class
