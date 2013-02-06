Imports Datastreamr.Framework.Interfaces
Imports Datastreamr.Framework
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Public Class SalesForcePersonTransformer
    Implements ITransformers(Of Person, SalesForcePerson)

    Public ReadOnly Property Id() As Integer Implements IHasId.Id
        Get

        End Get
    End Property

    Public Function Transform(ByVal list As IEnumerable(Of Object), ByVal params As IDictionary(Of String, ParamInfo)) As IEnumerable(Of Object) Implements ITransformers.Transform
        Throw New NotImplementedException()
    End Function

    Public ReadOnly Property Name() As String Implements ITransformers.Name
        Get

        End Get
    End Property

    Public ReadOnly Property Description() As String Implements ITransformers.Description
        Get

        End Get
    End Property

    Public ReadOnly Property ContentType() As String Implements ITransformers.ContentType
        Get

        End Get
    End Property

    Public Function InternalTransform(ByVal list As IEnumerable(Of Person), ByVal params As IDictionary(Of String, ParamInfo)) As IEnumerable(Of SalesForcePerson) Implements ITransformers(Of Person, SalesForcePerson).InternalTransform
        Throw New NotImplementedException()
    End Function

    Public Function GetParameters() As IDictionary(Of String, ParamInfo) Implements ITransformers(Of Person, SalesForcePerson).GetParameters
        Throw New NotImplementedException()
    End Function
End Class

Public Class SalesForcePerson
    Property Id() As Integer
    Property FirstName() As String
    Property Age() As String
End Class
