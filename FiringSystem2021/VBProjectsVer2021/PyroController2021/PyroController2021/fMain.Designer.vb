<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fMain
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
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.ComboBox2 = New System.Windows.Forms.ComboBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeTraceDirectory = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripMenuItem()
        Me.SetupComPortToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CleanRegistryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QuieryBuildDateToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ManualFireToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadShowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CreateShowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestGPSCalculationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LoadCSVToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestCaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.ComboBox3 = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(35, 32)
        Me.ListBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(696, 368)
        Me.ListBox1.TabIndex = 0
        '
        'ComboBox2
        '
        Me.ComboBox2.FormattingEnabled = True
        Me.ComboBox2.Location = New System.Drawing.Point(752, 9)
        Me.ComboBox2.Margin = New System.Windows.Forms.Padding(2)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(29, 21)
        Me.ComboBox2.TabIndex = 1
        Me.ComboBox2.Visible = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem1, Me.ManualFireToolStripMenuItem, Me.ShowToolStripMenuItem, Me.TestGPSCalculationsToolStripMenuItem, Me.LoadCSVToolStripMenuItem1, Me.TestCaseToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(4, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(830, 24)
        Me.MenuStrip1.TabIndex = 2
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ChangeTraceDirectory, Me.ToolStripMenuItem3, Me.ToolStripMenuItem4, Me.ToolStripMenuItem5, Me.ToolStripMenuItem6, Me.SetupComPortToolStripMenuItem, Me.CleanRegistryToolStripMenuItem, Me.QuieryBuildDateToolStripMenuItem})
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(49, 20)
        Me.ToolStripMenuItem1.Text = "Setup"
        '
        'ChangeTraceDirectory
        '
        Me.ChangeTraceDirectory.Name = "ChangeTraceDirectory"
        Me.ChangeTraceDirectory.Size = New System.Drawing.Size(204, 22)
        Me.ChangeTraceDirectory.Text = "ChangeTraceDirectory"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(204, 22)
        Me.ToolStripMenuItem3.Text = "Set Latency Delay"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(204, 22)
        Me.ToolStripMenuItem4.Text = "Setup Network Topology"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(204, 22)
        Me.ToolStripMenuItem5.Text = "Poll Network"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(204, 22)
        Me.ToolStripMenuItem6.Text = "Set Device Address"
        '
        'SetupComPortToolStripMenuItem
        '
        Me.SetupComPortToolStripMenuItem.Name = "SetupComPortToolStripMenuItem"
        Me.SetupComPortToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.SetupComPortToolStripMenuItem.Text = "Setup Com Port"
        '
        'CleanRegistryToolStripMenuItem
        '
        Me.CleanRegistryToolStripMenuItem.Name = "CleanRegistryToolStripMenuItem"
        Me.CleanRegistryToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.CleanRegistryToolStripMenuItem.Text = "Clean Registry"
        '
        'QuieryBuildDateToolStripMenuItem
        '
        Me.QuieryBuildDateToolStripMenuItem.Name = "QuieryBuildDateToolStripMenuItem"
        Me.QuieryBuildDateToolStripMenuItem.Size = New System.Drawing.Size(204, 22)
        Me.QuieryBuildDateToolStripMenuItem.Text = "Quiery Build Date"
        '
        'ManualFireToolStripMenuItem
        '
        Me.ManualFireToolStripMenuItem.Name = "ManualFireToolStripMenuItem"
        Me.ManualFireToolStripMenuItem.Size = New System.Drawing.Size(81, 20)
        Me.ManualFireToolStripMenuItem.Text = "Manual Fire"
        '
        'ShowToolStripMenuItem
        '
        Me.ShowToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LoadShowToolStripMenuItem, Me.CreateShowToolStripMenuItem})
        Me.ShowToolStripMenuItem.Name = "ShowToolStripMenuItem"
        Me.ShowToolStripMenuItem.Size = New System.Drawing.Size(48, 20)
        Me.ShowToolStripMenuItem.Text = "Show"
        '
        'LoadShowToolStripMenuItem
        '
        Me.LoadShowToolStripMenuItem.Name = "LoadShowToolStripMenuItem"
        Me.LoadShowToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
        Me.LoadShowToolStripMenuItem.Text = "Load Show"
        '
        'CreateShowToolStripMenuItem
        '
        Me.CreateShowToolStripMenuItem.Name = "CreateShowToolStripMenuItem"
        Me.CreateShowToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
        Me.CreateShowToolStripMenuItem.Text = "Create Show"
        '
        'TestGPSCalculationsToolStripMenuItem
        '
        Me.TestGPSCalculationsToolStripMenuItem.Enabled = False
        Me.TestGPSCalculationsToolStripMenuItem.Name = "TestGPSCalculationsToolStripMenuItem"
        Me.TestGPSCalculationsToolStripMenuItem.Size = New System.Drawing.Size(131, 20)
        Me.TestGPSCalculationsToolStripMenuItem.Text = "Test GPS Calculations"
        Me.TestGPSCalculationsToolStripMenuItem.Visible = False
        '
        'LoadCSVToolStripMenuItem1
        '
        Me.LoadCSVToolStripMenuItem1.Enabled = False
        Me.LoadCSVToolStripMenuItem1.Name = "LoadCSVToolStripMenuItem1"
        Me.LoadCSVToolStripMenuItem1.Size = New System.Drawing.Size(178, 20)
        Me.LoadCSVToolStripMenuItem1.Text = "Load ShowDatabase from CSV"
        Me.LoadCSVToolStripMenuItem1.Visible = False
        '
        'TestCaseToolStripMenuItem
        '
        Me.TestCaseToolStripMenuItem.Enabled = False
        Me.TestCaseToolStripMenuItem.Name = "TestCaseToolStripMenuItem"
        Me.TestCaseToolStripMenuItem.Size = New System.Drawing.Size(149, 20)
        Me.TestCaseToolStripMenuItem.Text = "Load Simulated Network"
        Me.TestCaseToolStripMenuItem.Visible = False
        '
        'ComboBox3
        '
        Me.ComboBox3.FormattingEnabled = True
        Me.ComboBox3.Location = New System.Drawing.Point(737, 44)
        Me.ComboBox3.Margin = New System.Windows.Forms.Padding(2)
        Me.ComboBox3.Name = "ComboBox3"
        Me.ComboBox3.Size = New System.Drawing.Size(85, 21)
        Me.ComboBox3.TabIndex = 3
        Me.ComboBox3.Visible = False
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(511, 1)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Clear Trace"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'fMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(830, 444)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ComboBox3)
        Me.Controls.Add(Me.ComboBox2)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "fMain"
        Me.Text = "Pyro Controller 2021"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChangeTraceDirectory As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents ManualFireToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestGPSCalculationsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadShowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TestCaseToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LoadCSVToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SetupComPortToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ComboBox3 As System.Windows.Forms.ComboBox
    Friend WithEvents CleanRegistryToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents QuieryBuildDateToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CreateShowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button1 As System.Windows.Forms.Button

End Class
