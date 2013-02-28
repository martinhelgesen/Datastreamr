Imports Datastreamr.Framework

Namespace Endpoints

    Public Class HRPersonEndpoint
        Implements IEndpoint(Of HRPerson, HRPersonParams)

        Public Function Deliver(params As HRPersonParams, source As DataContainer) As EndPointResult Implements IEndpoint(Of HRPerson, HRPersonParams).Deliver

        End Function

        Public Function GetParams() As HRPersonParams Implements IEndpoint(Of HRPerson, HRPersonParams).GetParams

        End Function
    End Class


    Public Interface IEndpoint(Of TProcessed, TParams As {New, StreamParams})
        'Function Map(source As DataContainer) As TProcessed
        Function Deliver(params As TParams, source As DataContainer) As EndPointResult
        Function GetParams() As TParams
    End Interface

    Public Class EndPointResult
    End Class

    Public Class HRPerson
        Property Identifier As String
        Property FirstName As String
        Property MiddleName As String
        Property LastName As String
        Property ShortName As String
        Property Gender As Integer
        Property BirthDate As String
        Property EmployeeNo As String
        Property PersonalNo As String
        Property Email As String
        Property Street1 As String
        Property Street2 As String
        Property PostNo As String
        Property Postarea As String
        Property Phone As String
        Property PhonePrivate As String
        Property Mobile As String
        Property Fax As String
        Property BankAccount1 As String
        Property BankAccount2 As String
        Property DepartmentName As String
        Property DepartmentNumber As String
        Property SetAsLeader As String
        Property EmployeeCategory As String
        Property EmployeePosition As String
        Property Nationality As String
        Property NextOfKind As String
        Property NextOfKindPhone As String
        Property EmploymentStartDate As String
        Property EmploymentPercent As String
        Property EmploymentEndDate As String
        Property IsActive As String
        Property NearestLeaderIdentifier As String
    End Class

    Public Class BaseEndpointParams
        Inherits StreamParams

        Public Sub New()
            MyBase.New()
            Add("EndpointAddress", New ParamInfo With {.Required = False, .Name = "EndpointAddress", .Type = GetType(String), .Description = "The address to the endpoint"})
        End Sub

        Property EndpointAddress As Boolean
            Get
                If Me("EndpointAddress").Value Is Nothing Then Return CType(Me("EndpointAddress").DefaultValue, Boolean)
                Return CType(Me("EndpointAddress").Value, Boolean)
            End Get
            Set(value As Boolean)
                Me("EndpointAddress").Value = value
            End Set
        End Property
    End Class

    Public Class HRPersonParams
        Inherits BaseEndpointParams

        Public Sub New()
            Add("Username", New ParamInfo With {.Required = False, .Name = "Username", .Type = GetType(String), .Description = "The username to connect to the endpoint"})
            Add("Password", New ParamInfo With {.Required = False, .Name = "Password", .Type = GetType(Boolean), .Description = "The password of the account", .DefaultValue = False})
            Add("PersonIdentifier", New ParamInfo With {.Required = False, .Name = "PersonIdentifier", .Type = GetType(String), .Description = "EmployeeNumber,SocialSecurityNumber,BirthDate"})
            Add("UnitIdentifier", New ParamInfo With {.Required = False, .Name = "UnitIdentifier", .Type = GetType(String), .Description = "DepartmentCode,InternalId"})
        End Sub

        Public Property Username() As String
            Get
                If Me("Username").Value Is Nothing Then Return CType(Me("Username").DefaultValue, String)
                Return CType(Me("Username").Value, String)
            End Get
            Set(value As String)
                Me("Username").Value = value
            End Set
        End Property
        Property Password As Boolean
            Get
                If Me("Password").Value Is Nothing Then Return CType(Me("Password").DefaultValue, Boolean)
                Return CType(Me("Password").Value, Boolean)
            End Get
            Set(value As Boolean)
                Me("Password").Value = value
            End Set
        End Property
        Property PersonIdentifier As Boolean
            Get
                If Me("PersonIdentifier").Value Is Nothing Then Return CType(Me("PersonIdentifier").DefaultValue, Boolean)
                Return CType(Me("PersonIdentifier").Value, Boolean)
            End Get
            Set(value As Boolean)
                Me("PersonIdentifier").Value = value
            End Set
        End Property
        Property UnitIdentifier As Boolean
            Get
                If Me("UnitIdentifier").Value Is Nothing Then Return CType(Me("UnitIdentifier").DefaultValue, Boolean)
                Return CType(Me("UnitIdentifier").Value, Boolean)
            End Get
            Set(value As Boolean)
                Me("UnitIdentifier").Value = value
            End Set
        End Property
    End Class
End Namespace