Imports Datastreamr.Framework.Interfaces
Imports Datastreamr.Framework

Namespace Endpoints
    Public MustInherit Class BaseEndpoint(Of TParams As {New, StreamParams})
        Implements IEndpoint(Of TParams)



        Public MustOverride Function Deliver(params As TParams, data As DataContainer) As EndPointResult Implements IEndpoint(Of TParams).Deliver
        Public MustOverride ReadOnly Property Name As String
        Public MustOverride ReadOnly Property Description As String
        Public MustOverride ReadOnly Property GetObjectMapInfo As List(Of ParamInfo) Implements IEndpoint(Of TParams).GetObjectMapInfo

        Public Function GetParams() As TParams Implements IEndpoint(Of TParams).GetParams
            Return New TParams
        End Function

        Public Function InternalDeliver(params As StreamParams, data As DataContainer) As EndPointResult Implements IEndpoint.InternalDeliver
            Dim p = Activator.CreateInstance(GetType(TParams), params)
            Return Deliver(CType(p, TParams), data)
        End Function
    End Class
End NameSpace