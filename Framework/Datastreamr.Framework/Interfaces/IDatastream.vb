Namespace Interfaces 

    Public Interface IDatastream(Of TParams As {StreamParams})   
        Function GetStream(ByVal params As TParams) As DataContainer
        Function GetParams() As TParams
        ReadOnly Property Name() As String
        ReadOnly Property Description() As String
    End Interface


End Namespace