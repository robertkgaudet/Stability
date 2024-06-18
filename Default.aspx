<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<!DOCTYPE html>
<html lang="en">
<head>
	<script async src="https://www.googletagmanager.com/gtag/js?id=UA-96039646-1"></script>
	<script type="text/javascript" src="/Web/js/jquery-1.11.3.min.js"></script>
	<script type="text/javascript" src="/Web/js/jquery-migrate-1.2.1.min.js"></script>
	<script type="text/javascript" src="/Web/js/bootstrap.min.js"></script>
	<script type="text/javascript" src="/Web/js/jquery.easing.min.js"></script>
	<script type="text/javascript" src="/Web/js/smoothscroll.js"></script>
	<script type="text/javascript" src="/Web/js/response.min.js"></script>
	<script type="text/javascript" src="/Web/js/jquery.placeholder.min.js"></script>
	<script type="text/javascript" src="/Web/js/jquery.fitvids.js"></script>
	<script type="text/javascript" src="/Web/js/jquery.imgpreload.min.js"></script>
	<script type="text/javascript" src="/Web/js/waypoints.min.js"></script>
	<script type="text/javascript" src="/Web/js/slick.min.js"></script>
	<script type="text/javascript" src="/Web/js/jquery.mousewheel-3.0.6.pack.js"></script>
	<script type="text/javascript" src="/Web/js/jquery.fancybox.pack.js"></script>
	<script type="text/javascript" src="/Web/js/parallax.min.js"></script>
	<script type="text/javascript" src="/Web/js/jquery.counterup.min.js"></script>
	<script type="text/javascript" src="/Web/js/jquery.BlackAndWhite.js"></script>
	<script type="text/javascript" src="/Web/js/script.js"></script>
	<script type="text/javascript" src="/Homer/vendor/sweetalert/lib/sweet-alert.min.js"></script>
	
	<!-- App scripts -->
	<script src="/Homer/vendor/scripts/homer.js"></script>
	<script src="/Homer/vendor/slimScroll/jquery.slimscroll.min.js"></script>
	<script src="/Homer/vendor/bootstrap/dist/js/bootstrap.min.js"></script>
	<script src="/Homer/vendor/metisMenu/dist/metisMenu.min.js"></script>
	<script src="/Homer/vendor/bootstrap-toggle-master/js/bootstrap-toggle.js"></script>
    <script src="/Homer/vendor/toastr/build/toastr.min.js"></script>
	<!-- Vendor styles -->
	<link rel="stylesheet" href="/Homer/vendor/fontawesome/css/font-awesome.css" />
	<link rel="stylesheet" href="/Homer/vendor/metisMenu/dist/metisMenu.css" />
	<link rel="stylesheet" href="/Homer/vendor/animate.css/animate.css" />
             
	<!-- App styles -->
	<link rel="stylesheet" href="/Homer/fonts/pe-icon-7-stroke/css/pe-icon-7-stroke.css" />
	<link rel="stylesheet" href="/Homer/fonts/pe-icon-7-stroke/css/helper.css" />
	<link rel="stylesheet" href="/Homer/styles/style.css">
	<link rel="stylesheet" href="/Homer/vendor/bootstrap/dist/css/bootstrap.css" />
	<link rel="stylesheet" href="/Homer/vendor/bootstrap-toggle-master/css/bootstrap2-toggle.css" />
    <link rel="stylesheet" href="/Homer/vendor/toastr/build/toastr.min.css" />

	<link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />

	<link rel='manifest' href='/manifest.json'>
	
	<script type="text/javascript">
		window.addEventListener("load", () => {
			if ("serviceWorker" in navigator) {
    			navigator.serviceWorker.register("service-worker.js");
		  	}
		});
	</script>

