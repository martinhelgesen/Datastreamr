Imports Datastreamr.Framework.Interfaces

Public MustInherit Class InternalDatastream(Of T, TParams As {StreamParams})
    Implements IInternalDatastream(Of T, TParams)

    Public MustOverride ReadOnly Property Name As String Implements IInternalDatastream.Name
    Public MustOverride ReadOnly Property Description As String Implements IInternalDatastream.Description
    Public MustOverride ReadOnly Property Id() As Integer Implements IInternalDatastream.Id
    Public MustOverride Overloads Function GetStream(ByVal params As TParams) As IEnumerable(Of T) Implements IInternalDatastream(Of T, TParams).GetStream

    Private Function IInternalDatastream_GetStream(ByVal params As StreamParams) As IEnumerable(Of Object) Implements IInternalDatastream.GetStream
        Return CType(GetStream(CType(params, TParams)), IEnumerable(Of Object))
    End Function

    Public MustOverride Function GetParams() As TParams

End Class
