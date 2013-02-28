Namespace Interfaces 

    Public Interface IDatastream(Of TParams As {StreamParams})   
        Function GetStream(ByVal params As TParams) As DataContainer
        Function GetParams() As TParams
        ReadOnly Property Name() As String
        ReadOnly Property Description() As String
    End Interface


End Namespace

Public Class DataContainer
    Property Data As List(Of Dictionary(Of String, Object))
    Property MetaData As List(Of PropertyDesc)
End Class

Public Class PropertyDesc
    Property Name() As String
    Property Type() As Type
    Property Description() As String
    Property MaxLength As String
End Class

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
Public Class HRPersonStreamParams
    Inherits StreamParams

    Public Sub New()
        Me.Add("p1", New ParamInfo)
        Me.Add("p2", New ParamInfo)
    End Sub
End Class