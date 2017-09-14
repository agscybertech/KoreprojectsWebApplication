Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class Contacts_Detail
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

        If Request.QueryString("msg") <> "" Then
            lblmsg.Text = Request.QueryString("msg")
        End If

        If Not IsPostBack Then
            Dim intUserType As Integer
            For Each intUserType In [Enum].GetValues(GetType(UserType))
                If intUserType > 100 Then
                    ddlType.Items.Add(New ListItem([Enum].GetName(GetType(UserType), intUserType), intUserType))
                End If
            Next intUserType

            Dim intUserStatus As Integer
            'ddlStatus.Items.Add(New ListItem("Not Approved", "0"))
            For Each intUserStatus In [Enum].GetValues(GetType(UserRelationshipStatus))
                If intUserStatus > 0 Then
                    ddlStatus.Items.Add(New ListItem([Enum].GetName(GetType(UserRelationshipStatus), intUserStatus), intUserStatus))
                End If
            Next intUserStatus

            LoadProfile(m_ContactId)
            'Response.Write("ServiceRate.aspx?id=" & System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & "&sid=" & m_Cryption.Encrypt("0", m_Cryption.cryptionKey))
        End If

        If ddlType.SelectedItem.ToString = "Supplier" Then
            trServiceRate.Visible = False
        Else
            ServiceRatesGrid.CompanyId = m_LoginUser.CompanyId
            ServiceRatesGrid.UserId = m_ContactId
        End If

        If m_ContactId > 0 Then
            Dim dsUserRelationship As DataSet = New DataSet()
            dsUserRelationship = m_ManagementService.GetUserRelationshipByPartyAPartyB(m_LoginUser.UserId, m_ContactId)
            If dsUserRelationship.Tables.Count > 0 Then
                If dsUserRelationship.Tables(0).Rows.Count > 0 Then
                    If Not IsDBNull(dsUserRelationship.Tables(0).Rows(0)("InvitationAcceptDate")) Then
                        btnInvite.Visible = False
                    End If
                Else
                    Response.Redirect("ContactDetail.aspx?id=" & Request.QueryString("id"))
                End If
            End If

            'imgInviteLink.Visible = True
        Else
            imgInviteLink.Visible = False
        End If

        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnInvite.Visible = False
            btnCreate.Visible = False
            btnRemove.Visible = False
            imgInviteLink.Visible = False
            btnSave.Visible = False
        End If

        txtBusinessName.Focus()
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
            Dim dsUserRelationship As DataSet
            dsUserRelationship = m_ManagementService.GetUserRelationshipByPartyAPartyB(m_LoginUser.UserId, ContactId)
            If dsUserRelationship.Tables.Count > 0 Then
                If dsUserRelationship.Tables(0).Rows.Count = 1 Then
                    ddlType.SelectedValue = dsUserRelationship.Tables(0).Rows(0).Item("Type")
                    ddlStatus.SelectedValue = dsUserRelationship.Tables(0).Rows(0).Item("Status")
                    If ddlStatus.SelectedValue = 2 Then
                        imgInviteLink.Visible = True
                    End If
                End If
            End If

            lblTitle.Text = "Edit " & ddlType.SelectedItem.ToString
            lblContactType.Text = ddlType.SelectedItem.ToString.ToUpper
            btnRemove.Attributes.Add("OnClick", "return confirm('Are you sure you want to delete this " & ddlType.SelectedItem.ToString & "?');")

            btnSave.Visible = True
            Dim dsUser As DataSet
            dsUser = m_ManagementService.GetUserByEmail(upContact.Email)
            If dsUser.Tables.Count > 0 Then
                If dsUser.Tables(0).Rows.Count > 0 Then
                    If Not IsDBNull(dsUser.Tables(0).Rows(0)("Password")) Then
                        btnSave.Visible = False
                        Response.Redirect("ContactDetail.aspx?id=" & Request.QueryString("id"))
                    End If
                End If
            End If
            btnClose.Visible = True
            btnRemove.Visible = True
            btnCreate.Visible = False

            Dim objProjectOwner As ProjectOwner = m_ManagementService.GetProjectOwnerByContactId(ContactId)
            LoadDetail(objProjectOwner.ProjectOwnerId)
        Else
            lblTitle.Text = "Add Contact"
            btnSave.Visible = False
            btnClose.Visible = True
            btnRemove.Visible = False
            btnCreate.Visible = True

            trServiceRate.Visible = False
            ServiceRatesGrid.CompanyId = m_LoginUser.CompanyId
            ServiceRatesGrid.UserId = m_ContactId
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If m_ContactId > 0 Then
                If ddlStatus.SelectedValue = "2" AndAlso Not m_ManagementService.CanAddOrActiveUser(m_LoginUser.UserId, m_ContactId) Then
                    lblmsg.Text = "You can't activate contact because you already have other active contacts as per you subscription limit."
                    Exit Sub
                End If
            Else
                If Not m_ManagementService.CanAddOrActiveUser(m_LoginUser.UserId, Nothing) Then
                    lblmsg.Text = "You can't add contact because you already have other active contacts as per you subscription limit."
                    Exit Sub
                End If
            End If

            SaveProfile(False)
            Response.Redirect("View.aspx")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub UpdateProfile(ByVal SaveWithInvite As Boolean)
        Dim returnUrl As String = Request.UrlReferrer.AbsoluteUri
        If returnUrl.IndexOf("&msg=") >= 0 Then
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("&msg="))
        End If
        If returnUrl.IndexOf("msg=") >= 0 Then
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("msg="))
        End If
        Dim ContactProfile As UserProfile = New UserProfile
        Dim Contact As Project = New Project
        If m_ContactId > 0 Then
            Dim dsCurrentUser As DataSet
            'dsCurrentUser = m_ManagementService.GetUserByEmail(ContactProfile.Email.Trim().ToLower())
            dsCurrentUser = m_ManagementService.GetUserByEmail(txtEmail.Text.Trim().ToLower())
            Dim objProjectOwner As ProjectOwner = m_ManagementService.GetProjectOwnerByContactId(m_ContactId)
            objProjectOwner.Name = txtBusinessName.Text.Trim
            objProjectOwner.Contact1 = txtHomePhone.Text.Trim
            objProjectOwner.Contact2 = txtWorkPhone.Text.Trim
            objProjectOwner.Contact3 = txtMobile.Text.Trim
            objProjectOwner.Address = txtAddress.Text.Trim
            objProjectOwner.Suburb = txtSuburb.Text.Trim
            objProjectOwner.City = txtCity.Text.Trim
            objProjectOwner.PostCode = txtPostCode.Text.Trim
            objProjectOwner.Region = txtRegion.Text.Trim
            objProjectOwner.Country = txtCountry.Text.Trim

            Dim NewFile As String = UploadImages(objProjectOwner.ProjectOwnerId, objProjectOwner.Identifier)
            If NewFile <> String.Empty Then
                objProjectOwner.Logo = NewFile
            End If
            m_ManagementService.UpdateProjectOwner(objProjectOwner)

            'Contact = m_ManagementService.GetProjectByProjectId(m_ContactId)
            'ContactProfile = m_ManagementService.GetUserProfileByUserID(Contact.ContactId)
            ContactProfile = m_ManagementService.GetUserProfileByUserID(m_ContactId)
            ContactProfile.FirstName = txtFirstName.Text.Trim()
            ContactProfile.LastName = txtLastName.Text.Trim()
            ContactProfile.Contact1 = txtHomePhone.Text.Trim()
            ContactProfile.Contact2 = txtWorkPhone.Text.Trim()
            ContactProfile.Contact3 = txtMobile.Text.Trim()
            ContactProfile.Email = txtEmail.Text.Trim()
            ContactProfile.Notes = tbxNote.Text.Trim()
            'Dim NewFile As String = UploadImages(Contact.ContactId, ContactProfile.Identifier)
            NewFile = UploadImages(m_ContactId, ContactProfile.Identifier)
            If NewFile <> String.Empty Then
                ContactProfile.PersonalPhoto = NewFile
            Else
                'patientProfile.PersonalPhoto = tbPersonalPhoto.Text
            End If

            m_ManagementService.UpdateUserProfile(ContactProfile)
            If dsCurrentUser.Tables.Count > 0 Then
                If dsCurrentUser.Tables(0).Rows.Count > 0 Then
                    Dim User As New User
                    User.UserId = dsCurrentUser.Tables(0).Rows(0)("UserId")
                    User.Email = txtEmail.Text.Trim().ToLower()
                    m_ManagementService.UpdateUserEmail(User)
                End If
            End If
            'm_ManagementService.UpdateProject(Contact)

            m_ManagementService.UpdateUserRelationshipType(m_LoginUser.UserId, m_ContactId, ddlType.SelectedValue)
            m_ManagementService.UpdateUserRelationshipStatus(m_LoginUser.UserId, m_ContactId, ddlStatus.SelectedValue)

            If SaveWithInvite And txtEmail.Text.Trim() <> String.Empty Then
                Dim cVoucherCode As New VoucherCodeFunctions
                Dim sToken As String = cVoucherCode.GenerateVoucherCodeGuid(16)
                m_ManagementService.UpdateUserRelationshipActionToken(m_LoginUser.UserId, m_ContactId, sToken)

                Dim ProjectOwner As ProjectOwner
                ProjectOwner = m_ManagementService.GetProjectOwnerByContactId(m_LoginUser.UserId)

                Dim MailMessage As New System.Net.Mail.MailMessage()
                Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
                'MailMessage.To.Add(New System.Net.Mail.MailAddress(ContactProfile.Email))
                MailMessage.To.Add(New System.Net.Mail.MailAddress(txtEmail.Text.Trim()))
                MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("AdminEmail"))

                If ProjectOwner.Name.Trim() = String.Empty Then
                    MailMessage.Subject = String.Format("{0} Invitation", Request.Url.Host.Replace("www.", String.Empty))
                Else
                    MailMessage.Subject = String.Format("{0} - {1} Invitation", ProjectOwner.Name.Trim(), Request.Url.Host.Replace("www.", String.Empty))
                End If

                'MailMessage.Body = String.Format("Hi there,<br><br>We received a request to add you into A4PP. If you like to join A4PP, click the link below:<br><br>http://a4pp.warpfusion.co.nz/joinus.aspx?email={0}&token={1}&act={2}<br><br>This link takes you to a page where you can set a new password.<br><br>Please ignore this message if you don't want to join us.<br><br>The A4PP Team", ContactProfile.Email.Trim, sToken, m_Cryption.Encrypt("Join", m_Cryption.cryptionKey))
                MailMessage.Body = String.Format("Hi there,<br><br>We received a request to add you into Kore Projects. If you like to join Kore Projects, click the link below:<br><br>http://{0}/joinus.aspx?email={1}&act={2}&token={3}<br><br>This link takes you to a page where you can set a new password.<br><br>Please ignore this message if you don't want to join us.<br><br>The Kore Projects Team", Request.Url.Authority, ContactProfile.Email.Trim, m_Cryption.Encrypt("Join", m_Cryption.cryptionKey), sToken)
                MailMessage.IsBodyHtml = True
                Try
                    emailClient.Send(MailMessage)
                Catch ex As Exception
                    Response.Redirect(String.Format("View.aspx?&msg=Invitation hasn't been sent - {0}", ex.Message))
                End Try

                m_ManagementService.UpdateUserRelationshipInvitationSentDate(m_LoginUser.UserId, m_ContactId, Now())
                Response.Redirect("View.aspx?&msg=Invitation has been sent.")
            Else
                Response.Redirect("View.aspx?msg=Your request is updated.")
            End If
        End If
    End Sub

    Protected Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        m_ManagementService.DeleteUserRelationshipByPartyAPartyB(m_LoginUser.UserId, m_ContactId, False)
        m_ManagementService.UpdateUserRelationshipInvitationAcceptDate(m_ContactId, m_LoginUser.UserId, Nothing)
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
        Response.Redirect("View.aspx")
    End Sub

    Private Function CreateContact() As Long
        Dim dsUser As DataSet = New DataSet()
        dsUser = m_ManagementService.GetUserByEmail(txtEmail.Text.Trim())
        Dim passwordExisted As Boolean = False
        If dsUser.Tables.Count > 0 Then
            If dsUser.Tables(0).Rows.Count > 0 Then
                m_ContactId = dsUser.Tables(0).Rows(0)("UserId")
                If Not IsDBNull(dsUser.Tables(0).Rows(0)("Password")) Then
                    passwordExisted = True
                End If
            End If
        End If

        If m_ContactId = 0 Then

        End If
        Dim ProjectOwner As New ProjectOwner
        ProjectOwner.Name = txtBusinessName.Text.Trim
        ProjectOwner.Contact1 = txtHomePhone.Text.Trim
        ProjectOwner.Contact2 = txtWorkPhone.Text.Trim
        ProjectOwner.Contact3 = txtMobile.Text.Trim
        ProjectOwner.Address = txtAddress.Text.Trim
        ProjectOwner.Suburb = txtSuburb.Text.Trim
        ProjectOwner.City = txtCity.Text.Trim
        ProjectOwner.PostCode = txtPostCode.Text.Trim
        ProjectOwner.Region = txtRegion.Text.Trim
        ProjectOwner.Country = txtCountry.Text.Trim

        Dim intCompanyID = m_ManagementService.CreateProjectOwner(ProjectOwner)
        ProjectOwner.ProjectOwnerId = intCompanyID

        Dim cVoucherCode As New VoucherCodeFunctions
        Dim strIdentifier As String = String.Format("{0}{1}", cVoucherCode.GenerateVoucherCodeGuid(16), intCompanyID)
        ProjectOwner.Identifier = strIdentifier
        m_ManagementService.UpdateProjectOwnerIdentifier(ProjectOwner)

        Dim NewFile As String
        If intCompanyID > 0 Then
            NewFile = UploadImages(intCompanyID, strIdentifier)
            If NewFile <> String.Empty Then
                ProjectOwner.Logo = NewFile
            End If
        End If

        Dim ContactProfile As UserProfile = New UserProfile
        Dim ContactUser As New User
        Dim userId As Long
        ContactUser.Email = txtEmail.Text.Trim()
        ContactUser.Type = ddlType.SelectedValue
        ContactUser.CompanyId = intCompanyID
        userId = m_ManagementService.CreateUser(ContactUser, m_LoginUser.UserId)

        ProjectOwner.ContactId = userId
        m_ManagementService.UpdateProjectOwner(ProjectOwner)

        Dim userProfileId As Long
        ContactProfile.UserId = userId
        ContactProfile.FirstName = txtFirstName.Text.Trim()
        ContactProfile.LastName = txtLastName.Text.Trim()
        ContactProfile.Contact1 = txtHomePhone.Text.Trim()
        ContactProfile.Contact2 = txtWorkPhone.Text.Trim()
        ContactProfile.Contact3 = txtMobile.Text.Trim()
        ContactProfile.Email = txtEmail.Text.Trim()
        ContactProfile.Notes = tbxNote.Text.Trim()

        userProfileId = m_ManagementService.CreateUserProfile(ContactProfile)
        ContactProfile.UserProfileId = userProfileId

        strIdentifier = String.Format("{0}{1}", userProfileId, cVoucherCode.GenerateVoucherCodeGuid(16))
        ContactProfile.Identifier = strIdentifier
        m_ManagementService.UpdateUserProfileIdentifier(ContactProfile)

        NewFile = UploadImages(userId, strIdentifier)
        If NewFile <> String.Empty Then
            ContactProfile.PersonalPhoto = NewFile
        Else
            'patientProfile.PersonalPhoto = tbPersonalPhoto.Text
        End If
        m_ManagementService.UpdateUserProfile(ContactProfile)

        m_ManagementService.UpdateUserRelationshipStatus(m_LoginUser.UserId, userId, ddlStatus.SelectedValue)

        Return userId
    End Function

    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        If m_ContactId > 0 Then
            If ddlStatus.SelectedValue = "2" AndAlso Not m_ManagementService.CanAddOrActiveUser(m_LoginUser.UserId, m_ContactId) Then
                lblmsg.Text = "You can't activate contact because you already have other active contacts as per you subscription limit."
                Exit Sub
            End If
        Else
            If Not m_ManagementService.CanAddOrActiveUser(m_LoginUser.UserId, Nothing) Then
                lblmsg.Text = "You can't add contact because you already have other active contacts as per you subscription limit."
                Exit Sub
            End If
        End If

        SaveProfile(False)
        Response.Redirect("View.aspx")
    End Sub

    Protected Sub btnInvite_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInvite.Click   
        If m_LoginUser.Email.Trim().ToLower() <> txtEmail.Text.Trim().ToLower() Then
            SaveProfile(True)
        End If
    End Sub

    Private Sub SaveProfile(ByVal SaveWithInvite As Boolean)
        Dim dsUser As DataSet = New DataSet()
        dsUser = m_ManagementService.GetUserByEmail(txtEmail.Text.Trim())
        Dim userChanged As Boolean = False
        Dim passwordExisted As Boolean = False
        If dsUser.Tables.Count > 0 Then
            If dsUser.Tables(0).Rows.Count > 0 Then
                If m_ContactId <> dsUser.Tables(0).Rows(0)("UserId") Then
                    userChanged = True
                End If
                m_ContactId = dsUser.Tables(0).Rows(0)("UserId")
                If Not IsDBNull(dsUser.Tables(0).Rows(0)("Password")) Then
                    passwordExisted = True
                End If
            End If
        End If

        Dim userId As Long
        If m_ContactId > 0 Then
            If Not passwordExisted Then
                UpdateProfile(SaveWithInvite)
            Else
                If Not userChanged Then
                    UpdateProfile(SaveWithInvite)
                End If
            End If
            userId = m_ContactId
            Dim dsUserRelationship As DataSet = New DataSet()
            dsUserRelationship = m_ManagementService.GetUserRelationshipByPartyAPartyB(m_LoginUser.UserId, userId)
            If dsUserRelationship.Tables.Count > 0 Then
                If dsUserRelationship.Tables(0).Rows.Count > 0 Then
                    m_ManagementService.UpdateUserRelationshipType(m_LoginUser.UserId, userId, ddlType.SelectedValue)
                    m_ManagementService.UpdateUserRelationshipStatus(m_LoginUser.UserId, userId, ddlStatus.SelectedValue)
                Else
                    m_ManagementService.CreateUserRelationship(m_LoginUser.UserId, userId, ddlType.SelectedValue, ddlStatus.SelectedValue)
                End If
            Else
                m_ManagementService.CreateUserRelationship(m_LoginUser.UserId, userId, ddlType.SelectedValue, ddlStatus.SelectedValue)
            End If
        Else
            userId = CreateContact()
        End If

        If SaveWithInvite Then
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
            Try
                emailClient.Send(MailMessage)
            Catch ex As Exception
                Response.Redirect(String.Format("View.aspx?&msg=Invitation hasn't been sent - {0}", ex.Message))
            End Try

            m_ManagementService.UpdateUserRelationshipInvitationSentDate(m_LoginUser.UserId, userId, Now())
            Response.Redirect("View.aspx?&msg=Invitation has been sent.")
        End If

        'Response.Redirect("View.aspx?&msg=" & String.Format("/joinus.aspx?email={0}&token={1}&act={2}", ContactProfile.Email.Trim, sToken, m_Cryption.Encrypt("Join", m_Cryption.cryptionKey)))
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

        m_ManagementService.UpdateUserRelationshipLinkID(m_LoginUser.UserId, m_ContactId, sToken)
        Response.Redirect("View.aspx?&msg=Kore Mobile invitation has been sent.")
    End Sub
End Class
