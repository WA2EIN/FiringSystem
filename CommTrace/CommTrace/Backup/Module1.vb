
Imports System
Imports System.IO.Ports
Imports System.Threading
Imports System.Diagnostics.Stopwatch
Imports Microsoft.VisualBasic


Module Module1



    Public Echo As Boolean
    Public AllowBump As Boolean
    Public Direct As Boolean
    Public TimerStarted As Boolean

    Public Structure UnitStruct
        Public Session As Boolean
        Public Armed As Boolean
        Public Voltage As Single
        Public iSession As Integer
        Public NumCue As Integer
        Public Msg As MessageClass
        Public Active As Boolean
        Public Status As String
    End Structure
    Public Structure SerialMsg
        Public Length As Byte
        Public Msg() As Byte
    End Structure
    Public Structure ShowA
        Public Time As String              ' Absolute Show Time in MS
        Public Unit As String              ' Unit Number
        Public Cue As String               ' Cue Number
    End Structure
    Public Structure ShowT
        Public Unit As String                ' Unit Number
        'Public Sequence As String            ' Unique Sequence Number
        Public CueNumber As String           ' Cue Number
        Public Port As String                ' Port Number
        Public NextSequence As String        ' Next Sequence or Null
        Public TimeBefore As String          ' Time before this event
    End Structure
    Public Const DEBUG = False
    Public Const MAXNODES As Integer = 50
    Public cStartup As String = "1"
    Public Const iStartup As Integer = 1
    Public Const cShutdown As Char = "2"
    Public Const iShutdown As Integer = 2
    Public Const cSafe As Char = "3"
    Public Const iSafe As Integer = 3
    Public Const cFire As Char = "4"
    Public Const iFire As Integer = 4
    Public Const iSense As Integer = 5
    Public Const cSense As Char = "5"
    Public Const iTrace As Integer = 6
    Public cACK As String = "7"
    Public Const iACK As Integer = 7
    Public Const iNAK As Integer = 8
    Public cNAK As String = "8"
    Public Const OK As Integer = 1
    Public Const NG As Integer = 0
    Public Const None As Integer = 0
    Public Const Input As Integer = 1
    Public Const Output As Integer = 2
    Public Const Radioin As Integer = 3
    Public Const Radioout As Integer = 4
    Public Const MaxPacketSize As Integer = 130
    Public Const MaxReplyLength As Integer = 47
    Public GlobalResetData As String = "10199999900000000"
    Public TimeMsg As String = "10199980000000000"
    Public TimeBase As String = "101999800"
    Public ShowTime As Long
    Public ComPort As CommClass
    Public Msg As MessageClass
    Public TimeMessage As String
    Public UnitIndex As Integer
    Public UnitArray(MAXNODES) As String
    Public SessionStatus As Boolean = False
    Public forms(10) As Form
    Public ShowStarted As Boolean
    Public ctr As Integer
    Public ShowType As String
    Public Clock As String



    Public Sub Traceit(ByVal Msg As String)

        'MainForm.ListBox1.Items.Add("TOT")
        MainForm.ListBox1.Items.Add(Msg)
    End Sub
    Public Sub Wait()
        Thread.Sleep(50)
    End Sub




End Module
