Public Class fVoltage
    Dim Voltage As String
    Dim NET As New cPollNetwork


    Private Sub Form2_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        NET.PollNetwork()
        For i = 1 To MAXNODES
            If Units(i).Active Then
                Voltage = Units(i).Voltage
                Voltage = Mid$(Voltage, 2)
                ListBox1.Items.Add("Module " & i & "     " & Mid$(Voltage, 1, 2) & "." & Mid$(Voltage, 3, 1) & " Volts")
            End If
        Next
    End Sub

    Private Sub fVoltage_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        fManualFire.Close()
    End Sub
End Class