' (C) Copyright P. Cranwell, 2014
Imports System
Imports System.IO.Ports
Imports System.Threading
Imports System.Math


#Const MAXEVENTS = 999



Public Class fShow
    Structure CueButton
        Dim But As Button
        Dim ButLoc As Point
        Dim Text As TextBox
        Dim TextLoc As Point
    End Structure
    Structure CueChain
        Dim Cue As Integer
        Dim Time As Long
        Dim Unit As Integer
        Dim Port As Integer
        Dim Seq As Integer
        Dim NextSeq As Integer
        Dim Effect As String
    End Structure


    Dim Simple As Boolean = False

    ' Cue Button Geometry


    Dim CueButLocX As Integer
    Dim CueButLocY As Integer = 40

    Dim CUE As Integer      ' set to cue when cue fired to give form wide visibility to cue value.

    Dim CueIndexPointer As Integer

    Dim EventItem As CueChain

    ' I tried to dynamically size the array second dimension (ReDim) BUT VB throws an exception
    '   SO, I allocate maximum Array size.   Oh well, memory is cheap.
    Dim CueEventChain(MAXCUES, MAXEVENTS) As cShowEvent    ' Array of ShowEvent Class

    Dim ShowID As String
    Dim i As Integer
    Dim k As Integer
    Dim l As Integer
    Dim Result As Boolean
    Dim CueButtons(80) As CueButton
    Dim CueCount As Integer
    Dim Elapsed As Double
    Dim NumberOfRows As Integer
    'Dim cue As Integer
    Dim Effect As String
    Dim cues(MAXCUES) As Integer
    Dim cuelists(MAXCUES) As ListBox
    Dim cueptr As Integer = 0
    Dim ShowT(16) As ShowT    ' NOTE: This is for 16 port field modules.  It should be expanded for larger field modules
    Dim ShowIndex As Integer
    Dim UnitsToProgram(MAXNODES)   ' A list of units to program
    Dim LastEventTime As Long


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' Define DataGridView1 column layout
    ' This changes when changes to DataGridView1 layout is changed
    Const KeyCol As Integer = 0
    Const ShowIDCol As Integer = 1
    Const ModuleCol As Integer = 2
    Const PortCol As Integer = 3
    Const CueCol As Integer = 4
    Const TimeCol As Integer = 5
    Const SeqCol As Integer = 6
    Const NextSeqCol As Integer = 7
    Const EffectCol = 9
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Private Sub ShowForm_Disposed(sender As Object, e As System.EventArgs) Handles Me.Disposed
        Dim Command = New cCommand

        If SIMULATE = False Then
            Command.GlobalDisarm()
            Command.MasterReset()
        End If


    End Sub
    Private Sub ShowForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim Net = New cPollNetwork
        Dim unit As String
        Dim port As String

        If Simple = True Then
            CueButLocX = 60
        Else
            CueButLocX = 300
        End If

        ' Poll the network & retrieve status from all online Field Module.
        '  This includes Unit IDs, Number of ports, Port status, Voltage, Armed Status and Show Loaded status
        ShowID = TextBox1.Text

        If SIMULATE = False Then
            If Net.PollNetwork() = False Then
                Me.Dispose()
                Exit Sub
            End If
        End If

        ' IF this is not a CSV file conversion then load from database
        If DatabaseDataHighIndex = 0 Then
            ' Fill DATAGRIDVIEW FROM DATABASE
            Me.ShowEventsTableAdapter.FillBy(Me.ShowDatabaseDataSet.ShowEvents, ShowID)

            ' Sort Ascending on Seq field
            DataGridView1.Sort(DataGridView1.Columns(1), ComponentModel.ListSortDirection.Ascending)
        Else
            ' LOAD DATAGRIDVIEW FROM IN MEMORY DATA STRUCTURE "DatabaseData"
            ' This is from a CSV file conversion operation
            Dim ShowEventsRow As ShowDatabaseDataSet.ShowEventsRow

            Dim Key As String
            Dim Show As String
            Dim i As Integer

            Key = ShowEventsTableAdapter.FindHighKey + 1
            Show = ShowEventsTableAdapter.FindHighShow + 1

            For i = 0 To DatabaseDataHighIndex - 1

                ShowEventsRow = ShowDatabaseDataSet.ShowEvents.NewShowEventsRow()

                ' Build Row with following logic
                ShowEventsRow.ShowID = Show
                ShowEventsRow.Seq = Key
                ShowEventsRow.Time = DatabaseData(i).Time
                ShowEventsRow.AreaNumber = DatabaseData(i).AreaNumber
                ShowEventsRow.AreaName = DatabaseData(i).AreaName
                ShowEventsRow.CaliberNumeric = DatabaseData(i).Caliber
                ShowEventsRow.Cue = DatabaseData(i).Cue
                ShowEventsRow.ItemNumber = DatabaseData(i).ItemNumber
                ShowEventsRow.ItemName = DatabaseData(i).ItemName
                ShowEventsRow.Effect = DatabaseData(i).ItemName
                ShowEventsRow.SlatID = DatabaseData(i).Slat
                ShowEventsRow._Module = DatabaseData(i).Unit
               
                ShowEventsRow.PortNumber = DatabaseData(i).Pin
                ShowEventsRow.Seq_ID = DatabaseData(i).Seq
                ShowEventsRow.Next_Seq = DatabaseData(i).NextSeq
                ShowEventsRow.Cue = DatabaseData(i).Cue

                unit = DatabaseData(i).Unit
                port = DatabaseData(i).Pin
                Units(unit).Ports(port).Port.valEffect = DatabaseData(i).ItemName


                ' Add row to the database
                ShowDatabaseDataSet.ShowEvents.Rows.Add(ShowEventsRow)


                Key = Key + 1

            Next

        End If


        NumberOfRows = DataGridView1.Rows.Count



        '---------------------------------------------------------------------------------------------
        For Me.i = 0 To NumberOfRows - 2

            DataGridView1.CurrentCell = DataGridView1.Rows(Me.i).Cells(ShowIDCol)
            unit = DataGridView1.CurrentRow.Cells(ModuleCol).Value
            UnitsToProgram(unit) = unit    ' Just remember we need to program this unit
            port = DataGridView1.CurrentRow.Cells(PortCol).Value
            If port = 0 Then
                MsgBox("Database Error,  Unit " & unit & " Port is Zero")
                Exit Sub
            End If
            Units(unit).Ports(port).IsInShow = True
            Effect = DataGridView1.CurrentRow.Cells(EffectCol).Value
            Units(unit).Ports(port).Port.valEffect = Effect



            ' NOTE: Ports are indexed based on 0 or 0-n
            If Units(unit).Active = False Or Units(unit).Ports(port).Port.eMatch = False Then
                DataGridView1.CurrentRow.DefaultCellStyle.BackColor = Color.Red
                TextBox2.BackColor = Color.Red
                TextBox2.Text = "Some eMatch connections need attention"
            End If
        Next

        '-------------------------------------------------------------------------------------------------

        Arm.Visible = False
        CueCount = 0
        ComboBox1.Items.Add("1")
        ComboBox1.Items.Add("2")
        ComboBox1.Text = "1"


    End Sub

    Private Sub ProgramModules_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProgModules.Click
        ' Program Field Modules with Show Data

        Dim NumberOfRows As Integer      ' Number of Rows in DataGridView
        Dim i As Integer
        Dim j As Integer
        Dim k As Integer
        Dim l As Integer
        Dim Unit As String
        Dim Port As String
        Dim UnitNumber As Integer
        Dim LastUnit As Integer
        Dim Hit As Boolean

        Dim WU As cWorkUnit
        Dim ProgramMsg As String
        Dim Reply As String
        Dim tot As Integer
        Dim NumWU As Integer
        Dim Command As New cCommand
        Dim cues(MAXCUES) As Integer
        'Dim cueptr As Integer = 1
        Dim cue As Integer
        Dim StopWatch As New cStopWatch
        Dim Net = New cPollNetwork

        LastEventTime = 0
        cueptr = 0
        Me.ProgModules.Visible = False

        Me.ActivateModules.Visible = False

        TextBox2.ForeColor = Color.White
        TextBox2.BackColor = Color.Red



        Traceit("Begin Programming Units")

        ' Start all Active Field Modules
        StartFieldModules()
        Application.DoEvents()



        tot = CommPort.ReadTOT

        UnitIndex = 1


        '----------------------------------------------------------------------------------------
        ' Determine Field Modules to program by stepping through datagridview
        '----------------------------------------------------------------------------------------

        NumberOfRows = DataGridView1.RowCount


        ' Set Red Row color if eMatch is bad

        For i = 0 To NumberOfRows - 2
            Hit = False

            ' Extract Field Module Address
            DataGridView1.CurrentCell = DataGridView1.Rows(i).Cells(ShowIDCol)
            k = DataGridView1.CurrentRow.Index

            ' Set Effect into Port Struct
            Unit = DataGridView1.CurrentRow.Cells(ModuleCol).Value
            Port = DataGridView1.CurrentRow.Cells(PortCol).Value
            Units(Unit).Ports(Port).Port.valEffect = DataGridView1.CurrentRow.Cells(EffectCol).Value

            ' Mark Unit for programming by searching UnitArray until this unit is found
            '     then mark it for programming.
            For j = 1 To NumberOfRows
                If UnitArray(j) = Unit Then
                    Hit = True
                    j = NumberOfRows
                End If
            Next

            If Hit = False Then
                UnitArray(UnitIndex) = Unit
                UnitIndex = UnitIndex + 1
            End If
        Next


        '-------------------------------------------------------------------------------------------------------------------------
        ' UnitIndex now has the count of units to program and
        ' UnitArray has a list of the units to program
        '-------------------------------------------------------------------------------------------------------------------------



        ' Check to make sure required Field Modules are Active and have active Sessions
        ' Program what you can and warn for offline units


        For i = 1 To UnitIndex - 1
            Dim Active As Boolean = False
            For j = 1 To MAXNODES
                If Units(UnitArray(i)).Session = True Then
                    j = MAXNODES
                    Continue For
                End If
                MsgBox("Field Module " & UnitArray(i) & " Not Active")
                j = MAXNODES
                Continue For
            Next
        Next





        '---------------------------------------------------------------------------------------------------
        ' Sort Datagridview into Seq order
        ' This is required for microprocessor programming
        '---------------------------------------------------------------------------------------------------


        CommPort.ReadTOT = 500

        '---------------------------------------------------------------------------
        ' Step thru UnitArray , field module by field module
        '---------------------------------------------------------------------------

        For i = 1 To UnitIndex - 1
            LastEventTime = 0
            ShowIndex = 0
            Unit = UnitArray(i)


            '---------------------------------------------------------------------------------
            ' Program Active Units
            '---------------------------------------------------------------------------------

            If Units(Unit).Active = True Then
                UnitNumber = Val(Unit)
                TextBox2.Text = "Programming Unit " & UnitNumber
                Application.DoEvents()



                '------------------------------------------------
                ' Program this unit
                '------------------------------------------------

                For j = 0 To NumberOfRows - 2
                    ' Step thru the DataGridView, row by row
                    DataGridView1.CurrentCell = DataGridView1.Rows(j).Cells(ShowIDCol)
                    k = DataGridView1.CurrentRow.Index

                    '-----------------------------------
                    ' Get all data for this module
                    '-----------------------------------
                    If Unit = DataGridView1.CurrentRow.Cells(ModuleCol).Value Then     ' Extract Unit Number




                        '-----------------------------------------
                        ' Load ShowT array with this unit's data
                        '-----------------------------------------
                        ShowT(ShowIndex).Unit = Unit
                        Try
                            ' extract cue from datagrid
                            cue = DataGridView1.CurrentRow.Cells(CueCol).Value

                            ' If a Cue is specified, add it to the list of cues
                            If cue > 0 Then
                                For k = 1 To MAXCUES
                                    If cues(k) = cue Then k = MAXCUES
                                    If cues(k) = 0 Then
                                        cues(k) = cue
                                        cueptr = cueptr + 1
                                        k = MAXCUES
                                    End If
                                Next
                            End If

                        Catch
                            ' catch null values in database.   Should have been coded 0
                            cue = 0
                        End Try



                        '----------------------------------------------------------------------------------
                        'NOTE:  Seq and Next Seq from datagrid refer to positions in the database
                        '       These values need to be resolved into values for the field unit modules.
                        '----------------------------------------------------------------------------------




                        ' Load the show data from the datagrid
                        ShowT(ShowIndex).CueNumber = cue
                        ShowT(ShowIndex).Port = DataGridView1.CurrentRow.Cells(PortCol).Value
                        ShowT(ShowIndex).TimeBefore = DataGridView1.CurrentRow.Cells(TimeCol).Value
                        ShowT(ShowIndex).NextSequence = DataGridView1.CurrentRow.Cells(NextSeqCol).Value





                        '   Force sequential time check with a new Cue

                        If cue > 0 Then
                            LastEventTime = 0
                        End If




                        '-----------------------------------------------------------------------------------------------------------------
                        ' Throw an error if this has been chained to and chained from has a before time greater than
                        '   my time.  Chained events must be in time sequence.
                        '-----------------------------------------------------------------------------------------------------------------
                        If LastEventTime = 0 Then
                            LastEventTime = ShowT(ShowIndex).TimeBefore
                        Else
                            If LastEventTime < ShowT(ShowIndex).TimeBefore Then
                                LastEventTime = ShowT(ShowIndex).TimeBefore
                            Else
                                If ShowIndex > 0 Then
                                    If ShowT(ShowIndex - 1).NextSequence > 0 Then
                                        MsgBox("Error:  Chaining to Time Lower than current")
                                        Me.Close()
                                        Exit Sub
                                    End If
                                End If

                            End If

                        End If

                        '-----------------------------------------------------------------------------------------------------------------


                        ShowIndex = ShowIndex + 1
                        NumWU = ShowIndex

                    End If
                Next


                DataGridView1.BackgroundColor = Color.Wheat
                ' Show events for each unit stored in ShowT
                ' Now, program the unit for this show

                If SIMULATE = False Then

                    Command.StartProgramming(UnitNumber)

                    Units(UnitNumber).Msg.NewMessage()
                    Units(UnitNumber).Msg.Command = "04"
                    Units(UnitNumber).Msg.Unit = UnitNumber


                    '--------------------------------------------------------------------------------------------------------
                    ' NOTE This logis has been set up for 16 pin field modules.  It needs to be expanded to work
                    '    with newer modules with more ports and more memory.
                    '
                    'For newer 32 bit units with expanded memory, the messages may be up to 255 characters each.
                    '    so the logic must be changed to support these additioonal designs
                    '--------------------------------------------------------------------------------------------------------


                    '----------------------------------------------------------------------------------
                    'NOTE:  Seq and Next Seq from ShowT(k) refer to positions in the database
                    '       These values need to be resolved into values for the field unit modules.
                    '----------------------------------------------------------------------------------

                    Dim Sequence As Integer = 2


                    ' Write first 8 events in this message
                    If NumWU > 8 Then ShowIndex = 8

                    For k = 0 To ShowIndex - 1
                        WU = New cWorkUnit
                        WU.Port = ShowT(k).Port
                        l = Len(ShowT(k).TimeBefore)
                        WU.MS = Mid$("00000000" & ShowT(k).TimeBefore, l + 1)
                        WU.Cue = ShowT(k).CueNumber
                        'WU.NextSeq = ShowT(k).NextSequence
                        If (Val(ShowT(k).NextSequence)) > 0 Then
                            WU.NextSeq = Sequence
                        Else
                            WU.NextSeq = "000"
                        End If
                        Sequence = Sequence + 1

                        Units(UnitNumber).Msg.AddWU(WU.TWorkUnit)
                    Next
                    Units(UnitNumber).Msg.BumpSendNumber()
                    ' WU are added to message is Seq Order 
                    ProgramMsg = Units(UnitNumber).Msg.CompleteMessage()
                    'MainForm.ListBox1.Items.Add(ProgramMsg)

                    Traceit(ProgramMsg)


                    CommPort.ReadTOT = 800

                    ' Program the field module with the first 8 events
                    Reply = CommPort.Transaction(ProgramMsg, UnitNumber)

                    If Reply = "" Then
                        Traceit("Transaction Error")
                        MsgBox("Cant Program Unit " & UnitNumber)
                        Command.MasterReset()
                        Me.Finalize()
                        Me.Dispose()
                        Exit Sub
                    Else
                        Traceit("Unit " & UnitNumber & " Programmed")
                    End If

                    If NumWU > 8 Then

                        ' Start Programming again for second programming message
                        Command.StartProgramming(UnitNumber)

                        Units(UnitNumber).Msg.NewMessage()
                        Units(UnitNumber).Msg.Command = "04"
                        Units(UnitNumber).Msg.Unit = UnitNumber


                        For k = 8 To NumWU - 1
                            WU = New cWorkUnit
                            WU.Port = ShowT(k).Port
                            l = Len(ShowT(k).TimeBefore)
                            WU.MS = Mid$("00000000" & ShowT(k).TimeBefore, l + 1)
                            WU.Cue = ShowT(k).CueNumber
                            'WU.Seq = ShowT(k).Sequence
                            ' WU.NextSeq = ShowT(k).NextSequence
                            If (Val(ShowT(k).NextSequence)) > 0 Then
                                WU.NextSeq = Sequence
                            Else
                                WU.NextSeq = "000"
                            End If
                            Sequence = Sequence + 1
                            Units(UnitNumber).Msg.AddWU(WU.TWorkUnit)
                        Next
                        Units(UnitNumber).Msg.BumpSendNumber()
                        ' WU are added to message is Seq Order 
                        ProgramMsg = Units(UnitNumber).Msg.CompleteMessage()

                        ' Write the remainder of events to the field module
                        Reply = CommPort.Transaction(ProgramMsg, UnitNumber)

                        If Reply = "" Then
                            Traceit("Transaction Error")
                            MsgBox("Cant Program Unit " & UnitNumber)
                            Command.MasterReset()
                            Me.Finalize()
                            Exit Sub
                        Else
                            Traceit("Unit " & UnitNumber & " Programmed")
                        End If

                    End If

                    ' End Programming this field module
                    Command.EndProgramming(UnitNumber)
                End If
            End If

        Next




        cueptr = cueptr + 1

        'cueptr = number of cues
        'cues has a list of cues
        UpdateDatabase.Visible = True
        'GetUnitStatus()


        ' PATCH
        If Simple = False Then
            BuildCueEventChain()
        End If

        BuildCueButtons(cueptr)

        TextBox2.Text = "Done"
        TextBox2.ForeColor = Color.White
        TextBox2.BackColor = Color.Green
        Elapsed = StopWatch.ElapsedTime()
        StopWatch.ReportToConsole("Total Program Time = ")
        'fTrace.Show()
        StopWatch.Done()


        CommPort.ReadTOT = tot
        LastUnit = UnitIndex





        Me.SaveShow.Visible = True
    End Sub

    Sub BuildCueButtons(ByVal CueIndex As Integer)
        Dim i As Integer

        ' BuildCueEventChain()

        ' CueCount = cueptr - 1
        CueCount = cueptr
        For i = 1 To CueCount
            Try
                CueButtons(i).But.Dispose()
            Catch
            End Try
            CueButtons(i).But = New Button
            CueButtons(i).But.Text = "Cue " & i

            CueButtons(i).ButLoc.X = ((i - 1) * CuebutLocX)
            CueButtons(i).ButLoc.Y = (CueButLocY)


            CueButtons(i).But.AutoSize = False
            CueButtons(i).But.MaximumSize = New System.Drawing.Size(70, 24)
            CueButtons(i).But.Location = CueButtons(i).ButLoc
            CueButtons(i).But.BackColor = Color.Green
            CueButtons(i).But.Enabled = False
            CueButtons(i).But.Visible = False


            AddHandler CueButtons(i).But.Click, AddressOf Cue_Button_Click

            Controls.Add(CueButtons(i).But)

            Application.DoEvents()

        Next
    End Sub

    Private Sub Cue_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim name As System.Object
        Dim Comd As New cCommand
        Dim Msg1 As New cMessage
        Dim WU As New cWorkUnit

        ' Cue fired


        name = sender

        ' fMain.Timer1.Interval = 1000

        ShowTime = 0

        CUE = Mid$(name.text, 5)
        Comd.TriggerCue(CUE)


        fMain.Timer1.Stop()
        fMain.Timer1.Interval = DEAD_MAN_INTERVAL
        fMain.Timer1.Enabled = True
        fMain.Timer1.Start()

        ' Start local timer and reset TOD = 0
        TOD = 0

        ' PATCH
        If Simple = False Then
            Timer1.Enabled = True
            Timer1.Start()
        End If


        CueButtons(Val(Mid$(name.text, 5))).But.BackColor = Color.Gray
        Traceit("Cue fired")
        Application.DoEvents()
        LongWait(75)

    End Sub
    Private Sub SaveShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveShow.Click
        Dim i As Integer
        Dim UnitNumber As Integer
        Dim Command As New cCommand

        TextBox2.ForeColor = Color.White
        TextBox2.BackColor = Color.Red
        TextBox2.Text = "Saving Show in EEPROM Slot"

        For i = 1 To UnitIndex - 1

            UnitNumber = UnitArray(i)
            If Units(UnitNumber).Active = True Then
                Command.Unit = UnitNumber
                Command.ProgramUnit(ComboBox1.Text, UnitNumber)
                TextBox2.Text = "Unit " & UnitNumber & " Programmed in slot " & ComboBox1.Text
                LongWait(150)
            End If
        Next
        TextBox2.ForeColor = Color.White
        TextBox2.BackColor = Color.Green

        Me.SaveShow.Visible = False
        Me.SetDefaultShow.Visible = True
    End Sub
    Sub StartFieldModules()
        Dim i As Integer
        Dim Command As New cCommand

        For i = 1 To MAXNODES
            If UnitsToProgram(i) > 0 Then
                If Units(i).Active = True Then
                    Application.DoEvents()
                    Command.StartSession(i)
                    If Units(i).Session = False Then
                        MsgBox("Cant Start Field Module " & i)
                        Exit Sub
                    End If
                End If
            End If

        Next
    End Sub

    Private Sub UpdateDatabase_Click(sender As System.Object, e As System.EventArgs) Handles UpdateDatabase.Click
        Me.ShowEventsTableAdapter.Update(Me.ShowDatabaseDataSet.ShowEvents)
    End Sub
    Private Sub Arm_Click(sender As System.Object, e As System.EventArgs) Handles Arm.Click
        Dim i As Integer
        Dim command As New cCommand
        Dim Net = New cPollNetwork

        TextBox2.Text = "Building Cue Buttons.  Please wait"
        'fClock.Show()

        ' Arm Active Units with Unit Arm commands

        For i = 1 To cueptr
            BuildCueButtons(i)
        Next


        For i = 1 To MAXNODES
            If UnitsToProgram(i) > 0 Then
                If Units(i).Session = True Then
                    TextBox2.Text = "Arming Unit " & UnitArray(i)
                    Application.DoEvents()
                    If SIMULATE = False Then
                        command.Arm(i)
                        ' LongWait(75)
                    Else
                        Units(i).Armed = True
                    End If

                    Application.DoEvents()
                End If
            End If

        Next


        If SIMULATE = False Then
            fMain.Timer1.Interval = DEAD_MAN_INTERVAL
            fMain.Timer1.Start()
            fMain.Timer1.Enabled = True
        End If

        ShowTime = 0


        Arm.Visible = False

        UpdateDatabase.Hide()


        For i = 1 To CueCount
            CueButtons(i).But.Enabled = True
            CueButtons(i).But.Visible = True
        Next

        TextBox2.Text = "All Units Armed"
        TextBox2.BackColor = Color.Red
        DataGridView1.Visible = False


        Application.DoEvents()

    End Sub

    Protected Overrides Sub Finalize()
        fClock.Close()
        MyBase.Finalize()
    End Sub

    Private Sub ActivateModules_Click(sender As System.Object, e As System.EventArgs) Handles ActivateModules.Click
        ' Load Show storted in modules

        Dim Command As New cCommand
        Dim i As Integer
        Dim Net As New cPollNetwork

        Me.ProgModules.Visible = False


        If SIMULATE = False Then
            StartFieldModules()
        End If


        GetCueList()
        Application.DoEvents()

        Arm.Visible = True
        Arm.BackColor = Color.Red
        ProgModules.Hide()
        SaveShow.Hide()
        SetDefaultShow.Hide()
        ActivateModules.Hide()
        Arm.Hide()
        ComboBox1.Hide()
        Label1.Hide()


        ' Issue Load Show command
        fMain.ComboBox2.Text = ComboBox1.Text
        ' Show ID passed from MainForm, Combobox1
        ' Change this, pass as a parameter. This is a hangover from old code
        UnitIndex = 2
        If SIMULATE = False Then
            Command.GlobalLoadShow()
        End If


        ' Wait for Field Modules to load show

        TextBox2.Text = "Loading Stored Show in Modules.   WAIT......"
        For i = 1 To 2
            TextBox2.ForeColor = Color.Red
            Application.DoEvents()
            Thread.Sleep(500)

            TextBox2.BackColor = Color.White
            Application.DoEvents()
            Thread.Sleep(500)
        Next

        TextBox2.Text = "Ready to Arm show."
        TextBox2.BackColor = Color.Green
        TextBox2.ForeColor = Color.White

        'Net.PollNetwork()

        Arm.Show()

    End Sub

    Sub GetCueList()

        Dim k As Integer

        ' Scan through DataGridView and get a list of all cues.

        NumberOfRows = DataGridView1.RowCount

        For j = 0 To NumberOfRows - 2
            ' Step thru the DataGridView, row by row
            DataGridView1.CurrentCell = DataGridView1.Rows(j).Cells(ShowIDCol)
            k = DataGridView1.CurrentRow.Index


            Try
                ' extract cue from datagrid
                cue = DataGridView1.CurrentRow.Cells(CueCol).Value

                ' Build an array of the cues in the show
                If cue > 0 Then
                    For k = 1 To MAXCUES
                        If cues(k) = CUE Then
                            k = MAXCUES
                            Continue For
                        End If
                        'k = MAXCUES
                        If cues(k) = 0 Then
                            cues(k) = CUE
                            cueptr = cueptr + 1
                            k = MAXCUES
                        End If
                    Next
                End If

            Catch
                ' catch null values in database
                cue = 0

            End Try

        Next


        ' PATCH
        If Simple = False Then
            BuildCueEventChain()
        End If




        ' cues contains a list of cues in the show


    End Sub
    Sub BuildCueEventChain()
        Dim i, j, k, l, m As Integer
        Dim SeqID As String
        Dim _SeqID As String
        Dim NextSeq As String
        Dim Time As String
        Dim Unit As String
        Dim _Unit As String
        Dim Port As String
        Dim Effect As String
        Dim cue As Integer
        Dim CueToSort As Integer
        Dim ThisCue = 0




        '---------------------------------------------------------------------------------------------------------
        ' Process DataGridView and build sorted events for each Cue to display on scheen
        '
        ' Walk through DataGridView1, when Cue found, run the NextSeq chain  and add data to array this Cue.
        ' When all cues have been processed, sort the array into Time sequence.
        '-------------------------------------------------------------------------------------------------------

        NumberOfRows = DataGridView1.RowCount
        ' temp fix
        NumberOfRows = NumberOfRows - 1
        For i = 0 To NumberOfRows - 1

            ' find next Cue
            Try
                cue = DataGridView1.Rows(i).Cells(CueCol).Value
            Catch
                Beep()
                ' trap blank rows, prevent abnormal termination
                ' on extra datagridview rows.
            End Try

            ' Find Cue in DataGridView
            If cue > 0 Then
                CueToSort = cue
                m = i     ' Remember where we left off in the scan

                ' Build event listboxes for cues
                Dim xpos
                xpos = (cue - 1) * CueButLocX
                If cuelists(cue) Is Nothing Then
                    cuelists(cue) = New ListBox
                    cuelists(cue).Location = New System.Drawing.Point(xpos, 65)
                    cuelists(cue).Name = "List" & cue
                    cuelists(cue).Size = New System.Drawing.Size(CueButLocX, 500)
                    cuelists(cue).ForeColor = System.Drawing.Color.Black
                    cuelists(cue).Font = New Font("Courier", 8)
                    Controls.Add(cuelists(cue))
                End If



                ' Run the chain of events
                NextSeq = 1
                While NextSeq > 0
                    ' cue found and it is another of the same cue ion the show
                    '    get next seq
                    SeqID = DataGridView1.Rows(i).Cells(SeqCol).Value
                    NextSeq = DataGridView1.Rows(i).Cells(NextSeqCol).Value
                    Time = DataGridView1.Rows(i).Cells(TimeCol).Value
                    Unit = DataGridView1.Rows(i).Cells(ModuleCol).Value
                    Port = DataGridView1.Rows(i).Cells(PortCol).Value
                    Units(Unit).Ports(Port).IsInShow = True
                    Units(Unit).Ports(Port).IsInCueChain = True
                    Effect = ""
                    Try
                        Effect = DataGridView1.Rows(i).Cells(EffectCol).Value
                    Catch
                    End Try



                    i = i
                    With EventItem
                        .Cue = cue
                        .Effect = Effect
                        .Unit = Unit
                        .Port = Port
                        .Time = Time
                    End With

                    ' Find empty spot in array
                    For l = 1 To MAXEVENTS
                        If (CueEventChain(cue, l) Is Nothing) Then
                            k = l
                            l = MAXEVENTS
                        End If
                    Next

                    ' put new ShowEvent class object into array

                    CueEventChain(cue, k) = New cShowEvent
                    CueEventChain(cue, k).Cue = cue
                    CueEventChain(cue, k).Unit = Unit
                    CueEventChain(cue, k).Port = Port
                    CueEventChain(cue, k).Effect = Effect
                    CueEventChain(cue, k).Time = Time


                    ' find next event in chain
                    If NextSeq > 0 Then
                        For j = 0 To NumberOfRows - 1
                            _Unit = DataGridView1.Rows(j).Cells(ModuleCol).Value
                            _SeqID = DataGridView1.Rows(j).Cells(SeqCol).Value
                            If Unit = _Unit And NextSeq = _SeqID Then
                                i = j
                                j = NumberOfRows - 1
                            End If
                        Next
                    End If

                End While
                i = m '+ 1
            End If
        Next


        For cue = 1 To MAXCUES
            If cuelists(cue) Is Nothing Then
                cue = MAXCUES
            Else
                ' Sort Event Chain into Time Sequence for this Cue
                BubbleSort(CueEventChain, cue)
            End If
        Next

    End Sub
    Sub BubbleSort(ByRef arr As cShowEvent(,), cue As Integer)
        Dim temp
        Dim i
        Dim ArraySize As Integer
        Dim switch = True


        ' arr = two dimensional array
        '   Dimension 1 = Cue ID
        '   Dimension 2 = Cue Event Class instances

        ' The Bubble Sort is slow, but adequate for this application


        ' Find number of cue events for this cue
        i = 1
        While i > 0
            If arr(cue, i) Is Nothing Then
                ArraySize = i - 1    ' get out
                i = -1
            Else
            End If
            i = i + 1
        End While


        ' Array Size is size of second dimension of array

        While switch

            switch = False

            For x = 1 To ArraySize - 1

                If arr(cue, x).Time > arr(cue, x + 1).Time Then

                    temp = arr(cue, x)

                    arr(cue, x) = arr(cue, x + 1)

                    arr(cue, x + 1) = temp

                    switch = True

                End If

            Next

        End While


        Dim TDisp, UDisp, PDisp As String


        ' Normalize the data format and Add event details to cue listbox
        For i = 1 To ArraySize
            TDisp = "00000000" & arr(cue, i).Time.ToString
            TDisp = Mid$(TDisp, Len(TDisp) - 7)
            UDisp = "   " & arr(cue, i).Unit
            UDisp = Mid$(UDisp, Len(UDisp) - 2)
            PDisp = "   " & arr(cue, i).Port
            PDisp = Mid$(PDisp, Len(PDisp) - 2)
            cuelists(cue).Items.Add(TDisp & "  " & UDisp & "  " & PDisp & "  " & arr(cue, i).Effect)
        Next


    End Sub
    Private Sub PortsView_Click(sender As System.Object, e As System.EventArgs) Handles PortsViewButton.Click
        fPortsView.Show()
    End Sub
   
    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick

        Dim i, j As Integer
        Dim sval As String
        Dim unit As Integer
        Dim port As Integer


        Try

            Application.DoEvents()
            ' Update Timer by 100MS and update Cue Event list point to show show progress
           
            j = cuelists(CUE).Items.Count

            ' For i = 0 To j - 1
            For i = 0 To j

                If i + 1 < j Then
                    sval = Mid$(cuelists(CUE).Items(i + 1).ToString, 1, 8)
                    ' Mark eMatch as fired
                    unit = Val(Mid$(cuelists(CUE).Items(i).ToString, 11, 3))
                    port = Val(Mid$(cuelists(CUE).Items(i).ToString, 16, 3))
                    Units(unit).Ports(port).Port.eMatchIsFired = True

                    If TOD < sval Then

                        cuelists(CUE).SelectedIndex = i


                        Application.DoEvents()
                        i = j  ' Done
                    End If
                Else
                    cuelists(CUE).SelectedIndex = j - 1
                    unit = Val(Mid$(cuelists(CUE).Items(i).ToString, 11, 3))
                    port = Val(Mid$(cuelists(CUE).Items(i).ToString, 16, 3))
                    Units(unit).Ports(port).Port.eMatchIsFired = True
                    Timer1.Stop()

                End If

            Next

        Catch
            Beep()
        End Try

    End Sub

    Private Sub SetDefaulShow_Click(sender As System.Object, e As System.EventArgs) Handles SetDefaultShow.Click
        Dim i As Integer
        Dim UnitNumber As Integer
        Dim Command As New cCommand
        Dim Net As New cPollNetwork

        TextBox2.ForeColor = Color.White
        TextBox2.BackColor = Color.Red
        TextBox2.Text = "Setting Default Show"

        For i = 1 To MAXNODES
            If UnitsToProgram(i) > 0 Then
                If Units(i).Active = True Then
                    UnitNumber = i
                    Command.Unit = UnitNumber
                    Command.SetDefaultShow(ComboBox1.Text)
                    TextBox2.Text = "Default Show " & ComboBox1.Text & " Set for unit " & UnitNumber
                End If
            End If

        Next i

        ' Net.PollNetwork()

        TextBox2.Text = "Ready to Arm show."
        TextBox2.BackColor = Color.Green
        TextBox2.ForeColor = Color.White
        Arm.ForeColor = Color.Red

        Me.SetDefaultShow.Visible = False
        Arm.Visible = True
    End Sub

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        If fClock.Visible = False Then
            fClock.Show()
        Else
            fClock.Hide()
        End If


    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class