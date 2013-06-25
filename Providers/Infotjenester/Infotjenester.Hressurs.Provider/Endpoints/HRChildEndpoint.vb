Imports Datastreamr.Framework.Endpoints
Imports Datastreamr.Framework
Imports LazyFramework
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Namespace Endpoints
    Public Class HRChildEndpoint
        Inherits TypeSafeEndPoint(Of HRPersonParams, HRChild)


        Public Overrides Function Deliver(ByVal params As HRPersonParams, ByVal values As DataContainer) As EndPointResult
            'Validate
            ValidateParams(params)


            'Transform values to Person and Child objects
            'values.Data.Sort()
            Dim persons As List(Of Person) = (From dic In values.Data Select InternalTransform(dic, params)).ToList()

            'Deliver            
            Dim request As New ImportPersonRequest With {
                                             .Persons = persons.ToArray,
                                             .PersonIdentifierType = CType([Enum].Parse(GetType(PersonIdentifierType), params.PersonIdentifier, True), PersonIdentifierType?),
                                             .UnitIdentifierType = CType([Enum].Parse(GetType(UnitIdentifierType), params.UnitIdentifier, True), UnitIdentifierType?)}
            'request.

            Dim service = ClassFactory.GetTypeInstance(Of IHRPersonProxy, PersonClientProxy)()
            Dim result = service.Import(request, params.Username, params.Password)

            Return New EndPointResult With {.success = False, .Result = Newtonsoft.Json.JsonConvert.SerializeObject(result)}
        End Function

        Private Function InternalTransformChildren(ByVal dictionary As Dictionary(Of String, Object), requestParams As HRPersonParams) As Person
            Dim child As New Child
            If ContainsAndNotEmpty(dictionary, "FirstName") Then child.FirstName = CType(dictionary("FirstName"), String)
            If ContainsAndNotEmpty(dictionary, "LastName") Then child.FirstName = CType(dictionary("LastName"), String)
        End Function
        Private Function InternalTransform(ByVal dictionary As Dictionary(Of String, Object), requestParams As HRPersonParams) As Person
            Dim person As New Person

            'Property Identifier As String
            If dictionary.ContainsKey("ParentIdentifier") Then person.PersonIdentifier = New PersonIdentifier With {.Value = CType(dictionary("ParentIdentifier"), String), .IdentifierType = CType([Enum].Parse(GetType(PersonIdentifierType), requestParams.PersonIdentifier, True), PersonIdentifierType?)}

            Return person
        End Function

        Private Function ContainsAndNotEmpty(d As Dictionary(Of String, Object), field As String) As Boolean
            Return d.ContainsKey(field) AndAlso Not String.IsNullOrEmpty(CType(d(field), String))
        End Function

        Private Sub ValidateParams(ByVal hrPersonParams As HRPersonParams)
            If String.IsNullOrEmpty(hrPersonParams.PersonIdentifier) Then
                Throw New ArgumentException("PersonIdentifier cannot be null", "PersonIdentifier")
            End If
        End Sub

        Public Overrides ReadOnly Property Name() As String
            Get
                Return "HRessurs Child import"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "HRessurs Child import"
            End Get
        End Property

    End Class

End Namespace