

Namespace Entities.CustomerStream
    Partial Class CustomerStream

        Private _id As Integer
        Private _name As String
        Private _streamtypeId As Integer
        Private _transformertypeId As Integer?
        Private _params As StreamParams
        Private _transformerParams As StreamParams
        Public Property Id As Integer
            Get
                Return _id
            End Get
            Set(value As Integer)
                If _id <> value Then
                    _id = value
                End If
            End Set
        End Property
        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                If _name <> value Then
                    _name = value
                End If
            End Set
        End Property
        Public Property StreamtypeId As Integer
            Get
                Return _streamtypeId
            End Get
            Set(value As Integer)
                If _streamtypeId <> value Then
                    _streamtypeId = value
                End If
            End Set
        End Property
        Public Property TransformertypeId As Integer?
            Get
                Return _transformertypeId
            End Get
            Set(value As Integer?)
                If _transformertypeId <> value Then
                    _transformertypeId = value.Value
                End If
            End Set
        End Property
        Public Property Params As StreamParams
            Get
                Return _params
            End Get
            Set(value As StreamParams)
                _params = value
            End Set
        End Property
        Public Property TransformerParams As StreamParams
            Get
                Return _transformerParams
            End Get
            Set(value As StreamParams)
                _transformerParams = value
            End Set
        End Property
    End Class
End Namespace
