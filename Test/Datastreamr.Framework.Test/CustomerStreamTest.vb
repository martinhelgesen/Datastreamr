Imports Datastreamr.Framework.Entities.CustomerStream.DataAccess
Imports Datastreamr.Framework.Entities.CustomerStream
Imports NSubstitute
Imports NUnit.Framework
Imports Infotjenester.Hressurs.Provider.Streams

<TestFixture> Public Class CustomerStreamTest

    <TestFixtureSetUp()> Sub setup()

    End Sub

    <Test()> Sub Test()
        Dim personsinCompany As CustomerStream = StreamStub()
        Dim resultdecorator = New CustomerStreamResultDecorator(personsinCompany, Nothing, Nothing)
        Dim result = resultdecorator.Result()
        Assert.IsNotNull(result)
    End Sub
    <Test()> Sub TestCustomerStreamFetch()

        Dim agg = Substitute.For(Of ICustomerStreamDataAccess)()
        agg.GetEntity(1).Returns(Function(p)
                                     Return StreamStub()
                                 End Function)

        Using session = New LazyFramework.ClassFactory.SessionInstance
            LazyFramework.ClassFactory.SetTypeInstanceForSession(Of ICustomerStreamDataAccess)(agg)
            Dim stream As CustomerStreamResultDecorator = Facade.CustomerStreamFacade.GetCustomerStream(1)
            Dim result = stream.Result()
            Assert.IsNotNull(result)
        End Using
    End Sub

    Private Function StreamStub() As CustomerStream
        Return New CustomerStream With {
            .Id = 1, .Name = "test",
            .Params = New PersonsInCompany.PersonsInCompanyParams With { _
                .IncludeAddresses = False,
                .IncludeChildren = False,
                .IncludeEmailAddresses = False,
                .IncludeEmployment = False,
                .IncludeNextOfKin = False,
                .IncludePhones = False,
                .IncludeSocialSecurityNumber = False,
                .MaxExportCount = 100,
                .Username = "grehan",
                .Password = "grehan1"
                },
            .StreamtypeId = 1}
    End Function

    <TestFixtureTearDown()> Public Sub teardown()

    End Sub
End Class
