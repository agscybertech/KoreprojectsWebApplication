<%@ page title="" language="VB" masterpagefile="~/Projects/MasterPage.master" autoeventwireup="false" inherits="Accounts_Thankyou" CodeFile="Thankyou.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table style="width: 90%" align="center" cellpadding="4" cellspacing="4">
    <tr>
		<td align="left"><h1 id="appointment">Congratulations!</h1></td>
	</tr>
    <tr>
        <td align="center">
            <table style="width: 100%">
			    <tr>
				    <td style="width: 50%"></td>
				    <td style="width: 50%" align="right"><span class="Apple-converted-space"><asp:Button ID="btnContinue" runat="server" Text="Continue"/>&nbsp;</span></td>
			    </tr>
		    </table>
        </td>
    </tr>
</table>
</asp:Content>


