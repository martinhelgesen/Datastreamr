Public Class JobExecutor
    Public Sub New(j As JobEntity)
        Dim ds = j.DataStream
        ValidateJob()
    End Sub

    Private Sub ValidateJob()
        'Throw New NotImplementedException()
    End Sub

    Public Function Start() As JobResult

    End Function
End Class

Public Class JobResult
End Class