Imports System.IO

Namespace Utils
    Public Class FileHelperInternal
        Implements IFileHelper

        Public Function GetFiles(path As String) As String() Implements IFileHelper.GetFiles
            Return Directory.GetFiles(path)
        End Function

        Public Function OpenFile(path As String) As StreamReader Implements IFileHelper.OpenFile
            Return New StreamReader(path, Text.Encoding.GetEncoding("iso-8859-1"))
        End Function
    End Class

    Public Interface IFileHelper
        Function GetFiles(path As String) As String()
        Function OpenFile(ByVal path As String) As StreamReader
    End Interface
End Namespace