<%@ WebHandler Language="VB" Class="RemindersCalendarFeed" %>

Imports System
Imports System.Data
Imports System.Web
Imports System.Web.Script.Serialization
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Public Class RemindersCalendarFeed : Implements IHttpHandler
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.Clear()
        context.Response.ContentType = "text/plain"
        context.Response.Write(GetRemindersCalendarFeed(context))
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function GetRemindersCalendarFeed(ByVal context As HttpContext) As String
        Dim result As String = String.Empty
        
        If Not context.Request.QueryString("id") Is Nothing Then
            Dim objCryption As New Cryption()
            Dim strUserId As String = String.Empty
            strUserId = objCryption.Decrypt(context.Request.QueryString("id"), objCryption.cryptionKey)
            'strUserId = "1"
            Dim UserId As Long
            If Long.TryParse(strUserId, UserId) Then
                If UserId > 0 Then
                    Dim objManagementService As New ManagementService
                    objManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
                    
                    Dim objProjectOwner As ProjectOwner = objManagementService.GetProjectOwnerByContactId(UserId)
                    Dim dsReminders As DataSet
                    dsReminders = objManagementService.GetRemindersValidforCalendarFeedByProjectOwnerId(objProjectOwner.ProjectOwnerId)
            
                    If dsReminders.Tables.Count > 0 Then
                        If dsReminders.Tables(0).Rows.Count > 0 Then
                            Dim row As DataRow
                            Dim arrCalendarEvents(dsReminders.Tables(0).Rows.Count) As Dictionary(Of String, String)
                            Dim dCalendarEvent As Dictionary(Of String, String)
                            Dim strId As String = String.Empty
                            Dim strTitle As String = String.Empty
                            Dim strStart As String = String.Empty
                            Dim strEnd As String = String.Empty
                            Dim strUrl As String = String.Empty
                            
                            For index As Integer = 0 To dsReminders.Tables(0).Rows.Count - 1
                                row = dsReminders.Tables(0).Rows(index)
                                dCalendarEvent = New Dictionary(Of String, String)()

                                strId = objCryption.Encrypt(String.Format("{0}", row("ReminderId")), objCryption.cryptionKey)
                                dCalendarEvent.Add("id", strId)

                                strTitle = String.Format("{0}", row("ReminderTitle"))
                                If strTitle.Trim() = String.Empty Then
                                    strTitle = String.Format("{0:d MMM h:mm tt}", row("StartDate"))
                                End If
                                dCalendarEvent.Add("title", strTitle)
                                                
                                strStart = String.Format("{0:yyyy-MM-dd}", row("StartDate"))
                                dCalendarEvent.Add("start", strStart)
                                strEnd = String.Format("{0:yyyy-MM-dd}", row("EndDate"))
                                If strEnd.Trim() <> String.Empty Then
                                    dCalendarEvent.Add("end", strEnd)
                                End If
                       
                                'strUrl = String.Format("/Projects/Detail.aspx?id={0}", strId)
                                strUrl = String.Format("http://{0}/Projects/EditReminder.aspx?id={1}", context.Request.Url.Host, strId)
                                dCalendarEvent.Add("url", strUrl)
                                
                                dCalendarEvent.Add("color", "#FFD8D8")
                                dCalendarEvent.Add("textColor", "#000000")
                                
                                If strStart.Trim() <> String.Empty Then
                                    arrCalendarEvents(index) = dCalendarEvent
                                End If
                            Next
                    
                            result = New JavaScriptSerializer().Serialize(arrCalendarEvents)
                            result = result.Replace(",null", String.Empty)
                            result = result.Replace("null,", String.Empty)
                        End If
                    End If
                End If
            End If
        End If
        
        Return result
    End Function
End Class