<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="Invite.aspx.vb" Inherits="Contacts_Invite" %>
<%@ Register TagPrefix="UC" TagName="ServiceRatesGrid" Src="~/WebControls/WebUserControlRateGrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
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
            <asp:Label ID="lblmsg" runat="server" Font-Size="X-Small" ForeColor="Red"></asp:Label>
        </div>

        <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
            <tr>
                <td colspan="2" align="left">
                    <h4>INVITE</h4>
                </td>
            </tr>
            <tr>
		        <td valign="top">
                    <div class="formlabel"><label>Email Address</label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtEmail" maxlength="100" runat="server" 
                            TabIndex="140" Width="270px"></asp:TextBox>                        
                    <asp:Button ID="btnAdd" runat="server" Text="+" Width="20px" /></div>
                </td>
                <td>
                    <asp:GridView ID="gvEmail" runat="server" AutoGenerateColumns="False" BorderColor="#D7D7D7" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    EmptyDataText="&lt;table width=&quot;100%&quot;&gt;&lt;tr&gt;&lt;td align=&quot;center&quot; width=&quot;90%&quot;&gt;No Record Found&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;" 
                    Width="100%">
                    <Columns>
                        <asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Center" Width="20" />
                            <ItemTemplate>
                                <asp:Button ID="btnRemove" runat="server" Text="-" CommandArgument='<%# Eval("Email", "{0}") %>' onclick="btnRemove_Click"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>               
                </td>
	        </tr>
            <tr>
		        <td colspan="2">
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required." ControlToValidate="txtEmail" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" Display="none" ErrorMessage="Email is not valid." ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>
                <asp:ValidationSummary ID="vsPersonalInformation" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 33%" align="left">&nbsp;</td>
                        <td align="center">                        

                        </td>
				        <td style="width: 33%" align="right"><span class="Apple-converted-space">
                            <asp:Button ID="btnInvite" runat="server" Text="Invite" CausesValidation="False" Font-Size="16px" TabIndex="190" />
                            <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" 
                                Font-Size="16px" TabIndex="210" />&nbsp;</span></td>
			        </tr>
		        </table>
		        </td>
	        </tr>
        </table>
    </div>
    <!-- content -->
</asp:Content>
