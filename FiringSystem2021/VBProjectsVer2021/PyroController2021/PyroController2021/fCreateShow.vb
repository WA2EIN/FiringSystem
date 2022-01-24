Public Class fCreateShow

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

    End Sub

    Private Sub fNewShow_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'ShowDatabaseDataSet1.Show' table. You can move, or remove it, as needed.
        Me.ShowTableAdapter.Fill(Me.ShowDatabaseDataSet1.Show)

    End Sub
End Class