<%@ Page Title="" Language="VB" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="false" CodeFile="KoreIOS.aspx.vb" Inherits="KoreIOS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="header_inner">
        <h1 style="float: left">Kore IOS App</h1>
        <div style="float: right; padding-top: 50px;">
            <asp:HyperLink ID="hplMobileView" CssClass="applink" runat="server" NavigateUrl="~/KoreMobile.aspx" Style="margin: 0px; padding: 0px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif; font-size: 25px; color: #ffffff; text-transform: uppercase; text-shadow: #000000 1px 0px 0px; text-decoration:none">Kore mobile view</asp:HyperLink>
            <asp:HyperLink ID="hplAndroid" CssClass="applink" runat="server" NavigateUrl="~/KoreAndroid.aspx" Style="margin-left: 20px; padding: 0px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif; font-size: 25px; color: #ffffff; text-transform: uppercase; text-shadow: #000000 1px 0px 0px; text-decoration:none">Kore android app</asp:HyperLink>
        </div>
    </div>
    <div class="content_inner content_blocks and_contents">
        <table>
            <tr>
                <td style="vertical-align: top; width: 100%;padding-right: 10px">
                    <h2>Dashboard</h2>
                    <p>Our Kore Android App is very easy to use and navigate between scoping jobs, your projects, contacts and your user settings.</p>
                    <br />
                    <p>Our Android app is designed so you can go out onto a job and be able to scope a job then and there on your phone and it saves into your scopes page online back in your office. </p>
                    <br/>
                    <p>So scoping can be done in real time and saved through kore so you can go back to the office and finish the scoping as you please.  </p>
                    <br/>
                    <p>Only staff in office are able to use the android app.</p>
                    <br />
                    <br />
                </td>
                <td style="vertical-align: top;">
                    <img src="Images/AndroidDashboard.png" style="vertical-align: middle" />
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td style="vertical-align: top; width: 100%; padding-right: 10px">
                    <h2 class="project_sc_and">Project Scope</h2>
                    <p>Our Project scope feature is easy for any office user and any manger/owner. </p>
                    <br/>
                    <p>Our Project Scopes feature allows you to go on site and have the ability to scope a job from your phone sending it back to the kore project on your computer where you can go back and fill in the rest of the minor details including your pricing.</p>
                    <br/>
                    <p>This makes scoping jobs easier and faster and eliminates paper trails. </p>
                    <br/>
                    <p>Being able to scope jobs on your phone on the go allows you to be out on the road scoping jobs all day, It’s quick, and easy.</p>
                </td>
                <td style="vertical-align: top;">
                    <img src="Images/AndroidProjectScope.png" style="vertical-align: middle" />
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td style="vertical-align: top; width: 100%; padding-right: 10px">
                    <h2 class="project_pg_and">Projects Page</h2>
                    <p>Just like our scopes page our projects page is easy to use and work. This allows you to edit/add/delete projects from your system through kore. </p>
                    <br/>
                    <p>When these are added or edited they will instantly change on your phone and your system keeping everyone up to speed. </p>
                    <br/>
                    <p>When viewing your project In projects page it gives you all the details about that specific project. </p>
                    <br/>
                    <p>So you can <strong>view/edit/add</strong>, on the go and not have to be in the office. Changes can be made anywhere on the road anytime.</p>
                </td>
                <td style="vertical-align: top;">
                    <img src="Images/AndroidAddProject.png" style="vertical-align: middle" />
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td style="vertical-align: top; width: 100%;padding-right: 10px">
                    <h2 class="setting_and">Settings</h2>
                    <p>Our settings page allows you to set up all your contacts, your worksheets and your service rates and the initial prerequisites for your projects. </p>
                    <br />
                    <p>So when you are going around scoping jobs on the go, adding or editing projects everything is there ready to insert. </p>
                    <br />
                    <p>Or if you are out on the job and forgot to add something in regarding worksheets or pricings, you can change it on the go as you please without having to travel back to the office to change it all</p>
                    <br />
                    <p><h4>Worksheet Setup</h4></p>
                    <p>In Worksheet Setup you fill out all your information regarding your worksheets</p>
                    <br />
                    <p><h4>Service Rates</h4></p>
                    <p>This is where you set up your service rate sheets for your work provided. Add a new Rate sheet by clicking the plus button in the top right corner</p>
                    <br />
                    <p><h4>Project Setup</h4></p>
                    <p>In Project Setup you fill out the boxes provided to set up your selections for your projects</p>
                </td>
                <td style="vertical-align: top;">
                    <img src="Images/android-setting.png" style="vertical-align: middle" />
                </td>
            </tr>
        </table>
        <br />
        <br />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="scriptContent" Runat="Server">
</asp:Content>

