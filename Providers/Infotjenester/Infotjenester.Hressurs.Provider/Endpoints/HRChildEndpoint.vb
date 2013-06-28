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
            Dim persons As New List(Of Person)
            Dim persongroups = values.Data.GroupBy(Function(d) d("ParentIdentifier"))
            For Each group In persongroups
                persons.Add(TransformPerson(group, params))
            Next

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

        Private Function TransformPerson(group As IEnumerable(Of Dictionary(Of String, Object)), params As HRPersonParams) As Person
            Dim person As New Person
            person.PersonIdentifier = New PersonIdentifier With {.IdentifierType = CType([Enum].Parse(GetType(PersonIdentifierType), params.PersonIdentifier, True), PersonIdentifierType?)}
            Dim children As New List(Of Child)
            For Each value In group
                If ContainsAndNotEmpty(value, "ParentIdentifier") Then person.PersonIdentifier.Value = CType(value("ParentIdentifier"), String)
                If ContainsAndNotEmpty(value, "ParentFirstName") Then person.FirstName = CType(value("ParentFirstName"), String)
                If ContainsAndNotEmpty(value, "ParentLastName") Then person.LastName = CType(value("ParentLastName"), String)
                children.Add(AddChild(value, params))
            Next
            person.Children = children.ToArray
            Return person
        End Function

        Private Function AddChild(ByVal dictionary As Dictionary(Of String, Object), requestParams As HRPersonParams) As Child
            Dim child As New Child
            If ContainsAndNotEmpty(dictionary, "FirstName") Then child.FirstName = CType(dictionary("FirstName"), String)
            If ContainsAndNotEmpty(dictionary, "LastName") Then child.LastName = CType(dictionary("LastName"), String)

            If ContainsAndNotEmpty(dictionary, "MiddleName") Then child.MiddleName = CType(dictionary("MiddleName"), String)
            If ContainsAndNotEmpty(dictionary, "Gender") Then
                child.Gender = CType(dictionary("Gender"), Gender)
            Else
                child.Gender = Gender.NotSet
            End If
            If ContainsAndNotEmpty(dictionary, "BirthDate") Then child.BirthDate = CDate(dictionary("BirthDate"))
            If ContainsAndNotEmpty(dictionary, "SplitCare") Then child.SplitCare = CType(dictionary("SplitCare"), Boolean)
            If ContainsAndNotEmpty(dictionary, "SplitCarePercent") Then child.SplitCarePercent = CType(dictionary("SplitCarePercent"), Integer)
            If ContainsAndNotEmpty(dictionary, "LivesWithParent") Then child.LivesWithParent = CType(dictionary("LivesWithParent"), Boolean)
            If ContainsAndNotEmpty(dictionary, "DisabledChild") Then child.DisabledChild = CType(dictionary("DisabledChild"), Boolean)
            Return child
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