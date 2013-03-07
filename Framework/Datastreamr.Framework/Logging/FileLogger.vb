Imports System.IO

Namespace Logging
    Public Class FileLogger
        Implements ILog, IDisposable
        
        Private ReadOnly _StringWriter As StreamWriter

        Public Sub New(path As String)
            _StringWriter = New StreamWriter(path, True, System.Text.Encoding.UTF8)
        End Sub

        Public Sub WriteLine(message As String) Implements ILog.WriteLine
            _StringWriter.WriteLine(message)
        End Sub

        Public Sub WriteLineFormat(format As String, ParamArray args() As String) Implements ILog.WriteLineFormat
            _StringWriter.WriteLine(format, args)
        End Sub

#Region "IDisposable Support"
        Private _DisposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me._DisposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    _StringWriter.Close()
                    _StringWriter.Dispose()
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
End NameSpace