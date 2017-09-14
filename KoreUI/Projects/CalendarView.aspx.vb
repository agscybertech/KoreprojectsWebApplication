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

Partial Class Projects_CalendarView
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_Cryption As New Cryption()    
    Private m_ProjectOwner As Long

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        'Dim m_SystemMessageControl As Interfaces_Rent_Ezi_UserControls_WebUserControl = CType(LoadControl("../UserControls/SystemMessagesControl.ascx"), Interfaces_Rent_Ezi_UserControls_WebUserControl)
        'm_SystemMessageControl = LoadControl("../UserControls/SystemMessagesControl.ascx")
        'm_SystemMessageControl.ForTenant = True
        'Dim user As User = Session("CurrentLogin")
        'AppointmentsGrid.CompanyId = user.CompanyId
        'AppointmentsGrid.BranchId = user.BranchId
        'AppointmentsGrid.UserId = user.UserId
        'AppointmentsGrid.UserType = user.Type

        'If m_LoginUser.BranchId = 0 Then
        '    If Session("BranchID") = Nothing Then
        '        Session("ReturnFromBranches") = "Patients.aspx"
        '        Response.Redirect("Branches.aspx")
        '    End If
        'End If

        'ProjectsGrid.CompanyId = m_LoginUser.CompanyId
        'If m_LoginUser.BranchId = 0 Then
        '    ProjectsGrid.BranchId = Session("BranchId")
        'Else
        '    ProjectsGrid.BranchId = m_LoginUser.BranchId
        'End If
        'ProjectsGrid.UserId = m_LoginUser.UserId
        'ProjectsGrid.UserType = m_LoginUser.Type
        'If Not Session("Keyword") Is Nothing Then
        '    ProjectsGrid.Keyword = Session("Keyword")   'for Search
        'Else
        '    ProjectsGrid.Keyword = ""   'for Search
        'End If
        If Request.QueryString("msg") <> "" Then
            lblMsg.Text = Request.QueryString("msg")
        End If
        Session("QuickCreate") = Request.Url

        lblTitle.Text = "Project Planner"

        LoadReminders()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
            'If m_LoginUser.Type < 1 Then
            '    Session("CurrentLogin") = Nothing
            '    Response.Redirect("../Signin.aspx?msg=Please contact administrator.")
            'End If
        End If
    End Sub

    Public Function GetEncryptId() As String
        Dim result As String = String.Empty
        result = m_Cryption.Encrypt(m_LoginUser.UserId.ToString(), m_Cryption.cryptionKey)
        Return result
    End Function

    Public Function GetGotoDateScript() As String
        Dim result As String = String.Empty
        Dim dtGotoDate As DateTime
        If DateTime.TryParse(Request.QueryString("date"), dtGotoDate) Then
            result = String.Format("$('#calendar').fullCalendar( 'gotoDate', {0}, {1}, {2});", dtGotoDate.Year, dtGotoDate.Month - 1, dtGotoDate.Day)
        End If
        Return result
    End Function

    Private Sub LoadReminders()
        Dim dsReminders As DataSet
        dsReminders = m_ManagementService.GetRemindersByProjectOwnerId(m_LoginUser.CompanyId)
        If dsReminders.Tables.Count > 0 Then
            rptReminders.DataSource = dsReminders.Tables(0)
            rptReminders.DataBind()

            Dim objHtmlGeneric As HtmlGenericControl
            'Dim strContent As String
            'Dim objRegex As Regex = New Regex("(\r\n|\r|\n)+")

            For Each repeatDataItem As RepeaterItem In rptReminders.Items
                If Not IsDBNull(dsReminders.Tables(0).Rows(repeatDataItem.ItemIndex)("ReminderTitle")) Then
                    objHtmlGeneric = New HtmlGenericControl("div")
                    objHtmlGeneric.Attributes.Add("style", "font-size:smaller;")
                    objHtmlGeneric.InnerHtml = dsReminders.Tables(0).Rows(repeatDataItem.ItemIndex)("ReminderTitle")
                    repeatDataItem.Controls.Add(objHtmlGeneric)
                End If

                If Not IsDBNull(dsReminders.Tables(0).Rows(repeatDataItem.ItemIndex)("Status")) Then
                    If dsReminders.Tables(0).Rows(repeatDataItem.ItemIndex)("Status") = 2 Then
                        If Not IsDBNull(dsReminders.Tables(0).Rows(repeatDataItem.ItemIndex)("StartDate")) Then
                            objHtmlGeneric = New HtmlGenericControl("img")
                            objHtmlGeneric.Attributes.Add("src", "../images/bell.gif")
                            objHtmlGeneric.Attributes.Add("title", String.Format("{0:d MMM h:mm tt}", dsReminders.Tables(0).Rows(repeatDataItem.ItemIndex)("StartDate")))
                            objHtmlGeneric.Attributes.Add("style", "float:left;")                            
                            repeatDataItem.Controls.Add(objHtmlGeneric)
                        End If
                    End If
                End If
                'If Not IsDBNull(dsReminders.Tables(0).Rows(repeatDataItem.ItemIndex)("ReminderContentData")) Then
                '    objHtmlGeneric = New HtmlGenericControl("div")
                '    strContent = String.Format("{0}", dsReminders.Tables(0).Rows(repeatDataItem.ItemIndex)("ReminderContentData"))
                '    strContent = objRegex.Replace(strContent, "<br /><br />")
                '    objHtmlGeneric.InnerHtml = strContent
                '    repeatDataItem.Controls.Add(objHtmlGeneric)
                'End If

                objHtmlGeneric = New HtmlGenericControl("a")
                objHtmlGeneric.Attributes.Add("href", String.Format("EditReminder.aspx?id={0}", m_Cryption.Encrypt(dsReminders.Tables(0).Rows(repeatDataItem.ItemIndex)("ReminderId").ToString(), m_Cryption.cryptionKey)))
                objHtmlGeneric.Attributes.Add("style", "float:right;")
                objHtmlGeneric.Attributes.Add("id", "noteboard_button")
                objHtmlGeneric.InnerText = "Edit"
                repeatDataItem.Controls.Add(objHtmlGeneric)
                objHtmlGeneric = New HtmlGenericControl("br")
                repeatDataItem.Controls.Add(objHtmlGeneric)
                objHtmlGeneric = New HtmlGenericControl("br")
                repeatDataItem.Controls.Add(objHtmlGeneric)
            Next
        End If
    End Sub
End Class