Public Class MapConfig
    Inherits List(Of MapInfo)

    Public Overloads Sub Add(fromName As String, toName As String, rule As String)
        Me.Add(New MapInfo With {.FromName = fromName, .ToName = toName, .Rule = rule})
    End Sub
End Class