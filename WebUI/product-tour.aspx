<%@ Page Language="C#" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="true" CodeFile="product-tour.aspx.cs" Inherits="product_tour" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div id="main_wrapper_inner">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header_inner">
        <h1 style="float: left">PRODUCT TOUR</h1>

        <div style="float: right; padding-top: 50px;">
            <asp:HyperLink ID="hplMobileView" CssClass="applink" runat="server" NavigateUrl="~/KoreMobile.aspx" Style="margin: 0px; padding: 0px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif; font-size: 25px; color: #ffffff; text-transform: uppercase; text-shadow: #000000 1px 0px 0px; text-decoration:none">Kore mobile view</asp:HyperLink>
            <asp:HyperLink ID="hplAndroid" CssClass="applink" runat="server" NavigateUrl="~/KoreAndroid.aspx" Style="margin-left: 20px; padding: 0px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif; font-size: 25px; color: #ffffff; text-transform: uppercase; text-shadow: #000000 1px 0px 0px; text-decoration:none">Kore android app</asp:HyperLink>
            <asp:HyperLink ID="hlpIos" CssClass="applink" runat="server" NavigateUrl="~/KoreIOS.aspx" Style="margin-left: 20px; padding: 0px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif; font-size: 25px; color: #ffffff; text-transform: uppercase; text-shadow: #000000 1px 0px 0px; text-decoration:none">Kore ios app</asp:HyperLink>
        </div>
    </div>
    <div class="content_inner">
        <!-- div class="inner_pic_wrapper"><img src="images/inner_pic.png" width="300" height="153" alt="" /></div -->

        <table border="0" cellpadding="0" cellspacing="0" width="100%">

            <tr>
                <td>
                    <h2>Kore Projects is the easy to use Project Management system designed specifically for small to medium sized businesses.</h2>
                    We combine the organizational benefits of other project management systems with a back to basics approach that will streamline your operations in hours, making sure that you get the absolute most out of your working week. Take our Product Tour to see how simple it can be to maximize your productivity by integrating Kore Projects software into your business.<br />
                    <br />

                    <h2>Projects Page</h2>
                    <br />
                    <h3>
                        <img width="450" vspace="20" hspace="20" height="402" border="0" align="right" src="ebiz-manager/images/infopages/ScreenShot1.png" alt="" />Oversee in Seconds</h3>
                    All your current and upcoming Projects in one, easy to view list.
                    <br />
                    <br />
                    <h2></h2>
                    <h3>Visibly Prioritize</h3>
                    Easy to use star ranking allows you and staff members to ensure vital jobs get done first.<br />
                    <br />
                    <br />
                    <h3>Up to the Minute Project Updates</h3>
                    The moment that a stage of the project is complete, a staff member can update the status. Every team member instantly sees the update so everyone stays informed and ahead of the game.<br />
                    <br />
                    <br />
                    <h3>Secure Archives</h3>
                    All previous jobs and invoices archived in one, easy to access location.<br />
                    <br />
                    <br />
                    <h3>Create New Projects Easily</h3>
                    Fill in the details of your every new project and they will be there for all your team to see.<br />
                    <br />
                    <h2></h2>
                    <h3>Projects in More Detail</h3>
                    Contact Information, job notes, files and photographs are all easy to upload to your Job Details page, where they can be accessed by all team members.
                    <br />
                    <br />
                    <br />
                    <h3>Invoice and Quote in Seconds</h3>
                    Add all the items of a new job and the cost will be calculated for you. Edit, remove or update items easily, and print paper copies of invoices or quotes if required.<span style="font-size: 11pt;"><strong><br />
                    </strong></span>
                    <br />
                    <br />
                    <br />
                    <img width="449" vspace="20" hspace="20" height="600" border="0" align="right" alt="" src="ebiz-manager/images/infopages/ScreenShot2.png" />
                    <h2><strong>
                        <br />
                    </strong>Scopes</h2>
                    <br />
                    <h3>Planning made Simple</h3>
                    Keep all your project scopes in one place. Add details, quotes and files as they arise until a scope becomes a new job.
                    <br />
                    <br />
                    <br />
                    <br />
                    <h2>
                        <br />
                        Contacts</h2>
                    <br />
                    <h3>Never Lose a Number Again</h3>
                    All your contacts in one place. Categorize your staff, contractors, clients and suppliers for convenience.<span style="font-size: 11pt;"><br />
                    </span>
                    <br />
                    <br />
                    <h3>Sharper Communication</h3>
                    Invite contacts to one or more jobs and allow them access to all current project information. Information flows effortlessly amongst team members and no time is wasted.<br />
                    <br />
                    <br />
                    <br />
                    <h2>
                        <br />
                        Settings</h2>
                    <br />
                    <h3>Customize your System</h3>
                    Input data relating to your company&rsquo;s services, costs and common job statuses. Once customized your system will allow you to carry out routine tasks such as invoicing, job status updates or worksheet creation in minimal time.<br />
                    <span style="font-size: 11pt;"></span>
                    <br />
                    <br />
                </td>
            </tr>

        </table>

    </div>


</asp:Content>
