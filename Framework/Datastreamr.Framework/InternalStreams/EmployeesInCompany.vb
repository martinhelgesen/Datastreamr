

Namespace InternalStreams
    'Public Class EmployeesInCompany
    '    Inherits InternalDatastream(Of HRUnitPersonEmployee)

    '    Public Overrides Function GetStream(ByVal params As IDictionary(Of String, ParamInfo)) As IEnumerable(Of HRUnitPersonEmployee)
    '        'Dim companyid As Integer = CType(params("CompanyId").Value, Integer)
    '        'Dim company = Personalarkiv.Facade.Company.GetInstance(companyid)
    '        'Dim employees = company.HRUnitPersonEmployees
    '        'Return CType(employees, IEnumerable(Of HRUnitPersonEmployee))
    '    End Function

    '    Public Overrides Function GetParams() As IDictionary(Of String, ParamInfo)
    '        Return New EmployeesInCompanyParams()
    '    End Function

    '    Public Overrides ReadOnly Property Name() As String
    '        Get
    '            Return "Alle ansatte i et firma"
    '        End Get
    '    End Property

    '    Public Overrides ReadOnly Property Description() As String
    '        Get
    '            Return "Alle ansatte i et firma"
    '        End Get
    '    End Property

    '    Public Overrides ReadOnly Property Id() As Integer
    '        Get
    '            Return 2
    '        End Get
    '    End Property

    '    Public Class EmployeesInCompanyParams
    '        Inherits Dictionary(Of String, ParamInfo)

    '        Public Sub New()
    '            Add("CompanyId", New ParamInfo With {.Name = "CompanyId", .Required = True, .Type = "Integer"})
    '        End Sub

    '        Property CompanyId() As Integer
    '            Get
    '                Return CType(Me("CompanyId").Value, Integer)
    '            End Get
    '            Set(value As Integer)
    '                Me("CompanyId").Value = value
    '            End Set
    '        End Property

    '    End Class
    'End Class
End Namespace