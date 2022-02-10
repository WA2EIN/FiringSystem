<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fShow
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            fClock.Close()
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
        Me.components = New System.ComponentModel.Container()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Arm = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.SeqDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShowIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ModuleDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PortNumberDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CueDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TimeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SeqIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NextSeqDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ItemNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.EffectDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BankNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BankNumberDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SlatIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SlatNumberDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AreaNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.AreaNumberDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CaliberNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CaliberNumericDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ItemNumberDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShowEventsBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ShowDatabaseDataSet = New PyroController2014.ShowDatabaseDataSet()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.ShowEventsTableAdapter = New PyroController2014.ShowDatabaseDataSetTableAdapters.ShowEventsTableAdapter()
        Me.UpdateDatabase = New System.Windows.Forms.Button()
        Me.ProgModules = New System.Windows.Forms.Button()
        Me.SaveShow = New System.Windows.Forms.Button()
        Me.SetDefaultShow = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ActivateModules = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PortsViewButton = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button1 = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ShowEventsBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ShowDatabaseDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(51, 10)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(30, 20)
        Me.TextBox1.TabIndex = 0
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(96, 10)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(235, 20)
        Me.TextBox2.TabIndex = 1
        '
        'Arm
        '
        Me.Arm.Location = New System.Drawing.Point(354, 4)
        Me.Arm.Margin = New System.Windows.Forms.Padding(2)
        Me.Arm.Name = "Arm"
        Me.Arm.Size = New System.Drawing.Size(56, 31)
        Me.Arm.TabIndex = 2
        Me.Arm.Text = "Arm"
        Me.Arm.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToOrderColumns = True
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.SeqDataGridViewTextBoxColumn, Me.ShowIDDataGridViewTextBoxColumn, Me.ModuleDataGridViewTextBoxColumn, Me.PortNumberDataGridViewTextBoxColumn, Me.CueDataGridViewTextBoxColumn, Me.TimeDataGridViewTextBoxColumn, Me.SeqIDDataGridViewTextBoxColumn, Me.NextSeqDataGridViewTextBoxColumn, Me.ItemNameDataGridViewTextBoxColumn, Me.EffectDataGridViewTextBoxColumn, Me.BankNameDataGridViewTextBoxColumn, Me.BankNumberDataGridViewTextBoxColumn, Me.SlatIDDataGridViewTextBoxColumn, Me.SlatNumberDataGridViewTextBoxColumn, Me.AreaNameDataGridViewTextBoxColumn, Me.AreaNumberDataGridViewTextBoxColumn, Me.CaliberNameDataGridViewTextBoxColumn, Me.CaliberNumericDataGridViewTextBoxColumn, Me.ItemNumberDataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.ShowEventsBindingSource
        Me.DataGridView1.Location = New System.Drawing.Point(9, 40)
        Me.DataGridView1.Margin = New System.Windows.Forms.Padding(2)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(874, 426)
        Me.DataGridView1.TabIndex = 4
        '
        'SeqDataGridViewTextBoxColumn
        '
        Me.SeqDataGridViewTextBoxColumn.DataPropertyName = "Seq"
        Me.SeqDataGridViewTextBoxColumn.HeaderText = "Seq"
        Me.SeqDataGridViewTextBoxColumn.Name = "SeqDataGridViewTextBoxColumn"
        '
        'ShowIDDataGridViewTextBoxColumn
        '
        Me.ShowIDDataGridViewTextBoxColumn.DataPropertyName = "ShowID"
        Me.ShowIDDataGridViewTextBoxColumn.HeaderText = "ShowID"
        Me.ShowIDDataGridViewTextBoxColumn.Name = "ShowIDDataGridViewTextBoxColumn"
        '
        'ModuleDataGridViewTextBoxColumn
        '
        Me.ModuleDataGridViewTextBoxColumn.DataPropertyName = "Module"
        Me.ModuleDataGridViewTextBoxColumn.HeaderText = "Module"
        Me.ModuleDataGridViewTextBoxColumn.Name = "ModuleDataGridViewTextBoxColumn"
        '
        'PortNumberDataGridViewTextBoxColumn
        '
        Me.PortNumberDataGridViewTextBoxColumn.DataPropertyName = "PortNumber"
        Me.PortNumberDataGridViewTextBoxColumn.HeaderText = "PortNumber"
        Me.PortNumberDataGridViewTextBoxColumn.Name = "PortNumberDataGridViewTextBoxColumn"
        '
        'CueDataGridViewTextBoxColumn
        '
        Me.CueDataGridViewTextBoxColumn.DataPropertyName = "Cue"
        Me.CueDataGridViewTextBoxColumn.HeaderText = "Cue"
        Me.CueDataGridViewTextBoxColumn.Name = "CueDataGridViewTextBoxColumn"
        '
        'TimeDataGridViewTextBoxColumn
        '
        Me.TimeDataGridViewTextBoxColumn.DataPropertyName = "Time"
        Me.TimeDataGridViewTextBoxColumn.HeaderText = "Time"
        Me.TimeDataGridViewTextBoxColumn.Name = "TimeDataGridViewTextBoxColumn"
        '
        'SeqIDDataGridViewTextBoxColumn
        '
        Me.SeqIDDataGridViewTextBoxColumn.DataPropertyName = "Seq ID"
        Me.SeqIDDataGridViewTextBoxColumn.HeaderText = "Seq ID"
        Me.SeqIDDataGridViewTextBoxColumn.Name = "SeqIDDataGridViewTextBoxColumn"
        '
        'NextSeqDataGridViewTextBoxColumn
        '
        Me.NextSeqDataGridViewTextBoxColumn.DataPropertyName = "Next Seq"
        Me.NextSeqDataGridViewTextBoxColumn.HeaderText = "Next Seq"
        Me.NextSeqDataGridViewTextBoxColumn.Name = "NextSeqDataGridViewTextBoxColumn"
        '
        'ItemNameDataGridViewTextBoxColumn
        '
        Me.ItemNameDataGridViewTextBoxColumn.DataPropertyName = "ItemName"
        Me.ItemNameDataGridViewTextBoxColumn.HeaderText = "ItemName"
        Me.ItemNameDataGridViewTextBoxColumn.Name = "ItemNameDataGridViewTextBoxColumn"
        '
        'EffectDataGridViewTextBoxColumn
        '
        Me.EffectDataGridViewTextBoxColumn.DataPropertyName = "Effect"
        Me.EffectDataGridViewTextBoxColumn.HeaderText = "Effect"
        Me.EffectDataGridViewTextBoxColumn.Name = "EffectDataGridViewTextBoxColumn"
        '
        'BankNameDataGridViewTextBoxColumn
        '
        Me.BankNameDataGridViewTextBoxColumn.DataPropertyName = "BankName"
        Me.BankNameDataGridViewTextBoxColumn.HeaderText = "BankName"
        Me.BankNameDataGridViewTextBoxColumn.Name = "BankNameDataGridViewTextBoxColumn"
        '
        'BankNumberDataGridViewTextBoxColumn
        '
        Me.BankNumberDataGridViewTextBoxColumn.DataPropertyName = "BankNumber"
        Me.BankNumberDataGridViewTextBoxColumn.HeaderText = "BankNumber"
        Me.BankNumberDataGridViewTextBoxColumn.Name = "BankNumberDataGridViewTextBoxColumn"
        '
        'SlatIDDataGridViewTextBoxColumn
        '
        Me.SlatIDDataGridViewTextBoxColumn.DataPropertyName = "SlatID"
        Me.SlatIDDataGridViewTextBoxColumn.HeaderText = "SlatID"
        Me.SlatIDDataGridViewTextBoxColumn.Name = "SlatIDDataGridViewTextBoxColumn"
        '
        'SlatNumberDataGridViewTextBoxColumn
        '
        Me.SlatNumberDataGridViewTextBoxColumn.DataPropertyName = "SlatNumber"
        Me.SlatNumberDataGridViewTextBoxColumn.HeaderText = "SlatNumber"
        Me.SlatNumberDataGridViewTextBoxColumn.Name = "SlatNumberDataGridViewTextBoxColumn"
        '
        'AreaNameDataGridViewTextBoxColumn
        '
        Me.AreaNameDataGridViewTextBoxColumn.DataPropertyName = "AreaName"
        Me.AreaNameDataGridViewTextBoxColumn.HeaderText = "AreaName"
        Me.AreaNameDataGridViewTextBoxColumn.Name = "AreaNameDataGridViewTextBoxColumn"
        '
        'AreaNumberDataGridViewTextBoxColumn
        '
        Me.AreaNumberDataGridViewTextBoxColumn.DataPropertyName = "AreaNumber"
        Me.AreaNumberDataGridViewTextBoxColumn.HeaderText = "AreaNumber"
        Me.AreaNumberDataGridViewTextBoxColumn.Name = "AreaNumberDataGridViewTextBoxColumn"
        '
        'CaliberNameDataGridViewTextBoxColumn
        '
        Me.CaliberNameDataGridViewTextBoxColumn.DataPropertyName = "CaliberName"
        Me.CaliberNameDataGridViewTextBoxColumn.HeaderText = "CaliberName"
        Me.CaliberNameDataGridViewTextBoxColumn.Name = "CaliberNameDataGridViewTextBoxColumn"
        '
        'CaliberNumericDataGridViewTextBoxColumn
        '
        Me.CaliberNumericDataGridViewTextBoxColumn.DataPropertyName = "CaliberNumeric"
        Me.CaliberNumericDataGridViewTextBoxColumn.HeaderText = "CaliberNumeric"
        Me.CaliberNumericDataGridViewTextBoxColumn.Name = "CaliberNumericDataGridViewTextBoxColumn"
        '
        'ItemNumberDataGridViewTextBoxColumn
        '
        Me.ItemNumberDataGridViewTextBoxColumn.DataPropertyName = "ItemNumber"
        Me.ItemNumberDataGridViewTextBoxColumn.HeaderText = "ItemNumber"
        Me.ItemNumberDataGridViewTextBoxColumn.Name = "ItemNumberDataGridViewTextBoxColumn"
        '
        'ShowEventsBindingSource
        '
        Me.ShowEventsBindingSource.DataMember = "ShowEvents"
        Me.ShowEventsBindingSource.DataSource = Me.ShowDatabaseDataSet
        '
        'ShowDatabaseDataSet
        '
        Me.ShowDatabaseDataSet.DataSetName = "ShowDatabaseDataSet"
        Me.ShowDatabaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(901, 63)
        Me.ComboBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(56, 21)
        Me.ComboBox1.TabIndex = 5
        '
        'ShowEventsTableAdapter
        '
        Me.ShowEventsTableAdapter.ClearBeforeFill = True
        '
        'UpdateDatabase
        '
        Me.UpdateDatabase.Location = New System.Drawing.Point(9, 469)
        Me.UpdateDatabase.Margin = New System.Windows.Forms.Padding(2)
        Me.UpdateDatabase.Name = "UpdateDatabase"
        Me.UpdateDatabase.Size = New System.Drawing.Size(56, 19)
        Me.UpdateDatabase.TabIndex = 6
        Me.UpdateDatabase.Text = "Update"
        Me.UpdateDatabase.UseVisualStyleBackColor = True
        '
        'ProgModules
        '
        Me.ProgModules.Location = New System.Drawing.Point(899, 95)
        Me.ProgModules.Margin = New System.Windows.Forms.Padding(2)
        Me.ProgModules.Name = "ProgModules"
        Me.ProgModules.Size = New System.Drawing.Size(89, 34)
        Me.ProgModules.TabIndex = 7
        Me.ProgModules.Text = "Program  Modules"
        Me.ProgModules.UseVisualStyleBackColor = True
        '
        'SaveShow
        '
        Me.SaveShow.Location = New System.Drawing.Point(899, 134)
        Me.SaveShow.Margin = New System.Windows.Forms.Padding(2)
        Me.SaveShow.Name = "SaveShow"
        Me.SaveShow.Size = New System.Drawing.Size(89, 38)
        Me.SaveShow.TabIndex = 8
        Me.SaveShow.Text = "Save Show in Modules"
        Me.SaveShow.UseVisualStyleBackColor = True
        Me.SaveShow.Visible = False
        '
        'SetDefaultShow
        '
        Me.SetDefaultShow.Location = New System.Drawing.Point(899, 177)
        Me.SetDefaultShow.Margin = New System.Windows.Forms.Padding(2)
        Me.SetDefaultShow.Name = "SetDefaultShow"
        Me.SetDefaultShow.Size = New System.Drawing.Size(89, 53)
        Me.SetDefaultShow.TabIndex = 9
        Me.SetDefaultShow.Text = "Set Default Show in Modules" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.SetDefaultShow.UseVisualStyleBackColor = True
        Me.SetDefaultShow.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(900, 37)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Show #"
        '
        'ActivateModules
        '
        Me.ActivateModules.Location = New System.Drawing.Point(899, 235)
        Me.ActivateModules.Margin = New System.Windows.Forms.Padding(2)
        Me.ActivateModules.Name = "ActivateModules"
        Me.ActivateModules.Size = New System.Drawing.Size(89, 53)
        Me.ActivateModules.TabIndex = 13
        Me.ActivateModules.Text = "Activate Modules Stored Show"
        Me.ActivateModules.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 12)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Show"
        '
        'PortsViewButton
        '
        Me.PortsViewButton.Location = New System.Drawing.Point(424, 4)
        Me.PortsViewButton.Margin = New System.Windows.Forms.Padding(2)
        Me.PortsViewButton.Name = "PortsViewButton"
        Me.PortsViewButton.Size = New System.Drawing.Size(77, 29)
        Me.PortsViewButton.TabIndex = 15
        Me.PortsViewButton.Text = "Ports View"
        Me.PortsViewButton.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(518, 2)
        Me.Button1.Margin = New System.Windows.Forms.Padding(2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(74, 31)
        Me.Button1.TabIndex = 16
        Me.Button1.Text = "Clock Toggle"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'fShow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(1070, 497)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.PortsViewButton)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ActivateModules)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SetDefaultShow)
        Me.Controls.Add(Me.SaveShow)
        Me.Controls.Add(Me.ProgModules)
        Me.Controls.Add(Me.UpdateDatabase)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Arm)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "fShow"
        Me.Text = "ShowForm"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ShowEventsBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ShowDatabaseDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Arm As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ShowDatabaseDataSet As PyroController2014.ShowDatabaseDataSet
    Friend WithEvents ShowEventsBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents ShowEventsTableAdapter As PyroController2014.ShowDatabaseDataSetTableAdapters.ShowEventsTableAdapter
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents UpdateDatabase As System.Windows.Forms.Button
    Friend WithEvents ProgModules As System.Windows.Forms.Button
    Friend WithEvents SaveShow As System.Windows.Forms.Button
    Friend WithEvents SetDefaultShow As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ActivateModules As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PortsViewButton As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents SeqDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShowIDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ModuleDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PortNumberDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CueDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TimeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SeqIDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NextSeqDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ItemNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EffectDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BankNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BankNumberDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SlatIDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SlatNumberDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AreaNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AreaNumberDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CaliberNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CaliberNumericDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ItemNumberDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
