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

Partial Class Projects_Detail
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices()
    Private m_LoginUser As New User
    Private Const repeaterTotalColumns As Integer = 4
    Private repeaterCount As Integer = 0
    Private repeaterTotalBoundItems As Integer = 0
    Private m_ProjectID As Integer = 0
    Private m_ScopeID As Integer = 0
    Private m_ProjectOwnerId As Long = 0
    Private m_ProjectOwnerUserId As Long = 0
    Public m_Cryption As New Cryption()
    Public m_FileItem As Integer

    Private Enum AppointmentsView
        Day = 0
        Week = 1
        Month = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ProjectID)
        End If

        If Request.QueryString("SID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("SID"), m_Cryption.cryptionKey), m_ScopeID)
            If m_ScopeID = 0 Then
                If Not Session("ScopeItemDuplication") Is Nothing Then
                    Session("ScopeItemDuplication") = Nothing
                End If

                Dim objScope As New Scope
                objScope.ProjectId = m_ProjectID
                objScope.GSTRate = Decimal.Parse(ConfigurationManager.AppSettings("GST"))
                m_ScopeID = m_ScopeService.CreateScope(objScope)
                Response.Redirect(String.Format("detail.aspx?id={0}&sid={1}", Request.QueryString("ID"), m_Cryption.Encrypt(m_ScopeID.ToString(), m_Cryption.cryptionKey)))

                lbnProjectView.Attributes.Add("style", "color:#0481D1;")
                lbnProjectView.Font.Bold = True
                lbnWorksheet.ForeColor = Drawing.Color.White
                imgScopePricing.Visible = False
                aPrintWorksheet.Visible = False
                divFooterTitle.Visible = False
                aGoogleMap.Visible = True
            Else
                lbnWorksheet.Attributes.Add("style", "color:#0481D1;")
                lbnWorksheet.Font.Bold = True
                lbnProjectView.ForeColor = Drawing.Color.White
                imgScopePricing.Visible = True
                aPrintWorksheet.Visible = True
                divFooterTitle.Visible = True
                aGoogleMap.Visible = False
            End If
        Else
            lbnProjectView.Attributes.Add("style", "color:#0481D1;")
            lbnProjectView.Font.Bold = True
            lbnWorksheet.ForeColor = Drawing.Color.White
            imgScopePricing.Visible = False
            aPrintWorksheet.Visible = False
            divFooterTitle.Visible = False
            aGoogleMap.Visible = True
        End If

        If Request.QueryString("act") <> String.Empty Then
            If m_Cryption.Decrypt(Request.QueryString("act"), m_Cryption.cryptionKey) = "approveall" Then
                m_ScopeService.UpdateScopeItemApproveByScopeId(m_ScopeID)
                m_ScopeService.UpdateTotal(m_ScopeID)
                'Response.Redirect(String.Format("detail.aspx?id={0}&sid={1}", Request.QueryString("ID"), Request.QueryString("SID")))
            End If
        End If

        If Request.QueryString("msg") <> String.Empty Then
            lblMsg.Text = Request.QueryString("msg")
        End If

        m_ProjectOwnerId = m_ManagementService.GetProjectByProjectId(m_ProjectID).ProjectOwnerId
        m_ProjectOwnerUserId = m_ManagementService.GetProjectOwnerByProjectOwnerId(m_ProjectOwnerId).ContactId

        lblTitle.Text = "Project"

        If Not IsPostBack Then
            If m_ScopeID > 0 Then
                'lblTitle.Text = "Worksheet"
                lblPrintScript.Text = "<script>" & "function doPrint() {window.open('" & "PrintScope.aspx?id=" & System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & "&sid=" & m_Cryption.Encrypt(m_ScopeID, m_Cryption.cryptionKey) & "','_blank')}" & "</script>"
                lblPrintScript.Text += "<script>" & "function doPrintScope() {window.open('" & "PrintScope.aspx?id=" & System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & "&sid=" & m_Cryption.Encrypt(m_ScopeID, m_Cryption.cryptionKey) & "&act=" & m_Cryption.Encrypt("ShowRate", m_Cryption.cryptionKey) & "','_blank')}" & "</script>"
                'imgPrintScope.Visible = True
                'imgPrintScope.Attributes.Add("onclick", "doPrintScope()")
            Else
                'lblTitle.Text = "Project Details"
                'lbnView.Visible = False
                lblPrintScript.Text = "<script>" & "function doPrint() {window.open('" & "Print.aspx?id=" & System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & "','_blank')}" & "</script>"
                'imgPrintScope.Visible = False
            End If

            If m_ScopeID > 0 Then
                divProjectView.Visible = False
                divScopeView.Visible = True
                trProjectTitle.Visible = False
                trProjectView.Visible = False
                trScopeTitle.Visible = True
                If m_ProjectOwnerUserId = m_LoginUser.UserId Then
                    ScopePrice.Visible = True
                Else
                    ScopePrice.Visible = False
                End If
            Else
                divProjectView.Visible = True
                divScopeView.Visible = False
                trProjectTitle.Visible = True
                trProjectView.Visible = True
                trScopeTitle.Visible = False
            End If

            Dim dsProject As New DataSet
            If m_ProjectID > 0 Then
                dsProject = m_ManagementService.GetProjectInfoByProjectId(m_ProjectID)
            End If
            If dsProject.Tables.Count > 0 Then
                If dsProject.Tables(0).Rows.Count = 1 Then
                    Dim ContactUserProfile As UserProfile
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ContactId")) Then
                        ContactUserProfile = m_ManagementService.GetUserProfileByUserID(dsProject.Tables(0).Rows(0).Item("ContactId"))
                    End If
                    If Not ContactUserProfile Is Nothing Then
                        fname.Text = ContactUserProfile.FirstName
                        lname.Text = ContactUserProfile.LastName
                        home.Text = ContactUserProfile.Contact1
                        work.Text = ContactUserProfile.Contact2
                        mob.Text = ContactUserProfile.Contact3
                        email.Text = ContactUserProfile.Email
                        If ContactUserProfile.PersonalPhoto <> String.Empty Then
                            imgPersonalPhoto.ImageUrl = String.Format("../images/{0}/{1}", ContactUserProfile.Identifier, ContactUserProfile.PersonalPhoto)
                            imgScopePhoto.ImageUrl = String.Format("../images/{0}/{1}", ContactUserProfile.Identifier, ContactUserProfile.PersonalPhoto)
                        Else
                            imgPersonalPhoto.ImageUrl = String.Format("../images/house.jpg")
                            imgScopePhoto.ImageUrl = String.Format("../images/house.jpg")
                        End If
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Address")) Then
                        address.Text = dsProject.Tables(0).Rows(0).Item("Address")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Suburb")) Then
                        suburb.Text = dsProject.Tables(0).Rows(0).Item("Suburb")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("City")) Then
                        city.Text = dsProject.Tables(0).Rows(0).Item("City")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Postcode")) Then
                        postcode.Text = dsProject.Tables(0).Rows(0).Item("Postcode")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Region")) Then
                        region.Text = dsProject.Tables(0).Rows(0).Item("Region")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Country")) Then
                        country.Text = dsProject.Tables(0).Rows(0).Item("Country")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Name")) Then
                        projectname.Text = dsProject.Tables(0).Rows(0).Item("Name")
                        projectnametitle.Text = dsProject.Tables(0).Rows(0).Item("Name")
                        scopenametitle.Text = dsProject.Tables(0).Rows(0).Item("Name")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")) Then
                        eqcclaimnumber.Text = dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")
                        eqcsclaimnumber.Text = dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("EstimatedTime")) Then
                        projectestimatedtime.Text = dsProject.Tables(0).Rows(0).Item("EstimatedTime")
                        trEstimatedTime.Visible = False
                    Else
                        trEstimatedTime.Visible = False
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
                        projectscopedate.Text = CType(dsProject.Tables(0).Rows(0).Item("ScopeDate"), DateTime).ToString("dd/MM/yyyy")
                        lblScopeDate.Text = CType(dsProject.Tables(0).Rows(0).Item("ScopeDate"), DateTime).ToString("dd/MM/yyyy")
                        trScopeDate.Visible = True
                    Else
                        trScopeDate.Visible = False
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("StartDate")) Then
                        projectstartdate.Text = CType(dsProject.Tables(0).Rows(0).Item("StartDate"), DateTime).ToString("dd/MM/yyyy")
                        lblStartDate.Text = CType(dsProject.Tables(0).Rows(0).Item("StartDate"), DateTime).ToString("dd/MM/yyyy")
                        trStartDate.Visible = True
                    Else
                        trStartDate.Visible = False
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("AssessmentDate")) Then
                        projectassessmentdate.Text = CType(dsProject.Tables(0).Rows(0).Item("AssessmentDate"), DateTime).ToString("dd/MM/yyyy")
                        trAssessmentDate.Visible = False
                    Else
                        trAssessmentDate.Visible = False
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("QuotationDate")) Then
                        projectquotationdate.Text = CType(dsProject.Tables(0).Rows(0).Item("QuotationDate"), DateTime).ToString("dd/MM/yyyy")
                        trQuotationDate.Visible = False
                    Else
                        trQuotationDate.Visible = False
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("GroupName")) Then
                        lblGroup.Text = dsProject.Tables(0).Rows(0).Item("GroupName")
                        trGroup.Visible = True
                    Else
                        trGroup.Visible = False
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("FinishDate")) Then
                        projectfinishdate.Text = CType(dsProject.Tables(0).Rows(0).Item("FinishDate"), DateTime).ToString("dd/MM/yyyy")
                        'trFinishDate.Visible = True
                        lblEndDate.Text = CType(dsProject.Tables(0).Rows(0).Item("FinishDate"), DateTime).ToString("dd/MM/yyyy")
                        trEnddDate.Visible = True
                    Else
                        'trFinishDate.Visible = False
                        trEnddDate.Visible = False
                    End If
                    'If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ProjectStatusName")) Then
                    '    projectstatus.Text = dsProject.Tables(0).Rows(0).Item("ProjectStatusName")
                    '    scopestatus.Text = dsProject.Tables(0).Rows(0).Item("ProjectStatusName")
                    'End If
                    Dim dsUserProjectStatus As DataSet
                    dsUserProjectStatus = m_ManagementService.GetUserProjectStatusValueByProjectIdUserId(m_ProjectID, m_LoginUser.UserId)
                    If dsUserProjectStatus.Tables.Count > 0 Then
                        If dsUserProjectStatus.Tables(0).Rows.Count > 0 Then
                            If Not IsDBNull(dsUserProjectStatus.Tables(0).Rows(0)("UserProjectStatusValue")) Then
                                projectstatus.Text = dsUserProjectStatus.Tables(0).Rows(0)("Name")
                                scopestatus.Text = dsUserProjectStatus.Tables(0).Rows(0)("Name")
                            End If
                            'If Not IsDBNull(dsUserProjectStatus.Tables(0).Rows(0)("Name")) Then
                            '    projectstatus.Text = dsUserProjectStatus.Tables(0).Rows(0)("Name")
                            '    scopestatus.Text = dsUserProjectStatus.Tables(0).Rows(0)("Name")
                            'Else
                            '    'To be Debug:
                            '    projectstatus.Text = "Scope"
                            '    scopestatus.Text = "Scope"
                            'End If
                        End If
                    End If
                    Image1.ImageUrl = "../images/EmptyStar.png"
                    Image7.ImageUrl = "../images/EmptyStar.png"
                    Image2.ImageUrl = "../images/EmptyStar.png"
                    Image8.ImageUrl = "../images/EmptyStar.png"
                    Image3.ImageUrl = "../images/EmptyStar.png"
                    Image9.ImageUrl = "../images/EmptyStar.png"
                    Image4.ImageUrl = "../images/EmptyStar.png"
                    Image10.ImageUrl = "../images/EmptyStar.png"
                    Image5.ImageUrl = "../images/EmptyStar.png"
                    Image11.ImageUrl = "../images/EmptyStar.png"
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Priority")) Then
                        Dim index As Integer
                        For index = 1 To dsProject.Tables(0).Rows(0).Item("Priority")
                            If index = 1 Then
                                Image1.ImageUrl = "../images/FilledStar.png"
                                Image7.ImageUrl = "../images/FilledStar.png"
                            ElseIf index = 2 Then
                                Image2.ImageUrl = "../images/FilledStar.png"
                                Image8.ImageUrl = "../images/FilledStar.png"
                            ElseIf index = 3 Then
                                Image3.ImageUrl = "../images/FilledStar.png"
                                Image9.ImageUrl = "../images/FilledStar.png"
                            ElseIf index = 4 Then
                                Image4.ImageUrl = "../images/FilledStar.png"
                                Image10.ImageUrl = "../images/FilledStar.png"
                            ElseIf index = 5 Then
                                Image5.ImageUrl = "../images/FilledStar.png"
                                Image11.ImageUrl = "../images/FilledStar.png"
                            End If
                        Next
                        'For index = dsProject.Tables(0).Rows(0).Item("Priority") + 1 To 5
                        '    If index = 1 Then
                        '        Image1.ImageUrl = "../images/EmptyStar.png"
                        '        Image7.ImageUrl = "../images/EmptyStar.png"
                        '    ElseIf index = 2 Then
                        '        Image2.ImageUrl = "../images/EmptyStar.png"
                        '        Image8.ImageUrl = "../images/EmptyStar.png"
                        '    ElseIf index = 3 Then
                        '        Image3.ImageUrl = "../images/EmptyStar.png"
                        '        Image9.ImageUrl = "../images/EmptyStar.png"
                        '    ElseIf index = 4 Then
                        '        Image4.ImageUrl = "../images/EmptyStar.png"
                        '        Image10.ImageUrl = "../images/EmptyStar.png"
                        '    ElseIf index = 5 Then
                        '        Image5.ImageUrl = "../images/EmptyStar.png"
                        '        Image11.ImageUrl = "../images/EmptyStar.png"
                        '    End If
                        'Next
                        'Else
                        '    For index = 1 To 5
                        '        If index = 1 Then
                        '            Image1.ImageUrl = "../images/EmptyStar.png"
                        '        ElseIf index = 2 Then
                        '            Image2.ImageUrl = "../images/EmptyStar.png"
                        '        ElseIf index = 3 Then
                        '            Image3.ImageUrl = "../images/EmptyStar.png"
                        '        ElseIf index = 4 Then
                        '            Image4.ImageUrl = "../images/EmptyStar.png"
                        '        ElseIf index = 5 Then
                        '            Image5.ImageUrl = "../images/EmptyStar.png"
                        '        End If
                        '    Next
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Hazard")) Then
                        'hazard.Text = "<img src='../Images/hazard.png' />&nbsp;" & dsProject.Tables(0).Rows(0).Item("Hazard")
                        hazard.Text = "<table width='80%'><tr><td valign='middle'><img src='../Images/hazard.png' /></td><td valign='middle'>" & dsProject.Tables(0).Rows(0).Item("Hazard") & "</td></tr></table>"
                    End If
                End If
            End If

            dsProject.Dispose()
        End If

        If m_ScopeID > 0 Then
            ScopesApprovedGrid.CompanyId = m_LoginUser.CompanyId
            ScopesApprovedGrid.BranchId = 0
            ScopesApprovedGrid.UserId = m_ProjectID
            ScopesApprovedGrid.UserType = m_LoginUser.Type
            ScopesApprovedGrid.ScopeID = m_ScopeID
            ScopesApprovedGrid.LoginUserId = m_LoginUser.UserId

            Dim dsScopesPending As DataSet = New DataSet()
            dsScopesPending = m_ScopeService.GetScopeItemsByScopeIdScopeItemStatusUserId(m_ScopeID, 1, m_LoginUser.UserId)
            If dsScopesPending.Tables.Count > 0 Then
                If dsScopesPending.Tables(0).Rows.Count > 0 Then
                    divScopePending.Visible = True
                Else
                    divScopePending.Visible = False
                End If
            End If

            ScopesPendingGrid.CompanyId = m_LoginUser.CompanyId
            ScopesPendingGrid.BranchId = 0
            ScopesPendingGrid.UserId = m_ProjectID
            ScopesPendingGrid.UserType = m_LoginUser.Type
            ScopesPendingGrid.ScopeID = m_ScopeID
            ScopesPendingGrid.LoginUserId = m_LoginUser.UserId

            Dim objScope As Scope = m_ScopeService.GetScopeByScopeId(m_ScopeID)
            lblPEX.Text = objScope.Cost1.ToString("c")
            lblPIN.Text = objScope.Total1.ToString("c")
            lblAEX.Text = objScope.Cost.ToString("c")
            lblAIN.Text = objScope.Total.ToString("c")
            lblGEX.Text = (objScope.Cost1 + objScope.Cost).ToString("c")
            lblGIN.Text = (objScope.Total1 + objScope.Total).ToString("c")
        End If

        If m_ProjectID > 0 Then
            Dim m_FileResult As DataSet = New DataSet()
            m_FileResult = m_ManagementService.GetUserFileByUserID(m_ProjectID)
            files.DataSource = m_FileResult.Tables(0)
            files.DataBind()
            ShowScript(m_FileResult)
            m_FileItem = m_FileResult.Tables(0).Rows.Count

            NotesGrid.CompanyId = m_LoginUser.CompanyId
            NotesGrid.BranchId = 0
            NotesGrid.UserId = m_ProjectID
            NotesGrid.UserType = m_LoginUser.Type
            NotesGrid.LoginUserId = m_LoginUser.UserId
            'If Session("AppointmentView") <> Nothing And Session("CurrentDate") <> Nothing Then
            '    Select Case Session("AppointmentView")
            '        Case AppointmentsView.Day
            '            AppointmentsGrid.DateFrom = Session("CurrentDate")
            '            AppointmentsGrid.DateTo = AppointmentsGrid.DateFrom
            '        Case AppointmentsView.Week
            '            AppointmentsGrid.DateFrom = DateAdd(DateInterval.Day, 0 - Session("CurrentDate").DayOfWeek, Session("CurrentDate"))
            '            AppointmentsGrid.DateTo = DateAdd(DateInterval.Day, 6, AppointmentsGrid.DateFrom)
            '        Case AppointmentsView.Month
            '            AppointmentsGrid.DateFrom = New Date(Session("CurrentDate").Year, Session("CurrentDate").Month, 1)
            '            AppointmentsGrid.DateTo = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, AppointmentsGrid.DateFrom))
            '    End Select
            'Else
            '    AppointmentsGrid.DateFrom = Today  'for Day Week Link
            '    AppointmentsGrid.DateTo = Today    'for Day Week Link
            'End If
            If Not Session("Keyword") Is Nothing Then
                NotesGrid.Keyword = Session("Keyword")   'for Search
            Else
                NotesGrid.Keyword = ""
            End If
            'Else
            '    Dim m_FileResult As DataSet = New DataSet()
            '    m_FileResult = m_ManagementService.GetUserFileByUserID(m_LoginUser.UserId)
            '    files.DataSource = m_FileResult.Tables(0)
            '    files.DataBind()
            '    ShowScript(m_FileResult)
            '    m_FileItem = m_FileResult.Tables(0).Rows.Count

            '    NotesGrid.CompanyId = m_LoginUser.CompanyId
            '    NotesGrid.BranchId = m_LoginUser.BranchId
            '    NotesGrid.UserId = m_LoginUser.UserId
            '    NotesGrid.UserType = m_LoginUser.Type
            '    NotesGrid.DateFrom = Today  'for Day Week Link
            '    NotesGrid.DateTo = Today    'for Day Week Link
            '    If Not Session("Keyword") Is Nothing Then
            '        NotesGrid.Keyword = Session("Keyword")   'for Search
            '    Else
            '        NotesGrid.Keyword = ""
            '    End If

            TradeNotesGrid.CompanyId = m_LoginUser.CompanyId
            TradeNotesGrid.BranchId = 0
            TradeNotesGrid.UserId = m_ProjectID
            TradeNotesGrid.UserType = m_LoginUser.Type
            TradeNotesGrid.LoginUserId = m_LoginUser.UserId
            If Not Session("Keyword") Is Nothing Then
                TradeNotesGrid.Keyword = Session("Keyword")   'for Search
            Else
                TradeNotesGrid.Keyword = ""
            End If

            JobGrid.CompanyId = m_LoginUser.CompanyId
            JobGrid.BranchId = 0
            JobGrid.UserId = m_ProjectID
            JobGrid.UserType = m_LoginUser.Type
            JobGrid.LoginUserId = m_LoginUser.UserId

            ReorderGrid1.ProjectID = m_ProjectID

            If Not Session("Keyword") Is Nothing Then
                JobGrid.Keyword = Session("Keyword")   'for Search
            Else
                JobGrid.Keyword = ""
            End If

            aGoogleMap.HRef = GetGoogleMapUrl()
        End If

        Dim blnScopePricing As Boolean = False
        Boolean.TryParse(Session("ScopePricing"), blnScopePricing)
        If Not blnScopePricing Then
            ScopePrice.Visible = False
        End If

        'Session("QuickCreate") = Request.Url
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Public Function showFile(ByVal OwnerEmail As Object, ByVal FileName As Object, ByVal FileExtension As Object) As String
        Dim result As String
        result = String.Format("../images/{0}/{1}.{2}", OwnerEmail, FileName, FileExtension)
        Return result
    End Function

    Protected Sub litItem_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lt As Literal = CType(sender, Literal)
        repeaterCount += 1
        If repeaterCount Mod repeaterTotalColumns = 1 Then
            lt.Text = "<tr>"
        End If

        Dim CurrentProject As Project
        CurrentProject = m_ManagementService.GetProjectByProjectId(Eval("Owner", "{0}"))

        If Not CurrentProject Is Nothing Then
            Dim CurrentContact As UserProfile = m_ManagementService.GetUserProfileByUserID(CurrentProject.ContactId)

            If Not CurrentContact Is Nothing Then
                'lt.Text += String.Format("<td><a id='PatientFile-{0}' href='{1}' title='{2}'><img src='{3}' width='100' height='100' /></a><br />{4}<br /><a href='javascript:ConfirmRemove('WARNING! You are about to remove image. Is it OK to continue?','UploadFile.aspx?act={5}')'>Remove</a></td>", repeaterCount, showFile(Eval("Owner", "{0}"), Eval("FileName", "{0}"), Eval("FileExtension", "{0}")), Eval("Description", "{0}"), showFile(Eval("Owner", "{0}"), Eval("FileName", "{0}"), Eval("FileExtension", "{0}")), Eval("Description", "{0}"), m_Cryption.Encrypt("remove-" & Eval("UserFileId", "{0}"), m_Cryption.cryptionKey))
                If Eval("FileExtension", "{0}") = "pdf" Then
                    lt.Text += String.Format("<td align='center'><b>{0}</b><br />({4})<br /><a target = '_blank' href='ViewPDF.aspx?path={1}' title='{0}'><img src='../images/pdf_icon2.gif' border='0' /></a><br /><a href='UploadFile.aspx?act={2}&id={3}' OnClick='return confirm(""Are you sure you want to delete this pdf?"");'>Remove</a></td>", Eval("Description", "{0}"), "http://" & Request.Url.Authority & "/images/" & CurrentContact.Identifier & "/" & Eval("FileName", "{0}") & "." & Eval("FileExtension", "{0}"), m_Cryption.Encrypt("remove-" & Eval("UserFileId", "{0}"), m_Cryption.cryptionKey), m_Cryption.Encrypt(Eval("Owner", "{0}"), m_Cryption.cryptionKey), Eval("JobName"))
                Else
                    lt.Text += String.Format("<td align='center'>{7}<br/><b>{0}</b><br /><a id='ProjectFile-{1}' href='{2}' title='{3}'><img src='{4}' width='150' height='113' vspace='5' border='0' /></a><br /><a href='UploadFile.aspx?act={5}&id={6}' OnClick='return confirm(""Are you sure you want to delete this image?"");'>Remove</a></td>", Eval("Description", "{0}"), repeaterCount, showFile(If(Not String.IsNullOrEmpty(Eval("UserPhotoUploadFolder", "{0}")), Eval("UserPhotoUploadFolder", "{0}"), CurrentContact.Identifier), Eval("FileName", "{0}"), Eval("FileExtension", "{0}")), Eval("Description", "{0}"), showFile(If(Not String.IsNullOrEmpty(Eval("UserPhotoUploadFolder", "{0}")), Eval("UserPhotoUploadFolder", "{0}"), CurrentContact.Identifier), Eval("FileName", "{0}"), Eval("FileExtension", "{0}")), m_Cryption.Encrypt("remove-" & Eval("UserFileId", "{0}"), m_Cryption.cryptionKey), m_Cryption.Encrypt(Eval("Owner", "{0}"), m_Cryption.cryptionKey), Convert.ToDateTime(Eval("CreatedDate")).ToString("dd/MM/yyyy"))
                End If
                If repeaterCount = repeaterTotalBoundItems Then
                    Dim index As Integer
                    For index = 0 To (repeaterTotalColumns - (repeaterCount Mod repeaterTotalColumns))
                        lt.Text += "<td></td>"
                    Next
                    lt.Text += "</tr>"
                End If
                If repeaterCount Mod repeaterTotalColumns = 0 Then
                    lt.Text += "</tr>"
                End If
            End If
        End If
    End Sub

    Protected Sub ShowScript(ByVal DataRS As DataSet)
        If DataRS.Tables(0).Rows.Count > 0 Then
            lblScript.Text = "<script type='text/javascript'>$(document).ready(function() {"
            Dim RowIndex As Integer = 0
            For RowIndex = 0 To DataRS.Tables(0).Rows.Count - 1
                lblScript.Text += "$(""a#ProjectFile-" & (RowIndex + 1) & """).fancybox({'transitionIn':'elastic','transitionOut':'elastic','titlePosition':'inside'});"
            Next
            lblScript.Text += "});</script>"
        End If
    End Sub

    Public Function ShowWidth() As String
        Dim TableWidth As String = String.Empty
        If m_FileItem >= 4 Then
            TableWidth = "width='100%'"
        Else
            If m_FileItem = 2 Then
                TableWidth = "width='50%'"
            ElseIf m_FileItem = 3 Then
                TableWidth = "width='75%'"
            End If
        End If
        Return TableWidth
    End Function

    Public Function GetGoogleMapUrl() As String
        Dim result As String = String.Empty
        Dim Project As New Project
        If m_ProjectID > 0 Then
            Project = m_ManagementService.GetProjectByProjectId(m_ProjectID)
        End If
        result = String.Format("https://maps.google.co.nz/maps?f=q&source=s_q&hl=en&geocode=&q={0},{1},{2}&ie=UTF8&z=16&output=embed", Replace(Project.Address, " ", "+"), Replace(Project.City, " ", "+"), Replace(Project.Country, " ", "+"))
        'result = String.Format("https://maps.google.co.nz/maps?f=q&source=s_q&hl=en&geocode=&q={0},{1},{2}&ie=UTF8&z=16&output=embed&vpsrc=6&t=h", Replace(Project.Address, " ", "+"), Replace(Project.City, " ", "+"), Replace(Project.Country, " ", "+"))
        'result = "https://maps.google.com/?output=embed&f=q&source=s_q&hl=en&geocode=&q=London+Eye,+County+Hall,+Westminster+Bridge+Road,+London,+United+Kingdom&hl=lv&ll=51.504155,-0.117749&spn=0.00571,0.016512&sll=56.879635,24.603189&sspn=10.280244,33.815918&vpsrc=6&hq=London+Eye&radius=15000&t=h&z=17"        
        Return result
    End Function

    Public Function ShowGoogleMapIcon() As String
        Dim result As String = String.Empty

        'If imgPrintScope.Visible = True Then
        If Request("SID") <> "" Then
            result = "display:none;"
        End If

        Return result
    End Function

    Public Function GetCalendarViewUrl() As String
        Dim result As String = "CalendarView.aspx"

        Dim dsProject As New DataSet
        Dim dsUserProjectStatus As DataSet
        Dim strQueryString As String = String.Empty

        If m_ProjectID > 0 Then
            dsProject = m_ManagementService.GetProjectInfoByProjectId(m_ProjectID)
            If dsProject.Tables.Count > 0 Then
                If dsProject.Tables(0).Rows.Count > 0 Then
                    dsUserProjectStatus = m_ManagementService.GetUserProjectStatusValueByProjectIdUserId(m_ProjectID, m_LoginUser.UserId)
                    If dsUserProjectStatus.Tables.Count > 0 Then
                        If dsUserProjectStatus.Tables(0).Rows.Count > 0 Then
                            If Not IsDBNull(dsUserProjectStatus.Tables(0).Rows(0)("UserProjectStatusValue")) Then
                                If dsUserProjectStatus.Tables(0).Rows(0)("UserProjectStatusValue") = -1000 Then
                                    If Not IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
                                        strQueryString = String.Format("?date={0:yyyy-MM}-01", dsProject.Tables(0).Rows(0).Item("ScopeDate"))
                                    End If
                                Else
                                    If Not IsDBNull(dsProject.Tables(0).Rows(0).Item("StartDate")) Then
                                        strQueryString = String.Format("?date={0:yyyy-MM}-01", dsProject.Tables(0).Rows(0).Item("StartDate"))
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If

        result = String.Format("{0}{1}", result, strQueryString)

        Return result
    End Function

    Protected Sub lbnProjectView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnProjectView.Click
        If Request.QueryString("SID") = String.Empty Then
            Response.Redirect(String.Format("Detail.aspx?id={0}&sid={1}", Request.QueryString("ID"), m_Cryption.Encrypt("0", m_Cryption.cryptionKey)))
        End If
        'If lblTitle.Text = "Project Details" Then
        '    lblTitle.Text = "Project Scope Details"
        '    divProjectView.Visible = False
        '    divScopeView.Visible = True
        '    trProjectTitle.Visible = False
        '    trProjectView.Visible = False
        '    trScopeTitle.Visible = True
        'Else
        'lblTitle.Text = "Project Details"
        divProjectView.Visible = True
        divScopeView.Visible = False
        trProjectTitle.Visible = True
        trProjectView.Visible = True
        trScopeTitle.Visible = False
        lblPrintScript.Text = "<script>" & "function doPrint() {window.open('" & "Print.aspx?id=" & System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & "','_blank')}" & "</script>"
        'imgPrintScope.Visible = False
        'End If

        lbnProjectView.Attributes.Add("style", "color:#0481D1;")
        lbnProjectView.Font.Bold = True
        lbnWorksheet.Attributes.Remove("style")
        lbnWorksheet.ForeColor = Drawing.Color.White
        imgScopePricing.Visible = False
        aPrintWorksheet.Visible = False
        divFooterTitle.Visible = False
        aGoogleMap.Visible = True
    End Sub

    Protected Sub lbnWorksheet_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnWorksheet.Click
        If Request.QueryString("SID") = String.Empty Then
            Response.Redirect(String.Format("Detail.aspx?id={0}&sid={1}", Request.QueryString("ID"), m_Cryption.Encrypt("0", m_Cryption.cryptionKey)))
        End If
        'If lblTitle.Text = "Project Details" Then
        'lblTitle.Text = "Worksheet"
        divProjectView.Visible = False
        divScopeView.Visible = True
        trProjectTitle.Visible = False
        trProjectView.Visible = False
        trScopeTitle.Visible = True
        lblPrintScript.Text = "<script>" & "function doPrint() {window.open('" & "PrintScope.aspx?id=" & System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & "&sid=" & m_Cryption.Encrypt(m_ScopeID, m_Cryption.cryptionKey) & "','_blank')}" & "</script>"
        lblPrintScript.Text += "<script>" & "function doPrintScope() {window.open('" & "PrintScope.aspx?id=" & System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & "&sid=" & m_Cryption.Encrypt(m_ScopeID, m_Cryption.cryptionKey) & "&act=" & m_Cryption.Encrypt("ShowRate", m_Cryption.cryptionKey) & "','_blank')}" & "</script>"
        'imgPrintScope.Visible = True
        'imgPrintScope.Attributes.Add("onclick", "doPrintScope()")
        'Else
        '    lblTitle.Text = "Project Details"
        '    divProjectView.Visible = True
        '    divScopeView.Visible = False
        '    trProjectTitle.Visible = True
        '    trProjectView.Visible = True
        '    trScopeTitle.Visible = False
        'End If

        lbnWorksheet.Attributes.Add("style", "color:#0481D1;")
        lbnWorksheet.Font.Bold = True
        lbnProjectView.Attributes.Remove("style")
        lbnProjectView.ForeColor = Drawing.Color.White
        imgScopePricing.Visible = True
        aPrintWorksheet.Visible = True
        divFooterTitle.Visible = True
        aGoogleMap.Visible = False
    End Sub

    Protected Sub imgScopePricing_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgScopePricing.Click
        Dim I As Integer
        Dim blnUpdate As Boolean = True
        Dim blnScopePricing As Boolean = False
        Boolean.TryParse(Session("ScopePricing"), blnScopePricing)

        If blnScopePricing Then
            Session("ScopePricing") = False

            For I = 0 To Request.Cookies.Count - 1
                If Request.Cookies.Item(I).Name = m_LoginUser.UserId.ToString Then
                    Request.Cookies.Remove(Request.Cookies.Item(I).Name)
                    'Request.Cookies.Item(I).Value = False
                    'blnUpdate = False
                    Exit For
                End If
            Next

            If blnUpdate Then
                Response.Cookies(m_LoginUser.UserId.ToString).Expires = DateAdd(DateInterval.Month, 1, Now)
                Response.Cookies(m_LoginUser.UserId.ToString)("ScopePricing") = False
                Response.Cookies(m_LoginUser.UserId.ToString)("GroupView") = Session("IsGroupView")
            End If
        Else
            Session("ScopePricing") = True

            For I = 0 To Request.Cookies.Count - 1
                If Request.Cookies.Item(I).Name = m_LoginUser.UserId.ToString Then
                    Request.Cookies.Remove(Request.Cookies.Item(I).Name)
                    'Request.Cookies.Item(I).Value = True
                    'blnUpdate = False
                    Exit For
                End If
            Next

            If blnUpdate Then
                Response.Cookies(m_LoginUser.UserId.ToString).Expires = DateAdd(DateInterval.Month, 1, Now)
                Response.Cookies(m_LoginUser.UserId.ToString)("ScopePricing") = False
                Response.Cookies(m_LoginUser.UserId.ToString)("GroupView") = Session("IsGroupView")
            End If
        End If

        Response.Redirect(String.Format("Detail.aspx?id={0}&sid={1}", Request.QueryString("ID"), Request.QueryString("SID")))
    End Sub

    Protected Sub lbnGroupView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnGroupView.Click
        Session("IsGroupView") = True
        UpdateCookies()
        Response.Redirect(String.Format("Detail.aspx?id={0}&sid={1}", Request.QueryString("ID"), Request.QueryString("SID")))
    End Sub

    Protected Sub lbnGridView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnGridView.Click
        Session("IsGroupView") = False
        UpdateCookies()
        Response.Redirect(String.Format("Detail.aspx?id={0}&sid={1}", Request.QueryString("ID"), Request.QueryString("SID")))
    End Sub

    Protected Sub UpdateCookies()
        Dim blnUpdate As Boolean = True
        For I = 0 To Request.Cookies.Count - 1
            If Request.Cookies.Item(I).Name = m_LoginUser.UserId.ToString Then
                Request.Cookies.Remove(Request.Cookies.Item(I).Name)
                'Request.Cookies.Item(I).Value = True
                'blnUpdate = False
                Exit For
            End If
        Next

        If blnUpdate Then
            Response.Cookies(m_LoginUser.UserId.ToString).Expires = DateAdd(DateInterval.Month, 1, Now)
            Response.Cookies(m_LoginUser.UserId.ToString)("ScopePricing") = Session("ScopePricing")
            Response.Cookies(m_LoginUser.UserId.ToString)("GroupView") = Session("IsGroupView")
        End If
    End Sub
End Class
