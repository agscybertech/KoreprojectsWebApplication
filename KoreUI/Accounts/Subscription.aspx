<%@ page title="" language="VB" masterpagefile="~/Projects/MasterPage.master" autoeventwireup="false" inherits="Accounts_Subscription" CodeFile="Subscription.aspx.vb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <table style="padding:8px;" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="background-image:url(../images/sub-nav-left.png); width:16px">
                        <img src="../images/sub-nav-left.png" width="16" height="40" style="border:0" />
                    </td>
                    <td style="background-image:url(../images/sub-nav-bg.png);">                        
                        <a href="../Contacts/OwnerService.aspx" style="text-decoration:none;pointer:hand;color:White;">Service Rates</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <a href="../Contacts/OwnerSetting.aspx" style="text-decoration:none;pointer:hand;color:White;">System Setup</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <a href="../Contacts/ProjectOwnerDetail.aspx" style="text-decoration:none;pointer:hand;color:White;">User Settings</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                        
                        <a href="Subscription.aspx" style="text-decoration:none;pointer:hand;color:White;">Subscription</a>
                        <a href="PaymentHistory.aspx" style="text-decoration:none;pointer:hand;color:White;display:none;">Payment History</a>
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
            <asp:Label ID="lblmsg" runat="server" Font-Size="X-Small" ForeColor="Red"></asp:Label>
        </div>

        <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
            <tr>
                <td id="tdOldPlan" runat="server">
                    <table width="100%">
                        <tr>
                            <td>
                                <h2><asp:Label ID="lblOldPlan" runat="server" Text="Old Plan"></asp:Label></h2>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblOldPlanDescription" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <h2><asp:Label ID="lblCurrentPlan" runat="server" Text="Current Plan"></asp:Label></h2>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCurrentPlanDescription" runat="server"></asp:Label>
                                <br />
                                <asp:Label ID="lblNextPaymentDue" runat="server"></asp:Label>                                
                            </td> 
                            <td width="40%" valign="top">
                                <asp:Button ID="btnEditPlan" runat="server" Text="Edit my plan" />                                
                            </td>
                        </tr>                   
                    </table>
                </td>
            </tr>
            <tr>
                <td><hr /></td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                <h2>Credit Card Details</h2>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPaymentInfo" runat="server"></asp:Label>
                            </td>
                            <td width="40%">
                                <asp:Button ID="btnCancelSubscription" runat="server" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <br />
    <!-- content -->
</asp:Content>

