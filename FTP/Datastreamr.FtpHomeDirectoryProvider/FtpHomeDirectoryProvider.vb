Imports System
Imports System.Security.AccessControl
Imports Microsoft.Web.FtpServer


Public Class FtpHomeDirectoryProvider
    Inherits BaseProvider
    Implements IFtpHomeDirectoryProvider

    Function IFtpHomeDirectoryProvider_GetUserHomeDirectoryData(ByVal sessionId As String, ByVal siteName As String, ByVal userName As String) As String Implements IFtpHomeDirectoryProvider.GetUserHomeDirectoryData
        Const errorString As String = "!ERROR!:"

        'Create user directory if not exists
        'Dim userNameWithoutMachineName = userName.Split("\"c)(1)
        Dim userNameWithoutMachineName = userName
        Dim IsError = userNameWithoutMachineName.StartsWith(errorString)
        If IsError Then
            userNameWithoutMachineName = userNameWithoutMachineName.Replace(errorString, "")
        End If

        Dim path = "C:\CustomerFtpFiles\" + userNameWithoutMachineName
        CreateDirectoryStructureIfNotExists(path)

        If IsError Then
            Return path + "\error"
        End If
        Return path
    End Function

    Private Sub CreateDirectoryStructureIfNotExists(ByVal path As String)
        If Not System.IO.Directory.Exists(path) Then
            System.IO.Directory.CreateDirectory(path)
            System.IO.Directory.CreateDirectory(path + "\incoming")
            System.IO.Directory.CreateDirectory(path + "\outgoing")
            System.IO.Directory.CreateDirectory(path + "\error")
            System.IO.Directory.CreateDirectory(path + "\error\incoming")
            System.IO.Directory.CreateDirectory(path + "\error\outgoing")
        End If
    End Sub
End Class
