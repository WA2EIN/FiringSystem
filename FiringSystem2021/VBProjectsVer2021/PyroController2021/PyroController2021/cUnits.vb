' (C) Copyright P. Cranwell, 2014
Public Class cUnits
    Private SessionIsActive As Integer
    Private IsActive As Boolean
    Private IsArmed As Integer
    Private UnitVoltage As Integer
    Private field As Integer
    Public Msg As cMessage
    Private i As Integer
    Private StatusData As String
    Public Ports(MaxPorts) As cPorts
    Private NumPorts As Integer
    Private Pins As String
    Private ShowID As String
    Private ValVoltage As String
    Private ValBut As Button
    Private ValText As TextBox
    Private ValTextLoc As Point
    Private ValThisTime As Boolean
    Private ValSlowPollReplyDelay As String
    Private ValNumberOfPorts As String
    Private ValMayBeActive As Boolean
    Private ValLat As String
    Private ValLon As String
    Private ValInShow As Boolean



    Public Property MayBeActive() As Boolean
        Get
            MayBeActive = ValMayBeActive
        End Get
        Set(ByVal value As Boolean)
            ValMayBeActive = value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Active = IsActive
        End Get
        Set(ByVal value As Boolean)
            IsActive = value
        End Set
    End Property
    Public Property Session() As Boolean
        Get
            Session = SessionIsActive
        End Get
        Set(ByVal value As Boolean)
            SessionIsActive = value
        End Set
    End Property
    Public Property ThisTime() As Boolean
        Get
            ThisTime = ValThisTime
        End Get
        Set(ByVal value As Boolean)
            ValThisTime = value
        End Set
    End Property
    Public Property Status() As String
        Get
            Status = StatusData
        End Get
        Set(ByVal value As String)
            StatusData = value
        End Set
    End Property
    Public Property SlowPollReplyDelay() As String
        Get
            SlowPollReplyDelay = ValSlowPollReplyDelay
        End Get
        Set(ByVal value As String)
            ValSlowPollReplyDelay = value
        End Set
    End Property
    Public Property NumberOfPorts() As String
        Get
            NumberOfPorts = ValNumberOfPorts
        End Get
        Set(ByVal value As String)
            ValNumberOfPorts = value
        End Set
    End Property
    Public Property PinStatus() As String
        Get
            PinStatus = Pins
        End Get
        Set(ByVal value As String)
            Pins = value
        End Set
    End Property
    Public Property Show() As String
        Get
            Show = ShowID
        End Get
        Set(ByVal value As String)
            ShowID = value
        End Set
    End Property
    Public Property Voltage() As String
        Get
            Voltage = ValVoltage
        End Get
        Set(ByVal value As String)
            ValVoltage = value
        End Set
    End Property
    Public Property Armed() As Boolean
        Get
            Armed = IsArmed
        End Get
        Set(ByVal value As Boolean)
            IsArmed = value
        End Set
    End Property
    Public Property But() As Button
        Get
            But = ValBut
        End Get
        Set(ByVal value As Button)
            ValBut = value
        End Set
    End Property

    Public Sub New(ByVal NumberOfPorts)
        Dim i As Integer
        NumPorts = NumberOfPorts

        Msg = New cMessage
        For i = 1 To NumberOfPorts
            Ports(i) = New cPorts()
        Next
        StatusData = ""
    End Sub

    Public Property Lat() As String
        Get
            Lat = ValLat
        End Get
        Set(ByVal value As String)
            ValLat = value
        End Set
    End Property

    Public Property Lon() As String
        Get
            Lon = ValLon
        End Get
        Set(ByVal value As String)
            ValLon = value
        End Set
    End Property
    
    Public Sub done()
        MyBase.Finalize()
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
