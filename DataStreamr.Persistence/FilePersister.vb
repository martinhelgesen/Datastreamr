Imports DataStreamr.Persistence.Config
Imports DataStreamr.Persistence.Interfaces

Friend Class FilePersister
    Implements IPersister

    Public Function Load(Of T)(dbName As String, objectName As String) As T Implements IPersister.Load
        Dim ret As T
        Using sReader As New IO.StreamReader(Path(dbName, GetType(T)) & objectName & ".json")
            ret = Newtonsoft.Json.JsonConvert.DeserializeObject(Of T)(sReader.ReadToEnd)
        End Using
        Return ret
    End Function

    Private Function Path(ByVal dbName As String, type As Type) As String
        Return filePath & dbName & "\" & type.Name & "\"
    End Function

    Protected ReadOnly Property filePath() As String
        Get
            Return LazyFramework.ClassFactory.GetTypeInstance(Of IFilePersisterFilePath, AppConfigFilePersisterFilePath)().FilePath
        End Get
    End Property

    Public Sub Persist(dbName As String, objectname As String, data As Object) Implements IPersister.Persist

        If Not IO.Directory.Exists(Path(dbName, data.GetType)) Then
            IO.Directory.CreateDirectory(Path(dbName, data.GetType))
        End If

        Using sWriter As New IO.StreamWriter(Path(dbName, data.GetType) & objectname & ".json", False)
            sWriter.Write(Newtonsoft.Json.JsonConvert.SerializeObject(data))
        End Using
    End Sub
End Class

