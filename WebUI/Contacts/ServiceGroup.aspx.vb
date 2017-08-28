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

Partial Class Contacts_ServiceGroup
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Private m_ServiceGroupID As Integer = 0
    Public m_ProjectOwnerTabIndex As Integer = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnAdd.Visible = False
            btnUpdate.Visible = False
            btnDelete.Visible = False
        End If

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ServiceGroupID)
        End If

        If Not IsPostBack Then
            ddlPublicAccess.Items.Add(New ListItem("Public", "0"))
            ddlPublicAccess.Items.Add(New ListItem("Assigned Only", "1"))
            ddlPublicAccess.Items.Add(New ListItem("Private", "2"))
            ddlPublicAccess.SelectedValue = "2"

            If m_ServiceGroupID > 0 Then
                Dim objServiceGroup As ServiceGroup
                objServiceGroup = m_ScopeService.GetServiceGroupByServiceGroupId(m_ServiceGroupID)
                txtServiceGroup.Text = objServiceGroup.Name
                If objServiceGroup.DisplayOrder <> 0 Then
                    txtDisplayOrder.Text = objServiceGroup.DisplayOrder
                End If
                ddlPublicAccess.SelectedValue = objServiceGroup.IsPrivate

                Dim dsServiceGroupRelationship As DataSet = New DataSet()
                dsServiceGroupRelationship = m_ScopeService.GetProjectOwnersAssignableByUserId(m_LoginUser.UserId)
                rpServiceGroupRelationship.DataSource = dsServiceGroupRelationship.Tables(0)
                rpServiceGroupRelationship.DataBind()

                btnAdd.Visible = False
                btnUpdate.Visible = True
                btnClose.Visible = True
                btnDelete.Visible = True
                trServiceGroupRelationship.Visible = False

                lblServiceGroup.Text = "UPDATE RATE SHEET"
            Else
                btnAdd.Visible = True
                btnUpdate.Visible = False
                btnClose.Visible = True
                btnDelete.Visible = False
                trServiceGroupRelationship.Visible = False

                lblServiceGroup.Text = "ADD RATE SHEET"
            End If
        End If

        txtServiceGroup.Focus()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtServiceGroup.Text <> String.Empty Then
            Dim objServiceGroup As New ServiceGroup
            objServiceGroup.UserId = m_LoginUser.UserId
            objServiceGroup.Name = txtServiceGroup.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objServiceGroup.DisplayOrder = intDisplayOrder
            objServiceGroup.IsPrivate = ddlPublicAccess.SelectedValue
            m_ScopeService.CreateServiceGroup(objServiceGroup)

            Response.Redirect(String.Format("OwnerService.aspx?msg=The service group is added successfully."))
        Else
            Response.Redirect(String.Format("OwnerService.aspx?msg=The service group is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If m_ServiceGroupID > 0 And txtServiceGroup.Text <> String.Empty Then
            Dim objServiceGroup As ServiceGroup = m_ScopeService.GetServiceGroupByServiceGroupId(m_ServiceGroupID)
            objServiceGroup.Name = txtServiceGroup.Text.Trim
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            objServiceGroup.DisplayOrder = intDisplayOrder
            objServiceGroup.IsPrivate = ddlPublicAccess.SelectedValue
            m_ScopeService.UpdateServiceGroup(objServiceGroup)

            Dim index As Integer
            For index = 0 To rpServiceGroupRelationship.Items.Count - 1
                Dim CurrentCheckBox As CheckBox = CType(rpServiceGroupRelationship.Items(index).FindControl("cbServiceGroupRelationship"), WebControls.CheckBox)
                Dim CurrentProjectOwnerID As Integer = 0
                Dim CurrentTextBox As TextBox = CType(rpServiceGroupRelationship.Items(index).FindControl("tbServiceGroupRelationship"), WebControls.TextBox)
                Integer.TryParse(m_Cryption.Decrypt(CurrentTextBox.Text, m_Cryption.cryptionKey), CurrentProjectOwnerID)
                If CurrentCheckBox.Checked And CurrentProjectOwnerID > 0 Then
                    m_ScopeService.CreateServiceGroupRelationship(m_ServiceGroupID, CurrentProjectOwnerID)
                ElseIf Not CurrentCheckBox.Checked And CurrentProjectOwnerID > 0 Then
                    m_ScopeService.DeleteServiceGroupRelationshipByServiceGroupIdProjectOwnerId(m_ServiceGroupID, CurrentProjectOwnerID)
                End If
            Next

            Response.Redirect(String.Format("OwnerService.aspx?msg=The service group is updated successfully."))
        Else
            Response.Redirect(String.Format("OwnerService.aspx?msg=The service group is invalid, please try again."))
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("OwnerService.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_ServiceGroupID > 0 Then
            Dim index As Integer
            For index = 0 To rpServiceGroupRelationship.Items.Count - 1
                Dim CurrentCheckBox As CheckBox = CType(rpServiceGroupRelationship.Items(index).FindControl("cbServiceGroupRelationship"), WebControls.CheckBox)
                Dim CurrentProjectOwnerID As Integer = 0
                Dim CurrentTextBox As TextBox = CType(rpServiceGroupRelationship.Items(index).FindControl("tbServiceGroupRelationship"), WebControls.TextBox)
                Integer.TryParse(m_Cryption.Decrypt(CurrentTextBox.Text, m_Cryption.cryptionKey), CurrentProjectOwnerID)
                m_ScopeService.DeleteServiceGroupRelationshipByServiceGroupIdProjectOwnerId(m_ServiceGroupID, CurrentProjectOwnerID)
            Next

            m_ScopeService.DeleteServiceGroupByServiceGroupId(m_ServiceGroupID, True)
            Response.Redirect(String.Format("OwnerService.aspx?msg=The service group is deleted successfully."))
        End If
    End Sub

    Public Function ShowWidth() As String
        Dim TableWidth As String = "200"
        Return TableWidth
    End Function

    Public Function checkBox(ByVal ProjectOwnerID As String)
        Dim dsServiceGroupRelationship As DataSet = m_ScopeService.GetServiceGroupRelationshipByServiceGroupIdProjectOwnerId(m_ServiceGroupID, ProjectOwnerID)
        If dsServiceGroupRelationship.Tables.Count > 0 Then
            If dsServiceGroupRelationship.Tables(0).Rows.Count > 0 Then
                Return "true"
            End If
        End If
        Return "false"
    End Function

    Public Function showTabIndex() As Integer
        m_ProjectOwnerTabIndex = m_ProjectOwnerTabIndex + 1
        Return Integer.Parse(String.Format("{0}", m_ProjectOwnerTabIndex))
    End Function
End Class
