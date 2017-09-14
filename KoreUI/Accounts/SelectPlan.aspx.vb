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

Partial Class Accounts_SelectPlan
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Public m_ServicePlanCount As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Not IsPostBack Then
            If Request.QueryString.Count = 0 And Session("SelecrPlanReferrer") = Nothing Then
                Session("SelecrPlanReferrer") = Request.UrlReferrer.ToString()
            End If
            Dim dsServicePlan As DataSet = New DataSet()
            dsServicePlan = m_ManagementService.GetPlans(False)
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
            CurrentTextBox = CType(rpServicePlan.Items(index).FindControl("txtNumberOfProjectCredits"), WebControls.TextBox)
            Dim numberOfProjectCredits As Integer = 0
            Decimal.TryParse(CurrentTextBox.Text, numberOfProjectCredits)
            If CurrentCheckBox.Checked And CurrentServicePlanID > 0 Then
                Session("CCAmount") = planPrice
                Session("NumberOfProjectCredits") = numberOfProjectCredits
                Dim strReturnUrl As String
                strReturnUrl = String.Format("{0}{1}?Pay={2}", ConfigurationManager.AppSettings("ProjectURL"), "Accounts/Thankyou.aspx", m_Cryption.Encrypt("1", m_Cryption.cryptionKey))
                'Response.Redirect(String.Format("https://payments.ebizsecure.co.nz/ppe/pmtservices_gw1/gateway/interface.asp?mid={0}&rid={1}***{2:yyyyMMddHHmmss}&sp={3}&PPage={4}", ConfigurationManager.AppSettings("EBIZID"), m_LoginUser.UserId, DateTime.Now, planPrice, Server.UrlEncode(strReturnUrl)), False)
                Response.Redirect(String.Format("https://payments.ebizsecure.co.nz/ppe/pmtservices_gw1/gateway/interface.asp?rid={0}***{1:yyyyMMddHHmmss}&sp={2}&PPage={3}", m_LoginUser.UserId, DateTime.Now, planPrice, Server.UrlEncode(strReturnUrl)), False)
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
End Class
