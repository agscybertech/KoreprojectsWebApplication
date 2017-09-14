<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="Timesheets.aspx.vb" Inherits="Contacts_Timesheets" %>
<%@ Register TagPrefix="UC" TagName="TimesheetGrid" Src="~/WebControls/WebUserControlTimesheetGrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <div align="center" width="100%">
            <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
        </div>
        <table width="100%" border="0" cellspacing="2" cellpadding="5">
            <tr>
                <td width="150">
                    <asp:Label ID="lblDateRange" runat="server" Text=""></asp:Label>
                </td>
                <td width="10">
                    <asp:LinkButton ID="lbnPrev" runat="server" Width="10px"><img src="../images/arrowL.png" border="0" /></asp:LinkButton>
                </td>
                <td style="background:none; text-align:center; font-weight:bold" width="60">
                    <asp:LinkButton ID="lbnCurrentBilling" runat="server">This Week</asp:LinkButton>
                </td>
                <td width="10">
                    <asp:LinkButton ID="lbnNext" runat="server" Width="10px"><img src="../images/arrowR.png" border="0" /></asp:LinkButton>
                </td>
                <td align="right">
                    <asp:Button ID="btnClose" runat="server" Text="Close Time Sheets" Visible="false" />&nbsp;&nbsp;&nbsp;
                    <input id="btnPrint" type="button" value="Print Time Sheets" onclick="window.open('../Print/Timesheets.aspx');" />
                </td>                                
            </tr>
        </table>
        <UC:TimesheetGrid ID="TimesheetGrid" runat="server" />
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


