Namespace Entities.CustomerStream.DataAccess
    Public Interface ICustomerStreamDataAccess
        Function GetEntity(id As Integer) As CustomerStream
        Function GetAllForCustomer() As IEnumerable(Of CustomerStream)
        Function Create(ByRef o As CustomerStream) As Boolean
        Function Update(ByRef o As CustomerStream) As Boolean
        Function Delete(ByRef o As CustomerStream) As Boolean
    End Interface
End Namespace
