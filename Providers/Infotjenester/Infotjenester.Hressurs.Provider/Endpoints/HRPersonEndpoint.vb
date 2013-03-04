Imports Datastreamr.Framework
Imports Datastreamr.Framework.Interfaces
Imports LazyFramework
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Namespace Endpoints

    Public Class HRPersonEndpoint
        Inherits BaseEndpoint(Of HRPersonParams)

        Public Overrides Function Deliver(params As HRPersonParams, mappedValues As DataContainer) As EndPointResult
            'Transform values to HRPerson objects
            Dim persons As List(Of Person) = (From dic In mappedValues.Data Select InternalTransform(dic)).ToList()

            'Validate
            ValidateParams(params)

            'Deliver            
            Dim request As New ImportRequest(New ImportPersonRequest With {
                                             .Persons = persons.ToArray,
                                             .PersonIdentifierType = CType([Enum].Parse(GetType(PersonIdentifierType), params.PersonIdentifier, True), PersonIdentifierType?),
                                             .UnitIdentifierType = CType([Enum].Parse(GetType(UnitIdentifierType), params.UnitIdentifier, True), UnitIdentifierType?)})
            Dim service = ClassFactory.GetTypeInstance(Of IPerson, PersonClientProxy)()
            If TypeOf (service) Is PersonClient Then
                Dim s = CType(service, PersonClient)
                s.ClientCredentials.UserName.UserName = params.Username
                s.ClientCredentials.UserName.Password = params.Password
            End If
            Dim result = service.Import(request)
            Return New EndPointResult With {.success = False}
        End Function

        Protected Friend Function InternalTransform(ByVal dictionary As Dictionary(Of String, Object)) As Person
            'TODO: ikke i noe interface eller baseklasse enda
            Return New Person
        End Function

        Protected Friend Sub ValidateParams(ByVal hrPersonParams As HRPersonParams)
            If String.IsNullOrEmpty(hrPersonParams.PersonIdentifier) Then
                Throw New ArgumentException("PersonIdentifier cannot be null", "PersonIdentifier")
            End If
            If String.IsNullOrEmpty(hrPersonParams.UnitIdentifier) Then
                Throw New ArgumentException("UnitIdentifier cannot be null", "UnitIdentifier")
            End If
        End Sub

        Public Overrides ReadOnly Property Description As String
            Get
                Return "HRessurs Personimport"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "HRessurs Personimport"
            End Get
        End Property

        Public Overrides ReadOnly Property GetMappingInfo As List(Of PropertyDesc)
            Get
                Dim retval As New List(Of PropertyDesc)
                retval.Add(New PropertyDesc With {.Name = "FirstName", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New PropertyDesc With {.Name = "LastName", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                Return retval
            End Get
        End Property
    End Class

    Public Class PersonClientProxy
        Inherits PersonClient

        Public Sub New()
            MyBase.New("BasicHttpBinding_IPerson")
        End Sub
    End Class

    Public MustInherit Class BaseEndpoint(Of TParams As {New, StreamParams})
        Implements IEndpoint(Of TParams)


        Public MustOverride Function Deliver(params As TParams, data As DataContainer) As EndPointResult Implements IEndpoint(Of TParams).Deliver
        Public MustOverride ReadOnly Property Name As String
        Public MustOverride ReadOnly Property Description As String
        Public MustOverride ReadOnly Property GetMappingInfo As List(Of PropertyDesc)

        Public Function GetParams() As TParams Implements IEndpoint(Of TParams).GetParams
            Return New TParams
        End Function
    End Class
End Namespace