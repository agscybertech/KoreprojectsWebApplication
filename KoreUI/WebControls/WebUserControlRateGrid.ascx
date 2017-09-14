<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WebUserControlRateGrid.ascx.vb" Inherits="WebControls_WebUserControlRateGrid" %>
<asp:GridView ID="gvRates" runat="server" AllowPaging="false" 
    AllowSorting="True" AutoGenerateColumns="False"
    BorderColor="#D7D7D7" BorderStyle="None" BorderWidth="1px" CellPadding="10" 
    EmptyDataText="&lt;table width=&quot;100%&quot;&gt;&lt;tr&gt;&lt;td align=&quot;center&quot; width=&quot;90%&quot;&gt;No Record Found&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;" 
    Width="100%">
    <Columns>
        <asp:BoundField DataField="Name" HeaderText="Service" SortExpression="Name" ItemStyle-Width="200" ItemStyle-HorizontalAlign="left" />
        <asp:BoundField DataField="CostRate" HeaderText="Cost Rate" SortExpression="CostRate"  ItemStyle-HorizontalAlign="Center" DataFormatString="{0:c}" />
        <asp:BoundField DataField="ChargeRate" HeaderText="Charge Rate" SortExpression="ChargeRate"  ItemStyle-HorizontalAlign="Center" DataFormatString="{0:c}" />
        <asp:BoundField DataField="Unit" HeaderText="Unit" SortExpression="Unit" />
        <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <asp:Label ID="lblStatus" runat="server" Text='<%# ShowStatus(Eval("Status", "{0}")) %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="ACTION">
            <ItemStyle HorizontalAlign="Center" Width="80" />
            <ItemTemplate>
                <a class="form_popup" href="ServiceRate.aspx?id=<%# m_Cryption.encrypt(Eval("ServiceId", "{0}"),m_Cryption.cryptionKey) %>&sid=<%# m_Cryption.encrypt(Eval("ServiceRateId", "{0}"),m_Cryption.cryptionKey) & string.format("&{0}",Datetime.Now.ToString("yyyyddMMhhmmss")) %>" style="text-decoration:none;">EDIT</a>
            </ItemTemplate>
        </asp:TemplateField>
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
