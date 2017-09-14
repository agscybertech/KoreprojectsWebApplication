<%@ Page Title="" Language="VB" MasterPageFile="~/Projects/MasterPage.master" AutoEventWireup="false" CodeFile="CalendarView.aspx.vb" Inherits="Projects_CalendarView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="UC" TagName="ProjectsGrid" Src="~/WebControls/WebUserControlProjectsGrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
<link href='../fullcalendar/fullcalendar.css' rel='stylesheet' />
<link href='../fullcalendar/fullcalendar.print.css' rel='stylesheet' media='print' />
<script type='text/javascript' src='../fullcalendar/lib/moment.min.js'></script>
<script type='text/javascript' src='../fullcalendar/lib/jquery.min.js'></script>
<script type='text/javascript' src='../fullcalendar/fullcalendar.min.js'></script>
<script type='text/javascript'>

    $(document).ready(function () {
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        var calendar = $('#calendar').fullCalendar({
            //header: {
            //    left: 'prev,next today',
            //    center: 'title',
            //    right: 'month,agendaWeek,agendaDay'
            //},
            header: {
                left: 'prev,next today',
                center: 'title',
                right: ''
            },
            selectable: true,
            selectHelper: true,
            select: function (start, end, allDay) {
                var allowReload;
                allowReload = true;
                var title = prompt('Project Name:');
                if (title) {
                    calendar.fullCalendar('renderEvent',
						{
						    title: title,
						    start: start,
						    end: end,
						    allDay: allDay
						},
						true // make the event "stick"
					);

                    var strStart;
                    strStart = "";
                    if (start != null) {
                        strStart = start.getFullYear().toString() + "-";
                        strStart = strStart + (start.getMonth() + 1 < 10 ? "0" : "") + (start.getMonth() + 1) + "-";
                        strStart = strStart + (start.getDate() < 10 ? "0" : "") + start.getDate();
                    }

                    var strEnd;
                    strEnd = "";
                    if (end != null) {
                        strEnd = end.getFullYear() + "-";
                        strEnd = strEnd + (end.getMonth() + 1 < 10 ? "0" : "") + (end.getMonth() + 1) + "-";
                        strEnd = strEnd + (end.getDate() < 10 ? "0" : "") + end.getDate();
                    }

                    var strUrl;
                    if (strEnd == "") {
                        strUrl = "edit.aspx?title=" + title + "&start=" + strStart;
                    }
                    else {
                        strUrl = "edit.aspx?title=" + title + "&start=" + strStart + "&end=" + strEnd;
                    }
                    allowReload = false;
                    window.location = strUrl;
                }
                calendar.fullCalendar('unselect');
                if (allowReload && navigator.appName == "Netscape") {
                    window.location.reload();
                }
            },
            editable: true,
            //events: "Calendar-json-events.php",
            // US Holidays
            //events: 'http://www.google.com/calendar/feeds/usa__en%40holiday.calendar.google.com/public/basic',			
            // NZ Holidays
            //events: 'http://www.google.com/calendar/feeds/new_zealand__en%40holiday.calendar.google.com/public/basic',

            eventSources: [
		        'UnarchivedProjectsCalendarFeed.ashx?id=<% =GetEncryptId() %>',
                'RemindersCalendarFeed.ashx?id=<% =GetEncryptId() %>',
                {
                    url: 'http://www.google.com/calendar/feeds/new_zealand__en%40holiday.calendar.google.com/public/basic',
                    color: 'grey',
                    textColor: 'white'
                }
            ],

            //eventColor: '#2499D7',
            eventClick: function (event) {
                var eventUrl = event.url;
                var domainName = window.location.hostname;
                if (eventUrl.indexOf(domainName) == -1) {
                    window.open(event.url, 'gcalevent', 'width=700,height=600');
                    return false;
                }
            },

            eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {

                //alert(
                //    event.start + " | " + event.end + " | " + event.id + " | " +
                //    event.title + " was moved " +
                //    dayDelta + " days and " +
                //    minuteDelta + " minutes."
                //);

                //if (allDay) {
                //    alert("Event is now all-day");
                //} else {
                //    alert("Event has a time-of-day");
                //}

                //if (!confirm("Are you sure about this change?")) {
                //    revertFunc();
                //}
                var allowDragDrop;
                allowDragDrop = false;

                var d = $('#calendar').fullCalendar('getDate');
                //alert("The current date of the calendar is " + d);
                d = new Date(d.getFullYear(), d.getMonth(), 1);
                //alert("The first Day of month is " + d);
                var dVal = d.valueOf();
                d = new Date(dVal - 1000 * 60 * 60 * 24 * d.getDay());
                //alert("The first Day of Calendar is " + d);
                var firstCalendarDate;
                firstCalendarDate = d;
                dVal = d.valueOf();
                d = new Date(dVal + 1000 * 60 * 60 * 24 * (7 * 6 - 1));
                //alert("The last Day of Calendar is " + d);
                var lastCalendarDate;
                lastCalendarDate = d;

                if (event.start.getTime() >= firstCalendarDate.getTime() && event.start.getTime() <= lastCalendarDate.getTime()) {
                    allowDragDrop = true;
                }
                //if (event.end == null) {
                //    allowDragDrop = true;
                //}
                //else {
                //    if (event.end.getTime() - event.start.getTime() < 1000 * 60 * 60 * 24 * 15) {
                //        allowDragDrop = true;
                //    }
                //}
                if (allowDragDrop) {
                    // Send a xmlhttp request for Update StartDate and/or EndDate
                    // If return fail, revert function
                    var strStart;
                    strStart = "";
                    if (event.start != null) {
                        strStart = event.start.getFullYear().toString() + "-";
                        strStart = strStart + (event.start.getMonth() + 1 < 10 ? "0" : "") + (event.start.getMonth() + 1) + "-";
                        strStart = strStart + (event.start.getDate() < 10 ? "0" : "") + event.start.getDate();
                    }

                    var strEnd;
                    strEnd = "";
                    if (event.end != null) {
                        strEnd = event.end.getFullYear() + "-";
                        strEnd = strEnd + (event.end.getMonth() + 1 < 10 ? "0" : "") + (event.end.getMonth() + 1) + "-";
                        strEnd = strEnd + (event.end.getDate() < 10 ? "0" : "") + event.end.getDate();
                    }

                    var strRequestUrl;
                    if (strEnd == "") {
                        strRequestUrl = "ProjectModificationService.ashx?id=" + event.id + "&start=" + strStart + "&mid=<% =GetEncryptId() %>";
                    }
                    else {
                        strRequestUrl = "ProjectModificationService.ashx?id=" + event.id + "&start=" + strStart + "&end=" + strEnd + "&mid=<% =GetEncryptId() %>";
                    }

                    var xmlhttp;
                    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
                        xmlhttp = new XMLHttpRequest();
                    }
                    else {// code for IE6, IE5
                        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
                    }
                    xmlhttp.open("GET", strRequestUrl, false);
                    xmlhttp.send();
                    if (xmlhttp.responseText != "True") {
                        revertFunc();
                    }
                }
                else {
                    revertFunc();
                }
            },

            eventResize: function (event, dayDelta, minuteDelta, revertFunc) {

                //alert(
                //    "The end date of " + event.title + "has been moved " +
                //    dayDelta + " days and " +
                //    minuteDelta + " minutes."
                //);

                //if (!confirm("is this okay?")) {
                //    revertFunc();
                //}

                // Send a xmlhttp request for Update EndDate
                // If return fail, revert function
                var strEnd;
                strEnd = "";
                if (event.end != null) {
                    strEnd = event.end.getFullYear() + "-";
                    strEnd = strEnd + (event.end.getMonth() + 1 < 10 ? "0" : "") + (event.end.getMonth() + 1) + "-";
                    strEnd = strEnd + (event.end.getDate() < 10 ? "0" : "") + event.end.getDate();
                }

                var strRequestUrl;
                strRequestUrl = "ProjectModificationService.ashx?id=" + event.id + "&end=" + strEnd + "&mid=<% =GetEncryptId() %>";

                var xmlhttp;
                if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
                    xmlhttp = new XMLHttpRequest();
                }
                else {// code for IE6, IE5
                    xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
                }
                xmlhttp.open("GET", strRequestUrl, false);
                xmlhttp.send();
                if (xmlhttp.responseText != "True") {
                    revertFunc();
                }
            },

            loading: function (bool) {
                if (bool) $('#loading').show();
                else $('#loading').hide();
            }
        });
        <% =GetGotoDateScript() %>
    });

