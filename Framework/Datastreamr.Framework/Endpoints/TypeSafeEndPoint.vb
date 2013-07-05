Namespace Endpoints
    Public MustInherit Class TypeSafeEndPoint(Of TParams As {New, BaseStreamParams}, TMapInfo)
        Inherits BaseEndpoint(Of TParams)
        Public Overrides ReadOnly Property GetObjectMapInfo As List(Of ParamInfo)
            Get
                Dim retval As New List(Of ParamInfo)
                For Each prop In GetType(TMapInfo).GetProperties
                    retval.Add(New ParamInfo With {.Name = prop.Name, .Type = prop.PropertyType})
                Next
                Return retval
            End Get
        End Property

        'Public MustOverride Function InternalTransform(dictionary As Dictionary(Of String, Object)) As TMapInfo
    End Class
End Namespace