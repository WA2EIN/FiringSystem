' (C) Copyright P. Cranwell, 2014
Public Class cMessage
    Private Message As String
    Private NextWU As Integer
    Private Code As String
    Private WUCount As Integer
    Private WUNumber() = {"00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16"}
    Private NextSendNum As Integer
    Private NextRecNum As Integer
    Private Comand As String
    Private UnitNumber As String
    Private Length As Integer
    Private RawData As String

    Public Property NextSendNumber()
        Get
            Return NextSendNum
        End Get
        Set(ByVal value)
            NextSendNum = value
        End Set
    End Property
    Public Property NextReceiveNumber()
        Get
            Return (NextRecNum)
        End Get
        Set(ByVal value)
            NextRecNum = value
        End Set
    End Property
    Public Property Command()
        Get
            Return Comand
        End Get
        Set(ByVal value)
            Comand = value
        End Set
    End Property
    Public Property Unit()
        Get
            Return UnitNumber
        End Get
        Set(ByVal value)
            UnitNumber = value
            UnitNumber = "000" & UnitNumber
            Length = Len(UnitNumber)
            UnitNumber = Mid$(UnitNumber, Length - 2)
        End Set
    End Property
    Public Sub New()
        Message = ""
        WUCount = 0
        Comand = ""
        UnitNumber = ""
        NextSendNum = 1
        NextRecNum = 1
    End Sub
    Public Sub NewMessage()
        Message = ""
        WUCount = 0
        Comand = ""
        UnitNumber = ""
    End Sub
    Public Sub AddWU(ByVal WU As String)
        Message = Message & WU
        WUCount = WUCount + 1
    End Sub
    Public Function CompleteMessage() As String
        Message = NextSendNumber & WUNumber(WUCount) & UnitNumber & Comand & Message
        Code = GenerateCheckCode(Message)
        Message = Message & Code
        Return Message
    End Function
    Public Function GenerateCheckCode(ByVal Msg As String) As String
        Dim IntTemp, i, d, c, cd, cc As Integer
        Dim fact() As Integer = {3, 7, 1}
        Dim cTemp As String
        Dim Codetab() As Integer = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}

        ' Check Code is calculated per bank routing code algorithm
        ' it requires only numeric data (digits 0-9) in the message.

        cd = 0
        cTemp = ""
        i = 0

        For c = 1 To Len(Msg)
            If i > 2 Then i = 0
            d = fact(i)
            cTemp = Mid$(Msg, c, 1)
            cd = cd + (d * Val(cTemp))
            i = i + 1

        Next c

        ' calculate remainder of div by 10
        IntTemp = cd Mod 10
        cc = Math.Abs(IntTemp)
        Return Codetab(cc)
    End Function
    Public Sub BumpSendNumber()
        If AllowBump = True Then
            NextSendNum = NextSendNum + 1
            If NextSendNum > 9 Then NextSendNum = 1
        End If
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
