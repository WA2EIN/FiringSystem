' (C) Copyright P. Cranwell, 2014
Public Class fUnitStatus
    Public GoodTextBoxArray(HighUnit - LowUnit + LowUnit + 1) As TextBox
    Public TOTTextBoxArray(HighUnit - LowUnit + LowUnit + 1) As TextBox
    Public BadTextBoxArray(HighUnit - LowUnit + LowUnit + 1) As TextBox
    Private Sub UnitStatusForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim LabelArray(HighUnit - LowUnit + LowUnit + 1) As Label
        Dim Heading As New TextBox
        Dim i As Integer


        Heading.Location = New Point(65, 50)
        Heading.Size = New System.Drawing.Size(412, 50)
        Heading.Text = "Unit #             Good Msg                           Tot                                Bad"
        Me.Controls.Add(Heading)

        For i = LowUnit To HighUnit
            LabelArray(i) = New Label
            LabelArray(i).Location = New Point(5, i * 25)
            LabelArray(i).Text = "Unit " & i
            LabelArray(i).TextAlign = ContentAlignment.MiddleRight
            Me.Controls.Add(LabelArray(i))



            GoodTextBoxArray(i) = New TextBox()
            GoodTextBoxArray(i).Location = New Point(125, i * 25)
            GoodTextBoxArray(i).Name = "Unit" & i & "Good"
            GoodTextBoxArray(i).Text = 0
            Me.Controls.Add(GoodTextBoxArray(i))

            TOTTextBoxArray(i) = New TextBox()
            TOTTextBoxArray(i).Location = New Point(250, i * 25)
            TOTTextBoxArray(i).Name = "Unit" & i & "TOT"
            TOTTextBoxArray(i).Text = 0
            Me.Controls.Add(TOTTextBoxArray(i))

            BadTextBoxArray(i) = New TextBox()
            BadTextBoxArray(i).Location = New Point(375, i * 25)
            BadTextBoxArray(i).Name = "Unit" & i & "Bad"
            BadTextBoxArray(i).Text = 0
            Me.Controls.Add(BadTextBoxArray(i))


        Next


    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        For i = LowUnit To HighUnit
            GoodTextBoxArray(i).Text = 0
            TOTTextBoxArray(i).Text = 0
            BadTextBoxArray(i).Text = 0
        Next
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TestRunning = False
        Application.DoEvents()
    End Sub

    Private Sub fUnitStatus_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class