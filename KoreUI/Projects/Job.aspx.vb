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
Imports System.Web.UI.HtmlControls
Imports System.Globalization

Partial Class Projects_Job
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Private m_ProjectID As Integer = 0
    Private m_CurrentJobID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Request.QueryString("ID") <> String.Empty Then
            Dim ActString As String = m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey)
            Dim ActArray As Array = ActString.Split("-")
            If ActArray.Length > 1 Then
                Integer.TryParse(ActArray(0), m_ProjectID)
                Integer.TryParse(ActArray(1), m_CurrentJobID)
            Else
                Integer.TryParse(ActArray(0), m_ProjectID)
            End If
        End If

        If Not IsPostBack Then
            Dim index As Integer = 0
            Dim dsProjectJobSetting As DataSet = New DataSet()
            dsProjectJobSetting = m_ManagementService.GetProjectJobsByProjectIdUserId(0, m_LoginUser.CompanyId)
            Dim unSelectItem As ListItem
            unSelectItem = New ListItem("--Select Job--", "-2")
            'unSelectItem.Attributes.Add("disabled", "disabled")
            ddlProjectJobSettings.Items.Add(unSelectItem)
            For index = 0 To dsProjectJobSetting.Tables(0).Rows.Count - 1
                ddlProjectJobSettings.Items.Add(New ListItem(dsProjectJobSetting.Tables(0).Rows(index)("Name"), dsProjectJobSetting.Tables(0).Rows(index)("JobValue")))
            Next
            ddlProjectJobSettings.Attributes.Add("onchange", String.Format("addJob('{0}','{1}','{2}','{3}');", ddlProjectJobSettings.ClientID, tbTitle.ClientID, divTitle.ClientID, divJobSetting.ClientID))

            unSelectItem = New ListItem("--Select Contact--", "-2")
            'unSelectItem.Attributes.Add("disabled", "disabled")
            ddlJobAssignee.Items.Add(unSelectItem)
            Dim dsStaff As DataSet = New DataSet()
            dsStaff = m_ManagementService.GetUserProfilesByPartyAType(m_LoginUser.UserId, UserType.Staff)
            If dsStaff.Tables(0).Rows.Count > 0 Then
                unSelectItem = New ListItem("Staff", "-1")
                unSelectItem.Attributes.Add("disabled", "disabled")
                ddlJobAssignee.Items.Add(unSelectItem)
                unSelectItem = New ListItem("------------------------", "-1")
                unSelectItem.Attributes.Add("disabled", "disabled")
                ddlJobAssignee.Items.Add(unSelectItem)
                For index = 0 To dsStaff.Tables(0).Rows.Count - 1
                    ddlJobAssignee.Items.Add(New ListItem(dsStaff.Tables(0).Rows(index)("FirstName"), m_Cryption.Encrypt(dsStaff.Tables(0).Rows(index)("UserID"), m_Cryption.cryptionKey)))
                Next
            End If
            Dim dsContractor As DataSet = New DataSet()
            dsContractor = m_ManagementService.GetUserProfilesByPartyAType(m_LoginUser.UserId, UserType.Contractor)
            If dsContractor.Tables(0).Rows.Count > 0 Then
                If ddlJobAssignee.Items.Count > 1 Then
                    unSelectItem = New ListItem("", "-1")
                    unSelectItem.Attributes.Add("disabled", "disabled")
                    ddlJobAssignee.Items.Add(unSelectItem)
                End If
                unSelectItem = New ListItem("Contractor", "-1")
                unSelectItem.Attributes.Add("disabled", "disabled")
                ddlJobAssignee.Items.Add(unSelectItem)
                unSelectItem = New ListItem("------------------------", "-1")
                unSelectItem.Attributes.Add("disabled", "disabled")
                ddlJobAssignee.Items.Add(unSelectItem)
                For index = 0 To dsContractor.Tables(0).Rows.Count - 1
                    ddlJobAssignee.Items.Add(New ListItem(dsContractor.Tables(0).Rows(index)("FirstName"), m_Cryption.Encrypt(dsContractor.Tables(0).Rows(index)("UserID"), m_Cryption.cryptionKey)))
                Next
            End If
            Dim dsSupplier As DataSet = New DataSet()
            dsSupplier = m_ManagementService.GetUserProfilesByPartyAType(m_LoginUser.UserId, UserType.Supplier)
            If dsSupplier.Tables(0).Rows.Count > 0 Then
                If ddlJobAssignee.Items.Count > 1 Then
                    unSelectItem = New ListItem("", "-1")
                    unSelectItem.Attributes.Add("disabled", "disabled")
                    ddlJobAssignee.Items.Add(unSelectItem)
                End If
                unSelectItem = New ListItem("Supplier", "-1")
                unSelectItem.Attributes.Add("disabled", "disabled")
                ddlJobAssignee.Items.Add(unSelectItem)
                unSelectItem = New ListItem("------------------------", "-1")
                unSelectItem.Attributes.Add("disabled", "disabled")
                ddlJobAssignee.Items.Add(unSelectItem)
                For index = 0 To dsSupplier.Tables(0).Rows.Count - 1
                    ddlJobAssignee.Items.Add(New ListItem(dsSupplier.Tables(0).Rows(index)("FirstName"), m_Cryption.Encrypt(dsSupplier.Tables(0).Rows(index)("UserID"), m_Cryption.cryptionKey)))
                Next
            End If
            ddlJobAssignee.Attributes.Add("onchange", String.Format("addUser('{0}','{1}');", ddlJobAssignee.ClientID, lblJobAssignee.ClientID))

            Dim intJobStatus As Integer
            For Each intJobStatus In [Enum].GetValues(GetType(JobStatus))
                ddlJobStatus.Items.Add(New ListItem(Replace([Enum].GetName(GetType(JobStatus), intJobStatus), "_", " "), intJobStatus))
            Next intJobStatus

            If m_CurrentJobID > 0 Then
                btnUpdate.Visible = True
                btnAdd.Visible = False
                btnDelete.Visible = True
                Dim objJob As New Job
                objJob = m_ManagementService.GetJobByJobId(m_CurrentJobID)
                If Not objJob.Description Is Nothing Then
                    tbxMessage.Text = objJob.Description
                End If
                If Not objJob.JobName Is Nothing Then
                    tbTitle.Text = objJob.JobName
                    'tbTitle.Attributes.Add("readonly", "")
                    For Each objItem As ListItem In ddlProjectJobSettings.Items
                        If objItem.Text = objJob.JobName Then
                            objItem.Selected = True
                            Dim userDiv As HtmlGenericControl = New HtmlGenericControl("div")
                            userDiv.ID = String.Format("JobSetting{0}", objItem.Value)
                            userDiv.InnerHtml = String.Format("{0} <input type='hidden' name='JobTitle' value='{0}' /><input type='hidden' name='JobValue' value='{1}' /><a onclick=""removeJob('{2}','{3}','{4}','{5}');"" title='Remove'>X</a>", objItem.Text, objItem.Value, ddlProjectJobSettings.ClientID, tbTitle.ClientID, divTitle.ClientID, divJobSetting.ClientID)
                            divTitle.Controls.Add(userDiv)
                            divJobSetting.Attributes.Add("style", "display:none;")
                            Exit For
                        End If
                    Next
                    'ddlProjectJobSettings.Attributes.Add("style", "display:none;")
                End If
                For index = 0 To objJob.JobAssignments.Count - 1
                    Dim objJobAssignment As JobAssignment = objJob.JobAssignments.Item(index)
                    Dim objUserProfile As UserProfile = objJobAssignment.UserProfile
                    Dim userDiv As HtmlGenericControl = New HtmlGenericControl("div")
                    userDiv.ID = m_Cryption.Encrypt(objUserProfile.UserProfileId, m_Cryption.cryptionKey)
                    userDiv.InnerHtml = String.Format("{0} <input type='hidden' name='UserNames' value='{0}' /><input type='hidden' name='UserIDs' value='{1}' /><a onclick=""removeUser('{1}','{2}');"" title='Remove'>X</a>", objUserProfile.FirstName, m_Cryption.Encrypt(objUserProfile.UserProfileId, m_Cryption.cryptionKey), lblJobAssignee.ClientID)
                    lblJobAssignee.Controls.Add(userDiv)

                    'If index = 0 Then
                    '    lblJobAssigneeName.Text = String.Format("<div id='{0}' name='{0}'>{1} <input type='hidden' name='UserNames' value='{1}' /><input type='hidden' name='UserIDs' value='{0}' /><a onclick=""removeUser('{0}','{2}');"" title='Remove'>X</a></div>", m_Cryption.Encrypt(objUserProfile.UserProfileId, m_Cryption.cryptionKey), objUserProfile.FirstName)
                    'Else
                    '    lblJobAssigneeName.Text = String.Format("{0}<div id='{1}' name='{1}'>{2} <input type='hidden' name='UserNames' value='{2}' /><input type='hidden' name='UserIDs' value='{1}' /><a onclick=""removeUser('{1}','{3}');"" title='Remove'>X</a></div>", lblJobAssigneeName.Text, m_Cryption.Encrypt(objUserProfile.UserProfileId, m_Cryption.cryptionKey), objUserProfile.FirstName)
                    'End If
                Next
                If objJob.DueDate <> Nothing Then
                    tbDueDate.Text = String.Format("{0:MM/dd/yyyy}", objJob.DueDate)
                End If
                ddlJobStatus.SelectedValue = objJob.Status

                lblJob.Text = "UPDATE JOB"
            Else
                tbDueDate.Text = String.Format("{0:MM/dd/yyyy}", Now)
                cbAddJobSetting.Checked = True
                btnUpdate.Visible = False
                btnAdd.Visible = True
                btnDelete.Visible = False
                lblJob.Text = "ADD JOB"
            End If
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If m_ProjectID > 0 Then
            Dim CurrentJob As New Job
            CurrentJob.JobName = tbTitle.Text.Trim
            CurrentJob.Description = tbxMessage.Text.Trim
            DateTime.TryParse(tbDueDate.Text, CurrentJob.DueDate)
            'If txtDueDateValid.Text.ToLower = "true" Then
            '    CurrentJob.DueDate = CType(String.Format("{0}-{1}-{2}", ddlDueDateYear.SelectedValue, ddlDueDateMonth.SelectedValue, ddlDueDateDay.SelectedValue), DateTime)
            'End If
            If ddlProjectJobSettings.SelectedValue > 0 Then
                CurrentJob.JobValue = ddlProjectJobSettings.SelectedValue
            Else
                CurrentJob.JobValue = 0
                If cbAddJobSetting.Checked = True Then
                    Dim objUserProjectJobSetting As New UserProjectJobSetting
                    Dim intProjectJobSettingID As Integer = 0
                    objUserProjectJobSetting.UserId = m_LoginUser.CompanyId
                    objUserProjectJobSetting.ProjectId = 0
                    objUserProjectJobSetting.Name = tbTitle.Text.Trim
                    Dim intDisplayOrder As Integer = 0
                    objUserProjectJobSetting.DisplayOrder = intDisplayOrder
                    intProjectJobSettingID = m_ManagementService.CreateUserProjectJobSetting(objUserProjectJobSetting)
                    If intProjectJobSettingID > 0 Then
                        objUserProjectJobSetting = New UserProjectJobSetting
                        objUserProjectJobSetting = m_ManagementService.GetUserProjectJobSettingByUserProjectJobSettingId(intProjectJobSettingID)
                        CurrentJob.JobValue = objUserProjectJobSetting.JobValue
                    End If
                End If
            End If
            CurrentJob.ProjectId = m_ProjectID
            CurrentJob.ProjectOwnerId = m_LoginUser.CompanyId
            CurrentJob.Status = ddlJobStatus.SelectedValue
            m_CurrentJobID = m_ManagementService.CreateJob(CurrentJob)
            m_ManagementService.UpdateJobStatus(m_CurrentJobID, CurrentJob.Status)

            If Request("UserIDs") <> "" Then
                If Request("UserIDs").IndexOf(",") > 0 Then
                    Dim arrUserID As Array = Split(Request("UserIDs"), ",")
                    For Each strUserID As String In arrUserID
                        If IsNumeric(m_Cryption.Decrypt(strUserID, m_Cryption.cryptionKey)) Then
                            Dim objJobAssignment As New JobAssignment
                            objJobAssignment.JobId = m_CurrentJobID
                            objJobAssignment.ProjectId = m_ProjectID
                            objJobAssignment.ProjectOwnerId = m_LoginUser.CompanyId
                            objJobAssignment.UserId = m_Cryption.Decrypt(strUserID, m_Cryption.cryptionKey)
                            m_ManagementService.CreateJobAssignment(objJobAssignment)
                        End If
                    Next
                Else
                    If IsNumeric(m_Cryption.Decrypt(Request("UserIDs"), m_Cryption.cryptionKey)) Then
                        Dim objJobAssignment As New JobAssignment
                        objJobAssignment.JobId = m_CurrentJobID
                        objJobAssignment.ProjectId = m_ProjectID
                        objJobAssignment.ProjectOwnerId = m_LoginUser.CompanyId
                        objJobAssignment.UserId = m_Cryption.Decrypt(Request("UserIDs"), m_Cryption.cryptionKey)
                        m_ManagementService.CreateJobAssignment(objJobAssignment)
                    End If
                End If
            End If

            Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The job is added successfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
        Else
            Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The job is added unsuccessfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If m_CurrentJobID > 0 And m_ProjectID > 0 Then
            Dim CurrentJob As New Job
            CurrentJob.JobName = tbTitle.Text.Trim
            CurrentJob.JobId = m_CurrentJobID
            CurrentJob.Description = tbxMessage.Text.Trim
            CurrentJob.DueDate = DateTime.ParseExact(tbDueDate.Text.ToString, "MM/dd/yyyy", Nothing)
            If ddlProjectJobSettings.SelectedValue > 0 Then
                CurrentJob.JobValue = ddlProjectJobSettings.SelectedValue
            Else
                CurrentJob.JobValue = 0
                If cbAddJobSetting.Checked = True Then
                    Dim objUserProjectJobSetting As New UserProjectJobSetting
                    Dim intProjectJobSettingID As Integer = 0
                    objUserProjectJobSetting.UserId = m_LoginUser.CompanyId
                    objUserProjectJobSetting.ProjectId = 0
                    objUserProjectJobSetting.Name = tbTitle.Text.Trim
                    Dim intDisplayOrder As Integer = 0
                    objUserProjectJobSetting.DisplayOrder = intDisplayOrder
                    intProjectJobSettingID = m_ManagementService.CreateUserProjectJobSetting(objUserProjectJobSetting)
                    If intProjectJobSettingID > 0 Then
                        objUserProjectJobSetting = New UserProjectJobSetting
                        objUserProjectJobSetting = m_ManagementService.GetUserProjectJobSettingByUserProjectJobSettingId(intProjectJobSettingID)
                        CurrentJob.JobValue = objUserProjectJobSetting.JobValue
                    End If
                End If
            End If
            CurrentJob.ProjectId = m_ProjectID
            CurrentJob.ProjectOwnerId = m_LoginUser.CompanyId
            CurrentJob.Status = ddlJobStatus.SelectedValue
            m_ManagementService.UpdateJob(CurrentJob)
            m_ManagementService.UpdateJobStatus(m_CurrentJobID, CurrentJob.Status)

            If Request("UserIDs") <> "" Then
                CurrentJob = m_ManagementService.GetJobByJobId(m_CurrentJobID)
                For index = 0 To CurrentJob.JobAssignments.Count - 1
                    Dim objJobAssignment As JobAssignment = CurrentJob.JobAssignments.Item(index)
                    Dim objUserProfile As UserProfile = objJobAssignment.UserProfile
                    m_ManagementService.DeleteJobAssignmentByJobIdUserId(m_CurrentJobID, objUserProfile.UserProfileId)
                Next
                If Request("UserIDs").IndexOf(",") > 0 Then
                    Dim arrUserID As Array = Split(Request("UserIDs"), ",")
                    For Each strUserID As String In arrUserID
                        If IsNumeric(m_Cryption.Decrypt(strUserID, m_Cryption.cryptionKey)) Then
                            Dim intUserID As Integer = m_Cryption.Decrypt(strUserID, m_Cryption.cryptionKey)
                            Dim objJobAssignment As New JobAssignment
                            objJobAssignment.JobId = m_CurrentJobID
                            objJobAssignment.ProjectId = m_ProjectID
                            objJobAssignment.ProjectOwnerId = m_LoginUser.CompanyId
                            objJobAssignment.UserId = intUserID
                            m_ManagementService.CreateJobAssignment(objJobAssignment)

                            'Dim objJobAssignment As New JobAssignment
                            'objJobAssignment.JobId = m_CurrentJobID
                            'objJobAssignment.ProjectId = m_ProjectID
                            'objJobAssignment.ProjectOwnerId = m_LoginUser.CompanyId
                            'objJobAssignment.UserId = m_Cryption.Decrypt(strUserID, m_Cryption.cryptionKey)
                            'm_ManagementService.CreateJobAssignment(objJobAssignment)
                        End If
                    Next
                Else
                    If IsNumeric(m_Cryption.Decrypt(Request("UserIDs"), m_Cryption.cryptionKey)) Then
                        Dim intUserID As Integer = m_Cryption.Decrypt(Request("UserIDs"), m_Cryption.cryptionKey)
                        Dim objJobAssignment As New JobAssignment
                        objJobAssignment.JobId = m_CurrentJobID
                        objJobAssignment.ProjectId = m_ProjectID
                        objJobAssignment.ProjectOwnerId = m_LoginUser.CompanyId
                        objJobAssignment.UserId = intUserID
                        m_ManagementService.CreateJobAssignment(objJobAssignment)
                    End If
                End If
            End If

            Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The job is updated successfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
        Else
            Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The job is updated unsuccessfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect(String.Format("Detail.aspx?ID={0}", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_CurrentJobID > 0 And m_ProjectID > 0 Then
            m_ManagementService.DeleteJobByJobId(m_CurrentJobID, False)

            Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The job is deleted successfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
        End If
    End Sub
End Class
