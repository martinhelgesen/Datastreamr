Imports Datastreamr.Framework
Imports Datastreamr.Framework.Interfaces

Namespace Endpoints

    Public Class HRPersonEndpoint
        Inherits BaseEndpoint(Of HRPersonParams)


        Public Overrides Function Deliver(params As HRPersonParams, mappedValues As DataContainer) As EndPointResult
            'Transform values to HRPerson objects
            Dim persons As New List(Of PersonServiceReference.Person)


            'deliver
            Dim service As New PersonServiceReference.PersonClient
            Dim request As New PersonServiceReference.ImportPersonRequest With {.Persons = persons.ToArray}
            Dim result = service.Import(request)
            'Return result
        End Function

        Public Overrides ReadOnly Property Description As String
            Get

            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get

            End Get
        End Property
    End Class

    Public MustInherit Class BaseEndpoint(Of TParams As {New, StreamParams})
        Implements IEndpoint(Of TParams)


        Public MustOverride Function Deliver(params As TParams, data As DataContainer) As EndPointResult Implements IEndpoint(Of TParams).Deliver
        Public MustOverride ReadOnly Property Name As String
        Public MustOverride ReadOnly Property Description As String

        Public Function GetParams() As TParams Implements IEndpoint(Of TParams).GetParams
            Return New TParams
        End Function
    End Class
End Namespace