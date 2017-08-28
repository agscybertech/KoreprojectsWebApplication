<%@ WebHandler Language="VB" Class="Templates" %>

Imports System
Imports System.Web
Imports System.IO

Public Class Templates : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.ContentType = "text/plain"
        Try
            Using sr As New StreamReader("D:\Projects\A4PP_PDFINPUT\WebMobileApp\templates.html")
                Dim line As String
                line = sr.ReadToEnd()
                context.Response.Write(line)
            End Using
        Catch e As Exception
            context.Response.Write("The file could not be read:")
            context.Response.Write(e.Message)
        End Try
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class