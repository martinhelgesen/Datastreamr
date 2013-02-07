Imports System.IO
Imports Microsoft.Web.FtpServer


Public Class FtpHomeDirectoryProvider
    Inherits BaseProvider
    Implements IFtpHomeDirectoryProvider

    Function IFtpHomeDirectoryProvider_GetUserHomeDirectoryData(ByVal sessionId As String, ByVal siteName As String, ByVal userName As String) As String Implements IFtpHomeDirectoryProvider.GetUserHomeDirectoryData
        'Create user directory if not exists

        Dim path = "C:\CustomerFtpFiles\" + userName
        CreateDirectoryStructureIfNotExists(path)

        Return path
    End Function

    Private Sub CreateDirectoryStructureIfNotExists(ByVal path As String)
        If Not Directory.Exists(path) Then
            Directory.CreateDirectory(path)
            Directory.CreateDirectory(path + "\incoming")
            Directory.CreateDirectory(path + "\outgoing")
            Directory.CreateDirectory(path + "\error")
            Directory.CreateDirectory(path + "\error\incoming")
            Directory.CreateDirectory(path + "\error\outgoing")
        End If
    End Sub
End Class
