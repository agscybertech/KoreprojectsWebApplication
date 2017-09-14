<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/WebSitePagesMaster.master" CodeFile="signup.aspx.cs" Inherits="signup" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div id="main_wrapper_inner">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="running_banner row-fluid">
        <div class="container">
            <div class="banner_text">
                <h1>TRY KORE PROJECTS</h1>
                <img src="images/banner-demo.jpg" alt="" class="running_ban_img" />
            </div>
            <!--banner_text-->

            <h2 class="text-center col-lg-12 col-md-12 col-sm-12">Click <a href="demo.aspx">HERE </a>to try our DEMOS or create your own new login below.
            </h2>
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div class="col-lg-6 col-md-6 col-sm-6">
                    <h3>Interested to see how Kore Projects can transform your company? Try for free today and start reaping the benefits of the most easy to use Project Management System on the market.</h3>
                    <p>Simply create your own login and get full access to the Kore Projects system. You will get a 30 Days Free Trial so you can try out all the features.</p>

                    <p>We hope you enjoy using Kore Projects. Contact us anytime if you have any questions. </p>
                </div>

                <div class="col-lg-6 col-md-6 col-sm-6">
                    <iframe src="signup_remote.aspx" scrolling="No"
                        marginwidth="0" marginheight="0" align="MIDDLE"
                        frameborder="No" allowtransparency="true" width="100%" height="390"></iframe>
                </div>


            </div>

        </div>
    </div>
</asp:Content>
