' (C) Copyright P. Cranwell, 2014
Public Class cPorts
    Private ValIsInShow As Boolean
    Private ValIsInCueChain As Boolean
    Public Structure PortStruct
        Dim eMatch As Boolean           ' False = BAD, TRUE = Good
        Dim eMatchIsFired As Boolean    ' False = Port not fired, True = Port has ben fired
        Dim Active As Boolean           ' TRUE = Port is Active
        Dim MS As ULong                 ' Port Fire Hold time in MS
        Dim Area As Integer             ' Numeric Area
        Dim Caliber As Integer          ' Numeric Caliber
        Dim Item As Integer             ' Numeric Item
        Dim Unit As Integer             ' Unit that this button is associated with
        Dim But As Button
        Dim ButLoc As Point
        Dim Text As TextBox
        Dim TextLoc As Point
        Dim valEffect As String         ' Pyro Effect associated with this button

    End Structure
    Public Port As PortStruct


    Public Sub New()
        Port = New PortStruct
        Port.Active = False
        Port.eMatch = False
        Port.eMatchIsFired = False
        Port.MS = 0
        Port.Area = 0
        Port.Caliber = 0
        Port.Item = 0
        Port.Unit = 0
        ValIsInCueChain = False
        ValIsInShow = False

    End Sub
    Public WriteOnly Property ActivatePort() As Integer
        Set(ByVal value As Integer)
            Port.Active = True
        End Set
    End Property
    Public WriteOnly Property DeactivatePort() As Integer
        Set(ByVal value As Integer)
            Port.Active = False
        End Set
    End Property
    Public Property Button() As Button
        Get
            Return Port.But
        End Get
        Set(ByVal value As Button)
            Port.But = value
        End Set
    End Property

    Public Property IsInShow() As Boolean
        Get
            IsInShow = ValIsInShow
        End Get
        Set(ByVal value As Boolean)
            ValIsInShow = value
        End Set
    End Property
    Public Property IsInCueChain() As Boolean
        Get
            IsInCueChain = ValIsInCueChain
        End Get
        Set(ByVal value As Boolean)
            ValIsInCueChain = value
        End Set
    End Property

    Public Property ButtonLoc() As Point
        Get
            Return Port.ButLoc
        End Get
        Set(ByVal value As Point)
            Port.ButLoc = value
        End Set
    End Property
    Public Property ButtonText() As TextBox
        Get
            Return Port.Text
        End Get
        Set(ByVal value As TextBox)
            Port.Text = value
        End Set
    End Property
    Public Property TextLocation() As Point
        Get
            Return Port.TextLoc
        End Get
        Set(ByVal value As Point)
            Port.TextLoc = value
        End Set
    End Property
    Public Property Effect() As String
        Get
            Return Port.valEffect
        End Get
        Set(ByVal value As String)
            Port.valEffect = value
        End Set
    End Property
    Public Property Unit() As Integer
        Get
            Return Port.Unit
        End Get
        Set(ByVal value As Integer)
            Port.Unit = value
        End Set
    End Property
End Class
