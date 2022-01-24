' (C) Copyright P. Cranwell, 2014

' This class provides an interface to issue commands to field modules

Imports System
Imports System.IO.Ports
Imports System.Threading
Imports System.Diagnostics.Stopwatch
Public Class cCommand
    Private WU As New cWorkUnit
    Private Status As Integer
    Private StatusMessage As String
    Private GlobalResetMessage As String
    Private UnitNumber As Integer
    'Private CueNumber As Integer
    Private Retry As Boolean
    Dim swatch = New cStopWatch
    Public ReadOnly Property Stat(ByVal UnitIndex As Integer) As String

        Get
            Dim WU = New cWorkUnit
            Dim Reply As String
            Dim i As Integer
            UnitStat = UnitIndex
            Reply = ""
            For i = 1 To 10
                WU.Port = "000"
                Units(UnitIndex).Msg.BumpSendNumber()
                Units(UnitIndex).Msg.NewMessage()
                Units(UnitIndex).Msg.Command = "05"
                Units(UnitIndex).Msg.Unit = UnitIndex
                Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
                StatusMessage = Units(UnitIndex).Msg.CompleteMessage
                Reply = CommPort.Transaction(StatusMessage, UnitIndex)
                If Len(Reply) > 15 Then i = 10
            Next

            If Len(Reply) < 16 Then
                Traceit("Transaction Error")
                Return ""
            End If

            StatusMessage = Reply

            Return StatusMessage
        End Get

    End Property
    Public Function QueryBuildDate(ByVal UnitIndex As Integer) As String
        Dim WU = New cWorkUnit
        Dim Reply As String
        Dim i As Integer
        UnitStat = UnitIndex
        Reply = ""
        For i = 1 To 10
            WU.Port = "000"
            Units(UnitIndex).Msg.BumpSendNumber()
            Units(UnitIndex).Msg.NewMessage()
            Units(UnitIndex).Msg.Command = "25"
            Units(UnitIndex).Msg.Unit = UnitIndex
            Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
            StatusMessage = Units(UnitIndex).Msg.CompleteMessage
            Reply = CommPort.Transaction(StatusMessage, UnitIndex)
            If Len(Reply) > 15 Then i = 10
        Next

        If Len(Reply) < 16 Then
            Traceit("Transaction Error")
            QueryBuildDate = ""
        End If

        StatusMessage = Reply

        QueryBuildDate = StatusMessage

    End Function

    Public WriteOnly Property Unit() As Integer
        Set(ByVal value As Integer)
            UnitNumber = value
            UnitStat = UnitNumber
        End Set
    End Property
    Public Sub TriggerCue(ByVal Cue As String)
        ' This method broadcasts a Trigger CUE message to all units
        Dim WU = New cWorkUnit
        Dim Unit = New cUnits(MaxPorts)
        Dim TriggerCueMsg As String
        Dim Reply As String

        WU.Port = Cue

        Unit.Msg.NewMessage()
        Unit.Msg.Command = "44"
        Unit.Msg.Unit = "999"
        Unit.Msg.AddWU(WU.WorkUnit)
        TriggerCueMsg = Unit.Msg.CompleteMessage
        Reply = ""
        CommPort.Broadcast(TriggerCueMsg)
        Thread.Sleep(50)
        Unit.done()
        WU.done()
        Traceit("Cue " & Cue & " Triggered")
    End Sub
    Public Sub MasterReset()
        ' Master Reset Resets all units to starting state.
        ' It generates a broadcast message and then resets all message numbers to 1


        Msg = New cMessage
        WU.Port = "999"
        Units(0).Msg.NextSendNumber = 1
        Units(0).Msg.NextReceiveNumber = 1
        Units(0).Msg.NewMessage()
        Units(0).Msg.Unit = "999"
        Units(0).Msg.Command = "99"
        Units(0).Msg.AddWU(WU.WorkUnit)
        GlobalResetMessage = Units(0).Msg.CompleteMessage

        For i = 1 To 999
            Units(i).Msg.NextSendNumber = 1
            Units(i).Msg.NextReceiveNumber = 1
            Units(i).Msg.NewMessage()
            Units(i).Armed = False
            Units(i).Session = False
            MsgBad(i) = 0
            MsgGood(i) = 0
        Next

        ShowTime = 0
        Try

            CommPort.Broadcast(GlobalResetMessage)
        Catch
            MsgBox("Comm port not active")
        End Try

    End Sub
    Public Sub SetSlowPollDelay(ByVal DelayString As String)
        ' SetSlowPollDelay broadcasts the Slow Poll delay for all active units

        Dim SlowPollDelayMessage As String

        ' Delay String is in the format AAADDDDD   where AAA is the field module physical address and
        '                                                DDDDD is the module Slow Poll Delay is MS

        Msg = New cMessage
        WU.Port = "999"
        Units(0).Msg.NextSendNumber = 1
        Units(0).Msg.NextReceiveNumber = 1
        Units(0).Msg.NewMessage()
        Units(0).Msg.Unit = "999"
        Units(0).Msg.Command = "93"
        Units(0).Msg.AddWU("999" & DelayString)
        SlowPollDelayMessage = Units(0).Msg.CompleteMessage


        ShowTime = 0
        Try

            CommPort.Broadcast(SlowPollDelayMessage)
        Catch
            MsgBox("Comm port not active")
        End Try

    End Sub
    Public Sub ShowIDAndSoftwareVersion()
        ' Generate broadcast message to display Unit ID (Red Led) and Software Version (Green LED)
        Dim IDMessage As String

        Msg = New cMessage
        WU.Port = "999"
        Units(0).Msg.NextSendNumber = 1
        Units(0).Msg.NextReceiveNumber = 1
        Units(0).Msg.NewMessage()
        Units(0).Msg.Unit = "999"
        Units(0).Msg.Command = "97"
        Units(0).Msg.AddWU(WU.WorkUnit)
        IDMessage = Units(0).Msg.CompleteMessage
        CommPort.Broadcast(IDMessage)
    End Sub
    Public Sub SetUnitAddress(ByVal Address As String)
        ' This method Sets the unit address for any field module on the network.
        ' NOTE:  Only one field module should be on the network.

        Dim SetAddressMessage As String

        Msg = New cMessage
        WU.Port = Address
        Units(0).Msg.NextSendNumber = 1
        Units(0).Msg.NextReceiveNumber = 1
        Units(0).Msg.NewMessage()
        Units(0).Msg.Command = "33"
        Units(0).Msg.Unit = "999"
        Units(0).Msg.AddWU(WU.WorkUnit)
        SetAddressMessage = Units(0).Msg.CompleteMessage

        ' two duplicate, sequential messages are required to change the unit address.

        CommPort.Broadcast(SetAddressMessage)
        Thread.Sleep(250)
        CommPort.Broadcast(SetAddressMessage)

        Traceit("Unit " & UnitIndex & " Address Set")

    End Sub
    Public Function SaveShowInEPROM(ByVal ShowID As String, ByVal UnitIndex As Integer)
        ' Save the show in EEPROM
        Dim ProgramUnitMessage As String
        Dim WU As New cWorkUnit
        Dim tot As Integer
        Dim Reply As String
        UnitStat = UnitIndex

        WU.Port = Right("00" & ShowID, 3)
        Units(UnitIndex).Msg.BumpSendNumber()
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "07"
        Units(UnitIndex).Msg.Unit = UnitNumber
        Units(UnitNumber).Msg.AddWU(WU.WorkUnit)
        ProgramUnitMessage = Units(UnitNumber).Msg.CompleteMessage
        Reply = ""
        tot = CommPort.ReadTOT
        ' Wait for PIC to write show to EEPROM
        '    Under normal conditions, it should take approx 3 seconds without
        '    network delays
        CommPort.ReadTOT = 6000
        Reply = CommPort.Transaction(ProgramUnitMessage, UnitNumber)
        If Reply = "" Then
            Traceit("Transaction Error")
            SaveShowInEPROM = False
        Else
            Traceit("Unit Programmed in slot # " & ShowID & " For Unit " & UnitNumber)
            SaveShowInEPROM = True
        End If

        ' restore TOT
        CommPort.ReadTOT = tot

    End Function
    Public Function LoadShow(ByVal ShowID As String, ByVal UnitIndex As Integer)
        ' Load Show from EEPROM
        Dim ProgramUnitMessage As String
        Dim WU As New cWorkUnit
        Dim tot As Integer
        Dim Reply As String

        UnitStat = UnitIndex
        WU.Port = Right("00" & ShowID, 3)
        Units(UnitIndex).Msg.BumpSendNumber()
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "08"
        Units(UnitIndex).Msg.Unit = UnitNumber
        Units(UnitNumber).Msg.AddWU(WU.WorkUnit)
        ProgramUnitMessage = Units(UnitNumber).Msg.CompleteMessage
        Reply = ""
        tot = CommPort.ReadTOT

        ' Wait for PIC to read show from EEPROM
        '    Under normal conditions, it should take approx 3 seconds without
        '    network delays

        CommPort.ReadTOT = 4000
        Reply = CommPort.Transaction(ProgramUnitMessage, UnitNumber)
        If Reply = "" Then
            Traceit("Transaction Error")
            LoadShow = False
        Else
            Traceit("Program # " & ShowID & " Loaded from EEPROM for Unit" & UnitNumber)
            LoadShow = True
        End If

        ' Restore TOT
        CommPort.ReadTOT = tot


    End Function
    Public Function SetDefaultShow(ByVal ShowID As String)

        Dim ProgramUnitMessage As String
        Dim WU As New cWorkUnit
        Dim tot As Integer
        Dim Reply As String

        UnitStat = UnitNumber
        WU.Port = Right("00" & ShowID, 3)
        Units(UnitNumber).Msg.BumpSendNumber()
        Units(UnitNumber).Msg.NewMessage()
        Units(UnitNumber).Msg.Command = "13"
        Units(UnitNumber).Msg.Unit = UnitNumber
        Units(UnitNumber).Msg.AddWU(WU.WorkUnit)
        ProgramUnitMessage = Units(UnitNumber).Msg.CompleteMessage
        Reply = ""
        tot = CommPort.ReadTOT
        CommPort.ReadTOT = 3000
        Reply = CommPort.Transaction(ProgramUnitMessage, UnitNumber)
        If Reply = "" Then
            Traceit("Transaction Error")
            SetDefaultShow = False
        Else
            Traceit("Default Program # " & ShowID & " Set in EEPROM for Unit " & UnitNumber)
            SetDefaultShow = True
        End If

        CommPort.ReadTOT = tot


    End Function
    Public Function Arm(ByVal UnitIndex As Integer) As Boolean
        ' ARM specific unit
        Dim WU = New cWorkUnit
        Dim ArmMsg As String
        Dim Reply As String

        CommPort.ReadTOT = 500

        UnitStat = UnitIndex
        WU.Port = "0000"
        Units(UnitIndex).Msg.BumpSendNumber()
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "10"
        Units(UnitIndex).Msg.Unit = UnitIndex
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        ArmMsg = Units(UnitIndex).Msg.CompleteMessage
        Reply = ""
        Reply = CommPort.Transaction(ArmMsg, UnitIndex)
        If Reply = "" Then
            Traceit("Transaction Error")
            Units(UnitIndex).Armed = False
            Arm = False
        Else
            Traceit("Unit " & UnitIndex & " Armed")
            Units(UnitIndex).Armed = True
            Arm = True
        End If


    End Function
    Public Function UnitReset(ByVal UnitIndex As Integer) As Boolean
        ' Reset Specific unit and reset message numbers.
        Dim WU = New cWorkUnit
        Dim ResetMsg As String

        UnitStat = UnitIndex
        Thread.Sleep(100)
        WU.Port = "999"
        Units(UnitIndex).Msg.BumpSendNumber()
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "99"
        Units(UnitIndex).Msg.Unit = UnitIndex
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        ResetMsg = Units(UnitIndex).Msg.CompleteMessage
        Units(UnitIndex).Msg.NextSendNumber = 1
        Units(UnitIndex).Msg.NextReceiveNumber = 1
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Armed = False
        Units(UnitIndex).Session = False

        CommPort.Broadcast(ResetMsg)

        Traceit("Unit " & UnitIndex & " Reset")
        Return True

    End Function
    Public Function Disarm(ByVal UnitIndex As Integer) As Boolean
        Dim WU = New cWorkUnit
        Dim DisarmMsg As String
        Dim Reply As String

        CommPort.ReadTOT = 500

        UnitStat = UnitIndex
        WU.Port = "000"
        Units(UnitIndex).Msg.BumpSendNumber()
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "11"
        Units(UnitIndex).Msg.Unit = UnitIndex
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        DisarmMsg = Units(UnitIndex).Msg.CompleteMessage
        Reply = ""
        Reply = CommPort.Transaction(DisarmMsg, UnitIndex)
        If Reply = "" Then
            Traceit("Transaction Error")
            Disarm = False
        Else
            Units(UnitIndex).Armed = False
            Traceit("Unit " & UnitIndex & " DisArmed")
            Disarm = True
        End If



    End Function
    Public Function EndProgramming(ByVal Unit As Integer) As Boolean
        Dim WU = New cWorkUnit
        Dim EndProgrammingMsg As String
        Dim Reply As String
        Dim tot As Integer
        UnitStat = Unit
        WU.Port = "000"
        tot = CommPort.ReadTOT
        CommPort.ReadTOT = 3000
        Units(Unit).Msg.BumpSendNumber()
        Units(Unit).Msg.NewMessage()
        Units(Unit).Msg.Command = "17"
        Units(Unit).Msg.Unit = Unit
        Units(Unit).Msg.AddWU(WU.WorkUnit)
        EndProgrammingMsg = Units(Unit).Msg.CompleteMessage
        Reply = ""
        Reply = CommPort.Transaction(EndProgrammingMsg, Unit)
        If Reply = "" Then
            Traceit("Transaction Error")
            Units(Unit).Session = False
            EndProgramming = False
        Else

            Units(Unit).Session = True
            Traceit("End of Programming for Unit " & Unit)
            EndProgramming = True
        End If
        CommPort.ReadTOT = tot
    End Function
    Public Function StartProgramming(ByVal Unit As Integer) As Boolean
        Dim WU = New cWorkUnit
        Dim StartProgrammingMsg As String
        Dim Reply As String
        Dim tot As Integer


        ' Define Show Type
        WU.Port = "002"

        tot = CommPort.ReadTOT
        'CommPort.ReadTOT = 3000
        CommPort.ReadTOT = 500
        Units(Unit).Msg.BumpSendNumber()
        Units(Unit).Msg.NewMessage()
        Units(Unit).Msg.Command = "16"
        Units(Unit).Msg.Unit = Unit
        Units(Unit).Msg.AddWU(WU.WorkUnit)
        StartProgrammingMsg = Units(Unit).Msg.CompleteMessage
        Reply = ""
        Reply = CommPort.Transaction(StartProgrammingMsg, Unit)
        If Reply = "" Then
            Traceit("Transaction Error")
            Units(Unit).Session = False
            StartProgramming = False
        Else
            Units(Unit).Session = True
            Traceit("Show Programming Started for Unit " & Unit)
            StartProgramming = True
        End If
        CommPort.ReadTOT = tot
    End Function
    Public Sub StartSession(ByVal UnitNumber As Integer)
        Dim WU = New cWorkUnit
        Dim StartMsg As String
        Dim Reply As String

        UnitStat = UnitNumber
        WU.Port = "000"
        Units(UnitNumber).Msg.NewMessage()
        Units(UnitNumber).Msg.Command = "01"
        Units(UnitNumber).Msg.Unit = UnitNumber
        Units(UnitNumber).Msg.AddWU(WU.WorkUnit)
        StartMsg = Units(UnitNumber).Msg.CompleteMessage
        Reply = ""

        Try
            CommPort.ReadTOT = 300

            Reply = CommPort.Transaction(StartMsg, UnitNumber)
            If Reply = "" Then
                Units(UnitNumber).Session = False
            Else
                Units(UnitNumber).Session = True
                Traceit("Sesion Started for Unit " & UnitNumber)
            End If
        Catch
            MsgBox("Comm port not active")
        End Try


    End Sub

    Public Sub AssignLogicalAddress(ByVal PhysicalAddress As String, ByVal VirtualAddress As String)
        Dim WU = New cWorkUnit
        Dim AssignMsg As String
        Dim Reply As String
        Dim UnitNumber As Integer



        UnitNumber = Val(PhysicalAddress)
        WU.Port = VirtualAddress
        Units(UnitNumber).Msg.BumpSendNumber()
        Units(UnitNumber).Msg.NewMessage()
        Units(UnitNumber).Msg.Command = "24"
        Units(UnitNumber).Msg.Unit = Val(PhysicalAddress)

        WU.Port = VirtualAddress
        Units(UnitNumber).Msg.AddWU(WU.WorkUnit)

        AssignMsg = Units(UnitNumber).Msg.CompleteMessage
        Reply = ""

        Try
            CommPort.ReadTOT = 300

            Reply = CommPort.Transaction(AssignMsg, UnitNumber)
            If Reply = "" Then
                Traceit("Logical Address assignment failed")
            Else

                Traceit("Logical Address " & VirtualAddress & " assigned to Physical Unit " & PhysicalAddress)
            End If
        Catch
            MsgBox("Comm port not active")
        End Try


    End Sub
    Public Function Fire(ByVal PortNumber As Integer) As Boolean
        Dim WU = New cWorkUnit
        Dim FireMsg As String
        Dim Reply As String

        UnitStat = UnitIndex
        WU.Port = Right("00" & PortNumber, 3)
        AllowBump = True
        Units(UnitIndex).Msg.BumpSendNumber()
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "04"
        Units(UnitIndex).Msg.Unit = UnitIndex
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        FireMsg = Units(UnitIndex).Msg.CompleteMessage
        Reply = CommPort.Transaction(FireMsg, UnitIndex)
        If Reply = "" Then
            Traceit("Transaction Error")
            Units(UnitIndex).Session = False
            Fire = False
        Else
            Units(UnitNumber).Session = True
            Traceit("Unit " & UnitIndex & " Port " & PortNumber & " Fired")
            Fire = True
        End If


    End Function

    Public Function StartTrace() As Boolean
        Dim WU = New cWorkUnit
        Dim StartTraceMsg As String
        Dim Reply As String

        UnitStat = UnitIndex
        Units(UnitIndex).Msg.BumpSendNumber()
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "06"
        Units(UnitIndex).Msg.Unit = UnitNumber
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        StartTraceMsg = Units(UnitIndex).Msg.CompleteMessage
        Reply = CommPort.Transaction(StartTraceMsg, UnitIndex)
        If Reply = "" Then
            Traceit("Transaction Error")
            Units(UnitIndex).Session = False

        Else
            fMain.ListBox1.Items.Add(Reply)
            Units(UnitNumber).Session = True
            Traceit("Unit " & UnitIndex & " Trace Started")
            fMain.ListBox1.Items.Add(" ")

        End If
        Return True
    End Function
    Public Function StopTrace() As Boolean
        Dim WU = New cWorkUnit
        Dim StopTraceMsg As String
        Dim Reply As String

        UnitStat = UnitIndex
        Units(UnitIndex).Msg.BumpSendNumber()
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "19"
        Units(UnitIndex).Msg.Unit = UnitNumber
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        StopTraceMsg = Units(UnitIndex).Msg.CompleteMessage
        Reply = CommPort.Transaction(StopTraceMsg, UnitIndex)
        If Reply = "" Then
            Traceit("Transaction Error")
            Units(UnitIndex).Session = False
        Else
            fMain.ListBox1.Items.Add(Reply)
            Units(UnitNumber).Session = True
            Traceit("Unit " & UnitIndex & " Trace Stopped")
            fMain.ListBox1.Items.Add(" ")
        End If
        Return True
    End Function
    Public Function GlobalArm()
        Dim WU = New cWorkUnit
        Dim GlobalArmMsg As String
        Dim Reply As String

        WU.Port = "999"
        WU.UnitNumber = "999"
        WU.Command = "10"
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "10"
        Units(UnitIndex).Msg.Unit = "999"
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        GlobalArmMsg = Units(UnitIndex).Msg.CompleteMessage
        Reply = ""
        CommPort.Broadcast(GlobalArmMsg)
        fMain.Timer1.Enabled = True
        ShowTime = 0
        Traceit("Timer Started")
        GlobalArm = True

    End Function
    Public Function GlobalDisarm()
        Dim WU = New cWorkUnit
        Dim GlobalDisarmMsg As String
        Dim Reply As String

        WU.Port = "999"
        WU.UnitNumber = "999"
        WU.Command = "11"

        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "11"
        Units(UnitIndex).Msg.Unit = "999"
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        GlobalDisarmMsg = Units(UnitIndex).Msg.CompleteMessage
        Reply = ""
        CommPort.Broadcast(GlobalDisarmMsg)
        fMain.Timer1.Enabled = False
        ShowTime = 0
        Traceit("Timer Stopped")
        GlobalDisarm = True


    End Function
    Public Function GlobalPause()
        Dim WU = New cWorkUnit
        Dim GlobalPauseMsg As String
        Dim Reply As String


        WU.Port = "999"
        WU.UnitNumber = "999"
        WU.Command = "20"

        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "20"
        Units(UnitIndex).Msg.Unit = "999"
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        GlobalPauseMsg = Units(UnitIndex).Msg.CompleteMessage
        Reply = ""
        CommPort.Broadcast(GlobalPauseMsg)
        fMain.Timer1.Enabled = False
        'ShowTime = 0
        Traceit("Timer Stopped")
        GlobalPause = True

    End Function
    Public Function GlobalContinue()
        Dim WU = New cWorkUnit
        Dim GlobalContinueMsg As String
        Dim Reply As String

        WU.Port = "999"
        WU.UnitNumber = "999"
        WU.Command = "21"

        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "21"
        Units(UnitIndex).Msg.Unit = "999"
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        GlobalContinueMsg = Units(UnitIndex).Msg.CompleteMessage
        Reply = ""
        CommPort.Broadcast(GlobalContinueMsg)
        fMain.Timer1.Enabled = True
        Traceit("Timer Started")
        GlobalContinue = True

    End Function
    Public Function GlobalLoadShow()

        Dim WU = New cWorkUnit
        Dim GlobalLoadShowMsg As String
        Dim Reply As String


        WU.Port = fMain.ComboBox2.Text
        WU.UnitNumber = "999"
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "08"
        Units(UnitIndex).Msg.Unit = "999"
        Units(UnitIndex).Msg.AddWU(WU.WorkUnit)
        GlobalLoadShowMsg = Units(UnitIndex).Msg.CompleteMessage
        Reply = ""
        fMain.ListBox1.Items.Add("Global Load show # " & WU.Port)
        CommPort.Broadcast(GlobalLoadShowMsg)

        GlobalLoadShow = True

    End Function
    Public Sub Poll()
        Dim PollMessage As String

        Msg = New cMessage
        WU.Port = "999"
        Units(0).Msg.NextSendNumber = 1
        Units(0).Msg.NextReceiveNumber = 1
        Units(0).Msg.NewMessage()
        Units(0).Msg.Unit = "999"
        Units(0).Msg.Command = "96"
        Units(0).Msg.AddWU(WU.WorkUnit)



        PollMessage = Units(0).Msg.CompleteMessage
        CommPort.Broadcast(PollMessage)



    End Sub
    Public Sub FastPoll(ByVal Data As String)
        Dim PollMessage As String


        Msg = New cMessage
        WU.Port = "999"
        Units(0).Msg.NextSendNumber = 1
        Units(0).Msg.NextReceiveNumber = 1
        Units(0).Msg.NewMessage()
        Units(0).Msg.Unit = "999"
        Units(0).Msg.Command = "95"
        ' Adding Raw Data in place of WU
        Units(0).Msg.AddWU("999" & Data)



        PollMessage = Units(0).Msg.CompleteMessage
        CommPort.Broadcast(PollMessage)



    End Sub
    Public Sub SlowPoll(ByVal Data As String)
        Dim PollMessage As String


        Msg = New cMessage
        WU.Port = "999"
        Units(0).Msg.NextSendNumber = 1
        Units(0).Msg.NextReceiveNumber = 1
        Units(0).Msg.NewMessage()
        Units(0).Msg.Unit = "999"
        Units(0).Msg.Command = "94"
        ' Adding Raw Data in place of WU
        Units(0).Msg.AddWU("999" & Data)



        PollMessage = Units(0).Msg.CompleteMessage
        CommPort.Broadcast(PollMessage)



    End Sub

    Public Sub SendCommTest()
        ' Generate broadcast message to display Unit ID (Red Led) and Software Version (Green LED)
        Dim IDMessage As String

        Msg = New cMessage
        WU.Port = "090"
        Units(0).Msg.NextSendNumber = 1
        Units(0).Msg.NextReceiveNumber = 1
        Units(0).Msg.NewMessage()
        Units(0).Msg.Unit = "999"
        Units(0).Msg.Command = "99"
        Units(0).Msg.AddWU(WU.WorkUnit)
        IDMessage = Units(0).Msg.CompleteMessage
        CommPort.Broadcast(IDMessage)
    End Sub
    Public Sub EchoCommTest()
        ' Generate broadcast message to display Unit ID (Red Led) and Software Version (Green LED)
        Dim Length As Integer
        Dim Reply As String
        Dim Comm As New cComm

        Msg = New cMessage
        Length = Comm.ReadVarString()
        Reply = CommPort.Data
        Comm.WriteVarString(Reply)
    End Sub

    Public Sub done()
        Me.Finalize()
    End Sub

    Public Sub New()

    End Sub

    Public Function ProgramUnit(ByVal ShowID As String, ByVal UnitIndex As Integer)
        ' Save the show in EEPROM
        Dim ProgramUnitMessage As String
        Dim WU As New cWorkUnit
        Dim tot As Integer
        Dim Reply As String
        UnitStat = UnitIndex

        WU.Port = Right("00" & ShowID, 3)
        Units(UnitIndex).Msg.BumpSendNumber()
        Units(UnitIndex).Msg.NewMessage()
        Units(UnitIndex).Msg.Command = "07"
        Units(UnitIndex).Msg.Unit = UnitNumber
        Units(UnitNumber).Msg.AddWU(WU.WorkUnit)
        ProgramUnitMessage = Units(UnitNumber).Msg.CompleteMessage
        Reply = ""
        tot = CommPort.ReadTOT
        ' Wait for PIC to write show to EEPROM
        '    Under normal conditions, it should take approx 3 seconds without
        '    network delays
        CommPort.ReadTOT = 6000
        Reply = CommPort.Transaction(ProgramUnitMessage, UnitNumber)
        If Reply = "" Then
            Traceit("Transaction Error")
            ProgramUnit = False
        Else
            Traceit("Unit Programmed in slot # " & ShowID & " For Unit " & UnitNumber)
            ProgramUnit = True
        End If

        ' restore TOT
        CommPort.ReadTOT = tot

    End Function
End Class
