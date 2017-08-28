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

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Dim cs As ClientScriptManager = Page.ClientScript
        m_LoginUser = m_ManagementService.Login(txtUser.Text.Trim, txtPassword.Text.Trim)
        If Not m_LoginUser Is Nothing Then
            If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
                'Dim requestUrl As String = String.Format("https://payments.ebizsecure.co.nz/ppe/pmtservices_gw1/gateway/interface_recurring_paymentinfo.asp?mid={0}", ConfigurationManager.AppSettings("EBIZID"))
                'requestUrl = String.Format("{0}&rid={1}***{2:yyyyMMddHHmmss}&rrid={1}***{2:yyyyMMddHHmmss}", requestUrl, m_LoginUser.UserId, DateTime.Now)
                'Dim doc As XmlDocument = New XmlDocument()
                'doc.Load(requestUrl)
                'm_ManagementService.UpdateUserAccountMonthlyPaymentInfo_ebizsecure(m_LoginUser.UserId, doc)
            End If
            Session("CurrentLogin") = m_LoginUser
            If Session("AcceptInvitation") Is Nothing Then
                cs.RegisterClientScriptBlock(Me.[GetType](), "RedirectScript", "window.parent.location = 'Projects/View.aspx'", True)
            Else
                Session("AcceptInvitation") = Nothing
                cs.RegisterClientScriptBlock(Me.[GetType](), "RedirectScript", "window.parent.location = 'Contacts/ProjectOwnerDetail.aspx'", True)
            End If
            If m_LoginUser.Type = 0 Then
                cs.RegisterClientScriptBlock(Me.[GetType](), "RedirectScript", "window.parent.location = 'Patient/View.aspx'", True)
            ElseIf m_LoginUser.Type = 1 Then
                cs.RegisterClientScriptBlock(Me.[GetType](), "RedirectScript", "window.parent.location = 'Consultant/View.aspx'", True)
            ElseIf m_LoginUser.Type = 2 Then
                'If m_LoginUser.LastBrowsedBranch > 0 Then
                '    Session("BranchID") = m_LoginUser.LastBrowsedBranch
                'End If
                cs.RegisterClientScriptBlock(Me.[GetType](), "RedirectScript", "window.parent.location = 'Admin/Appointments.aspx'", True)
            Else
                Session("CurrentLogin") = Nothing
                Response.Redirect("Login.aspx?msg=Please contact administrator.")

            End If
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

        If Request.QueryString("msg") <> String.Empty Then
            lblErrorMessage.Text = Request.QueryString("msg")
        End If
    End Sub
End Class
