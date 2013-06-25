Imports System.Configuration
Imports Datastreamr.JobRunner

Public Class CreditCardWatcherService

    Private _fw As FileWatcher
    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.
        ' Add code here to start your service. This method should set things
        _fw = New FileWatcher(ConfigurationManager.AppSettings("CreditCardRootPath"), "*.cct")
    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.
    End Sub

End Class
