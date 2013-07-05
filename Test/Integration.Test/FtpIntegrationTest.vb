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
            'StartFileWatching()
            'CreateFile()
            Console.ReadKey()
        End Using
    End Sub

    Private Sub CreateFile()
        Const s As String = "c:\Temp\ftp\*****\incoming\ImportToHressurs.tmp"
        File.Delete(s)
        File.Delete(s.Split("."c)(0) + ".job")
        Using fw = File.Create(s)
            Dim sw As New StreamWriter(fw)
            'sw.Write("1723;1723;Martin Helgesen;123;1")
            sw.WriteLine("230397;Andreas;Helgesen;1")
            sw.WriteLine("2408;Pia;Andersen;1")
            sw.Flush()
        End Using
        Dim fi = New FileInfo(s)
        fi.MoveTo(s.Split("."c)(0) + ".job")
    End Sub

    Private Sub CreateJob()
        If File.Exists("C:\Temp\persist\grehan\JobEntity\HREmployment.json") Then File.Delete("C:\Temp\persist\grehan\JobEntity\HREmployment.json")

        Dim j As New JobEntity
        j.Name = "HREmployment"
        'j.DataStreamTypeName = GetType(ValueSeparatedFileStream).AssemblyQualifiedName
        j.Endpoint = New HREmploymentEndpoint With {.StreamParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "DepartmentCode", .Password = "tullepassord"}}
        'j.EndpointTypeName = GetType(Infotjenester.Hressurs.Provider.Endpoints.HREmploymentEndpoint).AssemblyQualifiedName
        j.DataStream = New ValueSeparatedFileStream
        j.DataStream.SetParams(New ValueSeparatedFileStreamParams With {.ValueSeparator = ";", .FirstLineIsHeader = False})
        'j.EndpointParams = New HRPersonParams With {.PersonIdentifier = "EmployeeNumber", .UnitIdentifier = "DepartmentCode", .Password = ""}

        Dim ret As New MapConfig
        ret.Add("STATIC", "CompanyIdentifier", "return 'WF';")
        ret.Add("0", "PersonIdentifier", Nothing)
        ret.Add("1", "EmployeeNumber", Nothing)
        ret.Add("2", "EmployeeCategory", Nothing)
        ret.Add("3", "Position", Nothing)
        ret.Add("4", "FromDate", Nothing)
        ret.Add("5", "EndDate", Nothing)
        ret.Add("6", "PositionPercent", Nothing)
        ret.Add("7", "DepartmentIdentifier", Nothing)
        'ret.Add("2", "FirstName", Nothing)
        'ret.Add("3", "MiddleName", Nothing)
        'ret.Add("4", "LastName", Nothing)
        'ret.Add("5", "ShortName", Nothing)
        'ret.Add("6", "Gender", Nothing)
        'ret.Add("7", "BirthDate", Nothing)
        'ret.Add("8", "EmployeeNo", Nothing)
        'ret.Add("9", "PersonalNo", Nothing)
        'ret.Add("10", "Email", Nothing)
        'ret.Add("11", "Street1", Nothing)
        'ret.Add("12", "Street2", Nothing)
        'ret.Add("13", "Street3", Nothing)
        'ret.Add("14", "PostNo", Nothing)
        'ret.Add("15", "Postarea", Nothing)
        'ret.Add("16", "CountryCode", Nothing)
        'ret.Add("17", "Phone", Nothing)
        'ret.Add("18", "PhonePrivate", Nothing)
        'ret.Add("19", "Mobile", Nothing)
        'ret.Add("20", "Fax", Nothing)
        'ret.Add("21", "BankAccount1", Nothing)
        'ret.Add("22", "BankAccount2", Nothing)
        'ret.Add("23", "DepartmentIdentifier", Nothing)
        ''ret.Add("24", "SetAsLeader", Nothing)
        ''ret.Add("25", "EmployeeCategory", Nothing)
        ''ret.Add("26", "EmployeePosition", Nothing)
        'ret.Add("27", "Nationality", Nothing)
        'ret.Add("28", "NextOfKinFirstName", Nothing)
        'ret.Add("29", "NextOfKinLastName", Nothing)
        'ret.Add("30", "NextOfKinPhone", Nothing)
        ''ret.Add("31", "EmploymentStartDate", Nothing)
        ''ret.Add("32", "EmploymentEndDate", Nothing)
        'ret.Add("33", "IsActive", Nothing)
        ''ret.Add("34", "SpecifiedLeaderIdentifier", Nothing)
        ''ret.Add("35", "Username", Nothing)
        j.Mapconfig = ret

        JobFacade.SaveJob(j)

        DatastreamrContext.Current = Nothing

    End Sub

    Public Sub StartFileWatching()
        Dim fw As New FileWatcher("c:\Temp\ftp\", "*.job")
    End Sub

End Module
