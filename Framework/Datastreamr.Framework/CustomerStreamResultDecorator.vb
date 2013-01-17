Imports Datastreamr.Framework.Entities.CustomerStream
Imports Datastreamr.Framework.Interfaces
Imports Newtonsoft.Json

Public Class CustomerStreamResultDecorator
    Private _stream As IInternalDatastream
    Private _transformer As ITransformers
    Property Id() As Integer
    Property Name() As String
    Property StreamTypeId() As Integer
    Property TransformerTypeId() As Integer?
    Property Params() As New StreamParams
    Property TransformerParams As New StreamParams
    ReadOnly Property ContentType() As String
        Get
            Return Transformer.ContentType
        End Get
    End Property

    Public Sub New()

    End Sub

    Private _innerObject As CustomerStream
    Public Sub New(stream As CustomerStream)
        _innerObject = stream
        Id = stream.Id
        Name = stream.Name
        StreamTypeId = stream.StreamtypeId
        TransformerTypeId = stream.TransformertypeId
        stream.Params = stream.Params
        TransformerParams = stream.TransformerParams
    End Sub

    Public Sub New(ByVal customerStream As CustomerStream, ByVal overridedStreamParams As Dictionary(Of String, String), ByVal overridedTransformerParams As Dictionary(Of String, String))
        Me.New(customerStream)
        OverrideParams(overridedStreamParams, overridedTransformerParams)
    End Sub

    Private Sub OverrideParams(ByVal overridedStreamParams As Dictionary(Of String, String), ByVal overridedTransformerParams As Dictionary(Of String, String))
        If overridedStreamParams IsNot Nothing Then
            For Each overridedStreamParam In overridedStreamParams
                If Params.ContainsKey(overridedStreamParam.Key) Then
                    Params(overridedStreamParam.Key).Value = overridedStreamParam.Value
                End If
            Next
        End If
        If overridedTransformerParams IsNot Nothing Then
            For Each overridedParam In overridedTransformerParams
                If TransformerParams.ContainsKey(overridedParam.Key) Then
                    TransformerParams(overridedParam.Key).Value = overridedParam.Value
                End If
            Next
        End If
    End Sub

    'Public Shared Function GenerateStreamFromString(s As String) As Stream
    '    Dim str As New MemoryStream()
    '    Dim writer As New StreamWriter(str)
    '    writer.Write(s)
    '    writer.Flush()
    '    str.Position = 0
    '    Return str
    'End Function

    Private ReadOnly Property Stream() As IInternalDatastream
        Get
            If _stream Is Nothing Then
                _stream = InternalDatastreamFacade.GetProvider(StreamTypeId)
            End If
            Return _stream
        End Get
    End Property
    Private ReadOnly Property Transformer As ITransformers
        Get
            If _transformer Is Nothing AndAlso TransformerTypeId.HasValue Then
                _transformer = TransformerFacade.GetProvider(TransformerTypeId.Value)
            End If
            Return _transformer
        End Get
    End Property

    Function GetStream() As IEnumerable(Of Object)
        Return Stream.GetStream(Params)
    End Function

    Public Function Result() As String
        Dim sourceStream = Stream.GetStream(Params)
        If Transformer IsNot Nothing Then
            Return Transformer.Transform(sourceStream, TransformerParams)
        End If
        Return JsonConvert.SerializeObject(sourceStream)
    End Function
End Class