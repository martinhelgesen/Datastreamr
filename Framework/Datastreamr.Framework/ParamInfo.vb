Public Class ParamInfo
    Property Name() As String
    Property Type() As Type
    Property Value() As Object
    Property Description() As String
    Property MaxLength() As String
    Property DefaultValue As Object
    Property Required() As Boolean = False
    Property SelectableValues As List(Of String)
End Class
