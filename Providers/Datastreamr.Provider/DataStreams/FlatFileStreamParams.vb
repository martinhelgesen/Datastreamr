Imports Datastreamr.Framework

Namespace DataStreams
    Public Class FlatFileStreamParams
        Inherits BaseFileStreamParams

        Public Sub New()
            Add("FirstLineIsHeader",
                New ParamInfo _
                   With {.Required = False, .Name = "FirstLineIsHeader", .Type = GetType(Boolean),
                   .Description = "Is first line header?", .DefaultValue = False})
        End Sub

        Public Sub New(ByVal baseStreamParams As BaseStreamParams)
            MyBase.New(baseStreamParams)
        End Sub

        Property FirstLineIsHeader As Boolean
            Get
                If Me("FirstLineIsHeader").Value Is Nothing Then _
                    Return CType(Me("FirstLineIsHeader").DefaultValue, Boolean)
                Return CType(Me("FirstLineIsHeader").Value, Boolean)
            End Get
            Set(value As Boolean)
                Me("FirstLineIsHeader").Value = value
            End Set
        End Property
    End Class
End NameSpace