Imports System.Configuration
Imports Datastreamr.JobRunner

Module Module1
    Private _fw As FileWatcher

    Sub Main()
        Try
            Console.WriteLine("File Watcher Service Started")
            _fw = New FileWatcher(ConfigurationManager.AppSettings("FileWatcherRootPath"), "*.job")
            Console.ReadKey()
        Catch ex As Exception
            Console.Write(ex.Message)
            Console.ReadKey()
        End Try

    End Sub

End Module
