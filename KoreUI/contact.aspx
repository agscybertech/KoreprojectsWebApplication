<%@ page language="C#" autoeventwireup="true" inherits="contact" CodeFile="contact.aspx.cs" %>

<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder2" runat="server">
    <div id="main_wrapper_inner">
</asp:content>
<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
     <style type="text/css">
         .tableheader {
             padding: 5px 5px 5px 5px;
         }

         .tablebody {
             padding: 5px 5px 5px 5px;
         }
     </style>
    <div class="header_inner">
        <h1>Contact Us</h1>
    </div>
    <div class="content_inner">
        <div align="center">
                <table border="0" cellpadding="3">
                    <tr>
                        <td class="tableheader" align="right"><font face="Arial"><b>Company 
				Name </b></font></td>
                        <td class="tablebody" align="left">
                            <asp:TextBox id="CompanyName" width="120" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="tableheader" align="right"><font face="Arial"><b> 
				Full Name </b></font></td>
                        <td class="tablebody" align="left">
                            <asp:TextBox id="Name" width="120" runat="server"></asp:TextBox>
                            </td>
                    </tr>
                    <tr>
                        <td class="tableheader" align="right"><font face="Arial"><b>Email 
				Address </b></font></td>
                        <td class="tablebody" align="left">
                            <input type="text" name="EmailFrom" value="" size="30">
                            * Required</td>
                    </tr>
                    <tr>
                        <td class="tableheader" align="right"><font face="Arial"><b>Phone 
				Number </b></font></td>
                        <td class="tablebody" align="left">
                            <input type="text" name="Phone" value="" size="30">
                            * Required</td>
                    </tr>
                </table>
            </div>
            <p></p>
            <div align="center">
                <table border="0" cellpadding="3" id="table1">
                    <tr>
                        <td valign="top" class="tableheader" align="right"><b>
                            <font face="Arial">Your Message</font></b></td>
                        <td class="tablebody" align="left">
                            <textarea rows="10" name="bodytxt" cols="43"></textarea>&nbsp;&nbsp;</td>
                    </tr>
                    <tr>
                        <td valign="top" class="tableheader" align="right">
                            <b><font face="Arial">Verification</font></b></td>
                        <td class="tablebody" align="left">
                            <img src="captcha/captcha.bmp" id="imgCaptcha" />&nbsp;&nbsp;&nbsp;<a href="javascript:void(0)" onclick="RefreshImage('imgCaptcha')" style="font-size: x-small">Change Image</a><br />
                            <br />
                            <input name="captchacode" type="text" id="captchacode" size="45" value="Write the numbers in the image above" onclick="this.value = ''" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableheader"></td>
                        <td align="right" class="tablebody">
                            <input type="submit" name="Action" value="Submit"></td>
                    </tr>
                </table>
            </div>
        <br>
        <div align="center">
            <h2 style="margin-top: 0"></h2>
            <table width="100%" cellpadding="10" cellspacing="0" border="0">
                <tr>
                    <td align="center">
                        <iframe width="90%" height="400" frameborder="0" scrolling="no" marginheight="0" marginwidth="0" src="map.htm"></iframe>
                    </td>
                </tr>
            </table>
            <p>
                <br>
                <br>
                <strong>Rangiora Business Park</strong><br />
                Unit 12, 6 Cone Street, Rangiora, Christchurch, New Zealand<br />
                Phone: +64 (0)3 745 9119<br />
            </p>
        </div>
    </div>
    <script language="javascript">
        document.forms[0].Name.focus();
    </script>
</asp:content>

