Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class Contacts_ProjectSetting
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
            'If Not m_LoginUser.Type = 2 Then
            '    Session("CurrentLogin") = Nothing
            '    Response.Redirect("../Signin.aspx?msg=Please login as administrator!")
            'End If            
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Request.QueryString("msg") <> "" Then
            lblmsg.Text = Request.QueryString("msg")
        End If

        lblTitle.Text = "Settings"
        ProjectGroupGrid.CompanyId = m_LoginUser.CompanyId
        ProjectStatusGrid.UserId = m_LoginUser.UserId
        ProjectJobGrid.UserId = m_LoginUser.CompanyId
    End Sub
End Class
