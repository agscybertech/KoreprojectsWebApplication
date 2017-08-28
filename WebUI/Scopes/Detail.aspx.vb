Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class Scopes_Detail
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Private m_ScopeItemId As Long
    Private m_ScopeId As Long
    Private m_ProjectId As Long = 0
    Private m_ProjectOwnerId As Long = 0
    Private m_ProjectOwnerUserId As Long = 0
    Private m_Cryption As New Cryption
    Private m_ServiceGroupIndex As Long = 0

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
            'If m_LoginUser.Type = 0 Then
            '    btnDelete.Visible = False
            'End If
        End If
        If Request.QueryString("sid") <> String.Empty Then
            Long.TryParse(m_Cryption.Decrypt(Request.QueryString("sid"), m_Cryption.cryptionKey), m_ScopeItemId)
        Else
            m_ScopeItemId = 0
        End If
        If Request.QueryString("id") <> String.Empty Then
            Long.TryParse(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey), m_ScopeId)
        Else
            m_ScopeId = 0
        End If
        If Request.QueryString("msg") <> "" Then
            lblMsg.Text = Request.QueryString("msg")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnSave.Visible = False
            btnQuickSave.Visible = False
        End If

        m_ProjectId = m_ScopeService.GetScopeByScopeId(m_ScopeId).ProjectId
        Dim dsProject As New DataSet
        If m_ProjectId > 0 Then
            dsProject = m_ManagementService.GetProjectInfoByProjectId(m_ProjectId)
        End If
        If dsProject.Tables.Count > 0 Then
            If dsProject.Tables(0).Rows.Count = 1 Then
                If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Name")) Then
                    scopenametitle.Text = dsProject.Tables(0).Rows(0).Item("Name")
                End If
            End If
        End If

        m_ProjectOwnerId = m_ManagementService.GetProjectByProjectId(m_ProjectId).ProjectOwnerId
        m_ProjectOwnerUserId = m_ManagementService.GetProjectOwnerByProjectOwnerId(m_ProjectOwnerId).ContactId
        If Not IsPostBack Then
            Dim dr As DataRow
            Dim dsScopeGroup As DataSet = New DataSet()
            dsScopeGroup = m_ScopeService.GetWorksheetGroupsByProjectOwnerId(m_ProjectOwnerId)
            ddlGroup.Items.Add(New ListItem("None", "0"))
            If dsScopeGroup.Tables.Count > 0 Then
                If dsScopeGroup.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsScopeGroup.Tables(0).Rows
                        Dim liListItem As New ListItem
                        liListItem.Text = dr("Name")
                        liListItem.Value = dr("WorksheetGroupId")
                        'liListItem.Attributes.Add("onclick", "updateArea('" & dr("Name") & "');")
                        ddlGroup.Items.Add(liListItem)
                    Next
                End If
            End If

            Dim dsScopeItemArea As DataSet = New DataSet()
            dsScopeItemArea = m_ScopeService.GetAreasByProjectOwnerId(m_ProjectOwnerId)
            ddlArea.Items.Add(New ListItem("None", "None"))
            If dsScopeItemArea.Tables.Count > 0 Then
                If dsScopeItemArea.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsScopeItemArea.Tables(0).Rows
                        Dim liListItem As New ListItem
                        liListItem.Text = dr("Name")
                        liListItem.Value = dr("AreaId")
                        'liListItem.Attributes.Add("onclick", String.Format("updateTextControl('{0}','{1}');", dr("Name"), txtArea.ClientID))
                        ddlArea.Items.Add(liListItem)
                    Next
                End If
            End If
            ddlArea.Attributes.Add("onchange", String.Format("updateTextControl(this.options[this.selectedIndex].innerHTML,'{0}');", txtArea.ClientID))

            Dim dsScopeItem As DataSet = New DataSet()
            dsScopeItem = m_ScopeService.GetItemsByProjectOwnerId(m_ProjectOwnerId)
            ddlItem.Items.Add(New ListItem("None", "None"))
            If dsScopeItem.Tables.Count > 0 Then
                If dsScopeItem.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsScopeItem.Tables(0).Rows
                        Dim liListItem As New ListItem
                        liListItem.Text = dr("Name")
                        liListItem.Value = dr("ItemId")
                        'liListItem.Attributes.Add("onclick", "updateItem('" & dr("Name") & "');")
                        ddlItem.Items.Add(liListItem)
                    Next
                End If
            End If
            ddlItem.Attributes.Add("onchange", String.Format("updateTextControl(this.options[this.selectedIndex].innerHTML,'{0}');", tbxItem.ClientID))

            Dim dsScopeService As DataSet = New DataSet()
            dsScopeService = m_ScopeService.GetWorksheetServicesByProjectOwnerId(m_ProjectOwnerId)
            ddlService.Items.Add(New ListItem("None", "None"))
            If dsScopeService.Tables.Count > 0 Then
                If dsScopeService.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsScopeService.Tables(0).Rows
                        Dim liListItem As New ListItem
                        liListItem.Text = dr("Name")
                        liListItem.Value = dr("WorksheetServiceId")
                        'liListItem.Attributes.Add("onclick", "updateItem('" & dr("Name") & "');")
                        ddlService.Items.Add(liListItem)
                    Next
                End If
            End If
            If m_ScopeItemId > 0 Then
                Dim objScopeItem As ScopeItem
                objScopeItem = m_ScopeService.GetScopeItemByScopeItemId(m_ScopeItemId)

                If objScopeItem.ServiceGroupId = 0 And objScopeItem.Service Is Nothing And Not objScopeItem.Description Is Nothing Then
                    ddlService.Attributes.Add("onchange", String.Format("updateService(this.options[this.selectedIndex].innerHTML, this.selectedIndex,'{0}','{1}','{2}','{3}','{4}');", txtService.ClientID, ddlMeasurement.ClientID, tbxDescription.ClientID, tbxMeasurement.ClientID, objScopeItem.Description))
                Else
                    ddlService.Attributes.Add("onchange", String.Format("updateService(this.options[this.selectedIndex].innerHTML, this.selectedIndex,'{0}','{1}','{2}','{3}','');", txtService.ClientID, ddlMeasurement.ClientID, tbxDescription.ClientID, tbxMeasurement.ClientID))
                End If
            Else
                ddlService.Attributes.Add("onchange", String.Format("updateService(this.options[this.selectedIndex].innerHTML, this.selectedIndex,'{0}','{1}','{2}','{3}','');", txtService.ClientID, ddlMeasurement.ClientID, tbxDescription.ClientID, tbxMeasurement.ClientID))
            End If

            Dim dsAssignee As DataSet = New DataSet()
            'dsAssignee = m_ManagementService.GetUserProfilesByProjectOwnerID(m_LoginUser.CompanyId)
            dsAssignee = m_ManagementService.GetUserProfilesAssignableByPartyA(m_ProjectOwnerUserId)
            ddlAssign.Items.Add(New ListItem("None", "None"))
            If dsAssignee.Tables.Count > 0 Then
                If dsAssignee.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsAssignee.Tables(0).Rows
                        Dim liListItem As New ListItem
                        liListItem.Text = String.Format("{0} {1}", dr("FirstName"), dr("LastName"))
                        liListItem.Value = dr("UserId")
                        'liListItem.Attributes.Add("onclick", "updateAssign('" & String.Format("{0} {1}", dr("FirstName"), dr("LastName")) & "');")
                        ddlAssign.Items.Add(liListItem)
                    Next
                End If
            End If

            Dim dsScopeItemStatus As DataSet = New DataSet()
            dsScopeItemStatus = m_ScopeService.GetScopeItemStatuses()
            ddlStatus.DataSource = dsScopeItemStatus.Tables(0)
            ddlStatus.DataTextField = "Name"
            ddlStatus.DataValueField = "ScopeItemStatusId"
            ddlStatus.DataBind()

            LoadScopeItem(m_ScopeItemId)
        End If

        'Dim blnScopePricing As Boolean = False
        'Boolean.TryParse(Session("ScopePricing"), blnScopePricing)
        'If blnScopePricing Then
        '    tdRate.Visible = True
        'Else
        '    tdRate.Visible = False
        'End If

        ddlArea.Focus()
    End Sub

    Public Function showRate() As String
        Dim result As String
        Dim blnScopePricing As Boolean = False
        Boolean.TryParse(Session("ScopePricing"), blnScopePricing)
        If blnScopePricing Then
            result = "display:"
        Else
            result = "display:none"
        End If
        Return result
    End Function

    Private Sub LoadScopeItem(ByVal ScopeItemId As Long)
        Dim defaultMeasurement As String = String.Empty
        Dim dsUnits As DataSet
        dsUnits = m_ScopeService.GetUnits()
        If dsUnits.Tables.Count > 0 Then
            If dsUnits.Tables(0).Rows.Count > 0 Then
                For Each dr In dsUnits.Tables(0).Rows
                    If defaultMeasurement = String.Empty Then
                        defaultMeasurement = dr("Name")
                    End If
                    ddlMeasurement.Items.Add(New ListItem(dr("Name"), dr("Name")))
                Next
            End If
        End If
        tbxMeasurement.Attributes.Add("style", "display:none")

        If ScopeItemId > 0 Then
            Dim objScopeItem As ScopeItem
            Dim index As Integer
            objScopeItem = m_ScopeService.GetScopeItemByScopeItemId(ScopeItemId)
            If objScopeItem.ScopeGroupId > 0 Then
                For index = 0 To ddlGroup.Items.Count - 1
                    If ddlGroup.Items(index).Text = objScopeItem.ScopeGroup Then
                        ddlGroup.SelectedIndex = index
                        Exit For
                    End If
                Next
            End If
            txtArea.Text = objScopeItem.Area
            For index = 0 To ddlArea.Items.Count - 1
                If ddlArea.Items(index).Text = txtArea.Text Then
                    ddlArea.SelectedIndex = index
                    txtArea.Attributes.Add("style", "display:none")
                    'txtArea.Visible = False
                    Exit For
                End If
            Next
            tbxItem.Text = objScopeItem.Item
            For index = 0 To ddlItem.Items.Count - 1
                If ddlItem.Items(index).Text = tbxItem.Text Then
                    ddlItem.SelectedIndex = index
                    tbxItem.Attributes.Add("style", "display:none")
                    'tbxItem.Visible = False
                    Exit For
                End If
            Next
            If Not objScopeItem.WorksheetService Is Nothing Then
                txtService.Text = objScopeItem.WorksheetService
                For index = 0 To ddlService.Items.Count - 1
                    If ddlService.Items(index).Text = txtService.Text Then
                        ddlService.SelectedIndex = index
                        txtService.Attributes.Add("style", "display:none")

                        If ddlMeasurement.Items.Count > 0 Then
                            ddlMeasurement.Items.Clear()
                        End If
                        Dim dsScopeService As WorksheetService = m_ScopeService.GetWorksheetServiceByWorksheetServiceId(ddlService.Items(index).Value)
                        ddlMeasurement.Items.Add(New ListItem(dsScopeService.Unit, dsScopeService.Unit))
                        'txtService.Visible = False
                        Exit For
                    End If
                Next
            End If


            tbxAssign.Text = objScopeItem.AssignTo
            tbxService.Text = objScopeItem.Service
            tbxServiceGroup.Text = objScopeItem.ServiceGroupId
            tbxRM.Text = objScopeItem.AreaMeasurement

            Dim dsServiceGroup As DataSet = New DataSet()
            If objScopeItem.AssignToId > 0 Then
                ddlAssign.SelectedValue = objScopeItem.AssignToId
                Dim ProjectOwnerId_AssignTo As Long
                ProjectOwnerId_AssignTo = m_ManagementService.GetProjectOwnerByContactId(objScopeItem.AssignToId).ProjectOwnerId
                'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_LoginUser.UserId, ProjectOwnerId_AssignTo)
                'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_ProjectOwnerUserId, ProjectOwnerId_AssignTo)
                dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerIdLoginUserId(m_ProjectOwnerUserId, ProjectOwnerId_AssignTo, m_LoginUser.UserId)
                If dsServiceGroup.Tables(0).Rows.Count = 0 Then
                    'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_LoginUser.UserId, 0)
                    'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_ProjectOwnerUserId, 0)
                    dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerIdLoginUserId(m_ProjectOwnerUserId, 0, m_LoginUser.UserId)
                End If
            Else
                'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_LoginUser.UserId, 0)
                'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_ProjectOwnerUserId, 0)
                dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerIdLoginUserId(m_ProjectOwnerUserId, 0, m_LoginUser.UserId)
            End If

            If ddlServiceGroup.Items.Count > 0 Then
                ddlServiceGroup.Items.Clear()
            End If
            ddlServiceGroup.Items.Add(New ListItem("None", "0"))

            If ddlServicesByGroup.Items.Count > 0 Then
                ddlServicesByGroup.Items.Clear()
            End If

            If dsServiceGroup.Tables.Count > 0 Then
                If dsServiceGroup.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsServiceGroup.Tables(0).Rows
                        Dim liListItem As New ListItem
                        liListItem.Text = String.Format("{0}", dr("Name"))
                        liListItem.Value = dr("ServiceGroupId")
                        If Not tbxServiceGroup.Text Is Nothing Then
                            If liListItem.Value = tbxServiceGroup.Text Then
                                liListItem.Selected = True
                            End If
                        End If
                        ddlServiceGroup.Items.Add(liListItem)
                    Next
                End If
            End If

            If Not tbxServiceGroup.Text Is Nothing Then
                Dim dsService As DataSet = New DataSet()
                dsService = m_ScopeService.GetServicesByServiceGroupId(tbxServiceGroup.Text)
                If dsService.Tables.Count > 0 Then
                    If dsService.Tables(0).Rows.Count > 0 Then
                        For Each dr In dsService.Tables(0).Rows
                            Dim liListItem As New ListItem
                            'liListItem.Text = String.Format("{0} {1:c}", dr("Name"), dr("Rate"))
                            liListItem.Text = String.Format("{0}", dr("Name"))
                            liListItem.Value = dr("ServiceId")
                            If liListItem.Text = tbxService.Text Then
                                liListItem.Selected = True
                            End If
                            ddlServicesByGroup.Items.Add(liListItem)
                        Next
                    Else
                        ddlServicesByGroup.Items.Add(New ListItem("None", "None"))
                    End If
                End If
            End If

            tbxDescription.Text = objScopeItem.Description
            ddlStatus.SelectedValue = objScopeItem.ScopeItemStatusId
            tbxQTY.Text = objScopeItem.Quantity.ToString("n2")
            ddlMeasurement.SelectedValue = objScopeItem.Unit
            tbxMeasurement.Text = objScopeItem.Unit
            tbxRate.Text = objScopeItem.Rate.ToString("c")
            lblTitle.Text = "Edit Worksheet Item"
            btnDelete.Attributes.Add("OnClick", "return confirm('Are you sure you want to delete this Worksheet item?');")
            btnQuickDelete.Attributes.Add("OnClick", "return confirm('Are you sure you want to delete this Worksheet item?');")
            tbxDisplayOrder.Text = objScopeItem.DisplayOrder

            btnSave.Visible = True
            btnQuickSave.Visible = False
            btnClose.Visible = True
            btnQuickCancel.Visible = False
            btnDelete.Visible = True
            btnQuickDelete.Visible = False
        Else
            Dim dsServiceGroup As DataSet = New DataSet()
            'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_LoginUser.UserId, 0)
            'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_ProjectOwnerUserId, 0)
            dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerIdLoginUserId(m_ProjectOwnerUserId, 0, m_LoginUser.UserId)

            If ddlServiceGroup.Items.Count > 0 Then
                ddlServiceGroup.Items.Clear()
            End If
            ddlServiceGroup.Items.Add(New ListItem("None", "0"))

            If ddlServicesByGroup.Items.Count > 0 Then
                ddlServicesByGroup.Items.Clear()
            End If
            ddlServicesByGroup.Items.Add(New ListItem("None", "None"))

            If dsServiceGroup.Tables.Count > 0 Then
                If dsServiceGroup.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsServiceGroup.Tables(0).Rows
                        Dim liListItem As New ListItem
                        liListItem.Text = String.Format("{0}", dr("Name"))
                        liListItem.Value = dr("ServiceGroupId")
                        ddlServiceGroup.Items.Add(liListItem)
                    Next
                End If
            End If

            If Not Session("ScopeItemDuplication") Is Nothing Then
                Dim objScopeItem As ScopeItem
                objScopeItem = CType(Session("ScopeItemDuplication"), ScopeItem)
                If objScopeItem.ScopeGroupId > 0 Then
                    For index = 0 To ddlGroup.Items.Count - 1
                        If ddlGroup.Items(index).Text = objScopeItem.ScopeGroup Then
                            ddlGroup.SelectedIndex = index
                            Exit For
                        End If
                    Next
                End If
                txtArea.Text = objScopeItem.Area
                For index = 0 To ddlArea.Items.Count - 1
                    If ddlArea.Items(index).Text = txtArea.Text Then
                        ddlArea.SelectedIndex = index
                        txtArea.Attributes.Add("style", "display:none")
                        'txtArea.Visible = False
                        Exit For
                    End If
                Next
                tbxItem.Text = objScopeItem.Item
                For index = 0 To ddlItem.Items.Count - 1
                    If ddlItem.Items(index).Text = tbxItem.Text Then
                        ddlItem.SelectedIndex = index
                        tbxItem.Attributes.Add("style", "display:none")
                        'tbxItem.Visible = False
                        Exit For
                    End If
                Next
                If Not objScopeItem.WorksheetService Is Nothing Then
                    txtService.Text = objScopeItem.WorksheetService
                    For index = 0 To ddlService.Items.Count - 1
                        If ddlService.Items(index).Text = txtService.Text Then
                            ddlService.SelectedIndex = index
                            txtService.Attributes.Add("style", "display:none")
                            'txtService.Visible = False
                            Exit For
                        End If
                    Next
                End If

                tbxDescription.Text = objScopeItem.Description
                ddlMeasurement.SelectedValue = objScopeItem.Unit
                tbxMeasurement.Text = objScopeItem.Unit
            Else
                ddlMeasurement.SelectedValue = defaultMeasurement
                tbxMeasurement.Text = defaultMeasurement
            End If

            ddlStatus.SelectedValue = 2
            lblTitle.Text = "Add Worksheet Item"
            btnSave.Visible = True
            btnQuickSave.Visible = False
            btnClose.Visible = True
            btnQuickCancel.Visible = False
            btnDelete.Visible = False
            btnQuickDelete.Visible = False
        End If

        ddlMeasurement.Attributes.Add("onchange", String.Format("updateTextControl(this.options[this.selectedIndex].innerHTML,'{0}');", tbxMeasurement.ClientID))
        ddlServiceGroup.Attributes.Add("onchange", String.Format("updateServiceRateControl(this.selectedIndex,'{0}','{1}','{2}');", tbxServiceGroup.ClientID, ddlServicesByGroup.ClientID, tbxRate.ClientID))
        ddlServicesByGroup.Attributes.Add("onchange", String.Format("updateServiceRates(this.selectedIndex,'{0}','{1}','{2}');", tbxService.ClientID, tbxServiceGroup.ClientID, tbxRate.ClientID))
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, btnQuickSave.Click
        SaveScopeItem()
    End Sub

    Private Sub SaveScopeItem()
        Dim returnUrl As String = Request.UrlReferrer.AbsoluteUri
        If returnUrl.IndexOf("&msg=") >= 0 Then
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("&msg="))
        End If
        If returnUrl.IndexOf("msg=") >= 0 Then
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("msg="))
        End If
        If m_ScopeItemId > 0 Then
            Dim objScopeItem As ScopeItem
            objScopeItem = m_ScopeService.GetScopeItemByScopeItemId(m_ScopeItemId)
            If ddlGroup.SelectedValue <> "0" Then
                objScopeItem.ScopeGroup = ddlGroup.SelectedItem.Text
                objScopeItem.ScopeGroupId = ddlGroup.SelectedValue
            Else
                objScopeItem.ScopeGroup = Nothing
                objScopeItem.ScopeGroupId = 0
            End If
            objScopeItem.Area = txtArea.Text.Trim
            objScopeItem.AreaMeasurement = tbxRM.Text.Trim
            objScopeItem.Item = tbxItem.Text.Trim
            objScopeItem.WorksheetService = txtService.Text.Trim
            objScopeItem.AssignTo = tbxAssign.Text.Trim
            If ddlAssign.SelectedValue <> "None" Then
                objScopeItem.AssignToId = ddlAssign.SelectedValue
            Else
                objScopeItem.AssignToId = Nothing
            End If
            objScopeItem.Service = tbxService.Text.Trim
            objScopeItem.Description = tbxDescription.Text.Trim
            objScopeItem.ScopeItemStatusId = ddlStatus.SelectedValue
            Dim decQuan As Decimal = 0
            Decimal.TryParse(tbxQTY.Text.Trim, decQuan)
            objScopeItem.Quantity = decQuan
            'If ddlMeasurement.Items.Count > 0 Then
            '    objScopeItem.Unit = ddlMeasurement.SelectedItem.Text
            'End If
            objScopeItem.Unit = tbxMeasurement.Text
            Dim decRate As Decimal = 0
            Decimal.TryParse(Replace(tbxRate.Text.Trim, "$", ""), decRate)
            objScopeItem.Rate = decRate
            objScopeItem.Cost = objScopeItem.Quantity * objScopeItem.Rate
            Dim intServiceGroupID As Integer = 0
            Integer.TryParse(tbxServiceGroup.Text, intServiceGroupID)
            objScopeItem.ServiceGroupId = intServiceGroupID
            Dim intDisplayOrder As Integer = 0
            If tbxDisplayOrder.Text <> "" Then
                Integer.TryParse(tbxDisplayOrder.Text, intDisplayOrder)
            End If
            objScopeItem.DisplayOrder = intDisplayOrder
            Session("ScopeItemDuplication") = objScopeItem
            m_ScopeService.UpdateScopeItem(objScopeItem)
            m_ScopeService.UpdateTotal(objScopeItem.ScopeId)
            Response.Redirect(String.Format("../Projects/Detail.aspx?id={0}&sid={1}&msg=The Worksheet item is updated.", m_Cryption.Encrypt(objScopeItem.ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(m_ScopeId, m_Cryption.cryptionKey)))
        Else
            Dim objScopeItem As New ScopeItem
            If ddlGroup.SelectedValue <> "0" Then
                objScopeItem.ScopeGroup = ddlGroup.SelectedItem.Text
                objScopeItem.ScopeGroupId = ddlGroup.SelectedValue
            Else
                objScopeItem.ScopeGroup = Nothing
                objScopeItem.ScopeGroupId = 0
            End If
            objScopeItem.Area = txtArea.Text.Trim
            objScopeItem.AreaMeasurement = tbxRM.Text.Trim
            objScopeItem.Item = tbxItem.Text.Trim
            objScopeItem.WorksheetService = txtService.Text.Trim
            objScopeItem.AssignTo = tbxAssign.Text.Trim
            If ddlAssign.SelectedValue <> "None" Then
                objScopeItem.AssignToId = ddlAssign.SelectedValue
            End If
            objScopeItem.Service = tbxService.Text.Trim
            objScopeItem.Description = tbxDescription.Text.Trim
            objScopeItem.ScopeItemStatusId = ddlStatus.SelectedValue
            Dim decQuan As Decimal = 0
            Decimal.TryParse(tbxQTY.Text.Trim, decQuan)
            objScopeItem.Quantity = decQuan
            'objScopeItem.Unit = ddlMeasurement.SelectedItem.Text
            objScopeItem.Unit = tbxMeasurement.Text
            Dim decRate As Decimal = 0
            Decimal.TryParse(Replace(tbxRate.Text.Trim, "$", ""), decRate)
            objScopeItem.Rate = decRate
            objScopeItem.ScopeId = m_ScopeId
            objScopeItem.Cost = objScopeItem.Quantity * objScopeItem.Rate
            Dim intServiceGroupID As Integer = 0
            Integer.TryParse(tbxServiceGroup.Text, intServiceGroupID)
            objScopeItem.ServiceGroupId = intServiceGroupID
            Dim intDisplayOrder As Integer = 0
            If tbxDisplayOrder.Text <> "" Then
                Integer.TryParse(tbxDisplayOrder.Text, intDisplayOrder)
            End If
            objScopeItem.DisplayOrder = intDisplayOrder
            Session("ScopeItemDuplication") = objScopeItem
            m_ScopeService.CreateScopeItem(objScopeItem)
            m_ScopeService.UpdateTotal(objScopeItem.ScopeId)
            Response.Redirect(String.Format("../Projects/Detail.aspx?id={0}&sid={1}&msg=The Worksheet item is added.", m_Cryption.Encrypt(m_ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(m_ScopeId, m_Cryption.cryptionKey)))
        End If
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click, btnQuickDelete.Click
        Dim objScopeItem As ScopeItem
        objScopeItem = m_ScopeService.GetScopeItemByScopeItemId(m_ScopeItemId)
        m_ScopeService.DeleteScopeItemByScopeItemId(m_ScopeItemId)
        m_ScopeService.UpdateTotal(objScopeItem.ScopeId)
        Response.Redirect(String.Format("../Projects/Detail.aspx?id={0}&sid={1}&msg=The Worksheet item is deleted.", m_Cryption.Encrypt(objScopeItem.ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(m_ScopeId, m_Cryption.cryptionKey)))
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click, btnQuickCancel.Click
        Dim objScope As Scope
        objScope = m_ScopeService.GetScopeByScopeId(m_ScopeId)
        Response.Redirect(String.Format("../Projects/Detail.aspx?id={0}&sid={1}", m_Cryption.Encrypt(objScope.ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(m_ScopeId, m_Cryption.cryptionKey)))
    End Sub

    Private Sub LoadMeasurement(ByVal ddlServiceIndex As Integer, ByVal ddlServiceValue As String)
        'If ddlMeasurement.Items.Count > 0 Then
        '    ddlMeasurement.Items.Clear()
        'End If
        Dim blnUseStandard As Boolean = False
        If ddlServiceIndex > 0 Then
            Dim dsServiceRate As DataSet = New DataSet()
            dsServiceRate = m_ScopeService.GetServiceRatesByServiceId(CLng(ddlServiceValue))
            If dsServiceRate.Tables.Count > 0 Then
                If dsServiceRate.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsServiceRate.Tables(0).Rows
                        'Dim liListItem As New ListItem
                        'liListItem.Text = String.Format("{0}", dr("Unit"))
                        ''liListItem.Value = String.Format("{0}", dr("Unit"))
                        'liListItem.Value = CDec(dr("ChargeRate")).ToString("c")
                        'liListItem.Attributes.Add("onclick", "updateServiceRate('" & CDec(dr("ChargeRate")).ToString("c") & "');")
                        'ddlMeasurement.Items.Add(liListItem)
                        tbxRate.Text = CDec(dr("ChargeRate")).ToString("c")
                    Next
                Else
                    blnUseStandard = True
                End If
            Else
                blnUseStandard = True
            End If
        Else
            blnUseStandard = True
        End If
        'If blnUseStandard Then
        '    Dim dsUnits As DataSet
        '    dsUnits = m_ScopeService.GetUnits()
        '    If dsUnits.Tables.Count > 0 Then
        '        If dsUnits.Tables(0).Rows.Count > 0 Then
        '            For Each dr In dsUnits.Tables(0).Rows
        '                ddlMeasurement.Items.Add(New ListItem(dr("Name"), dr("Name")))
        '            Next
        '        End If
        '    End If
        '    'ddlMeasurement.Items.Add(New ListItem("hr", "hr"))
        '    'ddlMeasurement.Items.Add(New ListItem("m", "m"))
        '    'ddlMeasurement.Items.Add(New ListItem("m2", "m2"))
        '    'ddlMeasurement.Items.Add(New ListItem("LM", "LM"))
        'End If
    End Sub

    Private Sub LoadServiceDescription(ByVal ddlServiceIndex As Integer, ByVal ddlServiceValue As String)
        Dim objScopeItem As ScopeItem
        objScopeItem = m_ScopeService.GetScopeItemByScopeItemId(m_ScopeItemId)

        If ddlServiceIndex > 0 Then
            Dim dsService As DataSet = New DataSet()
            If ddlServiceIndex > 0 Then
                dsService = m_ScopeService.GetServiceByServiceId(CLng(ddlServiceValue))
                If dsService.Tables.Count > 0 Then
                    If dsService.Tables(0).Rows.Count > 0 Then
                        If objScopeItem.ServiceGroupId = 0 And objScopeItem.Service Is Nothing And Not objScopeItem.Description Is Nothing Then
                            If String.Format("{0}", dsService.Tables(0).Rows(0)("Description")) <> "" Then
                                tbxDescription.Text = String.Format("{0} - {1}", objScopeItem.Description, dsService.Tables(0).Rows(0)("Description"))
                            Else
                                tbxDescription.Text = String.Format("{0}", objScopeItem.Description)
                            End If
                        Else
                            tbxDescription.Text = String.Format("{0}", dsService.Tables(0).Rows(0)("Description"))
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub ddlAssign_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAssign.SelectedIndexChanged
        tbxAssign.Text = ddlAssign.SelectedItem.Text
        Dim dsServiceGroup As DataSet = New DataSet()
        If ddlAssign.SelectedIndex > 0 Then
            Dim ProjectOwnerId_AssignTo As Long
            ProjectOwnerId_AssignTo = m_ManagementService.GetProjectOwnerByContactId(ddlAssign.SelectedValue).ProjectOwnerId
            'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_LoginUser.UserId, ProjectOwnerId_AssignTo)
            'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_ProjectOwnerUserId, ProjectOwnerId_AssignTo)
            dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerIdLoginUserId(m_ProjectOwnerUserId, ProjectOwnerId_AssignTo, m_LoginUser.UserId)
            If dsServiceGroup.Tables(0).Rows.Count = 0 Then
                'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_LoginUser.UserId, 0)
                'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_ProjectOwnerUserId, 0)
                dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerIdLoginUserId(m_ProjectOwnerUserId, 0, m_LoginUser.UserId)
            End If
        Else
            'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_LoginUser.UserId, 0)
            'dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerId(m_ProjectOwnerUserId, 0)
            dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerIdLoginUserId(m_ProjectOwnerUserId, 0, m_LoginUser.UserId)
        End If
        
        If ddlServiceGroup.Items.Count > 0 Then
            ddlServiceGroup.Items.Clear()
        End If
        ddlServiceGroup.Items.Add(New ListItem("None", "0"))
        If dsServiceGroup.Tables.Count > 0 Then
            If dsServiceGroup.Tables(0).Rows.Count > 0 Then
                For Each dr In dsServiceGroup.Tables(0).Rows
                    Dim liListItem As New ListItem
                    liListItem.Text = String.Format("{0}", dr("Name"))
                    liListItem.Value = dr("ServiceGroupId")
                    ddlServiceGroup.Items.Add(liListItem)
                Next
            End If
        End If
    End Sub

    Private Sub LoadWorksheetMeasurement()
        If ddlMeasurement.Items.Count > 0 Then
            ddlMeasurement.Items.Clear()
        End If
        
        If ddlService.SelectedValue <> "" And ddlService.SelectedValue <> "None" Then
            Dim objWorksheetService As WorksheetService = m_ScopeService.GetWorksheetServiceByWorksheetServiceId(ddlService.SelectedValue)
            If Not objWorksheetService.Unit Is Nothing Then
                ddlMeasurement.Items.Add(New ListItem(objWorksheetService.Unit, objWorksheetService.Unit))
            End If
        Else
           Dim dsUnits As DataSet
            dsUnits = m_ScopeService.GetUnits()
            If dsUnits.Tables.Count > 0 Then
                If dsUnits.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsUnits.Tables(0).Rows
                        ddlMeasurement.Items.Add(New ListItem(dr("Name"), dr("Name")))
                    Next
                End If
            End If
        End If
    End Sub

    Private Sub LoadWorksheetDescription()
        Dim objScopeItem As ScopeItem
        objScopeItem = m_ScopeService.GetScopeItemByScopeItemId(m_ScopeItemId)

        If ddlService.SelectedValue <> "" And ddlService.SelectedValue <> "None" Then
            Dim objWorksheetService As WorksheetService = m_ScopeService.GetWorksheetServiceByWorksheetServiceId(ddlService.SelectedValue)
            If objScopeItem.ServiceGroupId = 0 And objScopeItem.Service Is Nothing And Not objScopeItem.Description Is Nothing Then
                If Not objWorksheetService.Description Is Nothing Then
                    tbxDescription.Text = String.Format("{0} - {1}", objScopeItem.Description, objWorksheetService.Description)
                Else
                    tbxDescription.Text = String.Format("{0}", objScopeItem.Description)
                End If
            Else
                tbxDescription.Text = String.Format("{0}", objWorksheetService.Description)
            End If
        End If
    End Sub

    Public Function loadWorksheetServices() As String
        Dim aryWorksheetServices As String = "var myWorksheetServices = new Array();"
        Dim dsScopeService As DataSet = New DataSet()
        Dim xIndex As Integer = 0
        dsScopeService = m_ScopeService.GetWorksheetServicesByProjectOwnerId(m_ProjectOwnerId)
        If dsScopeService.Tables.Count > 0 Then
            If dsScopeService.Tables(0).Rows.Count > 0 Then
                For Each dr In dsScopeService.Tables(0).Rows
                    aryWorksheetServices = String.Format("{0}myWorksheetServices[{1}]=new Array(4);", aryWorksheetServices, xIndex)
                    aryWorksheetServices = String.Format("{0}myWorksheetServices[{1}][0]='{2}';", aryWorksheetServices, xIndex, dr("WorksheetServiceId"))
                    aryWorksheetServices = String.Format("{0}myWorksheetServices[{1}][1]='{2}';", aryWorksheetServices, xIndex, Replace(dr("Name").ToString.Trim, "'", "\'"))
                    aryWorksheetServices = String.Format("{0}myWorksheetServices[{1}][2]='{2}';", aryWorksheetServices, xIndex, Replace(dr("Unit").ToString.Trim, "'", "\'"))
                    aryWorksheetServices = String.Format("{0}myWorksheetServices[{1}][3]='{2}';", aryWorksheetServices, xIndex, Replace(dr("Description").ToString.Trim, "'", "\'"))
                    xIndex += 1
                Next
            End If
        End If
        Return aryWorksheetServices
    End Function

    Public Function loadMeasurements() As String
        Dim aryMeasurements As String = "var myMeasurements = new Array();"
        Dim dsUnits As DataSet
        Dim xIndex As Integer = 0
        dsUnits = m_ScopeService.GetUnits()
        If dsUnits.Tables.Count > 0 Then
            If dsUnits.Tables(0).Rows.Count > 0 Then
                For Each dr In dsUnits.Tables(0).Rows
                    aryMeasurements = String.Format("{0}myMeasurements[{1}]='{2}';", aryMeasurements, xIndex, Replace(dr("Name").ToString.Trim, "'", "\'"))
                    xIndex += 1
                Next
            End If
        End If
        Return aryMeasurements
    End Function

    Public Function loadServiceRates() As String
        Dim aryServiceRates As String = "var myServiceRates = new Array();"
        Dim dsServiceGroup As DataSet = New DataSet()
        Dim xIndex As Integer = 0
        Dim yIndex As Integer = 0
        dsServiceGroup = m_ScopeService.GetServiceGroupsByUserIdProjectOwnerIdLoginUserId(m_ProjectOwnerUserId, 0, m_LoginUser.UserId)
        If dsServiceGroup.Tables.Count > 0 Then
            If dsServiceGroup.Tables(0).Rows.Count > 0 Then
                For Each dr In dsServiceGroup.Tables(0).Rows
                    Dim dsService As DataSet = New DataSet()
                    dsService = m_ScopeService.GetServicesByServiceGroupId(dr("ServiceGroupId"))
                    If dsService.Tables.Count > 0 Then
                        If dsService.Tables(0).Rows.Count > 0 Then
                            aryServiceRates = String.Format("{0}myServiceRates[{1}]=new Array({2});", aryServiceRates, xIndex, dsService.Tables(0).Rows.Count)
                            For Each sdr In dsService.Tables(0).Rows
                                Dim decRate As Decimal
                                Decimal.TryParse(sdr("Rate"), decRate)
                                aryServiceRates = String.Format("{0}myServiceRates[{1}][{2}]='{3}';", aryServiceRates, xIndex, yIndex, String.Format("{0};{1};{2};{3}", sdr("ServiceId"), Replace(sdr("Name").ToString.Trim, "'", "\'"), decRate.ToString("c"), dr("ServiceGroupId")))
                                yIndex += 1
                            Next
                            yIndex = 0
                        End If
                    End If
                    xIndex += 1
                Next
            End If
        End If
        Return aryServiceRates
    End Function
End Class
