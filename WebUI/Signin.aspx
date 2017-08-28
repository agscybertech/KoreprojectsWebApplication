<%@ Page Language="C#" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="true" CodeFile="Signin.aspx.cs" Inherits="Signin" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div id="main_wrapper_inner">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header_inner">
        <h1>Login</h1>
    </div>
    <div class="content_inner">
        <asp:Label ID="lblErrorMessage" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
        <iframe src="login.aspx" scrolling="No"
            marginwidth="0" marginheight="0" align="MIDDLE"
            frameborder="No" allowtransparency="true" width="100%" height="350"></iframe>
    </div>
</asp:Content>

