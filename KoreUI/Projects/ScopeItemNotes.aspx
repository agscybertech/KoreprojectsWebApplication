<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ScopeItemNotes.aspx.vb" Inherits="Projects_ScopeItemNotes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 99px;
        }
        .style2
        {
            width: 100px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <h1 id="appointment">NOTES</h1>    
    <table width="800px">
        <tr>
            <td class="style1">
                <div style="font-size: 16px; font-weight: bold;">Assign To:</div>
            </td>
            <td>
                <asp:Label ID="lblAssignTo" runat="server" Font-Size="16px"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table width="800px">
        <tr>
            <td class="style1">
                <div style="font-size: 16px; font-weight: bold;">Service:</div>
            </td>
            <td>
                <asp:Label ID="lblService" runat="server" Font-Size="16px"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table width="800px">
        <tr>
            <td class="style2">
                <div style="font-size: 16px; font-weight: bold;">Notes:</div>
            </td>
            <td>
                <asp:Label ID="lblNotes" runat="server" Font-Size="16px"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    </form>
</body>
</html>