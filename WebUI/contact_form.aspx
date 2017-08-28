<%@ Page Language="VB" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="false" CodeFile="contact_form.aspx.vb" Inherits="contact_form" %>

<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <div id="main_wrapper_inner">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="sm" runat="server">
    </asp:ScriptManager>
    <style type="text/css">
        .tableheader {
            padding: 5px 5px 5px 5px;
        }

        .tablebody {
            padding: 5px 5px 5px 5px;
        }
    </style>
    <div class="header_inner">
        <h1>Contact Us</h1>
    </div>
    <div class="content_inner">
        <div align="center">
            <asp:Label ID="lblErrorMessage" runat="server" Font-Size="Small" ForeColor="red"></asp:Label>
            <table border="0" cellpadding="3">
                <tr>
                    <td class="tableheader" align="right"><font face="Arial"><b>Company 
				Name </b></font></td>
                    <td class="tablebody" align="left">
                        <asp:TextBox ID="CompanyName" Width="220" runat="server"></asp:TextBox></td>
                    <td style="width: 85px"></td>
                </tr>
                <tr>
                    <td class="tableheader" align="right"><font face="Arial"><b> 
				Full Name </b></font></td>
                    <td class="tablebody" align="left">
                        <asp:TextBox ID="Name" Width="220" ClientIDMode="Static" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 85px">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="Name" Display="Dynamic" runat="server" ForeColor="#cc0000" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="tableheader" align="right"><font face="Arial"><b>Email 
				Address </b></font></td>
                    <td class="tablebody" align="left">
                        <asp:TextBox ID="EmailFrom" runat="server" Width="220"></asp:TextBox>
                    </td>
                    <td style="width: 85px">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="EmailFrom" Display="Dynamic" runat="server" ForeColor="#cc0000" ErrorMessage="* Required"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="tableheader" align="right"><font face="Arial"><b>Phone 
				Number </b></font></td>
                    <td class="tablebody" align="left">
                        <asp:TextBox ID="Phone" Width="220" runat="server"></asp:TextBox>
                    </td>
                    <td style="width: 85px">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="Phone" Display="Dynamic" runat="server" ForeColor="#cc0000" ErrorMessage="* Required"></asp:RequiredFieldValidator></td>
                </tr>
            </table>
            <p></p>
            <table border="0" cellpadding="3" id="table1">
                <tr>
                    <td valign="top" class="tableheader" align="right"><b>
                        <font face="Arial">Your Message</font></b></td>
                    <td class="tablebody" align="left">
                        <asp:TextBox ID="bodytxt" TextMode="MultiLine" Width="250" runat="server"></asp:TextBox>&nbsp;&nbsp;</td>
                    <td style="width: 35px"></td>
                </tr>
                <tr>
                    <td valign="top" class="tableheader" align="right">
                        <b><font face="Arial">Verification</font></b></td>
                    <td class="tablebody" align="left">
                        <asp:UpdatePanel ID="up1" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 180px">
                                            <cc1:CaptchaControl ID="CaptchaControl1" runat="server" Height="31px"
                                                Width="90px" CaptchaLength="5" BackColor="White"
                                                EnableViewState="False" />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:LinkButton ID="lnkrefresh" runat="server" OnClick="lnkrefresh_Click" Text="Change" CausesValidation="false"> </asp:LinkButton>
                                        </td>

                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <asp:TextBox ID="captchacode" Width="250" placeholder="Write the text in the image above" runat="server"></asp:TextBox>

                    </td>
                    <td style="width: 35px"></td>
                </tr>
                <tr>
                    <td class="tableheader"></td>
                    <td align="right" class="tablebody">
                        <asp:CustomValidator ID="CustomValidator1" ErrorMessage="Invalid. Please try again."
                            OnServerValidate="ValidateCaptcha" runat="server" />
                        <asp:Button ID="butsubmit" runat="server" OnClick="butsubmit_Click" Text="Submit" />
                    </td>
                    <td style="width: 35px"></td>
                </tr>
            </table>
        </div>

        <div align="center">
        </div>
        <%--<br>--%>
        <%--<div align="center">
            <h2 style="margin-top: 0"></h2>
            <table width="100%" cellpadding="10" cellspacing="0" border="0">
                <tr>
                    <td align="center">
                        <iframe width="90%" height="400" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="map.htm"></iframe>
                    </td>
                </tr>
            </table>
            <p>
                <br>
                <br>
                <strong>Rangiora Business Park</strong><br />
                Unit 12, 6 Cone Street, Rangiora, Christchurch, New Zealand<br />
                Phone: +64 (0)3 745 9119<br />
            </p>
        </div>--%>
    </div>
    <script language="javascript">
        document.forms[0].Name.focus();
    </script>
</asp:Content>

