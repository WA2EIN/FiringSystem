' (C) Copyright P. Cranwell, 2014
Imports System
Imports System.IO.Ports
Imports System.IO
Imports System.Threading
Imports System.Diagnostics.Stopwatch
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.ApplicationServices


Module mModule1
    ' GPS Data Structure
    Structure PinButton
        Dim But As Button
        Dim ButLoc As Point
    End Structure
    Public Structure AreaStruct
        Public Area As String
        Public AreaNumber As Integer
    End Structure
    Public Structure CaliberStruct
        Public Caliber As String
        Public CalimerNum As Integer
    End Structure
    Public Structure ItemStructure
        Public Item As String
        Public ItemNumber As Integer
    End Structure
    Public Structure DatabaseStruct
        Public Cue As Integer
        Public Unit As Integer
        Public Slat As String
        Public Pin As Integer
        Public Time As Long
        Public Seq As Integer
        Public NextSeq As Integer
        Public AreaName As String
        Public AreaNumber As Integer
        Public Caliber As Integer
        Public ItemName As String
        Public ItemNumber As Integer
    End Structure
    Public Structure CueEventsStruct
        Public Cue As Integer
        Public EventTime As Long
        Public EventName As String
        Public UnitNumber As String
        Public PortNumber As String
    End Structure
    Structure ModuleData
        Dim ModuleNumber As Integer
        Dim Active As Boolean
        Dim PinStatus As String
        Dim Show As Integer
        Dim Armed As Boolean
        Dim Voltage As String
        Dim But As Button
        Dim Text As TextBox
        Dim TextLoc As Point
        Dim ThisTime As Boolean
        Dim PollReplyDelay As String

    End Structure
    Public Structure SerialMsg
        Public Reset As Byte
        Public Length As Byte
        Public Msg() As Byte
    End Structure
    Public Structure ShowT
        Public Unit As String                ' Unit Number
        Public CueNumber As String           ' Cue Number
        Public Port As String                ' Port Number
        Public NextSequence As String        ' Next Sequence or Null
        Public TimeBefore As String          ' Time before this event
    End Structure
    Public Structure StringObject
        Private _value As String
        Public Property value() As String
            Get
                Return Me._value
            End Get
            Set(ByVal value As String)
                Me._value = value
            End Set
        End Property

        Public Sub New(ByVal value As String)
            Me._value = value
        End Sub
    End Structure
    Public Const DEAD_MAN_INTERVAL As Integer = 30000
    Public DEBUG As Boolean = False
    Public SIMULATE As Boolean
    Public Const MAXNODES As Integer = 999
    Public Const MAXCUES As Integer = 999
    Public Const MAXPORTS As Integer = 432
    Public Const MAXEVENTS As Integer = 9999
    Public Areas(999) As AreaStruct
    Public Calibers(999) As CaliberStruct
    Public Items(999) As ItemStructure
    Public DatabaseData(MAXEVENTS) As DatabaseStruct
    Public DatabaseDataIndex As Integer
    Public DatabaseDataHighIndex As Integer
    Public cStartup As String = "1"
    Public Const iStartup As Integer = 1
    Public Const cShutdown As Char = "2"
    Public Const iShutdown As Integer = 2
    Public Const cSafe As Char = "3"
    Public Const iSafe As Integer = 3
    Public Const cFire As Char = "4"
    Public Const iFire As Integer = 4
    Public Const iSense As Integer = 5
    Public Const cSense As Char = "5"
    Public Const iTrace As Integer = 6
    Public cACK As String = "7"
    Public Const iACK As Integer = 7
    Public Const iNAK As Integer = 8
    Public cNAK As String = "8"
    Public Const OK As Integer = 1
    Public Const NG As Integer = 0
    Public Const None As Integer = 0
    Public Const Input As Integer = 1
    Public Const Output As Integer = 2
    Public Const Radioin As Integer = 3
    Public Const Radioout As Integer = 4
    Public Const MaxPacketSize As Integer = 130
    Public Const MaxReplyLength As Integer = 47
    Public GlobalResetData As String = "1019999999900000000"
    Public TimeMsg As String = "1019999980000000000"
    Public TimeBase As String = "10199999800"
    Public ShowTime As Long
    Public Msg As cMessage
    Public TimeMessage As String
    Public UnitIndex As Integer
    Public UnitArray(MAXCUES) As String
    Public SessionStatus As Boolean = False
    Public EventList(256) As cWorkUnit
    Public forms(10) As Form
    Public AllowBump As Boolean
    Public Direct As Boolean
    Public ShowStarted As Boolean
    Public ctr As Integer
    Public TimerStarted As Boolean
    Public ShowType As String
    Public Echo As Boolean
    Public Cmd = New cCommand
    Public SerialPortName As String
    Public ModuleTest As Boolean
    Public LowUnit As Integer
    Public HighUnit As Integer
    Public TW As TextWriter
    Public MsgGood(MAXNODES) As Integer
    Public MsgBad(MAXNODES) As Integer
    Public UnitStat As Integer
    Public MSStart As Long
    Public DirPath As String
    Public TRACEACTIVE As Boolean
    Public Port As String
    Public TracePortName As String = "COM7"
    Public TracePort As cTraceComm
    Public TestRunning As Boolean
    Public NetworkLatency As Integer
    Public CommPort As cComm
    Public CSVSelectedColumn As Integer
    Public NumActiveUnits As Integer
    Public TopologyInitialized As Boolean = False
    Public TOD As Long         ' Keeps track of Time after Cue is fired



    Public Units(MAXNODES) As cUnits                ' Field Module definitions
    Public ShowEvents(MAXEVENTS) As CueEventsStruct     ' Show Event Array 

    Public ModuleIndex As Integer


    Public Sub Traceit(ByVal Msg As String)
        Dim line As String
        Dim Now As Date
        Dim MS As Double
        Dim MSDIFF As Double


        If (TRACEACTIVE = True) Then
            Now = TimeValue(TimeString)
            line = Now.Hour & ":" & Now.Minute & ":" & Now.Second & " " & Msg
            MS = ((Now.Hour * 3600) + Now.Minute * 60 + Now.Second) * 1000

            If MSStart = 0 Then
                MSStart = MS
            End If

            MSDIFF = MS - MSStart
            MSStart = MS

            fMain.ListBox1.Items.Add(line)
            line = line & vbCrLf

            If MSDIFF = 0 Then MSDIFF = 500

            ' Build a test case to enable testing with the PIC simulator.
            Try
                BuildTestCase(Msg, MSDIFF)


                My.Computer.FileSystem.WriteAllText(DirPath & "trace.txt", _
                line, True)
            Catch
            End Try



        End If

        Application.DoEvents()


    End Sub
    Public Sub Wait()
        Thread.Sleep(17)
    End Sub
    Public Sub xLongWait(ByVal MS As Integer)
        Dim interval As Integer
        interval = MS / 100
        For i = 1 To 100
            Thread.Sleep(interval)
            Application.DoEvents()

        Next
    End Sub
    Private Declare Function GetTickCount Lib "kernel32" () As Long
    Public Sub LongWait(MS As Integer)

        Dim lngTarget, lngNow As Long

        lngTarget = GetTickCount() + MS
        lngNow = GetTickCount()


        Do While (lngTarget > lngNow)
            lngNow = GetTickCount()
            Application.DoEvents()
        Loop

    End Sub

    Public Sub LongWait2(MS As Integer)

        Dim lngTarget, lngNow As Long
        Dim ct As Integer = 0


        lngNow = GetTickCount()
        lngTarget = lngNow + MS


        Do While (lngTarget > lngNow)
            lngNow = GetTickCount()
            Application.DoEvents()
            ct += 1
        Loop
        ct = ct
    End Sub
    Public Sub BuildTestCase(ByVal data As String, ByVal tdiff As Long)
        ' Build test case suitable for use by the PIC simulator.
        '  This will allow offline diagnosis of any problem by recreating the problem.
        '  The only difference is the time stamp which may not be accurate.
        '  The testcase, however, mainains a complete record, in test case format, of all network activity' created by the program.
        '  It does not contain PIC responses.  

        Dim caseline As String
        Dim workstring As String
        Dim Code As String
        Dim Ch As String

        Dim L As Integer
        Dim i, a, b As Integer
        Dim HexTable As String = "0123456789ABCDEF"
        Dim tvalue As String

        If (Mid$(data, 1, 3)) = "==>" Then

            workstring = Mid$(data, 4)
            L = Len(workstring)
            a = (L >> 4)
            b = L - (a * 16)

            Code = Mid$(HexTable, a + 1, 1) & Mid$(HexTable, b + 1, 1) & " "

            caseline = "FF FF FF " & Code

            For i = 1 To L
                Ch = "3" & Mid$(workstring, i, 1) & " "
                caseline = caseline & Ch
            Next


            tvalue = "wait " & tdiff & " ms"
            Try
                My.Computer.FileSystem.WriteAllText(DirPath & "testcase.txt", _
                 tvalue & vbCrLf, True)
                My.Computer.FileSystem.WriteAllText(DirPath & "testcase.txt", _
               caseline & vbCrLf, True)
            Catch
                Beep()
                MsgBox("Creating Trace Directory.  Restart System")
                CreateDirectory()
                End
            End Try
        ElseIf (Mid$(data, 1, 3)) = "<==" Then

            workstring = Mid$(data, 4)
            L = Len(workstring)
            a = (L >> 4)
            b = L - (a * 16)

            Code = Mid$(HexTable, a + 1, 1) & Mid$(HexTable, b + 1, 1) & " "

            caseline = "FF FF FF " & Code

            For i = 1 To L
                Ch = "3" & Mid$(workstring, i, 1) & " "
                caseline = caseline & Ch
            Next


            tvalue = "wait " & tdiff & " ms"
            Try
                My.Computer.FileSystem.WriteAllText(DirPath & "reversetestcase.txt", _
                 tvalue & vbCrLf, True)
                My.Computer.FileSystem.WriteAllText(DirPath & "reversetestcase.txt", _
               caseline & vbCrLf, True)
            Catch
                Beep()
                ' MsgBox("Creating Trace Directory.  Restart System")
                ' CreateDirectory()
                'End
            End Try

        End If

    End Sub
    

    Public Sub CleanRegistry()
        DeleteSetting("PYRO2014")
    End Sub
    Public Function GetDirPath() As String
        GetDirPath = GetSetting("PYRO2014", "New", "DirPath", vbNullString)
    End Function
    Public Function GetComPort() As String
        GetComPort = GetSetting("PYRO2014", "New", "Port", vbNullString)
    End Function
    Public Function GetNetworkLatency() As String
        GetNetworkLatency = GetSetting("PYRO2014", "New", "NetworkLatency", vbNullString)
    End Function
    Public Function GetMaxUnits() As String
        GetMaxUnits = GetSetting("PYRO2014", "New", "MaxUnits", vbNullString)
    End Function
    Public Function GetNetworkTopology()
        GetNetworkTopology = GetSetting("PYRO2014", "New", "NetworkTopology", vbNullString)
    End Function
    Public Sub SetComPort(ByVal NewPort As String)
        SaveSetting("PYRO2014", "New", "Port", NewPort)
        Port = NewPort
        Try
            CommPort.Cleanup()
        Catch
        End Try

        CommPort = New cComm
        CommPort.OpenPort()
    End Sub
    Public Sub CreateDirectory()
        Try
            Directory.CreateDirectory(DirPath)
        Catch
            Beep()
        End Try

    End Sub
    Public Sub InitUnits()
        Dim Data As String
        Dim HighUnit As Integer
        Dim i As Integer
        Dim AnyUnit As Integer = 0

        For i = 0 To MAXNODES                      ' Set up all possible Field Modules 
            Units(i) = New cUnits(MAXPORTS)    ' sets up the Field Module Ports at instanciation
            ' This will be changed from MAXPORTS to the number
            ' of ports on the Field Module, as defined by a system definition file. (TBD)

            Units(i).Armed = False
            Units(i).Session = False
            Units(i).MayBeActive = False
            Units(i).Status = ""
            Units(i).Voltage = 0
            'Units(i).NumberOfPorts = 16
            Units(i).NumberOfPorts = MAXPORTS
        Next i

        Data = GetNetworkTopology()
        If Data = "" Then
            fNetworkTopology.Show()
            Data = GetNetworkTopology()
            MsgBox("Network Topology needs to be defined")
            Exit Sub
        End If

        ' Bank 900 - 999
        HighUnit = Data.Substring(27, 3)
        If HighUnit > 0 Then
            AnyUnit = 1

            For i = 900 To HighUnit
                Units(i).MayBeActive = True
            Next
        End If

        ' 800 - 899 bank
        HighUnit = Data.Substring(24, 3)
        If HighUnit > 0 Then
            AnyUnit = 1
            For i = 800 To HighUnit
                Units(i).MayBeActive = True
            Next

        End If


        ' 700 - 799 bank
        HighUnit = Data.Substring(21, 3)
        If HighUnit > 0 Then
            AnyUnit = 1
            For i = 700 To HighUnit
                Units(i).MayBeActive = True
            Next
        End If

        ' 600 - 699 bank
        HighUnit = Data.Substring(18, 3)
        If HighUnit > 0 Then
            AnyUnit = 1
            For i = 600 To HighUnit
                Units(i).MayBeActive = True
            Next
        End If


        ' 500 - 599 bank
        HighUnit = Data.Substring(15, 3)
        If HighUnit > 0 Then
            AnyUnit = 1
            For i = 500 To HighUnit
                Units(i).MayBeActive = True
            Next
        End If

        ' 400 - 499 bank
        HighUnit = Data.Substring(12, 3)
        If HighUnit > 0 Then
            AnyUnit = 1
            For i = 400 To HighUnit
                Units(i).MayBeActive = True
            Next
        End If

        ' 300 - 399 bank
        HighUnit = Data.Substring(9, 3)
        If HighUnit > 0 Then
            AnyUnit = 1
            For i = 300 To HighUnit
                Units(i).MayBeActive = True
            Next
        End If

        ' 200 - 299 bank
        HighUnit = Data.Substring(6, 3)
        If HighUnit > 0 Then
            AnyUnit = 1
            For i = 200 To HighUnit
                Units(i).MayBeActive = True
            Next
        End If

        ' 100 -199 bank
        HighUnit = Data.Substring(3, 3)
        If HighUnit > 0 Then
            AnyUnit = 1
            For i = 100 To HighUnit
                Units(i).MayBeActive = True
            Next
        End If

        ' 0 - 99 bank
        HighUnit = Data.Substring(0, 3)
        If HighUnit > 0 Then
            AnyUnit = 1
            For i = 0 To HighUnit
                Units(i).MayBeActive = True
            Next
        End If

        If AnyUnit = 0 Then
            MsgBox("Network Topology Needs to be defined.")
        End If

        TopologyInitialized = True

    End Sub

End Module
