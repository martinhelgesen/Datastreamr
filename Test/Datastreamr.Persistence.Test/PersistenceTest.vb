Imports DataStreamr.Persistence.Interfaces
Imports LazyFramework
Imports NUnit.Framework
Imports NSubstitute

<TestFixture> Public Class PersistenceTest
    Private _session As ClassFactory.SessionInstance

    <SetUp> Public Sub SetUp()
        _session = New LazyFramework.ClassFactory.SessionInstance
        Dim filePersisterFilePath As IFilePersisterFilePath = NSubstitute.Substitute.For(Of Interfaces.IFilePersisterFilePath)()
        LazyFramework.ClassFactory.SetTypeInstance(Of DataStreamr.Persistence.Interfaces.IFilePersisterFilePath)(filePersisterFilePath)

        filePersisterFilePath.FilePath.Returns(Function() "c:\temp\persist\")

    End Sub

    <TearDown> Public Sub TearDown()
        _session = Nothing
    End Sub

    <Test> Public Sub ObjectIsPersisted()


        Dim data As New ToPersist
        data.A = "Petter"
        data.B = 123

        Persister.Current.Persist("Petter", "Test", data)
        Dim o = Persister.Current.Load(Of ToPersist)("Petter", "Test")

        Assert.AreEqual("Petter", o.A)
    End Sub

End Class


Public Class ToPersist
    Property A As String
    Property B As Integer
End Class
