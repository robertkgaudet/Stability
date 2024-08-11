<%@ page title="" language="C#" masterpagefile="~/V1//MasterPages/Homer.master" autoeventwireup="true" inherits="SurvivorRegister, App_Web_h4gntnip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<!-- Simple splash screen-->
				<div class="splash">s
					<div class="color-line"></div>
					<div class="splash-title">
						<h1>Homer - Responsive Admin Theme</h1>
						<p>Special AngularJS Admin Theme for small and medium webapp with very clean and aesthetic style and feel. </p>
						<div class="spinner"> <div class="rect1"></div> <div class="rect2"></div> <div class="rect3"></div> <div class="rect4"></div> <div class="rect5"></div> </div> </div> </div>
				<!--[if lt IE 7]>
				<p class="alert alert-danger">You are using an <strong>outdated</strong> browser. Please <a href="http://browsehappy.com/">upgrade your browser</a> to improve your experience.</p>
				<![endif]-->

				<!-- Header -->
				<div id="header">
					<div id="logo" class="light-version">
						<span>
							CrowdRelief
						</span>
					</div>
					<nav role="navigation">
						<div class="header-link hide-menu"><i class="fa fa-bars"></i></div>
						<div class="small-logo">
							<span class="text-primary">APP NAME</span>
						</div>
						<div class="mobile-menu">
							<button type="button" class="navbar-toggle mobile-menu-toggle" data-toggle="collapse" data-target="#mobile-collapse">
								<i class="fa fa-chevron-down"></i>
							</button>
							<div class="collapse mobile-navbar" id="mobile-collapse">
								<ul class="nav navbar-nav">
									<li>
										<a class="" href="#">Link</a>
									</li>
									<li>
										<a class="" href="#">Link</a>
									</li>
								</ul>
							</div>
						</div>
						<div class="navbar-right">
							<ul class="nav navbar-nav no-borders">
								<li>
									<a href="#">
										<i class="pe-7s-upload pe-rotate-90"></i>
									</a>
								</li>
							</ul>
						</div>
					</nav>
				</div>

				<!-- Navigation -->
				<aside id="menu">
					<div id="navigation">
						<div class="profile-picture">

							<div class="stats-label text-color">
								<span class="font-extra-bold font-uppercase">Username</span>

								<div class="dropdown">
									<a class="dropdown-toggle" href="#" data-toggle="dropdown">
										<small class="text-muted">Links <b class="caret"></b></small>
									</a>
									<ul class="dropdown-menu animated flipInX m-t-xs">
										<li><a href="#">Example link</a></li>
										<li><a href="#">Example link</a></li>
									</ul>
								</div>


							</div>
						</div>

						<ul class="nav" id="side-menu">
							<li class="active">
								<a href="index.html"> <span class="nav-label">Page 1</span> <span class="label label-success pull-right">start</span> </a>
							</li>
							<li>
								<a href="page2.html"> <span class="nav-label">Page 2</span> </a>
							</li>

						</ul>
					</div>
				</aside>

				<!-- Main Wrapper -->
				<div id="wrapper">

					<div class="content animate-panel">
						<div class="row">
							<div class="col-lg-12">
								<div class="hpanel">
									<div class="panel-heading">
										<div class="panel-tools">
											<a class="showhide"><i class="fa fa-chevron-up"></i></a>
											<a class="closebox"><i class="fa fa-times"></i></a>
										</div>
										Title
									</div>
									<div class="panel-body">
										Page 1
									</div>
								</div>
							</div>
						</div>
					</div>

					<!-- Footer-->
					<footer class="footer">
						<span class="pull-right">
							Example text
						</span>
						Company 2016
					</footer>

				</div>
</asp:Content>

