Imports Microsoft.VisualBasic
Imports System.Diagnostics
Imports System.Web
Imports System.Web.SessionState
Imports Warpfusion.Shared.Utilities

Public Class GlobalApplicationClass
    Inherits System.Web.HttpApplication
    'Private m_Log As New LogService()
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
        Dim ipAddress As String = String.Empty
        ipAddress = HttpContext.Current.Request.UserHostAddress
        Dim pageUrl As String = String.Empty
        pageUrl = HttpContext.Current.Request.RawUrl
        Dim userAgent As String = String.Empty
        userAgent = HttpContext.Current.Request.UserAgent
        Dim ErrorDescription As String = Server.GetLastError.ToString

        'm_Log.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        'm_Log.Log(LogCategory.ApplicationError, ipAddress, String.Format("Application Error Detected"), pageUrl, userAgent)

        Dim tosupport_address As String = System.Configuration.ConfigurationManager.AppSettings("SupportEmail")
        Dim toadmin_address As String = System.Configuration.ConfigurationManager.AppSettings("AdminEmail")
        Dim MailMessage As New System.Net.Mail.MailMessage()
        Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
        MailMessage.To.Add(New System.Net.Mail.MailAddress(tosupport_address))
        'MailMessage.CC.Add(New System.Net.Mail.MailAddress(toadmin_address))
        MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("ErrorEmailFromServer"))
        MailMessage.Subject = String.Format("Error in {0}", Request.Url.Host)
        MailMessage.Body = ErrorDescription
        MailMessage.IsBodyHtml = False
        Try
            emailClient.Send(MailMessage)
            'm_Log.Log(LogCategory.ApplicationError, ipAddress, String.Format("Error Emailed"), pageUrl, userAgent)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        'Response.Redirect("http://" + Request.Url.Host)        
        'Response.Redirect("step1.aspx")
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub
End Class