<meta name="mobile-web-app-capable" content="yes" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<meta name="application-name" content="Stability" />
<meta name="apple-mobile-web-app-title" content="Stability" />
<meta name="msapplication-starturl" content="/Default.aspx" />

	<script>
		window.dataLayer = window.dataLayer || [];
		function gtag() { dataLayer.push(arguments); }
		gtag('js', new Date());

		gtag('config', 'UA-96039646-1');
		
	</script>

	<script type="text/javascript">
		$(document).ready(function () {

			var volunteerURL = '';
			var nonProfitURL = '';
			var businessURL = '';

			$(".panelHelper").click(function () {
				if (volunteerURL) {
					location.href = volunteerURL;
				}
				else {
					DisasterSelectAlert("Choose a Disaster", "To Volunteer, please choose the disaster that affected you.", this);
				}
			});

			$(".panelNonProfit").click(function () {
				if (nonProfitURL) {
					location.href = nonProfitURL;
				}
				else {
					DisasterSelectAlert("Choose a Disaster", "To create a nonprofit campaign, please choose the disaster that affected you.", this);
				}
			});

			$(".panelBusiness").click(function () {
				if (businessURL) {
					location.href = businessURL;
				}
				else {
					DisasterSelectAlert("Choose a Disaster", "To register your business, please choose the disaster that affected you.", this);
				}
			});

			$("#disaster").change(function ()
				{
					var selectedEventFriendlyName = $(this).children("option:selected").val();
					var selectedEventName = $("option:selected").text();
					$("#spanDisasterName").html("Select the role you can play in helping people recover from <b>" +  selectedEventName + "</b>");

					$(".panelHelper").css("background-color", "#3498db");
					$(".panelNonProfit").css("background-color", "#3498db");
					$(".panelBusiness").css("background-color", "#3498db");

					volunteerURL = '/' + selectedEventFriendlyName + '/Helper';
					nonProfitURL = '/' + selectedEventFriendlyName + '/NonProfit';
					businessURL = '/' + selectedEventFriendlyName + '/Business';
			});
            
		});

		function DisasterSelectAlert(titleText, bodyText, obj)
		{
			$(obj).click(function () {
				swal({
					title: titleText,
					text: bodyText
				});
			});
		};
	</script>

	<!-- Your Basic Site Informations -->
	<title>Create A Community Disaster Relief Team - Stability</title>
	<meta charset="UTF-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="description" content="Turn your friends and family into an impactful disaster relief team to help your community.">
	<meta name="keywords" content="Cajun Navy, Ground Force, Ground Force Humanitarian Aid, Cajun Navy Ground Force, CajunNavy, Stability, Disaster Relief, Disaster, Disaster Response, Hurricane, Tornado, Flood, Wildfire, Heatwave">
	<meta name="author" content="Rob Gaudet, President, Stability">

	<meta property="og:image" content="https://www.Stability.org/V1/images/MSTEAM.png" />
	<meta property="og:title" content="Create A Community Disaster Relief Team - Stability" />
	<meta property="og:description" content="Turn your friends and family into an impactful disaster relief team to help your community." />

	<meta property="og:url" content="http://www.Stability.org/Default.aspx" />

	<!-- Mobile Specific Meta -->
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">

	<!-- Fonts -->
	<link href="https://fonts.googleapis.com/css?family=Noto+Sans:400,400italic,700" rel="stylesheet" type="text/css">
	<link href="https://fonts.googleapis.com/css?family=Raleway:400,500,700" rel="stylesheet" type="text/css">

	<!-- Stylesheets -->
	<link rel="stylesheet" href="/Web/css/bootstrap.min.css">
	<link rel="stylesheet" href="/Web/css/font-awesome.min.css">
	<link rel="stylesheet" href="/Web/css/slick.css">
	<link rel="stylesheet" href="/Web/css/slick-theme.css">
	<link rel="stylesheet" href="/Web/css/jquery.fancybox.css">
	<link rel="stylesheet" href="/Web/css/animate.min.css">
	<link rel="stylesheet" href="/Web/css/style.css">

	<!-- Custom Colors -->
	<!--<link rel="stylesheet" href="css/colors/green/color.css">-->
	<!--<link rel="stylesheet" href="css/colors/orange/color.css">-->
	<!--<link rel="stylesheet" href="css/colors/pink/color.css">-->
	<!--<link rel="stylesheet" href="css/colors/purple/color.css">-->
	<!--<link rel="stylesheet" href="css/colors/yellow/color.css">-->
	<!--[if lt IE 9]>
	<script src="/Web/js/html5.js"></script>
	<script src="/Web/js/respond.min.js"></script>
	<![endif]-->
	<!--[if lt IE 8]>
		<link rel="stylesheet" href="css/ie-older.css">
	<![endif]-->

	<noscript><link rel="stylesheet" href="/Web/css/no-js.css"></noscript>
	
	<link rel="apple-touch-icon" sizes="120x120" href="/apple-touch-icon.png">
	<link rel="icon" type="image/png" sizes="32x32" href="/favicon-32x32.png">
	<link rel="icon" type="image/png" sizes="16x16" href="/favicon-16x16.png">
	<link rel="manifest" href="/site.webmanifest">
	<link rel="mask-icon" href="/safari-pinned-tab.svg" color="#5bbad5">
	<meta name="msapplication-TileColor" content="#da532c">
	<meta name="theme-color" content="#b16565">
	<!-- Favicons
	<link rel="apple-touch-icon" href="images/apple-touch-icon.png">
	<link rel="apple-touch-icon" sizes="72x72" href="images/apple-touch-icon-72x72.png">
	<link rel="apple-touch-icon" sizes="114x114" href="images/apple-touch-icon-114x114.png">
		-->
	<style>
		.text-white {
			color: white;
		}

		.donate {
			background-color: white;
		}

		a:link {
			color: white;
		}

		/* visited link */
		a:visited {
			color: white;
		}

		/* mouse over link */
		a:hover {
			color: hotpink;
		}

		/* selected link */
		a:active {
			color: blue;
		}
		.homeLink
		{
			color:white;
		}
		.header-txt {
			margin-top:0px;
		}
		.navhead
		{
		background-color:#29498c;
		}
		
		.panelCount
		{
		margin-top:5px;
		background-color:#34495e;
		color:white;
		}

		.panelHelper, .panelNonProfit, .panelBusiness
		{
		margin-top:5px;
		background-color:#CCCCCC;
		color:white;
		}
		.panelSurvivor:hover, .panelHelper:hover, .panelNonProfit:hover, .panelBusiness:hover
		{
			cursor:pointer;
			border:solid white 1px;
		}
		h3{
		color:white;
		}

		#disaster:hover
		{
			cursor:pointer;
			background-color:darkgray;
			color:white;
		}

		#disaster
		{
			margin-left:10px;
			margin-right:10px;
			font-size: 17px;
			color:black;
			padding:3px;
			padding-left:20px;
			padding-right:20px;
		}
	</style>
