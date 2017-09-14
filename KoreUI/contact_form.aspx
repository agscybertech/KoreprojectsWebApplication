<%@ Page Language="VB" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="false" CodeFile="contact_form.aspx.vb" Inherits="contact_form" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="sm" runat="server">
    </asp:ScriptManager>
    <div class="running_banner row-fluid">
        <div class="container">
            <div class="banner_text">
                <h1>contact us</h1>
                <img src="images/banner-contact.jpg" alt="" class="running_ban_img" />
            </div>
            <!--banner_text-->
            <div class="text-center col-lg-12 col-md-12 col-sm-12">
                <asp:Label ID="lblErrorMessage" runat="server" Font-Size="Small" ForeColor="red"></asp:Label>
                <asp:CustomValidator ID="CustomValidator1" ErrorMessage="Please try again." OnServerValidate="ValidateCaptcha" runat="server" />
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12">
                <div class="demo_page_form">
                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <span>Company Name </span>
                        <asp:TextBox ID="CompanyName" runat="server"></asp:TextBox>
                    </div>

                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <span>Full Name</span>
                        <asp:TextBox ID="Name" ClientIDMode="Static" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="Name" Display="Dynamic" runat="server" ForeColor="#cc0000" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <span>Email Address</span>
                        <asp:TextBox ID="EmailFrom" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="EmailFrom" Display="Dynamic" runat="server" ForeColor="#cc0000" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <span>Phone Number </span>
                        <asp:TextBox ID="Phone" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="Phone" Display="Dynamic" runat="server" ForeColor="#cc0000" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-lg-12 col-md-12 col-sm-12">
                        <span>Your Message </span>
                        <asp:TextBox ID="bodytxt" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </div>

                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <span>Verification</span>
                        <asp:UpdatePanel ID="up1" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="padding-right: 10px">
                                            <cc1:CaptchaControl ID="CaptchaControl1" runat="server" Height="31px"
                                                Width="90px" CaptchaLength="5" BackColor="White"
                                                EnableViewState="False" />
                                        </td>
                                        <td style="padding-right: 10px">
                                            <asp:TextBox ID="captchacode" Width="250" placeholder="Write the text shown in images" runat="server"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:LinkButton ID="lnkrefresh" runat="server" OnClick="lnkrefresh_Click" Text="Change" CausesValidation="false"> </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="col-xs-12 text-left">
                        <asp:Button ID="butsubmit" runat="server" CssClass="my_btn" Width="90" OnClick="butsubmit_Click" style="margin-left:0px" Text="Submit" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

