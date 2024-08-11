<%@ page title="" language="C#" masterpagefile="~/S1/MasterPages/HomerNoNavigation.master" enableeventvalidation="false" autoeventwireup="true" inherits="V1_VictimAccount, App_Web_mjkl5wor" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/HomerNoNavigation.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<title>Sign Up for Stability</title>
    <link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
	<script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>

	<script type="text/javascript">
		$(document).ready(function ()
		{
			$(".logo-name").click(function () {
				document.location.href = "/default.aspx";
			});
		});

	</script>
	<style>
		.logo-name:hover
		{
			cursor:pointer;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="middle-box text-center loginscreen animated fadeInDown m-t-lg">
        <div>
            <div class="col-sm-4"></div>
            <div class="col-sm-4">
				<img class="img-responsive logo-name" src="/V1/Images/Logo-Horizontal.png" />
            </div>
            <div class="col-sm-4"></div>
		</div>
	</div>
	<div class="register-container animated fadeInDown">
    <div class="row">
        <div class="col-sm-1"></div>
        <div class="col-md-10">
			<h3>Register to Get Help</h3>
			<p><asp:Label ID="lblMessage" runat="server" text="Create an account first, then you'll create a help request ticket."></asp:Label></p>
			<div runat="server" id="divCaseManagerMessage" visible="false">
				<div class="alert alert-info">
					<a class="alert-link" href="#">NOTICE</a>
					<p>
						You are creating this account on behalf of someone else. The information you enter below will create an account for that person.
						<hr />
						Make sure to enter their proper email address and phone number. The username you enter below will be emailed to the user and a password will be generated for them.
					</p>
				</div>
			</div>
			<div runat="server" id="divError" visible="false">
				<div class="alert alert-danger">
					<a class="alert-link" href="#">REGISTRATION ERROR!!</a>
					<p>
						<asp:Literal ID="litError" runat="server"></asp:Literal>
					</p>
				</div>
			</div>
            <div class="hpanel">
                <div class="panel-body">
					<div class="row">
						<div class="form-group col-lg-12">
							<label>Firstname</label>
							<asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" required="" placeholder="First Name"></asp:TextBox>
						</div>
						<div class="form-group col-lg-12">
							<label>Lastname</label>
							<asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" required="" placeholder="Last Name"></asp:TextBox>
						</div>
						<div class="form-group col-lg-12">
							<label>Email Address</label>
							<asp:TextBox type="email" ID="txtEmail" runat="server" CssClass="form-control" required="" placeholder="Email"></asp:TextBox>
						</div>
						<div class="form-group col-lg-12">
							<label>Phone Number</label>
							<asp:TextBox ID="txtPhoneNumber" onkeypress="return isNumberKey(event)" MaxLength="10" TextMode="Phone" runat="server" CssClass="form-control" required="" placeholder="Phone Number"></asp:TextBox>
						</div>
						<div class="form-group col-lg-12">
							<label>Username</label>
							<asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required="" placeholder="Username"></asp:TextBox>
						</div>
						<div class="form-group col-lg-12" runat="server" id="divPassword">
							<label>Password</label>
							<asp:TextBox type="password" ID="txtPassword" runat="server" CssClass="form-control" required="" placeholder="Password"></asp:TextBox>
						</div>
					</div>
					<div class="text-center">
						<asp:Button CssClass="btn btn-success w-lg" runat="server" id="btnSubmit" OnClick="btnSubmit_Click" Text="Register"/>
						<div class="text-muted text-center">
							<small>
								<asp:Literal ID="litSignInMessage" runat="server"></asp:Literal>
							</small>
						</div>
					</div>
                </div>
            </div>
        </div>
        <div class="col-sm-1"></div>
    </div>
</div>
	<br />
	<br />
	<br />
	<br />
	<br />
	<br />
	<br />
	<br />
	<br />
	<br />
	<br />
	<br />
</asp:Content>