<%@ page title="" language="VB" masterpagefile="~/Projects/MasterPage.master" autoeventwireup="false" inherits="Accounts_topup" CodeFile="topup.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function checkAllChecked(targetID, controlCount) {
            for (i = 0; i < controlCount; i++) {
                document.getElementById('ContentPlaceHolder1_rpServicePlan_cbServicePlan_' + i).checked = false;
            }
            document.getElementById('ContentPlaceHolder1_rpServicePlan_cbServicePlan_' + targetID).checked = true;
        }
</script>
<table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
    <tr>
		<td align="left"><h1 id="appointment">Select Your Plan</h1></td>
	</tr>
    <tr>
        <td align="left">
            For every new project you create, it requires a Project Credit. Please select a bundle below to credit your Kore Project account. You can view your credit history anytime under Settings.
        </td>
    </tr>
    <tr>
		<td align="left">
            <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
            <asp:Repeater id="rpServicePlan" runat="server">
            <HeaderTemplate>
                <table border="0" cellspacing="5" cellpadding="5" <%=ShowWidth() %>>                
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox ID="cbServicePlan" Runat="server" EnableViewState="true" Text='<%#DataBinder.Eval(Container, "DataItem.Name")%>' Enabled='<%# IsPlanEnable(DataBinder.Eval(Container, "DataItem.PlanID")) %>'></asp:CheckBox>                            
                            <asp:TextBox ID="tbServicePlanID" runat="server" Text='<%# m_Cryption.encrypt(DataBinder.Eval(Container, "DataItem.PlanID"),m_Cryption.cryptionKey)%>' Visible="false"></asp:TextBox>
                            <asp:TextBox ID="tbPlanPrice" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Price")%>' Visible="false"></asp:TextBox>
                            <asp:TextBox ID="txtPlanId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.PlanId")%>' Visible="false"></asp:TextBox>                            
                            <asp:TextBox ID="txtNumberOfProjects" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NumberOfProjects")%>' Visible="false"></asp:TextBox>
                            <asp:TextBox ID="txtStorageSize" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.StorageSize")%>' Visible="false"></asp:TextBox>
                            <asp:TextBox ID="txtTerm" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Term")%>' Visible="false"></asp:TextBox>
                            <asp:TextBox ID="txtType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Type")%>' Visible="false"></asp:TextBox>
                            <asp:TextBox ID="txtRecurringBillingId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RecurringBillingId")%>' Visible="false"></asp:TextBox>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblPrice" runat="server" Text='<%# String.Format("{0:C}", DataBinder.Eval(Container, "DataItem.Price")) %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblPlanDescription" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.Description")%>'></asp:Label>
                        </td>
                    </tr>                                        
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        </td>
	</tr>
    <tr>
        <td>        
            <asp:Label ID="lblGSTMsg" runat="server" Text="All prices include GST (NZ Only)"></asp:Label>        
        </td>
    </tr>
    <tr>
        <td align="center">
            <table style="width: 100%">
			    <tr>
				    <td style="width: 50%"></td>
				    <td style="width: 50%" align="right"><span class="Apple-converted-space"><asp:Button ID="btnPay" runat="server" Text="Pay Now"/>
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" />
                        </span></td>
			    </tr>
		    </table>
        </td>
    </tr>
</table>
</asp:Content>

