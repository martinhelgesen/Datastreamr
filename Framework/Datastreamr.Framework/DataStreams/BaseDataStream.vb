Imports Datastreamr.Framework.Interfaces
Imports Newtonsoft.Json

Namespace InternalStreams
    Public MustInherit Class BaseDataStream(Of TParams As {BaseStreamParams, New})
        Implements IDatastream(Of TParams)

        Public MustOverride ReadOnly Property Description As String Implements IDatastream.Description

        Public Sub SetParams(ByVal dataBaseStreamParams As BaseStreamParams) Implements IDatastream.SetParams
            _StreamParams = New TParams
            _StreamParams.AddParams(dataBaseStreamParams)
        End Sub

        Public MustOverride ReadOnly Property Name As String Implements IDatastream.Name

        'Private _streamParams As TParams = Nothing
        <JsonProperty(ItemTypeNameHandling:=TypeNameHandling.Auto)>
        Public Property StreamParams As TParams Implements IDatastream(Of TParams).StreamParams

        Public Function GetParams() As TParams Implements IDatastream(Of TParams).GetParams
            Return New TParams
        End Function

        Public Function GetStream() As DataContainer Implements IDatastream(Of TParams).GetStream
            If StreamParams Is Nothing Then
                _StreamParams = New TParams()
            End If
            Return GetStreamInternal()
        End Function

        Public MustOverride Function GetStreamInternal() As DataContainer Implements IDatastream.GetStreamInternal

    End Class
End NameSpace