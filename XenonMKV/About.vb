Public Class About

    Private Sub CloseAboutBox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseAboutBox.Click
        Me.Hide()
    End Sub

    Private Sub About_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = GetMsg("UI_About_Title")
        VersionID.Text = GetMsg("UI_Title", New String() {Constants.BUILD_VERSION, Constants.BUILD_CODENAME})
        DevCredits.Text = GetMsg("UI_About_DevCredits")

        CreditText_Icons.Text = GetMsg("UI_About_CreditIcons")
        CreditLink_Icons.Text = GetMsg("UI_About_CreditLinkText")
        GPLText.Text = GetMsg("UI_About_GPL")
        CloseAboutBox.Text = GetMsg("UI_About_CloseButton")
    End Sub

    Private Sub CreditLink_Icons_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles CreditLink_Icons.LinkClicked
        System.Diagnostics.Process.Start(Constants.CREDIT_ICONS)
    End Sub
End Class