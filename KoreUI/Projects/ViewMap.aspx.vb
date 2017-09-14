Imports System
Imports System.Collections
Imports System.Linq
Imports System.Configuration
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities
Imports System.Data
Imports System.Data.SqlClient

Partial Class Projects_ViewMap
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Public m_Cryption As New Cryption()
    Private m_ProjectID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ProjectID)
        End If

        If m_ProjectID > 0 Then
            Dim mapView As HtmlControl = CType(Me.FindControl("mapview"), HtmlControl)
            Dim currentProject As Project = m_ManagementService.GetProjectByProjectId(m_ProjectID)
            If Not currentProject Is Nothing Then
                mapView.Attributes("src") = String.Format("https://maps.google.co.nz/maps?f=q&source=s_q&hl=en&geocode=&q={0},{1},{2}&ie=UTF8&z=14&z=16&output=embed", Replace(currentProject.Address, " ", "+"), Replace(currentProject.City, " ", "+"), Replace(currentProject.Country, " ", "+"))
            End If
        End If
    End Sub
End Class
