Public Class SelectTrack

    Private VideoTracks As New Dictionary(Of Integer, VideoTrack)
    Private AudioTracks As New Dictionary(Of Integer, AudioTrack)

    Private Sub SelectTrack_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ReloadControls()
    End Sub

    Private Sub AutoPickTracks_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AutoPickTracks.CheckedChanged
        If AutoPickTracks.Checked Then
            My.Settings.AutoPickTracks = True
        Else
            My.Settings.AutoPickTracks = False
        End If

        My.Settings.Save()
    End Sub

    Public Sub ReloadControls()
        VideoTrackList.Items.Clear()
        AudioTrackList.Items.Clear()

        AutoPickTracks.Checked = My.Settings.AutoPickTracks

        Dim s As String = ""

        For Each t As Track In TrackList
            If t.Type = "video" Then
                Dim vt As New VideoTrack
                vt = t
                VideoTracks.Add(t.ID, vt)
            ElseIf t.Type = "audio" Then
                Dim at As New AudioTrack
                at = t
                AudioTracks.Add(t.ID, at)
            End If
        Next

        For Each t As KeyValuePair(Of Integer, VideoTrack) In VideoTracks
            s = t.Value.ID & " (" & t.Value.Language & ") {" & t.Value.CodecID & "}"
            If t.Value.DefaultTrack Then
                s = s & " [default]"
            End If
            s = s & " @" & t.Value.FPS & "fps"
            VideoTrackList.Items.Add(s)
            If t.Value.DefaultTrack Then
                VideoTrackList.SelectedIndex = VideoTrackList.Items.Count - 1
            End If
        Next

        For Each t As KeyValuePair(Of Integer, AudioTrack) In AudioTracks
            s = t.Value.ID & " (" & t.Value.Language & ") {" & t.Value.CodecID & "}"
            If t.Value.DefaultTrack Then
                s = s & " [default]"
            End If
            AudioTrackList.Items.Add(s)
            If t.Value.Language = My.Settings.AlwaysUseLanguage Then
                AlwaysUseLanguage.Checked = True
                AudioTrackList.SelectedIndex = AudioTrackList.Items.Count - 1
            ElseIf t.Value.DefaultTrack Then
                AudioTrackList.SelectedIndex = AudioTrackList.Items.Count - 1
            End If
        Next

    End Sub

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        Dim s As String, i As Integer

        ' Get audio and video tracks
        Try
            s = AudioTrackList.GetItemText(AudioTrackList.SelectedItem)
            i = CInt(s.Substring(0, s.IndexOf(" ")))
            Dim at As AudioTrack = AudioTracks(i)

            s = VideoTrackList.GetItemText(VideoTrackList.SelectedItem)
            i = CInt(s.Substring(0, s.IndexOf(" ")))
            Dim vt As VideoTrack = VideoTracks(i)

            If AlwaysUseLanguage.Checked Then
                ' Get language name from audiotrack
                My.Settings.AlwaysUseLanguage = at.Language
            Else
                My.Settings.AlwaysUseLanguage = ""
            End If

            My.Settings.Save()
            s = ""

            If AudioTrackList.SelectedIndex = -1 Then
                GetAutoAudioTrack()
            Else
                curItem.AudTrack = at.ID
                curItem.AudCodec = at.CodecID
            End If


            If VideoTrackList.SelectedIndex = -1 Then
                GetAutoVideoTrack()
            Else
                curItem.VidTrack = vt.ID
                curItem.VidCodec = vt.CodecID
                curItem.FPS = vt.FPS
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        ' This will abort the process!
        curItem.AudTrack = -1
        curItem.VidTrack = -1
    End Sub

End Class