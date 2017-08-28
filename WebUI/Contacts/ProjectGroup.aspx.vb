Imports System.Data
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Globalization
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class Contacts_ProjectGroup
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption
    Private m_ProjectGroupID As Integer = 0

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
            btnAdd.Visible = False
            btnDelete.Visible = False
        End If

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ProjectGroupID)
        End If

        If Not IsPostBack Then
            If m_ProjectGroupID > 0 Then
                LoadDetail(m_ProjectGroupID)

                btnAdd.Visible = False
                btnSave.Visible = True
                btnClose.Visible = True
                btnDelete.Visible = True

                lblProjectGroup.Text = "UPDATE GROUP"
            Else
                btnAdd.Visible = True
                btnSave.Visible = False
                btnClose.Visible = True
                btnDelete.Visible = False

                lblProjectGroup.Text = "ADD GROUP"
            End If
        End If

        txtBusinessName.Focus()
    End Sub

    Private Sub LoadDetail(ByVal ProjectGroupId As Long)
        Dim ProjectGroup As ProjectGroup
        ProjectGroup = m_ManagementService.GetProjectGroupByProjectGroupId(ProjectGroupId)
        txtBusinessName.Text = ProjectGroup.Name
        txtEmail.Text = ProjectGroup.Email
        txtPhone.Text = ProjectGroup.Contact1
        txtFax.Text = ProjectGroup.Contact2
        txtMobile.Text = ProjectGroup.Contact3
        txtAddress.Text = ProjectGroup.Address
        txtSuburb.Text = ProjectGroup.Suburb
        txtCity.Text = ProjectGroup.City
        txtPostCode.Text = ProjectGroup.PostCode
        txtRegion.Text = ProjectGroup.Region
        txtCountry.Text = ProjectGroup.Country
        txtDisplayOrder.Text = ProjectGroup.DisplayOrder
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        SaveDetail()
    End Sub

    Private Sub SaveDetail()
        Dim returnUrl As String = Request.UrlReferrer.AbsoluteUri
        If returnUrl.IndexOf("&msg=") >= 0 Then
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("&msg="))
        End If
        If returnUrl.IndexOf("msg=") >= 0 Then
            returnUrl = returnUrl.Substring(0, returnUrl.IndexOf("msg="))
        End If

        If m_ProjectGroupID = 0 Then
            Dim ProjectGroup As New ProjectGroup
            ProjectGroup.Name = txtBusinessName.Text.Trim
            ProjectGroup.Email = txtEmail.Text.Trim
            ProjectGroup.Contact1 = txtPhone.Text.Trim
            ProjectGroup.Contact2 = txtFax.Text.Trim
            ProjectGroup.Contact3 = txtMobile.Text.Trim
            ProjectGroup.Address = txtAddress.Text.Trim
            ProjectGroup.Suburb = txtSuburb.Text.Trim
            ProjectGroup.City = txtCity.Text.Trim
            ProjectGroup.PostCode = txtPostCode.Text.Trim
            ProjectGroup.Region = txtRegion.Text.Trim
            ProjectGroup.Country = txtCountry.Text.Trim
            ProjectGroup.ProjectOwnerId = m_LoginUser.CompanyId

            Dim intDisplayOrder As Integer = 0
            Integer.TryParse(txtDisplayOrder.Text.Trim, intDisplayOrder)
            ProjectGroup.DisplayOrder = intDisplayOrder

            Dim intProjectGroupID = m_ManagementService.CreateProjectGroup(ProjectGroup)
            ProjectGroup.ProjectGroupId = intProjectGroupID

            Dim cVoucherCode As New VoucherCodeFunctions
            Dim strIdentifier As String = String.Format("{0}{1}", cVoucherCode.GenerateVoucherCodeGuid(16), intProjectGroupID)
            ProjectGroup.Identifier = strIdentifier
            m_ManagementService.UpdateProjectGroupIdentifier(ProjectGroup)

            Dim NewFile As String
            If intProjectGroupID > 0 Then
                NewFile = UploadImages(intProjectGroupID, strIdentifier)
                If NewFile <> String.Empty Then
                    ProjectGroup.Logo = NewFile
                End If
            End If

            m_ManagementService.UpdateProjectGroup(ProjectGroup)

            Response.Redirect("ProjectSetting.aspx?msg=Your request is updated and you can add new project now.")
        Else
            Dim ProjectGroup As ProjectGroup
            ProjectGroup = m_ManagementService.GetProjectGroupByProjectGroupId(m_ProjectGroupID)
            ProjectGroup.Name = txtBusinessName.Text.Trim
            ProjectGroup.Email = txtEmail.Text.Trim
            ProjectGroup.Contact1 = txtPhone.Text.Trim
            ProjectGroup.Contact2 = txtFax.Text.Trim
            ProjectGroup.Contact3 = txtMobile.Text.Trim
            ProjectGroup.Address = txtAddress.Text.Trim
            ProjectGroup.Suburb = txtSuburb.Text.Trim
            ProjectGroup.City = txtCity.Text.Trim
            ProjectGroup.PostCode = txtPostCode.Text.Trim
            ProjectGroup.Region = txtRegion.Text.Trim
            ProjectGroup.Country = txtCountry.Text.Trim

            Dim NewFile As String = UploadImages(ProjectGroup.ProjectGroupId, ProjectGroup.Identifier)
            If NewFile <> String.Empty Then
                ProjectGroup.Logo = NewFile
            End If

            m_ManagementService.UpdateProjectGroup(ProjectGroup)

            Response.Redirect("ProjectSetting.aspx?msg=Your request is updated.")
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
        Response.Redirect("ProjectSetting.aspx")
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        SaveDetail()
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_ProjectGroupID > 0 Then
            Dim ProjectGroup As ProjectGroup
            ProjectGroup = m_ManagementService.GetProjectGroupByProjectGroupId(m_ProjectGroupID)
            If Not ProjectGroup.Identifier Is Nothing Then
                If (System.IO.Directory.Exists(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), ProjectGroup.Identifier))) Then
                    If Not ProjectGroup.Logo Is Nothing Then
                        If File.Exists(String.Format("{0}\images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), ProjectGroup.Identifier, ProjectGroup.Logo)) Then
                            File.Delete(String.Format("{0}\images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), ProjectGroup.Identifier, ProjectGroup.Logo))
                        End If

                        If Not ProcessDirectory(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), ProjectGroup.Identifier)) Then
                            System.IO.Directory.Delete(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), ProjectGroup.Identifier), True)
                        End If
                    End If
                End If
            End If
            m_ManagementService.DeleteProjectGroupByProjectGroupId(m_ProjectGroupID)
            Response.Redirect(String.Format("ProjectSetting.aspx?msg=The group is deleted successfully."))
        End If
    End Sub

    Public Function ProcessDirectory(ByVal targetDirectory As String) As Boolean
        Dim fileEntries As String() = Directory.GetFiles(targetDirectory)
        Dim hasFiles As Boolean = False
        Dim fileName As String
        For Each fileName In fileEntries
            hasFiles = True
            Exit For
        Next fileName
        'Dim subdirectoryEntries As String() = Directory.GetDirectories(targetDirectory)
        ' Recurse into subdirectories of this directory. 
        'Dim subdirectory As String
        'For Each subdirectory In subdirectoryEntries
        'ProcessDirectory(subdirectory)
        'Next subdirectory
        Return hasFiles
    End Function
End Class
