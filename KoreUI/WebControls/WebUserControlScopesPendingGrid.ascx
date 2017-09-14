<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WebUserControlScopesPendingGrid.ascx.vb" Inherits="WebControls_WebUserControlScopesPendingGrid" %>
<asp:Repeater ID="rptScopesPending" runat="server">
    <ItemTemplate>
        <br /><h33><%# DataBinder.Eval(Container.DataItem, "Name")%></h33>
        <asp:GridView ID="gvScopesPending" runat="server" AllowPaging="false" 
            AllowSorting="false" AutoGenerateColumns="False"
            BorderColor="#D7D7D7" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
            EmptyDataText="&lt;table width=&quot;100%&quot;&gt;&lt;tr&gt;&lt;td align=&quot;center&quot; width=&quot;90%&quot;&gt;No Record Found&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;" 
            Width="100%">
            <Columns>
                <asp:BoundField DataField="ScopeGroup" HeaderText="Group" SortExpression="ScopeGroup" ItemStyle-Width="100" Visible="false"/>
                <asp:TemplateField HeaderText="Area" ItemStyle-Width="100" SortExpression="Area">
                    <ItemTemplate>
                        <asp:Label ID="lblArea" runat="server"><%# DisplayArea(Eval("Area", "{0}"), Eval("AreaMeasurement", "{0}"))%></asp:Label>                
                    </ItemTemplate>
                    <ItemStyle Width="150px" />            
                </asp:TemplateField>
                <asp:BoundField DataField="Item" HeaderText="ITEM" SortExpression="Item" ItemStyle-Width="100"/>
                <asp:BoundField DataField="Description" HeaderText="NOTES" ItemStyle-Width="200" Visible="false"/>
                <asp:TemplateField HeaderText="NOTES">
                    <ItemTemplate>
                        <asp:Label ID="lblNotes" runat="server"><%# DisplayNotes(Eval("ScopeItemId", "{0}"), Eval("AssignTo", "{0}"))%></asp:Label>                
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Font-Size="Smaller" />            
                </asp:TemplateField>
                <asp:BoundField DataField="Quantity" HeaderText="QTY" SortExpression="Quantity" DataFormatString="{0:N1}" Visible="false"/>
                <asp:TemplateField HeaderText="QTY">
                    <ItemTemplate>
                        <asp:Label ID="lblQTY" runat="server"><%# DisplayQTY(Eval("Quantity", "{0}"), Eval("Unit", "{0}"))%></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Rate" HeaderText="RATE" SortExpression="Rate" DataFormatString="{0:c}"/>
                <asp:BoundField DataField="Cost" HeaderText="COST" SortExpression="Cost" DataFormatString="{0:c}"/>
                <asp:BoundField DataField="AssignTo" HeaderText="ASSIGN TO" SortExpression="AssignTo" Visible="false"/>
                <asp:TemplateField HeaderText="ACTION">
                    <ItemStyle HorizontalAlign="Center" Width="80" />
                    <ItemTemplate>
                        <table border="0" cellpadding="4" cellspacing="4">
                            <tr>
                                <td>
                                    <asp:LinkButton ID="btnView" runat="server" 
                                        CommandArgument='<%# string.format("{0},{1}",m_Cryption.encrypt(Eval("ScopeID", "{0}"),m_Cryption.cryptionKey),m_Cryption.encrypt(Eval("ScopeItemID", "{0}"),m_Cryption.cryptionKey)) %>' 
                                        onclick="btnView_Click">EDIT</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="btnApprove" runat="server" 
                                        CommandArgument='<%# string.format("{0},{1}",m_Cryption.encrypt(Eval("ScopeID", "{0}"),m_Cryption.cryptionKey),m_Cryption.encrypt(Eval("ScopeItemID", "{0}"),m_Cryption.cryptionKey)) %>' 
                                        onclick="btnApprove_Click">APPROVE</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle BackColor="#F1F1F1" ForeColor="#464646" HorizontalAlign="Center" 
                Wrap="True" />
            <HeaderStyle BackColor="#cccccc" Font-Bold="True" ForeColor="#464646" 
                HorizontalAlign="Center" Height="30" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" 
                Wrap="True" />
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
    </ItemTemplate>
