<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="View.aspx.vb" Inherits="Contacts_View" %>
<%@ Register TagPrefix="UC" TagName="ContactsGrid" Src="~/WebControls/WebUserControlContactsGrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <a style="padding:8px 25px 8px 8px;float:right;" id="help" href="https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
            <a style="padding:8px;float:right;" id="addContact" href="detail.aspx" title="Add Contact"><img src="../images/contact-add.png" width="40" height="40" border="0" alt="Add Contact" /></a>
            <asp:LinkButton ID="lbnTimesheet" runat="server" style="padding:8px;float:right;" title="Timesheet"><img src="../images/timesheets.png" alt="Timesheet" border="0" /></asp:LinkButton>
            <a style="padding:8px;float:right;display:none;" id="invite" href="invite.aspx" title="Send Invitation"><img src="../images/invite.png" width="40" height="40" border="0" alt="Send Invitation" /></a>
            <table style="padding:8px;" border="0" cellpadding="0" cellspacing="0" width="280px">
                <tr>
                    <td style="background-image:url(../images/sub-nav-left.png); width:16px">
                        <img src="../images/sub-nav-left.png" width="16" height="40" style="border:0" />
                    </td>
                    <td style="background-image:url(../images/sub-nav-bg.png);">
                        <asp:LinkButton ID="lbnWorkers" runat="server">Staff</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lbnContractors" runat="server">Contractors</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lbnSuppliers" runat="server">Suppliers</asp:LinkButton>
                    </td>
                    <td style="background-image:url(../images/sub-nav-right.png); width:16px">
                    </td>
                </tr>
            </table>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <div align="center" width="100%">
            <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
        </div>
        <UC:ContactsGrid ID="ContactsGrid" runat="server" />
    </div>
    <!-- content -->

    <!-- footer -->
    <div class="clr" id="Div1">
        <div class="sep" id="Div2"></div>
        	<table width="100%" style="height:45px">
                <tr>
                    <td> 
                        <div runat="server" id="divFooterTitle">
                            
                        </div>
                    </td>
                </tr>
            </table> 
        <div class="clr sep" id="Div3"></div>
    </div>
    <!-- footer -->
</asp:Content>

