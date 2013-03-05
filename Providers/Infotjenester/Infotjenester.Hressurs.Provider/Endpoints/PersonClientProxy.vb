Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Namespace Endpoints

    Public Interface IHRPersonProxy
        Function Import(ByVal importRequest As ImportPersonRequest, ByVal username As String, ByVal password As String) As ImportPersonResponse
    End Interface

    Public Class PersonClientProxy
        Implements IHRPersonProxy

        Public Function Import(ByVal importRequest As ImportPersonRequest, ByVal username As String, ByVal password As String) As ImportPersonResponse Implements IHRPersonProxy.Import
            Dim service = New PersonClient("BasicHttpBinding_IPerson")
            service.ClientCredentials.UserName.UserName = username
            service.ClientCredentials.UserName.Password = password
            Return service.Import(importRequest)
        End Function
    End Class
End Namespace