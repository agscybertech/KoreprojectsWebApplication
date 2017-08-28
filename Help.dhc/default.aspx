<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="default_project.aspx.vb" Inherits="HelpConsole2010.default_project1" validateRequest="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">



<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" /> 
    <script type="text/javascript" src="../_engine/default.js"></script>
    <script type="text/javascript" src="../_engine/editor.js"></script>
    <script type="text/javascript" src="../_engine/page.js"></script>

    <link href="pagestyles.css" type="text/css" rel="stylesheet"/>
    <link href="../_engine/TableLayoutCss.css" type="text/css" rel="stylesheet"/>
    <link href="../_engine/AppStyles.css" type="text/css" rel="stylesheet"/>
    <link rel="shortcut icon" href="images/favicon.ico" type="image/x-icon" />
    <title></title>
    
</head>
<body id="body" class="skin_body" runat="server" >
    <form id="Form1" runat="server" >

    	<input id="txteditid" type="hidden" runat="server" />
    	<input id="txtaction" type="hidden" runat="server" />
    	<input id="txtexternalurl" type="hidden" runat="server" />
    	<input id="txtcontentschanged" type="hidden" runat="server" />
    	<input id="txtpagechanged" type="hidden" runat="server" />
  	   	<input id="NewEditorHTML" type="hidden" runat="server" />
        
        <!-- DIALOG WINDOW -->
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager Behaviors="Close, Move, Resize,Maximize" ID="RadWindowManager" DestroyOnClose="true" KeepInScreenBounds="true" runat="server" Skin="Black" Height="540px" VisibleStatusbar="False" Width="700px"></telerik:RadWindowManager>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                function openNewWindow(surl,w,h)
                    {
                   	var d = new Date()
                    var dummyparm = "";
                    dummyparm = "&dummy=" + d.getHours() + d.getMinutes() + d.getSeconds();
                    surl = surl + dummyparm
                    var oWnd = radopen(surl, "RadWindow1"); 
                    oWnd.setSize(w,h);
                    oWnd.center();
                    oWnd.set_modal(true);
                    }

                function openMessageWindow(surl)
                    {
                    var oWnd = radopen(surl, "RadWindow1");
                    oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move)  
                    oWnd.setSize(300,200);
                    oWnd.center();
                    oWnd.set_modal(true);
                    }                    
                    
                 function openURLWindow(sPageID,external)
                    {
                    var projectname = GetProjectName();
                    //var oWnd = radopen("../_engine/DialogContent.aspx?type=ShowURL&pageid="+sPageID+"&projectname="+projectname+"&external="+external,"RadWindow1");
                    var oWnd = radopen("../_engine/DialogContent.aspx?type=ShowURL&pageid=" + sPageID + "&projectname=" + projectname + "&external=" + external, "RadWindow1");
                    oWnd.set_behaviors(Telerik.Web.UI.WindowBehaviors.Move+Telerik.Web.UI.WindowBehaviors.Close)  
                    oWnd.setSize(575,200);
                    oWnd.center();
                    oWnd.set_modal(true);
                    }                    

            </script>
        </telerik:RadCodeBlock>
    	

    	<!-- DIALOG WINDOW -->
        <table id="tbldialog" runat="server" style="width:100%;height:100%;position:absolute;top:0px;left:0px;z-index:7;display:none">
            <tr>
    	        <td align="center">
                    
                    <table id="tblDialogWindow" runat="server" style="width:500px;height:300px;" cellpadding="0" cellspacing="0">
                        <tr style="height:36px">
                            <td style="width:8px"><img src="../_engine/images/dlg_top_left.gif" alt=""/></td>
                            <td style="background-image:url(../_engine/images/dlg_top_center.gif)">
                                <img id="imgcloseform" runat="server" onmouseover="this.src='../_engine/images/closeform_mouseover.gif'" onmouseout="this.src='../_engine/images/closeform.gif'" src="../_engine/images/closeform.gif" alt="close" style="cursor:hand;" align="right" onclick="CloseDialog()"/>
                                <img id="imgretractform" src="../_engine/images/retractform.gif" alt="Reduce Window Size" style="cursor:hand;display:none" align="right" onclick="RetractDialog()"/>
                                <img id="imgexpandform" src="../_engine/images/expandform.gif" alt="Increase Window Size" style="cursor:hand;display:none" align="right" onclick="ExpandDialog()"/>
                                <div id="spanheading" runat="server" style="font-size:11pt;font-family:Arial;color:#DDDDDD;padding-top:4px;padding-left:2px;text-align:left">Tools</div>
                            </td>
                            <td style="width: 8px"><img src="../_engine/images/dlg_top_right.gif" alt=""/></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../_engine/images/dlg_left_center.gif)"></td>
                            <td id="tddialog" style="background-image:url(../_engine/images/loading.gif);background-position:center center;background-repeat:no-repeat">
                                [content]

                            </td>
                            <td style="background-image: url(../_engine/images/dlg_right_center.gif)"></td>
                        </tr>
                        <tr style="height:8px;">
                            <td><img src="../_engine/images/dlg_bottom_left.gif" alt=""/></td>
                            <td id="tdmessage" style="font-size:9pt;font-family:Tahoma,Arial;background-image:url(../_engine/images/dlg_bottom_center.gif); width: 668px;" runat="server">
                                <span id="spanbreadcrumbs"></span>
                            </td>
                            <td><img src="../_engine/images/dlg_bottom_right.gif" alt=""/></td>
                        </tr>
                    </table>
	            </td>
    	    </tr>
        </table>

        <!-- LOGIN -->
   	    <table id="tblLogin" runat="server" style="width:100%;height:100%;position:absolute;top:0px;left:0px;display:none;z-index:6"><tr><td align="center">
            <table style="width:330px;height:208px;background-image:url(../_engine/images/login_back.gif);" cellpadding="4" cellspacing="0">
                <tr style="height:40px">
                    <td style="width:110">&nbsp;</td>
                    <td align="right" valign="top"><img onmouseover="this.src='../_engine/images/closeform_mouseover.gif'" onmouseout="this.src='../_engine/images/closeform.gif'" src="../_engine/images/closeform.gif" alt="close" style="cursor:hand;" onclick="CloseLogin()"/></td>
                </tr>
                <tr>
                    <td style="font-size:9pt;font-family:Tahoma,Arial;padding-left:20px" align="left">Username:</td>
                    <td><input style="width:185px" type="text" id="txtusername" value="" onkeypress="return LoginKeyPress(event);" runat="server"/><input id="txtactiveusername" type="hidden" runat="server"/></td>
                </tr>
                <tr>
                    <td style="font-size:9pt;font-family:Tahoma,Arial;padding-left:20px" align="left">Password:</td>
                    <td><input style="width:185px" type="password" id="txtpassword" value="" onkeypress="return LoginKeyPress(event);" runat="server"/><input id="txtactivepassword" type="hidden" runat="server"/></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="padding-right:20px" align="right" valign="bottom"><input style="width:120px;" type="button" id="Button1" value="Login" onclick="Login()" runat="server"/></td>
                </tr>
                <tr style="height:50px;">
                    <td id="tdloginmessage" colspan="2">&nbsp;</td>
                </tr>
            </table>
    	</td></tr></table>


    	<div id="divtransparency" style="width:100%;height:100%;z-index:5;position:absolute;top:0px;left:0px;display:none;background-color:Black;filter:progid:DXImageTransform.Microsoft.Alpha(opacity=80);-moz-opacity:0.80;opacity:.8;" runat="server"></div>

        <!-- SKIN -->
        <table id="tblmain" class="skin_container" runat="server" style="visibility:hidden;" align="center" border="0" cellpadding="0" cellspacing="0" >
            <tr>
                <td colspan="3" id="tdheader" class="skin_header" runat="server" style="height:30px" valign="top"></td>
            </tr>
            <tr>
                <td align="center">
                    <table id="tbldetailwrapper" class="skin_detailwrapper" style="table-layout:fixed" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="skin_contentswrapper" id="tdcontentswrapper" runat="server" valign="top">
                                <table style="width:100%;table-layout:fixed" cellpadding="0" cellspacing="0">
                                    <tr><td id="tdcontentsheader" class="skin_contentsheader" runat="server" style="height:30px;" valign="top" align="left">contents header</td></tr>
                                    <tr>
                                        <td id="tdcontents" class="skin_contents" runat="server" style="width:100%;" valign="top" align="left">
                                            <!-- CONTENTS, SEARCH, INDEX, PROPERTIES -->
    	                                    <div id="divcontents" runat="server" style="width:100%;overflow:auto;height:100%;">
    	                                        <ComponentArt:TreeView id="TreeView1" runat="server">
    	                                            
    	                                        </ComponentArt:TreeView>
    	                                    </div>
    	                                    <div id="divindex" runat="server" style="width:100%;overflow:auto;display:none"></div>
    	                                    <div id="divsearchresults" runat="server" style="width:100%;overflow:auto;display:none"></div>
    	                                    <div id="divproperties" runat="server" style="width:100%;overflow:auto;background-color:#c7cce0;display:none"></div>
                                        </td>
                                    </tr>
                                    <tr><td id="tdcontentsfooter" class="skin_contentsfooter" runat="server" style="height:50px" valign="top">contents footer</td></tr>
                                </table>
                            </td>
                            <td id="tdseparator" class="skin_separator" style="cursor:e-resize" onMouseDown="toDragMode(this)" onMouseUp="stopMoving()"></td>
                            <td id="tdpagewrapper" class="skin_pagewrapper" runat="server" valign="top">
                                <table  style="width:100%;" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="3" id="tdpageheader" class="skin_pageheader" runat="server" style="height:30px;" valign="top"></td>
                                    </tr>
                                    <tr>
                                        <td id="tdpageleft" class="skin_pageleft" runat="server"></td>
                                        <td id="tdpage" class="skin_page" runat="server" valign="top" align="left">
                                            <!-- PAGE -->
                                            <div id="divpage" runat="server" style="width:100%;height:100%"></div>
                                            <!-- EDITOR -->
                                            <div id="diveditor" runat="server" style="width:100%;height:100%;background-color:#2E2E2E;display:none;">
                                                <div class="RadEditor Default" style="height:26px;padding-top:3px;width:100%;position:relative;overflow:hidden" id="toolbar1"></div>
                                                <div class="RadEditor Default" style="display:none;height:26px;width:100%;position:relative;overflow:hidden" id="toolbar2"></div>
                                                <div class="RadEditor Default" style="display:none;height:26px;width:100%;position:relative;overflow:hidden" id="toolbar3"></div>
                                                <telerik:RadEditor ID="editor1" runat="server" ToolsFile="../_engine/ToolsFile.xml" OnClientLoad="OnClientLoad" OnClientModeChange="OnClientModeChange" OnClientSelectionChange="OnClientSelectionChange" DocumentManager-SearchPatterns="*.mov,*.txt,*.doc,*.pdf" FlashManager-SearchPatterns="*.swf,*.awi,*.mp4" MediaManager-SearchPatterns="*.mov,*.wma,*.wmv,*.avi,*.wav,*.mpeg,*.mpg,*.mpe,*.mp3,*.m3u,*.mid,*.midi,*.snd,*.mkv" ImageManager-MaxUploadFileSize="1000000" DocumentManager-MaxUploadFileSize="40000000" FlashManager-MaxUploadFileSize="40000000" MediaManager-MaxUploadFileSize="40000000"><Content></Content></telerik:RadEditor>
		                                        <script type="text/javascript">


		                                            Telerik.Web.UI.Editor.CommandList["More"] = function(commandName, editor, args) { ShowToolbar(editor, "full") }
		                                            Telerik.Web.UI.Editor.CommandList["Less"] = function(commandName, editor, args) { ShowToolbar(editor, "minimal") }
		                                            Telerik.Web.UI.Editor.CommandList["FileSave"] = function(commandName, editor, args) { Save(true) }
		                                            Telerik.Web.UI.Editor.CommandList["Close"] = function(commandName, editor, args) { loadpage(TreeView1.SelectedNode.get_id()) }
		                                            Telerik.Web.UI.Editor.CommandList["LinkPage"] = function(commandName, editor, args) { linkpage(commandName, editor, args) }
		                                            Telerik.Web.UI.Editor.CommandList["InsertCondition"] = function(commandName, editor, args) { InsertCondition(commandName, editor) }
		                                            Telerik.Web.UI.Editor.CommandList["InsertVariable"] = function(commandName, editor, args) { InsertVariable(commandName, editor) }
		                                            Telerik.Web.UI.Editor.CommandList["StickyNote"] = function(commandName, editor, args) { InsertStickyNote(editor) }
		                                            Telerik.Web.UI.Editor.CommandList["InsertPageObject"] = function(commandName, editor, args) { InsertPageObject(commandName, editor) }
		                                            Telerik.Web.UI.Editor.CommandList["GlobalSearchAndReplace"] = function(commandName, editor, args) { GlobalSearchAndReplace(editor) }
		                                            Telerik.Web.UI.Editor.CommandList["PasteImage"] = function(commandName, editor, args) { PasteImage(commandName, editor, args) }
                                    				
		                                        </script>
                                            </div>
                                        </td>
                                        <td id="tdpageright" class="skin_pageright" runat="server"></td>
                                    </tr>
                                    <tr><td colspan="3" id="tdpagefooter" class="skin_pagefooter" runat="server" style="height:50px" valign="top">Page Footer</td></tr>
                                </table>

                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3" id="tdfooter" class="skin_footer" runat="server" style="height:50px" valign="top">footer</td>
            </tr>
        </table>

        <div id="divHostedToolbar" runat="server" style="width:100%;height:40px;color:White;position:relative;z-index:6;background-color:black;background-image:url(../_engine/images/hosted_toolbar_back.gif);display:none"></div>

	    

<script type="text/javascript">
    function GetParm(name)
        {
        name = name.replace(/[\[]/,"\\\[").replace(/[\]]/,"\\\]");  
        var regexS = "[\\?&]"+name+"=([^&#]*)";  
        var regex = new RegExp( regexS );  
        var results = regex.exec( window.location.href );  
        if( results == null )
    	    {	
    	    return "";
            }
        else 
    	    {
            return results[1];
            }
        }
    var searchtext = GetParm("search");
    var condition = GetParm("condition");
    var filter = GetParm("filter");
		
    if (searchtext.length > 0) {
        DisplaySearch(searchtext,condition,filter);
        }
</script>

    </form>

</body>
</html>
