Imports System
Imports System.Collections
Imports System.Linq
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class signup_remote
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As User
    Private m_jsString As String

    Protected Sub btnSignup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSignup.Click
        If m_ManagementService.GetUserCountByEmail(txtUser.Text) = 0 Then
            Dim signUser As New User
            Dim cVoucherCode As New VoucherCodeFunctions
            Dim strIdentifier As String
            signUser.Email = txtUser.Text
            signUser.Password = txtPassword.Text
            signUser.Type = 2
            Dim signUserID As Long = m_ManagementService.CreateUser(signUser, 0)
            Dim signUserProfile As New UserProfile
            signUserProfile.UserId = signUserID
            signUserProfile.Email = txtUser.Text
            Dim userProfileId As Long = m_ManagementService.CreateUserProfile(signUserProfile)
            signUserProfile.UserProfileId = userProfileId
            strIdentifier = String.Format("{0}{1}", userProfileId, cVoucherCode.GenerateVoucherCodeGuid(16))
            signUserProfile.Identifier = strIdentifier
            m_ManagementService.UpdateUserProfileIdentifier(signUserProfile)
            Dim signUserOwner As New ProjectOwner
            signUserOwner.ContactId = signUserID
            Dim intCompanyID As Long = m_ManagementService.CreateProjectOwner(signUserOwner)
            signUserOwner.ProjectOwnerId = intCompanyID
            strIdentifier = String.Format("{0}{1}", cVoucherCode.GenerateVoucherCodeGuid(16), intCompanyID)
            signUserOwner.Identifier = strIdentifier
            m_ManagementService.UpdateProjectOwnerIdentifier(signUserOwner)
            m_LoginUser = m_ManagementService.Login(txtUser.Text.Trim, txtPassword.Text.Trim)
            If Not m_LoginUser Is Nothing Then
                'Dim dsPlans As DataSet
                'dsPlans = m_ManagementService.GetPlans(True)
                'Dim projectCredit As Integer = 0
                'Dim freePlanExisted As Boolean = False
                'If dsPlans.Tables.Count > 0 Then
                '    If dsPlans.Tables(0).Rows.Count > 0 Then
                '        For Each row As DataRow In dsPlans.Tables(0).Rows
                '            If IsDBNull(row("Price")) Then
                '                freePlanExisted = True
                '            Else
                '                If row("Price") = 0 Then
                '                    freePlanExisted = True
                '                End If
                '            End If
                '            If freePlanExisted Then
                '                If Not IsDBNull(row("NumberOfProjects")) Then
                '                    projectCredit = row("NumberOfProjects")
                '                    Exit For
                '                End If
                '            End If
                '        Next
                '    End If
                'End If
                'If projectCredit > 0 Then
                '    m_ManagementService.CreateUserAccount(m_LoginUser.UserId, projectCredit)
                '    m_ManagementService.CreateUserTransaction(m_LoginUser.UserId, "Free trial", 0, 0, projectCredit, projectCredit)
                'End If
                Dim numberOfProjects As Integer = 0
                Dim strPromoCode As String = ConfigurationManager.AppSettings("PromoCode_SignUp")
                Dim PromoCodeValid As Boolean = False
                PromoCodeValid = m_ManagementService.CheckPromoCodeValidByPromoCodeUserId(strPromoCode, m_LoginUser.UserId)
                If PromoCodeValid Then
                    Dim dsPlan As DataSet
                    dsPlan = m_ManagementService.GetPlanByPromoCodeUserId(strPromoCode, m_LoginUser.UserId)
                    If dsPlan.Tables.Count > 0 Then
                        If dsPlan.Tables(0).Rows.Count > 0 Then
                            Dim PlanId As Integer = 0
                            If Not IsDBNull(dsPlan.Tables(0).Rows(0)("PlanId")) Then
                                PlanId = dsPlan.Tables(0).Rows(0)("PlanId")
                            End If                            
                            If Not IsDBNull(dsPlan.Tables(0).Rows(0)("NumberOfProjects")) Then
                                numberOfProjects = dsPlan.Tables(0).Rows(0)("NumberOfProjects")
                            End If
                            Dim storageSize As Decimal = 0
                            If Not IsDBNull(dsPlan.Tables(0).Rows(0)("StorageSize")) Then
                                storageSize = dsPlan.Tables(0).Rows(0)("StorageSize")
                            End If
                            Dim term As Integer = 0
                            If Not IsDBNull(dsPlan.Tables(0).Rows(0)("Term")) Then
                                term = dsPlan.Tables(0).Rows(0)("Term")
                            End If
                            Dim NextBillingDate As DateTime
                            NextBillingDate = DateAdd(DateInterval.Month, term, Now)
                            m_ManagementService.UpdateUserAccountMonthly(m_LoginUser.UserId, PlanId, numberOfProjects, storageSize, NextBillingDate)
                            m_ManagementService.CreatePromotionRedeemed(strPromoCode, m_LoginUser.UserId)
                            m_ManagementService.CreateUserTransaction(m_LoginUser.UserId, String.Format("Promotion - Redeem {0}", strPromoCode), 0, 0, numberOfProjects, numberOfProjects)
                        End If
                    End If
                End If

                Session("CurrentLogin") = m_LoginUser

                Dim MailMessage As New System.Net.Mail.MailMessage()
                Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
                'MailMessage.To.Add(New System.Net.Mail.MailAddress(ContactProfile.Email))
                MailMessage.To.Add(New System.Net.Mail.MailAddress(m_LoginUser.Email))
                MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("AdminEmail"))

                MailMessage.Subject = String.Format("Welcome to {0}", Request.Url.Host.Replace("www.", String.Empty))

                If numberOfProjects < 1 Then
                    MailMessage.Body = String.Format("Hi there,<br><br>Welcome to join Kore Projects.<br><br>Your account has been activated.<br><br>The Kore Projects Team")
                Else
                    If numberOfProjects = 1 Then
                        MailMessage.Body = String.Format("Hi there,<br><br>Welcome to Kore Projects.<br><br>Your account has been activated with {0} project.<br>To visit Kore Projects please use the link below:<br><br>https://{1}<br><br>The Kore Projects Team", numberOfProjects, Request.Url.Authority)
                    Else
                        MailMessage.Body = String.Format("Hi there,<br><br>Welcome to Kore Projects.<br><br>Your account has been activated with {0} projects.<br>To visit Kore Projects please use the link below:<br><br>https://{1}<br><br>The Kore Projects Team", numberOfProjects, Request.Url.Authority)
                    End If
                End If

                MailMessage.IsBodyHtml = True
                Try
                    emailClient.Send(MailMessage)
                Catch ex As Exception
                    'Response.Redirect(String.Format("View.aspx?&msg=Invitation hasn't been sent - {0}", ex.Message))
                    Throw
                End Try

                Dim objServiceGroup As New ServiceGroup
                objServiceGroup.UserId = m_LoginUser.UserId
                objServiceGroup.Name = "Rate Sheet 1"
                Dim intDisplayOrder As Integer = 0
                objServiceGroup.DisplayOrder = intDisplayOrder
                objServiceGroup.IsPrivate = "2"
                m_ScopeService.CreateServiceGroup(objServiceGroup)
                objServiceGroup = New ServiceGroup
                objServiceGroup.UserId = m_LoginUser.UserId
                objServiceGroup.Name = "Rate Sheet 2"
                objServiceGroup.DisplayOrder = intDisplayOrder
                objServiceGroup.IsPrivate = "2"
                m_ScopeService.CreateServiceGroup(objServiceGroup)

                If Session("AcceptInvitation") Is Nothing Then
                    'm_jsString = "parent.location.href='Projects/View.aspx';"
                    m_jsString = "parent.location.href='Contacts/ProjectOwnerDetail.aspx?user=new';"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "ProjectsView", m_jsString, True)
                Else
                    Session("AcceptInvitation") = Nothing
                    m_jsString = "parent.location.href='Contacts/ProjectOwnerDetail.aspx?user=new';"
                    Page.ClientScript.RegisterStartupScript(Me.GetType(), "ProjectOwnerDetail", m_jsString, True)
                End If
            Else
                lblErrorMessage.Text = "The user email already exists. Please use a different email address.<br>Already a member? Click <a class='loginlink' onclick=""parent.location.href='http://www.koreprojects.com/login.asp';"" href='#'>here</a> to login."
            End If
        Else
            lblErrorMessage.Text = "The user email already exists. Please use a different email address.<br>Already a member? Click <a class='loginlink' onclick=""parent.location.href='http://www.koreprojects.com/login.asp';"" href='#'>here</a> to login."
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim redirectTolocalPage As Boolean = True
        If Not Request.UrlReferrer Is Nothing Then
            If Request.UrlReferrer.ToString().ToLower() = "http://www.koreprojects.com/signup.aspx" Or Request.UrlReferrer.ToString().ToLower() = "http://koreprojects.com/signup.aspx" Then
                redirectTolocalPage = False
            End If
        End If
        If Request.QueryString("msg") <> String.Empty Then
            redirectTolocalPage = False
        End If
        'If redirectTolocalPage And Not IsPostBack Then
        '    m_jsString = "parent.location.href='signup/default.aspx';"
        '    Page.ClientScript.RegisterStartupScript(Me.GetType(), "signup", m_jsString, True)
        'End If

        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
    End Sub
End Class
