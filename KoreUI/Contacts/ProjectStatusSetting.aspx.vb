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
Imports System.Data
Imports System.Data.SqlClient

Partial Class Contacts_ProjectStatusSetting
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Private m_UserProjectStatusSettingId As Long = 0
    Private m_ProjectId As Long = 0
    Public m_Cryption As New Cryption()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnAdd.Visible = False
            btnUpdate.Visible = False
            btnDelete.Visible = False
        End If

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_UserProjectStatusSettingId)
        End If

        If Request.QueryString("ProjectID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ProjectID"), m_Cryption.cryptionKey), m_ProjectId)
        End If

        If Not IsPostBack Then
            If m_UserProjectStatusSettingId > 0 Then
                Dim objProjectStatusSetting As UserProjectStatusSetting
                objProjectStatusSetting = m_ManagementService.GetUserProjectStatusSettingByUserProjectStatusSettingId(m_UserProjectStatusSettingId)
                txtName.Text = objProjectStatusSetting.Name
                If objProjectStatusSetting.DisplayOrder <> 0 Then
                    txtDisplayOrder.Text = objProjectStatusSetting.DisplayOrder
                End If

                btnAdd.Visible = False
                btnUpdate.Visible = True
                btnClose.Visible = True
                btnDelete.Visible = True

                lblUserProjectStatusSetting.Text = "UPDATE PROJECT STATUS"
            Else
                btnAdd.Visible = True
                btnUpdate.Visible = False
                btnClose.Visible = True
                btnDelete.Visible = False

                lblUserProjectStatusSetting.Text = "ADD PROJECT STATUS"
            End If
        End If

        txtName.Focus()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtName.Text <> String.Empty Then
            Dim objUserProjectStatusSetting As New UserProjectStatusSetting
            objUserProjectStatusSetting.UserId = m_LoginUser.UserId
            objUserProjectStatusSetting.ProjectId = m_ProjectId
            objUserProjectStatusSetting.Name = txtName.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objUserProjectStatusSetting.DisplayOrder = intDisplayOrder
            m_ManagementService.CreateUserProjectStatusSetting(objUserProjectStatusSetting)

            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The project status is added successfully."))
        Else
            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The project status is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If m_UserProjectStatusSettingId > 0 And txtName.Text <> String.Empty Then
            Dim objUserProjectStatusSetting As UserProjectStatusSetting = m_ManagementService.GetUserProjectStatusSettingByUserProjectStatusSettingId(m_UserProjectStatusSettingId)
            objUserProjectStatusSetting.Name = txtName.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objUserProjectStatusSetting.DisplayOrder = intDisplayOrder
            m_ManagementService.UpdateUserProjectStatusSetting(objUserProjectStatusSetting)

            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The project status is updated successfully."))
        Else
            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The project status is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("ProjectSetting.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_UserProjectStatusSettingId > 0 Then
            m_ManagementService.DeleteUserProjectStatusSettingByUserProjectStatusSettingId(m_UserProjectStatusSettingId)
            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The project status is deleted successfully."))
        End If
    End Sub
End Class