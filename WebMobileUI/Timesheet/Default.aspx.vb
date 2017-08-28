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

Partial Class Timesheet_Default
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_ProjectOwner As New ProjectOwner
    Private m_Cryption As New Cryption
    Private m_LastBillingEndDate As Date
    Private m_NextBillingEndDate As Date
    Private m_Timesheets As New DataSet

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        m_ProjectOwner = m_ManagementService.GetProjectOwnerByProjectOwnerId(m_LoginUser.CompanyId)

        Dim isValid As Boolean
        Dim dStart As Date
        Dim dEnd As Date

        isValid = False
        Dim rsLastBillingEnd As DataSet = m_ManagementService.GetTimesheetLastCycleEndDateByPartyAPartyB(m_LoginUser.CompanyId, m_LoginUser.UserId)
        If rsLastBillingEnd.Tables.Count > 0 Then
            If rsLastBillingEnd.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(rsLastBillingEnd.Tables(0).Rows(0)("LastCycleEndDate")) Then
                    m_LastBillingEndDate = DateAdd(DateInterval.Day, 1, rsLastBillingEnd.Tables(0).Rows(0)("LastCycleEndDate"))
                    isValid = True
                End If
            End If
        End If
        If Not isValid Then
            If Not IsNothing(m_ProjectOwner.PaymentStartDate) Then
                m_LastBillingEndDate = m_ProjectOwner.PaymentStartDate
                isValid = True
            End If
        End If

        If isValid Then
            If DateDiff(DateInterval.Day, m_LastBillingEndDate, Now) < 0 Then
                isValid = False
            End If
        End If

        If isValid Then
            Select Case m_ProjectOwner.Frequency
                Case "1,w"
                    m_NextBillingEndDate = DateAdd(DateInterval.Day, 7, m_LastBillingEndDate)
                    Do Until Now >= m_LastBillingEndDate And Now < m_NextBillingEndDate
                        m_LastBillingEndDate = m_NextBillingEndDate
                        m_NextBillingEndDate = DateAdd(DateInterval.Day, 7, m_LastBillingEndDate)
                    Loop
                Case "2,w"
                    m_NextBillingEndDate = DateAdd(DateInterval.Day, 14, m_LastBillingEndDate)
                    Do Until Now >= m_LastBillingEndDate And Now <= m_NextBillingEndDate
                        m_LastBillingEndDate = m_NextBillingEndDate
                        m_NextBillingEndDate = DateAdd(DateInterval.Day, 14, m_LastBillingEndDate)
                    Loop
                Case "1,m"
                    m_NextBillingEndDate = DateAdd(DateInterval.Month, 1, m_LastBillingEndDate)
                    Do Until Now >= m_LastBillingEndDate And Now <= m_NextBillingEndDate
                        m_LastBillingEndDate = m_NextBillingEndDate
                        m_NextBillingEndDate = DateAdd(DateInterval.Month, 1, m_LastBillingEndDate)
                    Loop
                Case Else
                    isValid = False
            End Select
        End If

        If isValid Then
            m_Timesheets = m_ManagementService.GetTimesheetEntryByPartyAPartyBEntryDateRange(m_LoginUser.CompanyId, m_LoginUser.UserId, m_LastBillingEndDate, DateAdd(DateInterval.Day, -1, m_NextBillingEndDate))
            rptTimeSheet.DataSource = m_Timesheets
            rptTimeSheet.DataBind()

            If m_Timesheets.Tables.Count > 0 Then
                If m_Timesheets.Tables(0).Rows.Count > 0 Then
                    Dim decTHour As Decimal
                    For Each tr As DataRow In m_Timesheets.Tables(0).Rows
                        If Not IsDBNull(tr("HRS")) Then
                            decTHour = decTHour + tr("HRS")
                        End If
                    Next
                    If decTHour > 0 Then
                        lblTHour.Text = String.Format("Total Hours: {0}", decTHour)
                    End If
                End If
            End If
        Else
            lblMsg.Text = "Oops! We are really sorry but this page was unable to display."
        End If

        If Request.QueryString("msg") <> "" Then
            lblMsg.Text = Request.QueryString("msg").ToString()
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
    End Sub

    Protected Sub SelectDate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim aryArgument As Array = Split(sender.CommandArgument, ",")
        Dim isValidID As Boolean
        Dim isValidDate As Boolean
        Dim intTimeSheetEntryID As Integer
        Dim dtTimeSheetEntryDate As Date
        isValidID = False
        isValidDate = False
        If aryArgument.Length = 2 Then
            If Not Integer.TryParse(aryArgument(0), intTimeSheetEntryID) Then
                If Date.TryParse(aryArgument(1), dtTimeSheetEntryDate) Then
                    isValidDate = True
                End If
            Else
                isValidID = True
            End If
        End If
        If isValidID Then
            Session("TimeSheetEntryID") = intTimeSheetEntryID
            Session("TimeSheetEntryDate") = Nothing
            Response.Redirect("CheckIn.aspx")
        End If
        If isValidDate Then
            Session("TimeSheetEntryID") = Nothing
            Session("TimeSheetEntryDate") = dtTimeSheetEntryDate
            Response.Redirect("CheckIn.aspx")
        End If
        lblMsg.Text = "Oops! We are really sorry but this page was unable to display."
    End Sub

    Protected Sub SelectDate_SimpleDDL_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim aryArgument As Array = Split(sender.CommandArgument, ",")
        Dim isValidID As Boolean
        Dim isValidDate As Boolean
        Dim intTimeSheetEntryID As Integer
        Dim dtTimeSheetEntryDate As Date
        isValidID = False
        isValidDate = False
        If aryArgument.Length = 2 Then
            If Not Integer.TryParse(aryArgument(0), intTimeSheetEntryID) Then
                If Date.TryParse(aryArgument(1), dtTimeSheetEntryDate) Then
                    isValidDate = True
                End If
            Else
                isValidID = True
            End If
        End If
        If isValidID Then
            Session("TimeSheetEntryID") = intTimeSheetEntryID
            Session("TimeSheetEntryDate") = Nothing
            Response.Redirect("CheckIn_SimpleDDL.aspx")
        End If
        If isValidDate Then
            Session("TimeSheetEntryID") = Nothing
            Session("TimeSheetEntryDate") = dtTimeSheetEntryDate
            Response.Redirect("CheckIn_SimpleDDL.aspx")
        End If
        lblMsg.Text = "Oops! We are really sorry but this page was unable to display."
    End Sub

    Protected Sub SelectDate_DDL_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim aryArgument As Array = Split(sender.CommandArgument, ",")
        Dim isValidID As Boolean
        Dim isValidDate As Boolean
        Dim intTimeSheetEntryID As Integer
        Dim dtTimeSheetEntryDate As Date
        isValidID = False
        isValidDate = False
        If aryArgument.Length = 2 Then
            If Not Integer.TryParse(aryArgument(0), intTimeSheetEntryID) Then
                If Date.TryParse(aryArgument(1), dtTimeSheetEntryDate) Then
                    isValidDate = True
                End If
            Else
                isValidID = True
            End If
        End If
        If isValidID Then
            Session("TimeSheetEntryID") = intTimeSheetEntryID
            Session("TimeSheetEntryDate") = Nothing
            Response.Redirect("CheckIn_DDL.aspx")
        End If
        If isValidDate Then
            Session("TimeSheetEntryID") = Nothing
            Session("TimeSheetEntryDate") = dtTimeSheetEntryDate
            Response.Redirect("CheckIn_DDL.aspx")
        End If
        lblMsg.Text = "Oops! We are really sorry but this page was unable to display."
    End Sub

    Public Function showDailyHours(ByVal DailyHours As String) As String
        If DailyHours = String.Empty Then
            Return "0 hr"
        End If
        Return DailyHours & " hr"
    End Function
End Class
