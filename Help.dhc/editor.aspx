<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="editor.aspx.vb" Inherits="HelpConsole2010.tools_skineditor" validateRequest="false" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <title>Editor</title>
    <link href="pagestyles.css" type="text/css" rel="stylesheet"/>
   	<link href="../_engine/AppStyles.css" type="text/css" rel="stylesheet"/>
    <script type="text/javascript" src="../_engine/editor.js"></script>
    <script type="text/javascript" src="../_engine/default.js"></script>
    <script type="text/javascript" src="../_engine/page.js"></script>
   	<script type="text/javascript">
   	    function OnClientLoad(editor) {
   	        //editor.fire("ToggleScreenMode"); 
   	        ShowToolbar(editor,"minimal")
   	        //Resize the editor
   	        Resize_Editor(editor)
   	        //show the editor
   	        //document.getElementById("editor1").style.visibility = "visible";
   	        //alert("less button = " + editor.getToolByName("Less").get_element().style.display);
   	        }

        function GetRadWindow()   
            {   
            var oWindow = null;   
            if (window.radWindow)   
                oWindow = window.radWindow;   
            else if (window.frameElement.radWindow)   
                oWindow = window.frameElement.radWindow;   
            return oWindow;   
            }  
            
        function ShowToolbar(editor,toolbar) {
            var type=document.getElementById("txtEditType").value
            //if the type is not 'skin' then hide the 'edit stylesheet' icon
//            if (type != "skin")
//                {
//                editor.getToolByName("Editstylesheet").get_element().style.display="none";
//                //document.getElementById("spanRegionHeight").style.display="none";
//                }
                
            if (toolbar == "full")
                {
                //Show modules
                var buttonsHolder1 = $get(editor.get_id() + "Module");          
                buttonsHolder1.style.display = ""
                //Show all toolbar buttons        
                editor.getToolByName("More").get_element().style.display="none";
                editor.getToolByName("Less").get_element().style.display="";
                disp="";
                }
            else
                {
                //Hide modules (exit if there was a problem getting a reference to the editor
                if (editor == "none") {return}

                var buttonsHolder1 = $get(editor.get_id() + "Module");          
                buttonsHolder1.style.display = "none"
                //Hide all toolbar buttons        
                editor.getToolByName("More").get_element().style.display="";
                editor.getToolByName("Less").get_element().style.display="none";
                disp="none";
                }
            //Show or hide the toolbar items
            editor.getToolByName("ImageManager").get_element().style.display=disp;
            editor.getToolByName("FlashManager").get_element().style.display=disp;
            editor.getToolByName("DocumentManager").get_element().style.display=disp;
            editor.getToolByName("InsertTable").get_element().style.display=disp;
   	        editor.getToolByName("InsertCondition").get_element().style.display=disp;
  	        editor.getToolByName("LinkPage").get_element().style.display=disp;
            editor.getToolByName("FontName").get_element().style.display=disp;
            editor.getToolByName("FontSize").get_element().style.display=disp;
            editor.getToolByName("Bold").get_element().style.display=disp;
            editor.getToolByName("Underline").get_element().style.display=disp;
            editor.getToolByName("Italic").get_element().style.display=disp;
            editor.getToolByName("BackColor").get_element().style.display=disp;
            editor.getToolByName("JustifyLeft").get_element().style.display=disp;
            editor.getToolByName("JustifyCenter").get_element().style.display=disp;
            editor.getToolByName("JustifyRight").get_element().style.display=disp;
            editor.getToolByName("ForeColor").get_element().style.display=disp;
            editor.getToolByName("LinkManager").get_element().style.display=disp;
            editor.getToolByName("InsertVariable").get_element().style.display=disp;
            //fire the toogle full screen function so that the modules are display correctly
   	        //editor.fire("ToggleScreenMode");
            //editor.fire("ToggleScreenMode");
   	        if (type == "variable") 
   	            {
       	        //If this is the variable editor, hide the 'insert variable' icon
   	            editor.getToolByName("InsertVariable").get_element().style.display="none";
   	            }
   	        else if(type == "skin")
   	            {
       	        //If this is the skin editor, hide appropriate icons
       	        editor.getToolByName("InsertCondition").get_element().style.display="none";
       	        editor.getToolByName("LinkPage").get_element().style.display="none";
       	        //editor.getToolByName("InsertVariable").get_element().style.display = "none";
       	    }
            }   

//    function SaveSkinRegion()
//        {
//        //load the region height into txtRegionHeight_fromServer so that it can be read from the server
//        document.getElementById("txtRegionHeight_fromServer").value = document.getElementById("txtRegionHeight").value;
//        //do postback which causes the skin region to be saved.
//        document.getElementById("txtaction").value = "save";
//        __doPostBack("","");
//        }                          	        

//    function LoadSkinRegion(editor, args)
//        {
//        //var name = args.get_name(); //The command name
//        //var val = args.get_value(); //The tool that initiated the command
//        var sSelectedIndex = document.getElementById("lstSkinRegion").options.selectedIndex
//	    var name = document.getElementById("lstSkinRegion").options[sSelectedIndex].text;
//	    var val = document.getElementById("lstSkinRegion").options[sSelectedIndex].value;
//	    
//        document.getElementById("txtaction").value = "loadregion";
//        document.getElementById("txtRegionName").value = val;
//        __doPostBack("","");

