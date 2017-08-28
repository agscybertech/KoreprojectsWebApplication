<%@ Page Title="" Language="VB" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="false" CodeFile="KoreMobile.aspx.vb" Inherits="KoreMobile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="header_inner">
        <h1 style="float: left">Kore Mobile View</h1>
        <div style="float: right; padding-top: 50px;">
            <asp:HyperLink ID="hplAndroid" CssClass="applink" runat="server" NavigateUrl="~/KoreAndroid.aspx" Style="margin-left: 20px; padding: 0px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif; font-size: 25px; color: #ffffff; text-transform: uppercase; text-shadow: #000000 1px 0px 0px; text-decoration: none">Kore android app</asp:HyperLink>
            <asp:HyperLink ID="hlpIos" CssClass="applink" runat="server" NavigateUrl="~/KoreIOS.aspx" Style="margin-left: 20px; padding: 0px; font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif; font-size: 25px; color: #ffffff; text-transform: uppercase; text-shadow: #000000 1px 0px 0px; text-decoration: none">Kore ios app</asp:HyperLink>
        </div>
    </div>
    <div class="content_inner" style="font-family: MyriadPro-BoldCond">
        <table>
            <tr>
                <td style="vertical-align: top; width: 100%; padding-right: 10px">
                    <h3>Dashboard</h3>
                    <p>Our Kore Mobile View can be used on IOS, Android and windows phone.</p>
                    <br />
                    <p>The dashboard is easy to use and understand. Everything on Kore Mobile is in real time so all information/photos/re orders are sent up to the top and received straight away.</p>
                    <br />
                    <p>The Mobile view is used by your staff members on the ground. They can also receive messages that come through the dashboard from the owners and staff members in the office. The dashboard lets you navigate between filling in timesheets, sending photos to the office, sending in re order forms and letting you be assigned to specific jobs.</p>
                    <br />
                    <br />
                    <h3>Jobs</h3>
                    <p>Jobs allow you to be registered to a job by the people up in the office/mangers.</p>
                    <br />
                    <p>When you are registered onto the job you can see all job details and information and notes that have been entered about that job. When you have seen the information on the job and the notes soon after being registered your staff on the ground can update the status about that job relating to the notes. I.E not started, in progress, on hold and completed. Sending all the information back to the office all in real life time.</p>
                    <br />
                    <br />
                    <h3>Time Sheets</h3>
                    <p>Timesheets are one of our many top features within Kore Projects. Timesheets are an easy alternative than paper trail. With timesheets you can fill them out daily or at the end of the week but we suggest you do it daily as the timesheets come off the price of the job and can be tracked each day.</p>
                    <br />
                    <p>Time sheets are simple to follow and fill out and also give you a section to fill in your description so your manger/owner can see what you have done for the day, Timesheets are sent in real time back to the office. Even though your timesheets are saved through kore online you can also print them out and keep them as a paper trail.</p>
                </td>
                <td>
                    <img src="Images/MobileDashboard.png" style="vertical-align: middle" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table>
            <tr>
                <td style="vertical-align: top; width: 100%; padding-right: 10px">
                    <h3>Photo Upload</h3>
                    <p>Photos are a quick and simple way to keep your manger/owner informed with the progress on site. Uploading photos from the staff on the ground is an easy way to show touch ups/problems/fixes and also allowing you to type a description so you know what the photo is about. Photos on the job can be archived into folders daily so you can always keep track of your photos on the job you are registered to.</p>
                    <br />
                    <br />
                    <h3>Reorder</h3>
                    <p>orders are another top feature of ours allowing your staff members on the ground send through re order forms in real time to that job. It’s easy and simple to follow, just fill in the re order box and send it through. It will come up on the job you are registered to so the amount of products can be tracked throughout the whole job allowing an easy flow of communication for materials when needed on the job. Also the prices of the products can be taken off the price list of the job so you can track where all your money is being spent and how much is being spent on products for that job.</p>
                </td>
                <td>
                    <img src="Images/MobileTimesheets.png" style="vertical-align: middle" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="scriptContent" runat="Server">
</asp:Content>

