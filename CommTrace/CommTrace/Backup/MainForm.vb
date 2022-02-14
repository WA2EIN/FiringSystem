Public Class MainForm
    Dim Comm As New CommClass
    Dim running As Boolean

    Private Sub MainForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub


    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'run()
        ListBox1.Items.Add("Starting V3")
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
            If Len > 0 Then
                'SW.Done()
                'SW.ReportToConsole()
                'ListBox1.Items.Add(Clock)
                ListBox1.Items.Add("<< " & Comm.Data & "          " & Clock)
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
End Class
