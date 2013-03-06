Namespace Interfaces
    Public Interface IPersister
        Function Load(Of T)(dbName As String, objectName As String) As T
        Sub Persist(dbName As String, objectname As String, data As Object)
    End Interface
End Namespace