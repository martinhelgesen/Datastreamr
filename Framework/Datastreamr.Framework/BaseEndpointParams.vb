Public Class BaseEndpointParams
    Inherits StreamParams

    Public Sub New()
        MyBase.New()
        Add("EndpointAddress",
            New ParamInfo _
               With {.Required = False, .Name = "EndpointAddress", .Type = GetType(String),
               .Description = "The address to the endpoint"})
    End Sub

    Property EndpointAddress As Boolean
        Get
            If Me("EndpointAddress").Value Is Nothing Then Return CType(Me("EndpointAddress").DefaultValue, Boolean)
            Return CType(Me("EndpointAddress").Value, Boolean)
        End Get
        Set(value As Boolean)
            Me("EndpointAddress").Value = value
        End Set
    End Property
End Class
