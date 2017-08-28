Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class Contacts_ContactDetail
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Private m_ContactId As Long
    Public m_Cryption As New Cryption

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
            Long.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ContactId)
        Else
            m_ContactId = 0
            'm_ProjectId = 1 ' use for debug set id to 1
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnRemove.Visible = False
        End If

        If Request.QueryString("msg") <> "" Then
            lblmsg.Text = Request.QueryString("msg")
        End If

        If Not IsPostBack Then
            LoadProfile(m_ContactId)
        End If
    End Sub

    Private Sub LoadDetail(ByVal ProjectOwnerId As Long)
        Dim ProjectOwner As ProjectOwner
        ProjectOwner = m_ManagementService.GetProjectOwnerByProjectOwnerId(ProjectOwnerId)
        txtBusinessName.Text = ProjectOwner.Name
        'txtPhone.Text = ProjectOwner.Contact1
        'txtFax.Text = ProjectOwner.Contact2
        'txtMobile.Text = ProjectOwner.Contact3
        'txtAccreditation.Text = ProjectOwner.Accreditation
        'txtEQRSupervisor.Text = ProjectOwner.EQRSupervisor
        txtAddress.Text = ProjectOwner.Address
        txtSuburb.Text = ProjectOwner.Suburb
        txtCity.Text = ProjectOwner.City
        txtPostCode.Text = ProjectOwner.PostCode
        txtRegion.Text = ProjectOwner.Region
        txtCountry.Text = ProjectOwner.Country
    End Sub

    Private Sub LoadProfile(ByVal ContactId As Long)
        If ContactId > 0 Then
            Dim upContact As UserProfile
            upContact = m_ManagementService.GetUserProfileByUserID(ContactId)
            txtFirstName.Text = upContact.FirstName
            txtLastName.Text = upContact.LastName
            txtHomePhone.Text = upContact.Contact1
            txtWorkPhone.Text = upContact.Contact2
            txtMobile.Text = upContact.Contact3
            txtEmail.Text = upContact.Email
            tbxNote.Text = upContact.Notes
            btnRemove.Attributes.Add("OnClick", "return confirm('Are you sure you want to delete this contact?');")

            btnClose.Visible = True
            'btnRemove.Visible = True

            Dim objProjectOwner As ProjectOwner = m_ManagementService.GetProjectOwnerByContactId(ContactId)
            LoadDetail(objProjectOwner.ProjectOwnerId)
        End If
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        m_ManagementService.DeleteUserRelationshipByPartyAPartyB(m_LoginUser.UserId, m_ContactId, False)
        m_ManagementService.UpdateUserRelationshipInvitationAcceptDate(m_ContactId, m_LoginUser.UserId, Nothing)
        Response.Redirect("View.aspx")
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("View.aspx")
    End Sub

    Protected Sub imgInviteLink_Click(sender As Object, e As ImageClickEventArgs) Handles imgInviteLink.Click
        Dim ContactProfile As UserProfile = New UserProfile()
        ContactProfile = m_ManagementService.GetUserProfileByUserID(m_ContactId)

        Dim ProjectOwner As ProjectOwner
        ProjectOwner = m_ManagementService.GetProjectOwnerByContactId(m_LoginUser.UserId)

        Dim UserAdmin As UserProfile = New UserProfile()
        UserAdmin = m_ManagementService.GetUserProfileByUserID(ProjectOwner.ContactId)

        Dim cVoucherCode As New VoucherCodeFunctions
        Dim sToken As String = cVoucherCode.GenerateVoucherCodeGuid(16)

        Dim MailMessage As New System.Net.Mail.MailMessage()
        Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
        If ContactProfile.FirstName <> String.Empty Then
            MailMessage.To.Add(New System.Net.Mail.MailAddress(String.Format("""{0}"" <{1}>", ContactProfile.FirstName, ContactProfile.Email)))
        Else
            MailMessage.To.Add(New System.Net.Mail.MailAddress(ContactProfile.Email))
        End If
        MailMessage.Bcc.Add(New System.Net.Mail.MailAddress("ping@warpfusion.co.nz"))
        If UserAdmin.Email <> String.Empty Then
            MailMessage.From = New System.Net.Mail.MailAddress(UserAdmin.Email)
        Else
            MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("AdminEmail"))
        End If

        If ProjectOwner.Name.Trim() = String.Empty Then
            MailMessage.Subject = String.Format("{0} Kore Mobile Access", Request.Url.Host.Replace("www.", String.Empty))
        Else
            MailMessage.Subject = String.Format("{0} - {1} Kore Mobile Access", ProjectOwner.Name.Trim(), Request.Url.Host.Replace("www.", String.Empty))
        End If

        'MailMessage.Body = String.Format("Hi there,<br><br>We received a request to add you into Kore Projects. If you like to join Kore Projects, click the link below:<br><br>http://{0}/joinus.aspx?email={1}&act={2}&token={3}<br><br>This link takes you to a page where you can add your timesheet everyday.<br><br>Please ignore this message if you have received this email already.<br><br>The Kore Projects Team", Request.Url.Authority, ContactProfile.Email.Trim, m_Cryption.Encrypt("Join", m_Cryption.cryptionKey), sToken)
        MailMessage.Body = String.Format("Hi {0},<br><br>The link below gives you access to Kore Mobile:<br><a href='{1}Login.aspx?link={2}'>{1}Login.aspx?link={2}</a><br><br>Open this link on your mobile to get access to your Jobs and Timesheet. Create a bookmark/shortcut on your smartphone for instant access.<br><br><b>It's important you update your Timesheet everyday as this is required to calculate your pay.</b><br><br>If you have any problems please contact our office manager immediately.<br>Email: {3}<br>Phone: {4}<br><br>Thank you<br>{5}", ContactProfile.FirstName, ConfigurationManager.AppSettings("TimeSheetURL"), m_Cryption.Encrypt(sToken, m_Cryption.cryptionKey), String.Format("<a href='mailto:{0}'>{0}</a>", UserAdmin.Email), ProjectOwner.Contact1, ProjectOwner.Name)
        MailMessage.IsBodyHtml = True
        Try
            emailClient.Send(MailMessage)
        Catch ex As Exception
            Response.Redirect(String.Format("View.aspx?&msg=Kore Mobile invitation has not been sent - {0}", ex.Message))
        End Try

        'm_ManagementService.UpdateUserRelationshipLinkID(m_LoginUser.UserId, m_ContactId, sToken)
        m_ManagementService.UpdateUserLinkID(m_LoginUser.UserId, sToken)
        Response.Redirect("View.aspx?&msg=Kore Mobile invitation has been sent.")
    End Sub
End Class