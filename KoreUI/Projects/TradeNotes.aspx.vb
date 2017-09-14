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
Partial Class Projects_TradeNotes
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        ' Remove Status Id Query String for Note Link in Project Grid
        'Dim ProjectStatusId As Integer = -1
        'If Request.QueryString("sid") <> String.Empty Then
        '    Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("sid"), m_Cryption.cryptionKey), ProjectStatusId)
        'End If

        Dim ProjectId As Integer = 0
        If Request.QueryString("id") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey), ProjectId)
        End If
        Dim dsUserProjectStatusValue As DataSet
        Dim UserProjectStatusValue As Long
        dsUserProjectStatusValue = m_ManagementService.GetUserProjectStatusValueByProjectIdUserId(ProjectId, m_LoginUser.UserId)
        If dsUserProjectStatusValue.Tables.Count > 0 Then
            If dsUserProjectStatusValue.Tables(0).Rows.Count > 0 Then
                UserProjectStatusValue = dsUserProjectStatusValue.Tables(0).Rows(0)("UserProjectStatusValue")
            End If
        End If
        Dim strContentHtml As String = String.Empty
        Dim dsNotes As DataSet = New DataSet()
        'dsNotes = m_ManagementService.GetUserNoteByUserID(CInt(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey)))
        'dsNotes = m_ManagementService.GetUserNotesByUserIDProjectStatusID(CInt(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey)), ProjectStatusId)
        'dsNotes = m_ManagementService.GetUserNotesByUserIDUserProjectStatusValue(CInt(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey)), ProjectStatusId)
        dsNotes = m_ManagementService.GetTradeNotesByUserIDUserProjectStatusValue(CInt(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey)), UserProjectStatusValue)
        Dim UserProjectStatusName As String = "General"
        Dim dsUserProjectStatusSettings As DataSet
        dsUserProjectStatusSettings = m_ManagementService.GetProjectStatusesByProjectIdUserId(ProjectId, m_LoginUser.UserId)
        For Each dr In dsUserProjectStatusSettings.Tables(0).Rows
            If dr("StatusValue") = UserProjectStatusValue Then
                UserProjectStatusName = dr("Name")
                Exit For
            End If
        Next
        If dsNotes.Tables.Count > 0 Then
            If dsNotes.Tables(0).Rows.Count > 0 Then
                strContentHtml = "<h4>Notes</h4><br />"
                For Each rowNote As DataRow In dsNotes.Tables(0).Rows
                    'strContentHtml = String.Format("{0}<div class='formlabel'>{1} {2}</div><div class='formlabel'>{3}</div><br />", strContentHtml, rowNote("ProjectStatusName"), rowNote("Description"), rowNote("NoteContent"))
                    strContentHtml = String.Format("{0}<div class='formlabel'>{1} {2}</div><div class='formlabel'>{3}</div><br />", strContentHtml, UserProjectStatusName, rowNote("Description"), rowNote("NoteContent"))
                Next
            End If
        End If

        noteRepeater.DataSource = dsNotes.Tables(0)
        noteRepeater.DataBind()

        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim intScpoeId As Long = 0
        Dim Scope As Scope
        Scope = m_ScopeService.GetScopeByProjectId(CInt(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey)))
        Dim strScopeDescription As String = String.Empty
        Dim dsScopeItem As DataSet = New DataSet()
        dsScopeItem = m_ScopeService.GetScopeItemsHasDescriptionByScopeIdScopeItemStatusUserId(Scope.ScopeId, 2, m_LoginUser.UserId)    'Approved Scope Items
        If dsScopeItem.Tables.Count > 0 Then
            If dsScopeItem.Tables(0).Rows.Count > 0 Then
                If strContentHtml = String.Empty Then
                    strContentHtml = "<h4>Notes</h4><br />"
                End If
                Dim index As Integer
                Dim strScopeItemDescription As String = String.Empty
                For index = 0 To dsScopeItem.Tables(0).Rows.Count - 1
                    strContentHtml = String.Format("{0}<div class='formlabel'>{1} {2}<br />{3}</div><div class='formlabel'>{4}</div><br />", strContentHtml, dsScopeItem.Tables(0).Rows(index)("Area"), dsScopeItem.Tables(0).Rows(index)("Item"), dsScopeItem.Tables(0).Rows(index)("Service"), dsScopeItem.Tables(0).Rows(index)("Description"))
                Next
            End If
        End If

        scopeDescriptionRepeater.DataSource = dsScopeItem.Tables(0)
        scopeDescriptionRepeater.DataBind()

        divNotes.InnerHtml = strContentHtml

        If UserProjectStatusValue > -1000 Then         'Change from ProjectStatusId > 0
            tblProjectNote.Visible = True
            tblScopeDescription.Visible = False
        Else
            tblProjectNote.Visible = True
            tblScopeDescription.Visible = True
        End If

    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Public Function GetUserProjectStatusName(ByVal strUserProjectStatusValue As String) As String
        Dim result As String = "General"
        Dim ProjectId As Integer = 0
        If Request.QueryString("id") <> String.Empty Then
            Integer.TryParse(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey), ProjectId)
        End If
        Dim dsUserProjectStatusSettings As DataSet
        dsUserProjectStatusSettings = m_ManagementService.GetProjectStatusesByProjectIdUserId(ProjectId, m_LoginUser.UserId)
        For Each dr In dsUserProjectStatusSettings.Tables(0).Rows
            If dr("StatusValue") = CInt(strUserProjectStatusValue) Then
                result = dr("Name")
                Exit For
            End If
        Next

        Return result
    End Function
End Class