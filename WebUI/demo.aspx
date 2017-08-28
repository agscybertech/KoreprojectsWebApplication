<%@ Page Language="C#" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="true" CodeFile="demo.aspx.cs" Inherits="demo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div id="main_wrapper_inner">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header_inner">
        <h1>Kore Projects Demos</h1>
    </div>

    <div class="content_inner">
        <p>Use the following logins:</p>
        <p>&nbsp;</p>
        <table width="100%" border="0" cellpadding="1" cellspacing="1">
            <tbody>
                <tr>
                    <td valign="top">
                        <h3>Painter Login</h3>
                        <p>User:&nbsp;demopainter@koreprojects.co.nz</p>
                        <p>Password: demo</p>
                    </td>
                    <td valign="top">
                        <h3>Scaffolder Login</h3>
                        <p>User:&nbsp;demoscaffolding@koreprojects.co.nz</p>
                        <p>Password: demo</p>
                    </td>
                </tr>
            </tbody>
        </table>
        <iframe src="Login.aspx" scrolling="No"
            marginwidth="0" marginheight="0" align="MIDDLE"
            frameborder="No" allowtransparency="true" width="100%" height="400"></iframe>
    </div>
</asp:Content>
