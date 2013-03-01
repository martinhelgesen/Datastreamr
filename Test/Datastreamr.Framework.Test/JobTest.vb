Imports Datastreamr.Framework.Interfaces
Imports Datastreamr.Framework.Facade
Imports Datastreamr.Framework.Entities
Imports LazyFramework
Imports NUnit.Framework
Imports NSubstitute
Imports LazyFramework.Utils

<TestFixture> Public Class JobTest

    Private _sessionInstance As ClassFactory.SessionInstance

    <SetUp> Public Sub Setup()
        _sessionInstance = New ClassFactory.SessionInstance
    End Sub
    <TearDown> Public Sub TearDown()
        _sessionInstance = Nothing
    End Sub

    <Test> Public Sub GetJob_CreatesDatastreamAndEndpointObjects()
        'Arrange
        Dim jobmock = NSubstitute.Substitute.For(Of IJobEntityDataAcces)()
        jobmock.WhenForAnyArgs(Sub(p) p.GetInstance(1, Nothing)).Do(Sub(p)
                                                                        Dim j = CType(p(1), JobEntity)
                                                                        j.DataStreamTypeName = GetType(InternalStreams.FtpFileStream).AssemblyQualifiedName
                                                                        j.EndpointTypeName = GetType(Infotjenester.Hressurs.Provider.Endpoints.HRPersonEndpoint).AssemblyQualifiedName
                                                                    End Sub)

        ClassFactory.SetTypeInstanceForSession(Of IJobEntityDataAcces)(jobmock)
        'Act
        Dim job = Facade.JobFacade.GetJob(1)
        Dim datastream = job.DataStream
        Dim endpoint = job.Endpoint

        'Assert
        Assert.IsInstanceOf(Of IDatastream)(datastream)
        Assert.IsInstanceOf(Of IEndpoint)(endpoint)
    End Sub

End Class
