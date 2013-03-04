
Imports LazyFramework
Imports LazyFramework.Utils

Public Interface IDatastreamrContext
    Property CurrentUser() As User
End Interface

Public Class DatastreamrContext
    Implements IDatastreamrContext
    Private Shared _datastreamrcontextSlotName As String = "DatastreamrContext"

    Public Shared Property Current As IDatastreamrContext
        Get
            Return ClassFactory.GetTypeInstance(Of IDatastreamrContext, DefaultDataStreamrContext)()
            'If Not ResponseThread.ThreadHasKey(_datastreamrcontextSlotName) Then
            '    ResponseThread.SetThreadValue(_datastreamrcontextSlotName, )
            'End If
            'Return ResponseThread.GetThreadValue(Of DatastreamrContext)(_datastreamrcontextSlotName)
        End Get
        Set(value As IDatastreamrContext)
            ResponseThread.SetThreadValue(_datastreamrcontextSlotName, value)
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
        Get
            Throw New NotImplementedException
        End Get
        Set(value As User)
            Throw New NotImplementedException
        End Set
    End Property

End Class

