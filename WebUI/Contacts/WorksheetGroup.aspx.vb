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

Partial Class Contacts_WorksheetGroup
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Private m_WorksheetGroupID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnAdd.Visible = False
            btnUpdate.Visible = False
            btnDelete.Visible = False
        End If

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_WorksheetGroupID)
        End If

        If Not IsPostBack Then
            If m_WorksheetGroupID > 0 Then
                Dim objWorksheetGroup As WorksheetGroup
                objWorksheetGroup = m_ScopeService.GetWorksheetGroupByWorksheetGroupId(m_WorksheetGroupID)
                txtWorksheetGroup.Text = objWorksheetGroup.Name
                If objWorksheetGroup.DisplayOrder <> 0 Then
                    txtDisplayOrder.Text = objWorksheetGroup.DisplayOrder
                End If

                btnAdd.Visible = False
                btnUpdate.Visible = True
                btnClose.Visible = True
                btnDelete.Visible = True

                lblWorksheetGroup.Text = "UPDATE GROUP"
            Else
                btnAdd.Visible = True
                btnUpdate.Visible = False
                btnClose.Visible = True
                btnDelete.Visible = False

                lblWorksheetGroup.Text = "ADD GROUP"
            End If
        End If

        txtWorksheetGroup.Focus()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Login.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtWorksheetGroup.Text <> String.Empty Then
            Dim objWorksheetGroup As New WorksheetGroup
            objWorksheetGroup.ProjectOwnerId = m_LoginUser.CompanyId
            objWorksheetGroup.Name = txtWorksheetGroup.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objWorksheetGroup.DisplayOrder = intDisplayOrder
            m_ScopeService.CreateWorksheetGroup(objWorksheetGroup)

            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The group is added successfully."))
        Else
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The group is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If m_WorksheetGroupID > 0 And txtWorksheetGroup.Text <> String.Empty Then
            Dim objWorksheetGroup As WorksheetGroup = m_ScopeService.GetWorksheetGroupByWorksheetGroupId(m_WorksheetGroupID)
            objWorksheetGroup.Name = txtWorksheetGroup.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objWorksheetGroup.DisplayOrder = intDisplayOrder
            m_ScopeService.UpdateWorksheetGroup(objWorksheetGroup)

            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The group is updated successfully."))
        Else
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The group is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("OwnerSetting.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_WorksheetGroupID > 0 Then
            m_ScopeService.DeleteWorksheetGroupByWorksheetGroupId(m_WorksheetGroupID)
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The group is deleted successfully."))
        End If
    End Sub
End Class
