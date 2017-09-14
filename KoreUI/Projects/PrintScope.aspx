<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrintScope.aspx.vb" Inherits="Projects_PrintScope" %>
<%@ Register TagPrefix="UC" TagName="ScopesApprovedGrid" Src="~/WebControls/WebUserControlPrintScopesApprovedGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="ScopesPendingGrid" Src="~/WebControls/WebUserControlPrintScopesPendingGrid.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Scope Print</title>
    <script src="../Scripts/jquery.tools.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.mousewheel-3.0.4.pack.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
    <link href="../Styles/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/template-print.css" rel="stylesheet" type="text/css" />
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
    <div class="contentwrap admin" style="width:90%;">
        <table id="profile" width="100%" cellpadding="2" cellspacing="2">
            <tr runat="server" id="trScopeTitle">
                <td valign="top">
                    <table border="0" width="100%">
                        <tr>
                            <td valign="bottom">
                                <asp:Image ID="imgScopePhoto" runat="server" Height="35" Visible="false" />
                                <h22><asp:Label runat="server" id="scopenametitle" name="scopename" size="40" 
                                    class="inputbox" ></asp:Label></h22><br />
                                <asp:Label runat="server" id="address" name="address" Font-Size="Smaller" class="inputbox"></asp:Label>
                            </td>
                            <td valign="bottom" align="right">
                                <table border="0" cellpadding="4" cellspacing="4" runat="server" id="tblEQCDetails">
                                    <tr>
                                        <td valign="bottom" style="display:none">
                                            <b>PRIORITY:&nbsp;</b>
                                            <asp:Image ID="Image7" runat="server" />
                                            <asp:Image ID="Image8" runat="server" />
                                            <asp:Image ID="Image9" runat="server" />
                                            <asp:Image ID="Image10" runat="server" />
                                            <asp:Image ID="Image11" runat="server" />
                                        </td>
                                        <td valign="bottom">
                                            <b>EQC Claim No:&nbsp;</b>
                                            <asp:Label runat="server" id="eqcsclaimnumber" name="eqcsclaimnumber" size="40" class="inputbox"></asp:Label>
                                        </td>
                                        <td valign="bottom" align="right" style="display:none">
                                            <b>STATUS:&nbsp;</b>
                                            <asp:Label runat="server" id="scopestatus" name="scopestatus" size="40" class="inputbox"></asp:Label>
                                        </td>
                                        <td valign="bottom" align="right" style="display:none">
                                            <b>Scope Date:&nbsp;</b>
                                            <asp:Label runat="server" id="projectscopedate" name="projectscopedate" size="40" class="inputbox"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table border="0" width="90%" runat="server" id="tblPropertyOwnerDetails">
                        <tr>
                            <td><b>Name:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="fname" name="fname" size="40" class="inputbox"></asp:Label>&nbsp;<asp:Label runat="server" id="lname" name="lname" size="40" class="inputbox"></asp:Label></td>
                            <td width="20px"></td>
                            <td><b>Project Start Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectstartdate" name="projectstartdate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Home Phone:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="home" name="home" size="40" class="inputbox"></asp:Label></td>
                            <td width="20px"></td>
                            <td><b>Project End Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectfinishdate" name="projectfinishdate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Work Phone:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="work" name="work" size="40" class="inputbox"></asp:Label></td>
                            <td width="20px"></td>
                            <td style="display:none"><b>Estimated Time of Completion:&nbsp;</b></td>
                            <td style="display:none"><asp:Label runat="server" id="projectestimatedtime" name="projectestimatedtime" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Mobile:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="mob" name="mob" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Email:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="email" name="email" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
   
        <div runat="server" id="divScopeView">
        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td valign="bottom">
                    <h3>Work Approved</h3>
                </td>
                <td valign="bottom" align="right"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <UC:ScopesApprovedGrid ID="ScopesApprovedGrid" runat="server" />
                </td>
            </tr>
        </table>
        <br />

        <div runat="server" id="divScopePending">
        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td valign="bottom">
                    <h3>Work Pending Approval</h3>
                </td>
                <td valign="bottom" align="right"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <UC:ScopesPendingGrid ID="ScopesPendingGrid" runat="server" />
                </td>
            </tr>
        </table>
        <br />
        </div>

        <table width="100%">
            <tr>
                <td align="right">
                    <table id="ScopePrice" cellpadding="4" cellspacing="4" runat="server">                
                        <tr>
                            <td>
                            
                            </td>
                            <td>
                                <b>Pending Total</b>
                            </td>
                            <td>
                            
                            </td>
                            <td>
                                <b>Approved Total</b>
                            </td>
                            <td>
                            
                            </td>
                            <td>
                                <b>Grand Total</b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                ex GST
                            </td>
                            <td align="right">
                                <asp:Label ID="lblPEX" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                            
                            </td>
                            <td align="right">
                                <asp:Label ID="lblAEX" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                            
                            </td>
                            <td align="right">
                                <b><asp:Label ID="lblGEX" runat="server" Text=""></asp:Label></b>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Incl GST
                            </td>
                            <td align="right">
                                <asp:Label ID="lblPIN" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                            
                            </td>
                            <td align="right">
                                <asp:Label ID="lblAIN" runat="server" Text=""></asp:Label>
                            </td>
                            <td>
                            
                            </td>
                            <td align="right">
                                <b><asp:Label ID="lblGIN" runat="server" Text=""></asp:Label></b>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <div class="apple_overlaymap" id="overlaymap">

        <!-- the external content is loaded inside this tag -->
        <div class="contentWrap"></div>

    </div>

    <div class="apple_overlaynote" id="overlaynote">

        <!-- the external content is loaded inside this tag -->
        <div class="contentWrap"></div>

    </div>

    <div class="apple_overlayfile" id="overlayfile">

        <!-- the external content is loaded inside this tag -->
        <div class="contentWrap"></div>

    </div>

    <!-- make all links with the 'rel' attribute open overlays -->
    <script type="text/javascript">

        $(function () {

            // if the function argument is given to overlay,
            // it is assumed to be the onBeforeLoad event listener
            $("a[rel]").overlay({

                mask: 'darkgrey',
                effect: 'apple',
                onClose: function () {
                    //window.location.reload(true);
                    window.location = self.location;
                    //history.go(1);
                },

                onBeforeLoad: function () {
                    $('#overlay div.contentWrap').html('');
                    $('#overlaynote div.contentWrap').html('');
                    $('#overlayfile div.contentWrap').html('');

                    // grab wrapper element inside content
                    var wrap = this.getOverlay().find(".contentWrap");

                    // load the page specified in the trigger
                    wrap.load(this.getTrigger().attr("href"));
                }

            });
        });
    </script>
    <!-- fancybox popups -->
    <script type="text/javascript">
        $(document).ready(function () {
            $(".iframe_map").fancybox({
                'width': '95%',
                'height': '95%',
                'autoScale': false,
                'transitionIn': 'none',
                'transitionOut': 'none',
                'type': 'iframe'
            });
            $(".form_popup").fancybox({
                'autoScale': false,
                'transitionIn': 'none',
                'transitionOut': 'none',
            });
        });
    </script>
    </form>
</body>
</html>