//        }
        
    function CloseForm()
        {
        var projectname = GetProjectName();
        var type=document.getElementById("txtEditType").value;
        if (type == "skin")
            {
            window.location = "../_engine/tools_skins.aspx?projectname="+projectname;
            }
        else if (type == "variable")
            {
            window.location = "../_engine/tools_variables.aspx?projectname="+projectname;
            }
        else if (type == "template")
            {
            window.location = "../_engine/tools_PageTemplates.aspx?projectname="+projectname;
            }

        }        
        
    function EditStyleSheet(skin)
        {
        //get the projectname
        var projectname = GetProjectName();
        window.location = "../_engine/tools_Styles.aspx?projectname="+projectname+"&stylesheet=skin&skin="+skin+"&callingpage=editor";
        }            

    function Resize_Editor(editor) {
        if (navigator.appName == "Microsoft Internet Explorer") {
            var pageHeight = getBrowserHeight() - 103;
        }
        else {
            var pageHeight = getBrowserHeight() - 36;
        }
        
        //resize the editor
        if (editor == null)
            {
            var editor = $find("editor1");
            }

            var pageWidth = document.getElementById("tblheader").offsetWidth-2;
            //var pageWidth = document.getElementById("tblheader").offsetWidth - 10;
            editor.setSize(pageWidth,pageHeight)
        }

    function SavePage()
        {
        //hide the header and editor so that the form doesn't appear to flicker while saving
        document.getElementById("tblheader").style.visibility="hidden";
        document.getElementById("editor1").style.visibility="hidden";
        }

   	</script>
   	
   	

</head>
<body id="body" class="editor_body"  runat="server">
    <form id="Form1" runat="server">
    <input id="txtRegionName" runat="server" type="hidden" />
    <input id="txtaction" runat="server" type="hidden" />
    <input id="txtEditType" runat="server" type="hidden" />
    <table id="tblheader" runat="server" style="width:100%;height:36px;background-color:#2E2E2E" cellpadding="0" cellspacing="6">
        <tr id="trMessage" runat="server" style="height:40px;display:none">
            <td colspan="7" valign="top" id="tdMessage" runat="server"></td>
        </tr>
        <tr>
            <td id="tdName" runat="server" style="width:200px">Name: <input id="txtName" runat="server" class="textbox_black"/></td>
            <td id="tdRegion" runat="server" style="width:200px">Edit Region:
                <asp:DropDownList ID="lstSkinRegion" runat="server" class="textbox_black" AutoPostBack="True">
                    <asp:ListItem Value="tdheader">Header</asp:ListItem>
                    <asp:ListItem Value="tdcontentsheader">Contents Header</asp:ListItem>
                    <asp:ListItem value="tdcontentsfooter">Contents Footer</asp:ListItem>
                    <asp:ListItem value="tdpageheader">Page Header</asp:ListItem>
                    <asp:ListItem value="tdpagefooter">Page Footer</asp:ListItem>
                    <asp:ListItem value="tdfooter">Footer</asp:ListItem>
                </asp:DropDownList></td>
                
            <td id="tdHeight" runat="server">Height: <input id="txtRegionHeight" class="textbox_black" runat="server" style="width:50px;"/></td>
            <td id="tdEditStylesheet" runat="server" style="width:100px">
                <table style="width:100px;height:22px;background-image:url(../_engine/images/button_back_large.gif)" cellpadding="0" cellspacing="0">
                    <tr>
                        <td id="tdEditStylesheet_button" runat="server">Edit StyleSheet</td>
                    </tr>
                </table>
            </td>
            <td>&nbsp;</td>
            <td style="width:100px;" align="right">
                <table style="width:100px;height:22px;background-image:url(../_engine/images/button_back_large.gif)" cellpadding="0" cellspacing="0">
                    <tr>
                        <td onclick="CloseForm()" style="cursor:pointer;text-align:center");>Cancel</td>
                    </tr>
                </table>
            </td>
            <td style="width:100px;" align="right">
                <asp:Button ID="btnSave" runat="server" Text="Save" BackColor="Transparent" BorderColor="Transparent" BorderStyle="None" style="background-image:url(../_engine/images/button_back_large.gif);cursor:pointer" Font-Size="8pt" ForeColor="#E0E0E0" Height="22px" Width="100px" OnClientClick="SavePage()"/>
            </td>
        </tr>
    </table>
    
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
    <telerik:RadEditor ID="editor1"  runat="server" skin="Black"  ToolsFile="../_engine/toolsfile_editor.xml" OnClientLoad="OnClientLoad"><Content></Content></telerik:RadEditor>
  
    
	<script type="text/javascript">
	    //Telerik.Web.UI.Editor.CommandList["FileSave"] = function(commandName, editor, args) {SaveSkinRegion()}
        Telerik.Web.UI.Editor.CommandList["More"] = function(commandName, editor, args) {ShowToolbar(editor,"full")}
        Telerik.Web.UI.Editor.CommandList["Less"] = function(commandName, editor, args) {ShowToolbar(editor,"minimal")}
        Telerik.Web.UI.Editor.CommandList["LinkPage"] = function(commandName, editor, args) {linkpage(commandName, editor, args)}
        Telerik.Web.UI.Editor.CommandList["InsertCondition"] = function(commandName, editor, args) {InsertCondition(commandName,editor)}
        Telerik.Web.UI.Editor.CommandList["InsertVariable"] = function(commandName, editor, args) {InsertVariable(commandName,editor)}  
        
        //Telerik.Web.UI.Editor.CommandList["LoadSkinRegion"] = function(commandName, editor, args) {LoadSkinRegion(editor,args)}
        //Telerik.Web.UI.Editor.CommandList["Editstylesheet"] = function(commandName, editor, args) {EditStyleSheet()}
        

	</script>

    </form>
</body>
</html>

