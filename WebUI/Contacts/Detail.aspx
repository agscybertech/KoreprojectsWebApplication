<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="Detail.aspx.vb" Inherits="Contacts_Detail" %>
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
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <asp:ImageButton ID="imgInviteLink" style="padding:8px 25px 8px 8px;float:right;cursor:pointer;" runat="server" ImageUrl="../images/contact-invite.png" width="40" height="40" border="0" ToolTip="Send Link" Visible="false" />
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
                    <div class="formlabel"><label>City</label></div>
	                <div class="formfield"><asp:TextBox ID="txtCity"
                    maxlength="100" runat="server" TabIndex="60"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Phone</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtHomePhone"
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
                    <div class="formlabel" id="Div11"><label>Fax<span style="display:none;">*</span></label></div>
	                <div class="formfield" id="Div12">
                        <asp:TextBox ID="txtWorkPhone"
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
                    <div class="formlabel" id="photo_lable"><label>Photo</label></div>
                    <div class="formfield" id="photo_input"><asp:FileUpload id="Txt_FileUpload" 
                            tabIndex="130" runat="server" ForeColor="Gray" Font-Size="16px" ></asp:FileUpload><br>
                        <asp:Label ID="lblNotice" runat="server" 
                            Text="Note: GIf, JPG, PNG, BMP Only. The file size must be under 5 MB." 
                            Width="600px"></asp:Label></div>
                </td>
                <td>
                    <div class="formlabel" id="ph_lable"><label>Email Address<span id="Span5" style="display:none;">*</span></label></div>
	                <div class="formfield" id="ph_input"><asp:TextBox ID="txtEmail" maxlength="100" runat="server" TabIndex="140"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="formlabel"><label>Notes</label></div>	                
                    <asp:TextBox ID="tbxNote" runat="server" maxlength="100" TabIndex="150" Rows="5" TextMode="MultiLine" Width="90%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Type</label></div>
	                <div class="formfield">
                        <asp:DropDownList ID="ddlType" runat="server" TabIndex="160">
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div class="formlabel"><label>Status</label></div>
	                <div class="formfield">
                        <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="170">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr runat="server" id="trServiceRate" visible="false">
		        <td colspan="2">
                    <h2 id="note">Service Rates<a id="button" class="form_popup" href="ServiceRate.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) %>&sid=<%= m_Cryption.encrypt("0",m_Cryption.cryptionKey) & string.format("&{0}",Datetime.Now.ToString("yyyyddMMhhmmss")) %>" style="float:right">ADD</a></h2>
                    <UC:ServiceRatesGrid ID="ServiceRatesGrid" runat="server" />
                    <br />
                    <hr />
                    <br />
                </td>
	        </tr>
            <tr>
		        <td colspan="2">
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ErrorMessage="First Name is required." ControlToValidate="txtFirstName" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" Enabled="false" ErrorMessage="Last Name is required." ControlToValidate="txtLastName" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required." ControlToValidate="txtEmail" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvHomePhone" runat="server" Enabled="false" ErrorMessage="Phone Number is required." ControlToValidate="txtHomePhone" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revFirstName" runat="server" ControlToValidate="txtFirstName" Display="None" ErrorMessage="First Name is not valid." ValidationExpression="^[a-zA-Z''-'\-\s]{1,40}$"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revLastName" runat="server" ControlToValidate="txtLastName" Display="None" ErrorMessage="Last Name is not valid." ValidationExpression="^[a-zA-Z''-'\-\s]{1,40}$"></asp:RegularExpressionValidator>                            
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" Display="none" ErrorMessage="Email is not valid." ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revHomePhone" runat="server" ControlToValidate="txtHomePhone" Display="None" ErrorMessage="Home Contact Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revWorkPhone" runat="server" ControlToValidate="txtWorkPhone" Display="None" ErrorMessage="Work Contact Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile" Display="None" ErrorMessage="Mobile Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:ValidationSummary ID="vsPersonalInformation" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 33%" align="left">
                            <asp:Button ID="btnRemove" runat="server" 
                                Text="Remove" CausesValidation="False" Font-Size="16px" TabIndex="180" /></td>
                        <td align="center">                        

                        </td>
				        <td style="width: 33%" align="right"><span class="Apple-converted-space">
                            <asp:Button ID="btnInvite" runat="server" Text="Invite" Font-Size="16px" TabIndex="190" Visible="false" />
                            <asp:Button ID="btnCreate" runat="server" Text="Create" Font-Size="16px" 
                                TabIndex="190" />
                            <asp:Button ID="btnSave" runat="server" Text="Save" Font-Size="16px" TabIndex="200" />
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

