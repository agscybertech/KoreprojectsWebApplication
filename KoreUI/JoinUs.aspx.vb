Imports System.Data
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class JoinUs
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_Cryption As New Cryption

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If m_Cryption.Decrypt(Request.QueryString("act"), m_Cryption.cryptionKey) = "Join" And Request.QueryString("email") <> String.Empty And Request.QueryString("token") <> String.Empty Then            
            Dim intAcceptInvitation As Integer
            intAcceptInvitation = m_ManagementService.UpdateUserRelationshipTypeStatusToken(Warpfusion.A4PP.Services.UserType.Contractor, Warpfusion.A4PP.Services.UserRelationshipStatus.Active, Request.QueryString("token"))
            If intAcceptInvitation > 0 Then
                Session("AcceptInvitation") = "1"
                Dim dsUser As DataSet
                dsUser = m_ManagementService.GetUserByEmail(Request.QueryString("email"))
                If dsUser.Tables.Count > 0 Then
                    If dsUser.Tables(0).Rows.Count > 0 Then
                        If IsDBNull(dsUser.Tables(0).Rows(0)("Password")) Then
                            Response.Redirect(String.Format("ResetPassword.aspx?email={0}&token={1}", Request.QueryString("email"), Request.QueryString("token")))
                        Else
                            Response.Redirect("Login.aspx")
                        End If
                    End If
                End If
                Response.Redirect("Login.aspx?msg=Your request to join Kore Projects is failed.")
            Else
                Response.Redirect("Login.aspx?msg=Your link is not valid or expired.")
            End If
        End If
        Response.Redirect("Login.aspx")
    End Sub
End Class
