<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="true" CodeFile="ProjectOwnerDetail.aspx.vb" Inherits="Contacts_ProjectOwnerDetail" %>
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
            <div runat="server" id="divTitle">
                <a style="padding:8px 25px 8px 8px;float:right;" id="help" href="https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
                <table style="padding:8px;" border="0" cellpadding="0" cellspacing="0" width="612px">
                    <tr>
                        <td style="background-image:url(../images/sub-nav-left.png); width:16px">
                            <img src="../images/sub-nav-left.png" width="16" height="40" style="border:0" />
                        </td>
                        <td style="background-image:url(../images/sub-nav-bg.png);">                            
                            <a href="OwnerSetting.aspx" style="text-decoration:none;pointer:hand;color:White;">Worksheet Setup</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="OwnerService.aspx" style="text-decoration:none;pointer:hand;color:White;">Service Rates</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="ProjectSetting.aspx" style="text-decoration:none;pointer:hand;color:White;">Project Setup</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <a href="ProjectOwnerDetail.aspx" style="text-decoration:none;pointer:hand;color:#0481D1;font-weight:bold;">User Settings</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;                        
                            <a href="../Accounts/Plans.aspx" style="text-decoration:none;pointer:hand;color:White;">Subscription</a>
                            <a href="../Accounts/PaymentHistory.aspx" style="text-decoration:none;pointer:hand;color:White;display:none;">Payment History</a>
                        </td>
                        <td style="background-image:url(../images/sub-nav-right.png); width:16px">
                        </td>
                    </tr>
                </table>
            </div>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <div align="center" width="100%">
            <asp:Label ID="lblmsg" runat="server" Font-Size="X-Small" ForeColor="Red"></asp:Label>
        </div>

        <table style="width: 90%" align="center" cellpadding="4" cellspacing="4" runat="server" id="tblOwnerDetail">
            <tr>
                <td colspan="2" align="left">
                    <h4>USER SETTINGS</h4>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel">
                        <label>Business Name<span style="display:none;">*</span></label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtBusinessName"
                    maxlength="100" runat="server" TabIndex="10"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>Address<span style="display:none;">*</span></label></div>
	                <div class="formfield"><asp:TextBox ID="txtAddress"
                    maxlength="100" runat="server" TabIndex="20"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>First Name<span style="display:none;">*</span></label></div>
	                <div class="formfield"><asp:TextBox ID="txtFirstName"
                    maxlength="100" runat="server" TabIndex="30"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>Suburb</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtSuburb"
                    maxlength="100" runat="server" TabIndex="40"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Last Name<span style="display:none;">*</span></label></div>
	                <div class="formfield"><asp:TextBox ID="txtLastName"
                    maxlength="100" runat="server" TabIndex="50"></asp:TextBox></div>                    
                </td>
                <td>
                    <div class="formlabel" id="ph_lable"><label>City</label></div>
	                <div class="formfield" id="ph_input"><asp:TextBox ID="txtCity"
                    maxlength="100" runat="server" TabIndex="60"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Phone</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtPhone"
                    maxlength="100" runat="server" TabIndex="70" onkeypress="return numberonly(this,event, false, false, true, true);" onblur="CleanInput(this);"></asp:TextBox></div>                    
                </td>
                <td>
                    <div class="formlabel"><label>Post Code</label></div>
                    <div class="formfield"><asp:TextBox ID="txtPostCode"
                    maxlength="100" runat="server" TabIndex="80"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Fax<span style="display:none;">*</span></label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtFax"
                    maxlength="100" runat="server" TabIndex="90" onkeypress="return numberonly(this,event, false, false, true, true);" onblur="CleanInput(this);"></asp:TextBox></div>                
                </td>
                <td>
                    <div class="formlabel"><label>Region</label></div>
                    <div class="formfield"><asp:TextBox ID="txtRegion"
                    maxlength="100" runat="server" TabIndex="100"></asp:TextBox></div>                
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel" id="mobile_lable"><label>Mobile</label></div>
	                <div class="formfield" id="mobile_input"><asp:TextBox ID="txtMobile"
                    maxlength="100" runat="server" TabIndex="110" onkeypress="return numberonly(this,event, false, false, true, true);" onblur="CleanInput(this);"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>Country</label></div>
                    <div class="formfield"><asp:TextBox ID="txtCountry"
                    maxlength="100" runat="server" TabIndex="120"></asp:TextBox></div>                
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel" id="photo_lable"><label>Logo</label></div>
                    <div class="formfield" id="photo_input"><asp:FileUpload id="Txt_FileUpload" 
                            tabIndex="130" runat="server" ForeColor="Gray" Font-Size="16px" ></asp:FileUpload><br>
                        <asp:Label ID="lblNotice" runat="server" 
                            Text="Note: GIf, JPG, PNG, BMP Only. The file size must be under 5 MB." 
                            Width="600px"></asp:Label></div>                
                </td>
                <td>
                    <div class="formlabel"><label>Email Address<span id="Span5" style="display:none;">*</span></label></div>
	                <div class="formfield"><asp:TextBox ID="txtEmail" maxlength="100" runat="server" TabIndex="140"></asp:TextBox></div>                
                </td>
            </tr>
            <tr style="display:none;">
                <td>                    
                    <div class="formlabel"><label>EQR Supervisor</label></div>
                    <div class="formfield"><asp:TextBox ID="txtEQRSupervisor"
                    maxlength="100" runat="server" TabIndex="150"></asp:TextBox></div>
                </td>
                <td>                    
                    <div class="formlabel"><label>Accreditation</label></div>
                    <div class="formfield"><asp:TextBox ID="txtAccreditation"
                    maxlength="100" runat="server" TabIndex="160"></asp:TextBox></div>
                </td>
            </tr>            
            <tr>
                <td>                    
                    <div class="formlabel"><label>GST Number</label></div>
                    <div class="formfield"><asp:TextBox ID="tbxGSTNumber"
                    maxlength="100" runat="server" TabIndex="165"></asp:TextBox></div>
                </td>
                <td style="display:none;">                    
                    <div class="formlabel"><label>Accreditation Number</label></div>
                    <div class="formfield"><asp:TextBox ID="tbxAccreditationNumber"
                    maxlength="100" runat="server" TabIndex="166"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
		        <td colspan="2">
                <br />
                <hr />
                <br />
                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone" Display="None" ErrorMessage="Phonet Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revFax" runat="server" ControlToValidate="txtFax" Display="None" ErrorMessage="Fax Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile" Display="None" ErrorMessage="Mobile Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:ValidationSummary ID="vsPersonalInformation" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 50%" align="left">&nbsp;</td>
				        <td style="width: 50%" align="right"><span class="Apple-converted-space">
                            <asp:Button ID="btnSave" runat="server" Text="Save" Font-Size="16px" TabIndex="170" />
                            <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" 
                                Font-Size="16px" TabIndex="180" />&nbsp;</span></td>
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