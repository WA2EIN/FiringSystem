' (C) Copyright P. Cranwell, 2014
Public Class fSelectShow



    Private Sub SelectShow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'ShowDatabaseDataSet1.Show' table. You can move, or remove it, as needed.
        Me.ShowTableAdapter.Fill(Me.ShowDatabaseDataSet.Show)
        Try
            Me.ShowTableAdapter.Fill(Me.ShowDatabaseDataSet.Show)
        Catch
            MsgBox("Show Database needs to be setup")
            End
        End Try


    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        fShow.TextBox1.Text = DataGridView1.CurrentRow.Cells(0).Value
        fShow.Show()
        Me.Hide()
    End Sub

    Private Sub bUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bUpdate.Click, bUpdate.Click
        Me.ShowTableAdapter.Update(Me.ShowDatabaseDataSet.Show)
    End Sub

    Private Sub DataGridView1_CellContentClick_1(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs)

    End Sub

    Private Sub DataGridView1_CellContentClick_2(sender As System.Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Me.Hide()

        fShow.TextBox1.Text = DataGridView1.CurrentRow.Cells(0).Value
        fShow.Show()

    End Sub
End Class



