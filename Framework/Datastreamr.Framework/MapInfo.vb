Public Class MapInfo
    Property FromName As String
    Property ToName As String
    Property Rule As String
End Class

Public Class MapConfig
    Inherits List(Of MapInfo)
End Class