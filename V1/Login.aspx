<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Narrow.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="V1_Login" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/1-Column-Narrow.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />

    <script>
        $(document).ready(function () {
            $(".loginLogo").click(function () {
                document.location.href = "/default.aspx";
            });
            $("body").tooltip({ selector: '[data-toggle=tooltip]' });
        });
    </script>

    <style>
        .loginLogo {
            margin: 10px;
            width: 300px;
        }

        .loginLogo:hover {
            cursor: pointer;
        }

        .rotating-logo:hover .spin-label {
            color: #5a2ca0;
        }

        .rotating-logo {
            margin: 10px auto;
            width: 100px;
            height: 100px;
            cursor: pointer;
            margin-right: 135px;
        }

        @keyframes spin {
            from { transform: rotate(0deg); }
            to { transform: rotate(360deg); }
        }

        @keyframes spin-reverse {
            from { transform: rotate(0deg); }
            to { transform: rotate(-360deg); }
        }

        .rotating-logo img {
            width: 100px;
            height: auto;
            animation: spin 5s linear infinite;
            transition: all 0.3s ease-in-out;
        }

        .rotating-logo.fast img { animation-duration: 0.8s; }
        .rotating-logo.slow img { animation-duration: 12s; }
        .rotating-logo.reverse img { animation-name: spin-reverse; }
        .rotating-logo.paused img { animation-play-state: paused; }
        .rotating-logo.burst img { animation-duration: 0.4s; }

        #btnSubmit { position: relative; }

        .error-message {
            color: #b20000;
            background-color: #ffe6e6;
            padding: 8px 12px;
            border: 1px solid #ffcccc;
            border-radius: 6px;
            display: inline-block;
            margin-top: 8px;
            font-weight: bold;
            font-size: 14px;
        }

        h1.loginLogo {
            margin-left: 20px;
        }

        .m-t-sm.text-center {
            margin-right: 10px;
        }

        @media(min-width: 992px) {
            .max-500-width {
                max-width: 500px;
            }
        }

        @media(max-width: 1200px) {
            .container, .row {
                width: 100% !important;
                margin-left: 0 !important;
                margin-right: 0 !important;
            }
        }

        .rotating-logo { margin: 0 auto !important; }
        h1.loginLogo { margin: auto !important; }

        .btn-social,
        .btn-social:hover,
        .btn-social:visited,
        .btn-social:active {
            color: #fff !important;
            text-decoration: none;
        }


            .btn-social {
                display: flex;
                align-items: center;
                justify-content: center;
                font-weight: bold;
                font-size: 14px;
                padding: 6px 5px;
                border-radius: 4px;
                color: #fff;
                white-space: nowrap;
                transition: background-color 0.3s ease; /* Smooth transition */
            }

            .btn-facebook {
                background-color: #3b5998;
                border: 1px solid #3b5998;
            }

            .btn-facebook:hover {
                background-color: #2d4373; /* Darker shade */
                border-color: #2d4373;
            }

            .btn-google {
                background-color: #4285f4;
                border: 1px solid #4285f4;
                margin-left: 12px;
            }

            .btn-google:hover {
                background-color: #3367d6; /* Darker shade */
                border-color: #3367d6;
            }

            .btn-social i {
                margin-right: 6px;
                font-size: 16px;
            }

    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-xs-2 col-sm-4 col-lg-4"></div>
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-sm-12 col-md-6 col-lg-4">
                    <div class="middle-box text-center loginscreen animated fadeInDown w-100">
                        <div class="text-center">
                            <h1 class="loginLogo m-0">
                                <div class="rotating-logo" id="rotatingLogo">
                                    <img src="/V1/Images/pinwheel.png" alt="Stability Logo" />
                                </div>
                                <img class="img-responsive" style="margin: 0 auto;" src="/V1/Images/Logo-Horizontal-cs.png" />
                            </h1>
                        </div>
                        <div class="m-t-sm text-center">
                            <h4>Disaster-Ready Communities</h4>
                            <p>
                                Be the lifeline your community needs.
                                <asp:Label ID="lblErrorMessage" CssClass="error-message" runat="server" Visible="false"></asp:Label>
                            </p>
                        </div>
                    </div>

                    <div class="m-t" role="form">
                        <div class="form-group">
                            <asp:TextBox ID="txtUsername" CssClass="form-control" runat="server" placeholder="Username" required=""></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtPassword" CssClass="form-control" TextMode="Password" runat="server" placeholder="Password" required=""></asp:TextBox>
                        </div>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success btn-block m-b" Text="Sign In" OnClick="btnSubmit_Click" OnClientClick="return showSpinner();" />
                        <button id="btnLoading" type="button" class="btn btn-success btn-block m-b" disabled style="display: none;">
                            <img src="/V1/Images/logo-icon-70x70-white-transparent.png" alt="Loading..." style="width: 24px; height: 24px; animation: spin 1s linear infinite;" />
                        </button>

             <div class="text-center m-t-sm d-flex justify-content-between gap-2" style="display: flex; gap: 10px;">
                <asp:LinkButton ID="btnFacebook" runat="server" CssClass="btn btn-social btn-facebook flex-fill" OnClick="btnFacebook_Click" >
                    <i class="fab fa-facebook-f"></i>Sign in with Facebook
                </asp:LinkButton>

                <asp:LinkButton ID="btnGoogle" runat="server" CssClass="btn btn-social btn-google flex-fill" OnClick="btnGoogle_Click">
                    <i class="fab fa-google"></i>Sign in with Google
                </asp:LinkButton>
            </div>



                        <div class="text-center">
                            <a href="/V1/PasswordReset.aspx"><small>Forgot password?</small></a>
                            <a href="/V1/GetUserName.aspx"><small style="margin-left: 5px;">Forgot username?</small></a>
                        </div>

                        <p class="text-muted text-center"><small>Do not have an account?</small></p>
                        <a class="btn btn-sm btn-info btn-block" href="/Register">Create an Account</a>
                    </div>

                    <p class="m-t-lg">
                        <small>
                            Stability empowers community groups to become force multipliers after natural disasters coordinating efforts between citizen run organizations and local emergency managers.
                        </small>
                    </p>
                </div>
            </div>
        </div>
        <div class="col-xs-2 col-sm-4 col-lg-4"></div>
    </div>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const logo = document.getElementById('rotatingLogo');
            const behaviors = ['fast', 'slow', 'paused', 'burst', ''];
            let isReverse = false;

            function spinLogo(event) {
                event.stopPropagation();
                logo.classList.remove('fast', 'slow', 'paused', 'burst', 'reverse');
                isReverse = !isReverse;
                if (isReverse) logo.classList.add('reverse');
                const behavior = behaviors[Math.floor(Math.random() * behaviors.length)];
                if (behavior) logo.classList.add(behavior);
            }

            logo.addEventListener('click', spinLogo);
            logo.addEventListener('mouseenter', spinLogo);
        });

        function showSpinner() {
            if (document.getElementById('<%= txtUsername.ClientID %>').value != '' &&
                document.getElementById('<%= txtPassword.ClientID %>').value != '') {
                var btn = document.getElementById('<%= btnSubmit.ClientID %>');
                var loadingBtn = document.getElementById('btnLoading');
                btn.style.display = 'none';
                loadingBtn.style.display = 'inline-block';
                __doPostBack('<%= btnSubmit.UniqueID %>', '');
                return false;
            }
        }
    </script>
</asp:Content>
