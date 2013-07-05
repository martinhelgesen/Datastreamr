Imports Datastreamr.Framework.Entities
Imports Datastreamr.Framework.DataStreams
Imports Datastreamr.Provider.DataStreams
Imports Infotjenester.Hressurs.Provider.Endpoints
Imports LazyFramework
Imports NSubstitute
Imports NUnit.Framework
Imports Datastreamr.Framework.Utils
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

<testfixture> Public Class JobExecutorTest

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

    <Test> Public Sub ExecuteJobConsumeFtpFileDeliverToHResourceProxyNoHeader()
        'Arrange Job
        Dim jobmock = NSubstitute.Substitute.For(Of IJobEntityDataAcces)()
        jobmock.WhenForAnyArgs(Sub(p) p.GetInstance("", "1", Nothing)).Do(Sub(p)
                                                                              Dim j = CType(p(2), JobEntity)

                                                                              j.Endpoint = New HRPersonEndpoint With {.StreamParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}}
                                                                              j.DataStream = New ValueSeparatedFileStream With {.StreamParams = New ValueSeparatedFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False}}
                                                                              'j.DataStream.SetParams(New ValueSeparatedFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False})
                                                                              'j.EndpointParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}

                                                                              Dim ret As New MapConfig
                                                                              ret.Add("0", "Identifier", Nothing)
                                                                              ret.Add("1", "FirstName", "return originalValue.split(' ')[0];")
                                                                              ret.Add("1", "LastName", "return originalValue.split(' ')[1];")
                                                                              j.Mapconfig = ret
                                                                          End Sub)

        ClassFactory.SetTypeInstanceForSession(Of IJobEntityDataAcces)(jobmock)
        Dim hrPersonProxy As IHRPersonProxy = Substitute.For(Of IHRPersonProxy)()
        ClassFactory.SetTypeInstanceForSession(Of IHRPersonProxy)(hrPersonProxy)

        'Arrange Filestream
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"SemicolonNoHeader"})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) StreamHelper.GenerateStreamReaderFromString(My.Resources.Semicolon_NoHeader))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim job = Facade.JobFacade.GetJob("1")
        Dim JobExecutor = New JobExecutor(job)
        Dim result = JobExecutor.Execute()

        'Assert
        hrPersonProxy.Received.Import(
            Arg.Is(Of ImportPersonRequest)(Function(p) ValidateReceivedPersons(p).All(Function(b) b)),
            Arg.Any(Of String),
            Arg.Any(Of String))
    End Sub

    <Test> Public Sub ExecuteJobConsumeFtpFileDeliverToHResourceProxyWithHeader()
        'Arrange Job
        Dim jobmock = NSubstitute.Substitute.For(Of IJobEntityDataAcces)()
        jobmock.WhenForAnyArgs(Sub(p) p.GetInstance("", "1", Nothing)).Do(Sub(p)
                                                                              Dim j = CType(p(2), JobEntity)
                                                                              'j.DataStreamTypeName = GetType(ValueSeparatedFileStream).AssemblyQualifiedName
                                                                              'j.EndpointTypeName = GetType(Infotjenester.Hressurs.Provider.Endpoints.HRPersonEndpoint).AssemblyQualifiedName
                                                                              j.Endpoint = New HRPersonEndpoint With {.StreamParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}}
                                                                              j.DataStream = New ValueSeparatedFileStream
                                                                              j.DataStream.SetParams(New ValueSeparatedFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = True})
                                                                              'j.EndpointParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}

                                                                              Dim ret As New MapConfig
                                                                              ret.Add("@EmployeeNumber", "Identifier", Nothing)
                                                                              ret.Add("@Name", "FirstName", "return originalValue.split(' ')[0];")
                                                                              ret.Add("@Name", "LastName", "return originalValue.split(' ')[1];")
                                                                              j.Mapconfig = ret
                                                                          End Sub)

        ClassFactory.SetTypeInstanceForSession(Of IJobEntityDataAcces)(jobmock)
        Dim hrPersonProxy As IHRPersonProxy = Substitute.For(Of IHRPersonProxy)()
        ClassFactory.SetTypeInstanceForSession(Of IHRPersonProxy)(hrPersonProxy)

        'Arrange Filestream
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"SemicolonNoHeader"})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) StreamHelper.GenerateStreamReaderFromString(My.Resources.Semicolon_Header))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim job = Facade.JobFacade.GetJob("1")
        Dim JobExecutor = New JobExecutor(job)
        Dim result = JobExecutor.Execute()

        'Assert
        hrPersonProxy.Received.Import(
            Arg.Is(Of ImportPersonRequest)(Function(p) ValidateReceivedPersons(p).All(Function(b) b)),
            Arg.Any(Of String),
            Arg.Any(Of String))
    End Sub


    Private Iterator Function ValidateReceivedPersons(ByVal importRequest As ImportPersonRequest) As IEnumerable(Of Boolean)
        Dim persons = importRequest.Persons
        Yield persons.Length = 3
        Yield persons(0).FirstName = "Martin"
        Yield persons(0).LastName = "Helgesen"
        Yield importRequest.PersonIdentifierType.Value = PersonIdentifierType.EmployeeNumber
        Yield importRequest.UnitIdentifierType.Value = UnitIdentifierType.Guid
    End Function

    <Test> Public Sub JsonTest()

        'Dim param As New JobEntity With {.DataStreamParamsSerialized = "{""ValueSeparator"":{""Name"":""ValueSeparator"",""Type"":""System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"",""Value"":"";"",""Description"":""ValueSeparator of file"",""MaxLength"":null,""DefaultValue"":null,""Required"":false}}"}
        'Dim fp As New FtpFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = True, .FilenameMatch = "1"}
        'Dim s = Newtonsoft.Json.JsonConvert.SerializeObject(fp)
        'Assert.AreEqual(1, param.DataStreamParams.Count)

    End Sub

End Class
