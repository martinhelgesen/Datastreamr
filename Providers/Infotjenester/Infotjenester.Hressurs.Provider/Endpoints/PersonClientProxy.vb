Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Namespace Endpoints

    Public Interface IHRPersonProxy
        Function Import(request As ImportRequest) As ImportResponse
    End Interface

    Public Class PersonClientProxy
        Implements IHRPersonProxy
        Public Function Import(request As ImportRequest) As ImportResponse Implements IHRPersonProxy.Import
            Throw New NotImplementedException
        End Function
    End Class
End Namespace