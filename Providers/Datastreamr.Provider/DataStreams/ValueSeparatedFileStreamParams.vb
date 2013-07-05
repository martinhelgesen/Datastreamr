Imports Datastreamr.Framework

Namespace DataStreams
    Public Class ValueSeparatedFileStreamParams
        Inherits FlatFileStreamParams

        Public Sub New(ByVal baseStreamParams As BaseStreamParams)
            MyBase.New(baseStreamParams)
        End Sub

        Public Sub New()
            MyBase.New()

            Add("ValueSeparator",
                New ParamInfo _
                   With {.Required = False, .Name = "ValueSeparator", .Type = GetType(String),
                   .Description = "ValueSeparator of file", .DefaultValue = ";"})
        End Sub

        Property ValueSeparator As String
            Get
                If Me("ValueSeparator").Value Is Nothing Then Return CType(Me("ValueSeparator").DefaultValue, String)
                Return CType(Me("ValueSeparator").Value, String)
            End Get
            Set(value As String)
                Me("ValueSeparator").Value = value
            End Set
        End Property
    End Class
End NameSpace