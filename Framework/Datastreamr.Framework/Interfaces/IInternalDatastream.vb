Namespace Interfaces

    Public Interface IInternalDatastream
        Inherits IHasId
        Function GetStream(ByVal params As StreamParams) As IEnumerable(Of Object)
        'Function GetParams() As StreamParams
        ReadOnly Property Name() As String
        ReadOnly Property Description() As String
    End Interface

    Public Interface IInternalDatastream(Of T, TParams As {StreamParams})
        Inherits IInternalDatastream
        Overloads Function GetStream(ByVal params As TParams) As IEnumerable(Of T)
    End Interface
End Namespace

Public Class StreamParams
    Inherits Dictionary(Of String, ParamInfo)

    Public Function GetValue(key As String) As Object
        If Me.ContainsKey(key) Then
            If Me(key).Value Is Nothing Then
                If Me(key).DefaultValue Is Nothing Then
                    If Me(key).Required = True Then
                        Throw New ArgumentException("Required parameter value missing", key)
                    End If
                Else
                    Return Me(key).DefaultValue
                End If
            Else
                Return Me(key).Value
            End If
        End If
        Throw New ArgumentException("Required parameter value missing. Possible corrupt parameterclass", key)
    End Function
End Class