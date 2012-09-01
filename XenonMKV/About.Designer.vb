<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class About
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
        Me.VersionID = New System.Windows.Forms.Label()
        Me.GPLText = New System.Windows.Forms.Label()
        Me.CloseAboutBox = New System.Windows.Forms.Button()
        Me.CreditText_Icons = New System.Windows.Forms.Label()
        Me.CreditLink_Icons = New System.Windows.Forms.LinkLabel()
        Me.DevCredits = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'VersionID
        '
        Me.VersionID.AutoSize = True
        Me.VersionID.Location = New System.Drawing.Point(13, 13)
        Me.VersionID.Name = "VersionID"
        Me.VersionID.Size = New System.Drawing.Size(85, 13)
        Me.VersionID.TabIndex = 0
        Me.VersionID.Text = "XenonMKV build "
        '
        'GPLText
        '
        Me.GPLText.AutoSize = True
        Me.GPLText.Location = New System.Drawing.Point(13, 137)
        Me.GPLText.Name = "GPLText"
        Me.GPLText.Size = New System.Drawing.Size(530, 13)
        Me.GPLText.TabIndex = 1
        Me.GPLText.Text = "This program is made available under the GNU GPL 2.0. Please view README.txt and " & _
            "gpl.txt for more details."
        '
        'CloseAboutBox
        '
        Me.CloseAboutBox.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseAboutBox.Location = New System.Drawing.Point(481, 162)
        Me.CloseAboutBox.Name = "CloseAboutBox"
        Me.CloseAboutBox.Size = New System.Drawing.Size(75, 23)
        Me.CloseAboutBox.TabIndex = 2
        Me.CloseAboutBox.Text = "Close"
        Me.CloseAboutBox.UseVisualStyleBackColor = True
        '
        'CreditText_Icons
        '
        Me.CreditText_Icons.AutoSize = True
        Me.CreditText_Icons.Location = New System.Drawing.Point(12, 108)
        Me.CreditText_Icons.Name = "CreditText_Icons"
        Me.CreditText_Icons.Size = New System.Drawing.Size(411, 13)
        Me.CreditText_Icons.TabIndex = 3
        Me.CreditText_Icons.Text = "Some icons used under Creative Commons Attribution License from famfamfam.com."
        '
        'CreditLink_Icons
        '
        Me.CreditLink_Icons.AutoSize = True
        Me.CreditLink_Icons.Location = New System.Drawing.Point(495, 107)
        Me.CreditLink_Icons.Name = "CreditLink_Icons"
        Me.CreditLink_Icons.Size = New System.Drawing.Size(46, 13)
        Me.CreditLink_Icons.TabIndex = 4
        Me.CreditLink_Icons.TabStop = True
        Me.CreditLink_Icons.Text = "Visit site"
        '
        'DevCredits
        '
        Me.DevCredits.AutoSize = True
        Me.DevCredits.Location = New System.Drawing.Point(13, 36)
        Me.DevCredits.Name = "DevCredits"
        Me.DevCredits.Size = New System.Drawing.Size(75, 13)
        Me.DevCredits.TabIndex = 5
        Me.DevCredits.Text = "developed by "
        '
        'About
        '
        Me.AcceptButton = Me.CloseAboutBox
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CloseAboutBox
        Me.ClientSize = New System.Drawing.Size(568, 197)
        Me.Controls.Add(Me.DevCredits)
        Me.Controls.Add(Me.CreditLink_Icons)
        Me.Controls.Add(Me.CreditText_Icons)
        Me.Controls.Add(Me.CloseAboutBox)
        Me.Controls.Add(Me.GPLText)
        Me.Controls.Add(Me.VersionID)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "About"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "About XenonMKV"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents VersionID As System.Windows.Forms.Label
    Friend WithEvents GPLText As System.Windows.Forms.Label
    Friend WithEvents CloseAboutBox As System.Windows.Forms.Button
    Friend WithEvents CreditText_Icons As System.Windows.Forms.Label
    Friend WithEvents CreditLink_Icons As System.Windows.Forms.LinkLabel
    Friend WithEvents DevCredits As System.Windows.Forms.Label
End Class
