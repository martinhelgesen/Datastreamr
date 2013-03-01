Imports Noesis.Javascript

Public Class Mapper
    Public Shared Function Map(src As DataContainer, config As IList(Of MapInfo)) As DataContainer
        Dim ret As New DataContainer With {.Data = New List(Of Dictionary(Of String, Object))}

        For Each dictionary In src.Data
            Dim newDic As New Dictionary(Of String, Object)
            For Each m In config
                newDic.Add(m.ToName, GetValue(dictionary, src.MetaData, m))
            Next
            ret.Data.Add(newDic)
        Next
        Return ret
    End Function

    Private Shared Function GetValue(ByVal dictionary As Dictionary(Of String, Object),
                                     ByVal propertyDesc As List(Of PropertyDesc), ByVal mapInfo As MapInfo) As Object
        If Not dictionary.ContainsKey(mapInfo.FromName) Then
            Return Nothing
        End If
        If String.IsNullOrEmpty(mapInfo.Rule) Then
            Return dictionary(mapInfo.FromName)
        End If
        Using context As New JavascriptContext
            context.SetParameter("p", dictionary(mapInfo.FromName))
            context.SetParameter("newValue", "")
            Return context.Run("f = function(originalValue) {" + mapInfo.Rule + "}; newValue = f(p);")
        End Using
    End Function
End Class