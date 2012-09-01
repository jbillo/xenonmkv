Imports System.Threading

Module MainCycle

    Public InCLIMode As Boolean

    Dim _process As Process
    Public outputThread As Thread
    Public errorThread As Thread
    Public scratchFolder As String = My.Settings.CustomScratchFolder
    Public flag_DTSFailed As Boolean = False
    Public flag_Aborted As Boolean = False
    Public flag_Cutoff As Boolean = False
    Public flag_mp4boxFailed As Boolean = False
    Public flag_needSafeSync As Boolean = False
    Public flag_CustomPAR As Double = 0
    Public curItem As New ConvertItem

    Public logToolOutput As Boolean = True

    Public TrackList As New List(Of Track)

    Dim _outputText As String
    Dim _errorText As String
    Dim _nextTask As Integer = 0
    Dim _VideoTrack As Integer = 0, _VideoFPS As String = Constants.DEFAULT_FPS
    Dim _VideoCodec As String = ".h264", _VideoPAR As String = Constants.DEFAULT_PAR
    Dim _AudioTrack As Integer = 0, _AudioCodec As String = ".ac3"
    Dim _AudioDuration As ULong = 0

    Dim _taskInProgress As Boolean = True
    Dim _DEBUG_skipExtract As Boolean = False
    Dim _isVerbose As Boolean = False

    Public Sub ResetFlags()
        flag_DTSFailed = False
        flag_Aborted = False
        flag_mp4boxFailed = False
        flag_needSafeSync = False
        flag_CustomPAR = 0
    End Sub

    Public Sub ClearTrackList()
        TrackList = New List(Of Track)
    End Sub

    Public Function GetTrackByID(ByVal track As Integer) As Track
        Dim i As Integer
        For i = 0 To TrackList.Count - 1
            If TrackList.Item(i).ID = track Then
                Return TrackList.Item(i)
            End If
        Next

        Return Nothing
    End Function

    Public Function MultiAVTracks() As Boolean
        Dim i As Integer, vc As Integer = 0, ac As Integer = 0
        For i = 0 To TrackList.Count - 1
            If TrackList(i).Type = "video" Then
                vc += 1
            ElseIf TrackList(i).Type = "audio" Then
                ac += 1
            End If
        Next

        If vc > 1 Or ac > 1 Then
            Return True
        End If

        Return False

    End Function

    Public Sub AutoPickTracks()
        ' Automatically pick tracks from list: use the defaults
        Try
            GetAutoAudioTrack()
            GetAutoVideoTrack()
        Catch ex As Exception
            ' Log this somehow?
        End Try
    End Sub

    Public Sub GetAutoAudioTrack()
        Dim i As Integer, at As Integer = -1, ax As Integer = -1, atc As String = "", axc As String = ""
        For i = 0 To TrackList.Count - 1
            If TrackList(i).Type = "audio" And at = -1 Then
                ' Check audio language/default setting
                If My.Settings.AlwaysUseLanguage <> "" And TrackList(i).Language = My.Settings.AlwaysUseLanguage Then
                    at = TrackList(i).ID
                    atc = TrackList(i).CodecID
                ElseIf TrackList(i).DefaultTrack Then
                    If My.Settings.AlwaysUseLanguage <> "" Then
                        ax = TrackList(i).ID
                        axc = TrackList(i).CodecID
                    Else
                        at = TrackList(i).ID
                        atc = TrackList(i).CodecID
                    End If
                End If
            End If
        Next

        If (at = -1 And ax = -1) Then
            ' No default track set? Use the first one.
            For i = 0 To TrackList.Count - 1
                If TrackList(i).Type = "audio" Then
                    curItem.AudTrack = TrackList(i).ID
                    curItem.AudCodec = TrackList(i).CodecID
                    Return
                End If
            Next
        End If

        If (at = -1) Then
            at = ax
            atc = axc
        End If

        curItem.AudCodec = atc
        curItem.AudTrack = at
    End Sub

    Public Sub GetAutoVideoTrack()
        Dim i As Integer, vt As Integer = -1
        For i = 0 To TrackList.Count - 1
            If TrackList(i).Type = "video" And vt = -1 And TrackList(i).DefaultTrack Then
                vt = TrackList(i).ID
                curItem.FPS = TrackList(i).FPS
                curItem.VidCodec = TrackList(i).CodecID
                Dim tempVT As VideoTrack = TryCast(TrackList(i), VideoTrack)
                If tempVT Is Nothing Then
                Else
                    curItem.PAR = tempVT.PixelAspectRatio
                End If
            End If
        Next

        If vt = -1 Then
            ' There was no default track set? Use the first one.
            For i = 0 To TrackList.Count - 1
                If TrackList(i).Type = "video" Then
                    curItem.VidTrack = TrackList(i).ID
                    curItem.VidCodec = TrackList(i).CodecID
                    curItem.FPS = TrackList(i).FPS
                    Dim tempVT As VideoTrack = TryCast(TrackList(i), VideoTrack)
                    If tempVT Is Nothing Then
                    Else
                        curItem.PAR = tempVT.PixelAspectRatio
                    End If
                    Return
                End If
            Next
        End If

        curItem.VidTrack = vt
    End Sub

    Public Property Debug_SkipExtract() As Boolean
        Get
            Return _DEBUG_skipExtract
        End Get
        Set(ByVal value As Boolean)
            _DEBUG_skipExtract = value
        End Set
    End Property

    Public Property IsVerbose() As Boolean
        Get
            Return _isVerbose
        End Get
        Set(ByVal value As Boolean)
            _isVerbose = value
        End Set
    End Property

    Public Property NextTask() As Integer
        Get
            Return _nextTask
        End Get
        Set(ByVal value As Integer)
            _nextTask = value
        End Set
    End Property

    Public Property process() As Process
        Get
            Return _process
        End Get
        Set(ByVal value As Process)
            _process = value
        End Set
    End Property

    Public Property TaskInProgress() As Boolean
        Get
            Return _taskInProgress
        End Get
        Set(ByVal value As Boolean)
            _taskInProgress = value
        End Set
    End Property

    Public Property OutputText() As String
        Get
            Return _outputText
        End Get
        Set(ByVal value As String)
            _outputText = value
        End Set
    End Property

    Public Function Gcd(ByVal num As Long, _
                     ByVal den As Long) As Long

        If (den Mod num = 1) Then Return 1
        While (den Mod num <> 0)
            Dim temp As Integer = num
            num = den Mod num
            den = temp
        End While

        Return num

    End Function

End Module
