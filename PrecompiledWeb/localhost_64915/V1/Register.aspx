<%@ page title="" language="C#" masterpagefile="~/S1/MasterPages/HomerNoNavigation.master" enableeventvalidation="false" autoeventwireup="true" inherits="V1_Register, App_Web_mjkl5wor" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/HomerNoNavigation.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<title>Sign Up for Stability</title>
    <link rel="stylesheet" href="/Homer/vendor/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-3.5.2/select2.css" />
	<link rel="stylesheet" href="/Homer/vendor/select2-bootstrap/select2-bootstrap.css" />
	<script src="/Homer/vendor/select2-3.5.2/select2.min.js"></script>
	
		<script>

		$(document).ready(function ()
		{
<%--			<%=preselectedDisasterJQuery%>

			$("#disasterEvent.dropdown-menu li").click(function ()
			{
				$("#btn-dropdown.disasterEvent").html($(this).text());
				$("#<%=hidEventId.ClientID%>").val($(this).attr('id'));
            });--%>

			<%=preselectedNonProfitJQuery%>

			$("#nonProfit.dropdown-menu li").click(function ()
			{
                $("#btn-NonProfitDropdown.nonProfit").html($(this).text());
                $("#<%=hidOrganizationId.ClientID%>").val($(this).attr('id'));
			});

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
			<h3>Register to create or join a Disaster Relief Team</h3>
			<b>When those most in need are struggling after a natural disaster, there is no time to waste.</b>
			<br />Instantly turn your network into a disaster relief team.
			<br />Churches, civic organizations, families, any kind of team now has the tools to start helping right away.
			<hr />
			<p><asp:Label ID="lblMessage" runat="server" text="Create your profile here to begin work with your team to rebuild your community."></asp:Label></p>
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
						<%--<div class="form-group col-lg-12">
							<label>Choose One</label>
							<br />Are you a survivor or a helper/volunteer?
							<div class="radio m-l-sm radio-primary">
								<asp:RadioButton type="radio" id="rdMemberTypeHelper" runat="server" value="helper" Font-Size="Larger" Text="Volunteer" GroupName="memberType" Checked="true" />
							</div>
							<div class="radio m-l-sm radio-primary">
								<asp:RadioButton type="radio" id="rdMemberTypeCaseManager" runat="server" value="survivor" Font-Size="Larger" Text="Case Manager" GroupName="memberType" />
							</div>
							<div class="radio m-l-sm radio-primary">
								<asp:RadioButton type="radio" id="rdMemberTypeSurvivor" runat="server" value="survivor" Font-Size="Larger" Text="Survivor" GroupName="memberType" />
							</div>
							<div class="radio m-l-sm radio-primary">
								<asp:RadioButton type="radio" id="rdMemberTypeNonProfit" runat="server" value="nonprofit" Font-Size="Larger" Text="NonProfit" GroupName="memberType" />
							</div>
							<div class="radio m-l-sm radio-primary">
								<asp:RadioButton type="radio" id="rdMemberTypeBusiness" runat="server" value="business" Font-Size="Larger" Text="Business" GroupName="memberType" />
							</div>
						</div>--%>
<%--						<div class="form-group col-lg-12">
							<label>Choose Your Community Portal</label>
							<div class="text-muted">If you do not see your community listed, <a href="mailto:support@stability.org" class="text-uppercase">email support@stability.org</a> and ask us to add it.</div>
							<div id="div1" class="dropdown m-b-md" runat="server">
								<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Select (Optional) <i class="fa fa-sort-down"></i></button>
								<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
									<%=disasterDropDown%>
								</ul>
							</div>
							<input type="hidden" id="hidEventId" runat="server" />
						</div>--%>
						<div class="form-group col-lg-12" runat="server" id="divChooseNonprofit">
							<label></label>
							<div id="div2" class="dropdown m-b-md" runat="server">
								<button id="btn-NonProfitDropdown" class="btn btn-outline btn-default nonProfit dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Select A Team (Optional) <i class="fa fa-sort-down"></i></button>
								<ul id="nonProfit" class="dropdown-menu text-center dropdown-volunteer required">
									<%=nonProfitDropDown%>
								</ul> Leave empty to create your own team.
							</div>
							<input type="hidden" id="hidOrganizationId" runat="server" />
						</div>
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
                        <asp:Button CssClass="btn btn-success w-lg" runat="server" id="btnSubmit" OnClick="btnSubmit_Click" Text="Register"/>
						<div class="text-muted text-center"><small>Already have an account?</small></div>
						<a class="btn btn-sm btn-default w-lg" href="/SignIn">Login</a>
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