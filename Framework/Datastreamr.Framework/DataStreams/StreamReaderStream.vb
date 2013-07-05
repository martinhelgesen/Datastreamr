Imports Datastreamr.Framework.Utils
Imports System.IO
Imports Datastreamr.Framework.InternalStreams

Namespace DataStreams
    Public MustInherit Class StreamReaderStream(Of TParams As {BaseStreamParams, New})
        Inherits BaseDataStream(Of TParams)

        Public MustOverride Overrides ReadOnly Property Name As String
        Public MustOverride Overrides ReadOnly Property Description As String
        Public MustOverride Overrides Function GetStreamInternal() As DataContainer
    End Class
End Namespace