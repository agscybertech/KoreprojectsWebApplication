<%@ Page Language="VB" AutoEventWireup="false" CodeFile="signup_remote.aspx.vb" Inherits="signup_remote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>    
    <link href="http://www.koreprojects.com/templates/css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="contentwrap admin" align="center">
        <div class="center">
    <!-- content -->
    <div id="content" align="center">
        <table cellpadding="4" cellspacing="4" align="left">
            <tr>
                <td>
<h2>Interested to see how Kore Projects can transform your company? Try for free today and start reaping the benefits of the most easy to use Project Management System on the market.</h2><br />
Simply create your own login and get full access to the Kore Projects system. You will get a 60 Day Free Trial so you can try out all the features.
<br /><br />
We hope you enjoy using Kore Projects. Contact us anytime if you have any questions.             
                </td>
            </tr>
        </table><br />
        <table cellpadding="4" cellspacing="4" align="center" style="margin-top:30px">
            <tr>
                <td>
                    <div class="formlabel"><label>Email Address</label></div>
                    <div class="formfield"><asp:TextBox ID="txtUser" runat="server"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Password</label></div>
                    <div class="formfield"><asp:TextBox ID="txtPassword" runat="server" 
                            TextMode="Password"></asp:TextBox></div>    
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Confirm Password</label></div>
                    <div class="formfield"><asp:TextBox ID="txtCPassword" runat="server" 
                            TextMode="Password"></asp:TextBox></div>    
                </td>
            </tr>
            <tr><td>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Enabled="true" ErrorMessage="Email is required." ControlToValidate="txtUser" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtUser" Display="none" ErrorMessage="Email is not valid." ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="rfvPW" runat="server" Enabled="true" ErrorMessage="Password is required." ControlToValidate="txtPassword" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvCPW" runat="server" Enabled="true" ErrorMessage="Confirm Password is required." ControlToValidate="txtCPassword" Display="None"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvPW" runat="server" ControlToCompare="txtPassword" ControlToValidate="txtCPassword" Display="none" ErrorMessage="Password does not match."></asp:CompareValidator>
                <asp:ValidationSummary ID="vsSignup" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
                <asp:Label ID="lblErrorMessage" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
                </td></tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnSignup" runat="server" Text="Create Login" OnClick="btnSignup_Click" 
                        Font-Size="16px" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:HyperLink ID="hlkForgottenPassward" runat="server" 
                        NavigateUrl="../ForgotPassword.aspx" Target="_top" Visible="False">Forgotten your password?<br/>CLICK HERE</asp:HyperLink>
                </td>
            </tr>
        </table>
    </div>
    <!-- content -->    
        </div>
    </div>
    </form>
</body>
</html>
