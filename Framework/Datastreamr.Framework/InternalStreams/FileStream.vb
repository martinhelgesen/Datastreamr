Imports Datastreamr.Framework.Interfaces
Imports System.IO

Namespace InternalStreams
    Public Class FileStream
        Implements IDatastream(Of FileStreamParams)

        Public ReadOnly Property Description As String Implements IDatastream(Of FileStreamParams).Description
            Get
                Return "Returns content of file in users Datastreamr FTP catalog"
            End Get
        End Property

        Public Function GetParams() As FileStreamParams Implements IDatastream(Of FileStreamParams).GetParams
            Return New FileStreamParams
        End Function

        Public Function GetStream(params As FileStreamParams) As DataContainer Implements IDatastream(Of FileStreamParams).GetStream
            Dim currentUser = DatastreamrContext.GetCurrentUser
            Dim rootCat = currentUser.rootCat + "\incoming\"
            Dim sr = FindFiles(rootCat, params.FilenameMatch)
            Return ConvertToDataContainer(sr,params)

        End Function

        Private Function ConvertToDataContainer(ByVal streamReader As StreamReader, ByVal fileStreamParams As FileStreamParams) As DataContainer
            Dim retval As New DataContainer
            If fileStreamParams.FirstLineIsHeader Then
                Dim line = streamReader.ReadLine
                'line.Split(CType(fileStreamParams.ValueSeparator, Char))
            End If

            retval.MetaData = Nothing
            Return retval
        End Function

        Private Function FindFiles(ByVal rootCat As String, ByVal filenameMatch As Object) As System.IO.StreamReader
            'Return
        End Function

        Public ReadOnly Property Name As String Implements IDatastream(Of FileStreamParams).Name
            Get
                Return "Datastreamr filestream reader"
            End Get
        End Property
    End Class

    Public Class DatastreamrContext
        Public Shared Current As DatastreamrContext

        Public Sub New()
            DatastreamrContext.Current = Me
        End Sub

        Public Property CurrentUser() As User
    End Class

    Public Class User
        Property Username As String
        Property Password As String
        Property FTPRootCatalog As String
    End Class

    Public Class FileStreamParams
        Inherits StreamParams

        Public Sub New()
            Add("FilenameMatch", New ParamInfo)
            Add("FirstLineIsHeader", New ParamInfo)
            Add("ValueSeparator", New ParamInfo)
            Add("FixedPositionDescriptor", New ParamInfo)
        End Sub

        Public Property FilenameMatch() As Object
        Property FirstLineIsHeader As Boolean
        Property ValueSeparator As String
    End Class
End Namespace