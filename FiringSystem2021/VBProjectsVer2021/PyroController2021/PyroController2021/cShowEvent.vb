' (C) Copyright P. Cranwell, 2014
Public Class cShowEvent
    Private vCue As Integer
    Private vTime As Long
    Private vUnit As Integer
    Private vPort As Integer
    Private vEffect As String
    Private vSeq As Integer
    Private vNextSeq As Integer
    Public Property Cue() As Long

        Get
            Return vCue
        End Get

        Set(ByVal value As Long)
            vCue = value
        End Set

    End Property
    Public Property Time() As Long

        Get
            Return vTime
        End Get

        Set(ByVal value As Long)
            vTime = value
        End Set

    End Property

    Public Property Unit As Integer
        Get
            Return vUnit
        End Get

        Set(ByVal value As Integer)
            vUnit = value
        End Set
    End Property

    Public Property Port As Integer
        Get
            Return vPort
        End Get

        Set(ByVal value As Integer)
            vPort = value
        End Set
    End Property
    Public Property Seq As Integer
        Get
            Return vSeq
        End Get

        Set(ByVal value As Integer)
            vSeq = value
        End Set
    End Property
    Public Property NextSeq As Integer
        Get
            Return vNextSeq
        End Get

        Set(ByVal value As Integer)
            vNextSeq = value
        End Set
    End Property
    Public Property Effect As String
        Get
            Return vEffect
        End Get

        Set(ByVal value As String)
            vEffect = value
        End Set
    End Property

End Class
