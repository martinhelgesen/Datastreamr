
Imports LazyFramework.Utils

Public Class DatastreamrContext
    Private Shared _datastreamrcontextSlotName As String = "DatastreamrContext"

    Public Shared Property Current As DatastreamrContext
        Get
            If ResponseThread.ThreadHasKey(_datastreamrcontextSlotName) Then
                Return ResponseThread.GetThreadValue (Of DatastreamrContext)(_datastreamrcontextSlotName)
            End If
            Throw New NotImplementedException
        End Get
        Set(value As DatastreamrContext)
            ResponseThread.SetThreadValue(_datastreamrcontextSlotName, value)
        End Set
    End Property

    Public Sub New()
        Current = Me
    End Sub

    Public Property CurrentUser() As User
End Class

