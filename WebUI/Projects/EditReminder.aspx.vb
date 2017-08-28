Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports System.Configuration
Imports System.Globalization
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities
Imports System.Data
Imports System.Data.SqlClient

Partial Class Projects_EditReminder
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_ProjectOwner As New ProjectOwner
    Public m_Cryption As New Cryption()
    Private m_ReminderId As Long = 0
    Private m_jsString As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        m_ProjectOwner = m_ManagementService.GetProjectOwnerByProjectOwnerId(m_LoginUser.CompanyId)

        lblTitle.Text = "Write a Note"
        btnDelete.Visible = False

        If Not IsPostBack Then
            'btnDone.Visible = True
            'Hide Done for now
            btnDone.Visible = False
            lblDone.Visible = False
            chkSetReminder.Visible = True

            m_jsString = "javascript:ShowStartTime(false);"
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "HideStartTime", m_jsString, True)

            Dim index As Integer
            ddlStartDateDay.Items.Add(New ListItem("Day", "0"))
            ddlFinishDateDay.Items.Add(New ListItem("Day", "0"))
            For index = 1 To 31
                If index < 10 Then
                    ddlStartDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("0{0}", index)))
                    ddlFinishDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("0{0}", index)))
                Else
                    ddlStartDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                    ddlFinishDateDay.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                End If
            Next

            ddlStartDateMonth.Items.Add(New ListItem("Month", "0"))
            ddlFinishDateMonth.Items.Add(New ListItem("Month", "0"))
            For index = 1 To 12
                If index < 10 Then
                    ddlStartDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("0{0}", index)))
                    ddlFinishDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("0{0}", index)))
                Else
                    ddlStartDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("{0}", index)))
                    ddlFinishDateMonth.Items.Add(New ListItem(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(index), String.Format("{0}", index)))
                End If
            Next

            ddlStartDateYear.Items.Add(New ListItem("Year", "0"))
            ddlFinishDateYear.Items.Add(New ListItem("Year", "0"))
            Dim intYear As Integer
            For index = -1 To 5
                intYear = Year(Now) + index
                ddlStartDateYear.Items.Add(New ListItem(String.Format("{0}", intYear), String.Format("{0}", intYear)))
                ddlFinishDateYear.Items.Add(New ListItem(String.Format("{0}", intYear), String.Format("{0}", intYear)))
            Next

            ddlStartDateHour.Items.Add(New ListItem("hh", "0"))
            ddlFinishDateHour.Items.Add(New ListItem("hh", "0"))
            For index = 0 To 23
                If index < 10 Then
                    ddlStartDateHour.Items.Add(New ListItem(String.Format("0{0}", index), String.Format("0{0}", index)))
                    ddlFinishDateHour.Items.Add(New ListItem(String.Format("0{0}", index), String.Format("0{0}", index)))
                Else
                    ddlStartDateHour.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                    ddlFinishDateHour.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                End If
            Next

            ddlStartDateMinute.Items.Add(New ListItem("mm", "0"))
            ddlFinishDateMinute.Items.Add(New ListItem("mm", "0"))
            For index = 0 To 59
                If index < 10 Then
                    ddlStartDateMinute.Items.Add(New ListItem(String.Format("0{0}", index), String.Format("0{0}", index)))
                    ddlFinishDateMinute.Items.Add(New ListItem(String.Format("0{0}", index), String.Format("0{0}", index)))
                Else
                    ddlStartDateMinute.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                    ddlFinishDateMinute.Items.Add(New ListItem(String.Format("{0}", index), String.Format("{0}", index)))
                End If
            Next
        End If

        If Request.QueryString("ID") <> String.Empty Then
            Long.TryParse(m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey), m_ReminderId)
            If Not IsPostBack Then
                If m_ReminderId > 0 Then
                    lblTitle.Text = "Update Note"
                    btnSave.Text = "Update"
                    btnDelete.Visible = True
                    LoadReminder()
                Else
                    btnDone.Visible = False
                End If
            End If
        Else
            If Not IsPostBack Then
                ddlStartDateDay.SelectedValue = String.Format("{0:dd}", Today)
                ddlStartDateMonth.SelectedValue = String.Format("{0:MM}", Today)
                ddlStartDateYear.SelectedValue = String.Format("{0:yyyy}", Today)
                btnDone.Visible = False
            End If
        End If
        txtReminderTitle.Focus()
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect("CalendarView.aspx")
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        m_ManagementService.DeleteReminderByReminderId(m_ReminderId)
        Response.Redirect("CalendarView.aspx")
    End Sub

    Private Sub LoadReminder()
        Dim objReminder As Reminder
        objReminder = m_ManagementService.GetReminderByReminderId(m_ReminderId)
        txtDisplayOrder.Text = objReminder.DisplayOrder
        'txtReminderTitle.Text = objReminder.ReminderTitle
        txtReminderTitle.Text = objReminder.ReminderContentData
        txtText.Text = objReminder.ReminderContentData
        ddlStatus.SelectedValue = objReminder.Status
        ddlEmailTimeSetting.SelectedValue = objReminder.EmailTimeSetting

        If Not objReminder.StartDate = Nothing Then
            ddlStartDateDay.SelectedValue = objReminder.StartDate.ToString("dd")
            ddlStartDateMonth.SelectedValue = objReminder.StartDate.ToString("MM")
            ddlStartDateYear.SelectedValue = objReminder.StartDate.ToString("yyyy")
            ddlStartDateHour.SelectedValue = objReminder.StartDate.ToString("HH")
            ddlStartDateMinute.SelectedValue = objReminder.StartDate.ToString("ss")
        Else
            ddlStartDateDay.SelectedValue = String.Format("{0:dd}", Today)
            ddlStartDateMonth.SelectedValue = String.Format("{0:MM}", Today)
            ddlStartDateYear.SelectedValue = String.Format("{0:yyyy}", Today)
        End If

        If Not objReminder.EndDate = Nothing Then
            ddlFinishDateDay.SelectedValue = objReminder.EndDate.ToString("dd")
            ddlFinishDateMonth.SelectedValue = objReminder.EndDate.ToString("MM")
            ddlFinishDateYear.SelectedValue = objReminder.EndDate.ToString("yyyy")
            ddlFinishDateHour.SelectedValue = objReminder.EndDate.ToString("HH")
            ddlFinishDateMinute.SelectedValue = objReminder.EndDate.ToString("ss")
        End If

        If objReminder.Status = 3 Then
            ' Status is Close
            chkSetReminder.Visible = False
            btnDone.Visible = False
            'lblDone.Visible = True
            'Hide Done for now
            lblDone.Visible = False
        Else
            If objReminder.Status = 2 Then
                ' Status is Reminder
                chkSetReminder.Checked = True
                m_jsString = "javascript:ShowStartTime(true);"
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShowStartTime", m_jsString, True)
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        SaveReminder()
    End Sub

    Protected Sub ddlStartDateDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStartDateDay.SelectedIndexChanged
        ValidateStartDate()
    End Sub

    Protected Sub ddlStartDateMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStartDateMonth.SelectedIndexChanged
        ValidateStartDate()
    End Sub

    Protected Sub ddlStartDateYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStartDateYear.SelectedIndexChanged
        ValidateStartDate()
    End Sub

    Protected Sub ddlFinishDateDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFinishDateDay.SelectedIndexChanged
        ValidateFinishDate()
    End Sub

    Protected Sub ddlFinishDateMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFinishDateMonth.SelectedIndexChanged
        ValidateFinishDate()
    End Sub

    Protected Sub ddlFinishDateYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFinishDateYear.SelectedIndexChanged
        ValidateFinishDate()
    End Sub

    Private Sub ValidateStartDate()
        Dim dtStartDate As DateTime
        Dim bStartDateValid As Boolean
        bStartDateValid = DateTime.TryParse(String.Format("{0}-{1}-{2}", ddlStartDateYear.SelectedValue, ddlStartDateMonth.SelectedValue, ddlStartDateDay.SelectedValue), dtStartDate)
        txtStartDateValid.Text = bStartDateValid.ToString()
    End Sub

    Private Sub ValidateFinishDate()
        Dim dtFinishDate As DateTime
        Dim bFinishDateValid As Boolean
        bFinishDateValid = DateTime.TryParse(String.Format("{0}-{1}-{2}", ddlFinishDateYear.SelectedValue, ddlFinishDateMonth.SelectedValue, ddlFinishDateDay.SelectedValue), dtFinishDate)
        txtFinishDateValid.Text = bFinishDateValid.ToString()
    End Sub

    Protected Sub btnDone_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDone.Click
        'Set Status to Close
        ddlStatus.SelectedValue = "3"
        SaveReminder()
    End Sub

    Private Sub SaveReminder()
        Dim objReminder As Reminder
        If m_ReminderId > 0 Then
            ' Update            
            objReminder = m_ManagementService.GetReminderByReminderId(m_ReminderId)
            'objReminder.ReminderTitle = txtReminderTitle.Text
            If txtReminderTitle.Text.Trim().Length > 50 Then
                objReminder.ReminderTitle = String.Format("{0}...", txtReminderTitle.Text.Trim().Substring(0, 47))
            Else
                objReminder.ReminderTitle = txtReminderTitle.Text
            End If
            If txtDisplayOrder.Text.Trim() <> String.Empty Then
                objReminder.DisplayOrder = txtDisplayOrder.Text
            End If
            objReminder.Status = ddlStatus.SelectedValue
            objReminder.EmailTimeSetting = ddlEmailTimeSetting.SelectedValue
            'objReminder.ReminderContentData = txtText.Text.Trim()
            objReminder.ReminderContentData = txtReminderTitle.Text.Trim()

            If ddlStartDateDay.SelectedIndex < 1 And ddlStartDateMonth.SelectedIndex < 1 And ddlStartDateYear.SelectedValue < 1 Then
                objReminder.StartDate = Nothing
            Else
                objReminder.StartDate = CType(String.Format("{0}-{1}-{2} {3}:{4}:00", ddlStartDateYear.SelectedValue, ddlStartDateMonth.SelectedValue, ddlStartDateDay.SelectedValue, ddlStartDateHour.SelectedValue, ddlStartDateMinute.SelectedValue), DateTime)
            End If

            If ddlFinishDateDay.SelectedIndex < 1 And ddlFinishDateMonth.SelectedIndex < 1 And ddlFinishDateYear.SelectedValue < 1 Then
                objReminder.EndDate = Nothing
            Else
                objReminder.EndDate = CType(String.Format("{0}-{1}-{2} {3}:{4}:00", ddlFinishDateYear.SelectedValue, ddlFinishDateMonth.SelectedValue, ddlFinishDateDay.SelectedValue, ddlFinishDateHour.SelectedValue, ddlFinishDateMinute.SelectedValue), DateTime)
            End If

            If objReminder.StartDate <> Nothing And chkSetReminder.Checked Then
                'Set Status to Reminder
                objReminder.Status = 2
            Else
                If Not chkSetReminder.Checked Then
                    objReminder.Status = 1
                    objReminder.StartDate = Nothing
                    objReminder.EmailTimeSetting = 0
                End If
            End If

            m_ManagementService.UpdateReminder(objReminder)
            Response.Redirect(String.Format("CalendarView.aspx?{0}&msg=The content is updated successfully.", Request.QueryString), True)
        Else
            objReminder = New Reminder()
            objReminder.ProjectOwnerId = m_LoginUser.CompanyId
            'objReminder.ReminderTitle = StrConv(txtReminderTitle.Text.Trim(), VbStrConv.ProperCase)            
            If txtReminderTitle.Text.Trim().Length > 50 Then
                objReminder.ReminderTitle = StrConv(String.Format("{0}...", txtReminderTitle.Text.Trim().Substring(0, 47)), VbStrConv.ProperCase)
            Else
                objReminder.ReminderTitle = StrConv(txtReminderTitle.Text.Trim(), VbStrConv.ProperCase)
            End If
            If txtDisplayOrder.Text.Trim() <> String.Empty Then
                objReminder.DisplayOrder = txtDisplayOrder.Text
            End If

            objReminder.Status = ddlStatus.SelectedValue
            objReminder.EmailTimeSetting = ddlEmailTimeSetting.SelectedValue
            'objReminder.ReminderContentData = txtText.Text.Trim()
            objReminder.ReminderContentData = txtReminderTitle.Text.Trim()

            If ddlStartDateDay.SelectedIndex < 1 And ddlStartDateMonth.SelectedIndex < 1 And ddlStartDateYear.SelectedValue < 1 Then
                objReminder.StartDate = Nothing
            Else
                objReminder.StartDate = CType(String.Format("{0}-{1}-{2} {3}:{4}:00", ddlStartDateYear.SelectedValue, ddlStartDateMonth.SelectedValue, ddlStartDateDay.SelectedValue, ddlStartDateHour.SelectedValue, ddlStartDateMinute.SelectedValue), DateTime)
            End If

            If ddlFinishDateDay.SelectedIndex < 1 And ddlFinishDateMonth.SelectedIndex < 1 And ddlFinishDateYear.SelectedValue < 1 Then
                objReminder.EndDate = Nothing
            Else
                objReminder.EndDate = CType(String.Format("{0}-{1}-{2} {3}:{4}:00", ddlFinishDateYear.SelectedValue, ddlFinishDateMonth.SelectedValue, ddlFinishDateDay.SelectedValue, ddlFinishDateHour.SelectedValue, ddlFinishDateMinute.SelectedValue), DateTime)
            End If

            If objReminder.StartDate <> Nothing And chkSetReminder.Checked Then
                'Set Status to Reminder
                objReminder.Status = 2
            Else
                If Not chkSetReminder.Checked Then
                    objReminder.Status = 1
                    objReminder.StartDate = Nothing
                    objReminder.EmailTimeSetting = 0
                End If
            End If

            m_ManagementService.CreateReminder(objReminder)
            Response.Redirect(String.Format("CalendarView.aspx?{0}&msg=The content is saved successfully.", Request.QueryString), True)
        End If
    End Sub
End Class