<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="View.aspx.vb" Inherits="Projects_View" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="UC" TagName="ProjectsGrid" Src="~/WebControls/WebUserControlProjectsGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="ScopesGrid" Src="~/WebControls/WebUserControlProjectsGrid.ascx" %>
<%@ Register TagPrefix="UC" TagName="ArchivesGrid" Src="~/WebControls/WebUserControlProjectsGrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
<script type="text/javascript" language="javascript">
    function controlEnter(obj, event) {
        var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
        if (keyCode == 13) {
            document.getElementById(obj).click();
            return false;
        }
        else {
            return true;
        }
    }
</script>

    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        <h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
        <div runat="server" id="divTitle">
        <a style="padding:8px 25px 8px 8px;float:right;" id="help" href="<% if request("archived") = "1" then %>http://my.koreprojects.co.nz/helpconsole2010/Help/default.aspx?pageid=Advanced_Function_2<% Elseif request("scoped") = "1" then %>http://my.koreprojects.co.nz/helpconsole2010/Help/default.aspx?pageid=scopes_page<% Else %>http://my.koreprojects.co.nz/helpconsole2010/Help/default.aspx?pageid=projects_page<% End If %>" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>        
        <a style="padding:8px;float:right;" id="addproject" href="edit.aspx" title="Add Project"><img src="../images/add-project.png" width="40" height="40" border="0" alt="Add Project" /></a>
        <a style="padding:8px;float:right;" id="calendarview" href="calendarview.aspx" title="Project Planner"><img src="../images/calendar.png" width="40" height="40" border="0" alt="Project Planner" /></a>
        <% If Request("scoped") = "1" Then%>
        <a style="padding:8px;float:right;" id="A1" href="PDFImport.aspx" title="PDF Import"><img src="../images/pdf-import.png" width="40" height="40" border="0" alt="PDF Import" /></a>
        <% End If%>
        </div> 
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <div align="center" width="100%">
            <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
        </div>
        <h6>PROJECTS</h6>
        <UC:ProjectsGrid ID="ProjectsGrid" runat="server" />
        <h6>SCOPES</h6>
        <UC:ScopesGrid ID="ScopesGrid" runat="server" />
        <h6>ARCHIVES</h6>
        <UC:ArchivesGrid ID="ArchivesGrid" runat="server" />
    </div>
    <!-- content -->

    <!-- footer -->
    <div class="clr" id="Div1">
        <div class="sep" id="Div2"></div>
        	<table width="100%">
                <tr>
                    <td valign="middle" style="padding-right:25px;">
                        <div class="textfield" style="margin-left: 25px;">
                            <asp:TextBox ID="tbxKeyword" runat="server" ></asp:TextBox>
                        </div>
                        <asp:ImageButton ID="ibnSearch" runat="server" class="button_serch" ImageUrl="../images/search-btn.png" /> 
                    </td>
                    <td width="350px">
                        <div runat="server" id="divFooterTitle">
                            <% If Request("archived") <> "1" And Request("scoped") <> "1" Then%>
                            <a style="padding:0 8px 0 8px;float:right;" id="healthsafety" href="../contents/HealthSafety.aspx" title="Health Safety"><img src="../images/healthsafety-but.png" width="40" height="40" border="0" alt="Health Safety" /></a>
                            <% End If%>
                        </div>
                    </td>
                </tr>
            </table> 
        <div class="clr sep" id="Div3"></div>
    </div>
    <!-- footer -->
</asp:Content>

