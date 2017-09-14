Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities


Partial Class Projects_Edit
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Private m_ProjectId As Long
    Private m_Cryption As New Cryption

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
            'If m_LoginUser.Type = 0 Then
            '    btnDelete.Visible = False
            'End If
            If m_LoginUser.CompanyId = 0 Then
                Response.Redirect("../Contacts/ProjectOwnerDetail.aspx?msg=Please fill your business details!")
            End If
        End If
        If Request.QueryString("id") <> String.Empty Then
            Long.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ProjectId)
        Else
            m_ProjectId = 0
            'm_ProjectId = 1 ' use for debug set id to 1
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        If m_ProjectId = 0 Then
            If Not Session("ScopeItemDuplication") Is Nothing Then
                Session("ScopeItemDuplication") = Nothing
            End If

            ' Check Project Credit
            'Dim projectCredit As Integer = 0
            'projectCredit = GetProjectCreditBalance()
            'If projectCredit = 0 Then
            '    Session("SelecrPlanReferrer") = Request.Url.ToString()
            '    Response.Redirect("../accounts/SelectPlan.aspx")
            'End If
            'Check Monthly Billing
            If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
                Response.Redirect("../accounts/plans.aspx")
            Else
                Dim ProjectOwnerId As Long
                ProjectOwnerId = m_ManagementService.GetProjectOwnerByContactId(m_LoginUser.UserId).ProjectOwnerId
                'Dim dsExistedProjects As DataSet = m_ManagementService.GetProjectsByProjectOwnerId(ProjectOwnerId)
                'Dim ExistedProjectsCount As Integer = 0
                'If dsExistedProjects.Tables.Count > 0 Then
                '    ExistedProjectsCount = dsExistedProjects.Tables(0).Rows.Count
                'End If
                Dim UnarchivedProjectsCount As Integer = m_ManagementService.GetUnarchivedProjectsCountByUserId(m_LoginUser.UserId)
                Dim dsUserAccount As DataSet = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
                Dim AllowedProjectsCount As Integer = 0
                If dsUserAccount.Tables.Count > 0 Then
                    If dsUserAccount.Tables(0).Rows.Count > 0 Then
                        If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("NumberOfProjects")) Then
                            AllowedProjectsCount = dsUserAccount.Tables(0).Rows(0)("NumberOfProjects")
                        End If
                    End If
                End If
                If AllowedProjectsCount <= UnarchivedProjectsCount Then
                    Response.Redirect("../accounts/plans.aspx")
                End If
            End If
        Else
            If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
                btnSave.Visible = False
            End If
        End If

        If Not IsPostBack Then
            Dim dsProjectStatus As DataSet = New DataSet()
            'dsProjectStatus = m_ManagementService.GetProjectStatuses()
            dsProjectStatus = m_ManagementService.GetProjectStatusesByProjectIdUserId(m_ProjectId, m_LoginUser.UserId)
            ddlProjectStatus.DataSource = dsProjectStatus.Tables(0)
            ddlProjectStatus.DataTextField = "Name"
            'ddlProjectStatus.DataValueField = "ProjectStatusId"
            ddlProjectStatus.DataValueField = "StatusValue"
            ddlProjectStatus.DataBind()

            Dim dsProjectGroup As DataSet = New DataSet()
            dsProjectGroup = m_ScopeService.GetProjectGroupsByProjectOwnerId(m_LoginUser.CompanyId)
            ddlGroup.Items.Add(New ListItem("Select", "0"))
            If dsProjectGroup.Tables.Count > 0 Then
                If dsProjectGroup.Tables(0).Rows.Count > 0 Then
                    For Each dr In dsProjectGroup.Tables(0).Rows
                        Dim liListItem As New ListItem
                        liListItem.Text = dr("Name")
                        liListItem.Value = dr("ProjectGroupId")
                        'liListItem.Attributes.Add("onclick", "updateItem('" & dr("Name") & "');")
                        ddlGroup.Items.Add(liListItem)
                    Next
                End If
            End If

            Dim index As Integer
            ddlScopeDateDay.Items.Add(New ListItem("Day", "0"))
            ddlStartDateDay.Items.Add(New ListItem("Day", "0"))
            ddlAssessmentDateDay.Items.Add(New ListItem("Day", "0"))
            ddlQuotationDateDay.Items.Add(New ListItem("Day", "0"))
            ddlFinishDateDay.Items.Add(New ListItem("Day", "0"))
            For index = 1 To 31
                If index < 10 Then
                    ddlScopeDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("0{0}", index)))
                    ddlStartDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("0{0}", index)))
                    ddlAssessmentDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("0{0}", index)))
                    ddlQuotationDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("0{0}", index)))
                    ddlFinishDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("0{0}", index)))
                Else
                    ddlScopeDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                    ddlStartDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                    ddlAssessmentDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                    ddlQuotationDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                    ddlFinishDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                End If
            Next

            ddlScopeDateMonth.Items.Add(New ListItem("Month", "0"))
            ddlStartDateMonth.Items.Add(New ListItem("Month", "0"))
            ddlAssessmentDateMonth.Items.Add(New ListItem("Month", "0"))
            ddlQuotationDateMonth.Items.Add(New ListItem("Month", "0"))
            ddlFinishDateMonth.Items.Add(New ListItem("Month", "0"))
            For index = 1 To 12
                If index < 10 Then
                    ddlScopeDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("0{0}", index)))
                    ddlStartDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("0{0}", index)))
                    ddlAssessmentDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("0{0}", index)))
                    ddlQuotationDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("0{0}", index)))
                    ddlFinishDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("0{0}", index)))
                Else
                    ddlScopeDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("{0}", index)))
                    ddlStartDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("{0}", index)))
                    ddlAssessmentDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("{0}", index)))
                    ddlQuotationDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("{0}", index)))
                    ddlFinishDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("{0}", index)))
                End If
            Next

            ddlScopeDateYear.Items.Add(New ListItem("Year", "0"))
            ddlStartDateYear.Items.Add(New ListItem("Year", "0"))
            ddlAssessmentDateYear.Items.Add(New ListItem("Year", "0"))
            ddlQuotationDateYear.Items.Add(New ListItem("Year", "0"))
            ddlFinishDateYear.Items.Add(New ListItem("Year", "0"))
            Dim intYear As Integer
            For index = -1 To 5
                intYear = Year(Now) + index
                ddlScopeDateYear.Items.Add(New ListItem(String.Format("{0}", intYear), String.Format("{0}", intYear)))
                ddlStartDateYear.Items.Add(New ListItem(String.Format("{0}", intYear), String.Format("{0}", intYear)))
                ddlAssessmentDateYear.Items.Add(New ListItem(String.Format("{0}", intYear), String.Format("{0}", intYear)))
                ddlQuotationDateYear.Items.Add(New ListItem(String.Format("{0}", intYear), String.Format("{0}", intYear)))
                ddlFinishDateYear.Items.Add(New ListItem(String.Format("{0}", intYear), String.Format("{0}", intYear)))
            Next

            If m_ProjectId = 0 Then
                btnDelete.Visible = False
                'ddlStartDateDay.SelectedIndex = Today.Day
                'ddlStartDateMonth.SelectedIndex = Today.Month
                'ddlStartDateYear.SelectedValue = Today.Year
            End If
            LoadProfile(m_ProjectId)
            'If m_PatientId > 0 Then
            '    'txtEmail.ReadOnly = True
            '    rfvEmail.Enabled = False
            '    btnDelete.Enabled = True
            'End If
        End If

        txtProjectName.Focus()
    End Sub

    Private Sub LoadProfile(ByVal ProjectId As Long)
        If ProjectId > 0 Then
            lblTitle.Text = "Edit Project"
            Dim dsProject As New DataSet
            ''dsPatientProfile = m_ManagementService.GetProjectInfoByUserId(UserId)
            dsProject = m_ManagementService.GetProjectInfoByProjectId(ProjectId)
            If dsProject.Tables.Count > 0 Then
                If dsProject.Tables(0).Rows.Count = 1 Then
                    Dim ContactProfile As UserProfile = New UserProfile()
                    ContactProfile = m_ManagementService.GetUserProfileByUserID(dsProject.Tables(0).Rows(0).Item("ContactId"))
                    txtFirstName.Text = ContactProfile.FirstName
                    txtLastName.Text = ContactProfile.LastName
                    txtHomePhone.Text = ContactProfile.Contact1
                    txtWorkPhone.Text = ContactProfile.Contact2
                    txtMobile.Text = ContactProfile.Contact3
                    txtEmail.Text = ContactProfile.Email

                    txtAddress.Text = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("Address"))
                    CascadingDropDown1.SelectedValue = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("CountryID"))
                    CascadingDropDown2.SelectedValue = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("RegionID"))
                    CascadingDropDown3.SelectedValue = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("CityID"))
                    CascadingDropDown4.SelectedValue = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("SuburbID"))


                    'txtSuburb.Text = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("Suburb"))
                    'txtDistrict.Text = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("City"))
                    'txtRegion.Text = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("Region"))

                    txtProjectName.Text = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("Name"))
                    txtEQCClaimNumber.Text = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("EQCClaimNumber"))
                    If Not IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
                        Dim dtScopeDate As DateTime
                        dtScopeDate = CType(dsProject.Tables(0).Rows(0).Item("ScopeDate"), DateTime)
                        ddlScopeDateDay.SelectedValue = dtScopeDate.ToString("dd")
                        ddlScopeDateMonth.SelectedValue = dtScopeDate.ToString("MM")
                        ddlScopeDateYear.SelectedValue = dtScopeDate.ToString("yyyy")
                    End If
                    If Not IsDBNull(dsProject.Tables(0).Rows(0).Item("StartDate")) Then
                        Dim dtStartDate As DateTime
                        dtStartDate = CType(dsProject.Tables(0).Rows(0).Item("StartDate"), DateTime)
                        ddlStartDateDay.SelectedValue = dtStartDate.ToString("dd")
                        ddlStartDateMonth.SelectedValue = dtStartDate.ToString("MM")
                        ddlStartDateYear.SelectedValue = dtStartDate.ToString("yyyy")
                    End If
                    If Not IsDBNull(dsProject.Tables(0).Rows(0).Item("AssessmentDate")) Then                        
                        Dim dtAssessmentDate As DateTime
                        dtAssessmentDate = CType(dsProject.Tables(0).Rows(0).Item("AssessmentDate"), DateTime)
                        ddlAssessmentDateDay.SelectedValue = dtAssessmentDate.ToString("dd")
                        ddlAssessmentDateMonth.SelectedValue = dtAssessmentDate.ToString("MM")
                        ddlAssessmentDateYear.SelectedValue = dtAssessmentDate.ToString("yyyy")
                    End If
                    If Not IsDBNull(dsProject.Tables(0).Rows(0).Item("QuotationDate")) Then
                        Dim dtQuotationDate As DateTime
                        dtQuotationDate = CType(dsProject.Tables(0).Rows(0).Item("QuotationDate"), DateTime)
                        ddlQuotationDateDay.SelectedValue = dtQuotationDate.ToString("dd")
                        ddlQuotationDateMonth.SelectedValue = dtQuotationDate.ToString("MM")
                        ddlQuotationDateYear.SelectedValue = dtQuotationDate.ToString("yyyy")
                    End If
                    If Not IsDBNull(dsProject.Tables(0).Rows(0).Item("FinishDate")) Then
                        Dim dtFinishDate As DateTime
                        dtFinishDate = CType(dsProject.Tables(0).Rows(0).Item("FinishDate"), DateTime)
                        ddlFinishDateDay.SelectedValue = dtFinishDate.ToString("dd")
                        ddlFinishDateMonth.SelectedValue = dtFinishDate.ToString("MM")
                        ddlFinishDateYear.SelectedValue = dtFinishDate.ToString("yyyy")
                    End If
                    If Not IsDBNull(dsProject.Tables(0).Rows(0).Item("Priority")) Then
                        PriorityRating.CurrentRating = dsProject.Tables(0).Rows(0).Item("Priority")
                    End If
                    txtHazard.Text = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("Hazard"))
                    txtEstimatedTime.Text = String.Format("{0}", dsProject.Tables(0).Rows(0).Item("EstimatedTime"))
                    'ddlProjectStatus.SelectedValue = dsProject.Tables(0).Rows(0).Item("ProjectStatusId")
                    Dim dsUserProjectStatus As DataSet
                    dsUserProjectStatus = m_ManagementService.GetUserProjectStatusValueByProjectIdUserId(m_ProjectId, m_LoginUser.UserId)
                    If dsUserProjectStatus.Tables.Count > 0 Then
                        If dsUserProjectStatus.Tables(0).Rows.Count > 0 Then
                            If Not IsDBNull(dsUserProjectStatus.Tables(0).Rows(0)("UserProjectStatusValue")) Then
                                ddlProjectStatus.SelectedValue = dsUserProjectStatus.Tables(0).Rows(0)("UserProjectStatusValue")
                            End If
                        End If
                    End If
                    If Not IsDBNull(dsProject.Tables(0).Rows(0).Item("GroupId")) Then
                        ddlGroup.SelectedValue = dsProject.Tables(0).Rows(0).Item("GroupId")
                    End If
                    'tbPersonalPhoto.Text = String.Format("{0}", dsPatientProfile.Tables(0).Rows(0).Item("PersonalPhoto"))
                End If
            End If
        Else
            lblTitle.Text = "Add Project"
            ' Default location to New Zealand, Canterbury, Christchurch
            CascadingDropDown1.SelectedValue = "554"
            CascadingDropDown2.SelectedValue = "14"
            CascadingDropDown3.SelectedValue = "57"

            ' Set Scope, Start, End Dates if available
            Dim dtStart As DateTime
            Dim dtEnd As DateTime
            If Not Request.QueryString("start") Is Nothing Then
                If DateTime.TryParse(Request.QueryString("start"), dtStart) Then
                    ddlScopeDateDay.SelectedValue = dtStart.ToString("dd")
                    ddlScopeDateMonth.SelectedValue = dtStart.ToString("MM")
                    ddlScopeDateYear.SelectedValue = dtStart.ToString("yyyy")

                    ddlStartDateDay.SelectedValue = dtStart.ToString("dd")
                    ddlStartDateMonth.SelectedValue = dtStart.ToString("MM")
                    ddlStartDateYear.SelectedValue = dtStart.ToString("yyyy")
                End If
            End If
            If Not Request.QueryString("end") Is Nothing Then
                If DateTime.TryParse(Request.QueryString("end"), dtEnd) Then
                    ddlFinishDateDay.SelectedValue = dtEnd.ToString("dd")
                    ddlFinishDateMonth.SelectedValue = dtEnd.ToString("MM")
                    ddlFinishDateYear.SelectedValue = dtEnd.ToString("yyyy")
                End If
            End If
            If dtStart = dtEnd Then
                ddlStartDateDay.SelectedIndex = 0
                ddlStartDateMonth.SelectedIndex = 0
                ddlStartDateYear.SelectedIndex = 0
                ddlFinishDateDay.SelectedIndex = 0
                ddlFinishDateMonth.SelectedIndex = 0
                ddlFinishDateYear.SelectedIndex = 0
            End If

            If ddlStartDateDay.SelectedIndex > 0 And ddlStartDateMonth.SelectedIndex > 0 And ddlStartDateYear.SelectedIndex > 0 And _
                ddlFinishDateDay.SelectedIndex > 0 And ddlFinishDateMonth.SelectedIndex > 0 And ddlFinishDateYear.SelectedIndex > 0 Then
                If ddlStartDateDay.SelectedIndex <> ddlFinishDateDay.SelectedIndex Or _
                    ddlStartDateMonth.SelectedIndex <> ddlFinishDateMonth.SelectedIndex Or _
                    ddlStartDateYear.SelectedIndex <> ddlFinishDateYear.SelectedIndex Then
                    ' If Start date and finish date are not same, set status default to New Project 
                    For index As Integer = 0 To ddlProjectStatus.Items.Count - 1
                        If ddlProjectStatus.Items(index).Value = -100 Then
                            ddlProjectStatus.SelectedIndex = index
                        End If
                    Next
                End If
            End If
            If Not Request.QueryString("title") Is Nothing Then
                txtProjectName.Text = Request.QueryString("title")
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        SaveProfile()
    End Sub

    Private Sub SaveProfile()
        Dim returnUrl As String = Request.UrlReferrer.AbsoluteUri
        If returnUrl.IndexOf("&msg=") >= 0 Then
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("&msg="))
        End If
        If returnUrl.IndexOf("msg=") >= 0 Then
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("msg="))
        End If
        Dim ContactProfile As UserProfile = New UserProfile
        Dim Project As Project = New Project
        If m_ProjectId > 0 Then
            Project = m_ManagementService.GetProjectByProjectId(m_ProjectId)
            ContactProfile = m_ManagementService.GetUserProfileByUserID(Project.ContactId)
            ContactProfile.FirstName = txtFirstName.Text.Trim()
            ContactProfile.LastName = txtLastName.Text.Trim()
            ContactProfile.Contact1 = txtHomePhone.Text.Trim()
            ContactProfile.Contact2 = txtWorkPhone.Text.Trim()
            ContactProfile.Contact3 = txtMobile.Text.Trim()
            ContactProfile.Email = txtEmail.Text.Trim()
            Dim NewFile As String = UploadImages(Project.ContactId, ContactProfile.Identifier)
            If NewFile <> String.Empty Then
                ContactProfile.PersonalPhoto = NewFile
            Else
                'patientProfile.PersonalPhoto = tbPersonalPhoto.Text
            End If

            m_ManagementService.UpdateUserProfile(ContactProfile)

            Project.Address = txtAddress.Text.Trim()
            Project.Suburb = txtSuburb.SelectedItem.Text
            If txtSuburb.SelectedItem.Value <> String.Empty Then
                Project.SuburbID = txtSuburb.SelectedItem.Value
            End If
            Project.City = txtDistrict.SelectedItem.Text
            If txtDistrict.SelectedItem.Value <> String.Empty Then
                Project.CityID = txtDistrict.SelectedItem.Value
            End If
            Project.Region = txtRegion.SelectedItem.Text
            If txtRegion.SelectedItem.Value <> String.Empty Then
                Project.RegionID = txtRegion.SelectedItem.Value
            End If
            Project.Country = ddlPAC.SelectedItem.Text
            If ddlPAC.SelectedItem.Value <> String.Empty Then
                Project.CountryID = ddlPAC.SelectedItem.Value
            End If            

            Project.Name = txtProjectName.Text.Trim()
            Project.EQCClaimNumber = txtEQCClaimNumber.Text.Trim()
            Project.EstimatedTime = txtEstimatedTime.Text.Trim()
            If ddlScopeDateDay.SelectedIndex < 1 And ddlScopeDateMonth.SelectedIndex < 1 And ddlScopeDateYear.SelectedValue < 1 Then
                Project.ScopeDate = Nothing
            Else
                Project.ScopeDate = CType(String.Format("{0}-{1}-{2}", ddlScopeDateYear.SelectedValue, ddlScopeDateMonth.SelectedValue, ddlScopeDateDay.SelectedValue), DateTime)
            End If
            If ddlStartDateDay.SelectedIndex < 1 And ddlStartDateMonth.SelectedIndex < 1 And ddlStartDateYear.SelectedValue < 1 Then
                Project.StartDate = Nothing
            Else
                Project.StartDate = CType(String.Format("{0}-{1}-{2}", ddlStartDateYear.SelectedValue, ddlStartDateMonth.SelectedValue, ddlStartDateDay.SelectedValue), DateTime)
            End If
            'Project.ProjectStatusId = ddlProjectStatus.SelectedValue
            If ddlGroup.SelectedValue <> "0" Then
                Project.GroupID = ddlGroup.SelectedValue
                Project.GroupName = ddlGroup.SelectedItem.Text
            Else
                Project.GroupID = 0
                Project.GroupName = String.Empty
            End If
            If ddlAssessmentDateDay.SelectedIndex < 1 And ddlAssessmentDateMonth.SelectedIndex < 1 And ddlAssessmentDateYear.SelectedValue < 1 Then
                Project.AssessmentDate = Nothing
            Else
                Project.AssessmentDate = CType(String.Format("{0}-{1}-{2}", ddlAssessmentDateYear.SelectedValue, ddlAssessmentDateMonth.SelectedValue, ddlAssessmentDateDay.SelectedValue), DateTime)
            End If
            If ddlQuotationDateDay.SelectedIndex < 1 And ddlQuotationDateMonth.SelectedIndex < 1 And ddlQuotationDateYear.SelectedValue < 1 Then
                Project.QuotationDate = Nothing
            Else
                Project.QuotationDate = CType(String.Format("{0}-{1}-{2}", ddlQuotationDateYear.SelectedValue, ddlQuotationDateMonth.SelectedValue, ddlQuotationDateDay.SelectedValue), DateTime)
            End If
            If ddlFinishDateDay.SelectedIndex < 1 And ddlFinishDateMonth.SelectedIndex < 1 And ddlFinishDateYear.SelectedValue < 1 Then
                Project.FinishDate = Nothing
            Else
                Project.FinishDate = CType(String.Format("{0}-{1}-{2}", ddlFinishDateYear.SelectedValue, ddlFinishDateMonth.SelectedValue, ddlFinishDateDay.SelectedValue), DateTime)
            End If
            Project.Priority = PriorityRating.CurrentRating
            Project.Hazard = txtHazard.Text.Trim()

            m_ManagementService.UpdateProject(Project)

            Dim UserProjectStatusValue As UserProjectStatusValue = New UserProjectStatusValue
            UserProjectStatusValue.ProjectId = Project.ProjectId
            UserProjectStatusValue.UserId = m_LoginUser.UserId
            UserProjectStatusValue.UserProjectStatusValue = ddlProjectStatus.SelectedValue
            m_ManagementService.UpdateUserProjectStatusValueByProjectIdUserId(UserProjectStatusValue)
            Response.Redirect(String.Format("Detail.aspx?{0}", Request.QueryString))
        Else
            Dim ContactUser As New User
            Dim userId As Long
            ContactUser.Email = txtEmail.Text.Trim()
            ContactUser.Type = 0
            userId = m_ManagementService.CreateUser(ContactUser, m_LoginUser.UserId)

            Dim userProfileId As Long
            ContactProfile.UserId = userId
            ContactProfile.FirstName = txtFirstName.Text.Trim()
            ContactProfile.LastName = txtLastName.Text.Trim()
            ContactProfile.Contact1 = txtHomePhone.Text.Trim()
            ContactProfile.Contact2 = txtWorkPhone.Text.Trim()
            ContactProfile.Contact3 = txtMobile.Text.Trim()
            ContactProfile.Email = txtEmail.Text.Trim()

            userProfileId = m_ManagementService.CreateUserProfile(ContactProfile)
            ContactProfile.UserProfileId = userProfileId

            Dim cVoucherCode As New VoucherCodeFunctions
            Dim strIdentifier As String = String.Format("{0}{1}", userProfileId, cVoucherCode.GenerateVoucherCodeGuid(16))
            ContactProfile.Identifier = strIdentifier
            m_ManagementService.UpdateUserProfileIdentifier(ContactProfile)

            'Dim NewFile As String = UploadImages(userId, ContactProfile.Email)
            Dim NewFile As String = UploadImages(userId, strIdentifier)
            If NewFile <> String.Empty Then
                ContactProfile.PersonalPhoto = NewFile
            Else
                'patientProfile.PersonalPhoto = tbPersonalPhoto.Text
            End If
            m_ManagementService.UpdateUserProfile(ContactProfile)

            Project.ContactId = userId
            Project.ProjectOwnerId = m_ManagementService.GetProjectOwnerByContactId(m_LoginUser.UserId).ProjectOwnerId
            Project.Address = txtAddress.Text.Trim()
            Project.Suburb = txtSuburb.SelectedItem.Text
            If txtSuburb.SelectedItem.Value <> String.Empty Then
                Project.SuburbID = txtSuburb.SelectedItem.Value
            End If
            Project.City = txtDistrict.SelectedItem.Text
            If txtDistrict.SelectedItem.Value <> String.Empty Then
                Project.CityID = txtDistrict.SelectedItem.Value
            End If            
            Project.Region = txtRegion.SelectedItem.Text
            If txtRegion.SelectedItem.Value <> String.Empty Then
                Project.RegionID = txtRegion.SelectedItem.Value
            End If
            Project.Country = ddlPAC.SelectedItem.Text
            If ddlPAC.SelectedItem.Value <> String.Empty Then
                Project.CountryID = ddlPAC.SelectedItem.Value
            End If        

            Project.Name = txtProjectName.Text.Trim()
            Project.EQCClaimNumber = txtEQCClaimNumber.Text.Trim()
            Project.EstimatedTime = txtEstimatedTime.Text.Trim()
            If ddlScopeDateDay.SelectedIndex < 1 And ddlScopeDateMonth.SelectedIndex < 1 And ddlScopeDateYear.SelectedValue < 1 Then
                Project.ScopeDate = Nothing
            Else
                Project.ScopeDate = CType(String.Format("{0}-{1}-{2}", ddlScopeDateYear.SelectedValue, ddlScopeDateMonth.SelectedValue, ddlScopeDateDay.SelectedValue), DateTime)
            End If
            If ddlStartDateDay.SelectedIndex < 1 And ddlStartDateMonth.SelectedIndex < 1 And ddlStartDateYear.SelectedValue < 1 Then
                Project.StartDate = Nothing
            Else
                Project.StartDate = CType(String.Format("{0}-{1}-{2}", ddlStartDateYear.SelectedValue, ddlStartDateMonth.SelectedValue, ddlStartDateDay.SelectedValue), DateTime)
            End If
            'Project.ProjectStatusId = ddlProjectStatus.SelectedValue
            Project.ProjectStatusId = 0
            If ddlGroup.SelectedValue <> "0" Then
                Project.GroupID = ddlGroup.SelectedValue
                Project.GroupName = ddlGroup.SelectedItem.Text
            Else
                Project.GroupID = 0
                Project.GroupName = String.Empty
            End If
            If ddlAssessmentDateDay.SelectedIndex < 1 And ddlAssessmentDateMonth.SelectedIndex < 1 And ddlAssessmentDateYear.SelectedValue < 1 Then
                Project.AssessmentDate = Nothing
            Else
                Project.AssessmentDate = CType(String.Format("{0}-{1}-{2}", ddlAssessmentDateYear.SelectedValue, ddlAssessmentDateMonth.SelectedValue, ddlAssessmentDateDay.SelectedValue), DateTime)
            End If
            If ddlQuotationDateDay.SelectedIndex < 1 And ddlQuotationDateMonth.SelectedIndex < 1 And ddlQuotationDateYear.SelectedValue < 1 Then
                Project.QuotationDate = Nothing
            Else
                Project.QuotationDate = CType(String.Format("{0}-{1}-{2}", ddlQuotationDateYear.SelectedValue, ddlQuotationDateMonth.SelectedValue, ddlQuotationDateDay.SelectedValue), DateTime)
            End If
            If ddlFinishDateDay.SelectedIndex < 1 And ddlFinishDateMonth.SelectedIndex < 1 And ddlFinishDateYear.SelectedValue < 1 Then
                Project.FinishDate = Nothing
            Else
                Project.FinishDate = CType(String.Format("{0}-{1}-{2}", ddlFinishDateYear.SelectedValue, ddlFinishDateMonth.SelectedValue, ddlFinishDateDay.SelectedValue), DateTime)
            End If
            Project.Priority = PriorityRating.CurrentRating
            Project.Hazard = txtHazard.Text.Trim()

            Dim newProjectId As Long
            newProjectId = m_ManagementService.CreateProject(Project)
            Project.ProjectId = newProjectId

            Dim UserProjectStatusValue As UserProjectStatusValue = New UserProjectStatusValue
            UserProjectStatusValue.ProjectId = Project.ProjectId
            UserProjectStatusValue.UserId = m_LoginUser.UserId
            UserProjectStatusValue.UserProjectStatusValue = ddlProjectStatus.SelectedValue
            m_ManagementService.CreateUserProjectStatusValue(UserProjectStatusValue)

            ' Check and update Project Credit            
            Dim projectCredit As Integer = 0
            projectCredit = GetProjectCreditBalance()
            If projectCredit > 0 Then
                m_ManagementService.UpdateUserAccount(m_LoginUser.UserId, projectCredit - 1)
                m_ManagementService.CreateUserTransaction(m_LoginUser.UserId, String.Format("Create Project", Project.Name), 0, 0, -1, projectCredit - 1)
            End If

            'If ddlProjectStatus.SelectedValue = -1000 Then
            '    Response.Redirect("View.aspx?scoped=1")
            'Else
            '    Response.Redirect("View.aspx")
            'End If

            Response.Redirect(String.Format("Detail.aspx?id={0}", m_Cryption.Encrypt(newProjectId.ToString(), m_Cryption.cryptionKey)))
        End If
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim Project As Project = New Project()      
        Project.ProjectId = m_ProjectId
        Project.DeactivatedDate = Today
        m_ManagementService.UpdateProjectDeactivated(Project)
        Response.Redirect("View.aspx")
    End Sub

    Private Function UploadImages(ByVal UserID As Integer, ByVal UserEmail As String) As String
        Dim NewFileName As String = String.Empty
        Try
            If Txt_FileUpload.FileName <> "" Then
                Dim fileName As String
                Dim FileType As String
                Dim namePosition As Int16
                Dim mStream = New MemoryStream()
                Dim strUserFileDescription As String

                Dim limitX As Integer = ConfigurationManager.AppSettings("USERPERSONALWIDTH")
                Dim limitY As Integer = ConfigurationManager.AppSettings("USERPERSONALHEIGHT")
                Dim x As Integer = 0
                Dim y As Integer = 0
                Dim newX As Integer = 0
                Dim newY As Integer = 0

                Dim imgFile As System.Drawing.Image
                imgFile = System.Drawing.Image.FromStream(Txt_FileUpload.PostedFile.InputStream)

                x = imgFile.Width
                y = imgFile.Height

                If (x > limitX) Then
                    newX = limitX
                    newY = CType((y * (limitX * 1.0 / x)), Integer)
                Else
                    newX = x
                    newY = y
                End If

                'If (x > limitX Or y > limitY) Then
                '    If (x * 1.0 / y >= limitX * 1.0 / limitY) Then
                '        newX = limitX
                '        newY = CType((y * (limitX * 1.0 / x)), Integer)
                '    Else
                '        newY = limitY
                '        newX = CType((x * (limitY * 1.0 / y)), Integer)
                '    End If
                'Else
                '    newX = x
                '    newY = y
                'End If
                'stream = Txt_FileUpload.PostedFile.InputStream
                resizeimage(imgFile, newX, newY, mStream)

                Dim uploadedFile(mStream.Length) As Byte
                'mStream.Read(uploadedFile, 0, mStream.Length)
                uploadedFile = mStream.ToArray()
                mStream.Close()
                namePosition = Txt_FileUpload.PostedFile.FileName.LastIndexOf("\") + 1
                fileName = Txt_FileUpload.PostedFile.FileName.Substring(namePosition)
                FileType = Txt_FileUpload.PostedFile.ContentType

                If FileType = "image/gif" Or FileType = "image/jpeg" Or FileType = "image/pjpeg" Or FileType = "image/png" Or FileType = "image/bmp" Then
                    NewFileName = String.Format("{0}", Now.ToString("yyyyMMddHHmmss"))
                    If UserID > 0 Then
                        strUserFileDescription = String.Format("{0}\images\{1}\{2}.jpg", ConfigurationManager.AppSettings("ProjectPath"), UserEmail, NewFileName)
                        If (Not System.IO.Directory.Exists(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), UserEmail))) Then
                            System.IO.Directory.CreateDirectory(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), UserEmail))
                        End If

                        Dim wFile As FileStream
                        wFile = New FileStream(strUserFileDescription, FileMode.Create)
                        wFile.Write(uploadedFile, 0, uploadedFile.Length)
                        wFile.Close()

                        Return NewFileName & ".jpg"
                    End If
                End If
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
        Return NewFileName
    End Function

    Sub resizeimage(ByVal objImage As System.Drawing.Image, ByVal width As Integer, ByVal height As Integer, ByRef imgStream As IO.MemoryStream)
        'Create the delegate
        Dim dummyCallBack As System.Drawing.Image.GetThumbnailImageAbort
        dummyCallBack = New  _
          System.Drawing.Image.GetThumbnailImageAbort(AddressOf ThumbnailCallback)
        Dim thumbNailImg As System.Drawing.Image
        objImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone)
        objImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone)
        thumbNailImg = objImage.GetThumbnailImage(width, height, dummyCallBack, IntPtr.Zero)
        'Response.ContentType = "image/gif"
        If height > Integer.Parse(ConfigurationManager.AppSettings("USERPERSONALHEIGHT")) Then
            Dim cropArea As Rectangle = New Rectangle(0, 0, ConfigurationManager.AppSettings("USERPERSONALWIDTH"), ConfigurationManager.AppSettings("USERPERSONALHEIGHT"))
            Dim bmpImage As Bitmap = New Bitmap(thumbNailImg)
            Dim bmpCrop As Bitmap = bmpImage.Clone(cropArea, bmpImage.PixelFormat)
            Dim cropThumbNail As System.Drawing.Image = CType(bmpCrop, System.Drawing.Image)
            cropThumbNail.Save(imgStream, ImageFormat.Jpeg)
            cropThumbNail.Dispose()
        Else
            thumbNailImg.Save(imgStream, ImageFormat.Jpeg)
        End If
        thumbNailImg.Dispose()
    End Sub

    Function ThumbnailCallback() As Boolean
        Return False
    End Function

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        'Response.Redirect(Request.UrlReferrer.AbsoluteUri)
        If m_ProjectId > 0 Then
            Response.Redirect(String.Format("Detail.aspx?{0}", Request.QueryString))
        Else
            If Request.QueryString("start") Is Nothing Then
                Response.Redirect("View.aspx")
            Else
                Response.Redirect("CalendarView.aspx")
            End If
        End If
    End Sub

    Protected Sub ddlStartDateDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStartDateDay.SelectedIndexChanged
        ValidateStartDate()
    End Sub

    Protected Sub ddlStartDateMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStartDateMonth.SelectedIndexChanged
        ValidateStartDate()
    End Sub

    Protected Sub ddlStartDateYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStartDateYear.SelectedIndexChanged
        ValidateStartDate()
    End Sub

    Protected Sub ddlAssessmentDateDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAssessmentDateDay.SelectedIndexChanged
        ValidateAssessmentDate()
    End Sub

    Protected Sub ddlAssessmentDateMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAssessmentDateMonth.SelectedIndexChanged
        ValidateAssessmentDate()
    End Sub

    Protected Sub ddlAssessmentDateYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAssessmentDateYear.SelectedIndexChanged
        ValidateAssessmentDate()
    End Sub

    Protected Sub ddlQuotationDateDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlQuotationDateDay.SelectedIndexChanged
        ValidateQuotationDate()
    End Sub

    Protected Sub ddlQuotationDateMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlQuotationDateMonth.SelectedIndexChanged
        ValidateQuotationDate()
    End Sub

    Protected Sub ddlQuotationDateYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlQuotationDateYear.SelectedIndexChanged
        ValidateQuotationDate()
    End Sub

    Protected Sub ddlFinishDateDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFinishDateDay.SelectedIndexChanged
        ValidateFinishDate()
    End Sub

    Protected Sub ddlFinishDateMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFinishDateMonth.SelectedIndexChanged
        ValidateFinishDate()
    End Sub

    Protected Sub ddlFinishDateYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFinishDateYear.SelectedIndexChanged
        ValidateFinishDate()
    End Sub

    Protected Sub ddlScopeDateDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlScopeDateDay.SelectedIndexChanged
        ValidateScopeDate()
    End Sub

    Protected Sub ddlScopeDateMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlScopeDateMonth.SelectedIndexChanged
        ValidateScopeDate()
    End Sub

    Protected Sub ddlScopeDateYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlScopeDateYear.SelectedIndexChanged
        ValidateScopeDate()
    End Sub

    Private Sub ValidateStartDate()
        Dim dtStartDate As DateTime
        Dim bStartDateValid As Boolean
        If String.Format("{0}-{1}-{2}", ddlStartDateYear.SelectedValue, ddlStartDateMonth.SelectedValue, ddlStartDateDay.SelectedValue) = "0-0-0" Then
            txtStartDateValid.Text = String.Empty
        Else
            bStartDateValid = DateTime.TryParse(String.Format("{0}-{1}-{2}", ddlStartDateYear.SelectedValue, ddlStartDateMonth.SelectedValue, ddlStartDateDay.SelectedValue), dtStartDate)
            txtStartDateValid.Text = bStartDateValid.ToString()
        End If
    End Sub

    Private Sub ValidateAssessmentDate()
        Dim dtAssessmentDate As DateTime
        Dim bAssessmentDateValid As Boolean
        bAssessmentDateValid = DateTime.TryParse(String.Format("{0}-{1}-{2}", ddlAssessmentDateYear.SelectedValue, ddlAssessmentDateMonth.SelectedValue, ddlAssessmentDateDay.SelectedValue), dtAssessmentDate)
        txtAssessmentDateValid.Text = bAssessmentDateValid.ToString()
    End Sub

    Private Sub ValidateQuotationDate()
        Dim dtQuotationDate As DateTime
        Dim bQuotationDateValid As Boolean
        bQuotationDateValid = DateTime.TryParse(String.Format("{0}-{1}-{2}", ddlQuotationDateYear.SelectedValue, ddlQuotationDateMonth.SelectedValue, ddlQuotationDateDay.SelectedValue), dtQuotationDate)
        txtQuotationDateValid.Text = bQuotationDateValid.ToString()
    End Sub

    Private Sub ValidateFinishDate()
        Dim dtFinishDate As DateTime
        Dim bFinishDateValid As Boolean
        If String.Format("{0}-{1}-{2}", ddlFinishDateYear.SelectedValue, ddlFinishDateMonth.SelectedValue, ddlFinishDateDay.SelectedValue) = "0-0-0" Then
            txtFinishDateValid.Text = String.Empty
        Else
            bFinishDateValid = DateTime.TryParse(String.Format("{0}-{1}-{2}", ddlFinishDateYear.SelectedValue, ddlFinishDateMonth.SelectedValue, ddlFinishDateDay.SelectedValue), dtFinishDate)
            txtFinishDateValid.Text = bFinishDateValid.ToString()
        End If
    End Sub

    Private Sub ValidateScopeDate()
        Dim dtScopeDate As DateTime
        Dim bScopeDateValid As Boolean
        If String.Format("{0}-{1}-{2}", ddlScopeDateYear.SelectedValue, ddlScopeDateMonth.SelectedValue, ddlScopeDateDay.SelectedValue) = "0-0-0" Then
            txtScopeDateValid.Text = String.Empty
        Else
            bScopeDateValid = DateTime.TryParse(String.Format("{0}-{1}-{2}", ddlScopeDateYear.SelectedValue, ddlScopeDateMonth.SelectedValue, ddlScopeDateDay.SelectedValue), dtScopeDate)
            txtScopeDateValid.Text = bScopeDateValid.ToString()
        End If
    End Sub

    Private Function GetProjectCreditBalance() As Integer
        Dim result As Integer = 0
        Dim dsUserAccount As DataSet = New DataSet()
        dsUserAccount = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
        If dsUserAccount.Tables.Count > 0 Then
            If dsUserAccount.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("ProjectCredit")) Then
                    result = dsUserAccount.Tables(0).Rows(0)("ProjectCredit")
                End If
            End If
        End If
        Return result
    End Function

    Protected Sub btnSameAsName_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSameAsName.Click
        txtAddress.Text = txtProjectName.Text.Trim()
    End Sub
End Class
