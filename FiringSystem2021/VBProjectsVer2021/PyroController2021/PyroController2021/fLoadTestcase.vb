' (C) Copyright P. Cranwell, 2014
Imports System.IO

Public Class fLoadTestcase
    Dim MyDataTable As New DataTable
    Dim filename As String = ""
    Dim CSVHeader As String

    Private Sub SaveModifiedTestCase_Click(sender As System.Object, e As System.EventArgs) Handles SaveModifiedTestCase.Click
        'Dim fname As String = OpenFileDialog1.FileName
        Dim ModuleID As String
        Dim fname As String
        fname = filename & "_1.CSV"

        Dim sw As New StreamWriter(fname)
        Dim OutLine As String
        Dim i, j As Integer

        ' Write CSV Header
        sw.WriteLine(CSVHeader)

        For i = 0 To DataGridView1.RowCount - 1
            Try
                OutLine = DataGridView1.Rows(i).Cells(0).Value.ToString
            Catch
                sw.Close()
                Exit Sub
            End Try



            ModuleID = OutLine
            If ModuleID > "002" Then
                For j = 1 To DataGridView1.ColumnCount - 1
                    OutLine = OutLine + "," + DataGridView1.Rows(i).Cells(j).Value.ToString
                Next
                j = 0
                sw.WriteLine(OutLine)
            Else
                i = DataGridView1.RowCount
            End If

        Next
        sw.Close()
    End Sub
    Private Sub ProcessTestCase_Click(sender As System.Object, e As System.EventArgs) Handles ProcessTestCase.Click
        Dim i, j As Integer
        Dim UnitIndex As Integer
        Dim SValue As String

        SIMULATE = True


        ' Build data structures for this test case

        For i = 0 To DataGridView1.Rows.Count - 1
            UnitIndex = Val(DataGridView1.Rows(i).Cells(1).Value)
            Units(UnitIndex).Session = True
            Units(UnitIndex).NumberOfPorts = Val(DataGridView1.Rows(i).Cells(2).Value)


            SValue = DataGridView1.Rows(i).Cells(3).Value
            If SValue = "Y" Or SValue = "y" Then
                Units(UnitIndex).Active = True
                Units(UnitIndex).MayBeActive = True
            Else
                Units(UnitIndex).Active = False
                Units(UnitIndex).MayBeActive = False
            End If

            SValue = DataGridView1.Rows(i).Cells(4).Value
            If SValue = "Y" Or SValue = "y" Then
                Units(UnitIndex).Armed = True
            Else
                Units(UnitIndex).Armed = False
            End If

            Units(UnitIndex).Lat = DataGridView1.Rows(i).Cells(5).Value
            Units(UnitIndex).Lon = DataGridView1.Rows(i).Cells(6).Value
            Units(UnitIndex).Voltage = DataGridView1.Rows(i).Cells(7).Value
            Units(UnitIndex).PinStatus = DataGridView1.Rows(i).Cells(8).Value



            ' Ports is indexed based on 1,
            ' PinStatus is indexed based on 0
            ' Microsoft should standardize this BS

            For j = 1 To Units(UnitIndex).NumberOfPorts
                Units(UnitIndex).Ports(j) = New cPorts()

                Units(UnitIndex).Ports(j).Port.Active = True
                If (Units(UnitIndex).PinStatus.Substring(j - 1, 1)) = "0" Then
                    ' Set to appropriate value based on pin string
                    Units(UnitIndex).Ports(j).Port.eMatch = True
                Else
                    Units(UnitIndex).Ports(j).Port.eMatch = False
                End If
            Next
        Next
        Me.Hide()
    End Sub
    Private Sub LoadTestcaseForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Show test cases in folder.

        Dim name As String = ""
        Dim myStream As Stream = Nothing
        Dim openFileDialog1 As New OpenFileDialog()
        Dim splits As String()


        ' Display Test Case CSV Files and allow User to select file

        openFileDialog1.InitialDirectory = "c:\PyroController"
        openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True


        ' Read CSV into 
        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Try
                name = openFileDialog1.FileName
                filename = name
                filename = filename.Substring(0, Len(filename) - 4)
                Dim sr As New StreamReader(name)
                CSVHeader = sr.ReadLine()
                sr.Close()
                sr.Dispose()

                Dim sr2 As New StreamReader(name)


                If (sr2 IsNot Nothing) Then

                    Try
                        Using sr2 'As StreamReader '= New StreamReader(filename)
                            'read the first line for the table column headers
                            splits = sr2.ReadLine.Split(","c)

                            For i As Integer = 0 To UBound(splits)
                                ' add to data table
                                MyDataTable.Columns.Add(splits(i))
                            Next
                            'read the rest of the lines to add rows to the table
                            Do While Not sr2.EndOfStream
                                splits = sr2.ReadLine.Split(","c)

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
                        MsgBox("No CSV File Selected")
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
                    sr.Close()
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

End Class