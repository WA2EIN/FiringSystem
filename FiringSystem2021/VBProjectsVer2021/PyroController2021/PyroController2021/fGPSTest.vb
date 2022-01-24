' (C) Copyright P. Cranwell, 2014
Public Class fGPSTest
    Structure wptType
        ' Waypoint Structure
        Dim lat As Double   ' Latitude
        Dim lon As Double   ' Longitude
        Dim ele As Double   ' Elevation
    End Structure

    Dim Distance As Double = 0
    Dim Course As Double = 0
    Dim Source As wptType
    Dim Destination As wptType
    Dim GPS As New cGPS
    Private Sub GPSTestForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim LatConverted As Double
        Dim LonConverted As Double

        LatConverted = FromLat.Text.Substring(0, 3)
        If LatConverted > 90 Then
            LatConverted = 180 - FromLat.Text.Substring(0, 3)
        End If
        LatConverted = LatConverted + (FromLat.Text.Substring(3, 5) / 100000)
        Source.lat = LatConverted

        LatConverted = ToLat.Text.Substring(0, 3)
        If LatConverted > 90 Then
            LatConverted = 180 - ToLat.Text.Substring(0, 3)
        End If
        LatConverted = LatConverted + ToLat.Text.Substring(3, 5) / 100000
        Destination.lat = LatConverted


        LonConverted = FromLon.Text.Substring(0, 3)
        If LonConverted > 180 Then
            LonConverted = 360 - FromLon.Text.Substring(0, 3)
        End If
        LonConverted = LonConverted + FromLon.Text.Substring(3, 5) / 100000
        Source.lon = LonConverted

        LonConverted = ToLon.Text.Substring(0, 3)
        If LonConverted > 180 Then
            LonConverted = 360 - ToLon.Text.Substring(0, 3)
        End If
        LonConverted = LonConverted + ToLon.Text.Substring(3, 5) / 100000
        Destination.lon = LonConverted


        GPS.GetCourseAndDistance(Source, Destination, Course, Distance)
        Bearing.Text = Course.ToString
        Dist.Text = Distance.ToString
    End Sub

    Private Sub FromLat_TextChanged(sender As System.Object, e As System.EventArgs) Handles FromLat.TextChanged

    End Sub

    Private Sub ToLon_TextChanged(sender As System.Object, e As System.EventArgs) Handles ToLon.TextChanged

    End Sub

    Private Sub Label10_Click(sender As System.Object, e As System.EventArgs)

    End Sub
End Class