Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class Contacts_ProjectOwnerDetail
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
            'If Not m_LoginUser.Type = 2 Then
            '    Session("CurrentLogin") = Nothing
            '    Response.Redirect("../Signin.aspx?msg=Please login as administrator!")
            'End If            
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If m_ManagementService.IsUserAccountOverDue(m_LoginUser.UserId) Then
            btnSave.Visible = False
        End If

        If Request.QueryString("msg") <> "" Then
            lblmsg.Text = Request.QueryString("msg")
        End If

        If Not IsPostBack Then
            lblTitle.Text = "Settings"
            LoadDetail(m_LoginUser.CompanyId)
        End If
    End Sub

    Private Sub LoadDetail(ByVal ProjectOwnerId As Long)
        Dim ProjectOwner As ProjectOwner
        ProjectOwner = m_ManagementService.GetProjectOwnerByProjectOwnerId(ProjectOwnerId)
        txtBusinessName.Text = ProjectOwner.Name
        txtPhone.Text = ProjectOwner.Contact1
        txtFax.Text = ProjectOwner.Contact2
        txtMobile.Text = ProjectOwner.Contact3
        txtAccreditation.Text = ProjectOwner.Accreditation
        txtEQRSupervisor.Text = ProjectOwner.EQRSupervisor
        txtAddress.Text = ProjectOwner.Address
        txtSuburb.Text = ProjectOwner.Suburb
        txtCity.Text = ProjectOwner.City
        txtPostCode.Text = ProjectOwner.PostCode
        txtRegion.Text = ProjectOwner.Region
        txtCountry.Text = ProjectOwner.Country
        tbxAccreditationNumber.Text = ProjectOwner.AccreditationNumber
        tbxGSTNumber.Text = ProjectOwner.GSTNumber

        Dim upContact As UserProfile
        upContact = m_ManagementService.GetUserProfileByUserID(m_LoginUser.UserId)
        txtFirstName.Text = upContact.FirstName
        txtLastName.Text = upContact.LastName
        txtEmail.Text = upContact.Email
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        SaveDetail(m_LoginUser.CompanyId)        
    End Sub

    Private Sub SaveDetail(ByVal ProjectOwnerId As Long)
        Dim returnUrl As String = Request.UrlReferrer.AbsoluteUri
        If returnUrl.IndexOf("&msg=") >= 0 Then
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("&msg="))
        End If
        If returnUrl.IndexOf("msg=") >= 0 Then
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("msg="))
        End If

        If m_LoginUser.CompanyId = 0 Then
            Dim ProjectOwner As New ProjectOwner
            ProjectOwner.Name = txtBusinessName.Text.Trim
            ProjectOwner.Contact1 = txtPhone.Text.Trim
            ProjectOwner.Contact2 = txtFax.Text.Trim
            ProjectOwner.Contact3 = txtMobile.Text.Trim
            ProjectOwner.Accreditation = txtAccreditation.Text.Trim
            ProjectOwner.AccreditationNumber = tbxAccreditationNumber.Text.Trim
            ProjectOwner.GSTNumber = tbxGSTNumber.Text.Trim
            ProjectOwner.EQRSupervisor = txtEQRSupervisor.Text.Trim
            ProjectOwner.Address = txtAddress.Text.Trim
            ProjectOwner.Suburb = txtSuburb.Text.Trim
            ProjectOwner.City = txtCity.Text.Trim
            ProjectOwner.PostCode = txtPostCode.Text.Trim
            ProjectOwner.Region = txtRegion.Text.Trim
            ProjectOwner.Country = txtCountry.Text.Trim
            ProjectOwner.ContactId = m_LoginUser.UserId

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
                m_LoginUser.CompanyId = intCompanyID
                Session("CurrentLogin") = m_LoginUser
            End If

            m_ManagementService.UpdateProjectOwner(ProjectOwner)

            Dim ContactProfile As UserProfile
            ContactProfile = m_ManagementService.GetUserProfileByUserID(m_LoginUser.UserId)
            ContactProfile.FirstName = txtFirstName.Text.Trim()
            ContactProfile.LastName = txtLastName.Text.Trim()
            ContactProfile.Contact1 = txtPhone.Text.Trim()
            ContactProfile.Contact2 = txtFax.Text.Trim()
            ContactProfile.Contact3 = txtMobile.Text.Trim()
            ContactProfile.Email = txtEmail.Text.Trim()
            'ContactProfile.Notes = tbxNote.Text.Trim()
            'Dim NewFile As String = UploadImages(Contact.ContactId, ContactProfile.Identifier)
            NewFile = UploadImages(m_LoginUser.UserId, ContactProfile.Identifier)
            If NewFile <> String.Empty Then
                ContactProfile.PersonalPhoto = NewFile
            Else
                'patientProfile.PersonalPhoto = tbPersonalPhoto.Text
            End If

            m_ManagementService.UpdateUserProfile(ContactProfile)

            Response.Redirect("View.aspx?msg=Your request is updated and you can add new project now.")
        Else
            Dim ContactProfile As UserProfile
            ContactProfile = m_ManagementService.GetUserProfileByUserID(m_LoginUser.UserId)
            Dim dsCurrentUser As DataSet
            dsCurrentUser = m_ManagementService.GetUserByEmail(ContactProfile.Email.Trim().ToLower())
            If ContactProfile.Email.Trim().ToLower() <> txtEmail.Text.Trim.ToLower() Then
                Dim dsNewUser As DataSet
                dsNewUser = m_ManagementService.GetUserByEmail(txtEmail.Text.Trim().ToLower())
                If dsNewUser.Tables.Count > 0 Then
                    If dsNewUser.Tables(0).Rows.Count > 0 Then
                        Response.Redirect("ProjectOwnerDetail.aspx?msg=Your changed email to one has been registered, your changes have not been updated.")
                    End If
                End If
            End If

            Dim ProjectOwner As ProjectOwner
            ProjectOwner = m_ManagementService.GetProjectOwnerByProjectOwnerId(ProjectOwnerId)
            ProjectOwner.Name = txtBusinessName.Text.Trim
            ProjectOwner.Contact1 = txtPhone.Text.Trim
            ProjectOwner.Contact2 = txtFax.Text.Trim
            ProjectOwner.Contact3 = txtMobile.Text.Trim
            ProjectOwner.Accreditation = txtAccreditation.Text.Trim
            ProjectOwner.AccreditationNumber = tbxAccreditationNumber.Text.Trim
            ProjectOwner.GSTNumber = tbxGSTNumber.Text.Trim
            ProjectOwner.EQRSupervisor = txtEQRSupervisor.Text.Trim
            ProjectOwner.Address = txtAddress.Text.Trim
            ProjectOwner.Suburb = txtSuburb.Text.Trim
            ProjectOwner.City = txtCity.Text.Trim
            ProjectOwner.PostCode = txtPostCode.Text.Trim
            ProjectOwner.Region = txtRegion.Text.Trim
            ProjectOwner.Country = txtCountry.Text.Trim

            Dim NewFile As String = UploadImages(ProjectOwner.ProjectOwnerId, ProjectOwner.Identifier)
            If NewFile <> String.Empty Then
                ProjectOwner.Logo = NewFile
            End If

            m_ManagementService.UpdateProjectOwner(ProjectOwner)


            ContactProfile.FirstName = txtFirstName.Text.Trim()
            ContactProfile.LastName = txtLastName.Text.Trim()
            ContactProfile.Contact1 = txtPhone.Text.Trim()
            ContactProfile.Contact2 = txtFax.Text.Trim()
            ContactProfile.Contact3 = txtMobile.Text.Trim()
            ContactProfile.Email = txtEmail.Text.Trim()
            'ContactProfile.Notes = tbxNote.Text.Trim()
            'Dim NewFile As String = UploadImages(Contact.ContactId, ContactProfile.Identifier)
            NewFile = UploadImages(m_LoginUser.UserId, ContactProfile.Identifier)
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

            'Response.Redirect("ProjectOwnerDetail.aspx?msg=Your request is updated.")
            Response.Redirect("../Projects/View.aspx")
        End If
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

                'Dim limitX As Integer = ConfigurationManager.AppSettings("USERPERSONALWIDTH")
                Dim limitY As Integer = ConfigurationManager.AppSettings("LOGOHEIGHT")
                Dim x As Integer = 0
                Dim y As Integer = 0
                Dim newX As Integer = 0
                Dim newY As Integer = 0

                Dim imgFile As System.Drawing.Image
                imgFile = System.Drawing.Image.FromStream(Txt_FileUpload.PostedFile.InputStream)

                x = imgFile.Width
                y = imgFile.Height

                If (y > limitY) Then
                    newY = limitY
                    newX = CType((x * (limitY * 1.0 / y)), Integer)
                Else
                    newX = x
                    newY = y
                End If

                'If (x > limitX) Then
                '    newX = limitX
                '    newY = CType((y * (limitX * 1.0 / x)), Integer)
                'Else
                '    newX = x
                '    newY = y
                'End If

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
End Class