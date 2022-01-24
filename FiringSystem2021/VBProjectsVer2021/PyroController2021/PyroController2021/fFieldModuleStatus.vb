' (C) Copyright P. Cranwell, 2014
Public Class fFieldModuleStatus

    Private Sub FieldoduleStatus_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Dim Index As Integer
    Dim NumPins As Integer

    Dim PinButtons(342) As PinButton
    Private Sub FieldModuleStatus_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Dim i As Integer
        Dim Row As Integer
        Dim Col As Integer
        Dim RowPos As Integer
        Dim ColPos As Integer


        NumPins = Len(Units(ModuleIndex).PinStatus)
        'NumPins = 432   (For testing program logic)

        For i = 0 To NumPins - 1
            With PinButtons(i)
                Row = Math.Floor(i / 17) + 1
                RowPos = (Row * 34) '+ 34
                Col = (i Mod 17)
                ColPos = Col * 45
                .But = New Button
                .But.Font = New System.Drawing.Font("arial", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                .But.Text = i + 1
                .But.AutoSize = False
                .But.MaximumSize = New System.Drawing.Size(40, 24)
                .ButLoc.X = ColPos
                .ButLoc.Y = RowPos
                .But.Location = .ButLoc
                If Units(ModuleIndex).PinStatus(i) = "1" Then
                    .But.BackColor = Color.LightPink
                Else

                    .But.BackColor = Color.LightGreen
                End If



                .But.Visible = True
                Controls.Add(.But)
            End With
            Application.DoEvents()


        Next
    End Sub
End Class