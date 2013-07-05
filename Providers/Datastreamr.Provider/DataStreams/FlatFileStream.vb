Imports Datastreamr.Framework
Imports Newtonsoft.Json

Namespace DataStreams
    ''' <summary>
    ''' Base class for Flatfiles
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <remarks></remarks>
    Public MustInherit Class FlatFileStream(Of T As {FlatFileStreamParams, New})
        Inherits BaseFileStream(Of T)

        Public Overrides Function GetStreamInternal() As DataContainer
            Dim firstLine As Boolean = True
            For Each line In Lines(StreamParams)
                If firstLine Then
                    CreateMetaData(line)
                    If Not StreamParams.FirstLineIsHeader Then
                        FillDataContainer(line)
                    End If
                    firstLine = False
                    Continue For
                End If
                FillDataContainer(line)
            Next

            Return Data
        End Function

        Private Iterator Function Lines(p As FlatFileStreamParams) As IEnumerable(Of String)
            Using sr = GetfileContent(CType(p, T))
                Do
                    Dim line = sr.ReadLine
                    If line Is Nothing Then Exit Do
                    Yield line
                Loop
            End Using
        End Function

        Protected MustOverride Sub FillDataContainer(line As String)
        Protected MustOverride Sub CreateMetaData(firstLine As String)

        <JsonIgnore>
        Protected Property Data As New DataContainer

    End Class
End NameSpace