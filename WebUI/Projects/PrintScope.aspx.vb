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

Partial Class Projects_PrintScope
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
                Dim objScope As New Scope
                objScope.ProjectId = m_ProjectID
                objScope.GSTRate = Decimal.Parse(ConfigurationManager.AppSettings("GST"))
                m_ScopeID = m_ScopeService.CreateScope(objScope)
            End If
        End If

        m_ProjectOwnerId = m_ManagementService.GetProjectByProjectId(m_ProjectID).ProjectOwnerId
        m_ProjectOwnerUserId = m_ManagementService.GetProjectOwnerByProjectOwnerId(m_ProjectOwnerId).ContactId

        If Not IsPostBack Then
            If m_ScopeID > 0 Then
                divScopeView.Visible = True
                trScopeTitle.Visible = True
                If m_ProjectOwnerUserId = m_LoginUser.UserId Then
                    ScopePrice.Visible = True
                Else
                    ScopePrice.Visible = False
                End If
            Else
                divScopeView.Visible = False
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
                            'imgPersonalPhoto.ImageUrl = String.Format("../images/{0}/{1}", ContactUserProfile.Identifier, ContactUserProfile.PersonalPhoto)
                            imgScopePhoto.ImageUrl = String.Format("../images/{0}/{1}", ContactUserProfile.Identifier, ContactUserProfile.PersonalPhoto)
                        Else
                            'imgPersonalPhoto.ImageUrl = String.Format("../images/house.jpg")
                            imgScopePhoto.ImageUrl = String.Format("../images/house.jpg")
                        End If
                    End If
                    'If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Address")) Then
                    '    address.Text = dsProject.Tables(0).Rows(0).Item("Address") & ", "
                    'End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Suburb")) Then
                        address.Text += dsProject.Tables(0).Rows(0).Item("Suburb") & ", "
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("City")) Then
                        address.Text += dsProject.Tables(0).Rows(0).Item("City") & " "
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Postcode")) Then
                        address.Text += dsProject.Tables(0).Rows(0).Item("Postcode") & ", "
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Region")) Then
                        address.Text += dsProject.Tables(0).Rows(0).Item("Region") & ", "
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Country")) Then
                        address.Text += dsProject.Tables(0).Rows(0).Item("Country")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Name")) Then
                        scopenametitle.Text = dsProject.Tables(0).Rows(0).Item("Name")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")) Then
                        eqcsclaimnumber.Text = dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("EstimatedTime")) Then
                        projectestimatedtime.Text = dsProject.Tables(0).Rows(0).Item("EstimatedTime")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ScopeDate")) Then
                        projectscopedate.Text = CType(dsProject.Tables(0).Rows(0).Item("ScopeDate"), DateTime).ToString("dd/MM/yyyy")
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("StartDate")) Then
                        projectstartdate.Text = CType(dsProject.Tables(0).Rows(0).Item("StartDate"), DateTime).ToString("dd/MM/yyyy")
                    End If
                    'If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("AssessmentDate")) Then
                    '    projectassessmentdate.Text = CType(dsProject.Tables(0).Rows(0).Item("AssessmentDate"), DateTime).ToString("dd/MM/yyyy")
                    'End If
                    'If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("QuotationDate")) Then
                    '    projectquotationdate.Text = CType(dsProject.Tables(0).Rows(0).Item("QuotationDate"), DateTime).ToString("dd/MM/yyyy")
                    '    trQuotationDate.Visible = True
                    'Else
                    '    trQuotationDate.Visible = False
                    'End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("FinishDate")) Then
                        projectfinishdate.Text = CType(dsProject.Tables(0).Rows(0).Item("FinishDate"), DateTime).ToString("dd/MM/yyyy")
                        'trFinishDate.Visible = True
                    Else
                        'trFinishDate.Visible = False
                    End If
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ProjectStatusName")) Then
                        scopestatus.Text = dsProject.Tables(0).Rows(0).Item("ProjectStatusName")
                    End If

                    Image7.ImageUrl = "../images/EmptyStar.png"
                    Image8.ImageUrl = "../images/EmptyStar.png"
                    Image9.ImageUrl = "../images/EmptyStar.png"
                    Image10.ImageUrl = "../images/EmptyStar.png"
                    Image11.ImageUrl = "../images/EmptyStar.png"
                    If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Priority")) Then
                        Dim index As Integer
                        For index = 1 To dsProject.Tables(0).Rows(0).Item("Priority")
                            If index = 1 Then
                                Image7.ImageUrl = "../images/FilledStar.png"
                            ElseIf index = 2 Then
                                Image8.ImageUrl = "../images/FilledStar.png"
                            ElseIf index = 3 Then
                                Image9.ImageUrl = "../images/FilledStar.png"
                            ElseIf index = 4 Then
                                Image10.ImageUrl = "../images/FilledStar.png"
                            ElseIf index = 5 Then
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
                    'If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Hazard")) Then
                    '    'hazard.Text = "<img src='../Images/hazard.png' />&nbsp;" & dsProject.Tables(0).Rows(0).Item("Hazard")
                    '    hazard.Text = "<table width='80%'><tr><td valign='middle'><img src='../Images/hazard.png' /></td><td valign='middle'>" & dsProject.Tables(0).Rows(0).Item("Hazard") & "</td></tr></table>"
                    'End If
                End If

                If eqcsclaimnumber.Text = "" Then
                    tblEQCDetails.visible = False
                End If

                If fname.Text = "" And lname.Text = "" And projectstartdate.Text = "" And home.Text = "" And projectfinishdate.Text = "" And work.Text = "" And mob.Text = "" And email.Text = "" Then
                    tblPropertyOwnerDetails.Visible = False
                End If
            End If

            dsProject.Dispose()

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
        End If

        If Request.Url.ToString.ToLower.IndexOf("/printscope.aspx") > 0 Then
            ScopePrice.Visible = False
            If Request.QueryString("act") <> "" Then
                If m_Cryption.Decrypt(Request.QueryString("act"), m_Cryption.cryptionKey) = "ShowRate" Then
                    ScopePrice.Visible = True
                End If
            End If
        End If

        Dim blnScopePricing As Boolean = False
        Boolean.TryParse(Session("ScopePricing"), blnScopePricing)
        If Not blnScopePricing Then
            ScopePrice.Visible = False
        Else
            ScopePrice.Visible = True
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub
End Class
