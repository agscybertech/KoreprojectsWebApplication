Imports System.Data
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities
Imports AjaxControlToolkit
Imports System.Threading

Partial Class WebControls_WebUserControlTimesheetGrid
    Inherits System.Web.UI.UserControl
    Private m_CompanyId As Long
    Private m_BranchId As Long
    Private m_UserId As Long
    Private m_UserType As Integer
    Private m_DateFrom As DateTime
    Private m_DateTo As DateTime
    Private m_Keyword As String
    Private m_ManagementService As New ManagementService()
    Private m_DatabaseService As New DatabaseService()
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

        If Not IsPostBack Then
            LoadTimesheetData()
        End If
    End Sub

    Private Sub LoadTimesheetData()
        If m_CompanyId > 0 And Not IsNothing(m_DateFrom) And Not IsNothing(m_DateTo) Then
            Dim dsTimesheet As DataSet = New DataSet()
            ''If m_UserType = 0 Then
            '    'dscontacts = m_ManagementService.GetUserProfilesByPartyA(m_UserId)
            '    dsTimesheet = m_ManagementService.GetUserProfilesRelatedByUserId(m_UserId)
            'Else
            '    dsTimesheet = m_ManagementService.GetUserProfilesByPartyAType(m_UserId, m_UserType)
            'End If

            dsTimesheet = m_ManagementService.GetTimesheetEntrySummaryByPartyAEntryDateRange(m_CompanyId, m_DateFrom, m_DateTo)
            'If dsTimesheet.Tables.Count > 0 Then
            '    If dsTimesheet.Tables(0).Rows.Count > 0 Then
            '        'For Each Coll As DataColumn In dsTimesheet.Tables(0).Columns


            '        'Next
            '        'For Each Row As DataRow In dsTimesheet.Tables(0).Rows

            '        'Next

            '        'For i As Integer = 0 To dsTimesheet.Tables(0).Columns.Count - 1
            '        '    If dsTimesheet.Tables(0).Rows(0).Item(i).ToString = "SubTotal" Then
            '        '        dsTimesheet.Tables(0).Rows(0).Item(i) = "Sub Total"
            '        '    ElseIf dsTimesheet.Tables(0).Rows(0).Item(i).ToString <> "UserId" And dsTimesheet.Tables(0).Rows(0).Item(i).ToString <> "Name" Then

            '        '    End If
            '        'Next

            '        dsTimesheet.Tables(0).Columns.RemoveAt(0)
            '    End If
            'End If
            gvTimesheet.PageSize = 18
            gvTimesheet.DataSource = dsTimesheet.Tables(0)
            gvTimesheet.DataBind()
        End If
    End Sub

    Protected Sub gvTimesheet_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTimesheet.DataBound
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvTimesheet.BottomPagerRow

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

            For i = 0 To gvTimesheet.PageCount - 1

                ' Create a ListItem object to represent a page.
                Dim pageNumber As Integer = i + 1
                Dim item As ListItem = New ListItem(pageNumber.ToString())

                ' If the ListItem object matches the currently selected
                ' page, flag the ListItem object as being selected. Because
                ' the DropDownList control is recreated each time the pager
                ' row gets created, this will persist the selected item in
                ' the DropDownList control.   
                If i = gvTimesheet.PageIndex Then

                    item.Selected = True

                End If

                ' Add the ListItem object to the Items collection of the 
                ' DropDownList.
                pageList.Items.Add(item)

            Next i

        End If

        If Not pageLabel Is Nothing Then

            ' Calculate the current page number.
            Dim currentPage As Integer = gvTimesheet.PageIndex + 1

            ' Update the Label control with the current page information.
            pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                " of " & gvTimesheet.PageCount.ToString()

        End If

        If gvTimesheet.Rows.Count > 0 Then
            If gvTimesheet.PageIndex = 0 Then
                If Not pagePre Is Nothing Then
                    pagePre.Visible = False
                End If
            ElseIf gvTimesheet.PageCount - 1 = gvTimesheet.PageIndex Then
                If Not pageNext Is Nothing Then
                    pageNext.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvTimesheet_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTimesheet.PageIndexChanging
        gvTimesheet.PageIndex = e.NewPageIndex
        gvTimesheet.DataBind()
    End Sub

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvTimesheet.BottomPagerRow

        ' Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)

        ' Set the PageIndex property to display that page selected by the user.
        gvTimesheet.PageIndex = pageList.SelectedIndex

        LoadTimesheetData()
        gvTimesheet.DataSource = SortDataTable(gvTimesheet.DataSource, True)
        gvTimesheet.DataBind()
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

    Protected Sub gvTimesheet_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvTimesheet.RowCreated
        e.Row.Cells(0).Visible = False
    End Sub

    Protected Sub gvTimesheet_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvTimesheet.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(1).Text = "TOTAL" Then
                e.Row.Style.Add("font-weight", "bold")
                e.Row.Cells(10).Text = ""
            Else
                e.Row.Cells(1).Text = String.Format("<a href='#' onclick='window.open(""../Print/Timesheets.aspx?id={0}"", ""_blank"", ""toolbar=yes,scrollbars=yes,resizable=yes,top=50,left=50,width=1000,height=650"");'>{1}</a>", m_Cryption.Encrypt(e.Row.Cells(0).Text, m_Cryption.cryptionKey), e.Row.Cells(1).Text)
                e.Row.Cells(10).Text = String.Format("<a href='#' onclick='window.open(""../Print/Timesheets.aspx?id={0}"", ""_blank"", ""toolbar=yes,scrollbars=yes,resizable=yes,top=50,left=50,width=1000,height=650"");'>{1}</a>", m_Cryption.Encrypt(e.Row.Cells(0).Text, m_Cryption.cryptionKey), e.Row.Cells(10).Text.ToUpper())
            End If
        End If
    End Sub

    Protected Sub gvTimesheet_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvTimesheet.Sorting
        GridViewSortExpression = e.SortExpression
        Dim pageIndex As Integer = gvTimesheet.PageIndex
        LoadTimesheetData()
        gvTimesheet.DataSource = SortDataTable(gvTimesheet.DataSource, False)
        gvTimesheet.DataBind()
        gvTimesheet.PageIndex = pageIndex
    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbnView As LinkButton = CType(sender, LinkButton)
        Response.Redirect(String.Format("../Contacts/Detail.aspx?id={0}", lbnView.CommandArgument))
        'Response.Write(String.Format("../Patient/view.aspx?id={0}", m_Cryption.Decrypt(lbnView.CommandArgument, m_Cryption.cryptionKey)))
    End Sub

    Protected Sub lbnPre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim PrePage As Integer = gvTimesheet.PageIndex - 1
        gvTimesheet.PageIndex = PrePage
        LoadTimesheetData()
    End Sub

    Protected Sub lbnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim NextPage As Integer = gvTimesheet.PageIndex + 1
        gvTimesheet.PageIndex = NextPage
        LoadTimesheetData()
    End Sub

    Public Function DisplayPhones(ByVal ContactId As String) As String
        Dim result As String = String.Empty
        Dim cUser As UserProfile
        cUser = m_ManagementService.GetUserProfileByUserID(ContactId)
        If cUser.Contact1 <> String.Empty Then
            result = String.Format("<b>H:</b>{0}", cUser.Contact1)
        End If
        If cUser.Contact2 <> String.Empty Then
            If result <> String.Empty Then
                result = String.Format("{0}&nbsp;&nbsp;&nbsp;<b>W:</b>{1}", result, cUser.Contact2)
            Else
                result = String.Format("<b>W:</b>{0}", cUser.Contact2)
            End If
        End If
        If cUser.Contact3 <> String.Empty Then
            If result <> String.Empty Then
                result = String.Format("{0}&nbsp;&nbsp;&nbsp;<b>M:</b>{1}", result, cUser.Contact3)
            Else
                result = String.Format("<b>M:</b>{0}", cUser.Contact3)
            End If
        End If
        result = String.Format("<font style='font-family:Verdana'>{0}</font>", result)
        Return result
    End Function

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
            result = String.Format("../images/male-avatar.jpg")
        End If
        Return result
    End Function
End Class
