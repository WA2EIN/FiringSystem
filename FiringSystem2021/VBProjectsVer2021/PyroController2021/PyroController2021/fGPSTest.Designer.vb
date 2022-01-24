<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fGPSTest
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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.FromLat = New System.Windows.Forms.TextBox()
        Me.ToLat = New System.Windows.Forms.TextBox()
        Me.FromLon = New System.Windows.Forms.TextBox()
        Me.ToLon = New System.Windows.Forms.TextBox()
        Me.Dist = New System.Windows.Forms.TextBox()
        Me.Bearing = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(176, 351)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Calc"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'FromLat
        '
        Me.FromLat.Location = New System.Drawing.Point(86, 60)
        Me.FromLat.Name = "FromLat"
        Me.FromLat.Size = New System.Drawing.Size(101, 22)
        Me.FromLat.TabIndex = 1
        Me.FromLat.Text = "03710222"
        '
        'ToLat
        '
        Me.ToLat.Location = New System.Drawing.Point(327, 60)
        Me.ToLat.Name = "ToLat"
        Me.ToLat.Size = New System.Drawing.Size(100, 22)
        Me.ToLat.TabIndex = 2
        Me.ToLat.Text = "03710223"
        '
        'FromLon
        '
        Me.FromLon.Location = New System.Drawing.Point(83, 100)
        Me.FromLon.Name = "FromLon"
        Me.FromLon.Size = New System.Drawing.Size(104, 22)
        Me.FromLon.TabIndex = 3
        Me.FromLon.Text = "28000000"
        '
        'ToLon
        '
        Me.ToLon.Location = New System.Drawing.Point(327, 100)
        Me.ToLon.Name = "ToLon"
        Me.ToLon.Size = New System.Drawing.Size(100, 22)
        Me.ToLon.TabIndex = 4
        Me.ToLon.Text = "28000000"
        '
        'Dist
        '
        Me.Dist.Location = New System.Drawing.Point(176, 295)
        Me.Dist.Name = "Dist"
        Me.Dist.Size = New System.Drawing.Size(100, 22)
        Me.Dist.TabIndex = 5
        '
        'Bearing
        '
        Me.Bearing.Location = New System.Drawing.Point(176, 323)
        Me.Bearing.Name = "Bearing"
        Me.Bearing.Size = New System.Drawing.Size(100, 22)
        Me.Bearing.TabIndex = 6
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 60)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 17)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "From Lat"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 100)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(68, 17)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "From Lon"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(268, 63)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 17)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "To Lat"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(268, 105)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(53, 17)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "To Lon"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(138, 298)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(32, 17)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Dist"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(282, 298)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(51, 17)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Meters"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(282, 328)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(117, 17)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Degrees Relative"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(113, 323)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(57, 17)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Bearing"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(80, 19)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(427, 17)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "Enter Values in Degrees.   .00000166 approx 6 feet at 37 Deg Lat."
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(83, 144)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(330, 17)
        Me.Label10.TabIndex = 16
        Me.Label10.Text = "Data Received from Module is in format DDDddddd"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(83, 164)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(284, 17)
        Me.Label11.TabIndex = 17
        Me.Label11.Text = "DDD in Degrees, ddddd is fractional degree"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(83, 191)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(118, 17)
        Me.Label12.TabIndex = 18
        Me.Label12.Text = "W Lat = 360 - Lat"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(83, 218)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(122, 17)
        Me.Label13.TabIndex = 19
        Me.Label13.Text = "S Lon = 180 - Lon"
        '
        'GPSTestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 386)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Bearing)
        Me.Controls.Add(Me.Dist)
        Me.Controls.Add(Me.ToLon)
        Me.Controls.Add(Me.FromLon)
        Me.Controls.Add(Me.ToLat)
        Me.Controls.Add(Me.FromLat)
        Me.Controls.Add(Me.Button1)
        Me.Name = "GPSTestForm"
        Me.Text = "GPS Test Form for developinng GPS functions "
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents FromLat As System.Windows.Forms.TextBox
    Friend WithEvents ToLat As System.Windows.Forms.TextBox
    Friend WithEvents FromLon As System.Windows.Forms.TextBox
    Friend WithEvents ToLon As System.Windows.Forms.TextBox
    Friend WithEvents Dist As System.Windows.Forms.TextBox
    Friend WithEvents Bearing As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
End Class
