Namespace Interfaces
    Public Interface IDatastream (Of TParams As {BaseStreamParams})
        Inherits IDatastream
        Function GetStream() As DataContainer
        Property StreamParams As TParams
        Function GetParams() As TParams
    End Interface

    Public Interface IDatastream
        'Function InternalGetStream(ByVal params As StreamParams) As DataContainer
        Function GetStreamInternal() As DataContainer
        ReadOnly Property Name() As String
        ReadOnly Property Description() As String
        Sub SetParams(ByVal dataBaseStreamParams As BaseStreamParams)
    End Interface
End Namespace