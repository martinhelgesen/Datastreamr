Imports System.Configuration
Imports DataStreamr.Persistence.Interfaces

Namespace Config
    Public Class AppConfigFilePersisterFilePath
        Implements IFilePersisterFilePath

        Public ReadOnly Property FilePath As String Implements IFilePersisterFilePath.FilePath
            Get
                If String.IsNullOrEmpty(ConfigurationManager.AppSettings("FilePersisterPath")) Then
                    Throw New Exception("Missing key 'FilePersisterPath' in app.config")
                End If
                Return ConfigurationManager.AppSettings("FilePersisterPath")
            End Get
        End Property
    End Class
End Namespace