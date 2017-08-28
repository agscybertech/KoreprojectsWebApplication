<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UploadFile.aspx.vb" Inherits="Projects_UploadFile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Upload File</title>
</head>
<body>
    <form id="frmUploadFile" runat="server">
    <div>
        <table style="width: 90%" align="center">
	        <tr>
		        <td><h1 id="appointment">UPLOAD FILE</h1></td>
	        </tr>
	        <tr>
		        <td align="center">
		        <table style="width: 400px" align="center" cellpadding="3">
			        <tr>
				        <td align="left">
	                        <div class="formlabel" id="title_lable"><asp:Label id="lblTitle" runat="server" Width="128px" Font-Bold="False" Text="Title"></asp:Label></div>
				            <div class="formfield" id="title_input"><asp:TextBox id="txt_Description" runat="server" Width="50%" MaxLength="50"></asp:TextBox></div>
                            <asp:RegularExpressionValidator
                                Display="Dynamic" ID="revTitle" runat="server" ErrorMessage="* Invalid entry and no more than 50 characters allows"
                                ControlToValidate="txt_Description" EnableClientScript="true" ValidationGroup="vdgFileUpload"
                                ValidationExpression="^.{1,50}$"></asp:RegularExpressionValidator>
				        </td>
			        </tr>
			        <tr>
				        <td align="left">
	                        <div class="formlabel" id="file_lable"><asp:Label id="lblSelect" runat="server" Font-Bold="False" Text="Select File"></asp:Label></div>
				            <div class="formfield" id="file_input"><asp:FileUpload id="Txt_FileUpload" runat="server" ForeColor="Gray"></asp:FileUpload>
                            <br />
                            <asp:Label ID="lblNotice" runat="server" Font-Size="x-Small" Text="Note: GIf, JPG, PNG, BMP and PDF Only.<br>The file size must be under 5 MB."></asp:Label></div>
				        </td>
			        </tr>
			        </table><br />
		        </td>
	        </tr>
	        <tr>
		        <td>
		        <table style="width: 100%">
			        <tr>
				        <td style="width: 50%"></td>
				        <td style="width: 50%" align="right"><span class="Apple-converted-space"><asp:Button id="btn_Upload" runat="server" Text="Upload" ValidationGroup="vdgFileUpload"></asp:Button>
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

