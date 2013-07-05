Imports Datastreamr.Provider.DataStreams
Imports System.IO
Imports LazyFramework
Imports NSubstitute
Imports NUnit.Framework
Imports Datastreamr.Framework.Utils

<TestFixture> Public Class ValueSeparatedFileStreamTest
    Private _sessionInstance As ClassFactory.SessionInstance

    <SetUp> Public Sub Setup()
        _sessionInstance = New LazyFramework.ClassFactory.SessionInstance

        Dim contextMock = Substitute.For(Of IDatastreamrContext)()
        contextMock.CurrentUser.ReturnsForAnyArgs(Function(p) New User With {.Username = "testuser", .Password = "testpwd", .RootPath = "C:\FTP"})
        ClassFactory.SetTypeInstanceForSession(Of IDatastreamrContext)(contextMock)
    End Sub
    <TearDown> Public Sub TearDown()
        _sessionInstance = Nothing
    End Sub

    <Test> Public Sub GetStream_NullParams_ShouldUseDefaultParams()
        'Arrange
        Dim fsmock = Substitute.For(Of ValueSeparatedFileStream)()

        'Act
        fsmock.GetStream()

        'Assert
        Assert.AreEqual(True, ValidateDefaultParams(fsmock).All(Function(p) p))
        fsmock.Received.GetStreamInternal()
    End Sub

    Private Iterator Function ValidateDefaultParams(ByVal s As ValueSeparatedFileStream) As IEnumerable(Of Boolean)
        Yield s.StreamParams.FilenameMatch = ""
        Yield s.StreamParams.FirstLineIsHeader = False
        Yield s.StreamParams.ValueSeparator = ";"
    End Function

    <Test> Public Sub GetStream_MultipleFileMatch_Returns_FirstFile()
        'Arrange
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"First", "Second"})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) New StreamReader(New MemoryStream(New Byte())))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim fs As New ValueSeparatedFileStream
        fs.SetParams(New ValueSeparatedFileStreamParams With {.FilenameMatch = "First"})
        fs.GetStream()

        'Assert
        filehelper.Received.OpenFile(Arg.Is(Of String)(Function(path) path = "First"))
    End Sub

    <Test> Public Sub GetStream_NoFileMatch_Returns_FileNotFoundException()
        'Arrange
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"First", "Second"})
        'filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) New StreamReader(New MemoryStream(New Byte())))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim fs As New ValueSeparatedFileStream
        fs.SetParams(New ValueSeparatedFileStreamParams With {.FilenameMatch = "sdfsdf"})
        Assert.Throws(Of CouldNotOpenFileException)(Sub() fs.GetStream())
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
        Dim fs As New ValueSeparatedFileStream
        fs.SetParams(New ValueSeparatedFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False})
        Dim data = fs.GetStream

        'Assert
        Assert.That(data.MetaData.Count = 14)
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
        Dim fs As New ValueSeparatedFileStream
        fs.SetParams(New ValueSeparatedFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = True})
        Dim data = fs.GetStream()

        'Assert
        Assert.That(data.MetaData.Count, [Is].EqualTo(14))
        Assert.That(data.MetaData(0).Name = "@EmployeeNumber")
        Assert.AreEqual(3, data.Data.Count)
        Assert.That(data.Data(0).ContainsKey("@EmployeeNumber"))
        Assert.That(data.Data(0)("@EmployeeNumber").ToString = "1962")
    End Sub

    <Test> Public Sub GetStream_WithEmptyFieldValues_ShouldBeEmptyString()
        'Arrange
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"Middleware"})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) StreamHelper.GenerateStreamReaderFromString(My.Resources.Middleware_FullPersonFile_WithHeader))
        ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim fs As New ValueSeparatedFileStream
        fs.SetParams(New ValueSeparatedFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = True})

        Dim data = fs.GetStream()

        'Assert
        Assert.AreEqual(data.MetaData(0).Name, "Identifier")
        Assert.AreEqual(1, data.Data.Count)
        Assert.AreEqual("", data.Data(0)("Username"))
    End Sub


    <Test> Public Sub GetStream_MultipleFileMatch_Regex_ReducesMatches_ThenTakesFirstMatchIfMultiple()
        'Arrange
        Dim filehelper = Substitute.For(Of IFileHelper)()
        filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"First", "SecondThird", "Second"})
        filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) New StreamReader(New MemoryStream(New Byte())))
        LazyFramework.ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Act
        Dim fs As New ValueSeparatedFileStream
        fs.SetParams(New ValueSeparatedFileStreamParams With {.FilenameMatch = "Second"})
        fs.GetStream()

        'Assert
        filehelper.Received.OpenFile(Arg.Is(Of String)(Function(path) path = "SecondThird"))
    End Sub

    <Test> Public Sub GetStream_FixedPositionFileNoHeader()
        ''Arrange
        'Dim filehelper = Substitute.For(Of IFileHelper)()
        'filehelper.GetFiles("").ReturnsForAnyArgs(Function(p) {"SemicolonNoHeader"})
        'filehelper.OpenFile("").ReturnsForAnyArgs(Function(p) StreamHelper.GenerateStreamReaderFromString(My.Resources.FixedPosition_NoHeader))
        'ClassFactory.SetTypeInstanceForSession(Of IFileHelper)(filehelper)

        'Dim fs As New ValueSeparatedFileStream
        'Dim data = fs.GetStream(New FtpFileStreamParams With {.FirstLineIsHeader = False, .FixedPositionDescriptor = "2,1,4,16,3,1,1,1,23,8,4,12,3,12,3,25,13,3,4,5,3,1,1,1,2,1,1,6,1,1,1,1,2,4,1,2,1,1,12,2,6,20,6,3,7,7,7,7,8,25,1,6,6,6,6,4,5,5,5,5,3,8"})

        'Assert.AreEqual(3, data.Data.Count)
        'Assert.AreEqual(62, data.Data(0).Keys.Count)
        'Dim dic = data.Data(0)
        'Assert.AreEqual(dic("0"), "05")
        Assert.IsTrue(False)
    End Sub

    <Test> Public Sub FileStream_Authentication()
        Assert.That(False)
    End Sub

End Class
