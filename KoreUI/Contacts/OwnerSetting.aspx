<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="OwnerSetting.aspx.vb" Inherits="Contacts_OwnerSetting" %>
<%@ Register TagPrefix="UC" TagName="AreaGrid" Src="~/WebControls/WebUserControlAreaGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="ItemGrid" Src="~/WebControls/WebUserControlItemGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="WorksheetGroupGrid" Src="~/WebControls/WebUserControlWorksheetGroupGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="WorksheetServiceGrid" Src="~/WebControls/WebUserControlServiceGrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <div runat="server" id="divTitle">
                <a style="padding:8px 25px 8px 8px;float:right;" id="help" href="https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
                <table style="padding-top:8px;" border="0" cellpadding="0" cellspacing="0" width="612px">
                    <tr>
                        <td style="background-image:url(../images/sub-nav-left.png); width:16px">
                            <img src="../images/sub-nav-left.png" width="16" height="40" style="border:0" />
                        </td>
                        <td style="background-image:url(../images/sub-nav-bg.png);">                            
                            <a href="OwnerSetting.aspx" style="text-decoration:none;pointer:hand;color:#0481D1;font-weight:bold;">Worksheet Setup</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="OwnerService.aspx" style="text-decoration:none;pointer:hand;color:White;">Service Rates</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="ProjectSetting.aspx" style="text-decoration:none;pointer:hand;color:White;">Project Setup</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                       
                            <a href="ProjectOwnerDetail.aspx" style="text-decoration:none;pointer:hand;color:White;">User Settings</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                        
                            <a href="../Accounts/Plans.aspx" style="text-decoration:none;pointer:hand;color:White;">Subscription</a>
                            <a href="../Accounts/PaymentHistory.aspx" style="text-decoration:none;pointer:hand;color:White;display:none;">Payment History</a>
                        </td>
                        <td style="background-image:url(../images/sub-nav-right.png); width:16px">
                        </td>
                    </tr>
                </table>
            </div>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <div align="center" width="100%">
            <asp:Label ID="lblmsg" runat="server" Font-Size="X-Small" ForeColor="Red"></asp:Label>
        </div>

        <table style="width: 90%" align="center" cellpadding="4" cellspacing="4" runat="server" id="tblOwnerDetail">
            <tr runat="server" id="trAreaItem">
                <td valign="top" style="width:25%">
                    <h2 id="note">Group<a id="button" class="form_popup" href="WorksheetGroup.aspx" style="float:right">ADD</a></h2>
                    <UC:WorksheetGroupGrid ID="WorksheetGroupGrid" runat="server" />
                </td>
                <td valign="top" style="width:25%">
                    <h2 id="H1">Area<a id="MiddleContent_A1" class="form_popup" href="Area.aspx" style="float:right">ADD</a></h2>
                    <UC:AreaGrid ID="AreaGrid" runat="server" />
                </td>
                <td valign="top" style="width:25%">
                    <h2 id="H2">Item<a id="MiddleContent_A2" class="form_popup" href="Item.aspx" style="float:right">ADD</a></h2>
                    <UC:ItemGrid ID="ItemGrid" runat="server" />
                </td>
                <td valign="top" style="width:25%">
                    <h2 id="H3">Service<a id="MiddleContent_A3" class="form_popup" href="WorksheetService.aspx" style="float:right">ADD</a></h2>
                    <UC:WorksheetServiceGrid ID="WorksheetServiceGrid" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <!-- content -->

    <!-- footer -->
    <div class="clr" id="Div1">
        <div class="sep" id="Div2"></div>
        	<table width="100%" style="height:45px">
                <tr>
                    <td> 
                    </td>
                </tr>
            </table> 
        <div class="clr sep" id="Div3"></div>
    </div>
    <!-- footer -->
</asp:Content>