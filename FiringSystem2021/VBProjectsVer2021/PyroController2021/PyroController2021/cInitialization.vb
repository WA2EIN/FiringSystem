' (C) Copyright P. Cranwell, 2014
Imports System
Imports System.IO.Ports
Imports System.Threading
Imports System.Diagnostics.Stopwatch

Public Class cInitialization

    Public Sub Init()

        ' Create data structures for all possible Units and all possible Ports
        '    This code is located in Module and can be relocated to this Class

        InitUnits()
        If TopologyInitialized = False Then Exit Sub

        SIMULATE = False

        Direct = False
        ShowStarted = False
        TimerStarted = False

        ' Build Time Message
        Dim WU = New cWorkUnit
        WU.Command = "98"    ' Time Message
        WU.UnitNumber = "99"       ' All Units
        WU.Port = "00"

        ' Use Unit 2 for Work message
        Units(2).Msg.NewMessage()
        Units(2).Msg.AddWU(WU.WorkUnit)
        TimeMessage = Units(2).Msg.CompleteMessage

        Echo = False

        DirPath = GetDirPath()
        If DirPath = "" Then
            MsgBox("Create Directory for Trace")
            CreateMyDirectory()
        End If


        ' Get data from Registry
        GetSerialPortNames()

        Try
            NetworkLatency = GetNetworkLatency()
        Catch
            NetworkLatency = 0
            MsgBox("Network Latency needs to be defined")
        End Try


        fLatencyX.TextBox1.Text = NetworkLatency

        If fSerialPorts.ComboBox1.Items.Count > 0 Then

        Else
            MsgBox("No COM Port found")
        End If


        Port = GetComPort()

        If Port = "" Then
            ' Setup Com Port
            MsgBox("COM Port needs to be setup.")
            Exit Sub
        Else
            ' Port has serial port name
            CommPort = New cComm

        End If


        ' Enable Tracing and Test Case Creation by default
        TRACEACTIVE = True


    End Sub

    Sub GetSerialPortNames()
        ' Show all available COM ports.
        For Each sp As String In My.Computer.Ports.SerialPortNames
            fSerialPorts.ComboBox1.Items.Add(sp)
        Next
    End Sub

    Private Sub CreateMyDirectory()
        fCreateMyDirectory.FolderBrowserDialog1.ShowDialog()
        DirPath = fCreateMyDirectory.FolderBrowserDialog1.SelectedPath & "\"
        SaveSetting("PYRO2014", "New", "DirPath", DirPath)
        CreateDirectory()
    End Sub
End Class


