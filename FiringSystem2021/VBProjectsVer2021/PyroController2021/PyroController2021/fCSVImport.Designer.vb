<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fCSVImport
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ModuleTextBox = New System.Windows.Forms.TextBox()
        Me.SlatTextBox = New System.Windows.Forms.TextBox()
        Me.PinTextBox = New System.Windows.Forms.TextBox()
        Me.CueTextBox = New System.Windows.Forms.TextBox()
        Me.TimeTextBox = New System.Windows.Forms.TextBox()
        Me.AreaTextBox = New System.Windows.Forms.TextBox()
        Me.CaliberTextBox = New System.Windows.Forms.TextBox()
        Me.ItemTextBox = New System.Windows.Forms.TextBox()
        Me.RadioButton1 = New System.Windows.Forms.RadioButton()
        Me.RadioButton2 = New System.Windows.Forms.RadioButton()
        Me.RadioButton3 = New System.Windows.Forms.RadioButton()
        Me.RadioButton4 = New System.Windows.Forms.RadioButton()
        Me.RadioButton5 = New System.Windows.Forms.RadioButton()
        Me.RadioButton6 = New System.Windows.Forms.RadioButton()
        Me.RadioButton7 = New System.Windows.Forms.RadioButton()
        Me.RadioButton8 = New System.Windows.Forms.RadioButton()
        Me.TimeMult = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ProcessData = New System.Windows.Forms.Button()
        Me.LoadCSV = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Save = New System.Windows.Forms.Button()
        Me.LoadButton = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(19, 43)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(508, 286)
        Me.DataGridView1.TabIndex = 0
        '
        'ModuleTextBox
        '
        Me.ModuleTextBox.Location = New System.Drawing.Point(599, 43)
        Me.ModuleTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.ModuleTextBox.Name = "ModuleTextBox"
        Me.ModuleTextBox.Size = New System.Drawing.Size(69, 20)
        Me.ModuleTextBox.TabIndex = 1
        '
        'SlatTextBox
        '
        Me.SlatTextBox.Location = New System.Drawing.Point(599, 73)
        Me.SlatTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.SlatTextBox.Name = "SlatTextBox"
        Me.SlatTextBox.Size = New System.Drawing.Size(69, 20)
        Me.SlatTextBox.TabIndex = 2
        '
        'PinTextBox
        '
        Me.PinTextBox.Location = New System.Drawing.Point(599, 105)
        Me.PinTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.PinTextBox.Name = "PinTextBox"
        Me.PinTextBox.Size = New System.Drawing.Size(69, 20)
        Me.PinTextBox.TabIndex = 3
        '
        'CueTextBox
        '
        Me.CueTextBox.Location = New System.Drawing.Point(599, 143)
        Me.CueTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.CueTextBox.Name = "CueTextBox"
        Me.CueTextBox.Size = New System.Drawing.Size(69, 20)
        Me.CueTextBox.TabIndex = 4
        '
        'TimeTextBox
        '
        Me.TimeTextBox.Location = New System.Drawing.Point(599, 173)
        Me.TimeTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TimeTextBox.Name = "TimeTextBox"
        Me.TimeTextBox.Size = New System.Drawing.Size(69, 20)
        Me.TimeTextBox.TabIndex = 5
        '
        'AreaTextBox
        '
        Me.AreaTextBox.Location = New System.Drawing.Point(599, 234)
        Me.AreaTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.AreaTextBox.Name = "AreaTextBox"
        Me.AreaTextBox.Size = New System.Drawing.Size(69, 20)
        Me.AreaTextBox.TabIndex = 6
        '
        'CaliberTextBox
        '
        Me.CaliberTextBox.Location = New System.Drawing.Point(599, 266)
        Me.CaliberTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.CaliberTextBox.Name = "CaliberTextBox"
        Me.CaliberTextBox.Size = New System.Drawing.Size(69, 20)
        Me.CaliberTextBox.TabIndex = 7
        '
        'ItemTextBox
        '
        Me.ItemTextBox.Location = New System.Drawing.Point(599, 298)
        Me.ItemTextBox.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.ItemTextBox.Name = "ItemTextBox"
        Me.ItemTextBox.Size = New System.Drawing.Size(69, 20)
        Me.ItemTextBox.TabIndex = 8
        '
        'RadioButton1
        '
        Me.RadioButton1.AutoSize = True
        Me.RadioButton1.Location = New System.Drawing.Point(538, 44)
        Me.RadioButton1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.RadioButton1.Name = "RadioButton1"
        Me.RadioButton1.Size = New System.Drawing.Size(60, 17)
        Me.RadioButton1.TabIndex = 9
        Me.RadioButton1.TabStop = True
        Me.RadioButton1.Text = "Module"
        Me.RadioButton1.UseVisualStyleBackColor = True
        '
        'RadioButton2
        '
        Me.RadioButton2.AutoSize = True
        Me.RadioButton2.Location = New System.Drawing.Point(538, 74)
        Me.RadioButton2.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.RadioButton2.Name = "RadioButton2"
        Me.RadioButton2.Size = New System.Drawing.Size(43, 17)
        Me.RadioButton2.TabIndex = 10
        Me.RadioButton2.TabStop = True
        Me.RadioButton2.Text = "Slat"
        Me.RadioButton2.UseVisualStyleBackColor = True
        '
        'RadioButton3
        '
        Me.RadioButton3.AutoSize = True
        Me.RadioButton3.Location = New System.Drawing.Point(538, 105)
        Me.RadioButton3.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.RadioButton3.Name = "RadioButton3"
        Me.RadioButton3.Size = New System.Drawing.Size(40, 17)
        Me.RadioButton3.TabIndex = 11
        Me.RadioButton3.TabStop = True
        Me.RadioButton3.Text = "Pin"
        Me.RadioButton3.UseVisualStyleBackColor = True
        '
        'RadioButton4
        '
        Me.RadioButton4.AutoSize = True
        Me.RadioButton4.Location = New System.Drawing.Point(538, 143)
        Me.RadioButton4.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.RadioButton4.Name = "RadioButton4"
        Me.RadioButton4.Size = New System.Drawing.Size(44, 17)
        Me.RadioButton4.TabIndex = 12
        Me.RadioButton4.TabStop = True
        Me.RadioButton4.Text = "Cue"
        Me.RadioButton4.UseVisualStyleBackColor = True
        '
        'RadioButton5
        '
        Me.RadioButton5.AutoSize = True
        Me.RadioButton5.Location = New System.Drawing.Point(538, 174)
        Me.RadioButton5.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.RadioButton5.Name = "RadioButton5"
        Me.RadioButton5.Size = New System.Drawing.Size(48, 17)
        Me.RadioButton5.TabIndex = 13
        Me.RadioButton5.TabStop = True
        Me.RadioButton5.Text = "Time"
        Me.RadioButton5.UseVisualStyleBackColor = True
        '
        'RadioButton6
        '
        Me.RadioButton6.AutoSize = True
        Me.RadioButton6.Location = New System.Drawing.Point(538, 234)
        Me.RadioButton6.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.RadioButton6.Name = "RadioButton6"
        Me.RadioButton6.Size = New System.Drawing.Size(47, 17)
        Me.RadioButton6.TabIndex = 14
        Me.RadioButton6.TabStop = True
        Me.RadioButton6.Text = "Area"
        Me.RadioButton6.UseVisualStyleBackColor = True
        '
        'RadioButton7
        '
        Me.RadioButton7.AutoSize = True
        Me.RadioButton7.Location = New System.Drawing.Point(538, 266)
        Me.RadioButton7.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.RadioButton7.Name = "RadioButton7"
        Me.RadioButton7.Size = New System.Drawing.Size(57, 17)
        Me.RadioButton7.TabIndex = 15
        Me.RadioButton7.TabStop = True
        Me.RadioButton7.Text = "Caliber"
        Me.RadioButton7.UseVisualStyleBackColor = True
        '
        'RadioButton8
        '
        Me.RadioButton8.AutoSize = True
        Me.RadioButton8.Location = New System.Drawing.Point(538, 298)
        Me.RadioButton8.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.RadioButton8.Name = "RadioButton8"
        Me.RadioButton8.Size = New System.Drawing.Size(45, 17)
        Me.RadioButton8.TabIndex = 16
        Me.RadioButton8.TabStop = True
        Me.RadioButton8.Text = "Item"
        Me.RadioButton8.UseVisualStyleBackColor = True
        '
        'TimeMult
        '
        Me.TimeMult.Location = New System.Drawing.Point(599, 205)
        Me.TimeMult.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TimeMult.Name = "TimeMult"
        Me.TimeMult.Size = New System.Drawing.Size(69, 20)
        Me.TimeMult.TabIndex = 17
        Me.TimeMult.Text = "1000"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(536, 207)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Time Mult"
        '
        'ProcessData
        '
        Me.ProcessData.Location = New System.Drawing.Point(624, 332)
        Me.ProcessData.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.ProcessData.Name = "ProcessData"
        Me.ProcessData.Size = New System.Drawing.Size(68, 36)
        Me.ProcessData.TabIndex = 19
        Me.ProcessData.Text = "Load Database"
        Me.ProcessData.UseVisualStyleBackColor = True
        '
        'LoadCSV
        '
        Me.LoadCSV.Location = New System.Drawing.Point(19, 10)
        Me.LoadCSV.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.LoadCSV.Name = "LoadCSV"
        Me.LoadCSV.Size = New System.Drawing.Size(56, 19)
        Me.LoadCSV.TabIndex = 20
        Me.LoadCSV.Text = "Select CSV"
        Me.LoadCSV.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Save
        '
        Me.Save.Location = New System.Drawing.Point(88, 10)
        Me.Save.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Save.Name = "Save"
        Me.Save.Size = New System.Drawing.Size(49, 19)
        Me.Save.TabIndex = 21
        Me.Save.Text = "Save"
        Me.Save.UseVisualStyleBackColor = True
        '
        'LoadButton
        '
        Me.LoadButton.Location = New System.Drawing.Point(148, 10)
        Me.LoadButton.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.LoadButton.Name = "LoadButton"
        Me.LoadButton.Size = New System.Drawing.Size(49, 19)
        Me.LoadButton.TabIndex = 22
        Me.LoadButton.Text = "Load"
        Me.LoadButton.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(538, 333)
        Me.Button1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(58, 34)
        Me.Button1.TabIndex = 23
        Me.Button1.Text = "Load Effects"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'fCSVImport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(701, 410)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.LoadButton)
        Me.Controls.Add(Me.Save)
        Me.Controls.Add(Me.LoadCSV)
        Me.Controls.Add(Me.ProcessData)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TimeMult)
        Me.Controls.Add(Me.RadioButton8)
        Me.Controls.Add(Me.RadioButton7)
        Me.Controls.Add(Me.RadioButton6)
        Me.Controls.Add(Me.RadioButton5)
        Me.Controls.Add(Me.RadioButton4)
        Me.Controls.Add(Me.RadioButton3)
        Me.Controls.Add(Me.RadioButton2)
        Me.Controls.Add(Me.RadioButton1)
        Me.Controls.Add(Me.ItemTextBox)
        Me.Controls.Add(Me.CaliberTextBox)
        Me.Controls.Add(Me.AreaTextBox)
        Me.Controls.Add(Me.TimeTextBox)
        Me.Controls.Add(Me.CueTextBox)
        Me.Controls.Add(Me.PinTextBox)
        Me.Controls.Add(Me.SlatTextBox)
        Me.Controls.Add(Me.ModuleTextBox)
        Me.Controls.Add(Me.DataGridView1)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "fCSVImport"
        Me.Text = "LoadCSV"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ModuleTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SlatTextBox As System.Windows.Forms.TextBox
    Friend WithEvents PinTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CueTextBox As System.Windows.Forms.TextBox
    Friend WithEvents TimeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents AreaTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CaliberTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ItemTextBox As System.Windows.Forms.TextBox
    Friend WithEvents RadioButton1 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton2 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton3 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton4 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton5 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton6 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton7 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton8 As System.Windows.Forms.RadioButton
    Friend WithEvents TimeMult As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ProcessData As System.Windows.Forms.Button
    Friend WithEvents LoadCSV As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Save As System.Windows.Forms.Button
    Friend WithEvents LoadButton As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
