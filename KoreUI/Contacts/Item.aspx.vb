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

Partial Class Contacts_Item
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Private m_ItemID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnAdd.Visible = False
            btnUpdate.Visible = False
            btnDelete.Visible = False
        End If

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ItemID)
        End If

        If Not IsPostBack Then
            If m_ItemID > 0 Then
                Dim objItem As Item
                objItem = m_ScopeService.GetItemByItemId(m_ItemID)
                txtItem.Text = objItem.Name
                If objItem.DisplayOrder <> 0 Then
                    txtDisplayOrder.Text = objItem.DisplayOrder
                End If

                btnAdd.Visible = False
                btnUpdate.Visible = True
                btnClose.Visible = True
                btnDelete.Visible = True

                lblItem.Text = "UPDATE ITEM"
            Else
                btnAdd.Visible = True
                btnUpdate.Visible = False
                btnClose.Visible = True
                btnDelete.Visible = False

                lblItem.Text = "ADD ITEM"
            End If
        End If

        txtItem.Focus()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtItem.Text <> String.Empty Then
            Dim objItem As New Item
            objItem.ProjectOwnerId = m_LoginUser.CompanyId
            objItem.Name = txtItem.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objItem.DisplayOrder = intDisplayOrder
            m_ScopeService.CreateItem(objItem)

            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The item is added successfully."))
        Else
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The item is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If m_ItemID > 0 And txtItem.Text <> String.Empty Then
            Dim objItem As Item = m_ScopeService.GetItemByItemId(m_ItemID)
            objItem.Name = txtItem.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objItem.DisplayOrder = intDisplayOrder
            m_ScopeService.UpdateItem(objItem)

            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The item is updated successfully."))
        Else
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The item is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("OwnerSetting.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_ItemID > 0 Then
            m_ScopeService.DeleteItemByItemId(m_ItemID)
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The item is deleted successfully."))
        End If
    End Sub
End Class
