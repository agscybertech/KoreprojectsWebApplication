<%@ Page Language="C#" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="true" CodeFile="Signin.aspx.cs" Inherits="Signin" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="running_banner row-fluid">
        <div class="container">
            <div class="banner_text">
                <h1>KORE PROJECTS</h1>
                <img src="images/banner-demo.jpg" alt="" class="running_ban_img" />
            </div>
            <!--banner_text-->

            <h2 class="text-center col-lg-12 col-md-12 col-sm-12">Login
            </h2>
            <div class="text-center col-lg-12 col-md-12 col-sm-12">
                <asp:Label ID="lblErrorMessage" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 offset3">
                <iframe src="Login.aspx" scrolling="No"
                    marginwidth="0" marginheight="0" align="MIDDLE"
                    frameborder="No" allowtransparency="true" width="100%" height="280"></iframe>
            </div>
             <div class="col-lg-6 col-md-6 col-sm-6 offset3 text-center">
                    <asp:HyperLink ID="hlkForgottenPassward" runat="server" NavigateUrl="ForgotPassword.aspx">Forgot your password?</asp:HyperLink>
                </div>
        </div>
    </div>
</asp:Content>

