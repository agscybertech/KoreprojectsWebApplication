<%@ Page Title="" Language="VB" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="false" CodeFile="ForgotPassword.aspx.vb" Inherits="ForgotPassword" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="running_banner row-fluid">
        <div class="container">
            <div class="banner_text">
                <h1>KORE PROJECTS</h1>
                <img src="images/banner-demo.jpg" alt="" class="running_ban_img" />
            </div>
            <!--banner_text-->

            <h2 class="text-center col-lg-12 col-md-12 col-sm-12">Forgot your password?
            </h2>
            <div class="text-center col-lg-12 col-md-12 col-sm-12">
                <asp:Label ID="lblErrorMessage" runat="server" ForeColor="red"></asp:Label>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required." ControlToValidate="txtUser" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtUser" Display="none" ErrorMessage="Email is not valid." ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>
                <asp:ValidationSummary ID="vsPersonalInformation" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server" />
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 offset3">
                <p>Enter your email address and we'll send you password reset instructions.</p>
                <div class="demo_page_form">
                    <span>email address</span>
                    <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
                    <asp:Button ID="btnLogin" runat="server" Text="Send" OnClick="btnLogin_Click" CssClass="my_btn" Style="    padding-bottom: 8px !important;
    margin: 0px !important;
    padding-right: 18px;
    padding-left: 0px;" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>


