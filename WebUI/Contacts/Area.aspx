<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Area.aspx.vb" Inherits="Contacts_Area" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Area</title>
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
    </script>
</head>
<body>
    <form id="frmArea" runat="server">
    <div>    
        <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
	        <tr>
		        <td align="left"><h1 id="appointment"><asp:Label ID="lblArea" runat="server" Text=""></asp:Label></h1></td>
	        </tr>
	        <tr>
		        <td align="center">
		        <table style="width: 600px" align="center" cellpadding="2" cellspacing="2">
			        <tr>
				        <td align="left">
	                       <label>Name<span style="display:none;">*</span></label>
				        </td>
                        <td align="left">
	                       <label>Display Order</label>
				        </td>
			        </tr>
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="txtArea"
                            maxlength="100" runat="server" width="180" TabIndex="10"></asp:TextBox>                            
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDisplayOrder"
                            maxlength="4" runat="server" width="40" onkeypress="return numberonly(this,event, false, false, false, false);" TabIndex="20"></asp:TextBox>
                        </td>
                    </tr>                    
			    </table><br />
		        </td>
	        </tr>
	        <tr>
		        <td>
                <asp:RequiredFieldValidator ID="rfvArea" runat="server" Enabled="false" ErrorMessage="Area is required." ControlToValidate="txtArea" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revArea" runat="server" ControlToValidate="txtArea" Display="None" ErrorMessage="Area is not valid, and no more than 40 characters allows." ValidationExpression="^.{1,40}$"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revDisplayOrder" runat="server" ControlToValidate="txtDisplayOrder" Display="None" ErrorMessage="Display order is not valid." ValidationExpression="^[\d]{1,4}$"></asp:RegularExpressionValidator>
                <asp:ValidationSummary ID="vsArea" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 50%"><span class="Apple-converted-space"><asp:Button ID="btnDelete" runat="server" 
                                Text="Delete" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete this area?');" TabIndex="30" />&nbsp;</span></td>
				        <td style="width: 50%" align="right"><span class="Apple-converted-space"><asp:Button ID="btnAdd" runat="server" Text="Add" TabIndex="40" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" TabIndex="50" />
				        <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" TabIndex="60" />&nbsp;</span></td>
			        </tr>
		        </table>
		        </td>
	        </tr>
        </table>
    </div>
    </form>
</body>
</html>
