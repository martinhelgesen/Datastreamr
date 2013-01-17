Imports Datastreamr.Framework.Entities.CustomerStream
Imports Datastreamr.Framework
Imports NUnit.Framework

<testfixture> Public Class CustomerStreamTest

    <TestFixtureSetUp()> Sub setup()

    End Sub

    <Test()> Sub test()
        Dim personsinCompany As CustomerStream = StreamStub()
        Dim resultdecorator = New CustomerStreamResultDecorator(personsinCompany, Nothing, Nothing)
        Dim result = resultdecorator.Result()
        Assert.IsNotNullOrEmpty(result)
    End Sub

    Private Function StreamStub() As CustomerStream
        Return New CustomerStream With {
            .Id = 1, .Name = "test",
            .Params = New StreamParams(),
            .StreamtypeId = 1}
    End Function

    <TestFixtureTearDown()> Public Sub teardown()

    End Sub
End Class
