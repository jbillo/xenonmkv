Public Class VideoTrack
    Inherits Track

    Dim _par As String

    Dim _width As Integer, _height As Integer

    Sub New()
        Me.Type = "video"
    End Sub

    Public Property PixelAspectRatio() As String
        Get
            Return _par
        End Get
        Set(ByVal value As String)
            _par = value
        End Set
    End Property

    Public Property Width() As Integer
        Get
            Return _width
        End Get
        Set(ByVal value As Integer)
            _width = value
        End Set
    End Property

    Public Property Height() As Integer
        Get
            Return _height
        End Get
        Set(ByVal value As Integer)
            _height = value
        End Set
    End Property

End Class
