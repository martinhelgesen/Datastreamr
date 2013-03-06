Imports LazyFramework

Public Interface IDatastreamrContext
    Property CurrentUser() As User
End Interface

Public Class DatastreamrContext
    Implements IDatastreamrContext

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

    Public Sub New()
        Current = Me
    End Sub

    Public Property CurrentUser() As User Implements IDatastreamrContext.CurrentUser
End Class

Public Class DefaultDataStreamrContext
    Implements IDatastreamrContext

    Public Property CurrentUser As User Implements IDatastreamrContext.CurrentUser

End Class

