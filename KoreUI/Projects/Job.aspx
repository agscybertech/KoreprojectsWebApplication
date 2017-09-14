<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Job.aspx.vb" Inherits="Projects_Job" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div>
        <table style="width: 90%" align="center">
	        <tr>
		        <td><h1 id="appointment"><asp:Label ID="lblJob" runat="server" Text=""></asp:Label></h1></td>
	        </tr>
	        <tr>
		        <td align="center">
		        <table style="width: 600px" align="center" cellpadding="3">
			        <tr>
				        <td style="width: 50%; vertical-align:top;">
	                       <asp:Label id="lblTitle" runat="server" Text="Title"></asp:Label>
				        </td>
				        <td style="width: 50%" colspan="2">
                            <asp:DropDownList ID="ddlProjectJobSettings" runat="server">
                            </asp:DropDownList><br />
                            <div id="divTitle" runat="server"></div>
	                        <div id="divJobSetting" runat="Server"><asp:TextBox id="tbTitle" runat="server" MaxLength="200" Width="200"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbAddJobSetting" runat="server" Text="Add to Job List" /></div>
                            <asp:RegularExpressionValidator
                                Display="Dynamic" ID="revTitle" runat="server" ErrorMessage="* Invalid entry and no more than 50 characters allows "
                                ControlToValidate="tbTitle" EnableClientScript="true" ValidationGroup="vdgJob"
                                ValidationExpression="^.{1,50}$"></asp:RegularExpressionValidator>                            
				        </td>
			        </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td style="width: 50%; vertical-align:top;">
                            <asp:Label id="lblJobAssigneeName" runat="server" Text="Assignee"></asp:Label>
                        </td>
                        <td colspan="2">
                            <div id="lblJobAssignee" runat="server"></div>
                            <asp:DropDownList ID="ddlJobAssignee" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr><td></td></tr>
                    <tr>
                        <td style="width: 50%; vertical-align:top;">
                            Due Date
                        </td>
                        <td style="width: 50%">
                            <asp:TextBox ID="tbDueDate" runat="server" Width="80px"></asp:TextBox>
                            <asp:CalendarExtender ID="ceDueDate" runat="server" TargetControlID="tbDueDate" Format="MM/dd/yyyy">
                            </asp:CalendarExtender>
                        </td>
                    </tr>
			        <tr>
				        <td style="width: 50%" valign="top">
	                       <asp:Label id="lblContent" runat="server" Text="Note"></asp:Label>
				        </td>
				        <td style="width: 50%">
	                       <asp:TextBox ID="tbxMessage" runat="server" Width="400" Height="200" TextMode="MultiLine"></asp:TextBox>
                           <asp:RegularExpressionValidator
                                Display="Dynamic" ID="revJob" runat="server" ErrorMessage="* Invalid entry and no more than 4000 characters allows "
                                ControlToValidate="tbxMessage" EnableClientScript="true" ValidationGroup="vdgJob"
                                ValidationExpression="^[\s\S]{0,4000}$"></asp:RegularExpressionValidator>
				        </td>
			        </tr>
                    <tr>
                        <td style="width: 50%" valign="top">
                            <asp:Label id="lblJobStatus" runat="server" Text="Status"></asp:Label>
                        </td>
				        <td style="width: 50%">
                            <asp:DropDownList ID="ddlJobStatus" runat="server" TabIndex="170">
                            </asp:DropDownList>
                        </td>
                    </tr>
			        </table><br />
		        </td>
	        </tr>
	        <tr>
		        <td>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 50%"><span class="Apple-converted-space"><asp:Button ID="btnDelete" runat="server" 
                                Text="Delete" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete this job?');" />&nbsp;</span></td>
				        <td style="width: 50%" align="right"><span class="Apple-converted-space"><asp:Button ID="btnAdd" runat="server" Text="Add" ValidationGroup="vdgJob" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" ValidationGroup="vdgJob" />
				        <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="False" />&nbsp;</span></td>
			        </tr>
		        </table>
		        </td>
	        </tr>
        </table>
    </div>
    <script type="text/javascript">
        function addUser(sourceID, targetID) {
            //if (document.getElementById(targetID).value == "") {

            //} else {

            //}
            if (document.getElementById(sourceID).value != "-2") {
                if (document.getElementById(document.getElementById(sourceID).value) == null) {
                    var sourceDDL = document.getElementById(sourceID);
                    var sourseText;
                    for (var i = 0; i < sourceDDL.options.length; i++) {
                        if (sourceDDL.options[i].value === document.getElementById(sourceID).value) {
                            sourseText = sourceDDL.options[i].innerText;
                            break;
                        }
                    }
                    var userdiv = document.createElement('div');
                    userdiv.setAttribute('id', document.getElementById(sourceID).value);
                    userdiv.innerHTML = sourseText + " <input type='hidden' name='UserNames' value='" + sourseText + "' /><input type='hidden' name='UserIDs' value='" + document.getElementById(sourceID).value + "' /><a onclick=\"removeUser(\'" + document.getElementById(sourceID).value + "\',\'" + targetID + "\');\" title='Remove' style='cursor:pointer;'>X</a>";
                    document.getElementById(targetID).appendChild(userdiv);
                    //document.getElementById(targetID).appendChild("<div id='" + document.getElementById(sourceID).value + "' name='" + document.getElementById(sourceID).value + "'>" + sourseText + " <input type='hidden' name='UserNames' value='" + sourseText + "' /><input type='hidden' name='UserIDs' value='" + document.getElementById(sourceID).value + "' /><a onclick='removeUser('" + document.getElementById(sourceID).value + "');' title='Remove'>X</a></div>");
                }
            }
        }

        function removeUser(sourceID, targetID) {
            var tDiv = document.getElementById(targetID);
            var sDiv = document.getElementById(sourceID);
            tDiv.removeChild(sDiv);
        }

        function addJob(sourceID, controlID, targetID, hideID) {
            var sourceDDL = document.getElementById(sourceID);
            var sourseText;
            for (var i = 0; i < sourceDDL.options.length; i++) {
                if (sourceDDL.options[i].value === document.getElementById(sourceID).value) {
                    sourseText = sourceDDL.options[i].innerText;
                    break;
                }
            }
            if (document.getElementById(sourceID).value > 0) {
                document.getElementById(targetID).innerHTML = '';
                document.getElementById(controlID).value = '';
                if (document.getElementById('JobSetting' + document.getElementById(sourceID).value) == null) {
                    var userdiv = document.createElement('div');
                    userdiv.setAttribute('id', 'JobSetting' + document.getElementById(sourceID).value);
                    userdiv.innerHTML = sourseText + " <input type='hidden' name='JobTitle' value='" + sourseText + "' /><input type='hidden' name='JobValue' value='" + document.getElementById(sourceID).value + "' /><a onclick=\"removeJob(\'" + sourceID + "\',\'" + controlID + "\',\'" + targetID + "\',\'" + hideID + "\');\" title='Remove' style='cursor:pointer;'>X</a>";
                    document.getElementById(targetID).appendChild(userdiv);
                    document.getElementById(controlID).value = sourseText;
                    document.getElementById(hideID).style.display = 'none';
                }
            } else {
                document.getElementById(targetID).innerHTML = '';
                document.getElementById(controlID).value = '';
                document.getElementById(hideID).style.display = '';
                document.getElementById(controlID).focus();
            }
        }

        function removeJob(sourceID, controlID, targetID, hideID) {
            var tDiv = document.getElementById(targetID);
            var sDiv = document.getElementById('JobSetting' + document.getElementById(sourceID).value);
            tDiv.removeChild(sDiv);
            document.getElementById(sourceID).value = -2;
            document.getElementById(controlID).value = '';
            document.getElementById(hideID).style.display = '';
            document.getElementById(controlID).focus();
        }
    </script>
    </form>
</body>
</html>

