﻿Imports Datastreamr.Framework
Imports NUnit.Framework

<testfixture> Public Class PersonsInCompanyTest

    <TestFixtureSetUp()> Sub setup()

    End Sub

    <Test()> Sub test()
        Dim personsinCompany As New Infotjenester.Hressurs.Provider.Streams.PersonsInCompany
        'Dim resultdecorator = New CustomerStreamResultDecorator(personsinCompany, Nothing, Nothing)
        Dim params = personsinCompany.GetParams()
        params.Username = "grehan"
        params.Password = "grehan1"
        Dim persons = personsinCompany.GetStream(params).ToList()
        Assert.That(persons.Count(), [Is].GreaterThan(0))
        Assert.AreEqual(119, persons.Count())
    End Sub

    <TestFixtureTearDown()> Public Sub teardown()

    End Sub
End Class
