Imports System.Math

Public Class fClock
    Dim TODWork As Integer
    Dim TODText As String


    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        TOD = TOD + 100

        TODWork = TOD / 1000
        TODText = Val(TODWork)
       
        TextBox2.Text = TODText
    End Sub

    Private Sub fClock_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Timer1.Start()
        Timer1.Enabled = True
        Me.TopMost = True

    End Sub
End Class