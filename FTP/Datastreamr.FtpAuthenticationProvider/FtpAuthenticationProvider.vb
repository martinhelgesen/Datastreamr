Imports System
Imports System.Runtime.InteropServices
Imports Microsoft.Web.FtpServer

Public Class FtpAuthenticationProvider
    Inherits BaseProvider
    Implements IFtpAuthenticationProvider
    Implements IFtpRoleProvider

    Public Function AuthenticateUser(ByVal sessionId As String, ByVal siteName As String, ByVal userName As String, ByVal userPassword As String, <Out> ByRef canonicalUserName As String) As Boolean Implements IFtpAuthenticationProvider.AuthenticateUser
        ' Note: You would add your own custom logic here.
        canonicalUserName = userName
        Dim strUserName As String = "MyUser"
        Dim strPassword As String = "MyPassword"

        ' Verify that the user name and password are valid.
        ' Note: In this example, the user name is case-insensitive
        ' and the password is case-sensitive.
        If ((userName.Equals(strUserName, StringComparison.OrdinalIgnoreCase)) = True) AndAlso userPassword = strPassword Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function IsUserInRole(ByVal sessionId As String, ByVal siteName As String, ByVal userName As String, ByVal userRole As String) As Boolean Implements IFtpRoleProvider.IsUserInRole
        ' Note: You would add your own custom logic here.
        Dim strUserName As String = "MyUser"
        Dim strRoleName As String = "MyRole"

        ' Verify that the user name and role name are valid.
        ' Note: In this example, both the user name and
        ' the role name are case-insensitive.
        If ((userName.Equals(strUserName, StringComparison.OrdinalIgnoreCase)) = True) AndAlso ((userRole.Equals(strRoleName, StringComparison.OrdinalIgnoreCase)) = True) Then
            Return True
        Else
            Return False
        End If
    End Function


End Class
