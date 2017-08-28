Imports System
Imports System.Web
Imports System.Collections
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Namespace Warpfusion.A4PP.WebServices
    <WebService([Namespace]:="http://mycompany.org/"), _
    WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1), _
    System.Web.Script.Services.ScriptService()> _
    Public Class MobileService
        Inherits System.Web.Services.WebService

        Public Sub New()

        End Sub 'New 


        'Uncomment the following line if using designed components  
        'InitializeComponent(); 

        <WebMethod()> _
        Public Function Greetings() As String
            Dim serverTime As String = _
                String.Format("Current date and time: {0}.", DateTime.Now)
            Dim greet As String = "Hello World. <br/>" + serverTime
            Return greet

        End Function 'Greetings

        <WebMethod()> _
        Public Function AddTimeSheet(ByVal TimeSheetDate As String, ByVal UserID As String, ByVal CompanyID As String, ByVal WorkStartTime As String, ByVal LunchStartTime As String, ByVal LunchEndTime As String, ByVal WorkEndTime As String) As String
            Dim m_CompanyID As Integer = 0
            Dim m_UserID As Integer = 0
            Dim m_TimeSheetDate As Date
            Dim aryTimesheetDate As Array
            Dim m_WorkStartTime As DateTime
            Dim m_LunchStartTime As DateTime
            Dim m_LunchEndTime As DateTime
            Dim m_WorkEndTime As DateTime
            Dim m_Cryption As New Cryption

            Dim isValid As Boolean = True

            If CompanyID = "" Then
                isValid = False
            End If

            If isValid Then
                If Not Integer.TryParse(m_Cryption.Decrypt(CompanyID, m_Cryption.cryptionKey), m_CompanyID) Then
                    isValid = False
                End If
            End If

            If isValid Then
                If m_CompanyID <= 0 Then
                    isValid = False
                End If
            End If

            If isValid Then
                If UserID = "" Then
                    isValid = False
                End If
            End If

            If isValid Then
                If Not Integer.TryParse(m_Cryption.Decrypt(UserID, m_Cryption.cryptionKey), m_UserID) Then
                    isValid = False
                End If
            End If

            If isValid Then
                If m_UserID <= 0 Then
                    isValid = False
                End If
            End If

            If isValid Then
                If TimeSheetDate = "" Then
                    isValid = False
                End If
            End If

            If isValid Then
                If TimeSheetDate.IndexOf("/") <= 0 Then
                    isValid = False
                End If
            End If

            If isValid Then
                aryTimesheetDate = Split(TimeSheetDate, "/")
                If aryTimesheetDate.Length <> 3 Then
                    isValid = False
                End If
            End If

            If isValid Then
                If Not Date.TryParse(String.Format("{0}-{1}-{2}", aryTimesheetDate(2), aryTimesheetDate(1), aryTimesheetDate(0)), m_TimeSheetDate) Then
                    isValid = False
                End If
            End If

            If isValid Then
                If WorkStartTime = "" Then
                    isValid = False
                End If
            End If

            If isValid Then
                If Not DateTime.TryParse(m_TimeSheetDate.ToString("yyyy/MM/dd") & " " & WorkStartTime, m_WorkStartTime) Then
                    isValid = False
                End If
            End If

            'If isValid Then
            '    If LunchStartTime = "" Then
            '        isValid = False
            '    End If
            'End If

            If isValid Then
                If LunchStartTime <> "" Then
                    If Not DateTime.TryParse(m_TimeSheetDate.ToString("yyyy/MM/dd") & " " & LunchStartTime, m_LunchStartTime) Then
                        isValid = False
                    End If
                End If
            End If

            'If isValid Then
            '    If LunchEndTime = "" Then
            '        isValid = False
            '    End If
            'End If

            If isValid Then
                If LunchEndTime <> "" Then
                    If Not DateTime.TryParse(m_TimeSheetDate.ToString("yyyy/MM/dd") & " " & LunchEndTime, m_LunchEndTime) Then
                        isValid = False
                    End If
                End If
            End If

            If isValid Then
                If WorkEndTime = "" Then
                    isValid = False
                End If
            End If

            If isValid Then
                If Not DateTime.TryParse(m_TimeSheetDate.ToString("yyyy/MM/dd") & " " & WorkEndTime, m_WorkEndTime) Then
                    isValid = False
                End If
            End If

            If isValid Then
                Dim m_ManagementService As New ManagementService
                m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
                Dim m_Timesheet As New TimesheetEntry
                m_Timesheet.PartyA = m_CompanyID
                m_Timesheet.PartyB = m_UserID
                m_Timesheet.WorkStart = m_WorkStartTime
                m_Timesheet.WorkEnd = m_WorkEndTime
                If LunchStartTime <> "" Then
                    m_Timesheet.LunchStart = m_LunchStartTime
                Else
                    m_Timesheet.LunchStart = Nothing
                End If
                If LunchEndTime <> "" Then
                    m_Timesheet.LunchEnd = m_LunchEndTime
                Else
                    m_Timesheet.LunchEnd = Nothing
                End If
                m_Timesheet.EntryDate = m_TimeSheetDate
                m_ManagementService.SaveTimesheetEntry(m_Timesheet)
                Return "Timesheet is updated!"
            End If

            Return "Please check if you enter the timesheet correctly!"
        End Function
    End Class

End Namespace
