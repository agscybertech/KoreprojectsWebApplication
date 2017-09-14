Imports System.Data
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities
Imports AjaxControlToolkit
Imports System.Threading

Partial Class WebControls_WebUserControlPrintScopesPendingGrid
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
        LoadScopesData()
        'End If
    End Sub

    Private Sub LoadScopesData()
        Dim dsScopes As DataSet = New DataSet()
        dsScopes = m_ScopeService.GetScopeItemsByScopeIdScopeItemStatusUserId(m_ScopeID, 1, m_LoginUserId)
        gvScopesPending.PageSize = 18
        gvScopesPending.DataSource = dsScopes.Tables(0)
        gvScopesPending.DataBind()

        If Request.Url.ToString.ToLower.IndexOf("/printscope.aspx") > 0 Then
            gvScopesPending.Columns(9).Visible = False
            gvScopesPending.Columns(7).Visible = False
            gvScopesPending.Columns(6).Visible = False
            If Request.QueryString("act") <> "" Then
                If m_Cryption.Decrypt(Request.QueryString("act"), m_Cryption.cryptionKey) = "ShowRate" Then
                    gvScopesPending.Columns(7).Visible = True
                    gvScopesPending.Columns(6).Visible = True
                End If
            End If
        End If

        Dim blnScopePricing As Boolean = False
        Boolean.TryParse(Session("ScopePricing"), blnScopePricing)
        If blnScopePricing Then
            gvScopesPending.Columns(7).Visible = True
            gvScopesPending.Columns(6).Visible = True
        Else
            gvScopesPending.Columns(7).Visible = False
            gvScopesPending.Columns(6).Visible = False
        End If
    End Sub

    Protected Sub gvScopesPending_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvScopesPending.DataBound
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvScopesPending.BottomPagerRow

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

            For i = 0 To gvScopesPending.PageCount - 1

                ' Create a ListItem object to represent a page.
                Dim pageNumber As Integer = i + 1
                Dim item As ListItem = New ListItem(pageNumber.ToString())

                ' If the ListItem object matches the currently selected
                ' page, flag the ListItem object as being selected. Because
                ' the DropDownList control is recreated each time the pager
                ' row gets created, this will persist the selected item in
                ' the DropDownList control.   
                If i = gvScopesPending.PageIndex Then

                    item.Selected = True

                End If

                ' Add the ListItem object to the Items collection of the 
                ' DropDownList.
                pageList.Items.Add(item)

            Next i

        End If

        If Not pageLabel Is Nothing Then

            ' Calculate the current page number.
            Dim currentPage As Integer = gvScopesPending.PageIndex + 1

            ' Update the Label control with the current page information.
            pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                " of " & gvScopesPending.PageCount.ToString()

        End If

        If gvScopesPending.Rows.Count > 0 Then
            If gvScopesPending.PageIndex = 0 Then
                If Not pagePre Is Nothing Then
                    pagePre.Visible = False
                End If
            ElseIf gvScopesPending.PageCount - 1 = gvScopesPending.PageIndex Then
                If Not pageNext Is Nothing Then
                    pageNext.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvScopesPending_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvScopesPending.PageIndexChanging
        gvScopesPending.PageIndex = e.NewPageIndex
        gvScopesPending.DataBind()
    End Sub

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvScopesPending.BottomPagerRow

        ' Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)

        ' Set the PageIndex property to display that page selected by the user.
        gvScopesPending.PageIndex = pageList.SelectedIndex

        LoadScopesData()
        gvScopesPending.DataSource = SortDataTable(gvScopesPending.DataSource, True)
        gvScopesPending.DataBind()
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

    Protected Sub gvScopesPending_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvScopesPending.Sorting
        GridViewSortExpression = e.SortExpression
        Dim pageIndex As Integer = gvScopesPending.PageIndex
        LoadScopesData()
        gvScopesPending.DataSource = SortDataTable(gvScopesPending.DataSource, False)
        gvScopesPending.DataBind()
        gvScopesPending.PageIndex = pageIndex
    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbnView As LinkButton = CType(sender, LinkButton)
        Dim arrIDs As Array = Split(lbnView.CommandArgument, ",")
        Response.Redirect(String.Format("../Scopes/Detail.aspx?id={0}&sid={1}", arrIDs(0), arrIDs(1)))
    End Sub

    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbnApprove As LinkButton = CType(sender, LinkButton)
        Dim arrIDs As Array = Split(lbnApprove.CommandArgument, ",")
        m_ScopeService.UpdateScopeItemApproveByScopeItemId(m_Cryption.Decrypt(arrIDs(1), m_Cryption.cryptionKey))
        m_ScopeService.UpdateTotal(m_Cryption.Decrypt(arrIDs(0), m_Cryption.cryptionKey))
        Response.Redirect(String.Format("Detail.aspx?id={0}&sid={1}", Request.QueryString("ID"), Request.QueryString("SID")))
        'Response.Write(String.Format("../Patient/view.aspx?id={0}", m_Cryption.Decrypt(lbnView.CommandArgument, m_Cryption.cryptionKey)))
    End Sub

    Protected Sub lbnPre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim PrePage As Integer = gvScopesPending.PageIndex - 1
        gvScopesPending.PageIndex = PrePage
        LoadScopesData()
    End Sub

    Protected Sub lbnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim NextPage As Integer = gvScopesPending.PageIndex + 1
        gvScopesPending.PageIndex = NextPage
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

    Public Function DisplayNotes(ByVal ScopeItemId As String, ByVal AssignTo As String) As String
        Dim result As String = String.Empty
        Dim strNotes As String = String.Empty
        Dim intNotesLength As Integer = 65
        Dim ScopeItem As ScopeItem
        ScopeItem = m_ScopeService.GetScopeItemByScopeItemId(CLng(ScopeItemId))
        strNotes = String.Format("{0}", ScopeItem.Description)
        If AssignTo.Trim() <> String.Empty Then
            strNotes = String.Format("{0} - {1}", AssignTo, strNotes)
        End If

        'If strNotes.Length > intNotesLength Then
        '    result = String.Format("{0} {1}...", result, strNotes.Substring(0, intNotesLength))
        'Else
        result = String.Format("{0} {1}", result, strNotes)
        'End If

        result = Replace(result.Trim(), Chr(13) & Chr(10), "<br>")
        'If result = String.Empty Or result.Length <= intNotesLength Then
        'result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'></td></tr></table>", result)
        'Else
        '    result = String.Format("<table width='100%'><tr><td>{0}</td><td align='right'><a class='form_popup' href='ScopeItemNotes.aspx?id={1}{2}'><img src='../images/info.png' alt='View Notes' width='26px' height='24px' border='0' /></a></td></tr></table>", result, m_Cryption.Encrypt(ScopeItemId, m_Cryption.cryptionKey), String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss")))
        'End If
        Return result
    End Function
End Class
