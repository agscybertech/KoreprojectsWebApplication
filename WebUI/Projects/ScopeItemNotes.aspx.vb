Imports Warpfusion.A4PP.Objects
Imports Warpfusion.A4PP.Services
Imports Warpfusion.Shared.Utilities
Partial Class Projects_ScopeItemNotes
    Inherits System.Web.UI.Page
    Private m_ScopeService As New ScopeServices
    Private m_LoginUser As New User
    Public m_Cryption As New Cryption()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        m_ScopeService.SQLConnection = ConfigurationManager.ConnectionStrings("ApplicationServices").ConnectionString
        Dim ScopeItem As ScopeItem
        ScopeItem = m_ScopeService.GetScopeItemByScopeItemId(CInt(m_Cryption.Decrypt(Request.QueryString("id"), m_Cryption.cryptionKey)))
        lblService.Text = String.Format("{0}", ScopeItem.Service)
        lblAssignTo.Text = String.Format("{0}", ScopeItem.AssignTo)
        lblNotes.Text = String.Format("{0}", ScopeItem.Description)

    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Session("CurrentLogin") Is Nothing Then
            Response.Redirect("../Signin.aspx?msg=Time out!")
        Else
            m_LoginUser = Session("CurrentLogin")
        End If
    End Sub
End Class
