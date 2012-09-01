<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SetAspectRatio
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
        Me.Description = New System.Windows.Forms.Label
        Me.Choice_Auto = New System.Windows.Forms.RadioButton
        Me.Choice_Custom = New System.Windows.Forms.RadioButton
        Me.CustomAR = New System.Windows.Forms.ComboBox
        Me.OK = New System.Windows.Forms.Button
        Me.Cancel = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Description
        '
        Me.Description.AutoSize = True
        Me.Description.Location = New System.Drawing.Point(12, 9)
        Me.Description.Name = "Description"
        Me.Description.Size = New System.Drawing.Size(528, 13)
        Me.Description.TabIndex = 0
        Me.Description.Text = "The display aspect ratio (DAR) for this file has been detected as ___. You can ov" & _
            "erride this option if needed."
        '
        'Choice_Auto
        '
        Me.Choice_Auto.AutoSize = True
        Me.Choice_Auto.Checked = True
        Me.Choice_Auto.Location = New System.Drawing.Point(15, 35)
        Me.Choice_Auto.Name = "Choice_Auto"
        Me.Choice_Auto.Size = New System.Drawing.Size(244, 17)
        Me.Choice_Auto.TabIndex = 1
        Me.Choice_Auto.TabStop = True
        Me.Choice_Auto.Text = "Keep the automatically detected aspect ratio."
        Me.Choice_Auto.UseVisualStyleBackColor = True
        '
        'Choice_Custom
        '
        Me.Choice_Custom.AutoSize = True
        Me.Choice_Custom.Location = New System.Drawing.Point(15, 58)
        Me.Choice_Custom.Name = "Choice_Custom"
        Me.Choice_Custom.Size = New System.Drawing.Size(191, 17)
        Me.Choice_Custom.TabIndex = 2
        Me.Choice_Custom.Text = "Set the aspect ratio for this file to:"
        Me.Choice_Custom.UseVisualStyleBackColor = True
        '
        'CustomAR
        '
        Me.CustomAR.FormattingEnabled = True
        Me.CustomAR.Items.AddRange(New Object() {"4:3", "16:9", "16:10"})
        Me.CustomAR.Location = New System.Drawing.Point(212, 58)
        Me.CustomAR.Name = "CustomAR"
        Me.CustomAR.Size = New System.Drawing.Size(121, 21)
        Me.CustomAR.TabIndex = 3
        '
        'OK
        '
        Me.OK.Location = New System.Drawing.Point(386, 96)
        Me.OK.Name = "OK"
        Me.OK.Size = New System.Drawing.Size(75, 23)
        Me.OK.TabIndex = 4
        Me.OK.Text = "OK"
        Me.OK.UseVisualStyleBackColor = True
        '
        'Cancel
        '
        Me.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel.Location = New System.Drawing.Point(467, 96)
        Me.Cancel.Name = "Cancel"
        Me.Cancel.Size = New System.Drawing.Size(75, 23)
        Me.Cancel.TabIndex = 5
        Me.Cancel.Text = "Cancel"
        Me.Cancel.UseVisualStyleBackColor = True
        '
        'SetAspectRatio
        '
        Me.AcceptButton = Me.OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel
        Me.ClientSize = New System.Drawing.Size(551, 131)
        Me.Controls.Add(Me.Cancel)
        Me.Controls.Add(Me.OK)
        Me.Controls.Add(Me.CustomAR)
        Me.Controls.Add(Me.Choice_Custom)
        Me.Controls.Add(Me.Choice_Auto)
        Me.Controls.Add(Me.Description)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SetAspectRatio"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "Set Display Aspect Ratio"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Description As System.Windows.Forms.Label
    Friend WithEvents Choice_Auto As System.Windows.Forms.RadioButton
    Friend WithEvents Choice_Custom As System.Windows.Forms.RadioButton
    Friend WithEvents CustomAR As System.Windows.Forms.ComboBox
    Friend WithEvents OK As System.Windows.Forms.Button
    Friend WithEvents Cancel As System.Windows.Forms.Button
End Class
