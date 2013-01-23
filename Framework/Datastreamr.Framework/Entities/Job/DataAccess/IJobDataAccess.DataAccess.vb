Namespace Entities.Job.DataAccess
    Public Interface IJobDataAccess
        Function GetEntity(id As Integer) As Job
        Function GetAllForCustomer() As IEnumerable(Of Job)
        Function Create(ByRef o As Job) As Boolean
        Function Update(ByRef o As Job) As Boolean
        Function Delete(ByRef o As Job) As Boolean
    End Interface
End Namespace
