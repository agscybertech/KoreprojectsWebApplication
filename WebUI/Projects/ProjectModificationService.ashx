<%@ WebHandler Language="VB" Class="ProjectModificationService" %>

Imports System
Imports System.Data
Imports System.Web
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Public Class ProjectModificationService : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        context.Response.Clear()
        context.Response.ContentType = "text/plain"
        context.Response.Write(ProjectModificationStatus(context).ToString())
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

    Private Function ProjectModificationStatus(ByVal context As HttpContext) As Boolean
        Dim objCryption As Cryption = New Cryption()
        Dim objManagementService As ManagementService = New ManagementService()
        objManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim UserId As Long = 0
        Dim objProject As Project
        Dim result As Boolean = False
        Dim ProjectName As String = String.Empty
        Dim ProjectId As Long = 0
        Dim dtStartDate As DateTime
        Dim dtFinishDate As DateTime
        Dim dtScopeDate As DateTime
        
        If Long.TryParse(objCryption.Decrypt(context.Request.QueryString("mid"), objCryption.cryptionKey), UserId) Then
            If Not context.Request.QueryString("id") Is Nothing Then
                'Update Project
                If Long.TryParse(objCryption.Decrypt(context.Request.QueryString("id"), objCryption.cryptionKey), ProjectId) Then
                    objProject = objManagementService.GetProjectByProjectId(ProjectId)
                    Dim dsUserProjectStatus As DataSet
                    dsUserProjectStatus = objManagementService.GetUserProjectStatusValueByProjectIdUserId(ProjectId, UserId)
                    If dsUserProjectStatus.Tables.Count > 0 Then
                        If dsUserProjectStatus.Tables(0).Rows.Count > 0 Then
                            If dsUserProjectStatus.Tables(0).Rows(0)("UserProjectStatusValue") = -1000 Then
                                If context.Request.QueryString("end") Is Nothing Then
                                    If DateTime.TryParse(context.Request.QueryString("start"), dtScopeDate) Then
                                        objProject.ScopeDate = dtScopeDate
                                    End If
                                Else
                                    If DateTime.TryParse(context.Request.QueryString("start"), dtStartDate) Then
                                        objProject.StartDate = dtStartDate
                                    End If
                                    If DateTime.TryParse(context.Request.QueryString("end"), dtFinishDate) Then
                                        objProject.FinishDate = dtFinishDate
                                    End If                                    
                                End If
                            Else
                                If DateTime.TryParse(context.Request.QueryString("start"), dtStartDate) Then
                                    objProject.StartDate = dtStartDate
                                End If
                                If DateTime.TryParse(context.Request.QueryString("end"), dtFinishDate) Then
                                    objProject.FinishDate = dtFinishDate
                                End If
                            End If
                            objManagementService.UpdateProject(objProject)
                            result = True
                        End If
                    End If
                End If
            End If
        End If
        
        Return result
    End Function
End Class