Public Class JobExecutor
    Private ReadOnly _job As JobEntity

    Public Sub New(job As JobEntity)
        _job = job
        ValidateJob()
    End Sub

    Private Sub ValidateJob()
        'Throw New NotImplementedException()
    End Sub

    Public Function Start() As JobResult
        'Dim datacontainer = _job.DataStream.InternalGetStream(DeSerialize(_job.DataStreamParams))
    End Function
End Class

Public Class JobResult
End Class