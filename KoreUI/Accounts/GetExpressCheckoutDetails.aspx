<%@ page language="C#" autoeventwireup="true" masterpagefile="~/Projects/MasterPage.master" inherits="GetExpressCheckoutDetails" CodeFile="GetExpressCheckoutDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
        <div runat="server" id="divTitle">
            <a style="padding: 8px 25px 8px 8px; float: right;" id="help" href="http://my.koreprojects.co.nz/helpconsole2010/Help/default.aspx?pageid=payment_history" title="Help" target="_blank">
                <img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
            <table style="padding: 8px;" border="0" cellpadding="0" cellspacing="0" width="612px">
                <tr>
                    <td style="background-image: url(../images/sub-nav-left.png); width: 16px">
                        <img src="../images/sub-nav-left.png" width="16" height="40" style="border: 0" />
                    </td>
                    <td style="background-image: url(../images/sub-nav-bg.png);">
                        <a href="../Contacts/OwnerSetting.aspx" style="text-decoration: none; pointer: hand; color: White;">Worksheet Setup</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="../Contacts/OwnerService.aspx" style="text-decoration: none; pointer: hand; color: White;">Service Rates</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="../Contacts/ProjectSetting.aspx" style="text-decoration: none; pointer: hand; color: White;">Project Setup</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="../Contacts/ProjectOwnerDetail.aspx" style="text-decoration: none; pointer: hand; color: White;">User Settings</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="Plans.aspx" style="text-decoration: none; pointer: hand; color: #0481D1; font-weight: bold;">Subscription</a>
                        <a href="PaymentHistory.aspx" style="text-decoration: none; pointer: hand; color: White; display: none;">Payment History</a>
                    </td>
                    <td style="background-image: url(../images/sub-nav-right.png); width: 16px"></td>
                </tr>
            </table>
        </div>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->
    <!-- content -->
    <div id="content" align="left">
        <table style="width: 100%" align="center" cellpadding="4" cellspacing="4">
            <tr>
                <td align="left">
                    <h1 id="appointment">Order Information</h1>
                </td>
            </tr>
            <tr>
               <%-- <td align="left" style="padding: 0 5 0 5;">
                    <p>For every new project you create, it requires a Project Credit. Please select a bundle below to credit your Kore Project account. You can view your credit history anytime under Settings.</p>
                    <asp:Label ID="lblUnarchivedProjectsCountMessage" runat="server"></asp:Label>
                </td>--%>
            </tr>
            <tr>
                <td>
                    <table border="1" style="width: 100%;">
                    <tr>
                        <th style="width: 45%;">Billing Details</th>
                        <th style="width: 40%;">Products
                        </th>
                        <th>Sub-Total
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <%=Session["Address_Name"]%>
                            <br />

                            <%=Session["Address_Street"]%>
                            <br />

                            <%=Session["Address_CityName"]%>
                            <br />

                            <%=Session["Address_StateOrProvince"]%>
                            <br />

                            <%=Session["Address_CountryName"]%>
                            <br />

                            <%=Session["Address_PostalCode"]%>
                        </td>
                        <td>
                            <%=Session["Product_Quantity"]%> X  <%=Session["Product_Name"]%>
                        </td>
                        <td style="text-align: center;">$<%=Session["Order_Total"]%> 
                        </td>
                    </tr>
                </table>
                </td>
            </tr>
            <tr align="center">
                
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td valign="middle">
                                <asp:ImageButton runat="server" ID="pay_now_button" OnClick="callDoExpressCheckout" ImageUrl="https://www.paypalobjects.com/webstatic/en_US/i/buttons/buy-logo-medium.png" AlternateText="Buy now with PayPal" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <%--<td align="center">
                    <asp:Label ID="lblPromoCodeMsg" runat="server" ForeColor="Red"></asp:Label>
                </td>--%>
            </tr>
            <tr>
               <%-- <td align="center">
                    <font size="1">
                        <asp:Label ID="lblGSTMsg" runat="server" Text="All prices include GST (NZ Only)"></asp:Label></font>
                </td>--%>
            </tr>

        </table>
    </div>
    <!-- content -->

    <!-- footer -->
    <div class="clr" id="Div1">
        <div class="sep" id="Div2"></div>
        <table width="100%" style="height: 45px">
            <tr>
                <td></td>
            </tr>
        </table>
        <div class="clr sep" id="Div3"></div>
    </div>
    <!-- footer -->
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script type="text/javascript" src="https://code.jquery.com/jquery.js"></script>

    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script type="text/javascript" src="../templates/js/bootstrap.min.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#pay_now_button").attr('value', 'Pay Now ($<%=Session["Order_Total"]%>)');
        });
   </script>
</asp:Content>
