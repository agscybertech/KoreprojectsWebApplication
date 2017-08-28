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
Partial Class Projects_Hazard
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        Dim strContentHtml As String = String.Empty
        Dim dsProject As DataSet = New DataSet()
        dsProject = m_ManagementService.GetProjectInfoByProjectId(CInt(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey)))
        If dsProject.Tables.Count > 0 Then
            If dsProject.Tables(0).Rows.Count > 0 Then
                strContentHtml = String.Format("{0}", dsProject.Tables(0).Rows(0)("Hazard"))
            End If
        End If
        divHazard.InnerHtml = strContentHtml
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

End Class
