<%@ Page Language="VB" AutoEventWireup="false" CodeFile="HealthSafetyEditor.aspx.vb" Inherits="Contents_HealthSafetyEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Health Safety Editor</title>
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
        function SelectContentType(ctl) {
            var selectedOptionValue = ctl.options[ctl.selectedIndex].value;
            if (selectedOptionValue == 'File') {
                document.getElementById('btnSave').value = 'Upload';
            }
            else {
                document.getElementById('btnSave').value = 'Save';
            }
            var i, optionValue, divTarget;
            for (i = 0; i < ctl.options.length; i++) {
                optionValue = ctl.options[i].value;
                divTarget = document.getElementById(optionValue);
                if (optionValue == selectedOptionValue) {
                    //divTarget.style.visibility = 'visible';
                    divTarget.style.display = '';
                }
                else {
                    //divTarget.style.visibility = 'hidden';
                    divTarget.style.display = 'none';
                }
            }
        }
    </script>
</head>
<body>
    <form id="frmHealthSafety" runat="server">
    <div>
        <table style="width: 90%" align="center">
	        <tr>
		        <td><h1 id="appointment">
                    <asp:Label ID="lblHealthSafety" runat="server"></asp:Label></h1></td>
	        </tr>
	        <tr>
		        <td align="center">
		        <table style="width: 400px" align="center" cellpadding="3">
                    <tr>
                        <td align="left">
                            <div class="formlabel"><asp:Label id="lblDisplayOrder" runat="server" Width="128px" Font-Bold="False" Text="Display Order"></asp:Label></div>
                            <div class="formfield"><asp:TextBox ID="txtDisplayOrder" maxlength="4" runat="server" width="40" onkeypress="return numberonly(this,event, false, false, false, false);"></asp:TextBox></div>
                        </td>
                        <td align="left">
                            <div class="formlabel"><asp:Label id="lblContentType" runat="server" Width="128px" Font-Bold="False" Text="Content Type"></asp:Label></div>
                            <div class="formfield">
                                <asp:DropDownList ID="ddlContentType" runat="server" onchange="SelectContentType(this);">
                                    <asp:ListItem Value="PlanText">Plan Text</asp:ListItem>
                                    <asp:ListItem Value="File">Image File</asp:ListItem>
                                    <asp:ListItem Value="File">Pdf File</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
			        <tr>
				        <td align="left" colspan="2">
	                        <div class="formlabel" id="title_lable"><asp:Label id="lblTitle" runat="server" Width="128px" Font-Bold="False" Text="Title"></asp:Label></div>
				            <div class="formfield" id="title_input"><asp:TextBox id="txtTitle" runat="server" 
                                    Width="500px" MaxLength="50"></asp:TextBox></div>
                            <asp:RegularExpressionValidator
                                Display="Dynamic" ID="revTitle" runat="server" ErrorMessage="* Invalid entry and no more than 50 characters allows"
                                ControlToValidate="txtTitle" EnableClientScript="true" ValidationGroup="vdgFileUpload"
                                ValidationExpression="^[\s\S]{0,50}$"></asp:RegularExpressionValidator>
				        </td>
			        </tr>
			        <tr>
				        <td align="left" colspan="2">
                            <div id="PlanText">
                                <div class="formlabel"><asp:Label id="lblText" runat="server" Font-Bold="False" Text="Text"></asp:Label></div>
    				            <div class="formfield"><asp:TextBox id="txtText" runat="server" Width="500" Height="200" TextMode="MultiLine"></asp:TextBox></div>
                                <asp:RegularExpressionValidator
                                    Display="Dynamic" ID="revText" runat="server" ErrorMessage="* Invalid entry and no more than 4000 characters allows"
                                    ControlToValidate="txtText" EnableClientScript="true" ValidationGroup="vdgFileUpload"
                                    ValidationExpression="^[\s\S]{0,4000}$"></asp:RegularExpressionValidator>
                            </div>
	                        <div id="File" style="display:none;">
                                <div class="formlabel" id="file_lable"><asp:Label id="lblSelect" runat="server" Font-Bold="False" Text="Select File"></asp:Label></div>
		    		            <div class="formfield" id="file_input"><asp:FileUpload id="Txt_FileUpload" runat="server" ForeColor="Gray"></asp:FileUpload>
                                <br />
                                <asp:Label ID="lblNotice" runat="server" Font-Size="x-Small" Text="Note: GIf, JPG, PNG, BMP and PDF Only.<br>The file size must be under 5 MB."></asp:Label></div>                            
                            </div>
				        </td>
			        </tr>
			        </table><br />
		        </td>
	        </tr>
	        <tr>
		        <td>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 50%">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" 
                                onclientclick="return confirm('Are you sure you want to delete this content?');" /></td>
				        <td style="width: 50%" align="right"><span class="Apple-converted-space">
                            <asp:Button id="btnSave" runat="server" Text="Save" 
                                ValidationGroup="vdgFileUpload"></asp:Button>
				        <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" />&nbsp;</span></td>
			        </tr>
		        </table>
		        </td>
	        </tr>
        </table>
    </div>
    </form>
</body>
</html>