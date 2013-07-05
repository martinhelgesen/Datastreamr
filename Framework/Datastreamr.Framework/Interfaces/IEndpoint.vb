Namespace Interfaces
    Public Interface IEndpoint (Of TParams As {New, BaseStreamParams})
        Inherits IEndpoint
        Function Deliver(data As DataContainer) As EndPointResult
        ReadOnly Property GetObjectMapInfo As List(Of ParamInfo)
        Property StreamParams As TParams
        Function GetParams() As TParams
    End Interface

    Public Interface IEndpoint
        Function InternalDeliver(data As DataContainer) As EndPointResult
        Sub SetParams(ByVal dataBaseStreamParams As BaseStreamParams)
    End Interface
End Namespace