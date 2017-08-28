<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="Dashboard_Default" %>

<!DOCTYPE html>
<!--[if lt IE 7 ]><html class="ie ie6" lang="en"> <![endif]-->
<!--[if IE 7 ]><html class="ie ie7" lang="en"> <![endif]-->
<!--[if IE 8 ]><html class="ie ie8" lang="en"> <![endif]-->
<!--[if (gte IE 9)|!(IE)]><!--><html lang="en"> <!--<![endif]-->
<head runat="server">
    <meta charset="utf-8">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <script>
        function GoTimesheet() {
            document.location.href='../Timesheet/'
        }
    </script>
    <link rel="stylesheet" href="../Styles/base.css">
	<link rel="stylesheet" href="../Styles/skeleton.css">
	<link rel="stylesheet" href="../Styles/layout.css">
    <!--[if lt IE 9]>
		<script src=""../Scripts/html5.js"></script>
	<![endif]-->
</head>
<body>
    
    <form id="form1" runat="server">
    <div class="container">
        <asp:Table ID="tblDash" runat="server" Height="320px" Width="240px">
            <asp:TableHeaderRow>
                <asp:TableCell HorizontalAlign="Left">
                    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="Right">
                    <asp:Label ID="lblUserName" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableHeaderRow>
            <asp:TableRow Height="100px" HorizontalAlign="Center" VerticalAlign="Middle">
                <asp:TableCell>
                    jobs
                </asp:TableCell>
                <asp:TableCell>
                    photos
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="100px" HorizontalAlign="Center" VerticalAlign="Middle">
                <asp:TableCell>
                    reordering
                </asp:TableCell>
                <asp:TableCell runat="server" ID="tdTimesheet">
                    timesheet
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableFooterRow>
                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                    message here
                </asp:TableCell>
            </asp:TableFooterRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
