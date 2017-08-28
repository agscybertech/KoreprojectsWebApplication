<%@ Page Title="" Language="VB" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="false" CodeFile="SystemRequirements.aspx.vb" Inherits="SystemRequirements" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="60%" border="0">
        <tr>
            <td align="left"><br /><br /><br />
                <asp:Label ID="lblErrorMsg" runat="server" Text="" Font-Size="Larger" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center"><br /><br /><br />
                <table border="0" width="100%" cellspacing="0" cellpadding="0">
                    <tr>
                        <td style="width: 70px">
                            <img src="Images/InternetExplorer_Logo.png" alt="Internet Explorer" height="64" width="64" />
                        </td>
                        <td style="width: 230px">
                            Internet Explorer 6.0 and above
                        </td>
                        <td style="width: 60px">
                            <a href="http://www.microsoft.com/windows/internet-explorer">Download</a>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70px">
                            <img src="Images/Firefox_Logo.png" alt="Firefox" height="64" width="64" />
                        </td>
                        <td style="width: 230px">
                            Firefox 3.0 and above
                        </td>
                        <td style="width: 60px">
                            <a href="http://www.firefox.com">Download</a>
                        </td>                    
                    </tr>
                    <tr>
                        <td style="width: 70px">
                            <img src="Images/Safari_Logo.png" alt="Safari" height="64" width="64" />
                        </td>
                        <td style="width: 230px">
                            Safari 4.0 and above
                        </td>
                        <td style="width: 60px">
                            <a href="http://www.apple.com/safari">Download</a>
                        </td>                    
                    </tr>
                    <tr>
                        <td style="width: 70px">
                            <img src="Images/Chrome_Logo.png" alt="Safari" height="64" width="64" />
                        </td>
                        <td style="width: 230px">
                            Chrome 3.0 and above
                        </td>
                        <td style="width: 60px">
                            <a href="http://www.google.com/chrome">Download</a>
                        </td>                    
                    </tr>
                </table>
                <br /><br /><br />
                <asp:LinkButton ID="lbErrorRelogin" runat="server" 
                    PostBackUrl="~/SystemRequirements.aspx" ForeColor="White">Please relogin again.</asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Content>

