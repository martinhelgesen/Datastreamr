Imports Datastreamr.Framework.Interfaces
Imports LazyFramework

Public MustInherit Class ProvidersFacade (Of T As IHasId)

    Private Shared _providers As Dictionary(Of Integer, T) = Nothing
    Private Shared PadLock As New Object

    Private Shared ReadOnly Property Providers As Dictionary(Of Integer, T)
        Get
            If _providers Is Nothing Then
                SyncLock PadLock
                    If _providers Is Nothing Then
                        Dim temp = New Dictionary(Of Integer, T)

                        For Each t In TypeValidation.FindAllClassesOfTypeInApplication(GetType(T))
                            Dim t2 As T = CType(Activator.CreateInstance(t), T)
                            temp.Add(t2.Id, t2)
                        Next

                        _providers = temp
                    End If
                End SyncLock
            End If
            Return _providers
        End Get
    End Property

    Public Shared Function GetAll() As IEnumerable(Of T)
        Return Providers.Values
    End Function

    Public Shared Function GetProvider(id As Integer) As T
        Return Providers(id)
    End Function
End Class