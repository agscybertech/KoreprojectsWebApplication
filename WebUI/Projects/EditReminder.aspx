<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="EditReminder.aspx.vb" Inherits="Projects_EditReminder" EnableEventValidation="false" %>
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
        function ShowStartTime(allow) {
            if (allow) {
                document.getElementById('divStartDate').style.display = '';
                document.getElementById('divStartDateReminder').style.display = '';
            }
            else {
                document.getElementById('divStartDate').style.display = 'none';
                document.getElementById('divStartDateReminder').style.display = 'none';
            }
        }
    </script>
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <div runat="server" id="divTitle">
            <a style="padding:8px;float:right;" id="help" href="http://my.koreprojects.co.nz/helpconsole2010/Help/default.aspx?pageid=Advanced_Function_1" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
            </div>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
	        <tr>
		        <td align="center">
		        <table width="100%" align="center" cellpadding="3">
                    <tr style="display:none;">
                        <td align="left">
                            <div class="formlabel"><Label>Display Order</Label></div>
                            <div class="formfield"><asp:TextBox ID="txtDisplayOrder" maxlength="4" 
                                    runat="server" width="40" 
                                    onkeypress="return numberonly(this,event, false, false, false, false);" 
                                    TabIndex="1"></asp:TextBox></div>
                        </td>
                        <td align="left">
                            <div class="formlabel"><Label>Status</Label></div>
                            <div class="formfield">
                                <asp:DropDownList ID="ddlStatus" runat="server" Width="100" TabIndex="2">
                                    <asp:ListItem Value="1">Open</asp:ListItem>
                                    <asp:ListItem Value="2">Remind</asp:ListItem>
                                    <asp:ListItem Value="3">Close</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
			        <tr>
				        <td align="left" colspan="2">
	                        <div class="formlabel" id="title_lable"><Label>Note</Label></div>
				            <div class="formfield" id="title_input">
                                <asp:TextBox id="txtReminderTitle" 
                                    runat="server" Width="500px" MaxLength="250" TabIndex="13"></asp:TextBox></div>
                            <asp:RegularExpressionValidator
                                Display="Dynamic" ID="revReminderTitle" runat="server" ErrorMessage="* Invalid entry and no more than 250 characters allows"
                                ControlToValidate="txtReminderTitle" EnableClientScript="true" ValidationGroup="vdgFileUpload"
                                ValidationExpression="^[\s\S]{0,250}$"></asp:RegularExpressionValidator>
				        </td>
			        </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div class="formlabel"><img alt="" src="../images/bell.gif" /> <asp:CheckBox ID="chkSetReminder" runat="server" OnClick="ShowStartTime(this.checked);" /> <label>Set Reminder</label></div>	                        
                            <div id="divStartDate" class="formfield">
                                <asp:TextBox ID="txtValid" runat="server" text="True" style="display:none;"></asp:TextBox>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td><asp:DropDownList ID="ddlStartDateDay" runat="server" AutoPostBack="True" 
                                            Width="70px" TabIndex="3">
                                        </asp:DropDownList></td>
                                                <td><asp:DropDownList ID="ddlStartDateMonth" runat="server" AutoPostBack="True" 
                                            Width="120px" TabIndex="4">
                                        </asp:DropDownList></td>
                                                <td><asp:DropDownList ID="ddlStartDateYear" runat="server" AutoPostBack="True" 
                                            Width="80px" TabIndex="5">
                                        </asp:DropDownList></td>
                                                <td> </td>
                                                <td><asp:DropDownList ID="ddlStartDateHour" runat="server" AutoPostBack="True" 
                                            Width="70px" TabIndex="6">
                                        </asp:DropDownList></td>
                                                <td align="center" valign="middle"><b>:</b></td>
                                                <td><asp:DropDownList ID="ddlStartDateMinute" runat="server" AutoPostBack="True" 
                                            Width="70px" TabIndex="7">
                                        </asp:DropDownList></td>
                                            </tr>
                                        </table>                                        
                                        <asp:TextBox ID="txtStartDateValid" runat="server" text="True" style="display:none;"></asp:TextBox>                            
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divStartDateReminder" class="formlabel" style="vertical-align: middle">
                                <label style="vertical-align: middle">Email Reminder&nbsp;</label>
                                <asp:DropDownList ID="ddlEmailTimeSetting" runat="server" style="vertical-align: middle">
                                    <asp:ListItem Value="0">No email reminder</asp:ListItem>
                                    <asp:ListItem Value="60" Selected="True">1 hr before</asp:ListItem>
                                    <asp:ListItem Value="120">2 hrs before</asp:ListItem>
                                    <asp:ListItem Value="240">4 hrs before</asp:ListItem>
                                    <asp:ListItem Value="720">12 hrs before</asp:ListItem>
                                    <asp:ListItem Value="1440">24 hrs before</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr style="display:none;">
                        <td align="left">
                            <div class="formlabel"><label>Finish Date</label></div>
                            <div class="formfield">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <table>
                                            <tr>
                                                <td><asp:DropDownList ID="ddlFinishDateDay" runat="server" AutoPostBack="True" 
                                            Width="70px" TabIndex="8">
                                        </asp:DropDownList></td>
                                                <td><asp:DropDownList ID="ddlFinishDateMonth" runat="server" AutoPostBack="True" 
                                            Width="120px" TabIndex="9">
                                        </asp:DropDownList></td>
                                                <td><asp:DropDownList ID="ddlFinishDateYear" runat="server" AutoPostBack="True" 
                                            Width="80px" TabIndex="10">
                                        </asp:DropDownList></td>
                                                <td> </td>
                                                <td><asp:DropDownList ID="ddlFinishDateHour" runat="server" AutoPostBack="True" 
                                            Width="70px" TabIndex="11">
                                        </asp:DropDownList></td>
                                                <td align="center" valign="middle"><b>:</b></td>
                                                <td><asp:DropDownList ID="ddlFinishDateMinute" runat="server" AutoPostBack="True" 
                                            Width="70px" TabIndex="12">
                                        </asp:DropDownList></td>
                                            </tr>
                                        </table>
                                        <asp:TextBox ID="txtFinishDateValid" runat="server" text="True" style="display:none;"></asp:TextBox>                            
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>                        
                        </td>
                    </tr>
			        <tr style="display:none;">
				        <td align="left" colspan="2">
                            <div class="formlabel"><Label>Text</Label></div>
    				        <div class="formfield">
                                <asp:TextBox id="txtText" runat="server" Width="500px" 
                                    Height="200" TextMode="MultiLine" TabIndex="14"></asp:TextBox></div>                            
				        </td>
			        </tr>
			        </table><br />
		        </td>
	        </tr>
	        <tr>
		        <td>
                <asp:CompareValidator ID="cvStartDateValid" runat="server" ControlToCompare="txtValid" ControlToValidate="txtStartDateValid" Display="none" ErrorMessage="Start Date is not valid."></asp:CompareValidator>
                <asp:CompareValidator ID="cvFinishDateValid" runat="server" ControlToCompare="txtValid" ControlToValidate="txtFinishDateValid" Display="none" ErrorMessage="Finish Date is not valid."></asp:CompareValidator>
                <asp:RegularExpressionValidator Display="Dynamic" ID="revText" runat="server" ErrorMessage="* Invalid entry and no more than 4000 characters allows"
                    ControlToValidate="txtText" EnableClientScript="true" ValidationGroup="vdgFileUpload" ValidationExpression="^[\s\S]{0,4000}$"></asp:RegularExpressionValidator>                            		        
                <asp:ValidationSummary ID="vsPersonalInformation" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
                <table style="width: 100%">
			        <tr>
				        <td width="30%">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" 
                                onclientclick="return confirm('Are you sure you want to delete this note?');" /></td>
                        <td align="center">
                            <asp:Button ID="btnDone" runat="server" Text="Done" /><asp:Label ID="lblDone" runat="server" Text="Done"></asp:Label>
                        </td>
				        <td width="30%" align="right"><span class="Apple-converted-space">
                            <asp:Button id="btnSave" runat="server" Text="Save" TabIndex="15"></asp:Button>
				        <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" />&nbsp;</span></td>
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