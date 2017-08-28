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
Imports System.Web.Mail

Partial Class ForgotPassword
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_VoucherCodeFunction As New VoucherCodeFunctions

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        If Not Session("CurrentLogin") Is Nothing Then
            Response.Redirect("ForgotPassword.aspx?msg=You can't reset your password until you log off.")
        End If

        If txtUser.Text.Trim <> String.Empty And txtUser.Text.Trim.IndexOf("@") > 0 And txtUser.Text.Trim.IndexOf("@") < (txtUser.Text.Trim.Length - 2) Then
            Dim voucherCode As String = m_VoucherCodeFunction.GenerateVoucherCodeGuid(16)
            m_ManagementService.UpdateUserActionToken(txtUser.Text.Trim, voucherCode)

            Dim MailMessage As New System.Net.Mail.MailMessage()
            Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
            MailMessage.To.Add(New System.Net.Mail.MailAddress(txtUser.Text.Trim))
            MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("AdminEmail"))
            MailMessage.Subject = "Reset Password"
            'MailMessage.Body = String.Format("Hi there,<br><br>We received a request to reset your A4PP password. If you want to reset your password, click the link below:<br><br>{0}ResetPassword.aspx?email={1}&token={2}<br><br>This link takes you to a page where you can choose a new password.<br><br>Please ignore this message if you don't want to change your password. Your password will not be reset.<br><br>The A4PP Team", ConfigurationManager.AppSettings("ProjectURL"), txtUser.Text.Trim, voucherCode)
            MailMessage.Body = String.Format("Hi there,<br><br>We received a request to reset your Kore Projects password. If you want to reset your password, click the link below:<br><br>http://{0}/ResetPassword.aspx?email={1}&token={2}<br><br>This link takes you to a page where you can choose a new password.<br><br>Please ignore this message if you don't want to change your password. Your password will not be reset.<br><br>The Kore Projects Team", Request.Url.Authority, txtUser.Text.Trim, voucherCode)
            MailMessage.IsBodyHtml = True
            emailClient.Send(MailMessage)
            Response.Redirect("PasswordReminderDone.aspx")
        Else
            Response.Redirect("ForgotPassword.aspx?msg=The email you entered is incorrect.")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Request.QueryString("msg") <> String.Empty Then
            lblErrorMessage.Text = Request.QueryString("msg")
        End If
    End Sub
End Class
