<%@ page title="" language="VB" masterpagefile="~/Projects/MasterPage.master" autoeventwireup="false" inherits="Accounts_PaymentHistory" CodeFile="PaymentHistory.aspx.vb" %>
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
                        <a href="../Contacts/ProjectOwnerDetail.aspx" style="text-decoration:none;pointer:hand;color:White;">User Settings</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                        
                        <a href="../Contacts/OwnerService.aspx" style="text-decoration:none;pointer:hand;color:White;">Service Rates</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <a href="../Contacts/OwnerSetting.aspx" style="text-decoration:none;pointer:hand;color:White;">System Setup</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <a href="Plans.aspx" style="text-decoration:none;pointer:hand;color:White;">Subscription</a>
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
            <tr runat="server" id="trServiceRate">
                <td>
                    <table style="width:100%">
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td>
                                            Subscription:
                                        </td>
                                        <td>                                        
                                            <asp:Label ID="lblPlanPrice" runat="server"></asp:Label>
                                            <asp:Label ID="lblSubscription" runat="server"></asp:Label>                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Next payment due:
                                        </td>
                                        <td>                                        
                                            <asp:Label ID="lblNextPaymentDue" runat="server"></asp:Label>                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Payment info:
                                        </td>
                                        <td>                                        
                                            <asp:Label ID="lblPaymentInfo" runat="server"></asp:Label>                                        
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnEditPlan" runat="server" Text="Edit my plan" /> 
                                &nbsp;<asp:Button ID="btnCancelSubscription" runat="server" Text="Cancel my subscription" />
                            &nbsp;<asp:Button ID="btnPaymentInfoXML" runat="server" Text="Payment Info XML" Visible="false" />
                            </td>                        
                        </tr>
                        <tr>
                            <td valign="bottom">
                                <h2>Your payment history</h2>                                        
                            </td>
                            <td valign="bottom" align="right">
                                <a id="button" href="selectplan.aspx" style="float:right;display:none;">TOP UP</a>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:GridView ID="gvPaymentHistory" runat="server" AllowPaging="false" 
                                    AllowSorting="True" AutoGenerateColumns="False"
                                    BorderColor="#D7D7D7" BorderStyle="None" BorderWidth="1px" CellPadding="10" 
                                    EmptyDataText="&lt;table width=&quot;100%&quot;&gt;&lt;tr&gt;&lt;td align=&quot;center&quot; width=&quot;90%&quot;&gt;No Record Found&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;" 
                                    Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="CreatedDate" HeaderText="Transaction Date" SortExpression="CreatedDate" ItemStyle-HorizontalAlign="center" DataFormatString="{0:D}" />
                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" ItemStyle-Width="140" ItemStyle-HorizontalAlign="left" />
                                        <asp:TemplateField HeaderText="Cost">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCreditAmount" runat="server"><%# ShowCreditAmount(Eval("CreditAmount", "{0}"), Eval("NumberOfProjectCredits", "{0}"))%></asp:Label>                                                
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Font-Size="Smaller" />            
                                        </asp:TemplateField>                                       
                                        <asp:BoundField DataField="CreditAmount" HeaderText="Purchase Amount" SortExpression="CreditAmount" ItemStyle-HorizontalAlign="center" DataFormatString="{0:c}" Visible="false" />
                                        <asp:BoundField DataField="DebitAmount" HeaderText="Refund Amount" SortExpression="DebitAmount" ItemStyle-HorizontalAlign="center" DataFormatString="{0:c}" Visible="false" />
                                        <asp:BoundField DataField="NumberOfProjectCredits" HeaderText="Project Credit" SortExpression="NumberOfProjectCredits" ItemStyle-HorizontalAlign="center" />
                                        <asp:BoundField DataField="ProjectCreditBalance" HeaderText="Project Credit Balance" SortExpression="ProjectCreditBalance" ItemStyle-HorizontalAlign="center" />                                        
                                    </Columns>
                                    <RowStyle BackColor="#F1F1F1" ForeColor="#464646" HorizontalAlign="Center" 
                                        Wrap="False" />
                                    <HeaderStyle BackColor="#cccccc" Font-Bold="True" ForeColor="#464646" 
                                        HorizontalAlign="Center" Height="30" />
                                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" 
                                        Wrap="False" />
                                    <PagerStyle ForeColor="#8C4510" HorizontalAlign="Right" />
                                    <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
                                    <PagerStyle BackColor="#cccccc" BorderWidth="0" ForeColor="#464646" />
                                    <PagerTemplate>
                                        <table id="tablenav" width="100%" border="0" cellspacing="5" cellpadding="0">
                                            <tr>
                                                <td align="left" style="width:68%">
                                                    <asp:Label ID="lblPageMessage" runat="server"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:ImageButton ID="lbnPre" OnClick="lbnPre_Click" runat="server" ImageUrl="../images/prev_bt.png" />&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="lblPageLabel" runat="server">Go to page:</asp:Label>
                                                    <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="true" 
                                                        OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                                                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;
                                                    <asp:ImageButton ID="lbnNext" OnClick="lbnNext_Click" runat="server" ImageUrl="../images/next_bt.png" />
                                                </td>
                                            </tr>
                                        </table>
                                    </PagerTemplate>
                                </asp:GridView>                                
                            </td>
                        </tr>
                    </table>
                </td>		        
	        </tr>                        
        </table>
    </div>
    <!-- content -->
</asp:Content>

