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

Partial Class Contacts_Area
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Private m_AreaID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnAdd.Visible = False
            btnUpdate.Visible = False
            btnDelete.Visible = False
        End If

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_AreaID)
        End If

        If Not IsPostBack Then
            If m_AreaID > 0 Then
                Dim objArea As Area
                objArea = m_ScopeService.GetAreaByAreaId(m_AreaID)
                txtArea.Text = objArea.Name
                If objArea.DisplayOrder <> 0 Then
                    txtDisplayOrder.Text = objArea.DisplayOrder
                End If

                btnAdd.Visible = False
                btnUpdate.Visible = True
                btnClose.Visible = True
                btnDelete.Visible = True

                lblArea.Text = "UPDATE AREA"
            Else
                btnAdd.Visible = True
                btnUpdate.Visible = False
                btnClose.Visible = True
                btnDelete.Visible = False

                lblArea.Text = "ADD AREA"
            End If
        End If

        txtArea.Focus()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtArea.Text <> String.Empty Then
            Dim objArea As New Area
            objArea.ProjectOwnerId = m_LoginUser.CompanyId
            objArea.Name = txtArea.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objArea.DisplayOrder = intDisplayOrder
            m_ScopeService.CreateArea(objArea)

            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The area is added successfully."))
        Else
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The area is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If m_AreaID > 0 And txtArea.Text <> String.Empty Then
            Dim objArea As Area = m_ScopeService.GetAreaByAreaId(m_AreaID)
            objArea.Name = txtArea.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objArea.DisplayOrder = intDisplayOrder
            m_ScopeService.UpdateArea(objArea)

            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The area is updated successfully."))
        Else
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The area is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("OwnerSetting.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_AreaID > 0 Then
            m_ScopeService.DeleteAreaByAreaId(m_AreaID)
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The area is deleted successfully."))
        End If
    End Sub
End Class
