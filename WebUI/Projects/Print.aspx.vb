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

Partial Class Projects_Print
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private Const repeaterTotalColumns As Integer = 4
    Private repeaterCount As Integer = 0
    Private repeaterTotalBoundItems As Integer = 0
    Private m_ProjectID As Integer = 0
    Public m_Cryption As New Cryption()
    Public m_FileItem As Integer

    Private Enum AppointmentsView
        Day = 0
        Week = 1
        Month = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ProjectID)
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
                        'imgPersonalPhoto.ImageUrl = String.Format("../images/{0}/{1}", email.Text, ContactUserProfile.PersonalPhoto)
                        imgPersonalPhoto.ImageUrl = String.Format("../images/{0}/{1}", ContactUserProfile.Identifier, ContactUserProfile.PersonalPhoto)
                    Else
                        'imgPersonalPhoto.ImageUrl = String.Format("../images/male-avatar.jpg")
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
                End If
                If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")) Then
                    eqcclaimnumber.Text = dsProject.Tables(0).Rows(0).Item("EQCClaimNumber")
                End If
                If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("StartDate")) Then
                    projectstartdate.Text = CType(dsProject.Tables(0).Rows(0).Item("StartDate"), DateTime).ToString("dd/MM/yyyy")
                End If
                If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("AssessmentDate")) Then
                    projectassessmentdate.Text = CType(dsProject.Tables(0).Rows(0).Item("AssessmentDate"), DateTime).ToString("dd/MM/yyyy")
                End If
                If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("ProjectStatusName")) Then
                    projectstatus.Text = dsProject.Tables(0).Rows(0).Item("ProjectStatusName")
                End If
                If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Priority")) Then
                    Dim index As Integer
                    For index = 1 To dsProject.Tables(0).Rows(0).Item("Priority")
                        If index = 1 Then
                            Image1.ImageUrl = "../images/FilledStar.png"
                        ElseIf index = 2 Then
                            Image2.ImageUrl = "../images/FilledStar.png"
                        ElseIf index = 3 Then
                            Image3.ImageUrl = "../images/FilledStar.png"
                        ElseIf index = 4 Then
                            Image4.ImageUrl = "../images/FilledStar.png"
                        ElseIf index = 5 Then
                            Image5.ImageUrl = "../images/FilledStar.png"
                        End If
                    Next
                    For index = dsProject.Tables(0).Rows(0).Item("Priority") + 1 To 5
                        If index = 1 Then
                            Image1.ImageUrl = "../images/EmptyStar.png"
                        ElseIf index = 2 Then
                            Image2.ImageUrl = "../images/EmptyStar.png"
                        ElseIf index = 3 Then
                            Image3.ImageUrl = "../images/EmptyStar.png"
                        ElseIf index = 4 Then
                            Image4.ImageUrl = "../images/EmptyStar.png"
                        ElseIf index = 5 Then
                            Image5.ImageUrl = "../images/EmptyStar.png"
                        End If
                    Next
                End If
                If Not Convert.IsDBNull(dsProject.Tables(0).Rows(0).Item("Hazard")) Then
                    hazard.Text = "<img src='../Images/hazard.png' />&nbsp;" & dsProject.Tables(0).Rows(0).Item("Hazard")
                End If
            End If
        End If

        dsProject.Dispose()

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
                    'lt.Text += String.Format("<td><div class='apple_overlayfile' id='overlayfile-{0}'><iframe src ='ViewPDF.aspx?path={1}' width='100%' height='440px' scrolling='no' frameborder='0'></iframe></div><a href='' title='{2}' rel='#overlayfile-{0}'><img src='../images/pdf_icon2.gif'/></a><br />{2}</td>", repeaterCount, ConfigurationManager.AppSettings("ProjectURL") & "images/" & OwnerProfile.Email & "/" & Eval("FileName", "{0}") & "." & Eval("FileExtension", "{0}"), Eval("Description", "{0}"))
                    'lt.Text += String.Format("<td align='center'><b>{0}</b><br /><a target = '_blank' href='ViewPDF.aspx?path={1}' title='{0}'><img src='../images/pdf_icon2.gif'/></a></td>", Eval("Description", "{0}"), ConfigurationManager.AppSettings("ProjectURL") & "images/" & CurrentContact.Email & "/" & Eval("FileName", "{0}") & "." & Eval("FileExtension", "{0}"))
                    'lt.Text += String.Format("<td align='center'><b>{0}</b><br /><a target = '_blank' href='ViewPDF.aspx?path={1}' title='{0}'><img src='../images/pdf_icon2.gif' border='0' /></a></td>", Eval("Description", "{0}"), ConfigurationManager.AppSettings("ProjectURL") & "images/" & CurrentContact.Identifier & "/" & Eval("FileName", "{0}") & "." & Eval("FileExtension", "{0}"))
                    lt.Text += String.Format("<td align='center'><b>{0}</b><br /><a target = '_blank' href='ViewPDF.aspx?path={1}' title='{0}'><img src='../images/pdf_icon2.gif' border='0' /></a></td>", Eval("Description", "{0}"), "http://" & Request.Url.Authority & "/images/" & CurrentContact.Identifier & "/" & Eval("FileName", "{0}") & "." & Eval("FileExtension", "{0}"))
                Else
                    'lt.Text += String.Format("<td><a id='ProjectFile-{0}' href='{1}' title='{2}'><img src='{3}' width='100' height='100' /></a><br />{4}</td>", repeaterCount, showFile(CurrentContact.Email, Eval("FileName", "{0}"), Eval("FileExtension", "{0}")), Eval("Description", "{0}"), showFile(CurrentContact.Email, Eval("FileName", "{0}"), Eval("FileExtension", "{0}")), Eval("Description", "{0}"))
                    'lt.Text += String.Format("<td align='center'><b>{0}</b><br /><a id='ProjectFile-{1}' href='{2}' title='{3}'><img src='{4}' width='150' vspace='5' /></a></td>", Eval("Description", "{0}"), repeaterCount, showFile(CurrentContact.Email, Eval("FileName", "{0}"), Eval("FileExtension", "{0}")), Eval("Description", "{0}"), showFile(CurrentContact.Email, Eval("FileName", "{0}"), Eval("FileExtension", "{0}")))
                    lt.Text += String.Format("<td align='center'><b>{0}</b><br /><a id='ProjectFile-{1}' href='{2}' title='{3}'><img src='{4}' width='150' vspace='5' border='0' /></a></td>", Eval("Description", "{0}"), repeaterCount, showFile(CurrentContact.Identifier, Eval("FileName", "{0}"), Eval("FileExtension", "{0}")), Eval("Description", "{0}"), showFile(CurrentContact.Identifier, Eval("FileName", "{0}"), Eval("FileExtension", "{0}")))
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
End Class
