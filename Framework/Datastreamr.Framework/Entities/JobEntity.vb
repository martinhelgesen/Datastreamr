Imports Datastreamr.Framework.Interfaces
Imports Newtonsoft.Json

Public Class JobEntity
    Property Id As Integer
    Property DataStreamTypeName As String
    Property EndpointTypeName As String
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
    
    Public Property DataStreamParams As StreamParams
    Public Property EndpointParams As StreamParams
    Public Property Mapconfig As MapConfig

End Class
