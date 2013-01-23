

Namespace Entities.Job
    Partial Class Job

        Private _id As Integer
        Private _name As String
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
    End Class
End Namespace
