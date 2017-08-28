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

Partial Class Contacts_Timesheets
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_ProjectOwner As New ProjectOwner

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Request.QueryString("msg") <> "" Then
            lblMsg.Text = Request.QueryString("msg")
        End If
        lblTitle.Text = "Time Sheets"

        Dim isValid As Boolean
        Dim dStart As Date
        Dim dEnd As Date

        isValid = True
        m_ProjectOwner = m_ManagementService.GetProjectOwnerByProjectOwnerId(m_LoginUser.CompanyId)

        If Not IsPostBack And IsNothing(Session("TimesheetStartDate")) Then
            If Not IsNothing(m_ProjectOwner.PaymentStartDate) Then
                dStart = m_ProjectOwner.PaymentStartDate
            Else
                isValid = False
            End If

            If isValid Then
                If DateDiff(DateInterval.Day, dStart, Now) < 0 Then
                    isValid = False
                End If
            End If

            If isValid Then
                Select Case m_ProjectOwner.Frequency
                    Case "1,w"
                        dEnd = DateAdd(DateInterval.Day, 7, dStart)
                        Do Until Now >= dStart And Now < dEnd
                            dStart = dEnd
                            dEnd = DateAdd(DateInterval.Day, 7, dStart)
                        Loop
                    Case "2,w"
                        dEnd = DateAdd(DateInterval.Day, 14, dStart)
                        Do Until Now >= dStart And Now <= dEnd
                            dStart = dEnd
                            dEnd = DateAdd(DateInterval.Day, 14, dStart)
                        Loop
                    Case "1,m"
                        dEnd = DateAdd(DateInterval.Month, 1, dStart)
                        Do Until Now >= dStart And Now <= dEnd
                            dStart = dEnd
                            dEnd = DateAdd(DateInterval.Month, 1, dStart)
                        Loop
                    Case Else
                        isValid = False
                End Select
            End If
        Else
            If Not IsNothing(Session("TimesheetStartDate")) Then
                dStart = Session("TimesheetStartDate")
            Else
                isValid = False
            End If

            If isValid Then
                If DateDiff(DateInterval.Day, dStart, Now) < 0 Then
                    isValid = False
                End If
            End If

            If isValid Then
                Select Case m_ProjectOwner.Frequency
                    Case "1,w"
                        dEnd = DateAdd(DateInterval.Day, 7, dStart)
                    Case "2,w"
                        dEnd = DateAdd(DateInterval.Day, 14, dStart)
                    Case "1,m"
                        dEnd = DateAdd(DateInterval.Month, 1, dStart)
                    Case Else
                        isValid = False
                End Select
            End If
        End If

        If IsValid Then
            TimesheetGrid.CompanyId = m_LoginUser.CompanyId
            TimesheetGrid.UserType = 0
            TimesheetGrid.BranchId = 0
            TimesheetGrid.UserId = m_LoginUser.UserId
            TimesheetGrid.Keyword = ""
            TimesheetGrid.DateFrom = dStart
            TimesheetGrid.DateTo = DateAdd(DateInterval.Day, -1, dEnd)
            lblDateRange.Text = String.Format("{0:d MMM yyyy} ~ {1:d MMM yyyy}", dStart, DateAdd(DateInterval.Day, -1, dEnd))
            Session("TimesheetStartDate") = dStart

            btnClose.Visible = False
            Dim dsTimesheets As DataSet = m_ManagementService.GetTimesheetEntrySummaryByPartyAEntryDateRange(m_LoginUser.CompanyId, dStart, DateAdd(DateInterval.Day, -1, dEnd))
            If dsTimesheets.Tables(0).Rows(0)("UserId") <> -1 Then
                Dim dsTimesheetDetails As DataSet = m_ManagementService.GetTimesheetEntryByPartyAPartyBEntryDateRange(m_LoginUser.CompanyId, dsTimesheets.Tables(0).Rows(0)("UserId"), dStart, DateAdd(DateInterval.Day, -1, dEnd))
                If IsDBNull(dsTimesheetDetails.Tables(0).Rows(0)("ProcessDate")) Then
                    btnClose.Text = "Close Time Sheets"
                    btnClose.OnClientClick = "if (confirm('Closing off time sheets for current period. OK to continue.')) {return true;} else {return false;}"
                    btnClose.Visible = True
                Else
                    btnClose.Text = "Open Time Sheets"
                    btnClose.OnClientClick = "if (confirm('Opening time sheets for current period. OK to continue.')) {return true;} else {return false;}"
                    btnClose.Visible = True
                End If
            End If

            'If DateDiff(DateInterval.Day, dStart, Now) >= 0 And DateDiff(DateInterval.Day, dEnd, Now) < 0 Then
            '    btnClose.Visible = True
            'Else
            '    btnClose.Visible = False
            'End If
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Login.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
            'If m_LoginUser.Type < 1 Then
            '    Session("CurrentLogin") = Nothing
            '    Response.Redirect("../Login.aspx?msg=Please contact administrator.")
            'End If
        End If
    End Sub

    Protected Sub lbnPrev_Click(sender As Object, e As EventArgs) Handles lbnPrev.Click
        Dim isValid As Boolean
        Dim dStart As Date
        Dim dEnd As Date

        isValid = True
        If Not IsNothing(Session("TimesheetStartDate")) Then
            dStart = Session("TimesheetStartDate")
        Else
            IsValid = False
        End If

        If IsValid Then
            If DateDiff(DateInterval.Day, dStart, Now) < 0 Then
                IsValid = False
            End If
        End If

        If IsValid Then
            Select Case m_ProjectOwner.Frequency
                Case "1,w"
                    dEnd = DateAdd(DateInterval.Day, -7, dStart)
                Case "2,w"
                    dEnd = DateAdd(DateInterval.Day, -14, dStart)
                Case "1,m"
                    dEnd = DateAdd(DateInterval.Month, -1, dStart)
                Case Else
                    IsValid = False
            End Select
        End If

        If isValid Then
            Session("TimesheetStartDate") = dEnd
            Response.Redirect(Request.RawUrl)
        End If
    End Sub

    Protected Sub lbnNext_Click(sender As Object, e As EventArgs) Handles lbnNext.Click
        Dim isValid As Boolean
        Dim dStart As Date
        Dim dEnd As Date

        isValid = True
        If Not IsNothing(Session("TimesheetStartDate")) Then
            dStart = Session("TimesheetStartDate")
        Else
            isValid = False
        End If

        If isValid Then
            If DateDiff(DateInterval.Day, dStart, Now) < 0 Then
                isValid = False
            End If
        End If

        If isValid Then
            Select Case m_ProjectOwner.Frequency
                Case "1,w"
                    dEnd = DateAdd(DateInterval.Day, 7, dStart)
                Case "2,w"
                    dEnd = DateAdd(DateInterval.Day, 14, dStart)
                Case "1,m"
                    dEnd = DateAdd(DateInterval.Month, 1, dStart)
                Case Else
                    isValid = False
            End Select
        End If

        If isValid Then
            Session("TimesheetStartDate") = dEnd
            Response.Redirect(Request.RawUrl)
        End If
    End Sub

    Protected Sub lbnCurrentBilling_Click(sender As Object, e As EventArgs) Handles lbnCurrentBilling.Click
        Dim isValid As Boolean
        Dim dStart As Date
        Dim dEnd As Date

        isValid = True
        If Not IsNothing(m_ProjectOwner.PaymentStartDate) Then
            dStart = m_ProjectOwner.PaymentStartDate
        Else
            IsValid = False
        End If

        If IsValid Then
            If DateDiff(DateInterval.Day, dStart, Now) < 0 Then
                IsValid = False
            End If
        End If

        If IsValid Then
            Select Case m_ProjectOwner.Frequency
                Case "1,w"
                    dEnd = DateAdd(DateInterval.Day, 7, dStart)
                    Do Until Now >= dStart And Now < dEnd
                        dStart = dEnd
                        dEnd = DateAdd(DateInterval.Day, 7, dStart)
                    Loop
                Case "2,w"
                    dEnd = DateAdd(DateInterval.Day, 14, dStart)
                    Do Until Now >= dStart And Now <= dEnd
                        dStart = dEnd
                        dEnd = DateAdd(DateInterval.Day, 14, dStart)
                    Loop
                Case "1,m"
                    dEnd = DateAdd(DateInterval.Month, 1, dStart)
                    Do Until Now >= dStart And Now <= dEnd
                        dStart = dEnd
                        dEnd = DateAdd(DateInterval.Month, 1, dStart)
                    Loop
                Case Else
                    IsValid = False
            End Select
        End If

        If isValid Then
            Session("TimesheetStartDate") = dStart
            Response.Redirect(Request.RawUrl)
        End If
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Dim isValid As Boolean
        Dim dStart As Date
        Dim dEnd As Date

        isValid = True
        If Not IsNothing(Session("TimesheetStartDate")) Then
            dStart = Session("TimesheetStartDate")
        Else
            isValid = False
        End If

        If isValid Then
            If DateDiff(DateInterval.Day, dStart, Now) < 0 Then
                isValid = False
            End If
        End If

        If isValid Then
            Select Case m_ProjectOwner.Frequency
                Case "1,w"
                    dEnd = DateAdd(DateInterval.Day, 7, dStart)
                Case "2,w"
                    dEnd = DateAdd(DateInterval.Day, 14, dStart)
                Case "1,m"
                    dEnd = DateAdd(DateInterval.Month, 1, dStart)
                Case Else
                    isValid = False
            End Select
        End If

        If isValid Then
            If btnClose.Text = "Open Time Sheets" Then
                m_ManagementService.OpenTimesheetEntryPartyAEntryDateRange(m_LoginUser.CompanyId, dStart, DateAdd(DateInterval.Day, -1, dEnd))
                btnClose.OnClientClick = "if (confirm('Closing off time sheets for current period. OK to continue.')) {return true;} else {return false;}"
                btnClose.Text = "Close Time Sheets"
            Else
                m_ManagementService.ProcessTimesheetEntryPartyAEntryDateRange(m_LoginUser.CompanyId, dStart, DateAdd(DateInterval.Day, -1, dEnd))
                btnClose.OnClientClick = "if (confirm('Opening time sheets for current period. OK to continue.')) {return true;} else {return false;}"
                btnClose.Text = "Open Time Sheets"
            End If
        End If
    End Sub
End Class
