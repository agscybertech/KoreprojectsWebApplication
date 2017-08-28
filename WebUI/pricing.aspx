<%@ Page Language="C#" MasterPageFile="~/WebSitePagesMaster.master" AutoEventWireup="true" CodeFile="pricing.aspx.cs" Inherits="pricing" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <style>
        .btn {
            display: inline-block;
            padding: 10px;
            margin-bottom: 0;
            font-size: 32px;
            line-height: 35px;
            color: #333;
            text-align: center;
            text-shadow: 0 1px 1px rgba(255,255,255,.75);
            vertical-align: middle;
            cursor: pointer;
            background-color: #f5f5f5;
            background-image: -moz-linear-gradient(top,#fff,#e6e6e6);
            background-image: -ms-linear-gradient(top,#fff,#e6e6e6);
            background-image: -webkit-gradient(linear,0 0,0 100%,from(#fff),to(#e6e6e6));
            background-image: -webkit-linear-gradient(top,#fff,#e6e6e6);
            background-image: -o-linear-gradient(top,#fff,#e6e6e6);
            background-image: linear-gradient(top,#fff,#e6e6e6);
            background-repeat: repeat-x;
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#e6e6e6', GradientType=0);
            border-color: #e6e6e6 #e6e6e6 #bfbfbf;
            border-color: rgba(0,0,0,.1) rgba(0,0,0,.1) rgba(0,0,0,.25);
            filter: progid:DXImageTransform.Microsoft.gradient(enabled = false);
            border: 1px solid #ccc;
            border-bottom-color: #b3b3b3;
            -webkit-border-radius: 2px;
            -moz-border-radius: 2px;
            border-radius: 2px;
            text-decoration: none;
            width: 548px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="header_inner">
        <h1>ABOUT US</h1>
    </div>
    <div class="content_inner">
        <table style="width: 100%">
            <tr>
                <td style="text-align: center">
                    <asp:Image runat="server" ImageUrl="~/Images/Pricing.png" />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <a href="signup.aspx" class="btn btn-large">START YOUR 30 DAYS FREE TRIAL</a>
                </td>
            </tr>
        </table>
        <%--<table border="0" cellpadding="0" cellspacing="0" width="100%">

            <tr>
                <td>
                    <h2>Choose the plan structure that will best suit your business</h2>
                    <p>
                        <br />
                        We understand that not every business will require the same from our project management software, which is why we have tiered our price structure into four convenient categories. You won't pay over the odds for projects that you won't use and you can alter your payment plan if you find that your requirements change at any time. Look over the table below to see which price plan will best suit the needs of your business.
                    </p>
                    <div id="pricing">
                        <div id="light" class="coll">
                            <div class="top"></div>
                            <div class="center">
                                <strong><span>5 staff</span><br />
                                    <span>2 GB storage</span></strong>
                            </div>
                            <div style="text-align: center;" class="mid">
                                <p>
                                    
                                </p>
                            </div>
                            <div class="bot"></div>
                        </div>
                        <div id="plus" class="coll">
                            <div class="top"></div>
                            <div class="center">
                                <strong><span>20 staff</span><br />
                                    <span>5 GB storage</span></strong>
                            </div>
                            <div style="text-align: center;" class="mid">
                                <p>
                                    
                                </p>
                            </div>
                            <div class="bot"></div>
                        </div>
                        <div id="pro" class="coll">
                            <div class="top"></div>
                            <div class="center">
                                <strong><span>50 staff</span><br />
                                    <span>15 GB storage</span></strong>
                            </div>
                            <div style="text-align: center;" class="mid">
                                <p>

                                </p>
                            </div>
                            <div class="bot"></div>
                        </div>
                        <div id="prem" class="coll">
                            <div class="top"></div>
                            <div class="center">
                                <strong><span>Unlimited staff</span><br />
                                    <span>40 GB storage</span></strong>
                            </div>
                            <div style="text-align: center;" class="mid">
                                <p>
                                    
                                </p>
                            </div>
                            <div class="bot"></div>
                        </div>
                    </div>
                    <div style="text-align: center;">
                        <img width="291" height="127" border="0" alt="" src="ebiz-manager/images/infopages/try-for-free.jpg" /><br />
                        <a href="signup.aspx">
                            <img border="0" alt="" src="ebiz-manager/images/infopages/get-started.jpg" /></a>
                    </div>
                    <p></p>
                </td>
            </tr>

        </table>--%>
    </div>
</asp:Content>
