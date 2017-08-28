<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TradeNotes.aspx.vb" Inherits="Projects_TradeNotes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <h1 id="appointment">TRADE NOTES</h1>
    <table width="800px" runat="server" id="tblProjectNote">
        <tr>
            <td>
                <asp:Repeater ID="noteRepeater" runat="server">
                    <ItemTemplate>
                        <div>
                            <table width="90%" cellpadding="10">
                                <tr>
                                    <td align="left" valign="top" style="width:220px;">
                                        <div style="font-size:16px; font-weight:bold;"><%# GetUserProjectStatusName(Eval("ProjectStatusId", "{0}")) %></div>        
                                    </td>
                                    <td align="left" valign="top">
                                        <div style="font-size:16px"><%# DataBinder.Eval(Container.DataItem, "NoteContent")%></div>
                                    </td>
                                    <td align="left" valign="top">
                                        <div style="font-size:16px"><%# DataBinder.Eval(Container.DataItem, "Description")%></div>            
                                    </td>                                    
                                </tr>
                            </table>
                            <br />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>                
            </td>
        </tr>
    </table>
    <table width="800px" runat="server" id="tblScopeDescription">
        <tr>
            <td>
                <asp:Repeater ID="scopeDescriptionRepeater" runat="server">
                    <ItemTemplate>
                        <div>
                            <table width="90%" cellpadding="10">
                                <tr>
                                    <td align="left" valign="top" style="width:220px;">
                                        <div style="font-size:16px; font-weight:bold;"><%# DataBinder.Eval(Container.DataItem, "Area")%> <%# DataBinder.Eval(Container.DataItem, "Item")%><br /><%# DataBinder.Eval(Container.DataItem, "Service")%></div>        
                                    </td>
                                    <td align="left" valign="top">
                                        <div style="font-size:16px"><%# DataBinder.Eval(Container.DataItem, "Description")%></div>            
                                    </td>                                    
                                </tr>
                            </table>
                            <br />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>                
            </td>
        </tr>
    </table>
    <div id="divNotes" runat="server" width="800px" style="display:none;">
    
    </div>
    </form>
</body>
</html>
