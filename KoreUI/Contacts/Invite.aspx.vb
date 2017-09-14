Imports System.Data
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class Contacts_Invite
    Inherits System.Web.UI.Page
    Private m_EmailDataTable As DataTable
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
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
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnInvite.Visible = False
        End If
        If Not IsPostBack Then
            If m_EmailDataTable Is Nothing Then
                CreateEmailDataTable()
            End If
            Session("EmailDataTable") = m_EmailDataTable
            btnInvite.Visible = False
        Else
            m_EmailDataTable = Session("EmailDataTable")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        AddEmail(txtEmail.Text.Trim.ToLower())
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btnRemove As Button = CType(sender, Button)
        RemoveEmail(btnRemove.CommandArgument)
    End Sub

    Protected Sub btnInvite_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInvite.Click
        If m_EmailDataTable.Rows.Count > 0 Then
            For Each dr As DataRow In m_EmailDataTable.Rows
                If m_LoginUser.Email.Trim().ToLower() <> dr("Email") Then
                    Invite(dr("Email"))
                End If
            Next
            If m_EmailDataTable.Rows.Count - 1 > 0 Then
                Response.Redirect("View.aspx?&msg=Invitations have been sent.")
            Else
                Response.Redirect("View.aspx?&msg=Invitation has been sent.")
            End If
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("View.aspx")
    End Sub

    Private Sub CreateEmailDataTable()
        m_EmailDataTable = New DataTable()
        m_EmailDataTable.Columns.Add("Email", GetType(String))

        Dim keyColumn(1) As DataColumn
        keyColumn(0) = m_EmailDataTable.Columns("Email")
        m_EmailDataTable.PrimaryKey = keyColumn
    End Sub

    Private Sub AddEmail(ByVal email As String)
        Dim isNew As Boolean = True
        For Each dr As DataRow In m_EmailDataTable.Rows
            If dr("Email") = email Then
                isNew = False
            End If
        Next
        If isNew Then
            m_EmailDataTable.Rows.Add(email.Trim().ToLower())
            m_EmailDataTable.AcceptChanges()
            gvEmail.DataSource = m_EmailDataTable
            gvEmail.DataBind()

            If m_EmailDataTable.Rows.Count > 0 Then
                btnInvite.Visible = True
            End If
        End If
    End Sub

    Private Sub RemoveEmail(ByVal email As String)
        For Each dr As DataRow In m_EmailDataTable.Rows
            If dr("Email") = email Then
                dr.Delete()
            End If
        Next
        m_EmailDataTable.AcceptChanges()
        gvEmail.DataSource = m_EmailDataTable
        gvEmail.DataBind()

        If m_EmailDataTable.Rows.Count > 0 Then
            btnInvite.Visible = False
        End If
    End Sub

    Private Sub Invite(ByVal email As String)       
        Dim dsUser As DataSet = New DataSet()
        dsUser = m_ManagementService.GetUserByEmail(email)
        Dim ContactId As Long = 0
        Dim passwordExisted As Boolean = False
        If dsUser.Tables.Count > 0 Then
            If dsUser.Tables(0).Rows.Count > 0 Then
                ContactId = dsUser.Tables(0).Rows(0)("UserId")
                If Not IsDBNull(dsUser.Tables(0).Rows(0)("Password")) Then
                    passwordExisted = True
                End If
            End If
        End If

        Dim userId As Long
        If ContactId > 0 Then
            userId = ContactId
            Dim dsUserRelationship As DataSet = New DataSet()
            dsUserRelationship = m_ManagementService.GetUserRelationshipByPartyAPartyB(m_LoginUser.UserId, userId)
            If dsUserRelationship.Tables.Count > 0 Then
                If dsUserRelationship.Tables(0).Rows.Count > 0 Then
                    ' Not Update if relationship is existed
                Else
                    'm_ManagementService.CreateUserRelationship(m_LoginUser.UserId, userId, Warpfusion.A4PP.Services.UserType.Customer, Warpfusion.A4PP.Services.UserRelationshipStatus.Inactive)
                    m_ManagementService.CreateUserRelationship(m_LoginUser.UserId, userId, Warpfusion.A4PP.Services.UserType.Contractor, Warpfusion.A4PP.Services.UserRelationshipStatus.Active)
                End If
            Else
                'm_ManagementService.CreateUserRelationship(m_LoginUser.UserId, userId, Warpfusion.A4PP.Services.UserType.Customer, Warpfusion.A4PP.Services.UserRelationshipStatus.Inactive)
                m_ManagementService.CreateUserRelationship(m_LoginUser.UserId, userId, Warpfusion.A4PP.Services.UserType.Contractor, Warpfusion.A4PP.Services.UserRelationshipStatus.Active)
            End If
        Else
            userId = CreateContact(email)
        End If

        Dim ContactProfile As UserProfile = New UserProfile()
        ContactProfile = m_ManagementService.GetUserProfileByUserID(userId)

        Dim cVoucherCode As New VoucherCodeFunctions
        Dim sToken As String = cVoucherCode.GenerateVoucherCodeGuid(16)
        m_ManagementService.UpdateUserRelationshipActionToken(m_LoginUser.UserId, userId, sToken)

        Dim ProjectOwner As ProjectOwner
        ProjectOwner = m_ManagementService.GetProjectOwnerByContactId(m_LoginUser.UserId)

        Dim MailMessage As New System.Net.Mail.MailMessage()
        Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
        MailMessage.To.Add(New System.Net.Mail.MailAddress(ContactProfile.Email))
        MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("AdminEmail"))

        If ProjectOwner.Name.Trim() = String.Empty Then
            MailMessage.Subject = String.Format("{0} Invitation", Request.Url.Host.Replace("www.", String.Empty))
        Else
            MailMessage.Subject = String.Format("{0} - {1} Invitation", ProjectOwner.Name.Trim(), Request.Url.Host.Replace("www.", String.Empty))
        End If

        'MailMessage.Body = String.Format("Hi there,<br><br>We received a request to add you into A4PP. If you like to join A4PP, click the link below:<br><br>http://a4pp.warpfusion.co.nz/joinus.aspx?email={0}&token={1}&act={2}<br><br>This link takes you to a page where you can set a new password.<br><br>Please ignore this message if you don't want to join us.<br><br>The A4PP Team", ContactProfile.Email.Trim, sToken, m_Cryption.Encrypt("Join", m_Cryption.cryptionKey))
        MailMessage.Body = String.Format("Hi there,<br><br>We received a request to add you into Kore Projects. If you like to join Kore Projects, click the link below:<br><br>http://{0}/joinus.aspx?email={1}&act={2}&token={3}<br><br>This link takes you to a page where you can set a new password.<br><br>Please ignore this message if you don't want to join us.<br><br>The Kore Projects Team", Request.Url.Authority, ContactProfile.Email.Trim, m_Cryption.Encrypt("Join", m_Cryption.cryptionKey), sToken)
        MailMessage.IsBodyHtml = True
        emailClient.Send(MailMessage)

        m_ManagementService.UpdateUserRelationshipInvitationSentDate(m_LoginUser.UserId, userId, Now())
    End Sub

    Private Function CreateContact(ByVal email As String) As Long
        Dim ProjectOwner As New ProjectOwner

        Dim intCompanyID = m_ManagementService.CreateProjectOwner(ProjectOwner)
        ProjectOwner.ProjectOwnerId = intCompanyID

        Dim cVoucherCode As New VoucherCodeFunctions
        Dim strIdentifier As String = String.Format("{0}{1}", cVoucherCode.GenerateVoucherCodeGuid(16), intCompanyID)
        ProjectOwner.Identifier = strIdentifier
        m_ManagementService.UpdateProjectOwnerIdentifier(ProjectOwner)

        Dim ContactProfile As UserProfile = New UserProfile
        Dim ContactUser As New User
        Dim userId As Long
        ContactUser.Email = email
        ContactUser.Type = Warpfusion.A4PP.Services.UserType.Contractor
        ContactUser.CompanyId = intCompanyID
        userId = m_ManagementService.CreateUser(ContactUser, m_LoginUser.UserId)

        ProjectOwner.ContactId = userId
        m_ManagementService.UpdateProjectOwner(ProjectOwner)

        Dim userProfileId As Long
        ContactProfile.UserId = userId
        ContactProfile.FirstName = email.Substring(0, InStr(email, "@") - 1)
        ContactProfile.Email = email

        userProfileId = m_ManagementService.CreateUserProfile(ContactProfile)
        ContactProfile.UserProfileId = userProfileId

        strIdentifier = String.Format("{0}{1}", userProfileId, cVoucherCode.GenerateVoucherCodeGuid(16))
        ContactProfile.Identifier = strIdentifier
        m_ManagementService.UpdateUserProfileIdentifier(ContactProfile)

        m_ManagementService.UpdateUserProfile(ContactProfile)

        'm_ManagementService.UpdateUserRelationshipStatus(m_LoginUser.UserId, userId, Warpfusion.A4PP.Services.UserRelationshipStatus.Inactive)
        m_ManagementService.UpdateUserRelationshipStatus(m_LoginUser.UserId, userId, Warpfusion.A4PP.Services.UserRelationshipStatus.Active)

        Return userId
    End Function
End Class
