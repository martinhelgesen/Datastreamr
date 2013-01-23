
Imports Raven.Client
Imports Raven.Client.Embedded

Namespace Entities.CustomerStream.DataAccess
    Partial Public Class CustomerStreamDataAccess
        Implements ICustomerStreamDataAccess

        Private _store As EmbeddableDocumentStore
        ReadOnly Property Store() As EmbeddableDocumentStore
            Get
                If _store Is Nothing Then
                    _store = New EmbeddableDocumentStore With {.DataDirectory = "localdatabase"}
                    _store.Initialize()                    
                End If
                Return _store
            End Get
        End Property

        Public Function GetEntity(id As Integer) As CustomerStream Implements ICustomerStreamDataAccess.GetEntity
            Using session = _store.OpenSession()
                Dim o = session.Load(Of CustomerStream)(id)
                Return o
            End Using
        End Function

        Public Function GetAll() As IEnumerable(Of CustomerStream) Implements ICustomerStreamDataAccess.GetAllForCustomer
            Using session = _store.OpenSession()
                Dim o = session.Query(Of CustomerStream).ToList()
                Return o
            End Using
        End Function

        Public Function Create(ByRef o As CustomerStream) As Boolean Implements ICustomerStreamDataAccess.Create
            Using session = _store.OpenSession()
                session.Store(o)
                session.SaveChanges()
            End Using
            Return True
        End Function

        Public Function Update(ByRef o As CustomerStream) As Boolean Implements ICustomerStreamDataAccess.Update
            Using session = _store.OpenSession()
                session.Store(o)
                session.SaveChanges()
            End Using
            Return True
        End Function
        Public Function Delete(ByRef o As CustomerStream) As Boolean Implements ICustomerStreamDataAccess.Delete
            Using session = _store.OpenSession()
                session.Delete(o)
                session.SaveChanges()
            End Using
            Return True
        End Function
    End Class
End Namespace
