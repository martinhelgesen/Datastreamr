Namespace BaseClasses
    Public Class DatastreamrBaseAggregate(Of TInterface, T As {TInterface, New})
        Inherits LazyFramework.GenericBaseAggregate(Of TInterface, T)
        Public Overrides Property DbName() As String
            Get
                Return ""
            End Get
            Set(value As String)

            End Set
        End Property

    End Class
End NameSpace