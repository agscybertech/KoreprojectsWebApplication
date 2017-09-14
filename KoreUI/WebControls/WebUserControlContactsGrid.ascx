<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WebUserControlContactsGrid.ascx.vb" Inherits="WebControls_WebUserControlContactsGrid" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
<asp:GridView ID="gvContacts" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False"
    BorderColor="#D7D7D7" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
    EmptyDataText="&lt;table width=&quot;100%&quot;&gt;&lt;tr&gt;&lt;td align=&quot;center&quot; width=&quot;90%&quot;&gt;No Record Found&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;" 
    Width="100%">
    <Columns>
         <asp:TemplateField>
            <ItemStyle HorizontalAlign="center" width="60" />
            <ItemTemplate>
                <asp:LinkButton ID="lbImageView" runat="server" onclick="btnView_Click" CommandArgument='<%# m_Cryption.encrypt(Eval("UserID", "{0}"),m_Cryption.cryptionKey) %>'><img src='<%# showFile(Eval("Identifier", "{0}"), Eval("PersonalPhoto", "{0}")) %>' style='<%# isFileExisted(Eval("Identifier", "{0}"), Eval("PersonalPhoto", "{0}")) %>' width="50" border="0"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="NAME">
            <ItemStyle HorizontalAlign="Left" Width="200" />
            <ItemTemplate>
                <asp:LinkButton ID="lbNameView" runat="server" CommandArgument='<%# m_Cryption.encrypt(Eval("UserID", "{0}"),m_Cryption.cryptionKey) %>' onclick="btnView_Click"><asp:Label ID="lblName" runat="server"><%# Eval("FirstName", "{0}")%>&nbsp;<%# Eval("LastName", "{0}")%></asp:Label></asp:LinkButton>                
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="PHONE">
            <ItemTemplate>
                <asp:Label ID="lblPhone" runat="server"><%# DisplayPhones(Eval("UserID", "{0}"))%></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Font-Size="Smaller" />            
        </asp:TemplateField>
        <asp:TemplateField  HeaderText="Status">
            <ItemTemplate>
                <asp:LinkButton runat="server" Text='<%# Eval("StatusName")%>' CommandName="ChangeStatus" CommandArgument='<%# Eval("UserID") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Email" HeaderText="EMAIL" SortExpression="Email" ItemStyle-Width="200"/>        
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