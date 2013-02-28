Public Class StreamParams
    Inherits Dictionary(Of String, ParamInfo)

    Public Function GetValue(key As String) As Object
        If ContainsKey(key) Then
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