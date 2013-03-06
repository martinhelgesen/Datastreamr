Imports Datastreamr.Framework.BaseClasses
Imports Datastreamr.Framework.Entities
Imports LazyFramework

Public Class JobAggregate
    Inherits DatastreamrBaseAggregate(Of IJobEntityDataAcces, JobEntityDataAccess)

    Public Function [Get](ByVal name As String) As JobEntity
        Dim dataaccess = ClassFactory.GetTypeInstance(Of IJobEntityDataAcces, JobEntityDataAccess)()
        Dim o As New JobEntity
        dataaccess.GetInstance(DbName, name, o)
        Return o
    End Function

    Public Sub Persist(data As JobEntity)
        Dim dataaccess = ClassFactory.GetTypeInstance(Of IJobEntityDataAcces, JobEntityDataAccess)()
        'Dim o As New JobEntity
        If String.IsNullOrEmpty(data.Name) Then
            Throw New ArgumentException("Name cannot be empty", "Name")
        End If
        dataaccess.Save(DbName, data)        
    End Sub

End Class
