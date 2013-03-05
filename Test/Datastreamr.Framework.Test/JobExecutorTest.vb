Imports Datastreamr.Framework.Entities
Imports Datastreamr.Framework.InternalStreams
Imports Infotjenester.Hressurs.Provider.Endpoints
Imports LazyFramework
Imports NSubstitute
Imports NUnit.Framework
Imports Datastreamr.Framework.Utils

<testfixture> Public Class JobExecutorTest

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

    <Test> Public Sub ExecuteJobConsumeFtpFileDeliverToHResourceProxy()
        'Arrange Job
        Dim jobmock = NSubstitute.Substitute.For(Of IJobEntityDataAcces)()
        jobmock.WhenForAnyArgs(Sub(p) p.GetInstance(1, Nothing)).Do(Sub(p)
                                                                        Dim j = CType(p(1), JobEntity)
                                                                        j.DataStreamTypeName = GetType(InternalStreams.FtpFileStream).AssemblyQualifiedName
                                                                        j.EndpointTypeName = GetType(Infotjenester.Hressurs.Provider.Endpoints.HRPersonEndpoint).AssemblyQualifiedName
                                                                        j.DataStreamParams = New FtpFileStreamParams
                                                                        j.EndpointParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}
                                                                        j.Mapconfig = New MapConfig
                                                                    End Sub)

        ClassFactory.SetTypeInstanceForSession(Of IJobEntityDataAcces)(jobmock)
        ClassFactory.SetTypeInstanceForSession(Of IHRPersonProxy)(Substitute.For(Of IHRPersonProxy))

        'Arrange Filestream
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"SemicolonNoHeader"})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) StreamHelper.GenerateStreamReaderFromString(My.Resources.Semicolon_NoHeader))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim job = Facade.JobFacade.GetJob(1)
        Dim JobExecutor = New JobExecutor(job)
        Dim result = JobExecutor.Execute()

        'Assert
        Assert.That(False)
    End Sub

    <Test> Public Sub JsonTest()

        'Dim param As New JobEntity With {.DataStreamParamsSerialized = "{""ValueSeparator"":{""Name"":""ValueSeparator"",""Type"":""System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"",""Value"":"";"",""Description"":""ValueSeparator of file"",""MaxLength"":null,""DefaultValue"":null,""Required"":false}}"}
        'Dim fp As New FtpFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = True, .FilenameMatch = "1"}
        'Dim s = Newtonsoft.Json.JsonConvert.SerializeObject(fp)
        'Assert.AreEqual(1, param.DataStreamParams.Count)

    End Sub

End Class
