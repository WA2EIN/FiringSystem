' (C) Copyright P. Cranwell, 2014
Public Class fMain

    Private Sub fMain_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
        Dim CMD As New cCommand
        CMD.MasterReset()
    End Sub

    Private Sub fMain_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    '    Message Structure

    'All messages have one of the following message formats:
    '(Except for the start of message and length code, all data is 7 bit ASCII Numeric to allow the use of
    'Check code logic.)

    'Message Header Format
    '    BYTE	1     	X’FF’      Start of Message	
    '    BYTE	2      	Message length (Binary) 1 - 255 
    '    BYTE	3      	MN = Message Number Modulo 10
    '    BYTES 4-5  	Number of Work Units
    '    BYTES 6-8    	Physical Unit Address 
    '    BYTES 9-10    	Command
    ' 	Unit specific commands:
    '                      	01 = Start Session
    '                      	02 = Stop Session
    '                      	04 = Fire 
    ' 		05 = Status (return status of e-matches, unit voltage, other device status information)
    '                       	06 = Activate Trace on for Unit
    '                       	07 = Save Show in EPROM
    '                       	08 = Load Show from EPROM
    '                       	10 = Arm Unit
    '                       	11 = Disarm Unit
    '                       	12 = Measure HV
    '                       	13 = Set Default Show
    '                       	14 = Save Show in EEPROM
    '                       	16 = Start of Programming for Unit
    '                       	17 = End of Programming for Unit'
    '                       	19 = Stop Unit Trace
    '                       	24 = Assign Logical Address to Unit 
    '                       	99 = Reset Unit
    '
    '
    'Broadcast message (All units) commands
    '
    '                       	08 = Load Show from EEPROM      
    '                       	10 = Global Arm
    '                   		11 = Global Disarm
    '                         	20 = Pause Zone                       
    '                         	21 = Continue Zone                
    '                         	33 = Set Unit Address into EEPROM 
    '                          	44 = Trigger Cue        
    '                           95 = Poll Fast            
    '                          	96 = Poll             
    '                          	97 = Trace                         
    '                          	98 = Current Time                
    '                          	99 = Reset Units     
    '                       	99 (Port 997) = Show Software ID and Physical Network Address
    '                       	99 (Port 090) = Send COMM Test
    '
    '
    '
    '        (1 – 14 work units) in the following Format 
    '
    '     	WU BYTES 1-2     	Port Number
    '            				00 = Base Unit
    '                   				01-nn are Port numbers      
    '
    '         	WU BYTES 3-10 		Time Code in MMMMMMMM Milliseconds  (Max 27 hours)
    '
    '        	Last Byte     		Check Digit
    '
    '
    '
    '  ACK Message Format  
    '
    '    	BYTE	1     	X’FF’      Start of Message	
    '    	BYTE	2      	Message length (Binary) 1 - 255 
    '    	BYTE	3      	MN = Message Number Modulo 10
    '   (Same as received message number )
    '       BYTES	4-5	Number of WU = 01
    ' 	    BYTES	6-8	Host Address = "001"
    '    	BYTES	9-10	ACK Command Code = “07"
    '       BYTES   11-29     Optional Status Informatio'n
    '       BYTE	11|30	Message Check Code
    '   NAK Message Format  
    '
    ' 	BYTE	1     	X’FF’      Start of Message	
    '   BYTE	2      	Message length (Binary) 1 - 255 
    '   BYTE	3      	MN = Message Number Modulo 10
    ' 			(Same as received message number )
    '   BYTES	4-5	Number of WU = 01
    ' 	BYTES	6-8	Host Address = "001"
    '   BYTES	9-10	NAK Command Code = “08”
    ' 	BYTE	11	Message Check Code
    '
    '
    'Optional Status Information 
    '
    '	Bytes 1-3	Number of Ports on Controller (nnn)
    '   Bytes 4-nnn	Port Status	1=No eMatch conductivity, 0 = eMatch Conductivity'
    '	Byte nnn+4	Armed Status	1 = Unit Armed, 0 = Unit disarmed'
    '	Byte nnn+5	Show ID	0 = No Show Loaded, 1-2 – ID of Loaded Show.
    '	Bytes nnn+6	Voltage		0126 = 12.6 Volts
    '	Bytes nnn+10	Phy Addr	Physical Unit Address
    '	Bytes nnn+13	Log Addr	Logical Address
    '
    '
    Private Sub fMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Initialization class and Init procedure initializes program
        Dim Init = New cInitialization
        DEBUG = True
        Init.Init()

    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As System.Object, e As System.EventArgs) Handles ChangeTraceDirectory.Click
        ' Create directory for Trace and Test Case data
        ' Let user select approprate directory or create directory for Trace and Test Case data
        Try
            fCreateMyDirectory.Show()
        Catch
            ListBox1.Items.Add("ERROR Creating Directory.")
        End Try

        fCreateMyDirectory.Dispose()

    End Sub
    Private Sub ToolStripMenuItem3_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem3.Click
        Try
            fLatencyX.Show()
        Catch
            ListBox1.Items.Add("ERROR setting network latency.")

        End Try
        'fLatency.Dispose()

    End Sub
    Private Sub ToolStripMenuItem4_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem4.Click

        Try
            fNetworkTopology.Show()
        Catch ex As Exception
            ListBox1.Items.Add("ERROR setting network topology.")

        End Try
        'fNetworkTopology.Dispose()

    End Sub
    Private Sub ToolStripMenuItem5_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem5.Click
        Try
            fSetup.Show()
        Catch
            ListBox1.Items.Add("ERROR setting up network. Check Network connections.")
        End Try

    End Sub
    Private Sub ToolStripMenuItem6_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripMenuItem6.Click
        'Try
        fSetFieldModuleAddress.Show()
        'Catch
        'ListBox1.Items.Add("ERROR Setting Field Module Address")
        'End Try
        '/ fSetFieldModuleAddress.Dispose()
    End Sub
    Private Sub ManualFireToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ManualFireToolStripMenuItem.Click
        fManualFire.Show()
    End Sub
    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Dim Msg As String
        Dim work As String
        Dim l As Integer
        Dim Msg1 As New cMessage
        Dim WU As New cWorkUnit
        Dim CCode As String


        ' Generate Time Message at 10 second intervals when system is armed.

        Msg = TimeBase
        ShowTime = ShowTime + Timer1.Interval

        work = "00000" & ShowTime
        l = Len(work)
        work = Mid$(work, l - 7, 8)
        Msg = Msg & work
        CCode = Msg1.GenerateCheckCode(Msg)
        Msg = Msg & CCode

        ' Broadcast time message
        CommPort.Broadcast(Msg)
        TOD = ShowTime

    End Sub


    Private Sub ListBox1_MouseDoubleClick(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles ListBox1.MouseDoubleClick
        ListBox1.Items.Clear()
    End Sub
    Private Sub TestGPSCalculationsToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TestGPSCalculationsToolStripMenuItem.Click
        fGPSTest.Show()
    End Sub
    Private Sub LoadCSVToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs)
        fCSVImport.Show()
    End Sub
    Private Sub LoadShowToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LoadShowToolStripMenuItem.Click
        fSelectShow.Show()
    End Sub
    Private Sub TestCaseToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TestCaseToolStripMenuItem.Click
        fLoadTestcase.Show()
    End Sub
    Private Sub LoadCSVToolStripMenuItem1_Click(sender As System.Object, e As System.EventArgs) Handles LoadCSVToolStripMenuItem1.Click
        fCSVImport.Show()
    End Sub

    Private Sub SetupComPortToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles SetupComPortToolStripMenuItem.Click
        fSerialPorts.Show()
    End Sub


    Private Sub CleanRegistryToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CleanRegistryToolStripMenuItem.Click
        CleanRegistry()
    End Sub

    Private Sub MenuStrip1_Disposed(sender As Object, e As System.EventArgs) Handles MenuStrip1.Disposed
        Dim CMD As New cCommand

        CMD.MasterReset()
        Timer1.Stop()
        Timer1.Enabled = False

    End Sub

    Private Sub ListBox1_MouseWheel(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles ListBox1.MouseWheel
        fDecode.ListBox1.Items.Clear()

        fDecode.Show()
        fDecode.ListBox1.Items.Add(ListBox1.SelectedItem.ToString)
    End Sub

    Private Sub QuieryBuildDateToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles QuieryBuildDateToolStripMenuItem.Click
        fQueryBuildDate.show()
    End Sub

    Private Sub ShowToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ShowToolStripMenuItem.Click

    End Sub

   

    Private Sub ComboBox3_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBox3.SelectedIndexChanged

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ListBox1.Items.Clear()
    End Sub

    
    Private Sub BuildToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BuildToolStripMenuItem.Click
        Input.Show()
    End Sub
End Class
