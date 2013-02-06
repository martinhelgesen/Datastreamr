Namespace Transporters


    Public Interface ITransport
        Sub Transport(stream As IEnumerable(Of Object), params As TransportParams)
        Function GetParams() As TransportParams
    End Interface
    Public Interface ITransport(Of T)
        Inherits ITransport
        Overloads Sub Transport(stream As IEnumerable(Of T), params As TransportParams)
    End Interface

    Public Class TransportParams
        Inherits Dictionary(Of String, ParamInfo)
    End Class

    Public Class FtpTransporter
        Implements ITransport

        Public Sub Transport(stream As IEnumerable(Of Object), params As TransportParams) Implements ITransport.Transport
        End Sub


        Public Function GetParams() As TransportParams Implements ITransport.GetParams
            Return New FtpTransporterParams()
        End Function

        Public Class FtpTransporterParams
            Inherits TransportParams

            Sub New()
                Add("Address", New ParamInfo With {.Name = "Address", .Type = GetType(String), .Required = True, .Description = "The address to the Ftp server"})
                Add("Username", New ParamInfo With {.Name = "Username", .Type = GetType(String), .Required = False, .Description = "The username to authenticate with"})
                Add("Password", New ParamInfo With {.Name = "Password", .Type = GetType(String), .Required = False, .Description = "The password for the user"})
                Add("Filename", New ParamInfo With {.Name = "Filename", .Type = GetType(String), .Required = False, .Description = "The name of the file the stream will be saved as"})
            End Sub

            Property Address() As String
                Get
                    Return CType(Me("Address").Value, String)
                End Get
                Set(value As String)
                    Me("Address").Value = value
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
            Property Filename() As String
                Get
                    Return CType(Me("Filename").Value, String)
                End Get
                Set(value As String)
                    Me("Filename").Value = value
                End Set
            End Property

        End Class
    End Class

    'Public Class WebServiceTransporter
    '    Inherits BaseTransporter

    '    Public Overrides Function GetParams() As TransportParams
    '        Return New WebServiceTransporterParams()
    '    End Function

    '    Public Overrides Sub Transport(ByVal stream As String, ByVal params As TransportParams)
    '        Throw New NotImplementedException()
    '    End Sub

    '    Public Class WebServiceTransporterParams
    '        Inherits TransportParams

    '        Sub New()
    '            Add("Address", New ParamInfo With {.Name = "Address", .Type = GetType(String), .Required = True, .Description = "The address to the Ftp server"})
    '            Add("Username", New ParamInfo With {.Name = "Username", .Type = GetType(String), .Required = False, .Description = "The username to authenticate with"})
    '            Add("Password", New ParamInfo With {.Name = "Password", .Type = GetType(String), .Required = False, .Description = "The password for the user"})
    '        End Sub
    '    End Class
    'End Class
End Namespace