</head>
<body>
	
	<!-- #header -->
	<header id="header" data-parallax="scroll" data-speed="0.2" data-natural-width="1920" data-natural-height="1080" data-image-src="/V1/images/MSTEAM.png">

		<!-- #navigation -->
		<nav id="navigation" class="navbar navhead">
			<!-- .container -->
			<div class="container">
				<div class="navbar-brand">
					<asp:HyperLink ID="hypLogo" runat="server">
						<img src="/Web/images/logo.png" alt="Logo" />
					</asp:HyperLink>
				</div>
				<ul class="nav navbar-nav">
					<li><a href="#features" class="smooth-scroll" style="color:white;">Features</a></li>
					<li><a href="#portal" class="smooth-scroll" style="color:white;">Community Portals</a></li>
					<li><a href="#contact" class="smooth-scroll" style="color:white;">Contact</a></li>
					<li><a href="/SignIn" class="smooth-scroll" style="color:white;">Sign In</a></li>
					<li class="menu-btn"><a href="/Register" class="smooth-scroll" style="color:white;">Team Registration</a></li>
				</ul>
			</div>
			<!-- .container end -->

		</nav>
	</header>











<section id="splash">
	<div id="join" class="bg-color bg-parallax" data-parallax="scroll" data-speed="0.2" data-natural-width="1920" data-natural-height="1080" data-image-src="/V1/images/MSTEAM.png">
		<div class="bg-overlay bg-overlay50 padding-top50">
			<!-- .container -->
			<div class="container">
				<div class="post-heading-center">
					<div class="row">
						<div class="col-xs-12">
							<h2>STABILITY</h2>
                            <h1>Community Disaster Relief Teams</h1>
						</div>
					</div>
                    <br />
					<div class="row">
						<div class="col-sm-12">
							 <h3>The size of the response is never equal to the impact of the disaster. Join Stability to help change that. </h3>
							<h1>Help your community after a disaster. </h1>
							<p class="left-align">
								We believe that capable members of any community can and must help each other, especially the vulnerable, after a natural disaster.
							</p>
							<p>
								<b>Create your own disaster relief team now.</b>
							</p>
							<a href="/Register" class="btn btn-success" style="color:white; font-size:18px;">Create Or Join A Team</a>
						</div>
					</div>
					<div class="row">
						<div class="col-sm-12 h-bg-yellow m-t-lg ">
							<div class="m-lg font-bold p-5" style="color:darkslategrey; padding:10px;">
								<h3 style="color:darkslategrey; margin-bottom:0px;">Need help?</h3>
								Create a help request to connect with a Community Disaster Relief Team
								<br />
								<a href="/GetHelp" class="btn btn-danger" style="color:white;  font-size:18px; margin:6px;">Request Help</a>
								<br /><br />
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	</section>
<section id="features">
    <div class="container" style="margin-top:50px;">
        <div class="row text-center m-t-lg">
            <div class="col-lg-12">
                <div class="post-heading-center">
					<h2>Disaster Relief Team <span>Features</span></h2>
				</div>
                <p>
                   Stabilities features connect you with capable co-workers, friends and family. With the right information and the right tools, we can all make an impact in our communities when disaster strikes.
                </p>
            </div>
        </div>
        <div class="row m-t-lg">
            <div class="col-md-4 col-md-offset-2">
                <strong>Organize a Team > Respond to Tickets</strong>
                <p>
					Families, civic organizations, fraternaties, sororities, sports teams, businesses... even groups of friends with a passion for helping can organize their own disaster response teams.
                </p>
            </div>
            <div class="col-md-4">
                <strong>Review Your Teams Activity</strong>
                <p>
					Disaster relief requires hands on labor, lots of it. Schedule and track people and things, recruit volunteers, raise funds and much more from the Team Activity Page.
                </p>
            </div>
        </div>
        <div class="row text-center m-t-lg">
            <div class="col-lg-12">
                <div class="post-heading-center">
					<h2>Respond Directly to <span>Help Requests</span></h2>
				</div>
                <p>
				Once your team is approved, you will see real help request tickets in your community that your disaster relief team can respond to.
            </div>
        </div>
        <div class="row m-t-lg">
            <div class="col-md-4 col-md-offset-2">
                
                <strong>Collaborate and Share Deployments</strong>
                <p>
					<ul>
						<li>Find and Add Friends</li>
						<li>Follow Other Communities</li>
						<li>See Other Teams</li>
					</ul>
                </p>
            </div>
            <div class="col-md-4">
                <strong>Track Everything on The Community Portal</strong>
                <p>
					<ul>
						<li>View Real Time Information</li>
						<li>Create a Public Website</li>
						<li>Share Uplifting Stories</li>
					</ul>
                </p>
               Much more!
            </div>
        </div>
        <div class="row">
			<div class="col-md-6 col-md-offset-3 text-center m-t-lg">
				<a href="/Register" class="btn btn-success" style="color:white; font-size:18px;">Create Or Join A Team</a>
				<hr />
			</div>
        </div>
    </div>
