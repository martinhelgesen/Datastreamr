Imports Datastreamr.Framework.DataStreams
Imports Datastreamr.Framework.Utils
Imports Datastreamr.Framework
Imports System.IO
Imports System.Text.RegularExpressions
Imports LazyFramework

Namespace DataStreams
    Public Class FtpFileStream
        Inherits StreamReaderStream(Of FtpFileStreamParams)

        Private _fileHelper As IFileHelper = ClassFactory.GetTypeInstance(Of IFileHelper, FileHelperInternal)()

        Public Overrides Function GetStreamInternal(params As StreamParams) As DataContainer
            Dim p = New FtpFileStreamParams(params)
            Dim currentUser = DatastreamrContext.Current.CurrentUser
            Dim rootCat = currentUser.FTPRootCatalog + "\incoming"
            Dim dc As DataContainer = Nothing
            Using sr = FindFile(rootCat, p.FilenameMatch)
                dc = ConvertToDataContainer(sr, p)
            End Using
            Return dc
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

        Public Overrides ReadOnly Property Description As String
            Get
                Return "Returns content of file in users Datastreamr FTP catalog"
            End Get

        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Datastreamr filestream reader"
            End Get
        End Property
    End Class
End NameSpace