Imports DataStreamr.Persistence.Interfaces

Public Class Persister

    Public Shared Function Current() As IPersister
        Return LazyFramework.ClassFactory.GetTypeInstance(Of IPersister, FilePersister)()
    End Function

End Class
