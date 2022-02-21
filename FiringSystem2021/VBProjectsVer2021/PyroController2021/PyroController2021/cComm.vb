' (C) Copyright P. Cranwell, 2014
Imports System
Imports System.IO.Ports
Imports System.Threading
Imports System.Diagnostics.Stopwatch
Imports Microsoft.VisualBasic
Public Class cComm
    Shared _continue As Boolean
    Shared _serialPort As SerialPort
    Private Message As String
    Public Reply As String
    Private ComPortName As String = SerialPortName
    Private ComBaudRate As Integer = 38400
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
    Private ErrorPosted As Boolean = False

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
    Property ReadTOT()
        Get
            Return _serialPort.ReadTimeout
        End Get
        Set(ByVal value)
            _serialPort.ReadTimeout = value
        End Set
    End Property
    Public Sub New()

        Dim sComparer As StringComparer = StringComparer.OrdinalIgnoreCase

        ' Create a new SerialPort object with default settings.
        ComPortName = Port
        _serialPort = New SerialPort()
        _serialPort.PortName = Port
        _serialPort.BaudRate = ComBaudRate
        _serialPort.Parity = Parity.None
        _serialPort.DataBits = 8
        _serialPort.StopBits = 2
        _serialPort.Handshake = Handshake.None

        ' Set the read/write timeouts
        _serialPort.ReadTimeout = 500
        _serialPort.WriteTimeout = 500

        Try
            _serialPort.Close()


            _serialPort.Open()
        Catch
            MsgBox("Comm port not available")
            Exit Sub

        End Try


        'ResetMessageNumbers()
        Sent = ""
        Reply = ""
        Retries = 3
        PurgePort()

        Trace = True

    End Sub
    Public Sub PurgePort()
        While _serialPort.BytesToRead > 0
            _serialPort.ReadChar()
        End While
    End Sub
    Public Function Transaction(ByVal Msg As String, ByVal Unit As Integer) As String

        Dim retry As Integer
        Dim Length As Integer
        Dim swatch As New cStopWatch
        'TraceForm.Show()

        LongWait(NetworkLatency)
        For retry = 1 To Retries
            'swatch.Start()
            WriteVarString(Msg)
            'swatch.ReportToConsole(Msg & "Write Complete")

            'swatch.Start()
            Length = ReadVarString()
            Reply = Me.Data
            'swatch.ReportToConsole("Reply = " & Reply)


            If Length > 0 Then
                ParseReply()
                If ReplyCode = ACK Then
                    AllowBump = True
                    If TimerStarted Then fMain.Timer1.Enabled = True
                    Try
                        fUnitStatus.GoodTextBoxArray(UnitStat).Text = fUnitStatus.GoodTextBoxArray(UnitStat).Text + 1
                    Catch
                        ' Beep()
                    End Try

                    MsgGood(UnitStat) = MsgGood(UnitStat) + 1
                    LongWait(25 + NetworkLatency)
                    Application.DoEvents()


                    Return Status
                End If

                If ReplyCode = NAK Then
                    ' If NAK received, it means slave has reset and message numbers are now 1,1
                    ' The retry operation will now restart with message number 1

                    'NOTE: A slave may reset due to network interference or power glitch.
                    '      If a complete message is not processed, within 1/2 sec of the beginning
                    '      of a message, the slave will reset itself and expect starting message numbr 1

                    Msg = Mid$(Msg, 2)
                    Msg = "1" & Msg
                    Length = Len(Msg)
                    Msg = Mid$(Msg, 1, Length - 1)
                    CCode = GenerateCheckCode(Msg)
                    Msg = Msg & CCode
                    Units(Unit).Msg.NextReceiveNumber = 1
                    Units(Unit).Msg.NextSendNumber = 1
                End If
            End If
            PurgePort()

            If retry < Retries Then
                Traceit("Retry Transmission")
                MsgBad(UnitStat) = MsgBad(UnitStat) + 1
                Try
                    fUnitStatus.TOTTextBoxArray(UnitStat).Text = fUnitStatus.TOTTextBoxArray(UnitStat).Text + 1
                Catch
                End Try

            End If
        Next
        AllowBump = False
        Try
            fUnitStatus.BadTextBoxArray(UnitStat).Text = fUnitStatus.BadTextBoxArray(UnitStat).Text + 1
        Catch
        End Try
        LongWait(25 + NetworkLatency)

        Return ""
    End Function
    Public Sub Broadcast(ByVal Msg As String)
        WriteVarString(Msg)
        LongWait(25 + NetworkLatency)
    End Sub
    Public Sub BroadcastReset(ByVal Msg As String)
        WriteHardReset(Msg)
        LongWait(25 + NetworkLatency)
    End Sub


    Public Function ReadString(ByVal Len As Integer) As String
        While (True)
            Try
                Message = ""
                ' Read up to Len characters
                For i = 1 To Len
                    Message = Message & Chr(_serialPort.ReadByte)
                Next i

                If Message.Substring(3, 2) = "02" Then
                    Traceit("<==" & Message)
                Else
                    Traceit("==>" & Message)
                End If

                If TimerStarted Then fMain.Timer1.Enabled = True

                Return Message

            Catch ex As TimeoutException
                Traceit("TOT")

                If TimerStarted Then fMain.Timer1.Enabled = True

                Return ""
            End Try
        End While

        Return Message
    End Function
    Public Function ReadVarString() As Integer
        Dim Len As Integer
        Dim C As Byte

        ' First character of the Message is a binary value
        ' defining the remaining number of characters to read


        If TimerStarted Then fMain.Timer1.Enabled = False

        While (True)
            Try

                ' Look for Header Synch byte and strip it
                ' This can handle multiple Synch bytes

                Len = 255

                While Len > 254
                    Message = ""
                    C = _serialPort.BaseStream.ReadByte
                    Len = Val(C)
                End While

                ' Len has message length


                ' Read up to Len characters
                For i = 1 To Val(Len)
                    Message = Message & Chr(_serialPort.BaseStream.ReadByte)
                Next i

                'If Message.Substring(3, 2) = "02" Then
                Traceit("<==" & Message)
                'Else
                'Traceit("==>" & Message)
                ' End If



                If TimerStarted Then fMain.Timer1.Enabled = True


                Return Val(Len)

            Catch ex As TimeoutException
                'Traceit("TOT")
                Application.DoEvents()
                If TimerStarted Then fMain.Timer1.Enabled = True

                Return -1
            End Try
        End While
        Return -1
    End Function
    Public Sub WriteString(ByVal Msg As String)
        Dim a As Integer
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
        Dim Message As New SerialMsg

        If TimerStarted Then fMain.Timer1.Enabled = False
        Message.Length = Len(Msg)

        Message.Msg = StrToByteArray(Msg)
        Message.Length = Message.Length

        LongWait(NetworkLatency)
        Try
            If (_serialPort.BaseStream.CanWrite) Then
                _serialPort.BaseStream.WriteByte(255)
                _serialPort.BaseStream.WriteByte(255)
                _serialPort.BaseStream.WriteByte(Message.Length)
                _serialPort.Write(Message.Msg, 0, Len(Msg))

                Traceit("==>" & Msg)
            Else
                fMain.ListBox1.Items.Add("  ************************Cant write")
            End If
        Catch
            If ErrorPosted = False Then
                ErrorPosted = True
                MsgBox("Comm Port not active")
                End
            End If

        End Try

        Application.DoEvents()

        If TimerStarted Then fMain.Timer1.Enabled = True
    End Sub

    Public Sub WriteHardReset(ByVal Msg As String)
        Dim Message As New SerialMsg

        If TimerStarted Then fMain.Timer1.Enabled = False
        Message.Length = Len(Msg)

        Message.Msg = StrToByteArray(Msg)
        Message.Length = Message.Length

        LongWait(NetworkLatency)
        Try
            If (_serialPort.BaseStream.CanWrite) Then
                _serialPort.BaseStream.WriteByte(255)
                _serialPort.BaseStream.WriteByte(255)
                _serialPort.BaseStream.WriteByte(255)
                _serialPort.BaseStream.WriteByte(255)
                _serialPort.BaseStream.WriteByte(255)
                _serialPort.BaseStream.WriteByte(Message.Length)
                _serialPort.Write(Message.Msg, 0, Len(Msg))

                Traceit("==>" & Msg)
            Else
                fMain.ListBox1.Items.Add("  ************************Cant write")
            End If
        Catch
            If ErrorPosted = False Then
                ErrorPosted = True
                MsgBox("Comm Port not active")
                End
            End If

        End Try

        Application.DoEvents()

        If TimerStarted Then fMain.Timer1.Enabled = True
    End Sub




    Private Sub ParseReply()

        Try
            ReplyMsgNumber = Left$(Reply, 1)
            ReplyCode = Mid$(Reply, 7, 2)
            ReplyStatus = Mid$(Reply, 9)
            If Len(Reply) > 5 Then

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
    Sub OpenPort()

        Try
            _serialPort.Close()
            _serialPort = New SerialPort()
            _serialPort.PortName = Port
            _serialPort.BaudRate = ComBaudRate
            _serialPort.Parity = Parity.None
            _serialPort.DataBits = 8
            _serialPort.StopBits = 2
            _serialPort.Handshake = Handshake.None

            ' Set the read/write timeouts
            _serialPort.ReadTimeout = 500
            _serialPort.WriteTimeout = 300
            _serialPort.Open()
        Catch
            If ErrorPosted = False Then
                ErrorPosted = True
                MsgBox("COMM Port not available. Setup COMM Port")
                End
            End If

        End Try

    End Sub
    Public Sub Cleanup()
        _serialPort.Close()
    End Sub



    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
 
