Public Class PollNetworkFormOld


    'Poll Fast

    'A Fast Poll is designed to return all pertinent Field Module information except the port status.
    'The Fast Poll is used during network setup, to determine which units are active and during show execution,
    ' to determine which units may have been reset and are un-armed.

    'The Fast Poll contains network topology information is place of the usual WU information.
    'The Network Topology information is a string containing a 3 byte numeric field for each bank of 100 field units.  

    'Bytes 1-3 = Number of units in Bank 0, Physical addresses 3-99
    'Bytes 4-6 = Number of units in Bank 1, Physical Addresses 100-199
    'Bytes 7-9 = Number of units in Bank 2, Physical Addresses 200-299
    '.'
    '.
    '.
    'Bytes 25-27 = Number of units in Bank 9, Physical Addresses 900-998


    'The Field modules, upon receiving a Fast Poll message, compute the amount of delay necessary before responding.
    '  They use 10 Milliseconds per unit and calculate the delay for all units with addresses preceding their own.
    '  This allows an orderly set of replies.   This is based on a serial port speed of 38400 BPS.

    'Since no port status information is returned, each Field Module requires only about 5.7 Milliseconds for its reply.
    '  10 Milliseconds is allocated for each unit to allow a small space between response messages.
    ' The field modules count the number of possible lower numbered units, based on Network Topology information, 
    '  and multiply this count by 10MS.

    'Because the Field Modules do not wait for units that are not on the network, as defined by the Network Topology information, 
    ' the Poll operation is completed in the shortest possible time.

    'Each response is composed of 25 characters.  This requires 5.7 Milliseconds per active field module.
    'A network, consisting of 50 field modules, can thus be Fast Polled in ½ second.
    'The returned responses contain the number of ports in the module, the physical unit address,
    ' the logical unit address, the armed status, the loaded show status and the measured voltage.
    '
    '   Set Slow Poll Field Module Delays
    '
    'The number of ports on the Field Module is returned and is used by the PC to Broadcast to the network in 1
    '  or a series of broadcast transmissions. The information in these transmissions consists of 1 or more information pairs
    '  that contain the module address and number of ports.  Information of a maximum of 24 modules is contained in each broadcast.
    'The information is in the format AAADDDDD where AAA is the physical address and DDDDD is the module reply delay in MS.
    '  If the network contains more than 24 modules, multiple broadcast messages are used.  

    'This information is used by the field modules when responding to Slow Poll operations where the port status information is
    ' included in the poll response.  The field modules use the broadcast delay before responding. 

    'The delay is computed as 7 MS per module plus .26MS per port.  A module with 16 ports is allocated 12 MS,
    ' a module with 144 ports is allocated 45MS and a module with 430 ports is allocated 119 MS.
    '  This minimizes the Slow Poll response time.
    '
    'As an example of the time required to poll 50 modules (25 16 port modules and 25 144 port modules), is calculated as :

    'Fast Poll = 50 * .01 sec = .5 Sec
    'Slow Poll =  25 * 12MS = .3 Sec + 25 * 45MS = 1.1 sec  (Total = 1.4 sec)

    'The total time to poll the above network is approx 2 sec.
    '
    '
    '    Slow Poll
    '
    '   A Slow Poll is broadcast to the network.. The Slow Poll message contains Network Topology information but this information
    '   is not required by the Field Modules. It is included for testing purposes.
    '
    '
    'The Slow Poll Reply onttains the number of Module Ports, the port and ematch status, 



    Private Running = True
    Private AnyReply As Boolean = False
    Private Sub Setup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        InitializeComponent()
    End Sub
    Private Sub PollNetworkFormOld_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Running = True
        Dim Data As String
        Dim len As Integer
        Dim ModuleAddress As Integer = 0
        Dim PreviousActiveModuleAddress As Integer
        Dim CMD As New CommandClass
        Dim NetworkTopology As String
        Dim Proc As New Process
        Dim addr As String



        ''''''''''''''''''''' Fast Poll the Network.  '''''''''''''''''''''''''''''

        CommPort.ReadTOT = 500
        Traceit("Resetting Network")
        CMD.MasterReset()
        LongWait(500)

        ' turn off all field modules
        For i = 1 To MAXNODES
            Units(i).Active = False
        Next

        Traceit("Getting Topology")
        NetworkTopology = GetNetworkTopology()

        Traceit("Fast Poll Field Modules")

        CMD.FastPoll(NetworkTopology)
        AnyReply = False
        len = 1
        NumActiveUnits = 0

        ' Read all Fast Poll responses until a timeout occures

        While len > 0
            ' Read until timeout.
            ' Fast Poll results in multiple units responding to a single poll operation.
            ' Each reply contains the Unit Physical and Logical address and the number of ports in the module

            len = CommPort.ReadVarString()
            If len > 0 Then
                NumActiveUnits = NumActiveUnits + 1
                Data = CommPort.Data
                'Traceit(Data)

                If Data.Length > 0 Then
                    AnyReply = True
                    ' For this Unit, extract Number of Ports, Armed Status, Voltage and ShowID and save in Units Array
                    ModuleAddress = ParseFastPollReply(Data)

                    With Units(ModuleAddress)
                        Units(Val(ModuleAddress)).Active = True
                        Units(Val(ModuleAddress)).MayBeActive = True
                        If PreviousActiveModuleAddress = 0 Then
                            ' Set delay for first Active Unit
                            Units(ModuleAddress).SlowPollReplyDelay = 11
                        Else
                            ' Delay based on a serial port speed of 38400 BPS
                            Units(ModuleAddress).SlowPollReplyDelay = Units(PreviousActiveModuleAddress).SlowPollReplyDelay + _
                            Int(Units(PreviousActiveModuleAddress).NumberOfPorts * 0.27 + 60)
                        End If
                        PreviousActiveModuleAddress = ModuleAddress
                    End With
                Else
                End If
            End If
        End While
        Application.DoEvents()


        If AnyReply = False Then
            MsgBox("No Response from Network")
            Me.Dispose()
            Exit Sub
        End If


        ''''''''''''''' Start Slow Poll ''''''''''''''''''''''''

        Traceit("Broadcast Slow Poll Delay")
        BroadcastSlowPollDelay()  ' Broadcast Module Polling Delay

        ' add slow poll here
        NetworkTopology = GetNetworkTopology()
        Traceit("Slow Poll Field Modules")


        CMD.SlowPoll(NetworkTopology)   'NOTE:  Topology information is added for debugging purposes.

        ' Get Slow Poll Results and extract Pin Status information
        ' The Pin Status information is put in the Units array.

        CommPort.ReadTOT = 300
        For i = 1 To NumActiveUnits
            Application.DoEvents()
            len = CommPort.ReadVarString
            If len > 7 Then
                Data = CommPort.Data
                addr = Data.Substring(len - 7, 3)
                Units(addr).PinStatus = Data.Substring(10, Units(addr).NumberOfPorts)
            End If
        Next
    End Sub

    Private Function ParseFastPollReply(ByVal Data As String) As Integer
        Dim Len As Integer
        Dim ModuleAddress As String
        Dim LogicalAddress As String
        Dim Armed As Boolean
        Dim Voltage As String
        Dim Show As String
        Dim Ports As String

        Len = Data.Length
        ModuleAddress = Data.Substring(Len - 7, 3)
        LogicalAddress = Data.Substring(Len - 4, 3)
        Voltage = Data.Substring(Len - 11, 4)
        Show = Data.Substring(Len - 12, 1)
        Armed = Data.Substring(Len - 13, 1)
        Ports = Data.Substring(7, 3)
        Units(ModuleAddress).Armed = Armed
        Units(ModuleAddress).Voltage = Voltage
        Units(ModuleAddress).Show = Show
        Units(ModuleAddress).NumberOfPorts = Ports
        ParseFastPollReply = Val(ModuleAddress)
    End Function
    Sub BroadcastSlowPollDelay()
        Dim DelayData As String
        Dim i As Integer
        Dim len As Integer
        Dim cmd As New CommandClass
        Dim unit As String
        Dim work As String
        Dim count As Integer


        DelayData = ""
        count = 0

        'Broadcast Slow Poll Delay to all online units
        'Logic to do this in groups of 24 Field modules
        '  The Maximum broadcast size can contain 24 Field Modules

        For i = 1 To MAXNODES
            If Units(i).Active Then
                count = count + 1
                unit = i
                unit = "000" & unit
                len = unit.Length
                unit = unit.Substring(len - 3, 3)
                work = "00000" & Units(i).SlowPollReplyDelay
                len = work.Length
                work = work.Substring(len - 5, 5)

                DelayData = DelayData & unit & work

                If count = 24 Then
                    work = "0" & count
                    len = work.Length
                    work = work.Substring(len - 2, 2)
                    DelayData = work & DelayData
                    cmd.SetSlowPollDelay(DelayData)
                End If

            End If
        Next
        If count < 24 Then
            work = "0" & count
            len = work.Length
            work = work.Substring(len - 2, 2)
            DelayData = work & DelayData
            cmd.SetSlowPollDelay(DelayData)
        End If

    End Sub
End Class