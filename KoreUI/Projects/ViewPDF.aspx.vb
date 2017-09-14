Imports System.Net

Partial Class Projects_ViewPDF
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim pdfPath As String = Request.QueryString("path")
        If pdfPath <> "" Then
            Dim client As WebClient = New WebClient()
            Dim buffer As Byte() = client.DownloadData(pdfPath)
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-length", buffer.Length.ToString())
            Response.BinaryWrite(buffer)
        End If
    End Sub
End Class
