Imports System.Collections
Imports System.Collections.Specialized
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Collections.Generic
Imports System.Linq
Imports PayPal.PayPalAPIInterfaceService
Imports PayPal.PayPalAPIInterfaceService.Model

Public Class PaypalPayment
    'Flag that determines the PayPal environment (live or sandbox)
    Private Const bSandbox As Boolean = True
    Private Const CVV2 As String = "CVV2"

    ' Live strings.
    Private pEndPointURL As String = "https://api-3t.paypal.com/nvp"
    Private host As String = "www.paypal.com"

    ' Sandbox strings.
    Private pEndPointURL_SB As String = "https://api-3t.sandbox.paypal.com/nvp"
    Private host_SB As String = "www.sandbox.paypal.com"

    Public APIUsername As String = "sandeep-facilitator_api1.bizautomation.com"
    Private APIPassword As String = "8ERNLVBRQZGGWFYE"
    Private APISignature As String = "AOxNGOMvuwGEjEKMMOo5ZOs0G9dcA3ONAE0xIXffjQar1GZlVF8GuD2p"



    ''' <summary>
    ''' Handles Set ExpressCheckout Payment Order
    ''' </summary>
    ''' <param name="contextHttp"></param>
    Private Sub SetExpressCheckoutPaymentOrder(contextHttp As HttpContext,
                                               ByVal currencyCode As String,
                                               ByVal paymentType As String,
                                               ByVal buyerEmail As String,
                                               ByVal itemAmount As String,
                                               ByVal itemQty As String,
                                               ByVal itemName As String,
                                               ByVal itemCategory As String,
                                               ByVal salesTax As String,
                                               ByVal shippingCharge As String,
                                               ByVal insuranceTotal As String,
                                               ByVal handlingCharge As String,
                                               ByVal totalTax As String,
                                               ByVal orderDescription As String,
                                               ByVal notifyURL As String)
        ' Configuration map containing signature credentials and other required configuration.
        ' For a full list of configuration parameters refer in wiki page 
        ' [https://github.com/paypal/sdk-core-dotnet/wiki/SDK-Configuration-Parameters]
        Dim configurationMap As Dictionary(Of String, String) = PayPal.Manager.ConfigManager.Instance.GetProperties()

        'TODO: change sandbox to live
        'configurationMap.Add("mode", "sandbox")
        'configurationMap.Add("account1.apiUsername", "jb-us-seller_api1.paypal.com")
        'configurationMap.Add("account1.apiPassword", "...")
        'configurationMap.Add("account1.apiSignature", "...")

        ' Create the PayPalAPIInterfaceServiceService service object to make the API call
        Dim service As New PayPalAPIInterfaceServiceService(configurationMap)

        Dim setExpressCheckoutReq As New SetExpressCheckoutRequestType()
        Dim details As New SetExpressCheckoutRequestDetailsType()

        'TODO: Change Sandbox to live URL
        Dim requestUrl As String = pEndPointURL_SB

        ' (Required) URL to which the buyer's browser is returned after choosing to pay with PayPal. For digital goods, you must add JavaScript to this page to close the in-context experience.
        ' Note:
        ' PayPal recommends that the value be the final review page on which the buyer confirms the order and payment or billing agreement.
        Dim uriBuilder As New UriBuilder(requestUrl)
        uriBuilder.Path = contextHttp.Request.ApplicationPath & (If(contextHttp.Request.ApplicationPath.EndsWith("/"), String.Empty, "/")) & "Accounts/DoExpressCheckout.aspx"
        Dim returnUrl As String = uriBuilder.Uri.ToString()

        ' (Required) URL to which the buyer is returned if the buyer does not approve the use of PayPal to pay you. For digital goods, you must add JavaScript to this page to close the in-context experience.
        ' Note:
        ' PayPal recommends that the value be the original page on which the buyer chose to pay with PayPal or establish a billing agreement.
        uriBuilder = New UriBuilder(requestUrl)
        uriBuilder.Path = contextHttp.Request.ApplicationPath & (If(contextHttp.Request.ApplicationPath.EndsWith("/"), String.Empty, "/")) & "Accounts/Subscription.aspx"
        Dim cancelUrl As String = uriBuilder.Uri.ToString()

        ' (Required) URL to which the buyer's browser is returned after choosing 
        ' to pay with PayPal. For digital goods, you must add JavaScript to this 
        ' page to close the in-context experience.
        ' Note:
        ' PayPal recommends that the value be the final review page on which the buyer 
        ' confirms the order and payment or billing agreement.
        ' Character length and limitations: 2048 single-byte characters
        details.ReturnURL = (returnUrl & Convert.ToString("?currencyCodeType=")) & currencyCode & "&paymentType=" & paymentType
        details.CancelURL = cancelUrl

        ' (Optional) Email address of the buyer as entered during checkout.
        ' PayPal uses this value to pre-fill the PayPal membership sign-up portion on the PayPal pages.
        ' Character length and limitations: 127 single-byte alphanumeric characters
        details.BuyerEmail = buyerEmail

        Dim itemTotal As Decimal = 0D
        Dim orderTotal As Decimal = 0D

        ' Cost of item. This field is required when you pass a value for ItemCategory.
        Dim amountItems As String = itemAmount

        ' Item quantity. This field is required when you pass a value for ItemCategory. 
        ' For digital goods (ItemCategory=Digital), this field is required.
        ' Character length and limitations: Any positive integer
        ' This field is introduced in version 53.0. 
        Dim qtyItems As String = itemQty

        ' Item name. This field is required when you pass a value for ItemCategory.
        ' Character length and limitations: 127 single-byte characters
        ' This field is introduced in version 53.0. 
        Dim names As String = itemName

        Dim lineItems As New List(Of PaymentDetailsItemType)()
        Dim item As New PaymentDetailsItemType()
        Dim amt As New BasicAmountType()

        ' PayPal uses 3-character ISO-4217 codes for specifying currencies in fields and variables. 
        amt.currencyID = DirectCast([Enum].Parse(GetType(CurrencyCodeType), currencyCode), CurrencyCodeType)
        amt.value = amountItems
        item.Quantity = Convert.ToInt32(qtyItems)
        item.Name = names
        item.Amount = amt

        ' Indicates whether an item is digital or physical. For digital goods, this field is required and must be set to Digital. It is one of the following values:
        ' 1.Digital
        ' 2.Physical
        ' This field is available since version 65.1. 
        item.ItemCategory = DirectCast([Enum].Parse(GetType(ItemCategoryType), itemCategory), ItemCategoryType)
        lineItems.Add(item)

        ' (Optional) Item sales tax.
        ' Note: You must set the currencyID attribute to one of 
        ' the 3-character currency codes for any of the supported PayPal currencies.
        ' Character length and limitations: Value is a positive number which cannot exceed $10,000 USD in any currency.
        ' It includes no currency symbol. It must have 2 decimal places, the decimal separator must be a period (.), 
        ' and the optional thousands separator must be a comma (,).
        If salesTax <> String.Empty Then
            item.Tax = New BasicAmountType(DirectCast([Enum].Parse(GetType(CurrencyCodeType), currencyCode), CurrencyCodeType), salesTax)
        End If

        itemTotal += Convert.ToDecimal(qtyItems) * Convert.ToDecimal(amountItems)
        orderTotal += itemTotal

        Dim payDetails As New List(Of PaymentDetailsType)()
        Dim paydtl As New PaymentDetailsType()

        ' How you want to obtain payment. When implementing parallel payments, 
        ' this field is required and must be set to Order.
        ' When implementing digital goods, this field is required and must be set to Sale.
        ' If the transaction does not include a one-time purchase, this field is ignored. 
        ' It is one of the following values:
        ' Sale – This is a final sale for which you are requesting payment (default).
        ' Authorization – This payment is a basic authorization subject to settlement with PayPal Authorization and Capture.
        ' Order – This payment is an order authorization subject to settlement with PayPal Authorization and Capture.
        paydtl.PaymentAction = DirectCast([Enum].Parse(GetType(PaymentActionCodeType), paymentType), PaymentActionCodeType)

        ' (Optional) Total shipping costs for this order.
        ' Note:
        ' You must set the currencyID attribute to one of the 3-character currency codes 
        ' for any of the supported PayPal currencies.
        ' Character length and limitations: 
        ' Value is a positive number which cannot exceed $10,000 USD in any currency.
        ' It includes no currency symbol. 
        ' It must have 2 decimal places, the decimal separator must be a period (.), 
        ' and the optional thousands separator must be a comma (,)
        If shippingCharge <> String.Empty Then
            Dim shippingTotal As New BasicAmountType()
            shippingTotal.value = shippingCharge
            shippingTotal.currencyID = DirectCast([Enum].Parse(GetType(CurrencyCodeType), currencyCode), CurrencyCodeType)
            orderTotal += Convert.ToDecimal(shippingCharge)
            paydtl.ShippingTotal = shippingTotal
        End If

        ' (Optional) Total shipping insurance costs for this order. 
        ' The value must be a non-negative currency amount or null if you offer insurance options.
        ' Note:
        ' You must set the currencyID attribute to one of the 3-character currency 
        ' codes for any of the supported PayPal currencies.
        ' Character length and limitations: 
        ' Value is a positive number which cannot exceed $10,000 USD in any currency. 
        ' It includes no currency symbol. It must have 2 decimal places,
        ' the decimal separator must be a period (.), 
        ' and the optional thousands separator must be a comma (,).
        ' InsuranceTotal is available since version 53.0.
        If insuranceTotal <> String.Empty Then
            paydtl.InsuranceTotal = New BasicAmountType(DirectCast([Enum].Parse(GetType(CurrencyCodeType), currencyCode), CurrencyCodeType), insuranceTotal)
            paydtl.InsuranceOptionOffered = "true"
            orderTotal += Convert.ToDecimal(insuranceTotal)
        End If

        ' (Optional) Total handling costs for this order.
        ' Note:
        ' You must set the currencyID attribute to one of the 3-character currency codes 
        ' for any of the supported PayPal currencies.
        ' Character length and limitations: Value is a positive number which 
        ' cannot exceed $10,000 USD in any currency.
        ' It includes no currency symbol. It must have 2 decimal places, 
        ' the decimal separator must be a period (.), and the optional 
        ' thousands separator must be a comma (,). 
        If handlingCharge <> String.Empty Then
            paydtl.HandlingTotal = New BasicAmountType(DirectCast([Enum].Parse(GetType(CurrencyCodeType), currencyCode), CurrencyCodeType), handlingCharge)
            orderTotal += Convert.ToDecimal(handlingCharge)
        End If

        ' (Optional) Sum of tax for all items in this order.
        ' Note:
        ' You must set the currencyID attribute to one of the 3-character currency codes
        ' for any of the supported PayPal currencies.
        ' Character length and limitations: Value is a positive number which 
        ' cannot exceed $10,000 USD in any currency. It includes no currency symbol.
        ' It must have 2 decimal places, the decimal separator must be a period (.),
        ' and the optional thousands separator must be a comma (,).
        If totalTax <> String.Empty Then
            paydtl.TaxTotal = New BasicAmountType(DirectCast([Enum].Parse(GetType(CurrencyCodeType), currencyCode), CurrencyCodeType), totalTax)
            orderTotal += Convert.ToDecimal(totalTax)
        End If

        ' (Optional) Description of items the buyer is purchasing.
        ' Note:
        ' The value you specify is available only if the transaction includes a purchase.
        ' This field is ignored if you set up a billing agreement for a recurring payment 
        ' that is not immediately charged.
        ' Character length and limitations: 127 single-byte alphanumeric characters
        If orderDescription <> String.Empty Then
            paydtl.OrderDescription = orderDescription
        End If

        Dim itemsTotal As New BasicAmountType()
        itemsTotal.value = Convert.ToString(itemTotal)

        ' PayPal uses 3-character ISO-4217 codes for specifying currencies in fields and variables. 
        itemsTotal.currencyID = DirectCast([Enum].Parse(GetType(CurrencyCodeType), currencyCode), CurrencyCodeType)

        paydtl.OrderTotal = New BasicAmountType(DirectCast([Enum].Parse(GetType(CurrencyCodeType), currencyCode), CurrencyCodeType), Convert.ToString(orderTotal))
        paydtl.PaymentDetailsItem = lineItems

        paydtl.ItemTotal = itemsTotal

        ' (Optional) Your URL for receiving Instant Payment Notification (IPN) 
        ' about this transaction. If you do not specify this value in the request, 
        ' the notification URL from your Merchant Profile is used, if one exists.
        ' Important:
        ' The notify URL applies only to DoExpressCheckoutPayment. 
        ' This value is ignored when set in SetExpressCheckout or GetExpressCheckoutDetails.
        ' Character length and limitations: 2,048 single-byte alphanumeric characters
        paydtl.NotifyURL = notifyURL

        payDetails.Add(paydtl)
        details.PaymentDetails = payDetails
        setExpressCheckoutReq.SetExpressCheckoutRequestDetails = details

        Dim expressCheckoutReq As New SetExpressCheckoutReq()
        expressCheckoutReq.SetExpressCheckoutRequest = setExpressCheckoutReq

        Dim response As SetExpressCheckoutResponseType = Nothing
        Try
            response = service.SetExpressCheckout(expressCheckoutReq)
        Catch ex As System.Exception
            contextHttp.Response.Write(ex.Message)
            Return
        End Try

        Dim responseValues As New Dictionary(Of String, String)()
        Dim redirectUrl As String = Nothing

        If Not response.Ack.ToString().Trim().ToUpper().Equals(AckCodeType.FAILURE.ToString()) AndAlso Not response.Ack.ToString().Trim().ToUpper().Equals(AckCodeType.FAILUREWITHWARNING.ToString()) Then
            redirectUrl = ConfigurationManager.AppSettings("PAYPAL_REDIRECT_URL").ToString() + "_express-checkout&token=" + response.Token
        End If
        responseValues.Add("Acknowledgement", response.Ack.ToString().Trim().ToUpper())

        'Display(contextHttp, "SetExpressCheckoutPaymentOrder", "SetExpressCheckout", responseValues, service.getLastRequest(), service.getLastResponse(), response.Errors, redirectUrl)
    End Sub




End Class


