' (C) Copyright P. Cranwell, 2014
Imports System.IO

Public Class fCSVImport

    ' NOTE:   Code is not finished
    '   ToDo:   
    '       Add logic to compile Area, Caliber, Item data
    '       Add logic to convert Slat and Pin to all numeric pin number
    '       Rework Data Table logic to check for and correct errors.

    Dim MyDataTable As New DataTable
    Dim filename As String = ""
    Dim IsProcessEffects As Boolean = False



    Private Sub ProcessData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProcessData.Click
        Dim i As Integer
        Dim j As Integer
        Dim UnitEventCount(MAXNODES) As Integer
        Dim unit As Integer
        Dim port As Integer


        Dim colname As String




        ' Retrieve data from Datagridview1 and load into DatabaseData array

        ' multiple statements are used because VB cant deal with complex statements

        'Try
        For i = 0 To DataGridView1.RowCount - 1

            j = Val(ModuleTextBox.Text)
            colname = DataGridView1.Columns(j).HeaderText
            Try
                DatabaseData(i).Unit = DataGridView1.Item(colname, i).Value
                unit = DatabaseData(i).Unit
                
            Catch
                DatabaseData(i).Unit = 0
            End Try

            If UnitEventCount(DatabaseData(i).Unit) = 0 Then
                UnitEventCount(DatabaseData(i).Unit) = 1
                DatabaseData(i).Cue = 1
                DatabaseData(i).Seq = 1
                DatabaseData(i).NextSeq = 2
            Else
                DatabaseData(i).Seq = DatabaseData(i - 1).NextSeq
                DatabaseData(i).NextSeq = DatabaseData(i).Seq + 1
            End If

            j = Val(CueTextBox.Text)
            colname = DataGridView1.Columns(j).HeaderText
            Try
                DatabaseData(i).Cue = DataGridView1.Item(colname, i).Value
            Catch
                DatabaseData(i).Cue = 0
            End Try
            If j = 0 Then DatabaseData(i).Cue = 0

            If TimeMult.Text = 0 Then
                MsgBox("Time Multiplier required")
                Exit Sub
            End If

            j = Val(TimeTextBox.Text)
            colname = DataGridView1.Columns(j).HeaderText
            Try
                DatabaseData(i).Time = DataGridView1.Item(colname, i).Value * Val(TimeMult.Text)

            Catch
                DatabaseData(i).Time = 0
            End Try





            j = Val(PinTextBox.Text)

            colname = DataGridView1.Columns(j).HeaderText
            Try
                DatabaseData(i).Pin = DataGridView1.Item(colname, i).Value
                port = DatabaseData(i).Pin
            Catch
                DatabaseData(i).Pin = 0
            End Try



            j = Val(SlatTextBox.Text)
            colname = DataGridView1.Columns(j).HeaderText
            Try
                DatabaseData(i).Slat = DataGridView1.Item(colname, i).Value
            Catch ex As Exception
                DatabaseData(i).Slat = 0
            End Try

            j = Val(AreaTextBox.Text)
            colname = DataGridView1.Columns(j).HeaderText
            Try
                DatabaseData(i).AreaName = DataGridView1.Item(colname, i).Value
            Catch ex As Exception
                DatabaseData(i).AreaName = 0
            End Try

            j = Val(CaliberTextBox.Text)
            colname = DataGridView1.Columns(j).HeaderText
            Try
                DatabaseData(i).Caliber = DataGridView1.Item(colname, i).Value
            Catch ex As Exception
                DatabaseData(i).Caliber = 0
            End Try

            j = Val(ItemTextBox.Text)
            colname = DataGridView1.Columns(j).HeaderText
            Try
                DatabaseData(i).ItemName = DataGridView1.Item(colname, i).Value

            Catch ex As Exception
                DatabaseData(i).ItemName = 0
            End Try

            Try
                Units(unit).Ports(port).Effect = DatabaseData(i).ItemName
            Catch
            End Try



        Next


        DatabaseDataHighIndex = i - 1

        If IsProcessEffects = True Then
        Else
            fShow.Show()
        End If

        Me.Hide()

    End Sub

    Private Sub LoadCSV_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadCSV.Click
        Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()
        Dim name As String

        ' Display CSV Files and allow User to select file

        openFileDialog1.InitialDirectory = "c:\PyroController"
        openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True



        ' Read CSV into 
        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                name = openFileDialog1.FileName
                Dim sr As New StreamReader(name)

                If (sr IsNot Nothing) Then
                    Dim splits As String()
                    Dim data As String
                    Try
                        Using sr 'As StreamReader '= New StreamReader(filename)
                            'read the first line for the table column headers
                            data = sr.ReadLine
                            splits = data.Split(","c)

                            For i As Integer = 0 To UBound(splits)
                                ' add to data table
                                MyDataTable.Columns.Add(splits(i))
                            Next
                            'read the rest of the lines to add rows to the table
                            Do While Not sr.EndOfStream
                                data = sr.ReadLine
                                splits = data.Split(","c)
                                'splits = sr.ReadLine.Split(","c)

                                ' Purge blank lines
                                Dim len As Integer

                                len = splits.Length
                                If len > 1 Then
                                    ' add to data table
                                    MyDataTable.Rows.Add(splits)
                                End If
                            Loop
                        End Using



                    Catch ex As Exception
                        MsgBox("CSV Processing Error.  File should not contain extra commas!!!")
                    Finally

                        ' Set the DataGridView data source
                        DataGridView1.DataSource = MyDataTable.DefaultView


                        ' Disable Sorting of DataGridView
                        Dim i As Integer
                        For i = 0 To DataGridView1.Columns.Count - 1
                            DataGridView1.Columns.Item(i).SortMode = DataGridViewColumnSortMode.Programmatic
                        Next i



                        Dim j As Integer

                        i = DataGridView1.Columns.GetColumnCount(DataGridViewElementStates.Selected)
                        For Each c As DataGridViewColumn In DataGridView1.SelectedColumns
                            j = c.Index
                        Next
                    End Try
                End If
            Catch Ex As Exception
                MessageBox.Show("Cannot read file from disk. Original error: " & Ex.Message)
            Finally
                ' Check this again, since we need to make sure we didn't throw an exception on open. 
                If (myStream IsNot Nothing) Then
                    myStream.Close()
                End If
            End Try
        End If
    End Sub

    Private Sub DataGridView1_ColumnHeaderMouseClick1(sender As Object, e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DataGridView1.ColumnHeaderMouseClick
        Dim RowCount As Integer
        Dim i As Integer
        Dim j As Integer
        Dim hit As Integer
        Dim max As Integer
        Dim CellData As String
        Dim UniqueData(1000) As String
        max = 0
        hit = 0

        ' On Header Right Click, process column

        If e.Button = Windows.Forms.MouseButtons.Right Then
            RowCount = DataGridView1.RowCount
            CSVSelectedColumn = e.ColumnIndex
            For i = 0 To RowCount - 1
                Try
                    CellData = DataGridView1.Item(CSVSelectedColumn, i).Value
                    For j = 0 To max
                        If UniqueData(j) = CellData Then
                            hit = 1
                            j = max
                            Continue For
                        End If
                    Next

                    If hit = 0 Then
                        UniqueData(max) = CellData
                        max = max + 1

                    End If
                    hit = 0
                Catch


                End Try




            Next

            ' Unique Data = unique list 
            If RadioButton1.Checked = True Then
                ModuleTextBox.Text = CSVSelectedColumn
            End If


            If RadioButton2.Checked = True Then
                SlatTextBox.Text = CSVSelectedColumn
            End If

            If RadioButton3.Checked = True Then
                PinTextBox.Text = CSVSelectedColumn
            End If


            If RadioButton4.Checked = True Then
                CueTextBox.Text = CSVSelectedColumn
            End If

            If RadioButton5.Checked = True Then
                TimeTextBox.Text = CSVSelectedColumn
            End If

            If RadioButton6.Checked = True Then
                AreaTextBox.Text = CSVSelectedColumn
            End If


            If RadioButton7.Checked = True Then
                CaliberTextBox.Text = CSVSelectedColumn
            End If

            If RadioButton8.Checked = True Then
                ItemTextBox.Text = CSVSelectedColumn
            End If





        End If

    End Sub

   
    Private Sub ProcessEffects_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click
        IsProcessEffects = True
    End Sub

    Private Sub LoadButton_Click(sender As System.Object, e As System.EventArgs) Handles LoadButton.Click

    End Sub
End Class