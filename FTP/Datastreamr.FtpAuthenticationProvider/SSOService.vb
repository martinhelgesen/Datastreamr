Imports System.Net
Imports System.Web

Public Class SSOService

    Public Function CreateSession(userName As String, password As String) As Boolean
        Dim crypto As New Crypto
        'Dim passwordHash = crypto.ComputeMD5HashWithSecretSalt(password.ToLower())
        Dim qs As String = String.Format("{0}|{1}", userName, password)

        qs = crypto.EncryptData(qs)
        qs = HttpUtility.UrlEncode(qs)

        Dim wr = WebRequest.Create(String.Format("https://logon.infotjenester.no/CreateFtpSession.aspx?{0}", qs))

        Dim response As HttpWebResponse = CType(wr.GetResponse(), HttpWebResponse)
        Dim streamReader As New IO.StreamReader(response.GetResponseStream)

        If response.StatusCode = HttpStatusCode.OK Then
            Dim g = streamReader.ReadToEnd()
            g = Web.HttpContext.Current.Server.UrlDecode(g)
            Dim guid = New Guid(g)
            Return True
        End If
        Return False
    End Function

End Class