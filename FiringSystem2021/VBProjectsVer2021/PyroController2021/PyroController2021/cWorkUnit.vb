' (C) Copyright P. Cranwell, 2014
Public Class cWorkUnit
    Private PortNumber As String
    Private TC As cTimeCode
    Private l As Integer
    Private CueNumber As String
    Private SeqNumber As String
    Private NextSeqNumber As String
    Private Cmd As String
    Private Unit As String
    Public Property Port()
        Get
            Return PortNumber
        End Get
        Set(ByVal value)
            PortNumber = value
            PortNumber = "000" & PortNumber
            l = Len(PortNumber)
            PortNumber = Mid$(PortNumber, l - 2)
        End Set
    End Property
    Public Property UnitNumber()
        Get
            Return Unit
        End Get
        Set(ByVal value)
            Unit = value
            Unit = "000" & Unit
            l = Len(Unit)
            Unit = Mid$(Unit, l - 1)
        End Set
    End Property
    Public Property Cue()
        Get
            Return CueNumber
        End Get
        Set(ByVal value)
            CueNumber = value
            CueNumber = "000" & CueNumber
            l = Len(CueNumber)
            CueNumber = Mid$(CueNumber, l - 2)
        End Set
    End Property
    Public Property Command()
        Get
            Return Cmd
        End Get
        Set(ByVal value)
            Cmd = value
        End Set
    End Property
    Public Property NextSeq()
        Get
            Return NextSeqNumber
        End Get
        Set(ByVal value)
            NextSeqNumber = value
            NextSeqNumber = "000" & NextSeqNumber
            l = Len(NextSeqNumber)
            NextSeqNumber = Mid$(NextSeqNumber, l - 2)
        End Set
    End Property
    Public Property Hour()
        Get
            Return TC.HH
        End Get
        Set(ByVal value)
            TC.HH = value
        End Set
    End Property
    Public Property Minute()
        Get
            Return TC.MM
        End Get
        Set(ByVal value)
            TC.MM = value
        End Set
    End Property
    Public Property Second()
        Get
            Return TC.SS
        End Get
        Set(ByVal value)
            TC.SS = value
        End Set
    End Property
    Public Property Frame()
        Get
            Return TC.FF
        End Get
        Set(ByVal value)
            TC.FF = value
        End Set
    End Property
    Public Property MS()
        Get
            Return TC.Milliseconds
        End Get
        Set(ByVal value)
            TC.Milliseconds = value
        End Set
    End Property
    Public Sub New()
        UnitNumber = "000"
        PortNumber = "000"
        TC = New cTimeCode
    End Sub
    Public Sub done()
        Me.Finalize()
    End Sub
    Public Function WorkUnit() As String
        Return Left$(Port, 3) & TC.SMPTE
    End Function
    Public Function TWorkUnit() As String
        Return CueNumber & TC.Milliseconds & Port & NextSeq
    End Function
    Public Function NullWorkUnit() As String
        Return "              "
    End Function
End Class
