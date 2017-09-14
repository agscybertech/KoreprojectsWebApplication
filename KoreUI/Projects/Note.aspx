<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Note.aspx.vb" Inherits="Projects_Note" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Note</title>
</head>
<body>
    <form id="frmNote" runat="server">
    <div>
        <table style="width: 90%" align="center">
	        <tr>
		        <td><h1 id="appointment"><asp:Label ID="lblNote" runat="server" Text=""></asp:Label></h1></td>
	        </tr>
	        <tr>
		        <td align="center">
		        <table style="width: 600px" align="center" cellpadding="3">
			        <tr style="display:none;">
				        <td style="width: 50%">
	                       <asp:Label id="lblTitle" runat="server" Text="Title"></asp:Label>
				        </td>
				        <td style="width: 50%">
	                       <asp:TextBox id="tbTitle" runat="server" MaxLength="200" Width="500"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                Display="Dynamic" ID="revTitle" runat="server" ErrorMessage="* Invalid entry and no more than 50 characters allows "
                                ControlToValidate="tbTitle" EnableClientScript="true" ValidationGroup="vdgNoted"
                                ValidationExpression="^.{1,50}$"></asp:RegularExpressionValidator>
				        </td>
			        </tr>
                    <tr>
                        <td>
                            <asp:Label id="lblProjectStatus" runat="server" Text="Status"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlProjectStatus" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
			        <tr>
				        <td style="width: 50%" valign="top">
	                       <asp:Label id="lblContent" runat="server" Text="Notes"></asp:Label>
				        </td>
				        <td style="width: 50%">
	                       <asp:TextBox ID="tbxMessage" runat="server" Width="500" Height="200" TextMode="MultiLine"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                Display="Dynamic" ID="revNote" runat="server" ErrorMessage="* Invalid entry and no more than 4000 characters allows "
                                ControlToValidate="tbxMessage" EnableClientScript="true" ValidationGroup="vdgNoted"
                                ValidationExpression="^[\s\S]{0,4000}$"></asp:RegularExpressionValidator>
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
                                Text="Delete" CausesValidation="False" OnClientClick="return confirm('Are you sure you want to delete this note?');" />&nbsp;</span></td>
				        <td style="width: 50%" align="right"><span class="Apple-converted-space"><asp:Button ID="btnAdd" runat="server" Text="Add" ValidationGroup="vdgNoted" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update" ValidationGroup="vdgNoted" />
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