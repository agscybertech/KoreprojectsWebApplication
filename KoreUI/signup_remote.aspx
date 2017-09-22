<%@ Page Language="VB" AutoEventWireup="false" CodeFile="signup_remote.aspx.vb" Inherits="signup_remote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js" type="text/javascript"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" id="hs-script-loader" async defer src="//js.hs-scripts.com/3895077.js"></script>
    <link href="~/css/my_style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="row-fluid">
            <div class="container">
                <div class="col-xs-12 text-center">
                    <asp:Label ID="lblErrorMessage" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Enabled="true" ErrorMessage="Email is required." ControlToValidate="txtUser" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtUser" Display="none" ErrorMessage="Email is not valid." ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvPW" runat="server" Enabled="true" ErrorMessage="Password is required." ControlToValidate="txtPassword" Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvCPW" runat="server" Enabled="true" ErrorMessage="Confirm Password is required." ControlToValidate="txtCPassword" Display="None"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvPW" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtCPassword" Display="none" ErrorMessage="Password does not match."></asp:CompareValidator>
                    <asp:ValidationSummary ID="vsSignup" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server" />
                </div>
                <div class="col-xs-12">
                    <div class="demo_page_form">
                        <span>email address</span>
                        <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
                        <span>Password</span>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <span>Confirm Password</span>
                        <asp:TextBox ID="txtCPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnSignup" runat="server" Text="Create Login" CssClass="my_btn" OnClick="btnSignup_Click" Style="padding: 11px 10px;" />
                    </div>
                </div>
                 <div class="col-xs-12 text-center">
                    <asp:HyperLink ID="hlkForgottenPassward" runat="server" NavigateUrl="ForgotPassword.aspx">Forgot your password?</asp:HyperLink>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
