<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/WebSitePagesMaster.master" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<asp:Content ContentPlaceHolderID="Scripts" ID="ContantHead" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            // Activate carousel
            $("#myCarousel").carousel();

            // Enable carousel control
            $(".left").click(function () {
                $("#myCarousel").carousel('prev');
            });
            $(".right").click(function () {
                $("#myCarousel").carousel('next');
            });

            // Enable carousel indicators
            $(".slide-one").click(function () {
                $("#myCarousel").carousel(0);
            });
            $(".slide-two").click(function () {
                $("#myCarousel").carousel(1);
            });
            $(".slide-three").click(function () {
                $("#myCarousel").carousel(2);
            });
            $(".slide-four").click(function () {
                $("#myCarousel").carousel(3);
            });
        });
    </script>
    <!----------slider-------------->

    <script type="text/javascript">
        $(document).ready(function () {
            // Activate carousel
            $("#myCarouse2").carousel();

            // Enable carousel control
            $(".left").click(function () {
                $("#myCarouse2").carousel('prev');
            });
            $(".right").click(function () {
                $("#myCarouse2").carousel('next');
            });

            // Enable carousel indicators
            $(".slide-one").click(function () {
                $("#myCarouse2").carousel(0);
            });
            $(".slide-two").click(function () {
                $("#myCarouse2").carousel(1);
            });
            $(".slide-three").click(function () {
                $("#myCarouse2").carousel(2);
            });

        });
    </script>
    <!----------slider-------------->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="banner row-fluid">
        <div id="myCarousel" class="carousel slide" data-interval="3000" data-ride="carousel">
            <!-- Carousel indicators -->
            <ol class="carousel-indicators">
                <li class="slide-one active"></li>
                <li class="slide-two"></li>
                <li class="slide-three"></li>
                <li class="slide-four"></li>
            </ol>
            <!-- Wrapper for carousel items -->
            <div class="carousel-inner">
                <div class="active item">
                    <img src="images/banner1.jpg" alt="First Slide" />
                    <div class="carousel-caption">
                        <div class="baner_text">
                            <p>Multiple projects visible </p>
                            <p>at once</p>
                        </div>
                        <img src="images/banner_phone.png" alt="" class="banner_mobile" />
                    </div>
                </div>
                <div class="item">
                    <img src="images/banner2.jpg" alt="Second Slide" />
                    <div class="carousel-caption">
                        <div class="baner_text">
                            <p>Real Time status updates </p>
                            <p>on all projects</p>
                        </div>
                        <img src="images/banner_phone.png" alt="" class="banner_mobile" />
                    </div>
                </div>
                <div class="item">
                    <img src="images/banner3.jpg" alt="Third Slide" />
                    <div class="carousel-caption">
                        <div class="baner_text">
                            <p>Secure Archives</p>
                        </div>
                        <img src="images/banner_phone.png" alt="" class="banner_mobile" />
                    </div>
                </div>
                <div class="item">
                    <img src="images/banner4.jpg" alt="Third Slide" />
                    <div class="carousel-caption">
                        <div class="baner_text">
                            <p>Customize your System</p>
                        </div>
                        <img src="images/banner_phone.png" alt="" class="banner_mobile" />
                    </div>
                </div>
            </div>
            <!-- Carousel controls -->
            <a class="carousel-control left">
                <span class="glyphicon glyphicon-chevron-left"></span>
            </a>
            <a class="carousel-control right">
                <span class="glyphicon glyphicon-chevron-right"></span>
            </a>
        </div>
    </div>
    <div class="row-fluid pannel1">
        <div class="container">
            <h1>Kore Projects is a mobile project
                <br />
                management system that is easy for anyone  to use.</h1>
            <a class="free_trial" href="#btnSignup" rel="" id="anchor1" class="anchorLink">free trial</a>
            <img src="images/top_2.png" class="img-responsive" alt="" />
        </div>
        <!--container-->
    </div>
    <!--row-fluid pannel1-->

    <div class="row-fluid pannel2">
        <div class="container">
            <h2>Take our Product Tour to see 
                <br />
                how simple it can be to maximize your productivity </h2>
            <ul class="pannel2_boxes">
                <li>
                    <img src="images/pro_tour1.jpg" alt="" class="img-responsive" />
                    <h4>Projects Page</h4>
                    <p>All your current and upcoming Projects in one, easy to view list.Easy to use star ranking allows you and staff members to ensure vital jobs get done first.he moment that a stage of the project is complete, a staff member can update the status.</p>
                </li>

                <li>
                    <img src="images/pro_tour2.jpg" alt="" class="img-responsive" />
                    <h4>Scopes</h4>
                    <p>Planning  is now made very Simple and Easy. You can keep all your project scopes in one place  and add details, quotes and files as they arise until a scope becomes a new job and you can even customize your System</p>
                </li>

                <li>
                    <img src="images/pro_tour3.jpg" alt="" class="img-responsive" />
                    <h4>Contacts</h4>
                    <p>All your contacts in one place. Categorize your staff, contractors, clients and suppliers for convenience.Invite contacts to one or more jobs and allow them access to all current project information. Information flows  to team members in no time</p>
                </li>
            </ul>

            <a class="start_free_trail" href="#btnSignup" rel="" id="anchor1" class="anchorLink">start A free trial</a>
            <a class="take_tour" href="product-tour.aspx">take a tour</a>


        </div>
        <!--container-->
    </div>
    <!--row-fluid pannel2-->

    <div class="row-fluid pannel3">
        <h2>Our customers gets BENEFITS 
            <br />
            to make  their businesses simpler and 
            <br />
            more efficient </h2>
        <div class="container">

            <div class="panel_3_left">
                <ul>
                    <li>
                        <img src="images/bene1.jpg" alt="" />
                        <div>
                            <h3>Clearer Organization</h3>
                        </div>
                    </li>
                    <li>
                        <img src="images/bene3.jpg" alt="" />
                        <div>
                            <h3>Easy
                                <br />
                                to
                                <br />
                                Use</h3>
                        </div>
                    </li>
                    <li>
                        <img src="images/bene4.jpg" alt="" />
                        <div>
                            <h3>Increased  Efficiency</h3>
                        </div>
                    </li>
                    <li>
                        <img src="images/bene5.jpg" alt="" />
                        <div>
                            <h3>Centralized 
                                <br />
                                Data</h3>
                        </div>
                    </li>
                </ul>
            </div>

            <div class="panel_3_right">
                <ul>
                    <li>
                        <img src="images/bene2.jpg" alt="" />
                        <div>
                            <h3>Sharper 
                                <br />
                                Communication</h3>
                        </div>
                    </li>
                    <li>
                        <img src="images/bene6.jpg" alt="" />
                        <div>
                            <h3>Customizable  Settings</h3>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <!--row-fluid pannel3-->

    <div class="row-fluid pannel4">
        <div class="container">
            <h2>TRUSTED BY BUSINESSES WORLDWIDE </h2>
            <ul class="client_logo">
                <li>
                    <img src="images/clogo1.jpg" alt="" /></li>
                <li>
                    <img src="images/clogo2.jpg" alt="" /></li>
               
                <li>
                    <img src="images/clogo5.jpg" alt="" /></li>
              	<li>
                    <img src="images/clogo6.jpg" alt="" /></li>
                     <li>
                    <img src="images/clogo3.jpg" alt="" /></li>
                <li>
                    <img src="images/clogo4.jpg" alt="" /></li>
            </ul>
        </div>
    </div>
    <!--<div class="row-fluid pannel4-->

    <div class="row-fluid pannel5">
        <div class="container">
            <div class="empl_shedule">
                <h5>MANAGE YOUR EMPLOYEE SCHEDULE FROM WHEREVER YOU ARE </h5>
                <p>Our website is compatible with IOS Android and Windows phone</p>
                <a href="KoreMobile.aspx" class="mobile_view"><span><img src="images/foote_logo.png" /> </span>mobile view</a>
                <a href="" class="google_play">
                    <img src="images/google_play.jpg" /></a>
            </div>
        </div>

    </div>
    <!--row-fluid pannel5-->

    <div class="row-fluid pannel6">
        <div class="container">
            <h2>Benefits of Kore Projects</h2>
            <ul class="pannel6_benefits">
                <li>
                    <img src="images/bene_btm1.jpg" alt="" />
                    <h4>Ultimate Organization</h4>
                    <p>Implement Kore Projects into your working day and become instantly more organised than ever before!</p>
                    <a href="ultimate-organization.aspx">read more </a>
                </li>

                <li>
                    <img src="images/bene_btm2.jpg" alt="" />
                    <h4>Maximum Productivity</h4>
                    <p>Make the most of every working hour. Use Kore Projects to cut wasted time out of your business  day. </p>
                    <a href="maximum-productivity.aspx">read more </a>
                </li>

                <li>
                    <img src="images/bene_btm3.jpg" alt="" />
                    <h4>Simple to Use</h4>
                    <p>Taking control of your business has never been easier. Kore Projects is simple to use and quick to get results.</p>
                    <a href="simple-to-use.aspx">read more </a>
                </li>
            </ul>
        </div>
    </div>
    <!--row-fluid pannel6-->

    <div class="row-fluid pannel7">
        <div class="container">
            <h2>project flow</h2>
            <h5>Kore Project is a <span>Mobile Project Management system</span> that is easy for anyone to use.</h5>
            <ul class="flow_icons">
                <li>
                    <span class="flow_one flow"><i></i></span>
                    Oversee in Seconds</li>
                <li>
                    <span class="flow_two"><i></i></span>Instant Project Updates</li>
                <li>
                    <span class="flow_three"><i></i></span>
                    Create New Projects Easily</li>
                <li>
                   <span class="flow_four"><i></i>	</span>
                    Secure Archives</li>

            </ul>

            <div class="flow_banner row-fluid">
                <div id="myCarouse2" class="carousel slide" data-interval="3000" data-ride="carousel">
                    <!-- Carousel indicators -->
                    <ol class="carousel-indicators">
                        <li class="slide-one active"></li>
                        <li class="slide-two"></li>
                        <li class="slide-three"></li>
                    </ol>
                    <!-- Wrapper for carousel items -->
                    <div class="carousel-inner">
                        <div class="active item">
                            <img src="images/project_flow1.png" alt="First Slide"/>
                        </div>
                        <div class="item">
                            <img src="images/project_flow2.png" alt="Second Slide"/>
                        </div>
                        <div class="item">
                            <img src="images/project_flow3.png" alt="Third Slide"/>
                        </div>

                    </div>
                    <!-- Carousel controls -->

                </div>
                <!--myCarousel-->
            </div>
            <!--project flow banner-->


        </div>
    </div>
    <!--row-fluid pannel7-->


    
    <!--<div class="row-fluid pannel8-->
</asp:Content>
