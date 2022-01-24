<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fSelectShow
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
        Me.components = New System.ComponentModel.Container()
        Me.ShowDatabaseDataSet = New PyroController2014.ShowDatabaseDataSet()
        Me.ShowBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ShowTableAdapter = New PyroController2014.ShowDatabaseDataSetTableAdapters.ShowTableAdapter()
        Me.ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShowIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShowNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DescriptionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bUpdate = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShowIDDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShowNameDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DescriptionDataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShowTypeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.ShowDatabaseDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ShowBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ShowDatabaseDataSet
        '
        Me.ShowDatabaseDataSet.DataSetName = "ShowDatabaseDataSet"
        Me.ShowDatabaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ShowBindingSource
        '
        Me.ShowBindingSource.DataMember = "Show"
        Me.ShowBindingSource.DataSource = Me.ShowDatabaseDataSet
        '
        'ShowTableAdapter
        '
        Me.ShowTableAdapter.ClearBeforeFill = True
        '
        'ID
        '
        Me.ID.DataPropertyName = "ID"
        Me.ID.HeaderText = "ID"
        Me.ID.Name = "ID"
        '
        'ShowIDDataGridViewTextBoxColumn
        '
        Me.ShowIDDataGridViewTextBoxColumn.DataPropertyName = "ShowID"
        Me.ShowIDDataGridViewTextBoxColumn.HeaderText = "ShowID"
        Me.ShowIDDataGridViewTextBoxColumn.Name = "ShowIDDataGridViewTextBoxColumn"
        '
        'ShowNameDataGridViewTextBoxColumn
        '
        Me.ShowNameDataGridViewTextBoxColumn.DataPropertyName = "ShowName"
        Me.ShowNameDataGridViewTextBoxColumn.HeaderText = "ShowName"
        Me.ShowNameDataGridViewTextBoxColumn.Name = "ShowNameDataGridViewTextBoxColumn"
        '
        'DescriptionDataGridViewTextBoxColumn
        '
        Me.DescriptionDataGridViewTextBoxColumn.DataPropertyName = "Description"
        Me.DescriptionDataGridViewTextBoxColumn.HeaderText = "Description"
        Me.DescriptionDataGridViewTextBoxColumn.Name = "DescriptionDataGridViewTextBoxColumn"
        '
        'bUpdate
        '
        Me.bUpdate.Location = New System.Drawing.Point(82, 189)
        Me.bUpdate.Name = "bUpdate"
        Me.bUpdate.Size = New System.Drawing.Size(107, 27)
        Me.bUpdate.TabIndex = 1
        Me.bUpdate.Text = "Update"
        Me.bUpdate.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDDataGridViewTextBoxColumn, Me.ShowIDDataGridViewTextBoxColumn1, Me.ShowNameDataGridViewTextBoxColumn1, Me.DescriptionDataGridViewTextBoxColumn1, Me.ShowTypeDataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.ShowBindingSource
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(556, 150)
        Me.DataGridView1.TabIndex = 2
        '
        'IDDataGridViewTextBoxColumn
        '
        Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
        Me.IDDataGridViewTextBoxColumn.HeaderText = "ID"
        Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
        '
        'ShowIDDataGridViewTextBoxColumn1
        '
        Me.ShowIDDataGridViewTextBoxColumn1.DataPropertyName = "ShowID"
        Me.ShowIDDataGridViewTextBoxColumn1.HeaderText = "ShowID"
        Me.ShowIDDataGridViewTextBoxColumn1.Name = "ShowIDDataGridViewTextBoxColumn1"
        '
        'ShowNameDataGridViewTextBoxColumn1
        '
        Me.ShowNameDataGridViewTextBoxColumn1.DataPropertyName = "ShowName"
        Me.ShowNameDataGridViewTextBoxColumn1.HeaderText = "ShowName"
        Me.ShowNameDataGridViewTextBoxColumn1.Name = "ShowNameDataGridViewTextBoxColumn1"
        '
        'DescriptionDataGridViewTextBoxColumn1
        '
        Me.DescriptionDataGridViewTextBoxColumn1.DataPropertyName = "Description"
        Me.DescriptionDataGridViewTextBoxColumn1.HeaderText = "Description"
        Me.DescriptionDataGridViewTextBoxColumn1.Name = "DescriptionDataGridViewTextBoxColumn1"
        '
        'ShowTypeDataGridViewTextBoxColumn
        '
        Me.ShowTypeDataGridViewTextBoxColumn.DataPropertyName = "Show Type"
        Me.ShowTypeDataGridViewTextBoxColumn.HeaderText = "Show Type"
        Me.ShowTypeDataGridViewTextBoxColumn.Name = "ShowTypeDataGridViewTextBoxColumn"
        '
        'fSelectShow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(623, 240)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.bUpdate)
        Me.Name = "fSelectShow"
        Me.Text = "SelectShowForm"
        CType(Me.ShowDatabaseDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ShowBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ShowDatabaseDataSet As PyroController2014.ShowDatabaseDataSet
    Friend WithEvents ShowBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents ShowTableAdapter As PyroController2014.ShowDatabaseDataSetTableAdapters.ShowTableAdapter
    Friend WithEvents ID As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShowIDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShowNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DescriptionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bUpdate As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShowIDDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShowNameDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DescriptionDataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShowTypeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
