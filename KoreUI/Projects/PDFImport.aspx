<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="PDFImport.aspx.vb" Inherits="Projects_PDFImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <div runat="server" id="divTitle">
            <a style="padding:8px;float:right;" id="help" href="<% if request("archived") = "1" then %>https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm<% Elseif request("scoped") = "1" then %>https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm<% Else %>https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm<% End If %>" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
            <% If Request("scoped") = "1" Then%>
            <a style="padding:8px;float:right;" id="A1" href="PDFImport.aspx" title="PDF Import"><img src="../images/add-project.png" width="40" height="40" border="0" alt="PDF Import" /></a>
            <% End If%>
            <% If Request("archived") <> "1" And Request("scoped") <> "1" Then%>
            <a style="padding:8px;float:right;" id="healthsafety" href="../contents/HealthSafety.aspx" title="Health Safety"><img src="../images/healthsafety-but.png" width="40" height="40" border="0" alt="Health Safety" /></a>
            <% End If%>
            <a style="padding:8px;float:right;" id="addproject" href="edit.aspx" title="Add Project"><img src="../images/add-project.png" width="40" height="40" border="0" alt="Add Project" /></a>
            <a style="padding:8px;float:right;" id="archives" href="view.aspx?archived=1" title="Project Archives"><img src="../images/archive.png" width="40" height="40" border="0" alt="Project Archives" /></a>
            <a style="padding:8px;float:right;" id="calendarview" href="calendarview.aspx" title="Project Planner"><img src="../images/calendar.png" width="40" height="40" border="0" alt="Project Planner" /></a>
            </div> 
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="center" style="height:200px; margin-top:180px;">
        <asp:Label ID="lbPlease" runat="server" Text="Please select your EQC PDF to import"></asp:Label>
        <asp:FileUpload id="Txt_FileUpload" runat="server" ForeColor="Gray" Font-Size="16px" ></asp:FileUpload>
        <asp:Button ID="btnSave" runat="server" Text="Import" Font-Size="16px" /><br><br>
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    </div>
    <!-- content -->
</asp:Content>

