<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SelectTime
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
        Me.VideoCutoffDesc = New System.Windows.Forms.Label
        Me.VideoCutoffTracker = New System.Windows.Forms.TrackBar
        Me.DontSplit = New System.Windows.Forms.CheckBox
        Me.OKCutoff = New System.Windows.Forms.Button
        Me.CancelCutoff = New System.Windows.Forms.Button
        Me.CutoffTimeCaption = New System.Windows.Forms.Label
        Me.VideoCutoffTime = New System.Windows.Forms.TextBox
        Me.TimeOK = New System.Windows.Forms.Label
        CType(Me.VideoCutoffTracker, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'VideoCutoffDesc
        '
        Me.VideoCutoffDesc.AutoSize = True
        Me.VideoCutoffDesc.Location = New System.Drawing.Point(12, 9)
        Me.VideoCutoffDesc.Name = "VideoCutoffDesc"
        Me.VideoCutoffDesc.Size = New System.Drawing.Size(579, 13)
        Me.VideoCutoffDesc.TabIndex = 0
        Me.VideoCutoffDesc.Text = "To keep this video under 4GB, select a cutoff time over 30 seconds. For example, " & _
            "you may want to remove the credits."
        '
        'VideoCutoffTracker
        '
        Me.VideoCutoffTracker.Location = New System.Drawing.Point(12, 36)
        Me.VideoCutoffTracker.Name = "VideoCutoffTracker"
        Me.VideoCutoffTracker.Size = New System.Drawing.Size(594, 45)
        Me.VideoCutoffTracker.TabIndex = 1
        '
        'DontSplit
        '
        Me.DontSplit.AutoSize = True
        Me.DontSplit.Location = New System.Drawing.Point(12, 107)
        Me.DontSplit.Name = "DontSplit"
        Me.DontSplit.Size = New System.Drawing.Size(365, 17)
        Me.DontSplit.TabIndex = 2
        Me.DontSplit.Text = "Don't cut this file off or split future files. (Change this in Tools/Options)"
        Me.DontSplit.UseVisualStyleBackColor = True
        '
        'OKCutoff
        '
        Me.OKCutoff.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.OKCutoff.Enabled = False
        Me.OKCutoff.Location = New System.Drawing.Point(452, 99)
        Me.OKCutoff.Name = "OKCutoff"
        Me.OKCutoff.Size = New System.Drawing.Size(75, 23)
        Me.OKCutoff.TabIndex = 3
        Me.OKCutoff.Text = "OK"
        Me.OKCutoff.UseVisualStyleBackColor = True
        '
        'CancelCutoff
        '
        Me.CancelCutoff.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CancelCutoff.Location = New System.Drawing.Point(533, 99)
        Me.CancelCutoff.Name = "CancelCutoff"
        Me.CancelCutoff.Size = New System.Drawing.Size(75, 23)
        Me.CancelCutoff.TabIndex = 4
        Me.CancelCutoff.Text = "Cancel"
        Me.CancelCutoff.UseVisualStyleBackColor = True
        '
        'CutoffTimeCaption
        '
        Me.CutoffTimeCaption.AutoSize = True
        Me.CutoffTimeCaption.Location = New System.Drawing.Point(12, 84)
        Me.CutoffTimeCaption.Name = "CutoffTimeCaption"
        Me.CutoffTimeCaption.Size = New System.Drawing.Size(241, 13)
        Me.CutoffTimeCaption.TabIndex = 5
        Me.CutoffTimeCaption.Text = "Cut this video off after                                        ."
        '
        'VideoCutoffTime
        '
        Me.VideoCutoffTime.Location = New System.Drawing.Point(131, 81)
        Me.VideoCutoffTime.Name = "VideoCutoffTime"
        Me.VideoCutoffTime.Size = New System.Drawing.Size(106, 21)
        Me.VideoCutoffTime.TabIndex = 6
        '
        'TimeOK
        '
        Me.TimeOK.Image = Global.XenonMKV.My.Resources.Resources.accept
        Me.TimeOK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.TimeOK.Location = New System.Drawing.Point(282, 84)
        Me.TimeOK.Name = "TimeOK"
        Me.TimeOK.Size = New System.Drawing.Size(112, 18)
        Me.TimeOK.TabIndex = 7
        Me.TimeOK.Text = "Time is valid."
        Me.TimeOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.TimeOK.Visible = False
        '
        'SelectTime
        '
        Me.AcceptButton = Me.OKCutoff
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CancelCutoff
        Me.ClientSize = New System.Drawing.Size(618, 134)
        Me.Controls.Add(Me.TimeOK)
        Me.Controls.Add(Me.VideoCutoffTime)
        Me.Controls.Add(Me.CutoffTimeCaption)
        Me.Controls.Add(Me.CancelCutoff)
        Me.Controls.Add(Me.OKCutoff)
        Me.Controls.Add(Me.DontSplit)
        Me.Controls.Add(Me.VideoCutoffTracker)
        Me.Controls.Add(Me.VideoCutoffDesc)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectTime"
        Me.Text = "Select Cutoff Time"
        CType(Me.VideoCutoffTracker, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents VideoCutoffDesc As System.Windows.Forms.Label
    Friend WithEvents VideoCutoffTracker As System.Windows.Forms.TrackBar
    Friend WithEvents DontSplit As System.Windows.Forms.CheckBox
    Friend WithEvents OKCutoff As System.Windows.Forms.Button
    Friend WithEvents CancelCutoff As System.Windows.Forms.Button
    Friend WithEvents CutoffTimeCaption As System.Windows.Forms.Label
    Friend WithEvents VideoCutoffTime As System.Windows.Forms.TextBox
    Friend WithEvents TimeOK As System.Windows.Forms.Label
End Class
