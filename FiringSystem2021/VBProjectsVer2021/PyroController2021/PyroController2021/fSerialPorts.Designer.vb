<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fSerialPorts
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
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.bSetPort = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ComboBox1
        '
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(28, 27)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(121, 24)
        Me.ComboBox1.TabIndex = 0
        '
        'bSetPort
        '
        Me.bSetPort.Location = New System.Drawing.Point(28, 75)
        Me.bSetPort.Name = "bSetPort"
        Me.bSetPort.Size = New System.Drawing.Size(121, 27)
        Me.bSetPort.TabIndex = 1
        Me.bSetPort.Text = "Set COMM Port"
        Me.bSetPort.UseVisualStyleBackColor = True
        '
        'fSerialPorts
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(307, 129)
        Me.Controls.Add(Me.bSetPort)
        Me.Controls.Add(Me.ComboBox1)
        Me.Name = "fSerialPorts"
        Me.Text = "SerailPortsForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents bSetPort As System.Windows.Forms.Button
End Class
