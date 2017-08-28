<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ServiceGroup.aspx.vb" Inherits="Contacts_ServiceGroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Service Group</title>
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
    <form id="frmServiceGroup" runat="server">
    <div>
        <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
	        <tr>
		        <td align="left"><h1 id="appointment"><asp:Label ID="lblServiceGroup" runat="server" Text=""></asp:Label></h1></td>
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
                        <td align="left" style="display:none">
                            <label>Access</label>
                        </td>
			        </tr>
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="txtServiceGroup"
                            maxlength="100" runat="server" width="180" TabIndex="1" ></asp:TextBox>                            
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDisplayOrder"
                            maxlength="4" runat="server" width="40" TabIndex="2" onkeypress="return numberonly(this,event, false, false, false, false);"></asp:TextBox>
                        </td>
                        <td align="left" style="display:none">
                            <asp:DropDownList ID="ddlPublicAccess" runat="server" TabIndex="3" width="100">
                            </asp:DropDownList>                            
                        </td>
                    </tr>
                    <tr runat="server" id="trServiceGroupRelationship">
                        <td colspan="3" align="left">
                            <h2 id="note">Assign To</h2>
                            <asp:Repeater id="rpServiceGroupRelationship" runat="server">
                                <HeaderTemplate>
                                    <table border="0" cellspacing="5" cellpadding="5" <%=ShowWidth() %>>                
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbServiceGroupRelationship" Runat="server" Enabled="True" EnableViewState="true" Checked='<%# checkBox(DataBinder.Eval(Container, "DataItem.ProjectOwnerID"))%>' Text='<%#DataBinder.Eval(Container, "DataItem.ContactName")%>' TabIndex='<%# showTabIndex() %>'></asp:CheckBox>
                                                <asp:TextBox ID="tbServiceGroupRelationship" runat="server" Text='<%# m_Cryption.encrypt(DataBinder.Eval(Container, "DataItem.ProjectOwnerID"),m_Cryption.cryptionKey)%>' Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>                                        
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
			    </table><br />
		        </td>
	        </tr>
	        <tr>
		        <td>
                <asp:RequiredFieldValidator ID="rfvServiceGroup" runat="server" Enabled="false" ErrorMessage="Service group is required." ControlToValidate="txtServiceGroup" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revServiceGroup" runat="server" ControlToValidate="txtServiceGroup" Display="None" ErrorMessage="Service group is not valid, and no more than 40 characters allows." ValidationExpression="^.{1,40}$"></asp:RegularExpressionValidator>
                <asp:RegularExpressionValidator ID="revDisplayOrder" runat="server" ControlToValidate="txtDisplayOrder" Display="None" ErrorMessage="Display order is not valid." ValidationExpression="^[\d]{1,4}$"></asp:RegularExpressionValidator>
                <asp:ValidationSummary ID="vsArea" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 50%"><span class="Apple-converted-space"><asp:Button ID="btnDelete" runat="server" 
                                Text="Delete" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete this area?');" TabIndex="32000" />&nbsp;</span></td>
				        <td style="width: 50%" align="right"><span class="Apple-converted-space"><asp:Button ID="btnAdd" runat="server" Text="Add" TabIndex="32010" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" TabIndex="32020" />
				        <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" TabIndex="32030" />&nbsp;</span></td>
			        </tr>
		        </table>
		        </td>
	        </tr>
        </table>
    </div>
    </form>
</body>
</html>
