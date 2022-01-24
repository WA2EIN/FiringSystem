' (C) Copyright P. Cranwell, 2014
Public Class cTimeCode
    Private Hour As String
    Private Minute As String
    Private Second As String
    Private Frame As String
    Private TimeStamp As String
    Private MS As String

    Public Property HH()
        Get
            Return Hour
        End Get
        Set(ByVal value)
            Hour = value
        End Set
    End Property
    Public Property MM()
        Get
            Return Minute
        End Get
        Set(ByVal value)
            Minute = value
        End Set
    End Property
    Public Property SS()
        Get
            Return Second
        End Get
        Set(ByVal value)
            Second = value

        End Set
    End Property
    Public Property FF()
        Get
            Return Frame
        End Get
        Set(ByVal value)
            Frame = value
        End Set
    End Property

    Public Property Milliseconds() As String
        Get
            Return MS
        End Get
        Set(ByVal value As String)
            MS = value
            Hour = Left$(MS, 2)
            Minute = Mid$(MS, 3, 2)
            Second = Mid$(MS, 5, 2)
            Frame = Mid$(MS, 7, 2)
        End Set
    End Property
    Public Function SMPTE() As String
        SMPTE = Left$(Hour, 2) & Left$(Minute, 2) & Left$(Second, 2) & Left$(Frame, 2)
    End Function

    Public Function GetMS() As String
        GetMS = MS
    End Function

    Public Sub New()
        Hour = "00"
        Minute = "00"
        Second = "00"
        Frame = "00"
        Milliseconds = "00000000"
    End Sub
End Class

