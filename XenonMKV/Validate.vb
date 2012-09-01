Module Validate

    Private p As Process

    Function ValidateLocalAppPath() As Boolean
        If AppPath.Contains("\\") Then
            MsgBox("This application cannot be run from a network (UNC) path. " & vbNewLine & vbNewLine & _
            "The conversion utilities must be run from a local or networked drive to continue. " & vbNewLine & _
            "Current path: " & AppPath, MsgBoxStyle.Exclamation, "Cannot Run From Network Path")
            Return False
        End If

        Return True
    End Function

    Function ReferenceFrameTable(ByVal width As Integer, ByVal height As Integer, ByVal refFrames As Integer) As Boolean
        If width = 0 Or height = 0 Or refFrames = 0 Then Return False

        Dim width1280 As New ArrayList, width1920 As New ArrayList

        width1280.Add(New Integer(1) {720, 9})
        width1280.Add(New Integer(1) {648, 10})
        width1280.Add(New Integer(1) {588, 11})
        width1280.Add(New Integer(1) {540, 12})
        width1280.Add(New Integer(1) {498, 13})
        width1280.Add(New Integer(1) {462, 14})
        width1280.Add(New Integer(1) {432, 15})
        width1280.Add(New Integer(1) {405, 16})

        width1920.Add(New Integer(1) {1088, 4})
        width1920.Add(New Integer(1) {864, 5})
        width1920.Add(New Integer(1) {720, 6})

        If width < 1280 Then
            Return False ' does not exceed table
        ElseIf width = 1280 Then
            Return RecurseWidth(width1280, 0, height, refFrames)
        ElseIf width = 1920 Then
            Return RecurseWidth(width1920, 0, height, refFrames)
        End If

        Return False
    End Function

    Private Function RecurseWidth(ByVal list As ArrayList, ByVal i As Integer, ByVal height As Integer, ByVal refFrames As Integer) As Boolean
        If i > list.Count Then Return False
        Dim x() As Integer = list(i)

        If height < x(0) Then Return RecurseWidth(list, i + 1, height, refFrames)

        If height = x(0) Then Return refFrames > x(1)

        If height > x(0) Then
            ' we're now one past 
            x = list(i - 1)
            Return refFrames > x(1)
        End If
    End Function

End Module
