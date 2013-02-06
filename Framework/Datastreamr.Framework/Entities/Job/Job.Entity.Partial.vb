Imports Datastreamr.Framework.Transporters

Namespace Entities.Job
    Partial Class Job
        Property Id() As Integer
        Property UserId() As Integer
        Property CustomerStreamId() As Integer
        Property TransporterTypeId() As Integer
        Property TransporterParams() As TransportParams
        Property ScheduledTask() As ScheduledTask
    End Class

    Public Class ScheduledTask
        Property Frequency() As TimeSpan
        Property LastRun() As JobResult
        ReadOnly Property NextRun() As DateTime
            Get
                Return LastRun.JobDate.Add(Frequency)
            End Get
        End Property
    End Class


    Public Class JobResult
        Property JobDate As DateTime
        Property Success As Boolean
        Property Output() As String
        Property Input() As String
    End Class

End Namespace
