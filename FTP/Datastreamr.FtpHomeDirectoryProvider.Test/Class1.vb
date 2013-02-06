Imports System.IO
Imports NUnit.Framework

<testfixture> Public Class Class1

    <Test> Public Sub TestDirectoryExists()
        Assert.IsTrue(Directory.Exists("C:\Program Files"))
    End Sub
    <Test> Public Sub TestDirectoryCreation()
        Dim dirinfo = Directory.CreateDirectory("C:\temp2")
        Assert.IsNotNull(dirinfo)
    End Sub
End Class