</script>
    
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
            <div runat="server" id="divTitle">
            <a style="padding:8px;float:right;" id="help" href="<% if request("archived") = "1" then %>https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm<% Elseif request("scoped") = "1" then %>https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm<% Else %>https://www.koreprojects.com/helpconsole2010/HelpSystem/default.htm<% End If %>" title="Help" target="_blank"><img src="../images/help-icon.png" width="40" height="40" border="0" alt="Help" /></a>
            <a style="padding:8px;float:right;" id="healthsafety" href="../contents/HealthSafety.aspx" title="Health Safety"><img src="../images/healthsafety-but.png" width="40" height="40" border="0" alt="Health Safety" /></a>
            <a style="padding:8px;float:right;" id="addproject" href="edit.aspx" title="Add Project"><img src="../images/add-project.png" width="40" height="40" border="0" alt="Add Project" /></a>
            <a style="padding:8px;float:right;" id="archives" href="view.aspx?archived=1" title="Project Archives"><img src="../images/archive.png" width="40" height="40" border="0" alt="Project Archives" /></a>
            <a style="padding:8px;float:right;" id="calendarview" href="calendarview.aspx" title="Project Planner"><img src="../images/calendar.png" width="40" height="40" border="0" alt="Project Planner" /></a>
            </div>
        <div class="clr sep" id="botsep"></div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <div align="center" width="100%">
            <asp:Label ID="lblMsg" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
        </div>
        <table width="100%">
            <tr>
                <td width="80%">
                    <div id='calendar'></div>
                </td>
                <td width="20%" valign="top">
                    <table style="width:100%;margin-top:46px" cellpadding="4" cellspacing="0" bgColor="#F4F2F3">
                        <tr>
                            <td valign="bottom" align="right" style="border-bottom:1px solid #000000; border-bottom-style:solid;">
                                <font size="3"><b>NOTEBOARD</b></font>
                            </td>
                            <td valign="middle" align="right" style="border-bottom:1px solid #000000; border-bottom-style:solid;">
                                <a id="noteboard_button" href="EditReminder.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & string.format("&{0}",Datetime.Now.ToString("yyyyddMMhhmmss")) %>" style="float:right;">ADD</a>                    
                                <a id="A1" class="form_popup" href="EditReminder.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) & string.format("&{0}",Datetime.Now.ToString("yyyyddMMhhmmss")) %>" style="float:right;display:none;">ADD</a>                    
                                <a id="A2" href="EditReminder.aspx?id=<%= System.Web.HttpUtility.UrlEncode(Request.QueryString("ID")) %>" rel="#overlaynote" style="float:right;display:none;">ADD</a>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" height="520px" valign="top">                    
                                <asp:Repeater ID="rptReminders" runat="server">
                                </asp:Repeater>                    
                            </td>
                        </tr>
                    </table>                
                </td>
            </tr>
        </table>        
    </div>
    <!-- content -->

    <!-- footer -->
    <div class="clr" id="Div1">
        <div class="sep" id="Div2"></div>
        	<table width="100%" style="height:45px">
                <tr>
                    <td> 
                    </td>
                </tr>
            </table> 
        <div class="clr sep" id="Div3"></div>
    </div>
    <!-- footer -->
</asp:Content>