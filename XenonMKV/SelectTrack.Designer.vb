<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectTrack
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
        Me.MultiTrackInfo = New System.Windows.Forms.Label
        Me.VideoTrackBox = New System.Windows.Forms.GroupBox
        Me.VideoTrackList = New System.Windows.Forms.ComboBox
        Me.AudioTrackBox = New System.Windows.Forms.GroupBox
        Me.AudioTrackList = New System.Windows.Forms.ComboBox
        Me.AlwaysUseLanguage = New System.Windows.Forms.CheckBox
        Me.Cancel = New System.Windows.Forms.Button
        Me.OK = New System.Windows.Forms.Button
        Me.AutoPickTracks = New System.Windows.Forms.CheckBox
        Me.VideoTrackBox.SuspendLayout()
        Me.AudioTrackBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'MultiTrackInfo
        '
        Me.MultiTrackInfo.AutoSize = True
        Me.MultiTrackInfo.Location = New System.Drawing.Point(12, 9)
        Me.MultiTrackInfo.Name = "MultiTrackInfo"
        Me.MultiTrackInfo.Size = New System.Drawing.Size(527, 13)
        Me.MultiTrackInfo.TabIndex = 0
        Me.MultiTrackInfo.Text = "This MKV file contains more than one video or audio track. Please select the trac" & _
            "ks to place in the output file."
        '
        'VideoTrackBox
        '
        Me.VideoTrackBox.Controls.Add(Me.VideoTrackList)
        Me.VideoTrackBox.Location = New System.Drawing.Point(11, 25)
        Me.VideoTrackBox.Name = "VideoTrackBox"
        Me.VideoTrackBox.Size = New System.Drawing.Size(528, 49)
        Me.VideoTrackBox.TabIndex = 1
        Me.VideoTrackBox.TabStop = False
        Me.VideoTrackBox.Text = "Video Track"
        '
        'VideoTrackList
        '
        Me.VideoTrackList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.VideoTrackList.FormattingEnabled = True
        Me.VideoTrackList.Location = New System.Drawing.Point(6, 20)
        Me.VideoTrackList.Name = "VideoTrackList"
        Me.VideoTrackList.Size = New System.Drawing.Size(516, 21)
        Me.VideoTrackList.TabIndex = 8
        '
        'AudioTrackBox
        '
        Me.AudioTrackBox.Controls.Add(Me.AudioTrackList)
        Me.AudioTrackBox.Controls.Add(Me.AlwaysUseLanguage)
        Me.AudioTrackBox.Location = New System.Drawing.Point(11, 80)
        Me.AudioTrackBox.Name = "AudioTrackBox"
        Me.AudioTrackBox.Size = New System.Drawing.Size(528, 72)
        Me.AudioTrackBox.TabIndex = 2
        Me.AudioTrackBox.TabStop = False
        Me.AudioTrackBox.Text = "Audio Track"
        '
        'AudioTrackList
        '
        Me.AudioTrackList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AudioTrackList.FormattingEnabled = True
        Me.AudioTrackList.Location = New System.Drawing.Point(6, 20)
        Me.AudioTrackList.Name = "AudioTrackList"
        Me.AudioTrackList.Size = New System.Drawing.Size(515, 21)
        Me.AudioTrackList.TabIndex = 7
        '
        'AlwaysUseLanguage
        '
        Me.AlwaysUseLanguage.AutoSize = True
        Me.AlwaysUseLanguage.Location = New System.Drawing.Point(6, 49)
        Me.AlwaysUseLanguage.Name = "AlwaysUseLanguage"
        Me.AlwaysUseLanguage.Size = New System.Drawing.Size(498, 17)
        Me.AlwaysUseLanguage.TabIndex = 6
        Me.AlwaysUseLanguage.Text = "Always use this language. (If there's more than one track in this language, use t" & _
            "he first available.)"
        Me.AlwaysUseLanguage.UseVisualStyleBackColor = True
        '
        'Cancel
        '
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(464, 160)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Cancel.TabIndex = 3
        Me.Cancel.Text = "Cancel"
        Me.Cancel.UseVisualStyleBackColor = True
        '
        'OK
        '
        Me.OK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OK.Location = New System.Drawing.Point(383, 160)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(75, 23)
        Me.OK.TabIndex = 4
        Me.OK.Text = "OK"
        Me.OK.UseVisualStyleBackColor = True
        '
        'AutoPickTracks
        '
        Me.AutoPickTracks.AutoSize = True
        Me.AutoPickTracks.Location = New System.Drawing.Point(11, 164)
        Me.AutoPickTracks.Name = "AutoPickTracks"
        Me.AutoPickTracks.Size = New System.Drawing.Size(349, 17)
        Me.AutoPickTracks.TabIndex = 5
        Me.AutoPickTracks.Text = "Don't ask me this again; try to pick the correct tracks automatically."
        Me.AutoPickTracks.UseVisualStyleBackColor = True
        '
        'SelectTrack
        '
        Me.AcceptButton = Me.OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel
        Me.ClientSize = New System.Drawing.Size(550, 193)
        Me.Controls.Add(Me.AutoPickTracks)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.AudioTrackBox)
        Me.Controls.Add(Me.VideoTrackBox)
        Me.Controls.Add(Me.MultiTrackInfo)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectTrack"
        Me.Text = "Select Video and Audio Tracks"
        Me.VideoTrackBox.ResumeLayout(False)
        Me.AudioTrackBox.ResumeLayout(False)
        Me.AudioTrackBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MultiTrackInfo As System.Windows.Forms.Label
    Friend WithEvents VideoTrackBox As System.Windows.Forms.GroupBox
    Friend WithEvents AudioTrackBox As System.Windows.Forms.GroupBox
    Friend WithEvents Cancel As System.Windows.Forms.Button
    Friend WithEvents VideoTrackList As System.Windows.Forms.ComboBox
    Friend WithEvents AudioTrackList As System.Windows.Forms.ComboBox
    Friend WithEvents AlwaysUseLanguage As System.Windows.Forms.CheckBox
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents AutoPickTracks As System.Windows.Forms.CheckBox
End Class
