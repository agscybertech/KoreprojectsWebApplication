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

Partial Class Contacts_WorksheetService
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Private m_WorksheetServiceID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnAdd.Visible = False
            btnUpdate.Visible = False
            btnDelete.Visible = False
        End If

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_WorksheetServiceID)
        End If

        If Not IsPostBack Then
            Dim dsUnit As DataSet
            dsUnit = m_ScopeService.GetUnits()
            If dsUnit.Tables.Count > 0 Then
                If dsUnit.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsUnit.Tables(0).Rows
                        ddlUnit.Items.Add(New ListItem(dr("Name"), dr("Name")))
                    Next
                End If
            End If

            If m_WorksheetServiceID > 0 Then
                Dim objService As WorksheetService
                objService = m_ScopeService.GetWorksheetServiceByWorksheetServiceId(m_WorksheetServiceID)
                txtWorksheetService.Text = objService.Name
                tbxNote.Text = objService.Description
                If objService.DisplayOrder <> 0 Then
                    txtDisplayOrder.Text = objService.DisplayOrder
                End If
                For index = 0 To ddlUnit.Items.Count - 1
                    If ddlUnit.Items(index).Value = objService.Unit Then
                        ddlUnit.SelectedIndex = index
                        Exit For
                    End If
                Next

                btnAdd.Visible = False
                btnUpdate.Visible = True
                btnClose.Visible = True
                btnDelete.Visible = True

                lblWorksheetService.Text = "UPDATE SERVICE"
            Else
                btnAdd.Visible = True
                btnUpdate.Visible = False
                btnClose.Visible = True
                btnDelete.Visible = False

                lblWorksheetService.Text = "ADD SERVICE"
            End If
        End If

        txtWorksheetService.Focus()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Login.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtWorksheetService.Text <> String.Empty Then
            Dim objService As New WorksheetService
            objService.ProjectOwnerId = m_LoginUser.CompanyId
            objService.Name = txtWorksheetService.Text.Trim
            objService.Description = tbxNote.Text
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objService.DisplayOrder = intDisplayOrder
            objService.Unit = ddlUnit.SelectedItem.Text
            m_ScopeService.CreateWorksheetService(objService)

            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The service is added successfully."))
        Else
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The service is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If m_WorksheetServiceID > 0 And txtWorksheetService.Text <> String.Empty Then
            Dim objService As WorksheetService = m_ScopeService.GetWorksheetServiceByWorksheetServiceId(m_WorksheetServiceID)
            objService.Name = txtWorksheetService.Text.Trim
            objService.Description = tbxNote.Text
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objService.DisplayOrder = intDisplayOrder
            objService.Unit = ddlUnit.SelectedItem.Text
            m_ScopeService.UpdateWorksheetService(objService)

            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The service is updated successfully."))
        Else
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The service is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("OwnerSetting.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_WorksheetServiceID > 0 Then
            m_ScopeService.DeleteWorksheetServiceByWorksheetServiceId(m_WorksheetServiceID)
            Response.Redirect(String.Format("OwnerSetting.aspx?msg=The service is deleted successfully."))
        End If
    End Sub
End Class
