Imports Datastreamr.Framework.Entities
Imports LazyFramework

Public Class JobAggregate
    Public Function [Get](ByVal id As Integer) As JobEntity
        Dim dataaccess = ClassFactory.GetTypeInstance (Of IJobEntityDataAcces, JobEntityDataAcces)()
        Dim o As New JobEntity
        dataaccess.GetInstance(id, o)
        Return o
    End Function
End Class
