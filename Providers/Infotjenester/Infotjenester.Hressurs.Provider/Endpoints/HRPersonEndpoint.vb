Imports Datastreamr.Framework
Imports Datastreamr.Framework.Interfaces
Imports LazyFramework
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Namespace Endpoints

    Public Class HRPersonEndpoint
        Inherits BaseEndpoint(Of HRPersonParams)

        Public Overrides Function Deliver(params As HRPersonParams, values As DataContainer) As EndPointResult
            'Transform values to HRPerson objects
            Dim persons As List(Of Person) = (From dic In values.Data Select InternalTransform(dic)).ToList()

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

        Public Overrides ReadOnly Property GetObjectMapInfo As List(Of ParamInfo)
            Get
                'TODO: Is this the optimal way of exposing values needed for mapping?
                Dim retval As New List(Of ParamInfo)
                retval.Add(New ParamInfo With {.Name = "Identifier", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "FirstName", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "MiddleName", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "LastName", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "ShortName", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "Gender", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "BirthDate", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "EmployeeNo", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "PersonalNo", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "Email", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "Street1", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "Street2", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "PostNo", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "Postarea", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "Phone", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "PhonePrivate", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "Mobile", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "Fax", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "BankAccount1", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "BankAccount2", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "DepartmentName", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "DepartmentNumber", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "SetAsLeader", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "EmployeeCategory", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "EmployeePosition", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "Nationality", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "NextOfKind", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "NextOfKindPhone", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "EmploymentStartDate", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "EmploymentPercent", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "EmploymentEndDate", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "IsActive", .Type = GetType(String), .Description = "", .MaxLength = "100"})
                retval.Add(New ParamInfo With {.Name = "NearestLeaderIdentifier", .Type = GetType(String), .Description = "", .MaxLength = "100"})
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
        Public MustOverride ReadOnly Property GetObjectMapInfo As List(Of ParamInfo)

        Public Function GetParams() As TParams Implements IEndpoint(Of TParams).GetParams
            Return New TParams
        End Function
    End Class
End Namespace