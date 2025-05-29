<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Narrow.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="TeamSelectionForRegistration.aspx.cs" Inherits="V1_TeamSelectionForRegistration" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/1-Column-Narrow.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Stability - Register by Selecting Your Team</title>
    <link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
    <link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
    <script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>
	
    <style>
        .form-container {
            margin-top: 20px;
        }

        .large-dropdown {
            font-size: 20px;
            padding: 10px;
            width: 100%;
            max-width: 500px;
        }
		.skip-link {
            display: inline-block;
            margin-top: 10px;
            font-size: 16px;
            color: #6c757d;
            text-decoration: underline;
        }

        .skip-link:hover {
            color: #5a6268;
            text-decoration: none;
        }
         .logo-name:hover {
            cursor: pointer;
        }

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
    </style>
    <script>
		$(document).ready(function () {
			$(".logo-name").click(function () {
				document.location.href = "/default.aspx";
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="container">
            <div class="row">
                <div class="col-11 col-sm-11 col-md-11 col-lg-11">
                    <div class="middle-box loginscreen animated fadeInDown">
                        <h1 class="loginLogo">
                            <div class="rotating-logo" id="rotatingLogo">
                                <img src="/V1/Images/pinwheel.png" alt="Stability Logo" />
                            </div>
                        </h1>
                    <div class="m-t" role="form">
						<div class="hpanel">
										<h3>
											Select Your Rotary Club – <asp:Literal runat="server" ID="litParentTeam"></asp:Literal>
										</h3>
										<p>
										Welcome! To begin your registration, please select your Rotary Club from the list below. These clubs are part of Rotary District 6840, serving communities across Southeast Louisiana and the Mississippi Gulf Coast.
											</p>
										<p>
										Choosing your club will help us personalize your registration and connect you with the right local leadership. If you don’t see your club listed, you can skip this step and continue with general registration.
										</p>
									    <asp:Panel runat="server" CssClass="form-container">
											<asp:DropDownList ID="ddlOrganizations"
															  runat="server"
															  CssClass="large-dropdown"
															  AutoPostBack="true"
															  OnSelectedIndexChanged="ddlOrganizations_SelectedIndexChanged">
											</asp:DropDownList>
											<br /><br />
											<a href="/register.aspx" class="skip-link">I don't see my club</a>
										</asp:Panel>
                        </div>
                    </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
