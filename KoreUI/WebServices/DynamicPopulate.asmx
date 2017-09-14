<%@ WebService Language="VB" Class="DynamicPopulate" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services

<ScriptService()> _
Public Class DynamicPopulate
    Inherits System.Web.Services.WebService
    
    <WebMethod()> _
    Public Function GetHtml(ByVal contextKey As String) As String
        Return String.Format("{0}", contextKey)
    End Function

End Class
