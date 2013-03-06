Imports LazyFramework

Namespace BaseClasses
    Public Class DatastreamrBaseAggregate(Of TInterface, T As {TInterface, New})
        Inherits GenericBaseAggregate(Of TInterface, T)

        Public Overrides Property DbName() As String
            Get
                Return DatastreamrContext.Current.CurrentUser.Username
            End Get
            Set(value As String)
            End Set
        End Property
    End Class
End NameSpace