<%@ Control Language="VB" AutoEventWireup="false" CodeFile="WebUserControlProjectsGrid.ascx.vb" Inherits="WebControls_WebUserControlProjectsGrid" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Repeater ID="rptProjectsGrid" runat="server">
    <ItemTemplate>
        <br /><h33><%# DataBinder.Eval(Container.DataItem, "Name")%></h33>
        <asp:GridView ID="gvProjects" runat="server" AllowPaging="False" 
            AllowSorting="False" AutoGenerateColumns="False" OnRowDataBound="gvCustomers_RowDataBound"
            BorderColor="#D7D7D7" BorderStyle="None" BorderWidth="1px" CellPadding="10" 
            EmptyDataText="&lt;table width=&quot;100%&quot;&gt;&lt;tr&gt;&lt;td align=&quot;center&quot; width=&quot;90%&quot;&gt;No Record Found&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;" 
            Width="100%">
            <Columns>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="center" width="60" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lbImageView" runat="server" onclick="btnView_Click" CommandArgument='<%# m_Cryption.encrypt(Eval("ProjectID", "{0}"),m_Cryption.cryptionKey) %>'><img src='<%# showFile(Eval("Identifier", "{0}"), Eval("PersonalPhoto", "{0}")) %>' style='<%# isFileExisted(Eval("Identifier", "{0}"), Eval("PersonalPhoto", "{0}")) %>' width="50" border="0"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NAME" SortExpression="Address">
                        <ItemStyle HorizontalAlign="Left" Width="150" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lbNameView" runat="server" CommandArgument='<%# m_Cryption.encrypt(Eval("ProjectID", "{0}"),m_Cryption.cryptionKey) %>' onclick="btnView_Click"><asp:Label ID="lblName" runat="server"><%# Eval("Name", "{0}")%></asp:Label></asp:LinkButton>                
                            <asp:TextBox ID="tbProjectID" runat="server" Text='<%# m_Cryption.encrypt(Eval("ProjectID", "{0}"),m_Cryption.cryptionKey) %>' Visible="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="NOTES">
                        <ItemTemplate>
                            <asp:Label ID="lblNotes" runat="server" Visible="false"><%# DisplayNotes(Eval("ProjectId", "{0}"), Eval("ProjectStatusId", "{0}"), Eval("ScopeDate", "{0}"), Eval("StartDate", "{0}"), Eval("AssessmentDate", "{0}"), Eval("Hazard", "{0}"))%></asp:Label>                
                            <asp:Label ID="lblUserNotes" runat="server"><%# DisplayNotes(Eval("ProjectId", "{0}"), Eval("ScopeDate", "{0}"), Eval("StartDate", "{0}"), Eval("AssessmentDate", "{0}"), Eval("Hazard", "{0}"))%></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Font-Size="Smaller" width="550px" />            
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PRIORITY">
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                        <ItemTemplate>
                            <table border="0"><tr><td>
                                <asp:ImageButton ID="imgGroupRatingClear" runat="server" CommandArgument='<%# m_Cryption.encrypt(Eval("ProjectID", "{0}"),m_Cryption.cryptionKey) %>'
                                        ImageUrl='<%# showClearImage(Eval("Priority", "{0}")) %>' onclick="imgPropertyRatingClear_Click" ToolTip="0" />
                            </td><td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Rating ID="GroupPropertyRating" runat="server"
                                            CurrentRating='<%# DisplayRating(Eval("Priority", "{0}")) %>'
                                            MaxRating="5"
                                            StarCssClass="ratingStar"
                                            WaitingStarCssClass="savedRatingStar"
                                            FilledStarCssClass="filledRatingStar"
                                            EmptyStarCssClass="emptyRatingStar"
                                            OnChanged="PropertyRating_Changed" Width="150px" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td></tr></table>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="STATUS">
                        <ItemStyle HorizontalAlign="Center" Width="160" />
                        <ItemTemplate>
                            <table border="0" cellpadding="4" cellspacing="4">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlProjectStatus" runat="server" OnSelectedIndexChanged="ddlProjectStatus_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>        
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPropertyStatus" runat="server" OnSelectedIndexChanged="ddlPropertyStatus_SelectedIndexChanged" AutoPostBack="true" Visible="false"></asp:DropDownList>        
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="imgArchive" runat="server" Width="20" UseSubmitBehavior="false"
                                            ImageUrl="../images/archive.png" onclick="imgArchive_Click" OnClientClick="return confirm('Are you sure you want to archive this project?');" Visible='<%# ShowArchive() %>' />
                                        <asp:ImageButton ID="imgunArchive" runat="server" Width="20" UseSubmitBehavior="false"
                                            ImageUrl="../images/unarchive.png" onclick="imgunArchive_Click" OnClientClick="return confirm('Are you sure you want to unarchive this project?');" Visible='<%# ShowunArchive() %>' />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ContactName" HeaderText="CONTACT" 
                        SortExpression="ContactName" ItemStyle-Width="140" Visible="false">
            <ItemStyle Width="140px"></ItemStyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="ACTION" Visible="false">
                        <ItemStyle HorizontalAlign="Center" Width="80" />
                        <ItemTemplate>
                            <asp:LinkButton ID="btnView" runat="server" 
                                CommandArgument='<%# m_Cryption.encrypt(Eval("ProjectID", "{0}"),m_Cryption.cryptionKey) %>' 
                                onclick="btnView_Click" Visible="false">VIEW</asp:LinkButton>                
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
<asp:GridView ID="gvProjects" runat="server" AllowPaging="True" 
    AllowSorting="True" AutoGenerateColumns="False"
    BorderColor="#D7D7D7" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
    EmptyDataText="&lt;table width=&quot;100%&quot;&gt;&lt;tr&gt;&lt;td align=&quot;center&quot; width=&quot;90%&quot;&gt;No Record Found&lt;/td&gt;&lt;/tr&gt;&lt;/table&gt;" 
    Width="100%">
    <Columns>
        <asp:TemplateField>
            <ItemStyle HorizontalAlign="center" width="60" />
            <ItemTemplate>
                <asp:LinkButton ID="lbImageView" runat="server" onclick="btnView_Click" CommandArgument='<%# m_Cryption.encrypt(Eval("ProjectID", "{0}"),m_Cryption.cryptionKey) %>'><img src='<%# showFile(Eval("Identifier", "{0}"), Eval("PersonalPhoto", "{0}")) %>' style='<%# isFileExisted(Eval("Identifier", "{0}"), Eval("PersonalPhoto", "{0}")) %>' width="50" border="0"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="NAME" SortExpression="Address">
            <ItemStyle HorizontalAlign="Left" Width="180" />
            <ItemTemplate>
                <asp:LinkButton ID="lbNameView" runat="server" CommandArgument='<%# m_Cryption.encrypt(Eval("ProjectID", "{0}"),m_Cryption.cryptionKey) %>' onclick="btnView_Click"><asp:Label ID="lblName" runat="server"><%# Eval("Name", "{0}")%></asp:Label></asp:LinkButton>                
                <asp:TextBox ID="tbProjectID" runat="server" Text='<%# m_Cryption.encrypt(Eval("ProjectID", "{0}"),m_Cryption.cryptionKey) %>' Visible="false"></asp:TextBox>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="NOTES">
            <ItemTemplate>
                <asp:Label ID="lblNotes" runat="server" Visible="false"><%# DisplayNotes(Eval("ProjectId", "{0}"), Eval("ProjectStatusId", "{0}"), Eval("ScopeDate", "{0}"), Eval("StartDate", "{0}"), Eval("AssessmentDate", "{0}"), Eval("Hazard", "{0}"))%></asp:Label>                
                <asp:Label ID="lblUserNotes" runat="server"><%# DisplayNotes(Eval("ProjectId", "{0}"), Eval("ScopeDate", "{0}"), Eval("StartDate", "{0}"), Eval("AssessmentDate", "{0}"), Eval("Hazard", "{0}"))%></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Font-Size="Smaller" width="500px" />            
        </asp:TemplateField>
        <asp:TemplateField HeaderText="PRIORITY">
            <ItemStyle HorizontalAlign="Center" Width="100px" />
            <ItemTemplate>
                <table border="0"><tr><td>
                    <asp:ImageButton ID="imgPropertyRatingClear" runat="server" CommandArgument='<%# m_Cryption.encrypt(Eval("ProjectID", "{0}"),m_Cryption.cryptionKey) %>'
                            ImageUrl='<%# showClearImage(Eval("Priority", "{0}")) %>' onclick="imgPropertyRatingClear_Click" ToolTip="0" />
                </td><td>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Rating ID="PropertyRating" runat="server"
                                CurrentRating='<%# DisplayRating(Eval("Priority", "{0}")) %>'
                                MaxRating="5"
                                StarCssClass="ratingStar"
                                WaitingStarCssClass="savedRatingStar"
                                FilledStarCssClass="filledRatingStar"
                                EmptyStarCssClass="emptyRatingStar"
                                OnChanged="PropertyRating_Changed" Width="150px" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td></tr></table>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="STATUS">
            <ItemStyle HorizontalAlign="Center" Width="160" />
            <ItemTemplate>
                <table border="0" cellpadding="4" cellspacing="4">
                    <tr>
                        <td>
                            <asp:DropDownList ID="ddlProjectStatus" runat="server" OnSelectedIndexChanged="ddlProjectStatus_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>        
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPropertyStatus" runat="server" OnSelectedIndexChanged="ddlPropertyStatus_SelectedIndexChanged" AutoPostBack="true" Visible="false"></asp:DropDownList>        
                        </td>
                        <td>
                            <asp:ImageButton ID="imgArchive" runat="server" Width="20" UseSubmitBehavior="false"
                                ImageUrl="../images/archive.png" onclick="imgArchive_Click" OnClientClick="return confirm('Are you sure you want to archive this project?');" Visible='<%# ShowArchive() %>' />
                            <asp:ImageButton ID="imgunArchive" runat="server" Width="20" UseSubmitBehavior="false"
                                ImageUrl="../images/unarchive.png" onclick="imgunArchive_Click" OnClientClick="return confirm('Are you sure you want to unarchive this project?');" Visible='<%# ShowunArchive() %>' />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="ContactName" HeaderText="CONTACT" 
            SortExpression="ContactName" ItemStyle-Width="140" Visible="false">
<ItemStyle Width="140px"></ItemStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="ACTION" Visible="false">
            <ItemStyle HorizontalAlign="Center" Width="80" />
            <ItemTemplate>
                <asp:LinkButton ID="btnView" runat="server" 
                    CommandArgument='<%# m_Cryption.encrypt(Eval("ProjectID", "{0}"),m_Cryption.cryptionKey) %>' 
                    onclick="btnView_Click" Visible="false">VIEW</asp:LinkButton>                
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