Namespace DataStreams
    Public Class FixedPositionFileStream
        Inherits FlatFileStream(Of FixedPositionFileStreamParams)
        
        Public Overrides ReadOnly Property Description As String
            Get
                Return "Fixed pos des"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Fixed pos Name"
            End Get
        End Property

        Protected Overrides Sub CreateMetaData(firstLine As String)

        End Sub

        Protected Overrides Sub FillDataContainer(line As String)

        End Sub
    End Class
End NameSpace