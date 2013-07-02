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
                Dim fields As String() = Nothing
                If Not String.IsNullOrEmpty(params.ValueSeparator) Then
                    fields = line.Split(CType(params.ValueSeparator, Char))
                ElseIf Not String.IsNullOrEmpty(params.FixedPositionDescriptor) Then
                    fields = params.FixedPositionDescriptor.Split(","c)
                End If

                For i = 0 To fields.Length - 1
                    retval.MetaData.Add(New ParamInfo With {.Name = i.ToString})
                Next
            End If

            'Data
            Do
                line = peekableStreamReader.ReadLine
                If line Is Nothing Then Exit Do
                Dim values As New Dictionary(Of String, Object)

                If Not String.IsNullOrEmpty(params.ValueSeparator) Then
                    Dim fields = line.Split(CType(params.ValueSeparator, Char))
                    For i = 0 To fields.Length - 1
                        values.Add(retval.MetaData(i).Name, fields(i))
                    Next
                End If

                If Not String.IsNullOrEmpty(params.FixedPositionDescriptor) Then
                    Dim lengths = params.FixedPositionDescriptor.Split(","c)
                    Dim start = 0
                    For i = 0 To lengths.Length - 1
                        If start + CInt(lengths(i)) > line.Length Then
                            Exit For
                        End If
                        values.Add(retval.MetaData(i).Name, line.Substring(start, CInt(lengths(i))))
                        start = start + CInt(lengths(i))
                    Next
                End If
                retval.Data.Add(values)
            Loop

            Return retval
        End Function
    End Class
End Namespace