Namespace Logging
    Public Interface ILog
        Inherits IDisposable
        Sub WriteLine(message As String)
        Sub WriteLineFormat(format As String, ParamArray args() As String)
    End Interface
End Namespace