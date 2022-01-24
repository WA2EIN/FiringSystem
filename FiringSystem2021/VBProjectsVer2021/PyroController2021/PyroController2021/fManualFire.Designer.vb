<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fManualFire
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
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.Clock = New System.Windows.Forms.TextBox()
        Me.Clock2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.bVoltage = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(2, 1)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(61, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Unarm"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Timer3
        '
        Me.Timer3.Interval = 1000
        '
        'Clock
        '
        Me.Clock.Font = New System.Drawing.Font("Microsoft Sans Serif", 19.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Clock.Location = New System.Drawing.Point(2, 59)
        Me.Clock.Name = "Clock"
        Me.Clock.Size = New System.Drawing.Size(54, 45)
        Me.Clock.TabIndex = 1
        '
        'Clock2
        '
        Me.Clock2.Font = New System.Drawing.Font("Microsoft Sans Serif", 19.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Clock2.Location = New System.Drawing.Point(2, 110)
        Me.Clock2.Name = "Clock2"
        Me.Clock2.Size = New System.Drawing.Size(54, 45)
        Me.Clock2.TabIndex = 2
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(107, 2)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(349, 22)
        Me.TextBox1.TabIndex = 3
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(2, 30)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(61, 23)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Refresh"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'bVoltage
        '
        Me.bVoltage.Location = New System.Drawing.Point(2, 170)
        Me.bVoltage.Name = "bVoltage"
        Me.bVoltage.Size = New System.Drawing.Size(71, 31)
        Me.bVoltage.TabIndex = 5
        Me.bVoltage.Text = "Voltage"
        Me.bVoltage.UseVisualStyleBackColor = True
        '
        'fManualFire
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.AutoScrollMargin = New System.Drawing.Size(5, 5)
        Me.ClientSize = New System.Drawing.Size(1070, 525)
        Me.Controls.Add(Me.bVoltage)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Clock2)
        Me.Controls.Add(Me.Clock)
        Me.Controls.Add(Me.Button1)
        Me.Name = "fManualFire"
        Me.Text = "ManualFireForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Timer3 As System.Windows.Forms.Timer
    Friend WithEvents Clock As System.Windows.Forms.TextBox
    Friend WithEvents Clock2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents bVoltage As System.Windows.Forms.Button
End Class
