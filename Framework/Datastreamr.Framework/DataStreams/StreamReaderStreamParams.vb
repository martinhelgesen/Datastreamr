Imports Datastreamr.Framework

Namespace DataStreams
    Public Class StreamReaderStreamParams
        Inherits StreamParams

        Public Sub New()
            Add("FirstLineIsHeader",
                New ParamInfo _
                   With {.Required = False, .Name = "FirstLineIsHeader", .Type = GetType(Boolean),
                   .Description = "Is first line header?", .DefaultValue = False})
            Add("ValueSeparator",
                New ParamInfo _
                   With {.Required = False, .Name = "ValueSeparator", .Type = GetType(String),
                   .Description = "ValueSeparator of file"})
            Add("FixedPositionDescriptor",
                New ParamInfo _
                   With {.Required = False, .Name = "FixedPositionDescriptor", .Type = GetType(String),
                   .Description = "Enter fixed position lengths here separated by comma"})
        End Sub

        Public Sub New(ByVal streamParams As StreamParams)
            MyBase.New(streamParams)
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

        Property ValueSeparator As String
            Get
                If Me("ValueSeparator").Value Is Nothing Then Return CType(Me("ValueSeparator").DefaultValue, String)
                Return CType(Me("ValueSeparator").Value, String)
            End Get
            Set(value As String)
                Me("ValueSeparator").Value = value
            End Set
        End Property

        Property FixedPositionDescriptor As String
            Get
                If Me("FixedPositionDescriptor").Value Is Nothing Then _
                    Return CType(Me("FixedPositionDescriptor").DefaultValue, String)
                Return CType(Me("FixedPositionDescriptor").Value, String)
            End Get
            Set(value As String)
                Me("FixedPositionDescriptor").Value = value
            End Set
        End Property
    End Class
End NameSpace