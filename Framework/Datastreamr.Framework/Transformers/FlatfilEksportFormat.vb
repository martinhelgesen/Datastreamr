

Namespace Transformers
    'Public Class FlatfilEksportFormat
    '    Implements ITransformers(Of HRUnitPersonEmployee)


    '    Private _params As IDictionary(Of String, ParamInfo)

    '    Public ReadOnly Property Id As Integer Implements IHasId.Id
    '        Get
    '            Return 1
    '        End Get
    '    End Property

    '    Public ReadOnly Property Description As String Implements ITransformers.Description
    '        Get
    '            Return "Flatfil fra eksport format"
    '        End Get
    '    End Property

    '    Public ReadOnly Property ContentType() As String Implements ITransformers.ContentType
    '        Get
    '            Return "text/plain"
    '        End Get
    '    End Property


    '    Public Function Transform(ByVal list As IEnumerable(Of Object), ByVal params As IDictionary(Of String, ParamInfo)) As String Implements ITransformers.Transform
    '        Return InternalTransform(CType(list, IEnumerable(Of HRUnitPersonEmployee)), params)
    '    End Function

    '    Public ReadOnly Property Name As String Implements ITransformers.Name
    '        Get
    '            Return "Flatfil fra eksport format"
    '        End Get
    '    End Property

    '    Public Function GetParameters() As IDictionary(Of String, ParamInfo) Implements ITransformers(Of HRUnitPersonEmployee).GetParameters
    '        'Return New Dictionary(Of String, ParamInfo) From {{"", New ParamInfo With {.Name = ""}}}
    '        Return New FlatfilEksportFormatParams()
    '    End Function

    '    Public Function InternalTransform(list As IEnumerable(Of HRUnitPersonEmployee), params As IDictionary(Of String, ParamInfo)) As String Implements ITransformers(Of HRUnitPersonEmployee).InternalTransform
    '        Dim exportHeaderId As Integer = CType(params("ExportHeaderId").Value, Integer)

    '        Dim exportEmployeeParams = New ExportEmployeeParams With {
    '            .UseFixedDepartment = CType(params("UseFixedDepartment").Value, Boolean),
    '            .ExportSelfDeclaration = CType(params("ExportSelfDeclaration").Value, Boolean),
    '            .ExportVacation = CType(params("ExportVacation").Value, Boolean)
    '            }
    '        If params("ExportChangedEmployeesSince").Value IsNot Nothing Then
    '            exportEmployeeParams.ExportChangedEmployeesSince = CType(params("ExportChangedEmployeesSince").Value, Date)
    '        End If

    '        Return ExportArchive.GetEmployees(list, exportHeaderId, exportEmployeeParams)
    '    End Function

    '    Public Class FlatfilEksportFormatParams
    '        Inherits Dictionary(Of String, ParamInfo)

    '        Public Sub New()
    '            Add("ExportHeaderId", New ParamInfo With {.Name = "ExportHeaderId", .Required = True, .Type = "Integer"})
    '            Add("UseFixedDepartment", New ParamInfo With {.Name = "UseFixedDepartment", .Required = True, .Type = "Boolean"})
    '            Add("ExportChangedEmployeesSince", New ParamInfo With {.Name = "ExportHeaderId", .Required = False, .Type = "Date?"})
    '            Add("ExportVacation", New ParamInfo With {.Name = "ExportVacation", .Required = True, .Type = "Boolean"})
    '            Add("ExportSelfDeclaration", New ParamInfo With {.Name = "ExportSelfDeclaration", .Required = True, .Type = "Boolean"})
    '        End Sub

    '        Public Property ExportHeaderId As Integer
    '            Get
    '                Return CType(Me("ExportHeaderId").Value, Integer)
    '            End Get
    '            Set(value As Integer)
    '                Me("ExportHeaderId").Value = value
    '            End Set
    '        End Property

    '        Public Property UseFixedDepartment As Boolean
    '            Get
    '                Return CType(Me("UseFixedDepartment").Value, Boolean)
    '            End Get
    '            Set(value As Boolean)
    '                Me("UseFixedDepartment").Value = value
    '            End Set
    '        End Property

    '        Public Property ExportVacation As Boolean
    '            Get
    '                Return CType(Me("ExportVacation").Value, Boolean)
    '            End Get
    '            Set(value As Boolean)
    '                Me("ExportVacation").Value = value
    '            End Set
    '        End Property

    '        Public Property ExportSelfDeclaration As Boolean
    '            Get
    '                Return CType(Me("ExportSelfDeclaration").Value, Boolean)
    '            End Get
    '            Set(value As Boolean)
    '                Me("ExportSelfDeclaration").Value = value
    '            End Set
    '        End Property

    '        Public Property ExportChangedEmployeesSince() As Nullable(Of DateTime)
    '            Get
    '                Return CType(Me("ExportChangedEmployeesSince").Value, Nullable(Of DateTime))
    '            End Get
    '            Set(value As Nullable(Of DateTime))
    '                Me("ExportChangedEmployeesSince").Value = value
    '            End Set
    '        End Property
    '    End Class
    'End Class
End Namespace
