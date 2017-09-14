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
Partial Class Accounts_topup
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Public m_ServicePlanCount As Integer = 0
    Private SelectedPlanID As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        'david added here to initial selected id
        SelectedPlanID = ""

        If Not IsPostBack Then
            If Request.QueryString.Count = 0 And Session("SelecrPlanReferrer") = Nothing Then
                Session("SelecrPlanReferrer") = Request.UrlReferrer.ToString()
            End If

            Dim ProjectOwnerId As Long
            ProjectOwnerId = m_ManagementService.GetProjectOwnerByContactId(m_LoginUser.UserId).ProjectOwnerId
            'Dim ExistedProjectsCount As Integer = 0
            'Dim dsProjects As DataSet
            'dsProjects = m_ManagementService.GetProjectsByProjectOwnerId(ProjectOwnerId)
            'If dsProjects.Tables.Count > 0 Then
            '    ExistedProjectsCount = dsProjects.Tables(0).Rows.Count
            'End If
            Dim UnarchivedProjectsCount As Integer = m_ManagementService.GetUnarchivedProjectsCountByUserId(m_LoginUser.UserId)
            Dim CurrentPlanId As Integer = 0
            Dim ValidCreditCard = False
            Dim PlanChanged = False
            Dim dsUserAccount As DataSet
            dsUserAccount = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
            If dsUserAccount.Tables.Count > 0 Then
                If dsUserAccount.Tables(0).Rows.Count > 0 Then
                    ' Comment out --- Show all plans            
                    'If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("PlanId")) Then
                    '    CurrentPlanId = dsUserAccount.Tables(0).Rows(0)("PlanId")
                    'End If
                    If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("ValidThru")) Then
                        Dim strValidThru As String = dsUserAccount.Tables(0).Rows(0)("ValidThru")
                        Dim ExpiryDate_Year As Integer = CInt("20" & strValidThru.Substring(2, 2))
                        Dim ExpiryDate_Month As Integer = CInt(strValidThru.Substring(0, 2))
                        Dim ExpiryDate As DateTime = New Date(ExpiryDate_Year, ExpiryDate_Month, 1)
                        ExpiryDate = DateAdd(DateInterval.Month, 1, ExpiryDate)
                        If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("PartialCreditCardNumber")) And Today < ExpiryDate Then
                            ValidCreditCard = True
                        End If

                        Dim OldPlanId As Integer = 0
                        If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("OldPlanId")) Then
                            OldPlanId = dsUserAccount.Tables(0).Rows(0)("OldPlanId")
                        End If
                        Dim PlanId As Integer = dsUserAccount.Tables(0).Rows(0)("PlanId")
                        If OldPlanId > 0 And PlanId <> OldPlanId Then
                            PlanChanged = True
                        End If
                    End If
                End If
            End If
            If ValidCreditCard Then
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("NextBillingDate")) Then
                    btnPay.Visible = False
                    btnUpdate.Visible = True
                    If PlanChanged Then
                        btnUpdate.Enabled = False
                    Else
                        btnUpdate.Enabled = True
                    End If
                Else
                    btnPay.Visible = True
                    btnUpdate.Visible = False
                End If
            Else
                btnPay.Visible = True
                btnUpdate.Visible = False
            End If
            Dim dsServicePlan As DataSet = New DataSet()
            'dsServicePlan = m_ManagementService.GetMonthlyPlans(ExistedProjectsCount, CurrentPlanId)
            dsServicePlan = m_ManagementService.GetMonthlyPlans(UnarchivedProjectsCount, CurrentPlanId)
            If dsServicePlan.Tables.Count > 0 Then
                If dsServicePlan.Tables(0).Rows.Count > 0 Then
                    m_ServicePlanCount = dsServicePlan.Tables(0).Rows.Count
                End If
            End If
            rpServicePlan.DataSource = dsServicePlan.Tables(0)
            rpServicePlan.DataBind()

            For index = 0 To rpServicePlan.Items.Count - 1
                Dim chkbox As CheckBox = rpServicePlan.Items(index).FindControl("cbServicePlan")
                chkbox.InputAttributes.Add("onchange", "checkAllChecked(" & index & "," & rpServicePlan.Items.Count & ")")
            Next

            Dim userProfile As UserProfile
            userProfile = m_ManagementService.GetUserProfileByUserID(m_LoginUser.UserId)
            If userProfile.Country = Nothing Or userProfile.Country.Trim().ToUpper() = "NEW ZEALAND" Then
                lblGSTMsg.Visible = True
            Else
                lblGSTMsg.Visible = False
            End If
        End If

        If Request.QueryString("msg") <> "" Then
            lblMsg.Text = Request.QueryString("msg")
        End If
    End Sub

    Public Function ShowWidth() As String
        Dim TableWidth As String = "200"
        Return TableWidth
    End Function

    Protected Sub btnPay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPay.Click
        For index = 0 To rpServicePlan.Items.Count - 1
            Dim CurrentCheckBox As CheckBox = CType(rpServicePlan.Items(index).FindControl("cbServicePlan"), WebControls.CheckBox)
            Dim CurrentServicePlanID As Integer = 0
            Dim CurrentTextBox As TextBox = CType(rpServicePlan.Items(index).FindControl("tbServicePlanID"), WebControls.TextBox)
            Integer.TryParse(m_Cryption.Decrypt(CurrentTextBox.Text, m_Cryption.cryptionKey), CurrentServicePlanID)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("tbPlanPrice"), WebControls.TextBox)
            Dim planPrice As Decimal = 0
            Decimal.TryParse(CurrentTextBox.Text, planPrice)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtPlanId"), WebControls.TextBox)
            Dim PlanId As Integer = 0
            Decimal.TryParse(CurrentTextBox.Text, PlanId)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtNumberOfProjects"), WebControls.TextBox)
            Dim numberOfProjects As Integer = 0
            Decimal.TryParse(CurrentTextBox.Text, numberOfProjects)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtStorageSize"), WebControls.TextBox)
            Dim storageSize As Decimal = 0
            Decimal.TryParse(CurrentTextBox.Text, storageSize)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtTerm"), WebControls.TextBox)
            Dim term As Integer = 0
            Decimal.TryParse(CurrentTextBox.Text, term)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtType"), WebControls.TextBox)
            Dim type As Integer = 0
            Decimal.TryParse(CurrentTextBox.Text, type)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtRecurringBillingId"), WebControls.TextBox)
            Dim RecurringBillingId As String = String.Empty
            RecurringBillingId = CurrentTextBox.Text
            If CurrentCheckBox.Checked And CurrentServicePlanID > 0 Then
                Session("CCAmount") = planPrice
                Session("PlanId") = PlanId
                Session("NumberOfProjects") = numberOfProjects
                Session("StorageSize") = storageSize
                Session("Term") = term
                Session("Type") = type
                Session("RecurringBillingId") = RecurringBillingId

                'Dim ReferenceId As String = String.Empty
                'Dim RecurringReferenceId As String = String.Empty
                'Dim dsUserAccount As DataSet
                'dsUserAccount = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
                'If dsUserAccount.Tables.Count > 0 Then
                '    If dsUserAccount.Tables(0).Rows.Count > 0 Then
                '        If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("ReferenceId")) And Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("RecurringReferenceId")) Then
                '            ReferenceId = dsUserAccount.Tables(0).Rows(0)("ReferenceId")
                '            RecurringReferenceId = dsUserAccount.Tables(0).Rows(0)("RecurringReferenceId")
                '        End If
                '    End If
                'End If
                Dim strReturnUrl As String
                strReturnUrl = String.Format("{0}{1}?Pay={2}", ConfigurationManager.AppSettings("ProjectURL"), "Accounts/Thankyou.aspx", m_Cryption.Encrypt("1", m_Cryption.cryptionKey))
                'Response.Redirect(String.Format("https://payments.ebizsecure.co.nz/ppe/pmtservices_gw1/gateway/interface.asp?mid={0}&rid={1}***{2:yyyyMMddHHmmss}&sp={3}&PPage={4}", ConfigurationManager.AppSettings("EBIZID"), m_LoginUser.UserId, DateTime.Now, planPrice, Server.UrlEncode(strReturnUrl)), False)
                Response.Redirect(String.Format("https://payments.ebizsecure.co.nz/ppe/pmtservices_gw1/gateway/interface.asp?rid={0}***{1:yyyyMMddHHmmss}&sp={2}&PPage={3}", m_LoginUser.UserId, DateTime.Now, planPrice, Server.UrlEncode(strReturnUrl)), False)
                'If ReferenceId <> String.Empty And RecurringReferenceId <> String.Empty Then
                '    Response.Redirect(String.Format("https://payments.ebizsecure.co.nz/ppe/pmtservices_gw1/gateway/interface_recurring.asp?rid={0}&rrid={1}&rbid={2}&TokenSetup=True&sp={3}&PPage={4}", ReferenceId, RecurringReferenceId, RecurringBillingId, planPrice, Server.UrlEncode(strReturnUrl)), False)
                'Else
                '    Response.Redirect(String.Format("https://payments.ebizsecure.co.nz/ppe/pmtservices_gw1/gateway/interface_recurring.asp?rid={0}***{1:yyyyMMddHHmmss}&rrid={0}***{1:yyyyMMddHHmmss}&rbid={2}&TokenRebill=True&sp={3}&PPage={4}", m_LoginUser.UserId, DateTime.Now, RecurringBillingId, planPrice, Server.UrlEncode(strReturnUrl)), False)
                'End If
            End If
        Next
        lblMsg.Text = "Please select your plan!"
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Public Function IsPlanEnable(ByVal PlanId As String) As String
        Dim result As String = "true"
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
        Return result
    End Function

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        For index = 0 To rpServicePlan.Items.Count - 1
            Dim CurrentCheckBox As CheckBox = CType(rpServicePlan.Items(index).FindControl("cbServicePlan"), WebControls.CheckBox)
            Dim CurrentServicePlanID As Integer = 0
            Dim CurrentTextBox As TextBox = CType(rpServicePlan.Items(index).FindControl("tbServicePlanID"), WebControls.TextBox)
            Integer.TryParse(m_Cryption.Decrypt(CurrentTextBox.Text, m_Cryption.cryptionKey), CurrentServicePlanID)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("tbPlanPrice"), WebControls.TextBox)
            Dim planPrice As Decimal = 0
            Decimal.TryParse(CurrentTextBox.Text, planPrice)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtPlanId"), WebControls.TextBox)
            Dim PlanId As Integer = 0
            Decimal.TryParse(CurrentTextBox.Text, PlanId)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtNumberOfProjects"), WebControls.TextBox)
            Dim numberOfProjects As Integer = 0
            Decimal.TryParse(CurrentTextBox.Text, numberOfProjects)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtStorageSize"), WebControls.TextBox)
            Dim storageSize As Decimal = 0
            Decimal.TryParse(CurrentTextBox.Text, storageSize)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtTerm"), WebControls.TextBox)
            Dim term As Integer = 0
            Decimal.TryParse(CurrentTextBox.Text, term)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtType"), WebControls.TextBox)
            Dim type As Integer = 0
            Decimal.TryParse(CurrentTextBox.Text, type)
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtRecurringBillingId"), WebControls.TextBox)
            Dim RecurringBillingId As String = String.Empty
            RecurringBillingId = CurrentTextBox.Text
            If CurrentCheckBox.Checked And CurrentServicePlanID > 0 Then
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
                    requestUrl = String.Format("{0}&rid={1}&rrid={2}&Command=change&rbid={3}", requestUrl, ReferenceId, RecurringReferenceId, RecurringBillingId)

                    Dim doc As XmlDocument = New XmlDocument()
                    doc.Load(requestUrl)

                    Dim xpath_ChangeBillingPaln As String = "RecurringBilling/ChangeBillingPlan"
                    Dim ns As XmlNamespaceManager = New XmlNamespaceManager(doc.NameTable)
                    Dim nodes As XmlNodeList = doc.SelectNodes(xpath_ChangeBillingPaln, ns)
                    Dim node As XmlNode = nodes(0)
                    Dim strStatus As String = node("Status").InnerText
                    Dim strStatusDesc As String = node("StatusDesc").InnerText
                    Dim strNextBillingDate As String = node("NextBillingDate").InnerText
                    If strStatus = "1" And strStatusDesc = "Success" Then
                        m_ManagementService.UpdateUserAccountMonthly(m_LoginUser.UserId, PlanId, numberOfProjects, storageSize, CDate(strNextBillingDate))
                        m_ManagementService.CreateUserTransaction(m_LoginUser.UserId, String.Format("Monthly Subscription - Update"), 0, 0, numberOfProjects, numberOfProjects)
                        Response.Redirect("topup.aspx")
                    End If
                End If
            End If
        Next
    End Sub

    Protected Sub rpServicePlan_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rpServicePlan.ItemCommand
        'david add here to get selected id
        If CType(e.Item.FindControl("'cbServicePlan"), CheckBox).Checked Then
            SelectedPlanID = CType(e.Item.FindControl("'txtPlanId"), TextBox).Text
        Else
            SelectedPlanID = ""
        End If
    End Sub
End Class
