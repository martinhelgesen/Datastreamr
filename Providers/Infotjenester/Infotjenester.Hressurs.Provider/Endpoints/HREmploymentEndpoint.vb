Imports Datastreamr.Framework.Endpoints
Imports Datastreamr.Framework
Imports LazyFramework
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Namespace Endpoints
    Public Class HREmploymentEndpoint
        Inherits TypeSafeEndPoint(Of HRPersonParams, HREmployment)


        Public Overrides Function Deliver(ByVal params As HRPersonParams, ByVal values As DataContainer) As EndPointResult
            'Validate
            ValidateParams(params)

            'Transform values to Person and Employment objects
            Dim persons As New List(Of Person)
            Dim persongroups = values.Data.GroupBy(Function(d) d("PersonIdentifier"))
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
            person.PersonIdentifier = New PersonIdentifier With {.IdentifierType = CType([Enum].Parse(GetType(PersonIdentifierType), params.PersonIdentifier, True), PersonIdentifierType?),
                                                                 .Value = CType(group(0)("PersonIdentifier"), String)
                                                                 }
            Dim tmpEmployees = New List(Of Employee)

            Dim employees = group.GroupBy(Function(d) d("CompanyIdentifier"))
            For Each employee In employees
                tmpEmployees.Add(TransformEmployee(employee,params))
            Next
            person.EmploymentInfo = tmpEmployees.ToArray
            Return person
        End Function

        Private Function TransformEmployee(employee As IEnumerable(Of Dictionary(Of String, Object)), ByVal params As HRPersonParams) As Employee
            Dim ret As New Employee
            Dim tmpEmployments As New List(Of Employment)

            ret.EmployedIn = New UnitIdentifier With {.Value = CType(employee(0)("CompanyIdentifier"), String), .Identifiertype = CType([Enum].Parse(GetType(UnitIdentifierType), params.UnitIdentifier, True), UnitIdentifierType?)}
            ret.EmployeeNumber = CType(employee(0)("EmployeeNumber"), String)

            Dim employments = employee.GroupBy(Function(d) d("FromDate"))
            For Each employment In employments
                tmpEmployments.Add(TransformEmployment(employment, params))
            Next
            ret.Employment = tmpEmployments.ToArray

            Return ret
        End Function

        Private Function TransformEmployment(employment As IEnumerable(Of Dictionary(Of String, Object)), ByVal params As HRPersonParams) As Employment
            Dim ret As New Employment
            Dim distlist = New List(Of EmploymentDistribution)

            ret.Position = New Position With {.Name = CType(employment(0)("Position"), String)}
            ret.Category = New Category With {.Name = CType(employment(0)("EmployeeCategory"), String)}
            ret.FromDate = CDate(employment(0)("FromDate"))
            If ContainsAndNotEmpty(employment(0), "EndDate") Then ret.ToDate = CDate(employment(0)("EndDate"))

            For Each distribution In employment
                Dim dist As New EmploymentDistribution
                If ContainsAndNotEmpty(distribution, "DepartmentIdentifier") Then dist.Unit = New UnitIdentifier With {.Value = CStr(distribution("DepartmentIdentifier")), .Identifiertype = CType([Enum].Parse(GetType(UnitIdentifierType), params.UnitIdentifier, True), UnitIdentifierType?)}
                If ContainsAndNotEmpty(distribution, "PositionPercent") Then dist.PositionPercent = CType(distribution("PositionPercent"), Decimal)
                distlist.Add(dist)
            Next

            ret.EmploymentDistributionList = distlist.ToArray

            Return ret
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