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

        Private Function InternalTransform(ByVal dictionary As Dictionary(Of String, Object), requestParams As HRPersonParams) As Person
            Dim person As New Person

            'Property Identifier As String
            If dictionary.ContainsKey("Identifier") Then person.PersonIdentifier = New PersonIdentifier With {.Value = CType(dictionary("Identifier"), String), .IdentifierType = CType([Enum].Parse(GetType(PersonIdentifierType), requestParams.PersonIdentifier, True), PersonIdentifierType?)}

            If dictionary.ContainsKey("FirstName") Then person.FirstName = CType(dictionary("FirstName"), String)
            If dictionary.ContainsKey("LastName") Then person.LastName = CType(dictionary("LastName"), String)
            If dictionary.ContainsKey("MiddleName") Then person.MiddleName = CType(dictionary("MiddleName"), String)
            If dictionary.ContainsKey("ShortName") Then person.ShortName = CType(dictionary("ShortName"), String)
            If dictionary.ContainsKey("PersonalNo") Then person.SocialSecurityNumber = CType(dictionary("PersonalNo"), String)
            'Email
            If dictionary.ContainsKey("Email") Then
                person.EMailAddresses = New EMailAddress() {New EMailAddress With {.Address = CType(dictionary("Email"), String), .Type = EMailType.Main}}
            End If

            'ParentUnit
            If dictionary.ContainsKey("DepartmentIdentifier") Then person.ParentUnitIdentifier = New UnitIdentifier With {.Value = CType(dictionary("DepartmentIdentifier"), String), .Identifiertype = CType([Enum].Parse(GetType(UnitIdentifierType), requestParams.UnitIdentifier, True), UnitIdentifierType?)}

            'Address
            If dictionary.Keys.Any(Function(s) {"Street1", "Street2", "PostNo", "Postarea", "CountryCode"}.Contains(s)) Then
                person.Addresses = New Address() {New Address}
                If dictionary.ContainsKey("Street1") Then person.Addresses(0).StreetName1 = CType(dictionary("Street1"), String)
                If dictionary.ContainsKey("Street2") Then person.Addresses(0).StreetName2 = CType(dictionary("Street2"), String)
                If dictionary.ContainsKey("PostNo") Then person.Addresses(0).ZipCode = CType(dictionary("PostNo"), String)
                If dictionary.ContainsKey("Postarea") Then person.Addresses(0).PostalArea = CType(dictionary("Postarea"), String)
                If dictionary.ContainsKey("CountryCode") Then person.Addresses(0).CountryCode = CType(dictionary("CountryCode"), String)
            End If
            'Phones
            Dim phones = New List(Of Phone)
            If dictionary.ContainsKey("Phone") Then phones.Add(New Phone With {.Number = CType(dictionary("Phone"), String), .Type = PhoneType.DirectNumber})
            If dictionary.ContainsKey("PhonePrivate") Then phones.Add(New Phone With {.Number = CType(dictionary("PhonePrivate"), String), .Type = PhoneType.Home})
            If dictionary.ContainsKey("Mobile") Then phones.Add(New Phone With {.Number = CType(dictionary("Mobile"), String), .Type = PhoneType.CellPhone})
            If dictionary.ContainsKey("Fax") Then phones.Add(New Phone With {.Number = CType(dictionary("Fax"), String), .Type = PhoneType.Fax})
            If phones.Count > 0 Then
                person.Phones = phones.ToArray
            End If

            If dictionary.ContainsKey("BankAccount1") Then person.BankAccount1 = CType(dictionary("BankAccount1"), String)
            If dictionary.ContainsKey("BankAccount2") Then person.BankAccount2 = CType(dictionary("BankAccount2"), String)
            If dictionary.ContainsKey("Nationality") Then person.CountryCode = CType(dictionary("Nationality"), String)

            'NextOfKind
            If dictionary.Keys.Any(Function(s) {"NextOfKindFirstName", "NextOfKindLastName", "NextOfKindPhone"}.Contains(s)) Then
                person.NextOfKinInfo = New NextOfKin() {New NextOfKin}
                If dictionary.ContainsKey("NextOfKindFirstName") Then person.NextOfKinInfo(0).FirstName = CType(dictionary("NextOfKindFirstName"), String)
                If dictionary.ContainsKey("NextOfKindLastName") Then person.NextOfKinInfo(0).FirstName = CType(dictionary("NextOfKindLastName"), String)
                If dictionary.ContainsKey("NextOfKindPhone") Then
                    person.NextOfKinInfo(0).Phones = New Phone() {New Phone With {.Number = CType(dictionary("NextOfKindPhone"), String), .Type = PhoneType.CellPhone}}
                End If
            End If

            If dictionary.ContainsKey("IsActive") Then person.IsDeactivated = Not CType(dictionary("IsActive"), Boolean)

            If dictionary.ContainsKey("SpecifiedLeaderIdentifier") Then
                Dim ptype = CType([Enum].Parse(GetType(PersonIdentifierType), requestParams.PersonIdentifier, True), PersonIdentifierType?)
                person.SpecifiedLeader = New PersonIdentifier With {.Value = CType(dictionary("SpecifiedLeaderIdentifier"), String), .IdentifierType = ptype}
            End If

            If dictionary.ContainsKey("EmployeeNo") Then
                person.EmploymentInfo = New Employee() {New Employee}
                Dim employee = person.EmploymentInfo(0)
                employee.EmployeeNumber = CType(dictionary("EmployeeNo"), String)
                If dictionary.ContainsKey("DepartmentIdentifier") Then employee.EmployedIn = New UnitIdentifier With {.Value = CType(dictionary("DepartmentIdentifier"), String), .Identifiertype = CType([Enum].Parse(GetType(UnitIdentifierType), requestParams.UnitIdentifier, True), UnitIdentifierType?)}

                If dictionary.Keys.Any(Function(s) {"EmploymentStartDate", "EmploymentPercent", "EmploymentEndDate", "EmployeeCategory", "EmployeePosition"}.Contains(s)) Then
                    employee.Employment = New Employment() {New Employment}
                    If dictionary.ContainsKey("EmploymentStartDate") Then employee.Employment(0).FromDate = CType(dictionary("EmploymentStartDate"), Date)
                    If dictionary.ContainsKey("EmploymentEndDate") Then employee.Employment(0).ToDate = CType(dictionary("EmploymentEndDate"), Date)
                    If dictionary.ContainsKey("EmployeeCategory") Then employee.Employment(0).Category = New Category With {.Name = CType(dictionary("EmployeeCategory"), String)}
                    If dictionary.ContainsKey("EmployeePosition") Then employee.Employment(0).Position = New Position With {.Name = CType(dictionary("EmployeePosition"), String)}
                End If

            End If

            If dictionary.ContainsKey("Gender") Then person.Gender = CType(dictionary("Gender"), Gender)
            If dictionary.ContainsKey("BirthDate") Then person.BirthDate = CType(dictionary("BirthDate"), Date)

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