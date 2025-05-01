<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Narrow.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="V1_Login" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/1-Column-Narrow.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
		<script>

			$(document).ready(function () {
				$(".loginLogo").click(function () {
					document.location.href = "/default.aspx";
				});
				$("body").tooltip({ selector: '[data-toggle=tooltip]' });
			});
		</script>

		<style>
			.loginLogo
			{
				margin:10px;
				width:300px;
			}
			.loginLogo:hover
			{
				cursor: pointer;
			}
		</style>
	<style>
		
		.rotating-logo:hover .spin-label {
			color: #5a2ca0;
		}
		/* Base logo container */
		.rotating-logo {
			justify-content: center;
			align-items: center;
			margin: 10px auto;
			width: 100px;
			height: 100px;
			display: flex;
			flex-direction: column;
			align-items: center;
			text-align: center;
			cursor: pointer; /* 👈 This makes the pinwheel show the finger cursor on hover */
		}


		/* Already defined keyframes */
		@keyframes spin {
			from {
				transform: rotate(0deg);
			}

			to {
				transform: rotate(360deg);
			}
		}

		@keyframes spin-reverse {
			from {
				transform: rotate(0deg);
			}

			to {
				transform: rotate(-360deg);
			}
		}

		/* Base logo */
		.rotating-logo img {
			width: 100px;
			height: auto;
			animation: spin 5s linear infinite;
			transition: all 0.3s ease-in-out;
		}

		/* Behaviors */
		.rotating-logo.fast img {
			animation-duration: 0.8s;
		}

		.rotating-logo.slow img {
			animation-duration: 12s;
		}

		.rotating-logo.reverse img {
			animation-name: spin-reverse;
		}

		.rotating-logo.paused img {
			animation-play-state: paused;
		}

		.rotating-logo.burst img {
			animation-duration: 0.4s;
		}
		#btnSubmit {
			position: relative;
		}
		.error-message {
			color: #b20000;
			background-color: #ffe6e6;
			padding: 8px 12px;
			border: 1px solid #ffcccc;
			border-radius: 6px;
			display: inline-block;
			margin-top: 8px;
			font-weight: bold;
			font-size:14px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
        <div class="row">
            <div class="col-xs-2 col-sm-4 col-lg-4"></div>
            <div class="col-xs-8 col-sm-4 col-lg-4" style="min-width:320px !important;max-width:320px !important;">
				<div class="middle-box text-center loginscreen animated fadeInDown">
					<h1 class="loginLogo">
					<div class="rotating-logo" id="rotatingLogo">
						<img src="/V1/Images/pinwheel.png" alt="Stability Logo" />
					</div>
						<img class="img-responsive" src="/V1/Images/Logo-Horizontal-cs.png" />
					</h1>
					<div class="m-t-sm text-center">
						<h4>
						Disaster-Ready Communities
						</h4>
					</div>
					<p>
						Be the lifeline your community needs.
						<asp:Label ID="lblErrorMessage" CssClass="error-message" runat="server" Visible="false"></asp:Label>
					</p>
				</div>
				<div class="m-t" role="form">
					<div class="form-group">
						<asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" placeholder="Username" required=""></asp:TextBox>
					</div>
					<div class="form-group">
						<asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server" placeholder="Password" required=""></asp:TextBox>
					</div>
					<asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success btn-block m-b" Text="Sign In" OnClick="btnSubmit_Click" OnClientClick="return showSpinner();" />
					<button 
						id="btnLoading" 
						type="button" 
						class="btn btn-success btn-block m-b" 
						disabled 
						style="display:none;">
						<img src="/V1/Images/logo-icon-70x70-white-transparent.png" alt="Loading..." style="width:24px; height:24px; animation: spin 1s linear infinite;" />
					</button>
					<div class="text-center">
					<a href="/V1/PasswordReset.aspx"><small>Forgot password?</small></a>
					<a href="/V1/GetUserName.aspx"><small style="margin-left:5px;">Forgot username?</small></a>
						</div>
					<p class="text-muted text-center"><small>Do not have an account?</small></p>
					<a class="btn btn-sm btn-info btn-block" href="/Register">Create an Account</a>
				</div>
				<p class="m-t-lg">
					<small>Stability empowers community groups to become force multipliers after natural disasters coordinating efforts between citizen run organizations and local emergency managers.</small>
				</p>
            </div>
            <div class="col-xs-2 col-sm-4 col-lg-4"></div>
		</div>
	<script>
		document.addEventListener('DOMContentLoaded', function () {
			const logo = document.getElementById('rotatingLogo');
			const behaviors = ['fast', 'slow', 'paused', 'burst', '']; // random fun
			let isReverse = false; // toggle tracker

			function spinLogo(event) {
				event.stopPropagation(); // 🚫 Prevent redirect via parent click

				// Remove previous classes
				logo.classList.remove('fast', 'slow', 'paused', 'burst', 'reverse');

				// Toggle direction
				isReverse = !isReverse;
				if (isReverse) {
					logo.classList.add('reverse');
				}

				// Apply one random spin behavior
				const behavior = behaviors[Math.floor(Math.random() * behaviors.length)];
				if (behavior) {
					logo.classList.add(behavior);
				}
			}

			// Trigger the spin on both events
			logo.addEventListener('click', spinLogo);
			logo.addEventListener('mouseenter', spinLogo);
		});

		function showSpinner() {
			var btn = document.getElementById('<%= btnSubmit.ClientID %>');
		var loadingBtn = document.getElementById('btnLoading');

		// Hide the real ASP.NET button
		btn.style.display = 'none';

		// Show the spinner button
		loadingBtn.style.display = 'inline-block';

		// Trigger ASP.NET postback manually
		__doPostBack('<%= btnSubmit.UniqueID %>', '');

		return false; // Prevent default postback
	}
	</script>
</asp:Content>