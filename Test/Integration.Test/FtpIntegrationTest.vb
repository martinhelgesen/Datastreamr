Imports Datastreamr.JobRunner
Imports System.IO
Imports Datastreamr.Framework
Imports DataStreamr.Provider.DataStreams
Imports DataStreamr.Framework.Facade
Imports Infotjenester.Hressurs.Provider.Endpoints
Imports LazyFramework

Public Module FtpIntegrationTest

    Public Sub Main()
        Using New ClassFactory.SessionInstance
            Using New DefaultDataStreamrContext With {.CurrentUser = New User With {.Username = "grehan"}}
                CreateJob()
            End Using
            StartFileWatching()
            CreateFile()
            Console.ReadKey()
        End Using
    End Sub

    Private Sub CreateFile()
        Const s As String = "c:\Temp\ftp\grehan\incoming\ImportToHressurs.tmp"
        File.Delete(s)
        File.Delete(s.Split("."c)(0) + ".job")
        Using fw = File.Create(s)
            Dim sw As New StreamWriter(fw)
            sw.Write("1723;1723;Martin Helgesen;123;1")
            sw.Flush()
        End Using
        Dim fi = New FileInfo(s)
        fi.MoveTo(s.Split("."c)(0) + ".job")
    End Sub

    Private Sub CreateJob()
        If File.Exists("C:\Temp\persist\grehan\JobEntity\ImportToHressurs.json") Then File.Delete("C:\Temp\persist\grehan\JobEntity\ImportToHressurs.json")

        Dim j As New JobEntity
        j.Name = "ImportToHressurs"
        j.DataStreamTypeName = GetType(FtpFileStream).AssemblyQualifiedName
        j.EndpointTypeName = GetType(Infotjenester.Hressurs.Provider.Endpoints.HRPersonEndpoint).AssemblyQualifiedName
        j.DataStreamParams = New FtpFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False}
        j.EndpointParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "DepartmentCode", .Password = "grehan1"}

        Dim ret As New MapConfig
        ret.Add("0", "Identifier", Nothing)
        ret.Add("1", "EmployeeNo", Nothing)
        ret.Add("2", "FirstName", "return originalValue.split(' ')[0];")
        ret.Add("2", "LastName", "return originalValue.split(' ')[1];")
        ret.Add("3", "DepartmentIdentifier", Nothing)
        ret.Add("4", "CompanyIdentifier", Nothing)
        'ret.Add("2", "SocialSec", Nothing)
        'ret.Add("3", "Email", Nothing)
        'ret.Add("4", "Phone", Nothing)
        'ret.Add("5", "PhonePrivate", Nothing)
        'ret.Add("6", "Mobile", Nothing)
        'ret.Add("7", "DepartmentIdentifier", Nothing)
        ''ret.Add("8", "Account", Nothing)
        'ret.Add("9", "EmployeeCategory", Nothing)
        'ret.Add("10", "EmployeePosition", Nothing)
        'ret.Add("11", "EmploymentStartDate", Nothing)
        j.Mapconfig = ret

        JobFacade.SaveJob(j)

        DatastreamrContext.Current = Nothing

    End Sub

    Public Sub StartFileWatching()
        Dim fw As New FileWatcher("c:\Temp\ftp\", "*.job")
    End Sub

End Module
