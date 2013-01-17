
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
                    Throw New NotImplementedException()
                End Get
                Set(ByVal value As Boolean)
                    Throw New NotImplementedException()
                End Set
            End Property

            Public Property IncludeChildren() As Boolean
                Get
                    Throw New NotImplementedException()
                End Get
                Set(ByVal value As Boolean)
                    Throw New NotImplementedException()
                End Set
            End Property

            Public Property IncludeEmailAddresses() As Boolean
                Get
                    Throw New NotImplementedException()
                End Get
                Set(ByVal value As Boolean)
                    Throw New NotImplementedException()
                End Set
            End Property

            Public Property IncludeNextOfKin() As Boolean
                Get
                    Throw New NotImplementedException()
                End Get
                Set(ByVal value As Boolean)
                    Throw New NotImplementedException()
                End Set
            End Property

            Public Property IncludePhones() As Boolean
                Get
                    Throw New NotImplementedException()
                End Get
                Set(ByVal value As Boolean)
                    Throw New NotImplementedException()
                End Set
            End Property

            Public Property IncludeSocialSecurityNumber() As Boolean
                Get
                    Throw New NotImplementedException()
                End Get
                Set(ByVal value As Boolean)
                    Throw New NotImplementedException()
                End Set
            End Property

            Public Property IncludeEmployment() As Boolean
                Get
                    Throw New NotImplementedException()
                End Get
                Set(ByVal value As Boolean)
                    Throw New NotImplementedException()
                End Set
            End Property

            Public Property MaxExportCount() As Integer?
                Get
                    Throw New NotImplementedException()
                End Get
                Set(ByVal value As Integer?)
                    Throw New NotImplementedException()
                End Set
            End Property
        End Class
    End Class
End Namespace

