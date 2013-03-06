Namespace Entities
    Public Interface IJobEntityDataAcces
        Sub GetInstance(ByVal dbname As String, ByVal name As String, ByRef o As JobEntity)
        Sub Save(dbname As String, ByRef o As JobEntity)
    End Interface
End Namespace