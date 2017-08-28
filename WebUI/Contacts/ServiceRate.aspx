<%@ Page Language="VB" AutoEventWireup="true" CodeFile="ServiceRate.aspx.vb" Inherits="Contacts_ServiceRate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Service Rate</title>
</head>
<body>
<script type="text/javascript">
    function checkAllChecked(targetID, controlCount) {
        var isEmpty;
        isEmpty = true;
        for (i = 0; i < controlCount; i++) {
            if (document.getElementById('rpServiceGroup_cbServiceGroup_' + i).checked == true) {
                isEmpty = false;
                break;
            }
        }
        if (isEmpty) {
            document.getElementById('rpServiceGroup_cbServiceGroup_' + targetID).checked = true;
            alert('Please select at least one service group!');
        }
    }
</script>
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
    <form id="frmServiceRate" runat="server">
    <div>
        <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
	        <tr>
		        <td align="left"><h1 id="appointment"><asp:Label ID="lblServiceRate" runat="server" Text=""></asp:Label></h1></td>
	        </tr>
	        <tr>
		        <td align="center">
		        <table style="width: 600px" align="center" cellpadding="2" cellspacing="2">
			        <tr>
				        <td align="left">
	                       <label>Service<span style="display:none;">*</span></label>
				        </td>
                        <td align="left">
                            <label>Display Order</label>
                        </td>
				        <td align="left">
	                       <label>Status</label>
				        </td>
			        </tr>
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="txtService"
                            maxlength="100" runat="server" width="180" TabIndex="1"></asp:TextBox>                            
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDisplayOrder"
                            maxlength="4" runat="server" width="40" onkeypress="return numberonly(this,event, false, false, false, false);" TabIndex="2"></asp:TextBox>
                            <asp:DropDownList ID="ddlService" runat="server" width="180" Visible="false" TabIndex="3">
                            </asp:DropDownList>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlStatus" runat="server" width="150" TabIndex="4">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr><td colspan="3"></td></tr>
                    <tr>
                        <td align="left">
                            <label>Cost Rate</label>
                        </td>
                        <td align="left">
                            <label>Charge Rate</label>
                        </td>
                        <td align="left">
                            <label style="display:none;">Unit</label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:TextBox ID="tbxCost" maxlength="100" runat="server" TabIndex="5" Width="180" onkeypress="return numberonly(this,event, false, true, false, false);"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxCharge" maxlength="100" runat="server" TabIndex="6" Width="180" onkeypress="return numberonly(this,event, false, true, false, false);"></asp:TextBox>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlUnit" runat="server" width="150" TabIndex="7" Visible="false">
                            </asp:DropDownList>                            
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td colspan="3" align="left">
                            <div class="formlabel"><label>Notes</label></div>	                
                            <asp:TextBox ID="tbxNote" runat="server" maxlength="100" TabIndex="8" Rows="5" TextMode="MultiLine" Width="95%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr runat="server" id="trServiceGroup">
                        <td colspan="3" align="left">
                            <h2 id="note">Rate Sheet</h2>
                            <asp:Repeater id="rpServiceGroup" runat="server">
                                <HeaderTemplate>
                                    <table border="0" cellspacing="5" cellpadding="5" <%=ShowWidth() %>>                
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="cbServiceGroup" Runat="server" Enabled="True" EnableViewState="true" Checked='<%# checkBox(DataBinder.Eval(Container, "DataItem.ServiceGroupID"))%>' Text='<%# DataBinder.Eval(Container, "DataItem.Name")%>' TabIndex='<%# showTabIndex() %>'></asp:CheckBox>
                                                <asp:TextBox ID="tbServiceGroup" runat="server" Text='<%# m_Cryption.encrypt(DataBinder.Eval(Container, "DataItem.ServiceGroupID"),m_Cryption.cryptionKey)%>' Visible="false"></asp:TextBox>
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
                <asp:RequiredFieldValidator ID="rfvService" runat="server" Enabled="false" ErrorMessage="Service is required." ControlToValidate="txtService" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revService" runat="server" ControlToValidate="txtService" Display="None" ErrorMessage="Service is not valid, and no more than 40 characters allows." ValidationExpression="^.{1,40}$"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="rfvCost" runat="server" Enabled="false" ErrorMessage="Cost Rate is required." ControlToValidate="tbxCost" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="rfvCharge" runat="server" Enabled="false" ErrorMessage="Charge Rate is required." ControlToValidate="tbxCharge" Display="None"></asp:RequiredFieldValidator>                
                <asp:ValidationSummary ID="vsServiceRate" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 50%"><span class="Apple-converted-space"><asp:Button ID="btnDelete" runat="server" 
                                Text="Delete" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete this service?');" TabIndex="32000" />&nbsp;</span></td>
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
