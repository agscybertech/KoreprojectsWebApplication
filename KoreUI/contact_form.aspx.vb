
Partial Class contact_form
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.QueryString.Count > 0 Then
            lblErrorMessage.Text = Request.QueryString("msg")
        End If
    End Sub

    Protected Sub butsubmit_Click(sender As Object, e As EventArgs)
        Dim flgmailsent As Boolean = False
        If Page.IsValid Then
            Dim MailMessage As New System.Net.Mail.MailMessage()
            Dim emailClient As New Net.Mail.SmtpClient(ConfigurationManager.AppSettings("SMTPServer"))
            'MailMessage.To.Add(New System.Net.Mail.MailAddress(ContactProfile.Email))
            MailMessage.To.Add(New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("AdminEmail")))
            MailMessage.From = New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("SupportEmail"))
            MailMessage.Subject = String.Format("Visitor - " + Name.Text + " wants to contact Kore Projects")
            MailMessage.Body = String.Format("Hi there,<br><br> I am " + Name.Text + " <br>My Email is " + EmailFrom.Text + " <br> and my Phone No. is " + Phone.Text + "<br> and my query is <br><br>" + bodytxt.Text)
            MailMessage.IsBodyHtml = True
            Try
                emailClient.Send(MailMessage)
                flgmailsent = True
            Catch ex As Exception
                lblErrorMessage.Text = "Error ! server could not send email"
                Throw
            End Try
        End If
        If flgmailsent = True Then
            Response.Redirect("contact_form.aspx?msg=We have received your mail and working on it Thank you")
        End If
    End Sub

    Protected Sub ValidateCaptcha(sender As Object, e As ServerValidateEventArgs)
        CaptchaControl1.ValidateCaptcha(captchacode.Text.Trim())
        e.IsValid = CaptchaControl1.UserValidated
        If Not e.IsValid Then
            lblErrorMessage.Text = "Invalid image code entered"
        End If
    End Sub

    Protected Sub Validate_form(sender As Object, args As ServerValidateEventArgs)
        If String.IsNullOrEmpty(Name.Text.Trim) Then
            args.IsValid = False
            lblErrorMessage.Text = "Name can not be empty"
        End If
        If String.IsNullOrEmpty(EmailFrom.Text.Trim) Then
            args.IsValid = False
            lblErrorMessage.Text = "Email can not be empty"
        End If
        If String.IsNullOrEmpty(Phone.Text.Trim) Then
            args.IsValid = False
            lblErrorMessage.Text = "Phone no. can not be empty"
        End If
        If Not Regex.IsMatch(Phone.Text.Trim, "^(\+0?1\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$") Then
            args.IsValid = False
            lblErrorMessage.Text = "Only numbers allowed in Phone no. field"
        End If
    End Sub

    Protected Sub lnkrefresh_Click(sender As Object, e As EventArgs)

    End Sub
End Class
