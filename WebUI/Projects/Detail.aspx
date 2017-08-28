<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="true" CodeFile="Detail.aspx.vb" Inherits="Projects_Detail" EnableEventValidation="false" %>
<%@ Register TagPrefix="UC" TagName="JobGrid" Src="~/WebControls/WebUserControlJobGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="NoteGrid" Src="~/WebControls/WebUserControlNoteGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="TradeNoteGrid" Src="~/WebControls/WebUserControlTradeNoteGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="ScopesApprovedGrid" Src="~/WebControls/WebUserControlScopesApprovedGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="ScopesPendingGrid" Src="~/WebControls/WebUserControlScopesPendingGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="ReorderGrid" Src="~/WebControls/WebUserControlReOrderGrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ContentPlaceHolder1_aGoogleMap").attr("href", $("#ContentPlaceHolder1_aGoogleMap").attr("href").replace("http://maps.google.co.nz", "https://maps.google.co.nz"));
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <asp:Label ID="lblPrintScript" runat="server" Text=""></asp:Label>
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <a style="padding:8px 25px 8px 8px;float:right;" id="help" href="<% if request("sid") <> "" then %>https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm<% Else %>https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm<% End If %>" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
            <a style="padding:8px;float:right;" id="addProject" href="edit.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) %>" title="Edit Project"><img src="../images/add-button.png" width="40" height="40" border="0" alt="Edit Project" /></a>
            <a style="padding:8px;float:right;cursor:pointer;" id="aPrintWorksheet" onclick="javascript:doPrint();" title="Print Worksheet" runat="server"><img src="../images/Printer.png" width="40" height="40" border="0" alt="Print Worksheet" /></a>
            <asp:ImageButton ID="imgScopePricing" style="padding:8px;float:right;cursor:pointer;" runat="server" ImageUrl="../images/toggle-price-but.png" width="40" height="40" border="0" ToolTip="Scope Pricing" />
            <asp:ImageButton ID="imgPrintScope" style="padding:8px;float:right;cursor:pointer;" runat="server" ImageUrl="../images/Printer-costs.png" width="40" height="40" border="0" ToolTip="Print Costing" Visible="false" />
            <a style="padding:8px;float:right;" id="aGoogleMap" title="Map" class="iframe_map" runat="server"><img src="../images/map-button.png" width="40" height="40" border="0" alt="Map" /></a>
            <a style="padding:8px;float:right;" id="calendarview" href="<% =GetCalendarViewUrl() %>" title="Project Planner"><img src="../images/calendar.png" width="40" height="40" border="0" alt="Project Planner" /></a>
            <table style="padding:8px;" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="background-image:url(../images/sub-nav-left.png); width:16px">
                        <img src="../images/sub-nav-left.png" width="16" height="40" style="border:0" />
                    </td>
                    <td style="background-image:url(../images/sub-nav-bg.png); width: 200px;">
                        <asp:LinkButton ID="lbnProjectView" runat="server" >Project Details</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lbnWorksheet" runat="server" >Worksheet</asp:LinkButton>
                    </td>
                    <td style="background-image:url(../images/sub-nav-right.png); width:16px">
                    </td>
                </tr>
            </table>            
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <div align="center" width="100%">
            <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
        </div>

        <!-- patient info -->
        <table id="profile" width="100%" cellpadding="2" cellspacing="2">
            <tr runat="server" id="trProjectTitle">
                <td valign="top">
                    <table width="100%">
                        <tr>
                            <td id="profimg" width="22%"><asp:Image ID="imgPersonalPhoto" runat="server" Height="125" /></td>
                            <td vlign="middle">
                                <table>
                                    <tr>
                                        <td>
                                            <font Size="6"><asp:Label runat="server" id="projectnametitle" name="projectname" size="40" class="inputbox"></asp:Label></font>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle">
                                            <asp:Label runat="server" id="hazard" name="hazard" size="40" class="inputbox"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="25%">
                                <table>
                                    <tr>
                                        <td>
                                            <b>PRIORITY:&nbsp;</b>
                                            <asp:Image ID="Image1" runat="server" />
                                            <asp:Image ID="Image2" runat="server" />
                                            <asp:Image ID="Image3" runat="server" />
                                            <asp:Image ID="Image4" runat="server" />
                                            <asp:Image ID="Image5" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>PROJECT STATUS:&nbsp;</b>
                                            <asp:Label runat="server" id="projectstatus" name="projectstatus" size="40" class="inputbox"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>EQC Claim No:&nbsp;</b>
                                            <asp:Label runat="server" id="eqcclaimnumber" name="eqcclaimnumber" size="40" class="inputbox"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="trProjectView">
                <td valign="top">
                    <table width="100%" height="200">
                        <tr>
                            <td valign="top">
                    <table height="auto">
                        <tr>
                            <td colspan="3" align="left">
                                <h4>PROJECT INFO</h4>
                            </td>
                        </tr>
                        <tr style="display:none;">
                            <td><b>Name:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectname" name="projectname" size="40" class="inputbox"></asp:Label></td>                        
                        </tr>                        
                        <tr runat="server" id="trAssessmentDate">
                            <td><b>Assessment Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectassessmentdate" name="projectassessmentdate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr runat="server" id="trQuotationDate">
                            <td><b>Quotation Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectquotationdate" name="projectquotationdate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr runat="server" id="trGroup">
                            <td><b>Project Group:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="lblGroup" name="lblGroup" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr runat="server" id="trScopeDate">
                            <td><b>Scope Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="lblScopeDate" name="lblScopeDate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr runat="server" id="trStartDate">
                            <td><b>Start Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="lblStartDate" name="lblStartDate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr runat="server" id="trEnddDate">
                            <td><b>End Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="lblEndDate" name="lblEndDate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                    </table>                            
                            </td>
                            <td width="33%" valign="top">
                    <table height="auto">
                        <tr>
                            <td colspan="3" align="left">
                                <h4>OWNER DETAILS</h4>
                            </td>
                        </tr>
                        <tr>
                            <td><b>Name:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="fname" name="fname" size="40" class="inputbox"></asp:Label>&nbsp;<asp:Label runat="server" id="lname" name="lname" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Home Phone:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="home" name="home" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Work Phone:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="work" name="work" size="40" class="inputbox"></asp:Label></td>
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
                            <td width="33%" valign="top">
                    <table height="auto">
                        <tr>
                            <td colspan="3" align="left">
                                <h4>LOCATION</h4>
                            </td>
                        </tr>
                        <tr>
                            <td><b>Street Address:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="address" name="address" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Suburb:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="suburb" name="suburb" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Town/City:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="city" name="city" size="40" class="inputbox"></asp:Label>&nbsp;<asp:Label runat="server" id="postcode" name="postcode" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Region:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="region" name="region" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Country:&nbsp;</b></td>                            
                            <td><asp:Label runat="server" id="country" name="country" size="40" class="inputbox"></asp:Label></td>
                        </tr>
					</table>                            
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="trScopeTitle">
                <td valign="top">
                    <table border="0" width="100%">
                        <tr>
                            <td valign="bottom">
                                <asp:Image ID="imgScopePhoto" runat="server" Height="35" Visible="false" />
                                <h22><asp:Label runat="server" id="scopenametitle" name="scopename" size="40" 
                                    class="inputbox" ></asp:Label></h22>
                            </td>
                            <td valign="bottom" align="right">
                                <table border="0" cellpadding="4" cellspacing="4">
                                    <tr>
                                        <td valign="bottom" style="display:none">
                                            <b>PRIORITY:&nbsp;</b>
                                            <asp:Image ID="Image7" runat="server" />
                                            <asp:Image ID="Image8" runat="server" />
                                            <asp:Image ID="Image9" runat="server" />
                                            <asp:Image ID="Image10" runat="server" />
                                            <asp:Image ID="Image11" runat="server" />
                                        </td>
                                        <td valign="bottom" style="display:none">
                                            <b>EQC Claim No:&nbsp;</b>
                                            <asp:Label runat="server" id="eqcsclaimnumber" name="eqcsclaimnumber" size="40" class="inputbox"></asp:Label>
                                        </td>
                                        <td valign="bottom" align="right">
                                            <b>PROJECT STATUS:&nbsp;</b>
                                            <asp:Label runat="server" id="scopestatus" name="scopestatus" size="40" class="inputbox"></asp:Label>
                                        </td>
                                        <td valign="bottom" align="right">
                                            <b>SCOPE DATE:&nbsp;</b>
                                            <asp:Label runat="server" id="projectscopedate" name="projectscopedate" size="40" class="inputbox"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
                
        <div runat="server" id="divProjectView">
        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td valign="bottom">
                    <h3 id="job">Project Jobs</h3>
                </td>
                <td valign="bottom" align="right">
                    <a id="MiddleContent_A6" class="form_popup" href="Job.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & string.format("&{0}",Datetime.Now.ToString("yyyyddMMhhmmss")) %>" style="float:right">ADD</a>
                    <a id="A1" href="Job.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) %>" rel="#overlaynote" style="float:right;display:none;">ADD</a>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <UC:JobGrid ID="JobGrid" runat="server" />
                </td>
            </tr>
        </table>
        <br />

        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td valign="bottom">
                    <h3 id="note">Project Notes</h3>
                </td>
                <td valign="bottom" align="right">
                    <a id="MiddleContent_A5" class="form_popup" href="Note.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & string.format("&{0}",Datetime.Now.ToString("yyyyddMMhhmmss")) %>" style="float:right">ADD</a>
                    <a id="A2" href="Note.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) %>" rel="#overlaynote" style="float:right;display:none;">ADD</a>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <UC:NoteGrid ID="NotesGrid" runat="server" />
                </td>
            </tr>
        </table>
        <br />

        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td valign="bottom">
                    <h3 id="trade">Trade Notes</h3>
                </td>
                <td valign="bottom" align="right">
                    <a id="MiddleContent_A2" class="form_popup" href="TradeNote.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & string.format("&{0}",Datetime.Now.ToString("yyyyddMMhhmmss")) %>" style="float:right">ADD</a>
                    <a id="A3" href="TradeNote.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) %>" rel="#overlaynote" style="float:right;display:none;">ADD</a>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <UC:TradeNoteGrid ID="TradeNotesGrid" runat="server" />
                </td>
            </tr>
        </table>
        <br />        

        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td valign="bottom">
                    <h3 id="H1">Reorder</h3>
                </td>
                <td valign="bottom" align="right">
                 
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <UC:ReorderGrid ID="ReorderGrid1" runat="server" />
                </td>
            </tr>
        </table>
        <br />

        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td valign="bottom">
                    <h3 id="file">Project Files</h3>
                </td>
                <td valign="bottom" align="right">
                    <a id="button" class="form_popup" href="UploadFile.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & string.format("&{0}",Datetime.Now.ToString("yyyyddMMhhmmss")) %>" style="float:right">UPLOAD</a><a id="MiddleContent_A1" href="UploadFile.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) %>" rel="#overlayfile" style="float:right;display:none;">UPLOAD</a>
                </td>
            </tr>
            <tr>
                <td colspan="2">
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
                </td>
            </tr>
        </table>
        </div>

        <div runat="server" id="divScopeView">
        <table width="100%" cellpadding="2" cellspacing="2">
            <tr>
                <td valign="bottom">
                    <h3>Work Approved</h3>
                </td>
                <td valign="bottom" align="right">
                    <a id="MiddleContent_A3" href="../Scopes/Detail.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("SID")) %>" style="float:right">ADD ITEM</a>
                    <a id="MiddleContent_A7" href="../downloadworksheet.ashx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("SID")) %>" style="float:right; margin-right:16px;">DOWNLOAD WORKSHEET</a>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
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
                <td valign="bottom" align="right">
                    <a id="MiddleContent_A4" href="Detail.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) %>&sid=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("SID")) %>&act=<%= m_Cryption.encrypt("approveall",m_Cryption.cryptionkey) %>" style="float:right">APPROVE ALL</a>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                    <UC:ScopesPendingGrid ID="ScopesPendingGrid" runat="server" />
                </td>
            </tr>
        </table>
        <br />
        </div>

        <table width="100%">
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <table id="ScopeDate" cellpadding="4" cellspacing="4">                
                        <tr runat="server" id="trEstimatedTime">
                            <td><b>Estimated Time of Completion:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectestimatedtime" name="projectestimatedtime" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr>
                            <td><b>Project Start Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectstartdate" name="projectstartdate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                        <tr runat="server" id="trFinishDate">
                            <td><b>Project End Date:&nbsp;</b></td>
                            <td><asp:Label runat="server" id="projectfinishdate" name="projectfinishdate" size="40" class="inputbox"></asp:Label></td>
                        </tr>
                    </table>
                </td>
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
    </div>
    <!-- content -->

    <!-- footer -->
    <div class="clr" id="Div1">
        <div class="sep" id="Div2"></div>
        	<table width="100%" style="height:45px">
                <tr>
                    <td>
                        <div runat="server" id="divFooterTitle">
                            <asp:LinkButton ID="lbnGridView" runat="server" style="padding:0 8px 0 8px;float:right;" title="Grid View"><img src="../images/project-list-btn.png" alt="List View" border="0" /></asp:LinkButton>
                            <asp:LinkButton ID="lbnGroupView" runat="server" style="padding:0 8px 0 8px;float:right;" title="Group View"><img src="../images/project-group-btn.png" alt="Group View" border="0" /></asp:LinkButton>
                        </div> 
                    </td>
                </tr>
            </table> 
        <div class="clr sep" id="Div3"></div>
    </div>
    <!-- footer -->
</asp:Content>

