Imports Datastreamr.Framework.Interfaces
Imports Datastreamr.Framework.Facade
Imports Datastreamr.Framework.Entities
Imports Datastreamr.Framework.DataStreams
Imports Datastreamr.Provider.DataStreams
Imports Infotjenester.Hressurs.Provider.Endpoints
Imports LazyFramework
Imports NUnit.Framework
Imports NSubstitute
Imports LazyFramework.Utils

<TestFixture> Public Class JobTest

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
    <Test> Public Sub GetJob_CallsRepository_WithSpecificArgs()
        'Arrange
        Dim jobmock = NSubstitute.Substitute.For(Of IJobEntityDataAcces)()
        ClassFactory.SetTypeInstanceForSession(Of IJobEntityDataAcces)(jobmock)
        Dim calledWith As JobEntity = Nothing
        jobmock.WhenForAnyArgs(Sub(f) f.GetInstance("", "1", Nothing)).Do(Sub(s) calledWith = s.Arg(Of JobEntity)())

        'Act
        Dim job = Facade.JobFacade.GetJob("1")

        'Assert
        Assert.IsNotNull(calledWith)
    End Sub

    <Test> Public Sub GetJob_CreatesDatastreamAndEndpointObjects()
        'Arrange
        Dim jobmock = NSubstitute.Substitute.For(Of IJobEntityDataAcces)()
        jobmock.WhenForAnyArgs(Sub(p) p.GetInstance("", "1", Nothing)).Do(Sub(p)
                                                                              Dim j = CType(p(2), JobEntity)
                                                                              'j.DataStreamTypeName = GetType(ValueSeparatedFileStream).AssemblyQualifiedName
                                                                              j.DataStream = New ValueSeparatedFileStream
                                                                              j.DataStream.SetParams(New ValueSeparatedFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False})
                                                                              'j.EndpointParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}
                                                                              j.Endpoint = New HRPersonEndpoint With {.StreamParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}}
                                                                              'j.EndpointTypeName = GetType(Infotjenester.Hressurs.Provider.Endpoints.HRPersonEndpoint).AssemblyQualifiedName
                                                                          End Sub)

        ClassFactory.SetTypeInstanceForSession(Of IJobEntityDataAcces)(jobmock)
        'Act
        Dim job = Facade.JobFacade.GetJob("1")
        Dim datastream = job.DataStream
        Dim endpoint = job.Endpoint

        'Assert
        Assert.IsInstanceOf(Of IDatastream)(datastream)
        Assert.IsInstanceOf(Of IEndpoint)(endpoint)
    End Sub


End Class
