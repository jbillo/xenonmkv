Public Class Track

    Dim _id As Integer
    Dim _lang As String
    Dim _type As String
    Dim _cid As String
    Dim _fps As Double
    Dim _default As Boolean

    Property ID() As Integer
        Get
            Return _id
        End Get
        Set(ByVal value As Integer)
            _id = value
        End Set
    End Property
    Property Language() As String
        Get
            Return _lang
        End Get
        Set(ByVal value As String)
            _lang = value
        End Set
    End Property
    Property Type() As String
        Get
            Return _type
        End Get
        Set(ByVal value As String)
            _type = value
        End Set
    End Property

    Property CodecID() As String
        Get
            Return _cid
        End Get
        Set(ByVal value As String)
            _cid = value
        End Set
    End Property
    Property FPS() As Double
        Get
            Return _fps
        End Get
        Set(ByVal value As Double)
            _fps = value
        End Set
    End Property
    Property DefaultTrack() As Boolean
        Get
            Return _default
        End Get
        Set(ByVal value As Boolean)
            _default = value
        End Set
    End Property

End Class
