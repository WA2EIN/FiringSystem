' (C) Copyright P. Cranwell, 2014
Public Class fLatencyX
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        SaveSetting("PYRO2014", "New", "NetworkLatency", TextBox1.Text)
        NetworkLatency = Val(TextBox1.Text)
        Me.Hide()
    End Sub


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub fLatency_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Beep()
    End Sub
End Class