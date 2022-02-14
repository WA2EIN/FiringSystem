Public Class MainForm
    Dim Comm As New CommClass
    Dim running As Boolean

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub


    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'run()
        ListBox1.Items.Add("Starting V3")
        Me.Text = "Set Comm Port"
        GetSerialPortNames()
        'runit()

    End Sub
    Sub runit()
        Dim Len As Integer
        Dim SW As New StopWatch
        Application.DoEvents()

        Comm.ReadTOT = 100
        Comm.PurgePort()
        SW.Start()
        While Running = True

            Len = Comm.ReadVarString()
            Application.DoEvents()
            If Len > 0 Then
                'SW.Done()
                'SW.ReportToConsole()
                'ListBox1.Items.Add(Clock)
                'ListBox1.Items.Add("<< " & Comm.Data & "          " & Clock)
                Application.DoEvents()
                'SW.Start()
            End If

        End While
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        running = True
        runit()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        running = False
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ListBox1.Items.Clear()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged

    End Sub
    Public Function GetComPort() As String
        GetComPort = GetSetting("PYROTRACE", "New", "Port", vbNullString)
    End Function
    Public Sub SetComPort(ByVal NewPort As String)
        SaveSetting("PYROTRACE", "New", "Port", NewPort)
    End Sub
    Sub GetSerialPortNames()
        ' Show all available COM ports.
        For Each sp As String In My.Computer.Ports.SerialPortNames
            ComboBox1.Items.Add(sp)
        Next
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        SetComPort(ComboBox1.Text)
        InitComm()
    End Sub


    Private Sub InitComm()

        Comm.Cleanup()
        Comm = New CommClass
        Comm.PortName = ComboBox1.Text
        Comm.PurgePort()
        Me.Text = "Using " & ComboBox1.Text
        SetComPort(ComboBox1.Text)
        
    End Sub
End Class
