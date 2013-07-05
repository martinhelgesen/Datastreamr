﻿Imports Noesis.Javascript
Imports LazyFramework

Public Class JobExecutor
    Private ReadOnly _job As JobEntity
    'Private _context As DatastreamrContext

    Public Sub New(job As JobEntity)
        _job = job
        '_context = context
        ValidateJob()
    End Sub

    Private Sub ValidateJob()
        'Throw New NotImplementedException()
    End Sub

    Public Function Execute() As JobResult
        Dim mapresult As DataContainer = Nothing
        Dim endpointResult As EndPointResult = Nothing
        Try
            InitializeV8Engine()

            Dim dataContainer = _job.DataStream.GetStreamInternal()
            Dim endpoint = _job.Endpoint
            mapresult = Mapper.Map(dataContainer, _job.Mapconfig)
            endpointResult = endpoint.InternalDeliver(mapresult)
            Return New JobResult With {.EndpointResult = endpointResult, .MapResult = mapresult, .Success = True}
        Catch ex As Exception
            Return New JobResult With {.EndpointResult = endpointResult, .MapResult = mapresult, .Success = False, .Exception = ex}
        End Try
    End Function

    Private Sub InitializeV8Engine()
        Try
            Using context = New JavascriptContext
                context.Run("return true;")
            End Using
        Catch ex As Exception
        End Try
    End Sub
End Class

