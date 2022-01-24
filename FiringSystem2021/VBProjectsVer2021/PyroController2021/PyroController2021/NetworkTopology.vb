Public Class NetworkTopology

    Private Sub NetworkTopologyForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Data As String

        Data = GetNetworkTopology()

        Bank0Max.Items.Add(0)
        For i = 3 To 99
            Bank0Max.Items.Add(i)
        Next
        Bank0Max.Text = Data.Substring(0, 3)

        Bank1Max.Items.Add(0)
        For i = 0 To 99
            Bank1Max.Items.Add(i + 100)
        Next
        Bank1Max.Text = Data.Substring(3, 3)

        Bank2Max.Items.Add(0)
        For i = 0 To 99
            Bank2Max.Items.Add(i + 200)
        Next
        Bank2Max.Text = Data.Substring(6, 3)


        Bank3Max.Items.Add(0)

        For i = 0 To 99
            Bank3Max.Items.Add(i + 300)
        Next
        Bank3Max.Text = Data.Substring(9, 3)


        Bank4Max.Items.Add(0)

        For i = 0 To 99
            Bank4Max.Items.Add(i + 400)
        Next
        Bank4Max.Text = Data.Substring(12, 3)

        Bank5Max.Items.Add(0)

        For i = 0 To 99
            Bank5Max.Items.Add(i + 500)
        Next
        Bank5Max.Text = Data.Substring(15, 3)

        Bank6Max.Items.Add(0)

        For i = 0 To 99
            Bank6Max.Items.Add(i + 600)
        Next
        Bank6Max.Text = Data.Substring(18, 3)

        Bank7Max.Items.Add(0)

        For i = 0 To 99
            Bank7Max.Items.Add(i + 700)
        Next

        Bank7Max.Text = Data.Substring(21, 3)

        Bank8Max.Items.Add(0)

        For i = 0 To 99
            Bank8Max.Items.Add(i + 800)
        Next
        Bank8Max.Text = Data.Substring(24, 3)

        Bank9Max.Items.Add(0)

        For i = 0 To 99
            Bank9Max.Items.Add(i + 900)
        Next
        Bank9Max.Text = Data.Substring(27, 3)
    End Sub

    Private Sub Bank2Max_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bank1Max.SelectedIndexChanged

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim data As String
        Dim work As String

        Dim l As Integer

        work = "000" & Bank0Max.Text
        l = Len(work)
        data = work.Substring(l - 3)

        work = "000" & Bank1Max.Text
        l = Len(work)
        data = data & work.Substring(l - 3)

        work = "000" & Bank2Max.Text
        l = Len(work)
        data = data & work.Substring(l - 3)

        work = "000" & Bank3Max.Text
        l = Len(work)
        data = data & work.Substring(l - 3)

        work = "000" & Bank4Max.Text
        l = Len(work)
        data = data & work.Substring(l - 3)

        work = "000" & Bank5Max.Text
        l = Len(work)
        data = data & work.Substring(l - 3)

        work = "000" & Bank6Max.Text
        l = Len(work)
        data = data & work.Substring(l - 3)

        work = "000" & Bank7Max.Text
        l = Len(work)
        data = data & work.Substring(l - 3)

        work = "000" & Bank8Max.Text
        l = Len(work)
        data = data & work.Substring(l - 3)

        work = "000" & Bank9Max.Text
        l = Len(work)
        data = data & work.Substring(l - 3)

        SaveSetting("PYRO", "New", "NetworkTopology", data)
    End Sub
End Class