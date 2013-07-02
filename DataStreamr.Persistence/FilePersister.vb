Imports DataStreamr.Persistence.Config
Imports DataStreamr.Persistence.Interfaces
Imports System.Text

Friend Class FilePersister
    Implements IPersister

    Private _getEncoding As Encoding = Text.Encoding.GetEncoding("ISO-8859-1")

    Public Function Load(Of T)(dbName As String, objectName As String) As T Implements IPersister.Load
        Dim ret As T
        Using sReader As New IO.StreamReader(Path(dbName, GetType(T)) & objectName & ".json", _getEncoding)
            ret = Newtonsoft.Json.JsonConvert.DeserializeObject(Of T)(sReader.ReadToEnd)
        End Using
        Return ret
    End Function

    Public Function LoadAll(Of T)(dbName As String) As IList(Of T) Implements IPersister.LoadAll
        Dim ret As New List(Of T)

        For Each f In New System.IO.DirectoryInfo(Path(dbName, GetType(T))).GetFiles("*.json")
            Using sReader As New IO.StreamReader(f.OpenRead, _getEncoding)
                ret.Add(Newtonsoft.Json.JsonConvert.DeserializeObject(Of T)(sReader.ReadToEnd))
            End Using
        Next

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
        If Not IO.Directory.Exists(filePath & dbName & "\") Then
            IO.Directory.CreateDirectory(filePath & dbName & "\")
        End If

        If Not IO.Directory.Exists(Path(dbName, data.GetType)) Then
            IO.Directory.CreateDirectory(Path(dbName, data.GetType))
        End If

        Using sWriter As New IO.StreamWriter(Path(dbName, data.GetType) & objectname & ".json", False)
            sWriter.Write(Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented))
        End Using

    End Sub
End Class
