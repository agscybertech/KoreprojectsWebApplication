<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Print.aspx.vb" Inherits="Projects_Print" %>
<%@ Register TagPrefix="UC" TagName="NoteGrid" Src="~/WebControls/WebUserControlNoteGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="TradeNoteGrid" Src="~/WebControls/WebUserControlTradeNoteGrid.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Print</title>
    <script src="../Scripts/jquery.tools.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.mousewheel-3.0.4.pack.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
    <link href="../Styles/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/template.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/template-adm.css" rel="stylesheet" type="text/css" />
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
    <div class="contentwrap admin" style="width:90%;">
        <table id="profile" width="100%" cellpadding="2" cellspacing="2">
            <tr valign="top">
                <td>
                    <table width="100%">
                        <tr>
                            <td id="profimg" width="25%"><asp:Image ID="imgPersonalPhoto" runat="server" /></td>
                            <td align="center"><font Size="12"><asp:Label runat="server" id="projectnametitle" name="projectname" size="40" class="inputbox"></asp:Label></font><br /><asp:Label runat="server" id="hazard" name="hazard" size="40" class="inputbox"></asp:Label></td>
                            <td width="25%"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <table width="100%">
                        <tr>
                            <td valign="top">
                    <table>
                        <tr>
                            <td colspan="3" align="left">
                                <h4>PROJECT INFO</h4>
                            </td>
                        </tr>
                        <tr>
                            <td id="label"><b>Project Name:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectname" name="projectname" size="40" class="inputbox"></asp:Label></td>                        
                        </tr>
                        <tr>
                            <td id="Td1"><b>EQC Claim No:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="eqcclaimnumber" name="eqcclaimnumber" size="40" class="inputbox"></asp:Label></td>                        
                        </tr>
                        <tr>
                            <td id="Td2"><b>Project Start Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectstartdate" name="projectstartdate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="Td3"><b>Project Assessment Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectassessmentdate" name="projectassessmentdate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="Td4"><b>Project Status:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectstatus" name="projectstatus" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="Td5"><b>Priority:&nbsp;</b></td>
                            <td>
                                <asp:Image ID="Image1" runat="server" />
                                <asp:Image ID="Image2" runat="server" />
                                <asp:Image ID="Image3" runat="server" />
                                <asp:Image ID="Image4" runat="server" />
                                <asp:Image ID="Image5" runat="server" />
                            </td>
                        </tr>
                    </table>                            
                            </td>
                            <td width="30%" valign="top">
                    <table id="profdetail" width="auto" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td colspan="3" align="left">
                                <h4>OWNER DETAILS</h4>
                            </td>
                        </tr>
                        <tr>
                            <td id="Td6"><b>Name:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="fname" name="fname" size="40" class="inputbox"></asp:Label>&nbsp;<asp:Label runat="server" id="lname" name="lname" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="lable"><b>Home Phone:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="home" name="home" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="Td7"><b>Work Phone:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="work" name="work" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="Td8"><b>Mobile:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="mob" name="mob" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="Td9"><b>Email:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="email" name="email" size="40" class="inputbox"></asp:Label></td>
                        </tr>
					</table>                            
                            </td>
                            <td width="30%" valign="top">
                    <table id="Table1" width="auto" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td colspan="3" align="left">
                                <h4>LOCATION</h4>
                            </td>
                        </tr>
                        <tr>
                            <td id="Td10"><b>Street Address:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="address" name="address" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="Td11"><b>Suburb:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="suburb" name="suburb" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="Td12"><b>Town/City:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="city" name="city" size="40" class="inputbox"></asp:Label>&nbsp;<asp:Label runat="server" id="postcode" name="postcode" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="Td13"><b>Region:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="region" name="region" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td id="Td14"><b>Country:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="country" name="country" size="40" class="inputbox"></asp:Label></td>
                        </tr>
					</table>                            
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
   
        <h2 id="file">Project Files</h2>
        <asp:Label ID="lblScript" runat="server" Text=""></asp:Label>
        <asp:Repeater id="files" runat="server">
            <HeaderTemplate>
                <table border="0" cellspacing="5" cellpadding="5" <%=ShowWidth() %>>                
            </HeaderTemplate>
            <ItemTemplate>
                    <asp:Literal ID="litItem" runat="server" OnDataBinding="litItem_DataBinding" />
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
        <br />        

        <h2 id="note">Project Notes</h2>
        <UC:NoteGrid ID="NotesGrid" runat="server" />
        <br />

        <h2 id="H1">Trade Notes</h2>
        <UC:TradeNoteGrid ID="TradeNotesGrid" runat="server" />
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
