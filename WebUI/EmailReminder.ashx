<%@ WebHandler Language="VB" Class="EmailReminder" %>

Imports System
Imports System.Data
Imports System.Web
Imports Warpfusion.A4PP.Services

Public Class EmailReminder : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.Clear()
        context.Response.ContentType = "text/plain"
        context.Response.Write(SendEmailReminder(context).ToString())
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function SendEmailReminder(ByVal context As HttpContext) As Boolean
        Dim result As Boolean = False
        Dim objManagementService As ManagementService = New ManagementService()
        objManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        Dim dsRemindersToEmail As DataSet
        dsRemindersToEmail = objManagementService.GetRemindersToEmail()
        
        Dim userProfileEmail As String = String.Empty
        Dim userEmail As String = String.Empty
        Dim MailMessage As New System.Net.Mail.MailMessage()
        Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
        If dsRemindersToEmail.Tables.Count > 0 Then
            If dsRemindersToEmail.Tables(0).Rows.Count > 0 Then
                For Each drReminderToEmail As DataRow In dsRemindersToEmail.Tables(0).Rows
                    userProfileEmail = String.Format("{0}", drReminderToEmail("UserProfileEmail")).Trim().ToLower()
                    userEmail = String.Format("{0}", drReminderToEmail("UserEmail")).Trim().ToLower()
                    MailMessage = New System.Net.Mail.MailMessage()
        
                    MailMessage.To.Add(New System.Net.Mail.MailAddress(userProfileEmail))
                    If userProfileEmail <> userEmail Then
                        MailMessage.CC.Add(New System.Net.Mail.MailAddress(userEmail))
                    End If
                    MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("SupportEmail"))
                    MailMessage.Subject = String.Format("Reminder: {0} @ {1} ", drReminderToEmail("ReminderTitle"), String.Format("{0:ddd MMM dd h:mmtt}", drReminderToEmail("StartDate")))
                    MailMessage.Body = String.Format("<b>{0}</b><br /><br />When {1}", drReminderToEmail("ReminderContentData"), String.Format("{0:ddd MMM dd h:mmtt}", drReminderToEmail("StartDate")))
                    MailMessage.IsBodyHtml = True
                    Try
                        emailClient.Send(MailMessage)
                        objManagementService.UpdateReminderEmailReminderSentByReminderId(drReminderToEmail("ReminderId"))
                        result = True
                    Catch ex As Exception
                        Throw
                    End Try
                Next
            End If
        End If
        
        Return result
    End Function
End Class