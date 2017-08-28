<%@ Page Title="" Language="VB" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Signup_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1>Signup</h1>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <table cellpadding="4" cellspacing="4" align="center" style="margin-top:60px"> 
            <tr>
                <td>
                    <div class="formlabel"><label>User's Email</label></div>
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
                    <asp:Button ID="btnSignup" runat="server" Text="Signup" OnClick="btnSignup_Click" 
                        Font-Size="16px" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:HyperLink ID="hlkForgottenPassward" runat="server" NavigateUrl="../ForgotPassword.aspx">Forgotten your password?<br/>CLICK HERE</asp:HyperLink>
                </td>
            </tr>
        </table>
    </div>
    <!-- content -->
</asp:Content>

