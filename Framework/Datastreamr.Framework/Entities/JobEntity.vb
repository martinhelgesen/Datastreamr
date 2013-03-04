Imports Datastreamr.Framework.Interfaces
Imports Newtonsoft.Json

Public Class JobEntity
    Property Id As Integer
    Property DataStreamTypeName As String
    Property DataStreamParamsSerialized As String
    Property EndpointTypeName As String
    Property EndpointParamsSerialized As String
    Property MapSerialized As String

    Private _DataStream As IDatastream = Nothing

    ReadOnly Property DataStream As IDatastream
        Get
            If _DataStream Is Nothing Then
                _DataStream = CType(Activator.CreateInstance(Type.GetType(DataStreamTypeName)), IDatastream)
            End If
            Return _DataStream
        End Get
    End Property

    Private _Endpoint As IEndpoint = Nothing

    ReadOnly Property Endpoint As IEndpoint
        Get
            If _Endpoint Is Nothing Then
                _Endpoint = CType(Activator.CreateInstance(Type.GetType(EndpointTypeName)), IEndpoint)
            End If
            Return _Endpoint
        End Get
    End Property

    Private _DataStreamParams As StreamParams = Nothing
    Public ReadOnly Property DataStreamParams As StreamParams
        Get
            If _DataStreamParams Is Nothing Then
                _DataStreamParams = Newtonsoft.Json.JsonConvert.DeserializeObject(Of StreamParams)(DataStreamParamsSerialized)
            End If
            Return _DataStreamParams
        End Get
    End Property

    Private _EndpointParams As StreamParams = Nothing
    Public ReadOnly Property EndpointParams As StreamParams
        Get
            If _EndpointParams Is Nothing Then
                _EndpointParams = Newtonsoft.Json.JsonConvert.DeserializeObject(Of StreamParams)(EndpointParamsSerialized)
            End If
            Return _EndpointParams
        End Get
    End Property

    Private _mapConfig As MapConfig = Nothing
    Public ReadOnly Property Mapconfig As MapConfig
        Get
            If _mapConfig Is Nothing Then
                _mapConfig = JsonConvert.DeserializeObject(Of MapConfig)(MapSerialized)
            End If
            Return _mapConfig
        End Get
    End Property

End Class
