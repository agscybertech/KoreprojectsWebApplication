
Partial Class Scopes_test
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        rb1.Attributes.Add("onclick", "update('format1');")
        rb2.Attributes.Add("onclick", "update('format2');")
        rb3.Attributes.Add("onclick", "update('format3');")
    End Sub
End Class
