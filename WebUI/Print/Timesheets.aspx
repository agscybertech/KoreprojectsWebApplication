<%@ Page Title="" Language="VB" MasterPageFile="~/DefaultMasterPage.master" AutoEventWireup="false" CodeFile="Timesheets.aspx.vb" Inherits="Print_Timesheets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery.tools.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
    <link href="../Styles/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .center {
            width:95% !important;
        }

        .table {
          width: 100%;
          max-width: 100%;
          margin-bottom: 20px;
          border-collapse:collapse;
        }
        .table > thead > tr > th,
        .table > tbody > tr > th,
        .table > tfoot > tr > th,
        .table > thead > tr > td,
        .table > tbody > tr > td,
        .table > tfoot > tr > td {
          padding: 8px;
          line-height: 1.42857143;
          vertical-align: top;
          border-top: 1px solid #ddd;
          text-align:center;
        }

        .table > thead > tr > th,
        .table > tfoot > tr > th,
        .table > tbody > tr > th {
          background-color:#ececec;
        }

        .table > thead > tr > th {
          vertical-align: bottom;
          border-bottom: 2px solid #ddd;
        }
        .table > caption + thead > tr:first-child > th,
        .table > colgroup + thead > tr:first-child > th,
        .table > thead:first-child > tr:first-child > th,
        .table > caption + thead > tr:first-child > td,
        .table > colgroup + thead > tr:first-child > td,
        .table > thead:first-child > tr:first-child > td {
          border-top: 0;
        }
        .table > tbody + tbody {
          border-top: 2px solid #ddd;
        }
        .table .table {
          background-color: #fff;
        }
        .table-condensed > thead > tr > th,
        .table-condensed > tbody > tr > th,
        .table-condensed > tfoot > tr > th,
        .table-condensed > thead > tr > td,
        .table-condensed > tbody > tr > td,
        .table-condensed > tfoot > tr > td {
          padding: 5px;
        }
        .table-bordered {
          border: 1px solid #ddd;
        }
        .table-bordered > thead > tr > th,
        .table-bordered > tbody > tr > th,
        .table-bordered > tfoot > tr > th,
        .table-bordered > thead > tr > td,
        .table-bordered > tbody > tr > td,
        .table-bordered > tfoot > tr > td {
          border: 1px solid #ddd;
        }
        .table-bordered > thead > tr > th,
        .table-bordered > thead > tr > td {
          border-bottom-width: 2px;
        }
    </style>
    <script type="text/javascript">
        function PrintTimesheet() {
            if (document.getElementsByClassName("editaction") != null) {
                for (var i = 0, max = document.getElementsByClassName("editaction").length; i < max; i++) {
                    document.getElementsByClassName("editaction")[i].style.display = 'none';
                }

                for (var i = 0, max = document.getElementsByClassName("usertitle").length; i < max; i++) {
                    document.getElementsByClassName("usertitle")[i].setAttribute("colspan", "6");
                }

                for (var i = 0, max = document.getElementsByClassName("userfooter").length; i < max; i++) {
                    document.getElementsByClassName("userfooter")[i].style.display = 'none';
                }
            }
            
            document.getElementById('btnPrint').style.display = 'none'; 
            window.print();
            document.getElementById('btnPrint').style.display = '';
            if (document.getElementsByClassName("editaction") != null) {
                for (var i = 0, max = document.getElementsByClassName("editaction").length; i < max; i++) {
                    document.getElementsByClassName("editaction")[i].style.display = '';
                }

                for (var i = 0, max = document.getElementsByClassName("usertitle").length; i < max; i++) {
                    document.getElementsByClassName("usertitle")[i].setAttribute("colspan", "7");
                }

                for (var i = 0, max = document.getElementsByClassName("userfooter").length; i < max; i++) {
                    document.getElementsByClassName("userfooter")[i].style.display = '';
                }
            }
        }

        $(document).ready(function () {

            $(".form_popup").fancybox({
                'autoScale': false,
                'transitionIn': 'none',
                'transitionOut': 'none',
                closeEffect: 'none',
                'afterClose': function () {
                    window.location.reload();
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- top -->
    <div class="clr" id="top">
        <div class="sep" id="topsep"></div>
        	<h1>Time Sheets</h1><input id="btnPrint" type="button" value="Print Time Sheets" onclick="PrintTimesheet();" style="float: right;margin-right: 10px;margin-top: 16px;" />
        <div class="clr sep" id="botsep">
            
        </div>
    </div>
    <!-- top -->

    <!-- content -->
    <div id="content" align="left">
        <asp:Label ID="lblPrint" runat="server"></asp:Label>
    </div>
    <!-- content -->
</asp:Content>
