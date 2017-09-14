Imports System.Data
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities
Imports AjaxControlToolkit
Imports System.Threading

Partial Class WebControls_WebUserControlProjectsGrid
    Inherits System.Web.UI.UserControl
    Private m_CompanyId As Long
    Private m_BranchId As Long
    Private m_UserId As Long
    Private m_UserType As Integer
    Private m_Archived As Boolean
    Private m_Scoped As Boolean
    Private m_LoginUserId As Long
    Private m_DateFrom As DateTime
    Private m_DateTo As DateTime
    Private m_Keyword As String
    Private m_SearchType As Integer = 1
    Private m_ManagementService As New ManagementService()
    Private m_DatabaseService As New DatabaseService()
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

    Public Property UserType() As Integer
        Get
            Return m_UserType
        End Get
        Set(ByVal value As Integer)
            m_UserType = value
        End Set
    End Property

    Public Property Archived() As Boolean
        Get
            Return m_Archived
        End Get
        Set(ByVal value As Boolean)
            m_Archived = value
        End Set
    End Property

    Public Property Scoped() As Boolean
        Get
            Return m_Scoped
        End Get
        Set(ByVal value As Boolean)
            m_Scoped = value
        End Set
    End Property

    Public Property LoginUserId() As Long
        Get
            Return m_LoginUserId
        End Get
        Set(ByVal value As Long)
            m_LoginUserId = value
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

    Public Property SearchType() As Integer
        Get
            Return m_SearchType
        End Get
        Set(ByVal value As Integer)
            m_SearchType = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_DatabaseService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Not IsPostBack Then
            LoadGroupData()
            LoadCustomersData()

            Dim m_GroupProjects As DataSet = New DataSet()
            m_GroupProjects = m_ManagementService.GetGroupProjectsNotUserArchivedByUserId(m_UserId)
            'm_GroupProjects = m_ManagementService.GetGroupProjectsByUserId(m_UserId, m_Keyword, m_SearchType)
            If m_GroupProjects.Tables.Count > 0 Then
                If m_GroupProjects.Tables(0).Rows.Count > 0 Then
                    'tblGroupView.Visible = True
                Else
                    'tblGroupView.Visible = False
                    Session("IsGroupView") = False
                    UpdateCookies()
                End If
            End If

            'If Session("IsGroupView") = Nothing Then
            '    Session("IsGroupView") = True
            'End If
            If Session("IsGroupView") Then
                rptProjectsGrid.Visible = True
                gvProjects.Visible = False
            Else
                rptProjectsGrid.Visible = False
                gvProjects.Visible = True
            End If

            If Request.Url.ToString.ToLower.IndexOf("search/view.aspx") > 0 Then
                rptProjectsGrid.Visible = False
                gvProjects.Visible = True
            End If
        End If
    End Sub

    Private Sub LoadGroupData()
        Dim dsGroup As DataSet = New DataSet()
        'dsGroup = m_ScopeService.GetProjectGroupsByProjectOwnerId(m_CompanyId)
        'If m_Archived Then
        'dsGroup = m_ManagementService.GetProjectGroupsUserArchivedByUserId(m_UserId)
        'Else
        If m_Scoped Then
            dsGroup = m_ManagementService.GetProjectGroupsByUserIdUserProjectStatusValue(m_UserId, m_Keyword, -1000)
        Else
            dsGroup = m_ManagementService.GetProjectGroupsNotUserArchivedByUserId(m_UserId, m_Keyword, m_SearchType)
        End If
        'End If
        'Dim newGroupRow As DataRow = dsGroup.Tables(0).NewRow()
        'newGroupRow("ProjectGroupId") = "0"
        'newGroupRow("Name") = "Unassigned"
        'dsGroup.Tables(0).Rows.Add(newGroupRow)
        rptProjectsGrid.DataSource = dsGroup.Tables(0)
        rptProjectsGrid.DataBind()
    End Sub

    Protected Sub rptProjectsGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptProjectsGrid.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim gvProjects As GridView = CType(e.Item.FindControl("gvProjects"), GridView)
            If Not gvProjects Is Nothing Then
                Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)
                'If m_Archived Then
                'gvProjects.DataSource = m_ManagementService.GetProjectsUserArchivedByUserIdGroupId(m_UserId, Convert.ToInt64(row("ProjectGroupId")))
                'Else
                If m_Scoped Then
                    gvProjects.DataSource = m_ManagementService.GetProjectsByUserIdGroupIdUserProjectStatusValue(m_UserId, Convert.ToInt64(row("ProjectGroupId")), -1000, m_Keyword)
                Else
                    gvProjects.DataSource = m_ManagementService.GetProjectsNotUserArchivedByUserIdGroupId(m_UserId, Convert.ToInt64(row("ProjectGroupId")), m_Keyword, m_SearchType)
                End If
                'End If
                gvProjects.DataBind()
            End If

            ' Retrieve the pager row.
            Dim pagerRow As GridViewRow = gvProjects.BottomPagerRow

            Dim pageList As DropDownList = Nothing
            Dim pageLabel As Label = Nothing
            Dim pagePre As LinkButton = Nothing
            Dim pageNext As LinkButton = Nothing

            If Not pagerRow Is Nothing Then
                ' Retrieve the DropDownList and Label controls from the row.
                pageList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)
                pageLabel = CType(pagerRow.Cells(0).FindControl("lblPageMessage"), Label)
                pagePre = CType(pagerRow.Cells(0).FindControl("lbnPre"), LinkButton)
                pageNext = CType(pagerRow.Cells(0).FindControl("lbnNext"), LinkButton)
            End If

            If Not pageList Is Nothing Then

                ' Create the values for the DropDownList control based on 
                ' the  total number of pages required to display the data
                ' source.
                Dim i As Integer

                For i = 0 To gvProjects.PageCount - 1

                    ' Create a ListItem object to represent a page.
                    Dim pageNumber As Integer = i + 1
                    Dim item As ListItem = New ListItem(pageNumber.ToString())

                    ' If the ListItem object matches the currently selected
                    ' page, flag the ListItem object as being selected. Because
                    ' the DropDownList control is recreated each time the pager
                    ' row gets created, this will persist the selected item in
                    ' the DropDownList control.   
                    If i = gvProjects.PageIndex Then

                        item.Selected = True

                    End If

                    ' Add the ListItem object to the Items collection of the 
                    ' DropDownList.
                    pageList.Items.Add(item)

                Next i

            End If

            If Not pageLabel Is Nothing Then

                ' Calculate the current page number.
                Dim currentPage As Integer = gvProjects.PageIndex + 1

                ' Update the Label control with the current page information.
                pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                    " of " & gvProjects.PageCount.ToString()

            End If

            If gvProjects.Rows.Count > 0 Then
                If gvProjects.PageIndex = 0 Then
                    If Not pagePre Is Nothing Then
                        pagePre.Visible = False
                    End If
                ElseIf gvProjects.PageCount - 1 = gvProjects.PageIndex Then
                    If Not pageNext Is Nothing Then
                        pageNext.Visible = False
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub LoadCustomersData()
        Dim dsProjects As DataSet = New DataSet()
        ''If m_Keyword.Length > 0 Then
        ''    dsCustomers = m_ManagementService.SearchProjectByUserId(m_BranchId, m_UserId, m_Keyword)
        ''Else
        ''    Select Case m_UserType
        ''        Case 2
        ''            If m_BranchId > 0 Then
        ''                dsCustomers = m_ManagementService.GetProjectInfoByBranchId(m_BranchId)
        ''            Else
        ''                dsCustomers = m_ManagementService.GetProjectInfoByUserId(m_UserId)
        ''            End If
        ''        Case 1
        ''            dsCustomers = m_ManagementService.GetProjectInfoByStaff(m_UserId)
        ''            'Case Else
        ''            'dsCustomers = m_ManagementService.GetAppointmentByCustomerDateRange(m_UserId, m_DateFrom, m_DateTo)
        ''    End Select
        ''End If
        'dsProjects = m_ManagementService.GetProjectsByProjectOwnerId(1)
        If m_Archived Then
            'If m_UserType = Warpfusion.A4PP.Services.UserType.Admin Then
            '    dsProjects = m_ManagementService.GetProjectsArchivedByProjectOwnerId(m_CompanyId, ConfigurationManager.AppSettings("NumberOfDaysToArchive"))
            'Else
            '    dsProjects = m_ManagementService.GetProjectsArchivedByUserId(m_UserId, ConfigurationManager.AppSettings("NumberOfDaysToArchive"))
            'End If
            'dsProjects = m_ManagementService.GetProjectsArchivedByUserId(m_UserId, ConfigurationManager.AppSettings("NumberOfDaysToArchive"))
            dsProjects = m_ManagementService.GetProjectsUserArchivedByUserId(m_UserId, m_Keyword)
        Else
            If m_Scoped Then
                'If m_UserType = Warpfusion.A4PP.Services.UserType.Admin Then
                '    dsProjects = m_ManagementService.GetProjectsByProjectOwnerIdProjectStatusId(m_CompanyId, ConfigurationManager.AppSettings("NumberOfDaysToArchive"), 0)
                'Else
                '    dsProjects = m_ManagementService.GetProjectsByUserIdProjectStatusId(m_UserId, ConfigurationManager.AppSettings("NumberOfDaysToArchive"), 0)
                'End If
                'dsProjects = m_ManagementService.GetProjectsByUserIdProjectStatusId(m_UserId, ConfigurationManager.AppSettings("NumberOfDaysToArchive"), 0)
                dsProjects = m_ManagementService.GetProjectsByUserIdUserProjectStatusValue(m_UserId, m_Keyword, -1000)
            Else
                'If m_UserType = Warpfusion.A4PP.Services.UserType.Admin Then
                '    dsProjects = m_ManagementService.GetProjectsNotArchivedByProjectOwnerId(m_CompanyId, ConfigurationManager.AppSettings("NumberOfDaysToArchive"))
                'Else
                '    dsProjects = m_ManagementService.GetProjectsNotArchivedByUserId(m_UserId, ConfigurationManager.AppSettings("NumberOfDaysToArchive"))
                'End If
                'dsProjects = m_ManagementService.GetProjectsNotArchivedByUserId(m_UserId, ConfigurationManager.AppSettings("NumberOfDaysToArchive"))
                dsProjects = m_ManagementService.GetProjectsNotUserArchivedByUserId(m_UserId, m_Keyword)
            End If
        End If
        gvProjects.PageSize = 18
        gvProjects.DataSource = dsProjects.Tables(0)
        gvProjects.DataBind()
    End Sub

    Protected Sub gvCustomers_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvProjects.DataBound
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvProjects.BottomPagerRow

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

            For i = 0 To gvProjects.PageCount - 1

                ' Create a ListItem object to represent a page.
                Dim pageNumber As Integer = i + 1
                Dim item As ListItem = New ListItem(pageNumber.ToString())

                ' If the ListItem object matches the currently selected
                ' page, flag the ListItem object as being selected. Because
                ' the DropDownList control is recreated each time the pager
                ' row gets created, this will persist the selected item in
                ' the DropDownList control.   
                If i = gvProjects.PageIndex Then

                    item.Selected = True

                End If

                ' Add the ListItem object to the Items collection of the 
                ' DropDownList.
                pageList.Items.Add(item)

            Next i

        End If

        If Not pageLabel Is Nothing Then

            ' Calculate the current page number.
            Dim currentPage As Integer = gvProjects.PageIndex + 1

            ' Update the Label control with the current page information.
            pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                " of " & gvProjects.PageCount.ToString()

        End If

        If gvProjects.Rows.Count > 0 Then
            If gvProjects.PageIndex = 0 Then
                If Not pagePre Is Nothing Then
                    pagePre.Visible = False
                End If
            ElseIf gvProjects.PageCount - 1 = gvProjects.PageIndex Then
                If Not pageNext Is Nothing Then
                    pageNext.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvCustomers_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvProjects.PageIndexChanging
        gvProjects.PageIndex = e.NewPageIndex
        gvProjects.DataBind()
    End Sub

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvProjects.BottomPagerRow

        ' Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)

        ' Set the PageIndex property to display that page selected by the user.
        gvProjects.PageIndex = pageList.SelectedIndex

        LoadCustomersData()
        gvProjects.DataSource = SortDataTable(gvProjects.DataSource, True)
        gvProjects.DataBind()
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

    Protected Sub gvCustomers_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvProjects.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ddlPropertyStatus As DropDownList = CType(e.Row.FindControl("ddlPropertyStatus"), DropDownList)
            ddlPropertyStatus.DataValueField = "ProjectStatusId"
            ddlPropertyStatus.DataTextField = "Name"
            ddlPropertyStatus.DataSource = m_DatabaseService.GetDataSet("GetProjectStatuses")
            ddlPropertyStatus.DataBind()
            ddlPropertyStatus.SelectedValue = DataBinder.Eval(e.Row.DataItem, "ProjectStatusId").ToString()

            Dim CurrentProjectId As Long
            CurrentProjectId = DataBinder.Eval(e.Row.DataItem, "ProjectId")
            Dim ddlProjectStatus As DropDownList = CType(e.Row.FindControl("ddlProjectStatus"), DropDownList)
            ddlProjectStatus.DataValueField = "StatusValue"
            ddlProjectStatus.DataTextField = "Name"
            ddlProjectStatus.DataSource = m_ManagementService.GetProjectStatusesByProjectIdUserId(CurrentProjectId, m_LoginUserId)
            ddlProjectStatus.DataBind()
            Dim dsUserProjectStatusValue As DataSet
            dsUserProjectStatusValue = m_ManagementService.GetUserProjectStatusValueByProjectIdUserId(CurrentProjectId, m_LoginUserId)
            If dsUserProjectStatusValue.Tables.Count > 0 Then
                If dsUserProjectStatusValue.Tables(0).Rows.Count > 0 Then
                    If Not IsDBNull(dsUserProjectStatusValue.Tables(0).Rows(0)("UserProjectStatusValue")) Then
                        ddlProjectStatus.SelectedValue = dsUserProjectStatusValue.Tables(0).Rows(0)("UserProjectStatusValue")
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub gvProjects_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvProjects.Sorting
        GridViewSortExpression = e.SortExpression
        Dim pageIndex As Integer = gvProjects.PageIndex
        LoadCustomersData()
        gvProjects.DataSource = SortDataTable(gvProjects.DataSource, False)
        gvProjects.DataBind()
        gvProjects.PageIndex = pageIndex
    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbnView As LinkButton = CType(sender, LinkButton)
        If m_Scoped Then
            Dim dsScope As Scope = m_ScopeService.GetScopeByProjectId(m_Cryption.Decrypt(lbnView.CommandArgument, m_Cryption.cryptionKey))
            Response.Redirect(String.Format("../Projects/Detail.aspx?id={0}&sid={1}", lbnView.CommandArgument, m_Cryption.Encrypt(dsScope.ScopeId, m_Cryption.cryptionKey)))
        Else
            Response.Redirect(String.Format("../Projects/Detail.aspx?id={0}", lbnView.CommandArgument))
        End If
    End Sub

    Protected Sub lbnPre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim PrePage As Integer = gvProjects.PageIndex - 1
        gvProjects.PageIndex = PrePage
        LoadCustomersData()
    End Sub

    Protected Sub lbnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim NextPage As Integer = gvProjects.PageIndex + 1
        gvProjects.PageIndex = NextPage
        LoadCustomersData()
    End Sub

    Public Function DisplayNotes(ByVal ProjectId As String, ByVal ProjectStatusId As String, ByVal ScopeDate As String, ByVal StartDate As String, ByVal AssessmentDate As String, ByVal Hazard As String) As String
        Dim result As String = String.Empty
        Dim strNotes As String = String.Empty
        Dim intNotesLength As Integer = 65
        Dim dsNotes As DataSet = New DataSet()
        Dim dsScopeItem As DataSet = New DataSet()
        'dsNotes = m_ManagementService.GetUserNoteByUserID(CInt(ProjectId))
        dsNotes = m_ManagementService.GetUserNotesByUserIDProjectStatusID(CInt(ProjectId), ProjectStatusId)
        If dsNotes.Tables.Count > 0 Then
            If dsNotes.Tables(0).Rows.Count > 0 Then
                strNotes = String.Format("{0}", dsNotes.Tables(0).Rows(0)("NoteContent"))
                strNotes = strNotes.Trim()
            End If
        End If
        Select Case ProjectStatusId
            Case "0"
                Dim intScpoeId As Long = 0
                Dim Scope As Scope
                Scope = m_ScopeService.GetScopeByProjectId(CInt(ProjectId))
                Dim strScopeDescription As String = String.Empty
                dsScopeItem = m_ScopeService.GetScopeItemsHasDescriptionByScopeIdScopeItemStatusUserId(Scope.ScopeId, 2, m_LoginUserId)    'Approved Scope Items
                If dsScopeItem.Tables.Count > 0 Then
                    If dsScopeItem.Tables(0).Rows.Count > 0 Then
                        Dim index As Integer
                        Dim strScopeItemDescription As String = String.Empty
                        For index = 0 To dsScopeItem.Tables(0).Rows.Count - 1
                            strScopeItemDescription = String.Format("{0}", dsScopeItem.Tables(0).Rows(index)("Description")).Trim()
                            If strScopeItemDescription <> String.Empty Then
                                If strScopeDescription = String.Empty Then
                                    strScopeDescription = strScopeItemDescription
                                Else
                                    strScopeDescription = String.Format("{0}, {1}", strScopeDescription, strScopeItemDescription)
                                End If
                            End If
                        Next
                    End If
                End If
                If strNotes = String.Empty Then
                    strNotes = strScopeDescription
                Else
                    strNotes = String.Format("{0}, {1}", strNotes, strScopeDescription)
                End If
                If strNotes.Length > intNotesLength - 24 Then
                    result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength - 24))
                Else
                    result = String.Format("{0} {1}", result, strNotes)
                End If
                If ScopeDate.Trim() <> String.Empty Then
                    If result.Trim() = String.Empty Then
                        result = String.Format("<b>Scope Date: {0}</b>", CType(ScopeDate, DateTime).ToString("dd/MM/yyyy"))
                    Else
                        result = String.Format("<b>Scope Date: {0}</b>, {1}", CType(ScopeDate, DateTime).ToString("dd/MM/yyyy"), result)
                    End If
                End If

            Case "1"
                If strNotes.Length > intNotesLength - 24 Then
                    result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength - 24))
                Else
                    result = String.Format("{0} {1}", result, strNotes)
                End If
                If StartDate.Trim() <> String.Empty Then
                    If result.Trim() = String.Empty Then
                        result = String.Format("<b>Start Date: {0}</b>", CType(StartDate, DateTime).ToString("dd/MM/yyyy"))
                    Else
                        result = String.Format("<b>Start Date: {0}</b>, {1}", CType(StartDate, DateTime).ToString("dd/MM/yyyy"), result)
                    End If
                End If
            Case "5"
                If strNotes.Length > intNotesLength - 29 Then
                    result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength - 29))
                Else
                    result = String.Format("{0} {1}", result, strNotes)
                End If
                If AssessmentDate.Trim() <> String.Empty Then
                    If result.Trim() = String.Empty Then
                        result = String.Format("<b>Assessment Date: {0}</b>", CType(AssessmentDate, DateTime).ToString("dd/MM/yyyy"))
                    Else
                        result = String.Format("<b>Assessment Date: {0}</b>, {1}", CType(AssessmentDate, DateTime).ToString("dd/MM/yyyy"), result)
                    End If
                End If
            Case Else
                If strNotes.Length > intNotesLength Then
                    result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength))
                Else
                    result = String.Format("{0} {1}", result, strNotes)
                End If
        End Select
        result = result.Trim()

        ' old Version img links
        'If Hazard.Trim() = String.Empty Then
        '    result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'></td></tr></table>", result)
        'Else
        '    result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Hazard.aspx?id={1}{2}'><img src='../images/hazard.png' alt='View Hazard' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        'End If

        'If dsNotes.Tables.Count > 0 Then
        '    If dsNotes.Tables(0).Rows.Count > 0 Then
        '        If Hazard.Trim() = String.Empty Then
        '            result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Notes.aspx?id={1}&sid={2}{3}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(ProjectStatusId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        '        Else
        '            result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Hazard.aspx?id={1}{3}'><img src='../images/hazard.png' alt='View Hazard' width='26px' height='24px' border='0' /></a>&nbsp;&nbsp;<a class='form_popup' href='Notes.aspx?id={1}&sid={2}{3}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(ProjectStatusId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        '        End If
        '    End If
        'End If

        'If ProjectStatusId = 0 Then
        '    If dsScopeItem.Tables.Count > 0 Then
        '        If dsScopeItem.Tables(0).Rows.Count > 0 Then
        '            If Hazard.Trim() = String.Empty Then
        '                result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Notes.aspx?id={1}&sid={2}{3}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(ProjectStatusId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        '            Else
        '                result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Hazard.aspx?id={1}{3}'><img src='../images/hazard.png' alt='View Hazard' width='26px' height='24px' border='0' /></a>&nbsp;&nbsp;<a class='form_popup' href='Notes.aspx?id={1}&sid={2}{3}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(ProjectStatusId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        '            End If
        '        End If
        '    End If
        'End If

        Dim strImgLinksHTML As String = String.Empty
        If Hazard.Trim().Length > 0 Then
            strImgLinksHTML = String.Format("<a class='form_popup' href='Hazard.aspx?id={0}{1}'><img src='../images/hazard.png' alt='View Hazard' width='26px' height='24px' border='0' /></a>", m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        End If

        Dim intNotesCount As Integer = 0
        If dsNotes.Tables.Count > 0 Then
            If dsNotes.Tables(0).Rows.Count > 0 Then
                intNotesCount = dsNotes.Tables(0).Rows.Count
            End If
        End If
        If ProjectStatusId = 0 Then
            If dsScopeItem.Tables.Count > 0 Then
                If dsScopeItem.Tables(0).Rows.Count > 0 Then
                    intNotesCount = dsScopeItem.Tables(0).Rows.Count
                End If
            End If
        End If
        If intNotesCount > 0 Then
            strImgLinksHTML = strImgLinksHTML & "&nbsp;&nbsp;" & String.Format("<a class='form_popup' href='Notes.aspx?id={0}&sid={1}{2}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a>", m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(ProjectStatusId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        End If

        Dim dsTradeNotes As DataSet = New DataSet()
        dsTradeNotes = m_ManagementService.GetTradeNotesByUserIDProjectStatusID(CInt(ProjectId), ProjectStatusId)
        If dsTradeNotes.Tables.Count > 0 Then
            If dsTradeNotes.Tables(0).Rows.Count > 0 Then
                strImgLinksHTML = strImgLinksHTML & "&nbsp;&nbsp;" & String.Format("<a class='form_popup' href='TradeNotes.aspx?id={0}&sid={1}{2}'><img src='../images/trade-notes.png' alt='View Trade Notes' width='26px' height='24px' border='0' /></a>", m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(ProjectStatusId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
            End If
        End If
        strImgLinksHTML = strImgLinksHTML.Trim()
        If strImgLinksHTML = String.Empty Then
            result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'></td></tr></table>", result)
        Else
            result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'>{1}</td></tr></table>", result, strImgLinksHTML)
        End If
        Return result
    End Function

    Public Function DisplayNotes(ByVal ProjectId As String, ByVal ScopeDate As String, ByVal StartDate As String, ByVal AssessmentDate As String, ByVal Hazard As String) As String
        Dim result As String = String.Empty
        Dim strNotes As String = String.Empty
        Dim intNotesLength As Integer = 65
        Dim dsNotes As DataSet = New DataSet()
        Dim dsScopeItem As DataSet = New DataSet()

        Dim UserProjectStatusValue As Integer
        Dim dsUserProjectStatus As DataSet
        dsUserProjectStatus = m_ManagementService.GetUserProjectStatusValueByProjectIdUserId(ProjectId, m_LoginUserId)
        If dsUserProjectStatus.Tables.Count > 0 Then
            If dsUserProjectStatus.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(dsUserProjectStatus.Tables(0).Rows(0)("UserProjectStatusValue")) Then
                    UserProjectStatusValue = dsUserProjectStatus.Tables(0).Rows(0)("UserProjectStatusValue")
                End If
            End If
        End If
        'dsNotes = m_ManagementService.GetUserNoteByUserID(CInt(ProjectId))
        dsNotes = m_ManagementService.GetUserNotesByUserIDProjectStatusID(CInt(ProjectId), UserProjectStatusValue)
        If dsNotes.Tables.Count > 0 Then
            If dsNotes.Tables(0).Rows.Count > 0 Then
                strNotes = String.Format("{0}", dsNotes.Tables(0).Rows(0)("NoteContent"))
                strNotes = strNotes.Trim()
            End If
        End If
        Select Case UserProjectStatusValue
            'Case "-1000"            'change from 0
            '    Dim intScpoeId As Long = 0
            '    Dim Scope As Scope
            '    Scope = m_ScopeService.GetScopeByProjectId(CInt(ProjectId))
            '    Dim strScopeDescription As String = String.Empty
            '    dsScopeItem = m_ScopeService.GetScopeItemsHasDescriptionByScopeIdScopeItemStatusUserId(Scope.ScopeId, 2, m_LoginUserId)    'Approved Scope Items
            '    If dsScopeItem.Tables.Count > 0 Then
            '        If dsScopeItem.Tables(0).Rows.Count > 0 Then
            '            Dim index As Integer
            '            Dim strScopeItemDescription As String = String.Empty
            '            For index = 0 To dsScopeItem.Tables(0).Rows.Count - 1
            '                strScopeItemDescription = String.Format("{0}", dsScopeItem.Tables(0).Rows(index)("Description")).Trim()
            '                If strScopeItemDescription <> String.Empty Then
            '                    If strScopeDescription = String.Empty Then
            '                        strScopeDescription = strScopeItemDescription
            '                    Else
            '                        strScopeDescription = String.Format("{0}, {1}", strScopeDescription, strScopeItemDescription)
            '                    End If
            '                End If
            '            Next
            '        End If
            '    End If
            '    If strNotes = String.Empty Then
            '        strNotes = strScopeDescription
            '    Else
            '        strNotes = String.Format("{0}, {1}", strNotes, strScopeDescription)
            '    End If
            '    If strNotes.Length > intNotesLength - 24 Then
            '        result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength - 24))
            '    Else
            '        result = String.Format("{0} {1}", result, strNotes)
            '    End If
            '    If ScopeDate.Trim() <> String.Empty Then
            '        If result.Trim() = String.Empty Then
            '            result = String.Format("<b>Scope Date: {0}</b>", CType(ScopeDate, DateTime).ToString("dd/MM/yyyy"))
            '        Else
            '            result = String.Format("<b>Scope Date: {0}</b>, {1}", CType(ScopeDate, DateTime).ToString("dd/MM/yyyy"), result)
            '        End If
            '    End If

            Case "-1000", "-100"         'Change from 1
                If strNotes.Length > intNotesLength - 24 Then
                    result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength - 24))
                Else
                    result = String.Format("{0} {1}", result, strNotes)
                End If
                If StartDate.Trim() <> String.Empty Then
                    If result.Trim() = String.Empty Then
                        result = String.Format("<b>Start Date: {0}</b>", CType(StartDate, DateTime).ToString("dd/MM/yyyy"))
                    Else
                        result = String.Format("<b>Start Date: {0}</b>, {1}", CType(StartDate, DateTime).ToString("dd/MM/yyyy"), result)
                    End If
                End If
            Case "1000"         'Change from 5
                If strNotes.Length > intNotesLength - 29 Then
                    result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength - 29))
                Else
                    result = String.Format("{0} {1}", result, strNotes)
                End If
                If AssessmentDate.Trim() <> String.Empty Then
                    If result.Trim() = String.Empty Then
                        result = String.Format("<b>Assessment Date: {0}</b>", CType(AssessmentDate, DateTime).ToString("dd/MM/yyyy"))
                    Else
                        result = String.Format("<b>Assessment Date: {0}</b>, {1}", CType(AssessmentDate, DateTime).ToString("dd/MM/yyyy"), result)
                    End If
                End If
            Case Else
                If strNotes.Length > intNotesLength Then
                    result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength))
                Else
                    result = String.Format("{0} {1}", result, strNotes)
                End If
        End Select
        result = result.Trim()

        ' Display Hazard in later part
        'If Hazard.Trim() = String.Empty Then
        '    result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'></td></tr></table>", result)
        'Else
        '    result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Hazard.aspx?id={1}{2}'><img src='../images/hazard.png' alt='View Hazard' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        'End If

        'If dsNotes.Tables.Count > 0 Then
        '    If dsNotes.Tables(0).Rows.Count > 0 Then
        '        If Hazard.Trim() = String.Empty Then
        '            result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Notes.aspx?id={1}&sid={2}{3}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(UserProjectStatusValue, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        '        Else
        '            result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Hazard.aspx?id={1}{3}'><img src='../images/hazard.png' alt='View Hazard' width='26px' height='24px' border='0' /></a>&nbsp;&nbsp;<a class='form_popup' href='Notes.aspx?id={1}&sid={2}{3}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(UserProjectStatusValue, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        '        End If
        '    End If
        'End If

        'If UserProjectStatusValue = -1000 Then  'Change from 0
        '    If dsScopeItem.Tables.Count > 0 Then
        '        If dsScopeItem.Tables(0).Rows.Count > 0 Then
        '            If Hazard.Trim() = String.Empty Then
        '                result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Notes.aspx?id={1}&sid={2}{3}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(UserProjectStatusValue, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        '            Else
        '                result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Hazard.aspx?id={1}{3}'><img src='../images/hazard.png' alt='View Hazard' width='26px' height='24px' border='0' /></a>&nbsp;&nbsp;<a class='form_popup' href='Notes.aspx?id={1}&sid={2}{3}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(UserProjectStatusValue, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        '            End If
        '        End If
        '    End If
        'End If

        ' Old Version of img links
        'Dim HasNotes As Boolean = False
        'If dsNotes.Tables.Count > 0 Then
        '    If dsNotes.Tables(0).Rows.Count > 0 Then
        '        HasNotes = True
        '    End If
        'End If

        'If UserProjectStatusValue = -1000 Then  'Change from 0
        '    If dsScopeItem.Tables.Count > 0 Then
        '        If dsScopeItem.Tables(0).Rows.Count > 0 Then
        '            HasNotes = True
        '        End If
        '    End If
        'End If
        'If HasNotes Then
        '    If Hazard.Trim() = String.Empty Then
        '        result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Notes.aspx?id={1}&sid={2}{3}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(UserProjectStatusValue, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        '    Else
        '        result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='Hazard.aspx?id={1}{3}'><img src='../images/hazard.png' alt='View Hazard' width='26px' height='24px' border='0' /></a>&nbsp;&nbsp;<a class='form_popup' href='Notes.aspx?id={1}&sid={2}{3}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(UserProjectStatusValue, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        '    End If
        'End If

        Dim strImgLinksHTML As String = String.Empty
        If Hazard.Trim().Length > 0 Then
            strImgLinksHTML = String.Format("<a class='form_popup' href='Hazard.aspx?id={0}{1}'><img src='../images/hazard.png' alt='View Hazard' width='26px' height='24px' border='0' /></a>", m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        End If

        Dim intNotesCount As Integer = 0
        If dsNotes.Tables.Count > 0 Then
            If dsNotes.Tables(0).Rows.Count > 0 Then
                intNotesCount = dsNotes.Tables(0).Rows.Count
            End If
        End If
        If UserProjectStatusValue = -1000 Then  'Change from 0
            If dsScopeItem.Tables.Count > 0 Then
                If dsScopeItem.Tables(0).Rows.Count > 0 Then
                    intNotesCount = dsScopeItem.Tables(0).Rows.Count
                End If
            End If
        End If
        If intNotesCount > 0 Then
            strImgLinksHTML = strImgLinksHTML & "&nbsp;&nbsp;" & String.Format("<a class='form_popup' href='Notes.aspx?id={0}&sid={1}{2}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a>", m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(UserProjectStatusValue, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        End If

        Dim dsTradeNotes As DataSet = New DataSet()
        dsTradeNotes = m_ManagementService.GetTradeNotesByUserIDProjectStatusID(CInt(ProjectId), UserProjectStatusValue)
        If dsTradeNotes.Tables.Count > 0 Then
            If dsTradeNotes.Tables(0).Rows.Count > 0 Then
                strImgLinksHTML = strImgLinksHTML & "&nbsp;&nbsp;" & String.Format("<a class='form_popup' href='TradeNotes.aspx?id={0}&sid={1}{2}'><img src='../images/trade-notes.png' alt='View Trade Notes' width='26px' height='24px' border='0' /></a>", m_Cryption.Encrypt(ProjectId, m_Cryption.cryptionKey), m_Cryption.Encrypt(UserProjectStatusValue, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
            End If
        End If
        strImgLinksHTML = strImgLinksHTML.Trim()
        If strImgLinksHTML = String.Empty Then
            result = String.Format("<table width='100%'><tr><td width='180px'>{0}</td><td align='right'></td></tr></table>", result)
        Else
            result = String.Format("<table width='100%'><tr><td width='180px'>{0}</td><td align='right'>{1}</td></tr></table>", result, strImgLinksHTML)
        End If

        Return result
    End Function

    Protected Sub PropertyRating_Changed(ByVal sender As Object, ByVal e As AjaxControlToolkit.RatingEventArgs)
        If Request.Url.ToString.ToLower.IndexOf("search/view.aspx") > 0 Then
            Dim myRating As Rating = sender
            Dim TargetName As String = Replace(myRating.UniqueID, "PropertyRating", "tbProjectID")
            Dim TargetPropertyID As String = String.Empty
            For Each grow In gvProjects.Rows
                Dim tbProjectId As TextBox = CType(grow.FindControl("tbProjectID"), TextBox)

                If tbProjectId.UniqueID.Contains(TargetName) Then
                    TargetPropertyID = tbProjectId.Text
                    Exit For
                End If
            Next

            'If e.Value = "1" Then
            '    If TargetPropertyID <> "" Then
            '        m_ManagementService.UpdateProjectPriority(m_Cryption.Decrypt(TargetPropertyID, m_Cryption.cryptionKey), 0)
            '    End If
            '    'e.CallbackResult = "-1;"

            '    'Page.ClientScript.RegisterStartupScript(Me.GetType(), "RatingClearClick", String.Format("javascript:$find('{0}').set_Rating(0);", myRating.BehaviorID), True)
            '    Response.Redirect(Request.RawUrl, False)
            'Else
            If TargetPropertyID <> "" Then
                m_ManagementService.UpdateProjectPriority(m_Cryption.Decrypt(TargetPropertyID, m_Cryption.cryptionKey), e.Value)
            End If
            'End If
        Else
            If Session("IsGroupView") Then
                Dim myRating As Rating = sender
                Dim TargetName As String = Replace(myRating.UniqueID, "GroupPropertyRating", "tbProjectID")
                Dim TargetPropertyID As String = String.Empty
                For Each rptItem In rptProjectsGrid.Items
                    Dim itmProjects As GridView = CType(rptItem.FindControl("gvProjects"), GridView)
                    For Each grow In itmProjects.Rows
                        Dim tbProjectId As TextBox = CType(grow.FindControl("tbProjectID"), TextBox)

                        If tbProjectId.UniqueID.Contains(TargetName) Then
                            TargetPropertyID = tbProjectId.Text
                            Exit For
                        End If
                    Next
                    If TargetPropertyID <> "" Then
                        Exit For
                    End If
                Next

                'If e.Value = "1" Then
                '    If TargetPropertyID <> "" Then
                '        m_ManagementService.UpdateProjectPriority(m_Cryption.Decrypt(TargetPropertyID, m_Cryption.cryptionKey), 0)
                '    End If

                '    'Page.ClientScript.RegisterStartupScript(Me.GetType(), "RatingClearClick", String.Format("javascript:$find('{0}').set_Rating(0);", myRating.BehaviorID), True)
                '    Response.Redirect(Request.RawUrl, False)
                'Else
                If TargetPropertyID <> "" Then
                    m_ManagementService.UpdateProjectPriority(m_Cryption.Decrypt(TargetPropertyID, m_Cryption.cryptionKey), e.Value)
                End If
                'End If
            Else
                Dim myRating As Rating = sender
                Dim TargetName As String = Replace(myRating.UniqueID, "PropertyRating", "tbProjectID")
                Dim TargetPropertyID As String = String.Empty
                For Each grow In gvProjects.Rows
                    Dim tbProjectId As TextBox = CType(grow.FindControl("tbProjectID"), TextBox)

                    If tbProjectId.UniqueID.Contains(TargetName) Then
                        TargetPropertyID = tbProjectId.Text
                        Exit For
                    End If
                Next

                'If e.Value = "1" Then
                '    If TargetPropertyID <> "" Then
                '        m_ManagementService.UpdateProjectPriority(m_Cryption.Decrypt(TargetPropertyID, m_Cryption.cryptionKey), 0)
                '    End If
                '    'e.CallbackResult = "-1;"

                '    'Page.ClientScript.RegisterStartupScript(Me.GetType(), "RatingClearClick", String.Format("javascript:$find('{0}').set_Rating(0);", myRating.BehaviorID), True)
                '    Response.Redirect(Request.RawUrl, False)
                'Else
                If TargetPropertyID <> "" Then
                    m_ManagementService.UpdateProjectPriority(m_Cryption.Decrypt(TargetPropertyID, m_Cryption.cryptionKey), e.Value)
                End If
                'End If
            End If
        End If

        'Thread.Sleep(400)
        'e.CallbackResult = "Update done. Value = " + e.Value + " Tag = " + e.Tag
        'Response.Redirect("test=" & myRating.CurrentRating, False)
    End Sub

    Public Function DisplayRating(ByVal ProjectRating As String) As String
        If ProjectRating <> String.Empty Then
            Return ProjectRating
        End If
        Return 0
    End Function

    Public Function showClearImage(ByVal ProjectRating As String) As String
        'If ProjectRating <> String.Empty Then
        Return "../Images/FilledStar.png"
        'End If
        'Return "../Images/EmptyStar.png"
    End Function

    Protected Sub ddlPropertyStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        'Dim ddlPropertyStatus As DropDownList = CType(sender, DropDownList)
        'Dim ddlTargetName As String = Replace(ddlPropertyStatus.UniqueID, "ddlPropertyStatus", "tbProjectID")
        'Dim TargetPropertyID As String = String.Empty
        'For Each grow In gvProjects.Rows
        '    Dim tbProjectId As TextBox = CType(grow.FindControl("tbProjectID"), TextBox)

        '    If tbProjectId.UniqueID.Contains(ddlTargetName) Then
        '        TargetPropertyID = tbProjectId.Text
        '        Exit For
        '    End If
        'Next
        'm_ManagementService.UpdateProjectProjectStatusId(m_Cryption.Decrypt(TargetPropertyID, m_Cryption.cryptionKey), ddlPropertyStatus.SelectedValue)

        Dim ddlPropertyStatus As DropDownList = CType(sender, DropDownList)
        Dim gvr As GridViewRow = CType(CType(sender, Control).NamingContainer, GridViewRow)
        Dim tbProjectId As TextBox = CType(gvr.FindControl("tbProjectID"), TextBox)
        m_ManagementService.UpdateProjectProjectStatusId(m_Cryption.Decrypt(tbProjectId.Text, m_Cryption.cryptionKey), ddlPropertyStatus.SelectedValue)

        Dim Project As New Project
        Dim strProjectId As String = m_Cryption.Decrypt(tbProjectId.Text, m_Cryption.cryptionKey)
        Project = m_ManagementService.GetProjectByProjectId(CType(strProjectId, Long))
        Dim lblNotes As Label = CType(gvr.FindControl("lblNotes"), Label)
        lblNotes.Text = DisplayNotes(strProjectId, ddlPropertyStatus.SelectedValue, Project.ScopeDate.ToString(), Project.StartDate.ToString(), Project.AssessmentDate.ToString(), Project.Hazard)
    End Sub

    Protected Sub ddlProjectStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim ddlProjectStatus As DropDownList = CType(sender, DropDownList)
        Dim gvr As GridViewRow = CType(CType(sender, Control).NamingContainer, GridViewRow)
        Dim tbProjectId As TextBox = CType(gvr.FindControl("tbProjectID"), TextBox)
        Dim isExisted_ProjectStatus As Boolean = False
        Dim UserProjectStatusValue As UserProjectStatusValue = New UserProjectStatusValue
        UserProjectStatusValue.UserId = m_LoginUserId
        UserProjectStatusValue.ProjectId = m_Cryption.Decrypt(tbProjectId.Text, m_Cryption.cryptionKey)
        UserProjectStatusValue.UserProjectStatusValue = ddlProjectStatus.SelectedValue
        Dim dsUserProjectStatusValue As DataSet
        dsUserProjectStatusValue = m_ManagementService.GetUserProjectStatusValueByProjectIdUserId(UserProjectStatusValue.ProjectId, UserProjectStatusValue.UserId)
        If dsUserProjectStatusValue.Tables.Count > 0 Then
            If dsUserProjectStatusValue.Tables(0).Rows.Count > 0 Then
                isExisted_ProjectStatus = True
                UserProjectStatusValue.UserProjectStatusValueId = dsUserProjectStatusValue.Tables(0).Rows(0)("UserProjectStatusValueId")
            End If
        End If

        If isExisted_ProjectStatus Then
            m_ManagementService.UpdateUserProjectStatusValueByProjectIdUserId(UserProjectStatusValue)
        Else
            m_ManagementService.CreateUserProjectStatusValue(UserProjectStatusValue)
        End If

        Response.Redirect(Request.RawUrl)

        Dim Project As New Project
        Dim strProjectId As String = m_Cryption.Decrypt(tbProjectId.Text, m_Cryption.cryptionKey)
        Project = m_ManagementService.GetProjectByProjectId(CType(strProjectId, Long))
        Dim lblNotes As Label = CType(gvr.FindControl("lblUserNotes"), Label)
        lblNotes.Text = DisplayNotes(strProjectId, Project.ScopeDate.ToString(), Project.StartDate.ToString(), Project.AssessmentDate.ToString(), Project.Hazard)
        LoadCustomersData()
    End Sub

    Public Function isFileExisted(ByVal UserEmail As Object, ByVal FileName As Object) As String
        'Dim result As String = "display:none;"
        'If File.Exists(String.Format("{0}\images\{0}\{1}", ConfigurationManager.AppSettings("ProjectPath"), UserID, FileName)) Then
        '    result = String.Empty
        'End If
        'Return result
        Return ""
    End Function

    Public Function showFile(ByVal UserEmail As Object, ByVal FileName As Object) As String
        Dim result As String
        If File.Exists(String.Format("{0}\images\{1}\{2}", ConfigurationManager.AppSettings("ProjectPath"), UserEmail, FileName)) Then
            result = String.Format("../images/{0}/{1}", UserEmail, FileName)
        Else
            result = String.Format("../images/house.jpg")
        End If
        Return result
    End Function

    Protected Sub imgArchive_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim gvr As GridViewRow = CType(CType(sender, Control).NamingContainer, GridViewRow)
        Dim tbProjectId As TextBox = CType(gvr.FindControl("tbProjectID"), TextBox)
        Dim Project As Project = New Project
        Project.ProjectId = CInt(m_Cryption.Decrypt(tbProjectId.Text, m_Cryption.cryptionKey))
        Project.ArchivedDate = Today
        'm_ManagementService.UpdateProjectArchived(Project)
        m_ManagementService.UpdateProjectArchivedDateByUserId(Project, m_LoginUserId)
        LoadCustomersData()
    End Sub

    Public Function showArchive()
        Dim result As Boolean = True
        If m_Archived Then
            result = False
        End If
        Return result
    End Function

    Protected Sub imgunArchive_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim gvr As GridViewRow = CType(CType(sender, Control).NamingContainer, GridViewRow)
        Dim tbProjectId As TextBox = CType(gvr.FindControl("tbProjectID"), TextBox)
        Dim Project As Project = New Project
        Project.ProjectId = CInt(m_Cryption.Decrypt(tbProjectId.Text, m_Cryption.cryptionKey))
        Project.ArchivedDate = Nothing
        'm_ManagementService.UpdateProjectArchived(Project)
        m_ManagementService.UpdateProjectArchivedDateByUserId(Project, m_LoginUserId)
        LoadCustomersData()
    End Sub

    Public Function showunArchive()
        Dim result As Boolean = False
        If m_Archived Then
            result = True
        End If
        Return result
    End Function

    'Protected Sub lbnGroupView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnGroupView.Click
    '    Session("IsGroupView") = True
    '    LoadGroupData()
    '    rptProjectsGrid.Visible = True
    '    gvProjects.Visible = False

    '    UpdateCookies()
    'End Sub

    'Protected Sub lbnGridView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbnGridView.Click
    '    Session("IsGroupView") = False
    '    LoadCustomersData()
    '    rptProjectsGrid.Visible = False
    '    gvProjects.Visible = True

    '    UpdateCookies()
    'End Sub

    Protected Sub UpdateCookies()
        Dim blnUpdate As Boolean = True
        For I = 0 To Request.Cookies.Count - 1
            If Request.Cookies.Item(I).Name = m_UserId.ToString Then
                Request.Cookies.Remove(Request.Cookies.Item(I).Name)
                'Request.Cookies.Item(I).Value = True
                'blnUpdate = False
                Exit For
            End If
        Next

        If blnUpdate Then
            Response.Cookies(m_UserId.ToString).Expires = DateAdd(DateInterval.Month, 1, Now)
            Response.Cookies(m_UserId.ToString)("ScopePricing") = Session("ScopePricing")
            Response.Cookies(m_UserId.ToString)("GroupView") = Session("IsGroupView")
        End If
    End Sub

    Protected Sub imgPropertyRatingClear_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim objBtn As ImageButton = CType(sender, ImageButton)
        Dim TargetPropertyID As String = m_Cryption.Decrypt(objBtn.CommandArgument, m_Cryption.cryptionKey)
        If TargetPropertyID <> "" Then
            m_ManagementService.UpdateProjectPriority(TargetPropertyID, 0)
        End If
        Response.Redirect(Request.RawUrl)
    End Sub
End Class
