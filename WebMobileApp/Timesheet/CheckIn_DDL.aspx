<%@ Page Title="" Language="VB" MasterPageFile="~/Timesheet/MasterPage.master" AutoEventWireup="false" CodeFile="CheckIn_DDL.aspx.vb" Inherits="Timesheet_CheckIn_DDL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link rel="stylesheet" href="../Styles/jquery-ui.css" type="text/css" media="all" />
    <link rel="stylesheet" href="../Styles/style.css" type="text/css" media="all" />
    <link rel="stylesheet" href="../Styles/mobiscroll.css" type="text/css" media="all" />
    <script src="../Scripts/jquery.min.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <script src="../Scripts/jquery-ui-timepicker-addon.js"></script>
    <script src="../Scripts/mobiscroll.js"></script>
    <script src="../Scripts/application.js"></script>

    <div align="center" width="100%">
        <asp:Label ID="lblCdate" runat="server" Font-Bold="true"></asp:Label>
        <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label><br />
        Work Start
        <asp:DropDownList ID="ddlHour" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlMins" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlAP" runat="server">
            <asp:ListItem Text="am" Value="am"></asp:ListItem>
            <asp:ListItem Text="pm" Value="pm"></asp:ListItem>
        </asp:DropDownList><br />
        Lunch Start
        <asp:DropDownList ID="ddlHour1" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlMins1" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlAP1" runat="server">
            <asp:ListItem Text="am" Value="am"></asp:ListItem>
            <asp:ListItem Text="pm" Value="pm"></asp:ListItem>
        </asp:DropDownList><br />
        Lunch End
        <asp:DropDownList ID="ddlHour2" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlMins2" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlAP2" runat="server">
            <asp:ListItem Text="am" Value="am"></asp:ListItem>
            <asp:ListItem Text="pm" Value="pm"></asp:ListItem>
        </asp:DropDownList><br />
        Work End
        <asp:DropDownList ID="ddlHour3" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlMins3" runat="server">
        </asp:DropDownList>
        <asp:DropDownList ID="ddlAP3" runat="server">
            <asp:ListItem Text="am" Value="am"></asp:ListItem>
            <asp:ListItem Text="pm" Value="pm"></asp:ListItem>
        </asp:DropDownList><br />
        <asp:Label ID="lblThour" runat="server"></asp:Label><br />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
    </div>
</asp:Content>

