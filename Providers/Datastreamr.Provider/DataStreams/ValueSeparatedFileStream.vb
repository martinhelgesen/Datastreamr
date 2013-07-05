Imports Datastreamr.Framework

Namespace DataStreams
    Public Class ValueSeparatedFileStream
        Inherits FlatFileStream(Of ValueSeparatedFileStreamParams)

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Value sep desc"
            End Get
        End Property

        'Public Overrides Function GetStreamInternal(ByVal params As BaseStreamParams) As DataContainer
        '    Dim p = New ValueSeparatedFileStreamParams(params)
        '    Dim dc As DataContainer = Nothing
        '    Using sr = GetfileContent(p)
        '        dc = ConvertToDataContainer(sr, p)
        '    End Using
        '    Return dc
        'End Function

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Value sep name"
            End Get
        End Property

        Protected Overrides Sub CreateMetaData(firstLine As String)

            If StreamParams.FirstLineIsHeader Then
                Dim fields = firstLine.Split(CType(StreamParams.ValueSeparator, Char))
                For i = 0 To fields.Length - 1
                    Data.MetaData.Add(New ParamInfo With {.Name = fields(i)})
                Next
            Else
                Dim fields As String() = Nothing
                If Not String.IsNullOrEmpty(StreamParams.ValueSeparator) Then
                    fields = firstLine.Split(CType(StreamParams.ValueSeparator, Char))
                End If

                For i = 0 To fields.Length - 1
                    Data.MetaData.Add(New ParamInfo With {.Name = i.ToString})
                Next
            End If
        End Sub

        Protected Overrides Sub FillDataContainer(line As String)
            Dim values As New Dictionary(Of String, Object)

            If Not String.IsNullOrEmpty(StreamParams.ValueSeparator) Then
                Dim fields = line.Split(CType(StreamParams.ValueSeparator, Char))
                For i = 0 To fields.Length - 1
                    values.Add(Data.MetaData(i).Name, fields(i))
                Next
            End If
            Data.Data.Add(values)
        End Sub
    End Class
End NameSpace