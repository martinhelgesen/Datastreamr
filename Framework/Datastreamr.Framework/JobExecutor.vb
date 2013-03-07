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
        Dim mapresult As DataContainer = Nothing
        Dim endpointResult As EndPointResult = Nothing
        Try
            Dim dataContainer = _job.DataStream.GetStreamInternal(_job.DataStreamParams)
            Dim endpoint = _job.Endpoint
            mapresult = Mapper.Map(dataContainer, _job.Mapconfig)
            endpointResult = endpoint.InternalDeliver(_job.EndpointParams, mapresult)
            Return New JobResult With {.EndpointResult = endpointResult, .MapResult = mapresult, .Success = True}
        Catch ex As Exception
            Return New JobResult With {.EndpointResult = endpointResult, .MapResult = mapresult, .Success = False, .Exception = ex}
        End Try
    End Function
End Class

