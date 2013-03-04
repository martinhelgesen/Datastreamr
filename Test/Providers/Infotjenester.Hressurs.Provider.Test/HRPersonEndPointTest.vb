Imports Datastreamr.Framework
Imports Infotjenester.Hressurs.Provider.Endpoints
Imports LazyFramework
Imports NSubstitute
Imports NUnit.Framework
Imports LazyFramework.Utils
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

<testfixture> Public Class HRPersonEndPointTest

    Private _sessionInstance As ClassFactory.SessionInstance

    <SetUp> Public Sub Setup()
        _sessionInstance = New ClassFactory.SessionInstance
        Dim contextMock = Substitute.For(Of IDatastreamrContext)()
        contextMock.CurrentUser.ReturnsForAnyArgs(Function(p) New User With {.Username = "testuser", .Password = "testpwd", .FTPRootCatalog = "C:\FTP"})
        ClassFactory.SetTypeInstanceForSession(Of IDatastreamrContext)(contextMock)

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

    <Test> Public Sub HRPersonEndpoint_Deliver_CallsServiceProxy()
        'Arrange
        Dim hrProxyMock = Substitute.For(Of IPerson)()
        ClassFactory.SetTypeInstanceForSession(Of IPerson)(hrProxyMock)

        'Act
        Dim endpoint As New HRPersonEndpoint
        Dim sourcedata = New DataContainer With {.Data = New List(Of Dictionary(Of String, Object))}
        Dim params = endpoint.GetParams
        params.PersonIdentifier = CType([Enum].Parse(GetType(PersonIdentifierType), "EmployeeNumber", True), PersonIdentifierType?)
        params.UnitIdentifier = CType([Enum].Parse(GetType(UnitIdentifierType), "DepartmentCode", True), UnitIdentifierType?)

        'Assert
        Dim result = endpoint.Deliver(params, sourcedata)
        hrProxyMock.Received.Import(Arg.Any(Of ImportRequest))
    End Sub

    <Test> Public Sub HRPersonEndpoint_Deliver_MapsCorrectlyToHRPerson()
        'Arrange
        Dim hrProxyMock = Substitute.For(Of IPerson)()
        ClassFactory.SetTypeInstanceForSession(Of IPerson)(hrProxyMock)

        'Act
        Dim endpoint As New HRPersonEndpoint
        Dim sourcedata = StubSourceData()
        Dim params = endpoint.GetParams
        params.PersonIdentifier = CType([Enum].Parse(GetType(PersonIdentifierType), "EmployeeNumber", True), PersonIdentifierType?)
        params.UnitIdentifier = CType([Enum].Parse(GetType(UnitIdentifierType), "DepartmentCode", True), UnitIdentifierType?)

        'Assert
        Dim result = endpoint.Deliver(params, sourcedata)
        hrProxyMock.Received.Import(Arg.Is(Of ImportRequest)(Function(p) ValidateReceivedPersons(p).All(Function(b) b = True)))
    End Sub


    Private Iterator Function ValidateReceivedPersons(ByVal importRequest As ImportRequest) As IEnumerable(Of Boolean)
        Dim persons = importRequest.messageRequest.Persons
        Yield persons.Length = 1
        Yield persons(0).FirstName = "Martin" AndAlso persons(0).LastName = "Helgesen"
    End Function

    Private Function StubSourceData() As DataContainer
        Dim dc As New DataContainer
        dc.Data = New List(Of Dictionary(Of String, Object))
        Dim dic = New Dictionary(Of String, Object) From {{"FirstName", "Martin"},
                                                          {"LastName", "Helgesen"}
                                                         }
        dc.Data.Add(dic)
        Return dc
    End Function


    Private Function HRPersonParamsHasDefaultValues(ByVal params As HRPersonParams) As Boolean
        If params.Username <> DatastreamrContext.Current.CurrentUser.Username Then Return False
        If params.Password IsNot Nothing Then Return False
        If params.PersonIdentifier IsNot Nothing Then Return False
        If params.UnitIdentifier IsNot Nothing Then Return False
        Return True
    End Function


End Class
