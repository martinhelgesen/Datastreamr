Imports Datastreamr.Framework.Logging

Public Class DefaultDataStreamrContext
    Implements IDatastreamrContext

    Public Sub New()
        DatastreamrContext.Current = Me
    End Sub

    Public Property CurrentUser As User Implements IDatastreamrContext.CurrentUser
    Public Property Logger As ILog Implements IDatastreamrContext.Logger

#Region "IDisposable Support"
    Private _DisposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me._DisposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                DatastreamrContext.Current = Nothing
                If Logger IsNot Nothing Then
                    Logger.Dispose()
                End If
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me._DisposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class