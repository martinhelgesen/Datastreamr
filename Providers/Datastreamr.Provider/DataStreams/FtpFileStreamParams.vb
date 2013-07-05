Imports Datastreamr.Framework
Imports Datastreamr.Framework.DataStreams

Namespace DataStreams

    ''' <summary>
    ''' Responsible for opening file content
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BaseFileStreamParams
        Inherits BaseStreamParams

        'Private _specificParams As T

        Public Sub New()
            MyBase.New()
            Add("FilenameMatch", New ParamInfo With {.Required = False, .Name = "FilenameMatch", .Type = GetType(String), .Description = "Which file to choose"})
            'SpecificParams = New T
            'For Each p In SpecificParams
            '    Add(p.Key, p.Value)
            'Next
        End Sub
        Public Sub New(ByVal baseStreamParams As BaseStreamParams)
            MyBase.New(baseStreamParams)
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

