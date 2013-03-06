Imports Datastreamr.JobRunner
Imports System.IO
Imports Datastreamr.Framework
Imports DataStreamr.Provider.DataStreams
Imports DataStreamr.Framework.Facade
Imports Infotjenester.Hressurs.Provider.Endpoints
Imports LazyFramework
Imports NSubstitute
Imports NUnit.Framework
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Public Module FtpIntegrationTest

    Public Sub Main()
        Using New ClassFactory.SessionInstance
            DatastreamrContext.Current = New DatastreamrContext With {.CurrentUser = New User With {.Username = "mhe"}}
            Dim webproxy = NSubstitute.Substitute.For(Of IHRPersonProxy)()
            webproxy.WhenForAnyArgs(Sub(p) p.Import(Nothing, "", "")).Do(Sub(p) Debug.Write(Newtonsoft.Json.JsonConvert.SerializeObject(p.Arg(Of ImportPersonRequest), Newtonsoft.Json.Formatting.Indented)))
            ClassFactory.SetTypeInstance(Of IHRPersonProxy)(webproxy)
            CreateJob()
            StartFileWatching()
            CreateFile()
            Console.ReadKey()
        End Using
    End Sub

    Private Sub CreateFile()
        Dim s = "c:\Temp\ftp\mhe\incoming\TestJob.tmp"
        Using fw = System.IO.File.Create(s)
            Dim sw As New StreamWriter(fw)
            sw.Write(Datastreamr.Framework.Test.My.Resources.Semicolon_NoHeader)
            sw.Flush()
        End Using
        Dim fi = New FileInfo(s)
        fi.MoveTo(s.Split("."c)(0) + ".job")
    End Sub

    Private Sub CreateJob()

        Dim j As New JobEntity
        j.Name = "TestJob"
        j.DataStreamTypeName = GetType(FtpFileStream).AssemblyQualifiedName
        j.EndpointTypeName = GetType(Infotjenester.Hressurs.Provider.Endpoints.HRPersonEndpoint).AssemblyQualifiedName
        j.DataStreamParams = New FtpFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False}
        j.EndpointParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}

        Dim ret As New MapConfig
        ret.Add("0", "Identifier", Nothing)
        ret.Add("1", "FirstName", "return originalValue.split(' ')[0];")
        ret.Add("1", "LastName", "return originalValue.split(' ')[1];")
        j.Mapconfig = ret

        JobFacade.SaveJob(j)

        DatastreamrContext.Current = Nothing

    End Sub

    Public Sub StartFileWatching()
        Dim fw As New FileWatcher("c:\Temp\ftp\", "*.job")
    End Sub

End Module
