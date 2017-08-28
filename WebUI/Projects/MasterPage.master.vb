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

Partial Class Projects_MasterPage
    Inherits System.Web.UI.MasterPage
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_UserProfile As New UserProfile
    Private m_ProjectOwner As New ProjectOwner
    Public m_Cryption As New Cryption()

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub lbnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnLogin.Click
        Session("CurrentLogin") = Nothing
        Session("Keyword") = Nothing
        Session.Clear()
        Response.Redirect("../Signin.aspx?msg=You've been successfully logged out.")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        If Session("ScopePricing") Is Nothing Or Session("IsGroupView") Is Nothing Then
            Dim I As Integer
            Dim blnUpdate As Boolean = True
            Session("ScopePricing") = False
            Session("IsGroupView") = True
            For I = 0 To Request.Cookies.Count - 1
                If Request.Cookies.Item(I).Name = m_LoginUser.UserId.ToString Then
                    'Session("ScopePricing") = Request.Cookies.Item(I).Value
                    If Request.Cookies.Item(I)("ScopePricing") <> "" Then
                        Session("ScopePricing") = Request.Cookies.Item(I)("ScopePricing")
                    End If
                    If Request.Cookies.Item(I)("GroupView") <> "" Then
                        Session("IsGroupView") = Request.Cookies.Item(I)("GroupView")
                    End If
                    blnUpdate = False
                    Exit For
                End If
            Next

            If blnUpdate Then
                Session("IsGroupView") = False
                Dim m_GroupProjects As DataSet = New DataSet()
                m_GroupProjects = m_ManagementService.GetGroupProjectsNotUserArchivedByUserId(m_LoginUser.UserId)
                If m_GroupProjects.Tables.Count > 0 Then
                    If m_GroupProjects.Tables(0).Rows.Count > 0 Then
                        Session("IsGroupView") = True
                    End If
                End If

                Response.Cookies(m_LoginUser.UserId.ToString).Expires = DateAdd(DateInterval.Month, 1, Now)
                Response.Cookies(m_LoginUser.UserId.ToString)("ScopePricing") = Session("ScopePricing")
                Response.Cookies(m_LoginUser.UserId.ToString)("GroupView") = Session("IsGroupView")
            End If
        End If

        m_ProjectOwner = m_ManagementService.GetProjectOwnerByProjectOwnerId(m_LoginUser.CompanyId)
        If m_ProjectOwner.Logo <> String.Empty Then
            imgLogo.ImageUrl = String.Format("..\images\{0}\{1}", m_ProjectOwner.Identifier, m_ProjectOwner.Logo)
        Else
            imgLogo.ImageUrl = "../images/logo-sm.jpg"
        End If
    End Sub

    Public Function GetHelpURL() As String
        Dim result As String = String.Empty
        result = ConfigurationManager.AppSettings("HelpURL")
        Return result
    End Function

    Protected Sub lbnLogin1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnLogin.Click
        Session("CurrentLogin") = Nothing
        Session("Keyword") = Nothing
        Session.Clear()
        Response.Redirect("../Signin.aspx?msg=You've been successfully logged out.")
    End Sub

    'Protected Sub lbnLogin1_Click1(sender As Object, e As EventArgs)
    '    Session("CurrentLogin") = Nothing
    '    Session("Keyword") = Nothing
    '    Session.Clear()
    '    Response.Redirect("../Signin.aspx?msg=You've been successfully logged out.")
    'End Sub
End Class

