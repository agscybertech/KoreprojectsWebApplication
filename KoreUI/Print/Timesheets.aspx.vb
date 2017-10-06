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

Partial Class Print_Timesheets
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_ProjectOwner As New ProjectOwner
    Public m_Cryption As New Cryption()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
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

        If isValid Then
            Dim iUser As Integer
            If Request("id") <> "" Then
                Integer.TryParse(m_Cryption.Decrypt(Request("id"), m_Cryption.cryptionKey), iUser)
            End If

            Dim strFinal As String = ""

            Dim dsTimesheets As DataSet = m_ManagementService.GetTimesheetEntryDetailsByPartyAPartyBEntryDateRange(m_LoginUser.CompanyId, iUser, dStart, DateAdd(DateInterval.Day, -1, dEnd))
            If dsTimesheets.Tables.Count > 0 Then
                For i As Integer = 0 To dsTimesheets.Tables.Count - 1
                    If dsTimesheets.Tables(i).Rows.Count > 0 Then
                        Dim sTable As String = "<table align='center' width='90%' class='table table-bordered'><thead><tr><th style='width:40px'>Sr. No</th><th style='width:20%'>Project Name</th><th style='width:20%'>Job</th><th style='width:100px'>Date</th><th style='width:70px'>Total hours</th><th>Description</th>" & If(dsTimesheets.Tables(i).Select("ProjectOwnerUserId=" & m_LoginUser.UserId).Length > 0, "<th class=""editaction"">Action</th>", "") & "</tr></thead><tbody>"
                        Dim sName As String = String.Format("{0} {1}", dsTimesheets.Tables(i).Rows(0)("FirstName"), dsTimesheets.Tables(i).Rows(0)("LastName"))
                        Dim sTime As Decimal = 0
                        Dim sRounded As Decimal = 0
                        sTable = String.Format("{0}<tr><th class=""usertitle"" align='left'  colspan='" & If(dsTimesheets.Tables(i).Select("ProjectOwnerUserId=" & m_LoginUser.UserId).Length > 0, 7, 6) & "'><b>{1}</b></th</tr>", sTable, sName)
                        Dim TotalHours As Double = 0
                        For Each rData As DataRow In dsTimesheets.Tables(i).Rows
                            sTable = sTable & "<tr><td>" & rData("ID") & "</td><td>" & rData("ProjectName") & "</td><td>" & rData("Job") & "</td><td>" & CDate(rData("EntryDate")).ToString("d MMM yyyy") & "</td><td>" & rData("TotalHours") & "</td><td style='text-align:left'>" & Convert.ToString(rData("Description")).Replace(vbCrLf, "<br/>").Replace(vbLf, "<br/>").Replace(vbCr, "<br/>").Replace(vbCrLf, "<br />") & "</td>" & If(dsTimesheets.Tables(i).Select("ProjectOwnerUserId=" & m_LoginUser.UserId).Length > 0, "<td class=""editaction"">" & String.Format("<a class='form_popup' href='Timesheet.aspx?TimeSheetEntryID={0}'>EDIT</a>" & "</td>", rData("TimesheetEntryId")), "") & "</tr>"
                            TotalHours = TotalHours + Convert.ToDouble(rData("TotalHours"))
                        Next
                        sTable = String.Format("{0}</tbody><tfoot><tr><th align='left' colspan='4'><b>Totals for {1}</b></th><th><b>{2}</b></th><th class=""userfooter""></th>" & If(dsTimesheets.Tables(i).Select("ProjectOwnerUserId=" & m_LoginUser.UserId).Length > 0, "<th></th>", "") & "</tr></tfoot></table><br />", sTable, sName, TotalHours)
                        strFinal = strFinal & sTable
                    End If
                Next
            End If

            lblPrint.Text = strFinal
        End If
    End Sub

    Function WorkingTimeConverstion(ByVal decMins As Decimal) As String
        'Dim timeElapsed As DateTime = New DateTime(1, 1, 1, 0, decMins, 0)
        'Return timeElapsed.ToString("HH:mm")
        Dim hours As Integer = decMins \ 60
        Dim minutes As Integer = decMins - (hours * 60)
        Dim sMin As String = minutes.ToString
        If sMin.Length = 1 Then
            sMin = String.Format("0{0}", sMin)
        End If
        Dim timeElapsed As String = CType(hours, String) & ":" & sMin
        Return timeElapsed
    End Function

    Function WorkingTimeRoundedConverstion(ByVal noQuarter As Decimal) As String
        'Dim timeElapsed As DateTime = New DateTime(1, 1, 1, 0, noQuarter * 15, 0)
        'Return timeElapsed.ToString("HH:mm")
        Dim hours As Integer = noQuarter * 15 \ 60
        Dim minutes As Integer = noQuarter * 15 - (hours * 60)
        Dim sMin As String = minutes.ToString
        If sMin.Length = 1 Then
            sMin = String.Format("0{0}", sMin)
        End If
        Dim timeElapsed As String = CType(hours, String) & ":" & sMin
        Return timeElapsed
    End Function

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
End Class
