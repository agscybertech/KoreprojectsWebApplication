Imports System.Data
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class WebControls_WebUserControlJobGrid
    Inherits System.Web.UI.UserControl
    Private m_CompanyId As Long
    Private m_BranchId As Long
    Private m_UserId As Long
    Private m_UserType As Integer
    Private m_LoginUserId As Long
    Private m_DateFrom As DateTime
    Private m_DateTo As DateTime
    Private m_Keyword As String
    Private m_ManagementService As New ManagementService()
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
        LoadJobsData()
    End Sub

    Private Sub LoadJobsData()
        Dim dsJobs As DataSet = New DataSet()
        dsJobs = m_ManagementService.GetJobsByProjectId(m_UserId)
        gvJobs.DataSource = dsJobs.Tables(0)
        gvJobs.DataBind()
    End Sub

    Protected Sub gvJobs_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvJobs.DataBound
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvJobs.BottomPagerRow

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

            For i = 0 To gvJobs.PageCount - 1

                ' Create a ListItem object to represent a page.
                Dim pageNumber As Integer = i + 1
                Dim item As ListItem = New ListItem(pageNumber.ToString())

                ' If the ListItem object matches the currently selected
                ' page, flag the ListItem object as being selected. Because
                ' the DropDownList control is recreated each time the pager
                ' row gets created, this will persist the selected item in
                ' the DropDownList control.   
                If i = gvJobs.PageIndex Then

                    item.Selected = True

                End If

                ' Add the ListItem object to the Items collection of the 
                ' DropDownList.
                pageList.Items.Add(item)

            Next i

        End If

        If Not pageLabel Is Nothing Then

            ' Calculate the current page number.
            Dim currentPage As Integer = gvJobs.PageIndex + 1

            ' Update the Label control with the current page information.
            pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                " of " & gvJobs.PageCount.ToString()

        End If

        If gvJobs.Rows.Count > 0 Then
            If gvJobs.PageIndex = 0 Then
                If Not pagePre Is Nothing Then
                    pagePre.Visible = False
                End If
            ElseIf gvJobs.PageCount - 1 = gvJobs.PageIndex Then
                If Not pageNext Is Nothing Then
                    pageNext.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvJobs_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvJobs.PageIndexChanging
        gvJobs.PageIndex = e.NewPageIndex
        gvJobs.DataBind()
    End Sub

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvJobs.BottomPagerRow

        ' Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)

        ' Set the PageIndex property to display that page selected by the user.
        gvJobs.PageIndex = pageList.SelectedIndex

        LoadJobsData()
        gvJobs.DataSource = SortDataTable(gvJobs.DataSource, True)
        gvJobs.DataBind()
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

    Protected Sub gvJobs_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvJobs.Sorting
        GridViewSortExpression = e.SortExpression
        Dim pageIndex As Integer = gvJobs.PageIndex
        LoadJobsData()
        gvJobs.DataSource = SortDataTable(gvJobs.DataSource, False)
        gvJobs.DataBind()
        gvJobs.PageIndex = pageIndex
    End Sub

    Protected Sub lbnPre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim PrePage As Integer = gvJobs.PageIndex - 1
        gvJobs.PageIndex = PrePage
        LoadJobsData()
    End Sub

    Protected Sub lbnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim NextPage As Integer = gvJobs.PageIndex + 1
        gvJobs.PageIndex = NextPage
        LoadJobsData()
    End Sub

    Public Function ShowNote(ByVal Content As String) As String
        Dim NoteContent As String = String.Empty
        If Content <> "" Then
            If Content.Length > 100 Then
                If Request.Url.ToString.ToLower.IndexOf("/print.aspx") > 0 Then
                    NoteContent = Content
                Else
                    NoteContent = Content.Substring(0, 100) & " ..."
                End If
            Else
                NoteContent = Content
            End If
        End If
        Return NoteContent
    End Function

    Public Function GetJobAssigneeName(ByVal strJobID As String) As String
        Dim result As String = String.Empty
        'Dim ProjectId As Integer = 0
        'If Request.QueryString("id") <> String.Empty Then
        '    Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey), ProjectId)
        'End If
        Dim objJob As Job
        Dim intCounter As Integer = 0
        objJob = m_ManagementService.GetJobByJobId(strJobID)

        Dim objUserProfile As UserProfile
        'JobAssignment Null issue fixed 
        'Ibran 4/5/2017
        ''If objJob.JobAssignments IsNot Nothing Then
        If (Not objJob.JobAssignments Is Nothing AndAlso objJob.JobAssignments.Count > 0) Then

            For Each objJobAssignment As JobAssignment In objJob.JobAssignments
                objUserProfile = objJobAssignment.UserProfile
                If intCounter = 0 Then
                    result = objUserProfile.FirstName.Trim
                Else
                    result = String.Format("{0}, {1}", result, objUserProfile.FirstName.Trim)
                End If
                intCounter += 1
            Next
        End If

        Return result
    End Function

    Public Function ShowStatus(ByVal StatusValue As String) As String
        Dim JobStatus As String = String.Empty
        Dim intJobStatus As Integer
        For Each intJobStatus In [Enum].GetValues(GetType(JobStatus))
            If intJobStatus.ToString = StatusValue Then
                JobStatus = Replace([Enum].GetName(GetType(JobStatus), intJobStatus), "_", " ")
                Exit For
            End If
        Next intJobStatus
        Return JobStatus
    End Function
End Class
