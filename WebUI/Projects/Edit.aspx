<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="Edit.aspx.vb" Inherits="Projects_Edit" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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
        function CopyName() {
            document.getElementById("ContentPlaceHolder1_txtAddress").value = document.getElementById("ContentPlaceHolder1_txtProjectName").value;
        }
    </script>
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <div runat="server" id="divTitle">
            <a style="padding:8px 25px 8px 8px;float:right;" id="help" href="https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
            </div>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
            <tr>
                <td colspan="2" align="left">
                    <h4>PROJECT DETAILS</h4>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Project Name<span>*</span></label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtProjectName"
                    maxlength="100" runat="server" TabIndex="1"></asp:TextBox>
                    </div>
                </td>
                <td valign="top">
                    <div class="formlabel" style="width:300px;"><label>Reference No<span>*</span></label> <font size="1">(PO No. / EQC Claim No. / Insurance No.)</font></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtEQCClaimNumber"
                    maxlength="100" runat="server" TabIndex="2"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <div class="formlabel"><label>Project Group</label></div>
	                <div class="formfield">
                        <asp:DropDownList ID="ddlGroup" runat="server" TabIndex="23">
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div class="formlabel"><label>Scope Date</label></div>
                    <div class="formfield">                        
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlScopeDateDay" runat="server" AutoPostBack="True" 
                                    Width="70px" TabIndex="3">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlScopeDateMonth" runat="server" AutoPostBack="True" 
                                    Width="120px" TabIndex="4">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlScopeDateYear" runat="server" AutoPostBack="True" 
                                    Width="80px" TabIndex="5">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtScopeDateValid" runat="server" text="True" style="display:none;"></asp:TextBox>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                    <div class="formlabel" style="display:none;"><label>Assessment Date</label></div>
                    <div class="formfield" style="display:none;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlAssessmentDateDay" runat="server" AutoPostBack="True" 
                                    Width="70px" TabIndex="6">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlAssessmentDateMonth" runat="server" 
                                    AutoPostBack="True" Width="120px" TabIndex="7">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlAssessmentDateYear" runat="server" AutoPostBack="True" 
                                    Width="80px" TabIndex="8">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtAssessmentDateValid" runat="server" text="True" style="display:none;"></asp:TextBox>                            
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel" style="display:none;"><label>Quotation Date</label></div>
                    <div class="formfield" style="display:none;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlQuotationDateDay" runat="server" AutoPostBack="True" 
                                    Width="70px" TabIndex="12">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlQuotationDateMonth" runat="server" AutoPostBack="True" 
                                    Width="120px" TabIndex="13">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlQuotationDateYear" runat="server" AutoPostBack="True" 
                                    Width="80px" TabIndex="14">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtQuotationDateValid" runat="server" text="True" style="display:none;"></asp:TextBox>                            
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>            
            <tr>
                <td>
                    <div class="formlabel"><label>Start Date<span>*</span></label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtValid" runat="server" text="True" style="display:none;"></asp:TextBox>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlStartDateDay" runat="server" AutoPostBack="True" 
                                    Width="70px" TabIndex="9">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlStartDateMonth" runat="server" AutoPostBack="True" 
                                    Width="120px" TabIndex="10">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlStartDateYear" runat="server" AutoPostBack="True" 
                                    Width="80px" TabIndex="11">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtStartDateValid" runat="server" text="True" style="display:none;"></asp:TextBox>                            
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>                
                </td>
                <td>
                    <div class="formlabel"><label>Finish Date</label></div>
                    <div class="formfield">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlFinishDateDay" runat="server" AutoPostBack="True" 
                                    Width="70px" TabIndex="15">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlFinishDateMonth" runat="server" AutoPostBack="True" 
                                    Width="120px" TabIndex="16">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlFinishDateYear" runat="server" AutoPostBack="True" 
                                    Width="80px" TabIndex="17">
                                </asp:DropDownList>
                                <asp:TextBox ID="txtFinishDateValid" runat="server" text="True" style="display:none;"></asp:TextBox>                            
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr style="display:none">
                <td>
                    <div class="formlabel"><label>Estimated Time of Completion</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtEstimatedTime"
                    maxlength="100" runat="server" TabIndex="18"></asp:TextBox>
                    </div>
                </td>                
            </tr>
            <tr>                
                <td>
                    <div class="formlabel"><label>Priority</label></div>
	                <div class="formfield">  
                        <asp:Rating ID="PriorityRating" runat="server"                            
                    MaxRating="5"
                    StarCssClass="ratingStar"
                    WaitingStarCssClass="savedRatingStar"
                    FilledStarCssClass="filledRatingStar"
                    EmptyStarCssClass="emptyRatingStar"                            
                    style="float: left;" TabIndex="19" /></div>
                </td>
                <td>
                    <div class="formlabel"><label>Project Status</label></div>
	                <div class="formfield">
                        <asp:DropDownList ID="ddlProjectStatus" runat="server" TabIndex="20">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="formlabel"><label>Hazard</label></div>
	                <asp:TextBox ID="txtHazard" Width="90%" maxlength="100" runat="server" 
                        TabIndex="21" Rows="4" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel" id="photo_lable"><label>Project Photo</label></div>
                    <div class="formfield" id="photo_input">
                        <asp:FileUpload id="Txt_FileUpload" 
                            tabIndex="22" runat="server" ForeColor="Gray" Font-Size="16px" ></asp:FileUpload><br>
                        <asp:Label ID="lblNotice" runat="server" 
                            Text="Note: GIf, JPG, PNG, BMP Only. The file size must be under 5 MB." 
                            Width="600px"></asp:Label></div>
                </td>                
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    <hr />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table width="100%">
                        <tr>
                            <td><h4>CLIENT DETAILS</h4></td>
                            <td align="right"><h5>(Optional)</h5></td>
                        </tr>
                    </table>                    
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>First Name<span style="display:none;">*</span></label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtFirstName"
                    maxlength="100" runat="server" TabIndex="23"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel"><label>Last Name<span style="display:none;">*</span></label></div>
	                <div class="formfield">
                        <asp:TextBox ID="txtLastName"
                    maxlength="100" runat="server" TabIndex="24"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Home Phone</label></div>
                    <div class="formfield">
                        <asp:TextBox ID="txtHomePhone"
                    maxlength="100" runat="server" TabIndex="25" 
                            onkeypress="return numberonly(this,event, false, false, true, true);" onblur="CleanInput(this);"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel" id="Div11"><label>Work Phone<span style="display:none;">*</span></label></div>
	                <div class="formfield" id="Div12">
                        <asp:TextBox ID="txtWorkPhone"
                    maxlength="100" runat="server" TabIndex="26" 
                            onkeypress="return numberonly(this,event, false, false, true, true);" onblur="CleanInput(this);"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel" id="mobile_lable"><label>Mobile</label></div>
	                <div class="formfield" id="mobile_input"><asp:TextBox ID="txtMobile"
                    maxlength="100" runat="server" TabIndex="27" 
                            onkeypress="return numberonly(this,event, false, false, true, true);" onblur="CleanInput(this);"></asp:TextBox></div>
                </td>
                <td>
                    <div class="formlabel" id="ph_lable"><label>Email Address<span id="Span5" style="display:none;">*</span></label></div>
	                <div class="formfield" id="ph_input"><asp:TextBox ID="txtEmail" maxlength="100" 
                            runat="server" TabIndex="28"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                    <hr />
                    <br />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table width="100%">
                        <tr>
                            <td><h4>PROJECT LOCATION</h4></td>
                            <td align="right"><h5>(Optional)</h5></td>
                        </tr>
                    </table>                    
                </td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Street Address
                        </label></div>
                    <input style="float:right; font-size: xx-small;" type="button" 
                        value="Same as Name" onclick="javascript: CopyName();" />
                    <asp:Button ID="btnSameAsName" style="float:right" runat="server" 
                            Text="Same as Name" Font-Size="7pt" Visible="false" />
	                <div class="formfield">
                        <asp:TextBox ID="txtAddress"
                    maxlength="100" runat="server" TabIndex="29" AutoPostBack="True"></asp:TextBox></div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Country</label></div>
	                <div class="formfield">
                        <asp:DropDownList id="ddlPAC" runat="server" TabIndex="30">
                        </asp:DropDownList>
                        <asp:cascadingdropdown id="CascadingDropDown1" runat="server" targetcontrolid="ddlPAC" servicepath="~/WebServices/AddressService.asmx" servicemethod="GetSelectedCountries" prompttext="Please select a country" loadingtext="[Loading countries...]" category="Country">
                        </asp:cascadingdropdown>
                    </div>
                </td>
                <td>
                    <div class="formlabel" id="email_lable"><label>Region</label></div>
	                <div class="formfield" id="email_input">
                        <asp:DropDownList ID="txtRegion" runat="server" TabIndex="31">
                        </asp:DropDownList>
                        <asp:CascadingDropDown id="CascadingDropDown2" runat="server" parentcontrolid="ddlPAC" targetcontrolid="txtRegion" servicepath="~/WebServices/AddressService.asmx" servicemethod="GetRegions" prompttext="Please select a region" loadingtext="[Loading regions...]" category="Region">
                        </asp:CascadingDropDown>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="formlabel"><label>Town/City</label></div>
	                <div class="formfield">
                        <asp:DropDownList ID="txtDistrict" runat="server" TabIndex="32">
                        </asp:DropDownList>
                        <asp:CascadingDropDown id="CascadingDropDown3" runat="server" parentcontrolid="txtRegion" targetcontrolid="txtDistrict" servicepath="~/WebServices/AddressService.asmx" servicemethod="GetDistricts" prompttext="Please select a district" loadingtext="[Loading districts...]" category="District">
                        </asp:CascadingDropDown>
                    </div>
                </td>
                <td>
                    <div class="formlabel" id="Div15"><label>Suburb</label></div>
	                <div class="formfield" id="Div16">
                        <asp:DropDownList ID="txtSuburb" runat="server" TabIndex="33">
                        </asp:DropDownList>
                        <asp:CascadingDropDown id="CascadingDropDown4" runat="server" parentcontrolid="txtDistrict" targetcontrolid="txtSuburb" servicepath="~/WebServices/AddressService.asmx" servicemethod="GetTowns" prompttext="Please select a town" loadingtext="[Loading towns...]" category="Town">
                        </asp:CascadingDropDown>
                    </div>
                </td>
            </tr>
	        <tr>
		        <td colspan="2">
                    <br />
                    <hr />
                    <br />
                </td>
	        </tr>
	        <tr>
		        <td colspan="2">
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" Enabled="false" ErrorMessage="First Name is required." ControlToValidate="txtFirstName" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" Enabled="false" ErrorMessage="Last Name is required." ControlToValidate="txtLastName" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Enabled="false" ErrorMessage="Email is required." ControlToValidate="txtEmail" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvWorkPhone" runat="server" Enabled="false" ErrorMessage="Work Contact Number is required." ControlToValidate="txtWorkPhone" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revFirstName" runat="server" ControlToValidate="txtFirstName" Display="None" ErrorMessage="First Name is not valid" ValidationExpression="^[a-zA-Z&''-'\-\s]{1,40}$"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revLastName" runat="server" ControlToValidate="txtLastName" Display="None" ErrorMessage="Last Name is not valid." ValidationExpression="^[a-zA-Z&''-'\-\s]{1,40}$"></asp:RegularExpressionValidator>                            
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" Display="none" ErrorMessage="Email is not valid." ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revHomePhone" runat="server" ControlToValidate="txtHomePhone" Display="None" ErrorMessage="Home Contact Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revWorkPhone" runat="server" ControlToValidate="txtWorkPhone" Display="None" ErrorMessage="Work Contact Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile" Display="None" ErrorMessage="Mobile Number is not valid." ValidationExpression="(\d| |\(|\)|-|\+){7,}"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="rfvProjectName" runat="server" ErrorMessage="Project Name is required." ControlToValidate="txtProjectName" Display="None"></asp:RequiredFieldValidator>                
                <asp:RequiredFieldValidator ID="rfvEQCClaimNumber" runat="server" ErrorMessage="Reference No is required." ControlToValidate="txtEQCClaimNumber" Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvStartDateDay" runat="server" ErrorMessage="Start Date Day is required." ControlToValidate="ddlStartDateDay" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvStartDateMonth" runat="server" ErrorMessage="Start Date Month is required." ControlToValidate="ddlStartDateMonth" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvStartDateYear" runat="server" ErrorMessage="Start Date Year is required." ControlToValidate="ddlStartDateYear" Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvScopeDateValid" runat="server" ControlToCompare="txtValid" ControlToValidate="txtScopeDateValid" Display="none" ErrorMessage="Scope Date is not valid."></asp:CompareValidator>
                <asp:CompareValidator ID="cvStartDateValid" runat="server" ControlToCompare="txtValid" ControlToValidate="txtStartDateValid" Display="none" ErrorMessage="Start Date is not valid."></asp:CompareValidator>
                <asp:CompareValidator ID="cvAssessmentDateValid" runat="server" ControlToCompare="txtValid" ControlToValidate="txtAssessmentDateValid" Display="none" ErrorMessage="Assessment Date is not valid."></asp:CompareValidator>
                <asp:CompareValidator ID="cvQuotationDateValid" runat="server" ControlToCompare="txtValid" ControlToValidate="txtQuotationDateValid" Display="none" ErrorMessage="Quotation Date is not valid."></asp:CompareValidator>
                <asp:CompareValidator ID="cvFinishDateValid" runat="server" ControlToCompare="txtValid" ControlToValidate="txtFinishDateValid" Display="none" ErrorMessage="Finish Date is not valid."></asp:CompareValidator>
                <asp:ValidationSummary ID="vsPersonalInformation" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 50%" align="left"><asp:Button ID="btnDelete" runat="server" 
                                Text="Delete" CausesValidation="False" Font-Size="16px" TabIndex="36" 
                                OnClientClick="return confirm('Are you sure you want to delete this project?');" /></td>
				        <td style="width: 50%" align="right"><span class="Apple-converted-space">
                            <asp:Button ID="btnSave" runat="server" Text="Save" Font-Size="16px" 
                                TabIndex="34" />
                            <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" 
                                Font-Size="16px" TabIndex="35" />&nbsp;</span></td>
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

