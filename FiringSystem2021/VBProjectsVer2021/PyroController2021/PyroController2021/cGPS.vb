' (C) Copyright P. Cranwell, 2014
Public Class cGPS
    '   This class provides support for converting a GPS data stream to positional information
    '       for plotting Field Module positions on a display.
    '
    'NOTE:  Resolution capability: .001 Min approx 4.8 feet at 37 Deg N.
    '
    '  Code taken from:  http://www.informit.com/guides/content.aspx?g=dotnet&seqNum=512 
    '      and modified for pyro system use.
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Structure wptType
        Dim lat As Double
        Dim lon As Double
        Dim ele As Double
    End Structure
    Private Function toDecimal(ByVal Pos As String) As Double
        'Convert the recieved NMEA(WGS-84) location to a decimal number
        'Pos="5601.0318"
        Dim PosDb As Double = CType(Replace(Pos, ".", ","), Double) 'Replace . with , (Used in danish doubles)
        Dim Deg As Double = Math.Floor(PosDb / 100)
        Dim DecPos As Double = Math.Round(Deg + ((PosDb - (Deg * 100)) / 60), 5)
        If DecPos < 1 Then
            DecPos = DecPos * -1    ' For negative values for S hemisphere and W of Greenwich.
        End If
        Return DecPos '=56.0172
    End Function
    Sub GetCourseAndDistance(Source As fGPSTest.wptType, Destination As fGPSTest.wptType, ByRef Course As Double, ByRef Distance As Double)
        ' convert latitude and longitude to radians
        ' Dim lat1 As Double = DegreesToRadians(CDbl(Val(GPSTestForm.FromLat.Text)))
        'Dim lon1 As Double = DegreesToRadians(CDbl(Val(GPSTestForm.FromLon.Text)))
        'Dim lat2 As Double = DegreesToRadians(CDbl(Val(GPSTestForm.ToLat.Text)))
        'Dim lon2 As Double = DegreesToRadians(CDbl(Val(GPSTestForm.ToLon.Text)))

        Dim lat1 As Double = DegreesToRadians(Source.lat)
        Dim lon1 As Double = DegreesToRadians(Source.lon)
        Dim lat2 As Double = DegreesToRadians(Destination.lat)
        Dim lon2 As Double = DegreesToRadians(Destination.lon)

        ' compute latitude and longitude differences
        Dim dlat As Double = lat2 - lat1
        Dim dlon As Double = lon2 - lon1

        Dim distanceNorth As Double = dlat
        Dim distanceEast As Double = dlon * Math.Cos(lat1)
        Dim dist As Double


        ' compute the distance in radians
        dist = Math.Sqrt(distanceNorth * distanceNorth + distanceEast * distanceEast)

        ' and convert the radians to meters
        dist = RadiansToMeters(dist)

        ' add the elevation difference to the calculation
        'Dim dele As Double = CDbl(pt2.ele - pt1.ele)
        Dim dele As Double = 0      ' Compute using the same elevation
        dist = Math.Sqrt(dist * dist + dele * dele)
        Distance = dist

        ' compute the course
        Course = Math.Atan2(distanceEast, distanceNorth) Mod (2 * Math.PI)
        Course = RadiansToDegrees(Course)
        If Course < 0 Then
            Course += 360
        End If
    End Sub
    Function DegreesToRadians(ByVal degrees As Double) As Double
        Return degrees * Math.PI / 180.0
    End Function
    Function RadiansToDegrees(ByVal radians As Double) As Double
        Return radians * 180.0 / Math.PI
    End Function
    Function RadiansToNauticalMiles(ByVal radians As Double) As Double
        ' There are 60 nautical miles for each degree
        Return radians * 60 * 180 / Math.PI
    End Function
    Function RadiansToMeters(ByVal radians As Double) As Double
        ' there are 1852 meters in a nautical mile
        Return 1852 * RadiansToNauticalMiles(radians)
    End Function

End Class
