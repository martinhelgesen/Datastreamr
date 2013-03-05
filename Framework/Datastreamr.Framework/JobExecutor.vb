Public Class JobExecutor
    Private ReadOnly _job As JobEntity

    Public Sub New(job As JobEntity)
        _job = job
        ValidateJob()
    End Sub

    Private Sub ValidateJob()
        'Throw New NotImplementedException()
    End Sub

    Public Function Execute() As JobResult
        Dim datacontainer = _job.DataStream.GetStreamInternal(_job.DataStreamParams)
        Dim endpoint = _job.Endpoint

        Dim mapresult = Mapper.Map(datacontainer, _job.Mapconfig)
        Dim endpointResult = endpoint.InternalDeliver(_job.EndpointParams, mapresult)
        Return New JobResult With {.EndpointResult = endpointResult, .MapResult = mapresult}
    End Function
End Class