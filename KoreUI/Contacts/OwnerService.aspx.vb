Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities
Imports AjaxControlToolkit

Partial Class Contacts_OwnerService
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption
    'Private UserControls As New List(Of WebControls_WebUserControlRateGrid)

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


        'If TabServices.Tabs.Count > 0 Then
        'For tabIndex = 1 To TabServices.Tabs.Count - 1
        'TabServices.Tabs.RemoveAt(tabIndex)
        'Next
        'End If
        'tabIndex = 1
        ServiceRateGroupGrid.CompanyId = m_LoginUser.CompanyId
        ServiceRateGroupGrid.UserId = m_LoginUser.UserId
        ServiceGroupGrid.CompanyId = 0
        ServiceGroupGrid.UserId = m_LoginUser.UserId
        If m_LoginUser.CompanyId = 0 Then
            trServiceRate.Visible = False
            trAreaItem.Visible = False
        End If
    End Sub
End Class
