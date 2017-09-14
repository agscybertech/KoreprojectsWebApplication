<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="true" CodeFile="Detail.aspx.vb" Inherits="Scopes_Detail" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript">
        <% = loadWorksheetServices() %>
        <% = loadMeasurements() %>
        <% = loadServiceRates() %>
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

        function updateTextControl(Content, TargetControl) {
            if (Content == 'None') {
                document.getElementById(TargetControl).value = '';
                document.getElementById(TargetControl).style.display = '';
            } else {
                document.getElementById(TargetControl).value = Content;
                document.getElementById(TargetControl).style.display = 'none';
            }
        }

        function updateService(TextContent, SelectionIndex, TargetControl, SelectionControl, DescriptionControl, MeasurementControl, DescriptionContent) {
            if (myWorksheetServices.length > SelectionIndex - 1 && SelectionIndex != 0) {
                document.getElementById(SelectionControl).options.length = 0;
                document.getElementById(SelectionControl).options[0] = new Option(myWorksheetServices[SelectionIndex - 1][2], myWorksheetServices[SelectionIndex - 1][2], false, false);
                document.getElementById(MeasurementControl).value = myWorksheetServices[SelectionIndex - 1][2];
                if (DescriptionContent != '') {
                    if (DescriptionContent.indexOf(" - ") > 0) {
                        document.getElementById(DescriptionControl).value = DescriptionContent;
                    } else {
                        document.getElementById(DescriptionControl).value = myWorksheetServices[SelectionIndex - 1][3] + ' - ' + DescriptionContent;
                    }
                } else {
                    document.getElementById(DescriptionControl).value = myWorksheetServices[SelectionIndex - 1][3];
                }
            } else {
                document.getElementById(SelectionControl).options.length = 0;
                if (myMeasurements.length > 0) {
                    for (i in myMeasurements) {
                        document.getElementById(SelectionControl).options[i] = new Option(myMeasurements[i], myMeasurements[i], false, false);
                    }
                }
                document.getElementById(MeasurementControl).value = myMeasurements[0];
                document.getElementById(DescriptionControl).value = DescriptionContent;
            }
            if (TextContent == 'None') {
                document.getElementById(TargetControl).value = '';
                document.getElementById(TargetControl).style.display = '';
            } else {
                document.getElementById(TargetControl).value = TextContent;
                document.getElementById(TargetControl).style.display = 'none';
            }
        }

        function updateServiceRateControl(SelectionIndex, TargetControl, SelectionControl, RateControl) {
            if (SelectionIndex != 0 && myServiceRates.length > SelectionIndex - 1) {
                document.getElementById(SelectionControl).options.length = 0;
                for (i in myServiceRates[SelectionIndex - 1]) {
                    var RateDetails = myServiceRates[SelectionIndex - 1][i].split(";");
                    document.getElementById(SelectionControl).options[i] = new Option(RateDetails[1], RateDetails[0], false, false);
                }
                document.getElementById(TargetControl).value = RateDetails[3];
            } else {
                document.getElementById(SelectionControl).options.length = 0;
                document.getElementById(SelectionControl).options[0] = new Option("None", "None", false, false);
                document.getElementById(TargetControl).value = "0";
            }
            document.getElementById(RateControl).value = "";
        }

        function updateServiceRates(SelectionIndex, TargetControl, GroupControl, RateControl) {
            var hasRate;
            hasRate = false;
            if (document.getElementById(GroupControl).value != "0") {
                for (i in myServiceRates) {
                    for (j in myServiceRates[i]) {
                        var RateDetails = myServiceRates[i][j].split(";");
                        if (RateDetails[3] == document.getElementById(GroupControl).value && j == SelectionIndex && !hasRate) {
                            document.getElementById(GroupControl).value = RateDetails[3];
                            document.getElementById(TargetControl).value = RateDetails[1];
                            document.getElementById(RateControl).value = RateDetails[2];
                            hasRate = true;
                        }
                    }
                }
            }

            if (!hasRate) {
                document.getElementById(GroupControl).value = "0";
                document.getElementById(TargetControl).value = "";
                document.getElementById(RateControl).value = "";
            }
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
        <div align="center" width="100%">
            <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
        </div>
        <asp:UpdatePanel ID="upArea" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlAssign" EventName="SelectedIndexChanged" />
                </Triggers>
                <ContentTemplate>
                <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
                    <tr>
                        <td colspan="2" align="left">
                            <h22><asp:Label runat="server" id="scopenametitle" name="scopename" size="40" 
                                    class="inputbox" ></asp:Label></h22>
                            <h4>SCOPE DETAILS</h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="formlabel"><label>Area<span style="display:none;">*</span></label></div>
	                        <div class="formfield">
                                <asp:DropDownList ID="ddlArea" runat="server" TabIndex="10">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td valign="bottom">
                            <div class="formlabel"><label></label></div>
	                        <div class="formfield">
                                <asp:TextBox ID="txtArea" maxlength="100" runat="server" TabIndex="20"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="formlabel"><label>Item<span style="display:none;">*</span></label></div>
	                        <div class="formfield">
                                <asp:DropDownList ID="ddlItem" runat="server" TabIndex="30">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td valign="bottom">
                            <div class="formlabel"><label></label></div>
	                        <div class="formfield">
                                <asp:TextBox ID="tbxItem" maxlength="100" runat="server" TabIndex="40"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="formlabel"><label>Service<span style="display:none;">*</span></label></div>
	                        <div class="formfield">
                                <asp:DropDownList ID="ddlService" runat="server" TabIndex="50">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td valign="bottom">
                            <div class="formlabel"><label></label></div>
	                        <div class="formfield">
                                <asp:TextBox ID="txtService" maxlength="100" runat="server" TabIndex="60"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="formlabel"><label>QTY<span style="display:none;">*</span></label></div>
                            <div class="formfield">
    	                        <asp:TextBox ID="tbxQTY" maxlength="100" runat="server" TabIndex="70" Width="150" onkeypress="return numberonly(this,event, false, true, false, false);"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                <asp:DropDownList ID="ddlMeasurement" runat="server" TabIndex="80" Width="100">
                                </asp:DropDownList>
                                <asp:TextBox ID="tbxMeasurement" maxlength="100" runat="server" TabIndex="85"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="formlabel"><label>Notes</label></div>	                
                            <asp:TextBox ID="tbxDescription" runat="server" TabIndex="90" Rows="5" TextMode="MultiLine" Width="90%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
                    <tr>
		                <td>
                            <table style="width: 100%">
			                    <tr>
				                    <td style="width: 50%" align="left"><asp:Button ID="btnQuickDelete" runat="server" 
                                            Text="Delete" CausesValidation="False" Font-Size="16px" TabIndex="100" /></td>
				                    <td style="width: 50%" align="right"><span class="Apple-converted-space">
                                        <asp:Button ID="btnQuickSave" runat="server" Text="Save" Font-Size="16px" TabIndex="110" />
                                        <asp:Button ID="btnQuickCancel" runat="server" Text="Cancel" CausesValidation="False" 
                                            Font-Size="16px" TabIndex="120" />&nbsp;</span></td>
			                    </tr>
		                    </table>
                            <hr />
                            <h6>OPTIONAL</h6>
                            <table style="width: 100%" cellpadding="4" cellspacing="4">
			                    <tr>
                                    <td>
                                        <div class="formlabel"><label>Group<span style="display:none;">*</span></label></div>
	                                    <div class="formfield">
                                            <asp:DropDownList ID="ddlGroup" runat="server" TabIndex="130">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="formlabel"><label>Area Measurement Notes</label></div>
                                        <div class="formfield">
                                            <asp:TextBox ID="tbxRM" maxlength="100" runat="server" TabIndex="140"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="formlabel"><label>Status</label></div>
	                                    <div class="formfield">
                                            <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="150">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="formlabel"><label>Display Order</label></div>
	                                    <div class="formfield">
                                            <asp:TextBox ID="tbxDisplayOrder" maxlength="10" Width="25" runat="server" TabIndex="160" onkeypress="return numberonly(this,event, false, true, false, false);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
		                    </table>
                            <br /><br />
                            <hr style="background-color:Gray;" size="5" />
                            <h4>BILLING DETAILS</h4>
                        </td>
	                </tr>
                </table>
                <table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
                    <tr>
                        <td>
                            <div class="formlabel"><label>Rate Sheet</label></div>
                            <div class="formfield"><asp:DropDownList runat="server" ID="ddlServiceGroup" TabIndex="170"/></div>
                        </td>
                        <td valign="bottom" style="display:none">
                            <div class="formlabel"><label><br /></label></div>
	                        <div class="formfield">
                                <asp:TextBox ID="tbxService" maxlength="100" runat="server" TabIndex="180"></asp:TextBox>
                                <asp:TextBox ID="tbxServiceGroup" runat="server"></asp:TextBox>
                            </div>
                            <br />
                        </td>
                        <td>
                            <div class="formlabel"><label>Service Rate</label></div>
                            <div class="formfield"><asp:DropDownList runat="server" ID="ddlServicesByGroup" TabIndex="185"/></div>
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td>
                            <div class="formlabel"><label>Assign To<span style="display:none;">*</span></label></div>
	                        <div class="formfield">
                                <asp:DropDownList ID="ddlAssign" runat="server" TabIndex="190" 
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td valign="bottom">
                            <div class="formlabel"><label></label></div>
	                        <div class="formfield">
                                <asp:TextBox ID="tbxAssign" maxlength="100" runat="server" TabIndex="200" Visible="false"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="<%=showRate()%>">
                            <div class="formlabel"><label>Rate<span style="display:none;">*</span></label></div>
	                        <div class="formfield">
                                <asp:TextBox ID="tbxRate" maxlength="100" runat="server" TabIndex="210" onkeypress="return numberonly(this,event, false, true, false, false);"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
                <table style="width: 90%; margin-top:50px;" align="center" cellpadding="4" cellspacing="4">
                    <tr>
		                <td>
                        <asp:RequiredFieldValidator ID="rfvArea" runat="server" ErrorMessage="Area is required." ControlToValidate="txtArea" Display="None"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revArea" runat="server" ControlToValidate="txtArea" Display="None" ErrorMessage="Area is not valid, and no more than 40 characters allows." ValidationExpression="^.{1,40}$"></asp:RegularExpressionValidator>
                        <asp:ValidationSummary ID="vsPersonalInformation" ShowMessageBox="true" ShowSummary="false" HeaderText="You must correct error in the following fields:" EnableClientScript="true" runat="server"/>
		                <table style="width: 100%">
			                <tr>
				                <td style="width: 50%" align="left"><asp:Button ID="btnDelete" runat="server" 
                                        Text="Delete" CausesValidation="False" Font-Size="16px" TabIndex="220" /></td>
				                <td style="width: 50%" align="right"><span class="Apple-converted-space">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" Font-Size="16px" TabIndex="230" />
                                    <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" 
                                        Font-Size="16px" TabIndex="240" />&nbsp;</span></td>
			                </tr>
		                </table>
		                </td>
	                </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
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

