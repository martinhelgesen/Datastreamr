Namespace Interfaces
    Public Interface IEndpoint (Of TParams As {New, StreamParams})
        Inherits IEndpoint
        Function Deliver(params As TParams, data As DataContainer) As EndPointResult
        Function GetParams() As TParams
    End Interface

    Public Interface IEndpoint
    End Interface
End Namespace