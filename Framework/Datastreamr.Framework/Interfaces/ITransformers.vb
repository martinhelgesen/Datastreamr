Namespace Interfaces

    Public Interface ITransformers(Of T)
        Inherits ITransformers
        Function InternalTransform(ByVal list As IEnumerable(Of T), ByVal params As IDictionary(Of String, ParamInfo)) As String
        Function GetParameters() As IDictionary(Of String, ParamInfo)
    End Interface

    Public Interface ITransformers
        Inherits IHasId
        Function Transform(ByVal list As IEnumerable(Of Object), ByVal params As IDictionary(Of String, ParamInfo)) As String
        ReadOnly Property Name As String
        ReadOnly Property Description As String
        ReadOnly Property ContentType() As String
    End Interface
End Namespace