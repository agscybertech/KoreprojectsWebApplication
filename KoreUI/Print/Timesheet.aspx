<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Timesheet.aspx.vb" Inherits="Contacts_Timesheet" Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.4.1.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <script src="../Scripts/jquery-ui-timepicker-addon.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
        
        <asp:Label ID="lblCdate" runat="server" Font-Bold="true"></asp:Label>
        <br /><br />
        <table>
            <tr>
                <td style="text-align:right; font-weight:bold">Work Start:</td>
                <td>
                    <asp:TextBox class="datetimepicker" ID="tbxWorkStart" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align:right; font-weight:bold">Lunch Start:</td>
                <td>
                    <asp:TextBox class="datetimepicker" ID="tbxLunchStart" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align:right; font-weight:bold">Lunch End:</td>
                <td>
                    <asp:TextBox class="datetimepicker" ID="tbxLunchEnd" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align:right; font-weight:bold">Work End:</td>
                <td>
                    <asp:TextBox class="datetimepicker" ID="tbxWorkEnd" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align:right; font-weight:bold">Total Hours:
                </td>
                <td>
                    <asp:Label ID="lblThour" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
