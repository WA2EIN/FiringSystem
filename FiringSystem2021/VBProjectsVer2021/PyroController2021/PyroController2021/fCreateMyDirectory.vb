' (C) Copyright P. Cranwell, 2014
Public Class fCreateMyDirectory

    Private Sub CreateMyDirectoryForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Let user select approprate directory or create directory for Trace and Test Case data
        Me.Visible = False  ' Hide the form.  No need to show it
        FolderBrowserDialog1.ShowDialog()
        DirPath = FolderBrowserDialog1.SelectedPath & "\"
        SaveSetting("PYRO2014", "New", "DirPath", DirPath)    ' Save to Registry
        CreateDirectory()    ' Located in Module1
        Me.Finalize()   ' Close the foorm
    End Sub
End Class