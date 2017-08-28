<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Styles/template.css" rel="stylesheet" type="text/css" />
    <link href="Styles/template-remote.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="contentwrap admin" align="center">
            <div class="center">

                <!-- content -->
                <div id="content" align="left">
                    <table cellpadding="4" cellspacing="4" align="center" style="margin-top: 60px">
                        <tr>
                            <td>
                                <div class="formlabel">
                                    <label>User's Email</label></div>
                                <div class="formfield">
                                    <asp:TextBox ID="txtUser" runat="server"></asp:TextBox></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="formlabel">
                                    <label>Password</label></div>
                                <div class="formfield">
                                    <asp:TextBox ID="txtPassword" runat="server"
                                        TextMode="Password"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblErrorMessage" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click"
                                    Font-Size="16px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:HyperLink ID="hlkForgottenPassward" runat="server" NavigateUrl="ForgotPassword.aspx">Forgotten your password?<br/>CLICK HERE</asp:HyperLink>
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
