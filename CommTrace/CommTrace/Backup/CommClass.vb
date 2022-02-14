
Imports System
Imports System.IO.Ports
Imports System.Threading
Imports System.Diagnostics.Stopwatch
Imports Microsoft.VisualBasic
Public Class CommClass
    '===================================================
    ' Session Start begins when the host sends a "1" to the controller.
    '   This process continues until the controller responds with a "2"
    '   to indicate that the session has started and the controller has
    '   entered "Passthru" mode.
    '
    ' After the session is active, communication to the remote field controllers follows the same format
    '   as normal hand controller communications.
    '
    '===================================================


    Shared _continue As Boolean
    Shared _serialPort As SerialPort
    Private Message As String
    Public Reply As String

    Private ComPortName As String = "COM6"
    Private ComBaudRate As Integer = 38400
    Private NextSendNumber As Integer
    Private NextRecNumber As Integer
    Private RecordNumber() = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}
    Private Sent As String   ' Used to remove Echo from A-WIT Serial port
    Private ACK As String = "07"
    Private NAK As String = "08"
    Private ReplyStatus As String
    Private UnitVoltage As String
    Private ReplyMsgNumber
    Private ReplyCode As String
    Private RawReply As String
    Private CCode As String
    Private Trace As Boolean
    Private Retries As Integer

    Public Property PortName() As String
        Get
            Return ComPortName
        End Get

        Set(ByVal value As String)
            _serialPort.Close()
            ComPortName = value
            _serialPort.PortName = ComPortName
            _serialPort.Open()
        End Set
    End Property
    Public ReadOnly Property Status()
        Get
            Return ReplyStatus
        End Get
    End Property
    Public ReadOnly Property Data()
        Get
            Return Message
        End Get
    End Property
    Public ReadOnly Property Voltage()
        Get
            Return UnitVoltage
        End Get
    End Property
    Public Property BaudRate()
        Get
            Return ComBaudRate
        End Get
        Set(ByVal value)
            ComBaudRate = value
            _serialPort.BaudRate = ComBaudRate
            _serialPort.Open()
        End Set
    End Property
    Public Property NumRetries()
        Get
            Return Retries
        End Get
        Set(ByVal value)
            Retries = value
        End Set
    End Property
    Property SendNumber()
        Get
            Return RecordNumber(NextSendNumber)
        End Get
        Set(ByVal value)
            NextSendNumber = value
        End Set
    End Property
    Property ReadTOT()
        Get
            Return _serialPort.ReadTimeout
        End Get
        Set(ByVal value)
            _serialPort.ReadTimeout = value
        End Set
    End Property
    Property RecNumber()
        Get
            Return RecordNumber(NextRecNumber)
        End Get
        Set(ByVal value)
            NextRecNumber = value
        End Set
    End Property
    Public Sub New()

        Dim sComparer As StringComparer = StringComparer.OrdinalIgnoreCase

        ' Create a new SerialPort object with default settings.


        _serialPort = New SerialPort()

        _serialPort.PortName = ComPortName
        _serialPort.BaudRate = ComBaudRate
        _serialPort.Parity = Parity.None
        _serialPort.DataBits = 8
        _serialPort.StopBits = 1
        _serialPort.Handshake = Handshake.None

        ' Set the read/write timeouts
        _serialPort.ReadTimeout = 2000
        _serialPort.ReadTimeout = 300
        _serialPort.WriteTimeout = 300


        _serialPort.Open()

        ResetMessageNumbers()
        Sent = ""
        Reply = ""
        Retries = 3
        PurgePort()
        Trace = True

    End Sub
    Public Sub ResetMessageNumbers()
        NextSendNumber = 1
        NextRecNumber = 1
    End Sub
    Public Sub PurgePort()
        ' While _serialPort.BytesToRead > 0
        '_serialPort.ReadChar()
        ' End While


    End Sub

    Public Function Transaction(ByVal Msg As String) As String

        Dim retry As Integer
        Dim Length As Integer


        For retry = 1 To Retries
            Thread.Sleep(5)
            WriteVarString(Msg)
            If Echo Then
                ' Remove Echo if partner echoes data
                Trace = False
                Length = ReadVarString()
                Trace = True
            End If
            Length = ReadVarString()
            Reply = ComPort.Data
            If Length > 0 Then
                ParseReply()
                If ReplyCode = ACK Then
                    AllowBump = True
                    If TimerStarted Then MainForm.Timer1.Enabled = True
                    Return Status
                End If
            End If
            PurgePort()

            If retry < Retries Then
                Traceit("Retry Transmission")
            End If
        Next
        AllowBump = False

        Return ""
    End Function

    Public Sub Broadcast(ByVal Msg As String)
        Dim Echodata As String


        PurgePort()

        WriteVarString(Msg)

        'RemoveEcho
        If Echo Then
            Echodata = ReadVarString()
        End If




        Return
    End Sub

    Public Function ReadString(ByVal Len As Integer) As String

        While (True)
            Try
                Message = ""
                ' Read up to Len characters
                For i = 1 To Len
                    Message = Message & Chr(_serialPort.ReadByte)
                Next i

                Traceit("<==" & Message)
                If TimerStarted Then MainForm.Timer1.Enabled = True

                Return Message

            Catch ex As TimeoutException
                'Traceit("TOT")
                'MainForm.ListBox1.Items.Add(Message)
                If TimerStarted Then MainForm.Timer1.Enabled = True

                Return ""
            End Try
        End While

        Return Message
    End Function
    Public Function ReadVarString() As Integer
        Dim Len As Integer

        ' First character of the Message is a binary value
        ' defining the remaining number of characters to read
        Application.DoEvents()
        While (True)
            Try

                Message = ""
                Len = Val(_serialPort.BaseStream.ReadByte)
                If Len = 255 Then
                    Len = Val(_serialPort.BaseStream.ReadByte)
                End If
                ' Read up to Len characters
                For i = 1 To Val(Len)
                    Message = Message & Chr(_serialPort.ReadByte)
                Next i
                'If Trace Then
                'Traceit(Len & "  " & "<==" & Message)
                'End If

                If TimerStarted Then MainForm.Timer1.Enabled = True

                Return Val(Len)

            Catch ex As TimeoutException
                'Traceit("TOT")
                ' MainForm.ListBox1.Items.Add(ComPort.Data)
                Application.DoEvents()

                If TimerStarted Then MainForm.Timer1.Enabled = True

                Return -1
            End Try
        End While
    End Function
    Public Sub WriteString(ByVal Msg As String)
        Dim a As Integer

        Thread.Sleep(5)
        For i = 0 To Len(Msg) - 1
            a = Len(Msg)
            _serialPort.Write(Msg(i))
        Next

        Traceit("==>" & Msg)

    End Sub
    Public Function StrToByteArray(ByVal str As String) As Byte()

        Dim encoding As New System.Text.UTF8Encoding()

        Return encoding.GetBytes(str)

    End Function
    Public Sub WriteVarString(ByVal Msg As String)

        Dim Length As Integer
        Dim Message As New SerialMsg


        Message.Length = Len(Msg)

        If Direct = False Then
            Length = Length - 1
        End If

        Message.Msg = StrToByteArray(Msg)

        Thread.Sleep(1)

        _serialPort.BaseStream.WriteByte(Message.Length)
        Thread.Sleep(3)    ' Delay for Software Uart Reads
        _serialPort.Write(Message.Msg, 0, Len(Msg))

        Traceit("==>" & Msg)

    End Sub
    Private Sub ParseReply()

        Try
            ReplyMsgNumber = Left$(Reply, 1)
            'ReplyCode = Mid$(Reply, 4, 2)     ' Incorrect ACK/NAK format
            'ReplyStatus = Mid$(Reply, 6)
            ReplyCode = Mid$(Reply, 6, 2)
            ReplyStatus = Mid$(Reply, 8)
            If Len(Reply) > 5 Then
                'ReplyStatus = Mid$(Reply, 9, 34)
                'a = InStr(Reply, " ")
                'Units(UnitIndex).Voltage = Mid$(Reply, a + 2, 4)
                Return
            Else
                ReplyStatus = ""
                Return
            End If

        Catch ex As Exception
            Reply = ""
            Traceit("Reply error")
            Return
        End Try

    End Sub
    Private Function GenerateCheckCode(ByVal Msg As String) As String
        Dim IntTemp, i, d, c, cd, cc As Integer
        Dim fact() As Integer = {3, 7, 1}
        Dim cTemp As String
        Dim Codetab() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}

        cd = 0
        cTemp = ""
        i = 0

        For c = 1 To Len(Msg)

            If i > 2 Then i = 0
            d = fact(i)
            cTemp = Mid$(Msg, c, 1)
            cd = cd + (d * Val(cTemp))
            i = i + 1

        Next c


        ' calculate remainder of div by 10
        IntTemp = cd Mod 10
        cc = Math.Abs(IntTemp)
        Return Codetab(cc)
    End Function
    Public Sub Cleanup()
        _serialPort.Close()
    End Sub




End Class
