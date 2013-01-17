Imports Datastreamr.Framework.BaseClasses
Imports Datastreamr.Framework.Entities.CustomerStream.DataAccess

Namespace Entities.CustomerStream
    Partial Public Class CustomerStreamAggregate
        Inherits DatastreamrBaseAggregate(Of ICustomerStreamDataAccess, CustomerStreamDataAccess)

        Public Function GetInstance(customerStreamId As Integer) As CustomerStream
            'Check if me.CurrentUser is allowed to this
            Dim retObj = Repository.GetEntity(customerStreamId)
            Return retObj
        End Function
        Public Function GetAllForCustomer() As IEnumerable(Of CustomerStream)
            'Check if me.CurrentUser is allowed to this
            Dim retObj = Repository.GetAllForCustomer()
            Return retObj
        End Function
        Public Function CreateInstance(ByVal o As CustomerStream) As Boolean
            'Validate the object
            'Check if me.CurrentUser is allowed to this
            'Setting default values for the object
            ValidateCreateEntity(o)
            Return Repository.Create(o)
        End Function
        Public Function UpdateInstance(ByVal o As CustomerStream) As Boolean
            'Check if me.CurrentUser is allowed to this
            'Validate the object
            ValidateUpdateEntity(o)
            Return Repository.Update(o)
        End Function
        Public Function DeleteInstance(ByVal o As CustomerStream) As Boolean
            'Check if me.CurrentUser is allowed to this
            'Validate the object
            ValidateDeleteEntity(o)
            Return Repository.Delete(o)
        End Function
    End Class
End Namespace
