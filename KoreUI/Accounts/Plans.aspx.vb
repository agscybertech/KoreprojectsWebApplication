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
Imports PayPal.Api

Partial Class Accounts_Plans
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Public m_ServicePlanCount As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblMsg.Text = ""
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Not IsPostBack Then
            lblTitle.Text = "Settings"

            Dim ProjectOwnerId As Long
            ProjectOwnerId = m_ManagementService.GetProjectOwnerByContactId(m_LoginUser.UserId).ProjectOwnerId

            Dim token As String = Request.Params("token")

            If Not String.IsNullOrEmpty(token) Then
                Dim config = ConfigManager.Instance.GetProperties()
                Dim accessToken = New OAuthTokenCredential(config).GetAccessToken()
                Dim apiContext = New APIContext(accessToken)

                Dim agreement As New Agreement() With {.token = token}
                Dim executedAgreement As Agreement = agreement.Execute(apiContext)

                If Not executedAgreement Is Nothing AndAlso executedAgreement.state.ToLower() = "active" Then
                    Dim agreementId As String = If(executedAgreement.id, "")
                    Dim lastPaymentDate As DateTime = DateTime.Now
                    Dim NextPaymentDate As DateTime = DateTime.Now.AddMonths(1)
                    Dim noOfUsers As Integer = Convert.ToInt32(If(Not Session("NoOfUsersSubscribed") Is Nothing, Session("NoOfUsersSubscribed"), 0))
                    Dim amount As Decimal = 12 + (Convert.ToInt32(If(Not Session("NoOfUsersSubscribed") Is Nothing, Session("NoOfUsersSubscribed"), 0)) * 5)

                    If Not executedAgreement.agreement_details Is Nothing Then
                        If Not executedAgreement.agreement_details.last_payment_date Is Nothing Then
                            lastPaymentDate = Convert.ToDateTime(executedAgreement.agreement_details.last_payment_date)
                        End If

                        If Not executedAgreement.agreement_details.next_billing_date Is Nothing Then
                            NextPaymentDate = Convert.ToDateTime(executedAgreement.agreement_details.next_billing_date)
                        End If
                    End If

                    If Not executedAgreement.agreement_details Is Nothing AndAlso _
                        Not executedAgreement.agreement_details.last_payment_amount Is Nothing AndAlso _
                        Convert.ToDecimal(executedAgreement.agreement_details.last_payment_amount.value) = amount Then
                        trPayment.Visible = False
                        trPaymentSucess.Visible = True
                        Try
                            m_ManagementService.SavePaypalBillingAgreement(m_LoginUser.UserId, True, lastPaymentDate, agreementId, "Monthly", amount, NextPaymentDate)
                            Dim ds As DataSet = m_ManagementService.UpdateSubscription(m_LoginUser.UserId, Convert.ToInt32(If(Not Session("NoOfUsersSubscribed") Is Nothing, Session("NoOfUsersSubscribed"), 0)), NextPaymentDate)

                            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                                If Not IsDBNull(ds.Tables(0).Rows(0)("NextBillingDate")) Then
                                    lblExpirationDate.ForeColor = Drawing.Color.Green
                                    lblExpirationDate.Text = "Your payment has been processed successfully. Your subscription is updated and will now expire on " & Convert.ToDateTime(ds.Tables(0).Rows(0)("NextBillingDate")).ToString("dd/MM/yyyy")
                                    litSubscriptionEndDate.Text = Convert.ToDateTime(ds.Tables(0).Rows(0)("NextBillingDate")).ToString("dd/MM/yyyy")
                                End If
                            End If

                            GetBillingAgreementDetail()
                        Catch ex As Exception
                            lblExpirationDate.ForeColor = Drawing.Color.Red
                            lblExpirationDate.Text = "Error ocurred while upading your subscription."
                        End Try
                    End If

                    Session.Remove("NoOfUsersSubscribed")
                    Session.Remove("NextBillingDate")
                Else
                    trPaymentSucess.Visible = False
                    trPayment.Visible = True
                    lblMsg.Text = "Error ocurred while processing your payment."
                    LoadPlanSelection()
                End If
            Else
                LoadPlanSelection()
            End If
        End If
    End Sub

    Private Sub LoadPlanSelection()
        Try
            Dim CurrentPlanId As Integer = 0
            Dim ValidCreditCard = False
            Dim PlanChanged = False
            Dim dsUserAccount As DataSet
            dsUserAccount = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
            If dsUserAccount.Tables.Count > 0 Then
                If dsUserAccount.Tables(0).Rows.Count > 0 Then
                    hdnTotalActiveUsers.Value = dsUserAccount.Tables(0).Rows(0)("totalActiveUsers")
                    If Not ddlUsers.Items.FindByValue(dsUserAccount.Tables(0).Rows(0)("totalActiveUsers")) Is Nothing Then
                        ddlUsers.Items.FindByValue(dsUserAccount.Tables(0).Rows(0)("totalActiveUsers")).Selected = True
                        lblTotal.Text = "$" & ((Convert.ToInt32(dsUserAccount.Tables(0).Rows(0)("totalActiveUsers")) * 5) + 12)
                    End If

                    If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("NextBillingDate")) Then
                        litSubscriptionEndDate.Text = Convert.ToDateTime(dsUserAccount.Tables(0).Rows(0)("NextBillingDate")).ToString("dd/MM/yyyy")
                    End If
                End If
            End If

            Dim userProfile As UserProfile
            userProfile = m_ManagementService.GetUserProfileByUserID(m_LoginUser.UserId)

            GetBillingAgreementDetail()

            ddlYear.Items.Clear()
            Dim listItem As New ListItem
            listItem.Text = "Select Year"
            listItem.Value = "0"
            ddlYear.Items.Add(listItem)
            For i As Integer = 0 To 19
                listItem = New ListItem()
                listItem.Text = DateTime.Now.Year + i
                listItem.Value = DateTime.Now.Year + i
                ddlYear.Items.Add(listItem)
            Next
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Public Function IsPlanEnable(ByVal PlanId As String, ByVal available As String) As String
        Dim result As String = Boolean.TrueString
        If available = "1" Then
            Dim CurrentPlanId As Integer = 0
            Dim dsUserAccount As DataSet
            dsUserAccount = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
            If dsUserAccount.Tables.Count > 0 Then
                If dsUserAccount.Tables(0).Rows.Count > 0 Then
                    If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("PlanId")) Then
                        CurrentPlanId = dsUserAccount.Tables(0).Rows(0)("PlanId")
                    End If
                End If
            End If
            If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("NextBillingDate")) And CurrentPlanId > 0 And CurrentPlanId.ToString() = PlanId Then
                result = False
            End If
        Else
            result = False
        End If
        Return result
    End Function

    Public Function ShowNumberOfProjects(ByVal numberofProjects As String) As String
        Dim result As String = numberofProjects
        If CInt(numberofProjects) = 99999 Then
            result = "Unlimited"
        End If
        Return result
    End Function

    Protected Sub ibtnPay_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnPay.Click
        Try
            If rbCreditCard.Checked Then
                If String.IsNullOrEmpty(txtCardNumber.Text.Trim()) Then
                    lblMsg.Text = "Card Number is required"
                    txtCardNumber.Focus()
                    Exit Sub
                End If

                If ddlMonth.SelectedValue = "0" Then
                    lblMsg.Text = "Expiry Date month is required"
                    ddlMonth.Focus()
                    Exit Sub
                End If

                If ddlYear.SelectedValue = "0" Then
                    lblMsg.Text = "Expiry Date year is required"
                    ddlYear.Focus()
                    Exit Sub
                End If

                If String.IsNullOrEmpty(txtCardCVV.Text.Trim()) Then
                    lblMsg.Text = "CVV is required"
                    txtCardCVV.Focus()
                    Exit Sub
                End If

                If String.IsNullOrEmpty(txtFirstName.Text.Trim()) Then
                    lblMsg.Text = "First Name is required"
                    txtFirstName.Focus()
                    Exit Sub
                End If

                If String.IsNullOrEmpty(txtLastName.Text.Trim()) Then
                    lblMsg.Text = "Last Name is required"
                    txtLastName.Focus()
                    Exit Sub
                End If

                If String.IsNullOrEmpty(txtAddress.Text.Trim()) Then
                    lblMsg.Text = "Address is required"
                    txtAddress.Focus()
                    Exit Sub
                End If

                If String.IsNullOrEmpty(txtCity.Text.Trim()) Then
                    lblMsg.Text = "City is required"
                    txtCity.Focus()
                    Exit Sub
                End If

                If String.IsNullOrEmpty(txtPostalCode.Text.Trim()) Then
                    lblMsg.Text = "Postal Code is required"
                    txtPostalCode.Focus()
                    Exit Sub
                End If

            End If

            'Authenticate with PayPal
            Dim config = ConfigManager.Instance.GetProperties()
            Dim accessToken = New OAuthTokenCredential(config).GetAccessToken()
            Dim apiContext = New APIContext(accessToken)

            Dim price As New Currency()
            price.value = 12 + (Convert.ToInt32(ddlUsers.SelectedValue) * 5)
            price.currency = "USD"

            Dim plan As New Plan
            plan.name = "Koreprojects Monthly Subscription"
            plan.description = "Koreprojects Monthly Subscription"
            plan.type = "INFINITE"

            Dim paymentDefinition As New PaymentDefinition
            paymentDefinition.name = "Standard Plan"
            paymentDefinition.type = "REGULAR"
            paymentDefinition.frequency = "MONTH"
            paymentDefinition.frequency_interval = "1"
            paymentDefinition.amount = price

            plan.payment_definitions = New List(Of PaymentDefinition)
            plan.payment_definitions.Add(paymentDefinition)

            Dim guid As String = System.Guid.NewGuid.ToString()
            Dim merchantPreferences As New MerchantPreferences
            merchantPreferences.setup_fee = price
            merchantPreferences.return_url = Request.Url.Scheme & "://" + Request.Url.Authority & Request.ApplicationPath & "/Accounts/Plans.aspx?guid=" & guid
            merchantPreferences.cancel_url = Request.Url.Scheme & "://" + Request.Url.Authority & Request.ApplicationPath & "/Accounts/Plans.aspx"
            merchantPreferences.auto_bill_amount = "YES"
            merchantPreferences.initial_fail_amount_action = "CANCEL"
            merchantPreferences.max_fail_attempts = "0"

            plan.merchant_preferences = merchantPreferences

            Dim createdPlan As Plan = plan.Create(apiContext)

            Dim tempPlan As New Plan()
            tempPlan.state = "ACTIVE"

            Dim PatchRequest As New PatchRequest()
            PatchRequest.Add(New Patch() With {.op = "replace", .path = "/", .value = tempPlan})

            createdPlan.Update(apiContext, PatchRequest)

            Dim payer As New Payer()
            Dim billingAddress As New Address
            If rbCreditCard.Checked Then
                payer.payment_method = "CREDIT_CARD"

                Dim fundingInstrument As New FundingInstrument
                Dim createdCard As New CreditCard
                createdCard.type = ddlCardType.SelectedValue
                createdCard.number = txtCardNumber.Text  '4032038703494082
                createdCard.first_name = txtFirstName.Text
                createdCard.last_name = txtLastName.Text
                createdCard.expire_month = ddlMonth.SelectedValue '8
                createdCard.expire_year = ddlYear.SelectedValue '2022
                createdCard.cvv2 = txtCardCVV.Text '874

                billingAddress.line1 = txtAddress.Text
                billingAddress.city = txtCity.Text
                billingAddress.state = ""
                billingAddress.postal_code = txtPostalCode.Text
                billingAddress.country_code = ddlCountry.SelectedValue
                createdCard.billing_address = billingAddress

                fundingInstrument.credit_card = createdCard
                payer.funding_instruments = New List(Of FundingInstrument)()
                payer.funding_instruments.Add(fundingInstrument)
            Else
                payer.payment_method = "paypal"
            End If



            Dim agreement As New Agreement()
            agreement.name = "Koreprojects Monthly Subscription"
            agreement.description = "Koreprojects Monthly Subscription"
            agreement.start_date = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time")).AddMonths(1).ToString("yyyy-MM-ddThh:mm:ssZ")
            agreement.payer = payer
            If rbCreditCard.Checked Then
                agreement.shipping_address = billingAddress
            End If
            agreement.plan = New Plan() With {.id = createdPlan.id}

            Session("NoOfUsersSubscribed") = ddlUsers.SelectedValue
            Dim createdAgreement As Agreement = agreement.Create(apiContext)

            If rbCreditCard.Checked Then
                If Not createdAgreement.id Is Nothing Then
                    Dim agreementId As String = If(createdAgreement.id, "")
                    Dim lastPaymentDate As DateTime = DateTime.Now
                    Dim NextPaymentDate As DateTime = DateTime.Now.AddMonths(1)
                    Dim noOfUsers As Integer = Convert.ToInt32(If(Not Session("NoOfUsersSubscribed") Is Nothing, Session("NoOfUsersSubscribed"), 0))
                    Dim amount As Decimal = 12 + (Convert.ToInt32(If(Not Session("NoOfUsersSubscribed") Is Nothing, Session("NoOfUsersSubscribed"), 0)) * 5)

                    If Not createdAgreement.agreement_details Is Nothing Then
                        If Not createdAgreement.agreement_details.last_payment_date Is Nothing Then
                            lastPaymentDate = Convert.ToDateTime(createdAgreement.agreement_details.last_payment_date)
                        End If

                        If Not createdAgreement.agreement_details.next_billing_date Is Nothing Then
                            NextPaymentDate = Convert.ToDateTime(createdAgreement.agreement_details.next_billing_date)
                        End If
                    End If

                    If createdAgreement.state.ToLower() = "active" AndAlso
                        Not createdAgreement.agreement_details Is Nothing AndAlso
                        Not createdAgreement.agreement_details.last_payment_amount Is Nothing AndAlso
                        Convert.ToDecimal(createdAgreement.agreement_details.last_payment_amount.value) = amount Then
                        trPayment.Visible = False
                        trPaymentSucess.Visible = True
                        Try
                            m_ManagementService.SavePaypalBillingAgreement(m_LoginUser.UserId, True, lastPaymentDate, agreementId, "Monthly", amount, NextPaymentDate)
                            Dim ds As DataSet = m_ManagementService.UpdateSubscription(m_LoginUser.UserId, Convert.ToInt32(If(Not Session("NoOfUsersSubscribed") Is Nothing, Session("NoOfUsersSubscribed"), 0)), NextPaymentDate)

                            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0).Rows.Count > 0 Then
                                If Not IsDBNull(ds.Tables(0).Rows(0)("NextBillingDate")) Then
                                    lblExpirationDate.ForeColor = Drawing.Color.Green
                                    lblExpirationDate.Text = "Your payment has been processed successfully. Your subscription is updated and will now expire on " & Convert.ToDateTime(ds.Tables(0).Rows(0)("NextBillingDate")).ToString("dd/MM/yyyy")
                                    litSubscriptionEndDate.Text = Convert.ToDateTime(ds.Tables(0).Rows(0)("NextBillingDate")).ToString("dd/MM/yyyy")
                                End If
                            End If

                            GetBillingAgreementDetail()
                        Catch ex As Exception
                            lblExpirationDate.ForeColor = Drawing.Color.Red
                            lblExpirationDate.Text = "Error ocurred while upading your subscription."
                        End Try
                    Else
                        trPaymentSucess.Visible = False
                        trPayment.Visible = True
                        lblMsg.Text = "Error ocurred while processing your payment."
                    End If

                    Session.Remove("NoOfUsersSubscribed")
                    Session.Remove("NextBillingDate")
                Else
                    LoadPlanSelection()
                End If
            Else
                Dim links = createdAgreement.links.GetEnumerator()
                While (links.MoveNext())
                    Dim link = links.Current
                    If (link.rel.ToLower().Trim().Equals("approval_url")) Then
                        Response.Redirect(link.href, False)
                    End If
                End While
            End If
        Catch ex As Exception
            If TypeOf ex Is PayPal.ConnectionException Then
                m_ManagementService.LogError(DirectCast(ex, PayPal.ConnectionException).Response,"")
            End If
            m_ManagementService.LogError(ex.Message, ex.StackTrace)
            lblMsg.Text = "Unknown error ocurred while processing payment."
        End Try
    End Sub

    Private Sub GetBillingAgreementDetail()
        Try
            Dim dsBillingAgreement As DataSet = m_ManagementService.GetActiveBillingAgreementByUser(m_LoginUser.UserId)

            If Not dsBillingAgreement Is Nothing AndAlso dsBillingAgreement.Tables.Count > 0 AndAlso dsBillingAgreement.Tables(0).Rows.Count > 0 Then
                lblLastPaymentDate.Text = Convert.ToDateTime(dsBillingAgreement.Tables(0).Rows(0)("LastPaymentDate")).ToString("dd-MM-yyyy")
                lblLastPaymentAmount.Text = "$" & Convert.ToDecimal(dsBillingAgreement.Tables(0).Rows(0)("LastPaymentAmount"))
                lblNextBillingDate.Text = Convert.ToDateTime(dsBillingAgreement.Tables(0).Rows(0)("NextPaymentDate")).ToString("dd-MM-yyyy")
                hdnPaypalBillingAgreementID.Value = Convert.ToString(dsBillingAgreement.Tables(0).Rows(0)("PaypalAgreementID"))

                trPaymentSucess.Visible = True
                trPayment.Visible = False
            Else
                trPaymentSucess.Visible = False
                trPayment.Visible = True
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Protected Sub lnkCancelSubscription_Click(sender As Object, e As EventArgs) Handles lnkCancelSubscription.Click
        Try
            'Authenticate with PayPal
            Dim config = ConfigManager.Instance.GetProperties()
            Dim accessToken = New OAuthTokenCredential(config).GetAccessToken()
            Dim apiContext = New APIContext(accessToken)

            Dim agreement As New Agreement() With {.id = hdnPaypalBillingAgreementID.Value}
            agreement.Cancel(apiContext, New AgreementStateDescriptor() With {.note = "Agreement Cancelled By User."})

            m_ManagementService.CancelBillingSubscription(hdnPaypalBillingAgreementID.Value)

            Response.Redirect("~/Accounts/Plans.aspx", False)
        Catch ex As Exception
            lblMsg.Text = "Unknown error ocurred while cancelling your subscription."
        End Try
    End Sub
End Class
