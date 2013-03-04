Imports Datastreamr.Framework.Entities
Imports Datastreamr.Framework.InternalStreams
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

    <Test> Public Sub ExecuteJob_ConsumeFtpFile_DeliverToHResourceProxy()
        'Arrange Job
        Dim jobmock = NSubstitute.Substitute.For(Of IJobEntityDataAcces)()
        jobmock.WhenForAnyArgs(Sub(p) p.GetInstance(1, Nothing)).Do(Sub(p)
                                                                        Dim j = CType(p(1), JobEntity)
                                                                        j.DataStreamTypeName = GetType(InternalStreams.FtpFileStream).AssemblyQualifiedName                                                                        
                                                                        j.EndpointTypeName = GetType(Infotjenester.Hressurs.Provider.Endpoints.HRPersonEndpoint).AssemblyQualifiedName
                                                                    End Sub)

        ClassFactory.SetTypeInstanceForSession(Of IJobEntityDataAcces)(jobmock)

        'Arrange Filestream
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"SemicolonNoHeader"})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) StreamHelper.GenerateStreamReaderFromString(My.Resources.Semicolon_NoHeader))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        'Dim fs As New FtpFileStream
        'Dim data = fs.GetStream(New FtpFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False})

        'Act
        Dim job = Facade.JobFacade.GetJob(1)
        Dim JobExecutor = New JobExecutor(job)
        Dim result = jobExecutor.Start()

        'Assert
        Assert.That(False)
    End Sub

End Class
