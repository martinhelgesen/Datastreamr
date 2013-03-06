Imports System.IO
Imports Datastreamr.Framework
Imports LazyFramework

Public Class FileWatcher
    Private ReadOnly _path As String
    Private ReadOnly _fileWatcher As FileSystemWatcher
    Private _filter As String

    Public Sub New(path As String, filter As String)
        _filter = filter
        _path = path
        _fileWatcher = New FileSystemWatcher With {.Path = _path, .IncludeSubdirectories = True}
        _fileWatcher.EnableRaisingEvents = True
        AddHandler _fileWatcher.Renamed, AddressOf FileHandler
    End Sub

    Private Sub FileHandler(ByVal sender As Object, ByVal e As FileSystemEventArgs)
        'TODO: Denne må vekk
        Dim username = e.FullPath.Split("\"c)(3)

        If e.ChangeType = WatcherChangeTypes.Renamed Then
            Using New ClassFactory.SessionInstance
                Dim context As IDatastreamrContext = New DatastreamrContext With {.CurrentUser = New User With {.Username = username, .FTPRootCatalog = _path + "\" + username}}
                DatastreamrContext.Current = context

                Dim jobName As String = GetName(e.Name)
                Dim job = Facade.JobFacade.GetJob(jobName)
                Dim jobExec As New JobExecutor(job)
                Dim result = jobExec.Execute()
                If result.Success = True Then
                    'TODO: Her må det loggføres og flytte fil
                    Throw New NotImplementedException
                Else
                    'TODO: HEr må det feilhåndteres og flytte fil
                    Throw New NotImplementedException
                End If
            End Using
        End If
    End Sub

    Private Function GetName(ByVal name As String) As String
        Dim s = name.Split("\"c)
        Return s(s.Length - 1).Split("."c)(0)
    End Function
End Class
