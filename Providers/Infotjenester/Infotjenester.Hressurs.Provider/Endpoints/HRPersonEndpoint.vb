Imports Datastreamr.Framework
Imports Datastreamr.Framework.Interfaces
Imports LazyFramework
Imports Infotjenester.Hressurs.Provider.PersonServiceReference

Namespace Endpoints

    Public Class HRPersonEndpoint
        Inherits TypeSafeEndPoint(Of HRPersonParams, HRPerson)

        Public Overrides Function Deliver(params As HRPersonParams, values As DataContainer) As EndPointResult
            'Validate
            ValidateParams(params)

            'Transform values to HRPerson objects
            Dim persons As List(Of Person) = (From dic In values.Data Select InternalTransform(dic)).ToList()

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
        Public MustOverride ReadOnly Property GetObjectMapInfo As List(Of ParamInfo) Implements IEndpoint(Of TParams).GetObjectMapInfo

        Public Function GetParams() As TParams Implements IEndpoint(Of TParams).GetParams
            Return New TParams
        End Function

        Public Function InternalDeliver(params As StreamParams, data As DataContainer) As EndPointResult Implements IEndpoint.InternalDeliver
            Dim p = Activator.CreateInstance(GetType(TParams), params)
            Return Deliver(CType(p, TParams), data)
        End Function
    End Class

    Public MustInherit Class TypeSafeEndPoint(Of TParams As {New, StreamParams}, TMapInfo)
        Inherits BaseEndpoint(Of TParams)
        Public Overrides ReadOnly Property GetObjectMapInfo As List(Of ParamInfo)
            Get
                Dim retval As New List(Of ParamInfo)
                For Each prop In GetType(TMapInfo).GetProperties
                    retval.Add(New ParamInfo With {.Name = prop.Name, .Type = prop.PropertyType})
                Next
                Return retval
            End Get
        End Property

        'Public MustOverride Function InternalTransform(dictionary As Dictionary(Of String, Object)) As TMapInfo
    End Class
End Namespace