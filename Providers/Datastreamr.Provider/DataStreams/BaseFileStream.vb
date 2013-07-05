Imports Datastreamr.Framework.DataStreams
Imports Datastreamr.Framework.Utils
Imports Datastreamr.Framework
Imports System.IO
Imports System.Text.RegularExpressions
Imports LazyFramework

Namespace DataStreams
    ''' <summary>
    ''' Responsible for opening file content
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BaseFileStream(Of T As {BaseFileStreamParams, New})
        Inherits StreamReaderStream(Of T)

        Private ReadOnly _fileHelper As IFileHelper = ClassFactory.GetTypeInstance(Of IFileHelper, FileHelperInternal)()

        Public Function GetfileContent(params As T) As StreamReader
            Dim currentUser = DatastreamrContext.Current.CurrentUser
            Dim rootCat = currentUser.RootPath + "\incoming"
            Return FindFile(rootCat, params.FilenameMatch)
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
            Dim streamReader As StreamReader = Nothing
            Try
                streamReader = _fileHelper.OpenFile(filteredFiles(0))
                If streamReader Is Nothing Then
                    Throw New ApplicationException("Streamreader is nothing")
                End If
                Return streamReader
            Catch ex As Exception
                Throw New CouldNotOpenFileException(ex)
            End Try
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

    End Class

    Public Class CouldNotOpenFileException
        Inherits Exception

        Public Sub New(ByVal exception As Exception)
            MyBase.New("Could not open file", exception)
        End Sub
    End Class
End Namespace


'          