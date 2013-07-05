Public Class BaseStreamParams
    Inherits Dictionary(Of String, ParamInfo)

    Public Sub New()

    End Sub

    Public Sub AddParams(params As BaseStreamParams)
        For Each p In params
            If Not Me.ContainsKey(p.Key) Then
                Add(p.Key, p.Value)
            Else
                Me(p.Key) = p.Value
            End If
        Next
    End Sub

    Public Sub New(params As BaseStreamParams)
        Me.New()
        AddParams(params)
    End Sub

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