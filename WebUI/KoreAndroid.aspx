<%@ Page Title="" Language="VB" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="false" CodeFile="KoreAndroid.aspx.vb" Inherits="KoreAndroid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="header_inner">
        <h1 style="float: left">Kore Android App</h1>
        <div style="float: right; padding-top: 50px;">
            <asp:HyperLink ID="hplMobileView" CssClass="applink" runat="server" NavigateUrl="~/KoreMobile.aspx" Style="margin: 0px; padding: 0px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif; font-size: 25px; color: #ffffff; text-transform: uppercase; text-shadow: #000000 1px 0px 0px; text-decoration:none">Kore mobile view</asp:HyperLink>
            <asp:HyperLink ID="hlpIos" CssClass="applink" runat="server" NavigateUrl="~/KoreIOS.aspx" Style="margin-left: 20px; padding: 0px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif; font-size: 25px; color: #ffffff; text-transform: uppercase; text-shadow: #000000 1px 0px 0px; text-decoration:none">Kore ios app</asp:HyperLink>
        </div>
    </div>
    <div class="content_inner">
        <table>
            <tr>
                <td>
                    <img src="Images/AndroidDashboard.png" style="vertical-align: middle" />
                </td>
                <td style="vertical-align: top; width: 100%; padding-left: 10px">
                    <h3>Dashboard</h3>
                    The moment that a stage of the project is complete, a staff member can update the status. Every team member instantly sees the update so everyone stays informed and ahead of the game.<br />
                    <br />
                    <br />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table>
            <tr>
                <td style="vertical-align: top; width: 100%; padding-right: 10px">
                    <h3>Project Scope</h3>
                    The moment that a stage of the project is complete, a staff member can update the status. Every team member instantly sees the update so everyone stays informed and ahead of the game.<br />
                    <br />
                    <br />
                </td>
                <td>
                    <img src="Images/AndroidProjectScope.png" style="vertical-align: middle" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <img src="Images/AndroidAddProject.png" style="vertical-align: middle" />
                </td>
                <td style="vertical-align: top; width: 100%; padding-left: 10px">
                    <h3>Project</h3>
                    The moment that a stage of the project is complete, a staff member can update the status. Every team member instantly sees the update so everyone stays informed and ahead of the game.<br />
                    <br />
                    <br />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table>
            <tr>
                <td style="vertical-align: top; width: 100%; padding-right: 10px">
                    <h3>Settings</h3>
                    The moment that a stage of the project is complete, a staff member can update the status. Every team member instantly sees the update so everyone stays informed and ahead of the game.<br />
                    <br />
                    <br />
                </td>
                <td>
                    <img src="Images/AndroidSettings.png" style="vertical-align: middle" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="scriptContent" Runat="Server">
</asp:Content>

