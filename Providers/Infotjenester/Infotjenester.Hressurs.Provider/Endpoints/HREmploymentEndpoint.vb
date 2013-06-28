Imports Datastreamr.Framework.Endpoints
Imports Datastreamr.Framework
Imports LazyFramework
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Namespace Endpoints
    Public Class HREmploymentEndpoint
        Inherits TypeSafeEndPoint(Of HRPersonParams, HREmployment)


        Public Overrides Function Deliver(ByVal params As HRPersonParams, ByVal values As DataContainer) As EndPointResult
            ''Validate
            'ValidateParams(params)

            ''Transform values to Person and Child objects
            'Dim persons As New List(Of Person)
            'Dim persongroups = values.Data.GroupBy(Function(d) d("ParentIdentifier"))
            'For Each group In persongroups
            '    persons.Add(TransformPerson(group, params))
            'Next

            ''Deliver            
            'Dim request As New ImportPersonRequest With {
            '                                 .Persons = persons.ToArray,
            '                                 .PersonIdentifierType = CType([Enum].Parse(GetType(PersonIdentifierType), params.PersonIdentifier, True), PersonIdentifierType?),
            '                                 .UnitIdentifierType = CType([Enum].Parse(GetType(UnitIdentifierType), params.UnitIdentifier, True), UnitIdentifierType?)}
            ''request.

            'Dim service = ClassFactory.GetTypeInstance(Of IHRPersonProxy, PersonClientProxy)()
            'Dim result = service.Import(request, params.Username, params.Password)

            'Return New EndPointResult With {.success = False, .Result = Newtonsoft.Json.JsonConvert.SerializeObject(result)}
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
                Return "HRessurs Employment import"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "HRessurs Employment import"
            End Get
        End Property

    End Class

End Namespace