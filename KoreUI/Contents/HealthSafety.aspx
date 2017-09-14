<%@ Page Title="Health and Safety" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="true" CodeFile="HealthSafety.aspx.vb" Inherits="Contents_HealthSafety" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <asp:Label ID="lblPrintScript" runat="server" Text=""></asp:Label>
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text="Health & Safety"></asp:Label></h1>
            <a style="padding:8px;float:right;" id="help" href="https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
            <a style="padding:8px;float:right;" id="manageHealthSafety" href="HealthSafetyManager.aspx" title="Manage Health Safety"><img src="../images/add-button.png" width="40" height="40" border="0" alt="Manage Health Safety" /></a>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <div align="center" width="100%">
            <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
        </div>

        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td>                    
                    <asp:Repeater ID="rptDynamicPageContents" runat="server">
                    </asp:Repeater>                    
                </td>
            </tr>           
            <tr style="display:none;">
                <td>                    
                    <asp:Repeater ID="rptDynamicPageContentsNotFile" runat="server">
                    </asp:Repeater>                    
                </td>
            </tr>
            <tr style="display:none;">
                <td>     
                    <asp:Repeater id="rptDynamicPageContentsFile" runat="server">
                        <HeaderTemplate>
                            <table border="0" cellspacing="5" cellpadding="5" <%=ShowWidth() %>>                
                        </HeaderTemplate>
                        <ItemTemplate>
                                <asp:Literal ID="litItem" runat="server" OnDataBinding="litItem_DataBinding" />
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>                                                   
                </td>
            </tr>
        </table>

    </div>
    <!-- content -->
</asp:Content>