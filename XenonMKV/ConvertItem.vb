Public Class ConvertItem
    Private _deleteOnFinish As Boolean = False
    Private _isSplit As Boolean = False
    Private _fileName As String
    Private _isConverted As Boolean = False
    Private _partNum As Integer = 0
    Private _origName As String = ""
    Private _eac3ToHasRun As Boolean = False
    Private _guid As String = ""

    Private _vt As Integer = -1
    Private _at As Integer = -1
    Private _ac As String = ""
    Private _vc As String = ""
    Private _audLength As Integer = -1
    Private _fps As Double = Constants.DEFAULT_FPS
    Private _par As String = Constants.DEFAULT_PAR

    Public Sub New()
        Me.GUID = System.Guid.NewGuid.ToString
    End Sub

    Public Sub New(ByVal filename As String)
        Me.FileName = filename
        Me.GUID = System.Guid.NewGuid.ToString
    End Sub

    Public Sub New(ByVal filename As String, ByVal isSplit As Boolean)
        Me.FileName = filename
        Me.IsSplit = isSplit
        Me.GUID = System.Guid.NewGuid.ToString
    End Sub

    Public Property DeleteOnFinish() As Boolean
        Get
            Return _deleteOnFinish
        End Get
        Set(ByVal value As Boolean)
            _deleteOnFinish = value
        End Set
    End Property

    Public Property IsSplit() As Boolean
        Get
            Return _isSplit
        End Get
        Set(ByVal value As Boolean)
            _isSplit = value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = value
        End Set
    End Property

    Public Property PartNumber() As Integer
        Get
            Return _partNum
        End Get
        Set(ByVal value As Integer)
            _partNum = value
        End Set
    End Property

    Public Property OriginalFileName() As String
        Get
            Return _origName
        End Get
        Set(ByVal value As String)
            _origName = value
        End Set
    End Property

    Public Property EAC3ToHasRun() As Boolean
        Get
            Return _eac3ToHasRun
        End Get
        Set(ByVal value As Boolean)
            _eac3ToHasRun = value
        End Set
    End Property

    Public Property GUID() As String
        Get
            Return _guid
        End Get
        Set(ByVal value As String)
            _guid = value
        End Set
    End Property

    Public Property VidTrack() As Integer
        Get
            Return _vt
        End Get
        Set(ByVal value As Integer)
            _vt = value
        End Set
    End Property

    Public Property AudTrack() As Integer
        Get
            Return _at
        End Get
        Set(ByVal value As Integer)
            _at = value
        End Set
    End Property

    Public Property AudCodec() As String
        Get
            Return _ac
        End Get
        Set(ByVal value As String)
            _ac = value
        End Set
    End Property

    Public Property AudLength() As Integer
        Get
            Return _audLength
        End Get
        Set(ByVal value As Integer)
            _audLength = value
        End Set
    End Property

    Public Property VidCodec() As String
        Get
            Return _vc
        End Get
        Set(ByVal value As String)
            _vc = value
        End Set
    End Property

    Public Property FPS() As String
        Get
            Return _fps
        End Get
        Set(ByVal value As String)
            Double.TryParse(value, _fps)
            If _fps = 0 Then
                _fps = Constants.DEFAULT_FPS
            End If
        End Set
    End Property

    Public Property PAR() As String
        Get
            Return _par
        End Get
        Set(ByVal value As String)
            _par = value
        End Set
    End Property

End Class

