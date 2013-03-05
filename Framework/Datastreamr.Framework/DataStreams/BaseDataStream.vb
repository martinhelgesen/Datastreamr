Imports Datastreamr.Framework.Interfaces

Namespace InternalStreams
    Public MustInherit Class BaseDataStream (Of TParams As {New, StreamParams})
        Implements IDatastream(Of TParams)



        Public MustOverride ReadOnly Property Description As String Implements IDatastream.Description
        Public MustOverride ReadOnly Property Name As String Implements IDatastream.Name
        'Protected Friend MustOverride Function GetStreamInternal(ByVal params As StreamParams) As DataContainer

        Public Shared Function GetParams() As TParams
            Return New TParams
        End Function

        Public Function GetStream(params As TParams) As DataContainer Implements IDatastream(Of TParams).GetStream
            If params Is Nothing Then
                params = New TParams()
            End If
            Return GetStreamInternal(params)
        End Function

        Public MustOverride Function GetStreamInternal(params As StreamParams) As DataContainer Implements IDatastream.GetStreamInternal
    End Class
End NameSpace