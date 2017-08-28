<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" Inherits="Accounts_Plans" CodeFile="Plans.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("[id$=ddlUsers]").change(function () {
                $("[id$=lblTotal]").text("$" + (12 + (parseInt($("[id$=ddlUsers]").val()) * 5)))
            });

            $("[id$=rbCreditCard]").change(function () {
                if ($(this).is(":checked")) {
                    $("[id$=trCreditCard]").show();
                } else {
                    $("[id$=trCreditCard]").hide();
                }
            });
        });

        function MakePayment() {
            if (parseInt($("[id$=hdnTotalActiveUsers]").val()) > parseInt($("[id$=ddlUsers]").val())) {
                return confirm("You have " + $("[id$=hdnTotalActiveUsers]").val() + " active users and selected subscription of " + $("[id$=ddlUsers]").val() + " users. Access of " + (parseInt($("[id$=hdnTotalActiveUsers]").val()) - parseInt($("[id$=ddlUsers]").val())).toString() + " users will be disabled. Do you want to proceed with payment?")
            } else {
                return true;
            }
        }
    </script>
    <style type="text/css">
        .tblBillingAgreement {
            border-collapse: collapse;
            width: 100%;
        }

        table.tblBillingAgreement th, table.tblBillingAgreement td {
            border-width: 1px;
            border-style: solid;
        }

        table.tblBillingAgreement th {
            color: #464646;
            background-color: #CCCCCC;
            font-weight: bold;
            height: 30px;
        }

        table.tblBillingAgreement td {
            background-color: #F1F1F1;
            padding: 8px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
        <div runat="server" id="divTitle">
            <a style="padding: 8px 25px 8px 8px; float: right;" id="help" href="https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm" title="Help" target="_blank">
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
                    <h1 id="appointment">Select Your Plan<div style="float: right; font-size: 16px; margin-top: 14px;">
                        Subscription End Date:
                        <asp:Literal ID="litSubscriptionEndDate" runat="server"></asp:Literal>
                    </div>
                    </h1>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Text=""></asp:Label>
                </td>
            </tr>
            <tr id="trPaymentSucess" runat="server" visible="false">
                <td style="text-align: center; font-size: 14px;">
                    <asp:Label ID="lblExpirationDate" ForeColor="Green" runat="server" Text=""></asp:Label>
                    <table id="tblBillingAgreement" runat="server" class="tblBillingAgreement">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Last Payment Date</th>
                                <th>Last Payment Amount</th>
                                <th>Next Billing Date</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>Koreprojects Recurring Monthly Subscription</td>
                                <td>
                                    <asp:Label runat="server" ID="lblLastPaymentDate"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblLastPaymentAmount"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblNextBillingDate"></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkCancelSubscription" runat="server">Cancel Subscription</asp:LinkButton>
                                    <asp:HiddenField ID="hdnPaypalBillingAgreementID" runat="server" />
                                </td>
                            </tr>
                        </tbody>

                    </table>
                    <b style="color: green;"></b>. 
                </td>
            </tr>
            <tr id="trPayment" runat="server">
                <td>
                    <table style="width: 100%">
                        <tr>
                            <td style="text-align: center">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Pricing.png" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; padding: 20px">
                                <b>Pay by</b>
                                <asp:RadioButton ID="rbPaypal" runat="server" Text="Paypal" GroupName="PaymentMethod" Checked="true" />
                                <asp:RadioButton ID="rbCreditCard" runat="server" Text="Credit Card" GroupName="PaymentMethod" />
                            </td>
                        </tr>
                        <tr id="trCreditCard" runat="server" style="display:none">
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td style="text-align:right"><b>Card Type</b> <span style="color: red">*</span></td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlCardType">
                                                <asp:ListItem Text="Visa" Value="visa"></asp:ListItem>
                                                <asp:ListItem Text="MasterCard" Value="mastercard"></asp:ListItem>
                                                <asp:ListItem Text="Discover" Value="discover"></asp:ListItem>
                                                <asp:ListItem Text="American Express" Value="amex"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td style="text-align:right"><b>Card Number</b> <span style="color: red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtCardNumber" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right"><b>Expiry Date</b> <span style="color: red">*</span></td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlMonth">
                                                <asp:ListItem Text="Select Month" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="1 (Jan)" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="2 (Feb)" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="3 (Mar)" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="4 (Apr)" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="5 (May)" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="6 (Jun)" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="7 (Jul)" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="8 (Aug)" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="9 (Sep)" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="10 (Oct)" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="11 (Nov)" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="12 (Dec)" Value="12"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList runat="server" ID="ddlYear">
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td style="text-align:right"><b>CVV</b> <span style="color: red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtCardCVV" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right"><b>First Name</b> <span style="color: red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td style="text-align:right"><b>Last Name</b> <span style="color: red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right"><b>Address</b> <span style="color: red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                                        </td>
                                        <td></td>
                                        <td style="text-align:right"><b>City</b> <span style="color: red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right"><b>Country</b> <span style="color: red">*</span></td>
                                        <td>
                                            <asp:DropDownList ID="ddlCountry" runat="server">
                                                <asp:ListItem Text="ALBANIA" Value="AL"></asp:ListItem>
                                                <asp:ListItem Text="ALGERIA" Value="DZ"></asp:ListItem>
                                                <asp:ListItem Text="ANDORRA" Value="AD"></asp:ListItem>
                                                <asp:ListItem Text="ANGOLA" Value="AO"></asp:ListItem>
                                                <asp:ListItem Text="ANGUILLA" Value="AI"></asp:ListItem>
                                                <asp:ListItem Text="ANTIGUA & BARBUDA" Value="AG"></asp:ListItem>
                                                <asp:ListItem Text="ARGENTINA" Value="AR"></asp:ListItem>
                                                <asp:ListItem Text="ARMENIA" Value="AM"></asp:ListItem>
                                                <asp:ListItem Text="ARUBA" Value="AW"></asp:ListItem>
                                                <asp:ListItem Text="AUSTRALIA" Value="AU"></asp:ListItem>
                                                <asp:ListItem Text="AUSTRIA" Value="AT"></asp:ListItem>
                                                <asp:ListItem Text="AZERBAIJAN" Value="AZ"></asp:ListItem>
                                                <asp:ListItem Text="BAHAMAS" Value="BS"></asp:ListItem>
                                                <asp:ListItem Text="BAHRAIN" Value="BH"></asp:ListItem>
                                                <asp:ListItem Text="BARBADOS" Value="BB"></asp:ListItem>
                                                <asp:ListItem Text="BELARUS" Value="BY"></asp:ListItem>
                                                <asp:ListItem Text="BELGIUM" Value="BE"></asp:ListItem>
                                                <asp:ListItem Text="BELIZE" Value="BZ"></asp:ListItem>
                                                <asp:ListItem Text="BENIN" Value="BJ"></asp:ListItem>
                                                <asp:ListItem Text="BERMUDA" Value="BM"></asp:ListItem>
                                                <asp:ListItem Text="BHUTAN" Value="BT"></asp:ListItem>
                                                <asp:ListItem Text="BOLIVIA" Value="BO"></asp:ListItem>
                                                <asp:ListItem Text="BOSNIA & HERZEGOVINA" Value="BA"></asp:ListItem>
                                                <asp:ListItem Text="BOTSWANA" Value="BW"></asp:ListItem>
                                                <asp:ListItem Text="BRAZIL" Value="BR"></asp:ListItem>
                                                <asp:ListItem Text="BRITISH VIRGIN ISLANDS" Value="VG"></asp:ListItem>
                                                <asp:ListItem Text="BRUNEI" Value="BN"></asp:ListItem>
                                                <asp:ListItem Text="BULGARIA" Value="BG"></asp:ListItem>
                                                <asp:ListItem Text="BURKINA FASO" Value="BF"></asp:ListItem>
                                                <asp:ListItem Text="BURUNDI" Value="BI"></asp:ListItem>
                                                <asp:ListItem Text="CAMBODIA" Value="KH"></asp:ListItem>
                                                <asp:ListItem Text="CAMEROON" Value="CM"></asp:ListItem>
                                                <asp:ListItem Text="CANADA" Value="CA"></asp:ListItem>
                                                <asp:ListItem Text="CAPE VERDE" Value="CV"></asp:ListItem>
                                                <asp:ListItem Text="CAYMAN ISLANDS" Value="KY"></asp:ListItem>
                                                <asp:ListItem Text="CHAD" Value="TD"></asp:ListItem>
                                                <asp:ListItem Text="CHILE" Value="CL"></asp:ListItem>
                                                <asp:ListItem Text="CHINA" Value="C2"></asp:ListItem>
                                                <asp:ListItem Text="COLOMBIA" Value="CO"></asp:ListItem>
                                                <asp:ListItem Text="COMOROS" Value="KM"></asp:ListItem>
                                                <asp:ListItem Text="CONGO - BRAZZAVILLE" Value="CG"></asp:ListItem>
                                                <asp:ListItem Text="CONGO - KINSHASA" Value="CD"></asp:ListItem>
                                                <asp:ListItem Text="COOK ISLANDS" Value="CK"></asp:ListItem>
                                                <asp:ListItem Text="COSTA RICA" Value="CR"></asp:ListItem>
                                                <asp:ListItem Text="CÔTE D’IVOIRE" Value="CI"></asp:ListItem>
                                                <asp:ListItem Text="CROATIA" Value="HR"></asp:ListItem>
                                                <asp:ListItem Text="CYPRUS" Value="CY"></asp:ListItem>
                                                <asp:ListItem Text="CZECH REPUBLIC" Value="CZ"></asp:ListItem>
                                                <asp:ListItem Text="DENMARK" Value="DK"></asp:ListItem>
                                                <asp:ListItem Text="DJIBOUTI" Value="DJ"></asp:ListItem>
                                                <asp:ListItem Text="DOMINICA" Value="DM"></asp:ListItem>
                                                <asp:ListItem Text="DOMINICAN REPUBLIC" Value="DO"></asp:ListItem>
                                                <asp:ListItem Text="ECUADOR" Value="EC"></asp:ListItem>
                                                <asp:ListItem Text="EGYPT" Value="EG"></asp:ListItem>
                                                <asp:ListItem Text="EL SALVADOR" Value="SV"></asp:ListItem>
                                                <asp:ListItem Text="ERITREA" Value="ER"></asp:ListItem>
                                                <asp:ListItem Text="ESTONIA" Value="EE"></asp:ListItem>
                                                <asp:ListItem Text="ETHIOPIA" Value="ET"></asp:ListItem>
                                                <asp:ListItem Text="FALKLAND ISLANDS" Value="FK"></asp:ListItem>
                                                <asp:ListItem Text="FAROE ISLANDS" Value="FO"></asp:ListItem>
                                                <asp:ListItem Text="FIJI" Value="FJ"></asp:ListItem>
                                                <asp:ListItem Text="FINLAND" Value="FI"></asp:ListItem>
                                                <asp:ListItem Text="FRANCE" Value="FR"></asp:ListItem>
                                                <asp:ListItem Text="FRENCH GUIANA" Value="GF"></asp:ListItem>
                                                <asp:ListItem Text="FRENCH POLYNESIA" Value="PF"></asp:ListItem>
                                                <asp:ListItem Text="GABON" Value="GA"></asp:ListItem>
                                                <asp:ListItem Text="GAMBIA" Value="GM"></asp:ListItem>
                                                <asp:ListItem Text="GEORGIA" Value="GE"></asp:ListItem>
                                                <asp:ListItem Text="GERMANY" Value="DE"></asp:ListItem>
                                                <asp:ListItem Text="GIBRALTAR" Value="GI"></asp:ListItem>
                                                <asp:ListItem Text="GREECE" Value="GR"></asp:ListItem>
                                                <asp:ListItem Text="GREENLAND" Value="GL"></asp:ListItem>
                                                <asp:ListItem Text="GRENADA" Value="GD"></asp:ListItem>
                                                <asp:ListItem Text="GUADELOUPE" Value="GP"></asp:ListItem>
                                                <asp:ListItem Text="GUATEMALA" Value="GT"></asp:ListItem>
                                                <asp:ListItem Text="GUINEA" Value="GN"></asp:ListItem>
                                                <asp:ListItem Text="GUINEA-BISSAU" Value="GW"></asp:ListItem>
                                                <asp:ListItem Text="GUYANA" Value="GY"></asp:ListItem>
                                                <asp:ListItem Text="HONDURAS" Value="HN"></asp:ListItem>
                                                <asp:ListItem Text="HONG KONG SAR CHINA" Value="HK"></asp:ListItem>
                                                <asp:ListItem Text="HUNGARY" Value="HU"></asp:ListItem>
                                                <asp:ListItem Text="ICELAND" Value="IS"></asp:ListItem>
                                                <asp:ListItem Text="INDIA" Value="IN"></asp:ListItem>
                                                <asp:ListItem Text="INDONESIA" Value="ID"></asp:ListItem>
                                                <asp:ListItem Text="IRELAND" Value="IE"></asp:ListItem>
                                                <asp:ListItem Text="ISRAEL" Value="IL"></asp:ListItem>
                                                <asp:ListItem Text="ITALY" Value="IT"></asp:ListItem>
                                                <asp:ListItem Text="JAMAICA" Value="JM"></asp:ListItem>
                                                <asp:ListItem Text="JAPAN" Value="JP"></asp:ListItem>
                                                <asp:ListItem Text="JORDAN" Value="JO"></asp:ListItem>
                                                <asp:ListItem Text="KAZAKHSTAN" Value="KZ"></asp:ListItem>
                                                <asp:ListItem Text="KENYA" Value="KE"></asp:ListItem>
                                                <asp:ListItem Text="KIRIBATI" Value="KI"></asp:ListItem>
                                                <asp:ListItem Text="KUWAIT" Value="KW"></asp:ListItem>
                                                <asp:ListItem Text="KYRGYZSTAN" Value="KG"></asp:ListItem>
                                                <asp:ListItem Text="LAOS" Value="LA"></asp:ListItem>
                                                <asp:ListItem Text="LATVIA" Value="LV"></asp:ListItem>
                                                <asp:ListItem Text="LESOTHO" Value="LS"></asp:ListItem>
                                                <asp:ListItem Text="LIECHTENSTEIN" Value="LI"></asp:ListItem>
                                                <asp:ListItem Text="LITHUANIA" Value="LT"></asp:ListItem>
                                                <asp:ListItem Text="LUXEMBOURG" Value="LU"></asp:ListItem>
                                                <asp:ListItem Text="MACEDONIA" Value="MK"></asp:ListItem>
                                                <asp:ListItem Text="MADAGASCAR" Value="MG"></asp:ListItem>
                                                <asp:ListItem Text="MALAWI" Value="MW"></asp:ListItem>
                                                <asp:ListItem Text="MALAYSIA" Value="MY"></asp:ListItem>
                                                <asp:ListItem Text="MALDIVES" Value="MV"></asp:ListItem>
                                                <asp:ListItem Text="MALI" Value="ML"></asp:ListItem>
                                                <asp:ListItem Text="MALTA" Value="MT"></asp:ListItem>
                                                <asp:ListItem Text="MARSHALL ISLANDS" Value="MH"></asp:ListItem>
                                                <asp:ListItem Text="MARTINIQUE" Value="MQ"></asp:ListItem>
                                                <asp:ListItem Text="MAURITANIA" Value="MR"></asp:ListItem>
                                                <asp:ListItem Text="MAURITIUS" Value="MU"></asp:ListItem>
                                                <asp:ListItem Text="MAYOTTE" Value="YT"></asp:ListItem>
                                                <asp:ListItem Text="MEXICO" Value="MX"></asp:ListItem>
                                                <asp:ListItem Text="MICRONESIA" Value="FM"></asp:ListItem>
                                                <asp:ListItem Text="MOLDOVA" Value="MD"></asp:ListItem>
                                                <asp:ListItem Text="MONACO" Value="MC"></asp:ListItem>
                                                <asp:ListItem Text="MONGOLIA" Value="MN"></asp:ListItem>
                                                <asp:ListItem Text="MONTENEGRO" Value="ME"></asp:ListItem>
                                                <asp:ListItem Text="MONTSERRAT" Value="MS"></asp:ListItem>
                                                <asp:ListItem Text="MOROCCO" Value="MA"></asp:ListItem>
                                                <asp:ListItem Text="MOZAMBIQUE" Value="MZ"></asp:ListItem>
                                                <asp:ListItem Text="NAMIBIA" Value="NA"></asp:ListItem>
                                                <asp:ListItem Text="NAURU" Value="NR"></asp:ListItem>
                                                <asp:ListItem Text="NEPAL" Value="NP"></asp:ListItem>
                                                <asp:ListItem Text="NETHERLANDS" Value="NL"></asp:ListItem>
                                                <asp:ListItem Text="NEW CALEDONIA" Value="NC"></asp:ListItem>
                                                <asp:ListItem Text="NEW ZEALAND" Value="NZ"></asp:ListItem>
                                                <asp:ListItem Text="NICARAGUA" Value="NI"></asp:ListItem>
                                                <asp:ListItem Text="NIGER" Value="NE"></asp:ListItem>
                                                <asp:ListItem Text="NIGERIA" Value="NG"></asp:ListItem>
                                                <asp:ListItem Text="NIUE" Value="NU"></asp:ListItem>
                                                <asp:ListItem Text="NORFOLK ISLAND" Value="NF"></asp:ListItem>
                                                <asp:ListItem Text="NORWAY" Value="NO"></asp:ListItem>
                                                <asp:ListItem Text="OMAN" Value="OM"></asp:ListItem>
                                                <asp:ListItem Text="PALAU" Value="PW"></asp:ListItem>
                                                <asp:ListItem Text="PANAMA" Value="PA"></asp:ListItem>
                                                <asp:ListItem Text="PAPUA NEW GUINEA" Value="PG"></asp:ListItem>
                                                <asp:ListItem Text="PARAGUAY" Value="PY"></asp:ListItem>
                                                <asp:ListItem Text="PERU" Value="PE"></asp:ListItem>
                                                <asp:ListItem Text="PHILIPPINES" Value="PH"></asp:ListItem>
                                                <asp:ListItem Text="PITCAIRN ISLANDS" Value="PN"></asp:ListItem>
                                                <asp:ListItem Text="POLAND" Value="PL"></asp:ListItem>
                                                <asp:ListItem Text="PORTUGAL" Value="PT"></asp:ListItem>
                                                <asp:ListItem Text="QATAR" Value="QA"></asp:ListItem>
                                                <asp:ListItem Text="RÉUNION" Value="RE"></asp:ListItem>
                                                <asp:ListItem Text="ROMANIA" Value="RO"></asp:ListItem>
                                                <asp:ListItem Text="RUSSIA" Value="RU"></asp:ListItem>
                                                <asp:ListItem Text="RWANDA" Value="RW"></asp:ListItem>
                                                <asp:ListItem Text="SAMOA" Value="WS"></asp:ListItem>
                                                <asp:ListItem Text="SAN MARINO" Value="SM"></asp:ListItem>
                                                <asp:ListItem Text="SÃO TOMÉ & PRÍNCIPE" Value="ST"></asp:ListItem>
                                                <asp:ListItem Text="SAUDI ARABIA" Value="SA"></asp:ListItem>
                                                <asp:ListItem Text="SENEGAL" Value="SN"></asp:ListItem>
                                                <asp:ListItem Text="SERBIA" Value="RS"></asp:ListItem>
                                                <asp:ListItem Text="SEYCHELLES" Value="SC"></asp:ListItem>
                                                <asp:ListItem Text="SIERRA LEONE" Value="SL"></asp:ListItem>
                                                <asp:ListItem Text="SINGAPORE" Value="SG"></asp:ListItem>
                                                <asp:ListItem Text="SLOVAKIA" Value="SK"></asp:ListItem>
                                                <asp:ListItem Text="SLOVENIA" Value="SI"></asp:ListItem>
                                                <asp:ListItem Text="SOLOMON ISLANDS" Value="SB"></asp:ListItem>
                                                <asp:ListItem Text="SOMALIA" Value="SO"></asp:ListItem>
                                                <asp:ListItem Text="SOUTH AFRICA" Value="ZA"></asp:ListItem>
                                                <asp:ListItem Text="SOUTH KOREA" Value="KR"></asp:ListItem>
                                                <asp:ListItem Text="SPAIN" Value="ES"></asp:ListItem>
                                                <asp:ListItem Text="SRI LANKA" Value="LK"></asp:ListItem>
                                                <asp:ListItem Text="ST. HELENA" Value="SH"></asp:ListItem>
                                                <asp:ListItem Text="ST. KITTS & NEVIS" Value="KN"></asp:ListItem>
                                                <asp:ListItem Text="ST. LUCIA" Value="LC"></asp:ListItem>
                                                <asp:ListItem Text="ST. PIERRE & MIQUELON" Value="PM"></asp:ListItem>
                                                <asp:ListItem Text="ST. VINCENT & GRENADINES" Value="VC"></asp:ListItem>
                                                <asp:ListItem Text="SURINAME" Value="SR"></asp:ListItem>
                                                <asp:ListItem Text="SVALBARD & JAN MAYEN" Value="SJ"></asp:ListItem>
                                                <asp:ListItem Text="SWAZILAND" Value="SZ"></asp:ListItem>
                                                <asp:ListItem Text="SWEDEN" Value="SE"></asp:ListItem>
                                                <asp:ListItem Text="SWITZERLAND" Value="CH"></asp:ListItem>
                                                <asp:ListItem Text="TAIWAN" Value="TW"></asp:ListItem>
                                                <asp:ListItem Text="TAJIKISTAN" Value="TJ"></asp:ListItem>
                                                <asp:ListItem Text="TANZANIA" Value="TZ"></asp:ListItem>
                                                <asp:ListItem Text="THAILAND" Value="TH"></asp:ListItem>
                                                <asp:ListItem Text="TOGO" Value="TG"></asp:ListItem>
                                                <asp:ListItem Text="TONGA" Value="TO"></asp:ListItem>
                                                <asp:ListItem Text="TRINIDAD & TOBAGO" Value="TT"></asp:ListItem>
                                                <asp:ListItem Text="TUNISIA" Value="TN"></asp:ListItem>
                                                <asp:ListItem Text="TURKMENISTAN" Value="TM"></asp:ListItem>
                                                <asp:ListItem Text="TURKS & CAICOS ISLANDS" Value="TC"></asp:ListItem>
                                                <asp:ListItem Text="TUVALU" Value="TV"></asp:ListItem>
                                                <asp:ListItem Text="UGANDA" Value="UG"></asp:ListItem>
                                                <asp:ListItem Text="UKRAINE" Value="UA"></asp:ListItem>
                                                <asp:ListItem Text="UNITED ARAB EMIRATES" Value="AE"></asp:ListItem>
                                                <asp:ListItem Text="UNITED KINGDOM" Value="GB"></asp:ListItem>
                                                <asp:ListItem Text="UNITED STATES" Value="US"></asp:ListItem>
                                                <asp:ListItem Text="URUGUAY" Value="UY"></asp:ListItem>
                                                <asp:ListItem Text="VANUATU" Value="VU"></asp:ListItem>
                                                <asp:ListItem Text="VATICAN CITY" Value="VA"></asp:ListItem>
                                                <asp:ListItem Text="VENEZUELA" Value="VE"></asp:ListItem>
                                                <asp:ListItem Text="VIETNAM" Value="VN"></asp:ListItem>
                                                <asp:ListItem Text="WALLIS & FUTUNA" Value="WF"></asp:ListItem>
                                                <asp:ListItem Text="YEMEN" Value="YE"></asp:ListItem>
                                                <asp:ListItem Text="ZAMBIA" Value="ZM"></asp:ListItem>
                                                <asp:ListItem Text="ZIMBABWE" Value="ZW"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td style="text-align:right"><b>Postal Code</b> <span style="color: red">*</span></td>
                                        <td>
                                            <asp:TextBox ID="txtPostalCode" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table style="margin: 0 auto; margin-top: 20px; font-size: 16px;">
                                    <tr>
                                        <td>$12 for Administrator
                                        </td>
                                        <td>+
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlUsers" runat="server">
                                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                                <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                                <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                                <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                                <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                                <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                                <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                                <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                                <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                                <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                                <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                                <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                                <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                                <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                                <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                                <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                                <asp:ListItem Value="19" Text="19"></asp:ListItem>
                                                <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                                <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                                <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                                <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                                <asp:ListItem Value="24" Text="24"></asp:ListItem>
                                                <asp:ListItem Value="25" Text="25"></asp:ListItem>
                                                <asp:ListItem Value="26" Text="26"></asp:ListItem>
                                                <asp:ListItem Value="27" Text="27"></asp:ListItem>
                                                <asp:ListItem Value="28" Text="28"></asp:ListItem>
                                                <asp:ListItem Value="29" Text="29"></asp:ListItem>
                                                <asp:ListItem Value="30" Text="30"></asp:ListItem>
                                                <asp:ListItem Value="31" Text="31"></asp:ListItem>
                                                <asp:ListItem Value="32" Text="32"></asp:ListItem>
                                                <asp:ListItem Value="33" Text="33"></asp:ListItem>
                                                <asp:ListItem Value="34" Text="34"></asp:ListItem>
                                                <asp:ListItem Value="35" Text="35"></asp:ListItem>
                                                <asp:ListItem Value="36" Text="36"></asp:ListItem>
                                                <asp:ListItem Value="37" Text="37"></asp:ListItem>
                                                <asp:ListItem Value="38" Text="38"></asp:ListItem>
                                                <asp:ListItem Value="39" Text="39"></asp:ListItem>
                                                <asp:ListItem Value="40" Text="40"></asp:ListItem>
                                                <asp:ListItem Value="41" Text="41"></asp:ListItem>
                                                <asp:ListItem Value="42" Text="42"></asp:ListItem>
                                                <asp:ListItem Value="43" Text="43"></asp:ListItem>
                                                <asp:ListItem Value="44" Text="44"></asp:ListItem>
                                                <asp:ListItem Value="45" Text="45"></asp:ListItem>
                                                <asp:ListItem Value="46" Text="46"></asp:ListItem>
                                                <asp:ListItem Value="47" Text="47"></asp:ListItem>
                                                <asp:ListItem Value="48" Text="48"></asp:ListItem>
                                                <asp:ListItem Value="49" Text="49"></asp:ListItem>
                                                <asp:ListItem Value="50" Text="50"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>Users X
                                        </td>
                                        <td>$5
                                        </td>
                                        <td>=
                                        </td>
                                        <td style="width: 40px">
                                            <asp:Label ID="lblTotal" runat="server" Text="$17" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnPay" runat="server" AlternateText="PayPal" ImageUrl="https://www.paypalobjects.com/webstatic/en_US/i/btn/png/silver-rect-paypal-34px.png" OnClientClick="return MakePayment();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <hr />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <font size="1">
                        <asp:Label ID="lblGSTMsg" runat="server" Text="All prices include GST (NZ Only)"></asp:Label></font>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hdnTotalActiveUsers" runat="server" />
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
</asp:Content>

