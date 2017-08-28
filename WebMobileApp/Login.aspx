<%@ Page Title="" Language="VB" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1>KORE MOBILE</h1>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left" style="display:none;">
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
            <tr><td><asp:Label ID="lblErrorMessage" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label></td></tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" 
                        Font-Size="16px" />
                </td>
            </tr>
        </table>
    </div>
    <div id="Div1" align="left">
        <p>To get access you need to click on your <b>Kore Mobile Access Link</b> originally emailed to you, or open a bookmark/shortcut you may have already saved for this.</p>
        <p>Any problems please contact your Office Manager.</p>
    </div>
    <!-- content -->
</asp:Content>

