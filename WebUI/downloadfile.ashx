<%@ WebHandler Language="VB" Class="downloadfile" %>

Imports System
Imports System.Web
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Public Class downloadfile : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.Clear()
        If Not context.Request.QueryString("filename") Is Nothing Then
            Dim objCryption As Cryption = New Cryption()
            Dim strFileName As String
            strFileName = objCryption.Decrypt(context.Request.QueryString("filename"), objCryption.cryptionKey)

            Dim strExtension As String
            strExtension = Split(strFileName, ".")(1).ToLower()
            Dim strContentType As String = "text/plain"
            Select Case strExtension
                Case "pdf"
                    strContentType = "application/pdf"
                Case "bmp"
                    strContentType = "image/bmp"
                Case "gif"
                    strContentType = "image/gif"
                Case "jpeg", "jpg", "jpe"
                    strContentType = "image/jpeg"
            End Select
                
            Dim cVoucherCode As New VoucherCodeFunctions
            Dim sToken As String = cVoucherCode.GenerateVoucherCodeGuid(16)
            
            context.Response.ContentType = strContentType
            context.Response.AppendHeader("Content-Disposition", String.Format("attachment; filename={0}{1}.{2}", Now.ToString("yyyyMMddHHmmss"), sToken, strExtension))
            context.Response.TransmitFile(strFileName)
              
            context.Response.End()
        End If
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class