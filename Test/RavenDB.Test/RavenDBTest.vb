Imports Raven.Abstractions.Logging
Imports Raven.Client
Imports Raven.Client.Listeners
Imports Raven.Client.Embedded
Imports NUnit.Framework

<TestFixture> Public Class RavenDBTest
    Private _documentStore As EmbeddableDocumentStore

    <TestFixtureSetUp()>
    Public Sub Setup()
        LogManager.RegisterTarget(Of DebugTarget)()
    End Sub

    Class DebugTarget
        Inherits Target
        Public Overrides Sub Write(logEvent As LogEventInfo)
            Debug.WriteLine("{0} - {1} - {2}", _
                            logEvent.TimeStamp.ToLocalTime().ToString("hh:mm:ss.fff"), _
                            logEvent.Level, _
                            logEvent.FormattedMessage)
        End Sub
    End Class

    '<Test> Public Sub CreateDB()
    '    Using documentStore As New EmbeddableDocumentStore With {.RunInMemory = True}

    '        documentStore.Initialize()

    '        Using session = documentStore.OpenSession()
    '            session.Store(New CustomerStream With {.Id = 4, .Name = "177mdffarsdfdffds6t2in611"})
    '            session.Store(New CustomerStream With {.Id = 2, .Name = "17fd7martrsdfdffds6t2in611"})
    '            session.Store(New CustomerStream With {.Id = 3, .Name = "re177marsdfdfffdfds6t2in611"})
    '            session.SaveChanges()
    '        End Using

    '        Using session = documentStore.OpenSession()
    '            Dim results As IList(Of CustomerStream)

    '            results = session.Query(Of CustomerStream)() _
    '                             .Customize(Function(x) x.WaitForNonStaleResults()) _
    '                             .ToList()

    '            For Each testclass In results
    '                Debug.WriteLine(testclass.Name)
    '                session.Delete(testclass)
    '            Next
    '            session.SaveChanges()

    '            results = session.Query(Of CustomerStream)() _
    '                             .Customize(Function(x) x.WaitForNonStaleResults()) _
    '                             .ToList()

    '            Assert.AreEqual(0, results.Count)
    '        End Using

    '    End Using
    'End Sub

    <TestFixtureTearDown()>
    Public Sub TearDown()
    End Sub

End Class

Public Class NoStaleQueryListener
    Implements IDocumentQueryListener

    Public Sub BeforeQueryExecuted(ByVal queryCustomization As IDocumentQueryCustomization) Implements IDocumentQueryListener.BeforeQueryExecuted
        queryCustomization.WaitForNonStaleResults()
    End Sub
End Class
