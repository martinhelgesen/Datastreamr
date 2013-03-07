Imports System.Configuration
Imports Datastreamr.JobRunner

Public Class FileWatcherService
    Private _fw As FileWatcher

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        _fw = New FileWatcher(ConfigurationManager.AppSettings("FileWatcherRootPath"), "*.job")
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
        _fw = Nothing
    End Sub

End Class
