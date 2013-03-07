Imports System.IO
Imports Datastreamr.Framework
Imports LazyFramework
Imports Datastreamr.Framework.Logging

Public Class FileWatcher
    Private ReadOnly _path As String
    'Private ReadOnly _FileLoggerPath As String = "C:\"
    Private ReadOnly _fileWatcher As FileSystemWatcher
    Private _filter As String
    ReadOnly Property LogFilePrefix As String
        Get
            Return Date.Now.ToString("yyyy-MM-dd-HH-mm-ss")
        End Get
    End Property

    Public Sub New(path As String, filter As String)
        _filter = filter
        _path = path
        _fileWatcher = New FileSystemWatcher With {.Path = _path, .IncludeSubdirectories = True, .Filter = _filter}
        _fileWatcher.EnableRaisingEvents = True
        AddHandler _fileWatcher.Renamed, AddressOf FileHandler
    End Sub

    Private Sub FileHandler(ByVal sender As Object, ByVal e As FileSystemEventArgs)
        If Not e.FullPath.Contains("\incoming\") Then
            Return
        End If

        Dim username = e.FullPath.Replace(_path, "").Split("\"c)(0)

        If e.ChangeType = WatcherChangeTypes.Renamed Then
            Using New ClassFactory.SessionInstance
                Dim logPath As String = _path & "\" & username & "\log"
                Using New DefaultDataStreamrContext With {.CurrentUser = New User With {.Username = username, .FTPRootCatalog = _path + "\" + username},
                                                          .Logger = New FileLogger(logPath & "\")}
                    Dim jobName As String = GetName(e.Name)
                    Dim job = Facade.JobFacade.GetJob(jobName)
                    Dim jobExec As New JobExecutor(job)
                    Dim result = jobExec.Execute()
                    If result.Success = True Then
                        Directory.CreateDirectory(logPath)
                        File.Move(e.FullPath, logPath & "\" & LogFilePrefix & "-" & GetName(e.Name, True))
                    Else
                        Dim errorPath As String = _path & "\" & username & "\error"
                        Directory.CreateDirectory(errorPath)
                        File.Move(e.FullPath, errorPath & "\" & LogFilePrefix & "-" & GetName(e.Name, True))
                    End If
                End Using
            End Using
        End If
    End Sub

    Private Function GetName(ByVal name As String, Optional extension As Boolean = False) As String
        Dim s = name.Split("\"c)
        If extension Then
            Return s(s.Length - 1)
        Else
            Return s(s.Length - 1).Split("."c)(0)
        End If

    End Function
End Class
