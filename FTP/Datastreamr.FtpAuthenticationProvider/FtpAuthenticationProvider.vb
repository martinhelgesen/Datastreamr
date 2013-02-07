
Imports System.Runtime.InteropServices
Imports Microsoft.Web.FtpServer

Public Class FtpAuthenticationProvider
    Inherits BaseProvider
    Implements IFtpAuthenticationProvider
    Implements IFtpRoleProvider

    Private Shared _credentials As New Hashtable

    Public Function AuthenticateUser(ByVal sessionId As String, ByVal siteName As String, ByVal userName As String, ByVal userPassword As String, <Out> ByRef canonicalUserName As String) As Boolean Implements IFtpAuthenticationProvider.AuthenticateUser
        canonicalUserName = userName

        If _credentials.ContainsKey(userName) Then
            Dim pwd As String = CType(_credentials(userName), String)
            If String.Compare(pwd, userPassword, True) = 0 Then
                Return True
            End If
        End If
        'username is found in cache, but password is different. || username is not found
        'Call to SSO must be invoked.
        With New SSOService
            If .CreateSession(userName, userPassword) Then
                UpdateCache(userName,userPassword)
                Return True
            End If
        End With
        Return False
    End Function

    Private Sub UpdateCache(ByVal userName As String, ByVal userPassword As String)
        _credentials.Remove(userName)
        _credentials.Add(userName, userPassword)
    End Sub

    Public Function IsUserInRole(ByVal sessionId As String, ByVal siteName As String, ByVal userName As String, ByVal userRole As String) As Boolean Implements IFtpRoleProvider.IsUserInRole
        Return True
    End Function


End Class