<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Basic.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="middle-box text-center loginscreen animated fadeInDown">
        <div>
            <div>
                <h1 class="logo-name">
					<div class="rotating-logo" id="rotatingLogo">
						<img src="/V1/Images/pinwheel.png" alt="Stability Logo" />
					</div>
					<img class="img-responsive" src="/V1/Images/Logo-Horizontal.png" />
                </h1>
				<div class="p-sm text-center">
					Welcome to Stability<br /> Disaster relief done differently.
				</div>
            </div>
            <p>Sign in now to get started.</p>
            <div class="m-t" role="form">
                <div class="form-group">
					<asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" placeholder="Username" required=""></asp:TextBox>
                </div>
                <div class="form-group">
					<asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server" placeholder="Password" required=""></asp:TextBox>
                </div>
					<asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success btn-block m-b" Text="Sign In" OnClick="btnSubmit_Click" OnClientClick="return showSpinner();" />

                <a href="/V1/PasswordReset.aspx"><small>Forgot password?</small></a>
                <p class="text-muted text-center"><small>Do not have an account?</small></p>
                <a class="btn btn-sm btn-success btn-block" href="/Register">Create an Account</a>
            </div>
            <p class="m-t">
				<small>We believe every community member has something valuable to contribute after a natural disaster. Stability was created to empower teams to help the most vulnerable affected by disasters to recovery more quickly.</small>
            </p>
        </div>
    </div>

	<script>
		document.addEventListener('DOMContentLoaded', function () {
			const logo = document.getElementById('rotatingLogo');
			const behaviors = ['fast', 'slow', 'paused', 'burst', '']; // random fun
			let isReverse = false; // toggle tracker

			logo.addEventListener('mouseenter', (event) => {
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
			});
		});


		function showSpinner() {
			var btn = document.getElementById('<%= btnSubmit.ClientID %>');

		// Prevent adding multiple spinners
		if (btn.querySelector('.spinner')) return false;
		btn.innerHTML = '';

		// Create spinner image dynamically
		var spinner = document.createElement('img');
		spinner.src = '/V1/Images/pinwheel.png'; // ✅ Confirm this path is correct
		spinner.className = 'spinner';
		spinner.style.width = '20px';
		spinner.style.height = '20px';
		spinner.style.marginLeft = '10px';
		spinner.style.verticalAlign = 'middle';
		spinner.style.animation = 'spin 1s linear infinite';
		spinner.style.display = 'inline-block'; // 🔥 make sure it’s visible

		// Append to button
		btn.appendChild(spinner);

		// Manually trigger ASP.NET postback so DOM has time to update
			__doPostBack('<%= btnSubmit.UniqueID %>', '');

			return false; // Prevent default postback to ensure spinner renders first
		}
	</script>
</asp:Content>