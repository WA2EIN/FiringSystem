﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fLoadTestcase
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
        Me.SaveModifiedTestCase = New System.Windows.Forms.Button()
        Me.ProcessTestCase = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SaveModifiedTestCase
        '
        Me.SaveModifiedTestCase.Location = New System.Drawing.Point(12, 12)
        Me.SaveModifiedTestCase.Name = "SaveModifiedTestCase"
        Me.SaveModifiedTestCase.Size = New System.Drawing.Size(178, 23)
        Me.SaveModifiedTestCase.TabIndex = 1
        Me.SaveModifiedTestCase.Text = "Save Modified Test Case"
        Me.SaveModifiedTestCase.UseVisualStyleBackColor = True
        '
        'ProcessTestCase
        '
        Me.ProcessTestCase.Location = New System.Drawing.Point(217, 12)
        Me.ProcessTestCase.Name = "ProcessTestCase"
        Me.ProcessTestCase.Size = New System.Drawing.Size(161, 23)
        Me.ProcessTestCase.TabIndex = 2
        Me.ProcessTestCase.Text = "Process Test Case"
        Me.ProcessTestCase.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(404, 12)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 22)
        Me.TextBox1.TabIndex = 3
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Location = New System.Drawing.Point(12, 55)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 24
        Me.DataGridView1.Size = New System.Drawing.Size(1085, 311)
        Me.DataGridView1.TabIndex = 4
        '
        'LoadTestcaseForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1109, 391)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.ProcessTestCase)
        Me.Controls.Add(Me.SaveModifiedTestCase)
        Me.Name = "LoadTestcaseForm"
        Me.Text = "LoadTestcaseForm"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SaveModifiedTestCase As System.Windows.Forms.Button
    Friend WithEvents ProcessTestCase As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
End Class
