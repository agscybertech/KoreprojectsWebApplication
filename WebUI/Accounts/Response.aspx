<%@ page language="C#" masterpagefile="~/Projects/MasterPage.master" autoeventwireup="true" inherits="Response" CodeFile="Response.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="header_inner">
        <h1>Response</h1>
    </div>
    <div class="content_inner">
        <h4 class="text-center">Sorry! An unexpected error occured.</h4>
    <table class="text-center" style="border-collapse: collapse" align="center" cellspacing="5" width="85%">
            <tr>
                <td>
                    <b><u>API Error Message:</u></b> <asp:Label ID="apierror" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <a href="Index.aspx">Back</a>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>







