' (C) Copyright P. Cranwell, 2014
Public Class cStopWatch
    Declare Function QueryPerformanceCounter Lib "Kernel32" (ByRef X As Long) As Short
    Declare Function QueryPerformanceFrequency Lib "Kernel32" (ByRef X As Long) As Short
    Private _start As Long
    Private _end As Long
    Private _freq As Long
    Private _overhead As Long
    Private _desc As String
    Public Sub New()
        Me.New("")
    End Sub
    Public Sub New(ByVal description As String)
        If (QueryPerformanceCounter(_start) = 0) Then
            Throw New StopwatchException()
        End If
        If (QueryPerformanceCounter(_end) = 0) Then
            Throw New StopwatchException()
        End If
        ' calculate the time just to call start and end
        _overhead = (_end - _start)
        Me.Start(description)
    End Sub
    Public Sub Start(Optional ByVal description As String = "")
        _desc = description
        QueryPerformanceFrequency(_freq)
        _end = 0
        If (QueryPerformanceCounter(_start) = 0) Then
            Throw New StopwatchException()
        End If
    End Sub

    Public Function Done() As Double
        If (QueryPerformanceCounter(_end) = 0) Then
            Throw New StopwatchException()
        End If
        Return Me.ElapsedTime()
    End Function

    Public Function ElapsedTime() As Double
        If (_end = 0) Then
            Dim _now As Long
            If (QueryPerformanceCounter(_now) = 0) Then
                Throw New StopwatchException()
            End If
            Return (((_now - _start) - _overhead) / _freq)
        End If
        Return (((_end - _start) - _overhead) / _freq)
    End Function
    Public Sub ReportToConsole(ByVal Msg As String)
        If (_end = 0) Then Me.Done()
        fTrace.ListBox1.Items.Add(Msg & "    " & Me.ElapsedTime() & "    Seconds")
    End Sub
End Class
Public Class StopwatchException
    Inherits System.ApplicationException
    Overrides ReadOnly Property Message() As String
        Get
            Return "Stopwatch: QueryPerformanceCounter[Kernel32] returned 0"
        End Get
    End Property
End Class

