Imports Datastreamr.Framework.InternalStreams
Imports Datastreamr.Framework.Interfaces
Imports System.IO
Imports LazyFramework
Imports NSubstitute
Imports NUnit.Framework
Imports Datastreamr.Framework.Utils

<TestFixture> Public Class FileStreamTest
    Private _sessionInstance As ClassFactory.SessionInstance
    Private Const _datastreamrcontextSlotName As String = "DatastreamrContext"

    <SetUp> Public Sub Setup()
        _sessionInstance = New LazyFramework.ClassFactory.SessionInstance
        'LazyFramework.ClassFactory.SetTypeInstanceForSession(Of IDatastrea)()
        LazyFramework.Utils.ResponseThread.SetThreadValue(_datastreamrcontextSlotName, New DatastreamrContext With {.CurrentUser = New User With {.Username = "testuser", .Password = "testpwd", .FTPRootCatalog = "C:\FTP"}})
    End Sub
    <TearDown> Public Sub TearDown()
        _sessionInstance = Nothing
        'LazyFramework.Utils.ResponseThread.SetThreadValue(_datastreamrcontextSlotName, Nothing)
        'LazyFramework.Utils.ResponseThread.SetThreadValue(_filehelpercontextSlotName, Nothing)
    End Sub

    <Test> Public Sub InitialFileStreamParamsValues()
        'Act
        Dim fs As New InternalStreams.FtpFileStream
        Dim params = fs.GetParams
        Assert.IsTrue(FileStreamParamsHasDefaultValues(params))
    End Sub

    Private Function FileStreamParamsHasDefaultValues(ByVal params As FtpFileStreamParams) As Boolean
        If params.FilenameMatch IsNot Nothing Then Return False
        If params.ValueSeparator IsNot Nothing Then Return False
        If params.FixedPositionDescriptor IsNot Nothing Then Return False
        If params.FirstLineIsHeader Then Return False
        Return True
    End Function

    <Test> Public Sub GetStream_NullParams_ShouldUseDefaultParams()
        'Arrange
        Dim fsmock = Substitute.For(Of FtpFileStream)()

        'Act
        fsmock.GetStream(Nothing)

        'Assert
        fsmock.Received.GetStreamInternal(Arg.Is(Of FtpFileStreamParams)(Function(fsp) FileStreamParamsHasDefaultValues(fsp)))
    End Sub

    <Test> Public Sub GetStream_NofileFound_Returns_Nothing()
        'Arrange
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) Nothing)
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim fs As New FtpFileStream
        Dim datacontainer = fs.GetStream(New FtpFileStreamParams)

        'Assert
        Assert.IsNull(datacontainer)
    End Sub

    <Test> Public Sub GetStream_MultipleFileMatch_Returns_FirstFile()
        'Arrange
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"First", "Second"})
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim fs As New FtpFileStream
        fs.GetStream(New FtpFileStreamParams)

        'Assert
        filehelper.Received.OpenFile(Arg.Is(Of String)(Function(path) path = "First"))
    End Sub

    <Test> Public Sub GetStream_FileWithHeaderWhenNoHeaderExpected_WhatToDo()
        'Should we handle this?
        'Assert.AreEqual(1, 2)
    End Sub

    <Test> Public Sub GetStream_SemiColonFileWithNoHeader_CorrectValueSeparator()
        'Arrange
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"SemicolonNoHeader"})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) StreamHelper.GenerateStreamReaderFromString(My.Resources.Semicolon_NoHeader))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim fs As New FtpFileStream
        Dim data = fs.GetStream(New FtpFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False})

        'Assert
        Assert.That(data.MetaData.Count, [Is].EqualTo(14))
        Assert.IsTrue(data.MetaData(0).Name = "0")
        Assert.AreEqual(3, data.Data.Count)
        Assert.That(data.Data(0).ContainsKey("0"))
        Assert.That(data.Data(0)("0").ToString = "1962")
    End Sub

    <Test> Public Sub GetStream_SemiColonFileWithHeader_CorrectValueSeparator()
        'Arrange
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"SemicolonHeader"})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) StreamHelper.GenerateStreamReaderFromString(My.Resources.Semicolon_Header))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim fs As New FtpFileStream
        Dim data = fs.GetStream(New FtpFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = True})

        'Assert
        Assert.That(data.MetaData.Count, [Is].EqualTo(14))
        Assert.That(data.MetaData(0).Name = "@EmployeeNumber")
        Assert.AreEqual(3, data.Data.Count)
        Assert.That(data.Data(0).ContainsKey("@EmployeeNumber"))
        Assert.That(data.Data(0)("@EmployeeNumber").ToString = "1962")
    End Sub


    <Test> Public Sub GetStream_MultipleFileMatch_Regex_ReducesMatches_ThenTakesFirstMatchIfMultiple()
        'Arrange
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"First", "SecondThird", "Second"})
        LazyFramework.ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim fs As New InternalStreams.FtpFileStream
        fs.GetStream(New FtpFileStreamParams With {.FilenameMatch = "Second"})

        'Assert
        filehelper.Received.OpenFile(Arg.Is(Of String)(Function(path) path = "SecondThird"))
    End Sub

    <Test> Public Sub GetStream_FixedPositionFileNoHeader()
        Assert.That(False)
    End Sub

    <Test> Public Sub FileStrea_Authentication()
        Assert.That(False)
    End Sub

End Class
