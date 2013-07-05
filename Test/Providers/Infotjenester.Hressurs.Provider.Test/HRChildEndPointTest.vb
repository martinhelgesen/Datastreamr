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
        contextMock.CurrentUser.ReturnsForAnyArgs(Function(p) New User With {.Username = "testuser", .Password = "testpwd", .RootPath = "C:\FTP"})
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
        endpoint.StreamParams = params

        'Assert
        Dim result = endpoint.Deliver(sourcedata)
        hrProxyMock.Received.Import(Arg.Any(Of ImportPersonRequest), Arg.Any(Of String), Arg.Any(Of String))
    End Sub

    <Test> Public Sub HRChildEndpoint_Deliver_MapsCorrectlyToHRPersonAndHRChild()
        'Arrange
        Dim hrProxyMock = Substitute.For(Of IHRPersonProxy)()
        ClassFactory.SetTypeInstanceForSession(Of IHRPersonProxy)(hrProxyMock)

        'Act
        Dim endpoint As New HRChildEndpoint
        Dim sourcedata = StubSourceData()
        Dim params = endpoint.GetParams
        params.PersonIdentifier = CType([Enum].Parse(GetType(PersonIdentifierType), "EmployeeNumber", True), PersonIdentifierType?)
        params.UnitIdentifier = CType([Enum].Parse(GetType(UnitIdentifierType), "DepartmentCode", True), UnitIdentifierType?)
        endpoint.StreamParams = params

        'Assert
        Dim result = endpoint.Deliver(sourcedata)
        hrProxyMock.ReceivedWithAnyArgs.Import(Nothing, "", "")
        hrProxyMock.Received.Import(Arg.Is(Of ImportPersonRequest)(Function(p) ValidateReceivedChildren(p).All(Function(b) b = True)), Arg.Any(Of String), Arg.Any(Of String))
    End Sub

    <Test> Public Sub Deliver_TestInternalMapping()
        'Arrange Job
        Dim jobmock = NSubstitute.Substitute.For(Of IJobEntityDataAcces)()
        jobmock.WhenForAnyArgs(Sub(p) p.GetInstance("", "1", Nothing)).Do(Sub(p)
                                                                              Dim j = CType(p(2), JobEntity)
                                                                              'j.DataStreamTypeName = GetType(ValueSeparatedFileStream).AssemblyQualifiedName
                                                                              'j.EndpointTypeName = GetType(HRChildEndpoint).AssemblyQualifiedName
                                                                              j.Endpoint = New HRChildEndpoint With {.StreamParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}}

                                                                              j.DataStream = New ValueSeparatedFileStream
                                                                              j.DataStream.SetParams(New ValueSeparatedFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False})
                                                                              'j.EndpointParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}

                                                                              Dim ret As New MapConfig
                                                                              ret.Add("0", "ParentIdentifier", Nothing)
                                                                              ret.Add("1", "ParentFirstName", Nothing)
                                                                              ret.Add("2", "ParentLastName", Nothing)
                                                                              ret.Add("3", "FirstName", "var splitString = function (s) { var splitChar = ' '; if (s.indexOf(',') > 0) { splitChar = ',' } var arr = s.split(splitChar); if (arr.length > 1) { var x = arr[0]; arr.splice(0, 1); return [x, arr.join(splitChar)] } else { return [s, s] } }; return splitString(originalValue)[0];")
                                                                              ret.Add("3", "LastName", "var splitString = function (s) { var splitChar = ' '; if (s.indexOf(',') > 0) { splitChar = ',' } var arr = s.split(splitChar); if (arr.length > 1) { var x = arr[0]; arr.splice(0, 1); return [x, arr.join(splitChar)] } else { return [s, s] } }; return splitString(originalValue)[1];")
                                                                              ret.Add("4", "BirthDate", Nothing)
                                                                              j.Mapconfig = ret
                                                                          End Sub)

        ClassFactory.SetTypeInstanceForSession(Of IJobEntityDataAcces)(jobmock)
        Dim hrPersonProxy As IHRPersonProxy = Substitute.For(Of IHRPersonProxy)()
        ClassFactory.SetTypeInstanceForSession(Of IHRPersonProxy)(hrPersonProxy)

        'Arrange Filestream
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {""})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) StreamHelper.GenerateStreamReaderFromString(My.Resources.HRChild))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim job = Facade.JobFacade.GetJob("1")
        Dim JobExecutor = New JobExecutor(job)
        Dim result = JobExecutor.Execute()


        'Assert
        Assert.AreEqual(True, result.Success)
        hrPersonProxy.Received.Import(
            Arg.Is(Of ImportPersonRequest)(Function(p) ValidateReceivedChild_Mapping(p).All(Function(b) b)),
            Arg.Any(Of String),
            Arg.Any(Of String))
    End Sub

    Private Iterator Function ValidateReceivedChildren(ByVal importRequest As ImportPersonRequest) As IEnumerable(Of Boolean)
        Dim persons = importRequest.Persons
        Yield persons.Length = 3
        Yield persons.FirstOrDefault(Function(p) p.PersonIdentifier.Value = "230397").Children.Count = 1
        Yield persons.FirstOrDefault(Function(p) p.PersonIdentifier.Value = "2011").Children.Count = 2
        Yield persons.FirstOrDefault(Function(p) p.PersonIdentifier.Value = "1911").Children.Count = 2

        Dim child = persons.FirstOrDefault(Function(p) p.PersonIdentifier.Value = "230397").Children(0)
        Yield child.FirstName = "Andreas"
        Yield child.LastName = "Helgesen"
        Yield child.BirthDate Is Nothing
        Yield child.DisabledChild Is Nothing
        Yield child.Gender = Gender.NotSet
        Yield child.LivesWithParent Is Nothing
        Yield child.MiddleName Is Nothing
        Yield child.SplitCare Is Nothing
        Yield child.SplitCarePercent Is Nothing
    End Function
    Private Iterator Function ValidateReceivedChild_Mapping(ByVal importRequest As ImportPersonRequest) As IEnumerable(Of Boolean)
        Dim persons = importRequest.Persons
        Yield persons.Length = 1
        Yield persons(0).FirstName = "Age"
        Yield persons(0).LastName = "Huskesen"
        Yield persons(0).Children.Count = 1
        Yield persons(0).Children(0).FirstName = "Andreas"
        Yield persons(0).Children(0).LastName = "Helgesen"
        Yield persons(0).Children(0).BirthDate = CDate("19.04.2012")
        Yield persons(0).Children(0).DisabledChild Is Nothing
        Yield persons(0).Children(0).Gender = Gender.NotSet
        Yield persons(0).Children(0).LivesWithParent Is Nothing
        Yield persons(0).Children(0).MiddleName Is Nothing
        Yield persons(0).Children(0).SplitCare Is Nothing
        Yield persons(0).Children(0).SplitCarePercent Is Nothing
    End Function

    Private Function StubSourceData() As DataContainer
        Dim dc As New DataContainer
        dc.Data = New List(Of Dictionary(Of String, Object))

        Dim dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "230397"},
                                                          {"ParentFirstName", "Age"},
                                                          {"ParentLastName", "Huskesen"},
                                                          {"FirstName", "Andreas"},
                                                          {"LastName", "Helgesen"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "1911"},
                                                          {"ParentFirstName", "Eilen"},
                                                          {"ParentLastName", "Andersen"},
                                                          {"FirstName", "Pia"},
                                                          {"LastName", "Andersen"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "1911"},
                                                          {"ParentFirstName", "Eilen"},
                                                          {"ParentLastName", "Andersen"},
                                                          {"FirstName", "Jesper"},
                                                          {"LastName", "Andersen"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "2011"},
                                                          {"ParentFirstName", "Thorbjørn"},
                                                          {"ParentLastName", "Helgesen"},
                                                          {"FirstName", "Anette"},
                                                          {"LastName", "Helgesen"}
                                                         }
        dc.Data.Add(dic)

        dic = New Dictionary(Of String, Object) From {{"ParentIdentifier", "2011"},
                                                          {"ParentFirstName", "Thorbjørn"},
                                                          {"ParentLastName", "Helgesen"},
                                                          {"FirstName", "Lene"},
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

        Dim groups = dc.Data.GroupBy(Function(d) d("ParentIdentifier"))
        For Each group In groups
            Debug.WriteLine("key = {0}:", group.Key)
            For Each value In group
                Debug.WriteLine("value = {0}:", value)
            Next
        Next
    End Sub

End Class
