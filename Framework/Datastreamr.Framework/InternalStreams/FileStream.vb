Imports Datastreamr.Framework.Utils
Imports System.IO
Imports LazyFramework
Imports System.Text.RegularExpressions

Namespace InternalStreams
    Public Class FtpFileStream
        Inherits BaseDataStream(Of FtpFileStreamParams)

        Private _fileHelper As IFileHelper = ClassFactory.GetTypeInstance(Of IFileHelper, FileHelperInternal)()

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Returns content of file in users Datastreamr FTP catalog"
            End Get
        End Property

        Public Overrides Function GetStreamInternal(params As StreamParams) As DataContainer
            Dim p = New FtpFileStreamParams(params)
            Dim currentUser = DatastreamrContext.Current.CurrentUser
            Dim rootCat = currentUser.FTPRootCatalog + "\incoming\"
            Dim sr = FindFile(rootCat, p.FilenameMatch)
            Return ConvertToDataContainer(sr, p)
        End Function

        Private Function ConvertToDataContainer(ByVal streamReader As StreamReader, ByVal params As FtpFileStreamParams) _
            As DataContainer
            If streamReader Is Nothing Then
                Return Nothing
            End If
            Dim peekableStreamReader = New PeekableStreamReaderAdapter(streamReader)
            Dim _
                retval As _
                    New DataContainer _
                    With {.MetaData = New List(Of PropertyDesc), .Data = New List(Of Dictionary(Of String, Object))}

            'Metadata
            Dim line As String
            If params.FirstLineIsHeader Then
                line = peekableStreamReader.ReadLine
                Dim fields = line.Split(CType(params.ValueSeparator, Char))
                For i = 0 To fields.Length - 1
                    retval.MetaData.Add(New PropertyDesc With {.Name = fields(i)})
                Next
            Else
                line = peekableStreamReader.PeekLine
                Dim fields = line.Split(CType(params.ValueSeparator, Char))
                For i = 0 To fields.Length - 1
                    retval.MetaData.Add(New PropertyDesc With {.Name = i.ToString})
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

        Private Function FindFile(ByVal path As String, ByVal filenameMatch As String) As StreamReader
            If String.IsNullOrEmpty(path) Then
                Return Nothing
            End If
            Dim files = _fileHelper.GetFiles(path)
            Dim filteredFiles = FilterWithRegex(files, filenameMatch)
            If filteredFiles Is Nothing Then
                Return Nothing
            End If
            Return _fileHelper.OpenFile(filteredFiles(0))
        End Function

        Private Function FilterWithRegex(ByVal strings As String(), ByVal filenameMatch As String) As String()
            If filenameMatch Is Nothing Then
                Return strings
            End If
            Dim RegexObj As New Regex(filenameMatch)
            Dim list As New List(Of String)
            For i As Integer = 0 To strings.Length - 1
                If RegexObj.IsMatch(strings(i)) Then
                    list.Add(strings(i))
                End If
            Next
            Return list.ToArray
        End Function


        Public Overrides ReadOnly Property Name As String
            Get
                Return "Datastreamr filestream reader"
            End Get
        End Property
    End Class

    Public Class FtpFileStreamParams
        Inherits StreamParams

        Public Sub New()
            Add("FilenameMatch",
                New ParamInfo _
                   With {.Required = False, .Name = "FilenameMatch", .Type = GetType(String),
                   .Description = "Which file to choose"})
            Add("FirstLineIsHeader",
                New ParamInfo _
                   With {.Required = False, .Name = "FirstLineIsHeader", .Type = GetType(Boolean),
                   .Description = "Is first line header?", .DefaultValue = False})
            Add("ValueSeparator",
                New ParamInfo _
                   With {.Required = False, .Name = "ValueSeparator", .Type = GetType(String),
                   .Description = "ValueSeparator of file"})
            Add("FixedPositionDescriptor",
                New ParamInfo _
                   With {.Required = False, .Name = "FixedPositionDescriptor", .Type = GetType(String),
                   .Description = "Enter fixed position lengths here separated by comma"})
        End Sub

        Public Sub New(ByVal streamParams As StreamParams)
            MyBase.New(streamParams)
        End Sub

        Public Property FilenameMatch() As String
            Get
                If Me("FilenameMatch").Value Is Nothing Then Return CType(Me("FilenameMatch").DefaultValue, String)
                Return CType(Me("FilenameMatch").Value, String)
            End Get
            Set(value As String)
                Me("FilenameMatch").Value = value
            End Set
        End Property

        Property FirstLineIsHeader As Boolean
            Get
                If Me("FirstLineIsHeader").Value Is Nothing Then _
                    Return CType(Me("FirstLineIsHeader").DefaultValue, Boolean)
                Return CType(Me("FirstLineIsHeader").Value, Boolean)
            End Get
            Set(value As Boolean)
                Me("FirstLineIsHeader").Value = value
            End Set
        End Property

        Property ValueSeparator As String
            Get
                If Me("ValueSeparator").Value Is Nothing Then Return CType(Me("ValueSeparator").DefaultValue, String)
                Return CType(Me("ValueSeparator").Value, String)
            End Get
            Set(value As String)
                Me("ValueSeparator").Value = value
            End Set
        End Property

        Property FixedPositionDescriptor As String
            Get
                If Me("FixedPositionDescriptor").Value Is Nothing Then _
                    Return CType(Me("FixedPositionDescriptor").DefaultValue, String)
                Return CType(Me("FixedPositionDescriptor").Value, String)
            End Get
            Set(value As String)
                Me("FixedPositionDescriptor").Value = value
            End Set
        End Property
    End Class
End Namespace