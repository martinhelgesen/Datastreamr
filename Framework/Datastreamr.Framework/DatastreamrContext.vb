Imports LazyFramework

Public Class DatastreamrContext
    Public Shared Property Current As IDatastreamrContext
        Get
            Return ClassFactory.GetTypeInstance(Of IDatastreamrContext)()
        End Get
        Set(value As IDatastreamrContext)
            If value Is Nothing Then
                ClassFactory.RemoveTypeInstanceForSession(Of IDatastreamrContext)()
            Else
                ClassFactory.SetTypeInstanceForSession(Of IDatastreamrContext)(value)
            End If
        End Set
    End Property
End Class