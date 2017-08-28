Imports System.Data
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class WebControls_WebUserControlRateGrid
    Inherits System.Web.UI.UserControl
    Private m_CompanyId As Long
    Private m_BranchId As Long
    Private m_UserId As Long
    Private m_ServiceGroupId As Long
    Private m_UserType As Integer
    Private m_DateFrom As DateTime
    Private m_DateTo As DateTime
    Private m_Keyword As String
    Private m_ManagementService As New ManagementService()
    Private m_ScopeService As New ScopeServices()
    Public m_Cryption As New Cryption()
    'Private UserControls As List(Of WebControls_WebUserControlRateGrid)

    Public Property CompanyId() As Long
        Get
            Return m_CompanyId
        End Get
        Set(ByVal value As Long)
            m_CompanyId = value
        End Set
    End Property

    Public Property BranchId() As Long
        Get
            Return m_BranchId
        End Get
        Set(ByVal value As Long)
            m_BranchId = value
        End Set
    End Property

    Public Property UserId() As Long
        Get
            Return m_UserId
        End Get
        Set(ByVal value As Long)
            m_UserId = value
        End Set
    End Property

    Public Property ServiceGroupId() As Long
        Get
            Return m_ServiceGroupId
        End Get
        Set(ByVal value As Long)
            m_ServiceGroupId = value
        End Set
    End Property

    Public Property UserType() As Integer
        Get
            Return m_UserType
        End Get
        Set(ByVal value As Integer)
            m_UserType = value
        End Set
    End Property

    Public Property DateFrom() As DateTime
        Get
            Return m_DateFrom
        End Get
        Set(ByVal value As DateTime)
            m_DateFrom = value
        End Set
    End Property

    Public Property DateTo() As DateTime
        Get
            Return m_DateTo
        End Get
        Set(ByVal value As DateTime)
            m_DateTo = value
        End Set
    End Property

    Public Property Keyword() As String
        Get
            Return m_Keyword
        End Get
        Set(ByVal value As String)
            m_Keyword = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        LoadRatesData()
    End Sub

    Private Sub LoadRatesData()
        Dim dsRates As DataSet = New DataSet()
        'If (Session("UserControls") IsNot Nothing) Then
        '    UserControls = CType(Session("UserControls"), List(Of WebControls_WebUserControlRateGrid))
        '    'For Each UserControlID As WebControls_WebUserControlRateGrid In UserControls
        '    'Next
        '    Dim uc As WebControls_WebUserControlRateGrid = UserControls.Item(UserControls.Count - 1)
        '    If uc.ServiceGroupId > 0 Then
        '        dsRates = m_ScopeService.GetAllServiceRatesByServiceGroupId(m_ServiceGroupId)
        '    Else
        '        dsRates = m_ScopeService.GetAllServiceRatesByProjectOwnerIdUserId(m_CompanyId, m_UserId)
        '    End If
        '    UserControls.RemoveAt(UserControls.Count - 1)
        'Else
        '    dsRates = m_ScopeService.GetAllServiceRatesByProjectOwnerIdUserId(m_CompanyId, m_UserId)
        'End If

        If m_ServiceGroupId > 0 Then
            dsRates = m_ScopeService.GetAllServiceRatesByServiceGroupId(m_ServiceGroupId)
        Else
            dsRates = m_ScopeService.GetAllServiceRatesByProjectOwnerIdUserId(m_CompanyId, m_UserId)
        End If

        gvRates.DataSource = dsRates.Tables(0)
        gvRates.DataBind()

        If Request.Url.ToString.ToLower.IndexOf("/print.aspx") > 0 Then
            gvRates.Columns(3).Visible = False
        End If
    End Sub

    Protected Sub gvRates_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvRates.DataBound
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvRates.BottomPagerRow

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

            For i = 0 To gvRates.PageCount - 1

                ' Create a ListItem object to represent a page.
                Dim pageNumber As Integer = i + 1
                Dim item As ListItem = New ListItem(pageNumber.ToString())

                ' If the ListItem object matches the currently selected
                ' page, flag the ListItem object as being selected. Because
                ' the DropDownList control is recreated each time the pager
                ' row gets created, this will persist the selected item in
                ' the DropDownList control.   
                If i = gvRates.PageIndex Then

                    item.Selected = True

                End If

                ' Add the ListItem object to the Items collection of the 
                ' DropDownList.
                pageList.Items.Add(item)

            Next i

        End If

        If Not pageLabel Is Nothing Then

            ' Calculate the current page number.
            Dim currentPage As Integer = gvRates.PageIndex + 1

            ' Update the Label control with the current page information.
            pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                " of " & gvRates.PageCount.ToString()

        End If

        If gvRates.Rows.Count > 0 Then
            If gvRates.PageIndex = 0 Then
                If Not pagePre Is Nothing Then
                    pagePre.Visible = False
                End If
            ElseIf gvRates.PageCount - 1 = gvRates.PageIndex Then
                If Not pageNext Is Nothing Then
                    pageNext.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvRates_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRates.PageIndexChanging
        gvRates.PageIndex = e.NewPageIndex
        gvRates.DataBind()
    End Sub

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvRates.BottomPagerRow

        ' Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)

        ' Set the PageIndex property to display that page selected by the user.
        gvRates.PageIndex = pageList.SelectedIndex

        LoadRatesData()
        gvRates.DataSource = SortDataTable(gvRates.DataSource, True)
        gvRates.DataBind()
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

    Protected Sub gvRates_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvRates.Sorting
        GridViewSortExpression = e.SortExpression
        Dim pageIndex As Integer = gvRates.PageIndex
        LoadRatesData()
        gvRates.DataSource = SortDataTable(gvRates.DataSource, False)
        gvRates.DataBind()
        gvRates.PageIndex = pageIndex
    End Sub

    Protected Sub lbnPre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim PrePage As Integer = gvRates.PageIndex - 1
        gvRates.PageIndex = PrePage
        LoadRatesData()
    End Sub

    Protected Sub lbnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim NextPage As Integer = gvRates.PageIndex + 1
        gvRates.PageIndex = NextPage
        LoadRatesData()
    End Sub

    Public Function showStatus(ByVal Status As String) As String
        Dim result As String = "OFF"
        If Status <> "True" Then
            result = "ON"
        End If
        Return result
    End Function
End Class
