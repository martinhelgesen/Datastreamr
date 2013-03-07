Imports Datastreamr.Framework.Endpoints
Imports Datastreamr.Framework
Imports LazyFramework
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Namespace Endpoints

    Public Class HRPersonEndpoint
        Inherits TypeSafeEndPoint(Of HRPersonParams, HRPerson)

        Public Overrides Function Deliver(params As HRPersonParams, values As DataContainer) As EndPointResult
            'Validate
            ValidateParams(params)

            'Transform values to HRPerson objects
            Dim persons As List(Of Person) = (From dic In values.Data Select InternalTransform(dic)).ToList()

            'Deliver            
            Dim request As New ImportPersonRequest With {
                                             .Persons = persons.ToArray,
                                             .PersonIdentifierType = CType([Enum].Parse(GetType(PersonIdentifierType), params.PersonIdentifier, True), PersonIdentifierType?),
                                             .UnitIdentifierType = CType([Enum].Parse(GetType(UnitIdentifierType), params.UnitIdentifier, True), UnitIdentifierType?)}

            Dim service = ClassFactory.GetTypeInstance(Of IHRPersonProxy, PersonClientProxy)()
            Dim result = service.Import(request, params.Username, params.Password)

            Return New EndPointResult With {.success = False, .Result = Newtonsoft.Json.JsonConvert.SerializeObject(result)}
        End Function

        Private Function InternalTransform(ByVal dictionary As Dictionary(Of String, Object)) As Person
            'TODO: Should this be a part of interface or base class?
            Dim person As New Person
            If dictionary.ContainsKey("FirstName") Then person.FirstName = CType(dictionary("FirstName"), String)
            If dictionary.ContainsKey("LastName") Then person.LastName = CType(dictionary("LastName"), String)
            Return person
        End Function

        Private Sub ValidateParams(ByVal hrPersonParams As HRPersonParams)
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

    End Class
End Namespace