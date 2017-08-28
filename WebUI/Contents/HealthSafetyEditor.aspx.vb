Imports System
Imports System.Collections
Imports System.Collections.Generic
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
Imports System.Drawing.Imaging
Imports System.Drawing.Drawing2D
Imports System.IO

Partial Class Contents_HealthSafetyEditor
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_ProjectOwner As New ProjectOwner
    Public m_Cryption As New Cryption()
    Private m_DynamicPageId As Integer
    Private m_DynamicPageContentId As Long = 0
    Private m_jsString As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        m_ProjectOwner = m_ManagementService.GetProjectOwnerByProjectOwnerId(m_LoginUser.CompanyId)       
        
        ' Dynamic Page Id for Health & Safety is 1
        m_DynamicPageId = 1

        lblHealthSafety.Text = "Create Health Safety Content"
        lblContentType.Visible = True
        ddlContentType.Visible = True
        btnDelete.Visible = False

        If Request.QueryString("ID") <> String.Empty Then
            Long.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_DynamicPageContentId)
            If Not IsPostBack Then
                If m_DynamicPageContentId > 0 Then
                    lblHealthSafety.Text = "Update Health Safety Content"
                    btnSave.Text = "Update"
                    lblContentType.Visible = False
                    ddlContentType.Visible = False
                    btnDelete.Visible = True
                    LoadDynamicPageContent()
                End If
            End If
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("HealthSafety.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        m_ManagementService.DeleteDynamicPageContentByDynamicPageContentId(m_DynamicPageContentId)
        Response.Redirect("HealthSafety.aspx")
    End Sub

    Private Sub LoadDynamicPageContent()
        Dim objDynamicPageContent As DynamicPageContent
        objDynamicPageContent = m_ManagementService.GetDynamicPageContentByDynamicPageContentId(m_DynamicPageContentId)
        txtDisplayOrder.Text = objDynamicPageContent.DisplayOrder
        txtTitle.Text = objDynamicPageContent.ContentTitle
        txtText.Text = objDynamicPageContent.ContentData
        If objDynamicPageContent.ContentTypeId = 1 Then
            ddlContentType.SelectedIndex = 0
            txtText.Visible = True
            lblText.Visible = True
        Else
            ddlContentType.SelectedIndex = 1
            txtText.Visible = False
            lblText.Visible = False
        End If        
        m_jsString = String.Format("javascript:SelectContentType(document.getElementById('{0}'));", ddlContentType.ClientID)
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "SelectContentType", m_jsString, True)
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim objDynamicPageContent As DynamicPageContent
        If m_DynamicPageContentId > 0 Then
            ' Update            
            objDynamicPageContent = m_ManagementService.GetDynamicPageContentByDynamicPageContentId(m_DynamicPageContentId)            
            objDynamicPageContent.ContentTitle = txtTitle.Text
            If txtDisplayOrder.Text.Trim() <> String.Empty Then
                objDynamicPageContent.DisplayOrder = txtDisplayOrder.Text
            End If
            If objDynamicPageContent.ContentTypeId = 1 Then
                objDynamicPageContent.ContentData = txtText.Text.Trim()
            End If
            m_ManagementService.UpdateDynamicPageContent(objDynamicPageContent)
            Response.Redirect(String.Format("HealthSafety.aspx?{0}&msg=The content is updated successfully.", Request.QueryString), True)
        Else
            objDynamicPageContent = New DynamicPageContent()
            objDynamicPageContent.DynamicPageId = m_DynamicPageId
            objDynamicPageContent.ProjectOwnerId = m_LoginUser.CompanyId
            objDynamicPageContent.ContentTitle = StrConv(txtTitle.Text.Trim(), VbStrConv.ProperCase)
            If txtDisplayOrder.Text.Trim() <> String.Empty Then
                objDynamicPageContent.DisplayOrder = txtDisplayOrder.Text
            End If

            ' Save
            If ddlContentType.SelectedIndex = 0 Then
                ' Plain Text
                objDynamicPageContent.ContentTypeId = 1
                objDynamicPageContent.ContentData = txtText.Text.Trim()
                m_ManagementService.CreateDynamicPageContent(objDynamicPageContent)
                Response.Redirect(String.Format("HealthSafety.aspx?{0}&msg=The content is saved successfully.", Request.QueryString), True)
            Else
                ' File                    
                Try
                    If Txt_FileUpload.FileName <> "" And m_ProjectOwner.Identifier <> String.Empty Then

                        Dim fileName As String
                        Dim FileType As String
                        Dim namePosition As Int16
                        Dim mStream = New MemoryStream()
                        Dim strUserFileDescription As String

                        namePosition = Txt_FileUpload.PostedFile.FileName.LastIndexOf("\") + 1
                        fileName = Txt_FileUpload.PostedFile.FileName.Substring(namePosition)
                        FileType = Txt_FileUpload.PostedFile.ContentType

                        If FileType = "image/gif" Or FileType = "image/jpeg" Or FileType = "image/pjpeg" Or FileType = "image/png" Or FileType = "image/bmp" Then
                            Dim limitX As Integer = ConfigurationManager.AppSettings("USERPHOTOWIDTH")
                            Dim limitY As Integer = ConfigurationManager.AppSettings("USERPHOTOHEIGHT")
                            Dim x As Integer = 0
                            Dim y As Integer = 0
                            Dim newX As Integer = 0
                            Dim newY As Integer = 0

                            Dim imgFile As System.Drawing.Image
                            imgFile = System.Drawing.Image.FromStream(Txt_FileUpload.PostedFile.InputStream)

                            x = imgFile.Width
                            y = imgFile.Height

                            If (x > limitX Or y > limitY) Then
                                If (x * 1.0 / y >= limitX * 1.0 / limitY) Then
                                    newX = limitX
                                    newY = CType((y * (limitX * 1.0 / x)), Integer)
                                Else
                                    newY = limitY
                                    newX = CType((x * (limitY * 1.0 / y)), Integer)
                                End If
                            Else
                                newX = x
                                newY = y
                            End If
                            'stream = Txt_FileUpload.PostedFile.InputStream
                            resizeimage(imgFile, newX, newY, mStream)

                            Dim uploadedFile(mStream.Length) As Byte
                            'mStream.Read(uploadedFile, 0, mStream.Length)
                            uploadedFile = mStream.ToArray()
                            mStream.Close()


                            Dim NewFileName As String = String.Format("{0}", Now.ToString("yyyyMMddHHmmss"))
                            If m_ProjectOwner.Identifier <> String.Empty Then
                                strUserFileDescription = String.Format("{0}\images\{1}\{2}.jpg", ConfigurationManager.AppSettings("ProjectPath"), m_ProjectOwner.Identifier, NewFileName)
                                If (Not System.IO.Directory.Exists(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_ProjectOwner.Identifier))) Then
                                    System.IO.Directory.CreateDirectory(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_ProjectOwner.Identifier))
                                End If
                                'Else
                                '    strUserFileDescription = String.Format("{0}\images\{1}\{2}.jpg", ConfigurationManager.AppSettings("ProjectPath"), m_LoginUser.Email, NewFileName)
                                '    If (Not System.IO.Directory.Exists(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_LoginUser.Email))) Then
                                '        System.IO.Directory.CreateDirectory(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_LoginUser.Email))
                                '    End If
                            End If

                            Dim wFile As FileStream
                            wFile = New FileStream(strUserFileDescription, FileMode.Create)
                            wFile.Write(uploadedFile, 0, uploadedFile.Length)
                            wFile.Close()

                            objDynamicPageContent.ContentTypeId = 2
                            objDynamicPageContent.ContentData = String.Format("{0}.jpg", NewFileName)
                            m_ManagementService.CreateDynamicPageContent(objDynamicPageContent)
                        ElseIf FileType = "application/pdf" Then
                            Dim NewFileName As String = String.Format("{0}", Now.ToString("yyyyMMddHHmmss"))
                            strUserFileDescription = String.Format("{0}\images\{1}\{2}.pdf", ConfigurationManager.AppSettings("ProjectPath"), m_ProjectOwner.Identifier, NewFileName)
                            If (Not System.IO.Directory.Exists(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_ProjectOwner.Identifier))) Then
                                System.IO.Directory.CreateDirectory(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_ProjectOwner.Identifier))
                            End If

                            Txt_FileUpload.PostedFile.SaveAs(strUserFileDescription)

                            objDynamicPageContent.ContentTypeId = 3
                            objDynamicPageContent.ContentData = String.Format("{0}.pdf", NewFileName)
                            m_ManagementService.CreateDynamicPageContent(objDynamicPageContent)
                        Else
                            Response.Redirect(String.Format("HealthSafety.aspx?{0}&msg=The system can't upload this file type.", Request.QueryString), True)
                        End If
                    Else
                        Response.Redirect(String.Format("HealthSafety.aspx?{0}&msg=No file is uploaded.", Request.QueryString.ToString), True)
                    End If
                Catch ex As Exception
                    Response.Write(ex.Message)
                    Response.Redirect(String.Format("HealthSafety.aspx?{0}&msg=The file is uploaded unsuccessfully.", Request.QueryString), True)
                End Try
                Response.Redirect(String.Format("HealthSafety.aspx?{0}&msg=The file is uploaded successfully.", Request.QueryString), True)
            End If
        End If
    End Sub

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
        thumbNailImg.Save(imgStream, ImageFormat.Jpeg)
        thumbNailImg.Dispose()
    End Sub

    Function ThumbnailCallback() As Boolean
        Return False
    End Function
End Class