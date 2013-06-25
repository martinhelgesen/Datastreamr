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

<TestFixture> Public Class HRChildEndPointTest

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

    <Test> Public Sub HRChildEndpoint_UsernameIsDefaultFromContext()
        Dim endpoint As New HRChildEndpoint
        Dim params = endpoint.GetParams
        Assert.That(params.Username = DatastreamrContext.Current.CurrentUser.Username)
    End Sub
    <Test> Public Sub HRChildEndpoint_Parameters_DefaultValues()
        Dim endpoint As New HRPersonEndpoint
        Dim params = endpoint.GetParams
        Assert.That(HRPersonParamsHasDefaultValues(params))
    End Sub

    <Test> Public Sub HRChildEndpoint_Deliver_CallsServiceProxy()
        'Arrange
        Dim hrProxyMock = Substitute.For(Of IHRPersonProxy)()
        ClassFactory.SetTypeInstanceForSession(Of IHRPersonProxy)(hrProxyMock)

        'Act
        Dim endpoint As New HRPersonEndpoint
        Dim sourcedata = New DataContainer
        Dim params = endpoint.GetParams
        params.PersonIdentifier = CType([Enum].Parse(GetType(PersonIdentifierType), "EmployeeNumber", True), PersonIdentifierType?)
        params.UnitIdentifier = CType([Enum].Parse(GetType(UnitIdentifierType), "DepartmentCode", True), UnitIdentifierType?)

        'Assert
        Dim result = endpoint.Deliver(params, sourcedata)
        hrProxyMock.Received.Import(Arg.Any(Of ImportPersonRequest), Arg.Any(Of String), Arg.Any(Of String))
    End Sub

    <Test> Public Sub HRChildEndpoint_Deliver_MapsCorrectlyToHRPersonAndHRChild()
        'Arrange
        Dim hrProxyMock = Substitute.For(Of IHRPersonProxy)()
        ClassFactory.SetTypeInstanceForSession(Of IHRPersonProxy)(hrProxyMock)

        'Act
        Dim endpoint As New HRPersonEndpoint
        Dim sourcedata = StubSourceData()
        Dim params = endpoint.GetParams
        params.PersonIdentifier = CType([Enum].Parse(GetType(PersonIdentifierType), "EmployeeNumber", True), PersonIdentifierType?)
        params.UnitIdentifier = CType([Enum].Parse(GetType(UnitIdentifierType), "DepartmentCode", True), UnitIdentifierType?)

        'Assert
        Dim result = endpoint.Deliver(params, sourcedata)
        hrProxyMock.ReceivedWithAnyArgs.Import(Nothing, "", "")
        hrProxyMock.Received.Import(Arg.Is(Of ImportPersonRequest)(Function(p) ValidateReceivedChildren(p).All(Function(b) b = True)), Arg.Any(Of String), Arg.Any(Of String))
    End Sub

    <Test> Public Sub Deliver_TestInternalMapping()
        'Arrange Job
        Dim jobmock = NSubstitute.Substitute.For(Of IJobEntityDataAcces)()
        jobmock.WhenForAnyArgs(Sub(p) p.GetInstance("", "1", Nothing)).Do(Sub(p)
                                                                              Dim j = CType(p(2), JobEntity)
                                                                              j.DataStreamTypeName = GetType(FtpFileStream).AssemblyQualifiedName
                                                                              j.EndpointTypeName = GetType(HRPersonEndpoint).AssemblyQualifiedName
                                                                              j.DataStreamParams = New FtpFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False}
                                                                              j.EndpointParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}

                                                                              Dim ret As New MapConfig
                                                                              ret.Add("0", "Identifier", Nothing)
                                                                              ret.Add("0", "EmployeeNo", Nothing)
                                                                              ret.Add("1", "FirstName", "return originalValue.split(' ')[0];")
                                                                              ret.Add("1", "LastName", "return originalValue.split(' ')[1];")
                                                                              ret.Add("2", "PersonalNo", Nothing)
                                                                              ret.Add("3", "Email", Nothing)
                                                                              ret.Add("4", "Phone", Nothing)
                                                                              ret.Add("5", "PhonePrivate", Nothing)
                                                                              ret.Add("6", "Mobile", Nothing)
                                                                              ret.Add("7", "DepartmentIdentifier", Nothing)
                                                                              'ret.Add("8", "Account", Nothing)
                                                                              ret.Add("9", "EmployeeCategory", Nothing)
                                                                              ret.Add("10", "EmployeePosition", Nothing)
                                                                              ret.Add("11", "EmploymentStartDate", Nothing)
                                                                              j.Mapconfig = ret
                                                                          End Sub)

        ClassFactory.SetTypeInstanceForSession(Of IJobEntityDataAcces)(jobmock)
        Dim hrPersonProxy As IHRPersonProxy = Substitute.For(Of IHRPersonProxy)()
        ClassFactory.SetTypeInstanceForSession(Of IHRPersonProxy)(hrPersonProxy)

        'Arrange Filestream
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"SemicolonNoHeader"})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) StreamHelper.GenerateStreamReaderFromString(Datastreamr.Framework.Test.My.Resources.Semicolon_NoHeader))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim job = Facade.JobFacade.GetJob("1")
        Dim JobExecutor = New JobExecutor(job)
        Dim result = JobExecutor.Execute()


        'Assert
        Assert.AreEqual(True, result.Success)
        hrPersonProxy.Received.Import(
            Arg.Is(Of ImportPersonRequest)(Function(p) ValidateReceivedPersons_AllFields(p).All(Function(b) b)),
            Arg.Any(Of String),
            Arg.Any(Of String))
    End Sub

    Private Iterator Function ValidateReceivedChildren(ByVal importRequest As ImportPersonRequest) As IEnumerable(Of Boolean)
        Dim persons = importRequest.Persons
        Yield persons.Length = 1
        Yield persons(0).Children.Count = 3
    End Function
    Private Iterator Function ValidateReceivedPersons_AllFields(ByVal importRequest As ImportPersonRequest) As IEnumerable(Of Boolean)
        Dim persons = importRequest.Persons
        Yield persons.Length = 3
        Yield persons(0).FirstName = "Martin"
        Yield persons(0).LastName = "Helgesen"
        Yield persons(0).SocialSecurityNumber Is Nothing
        Yield persons(0).EmploymentInfo(0).EmployeeNumber = "1962"
        Yield persons(0).EMailAddresses(0).Address = "test1@test.no"
        Yield persons(0).Phones(0).Number = "69971705"
        Yield persons(0).Phones(1).Number = "69157337"
        Yield persons(0).Phones(2).Number = "97125917"
        Yield persons(0).ParentUnitIdentifier.Value = "23"
        Yield persons(0).EmploymentInfo(0).Employment(0).Category.Name = "Fast stilling"
        Yield persons(0).EmploymentInfo(0).Employment(0).Position.Name.Trim = "FAGKONSULENT (FORRETNINGSUTVIKLING)"
        Yield persons(0).EmploymentInfo(0).Employment(0).FromDate = Date.Parse("01.04.2007")
    End Function

    Private Function StubSourceData() As DataContainer
        Dim dc As New DataContainer
        dc.Data = New List(Of Dictionary(Of String, Object))
        Dim dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "2408"},
                                                          {"FirstName", "Andreas"},
                                                          {"LastName", "Helgesen"}
                                                         }
        dc.Data.Add(dic)
        dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "2408"},
                                                          {"FirstName", "Pia"},
                                                          {"LastName", "Andersen"}
                                                         }
        dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "2408"},
                                                          {"FirstName", "Jesper"},
                                                          {"LastName", "Andersen"}
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

    <Test> Public Sub TestSortAndGroupDictionary()
        Dim dc As New DataContainer
        dc.Data = New List(Of Dictionary(Of String, Object))

        Dim dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "2408"},
                                                          {"FirstName", "Andreas"},
                                                          {"LastName", "Helgesen"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "1911"},
                                                          {"FirstName", "Pia"},
                                                          {"LastName", "Andersen"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "2408"},
                                                          {"FirstName", "Jesper"},
                                                          {"LastName", "Andersen"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "2011"},
                                                          {"FirstName", "Anette"},
                                                          {"LastName", "Helgesen"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "2408"},
                                                          {"FirstName", "Jesper"},
                                                          {"LastName", "Andersen"}
                                                         }
        dc.Data.Add(dic)


    End Sub

End Class
