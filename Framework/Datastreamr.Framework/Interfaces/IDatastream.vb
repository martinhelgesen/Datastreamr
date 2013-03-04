Namespace Interfaces
    Public Interface IDatastream (Of TParams As {StreamParams})
        Inherits IDatastream
        Function GetStream(ByVal params As TParams) As DataContainer
    End Interface

    Public Interface IDatastream
        'Function InternalGetStream(ByVal params As StreamParams) As DataContainer
        Function GetStreamInternal(ByVal params As StreamParams) As DataContainer
        ReadOnly Property Name() As String
        ReadOnly Property Description() As String
    End Interface
End Namespace