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

Partial Class Contacts_View
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        
        If Request.QueryString("msg") <> "" Then
            lblMsg.Text = Request.QueryString("msg")
        End If
        Session("QuickCreate") = Request.Url
        lblTitle.Text = "Contacts"

        'Dim currentProjectOwner As ProjectOwner = m_ManagementService.GetProjectOwnerByContactId(m_LoginUser.UserId)
        ContactsGrid.CompanyId = m_LoginUser.CompanyId
        ContactsGrid.UserType = 0
        If Not Request.QueryString("Typed") Is Nothing Then
            If Request.QueryString("Typed") = UserType.Contractor Then
                ContactsGrid.UserType = Request.QueryString("Typed")
                lbnContractors.Attributes.Add("style", "color:#0481D1;")
                lbnContractors.Font.Bold = True
                lbnSuppliers.ForeColor = Drawing.Color.White
                lbnWorkers.ForeColor = Drawing.Color.White
                'lblTitle.Text = "Contractors"
            ElseIf Request.QueryString("Typed") = UserType.Supplier Then
                ContactsGrid.UserType = Request.QueryString("Typed")
                lbnSuppliers.Attributes.Add("style", "color:#0481D1;")
                lbnSuppliers.Font.Bold = True
                lbnContractors.ForeColor = Drawing.Color.White
                lbnWorkers.ForeColor = Drawing.Color.White
                'lblTitle.Text = "Suppliers"
            ElseIf Request.QueryString("Typed") = UserType.Staff Then
                ContactsGrid.UserType = Request.QueryString("Typed")
                lbnWorkers.Attributes.Add("style", "color:#0481D1;")
                lbnWorkers.Font.Bold = True
                lbnContractors.ForeColor = Drawing.Color.White
                lbnSuppliers.ForeColor = Drawing.Color.White
                'lblTitle.Text = "Staff"
            Else
                lbnContractors.ForeColor = Drawing.Color.White
                lbnSuppliers.ForeColor = Drawing.Color.White
                lbnWorkers.ForeColor = Drawing.Color.White
            End If
        Else
            lbnContractors.ForeColor = Drawing.Color.White
            lbnSuppliers.ForeColor = Drawing.Color.White
            lbnWorkers.ForeColor = Drawing.Color.White
        End If
        ContactsGrid.BranchId = 0
        ContactsGrid.UserId = m_LoginUser.UserId
        ContactsGrid.Keyword = ""
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Login.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
            'If m_LoginUser.Type < 1 Then
            '    Session("CurrentLogin") = Nothing
            '    Response.Redirect("../Login.aspx?msg=Please contact administrator.")
            'End If
        End If
    End Sub

    Protected Sub lbnWorkers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnWorkers.Click
        Response.Redirect("View.aspx?Typed=101")
    End Sub

    Protected Sub lbnContractors_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnContractors.Click
        Response.Redirect("View.aspx?Typed=102")
    End Sub

    Protected Sub lbnSuppliers_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnSuppliers.Click
        Response.Redirect("View.aspx?Typed=103")
    End Sub

    Protected Sub lbnTimesheet_Click(sender As Object, e As EventArgs) Handles lbnTimesheet.Click
        Response.Redirect("Timesheets.aspx")
    End Sub
End Class
