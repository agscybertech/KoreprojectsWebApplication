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
Imports System.Xml

Partial Class Timesheet_CheckIn
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_ProjectOwner As New ProjectOwner
    Private m_Cryption As New Cryption
    Private m_Timesheet As TimesheetEntry
    Private isValidID As Boolean
    Private isValidDate As Boolean
    Private intTimeSheetEntryID As Integer
    Private dtTimeSheetEntryDate As Date

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        
        isValidID = False
        isValidDate = False
        If Session("TimeSheetEntryDate") Is Nothing Then
            If Integer.TryParse(Session("TimeSheetEntryID"), intTimeSheetEntryID) Then
                isValidID = True
            End If
        End If
        If Session("TimeSheetEntryID") Is Nothing Then
            If Date.TryParse(Session("TimeSheetEntryDate"), dtTimeSheetEntryDate) Then
                lblCdate.Text = dtTimeSheetEntryDate.ToString("ddd d-MMM-yyyy")
                isValidDate = True
            End If
        End If
        If isValidID Then
            m_Timesheet = m_ManagementService.GetTimesheetEntryByTimesheetEntryId(intTimeSheetEntryID)
            lblCdate.Text = m_Timesheet.EntryDate.ToString("ddd d-MMM-yyyy")
            If Not IsPostBack Then
                If m_Timesheet.WorkStart <> Nothing Then
                    tbxWorkStart.Text = Replace(m_Timesheet.WorkStart.ToString("hh:mm tt"), ".", "")
                End If
                If m_Timesheet.WorkEnd <> Nothing Then
                    tbxWorkEnd.Text = Replace(m_Timesheet.WorkEnd.ToString("hh:mm tt"), ".", "")
                End If
                If m_Timesheet.LunchStart <> Nothing Then
                    tbxLunchStart.Text = Replace(m_Timesheet.LunchStart.ToString("hh:mm tt"), ".", "")
                End If
                If m_Timesheet.LunchEnd <> Nothing Then
                    tbxLunchEnd.Text = Replace(m_Timesheet.LunchEnd.ToString("hh:mm tt"), ".", "")
                End If
                If m_Timesheet.WorkStart <> Nothing And m_Timesheet.WorkEnd <> Nothing Then
                    lblThour.Text = String.Format("Total Hours: {0}", DateDiff(DateInterval.Minute, m_Timesheet.WorkStart, m_Timesheet.WorkEnd) / 60)
                    If m_Timesheet.LunchStart <> Nothing And m_Timesheet.LunchEnd <> Nothing Then
                        lblThour.Text = String.Format("Total Hours: {0}", (DateDiff(DateInterval.Minute, m_Timesheet.WorkStart, m_Timesheet.WorkEnd) - DateDiff(DateInterval.Minute, m_Timesheet.LunchStart, m_Timesheet.LunchEnd)) / 60)
                    End If
                End If
            End If
        ElseIf isValidDate Then
            btnDelete.Visible = False
        Else
            lblMsg.Text = "Oops! We are really sorry but this page was unable to display."
        End If

        If Not isValidID Then
            tbxWorkStart.Attributes.Add("onchange", String.Format("setTargetTime(this.value,'{0}','{1}','{2}')", tbxWorkEnd.ClientID, tbxLunchStart.ClientID, tbxLunchEnd.ClientID))
            tbxLunchStart.Attributes.Add("onchange", String.Format("setTargetTime(this.value,'{0}','{1}','{2}')", tbxWorkEnd.ClientID, String.Empty, tbxLunchEnd.ClientID))
            tbxLunchEnd.Attributes.Add("onchange", String.Format("setTargetTime(this.value,'{0}','{1}','{2}')", tbxWorkEnd.ClientID, String.Empty, String.Empty))
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Login.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
            'If m_LoginUser.Type = 0 Then
            '    btnDelete.Visible = False
            'End If
        End If
        If Request.QueryString("msg") <> "" Then
            lblMsg.Text = Request.QueryString("msg")
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If isValidID Then
            If tbxWorkStart.Text = String.Empty Then
                m_Timesheet.WorkStart = Nothing
            Else
                DateTime.TryParse(m_Timesheet.EntryDate.ToString("yyyy/MM/dd") & " " & tbxWorkStart.Text, m_Timesheet.WorkStart)
            End If
            If tbxWorkEnd.Text = String.Empty Then
                m_Timesheet.WorkEnd = Nothing
            Else
                DateTime.TryParse(m_Timesheet.EntryDate.ToString("yyyy/MM/dd") & " " & tbxWorkEnd.Text, m_Timesheet.WorkEnd)
            End If
            If tbxLunchStart.Text = String.Empty Then
                m_Timesheet.LunchStart = Nothing
            Else
                DateTime.TryParse(m_Timesheet.EntryDate.ToString("yyyy/MM/dd") & " " & tbxLunchStart.Text, m_Timesheet.LunchStart)
            End If
            If tbxLunchEnd.Text = String.Empty Then
                m_Timesheet.LunchEnd = Nothing
            Else
                DateTime.TryParse(m_Timesheet.EntryDate.ToString("yyyy/MM/dd") & " " & tbxLunchEnd.Text, m_Timesheet.LunchEnd)
            End If
            m_ManagementService.UpdateTimesheetEntry(m_Timesheet)
        ElseIf isValidDate Then
            m_Timesheet = New TimesheetEntry()
            m_Timesheet.PartyA = m_LoginUser.CompanyId
            m_Timesheet.PartyB = m_LoginUser.UserId
            If tbxWorkStart.Text = String.Empty Then
                m_Timesheet.WorkStart = Nothing
            Else
                DateTime.TryParse(dtTimeSheetEntryDate.ToString("yyyy/MM/dd") & " " & tbxWorkStart.Text, m_Timesheet.WorkStart)
            End If
            If tbxWorkEnd.Text = String.Empty Then
                m_Timesheet.WorkEnd = Nothing
            Else
                DateTime.TryParse(dtTimeSheetEntryDate.ToString("yyyy/MM/dd") & " " & tbxWorkEnd.Text, m_Timesheet.WorkEnd)
            End If
            If tbxLunchStart.Text = String.Empty Then
                m_Timesheet.LunchStart = Nothing
            Else
                DateTime.TryParse(dtTimeSheetEntryDate.ToString("yyyy/MM/dd") & " " & tbxLunchStart.Text, m_Timesheet.LunchStart)
            End If
            If tbxLunchEnd.Text = String.Empty Then
                m_Timesheet.LunchEnd = Nothing
            Else
                DateTime.TryParse(dtTimeSheetEntryDate.ToString("yyyy/MM/dd") & " " & tbxLunchEnd.Text, m_Timesheet.LunchEnd)
            End If
            DateTime.TryParse(dtTimeSheetEntryDate.ToString("yyyy/MM/dd"), m_Timesheet.EntryDate)
            m_ManagementService.CreateTimesheetEntry(m_Timesheet)
        End If
        Response.Redirect("../Timesheet/?msg=Timesheet is saved!")
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As System.EventArgs) Handles btnDelete.Click
        m_ManagementService.DeleteTimesheetEntry(intTimeSheetEntryID, False)
        Response.Redirect("../Timesheet/?msg=Timesheet is deleted!")
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Response.Redirect("../Timesheet/")
    End Sub
End Class
