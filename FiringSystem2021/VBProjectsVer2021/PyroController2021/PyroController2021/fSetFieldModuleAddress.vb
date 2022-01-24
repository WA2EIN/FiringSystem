' (C) Copyright P. Cranwell, 2014
Public Class fSetFieldModuleAddress
    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim Cmd = New cCommand
        Cmd.SetUnitAddress(TextBox1.Text)
        Me.Hide()
    End Sub
    Private Sub fSetFieldModuleAddress_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Beep()
    End Sub
End Class