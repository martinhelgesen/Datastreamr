Namespace Facade
    Public Class CustomerStreamFacade
        Public Shared Function GetCustomerStream(id As Integer, streamParams As Dictionary(Of String, String), transformerParams As Dictionary(Of String, String)) As CustomerStreamResultDecorator
            'Dim agg As New Personalarkiv.Datastream.Aggregates.CustomerStream
            'Dim retval = agg.GetInstance(id)
            'Return New CustomerStreamResultDecorator(retval, streamParams, transformerParams)
        End Function
        Public Shared Function GetCustomerStream(id As Integer) As CustomerStreamResultDecorator
            'Return GetCustomerStream(id, Nothing, Nothing)
        End Function
        Public Shared Function UpdateCustomerStream(obj As CustomerStreamResultDecorator) As Boolean
            'Throw New NotImplementedException()
        End Function
    End Class
End Namespace