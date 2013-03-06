Namespace Facade
    Public Class JobFacade
        Public Shared Function GetJob(name As String) As JobEntity
            Dim agg As New JobAggregate()
            Dim job = agg.Get(name)
            Return job
        End Function

        Public Shared Sub SaveJob(job As JobEntity)
            Dim agg As New JobAggregate()
            agg.Persist(job)
        End Sub

    End Class
End Namespace