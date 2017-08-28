Imports System.Data
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class WebControls_WebUserControlServiceGroupGrid
    Inherits System.Web.UI.UserControl
    Private m_CompanyId As Long
    Private m_UserId As Long
    Private m_Keyword As String
    Private m_ManagementService As New ManagementService()
    Private m_ScopeService As New ScopeServices()
    Public m_Cryption As New Cryption()

    Public Property CompanyId() As Long
        Get
            Return m_CompanyId
        End Get
        Set(ByVal value As Long)
            m_CompanyId = value
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
        LoadServiceGroup()
    End Sub

    Private Sub LoadServiceGroup()
        Dim dsServiceGroup As DataSet = New DataSet()
        dsServiceGroup = m_ScopeService.GetServiceGroupsByUserId(m_UserId)
        gvServiceGroup.DataSource = dsServiceGroup.Tables(0)
        gvServiceGroup.DataBind()
    End Sub

    Protected Sub gvServiceGroup_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvServiceGroup.DataBound
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvServiceGroup.BottomPagerRow

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

            For i = 0 To gvServiceGroup.PageCount - 1

                ' Create a ListItem object to represent a page.
                Dim pageNumber As Integer = i + 1
                Dim item As ListItem = New ListItem(pageNumber.ToString())

                ' If the ListItem object matches the currently selected
                ' page, flag the ListItem object as being selected. Because
                ' the DropDownList control is recreated each time the pager
                ' row gets created, this will persist the selected item in
                ' the DropDownList control.   
                If i = gvServiceGroup.PageIndex Then

                    item.Selected = True

                End If

                ' Add the ListItem object to the Items collection of the 
                ' DropDownList.
                pageList.Items.Add(item)

            Next i

        End If

        If Not pageLabel Is Nothing Then

            ' Calculate the current page number.
            Dim currentPage As Integer = gvServiceGroup.PageIndex + 1

            ' Update the Label control with the current page information.
            pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                " of " & gvServiceGroup.PageCount.ToString()

        End If

        If gvServiceGroup.Rows.Count > 0 Then
            If gvServiceGroup.PageIndex = 0 Then
                If Not pagePre Is Nothing Then
                    pagePre.Visible = False
                End If
            ElseIf gvServiceGroup.PageCount - 1 = gvServiceGroup.PageIndex Then
                If Not pageNext Is Nothing Then
                    pageNext.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvServiceGroup_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvServiceGroup.PageIndexChanging
        gvServiceGroup.PageIndex = e.NewPageIndex
        gvServiceGroup.DataBind()
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

    Protected Sub gvServiceGroup_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvServiceGroup.Sorting
        GridViewSortExpression = e.SortExpression
        Dim pageIndex As Integer = gvServiceGroup.PageIndex
        LoadServiceGroup()
        gvServiceGroup.DataSource = SortDataTable(gvServiceGroup.DataSource, False)
        gvServiceGroup.DataBind()
        gvServiceGroup.PageIndex = pageIndex
    End Sub

    Protected Sub lbnPre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim PrePage As Integer = gvServiceGroup.PageIndex - 1
        gvServiceGroup.PageIndex = PrePage
        LoadServiceGroup()
    End Sub

    Protected Sub lbnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim NextPage As Integer = gvServiceGroup.PageIndex + 1
        gvServiceGroup.PageIndex = NextPage
        LoadServiceGroup()
    End Sub

    Public Function showStatus(ByVal Status As String) As String
        Dim result As String = "Public"
        If Status = "1" Then
            result = "Assigned Only"
        ElseIf Status = "2" Then
            result = "Private"
        End If
        Return result
    End Function

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvServiceGroup.BottomPagerRow

        ' Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)

        ' Set the PageIndex property to display that page selected by the user.
        gvServiceGroup.PageIndex = pageList.SelectedIndex

        LoadServiceGroup()
        gvServiceGroup.DataSource = SortDataTable(gvServiceGroup.DataSource, True)
        gvServiceGroup.DataBind()
    End Sub
End Class
