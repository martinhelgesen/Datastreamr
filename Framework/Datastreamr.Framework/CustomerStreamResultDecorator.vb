Imports Datastreamr.Framework.Entities.CustomerStream
Imports Datastreamr.Framework.Interfaces
Imports Newtonsoft.Json

''' <summary>
''' Returns the result of the CustomerStream. If transformer is registered then stream is applied with the transformer
''' </summary>
''' <remarks></remarks>
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

    Public _inner As CustomerStream
    Public Sub New(stream As CustomerStream)
        _inner = stream
        Id = stream.Id
        Name = stream.Name
        StreamTypeId = stream.StreamtypeId
        TransformerTypeId = stream.TransformertypeId
        Params = stream.Params
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

    Public Function Result() As IEnumerable(Of Object)
        Dim sourceStream = Stream.GetStream(Params)
        If Transformer IsNot Nothing Then
            Return Transformer.Transform(sourceStream, TransformerParams)
        End If
        Return sourceStream
    End Function
End Class