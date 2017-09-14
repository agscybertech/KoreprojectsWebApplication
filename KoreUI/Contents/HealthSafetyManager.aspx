<%@ Page Title="Health and Safety" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="true" CodeFile="HealthSafetyManager.aspx.vb" Inherits="Contents_HealthSafetyManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <asp:Label ID="lblPrintScript" runat="server" Text=""></asp:Label>
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text="Health & Safety"></asp:Label></h1>                     
            <a style="padding:8px;float:right;" id="help" href="http://my.koreprojects.co.nz/helpconsole2010/Help/default.aspx" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
            <a style="padding:8px;float:right;" id="healthsafety" href="../contents/HealthSafety.aspx" title="Health Safety"><img src="../images/healthsafety-but.png" width="40" height="40" border="0" alt="Health Safety" /></a>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <div align="center" width="100%">
            <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
        </div>

        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td valign="bottom">
                    <h3 id="note"></h3>
                </td>
                <td valign="bottom" align="right">
                    <a id="MiddleContent_button" class="form_popup" href="HealthSafetyEditor.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & string.format("&{0}",Datetime.Now.ToString("yyyyddMMhhmmss")) %>" style="float:right">ADD</a>                    
                </td>
            </tr>
            <tr>
                <td colspan="2">                    
                    <asp:Repeater ID="rptDynamicPageContents" runat="server">
                    </asp:Repeater>                    
                </td>
            </tr>
        </table>

    </div>
    <!-- content -->
</asp:Content>