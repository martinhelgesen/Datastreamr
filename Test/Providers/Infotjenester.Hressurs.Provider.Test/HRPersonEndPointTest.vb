Imports Datastreamr.Framework
Imports Infotjenester.Hressurs.Provider.Endpoints
Imports LazyFramework
Imports NUnit.Framework
Imports LazyFramework.Utils

<testfixture> Public Class HRPersonEndPointTest

    Private _sessionInstance As ClassFactory.SessionInstance
    Private Const _datastreamrcontextSlotName As String = "DatastreamrContext"

    <SetUp> Public Sub Setup()
        _sessionInstance = New ClassFactory.SessionInstance
        ResponseThread.SetThreadValue(_datastreamrcontextSlotName, New DatastreamrContext With {.CurrentUser = New User With {.Username = "testuser", .Password = "testpwd", .FTPRootCatalog = "C:\FTP"}})
    End Sub
    <TearDown> Public Sub TearDown()
        _sessionInstance = Nothing
    End Sub

    <Test> Public Sub HRPersonEndpoint_UsernameIsDefaultFromContext()
        Dim endpoint As New HRPersonEndpoint
        Dim params = endpoint.GetParams
        Assert.That(params.Username = DatastreamrContext.Current.CurrentUser.Username)
    End Sub
    <Test> Public Sub HRPersonEndpoint_Parameters_DefaultValues()
        Dim endpoint As New HRPersonEndpoint
        Dim params = endpoint.GetParams
        Assert.That(HRPersonParamsHasDefaultValues(params))
    End Sub
    <Test> Public Sub HRPersonEndpoint_DictionaryMapsCorrectToHRPerson()
        Dim endpoint As New HRPersonEndpoint
        Dim sourcedata = New DataContainer With {.Data = New List(Of Dictionary(Of String, Object))}
        Dim params = endpoint.GetParams
        'Dim result = endpoint.Deliver(params, sourcedata)
        Assert.AreEqual(1, 2)
    End Sub


    Private Function HRPersonParamsHasDefaultValues(ByVal params As HRPersonParams) As Boolean
        If params.Username <> DatastreamrContext.Current.CurrentUser.Username Then Return False
        If params.Password IsNot Nothing Then Return False
        If params.PersonIdentifier IsNot Nothing Then Return False
        If params.UnitIdentifier IsNot Nothing Then Return False
        Return True
    End Function


End Class
