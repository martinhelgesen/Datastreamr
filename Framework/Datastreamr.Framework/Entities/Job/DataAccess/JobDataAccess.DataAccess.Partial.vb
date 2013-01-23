
Imports Raven.Client
Imports Raven.Client.Embedded

Namespace Entities.Job.DataAccess
    Partial Public Class JobDataAccess
        Implements IJobDataAccess

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

        Public Function GetEntity(id As Integer) As Job Implements IJobDataAccess.GetEntity
            Using session = _store.OpenSession()
                Dim o = session.Load(Of Job)(id)
                Return o
            End Using
        End Function

        Public Function GetAll() As IEnumerable(Of Job) Implements IJobDataAccess.GetAllForCustomer
            Using session = _store.OpenSession()
                Dim o = session.Query(Of Job).ToList()
                Return o
            End Using
        End Function

        Public Function Create(ByRef o As Job) As Boolean Implements IJobDataAccess.Create
            Using session = _store.OpenSession()
                session.Store(o)
                session.SaveChanges()
            End Using
            Return True
        End Function

        Public Function Update(ByRef o As Job) As Boolean Implements IJobDataAccess.Update
            Using session = _store.OpenSession()
                session.Store(o)
                session.SaveChanges()
            End Using
            Return True
        End Function
        Public Function Delete(ByRef o As Job) As Boolean Implements IJobDataAccess.Delete
            Using session = _store.OpenSession()
                session.Delete(o)
                session.SaveChanges()
            End Using
            Return True
        End Function
    End Class
End Namespace
