Namespace Interfaces

    Public Interface ITransformers(Of TIn, TOut)
        Inherits ITransformers
        Function InternalTransform(ByVal list As IEnumerable(Of TIn), ByVal params As IDictionary(Of String, ParamInfo)) As IEnumerable(Of TOut)
        Function GetParameters() As IDictionary(Of String, ParamInfo)
    End Interface

    Public Interface ITransformers
        Inherits IHasId
        Function Transform(ByVal list As IEnumerable(Of Object), ByVal params As IDictionary(Of String, ParamInfo)) As IEnumerable(Of Object)
        ReadOnly Property Name As String
        ReadOnly Property Description As String
        ReadOnly Property ContentType() As String
    End Interface
End Namespace