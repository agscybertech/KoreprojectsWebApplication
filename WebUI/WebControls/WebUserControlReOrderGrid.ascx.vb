Imports System.Data
Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities

Partial Class WebControls_WebUserControlReOrderGrid
    Inherits System.Web.UI.UserControl
    Private m_CompanyId As Long
    Private m_BranchId As Long
    Private m_UserId As Long
    Private m_ProjectId As Integer
    Private m_LoginUserId As Long
    Private m_DateFrom As DateTime
    Private m_DateTo As DateTime
    Private m_Keyword As String
    Private m_ManagementService As New ManagementService()
    Public m_Cryption As New Cryption()
    Public Property ProjectID() As Integer
        Get
            Return m_ProjectId
        End Get
        Set(ByVal value As Integer)
            m_ProjectId = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ManagementService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        LoadReorderData()
    End Sub

    Private Sub LoadReorderData()
        Dim dsRorders As DataSet = New DataSet()
        dsRorders = m_ManagementService.GetReorder(m_ProjectId)
        gvReOrder.DataSource = dsRorders.Tables(0)
        gvReOrder.DataBind()
    End Sub


End Class
