
Partial Class SystemRequirements
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.UrlReferrer Is Nothing Then
            lblErrorMsg.Text = String.Format("You are using {0} {1}, which is an older version of {0}, and is not supported by our system. Please update it with the latest version of {0}.", Request.Browser.Browser, Request.Browser.Version)
        End If
    End Sub
End Class
