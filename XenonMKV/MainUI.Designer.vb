<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainUI
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainUI))
        Me.ProgressBar = New System.Windows.Forms.ProgressBar()
        Me.StartRecode = New System.Windows.Forms.Button()
        Me.IsTalkative = New System.Windows.Forms.CheckBox()
        Me.OpenFile = New System.Windows.Forms.OpenFileDialog()
        Me.WarningPanel = New System.Windows.Forms.Panel()
        Me.WarningText = New System.Windows.Forms.Label()
        Me.WarningPicture = New System.Windows.Forms.PictureBox()
        Me.InputPanel = New System.Windows.Forms.Panel()
        Me.PreviewMedia = New System.Windows.Forms.Button()
        Me.BrowseSingleFile = New System.Windows.Forms.Button()
        Me.SingleFileName = New System.Windows.Forms.TextBox()
        Me.SourceType = New System.Windows.Forms.Label()
        Me.OutputFolderPanel = New System.Windows.Forms.Panel()
        Me.ViewOutputFolder = New System.Windows.Forms.Button()
        Me.BrowseOutputFolder = New System.Windows.Forms.Button()
        Me.OutputFolder = New System.Windows.Forms.TextBox()
        Me.OutputFolderTitle = New System.Windows.Forms.Label()
        Me.OutputFolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.ScrollLog = New System.Windows.Forms.CheckBox()
        Me.ConversionTypePanel = New System.Windows.Forms.Panel()
        Me.ConvertFolder = New System.Windows.Forms.RadioButton()
        Me.ConvertSingle = New System.Windows.Forms.RadioButton()
        Me.InputFolderBrowser = New System.Windows.Forms.FolderBrowserDialog()
        Me.DefaultMenu = New System.Windows.Forms.MenuStrip()
        Me.ToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnableMP4PlaybackInMediaPlayerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteTemporaryFilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoveOutputToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AllowNetworkPathToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator_Tools = New System.Windows.Forms.ToolStripSeparator()
        Me.MKVInformationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OptionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WebsiteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ForumToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckForUpdateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator_Help = New System.Windows.Forms.ToolStripSeparator()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TrayIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.TrayMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.HideShowXenonMKVToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SToolStripMenuItem = New System.Windows.Forms.ToolStripSeparator()
        Me.ExitToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tmrTooltip = New System.Windows.Forms.Timer(Me.components)
        Me.CancelProcess = New System.Windows.Forms.Button()
        Me.Log = New System.Windows.Forms.ListView()
        Me.LogIcon = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.LogImages = New System.Windows.Forms.ImageList(Me.components)
        Me.btnOptions = New System.Windows.Forms.Button()
        Me.WarningPanel.SuspendLayout()
        CType(Me.WarningPicture, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InputPanel.SuspendLayout()
        Me.OutputFolderPanel.SuspendLayout()
        Me.ConversionTypePanel.SuspendLayout()
        Me.DefaultMenu.SuspendLayout()
        Me.TrayMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'ProgressBar
        '
        Me.ProgressBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBar.Location = New System.Drawing.Point(7, 365)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(605, 27)
        Me.ProgressBar.TabIndex = 3
        '
        'StartRecode
        '
        Me.StartRecode.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StartRecode.Enabled = False
        Me.StartRecode.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StartRecode.Image = Global.XenonMKV.My.Resources.Resources.accept
        Me.StartRecode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.StartRecode.Location = New System.Drawing.Point(513, 398)
        Me.StartRecode.Name = "StartRecode"
        Me.StartRecode.Size = New System.Drawing.Size(99, 33)
        Me.StartRecode.TabIndex = 4
        Me.StartRecode.Text = "Start"
        Me.StartRecode.UseVisualStyleBackColor = True
        '
        'IsTalkative
        '
        Me.IsTalkative.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.IsTalkative.AutoSize = True
        Me.IsTalkative.Checked = True
        Me.IsTalkative.CheckState = System.Windows.Forms.CheckState.Checked
        Me.IsTalkative.Location = New System.Drawing.Point(7, 398)
        Me.IsTalkative.Name = "IsTalkative"
        Me.IsTalkative.Size = New System.Drawing.Size(112, 17)
        Me.IsTalkative.TabIndex = 5
        Me.IsTalkative.Text = "Log all tool output"
        Me.IsTalkative.UseVisualStyleBackColor = True
        '
        'OpenFile
        '
        Me.OpenFile.DefaultExt = "mkv"
        Me.OpenFile.Filter = "MKV Files (*.mkv)|*.mkv"
        '
        'WarningPanel
        '
        Me.WarningPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WarningPanel.Controls.Add(Me.WarningText)
        Me.WarningPanel.Controls.Add(Me.WarningPicture)
        Me.WarningPanel.Location = New System.Drawing.Point(7, 125)
        Me.WarningPanel.Name = "WarningPanel"
        Me.WarningPanel.Size = New System.Drawing.Size(605, 25)
        Me.WarningPanel.TabIndex = 9
        Me.WarningPanel.Visible = False
        '
        'WarningText
        '
        Me.WarningText.AutoSize = True
        Me.WarningText.Location = New System.Drawing.Point(27, 6)
        Me.WarningText.Name = "WarningText"
        Me.WarningText.Size = New System.Drawing.Size(0, 13)
        Me.WarningText.TabIndex = 1
        '
        'WarningPicture
        '
        Me.WarningPicture.Image = CType(resources.GetObject("WarningPicture.Image"), System.Drawing.Image)
        Me.WarningPicture.Location = New System.Drawing.Point(3, 3)
        Me.WarningPicture.Name = "WarningPicture"
        Me.WarningPicture.Size = New System.Drawing.Size(18, 19)
        Me.WarningPicture.TabIndex = 0
        Me.WarningPicture.TabStop = False
        '
        'InputPanel
        '
        Me.InputPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InputPanel.Controls.Add(Me.PreviewMedia)
        Me.InputPanel.Controls.Add(Me.BrowseSingleFile)
        Me.InputPanel.Controls.Add(Me.SingleFileName)
        Me.InputPanel.Controls.Add(Me.SourceType)
        Me.InputPanel.Location = New System.Drawing.Point(8, 58)
        Me.InputPanel.Name = "InputPanel"
        Me.InputPanel.Size = New System.Drawing.Size(604, 26)
        Me.InputPanel.TabIndex = 10
        '
        'PreviewMedia
        '
        Me.PreviewMedia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PreviewMedia.Image = Global.XenonMKV.My.Resources.Resources.monitor
        Me.PreviewMedia.Location = New System.Drawing.Point(547, 2)
        Me.PreviewMedia.Name = "PreviewMedia"
        Me.PreviewMedia.Size = New System.Drawing.Size(24, 23)
        Me.PreviewMedia.TabIndex = 12
        Me.PreviewMedia.UseVisualStyleBackColor = True
        '
        'BrowseSingleFile
        '
        Me.BrowseSingleFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BrowseSingleFile.Image = Global.XenonMKV.My.Resources.Resources.folder_explore
        Me.BrowseSingleFile.Location = New System.Drawing.Point(577, 2)
        Me.BrowseSingleFile.Name = "BrowseSingleFile"
        Me.BrowseSingleFile.Size = New System.Drawing.Size(24, 23)
        Me.BrowseSingleFile.TabIndex = 11
        Me.BrowseSingleFile.UseVisualStyleBackColor = True
        '
        'SingleFileName
        '
        Me.SingleFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SingleFileName.Location = New System.Drawing.Point(109, 2)
        Me.SingleFileName.Name = "SingleFileName"
        Me.SingleFileName.Size = New System.Drawing.Size(433, 21)
        Me.SingleFileName.TabIndex = 9
        '
        'SourceType
        '
        Me.SourceType.Location = New System.Drawing.Point(7, 5)
        Me.SourceType.Name = "SourceType"
        Me.SourceType.Size = New System.Drawing.Size(96, 18)
        Me.SourceType.TabIndex = 7
        Me.SourceType.Text = "Single MKV file:"
        Me.SourceType.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'OutputFolderPanel
        '
        Me.OutputFolderPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OutputFolderPanel.Controls.Add(Me.ViewOutputFolder)
        Me.OutputFolderPanel.Controls.Add(Me.BrowseOutputFolder)
        Me.OutputFolderPanel.Controls.Add(Me.OutputFolder)
        Me.OutputFolderPanel.Controls.Add(Me.OutputFolderTitle)
        Me.OutputFolderPanel.Location = New System.Drawing.Point(8, 90)
        Me.OutputFolderPanel.Name = "OutputFolderPanel"
        Me.OutputFolderPanel.Size = New System.Drawing.Size(604, 29)
        Me.OutputFolderPanel.TabIndex = 11
        '
        'ViewOutputFolder
        '
        Me.ViewOutputFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ViewOutputFolder.Image = Global.XenonMKV.My.Resources.Resources.zoom
        Me.ViewOutputFolder.Location = New System.Drawing.Point(547, 3)
        Me.ViewOutputFolder.Name = "ViewOutputFolder"
        Me.ViewOutputFolder.Size = New System.Drawing.Size(24, 23)
        Me.ViewOutputFolder.TabIndex = 3
        Me.ViewOutputFolder.UseVisualStyleBackColor = True
        '
        'BrowseOutputFolder
        '
        Me.BrowseOutputFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BrowseOutputFolder.Image = Global.XenonMKV.My.Resources.Resources.folder_explore
        Me.BrowseOutputFolder.Location = New System.Drawing.Point(577, 3)
        Me.BrowseOutputFolder.Name = "BrowseOutputFolder"
        Me.BrowseOutputFolder.Size = New System.Drawing.Size(24, 23)
        Me.BrowseOutputFolder.TabIndex = 2
        Me.BrowseOutputFolder.UseVisualStyleBackColor = True
        '
        'OutputFolder
        '
        Me.OutputFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OutputFolder.Location = New System.Drawing.Point(109, 3)
        Me.OutputFolder.Name = "OutputFolder"
        Me.OutputFolder.Size = New System.Drawing.Size(433, 21)
        Me.OutputFolder.TabIndex = 1
        '
        'OutputFolderTitle
        '
        Me.OutputFolderTitle.AutoSize = True
        Me.OutputFolderTitle.Location = New System.Drawing.Point(4, 8)
        Me.OutputFolderTitle.Name = "OutputFolderTitle"
        Me.OutputFolderTitle.Size = New System.Drawing.Size(99, 13)
        Me.OutputFolderTitle.TabIndex = 0
        Me.OutputFolderTitle.Text = "Destination folder: "
        Me.OutputFolderTitle.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'OutputFolderBrowser
        '
        Me.OutputFolderBrowser.Description = "Select the folder to place the converted MP4 file(s)."
        '
        'ScrollLog
        '
        Me.ScrollLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ScrollLog.AutoSize = True
        Me.ScrollLog.Checked = True
        Me.ScrollLog.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ScrollLog.Location = New System.Drawing.Point(7, 415)
        Me.ScrollLog.Name = "ScrollLog"
        Me.ScrollLog.Size = New System.Drawing.Size(68, 17)
        Me.ScrollLog.TabIndex = 12
        Me.ScrollLog.Text = "Scroll log"
        Me.ScrollLog.UseVisualStyleBackColor = True
        '
        'ConversionTypePanel
        '
        Me.ConversionTypePanel.Controls.Add(Me.ConvertFolder)
        Me.ConversionTypePanel.Controls.Add(Me.ConvertSingle)
        Me.ConversionTypePanel.Location = New System.Drawing.Point(7, 27)
        Me.ConversionTypePanel.Name = "ConversionTypePanel"
        Me.ConversionTypePanel.Size = New System.Drawing.Size(273, 24)
        Me.ConversionTypePanel.TabIndex = 13
        '
        'ConvertFolder
        '
        Me.ConvertFolder.AutoSize = True
        Me.ConvertFolder.Location = New System.Drawing.Point(135, 3)
        Me.ConvertFolder.Name = "ConvertFolder"
        Me.ConvertFolder.Size = New System.Drawing.Size(104, 17)
        Me.ConvertFolder.TabIndex = 1
        Me.ConvertFolder.Text = "Convert a folder"
        Me.ConvertFolder.UseVisualStyleBackColor = True
        '
        'ConvertSingle
        '
        Me.ConvertSingle.AutoSize = True
        Me.ConvertSingle.Checked = True
        Me.ConvertSingle.Location = New System.Drawing.Point(5, 3)
        Me.ConvertSingle.Name = "ConvertSingle"
        Me.ConvertSingle.Size = New System.Drawing.Size(102, 17)
        Me.ConvertSingle.TabIndex = 0
        Me.ConvertSingle.TabStop = True
        Me.ConvertSingle.Text = "Convert one file"
        Me.ConvertSingle.UseVisualStyleBackColor = True
        '
        'InputFolderBrowser
        '
        Me.InputFolderBrowser.Description = "Select the folder to read the original MKV file(s) from."
        '
        'DefaultMenu
        '
        Me.DefaultMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolsToolStripMenuItem, Me.HelpToolStripMenuItem})
        Me.DefaultMenu.Location = New System.Drawing.Point(0, 0)
        Me.DefaultMenu.Name = "DefaultMenu"
        Me.DefaultMenu.Size = New System.Drawing.Size(624, 24)
        Me.DefaultMenu.TabIndex = 15
        Me.DefaultMenu.Text = "MenuStrip1"
        '
        'ToolsToolStripMenuItem
        '
        Me.ToolsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EnableMP4PlaybackInMediaPlayerToolStripMenuItem, Me.DeleteTemporaryFilesToolStripMenuItem, Me.MoveOutputToolStripMenuItem, Me.AllowNetworkPathToolStripMenuItem, Me.ToolStripSeparator_Tools, Me.MKVInformationToolStripMenuItem, Me.OptionsToolStripMenuItem})
        Me.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem"
        Me.ToolsToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.ToolsToolStripMenuItem.Text = "Tools"
        '
        'EnableMP4PlaybackInMediaPlayerToolStripMenuItem
        '
        Me.EnableMP4PlaybackInMediaPlayerToolStripMenuItem.Name = "EnableMP4PlaybackInMediaPlayerToolStripMenuItem"
        Me.EnableMP4PlaybackInMediaPlayerToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.EnableMP4PlaybackInMediaPlayerToolStripMenuItem.Text = "Enable .mp4 in Media Player"
        '
        'DeleteTemporaryFilesToolStripMenuItem
        '
        Me.DeleteTemporaryFilesToolStripMenuItem.Name = "DeleteTemporaryFilesToolStripMenuItem"
        Me.DeleteTemporaryFilesToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.DeleteTemporaryFilesToolStripMenuItem.Text = "Delete Temporary Files"
        '
        'MoveOutputToolStripMenuItem
        '
        Me.MoveOutputToolStripMenuItem.Name = "MoveOutputToolStripMenuItem"
        Me.MoveOutputToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.MoveOutputToolStripMenuItem.Text = "Move Output File"
        Me.MoveOutputToolStripMenuItem.Visible = False
        '
        'AllowNetworkPathToolStripMenuItem
        '
        Me.AllowNetworkPathToolStripMenuItem.Name = "AllowNetworkPathToolStripMenuItem"
        Me.AllowNetworkPathToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.AllowNetworkPathToolStripMenuItem.Text = "Allow Network Path"
        '
        'ToolStripSeparator_Tools
        '
        Me.ToolStripSeparator_Tools.Name = "ToolStripSeparator_Tools"
        Me.ToolStripSeparator_Tools.Size = New System.Drawing.Size(220, 6)
        '
        'MKVInformationToolStripMenuItem
        '
        Me.MKVInformationToolStripMenuItem.Name = "MKVInformationToolStripMenuItem"
        Me.MKVInformationToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.MKVInformationToolStripMenuItem.Text = "MKV Information"
        '
        'OptionsToolStripMenuItem
        '
        Me.OptionsToolStripMenuItem.Name = "OptionsToolStripMenuItem"
        Me.OptionsToolStripMenuItem.Size = New System.Drawing.Size(223, 22)
        Me.OptionsToolStripMenuItem.Text = "Options"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.WebsiteToolStripMenuItem, Me.ForumToolStripMenuItem, Me.CheckForUpdateToolStripMenuItem, Me.ToolStripSeparator_Help, Me.AboutToolStripMenuItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'WebsiteToolStripMenuItem
        '
        Me.WebsiteToolStripMenuItem.Name = "WebsiteToolStripMenuItem"
        Me.WebsiteToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.WebsiteToolStripMenuItem.Text = "Website"
        '
        'ForumToolStripMenuItem
        '
        Me.ForumToolStripMenuItem.Name = "ForumToolStripMenuItem"
        Me.ForumToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.ForumToolStripMenuItem.Text = "Forum"
        '
        'CheckForUpdateToolStripMenuItem
        '
        Me.CheckForUpdateToolStripMenuItem.Name = "CheckForUpdateToolStripMenuItem"
        Me.CheckForUpdateToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.CheckForUpdateToolStripMenuItem.Text = "Check for Update"
        '
        'ToolStripSeparator_Help
        '
        Me.ToolStripSeparator_Help.Name = "ToolStripSeparator_Help"
        Me.ToolStripSeparator_Help.Size = New System.Drawing.Size(163, 6)
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.AboutToolStripMenuItem.Text = "About..."
        '
        'TrayIcon
        '
        Me.TrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.TrayIcon.ContextMenuStrip = Me.TrayMenu
        Me.TrayIcon.Icon = CType(resources.GetObject("TrayIcon.Icon"), System.Drawing.Icon)
        Me.TrayIcon.Text = "XenonMKV"
        Me.TrayIcon.Visible = True
        '
        'TrayMenu
        '
        Me.TrayMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HideShowXenonMKVToolStripMenuItem1, Me.SToolStripMenuItem, Me.ExitToolStripMenuItem1})
        Me.TrayMenu.Name = "TrayMenu"
        Me.TrayMenu.Size = New System.Drawing.Size(196, 54)
        Me.TrayMenu.Text = "Tray"
        '
        'HideShowXenonMKVToolStripMenuItem1
        '
        Me.HideShowXenonMKVToolStripMenuItem1.Name = "HideShowXenonMKVToolStripMenuItem1"
        Me.HideShowXenonMKVToolStripMenuItem1.Size = New System.Drawing.Size(195, 22)
        Me.HideShowXenonMKVToolStripMenuItem1.Text = "Hide/Show XenonMKV"
        '
        'SToolStripMenuItem
        '
        Me.SToolStripMenuItem.Name = "SToolStripMenuItem"
        Me.SToolStripMenuItem.Size = New System.Drawing.Size(192, 6)
        '
        'ExitToolStripMenuItem1
        '
        Me.ExitToolStripMenuItem1.Name = "ExitToolStripMenuItem1"
        Me.ExitToolStripMenuItem1.Size = New System.Drawing.Size(195, 22)
        Me.ExitToolStripMenuItem1.Text = "Exit"
        '
        'tmrTooltip
        '
        Me.tmrTooltip.Interval = 2500
        '
        'CancelProcess
        '
        Me.CancelProcess.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CancelProcess.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelProcess.Enabled = False
        Me.CancelProcess.Image = Global.XenonMKV.My.Resources.Resources._stop
        Me.CancelProcess.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.CancelProcess.Location = New System.Drawing.Point(408, 398)
        Me.CancelProcess.Name = "CancelProcess"
        Me.CancelProcess.Size = New System.Drawing.Size(99, 33)
        Me.CancelProcess.TabIndex = 17
        Me.CancelProcess.Text = "Cancel"
        Me.CancelProcess.UseVisualStyleBackColor = True
        '
        'Log
        '
        Me.Log.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Log.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.LogIcon})
        Me.Log.FullRowSelect = True
        Me.Log.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.Log.Location = New System.Drawing.Point(7, 156)
        Me.Log.Name = "Log"
        Me.Log.Size = New System.Drawing.Size(605, 203)
        Me.Log.SmallImageList = Me.LogImages
        Me.Log.TabIndex = 18
        Me.Log.UseCompatibleStateImageBehavior = False
        Me.Log.View = System.Windows.Forms.View.Details
        '
        'LogIcon
        '
        Me.LogIcon.Text = ""
        Me.LogIcon.Width = 18
        '
        'LogImages
        '
        Me.LogImages.ImageStream = CType(resources.GetObject("LogImages.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.LogImages.TransparentColor = System.Drawing.Color.Transparent
        Me.LogImages.Images.SetKeyName(0, "accept.png")
        Me.LogImages.Images.SetKeyName(1, "error.png")
        Me.LogImages.Images.SetKeyName(2, "hourglass.png")
        Me.LogImages.Images.SetKeyName(3, "information.png")
        Me.LogImages.Images.SetKeyName(4, "stop.png")
        '
        'btnOptions
        '
        Me.btnOptions.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOptions.Image = Global.XenonMKV.My.Resources.Resources.cog
        Me.btnOptions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnOptions.Location = New System.Drawing.Point(279, 399)
        Me.btnOptions.Name = "btnOptions"
        Me.btnOptions.Size = New System.Drawing.Size(99, 33)
        Me.btnOptions.TabIndex = 19
        Me.btnOptions.Text = "Options"
        Me.btnOptions.UseVisualStyleBackColor = True
        '
        'MainUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CancelProcess
        Me.ClientSize = New System.Drawing.Size(624, 444)
        Me.Controls.Add(Me.btnOptions)
        Me.Controls.Add(Me.Log)
        Me.Controls.Add(Me.ConversionTypePanel)
        Me.Controls.Add(Me.ScrollLog)
        Me.Controls.Add(Me.OutputFolderPanel)
        Me.Controls.Add(Me.InputPanel)
        Me.Controls.Add(Me.CancelProcess)
        Me.Controls.Add(Me.WarningPanel)
        Me.Controls.Add(Me.IsTalkative)
        Me.Controls.Add(Me.StartRecode)
        Me.Controls.Add(Me.ProgressBar)
        Me.Controls.Add(Me.DefaultMenu)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.DefaultMenu
        Me.MinimumSize = New System.Drawing.Size(480, 380)
        Me.Name = "MainUI"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "XenonMKV build"
        Me.WarningPanel.ResumeLayout(False)
        Me.WarningPanel.PerformLayout()
        CType(Me.WarningPicture, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InputPanel.ResumeLayout(False)
        Me.InputPanel.PerformLayout()
        Me.OutputFolderPanel.ResumeLayout(False)
        Me.OutputFolderPanel.PerformLayout()
        Me.ConversionTypePanel.ResumeLayout(False)
        Me.ConversionTypePanel.PerformLayout()
        Me.DefaultMenu.ResumeLayout(False)
        Me.DefaultMenu.PerformLayout()
        Me.TrayMenu.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents StartRecode As System.Windows.Forms.Button
    Friend WithEvents IsTalkative As System.Windows.Forms.CheckBox
    Friend WithEvents OpenFile As System.Windows.Forms.OpenFileDialog
    Friend WithEvents WarningPanel As System.Windows.Forms.Panel
    Friend WithEvents WarningPicture As System.Windows.Forms.PictureBox
    Friend WithEvents WarningText As System.Windows.Forms.Label
    Friend WithEvents InputPanel As System.Windows.Forms.Panel
    Friend WithEvents SingleFileName As System.Windows.Forms.TextBox
    Friend WithEvents SourceType As System.Windows.Forms.Label
    Friend WithEvents BrowseSingleFile As System.Windows.Forms.Button
    Friend WithEvents OutputFolderPanel As System.Windows.Forms.Panel
    Friend WithEvents OutputFolder As System.Windows.Forms.TextBox
    Friend WithEvents OutputFolderTitle As System.Windows.Forms.Label
    Friend WithEvents BrowseOutputFolder As System.Windows.Forms.Button
    Friend WithEvents OutputFolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ScrollLog As System.Windows.Forms.CheckBox
    Friend WithEvents ConversionTypePanel As System.Windows.Forms.Panel
    Friend WithEvents ConvertFolder As System.Windows.Forms.RadioButton
    Friend WithEvents ConvertSingle As System.Windows.Forms.RadioButton
    Friend WithEvents InputFolderBrowser As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents DefaultMenu As System.Windows.Forms.MenuStrip
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WebsiteToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ForumToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator_Help As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents TrayIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents ToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EnableMP4PlaybackInMediaPlayerToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator_Tools As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents OptionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmrTooltip As System.Windows.Forms.Timer
    Friend WithEvents TrayMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents HideShowXenonMKVToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SToolStripMenuItem As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ExitToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CancelProcess As System.Windows.Forms.Button
    Friend WithEvents PreviewMedia As System.Windows.Forms.Button
    Friend WithEvents Log As System.Windows.Forms.ListView
    Friend WithEvents LogIcon As System.Windows.Forms.ColumnHeader
    Friend WithEvents LogImages As System.Windows.Forms.ImageList
    Friend WithEvents MKVInformationToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteTemporaryFilesToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MoveOutputToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AllowNetworkPathToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ViewOutputFolder As System.Windows.Forms.Button
    Friend WithEvents CheckForUpdateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnOptions As System.Windows.Forms.Button

End Class
