Imports PayPal.Api
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services

Partial Class Accounts_PaypalWebhook
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
            Dim config = ConfigManager.Instance.GetProperties()
            Dim accessToken = New OAuthTokenCredential(config).GetAccessToken()
            Dim apiContext = New APIContext(accessToken)

            ' Get the received request's headers
            Dim requestheaders = HttpContext.Current.Request.Headers

            ' Get the received request's body
            Dim requestBody = String.Empty
            Dim memstream As New MemoryStream()
            HttpContext.Current.Request.InputStream.CopyTo(memstream)
            memstream.Position = 0
            Using reader = New System.IO.StreamReader(memstream)
                requestBody = reader.ReadToEnd()
            End Using

            Dim jsonBody As Object = JObject.Parse(requestBody)
            Dim webhookId As String = jsonBody.SelectToken("id").ToString()
            Dim ev = WebhookEvent.[Get](apiContext, webhookId)
            ' We have all the information the SDK needs, so perform the validation.
            ' Note: at least on Sandbox environment this returns false.
            ' var isValid = WebhookEvent.ValidateReceivedEvent(apiContext, ToNameValueCollection(requestheaders), requestBody, webhookId);

            Select Case ev.event_type
                Case "BILLING.SUBSCRIPTION.CANCELLED"
                    m_ManagementService.CancelBillingSubscription(ev.resource.SelectToken("id").ToString())
                    Exit Select
                Case "PAYMENT.SALE.COMPLETED"
                    Dim agreementID As String = ev.resource.SelectToken("billing_agreement_id").ToString()
                    Dim lastPaymentDate As DateTime
                    Dim lastPaymentAmount As Decimal
                    Dim nextBillingDate As DateTime

                    Dim agreement As Agreement = agreement.Get(apiContext, agreementID)

                    If Not agreement Is Nothing Then
                        If Not agreement.agreement_details Is Nothing AndAlso _
                            Not agreement.agreement_details.last_payment_date Is Nothing AndAlso _
                            Not agreement.agreement_details.next_billing_date Is Nothing AndAlso _
                            Not agreement.agreement_details.last_payment_amount Is Nothing Then

                            lastPaymentDate = Convert.ToDateTime(agreement.agreement_details.last_payment_date)
                            nextBillingDate = Convert.ToDateTime(agreement.agreement_details.next_billing_date)
                            lastPaymentAmount = Convert.ToDecimal(agreement.agreement_details.last_payment_amount.value)
                            m_ManagementService.UpdateBillingSubscription(agreementID, lastPaymentDate, lastPaymentAmount, nextBillingDate)
                        End If
                    End If

                    Exit Select
                Case Else
                    ' Handle other webhooks
                    Exit Select
            End Select
        Catch ex As Exception
            m_ManagementService.LogError(ex.Message, "")
        End Try
    End Sub
End Class
