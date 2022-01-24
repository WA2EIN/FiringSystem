Public Class fQueryBuildDate

    Private Sub TextBox1_TextChanged(sender As System.Object, e As System.EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        Dim CMD As New cCommand
        CommPort.NumRetries = 1
        CMD.QueryBuildDate(TextBox1.Text)

    End Sub
End Class