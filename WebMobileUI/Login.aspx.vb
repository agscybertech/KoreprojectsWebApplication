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
Imports System.Data.SqlClient
Imports System.Xml

Partial Class Login
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As User
    Private m_Cryption As New Cryption

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        m_LoginUser = m_ManagementService.Login(txtUser.Text.Trim, txtPassword.Text.Trim)
        If Not m_LoginUser Is Nothing Then
            Session("CurrentLogin") = m_LoginUser
            Response.Redirect("Dashboard/")
        Else
            Response.Redirect("Login.aspx?msg=The email or password you entered is incorrect.")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Response.Write("test=" & Request.UserHostAddress())
        'Session("CompanyId") = 1

        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        Dim BrowserUpdateRequired As Decimal = 0
        Dim BrowserVersion As Decimal
        Decimal.TryParse(Request.Browser.Version, BrowserVersion)

        Select Case (Request.Browser.Browser)
            Case "Firefox"
                If (BrowserVersion < 3) Then
                    BrowserUpdateRequired = 3
                End If
                Exit Select
            Case "IE"
                If (BrowserVersion < 6) Then
                    BrowserUpdateRequired = 6
                End If
                Exit Select
            Case "Safari"
                If (BrowserVersion < 4) Then
                    BrowserUpdateRequired = 4
                End If
                Exit Select
            Case "Chrome"
                If (BrowserVersion < 3) Then
                    BrowserUpdateRequired = 3
                End If
                Exit Select
        End Select

        If (BrowserUpdateRequired > 0) Then
            Response.Redirect("SystemRequirements.aspx")
        End If

        If Request.QueryString("Link") <> String.Empty Then
            m_LoginUser = m_ManagementService.Linkin(m_Cryption.Decrypt(Request.QueryString("Link"), m_Cryption.cryptionKey))
            If Not m_LoginUser Is Nothing Then
                Session("CurrentLogin") = m_LoginUser
                Response.Redirect("Dashboard/")
            Else
                Response.Redirect("Login.aspx?msg=The link is expired.")
            End If
        End If

        If Request.QueryString("msg") <> String.Empty Then
            lblErrorMessage.Text = Request.QueryString("msg")
        End If
    End Sub
End Class