</section>
<section id="portal" class="bg-light">
    <div class="container">
        <div class="row text-center m-t-lg">
            <div class="col-lg-12">
                <div class="post-heading-center">
					<h2>Community <span>Portals</span></h2>
				</div>
                <p>When disaster strikes and your neighbors are in trouble, there is no time to waste.<br />Get ready today by forming a disaster relief team to begin helping your community right away.</p>
            </div>
        </div>
        <div class="row text-center">
            <div class="col-md-3">
                <h4 class="m-t-lg"><i class="pe-7s-umbrella pe-2x text-primary icon-big"></i></h4>
                <strong>Help Aging Americans</strong>
                <p>Today, over 100 Million Americans are over age 60. This is the reality for the next 50 years, it's time for an American movement to assist when they are in crisis.</p>
            </div>
            <div class="col-md-3">
                <h4 class="m-t-lg"><i class="pe-7s-global pe-2x text-danger icon-big"></i></h4>
                <strong>Create A Team Website</strong>
                <p>Your church, civic organization or sports team probably has a website but it's not designed to organize disaster efforts. A Stability website automatically updates your greater congregation or community with your efforts.</p>
            </div>
            <div class="col-md-3">
                <h4 class="m-t-lg"><i class="pe-7s-users pe-2x text-primary icon-big"></i></h4>
                <strong>Recruit Team Members</strong>
                <p>Communities have capable individuals who are willing to help, all they need is someone to step and organize them. Stability is a team based platform for learning how best to respond then making things happen, fast.</p>
            </div>
            <div class="col-md-3">
                <h4 class="m-t-lg"><i class="pe-7s-gleam pe-2x text-success icon-big"></i></h4>
                <strong>Respond To Requests for Help</strong>
                <p>Once approved, your team will be able to see and respond directly to help requests from the most vulnerable members of your community.</p>
            </div>
        </div>
        <div class="row text-center">
            <div class="col-md-3">
                <h4 class="m-t-lg"><i class="pe-7s-display1 pe-2x text-warning icon-big"></i></h4>
                <strong>Collaborate with Other Teams</strong>
                <p>When each team shares what they are doing, badly needed resources can be properly spread into communities that are being underserved.</p>
            </div>
            <div class="col-md-3">
                <h4 class="m-t-lg"><i class="pe-7s-share pe-2x text-danger icon-big"></i></h4>
                <strong>Inspire, Connect and Share</strong>
                <p>Connect with other team members and show gratitude for their work. When we tell stories, it expands the relief and more people get helped.</p>
            </div>
            <div class="col-md-3">
                <h4 class="m-t-lg"><i class="pe-7s-science pe-2x text-warning icon-big"></i></h4>
                <strong>Track Every Minute & Every Morsel</strong>
                <p>Every effort doubles in value when it's tracked. Your efforts help your county/parish Office of Emergency Managment save money when applying for FEMA Grants. Stability helps you track every minute of effort and donated resource. </p>
            </div>
            <div class="col-md-3">
                <h4 class="m-t-lg"><i class="pe-7s-airplay pe-2x text-success icon-big"></i></h4>
                <strong>Earn Certifications</strong>
                <p>Prepare your team with a full range of quick and easy to learn online Trainings and Certifications based on years of realy experience.</p>
            </div>
        </div>
        <div class="row">
			<div class="col-md-6 col-md-offset-3 text-center m-t-lg">
				<a href="/Register" class="btn btn-success" style="color:white; font-size:18px;">Create Or Join A Team</a>
				<hr />
			</div>
        </div>
    </div>
