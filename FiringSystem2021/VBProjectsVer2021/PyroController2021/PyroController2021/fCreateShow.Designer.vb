<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fCreateShow
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ShowBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.ShowDatabaseDataSet1 = New PyroController2014.ShowDatabaseDataSet1()
        Me.ShowTableAdapter = New PyroController2014.ShowDatabaseDataSet1TableAdapters.ShowTableAdapter()
        Me.IDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShowIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShowNameDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DescriptionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ShowTypeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ShowBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ShowDatabaseDataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(32, 294)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(106, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Create Show"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.IDDataGridViewTextBoxColumn, Me.ShowIDDataGridViewTextBoxColumn, Me.ShowNameDataGridViewTextBoxColumn, Me.DescriptionDataGridViewTextBoxColumn, Me.ShowTypeDataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.ShowBindingSource
        Me.DataGridView1.Location = New System.Drawing.Point(12, 24)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(471, 150)
        Me.DataGridView1.TabIndex = 5
        '
        'ShowBindingSource
        '
        Me.ShowBindingSource.DataMember = "Show"
        Me.ShowBindingSource.DataSource = Me.ShowDatabaseDataSet1
        '
        'ShowDatabaseDataSet1
        '
        Me.ShowDatabaseDataSet1.DataSetName = "ShowDatabaseDataSet1"
        Me.ShowDatabaseDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'ShowTableAdapter
        '
        Me.ShowTableAdapter.ClearBeforeFill = True
        '
        'IDDataGridViewTextBoxColumn
        '
        Me.IDDataGridViewTextBoxColumn.DataPropertyName = "ID"
        Me.IDDataGridViewTextBoxColumn.HeaderText = "ID"
        Me.IDDataGridViewTextBoxColumn.Name = "IDDataGridViewTextBoxColumn"
        Me.IDDataGridViewTextBoxColumn.Visible = False
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
        Me.DescriptionDataGridViewTextBoxColumn.Visible = False
        '
        'ShowTypeDataGridViewTextBoxColumn
        '
        Me.ShowTypeDataGridViewTextBoxColumn.DataPropertyName = "Show Type"
        Me.ShowTypeDataGridViewTextBoxColumn.HeaderText = "Show Type"
        Me.ShowTypeDataGridViewTextBoxColumn.Name = "ShowTypeDataGridViewTextBoxColumn"
        Me.ShowTypeDataGridViewTextBoxColumn.Visible = False
        '
        'fCreateShow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(866, 560)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Button1)
        Me.Name = "fCreateShow"
        Me.Text = "fNewShow"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ShowBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ShowDatabaseDataSet1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents ShowDatabaseDataSet1 As PyroController2014.ShowDatabaseDataSet1
    Friend WithEvents ShowBindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents ShowTableAdapter As PyroController2014.ShowDatabaseDataSet1TableAdapters.ShowTableAdapter
    Friend WithEvents IDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShowIDDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShowNameDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DescriptionDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ShowTypeDataGridViewTextBoxColumn As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
