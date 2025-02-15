<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Narrow.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="V1_Register" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/1-Column-Narrow.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Register on Stability - Disaster-Ready Communities</title>
    <link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
    <link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
    <link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
    <script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>

    <script>

        $(document).ready(function () {
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

    </script>
    <style>
        .logo-name:hover {
            cursor: pointer;
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
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-xs-1 col-sm-2 col-md-3 col-lg-3"></div>
        <div class="col-xs-10 col-sm-8 col-sm-6 col-lg-6" style="min-width: 450px !important; max-width: 500px !important;">
            <div class="middle-box text-center loginscreen animated fadeInDown">
                <h1 class="loginLogo">
                    <img class="img-responsive" src="/V1/Images/Logo-Horizontal-cs.png" />
                </h1>
                <div class="m-t-sm text-center">
                    <h4>Disaster-Ready Communities
                    </h4>
                </div>
                <p>
                    Be the lifeline your community needs.
                </p>
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
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" required="" placeholder="Address"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>City  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" required="" placeholder="City"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label for="ddlState">State <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <div class="dropdown-wrapper">
                                    <select id="ddlState" runat="server" class="custom-dropdown">
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
                                <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" required="" placeholder="Zip Code"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>Email Address  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox type="email" ID="txtEmail" runat="server" CssClass="form-control" required="" placeholder="Email"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>Phone Number  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox ID="txtPhoneNumber" onkeypress="return isNumberKey(event)" MaxLength="10" TextMode="Phone" runat="server" CssClass="form-control" required="" placeholder="Phone Number"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>Username  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required="" placeholder="Username"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12">
                                <label>Password  <span class="text-danger" style="font-size: 2rem; line-height: 1;">*</span></label>
                                <asp:TextBox type="password" ID="txtPassword" runat="server" CssClass="form-control" required="" placeholder="Password"></asp:TextBox>
                            </div>
                            <div class="form-group col-lg-12" style="display: flex; align-items: center;">
                                <asp:CheckBox ID="chkMessageOptIn" runat="server" Text="" />
                                <label for="chkMessageOptIn" style="margin-left: 10px;">Receive SMS Messages For Volunteer Opportunities and Deployments</label>
                            </div>
                        </div>
                        <div class="text-center center-block justify-content-center">
                            <div class="row">
                                <div class="col-sm-2"></div>
                                <div class="col-sm-8">
                                    <asp:Button CssClass="btn btn-success btn-block w-lg" runat="server" ID="btnSubmit" OnClick="btnSubmit_Click" Text="Create My Account" />
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
</asp:Content>
