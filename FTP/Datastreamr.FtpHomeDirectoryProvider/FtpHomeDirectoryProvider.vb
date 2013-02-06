Imports System
Imports System.Security.AccessControl
Imports Microsoft.Web.FtpServer


Public Class FtpHomeDirectoryProvider
    Inherits BaseProvider
    Implements IFtpHomeDirectoryProvider

    Function IFtpHomeDirectoryProvider_GetUserHomeDirectoryData(ByVal sessionId As String, ByVal siteName As String, ByVal userName As String) As String Implements IFtpHomeDirectoryProvider.GetUserHomeDirectoryData

        'Create user directory if not exists
        Dim path = "C:\CustomerFtpFiles\" + userName.Split(CType("\", Char))(1)
        If Not System.IO.Directory.Exists(path) Then
            System.IO.Directory.CreateDirectory(path)
            System.IO.Directory.CreateDirectory(path + "\incoming")
            System.IO.Directory.CreateDirectory(path + "\outgoing")
            System.IO.Directory.CreateDirectory(path + "\error")
        End If
        ' Return the user's home directory based on their user name.
        Return (path)

    End Function

End Class