</section>

	<!-- #contact -->
	<div id="contact" class="container-padding10060">

		<!-- .container -->
		<div class="container">

			<div class="post-heading-center">
				<h2>Get in <span>Touch</span></h2>
			</div>

			<!-- .row -->
			<div class="row">

				<div class="col-sm-6 margin-bottom40">
					<div class="affa-contact-info alignright">
						<!-- 1 -->
						<img src="/Web/images/content/icon/phone-call-6.png" alt="Icon" class="animation" data-animation="animation-fade-in-left">
						<h4>Phone Number</h4>
						<p>(318) 572-3161</p>
					</div>
				</div>

				<div class="col-sm-6 margin-bottom40">

					<div class="affa-contact-info alignleft">
						<!-- 2 -->
						<img src="/Web/images/content/icon/mail-2.png" alt="Icon" class="animation" data-animation="animation-fade-in-left">
						<h4>Email Address</h4>
						<p>Support@Stability.org</p>
					</div>
				</div>

			</div>
			<!-- .row end -->

		</div>
		<!-- .container end -->

	</div>
	<!-- #contact end -->
	<!-- #footer -->
	<footer id="footer">

		<div class="container">
			<div class="footer-logo animation" data-animation="animation-fade-in-down"><img src="/Web/images/logo.png" alt="Logo"></div>
			<div class="footer-socials">
				<a href="http://www.Facebook.com/CrowdRelief" title="Facebook" class="animation" data-animation="animation-bounce-in"><i class="fa fa-facebook"></i></a>
				<a href="http://www.Twitter.com/CrowdReliefApp" title="Twitter" class="animation" data-animation="animation-bounce-in" data-delay="200"><i class="fa fa-twitter"></i></a>
				<a href="http://www.Instagram.com/CrowdRelief" title="Instagram" class="animation" data-animation="animation-bounce-in" data-delay="400"><i class="fa fa-instagram"></i></a>
			</div>
		</div>

		<div class="footer-copyright">
			<div class="container">
				Stability was created to empower citizens to help neighbors affected by disasters recovery more quickly.
				<br />
				<p>&copy; 2024 Copyright. All Rights Reserved. Created by <a href="http://www.Stability.org" target="_blank">Stability</a></p>
			</div>
		</div>

	</footer>
	<!-- #footer end -->

</body>

</html>