Imports LazyFramework

Public Class JobExecutor
    Private ReadOnly _job As JobEntity
    'Private _context As DatastreamrContext

    Public Sub New(job As JobEntity)
        _job = job
        '_context = context
        ValidateJob()
    End Sub

    Private Sub ValidateJob()
        'Throw New NotImplementedException()
    End Sub

    Public Function Execute() As JobResult
        Dim dataContainer = _job.DataStream.GetStreamInternal(_job.DataStreamParams)
        Dim endpoint = _job.Endpoint
        Dim mapresult = Mapper.Map(dataContainer, _job.Mapconfig)
        Dim endpointResult = endpoint.InternalDeliver(_job.EndpointParams, mapresult)
        Return New JobResult With {.EndpointResult = endpointResult, .MapResult = mapresult}
    End Function
End Class

