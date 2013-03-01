Imports System.IO

Namespace Utils
    Public Class StreamHelper
        Public Shared Function GenerateStreamFromString(s As String) As Stream
            Dim stream As New MemoryStream()
            Dim writer As New StreamWriter(stream)
            writer.Write(s)
            writer.Flush()
            stream.Position = 0
            Return stream
        End Function

        Public Shared Function GenerateStreamReaderFromString(ByVal s As String) As StreamReader
            Return New StreamReader(GenerateStreamFromString(s))
        End Function
    End Class

    Public Class PeekableStreamReaderAdapter
        Private ReadOnly Underlying As StreamReader
        Private ReadOnly BufferedLines As Queue(Of String)

        Public Sub New(underlying__1 As StreamReader)
            Underlying = underlying__1
            BufferedLines = New Queue(Of String)()
        End Sub

        Public Function PeekLine() As String
            Dim line As String = Underlying.ReadLine()
            If line Is Nothing Then
                Return Nothing
            End If
            BufferedLines.Enqueue(line)
            Return line
        End Function


        Public Function ReadLine() As String
            If BufferedLines.Count > 0 Then
                Return BufferedLines.Dequeue()
            End If
            Return Underlying.ReadLine()
        End Function
    End Class
End Namespace