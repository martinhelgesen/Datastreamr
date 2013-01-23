
Imports Datastreamr.Framework
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Namespace Streams
    Public Class PersonsInCompany
        Inherits InternalDatastream(Of Person, PersonsInCompanyParams)

        Public Overrides Function GetParams() As PersonsInCompanyParams
            Return New PersonsInCompanyParams()
        End Function

        Public Overrides ReadOnly Property Id() As Integer
            Get
                Return 1
            End Get
        End Property

        Public Overrides Function GetStream(ByVal params As PersonsInCompanyParams) As IEnumerable(Of Person)

            Dim client As New PersonServiceReference.PersonClient("BasicHttpBinding_IPerson")
            client.ClientCredentials.UserName.UserName = params.Username
            client.ClientCredentials.UserName.Password = params.Password
            Dim request As New PersonServiceReference.ExportPersonRequest
            With request
                .ClientLogMessage = "Dette er en test av eksport av personer"
                .FromUnit = params.CompanyId
                .IncludeDeactivated = False
                .IncludeAddresses = params.IncludeAddresses
                .IncludeChildren = params.IncludeChildren
                .IncludeEmailAddresses = params.IncludeEmailAddresses
                .IncludeNextOfKin = params.IncludeNextOfKin
                .IncludePhones = params.IncludePhones
                .IncludeSocialSecurityNumber = params.IncludeSocialSecurityNumber
                .IncludeEmployment = params.IncludeEmployment
                .MaxExportCount = params.MaxExportCount
            End With
            Dim response As ExportPersonResponse = client.Export(request)
            Return response.Persons
        End Function

        Public Overrides ReadOnly Property Description As String
            Get
                Return "All persons in a company"
            End Get
        End Property

        Public Overrides ReadOnly Property Name As String
            Get
                Return "Persons in company."
            End Get
        End Property

        Public Class PersonsInCompanyParams
            Inherits StreamParams

            Public Sub New()
                MyBase.New()
                Add("CompanyIdentifier", New ParamInfo With {.Name = "CompanyId", .Type = GetType(UnitIdentifier), .Required = False, .Description = "Please identify the company unit to fetch from"})
                Add("Username", New ParamInfo With {.Name = "Username", .Type = GetType(String), .Required = True, .Description = "The username to fetch data from HRessurs"})
                Add("Password", New ParamInfo With {.Name = "Password", .Type = GetType(String), .Required = True, .Description = "The password for the user"})
                Add("IncludeAddresses", New ParamInfo With {.Name = "IncludeAddresses", .Type = GetType(Boolean), .Required = False, .Description = "", .DefaultValue = False})
                Add("IncludeChildren", New ParamInfo With {.Name = "IncludeChildren", .Type = GetType(Boolean), .Required = False, .Description = "", .DefaultValue = False})
                Add("IncludeEmailAddresses", New ParamInfo With {.Name = "IncludeEmailAddresses", .Type = GetType(Boolean), .Required = False, .Description = "", .DefaultValue = False})
                Add("IncludeNextOfKin", New ParamInfo With {.Name = "IncludeNextOfKin", .Type = GetType(Boolean), .Required = False, .Description = "", .DefaultValue = False})
                Add("IncludePhones", New ParamInfo With {.Name = "IncludePhones", .Type = GetType(Boolean), .Required = False, .Description = "", .DefaultValue = False})
                Add("IncludeSocialSecurityNumber", New ParamInfo With {.Name = "IncludeSocialSecurityNumber", .Type = GetType(Boolean), .Required = False, .Description = "", .DefaultValue = False})
                Add("IncludeEmployment", New ParamInfo With {.Name = "IncludeEmployment", .Type = GetType(Boolean), .Required = False, .Description = "", .DefaultValue = False})
                Add("MaxExportCount", New ParamInfo With {.Name = "IncludeEmployment", .Type = GetType(Boolean), .Required = False, .Description = "", .DefaultValue = False})
            End Sub

            Property CompanyId() As UnitIdentifier
                Get
                    Return CType(Me("CompanyIdentifier").Value, UnitIdentifier)
                End Get
                Set(value As UnitIdentifier)
                    Me("CompanyIdentifier").Value = value
                End Set
            End Property

            Property Username() As String
                Get
                    Return CType(Me("Username").Value, String)
                End Get
                Set(value As String)
                    Me("Username").Value = value
                End Set
            End Property
            Property Password() As String
                Get
                    Return CType(Me("Password").Value, String)
                End Get
                Set(value As String)
                    Me("Password").Value = value
                End Set
            End Property

            Public Property IncludeAddresses() As Boolean
                Get
                    Return CType(Me("IncludeAddresses").Value, Boolean)
                End Get
                Set(ByVal value As Boolean)
                    Me("IncludeAddresses").Value = value
                End Set
            End Property

            Public Property IncludeChildren() As Boolean
                Get
                    Return CType(Me("IncludeChildren").Value, Boolean)
                End Get
                Set(ByVal value As Boolean)
                    Me("IncludeChildren").Value = value
                End Set
            End Property

            Public Property IncludeEmailAddresses() As Boolean
                Get
                    Return CType(Me("IncludeEmailAddresses").Value, Boolean)
                End Get
                Set(ByVal value As Boolean)
                    Me("IncludeEmailAddresses").Value = value
                End Set
            End Property

            Public Property IncludeNextOfKin() As Boolean
                Get
                    Return CType(Me("IncludeNextOfKin").Value, Boolean)
                End Get
                Set(ByVal value As Boolean)
                    Me("IncludeNextOfKin").Value = value
                End Set
            End Property

            Public Property IncludePhones() As Boolean
                Get
                    Return CType(Me("IncludePhones").Value, Boolean)
                End Get
                Set(ByVal value As Boolean)
                    Me("IncludePhones").Value = value
                End Set
            End Property

            Public Property IncludeSocialSecurityNumber() As Boolean
                Get
                    Return CType(Me("IncludeSocialSecurityNumber").Value, Boolean)
                End Get
                Set(ByVal value As Boolean)
                    Me("IncludeSocialSecurityNumber").Value = value
                End Set
            End Property

            Public Property IncludeEmployment() As Boolean
                Get
                    Return CType(Me("IncludeEmployment").Value, Boolean)
                End Get
                Set(ByVal value As Boolean)
                    Me("IncludeEmployment").Value = value
                End Set
            End Property

            Public Property MaxExportCount() As Integer
                Get
                    Return CType(Me("MaxExportCount").Value, Integer)
                End Get
                Set(ByVal value As Integer)
                    Me("MaxExportCount").Value = value
                End Set
            End Property

        End Class
    End Class
End Namespace

