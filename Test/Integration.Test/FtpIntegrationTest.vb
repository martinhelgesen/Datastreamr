Imports Datastreamr.JobRunner
Imports System.IO
Imports Datastreamr.Framework
Imports DataStreamr.Provider.DataStreams
Imports DataStreamr.Framework.Facade
Imports Infotjenester.Hressurs.Provider.Endpoints
Imports LazyFramework
Imports NSubstitute
Imports Infotjenester.Hressurs.Provider.PersonServiceReference
Imports Infotjenester.Hressurs.Provider

Public Module FtpIntegrationTest

    Public Sub Main()
        Using New ClassFactory.SessionInstance
            DatastreamrContext.Current = New DefaultDataStreamrContext With {.CurrentUser = New User With {.Username = "mhe"}}
            Dim webproxy = Substitute.For(Of IHRPersonProxy)()
            webproxy.WhenForAnyArgs(Sub(p) p.Import(Nothing, "", "")).Do(Sub(p) Debug.Write(Newtonsoft.Json.JsonConvert.SerializeObject(p.Arg(Of ImportPersonRequest), Newtonsoft.Json.Formatting.Indented)))
            ClassFactory.SetTypeInstance(Of IHRPersonProxy)(webproxy)
            CreateJob()
            StartFileWatching()
            CreateFile()
            Console.ReadKey()
        End Using
    End Sub

    Private Sub CreateFile()
        Const s As String = "c:\Temp\ftp\mhe\incoming\TestJob.tmp"
        File.Delete(s)
        File.Delete(s.Split("."c)(0) + ".job")
        Using fw = File.Create(s)
            Dim sw As New StreamWriter(fw)
            sw.Write(Datastreamr.Framework.Test.My.Resources.Semicolon_NoHeader)
            sw.Flush()
        End Using
        Dim fi = New FileInfo(s)
        fi.MoveTo(s.Split("."c)(0) + ".job")
    End Sub

    Private Sub CreateJob()
        File.Delete("C:\Temp\persist\mhe\JobEntity\TestJob.json")
        Dim j As New JobEntity
        j.Name = "TestJob"
        j.DataStreamTypeName = GetType(FtpFileStream).AssemblyQualifiedName
        j.EndpointTypeName = GetType(Infotjenester.Hressurs.Provider.Endpoints.HRPersonEndpoint).AssemblyQualifiedName
        j.DataStreamParams = New FtpFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False}
        j.EndpointParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "guid"}

        Dim ret As New MapConfig
        ret.Add("0", "Identifier", Nothing)
        ret.Add("0", "EmployeeNo", Nothing)
        ret.Add("1", "FirstName", "return originalValue.split(' ')[0];")
        ret.Add("1", "LastName", "return originalValue.split(' ')[1];")
        'ret.Add("2", "SocialSec", Nothing)
        ret.Add("3", "Email", Nothing)
        ret.Add("4", "Phone", Nothing)
        ret.Add("5", "PhonePrivate", Nothing)
        ret.Add("6", "Mobile", Nothing)
        ret.Add("7", "DepartmentIdentifier", Nothing)
        'ret.Add("8", "Account", Nothing)
        ret.Add("9", "EmployeeCategory", Nothing)
        ret.Add("10", "EmployeePosition", Nothing)
        ret.Add("11", "EmploymentStartDate", Nothing)
        j.Mapconfig = ret

        JobFacade.SaveJob(j)

        DatastreamrContext.Current = Nothing

    End Sub

    Public Sub StartFileWatching()
        Dim fw As New FileWatcher("c:\Temp\ftp\", "*.job")
    End Sub

End Module
