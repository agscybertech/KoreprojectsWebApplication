Imports System.Data
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities
Imports AjaxControlToolkit
Imports System.Threading

Partial Class WebControls_WebUserControlScopesApprovedGrid
    Inherits System.Web.UI.UserControl

    Private m_CompanyId As Long
    Private m_BranchId As Long
    Private m_UserId As Long
    Private m_UserType As Integer
    Private m_ScopeID As Integer
    Private m_LoginUserId As Long
    Private m_DateFrom As DateTime
    Private m_DateTo As DateTime
    Private m_Keyword As String
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

    Public Property ScopeID() As Integer
        Get
            Return m_ScopeID
        End Get
        Set(ByVal value As Integer)
            m_ScopeID = value
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_DatabaseService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        'If Not IsPostBack Then
        LoadGroupData()
        LoadScopesData()

        If Session("IsGroupView") Then
            rptScopesApproved.Visible = True
            gvScopesApproved.Visible = False
        Else
            rptScopesApproved.Visible = False
            gvScopesApproved.Visible = True
        End If
        'End If
    End Sub

    Private Sub LoadGroupData()
        Dim dsGroup As DataSet = New DataSet()
        'dsGroup = m_ScopeService.GetWorksheetGroupsByProjectOwnerId(m_CompanyId)
        dsGroup = m_ScopeService.GetWorksheetGroupsByScopeIdScopeItemStatusUserId(m_ScopeID, 2, m_LoginUserId)
        'Dim newGroupRow As DataRow = dsGroup.Tables(0).NewRow()
        'newGroupRow("WorksheetGroupId") = "0"
        'newGroupRow("Name") = "Unassigned"
        'dsGroup.Tables(0).Rows.Add(newGroupRow)
        rptScopesApproved.DataSource = dsGroup.Tables(0)
        rptScopesApproved.DataBind()
    End Sub

    Protected Sub rptScopesApproved_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptScopesApproved.ItemDataBound
        If (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim gvGroupScopesApproved As GridView = CType(e.Item.FindControl("gvScopesApproved"), GridView)
            If Not gvGroupScopesApproved Is Nothing Then
                Dim row As DataRowView = CType(e.Item.DataItem, DataRowView)
                gvGroupScopesApproved.DataSource = m_ScopeService.GetScopeItemsByScopeIdScopeItemStatusUserIdGroupId(m_ScopeID, 2, m_LoginUserId, Convert.ToInt64(row("WorksheetGroupId")))
                gvGroupScopesApproved.DataBind()

                'If Request.Url.ToString.ToLower.IndexOf("/printscope.aspx") > 0 Then
                '    gvGroupScopesApproved.Columns(9).Visible = False
                '    gvGroupScopesApproved.Columns(7).Visible = False
                '    gvGroupScopesApproved.Columns(6).Visible = False
                '    If Request.QueryString("act") <> "" Then
                '        If m_Cryption.Decrypt(Request.QueryString("act"), m_Cryption.cryptionKey) = "ShowRate" Then
                '            gvGroupScopesApproved.Columns(7).Visible = True
                '            gvGroupScopesApproved.Columns(6).Visible = True
                '        End If
                '    End If
                'End If

                Dim blnScopePricing As Boolean = False
                Boolean.TryParse(Session("ScopePricing"), blnScopePricing)
                If blnScopePricing Then
                    gvGroupScopesApproved.Columns(7).Visible = True
                    gvGroupScopesApproved.Columns(8).Visible = True
                Else
                    gvGroupScopesApproved.Columns(7).Visible = False
                    gvGroupScopesApproved.Columns(8).Visible = False
                End If
            End If

            ' Retrieve the pager row.
            Dim pagerRow As GridViewRow = gvGroupScopesApproved.BottomPagerRow

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

                For i = 0 To gvGroupScopesApproved.PageCount - 1

                    ' Create a ListItem object to represent a page.
                    Dim pageNumber As Integer = i + 1
                    Dim item As ListItem = New ListItem(pageNumber.ToString())

                    ' If the ListItem object matches the currently selected
                    ' page, flag the ListItem object as being selected. Because
                    ' the DropDownList control is recreated each time the pager
                    ' row gets created, this will persist the selected item in
                    ' the DropDownList control.   
                    If i = gvGroupScopesApproved.PageIndex Then

                        item.Selected = True

                    End If

                    ' Add the ListItem object to the Items collection of the 
                    ' DropDownList.
                    pageList.Items.Add(item)

                Next i

            End If

            If Not pageLabel Is Nothing Then

                ' Calculate the current page number.
                Dim currentPage As Integer = gvGroupScopesApproved.PageIndex + 1

                ' Update the Label control with the current page information.
                pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                    " of " & gvGroupScopesApproved.PageCount.ToString()

            End If

            If gvGroupScopesApproved.Rows.Count > 0 Then
                If gvGroupScopesApproved.PageIndex = 0 Then
                    If Not pagePre Is Nothing Then
                        pagePre.Visible = False
                    End If
                ElseIf gvGroupScopesApproved.PageCount - 1 = gvGroupScopesApproved.PageIndex Then
                    If Not pageNext Is Nothing Then
                        pageNext.Visible = False
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub LoadScopesData()
        Dim dsScopes As DataSet = New DataSet()
        dsScopes = m_ScopeService.GetScopeItemsByScopeIdScopeItemStatusUserId(m_ScopeID, 2, m_LoginUserId)
        gvScopesApproved.PageSize = 18
        gvScopesApproved.DataSource = dsScopes.Tables(0)
        gvScopesApproved.DataBind()

        'If Request.Url.ToString.ToLower.IndexOf("/printscope.aspx") > 0 Then
        '    gvScopesApproved.Columns(9).Visible = False
        '    gvScopesApproved.Columns(7).Visible = False
        '    gvScopesApproved.Columns(6).Visible = False
        '    If Request.QueryString("act") <> "" Then
        '        If m_Cryption.Decrypt(Request.QueryString("act"), m_Cryption.cryptionKey) = "ShowRate" Then
        '            gvScopesApproved.Columns(7).Visible = True
        '            gvScopesApproved.Columns(6).Visible = True
        '        End If
        '    End If
        'End If

        Dim blnScopePricing As Boolean = False
        Boolean.TryParse(Session("ScopePricing"), blnScopePricing)
        If blnScopePricing Then
            gvScopesApproved.Columns(7).Visible = True
            gvScopesApproved.Columns(8).Visible = True
        Else
            gvScopesApproved.Columns(7).Visible = False
            gvScopesApproved.Columns(8).Visible = False
        End If
    End Sub

    Protected Sub gvScopesApproved_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvScopesApproved.DataBound
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvScopesApproved.BottomPagerRow

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

            For i = 0 To gvScopesApproved.PageCount - 1

                ' Create a ListItem object to represent a page.
                Dim pageNumber As Integer = i + 1
                Dim item As ListItem = New ListItem(pageNumber.ToString())

                ' If the ListItem object matches the currently selected
                ' page, flag the ListItem object as being selected. Because
                ' the DropDownList control is recreated each time the pager
                ' row gets created, this will persist the selected item in
                ' the DropDownList control.   
                If i = gvScopesApproved.PageIndex Then

                    item.Selected = True

                End If

                ' Add the ListItem object to the Items collection of the 
                ' DropDownList.
                pageList.Items.Add(item)

            Next i

        End If

        If Not pageLabel Is Nothing Then

            ' Calculate the current page number.
            Dim currentPage As Integer = gvScopesApproved.PageIndex + 1

            ' Update the Label control with the current page information.
            pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                " of " & gvScopesApproved.PageCount.ToString()

        End If

        If gvScopesApproved.Rows.Count > 0 Then
            If gvScopesApproved.PageIndex = 0 Then
                If Not pagePre Is Nothing Then
                    pagePre.Visible = False
                End If
            ElseIf gvScopesApproved.PageCount - 1 = gvScopesApproved.PageIndex Then
                If Not pageNext Is Nothing Then
                    pageNext.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvScopesApproved_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvScopesApproved.PageIndexChanging
        gvScopesApproved.PageIndex = e.NewPageIndex
        gvScopesApproved.DataBind()
    End Sub

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvScopesApproved.BottomPagerRow

        ' Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)

        ' Set the PageIndex property to display that page selected by the user.
        gvScopesApproved.PageIndex = pageList.SelectedIndex

        LoadScopesData()
        gvScopesApproved.DataSource = SortDataTable(gvScopesApproved.DataSource, True)
        gvScopesApproved.DataBind()
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

    Protected Sub gvScopesApproved_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvScopesApproved.Sorting
        GridViewSortExpression = e.SortExpression
        Dim pageIndex As Integer = gvScopesApproved.PageIndex
        LoadScopesData()
        gvScopesApproved.DataSource = SortDataTable(gvScopesApproved.DataSource, False)
        gvScopesApproved.DataBind()
        gvScopesApproved.PageIndex = pageIndex
    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbnView As LinkButton = CType(sender, LinkButton)
        Dim arrIDs As Array = Split(lbnView.CommandArgument, ",")
        Response.Redirect(String.Format("../Scopes/Detail.aspx?id={0}&sid={1}", arrIDs(0), arrIDs(1)))
        'Response.Write(String.Format("../Patient/view.aspx?id={0}", m_Cryption.Decrypt(lbnView.CommandArgument, m_Cryption.cryptionKey)))
    End Sub

    Protected Sub btnDisapprove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbnDisapprove As LinkButton = CType(sender, LinkButton)
        Dim arrIDs As Array = Split(lbnDisapprove.CommandArgument, ",")
        m_ScopeService.UpdateScopeItemDisapproveByScopeItemId(m_Cryption.Decrypt(arrIDs(1), m_Cryption.cryptionKey))
        m_ScopeService.UpdateTotal(m_Cryption.Decrypt(arrIDs(0), m_Cryption.cryptionKey))
        Response.Redirect(String.Format("Detail.aspx?id={0}&sid={1}", Request.QueryString("ID"), Request.QueryString("SID")))
        'Response.Write(String.Format("../Patient/view.aspx?id={0}", m_Cryption.Decrypt(lbnView.CommandArgument, m_Cryption.cryptionKey)))
    End Sub

    Protected Sub lbnPre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim PrePage As Integer = gvScopesApproved.PageIndex - 1
        gvScopesApproved.PageIndex = PrePage
        LoadScopesData()
    End Sub

    Protected Sub lbnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim NextPage As Integer = gvScopesApproved.PageIndex + 1
        gvScopesApproved.PageIndex = NextPage
        LoadScopesData()
    End Sub

    Public Function DisplayQTY(ByVal QTY As String, ByVal Unit As String) As String
        Dim result As String = String.Empty
        Dim decQTY As Decimal
        Decimal.TryParse(QTY, decQTY)
        If Unit <> "None" Then
            result = String.Format("{0:N2} {1}", decQTY, Unit)
        Else
            result = decQTY.ToString("N1")
        End If
        Return result
    End Function

    Public Function DisplayArea(ByVal Area As String, ByVal AreaMeasurement As String) As String
        Dim strArea As String
        Dim intAreaLength As Integer = 25
        If AreaMeasurement <> "" Then
            If AreaMeasurement.Length > intAreaLength Then
                strArea = String.Format("{0}<br>{1}...", Area, AreaMeasurement.Substring(0, intAreaLength))
            Else
                strArea = String.Format("{0}<br>{1}", Area, AreaMeasurement)
            End If
        Else
            strArea = Area
        End If
        Return strArea
    End Function

    Public Function DisplayNotes(ByVal ScopeItemId As String, ByVal AssignTo As String) As String
        Dim result As String = String.Empty
        Dim strNotes As String = String.Empty
        Dim intNotesLength As Integer = 40
        Dim ScopeItem As ScopeItem
        ScopeItem = m_ScopeService.GetScopeItemByScopeItemId(CLng(ScopeItemId))
        strNotes = String.Format("{0}", ScopeItem.Description)
        If AssignTo.Trim() <> String.Empty Then
            strNotes = String.Format("{0} - {1}", AssignTo, strNotes)
        End If

        If strNotes.Length > intNotesLength Then
            result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength))
        Else
            result = String.Format("{0} {1}", result, strNotes)
        End If

        result = result.Trim()
        If result = String.Empty Or result.Length <= intNotesLength Then
            result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'></td></tr></table>", result)
        Else
            result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='ScopeItemNotes.aspx?id={1}{2}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ScopeItemId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        End If
        Return result
    End Function
End Class
