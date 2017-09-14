<%@ Page Title="" Language="VB" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="false" CodeFile="ResetPassword.aspx.vb" Inherits="ResetPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblPageTitle" runat="server" Text="Reset Your Password"></asp:Label>
        </h1>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
<table cellpadding="4" cellspacing="4" align="center"> 
    <tr>
        <td>
            <div class="formlabel"><label>User's Email</label></div>
            <div class="formfield"><asp:TextBox ID="txtUser" runat="server" ReadOnly="true"></asp:TextBox></div>
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
            <div class="formfield"><asp:TextBox ID="txtConfirmPassword" runat="server" 
                    TextMode="Password"></asp:TextBox></div>
        </td>
    </tr>
    <tr><td colspan="2"><asp:Label ID="lblErrorMessage" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label></td></tr>
    <tr>
        <td colspan="2" align="center">
            <asp:Button ID="btnLogin" runat="server" Text="Submit" OnClick="btnLogin_Click" 
                Font-Size="16px" />
        </td>
    </tr>
</table>
    </div>
    <!-- content -->
</asp:Content>

