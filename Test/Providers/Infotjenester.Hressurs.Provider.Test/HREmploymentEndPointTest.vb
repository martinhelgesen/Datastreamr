Imports Datastreamr.Framework
Imports Datastreamr.Framework.Entities
Imports Datastreamr.Provider.DataStreams
Imports Infotjenester.Hressurs.Provider.Endpoints
Imports LazyFramework
Imports NSubstitute
Imports NUnit.Framework
Imports LazyFramework.Utils
Imports Infotjenester.Hressurs.Provider.PersonServiceReference
Imports Datastreamr.Framework.Utils

<TestFixture> Public Class HREmploymentEndPointTest

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

    <Test> Public Sub HREmploymentEndpoint_UsernameIsDefaultFromContext()
        Dim endpoint As New HRChildEndpoint
        Dim params = endpoint.GetParams
        Assert.That(params.Username = DatastreamrContext.Current.CurrentUser.Username)
    End Sub

    <Test> Public Sub HREmploymentEndpoint_Parameters_DefaultValues()
        Dim endpoint As New HRPersonEndpoint
        Dim params = endpoint.GetParams
        Assert.That(HRPersonParamsHasDefaultValues(params))
    End Sub

    <Test> Public Sub HREmploymentEndpoint_Deliver_MapsCorrectlyToHRPersonAndHREmployment()
        'Arrange
        Dim hrProxyMock = Substitute.For(Of IHRPersonProxy)()
        ClassFactory.SetTypeInstanceForSession(Of IHRPersonProxy)(hrProxyMock)

        'Act
        Dim endpoint As New HREmploymentEndpoint
        Dim sourcedata = StubSourceData2()
        Dim params = endpoint.GetParams
        params.PersonIdentifier = CType([Enum].Parse(GetType(PersonIdentifierType), "EmployeeNumber", True), PersonIdentifierType?)
        params.UnitIdentifier = CType([Enum].Parse(GetType(UnitIdentifierType), "DepartmentCode", True), UnitIdentifierType?)

        'Assert
        Dim result = endpoint.Deliver(params, sourcedata)        
        hrProxyMock.ReceivedWithAnyArgs(1).Import(Nothing, "", "")
        hrProxyMock.Received.Import(Arg.Is(Of ImportPersonRequest)(Function(p) ValidateReceivedEmployee(p).All(Function(b) b = True)), Arg.Any(Of String), Arg.Any(Of String))
    End Sub

    Private Function StubSourceData2() As DataContainer
        Dim dc As New DataContainer
        dc.Data = New List(Of Dictionary(Of String, Object))

        Dim dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "2408"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "199"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utvikler"},
                                                          {"FromDate", "18.04.2012"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "100,0"},
                                                          {"DepartmentIdentifier", "IT"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "2702"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "IT"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "2702"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
                                                         }
        dc.Data.Add(dic)

        Return dc
    End Function

    <Test> Public Sub HREmploymentEndpoint_Deliver_CalledMultipleTimes()
        'Arrange
        Dim hrProxyMock = Substitute.For(Of IHRPersonProxy)()
        ClassFactory.SetTypeInstanceForSession(Of IHRPersonProxy)(hrProxyMock)

        'Act
        Dim endpoint As New HREmploymentEndpoint
        Dim sourcedata = StubSourceData()
        Dim params = endpoint.GetParams
        params.PersonIdentifier = CType([Enum].Parse(GetType(PersonIdentifierType), "EmployeeNumber", True), PersonIdentifierType?)
        params.UnitIdentifier = CType([Enum].Parse(GetType(UnitIdentifierType), "DepartmentCode", True), UnitIdentifierType?)

        'Assert
        Dim result = endpoint.Deliver(params, sourcedata)
        'hrProxyMock.ReceivedWithAnyArgs.Import(Nothing, "", "")
        hrProxyMock.ReceivedWithAnyArgs(2).Import(Nothing, "", "")
        hrProxyMock.Received.Import(Arg.Is(Of ImportPersonRequest)(Function(p) ValidateFromCalledMultipleTimes(p).All(Function(b) b = True)), Arg.Any(Of String), Arg.Any(Of String))
    End Sub


    Private Iterator Function ValidateReceivedEmployee(ByVal importRequest As ImportPersonRequest) As IEnumerable(Of Boolean)
        Dim persons = importRequest.Persons
        Yield persons.Length = 2
        Yield persons.FirstOrDefault(Function(p) p.PersonIdentifier.Value = "2408").EmploymentInfo.Length = 1

        Dim employee = persons.FirstOrDefault(Function(p) p.PersonIdentifier.Value = "2408").EmploymentInfo(0)
        Yield employee.EmployedIn.Value = "ITAS"
        Yield employee.EmployeeNumber = "199"
        Dim employment = employee.Employment(0)
        Yield employment.Category.Name = "F"
        Yield employment.FromDate = CDate("18.04.2012")
        Yield employment.Position.Name = "Utvikler"
        Yield employment.ToDate Is Nothing
        Yield employment.EmploymentDistributionList(0).Unit.Value = "IT"
        Yield employment.EmploymentDistributionList(0).PositionPercent = 100D

        Yield persons.FirstOrDefault(Function(p) p.PersonIdentifier.Value = "2702").EmploymentInfo.Length = 1
        employee = persons.FirstOrDefault(Function(p) p.PersonIdentifier.Value = "2702").EmploymentInfo(0)
        Yield employee.EmployedIn.Value = "ITAS"
        Yield employee.EmployeeNumber = "200"
        employment = employee.Employment(0)
        Yield employment.Category.Name = "F"
        Yield employment.FromDate = CDate("20.01.2010")
        Yield employment.Position.Name = "Utviklersjef"
        Yield employment.ToDate Is Nothing
        Yield employment.EmploymentDistributionList(0).Unit.Value = "IT"
        Yield employment.EmploymentDistributionList(0).PositionPercent = 50D
        Yield employment.EmploymentDistributionList(1).Unit.Value = "SALG"
        Yield employment.EmploymentDistributionList(1).PositionPercent = 50D
    End Function
    Private Iterator Function ValidateFromCalledMultipleTimes(ByVal importRequest As ImportPersonRequest) As IEnumerable(Of Boolean)
        Dim persons = importRequest.Persons
        Yield persons.Length = 2
        Yield persons.FirstOrDefault(Function(p) p.PersonIdentifier.Value = "jgr").EmploymentInfo.Length = 1
    End Function

    Private Function StubSourceData() As DataContainer
        Dim dc As New DataContainer
        dc.Data = New List(Of Dictionary(Of String, Object))

        Dim dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "2408"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "199"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utvikler"},
                                                          {"FromDate", "18.04.2012"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "100,0"},
                                                          {"DepartmentIdentifier", "IT"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "2702"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "IT"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "1234"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "5678"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "asd"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "34rf"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "45y"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "j65r"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "i7t"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "34yerg"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "jgr"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"PersonIdentifier", "6ijt"},
                                                          {"CompanyIdentifier", "ITAS"},
                                                          {"EmployeeNumber", "200"},
                                                          {"EmployeeCategory", "F"},
                                                          {"Position", "Utviklersjef"},
                                                          {"FromDate", "20.01.2010"},
                                                          {"EndDate", ""},
                                                          {"PositionPercent", "50,0"},
                                                          {"DepartmentIdentifier", "SALG"}
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
