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

Partial Class Accounts_PaymentHistory
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        lblTitle.Text = "Settings"
        If Not IsPostBack Then
            LoadUserAccountData()
            LoadPaymentsData()
        End If
    End Sub

    Private Sub LoadPaymentsData()
        Dim dsPayments As DataSet = New DataSet()
        dsPayments = m_ManagementService.GetUserTransactionsByUserId(m_LoginUser.UserId)
        gvPaymentHistory.DataSource = dsPayments.Tables(0)
        gvPaymentHistory.DataBind()

        If dsPayments.Tables.Count > 0 Then
            If dsPayments.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(dsPayments.Tables(0).Rows(0)("CreditAmount")) Then
                    lblPlanPrice.Text = String.Format("${0:f2}", dsPayments.Tables(0).Rows(0)("CreditAmount"))
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

    Protected Sub gvPaymentHistory_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPaymentHistory.DataBound
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvPaymentHistory.BottomPagerRow

        Dim pageList As DropDownList = Nothing
        Dim pageLabel As Label = Nothing
        Dim pagePre As ImageButton = Nothing
        Dim pageNext As ImageButton = Nothing

        If Not pagerRow Is Nothing Then
            ' Retrieve the DropDownList and Label controls from the row.
            pageList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)
            pageLabel = CType(pagerRow.Cells(0).FindControl("lblPageMessage"), Label)
            pagePre = CType(pagerRow.Cells(0).FindControl("lbnPre"), ImageButton)
            pageNext = CType(pagerRow.Cells(0).FindControl("lbnNext"), ImageButton)
        End If

        If Not pageList Is Nothing Then

            ' Create the values for the DropDownList control based on 
            ' the  total number of pages required to display the data
            ' source.
            Dim i As Integer

            For i = 0 To gvPaymentHistory.PageCount - 1

                ' Create a ListItem object to represent a page.
                Dim pageNumber As Integer = i + 1
                Dim item As ListItem = New ListItem(pageNumber.ToString())

                ' If the ListItem object matches the currently selected
                ' page, flag the ListItem object as being selected. Because
                ' the DropDownList control is recreated each time the pager
                ' row gets created, this will persist the selected item in
                ' the DropDownList control.   
                If i = gvPaymentHistory.PageIndex Then

                    item.Selected = True

                End If

                ' Add the ListItem object to the Items collection of the 
                ' DropDownList.
                pageList.Items.Add(item)

            Next i

        End If

        If Not pageLabel Is Nothing Then

            ' Calculate the current page number.
            Dim currentPage As Integer = gvPaymentHistory.PageIndex + 1

            ' Update the Label control with the current page information.
            pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                " of " & gvPaymentHistory.PageCount.ToString()

        End If

        If gvPaymentHistory.Rows.Count > 0 Then
            If gvPaymentHistory.PageIndex = 0 Then
                If Not pagePre Is Nothing Then
                    pagePre.Visible = False
                End If
            ElseIf gvPaymentHistory.PageCount - 1 = gvPaymentHistory.PageIndex Then
                If Not pageNext Is Nothing Then
                    pageNext.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvPaymentHistory_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPaymentHistory.PageIndexChanging
        gvPaymentHistory.PageIndex = e.NewPageIndex
        gvPaymentHistory.DataBind()
    End Sub

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvPaymentHistory.BottomPagerRow

        ' Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)

        ' Set the PageIndex property to display that page selected by the user.
        gvPaymentHistory.PageIndex = pageList.SelectedIndex

        LoadPaymentsData()
        gvPaymentHistory.DataSource = SortDataTable(gvPaymentHistory.DataSource, True)
        gvPaymentHistory.DataBind()
    End Sub
    Private Property GridViewSortDirection() As String
        Get
            Return IIf(ViewState("SortDirection") = Nothing, "ASC", ViewState("SortDirection"))
        End Get
        Set(ByVal value As String)
            ViewState("SortDirection") = value
        End Set
    End Property

    Private Property GridViewSortExpression() As String
        Get
            Return IIf(ViewState("SortExpression") = Nothing, String.Empty, ViewState("SortExpression"))
        End Get
        Set(ByVal value As String)
            ViewState("SortExpression") = value
        End Set
    End Property

    Private Function GetSortDirection() As String
        Select Case GridViewSortDirection
            Case "ASC"
                GridViewSortDirection = "DESC"
            Case "DESC"
                GridViewSortDirection = "ASC"
        End Select
        Return GridViewSortDirection
    End Function

    Protected Function SortDataTable(ByVal dataTable As DataTable, ByVal isPageIndexChanging As Boolean) As DataView
        If Not dataTable Is Nothing Then
            Dim dataView As New DataView(dataTable)
            If GridViewSortExpression <> String.Empty Then
                If isPageIndexChanging Then
                    dataView.Sort = String.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection)
                Else
                    dataView.Sort = String.Format("{0} {1}", GridViewSortExpression, GetSortDirection())
                End If
            End If
            Return dataView
        Else
            Return New DataView()
        End If
    End Function

    Protected Sub gvPaymentHistory_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvPaymentHistory.Sorting
        GridViewSortExpression = e.SortExpression
        Dim pageIndex As Integer = gvPaymentHistory.PageIndex
        LoadPaymentsData()
        gvPaymentHistory.DataSource = SortDataTable(gvPaymentHistory.DataSource, False)
        gvPaymentHistory.DataBind()
        gvPaymentHistory.PageIndex = pageIndex
    End Sub

    Protected Sub lbnPre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim PrePage As Integer = gvPaymentHistory.PageIndex - 1
        gvPaymentHistory.PageIndex = PrePage
        LoadPaymentsData()
    End Sub

    Protected Sub lbnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim NextPage As Integer = gvPaymentHistory.PageIndex + 1
        gvPaymentHistory.PageIndex = NextPage
        LoadPaymentsData()
    End Sub

    Public Function ShowCreditAmount(ByVal CreditAmount As String, ByVal NumberOfProjectCredits As String) As String
        Dim result As String = String.Empty
        Dim decCreditAmount As Decimal = 0
        If Not Decimal.TryParse(CreditAmount, decCreditAmount) Then
            decCreditAmount = 0
        End If
        Dim intNumberOfProjectCredits As Integer = 0
        If Not Integer.TryParse(NumberOfProjectCredits, intNumberOfProjectCredits) Then
            intNumberOfProjectCredits = 0
        End If
        If intNumberOfProjectCredits > 0 Then
            result = String.Format("{0:C}", decCreditAmount)
        End If
        Return result
    End Function

    Protected Sub btnEditPlan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditPlan.Click
        Response.Redirect("topup.aspx")
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
                Response.Redirect("PaymentHistory.aspx")
            End If
        End If
    End Sub

    Private Sub LoadUserAccountData()
        Dim dsUserAccount As DataSet = New DataSet()
        dsUserAccount = m_ManagementService.GetUserAccountByUserID(m_LoginUser.UserId)
        If dsUserAccount.Tables.Count > 0 Then
            If dsUserAccount.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("NumberOfProjects")) Then
                    lblSubscription.Text = String.Format("{0} project(s)", dsUserAccount.Tables(0).Rows(0)("NumberOfProjects"))
                End If
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("NextBillingDate")) Then
                    lblNextPaymentDue.Text = String.Format("{0:dd MMM yyyy}", dsUserAccount.Tables(0).Rows(0)("NextBillingDate"))
                End If
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("PartialCreditCardNumber")) Then
                    lblPaymentInfo.Text = String.Format("{0} Valid Thru {1:MM/yy}", dsUserAccount.Tables(0).Rows(0)("PartialCreditCardNumber"), dsUserAccount.Tables(0).Rows(0)("ValidThru"))
                End If
                If Not IsDBNull(dsUserAccount.Tables(0).Rows(0)("NextBillingDate")) Then
                    btnCancelSubscription.Enabled = True
                Else
                    btnCancelSubscription.Enabled = False
                End If

            End If
        End If
    End Sub

    Protected Sub btnPaymentInfoXML_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPaymentInfoXML.Click
        Response.Redirect(String.Format("https://payments.ebizsecure.co.nz/ppe/pmtservices_gw1/gateway/interface_recurring_paymentinfo.asp?rid={0}***{1:yyyyMMddHHmmss}&rrid={0}***{1:yyyyMMddHHmmss}", m_LoginUser.UserId, Now))
    End Sub
End Class
