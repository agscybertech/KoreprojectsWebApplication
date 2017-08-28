<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ProjectGroup.aspx.vb" Inherits="Contacts_ProjectGroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Project Group</title>
    <script type="text/javascript">
        function numberonly(ctl, evt, allowNagitive, allowDecimal, allowSpace, allowDash) {
            var result = false;
            var k = document.all ? evt.keyCode : evt.which;
            var strValue = ctl.value + String.fromCharCode(k);
            if (k > 47 && k < 58 || k == 8 || k == 127 || k == 0) {
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
      <table style="width: 90%" align="center" cellpadding="4" cellspacing="4" runat="server" id="tblProjectGroup">
            <tr>
		        <td align="left" colspan="2"><h1 id="appointment"><asp:Label ID="lblProjectGroup" runat="server" Text=""></asp:Label></h1></td>
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
                    <div class="formlabel"><label>Email Address<span style="display:none;">*</span></label></div>
	                <div class="formfield"><asp:TextBox ID="txtEmail" maxlength="100" runat="server" TabIndex="30"></asp:TextBox></div>
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
                    <div class="formlabel"><label>Phone</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtPhone"
                    maxlength="100" runat="server" TabIndex="50" onkeypress="return numberonly(this,event, false, false, true, true);" onblur="CleanInput(this);"></asp:TextBox></div>                    
                </td>
                <td>
                    <div class="formlabel" id="ph_lable"><label>City</label></div>
	                <div class="formfield" id="ph_input"><asp:TextBox ID="txtCity"
                    maxlength="100" runat="server" TabIndex="60"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                     <div class="formlabel"><label>Fax<span style="display:none;">*</span></label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtFax"
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
                     <div class="formlabel" id="mobile_lable"><label>Mobile</label></div>
	                <div class="formfield" id="mobile_input"><asp:TextBox ID="txtMobile"
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
                    <div class="formlabel" id="photo_lable" style="display:none"><label>Logo</label></div>
                    <div class="formfield" id="photo_input" style="display:none"><asp:FileUpload id="Txt_FileUpload" 
                            tabIndex="110" runat="server" ForeColor="Gray" Font-Size="16px" ></asp:FileUpload><br>
                        <asp:Label ID="lblNotice" runat="server" 
                            Text="Note: GIf, JPG, PNG, BMP Only. The file size must be under 5 MB." 
                            Width="600px"></asp:Label></div>
                    <div class="formlabel" id="display_lable"><label>Display Order</label></div>
                    <div class="formfield"><asp:TextBox ID="txtDisplayOrder"
                            maxlength="4" runat="server" width="40" onkeypress="return numberonly(this,event, false, false, false, false);" TabIndex="110"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>Country</label></div>
                    <div class="formfield"><asp:TextBox ID="txtCountry"
                    maxlength="100" runat="server" TabIndex="120"></asp:TextBox></div>                
                </td>
            </tr>           
            <tr>
		        <td colspan="2">
                <br />
                <hr />
                <br />
                <asp:RequiredFieldValidator ID="rfvBusinessName" runat="server" ErrorMessage="Business Name is required." ControlToValidate="txtBusinessName" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required." ControlToValidate="txtEmail" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revBusinessName" runat="server" ControlToValidate="txtBusinessName" Display="None" ErrorMessage="Business Name is not valid." ValidationExpression="^[a-zA-Z''-'\-\s]{1,40}$"></asp:RegularExpressionValidator>                            
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" Display="none" ErrorMessage="Email is not valid." ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone" Display="None" ErrorMessage="Phonet Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revFax" runat="server" ControlToValidate="txtFax" Display="None" ErrorMessage="Fax Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile" Display="None" ErrorMessage="Mobile Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:ValidationSummary ID="vsPersonalInformation" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 50%" align="left"><span class="Apple-converted-space"><asp:Button ID="btnDelete" runat="server" 
                                Text="Delete" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete this group?');" TabIndex="140" />&nbsp;</span></td>
				        <td style="width: 50%" align="right"><span class="Apple-converted-space">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" Font-Size="16px" TabIndex="150" />
                            <asp:Button ID="btnSave" runat="server" Text="Update" Font-Size="16px" TabIndex="160" />
                            <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" 
                                Font-Size="16px" TabIndex="170" />&nbsp;</span></td>
			        </tr>
		        </table>
		        </td>
	        </tr>
        </table>        
    </div>
    </form>
</body>
</html>
