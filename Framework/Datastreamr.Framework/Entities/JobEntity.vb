Imports Datastreamr.Framework.Interfaces
Imports Newtonsoft.Json

Public Class JobEntity
    Property Name As String
    'Property DataStreamTypeName As String
    'Property EndpointTypeName As String
    Property MapSerialized As String

    <JsonProperty(ItemTypeNameHandling:=TypeNameHandling.Auto)> Public Property DataStream As IDatastream

    <JsonProperty(ItemTypeNameHandling:=TypeNameHandling.Auto)> Public Property Endpoint As IEndpoint


    'Public Property DataBaseStreamParams As BaseStreamParams
    'Public Property EndpointParams As BaseStreamParams
    Public Property Mapconfig As MapConfig


End Class
