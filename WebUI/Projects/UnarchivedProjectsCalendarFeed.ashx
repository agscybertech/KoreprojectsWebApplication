<%@ WebHandler Language="VB" Class="UnarchivedProjectsCalendarFeed" %>

Imports System
Imports System.Data
Imports System.Web
Imports System.Web.Script.Serialization
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Public Class UnarchivedProjectsCalendarFeed : Implements IHttpHandler
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.Clear()
        context.Response.ContentType = "text/plain"
        context.Response.Write(GetUnarchivedProjectsCalendarFeed(context))
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function GetUnarchivedProjectsCalendarFeed(ByVal context As HttpContext) As String
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
            
                    Dim dsUnarchivedProjects As DataSet
                    dsUnarchivedProjects = objManagementService.GetUnarchivedProjectsByUserId(UserId)
            
                    If dsUnarchivedProjects.Tables.Count > 0 Then
                        If dsUnarchivedProjects.Tables(0).Rows.Count > 0 Then                            
                            Dim row As DataRow
                            Dim dsUserProjectStatusValue As DataSet
                            Dim strUserProjectStatusName As String
                            Dim arrCalendarEvents(dsUnarchivedProjects.Tables(0).Rows.Count) As Dictionary(Of String, String)
                            Dim dCalendarEvent As Dictionary(Of String, String)
                            Dim strId As String = String.Empty                           
                            Dim strTitle As String = String.Empty
                            Dim strStart As String = String.Empty
                            Dim strEnd As String = String.Empty
                            Dim strUrl As String = String.Empty
                            
                            For index As Integer = 0 To dsUnarchivedProjects.Tables(0).Rows.Count - 1
                                row = dsUnarchivedProjects.Tables(0).Rows(index)
                                dsUserProjectStatusValue = New DataSet()
                                dsUserProjectStatusValue = objManagementService.GetUserProjectStatusValueByProjectIdUserId(row("ProjectId"), UserId)
                                strUserProjectStatusName = String.Empty
                                If dsUserProjectStatusValue.Tables.Count > 0 Then
                                    If dsUserProjectStatusValue.Tables(0).Rows.Count > 0 Then
                                        strUserProjectStatusName = String.Format("{0}", dsUserProjectStatusValue.Tables(0).Rows(0)("Name")).Trim()
                                    End If
                                End If
                                dCalendarEvent = New Dictionary(Of String, String)()

                                strId = objCryption.Encrypt(String.Format("{0}", row("ProjectId")), objCryption.cryptionKey)
                                dCalendarEvent.Add("id", strId)

                                strTitle = String.Format("{0}", row("Name"))
                                If strTitle.Trim() = String.Empty Then
                                    strTitle = String.Format("{0}", row("Address"))
                                End If
                                If strUserProjectStatusName <> String.Empty Then
                                    strTitle = String.Format("{0}: {1}", strUserProjectStatusName.ToUpper(), strTitle)
                                End If
                                dCalendarEvent.Add("title", strTitle)
                                                
                                If IsDBNull(row("UserProjectStatusValue")) Then
                                    strStart = String.Format("{0:yyyy-MM-dd}", row("ScopeDate"))
                                    dCalendarEvent.Add("start", strStart)
                                Else
                                    If row("UserProjectStatusValue") = -1000 Then
                                        ' Project with Scope Status
                                        strStart = String.Format("{0:yyyy-MM-dd}", row("ScopeDate"))
                                        dCalendarEvent.Add("start", strStart)
                                    Else
                                        strStart = String.Format("{0:yyyy-MM-dd}", row("StartDate"))
                                        dCalendarEvent.Add("start", strStart)
                                        strEnd = String.Format("{0:yyyy-MM-dd}", row("FinishDate"))
                                        If strEnd.Trim() <> String.Empty Then
                                            dCalendarEvent.Add("end", strEnd)
                                        End If
                                    End If
                                End If
                        
                                'strUrl = String.Format("/Projects/Detail.aspx?id={0}", strId)
                                strUrl = String.Format("http://{0}/Projects/Detail.aspx?id={1}", context.Request.Url.Host, strId)
                                dCalendarEvent.Add("url", strUrl)
                                
                                If IsDBNull(row("UserProjectStatusValue")) Then
                                    dCalendarEvent.Add("color", "#E2FFF0")
                                    dCalendarEvent.Add("textColor", "#000000")
                                Else
                                    If row("UserProjectStatusValue") = -1000 Then
                                        'dCalendarEvent.Add("className", "EventColorScope")
                                        'dCalendarEvent.Add("backgroundColor", "#43965E")
                                        dCalendarEvent.Add("color", "#E2FFF0")
                                        dCalendarEvent.Add("textColor", "#000000")
                                    Else
                                        'dCalendarEvent.Add("className", "EventColorProject")
                                        dCalendarEvent.Add("color", "#D7F2FF")
                                        dCalendarEvent.Add("textColor", "#000000")
                                    End If
                                End If
                        
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