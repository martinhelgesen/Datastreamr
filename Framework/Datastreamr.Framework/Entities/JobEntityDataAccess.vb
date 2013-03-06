Namespace Entities
    Public Class JobEntityDataAccess
        Implements IJobEntityDataAcces

        Public Sub GetInstance(ByVal dbname As String, ByVal name As String, ByRef o As JobEntity) Implements IJobEntityDataAcces.GetInstance
            o = Persistence.Persister.Current.Load(Of JobEntity)(dbname, name)
        End Sub

        Public Sub Save(ByVal dbname As String, ByRef o As JobEntity) Implements IJobEntityDataAcces.Save
            Persistence.Persister.Current.Persist(dbname, o.Name, o)
        End Sub
    End Class
End Namespace