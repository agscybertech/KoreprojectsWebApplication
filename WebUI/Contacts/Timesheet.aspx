<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Timesheet.aspx.vb" Inherits="Contacts_Timesheet" Debug="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblCdate" runat="server" Font-Bold="true"></asp:Label>
        <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label><br />
        Work Start
        <asp:TextBox class="datetimepicker" ID="tbxWorkStart" runat="server"></asp:TextBox><br />
        Lunch Start
        <asp:TextBox class="datetimepicker" ID="tbxLunchStart" runat="server"></asp:TextBox><br />
        Lunch End
        <asp:TextBox class="datetimepicker" ID="tbxLunchEnd" runat="server"></asp:TextBox><br />
        Work End
        <asp:TextBox class="datetimepicker" ID="tbxWorkEnd" runat="server"></asp:TextBox><br />
        <asp:Label ID="lblThour" runat="server"></asp:Label><br />
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
        <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
    </div>
    </form>
</body>
</html>
