Imports Datastreamr.Framework.Logging

Public Interface IDatastreamrContext
    Inherits IDisposable
    Property CurrentUser() As User
    Property Logger As ILog
End Interface