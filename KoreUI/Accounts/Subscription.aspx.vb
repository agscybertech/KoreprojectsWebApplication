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

Partial Class Accounts_Subscription
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        lblTitle.Text = "Settings"
        If Not IsPostBack Then
            LoadUserAccountData()
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnEditPlan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditPlan.Click
        Response.Redirect("plans.aspx")
    End Sub

    Protected Sub btnCancelSubscription_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelSubscription.Click
        Dim ReferenceId As String = String.Empty
        Dim RecurringReferenceId As String = String.Empty
        Dim dsUserAccount As DataSet
        dsUserAccount = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
        If dsUserAccount.Tables.Count > 0 Then
            If dsUserAccount.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("ReferenceId")) And Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("RecurringReferenceId")) Then
                    ReferenceId = dsUserAccount.Tables(0).Rows(0)("ReferenceId")
                    RecurringReferenceId = dsUserAccount.Tables(0).Rows(0)("RecurringReferenceId")
                End If
            End If
        End If
        If ReferenceId <> String.Empty And RecurringReferenceId <> String.Empty Then
            Dim requestUrl As String = String.Format("https://payments.ebizsecure.co.nz/ppe/pmtservices_gw1/gateway/interface_recurring_services.asp?mid={0}", ConfigurationManager.AppSettings("EBIZID"))
            requestUrl = String.Format("{0}&rid={1}&rrid={2}&Command=delete", requestUrl, ReferenceId, RecurringReferenceId)

            Dim doc As XmlDocument = New XmlDocument()
            doc.Load(requestUrl)

            Dim xpath_DeleteBillingPaln As String = "RecurringBilling/DeleteBillingPlan"
            Dim ns As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
            Dim nodes As XmlNodeList = doc.SelectNodes(xpath_DeleteBillingPaln, ns)
            Dim node As XmlNode = nodes(0)
            Dim strStatus As String = node("Status").InnerText
            Dim strStatusDesc As String = node("StatusDesc").InnerText
            If strStatus = "1" And strStatusDesc = "Success" Then
                m_ManagementService.UpdateUserAccountMonthlyCancelSubscription(m_LoginUser.UserId)
                m_ManagementService.CreateUserTransaction(m_LoginUser.UserId, String.Format("Monthly Subscription - Cancel"), 0, 0, 0, 0)
                Response.Redirect("Subscription.aspx")
            End If
        End If
    End Sub

    Private Sub LoadUserAccountData()
        Dim dsUserAccount As DataSet = New DataSet()
        dsUserAccount = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
        If dsUserAccount.Tables.Count > 0 Then
            If dsUserAccount.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("PartialCreditCardNumber")) Then
                    lblPaymentInfo.Text = String.Format("{0}: {1}, Valid Thru {2}", CreditCardType(dsUserAccount.Tables(0).Rows(0)("PartialCreditCardNumber")), dsUserAccount.Tables(0).Rows(0)("PartialCreditCardNumber"), String.Format("{0}", dsUserAccount.Tables(0).Rows(0)("ValidThru")).Insert(2, "/"))
                Else
                    Response.Redirect("plans.aspx")
                End If
                tdOldPlan.Visible = False
                btnEditPlan.Visible = True
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("OldPlanId")) Then
                    If dsUserAccount.Tables(0).Rows(0)("OldPlanId") <> dsUserAccount.Tables(0).Rows(0)("PlanId") Then
                        tdOldPlan.Visible = True
                        btnEditPlan.Visible = False
                        Dim dsOldPlan As DataSet
                        dsOldPlan = m_ManagementService.GetPlanByPlanId(dsUserAccount.Tables(0).Rows(0)("OldPlanId"))
                        lblOldPlanDescription.Text = String.Format("Plan: {0} - {1}", dsOldPlan.Tables(0).Rows(0)("Name"), dsOldPlan.Tables(0).Rows(0)("Description"))
                    End If
                End If
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("PlanId")) Then
                    If dsUserAccount.Tables(0).Rows(0)("PlanId") > 0 Then
                        Dim dsCurrentPlan As DataSet
                        dsCurrentPlan = m_ManagementService.GetPlanByPlanId(dsUserAccount.Tables(0).Rows(0)("PlanId"))
                        lblCurrentPlanDescription.Text = String.Format("Plan: {0} - {1}", dsCurrentPlan.Tables(0).Rows(0)("Name"), dsCurrentPlan.Tables(0).Rows(0)("Description"))
                    End If
                End If
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("NextBillingDate")) Then
                    lblNextPaymentDue.Text = String.Format("Until: {0:dd MMM yyyy}", dsUserAccount.Tables(0).Rows(0)("NextBillingDate"))
                End If
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("NextBillingDate")) And Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("ReferenceId")) And Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("RecurringReferenceId")) Then
                    btnCancelSubscription.Visible = True
                Else
                    btnCancelSubscription.Visible = False
                End If
            End If
        End If
    End Sub

    Private Function CreditCardType(ByVal PartialCreditCardNumber As String) As String
        Dim result As String = "Credit Card"

        If PartialCreditCardNumber.Substring(0, 1) = "4" Then
            result = "Visa"
        Else
            Select Case PartialCreditCardNumber.Substring(0, 2)
                Case "34", "37"
                    result = "American Express"
                Case "51", "52", "53", "54", "55"
                    result = "MasterCard"
            End Select
        End If

        Return result
    End Function
End Class