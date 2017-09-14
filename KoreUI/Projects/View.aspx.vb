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

Partial Class Projects_View
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        'Dim m_SystemMessageControl As Interfaces_Rent_Ezi_UserControls_WebUserControl = CType(LoadControl("../UserControls/SystemMessagesControl.ascx"), Interfaces_Rent_Ezi_UserControls_WebUserControl)
        'm_SystemMessageControl = LoadControl("../UserControls/SystemMessagesControl.ascx")
        'm_SystemMessageControl.ForTenant = True
        'Dim user As User = Session("CurrentLogin")
        'AppointmentsGrid.CompanyId = user.CompanyId
        'AppointmentsGrid.BranchId = user.BranchId
        'AppointmentsGrid.UserId = user.UserId
        'AppointmentsGrid.UserType = user.Type

        'If m_LoginUser.BranchId = 0 Then
        '    If Session("BranchID") = Nothing Then
        '        Session("ReturnFromBranches") = "Patients.aspx"
        '        Response.Redirect("Branches.aspx")
        '    End If
        'End If

        'ProjectsGrid.CompanyId = m_LoginUser.CompanyId
        'If m_LoginUser.BranchId = 0 Then
        '    ProjectsGrid.BranchId = Session("BranchId")
        'Else
        '    ProjectsGrid.BranchId = m_LoginUser.BranchId
        'End If
        'ProjectsGrid.UserId = m_LoginUser.UserId
        'ProjectsGrid.UserType = m_LoginUser.Type
        'If Not Session("Keyword") Is Nothing Then
        '    ProjectsGrid.Keyword = Session("Keyword")   'for Search
        'Else
        '    ProjectsGrid.Keyword = ""   'for Search
        'End If
        If Request.QueryString("msg") <> "" Then
            lblMsg.Text = Request.QueryString("msg")
        End If
        Session("QuickCreate") = Request.Url

        Dim currentProjectOwner As ProjectOwner = m_ManagementService.GetProjectOwnerByContactId(m_LoginUser.UserId)

        Dim intSearchType As Integer = 2

        'If Not Session("SearchType") Is Nothing Then
        '    Integer.TryParse(Session("SearchType"), intSearchType)
        'End If

        If Not IsPostBack Then
            If Not Request.QueryString("kw") Is Nothing Then
                Dim dtKeyword As DateTime
                If DateTime.TryParse(Request.QueryString("kw"), dtKeyword) Then
                    Session("Keyword") = dtKeyword.Date.ToString("yyyy-MM-dd")
                Else
                    Session("Keyword") = Request.QueryString("kw").Trim()
                End If
                tbxKeyword.Text = Session("Keyword")
            Else
                Session("Keyword") = String.Empty
                tbxKeyword.Text = Session("Keyword")
            End If
        End If

        If Request.QueryString("Archived") = Nothing Then
            ProjectsGrid.Archived = False
        Else
            ProjectsGrid.Archived = True
            intSearchType = 3
            Session("SearchType") = intSearchType
            'divTitle.Visible = False
        End If

        If intSearchType = 3 Then
            ProjectsGrid.Archived = True
            lblTitle.Text = "Archived Projects"
            ddlSearchType.SelectedValue = "3"
        ElseIf intSearchType = 1 Then
            lblTitle.Text = "All Projects"
            ddlSearchType.SelectedValue = "1"
        ElseIf intSearchType = 2 Then
            lblTitle.Text = "Projects"
            ddlSearchType.SelectedValue = "2"
        End If

        If Request.QueryString("Scoped") = Nothing Then
            ProjectsGrid.Scoped = False
            'divSearchType.Visible = True
        Else
            ProjectsGrid.Scoped = True
            'divSearchType.Visible = False
            lblTitle.Text = "Scopes"
            'divTitle.Visible = False
        End If

        Session("SearchType") = intSearchType

        ProjectsGrid.CompanyId = m_LoginUser.CompanyId
        ProjectsGrid.UserType = m_LoginUser.Type
        ProjectsGrid.BranchId = 0
        'ProjectsGrid.UserId = currentProjectOwner.ProjectOwnerId
        ProjectsGrid.LoginUserId = m_LoginUser.UserId
        ProjectsGrid.UserId = m_LoginUser.UserId
        If Session("Keyword") Is Nothing Then
            ProjectsGrid.Keyword = ""
        Else
            ProjectsGrid.Keyword = Session("Keyword")
        End If
        If Session("SearchType") Is Nothing Then
            ProjectsGrid.SearchType = 2
        Else
            ProjectsGrid.SearchType = Session("SearchType")
        End If

        tbxKeyword.Attributes.Add("onkeypress", "return controlEnter('" + ibnSearch.ClientID + "', event)")
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
            'If m_LoginUser.Type < 1 Then
            '    Session("CurrentLogin") = Nothing
            '    Response.Redirect("../Signin.aspx?msg=Please contact administrator.")
            'End If
        End If
    End Sub

    Protected Sub ibnSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibnSearch.Click
        'Dim dtKeyword As DateTime
        'If DateTime.TryParse(tbxKeyword.Text.Trim(), dtKeyword) Then
        '    Session("Keyword") = dtKeyword.Date.ToString("yyyy-MM-dd")
        'Else
        '    Session("Keyword") = tbxKeyword.Text.Trim()
        'End If

        'If ddlSearchType.SelectedValue <> "" Then
        '    Dim intSearchType As Integer = 2
        '    Integer.TryParse(ddlSearchType.SelectedValue, intSearchType)
        '    Session("SearchType") = intSearchType

        '    If intSearchType = 3 Then
        '        Response.Redirect("View.aspx?Archived=1")
        '    End If
        'End If
        'Response.Redirect(Request.RawUrl)

        If tbxKeyword.Text <> "" Then
            If Not Request.QueryString("Archived") Is Nothing Then
                'Response.Redirect("View.aspx?Archived=1&kw=" & tbxKeyword.Text)
            ElseIf Not Request.QueryString("Scoped") Is Nothing Then
                'Response.Redirect("View.aspx?Scoped=1&kw=" & tbxKeyword.Text)
            Else
                'Response.Redirect("View.aspx?kw=" & tbxKeyword.Text)
            End If
            Response.Redirect("../Search/View.aspx?kw=" & tbxKeyword.Text)
        Else
            Dim returnUrl As String = Request.RawUrl.ToLower
            If returnUrl.IndexOf("&kw=") >= 0 Then
                returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("&kw="))
            End If
            If returnUrl.IndexOf("?kw=") >= 0 Then
                returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("?kw="))
            End If
            Response.Redirect(returnUrl)
        End If
    End Sub

    Protected Sub lbnGroupView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnGroupView.Click
        Session("IsGroupView") = True
        UpdateCookies()
        Response.Redirect(Request.RawUrl)
    End Sub

    Protected Sub lbnGridView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnGridView.Click
        Session("IsGroupView") = False
        UpdateCookies()
        Response.Redirect(Request.RawUrl)
    End Sub

    Protected Sub UpdateCookies()
        Dim blnUpdate As Boolean = True
        For I = 0 To Request.Cookies.Count - 1
            If Request.Cookies.Item(I).Name = m_LoginUser.UserId.ToString Then
                Request.Cookies.Remove(Request.Cookies.Item(I).Name)
                'Request.Cookies.Item(I).Value = True
                'blnUpdate = False
                Exit For
            End If
        Next

        If blnUpdate Then
            Response.Cookies(m_LoginUser.UserId.ToString).Expires = DateAdd(DateInterval.Month, 1, Now)
            Response.Cookies(m_LoginUser.UserId.ToString)("ScopePricing") = Session("ScopePricing")
            Response.Cookies(m_LoginUser.UserId.ToString)("GroupView") = Session("IsGroupView")
        End If
    End Sub
End Class
