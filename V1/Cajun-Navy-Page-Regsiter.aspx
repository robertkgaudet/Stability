<%@ Page Title="" Language="C#" MasterPageFile="~/S1/MasterPages/HomerNoNavigation.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Cajun-Navy-Page-Regsiter.aspx.cs" Inherits="V1_Cajun_Navy_Page_Register" %>
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
		});

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="middle-box text-center loginscreen animated fadeInDown m-t-lg">
        <div>
            <div class="col-sm-4"></div>
            <div class="col-sm-4">
				<img class="img-responsive" src="/V1/Images/Logo-Horizontal.png" />
            </div>
            <div class="col-sm-4"></div>
		</div>
	</div>
	<div class="register-container animated fadeInDown">
    <div class="row">
        <div class="col-sm-5">
            <h3>10 Reasons to Create Your Own Cajun Navy Group Today!</h3>
            <p></p>

            <dl>
                <dt>1. Be Proactive, Not Reactive</dt>

                <dt>2. Strengthen Community Bonds</dt>

                <dt>3. Local Knowledge is Crucial</dt>

                <dt>4. Quick Response Saves Lives</dt>

                <dt>5. Build a Culture of Preparedness</dt>

                <dt>6. Empower Yourself and Others</dt>

                <dt>7. Reduce Dependency on External Aid</dt>

                <dt>8. Utilize Community Resources</dt>

                <dt>9. Inspire Future Generations</dt>

                <dt>10. Make a Tangible Difference</dt>
            </dl>
        </div>
        <div class="col-md-7">
			<h3>Create Your Cajun Navy Group Today.<br />
                In Louisiana, we don't wait for help, we become the help; register, organize and act now!</h3>
                <b>Strengthen Community Bonds</b> - Organizing disaster response efforts fosters stronger relationships among community members. Together, we are stronger and more resilient.
			<p><asp:Label ID="lblMessage" runat="server" text=""></asp:Label></p>
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
                        <ul style="font-size:14px;">
                            <li>First create a Personal Profile to start to help rebuild lives after natural disaster.</li>
                            <li>Next create a Cajun Navy group that you can invite friends and family to join.</li>
                        </ul>
                        <form action="#" id="loginForm">
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
									<label>Address</label>
									<asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" required="" placeholder="Address"></asp:TextBox>
								</div>
								<div class="form-group col-lg-12">
									<label>City</label>
									<asp:TextBox ID="txtCity" runat="server" CssClass="form-control" required="" placeholder="City"></asp:TextBox>
								</div>
								<div class="form-group col-lg-12">
									<label>State</label>
                                    <select id="ddlState" runat="server">
                                        <option value="">Select a state</option>
                                        <option value="AL">Alabama</option>
                                        <option value="AK">Alaska</option>
                                        <option value="AZ">Arizona</option>
                                        <option value="AR">Arkansas</option>
                                        <option value="CA">California</option>
                                        <option value="CO">Colorado</option>
                                        <option value="CT">Connecticut</option>
                                        <option value="DE">Delaware</option>
                                         <option value="DC">D.C.</option>
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
								<div class="form-group col-lg-12">
									<label>Zip Code</label>
									<asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" required="" placeholder="Zip Code"></asp:TextBox>
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
								<div class="form-group col-lg-12">
									<label>Password</label>
									<asp:TextBox type="password" ID="txtPassword" runat="server" CssClass="form-control" required="" placeholder="Password"></asp:TextBox>
								</div>
                            </div>
                            <div class="text-center">
                                <asp:Button CssClass="btn btn-success w-lg" runat="server" id="btnSubmit" OnClick="btnSubmit_Click" Text="Create my Cajun Navy Group"/>
								<div class="text-muted text-center"><small>Already have an account?</small></div>
								<a class="btn btn-sm btn-default w-lg" href="/SignIn">Login</a>
                            </div>
                        </form>
						<div class="row m-b-lg m-t-lg">
							<div class="col-md-12 text-center">

								Stability was created to help those affected by disasters recovery more quickly.
								<br />
								<br/> 2024 Copyright Stability
							</div>
						</div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>