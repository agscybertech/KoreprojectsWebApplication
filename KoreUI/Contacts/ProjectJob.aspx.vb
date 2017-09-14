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

Partial Class Contacts_ProjectJob
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Private m_UserProjectJobSettingId As Long = 0
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
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_UserProjectJobSettingId)
        End If

        If Request.QueryString("ProjectID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ProjectID"), m_Cryption.cryptionKey), m_ProjectId)
        End If

        If Not IsPostBack Then
            If m_UserProjectJobSettingId > 0 Then
                Dim objProjectJobSetting As UserProjectJobSetting
                objProjectJobSetting = m_ManagementService.GetUserProjectJobSettingByUserProjectJobSettingId(m_UserProjectJobSettingId)
                txtName.Text = objProjectJobSetting.Name
                If objProjectJobSetting.DisplayOrder <> 0 Then
                    txtDisplayOrder.Text = objProjectJobSetting.DisplayOrder
                End If

                btnAdd.Visible = False
                btnUpdate.Visible = True
                btnClose.Visible = True
                btnDelete.Visible = True

                lblUserProjectJobSetting.Text = "UPDATE PROJECT JOB"
            Else
                btnAdd.Visible = True
                btnUpdate.Visible = False
                btnClose.Visible = True
                btnDelete.Visible = False

                lblUserProjectJobSetting.Text = "ADD PROJECT JOB"
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
            Dim objUserProjectJobSetting As New UserProjectJobSetting
            objUserProjectJobSetting.UserId = m_LoginUser.CompanyId
            objUserProjectJobSetting.ProjectId = m_ProjectId
            objUserProjectJobSetting.Name = txtName.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objUserProjectJobSetting.DisplayOrder = intDisplayOrder
            m_ManagementService.CreateUserProjectJobSetting(objUserProjectJobSetting)

            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The project job is added successfully."))
        Else
            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The project job is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If m_UserProjectJobSettingId > 0 And txtName.Text <> String.Empty Then
            Dim objUserProjectJobSetting As UserProjectJobSetting = m_ManagementService.GetUserProjectJobSettingByUserProjectJobSettingId(m_UserProjectJobSettingId)
            objUserProjectJobSetting.Name = txtName.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objUserProjectJobSetting.DisplayOrder = intDisplayOrder
            m_ManagementService.UpdateUserProjectJobSetting(objUserProjectJobSetting)

            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The project job is updated successfully."))
        Else
            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The project job is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("ProjectSetting.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_UserProjectJobSettingId > 0 Then
            m_ManagementService.DeleteUserProjectJobSettingByUserProjectJobSettingId(m_UserProjectJobSettingId)
            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The project job is deleted successfully."))
        End If
    End Sub
End Class
