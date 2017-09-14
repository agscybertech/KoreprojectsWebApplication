<%@ Page Language="C#" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="true" CodeFile="demo.aspx.cs" Inherits="demo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="running_banner row-fluid">
        <div class="container">
            <div class="banner_text">
                <h1>TRY KORE PROJECTS</h1>
                <img src="images/banner-demo.jpg" alt="" class="running_ban_img" />
            </div>
            <!--banner_text-->

            <h2 class="text-center col-lg-12 col-md-12 col-sm-12">Try our DEMOS
            </h2>
            <div class="col-lg-12 col-md-12 col-sm-12 text-center">
                <h3>Interested to see how Kore Projects can transform your company? Use following logins.</h3>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div class="col-lg-6 col-md-6 col-sm-6 text-center">
                    <h3>Painter Login</h3>
                    <p>User:&nbsp;demopainter@koreprojects.co.nz</p>
                    <p>Password: demo</p>
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6 text-center">
                    <h3>Scaffolder Login</h3>
                    <p>User:&nbsp;demoscaffolding@koreprojects.co.nz</p>
                    <p>Password: demo</p>
                </div>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-4 offset4">
                <iframe src="Login.aspx" scrolling="No"
                    marginwidth="0" marginheight="0" align="MIDDLE"
                    frameborder="No" allowtransparency="true" width="100%" height="400"></iframe>
            </div>
        </div>
    </div>
</asp:Content>
