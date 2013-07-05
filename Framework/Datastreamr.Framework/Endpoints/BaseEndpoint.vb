Imports Datastreamr.Framework.Interfaces

Namespace Endpoints

    Public MustInherit Class BaseEndpoint(Of TParams As {New, BaseStreamParams})
        Implements IEndpoint(Of TParams)


        Public MustOverride Function Deliver(data As DataContainer) As EndPointResult Implements IEndpoint(Of TParams).Deliver
        Public MustOverride ReadOnly Property Name As String
        Public MustOverride ReadOnly Property Description As String
        Public MustOverride ReadOnly Property GetObjectMapInfo As List(Of ParamInfo) Implements IEndpoint(Of TParams).GetObjectMapInfo

        Public Function InternalDeliver(data As DataContainer) As EndPointResult Implements IEndpoint.InternalDeliver
            Return Deliver(data)
        End Function

        Public Property StreamParams As TParams Implements IEndpoint(Of TParams).StreamParams

        Public Sub SetParams(dataBaseStreamParams As BaseStreamParams) Implements IEndpoint.SetParams
            _StreamParams = New TParams
            _StreamParams.AddParams(dataBaseStreamParams)
        End Sub

        Public Function GetParams() As TParams Implements IEndpoint(Of TParams).GetParams
            Return New TParams
        End Function
    End Class
End Namespace