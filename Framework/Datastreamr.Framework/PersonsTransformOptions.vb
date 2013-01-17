

'Public Class PersonsTransformOptions
'    Inherits Dictionary(Of String, ParamInfo)
'    Implements ITransformPersonOptions

'    Private _paramInfos As IDictionary(Of String, ParamInfo)

'    Public Sub New()
'        Add("PersonIdentifierType", New ParamInfo With {.Name = "PersonIdentifierType", .Required = True, .Type = "Integer"})
'        Add("UnitIdentifierType", New ParamInfo With {.Name = "UnitIdentifierType", .Required = True, .Type = "Integer"})
'        Add("IncludeDeactivated", New ParamInfo With {.Name = "IncludeDeactivated", .Required = True, .Type = "Boolean"})
'        Add("IncludeChildren", New ParamInfo With {.Name = "IncludeChildren", .Required = True, .Type = "Boolean"})
'        Add("IncludePhones", New ParamInfo With {.Name = "IncludePhones", .Required = True, .Type = "Boolean"})
'        Add("IncludeEmailAddresses", New ParamInfo With {.Name = "IncludeEmailAddresses", .Required = True, .Type = "Boolean"})
'        Add("IncludeAddresses", New ParamInfo With {.Name = "IncludeAddresses", .Required = True, .Type = "Boolean"})
'        Add("IncludeNextOfKin", New ParamInfo With {.Name = "IncludeNextOfKin", .Required = True, .Type = "Boolean"})
'        Add("IncludeSocialSecurityNumber", New ParamInfo With {.Name = "IncludeSocialSecurityNumber", .Required = True, .Type = "Boolean"})
'        Add("IncludeEmployment", New ParamInfo With {.Name = "IncludeEmployment", .Required = True, .Type = "Boolean"})
'        Add("IncludePersonalImage", New ParamInfo With {.Name = "IncludePersonalImage", .Required = True, .Type = "Boolean"})
'    End Sub

'    Public Property PersonIdentifierType() As PersonIdentifierType? Implements IUnitIdentifierOptions.PersonIdentifierType
'        Get
'            Return CType(Me("PersonIdentifierType").Value, ServiceDataContracts.PersonIdentifierType?)
'        End Get
'        Set(value As PersonIdentifierType?)
'            Me("PersonIdentifierType").Value = value
'        End Set
'    End Property
'    Public Property UnitIdentifierType() As UnitIdentifierType? Implements IUnitIdentifierOptions.UnitIdentifierType
'        Get
'            Return CType(Me("UnitIdentifierType").Value, ServiceDataContracts.UnitIdentifierType?)
'        End Get
'        Set(value As UnitIdentifierType?)
'            Me("UnitIdentifierType").Value = value
'        End Set
'    End Property
'    Public Property IncludeDeactivated() As Boolean Implements ITransformPersonOptions.IncludeDeactivated
'        Get
'            Return CBool(Me("IncludeDeactivated").Value)
'        End Get
'        Set(value As Boolean)
'            Me("IncludeDeactivated").Value = value
'        End Set
'    End Property
'    Public Property IncludeChildren() As Boolean Implements ITransformPersonOptions.IncludeChildren
'        Get
'            Return CBool(Me("IncludeChildren").Value)
'        End Get
'        Set(value As Boolean)
'            Me("IncludeChildren").Value = value
'        End Set
'    End Property
'    Public Property IncludePhones() As Boolean Implements ITransformPersonOptions.IncludePhones
'        Get
'            Return CBool(Me("IncludePhones").Value)
'        End Get
'        Set(value As Boolean)
'            Me("IncludePhones").Value = value
'        End Set
'    End Property
'    Public Property IncludeEmailAddresses() As Boolean Implements ITransformPersonOptions.IncludeEmailAddresses
'        Get
'            Return CBool(Me("IncludeEmailAddresses").Value)
'        End Get
'        Set(value As Boolean)
'            Me("IncludeEmailAddresses").Value = value
'        End Set
'    End Property
'    Public Property IncludeAddresses() As Boolean Implements ITransformPersonOptions.IncludeAddresses
'        Get
'            Return CBool(Me("IncludeAddresses").Value)
'        End Get
'        Set(value As Boolean)
'            Me("IncludeAddresses").Value = value
'        End Set
'    End Property
'    Public Property IncludeNextOfKin() As Boolean Implements ITransformPersonOptions.IncludeNextOfKin
'        Get
'            Return CBool(Me("IncludeNextOfKin").Value)
'        End Get
'        Set(value As Boolean)
'            Me("IncludeNextOfKin").Value = value
'        End Set
'    End Property
'    Public Property IncludeSocialSecurityNumber() As Boolean Implements ITransformPersonOptions.IncludeSocialSecurityNumber
'        Get
'            Return CBool(Me("IncludeSocialSecurityNumber").Value)
'        End Get
'        Set(value As Boolean)
'            Me("IncludeSocialSecurityNumber").Value = value
'        End Set
'    End Property
'    Public Property IncludeEmployment() As Boolean Implements ITransformPersonOptions.IncludeEmployment
'        Get
'            Return CBool(Me("IncludeEmployment").Value)
'        End Get
'        Set(value As Boolean)
'            Me("IncludeEmployment").Value = value
'        End Set
'    End Property
'    Public Property IncludePersonalImage() As Boolean Implements ITransformPersonOptions.IncludePersonalImage
'        Get
'            Return CBool(Me("IncludePersonalImage").Value)
'        End Get
'        Set(value As Boolean)
'            Me("IncludePersonalImage").Value = value
'        End Set
'    End Property
'End Class