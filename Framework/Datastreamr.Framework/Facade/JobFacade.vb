Namespace Facade
    Public Class JobFacade
        Public Shared Function GetJob(id As Integer) As JobEntity
            Dim agg As New JobAggregate()
            Dim job = agg.Get(id)
            Return job
        End Function
    End Class
End Namespace