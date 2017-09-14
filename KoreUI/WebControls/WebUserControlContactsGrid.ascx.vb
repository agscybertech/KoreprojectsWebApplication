Imports System.Data
Imports System.IO
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities
Imports AjaxControlToolkit
Imports System.Threading

Partial Class WebControls_WebUserControlContactsGrid
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
        If Not IsPostBack Then
            LoadContactsData()
        End If
    End Sub

    Private Sub LoadContactsData()
        Dim dscontacts As DataSet = New DataSet()
        If m_UserType = 0 Then
            'dscontacts = m_ManagementService.GetUserProfilesByPartyA(m_UserId)
            dscontacts = m_ManagementService.GetUserProfilesRelatedByUserId(m_UserId)
        Else
            dscontacts = m_ManagementService.GetUserProfilesByPartyAType(m_UserId, m_UserType)
        End If
        gvcontacts.PageSize = 18
        gvcontacts.DataSource = dscontacts.Tables(0)
        gvcontacts.DataBind()
    End Sub

    Protected Sub gvContacts_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvcontacts.DataBound
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvcontacts.BottomPagerRow

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

            For i = 0 To gvcontacts.PageCount - 1

                ' Create a ListItem object to represent a page.
                Dim pageNumber As Integer = i + 1
                Dim item As ListItem = New ListItem(pageNumber.ToString())

                ' If the ListItem object matches the currently selected
                ' page, flag the ListItem object as being selected. Because
                ' the DropDownList control is recreated each time the pager
                ' row gets created, this will persist the selected item in
                ' the DropDownList control.   
                If i = gvcontacts.PageIndex Then

                    item.Selected = True

                End If

                ' Add the ListItem object to the Items collection of the 
                ' DropDownList.
                pageList.Items.Add(item)

            Next i

        End If

        If Not pageLabel Is Nothing Then

            ' Calculate the current page number.
            Dim currentPage As Integer = gvContacts.PageIndex + 1

            ' Update the Label control with the current page information.
            pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                " of " & gvContacts.PageCount.ToString()

        End If

        If gvContacts.Rows.Count > 0 Then
            If gvContacts.PageIndex = 0 Then
                If Not pagePre Is Nothing Then
                    pagePre.Visible = False
                End If
            ElseIf gvContacts.PageCount - 1 = gvContacts.PageIndex Then
                If Not pageNext Is Nothing Then
                    pageNext.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvContacts_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvContacts.PageIndexChanging
        gvContacts.PageIndex = e.NewPageIndex
        gvContacts.DataBind()
    End Sub

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvContacts.BottomPagerRow

        ' Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)

        ' Set the PageIndex property to display that page selected by the user.
        gvContacts.PageIndex = pageList.SelectedIndex

        LoadContactsData()
        gvContacts.DataSource = SortDataTable(gvContacts.DataSource, True)
        gvContacts.DataBind()
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

    Protected Sub gvContacts_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvContacts.RowCommand
        If e.CommandName = "ChangeStatus" Then
            Dim lkbStatus As LinkButton = DirectCast(e.CommandSource, LinkButton)

            If lkbStatus.Text.Trim().ToLower() = "inactive" AndAlso Not m_ManagementService.CanAddOrActiveUser(m_UserId, e.CommandArgument) Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "StatusChange", "alert(""You can't activate contact because you already have other active contacts as per you subscription limit."");", True)
                Exit Sub
            Else
                m_ManagementService.ChangeContactStatus(e.CommandArgument, If(lkbStatus.Text.Trim().ToLower() = "inactive", 2, 1))
            End If

            LoadContactsData()
        End If
    End Sub

    Protected Sub gvContacts_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvContacts.Sorting
        GridViewSortExpression = e.SortExpression
        Dim pageIndex As Integer = gvContacts.PageIndex
        LoadContactsData()
        gvContacts.DataSource = SortDataTable(gvContacts.DataSource, False)
        gvContacts.DataBind()
        gvContacts.PageIndex = pageIndex
    End Sub

    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lbnView As LinkButton = CType(sender, LinkButton)
        Response.Redirect(String.Format("../Contacts/Detail.aspx?id={0}", lbnView.CommandArgument))
        'Response.Write(String.Format("../Patient/view.aspx?id={0}", m_Cryption.Decrypt(lbnView.CommandArgument, m_Cryption.cryptionKey)))
    End Sub

    Protected Sub lbnPre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim PrePage As Integer = gvContacts.PageIndex - 1
        gvContacts.PageIndex = PrePage
        LoadContactsData()
    End Sub

    Protected Sub lbnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim NextPage As Integer = gvContacts.PageIndex + 1
        gvContacts.PageIndex = NextPage
        LoadContactsData()
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
