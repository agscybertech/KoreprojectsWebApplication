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
Imports System.Xml

Partial Class Dashboard_Default
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_ProjectOwner As New ProjectOwner
    Private m_UserProfile As New UserProfile
    Private m_Cryption As New Cryption

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ProjectOwner = m_ManagementService.GetProjectOwnerByProjectOwnerId(m_LoginUser.CompanyId)
        m_UserProfile = m_ManagementService.GetUserProfileByUserID(m_LoginUser.UserId)
        If Not IsPostBack Then
            lblCompanyName.Text = m_ProjectOwner.Name
            lblUserName.Text = String.Format("{0} {1}", m_UserProfile.FirstName, m_UserProfile.LastName)
        End If
        tdTimesheet.Attributes.Add("onclick", "GoTimesheet()")
        tdTimesheet.Attributes.Add("style", "cursor:pointer")
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Login.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
            'If m_LoginUser.Type = 0 Then
            '    btnDelete.Visible = False
            'End If
        End If
    End Sub
End Class
