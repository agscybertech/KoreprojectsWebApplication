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
Imports System.Xml

Partial Class Accounts_Thankyou
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()

    Protected Sub btnContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinue.Click
        If Session("SelecrPlanReferrer") = Nothing Then
            Response.Redirect("../Projects/edit.aspx")
        Else
            Dim returnUrl As String = String.Empty
            returnUrl = Session("SelecrPlanReferrer")
            Session("SelecrPlanReferrer") = Nothing
            Response.Redirect(returnUrl)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        If Request.QueryString("pay") <> "" Then
            If m_Cryption.Decrypt(Request.QueryString("pay"), m_Cryption.cryptionKey) = "1" Then
                If Session("CCAmount") <> Nothing Then
                    ' Check and update project credit
                    Dim projectCredit As Integer = 0
                    Dim firstBillingDate As DateTime
                    Dim lastBillingDate As DateTime
                    Dim dsUserAccount As DataSet = New DataSet()
                    dsUserAccount = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
                    If dsUserAccount.Tables.Count > 0 Then
                        If dsUserAccount.Tables(0).Rows.Count > 0 Then
                            If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("ProjectCredit")) Then
                                Select Case Session("Type")
                                    Case 1
                                        projectCredit = dsUserAccount.Tables(0).Rows(0)("ProjectCredit")
                                    Case 2
                                        If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("FirstBillingDate")) Then
                                            firstBillingDate = dsUserAccount.Tables(0).Rows(0)("FirstBillingDate")
                                        Else
                                            firstBillingDate = Today
                                        End If
                                        If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("LastBillingDate")) Then
                                            lastBillingDate = dsUserAccount.Tables(0).Rows(0)("LastBillingDate")
                                        Else
                                            lastBillingDate = Today
                                        End If
                                End Select
                            End If
                        End If
                    End If

                    Select Case Session("Type")
                        Case 1
                            m_ManagementService.UpdateUserAccount(m_LoginUser.UserId, projectCredit + Session("NumberOfProjectCredits"))
                            m_ManagementService.CreateUserTransaction(m_LoginUser.UserId, String.Format("Top up project credit"), Session("CCAmount"), 0, Session("NumberOfProjectCredits"), projectCredit + Session("numberOfProjectCredits"))
                            If Session("NumberOfProjectCredits") <> Nothing Then
                                Dim MailMessage As New System.Net.Mail.MailMessage()
                                Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
                                'MailMessage.To.Add(New System.Net.Mail.MailAddress(ContactProfile.Email))
                                MailMessage.To.Add(New System.Net.Mail.MailAddress(m_LoginUser.Email))
                                MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("AdminEmail"))

                                MailMessage.Subject = String.Format("Thank you for top-up {0}", Request.Url.Host.Replace("www.", String.Empty))

                                If Session("NumberOfProjectCredits") = 1 Then
                                    MailMessage.Body = String.Format("Hi there,<br><br>Your account has been top up with {0} project credit.<br>To visit Kore Projects please use the link below:<br><br>http://{1}<br><br>The Kore Projects Team", projectCredit, Request.Url.Authority)
                                Else
                                    MailMessage.Body = String.Format("Hi there,<br><br>Your account has been top up with {0} project credits.<br>To visit Kore Projects please use the link below:<br><br>http://{1}<br><br>The Kore Projects Team", projectCredit, Request.Url.Authority)
                                End If

                                MailMessage.IsBodyHtml = True
                                Try
                                    emailClient.Send(MailMessage)
                                Catch ex As Exception
                                    'Response.Redirect(String.Format("View.aspx?&msg=Invitation hasn't been sent - {0}", ex.Message))
                                    Throw
                                End Try
                            End If
                        Case 2
                            Dim NextBillingDate As DateTime
                            Dim EstimateBillingDate As DateTime
                            EstimateBillingDate = DateAdd(DateInterval.Month, Session("Term"), lastBillingDate)
                            Dim strNextBillingDate As String = String.Format("{0}-{1}-{2}", Year(EstimateBillingDate), Month(EstimateBillingDate), Day(firstBillingDate))
                            If Not DateTime.TryParse(strNextBillingDate, NextBillingDate) Then
                                NextBillingDate = New DateTime(Year(EstimateBillingDate), Month(EstimateBillingDate), 1)
                                NextBillingDate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, NextBillingDate))
                            End If
                            m_ManagementService.UpdateUserAccountMonthly(m_LoginUser.UserId, Session("PlanId"), Session("NumberOfProjects"), Session("StorageSize"), NextBillingDate)
                            m_ManagementService.CreateUserTransaction(m_LoginUser.UserId, String.Format("Monthly Subscription"), Session("CCAmount"), 0, Session("NumberOfProjects"), Session("numberOfProjects"))
                            Dim requestUrl As String = String.Format("https://payments.ebizsecure.co.nz/ppe/pmtservices_gw1/gateway/interface_recurring_paymentinfo.asp?mid={0}", ConfigurationManager.AppSettings("EBIZID"))
                            requestUrl = String.Format("{0}&rid={1}***{2:yyyyMMddHHmmss}&rrid={1}***{2:yyyyMMddHHmmss}", requestUrl, m_LoginUser.UserId, DateTime.Now)

                            Dim doc As XmlDocument = New XmlDocument()
                            doc.Load(requestUrl)
                            m_ManagementService.UpdateUserAccountMonthlyPaymentInfo_ebizsecure(m_LoginUser.UserId, doc)

                            If Session("NumberOfProjects") <> Nothing Then
                                Dim MailMessage As New System.Net.Mail.MailMessage()
                                Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
                                'MailMessage.To.Add(New System.Net.Mail.MailAddress(ContactProfile.Email))
                                MailMessage.To.Add(New System.Net.Mail.MailAddress(m_LoginUser.Email))
                                MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("AdminEmail"))

                                MailMessage.Subject = String.Format("Thank you for Subscription {0}", Request.Url.Host.Replace("www.", String.Empty))

                                If Session("NumberOfProjects") = 1 Then
                                    MailMessage.Body = String.Format("Hi there,<br><br>Your account has been subscribed with {0} project.<br>To visit Kore Projects please use the link below:<br><br>http://{1}<br><br>The Kore Projects Team", Session("NumberOfProjects"), Request.Url.Authority)
                                Else
                                    MailMessage.Body = String.Format("Hi there,<br><br>Your account has been subscribed with {0} projects.<br>To visit Kore Projects please use the link below:<br><br>http://{1}<br><br>The Kore Projects Team", Session("NumberOfProjects"), Request.Url.Authority)
                                End If

                                MailMessage.IsBodyHtml = True
                                Try
                                    emailClient.Send(MailMessage)
                                Catch ex As Exception
                                    'Response.Redirect(String.Format("View.aspx?&msg=Invitation hasn't been sent - {0}", ex.Message))
                                    Throw
                                End Try
                            End If
                    End Select

                    'Clear session
                    Session("CCAmount") = Nothing
                    Session("PlanId") = Nothing
                    Session("NumberOfProjectCredits") = Nothing
                    Session("NumberOfProjects") = Nothing
                    Session("StorageSize") = Nothing
                    Session("Term") = Nothing
                    Session("Type") = Nothing
                    Session("RecurringBillingId") = Nothing
                End If
            Else
                Response.Redirect("SelectPlan.aspx?msg=Your payment is not successful, please try again or contact administrator.")
            End If
        Else
            Response.Redirect("SelectPlan.aspx?msg=Your payment is not successful, please try again or contact administrator.")
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
