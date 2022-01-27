' (C) Copyright P. Cranwell, 2014
Public Class fSerialPorts
    Private Sub bSetPort_Click(sender As System.Object, e As System.EventArgs) Handles bSetPort.Click
        ComboBox1.Text = ComboBox1.SelectedItem.ToString
        SetComPort(ComboBox1.Text)
        Me.Hide()
    End Sub

    Private Sub fSerialPorts_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ComboBox1.Text = GetComPort()
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class