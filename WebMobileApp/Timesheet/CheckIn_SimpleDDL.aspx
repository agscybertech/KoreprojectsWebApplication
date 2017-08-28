<%@ Page Title="" Language="VB" MasterPageFile="~/Timesheet/MasterPage.master" AutoEventWireup="false" CodeFile="CheckIn_SimpleDDL.aspx.vb" Inherits="Timesheet_CheckIn_SimpleDDL" %>

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
    <script>
        function setTargetTime(tValue, targer1, targer2, targer3) {
            if (targer1 != '') {
                //if (document.getElementById(targer1).value == '') {
                document.getElementById(targer1).value = tValue;
                //}
            }
            if (targer2 != '') {
                //if (document.getElementById(targer2).value == '') {
                document.getElementById(targer2).value = tValue;
                //}
            }
            if (targer3 != '') {
                //if (document.getElementById(targer3).value == '') {
                document.getElementById(targer3).value = tValue;
                //}
            }
        }
    </script>
    <div align="center" width="100%">
        <asp:Label ID="lblCdate" runat="server" Font-Bold="true"></asp:Label>
        <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label><br />
        Work Start
        <asp:DropDownList ID="DropDownList1" runat="server">
        </asp:DropDownList><br />
        Lunch Start
        <asp:DropDownList ID="DropDownList2" runat="server">
        </asp:DropDownList><br />
        Lunch End
        <asp:DropDownList ID="DropDownList3" runat="server">
        </asp:DropDownList><br />
        Work End
        <asp:DropDownList ID="DropDownList4" runat="server">
        </asp:DropDownList><br />
        <asp:Label ID="lblThour" runat="server"></asp:Label><br />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
    </div>
</asp:Content>

