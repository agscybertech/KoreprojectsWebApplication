<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="ContactDetail.aspx.vb" Inherits="Contacts_ContactDetail" %>
<%@ Register TagPrefix="UC" TagName="ServiceRatesGrid" Src="~/WebControls/WebUserControlRateGrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <script type="text/javascript">
        function numberonly(ctl, evt, allowNagitive, allowDecimal, allowSpace, allowDash) {
            var result = false;
            var k = document.all ? evt.keyCode : evt.which;
            var strValue = ctl.value + String.fromCharCode(k);
            if (k > 47 && k < 58 || k == 8 || k == 127) {
                result = true;
            }
            else {
                if (allowDecimal && k == 46) {
                    if (strValue.indexOf('.') == strValue.lastIndexOf('.')) {
                        result = true;
                    }
                }
                else {
                    if (allowNagitive && k == 45) {
                        if (strValue.indexOf('-') == strValue.lastIndexOf('-') && strValue.indexOf('-') == 0) {
                            result = true;
                        }
                    }
                    else {
                        if (allowSpace && k == 32) {
                            if (strValue.indexOf(' ') != 0) {
                                result = true;
                            }
                        }
                        else {
                            if (allowDash && k == 45) {
                                if (strValue.indexOf('-') != 0) {
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        function CleanInput(ctl) {
            var inputString = ctl.value;
            var cleanString = '';
            for (var i = 0; i < inputString.length; i++) {
                var charCode = inputString.charCodeAt(i)
                if (charCode >= 40 && charCode <= 176) {
                    cleanString += String.fromCharCode(charCode);
                }
            }
            ctl.value = cleanString;
            //var unicodeString = '';
            //for (var i = 0; i < inputString.length; i++) {
            //    var theUnicode = inputString.charCodeAt(i).toString(16).toUpperCase();
            //    while (theUnicode.length < 4) {
            //        theUnicode = '0' + theUnicode;
            //    }
            //    theUnicode = '\\u' + theUnicode;
            //    unicodeString += theUnicode;
            //}
            //alert(unicodeString);
        }
    </script>
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <asp:ImageButton ID="imgInviteLink" style="padding:8px 25px 8px 8px;float:right;cursor:pointer;" runat="server" ImageUrl="../images/help-icon.png" width="40" height="40" border="0" ToolTip="Send Link" />
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <div align="center" width="100%">
            <asp:Label ID="lblmsg" runat="server" Font-Size="X-Small" ForeColor="Red"></asp:Label>
        </div>        
        
        <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
            <tr>
                <td colspan="2" align="left">
                    <h4><asp:Label ID="lblContactType" runat="server" Text=""></asp:Label> DETAILS</h4>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel">
                        <label>Business Name<span style="display:none;">*</span></label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtBusinessName"
                    maxlength="100" runat="server" TabIndex="10" ReadOnly="True"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>Address<span style="display:none;">*</span></label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtAddress"
                    maxlength="100" runat="server" TabIndex="20" ReadOnly="True"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>First Name<span style="display:none;">*</span></label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtFirstName"
                    maxlength="100" runat="server" TabIndex="30" ReadOnly="True"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>Suburb</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtSuburb"
                    maxlength="100" runat="server" TabIndex="40" ReadOnly="True"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Last Name<span style="display:none;">*</span></label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtLastName"
                    maxlength="100" runat="server" TabIndex="50" ReadOnly="True"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>City</label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtCity"
                    maxlength="100" runat="server" TabIndex="60" ReadOnly="True"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Phone</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtHomePhone"
                    maxlength="100" runat="server" TabIndex="70" ReadOnly="True" onkeypress="return numberonly(this,event, false, false, true, true);" onblur="CleanInput(this);"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>Post Code</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtPostCode"
                    maxlength="100" runat="server" TabIndex="80" ReadOnly="True"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel" id="Div11"><label>Fax<span style="display:none;">*</span></label></div>
	                <div class="formfield" id="Div12">
                        <asp:TextBox ID="txtWorkPhone"
                    maxlength="100" runat="server" TabIndex="90" ReadOnly="True" onkeypress="return numberonly(this,event, false, false, true, true);" onblur="CleanInput(this);"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>Region</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtRegion"
                    maxlength="100" runat="server" TabIndex="100" ReadOnly="True"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel" id="mobile_lable"><label>Mobile</label></div>
	                <div class="formfield" id="mobile_input">
                        <asp:TextBox ID="txtMobile"
                    maxlength="100" runat="server" TabIndex="110" ReadOnly="True" onkeypress="return numberonly(this,event, false, false, true, true);" onblur="CleanInput(this);"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>Country</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtCountry"
                    maxlength="100" runat="server" TabIndex="120" ReadOnly="True"></asp:TextBox></div>                
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel" id="photo_lable"></div>
                    <div class="formfield" id="photo_input"></div>
                </td>
                <td>
                    <div class="formlabel" id="ph_lable"><label>Email Address<span id="Span5" style="display:none;">*</span></label></div>
	                <div class="formfield" id="ph_input"><asp:TextBox ID="txtEmail" maxlength="100" 
                            runat="server" TabIndex="140" ReadOnly="True"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="formlabel"><label>Notes</label></div>	                
                    <asp:TextBox ID="tbxNote" runat="server" maxlength="100" TabIndex="150" 
                        Rows="5" TextMode="MultiLine" Width="90%" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
		        <td colspan="2">
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 33%" align="left">
                            <asp:Button ID="btnRemove" runat="server" 
                                Text="Remove" CausesValidation="False" Font-Size="16px" TabIndex="180" Visible="false" /></td>
                        <td align="center">                        

                        </td>
				        <td style="width: 33%" align="right"><span class="Apple-converted-space">
                            <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" 
                                Font-Size="16px" TabIndex="210" />&nbsp;</span></td>
			        </tr>
		        </table>
		        </td>
	        </tr>
        </table>
    </div>
    <!-- content -->

    <!-- footer -->
    <div class="clr" id="Div1">
        <div class="sep" id="Div2"></div>
        	<table width="100%" style="height:45px">
                <tr>
                    <td> 
                    </td>
                </tr>
            </table> 
        <div class="clr sep" id="Div3"></div>
    </div>
    <!-- footer -->
</asp:Content>
