Imports Datastreamr.Framework.Utils
Imports System.IO
Imports Datastreamr.Framework.InternalStreams

Namespace DataStreams
    Public MustInherit Class StreamReaderStream(Of TParams As {StreamParams, New})
        Inherits BaseDataStream(Of TParams)

        Public MustOverride Overrides ReadOnly Property Name As String
        Public MustOverride Overrides ReadOnly Property Description As String
        Public MustOverride Overrides Function GetStreamInternal(params As StreamParams) As DataContainer
        Protected Function ConvertToDataContainer(ByVal streamReader As StreamReader, ByVal params As StreamReaderStreamParams) _
            As DataContainer
            If streamReader Is Nothing Then
                Return Nothing
            End If
            Dim peekableStreamReader = New PeekableStreamReaderAdapter(streamReader)
            Dim _
                retval As _
                    New DataContainer _
                    With {.MetaData = New List(Of ParamInfo), .Data = New List(Of Dictionary(Of String, Object))}

            'Metadata
            Dim line As String
            If params.FirstLineIsHeader Then
                line = peekableStreamReader.ReadLine
                Dim fields = line.Split(CType(params.ValueSeparator, Char))
                For i = 0 To fields.Length - 1
                    retval.MetaData.Add(New ParamInfo With {.Name = fields(i)})
                Next
            Else
                line = peekableStreamReader.PeekLine
                Dim fields = line.Split(CType(params.ValueSeparator, Char))
                For i = 0 To fields.Length - 1
                    retval.MetaData.Add(New ParamInfo With {.Name = i.ToString})
                Next
            End If

            'Data
            Do
                line = peekableStreamReader.ReadLine
                If line Is Nothing Then Exit Do

                Dim values As New Dictionary(Of String, Object)
                Dim fields = line.Split(CType(params.ValueSeparator, Char))
                For i = 0 To fields.Length - 1
                    values.Add(retval.MetaData(i).Name, fields(i))
                Next
                retval.Data.Add(values)
            Loop

            Return retval
        End Function
    End Class
End Namespace