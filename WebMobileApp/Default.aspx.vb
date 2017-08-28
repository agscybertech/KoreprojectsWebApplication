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

Partial Class _Default
    Inherits System.Web.UI.Page
    Private m_ManagementService As New ManagementService
    Private m_LoginUser As New User
    Private m_ProjectOwner As New ProjectOwner
    Private m_Cryption As New Cryption
    Private m_LastBillingEndDate As Date
    Private m_NextBillingEndDate As Date
    Private m_Timesheets As New DataSet

    'Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    '    If Session("CurrentLogin") Is Nothing Then
    '        Response.Redirect("../Login.aspx?msg=Time out!")
    '    Else
    '        m_LoginUser = Session("CurrentLogin")
    '        'If m_LoginUser.Type = 0 Then
    '        '    btnDelete.Visible = False
    '        'End If
    '    End If
    'End Sub
End Class
