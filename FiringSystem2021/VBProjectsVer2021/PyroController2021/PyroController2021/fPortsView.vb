' (C) Copyright P. Cranwell, 2014
Public Class fPortsView
    Dim i As Integer
    Dim Result As Boolean
    Dim Column As Integer = 0
    Dim clockctr As Long
    Dim TempPoint As Point
    Dim TotalShowTime As Integer = 0
    Dim TimeFromLastEvent As Integer = 0

    ' Define Button Geometry
    Dim ColumnWidth = 60
    Dim ColumnOffset = 80
    Dim RowHeight = 24
    Private TTip As New ToolTip



    Sub BuildScreen()
        Column = 0
        Application.DoEvents()
        BuildFieldModuleButtons()
    End Sub
    Private Sub BuildFieldModuleButtons()
        Dim Command = New cCommand
        Dim i As Integer
        Dim j As Integer
        Dim Ematch As Boolean
        Dim NumCues As Integer

        For i = 3 To MAXNODES
            If Units(i).Active = True Then
                NumCues = Units(i).NumberOfPorts
                BuildButtons(i)
                Application.DoEvents()

                For j = 1 To Units(i).NumberOfPorts
                    If Mid$(Units(i).PinStatus, j, 1) = 1 Then
                        Ematch = False
                    Else
                        Ematch = True
                    End If

                    Units(i).Ports(j).Port.eMatch = Ematch
                Next
            End If
        Next
    End Sub
    Sub BuildButtons(ByVal UnitIndex As Integer)
        Dim i As Integer
        Dim textbox1 As New TextBox
        Dim label1 As New Label


        label1.Size = New Size(60, 20)
        label1.Location = New Point((60 * Column) + 80, 24)
        label1.Text = UnitIndex


        label1.ForeColor = Color.Black
        label1.TextAlign = ContentAlignment.MiddleCenter

        Button1.BackColor = Color.SpringGreen
        Button2.BackColor = Color.LightSeaGreen
        Button3.BackColor = Color.LightSteelBlue
        Button4.BackColor = Color.DarkGray
        Button5.BackColor = Color.Red



        Me.Controls.Add(label1)

        For i = 1 To Units(UnitIndex).NumberOfPorts
            ' Try to cleanup any existing buttons
            Try
                Units(UnitIndex).Ports(i).Button.Dispose()
                Application.DoEvents()
            Catch
            End Try


            With Units(UnitIndex).Ports(i)
                .Button = New Button
                .Button.Font = New System.Drawing.Font("arial", 7.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                .Button.Text = "U" & UnitIndex & " C" & i
                .Button.Text = "P " & i & "     U" & UnitIndex
                TempPoint.X = (ColumnWidth * (Column)) + ColumnOffset
                TempPoint.Y = ((i + 1) * RowHeight)
                .ButtonLoc = TempPoint
                .Button.AutoSize = False
                .Button.MaximumSize = New System.Drawing.Size(60, 24)
                .Button.Location = .ButtonLoc
                .Unit = UnitIndex
                .Button.Name = UnitIndex
                .Button.Tag = UnitIndex


                TTip.InitialDelay = 10
                TTip.SetToolTip(.Button, .Port.valEffect)

                If .IsInShow = False Then
                    .Button.Visible = False
                End If
                


            End With
            If Units(UnitIndex).Ports(i).Port.eMatch = True Then
                If Units(UnitIndex).Ports(i).IsInShow = True Then
                    If Units(UnitIndex).Ports(i).Port.eMatch = True Then
                        If Units(UnitIndex).Ports(i).IsInCueChain = True Then
                            Units(UnitIndex).Ports(i).Button.BackColor = Color.SpringGreen
                        Else
                            Units(UnitIndex).Ports(i).Button.BackColor = Color.LightSeaGreen
                        End If
                    Else
                        Units(UnitIndex).Ports(i).Button.BackColor = Color.Red
                    End If



                Else
                    Units(UnitIndex).Ports(i).Button.BackColor = Color.LightSteelBlue
                    Units(UnitIndex).Ports(i).Button.Visible = False
                End If
                Else
                Units(UnitIndex).Ports(i).Button.BackColor = Color.Red
                End If

            If Units(UnitIndex).Ports(i).Port.eMatchIsFired = True Then
                Units(UnitIndex).Ports(i).Button.BackColor = Color.DarkGray
            End If


            ' Add handler code for a buton click
            AddHandler Units(UnitIndex).Ports(i).Button.Click, AddressOf Button_Click
            AddHandler Units(UnitIndex).Ports(i).Button.MouseDown, AddressOf Button_MouseDown

            Controls.Add(Units(UnitIndex).Ports(i).Button)
            Application.DoEvents()
        Next
        Column = Column + 1
    End Sub
    Private Sub Button_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        ' If Button ir Right Clicked, offer user option to either
        '   Disarm or Rearm the associated unit.

        Dim style = MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2

        Dim Txt As String
        Dim Unit As Integer
        Dim ptr As Integer
        Dim Cmd As New cCommand

        If e.Button = Windows.Forms.MouseButtons.Right Then

            Txt = sender.ToString
            ptr = InStr(Txt, "U")
            Unit = Val(Mid$(Txt, ptr + 1))
            If Units(Unit).Armed = True Then
                ' Define a title for the message box. 
                Dim msg = "Disarm Unit ?"
                ' Display the message box and save the response, Yes or No. 
                Dim response = MsgBox(msg, style)
                If response = MsgBoxResult.Yes Then
                    MsgBox("Disarming Unit")
                    Cmd.Disarm(Unit)
                    Units(Unit).Armed = False
                    Column = 0
                    BuildFieldModuleButtons()
                End If

            Else
                ' Define a title for the message box. 

                Dim msg = "Rearm Unit ?"
                ' Display the message box and save the response, Yes or No. 
                Dim response = MsgBox(msg, style)
                If response = MsgBoxResult.Yes Then
                    MsgBox("Rearming Unit")
                    Cmd.Arm(Unit)
                    Units(Unit).Armed = True
                    Column = 0
                    BuildFieldModuleButtons()
                End If
            End If
        End If


    End Sub
    Private Sub Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
   
    Dim Port As Integer
    Dim WU As New cWorkUnit
    Dim Command = New cCommand

        WU = New cWorkUnit

    'Extract Port number from name
        Port = Val(Mid$(sender.text, 2))

    ' Search the Buts table to find a matching object and retrieve the Unit Number
    '   The Unit Number was saved in the Buts table when the button was built

    Dim i, j As Integer
    Dim UnitID As String





    ' Hunt and Peck Logic to find Unit because Microsoft didnt include enough function on Button Click

        For i = 0 To MAXNODES
            If Units(i).MayBeActive = True Then
                For j = 1 To Units(i).NumberOfPorts
                    If sender.Equals(Units(i).Ports(j).Button) Then
                        UnitID = i
                        UnitIndex = Val(UnitID)
                        i = MAXNODES
                    End If

                Next
            End If
        Next

        If (Units(UnitIndex).Session = True) Then

            If (Units(UnitIndex).Ports(Port).Port.eMatch = True) Then
                If SIMULATE = False Then
                    Command.Fire(Port)
                Else
                    fMain.ListBox1.Items.Add("Simulated Unit " & UnitIndex & " Port " & Port & " fired")
                End If

                Units(UnitIndex).Ports(Port).Port.Active = False
                Units(UnitIndex).Ports(Port).Button.BackColor = Color.DarkGray
                Units(UnitIndex).Ports(Port).Port.eMatchIsFired = True
            Else
                If SIMULATE = False Then
                    Command.Fire(Port)
                End If

                Units(UnitIndex).Ports(Port).Button.BackColor = Color.Gray
            End If
        End If

    End Sub

    Protected Overrides Sub Finalize()
        ' This deactivates the 10 second time message when the system is disarmed.
        fMain.Timer1.Interval = 10000
        fMain.Timer1.Enabled = False
        fMain.Timer1.Stop()
        MyBase.Finalize()
    End Sub

    Private Sub fPortsView_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
        Me.Dispose()
    End Sub

    Private Sub PortsView_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        BuildScreen()
        Application.DoEvents()
    End Sub

    Private Sub bRefresh_Click(sender As System.Object, e As System.EventArgs) Handles bRefresh.Click
        Column = 0
        BuildFieldModuleButtons()
    End Sub
End Class
    