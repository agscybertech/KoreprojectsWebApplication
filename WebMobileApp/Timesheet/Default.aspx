<%@ Page Title="" Language="VB" MasterPageFile="~/Timesheet/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Timesheet_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div align="center" width="100%">
        <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label><br />
        <asp:Repeater ID="rptTimeSheet" runat="server">
            <ItemTemplate>
                <div align="left">
                    <h2><a name='Date-<%# DataBinder.Eval(Container.DataItem, "DT", "{0:d-MMM-yyyy}")%>' /><%# DataBinder.Eval(Container.DataItem, "DT", "{0:d MMM yyyy}")%></h2>
                    <%# DataBinder.Eval(Container.DataItem, "DT", "{0:ddd}")%>
                    <%# showDailyHours(DataBinder.Eval(Container.DataItem, "HRS").ToString())%>
                    <asp:LinkButton ID="LBEdit" runat="server" OnClick="SelectDate_Click" CommandArgument='<%# Eval("TimeSheetEntryID", "{0}") & "," & DataBinder.Eval(Container.DataItem, "DT", "{0:yyyy-MM-dd}") %>'>Edit</asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LBEdit_SimpleDDL" runat="server" OnClick="SelectDate_SimpleDDL_Click" CommandArgument='<%# Eval("TimeSheetEntryID", "{0}") & "," & DataBinder.Eval(Container.DataItem, "DT", "{0:yyyy-MM-dd}") %>'>Edit_SimpleDDL</asp:LinkButton>
                    &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="LBEdit_DDL" runat="server" OnClick="SelectDate_DDL_Click" CommandArgument='<%# Eval("TimeSheetEntryID", "{0}") & "," & DataBinder.Eval(Container.DataItem, "DT", "{0:yyyy-MM-dd}") %>'>Edit_DDL</asp:LinkButton>
                </div>
            </ItemTemplate>
        </asp:Repeater><br />
        <asp:Label ID="lblTHour" runat="server"></asp:Label>
    </div>
</asp:Content>

