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

        If Request.QueryString("link") <> String.Empty Then
            m_LoginUser = m_ManagementService.Linkin(m_Cryption.Decrypt(Request.QueryString("link"), m_Cryption.cryptionKey))
            If Not m_LoginUser Is Nothing Then
                Session("CurrentLogin") = m_LoginUser
                UpdateCookies(Request.QueryString("link"))
                Response.Redirect("Dashboard/")
            Else
                Response.Redirect("Login.aspx?msg=The link is expired.")
            End If
        ElseIf Request.QueryString("act") <> String.Empty Then
            For I = 0 To Request.Cookies.Count - 1
                If Request.Cookies.Item(I).Name = "KoreMobileApp" Then
                    Response.Cookies("KoreMobileApp").Expires = DateTime.Now
                    Response.Cookies("KoreMobileApp")("Link") = ""
                    'Request.Cookies.Remove(Request.Cookies.Item(I).Name)
                    'Exit For
                End If
            Next
        Else
            'If Session("CurrentLogin") Is Nothing Then
            For I = 0 To Request.Cookies.Count - 1
                If Request.Cookies.Item(I).Name = "KoreMobileApp" Then
                    If Request.Cookies.Item(I)("Link") <> "" Then
                        m_LoginUser = m_ManagementService.Linkin(m_Cryption.Decrypt(Request.Cookies.Item(I)("Link"), m_Cryption.cryptionKey))
                        If Not m_LoginUser Is Nothing Then
                            Session("CurrentLogin") = m_LoginUser
                            Response.Redirect("Dashboard/")
                        End If
                    End If
                End If
            Next
            'End If
        End If

        If Request.QueryString("msg") <> String.Empty Then
            lblErrorMessage.Text = Request.QueryString("msg")
        End If

        'lblErrorMessage.Text = m_Cryption.Encrypt("test12345", m_Cryption.cryptionKey)
    End Sub

    Protected Sub UpdateCookies(ByVal LinkID As String)
        'For I = 0 To Request.Cookies.Count - 1
        '    If Request.Cookies.Item(I).Name = "KoreMobileApp" Then
        '        Request.Cookies.Remove(Request.Cookies.Item(I).Name)
        '        Exit For
        '    End If
        'Next

        Response.Cookies("KoreMobileApp").Expires = DateAdd(DateInterval.Month, 1, Now)
        Response.Cookies("KoreMobileApp")("Link") = LinkID
    End Sub
End Class
