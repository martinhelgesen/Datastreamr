Imports Datastreamr.Framework
Imports Datastreamr.Framework.DataStreams

Namespace DataStreams

    Public Class FtpFileStreamParams
        Inherits StreamReaderStreamParams
        Public Sub New()
            MyBase.New()
            Add("FilenameMatch", New ParamInfo With {.Required = False, .Name = "FilenameMatch", .Type = GetType(String), .Description = "Which file to choose"})
        End Sub
        Public Sub New(ByVal streamParams As StreamParams)
            MyBase.New(streamParams)
        End Sub
        Public Property FilenameMatch() As String
            Get
                If Me("FilenameMatch").Value Is Nothing Then Return CType(Me("FilenameMatch").DefaultValue, String)
                Return CType(Me("FilenameMatch").Value, String)
            End Get
            Set(value As String)
                Me("FilenameMatch").Value = value
            End Set
        End Property
    End Class
End Namespace