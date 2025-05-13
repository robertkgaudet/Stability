<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Narrow.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="V1_Register" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/1-Column-Narrow.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Register on Stability - Disaster-Ready Communities</title>
    <link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
    <link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
    <link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
    <script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>

    <script>
        var address;
        var city;
        var state;
        var zip;
        var lookupComplete = false;
        var IsDuplicateClear = true;

        $(document).ready(function () {

            $('#divAddressMessage').hide();
            $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
<%--		<%=preselectedDisasterJQuery%>

			$("#disasterEvent.dropdown-menu li").click(function ()
			{
				$("#btn-dropdown.disasterEvent").html($(this).text());
				$("#<%=hidEventId.ClientID%>").val($(this).attr('id'));
            });--%>

			<%=preselectedNonProfitJQuery%>

            $("#nonProfit.dropdown-menu li").click(function () {
                $("#btn-NonProfitDropdown.nonProfit").html($(this).text());
                $("#<%=hidOrganizationId.ClientID%>").val($(this).attr('id'));
            });

            $(".logo-name").click(function () {
                document.location.href = "/default.aspx";
            });
        });

        function CheckDuplicate(controlName, sender) {
            if (sender.value) {
                $.ajax(
                    {
                        type: "GET",
                        url: "/V1/Handlers/GetDuplicateUserDetails.ashx?control=" + controlName + "&value=" + sender.value,
                        contentType: "text/plain; charset=utf-8",
                        dataType: "html",
                        success: function (data) {
                            if (data == "Yes") {
                                IsDuplicateClear = false;
                                if (controlName == "Email") {
                                    $(".error-message-email").show();
                                    $(".response-message-email").hide();
                                }
                                else {
                                    $(".error-message-username").show();
                                    $(".response-message-username").hide();
                                }
                                $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
                            }
                            else {
                                IsDuplicateClear = true;
                                if (controlName == "Email") {
                                    $(".error-message-email").hide();
                                    $(".response-message-email").show();
                                }
                                else {
                                    $(".error-message-username").hide();
                                    $(".response-message-username").show();
                                }
                                if (lookupComplete) {
                                    $("#<%=btnSubmit.ClientID%>").attr("disabled", false);
                                }
                            }
                        },
                        error: function (request, status, error) {
                            if (controlName == "Email") {
                                $(".error-message-email").text("An error occurred. Please try again.").show();
                                $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
                            }
                            else {
                                $(".error-message-username").text("An error occurred. Please try again.").show();
                                $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
                            }
                        }
                    });
            }
            else {
                $(".error-message-username").hide();
                $(".response-message-username").hide();
                $(".error-message-email").hide();
                $(".response-message-email").hide();
                $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
            }
        }

        function CheckAddressValues(controlName, sender) {
            switch (controlName) {
                case "address":
                    if (sender.value) {
                        address = sender.value;
                    }
                    break;
                case "city":
                    if (sender.value) {
                        city = sender.value;
                    }
                    break;
                case "state":
                    if (sender.value) {
                        state = sender.value;
                    }
                    break;
                case "zip":
                    if (sender.value) {
                        zip = sender.value;
                    }
                    break;
                default:
                // code block
            }

            if ((address) && (city) && (state) && (zip) && (lookupComplete == false)) {
                $('#divAddressMessage').show();
                SetLatitudeLongitude(address + " " + city + ", " + state + " " + zip);
            }
        }

        function SetLatitudeLongitude(address) {
            $.ajax(
                {
                    type: "GET",
                    url: "/V1/Handlers/GetGoogleAddressInfo.ashx?address=" + address,
                    contentType: "text/plain; charset=utf-8",
                    dataType: "html",
                    success: function (data) {
                        if (data != "") {
                            var results = data.split("|");
                            var isPartialMatch = results[0];
                            var duplicate = results[12];
                            if ((isPartialMatch == 'False' || isPartialMatch == 'false') && (duplicate == 'False' || duplicate == 'false')) {
                                var latitude = results[1];
                                var longitude = results[2];
                                var street_number = results[3];
                                var street = results[4];
                                var city = results[5];
                                var state = results[6];
                                var country = results[7];
                                var postal_code = results[8];
                                var county = results[9];
                                var googlePlaceId = results[10];
                                var formattedAddress = results[11];
                                var locationType = results[13];
                                var cityCode = results[14];
                                var addressId = results[15];

                                $("#divMapMessage").addClass("alert-success");
                                $("#divMapMessage").removeClass("alert-danger");
                                $("#iFontAwesome").removeClass("fa-warning");
                                $("#iFontAwesome").addClass("fa-map-marker");
                                var successMessage = "Address Lookup Successful!";
                                $("#<%=hidAddressData.ClientID%>").val(data);
                                $('#<%=lblAddressMessage.ClientID%>').text(successMessage);
                                lookupComplete = true;
                                if (IsDuplicateClear) {
                                    $("#<%=btnSubmit.ClientID%>").attr("disabled", false);
                                }
                            }
                            else if (duplicate == 'True' || duplicate == 'true') {
                                //Address already exists.
                                $("#divMapMessage").removeClass("alert-success");
                                $("#divMapMessage").addClass("alert-danger");
                                $("#iFontAwesome").addClass("fa-warning");
                                $("#iFontAwesome").removeClass("fa-map-marker");
                                $("#<%=hidAddressData.ClientID%>").val(data);
                                lookupComplete = false;
                                var errorMessage = " This address already exists (" + address + "). Press 'Next' to edit in the Stability Location Manager. Web Service Message: " + data;
                                $('#<%=lblAddressMessage.ClientID%>').text(errorMessage);
                                if (IsDuplicateClear) {
                                    $("#<%=btnSubmit.ClientID%>").attr("disabled", false);
                                }
                            }
                            else {
                                //Error getting the information
                                $("#divMapMessage").removeClass("alert-success");
                                $("#divMapMessage").addClass("alert-danger");
                                $("#iFontAwesome").addClass("fa-warning");
                                $("#iFontAwesome").removeClass("fa-map-marker");
                                lookupComplete = false;
                                var errorMessage = " Please check your address. Google returned an error matching the address you provided. (" + address + ") Web Service Message: " + data;
                                $('#<%=lblAddressMessage.ClientID%>').text(errorMessage);
                                $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
                            }
                        }
                    },
                    error: function (request, status, error) {
                        $("#divMapMessage").removeClass("alert-success");
                        $("#divMapMessage").addClass("alert-danger");
                        $("#iFontAwesome").addClass("fa-warning");
                        $("#iFontAwesome").removeClass("fa-map-marker");
                        lookupComplete = false;
                        $('#<%=lblAddressMessage.ClientID%>').text(" Error retrieving address information from Google. " + request.statusText + ' - ' + error + ' - ' + status);
                        $("#<%=btnSubmit.ClientID%>").attr("disabled", true);
                    }
                });
        }

    </script>
    <style>
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
    <style>
        .dropdown-wrapper {
            position: relative;
            width: 100%;
        }

        .custom-dropdown {
            width: 100%;
            padding: 10px;
            font-size: 14px;
            border: 1px solid #ced4da;
            border-radius: 5px;
            background-color: #fff;
            color: #495057;
            box-shadow: 0px 2px 5px rgba(0, 0, 0, 0.1);
            outline: none;
            appearance: none;
            cursor: pointer;
        }

            .custom-dropdown:focus {
                border-color: #80bdff;
                box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
            }

        .dropdown-wrapper::after {
            content: "▼";
            position: absolute;
            top: 50%;
            right: 15px;
            transform: translateY(-50%);
            font-size: 12px;
            color: #495057;
            pointer-events: none;
        }

        .response-message {
            font-size: 20px;
            color: green;
            display: none;
        }

        .error-message {
            color: red;
            font-weight: bold;
            display: none;
        }

        #nonProfit {
            max-height: 350px;
            overflow-y: auto;
            overflow-x: hidden;
            font-size: 16px;
            padding: 12px 20px;
        }

            #nonProfit::-webkit-scrollbar {
                width: 16px; /* wider scrollbar */
            }

            #nonProfit::-webkit-scrollbar-thumb {
                background-color: #888; /* color of scrollbar handle */
                border-radius: 8px; /* rounded corners */
            }

                #nonProfit::-webkit-scrollbar-thumb:hover {
                    background-color: #555; /* color on hover */
                }

        img.img-responsive {
            max-width: 100%;
            height: auto;
            display: inline;
        }

        .m-t-sm.text-center {
            margin-right: 40px;
        }

        h1.loginLogo {
            margin-right: 40px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-xs-1 col-sm-2 col-md-3 col-lg-3"></div>
        <div class="col-xs-10 col-sm-8 col-sm-6 col-lg-6" style="min-width: 450px !important; max-width: 500px !important;margin-left:30px !important;">
            <div class="middle-box text-center loginscreen animated fadeInDown">
                <h1 class="loginLogo">
                    <div class="rotating-logo" id="rotatingLogo">
                        <img src="/V1/Images/pinwheel.png" alt="Stability Logo" />
                    </div>
                    <img class="img-responsive" src="/V1/Images/Logo-Horizontal-cs.png" />
                </h1>
                <div class="m-t-sm text-center">
                    <h4>Disaster-Ready Communities
                    </h4>

                    <p>
                        Be the lifeline your community needs.
                    </p>
                </div>
            </div>
            <div class="m-t" role="form">
                <h3>Create an Account</h3>
                Employees, churches or civic organizations can instantly help after a disaster.
				<hr />
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
                            <div class="form-group col-lg-12 line-s" runat="server" id="divChooseNonprofit">
                                <label>Find Your Group</label>
                                <br />
                                <small>Leave empty to create your own.</small>
                                <div id="div2" class="dropdown m-b-md" runat="server">
                                    <button id="btn-NonProfitDropdown" class="btn btn-outline btn-default nonProfit dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Select A Team (Optional) <i class="fa fa-sort-down"></i></button>
                                    <ul id="nonProfit" class="dropdown-menu text-center dropdown-volunteer required">
                                        <%=nonProfitDropDown%>
                                    </ul>
                                </div>
                                <input type="hidden" id="hidOrganizationId" runat="server" />
                            </div>
                            <div class="form-group col-lg-12">
                                <label>First Name  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" required="" placeholder="First Name"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>Last Name  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" required="" placeholder="Last Name"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>Address  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox ID="txtAddress" runat="server" onblur="CheckAddressValues('address', this)" CssClass="form-control" required="" placeholder="Address"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>City  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox ID="txtCity" runat="server" onblur="CheckAddressValues('city', this)" CssClass="form-control" required="" placeholder="City"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label for="ddlState">State <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <div class="dropdown-wrapper">
                                    <select id="ddlState" runat="server" class="custom-dropdown" onblur="CheckAddressValues('state', this)">
                                        <option value="">Select a state</option>
                                        <option value="AL">Alabama</option>
                                        <option value="AK">Alaska</option>
                                        <option value="AZ">Arizona</option>
                                        <option value="AR">Arkansas</option>
                                        <option value="CA">California</option>
                                        <option value="CO">Colorado</option>
                                        <option value="CT">Connecticut</option>
                                        <option value="DE">Delaware</option>
                                        <option value="FL">Florida</option>
                                        <option value="GA">Georgia</option>
                                        <option value="HI">Hawaii</option>
                                        <option value="ID">Idaho</option>
                                        <option value="IL">Illinois</option>
                                        <option value="IN">Indiana</option>
                                        <option value="IA">Iowa</option>
                                        <option value="KS">Kansas</option>
                                        <option value="KY">Kentucky</option>
                                        <option value="LA">Louisiana</option>
                                        <option value="ME">Maine</option>
                                        <option value="MD">Maryland</option>
                                        <option value="MA">Massachusetts</option>
                                        <option value="MI">Michigan</option>
                                        <option value="MN">Minnesota</option>
                                        <option value="MS">Mississippi</option>
                                        <option value="MO">Missouri</option>
                                        <option value="MT">Montana</option>
                                        <option value="NE">Nebraska</option>
                                        <option value="NV">Nevada</option>
                                        <option value="NH">New Hampshire</option>
                                        <option value="NJ">New Jersey</option>
                                        <option value="NM">New Mexico</option>
                                        <option value="NY">New York</option>
                                        <option value="NC">North Carolina</option>
                                        <option value="ND">North Dakota</option>
                                        <option value="OH">Ohio</option>
                                        <option value="OK">Oklahoma</option>
                                        <option value="OR">Oregon</option>
                                        <option value="PA">Pennsylvania</option>
                                        <option value="RI">Rhode Island</option>
                                        <option value="SC">South Carolina</option>
                                        <option value="SD">South Dakota</option>
                                        <option value="TN">Tennessee</option>
                                        <option value="TX">Texas</option>
                                        <option value="UT">Utah</option>
                                        <option value="VT">Vermont</option>
                                        <option value="VA">Virginia</option>
                                        <option value="WA">Washington</option>
                                        <option value="WV">West Virginia</option>
                                        <option value="WI">Wisconsin</option>
                                        <option value="WY">Wyoming</option>
                                    </select>
                                </div>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>Zip Code  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" onblur="CheckAddressValues('zip', this)" required="" placeholder="Zip Code"></asp:TextBox>
                            </div>

                            <div id="divAddressMessage" class="form-group col-lg-12">
                                <div id="divMapMessage" class="alert m-b-lg p-sm">
                                    <i id="iFontAwesome" class="fa"></i>
                                    <asp:Label runat="server" ID="lblAddressMessage"></asp:Label>
                                    <asp:HiddenField ID="hidAddressData" runat="server"></asp:HiddenField>
                                </div>
                            </div>

                            <div class="form-group col-lg-12">
                                <label>Email Address  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox type="email" ID="txtEmail" runat="server" onblur="CheckDuplicate('Email', this)" CssClass="form-control" required="" placeholder="Email"></asp:TextBox>
                                <span class="error-message error-message-email">This email address already exists. Do you want to <a href="/SignIn">Sign In</a>?</span>
                                <span class="response-message response-message-email">
                                    <i class="fa fa-check" aria-hidden="true"></i>
                                </span>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>Phone Number  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox ID="txtPhoneNumber" onkeypress="return isNumberKey(event)" MaxLength="10" TextMode="Phone" runat="server" CssClass="form-control" required="" placeholder="Phone Number"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>Username  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" onblur="CheckDuplicate('Username', this)" required="" placeholder="Username"></asp:TextBox>
                                <span class="error-message error-message-username">This user is already in use.</span>
                                <span class="response-message response-message-username">
                                    <i class="fa fa-check" aria-hidden="true"></i>
                                </span>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>Password  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox type="password" ID="txtPassword" runat="server" CssClass="form-control" required="" placeholder="Password"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12" style="display: flex; align-items: center;">
                                <asp:CheckBox ID="chkMessageOptIn" runat="server" Text="" />
                                <label for="ContentPlaceHolder1_chkMessageOptIn" style="margin: 5px 10px 0px;">Receive SMS Messages For Volunteer Opportunities</label>
                            </div>
                        </div>
                        <div class="text-center center-block justify-content-center">
                            <div class="row">
                                <div class="col-sm-2"></div>
                                <div class="col-sm-8">
                                    <asp:Button CssClass="btn btn-success btn-block w-lg" runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Create My Account" OnClientClick="return showSpinner();" />
                                    <button
                                        id="btnLoading"
                                        type="button"
                                        class="btn btn-success btn-block m-b"
                                        disabled
                                        style="display: none;">
                                        <img src="/V1/Images/logo-icon-70x70-white-transparent.png" alt="Loading..." style="width: 24px; height: 24px; animation: spin 1s linear infinite;" />
                                    </button>
                                    <div class="text-muted text-center m-t-lg"><small>Already have an account?</small></div>
                                    <a class="btn btn-sm btn-block btn-info w-lg" href="/SignIn">Sign In</a>
                                </div>
                                <div class="col-sm-2"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xs-1 col-sm-2 col-md-3 col-lg-3"></div>
    </div>
    <!-- Meta Pixel Code -->
    <script>
        !function (f, b, e, v, n, t, s) {
            if (f.fbq) return; n = f.fbq = function () {
                n.callMethod ?
                    n.callMethod.apply(n, arguments) : n.queue.push(arguments)
            };
            if (!f._fbq) f._fbq = n; n.push = n; n.loaded = !0; n.version = '2.0';
            n.queue = []; t = b.createElement(e); t.async = !0;
            t.src = v; s = b.getElementsByTagName(e)[0];
            s.parentNode.insertBefore(t, s)
        }(window, document, 'script',
            'https://connect.facebook.net/en_US/fbevents.js');
        fbq('init', '836626721309055');
        fbq('track', 'PageView');
    </script>
    <noscript>
        <img height="1" width="1" style="display: none"
            src="https://www.facebook.com/tr?id=836626721309055&ev=PageView&noscript=1" />
    </noscript>
    <!-- End Meta Pixel Code -->
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