</asp:Repeater>
<asp:GridView ID="gvScopesPending" runat="server" AllowPaging="false" 
    AllowSorting="True" AutoGenerateColumns="False"
    BorderColor="#D7D7D7" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
    EmptyDataText="&lt;table width=&quot;100%&quot;&gt;&lt;tr&gt;&lt;td align=&quot;center&quot; width=&quot;90%&quot;&gt;No Record Found&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;" 
    Width="100%">
    <Columns>
        <asp:BoundField DataField="ScopeGroup" HeaderText="Group" SortExpression="ScopeGroup" ItemStyle-Width="100" Visible="false"/>
        <asp:TemplateField HeaderText="Area" ItemStyle-Width="100" SortExpression="Area">
            <ItemTemplate>
                <asp:Label ID="lblArea" runat="server"><%# DisplayArea(Eval("Area", "{0}"), Eval("AreaMeasurement", "{0}"))%></asp:Label>                
            </ItemTemplate>
            <ItemStyle Width="150px"/>            
        </asp:TemplateField>
        <asp:BoundField DataField="Item" HeaderText="ITEM" SortExpression="Item" ItemStyle-Width="100"/>
        <asp:BoundField DataField="Description" HeaderText="NOTES" ItemStyle-Width="200" Visible="false"/>
        <asp:TemplateField HeaderText="NOTES">
            <ItemTemplate>
                <asp:Label ID="lblNotes" runat="server"><%# DisplayNotes(Eval("ScopeItemId", "{0}"), Eval("AssignTo", "{0}"))%></asp:Label>                
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Font-Size="Smaller" />            
        </asp:TemplateField>
        <asp:BoundField DataField="Quantity" HeaderText="QTY" SortExpression="Quantity" DataFormatString="{0:N1}" Visible="false"/>
        <asp:TemplateField HeaderText="QTY">
            <ItemTemplate>
                <asp:Label ID="lblQTY" runat="server"><%# DisplayQTY(Eval("Quantity", "{0}"), Eval("Unit", "{0}"))%></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Rate" HeaderText="RATE" SortExpression="Rate" DataFormatString="{0:c}"/>
        <asp:BoundField DataField="Cost" HeaderText="COST" SortExpression="Cost" DataFormatString="{0:c}"/>
        <asp:BoundField DataField="AssignTo" HeaderText="ASSIGN TO" SortExpression="AssignTo" Visible="false"/>
        <asp:TemplateField HeaderText="ACTION">
            <ItemStyle HorizontalAlign="Center" Width="80" />
            <ItemTemplate>
                <table border="0" cellpadding="4" cellspacing="4">
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnView" runat="server" 
                                CommandArgument='<%# string.format("{0},{1}",m_Cryption.encrypt(Eval("ScopeID", "{0}"),m_Cryption.cryptionKey),m_Cryption.encrypt(Eval("ScopeItemID", "{0}"),m_Cryption.cryptionKey)) %>' 
                                onclick="btnView_Click">EDIT</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="btnApprove" runat="server" 
                                CommandArgument='<%# string.format("{0},{1}",m_Cryption.encrypt(Eval("ScopeID", "{0}"),m_Cryption.cryptionKey),m_Cryption.encrypt(Eval("ScopeItemID", "{0}"),m_Cryption.cryptionKey)) %>' 
                                onclick="btnApprove_Click">APPROVE</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <RowStyle BackColor="#F1F1F1" ForeColor="#464646" HorizontalAlign="Center" 
        Wrap="True" />
    <HeaderStyle BackColor="#cccccc" Font-Bold="True" ForeColor="#464646" 
        HorizontalAlign="Center" Height="30" />
    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" 
        Wrap="True" />
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
