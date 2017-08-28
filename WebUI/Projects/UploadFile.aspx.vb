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

Partial Class Projects_UploadFile
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Private m_ProjectID As Integer = 0
    Private m_ContactUserProfile As New UserProfile

    Protected Sub btn_Upload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Upload.Click
        Try
            If Txt_FileUpload.FileName <> "" And m_ProjectID > 0 Then

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
                    If m_ProjectID > 0 Then
                        strUserFileDescription = String.Format("{0}\images\{1}\{2}.jpg", ConfigurationManager.AppSettings("ProjectPath"), m_ContactUserProfile.Identifier, NewFileName)
                        If (Not System.IO.Directory.Exists(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_ContactUserProfile.Identifier))) Then
                            System.IO.Directory.CreateDirectory(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_ContactUserProfile.Identifier))
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

                    Dim CurrentFile As New UserFile
                    If m_ProjectID > 0 Then
                        CurrentFile.Owner = m_ProjectID
                        'Else
                        '    CurrentFile.Owner = m_LoginUser.UserId
                    End If
                    CurrentFile.FileName = NewFileName
                    CurrentFile.FileExtension = "jpg"
                    CurrentFile.Description = txt_Description.Text.Trim
                    m_ManagementService.CreateUserFile(CurrentFile)
                ElseIf FileType = "application/pdf" Then
                    Dim NewFileName As String = String.Format("{0}", Now.ToString("yyyyMMddHHmmss"))
                    If m_ProjectID > 0 Then
                        strUserFileDescription = String.Format("{0}\images\{1}\{2}.pdf", ConfigurationManager.AppSettings("ProjectPath"), m_ContactUserProfile.Identifier, NewFileName)
                        If (Not System.IO.Directory.Exists(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_ContactUserProfile.Identifier))) Then
                            System.IO.Directory.CreateDirectory(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_ContactUserProfile.Identifier))
                        End If
                        'Else
                        '    strUserFileDescription = String.Format("{0}\images\{1}\{2}.pdf", ConfigurationManager.AppSettings("ProjectPath"), m_LoginUser.Email, NewFileName)
                        '    If (Not System.IO.Directory.Exists(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_LoginUser.Email))) Then
                        '        System.IO.Directory.CreateDirectory(String.Format("{0}\images\{1}", ConfigurationManager.AppSettings("ProjectPath"), m_LoginUser.Email))
                        '    End If
                    End If

                    Txt_FileUpload.PostedFile.SaveAs(strUserFileDescription)

                    Dim CurrentFile As New UserFile
                    If m_ProjectID > 0 Then
                        CurrentFile.Owner = m_ProjectID
                        'Else
                        '    CurrentFile.Owner = m_LoginUser.UserId
                    End If
                    CurrentFile.FileName = NewFileName
                    CurrentFile.FileExtension = "pdf"
                    CurrentFile.Description = txt_Description.Text.Trim
                    CurrentFile.UserPhotoUploadFolder = m_ContactUserProfile.Identifier
                    m_ManagementService.CreateUserFile(CurrentFile)
                Else
                    Response.Redirect(String.Format("Detail.aspx?{0}&msg=The system can't upload this file type.", Request.QueryString), True)
                End If
            Else
                Response.Redirect(String.Format("Detail.aspx?{0}&msg=No file is uploaded.", Request.QueryString.ToString), True)
            End If
        Catch ex As Exception
            Response.Write(ex.Message)
            Response.Redirect(String.Format("Detail.aspx?{0}&msg=The file is uploaded unsuccessfully.", Request.QueryString), True)
        End Try

        Response.Redirect(String.Format("Detail.aspx?{0}&msg=The file is uploaded successfully.", Request.QueryString), True)
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Request.QueryString("ID") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ProjectID)

            If m_ProjectID > 0 Then
                Dim CurrentProject As Project = m_ManagementService.GetProjectByProjectId(m_ProjectID)
                m_ContactUserProfile = m_ManagementService.GetUserProfileByUserID(CurrentProject.ContactId)
            End If
        End If

        If Request.QueryString("act") <> String.Empty Then
            Dim ActString As String = m_Cryption.Decrypt(Request.QueryString("act"), m_Cryption.cryptionKey)
            Dim ActArray As Array = ActString.Split("-")
            Dim UserFileID As Integer = 0
            Dim UserAction As String
            If ActArray.Length > 1 Then
                UserAction = ActArray(0)
                Integer.TryParse(ActArray(1), UserFileID)

                If UserFileID > 0 And UserAction.ToLower.Equals("remove") Then
                    Dim dsUserFile As New DataSet
                    dsUserFile = m_ManagementService.GetUserFileByUserFileID(UserFileID)
                    If dsUserFile.Tables.Count > 0 Then
                        If dsUserFile.Tables(0).Rows.Count = 1 Then
                            If System.IO.File.Exists(String.Format("{0}\images\{1}\{2}.pdf", ConfigurationManager.AppSettings("ProjectPath"), m_ContactUserProfile.Identifier, dsUserFile.Tables(0).Rows(0).Item("FileName"))) = True Then
                                System.IO.File.Delete(String.Format("{0}\images\{1}\{2}.pdf", ConfigurationManager.AppSettings("ProjectPath"), m_ContactUserProfile.Identifier, dsUserFile.Tables(0).Rows(0).Item("FileName")))
                            End If
                        End If
                    End If

                    m_ManagementService.DeleteUserFileByUserFileIDOwner(UserFileID, m_ProjectID)
                    Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The image is deleted successfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
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
        Response.Redirect(String.Format("Detail.aspx?{0}", Request.QueryString))
    End Sub
End Class
