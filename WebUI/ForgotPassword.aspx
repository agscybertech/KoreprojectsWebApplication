<%@ Page Title="" Language="VB" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="false" CodeFile="ForgotPassword.aspx.vb" Inherits="ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1>Forgot your password?</h1>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <table cellpadding="4" cellspacing="4" align="center"> 
            <tr>
                <td align="left">
                    <div><label>Enter your email address and we'll send you password reset instructions.</label></div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div class="formlabel"><label>Email</label></div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div class="formfield"><asp:TextBox ID="txtUser" runat="server" Width="270px"></asp:TextBox></div>
                </td>
            </tr>
            <tr><td align="left"><asp:Label ID="lblErrorMessage" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label></td></tr>
            <tr>
                <td>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required." ControlToValidate="txtUser" Display="None"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtUser" Display="none" ErrorMessage="Email is not valid." ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>                
                    <asp:ValidationSummary ID="vsPersonalInformation" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Button ID="btnLogin" runat="server" Text="Send" OnClick="btnLogin_Click" Font-Size="16px" />
                </td>
            </tr>
        </table>
    </div>
    <!-- content -->
</asp:Content>

