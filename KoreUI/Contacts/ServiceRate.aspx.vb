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

Partial Class Contacts_ServiceRate
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Private m_ContactID As Integer = 0
    Private m_ContactOwnerID As Integer = 0
    Private m_ServiceID As Integer = 0
    Private m_ServiceRateID As Integer = 0
    Public m_ServiceGroupCount As Integer = 0
    Public m_ServiceGroupTabIndex As Integer = 10

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnAdd.Visible = False
            btnUpdate.Visible = False
            btnDelete.Visible = False
        End If

        If Request.QueryString("SID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("SID"), m_Cryption.cryptionKey), m_ServiceRateID)
        End If

        If m_ServiceRateID > 0 Then
            If Request.QueryString("ID") <> String.Empty Then
                Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ServiceID)
            End If
        Else
            If Request.QueryString("ID") <> String.Empty Then
                Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ContactID)
            End If
        End If

        Dim dsService As DataSet
        If m_ContactID = 0 Then
            If m_ServiceID > 0 Then
                dsService = m_ScopeService.GetAllServiceRatesByServiceId(m_ServiceID)
                If dsService.Tables.Count > 0 Then
                    If dsService.Tables(0).Rows.Count > 0 Then
                        If Not IsDBNull(dsService.Tables(0).Rows(0).Item("UserId")) Then
                            Long.TryParse(dsService.Tables(0).Rows(0).Item("UserId"), m_ContactID)
                        End If
                    End If
                End If
            End If
        End If

        If m_ContactID > 0 Then
            Dim objContactOwner As ProjectOwner = m_ManagementService.GetProjectOwnerByContactId(m_ContactID)
            m_ContactOwnerID = objContactOwner.ProjectOwnerId
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
            ddlStatus.Items.Add(New ListItem("ON", "ON"))
            ddlStatus.Items.Add(New ListItem("OFF", "OFF"))

            dsService = m_ScopeService.GetServicesByProjectOwnerIdUserId(m_LoginUser.CompanyId, m_ContactID)
            ddlService.Items.Add(New ListItem("Select Service", "Select Service"))
            If dsService.Tables.Count > 0 Then
                If dsService.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsService.Tables(0).Rows
                        If Not IsDBNull(dr("Name")) Then
                            Dim liListItem As New ListItem
                            liListItem.Text = dr("Name")
                            liListItem.Value = dr("ServiceId")
                            'liListItem.Attributes.Add("onclick", "updateService('" & dr("Name") & "');")
                            ddlService.Items.Add(liListItem)
                        End If
                    Next
                End If
            End If

            If m_ServiceRateID > 0 Then
                Dim dsServiceRate As DataSet = m_ScopeService.GetServiceRateByServiceRateId(m_ServiceRateID)
                Dim index As Integer
                If dsServiceRate.Tables.Count > 0 Then
                    If dsServiceRate.Tables(0).Rows.Count > 0 Then
                        txtService.Text = dsServiceRate.Tables(0).Rows(0).Item("Name")
                        If Not IsDBNull(dsServiceRate.Tables(0).Rows(0).Item("DisplayOrder")) Then
                            txtDisplayOrder.Text = dsServiceRate.Tables(0).Rows(0).Item("DisplayOrder")
                        End If
                        If Not IsDBNull(dsServiceRate.Tables(0).Rows(0).Item("CostRate")) Then
                            tbxCost.Text = FormatCurrency(dsServiceRate.Tables(0).Rows(0).Item("CostRate"))
                        End If
                        If Not IsDBNull(dsServiceRate.Tables(0).Rows(0).Item("ChargeRate")) Then
                            tbxCharge.Text = FormatCurrency(dsServiceRate.Tables(0).Rows(0).Item("ChargeRate"))
                        End If                        

                        For index = 0 To ddlUnit.Items.Count - 1
                            If ddlUnit.Items(index).Value = dsServiceRate.Tables(0).Rows(0).Item("Unit") Then
                                ddlUnit.SelectedIndex = index
                                Exit For
                            End If
                        Next
                        ddlStatus.SelectedValue = "ON"
                        If Not IsDBNull(dsServiceRate.Tables(0).Rows(0).Item("Status")) Then
                            If dsServiceRate.Tables(0).Rows(0).Item("Status") Then
                                ddlStatus.SelectedValue = "OFF"
                            End If
                        End If

                        If Not IsDBNull(dsServiceRate.Tables(0).Rows(0).Item("Description")) Then
                            tbxNote.Text = dsServiceRate.Tables(0).Rows(0).Item("Description")
                        End If
                    End If
                End If

                Dim dsServiceGroup As DataSet = New DataSet()
                If m_ContactOwnerID > 0 Then
                    dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_LoginUser.UserId, m_ContactOwnerID)
                Else
                    dsServiceGroup = m_ScopeService.GetServiceGroupsByUserId(m_LoginUser.UserId)
                End If

                trServiceGroup.Visible = False
                If dsServiceGroup.Tables.Count > 0 Then
                    If dsServiceGroup.Tables(0).Rows.Count > 0 Then
                        trServiceGroup.Visible = True
                        m_ServiceGroupCount = dsServiceGroup.Tables(0).Rows.Count
                    End If
                End If
                rpServiceGroup.DataSource = dsServiceGroup.Tables(0)
                rpServiceGroup.DataBind()

                For index = 0 To rpServiceGroup.Items.Count - 1
                    Dim chkbox As CheckBox = rpServiceGroup.Items(index).FindControl("cbServiceGroup")
                    chkbox.InputAttributes.Add("onchange", "checkAllChecked(" & index & "," & rpServiceGroup.Items.Count & ")")
                Next

                btnAdd.Visible = False
                btnUpdate.Visible = True
                btnClose.Visible = True
                btnDelete.Visible = True

                ddlService.Enabled = False
                'txtService.Enabled = False

                lblServiceRate.Text = "UPDATE SERVICE RATE"
            Else
                btnAdd.Visible = True
                btnUpdate.Visible = False
                btnClose.Visible = True
                btnDelete.Visible = False
                'trServiceGroup.Visible = False

                Dim dsServiceGroup As DataSet = New DataSet()
                If m_ContactOwnerID > 0 Then
                    dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_LoginUser.UserId, m_ContactOwnerID)
                Else
                    dsServiceGroup = m_ScopeService.GetServiceGroupsByUserId(m_LoginUser.UserId)
                End If

                trServiceGroup.Visible = False
                Dim intServiceGroupIndex As Integer
                If dsServiceGroup.Tables.Count > 0 Then
                    If dsServiceGroup.Tables(0).Rows.Count > 0 Then
                        trServiceGroup.Visible = True
                        m_ServiceGroupCount = dsServiceGroup.Tables(0).Rows.Count
                        If Session("SelectedServiceGroupIndex") = Nothing Then
                            intServiceGroupIndex = 0
                        Else
                            If Session("SelectedServiceGroupIndex") < 1 Then
                                intServiceGroupIndex = 0
                            Else
                                intServiceGroupIndex = Session("SelectedServiceGroupIndex") - 1
                            End If
                            'Session("SelectedServiceGroupIndex") = Nothing
                        End If
                    End If
                End If
                rpServiceGroup.DataSource = dsServiceGroup.Tables(0)
                rpServiceGroup.DataBind()



                For index = 0 To rpServiceGroup.Items.Count - 1
                    Dim chkbox As CheckBox = rpServiceGroup.Items(index).FindControl("cbServiceGroup")
                    chkbox.InputAttributes.Add("onchange", "checkAllChecked(" & index & "," & rpServiceGroup.Items.Count & ")")
                    If index = intServiceGroupIndex Then
                        chkbox.Checked = True
                    Else
                        chkbox.Checked = False
                    End If
                Next

                lblServiceRate.Text = "ADD SERVICE RATE"
            End If
        End If

        txtService.Focus()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Login.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If txtService.Text <> String.Empty Then
            Dim index As Integer
            Dim isNewService As Boolean = True
            For index = 0 To ddlService.Items.Count - 1
                If ddlService.Items(index).Text.ToLower = txtService.Text.Trim.ToLower Then
                    isNewService = False
                    ddlService.SelectedIndex = index
                    Exit For
                End If
            Next

            If isNewService Then
                Dim objService As New Service
                objService.Name = txtService.Text.Trim
                objService.ProjectOwnerId = m_LoginUser.CompanyId
                objService.UserId = m_ContactID
                objService.Description = tbxNote.Text.Trim
                Dim intDisplayOrder As Integer = 0
                Integer.TryParse(txtDisplayOrder.Text, intDisplayOrder)
                objService.DisplayOrder = intDisplayOrder
                m_ServiceID = m_ScopeService.CreateService(objService)
            Else
                m_ServiceID = ddlService.SelectedValue
            End If

            Dim objServiceRate As New ServiceRate
            objServiceRate.ServiceId = m_ServiceID
            If ddlStatus.SelectedValue = "ON" Then
                objServiceRate.Disabled = False
            Else
                objServiceRate.Disabled = True
            End If
            If tbxCost.Text.Trim() <> String.Empty Then
                objServiceRate.CostRate = tbxCost.Text.Trim
            End If
            If tbxCharge.Text.Trim() <> String.Empty Then
                objServiceRate.ChargeRate = tbxCharge.Text.Trim
            End If
            objServiceRate.Unit = ddlUnit.SelectedValue
            m_ScopeService.CreateServiceRate(objServiceRate)

            For index = 0 To rpServiceGroup.Items.Count - 1
                Dim CurrentCheckBox As CheckBox = CType(rpServiceGroup.Items(index).FindControl("cbServiceGroup"), WebControls.CheckBox)
                Dim CurrentServiceGroupID As Integer = 0
                Dim CurrentTextBox As TextBox = CType(rpServiceGroup.Items(index).FindControl("tbServiceGroup"), WebControls.TextBox)
                Integer.TryParse(m_Cryption.Decrypt(CurrentTextBox.Text, m_Cryption.cryptionKey), CurrentServiceGroupID)
                If CurrentCheckBox.Checked And CurrentServiceGroupID > 0 Then
                    m_ScopeService.CreateServiceGroupItem(CurrentServiceGroupID, m_ServiceID)
                ElseIf Not CurrentCheckBox.Checked And CurrentServiceGroupID > 0 Then
                    m_ScopeService.DeleteServiceGroupItemByServiceGroupIdServiceId(CurrentServiceGroupID, m_ServiceID)
                End If
            Next

            If m_ContactID > 0 Then
                Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The service rate is added successfully.", m_Cryption.Encrypt(m_ContactID, m_Cryption.cryptionKey)))
            Else
                Response.Redirect(String.Format("OwnerService.aspx?msg=The service rate is added successfully."))
            End If
        End If
        Response.Redirect(String.Format("OwnerService.aspx?msg=The service rate is not added, please enter all the details."))
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If m_ServiceRateID > 0 Then
            Dim objService As New Service
            objService.Name = txtService.Text.Trim
            objService.ProjectOwnerId = m_LoginUser.CompanyId
            objService.UserId = m_ContactID
            objService.Description = tbxNote.Text.Trim
            objService.ServiceId = m_ServiceID
            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text, intDisplayOrder)
            objService.DisplayOrder = intDisplayOrder
            m_ScopeService.UpdateService(objService)

            Dim objServiceRate As New ServiceRate
            objServiceRate.ServiceId = m_ServiceID
            objServiceRate.ServiceRateId = m_ServiceRateID
            If ddlStatus.SelectedValue = "ON" Then
                objServiceRate.Disabled = False
            Else
                objServiceRate.Disabled = True
            End If
            objServiceRate.CostRate = Replace(tbxCost.Text.Trim, "$", "")
            objServiceRate.ChargeRate = Replace(tbxCharge.Text.Trim, "$", "")
            objServiceRate.Unit = ddlUnit.SelectedValue
            m_ScopeService.UpdateServiceRate(objServiceRate)

            For index = 0 To rpServiceGroup.Items.Count - 1
                Dim CurrentCheckBox As CheckBox = CType(rpServiceGroup.Items(index).FindControl("cbServiceGroup"), WebControls.CheckBox)
                Dim CurrentServiceGroupID As Integer = 0
                Dim CurrentTextBox As TextBox = CType(rpServiceGroup.Items(index).FindControl("tbServiceGroup"), WebControls.TextBox)
                Integer.TryParse(m_Cryption.Decrypt(CurrentTextBox.Text, m_Cryption.cryptionKey), CurrentServiceGroupID)
                If CurrentCheckBox.Checked And CurrentServiceGroupID > 0 Then
                    m_ScopeService.CreateServiceGroupItem(CurrentServiceGroupID, m_ServiceID)
                ElseIf Not CurrentCheckBox.Checked And CurrentServiceGroupID > 0 Then
                    m_ScopeService.DeleteServiceGroupItemByServiceGroupIdServiceId(CurrentServiceGroupID, m_ServiceID)
                End If
            Next

            If m_ContactID > 0 Then
                Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The service rate is updated successfully.", m_Cryption.Encrypt(m_ContactID, m_Cryption.cryptionKey)))
            Else
                Response.Redirect(String.Format("OwnerService.aspx?msg=The service rate is updated successfully."))
            End If
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If m_ContactID > 0 Then
            Response.Redirect(String.Format("Detail.aspx?ID={0}", m_Cryption.Encrypt(m_ContactID, m_Cryption.cryptionKey)))
        Else
            If m_ServiceID > 0 Then
                Dim dsService As DataSet = m_ScopeService.GetAllServiceRatesByServiceId(m_ServiceID)
                If dsService.Tables.Count > 0 Then
                    If dsService.Tables(0).Rows.Count > 0 Then
                        If Not IsDBNull(dsService.Tables(0).Rows(0).Item("UserId")) Then
                            Long.TryParse(dsService.Tables(0).Rows(0).Item("UserId"), m_ContactID)
                        End If
                    End If
                End If
            End If

            If m_ContactID > 0 Then
                Response.Redirect(String.Format("Detail.aspx?ID={0}", m_Cryption.Encrypt(m_ContactID, m_Cryption.cryptionKey)))
            Else
                Response.Redirect("OwnerService.aspx")
            End If
        End If
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_ServiceRateID > 0 Then
            m_ScopeService.DeleteServiceRateByServiceRateId(m_ServiceRateID)
            m_ScopeService.DeleteServiceByServiceId(m_ServiceID)

            For index = 0 To rpServiceGroup.Items.Count - 1
                Dim CurrentCheckBox As CheckBox = CType(rpServiceGroup.Items(index).FindControl("cbServiceGroup"), WebControls.CheckBox)
                Dim CurrentServiceGroupID As Integer = 0
                Dim CurrentTextBox As TextBox = CType(rpServiceGroup.Items(index).FindControl("tbServiceGroup"), WebControls.TextBox)
                Integer.TryParse(m_Cryption.Decrypt(CurrentTextBox.Text, m_Cryption.cryptionKey), CurrentServiceGroupID)
                m_ScopeService.DeleteServiceGroupItemByServiceGroupIdServiceId(CurrentServiceGroupID, m_ServiceID)
            Next

            If m_ContactID > 0 Then
                Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The service rate is deleted", m_Cryption.Encrypt(m_ContactID, m_Cryption.cryptionKey)))
            Else
                Response.Redirect(String.Format("OwnerService.aspx?msg=The service rate is deleted"))
            End If
        End If
    End Sub

    Public Function ShowWidth() As String
        Dim TableWidth As String = "200"
        Return TableWidth
    End Function

    Public Function checkBox(ByVal ServiceGroupID As String)
        Dim dsServiceGroupItem As DataSet = m_ScopeService.GetServiceGroupItemByServiceGroupIdServiceId(ServiceGroupID, m_ServiceID)
        If dsServiceGroupItem.Tables.Count > 0 Then
            If dsServiceGroupItem.Tables(0).Rows.Count > 0 Then
                Return "true"
            End If
        End If
        Return "false"
    End Function

    Public Function showTabIndex() As Integer
        m_ServiceGroupTabIndex = m_ServiceGroupTabIndex + 1
        Return Integer.Parse(String.Format("{0}", m_ServiceGroupTabIndex))
    End Function
End Class
