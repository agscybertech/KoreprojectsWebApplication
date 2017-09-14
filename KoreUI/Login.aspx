<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/bootstrap.min.js" type="text/javascript"></script>
    <script src="js/bootstrap.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js" type="text/javascript"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" type="text/javascript"></script>
    <link href="~/css/my_style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="row-fluid">
            <div class="container">
                <div class="col-xs-12 text-center">
                    <asp:Label ID="lblErrorMessage" runat="server" Font-Size="X-Small" ForeColor="red"></asp:Label>
                </div>
                <div class="col-xs-12">
                    <div class="demo_page_form">
                        <span>email address</span>
                        <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
                        <span>Password</span>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:Button ID="btnLogin" runat="server" CssClass="my_btn" Text="Login" OnClick="btnLogin_Click" Style="padding: 11px 10px;" />
                        
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
