Namespace Interfaces
    Public Interface IEndpoint (Of TParams As {New, StreamParams})
        Inherits IEndpoint
        Function Deliver(params As TParams, data As DataContainer) As EndPointResult
        Function GetParams() As TParams
        ReadOnly Property GetObjectMapInfo As List(Of ParamInfo)
    End Interface

    Public Interface IEndpoint
        Function InternalDeliver(params As StreamParams, data As DataContainer) As EndPointResult
    End Interface
End Namespace