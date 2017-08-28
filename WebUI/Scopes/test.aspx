<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="test.aspx.vb" Inherits="Scopes_test" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</asp:ToolkitScriptManager>
<script  type="text/javascript">
    function update(value) {
        var behavior = $find('dpe1');
        if (behavior) {
            behavior.populate(value);
        }
    }
    </script>
<asp:Panel ID="panel1" runat="server">
  <input type="radio" id="rb1" name="format" value="format1" runat="server" />m-d-y
  <input type="radio" id="rb2" name="format" value="format2" runat="server" />d.m.y
  <input type="radio" id="rb3" name="format" value="format3" runat="server" />y/m/d
</asp:Panel>
<label id="myDate" runat="server" />
<asp:DynamicPopulateExtender ID="dpe1" BehaviorID="dpe1" runat="server" ClearContentsDuringUpdate="true"
  TargetControlID="myDate" 
  ServicePath="../WebServices/DynamicPopulate.vb.asmx" ServiceMethod="getDate" />
</asp:Content>

