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

Partial Class ResetPassword
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Request.QueryString("email") = String.Empty Or Request.QueryString("token") = String.Empty Then
            Response.Redirect("Login.aspx?msg=Your request to reset your password is failed.")
        Else
            If Not Session("AcceptInvitation") Is Nothing Then
                lblPageTitle.Text = "Create Your Password"
            End If
            If Request.QueryString("msg") <> String.Empty Then
                lblErrorMessage.Text = Request.QueryString("msg")
            End If
            txtUser.Text = Request.QueryString("email").Trim
        End If
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        If txtPassword.Text.Trim.Equals(txtConfirmPassword.Text.Trim) Then
            If txtPassword.Text.Trim.Length >= 8 Then
                Dim CurrentUser As New User
                CurrentUser.ActionToken = String.Empty
                CurrentUser.Email = Request.QueryString("email").Trim
                CurrentUser.ActionToken = Request.QueryString("token").Trim
                CurrentUser.Password = txtPassword.Text.Trim
                m_ManagementService.UpdateUserPasswordEmailToken(CurrentUser)
                Response.Redirect("Login.aspx?msg=Your password has been reset successfully.")
            Else
                Response.Redirect(String.Format("ResetPassword.aspx?email={0}&token={1}&msg=Your password needs at least eight characters.", Request.QueryString("email"), Request.QueryString("token")))
            End If
        Else
            Response.Redirect("ResetPassword.aspx?msg=Your password and confirmed password are different.")
        End If
    End Sub
End Class
