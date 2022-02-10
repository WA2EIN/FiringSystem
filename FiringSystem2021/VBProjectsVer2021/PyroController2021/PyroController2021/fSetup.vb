' (C) Copyright P. Cranwell, 2014
Public Class fSetup
    Private Running = True
    Private AnyReply As Boolean = False
    Private Sub Setup_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus
        'Cmd = New CommandClass
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Running = True
        'Dim Data As String
        ' Dim Addr As Integer
        'Dim len As Integer
        'Dim ModuleAddress As Integer
        'Dim X As Integer
        Dim CMD As New cCommand
        'Dim ButLoc As Point
        'Dim NetworkTopology As String
        Dim Proc As New Process


        ' Start Polling Network

        Button1.Visible = False
        Button2.Visible = True

        CommPort.ReadTOT = 500

        While Running
            RichTextBox2.Text = "Resetting Network"
            CMD.MasterReset()
            xLongWait(500)

            RichTextBox2.Text = "Show Addresses on Modules"
            Application.DoEvents()
            Traceit("Show Unit Addresses")
            CMD.ShowIDAndSoftwareVersion()
            Application.DoEvents()


            'Delay 20 Sec.  Worst case based on 999 address
            RichTextBox2.Text = "Delay 20 Sec"
            xLongWait(20000)

            ' turn off all field modules
            For i = 1 To 999
                Units(i).Active = False
            Next

        End While

        MyBase.Dispose()
        fManualFire.Show()



    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Running = False
        Button2.Visible = False
        Button1.Visible = True
        Application.DoEvents()

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
    Sub CalculateFieldModuleSlowPollDelay()
        Dim i As Integer
        Dim j As Integer
        Dim len As Integer
        Dim delay As String
        Dim TotalDelay As Integer

        ' Calculate Slow Poll Delay for all active units + sum of delays of all preceeding units


        TotalDelay = 11

        For i = 1 To MAXNODES
            Units(i).SlowPollReplyDelay = "00025"
        Next

        For i = 1 To MAXNODES
            If Units(i).Active = True Then
                TotalDelay = TotalDelay + Units(i).NumberOfPorts * 0.27 + 60  ' Delay of header rounded up by 1
                For j = i + 1 To 999
                    If Units(j).Active = True Then
                        delay = TotalDelay
                        delay = "00000" & delay
                        len = delay.Length
                        delay = delay.Substring(len - 5, 5)
                        Units(j).SlowPollReplyDelay = delay   ' Delay is in MS, a max of 999 seconds
                        Traceit("Unit " & j & " = " & delay & " MS")
                    End If
                Next
            End If
        Next
    End Sub
    Sub SetSlowPollDelay()
        Dim DelayData As String
        Dim i As Integer
        Dim len As Integer
        Dim cmd As New cCommand
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
                DelayData = DelayData & unit & Units(i).SlowPollReplyDelay

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
    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim name As System.Object

        ' Get Button Name   (Cue n)
        name = sender
        'Extract Module index from name
        ModuleIndex = Val(name.text)
        fFieldModuleStatus.Show()
    End Sub
    Private Sub SetupFom_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub RichTextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles RichTextBox1.TextChanged

    End Sub
End Class