﻿Imports System.Data
Imports System.IO
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class WebControls_WebUserControlProjectGroupGrid
    Inherits System.Web.UI.UserControl
    Private m_CompanyId As Long
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
        LoadAreaData()
    End Sub

    Private Sub LoadAreaData()
        Dim dsArea As DataSet = New DataSet()
        dsArea = m_ScopeService.GetProjectGroupsByProjectOwnerId(m_CompanyId)
        gvProjectGroup.DataSource = dsArea.Tables(0)
        gvProjectGroup.DataBind()
    End Sub

    Protected Sub gvProjectGroup_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvProjectGroup.DataBound
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvProjectGroup.BottomPagerRow

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

            For i = 0 To gvProjectGroup.PageCount - 1

                ' Create a ListItem object to represent a page.
                Dim pageNumber As Integer = i + 1
                Dim item As ListItem = New ListItem(pageNumber.ToString())

                ' If the ListItem object matches the currently selected
                ' page, flag the ListItem object as being selected. Because
                ' the DropDownList control is recreated each time the pager
                ' row gets created, this will persist the selected item in
                ' the DropDownList control.   
                If i = gvProjectGroup.PageIndex Then

                    item.Selected = True

                End If

                ' Add the ListItem object to the Items collection of the 
                ' DropDownList.
                pageList.Items.Add(item)

            Next i

        End If

        If Not pageLabel Is Nothing Then

            ' Calculate the current page number.
            Dim currentPage As Integer = gvProjectGroup.PageIndex + 1

            ' Update the Label control with the current page information.
            pageLabel.Text = "Showing Page: " & currentPage.ToString() & _
                " of " & gvProjectGroup.PageCount.ToString()

        End If

        If gvProjectGroup.Rows.Count > 0 Then
            If gvProjectGroup.PageIndex = 0 Then
                If Not pagePre Is Nothing Then
                    pagePre.Visible = False
                End If
            ElseIf gvProjectGroup.PageCount - 1 = gvProjectGroup.PageIndex Then
                If Not pageNext Is Nothing Then
                    pageNext.Visible = False
                End If
            End If
        End If
    End Sub

    Protected Sub gvProjectGroup_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvProjectGroup.PageIndexChanging
        gvProjectGroup.PageIndex = e.NewPageIndex
        gvProjectGroup.DataBind()
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

    Protected Sub gvProjectGroup_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvProjectGroup.Sorting
        GridViewSortExpression = e.SortExpression
        Dim pageIndex As Integer = gvProjectGroup.PageIndex
        LoadAreaData()
        gvProjectGroup.DataSource = SortDataTable(gvProjectGroup.DataSource, False)
        gvProjectGroup.DataBind()
        gvProjectGroup.PageIndex = pageIndex
    End Sub

    Protected Sub lbnPre_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim PrePage As Integer = gvProjectGroup.PageIndex - 1
        gvProjectGroup.PageIndex = PrePage
        LoadAreaData()
    End Sub

    Protected Sub lbnNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim NextPage As Integer = gvProjectGroup.PageIndex + 1
        gvProjectGroup.PageIndex = NextPage
        LoadAreaData()
    End Sub

    Public Function showStatus(ByVal Status As String) As String
        Dim result As String = "OFF"
        If Status <> "True" Then
            result = "ON"
        End If
        Return result
    End Function

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Retrieve the pager row.
        Dim pagerRow As GridViewRow = gvProjectGroup.BottomPagerRow

        ' Retrieve the PageDropDownList DropDownList from the bottom pager row.
        Dim pageList As DropDownList = CType(pagerRow.Cells(0).FindControl("ddlPages"), DropDownList)

        ' Set the PageIndex property to display that page selected by the user.
        gvProjectGroup.PageIndex = pageList.SelectedIndex

        LoadAreaData()
        gvProjectGroup.DataSource = SortDataTable(gvProjectGroup.DataSource, True)
        gvProjectGroup.DataBind()
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
End Class
