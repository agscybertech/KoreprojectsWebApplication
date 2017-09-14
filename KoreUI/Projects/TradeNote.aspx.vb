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

Partial Class Projects_TradeNote
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()
    Private m_ProjectID As Integer = 0
    Private m_CurrentNoteID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString

        If Request.QueryString("ID") <> String.Empty Then
            Dim ActString As String = m_Cryption.Decrypt(Request.QueryString("ID"), m_Cryption.cryptionKey)
            Dim ActArray As Array = ActString.Split("-")
            If ActArray.Length > 1 Then
                Integer.TryParse(ActArray(0), m_ProjectID)
                Integer.TryParse(ActArray(1), m_CurrentNoteID)
            Else
                Integer.TryParse(ActArray(0), m_ProjectID)
            End If
        End If

        If Not IsPostBack Then
            Dim index As Integer = 0
            Dim dsProjectStatus As DataSet = New DataSet()
            'dsProjectStatus = m_ManagementService.GetProjectStatuses()
            dsProjectStatus = m_ManagementService.GetProjectStatusesByProjectIdUserId(m_ProjectID, m_LoginUser.UserId)
            Dim unSelectItem As ListItem
            unSelectItem = New ListItem("--Select Status--", "-2")
            unSelectItem.Attributes.Add("disabled", "disabled")
            ddlProjectStatus.Items.Add(unSelectItem)
            For index = 0 To dsProjectStatus.Tables(0).Rows.Count - 1
                'ddlProjectStatus.Items.Add(New ListItem(dsProjectStatus.Tables(0).Rows(index)("Name"), dsProjectStatus.Tables(0).Rows(index)("ProjectStatusId")))
                ddlProjectStatus.Items.Add(New ListItem(dsProjectStatus.Tables(0).Rows(index)("Name"), dsProjectStatus.Tables(0).Rows(index)("StatusValue")))
            Next
            ddlProjectStatus.Items.Add(New ListItem("General", "-1"))
            'ddlProjectStatus.DataSource = dsProjectStatus.Tables(0)
            'ddlProjectStatus.DataTextField = "Name"
            'ddlProjectStatus.DataValueField = "ProjectStatusId"
            'ddlProjectStatus.DataBind()

            If m_CurrentNoteID > 0 Then
                btnUpdate.Visible = True
                btnAdd.Visible = False
                btnDelete.Visible = True
                Dim dsUserFile As New DataSet
                dsUserFile = m_ManagementService.GetTradeNoteByUserNoteID(m_CurrentNoteID)

                If dsUserFile.Tables.Count > 0 Then
                    If dsUserFile.Tables(0).Rows.Count = 1 Then
                        If Not IsDBNull(dsUserFile.Tables(0).Rows(0).Item("ProjectStatusId")) Then
                            For index = 0 To ddlProjectStatus.Items.Count - 1
                                If ddlProjectStatus.Items(index).Value = dsUserFile.Tables(0).Rows(0).Item("ProjectStatusId") Then
                                    ddlProjectStatus.SelectedIndex = index
                                    Exit For
                                End If
                            Next
                        Else
                            ddlProjectStatus.SelectedValue = "-1"
                        End If
                        If Not IsDBNull(dsUserFile.Tables(0).Rows(0).Item("NoteContent")) Then
                            tbxMessage.Text = dsUserFile.Tables(0).Rows(0).Item("NoteContent")
                        End If
                        If Not IsDBNull(dsUserFile.Tables(0).Rows(0).Item("Description")) Then
                            tbTitle.Text = dsUserFile.Tables(0).Rows(0).Item("Description")
                        End If
                    End If
                End If

                lblNote.Text = "UPDATE TRADE NOTE"
            Else
                btnUpdate.Visible = False
                btnAdd.Visible = True
                btnDelete.Visible = False
                lblNote.Text = "ADD TRADE NOTE"
                If m_ProjectID > 0 Then
                    Dim Project As Project
                    Project = m_ManagementService.GetProjectByProjectId(m_ProjectID)
                    For index = 0 To ddlProjectStatus.Items.Count - 1
                        If ddlProjectStatus.Items(index).Value = Project.ProjectStatusId Then
                            ddlProjectStatus.SelectedIndex = index
                            Exit For
                        End If
                    Next
                End If
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
            Dim CurrentNote As New UserNote
            CurrentNote.Owner = m_ProjectID
            CurrentNote.Description = tbTitle.Text.Trim
            CurrentNote.NoteContent = tbxMessage.Text.Trim
            CurrentNote.ProjectStatusId = ddlProjectStatus.SelectedValue
            m_ManagementService.CreateTradeNote(CurrentNote)

            Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The note is added successfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
        Else
            Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The note is added unsuccessfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If m_CurrentNoteID > 0 And m_ProjectID > 0 Then
            Dim CurrentNote As New UserNote
            CurrentNote.Owner = m_ProjectID
            CurrentNote.UserNoteId = m_CurrentNoteID
            CurrentNote.Description = tbTitle.Text.Trim
            CurrentNote.NoteContent = tbxMessage.Text.Trim
            CurrentNote.ProjectStatusId = ddlProjectStatus.SelectedValue
            m_ManagementService.UpdateTradeNote(CurrentNote)

            Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The note is updated successfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
        Else
            Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The note is updated unsuccessfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Response.Redirect(String.Format("Detail.aspx?ID={0}", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If m_CurrentNoteID > 0 And m_ProjectID > 0 Then
            m_ManagementService.DeleteTradeNoteByUserNoteIDOwner(m_CurrentNoteID, m_ProjectID)

            Response.Redirect(String.Format("Detail.aspx?ID={0}&msg=The note is updated successfully.", m_Cryption.Encrypt(m_ProjectID, m_Cryption.cryptionKey)))
        End If
    End Sub
End Class