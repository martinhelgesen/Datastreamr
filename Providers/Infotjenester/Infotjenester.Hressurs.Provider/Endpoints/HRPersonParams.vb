Imports Datastreamr.Framework

Namespace Endpoints
    Public Class HRPersonParams
        Inherits BaseEndpointParams

        
        Public Sub New()
            MyBase.New()
            Add("Username", New ParamInfo With {.Required = False, .Name = "Username", .Type = GetType(String), .Description = "The username to connect to the endpoint", .DefaultValue = DatastreamrContext.Current.CurrentUser.Username})
            Add("Password", New ParamInfo With {.Required = False, .Name = "Password", .Type = GetType(String), .Description = "The password of the account"})
            Add("PersonIdentifier", New ParamInfo With {.Required = False, .Name = "PersonIdentifier", .Type = GetType(String), .Description = "EmployeeNumber,SocialSecurityNumber,BirthDate"})
            Add("UnitIdentifier", New ParamInfo With {.Required = False, .Name = "UnitIdentifier", .Type = GetType(String), .Description = "DepartmentCode,InternalId"})
        End Sub

        Public Sub New(streamParams As StreamParams)
            MyBase.New(streamParams)
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
        Property Password As String
            Get
                If Me("Password").Value Is Nothing Then Return CType(Me("Password").DefaultValue, String)
                Return CType(Me("Password").Value, String)
            End Get
            Set(value As String)
                Me("Password").Value = value
            End Set
        End Property
        Property PersonIdentifier As String
            Get
                If Me("PersonIdentifier").Value Is Nothing Then Return CType(Me("PersonIdentifier").DefaultValue, String)
                Return CType(Me("PersonIdentifier").Value, String)
            End Get
            Set(value As String)
                Me("PersonIdentifier").Value = value
            End Set
        End Property
        Property UnitIdentifier As String
            Get
                If Me("UnitIdentifier").Value Is Nothing Then Return CType(Me("UnitIdentifier").DefaultValue, String)
                Return CType(Me("UnitIdentifier").Value, String)
            End Get
            Set(value As String)
                Me("UnitIdentifier").Value = value
            End Set
        End Property
    End Class
End NameSpace