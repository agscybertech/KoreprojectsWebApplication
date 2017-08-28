<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WebUserControlJobGrid.ascx.vb" Inherits="WebControls_WebUserControlJobGrid" %>
<asp:GridView ID="gvJobs" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False"
    BorderColor="#D7D7D7" BorderStyle="None" BorderWidth="1px" CellPadding="10" 
    EmptyDataText="&lt;table width=&quot;100%&quot;&gt;&lt;tr&gt;&lt;td align=&quot;center&quot; width=&quot;90%&quot;&gt;No Record Found&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;" 
    Width="100%">
    <Columns>
        <asp:BoundField DataField="JobName" HeaderText="Project Job" SortExpression="JobName" ItemStyle-Width="140" ItemStyle-HorizontalAlign="left" />
        <asp:TemplateField HeaderText="Assignee">
            <ItemStyle HorizontalAlign="Left" Width="160" />
            <ItemTemplate>
                <asp:Label ID="lblJob" runat="server" Text='<%# GetJobAssigneeName(Eval("JobId", "{0}"))%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Note" SortExpression="Description">
            <ItemStyle HorizontalAlign="Left" Wrap="true" />
            <ItemTemplate>
                <asp:Label ID="lblJobNote" runat="server" Text='<%# ShowNote(Eval("Description", "{0}"))%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate" DataFormatString="{0:ddd dd/MM/yyyy}" HtmlEncode="False"  ItemStyle-Width="160" />
        <asp:TemplateField HeaderText="Status" SortExpression="Status">
            <ItemStyle HorizontalAlign="Left" Wrap="true" />
            <ItemTemplate>
                <asp:Label ID="lblJobStatus" runat="server" Text='<%# ShowStatus(Eval("Status", "{0}"))%>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="ACTION">
            <ItemStyle HorizontalAlign="Center" Width="80" />
            <ItemTemplate>
                <a href="Job.aspx?id=<%# m_Cryption.Encrypt(Eval("ProjectId", "{0}") & "-" & Eval("JobId", "{0}"), m_Cryption.cryptionKey)%>" rel="#overlaynote" style="text-decoration:none;display:none;">EDIT</a>
                <a class="form_popup" href="Job.aspx?id=<%# m_Cryption.Encrypt(Eval("ProjectId", "{0}") & "-" & Eval("JobId", "{0}"), m_Cryption.cryptionKey) & String.Format("&{0}", DateTime.Now.ToString("yyyyddMMhhmmss"))%>" style="text-decoration:none;">EDIT</a>
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