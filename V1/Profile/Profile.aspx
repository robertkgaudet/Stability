<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="V1_Profile_Profile" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.btnProfilePhoto').click(function () {
				window.location.href = '/V1/Profile/ProfilePhotoUpload.aspx?userId=<%=userId%>';
				return false;
			})
		});
	</script>

	<style>
		#<%=profileImageStyle%>

		.chkboxlist td 
		{
			font-size:medium;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:HiddenField id="hidProfileId" runat="server"></asp:HiddenField>

<div class="content">
	<div class="row">
		<div class="col-xs-12">
			<div class="hpanel">
				<div class="panel-body">
					<h2 class="font-light m-b-xs">
						Member Profile
					</h2>
					<small>Welcome to your member profile page.</small>
				</div>
			</div>
		</div>
	</div>

	<div class="row">
		<div class="col-lg-4">
			<div class="hpanel hgreen" id="divEditScreen" runat="server" visible="false">
				<div class="panel-heading hbuilt">
					Update My Member Profile
				</div>
				<div class="panel-body">
					<dl class="dl-horizontal">
						<dt>
							<asp:HyperLink runat="server" id="hypDisasters" Text="Choose A Community" NavigateURL="EditDisasters.aspx"></asp:HyperLink>
						</dt>
						<dd>
							Choose the community portals you're working.
						</dd>
						<dt>
							<asp:HyperLink runat="server" id="HyperLink1" Text="Available Dates" NavigateURL="AvailableDates.aspx"></asp:HyperLink>
						</dt>
						<dd>
							What dates are you available to deploy and help?
						</dd>
						<dt>
							<asp:HyperLink runat="server" id="hypCauses" Text="Choose A Deployment" NavigateURL="EditNonProfitCauses.aspx"></asp:HyperLink>
						</dt>
						<dd>
							Choose a deployment you are on a team for.
						</dd>
						<dt>
							<asp:HyperLink runat="server" id="hypNonProfits" Text="Select Team" NavigateURL="EditNonProfits.aspx"></asp:HyperLink>
						</dt>
						<dd>
							Which team are you working with?
						</dd>
						<dt>
							<asp:HyperLink runat="server" id="hypProfile" Text="Edit Profile" NavigateURL="ProfileEdit.aspx"></asp:HyperLink>
						</dt>
						<dd>
							Update your personal profile information.
						</dd>
						<dt>
							<asp:HyperLink runat="server" id="hypSkills" Text="Edit My Skills" NavigateURL="EditSkills.aspx"></asp:HyperLink>
						</dt>
						<dd>
							By selecting your skills, you can be matched with deployments, events and teams that may need your help.
						</dd>
						<dt>
							<asp:HyperLink runat="server" id="hypResources" Text="Edit My Resources" NavigateURL="EditResources.aspx"></asp:HyperLink>
						</dt>
						<dd>
							List the resources you have that you are willing to use to help others in times of disater.
						</dd>
						<dt>
							<asp:HyperLink runat="server" id="hypIDCard" Text="Print ID Card" NavigateURL="/IDCard"></asp:HyperLink>
						</dt>
						<dd>
							Print an ID card to show when helping.
						</dd>
					</dl>
				</div>
			</div>
			<div class="hpanel hgreen" id="divEditProfile" runat="server" visible="false">
				<div class="panel-heading hbuilt">
					Update My Personal Information
				</div>
				<div class="panel-body">
					<dl class="dl-horizontal">
						<dt>
							<asp:HyperLink runat="server" id="HyperLink4" Text="Change Password" NavigateURL="EditPassword.aspx"></asp:HyperLink>
						</dt>
						<dd>
							Update your password.
						</dd>
						<dt>
							<asp:HyperLink runat="server" id="HyperLink3" Text="Change Email" NavigateURL="EditEmail.aspx"></asp:HyperLink>
						</dt>
						<dd>
							Modify your email address.
						</dd>
						<dt>
							<asp:HyperLink runat="server" id="HyperLink5" Text="Change Username" NavigateURL="Username.aspx"></asp:HyperLink>
						</dt>
						<dd>
							Select a new username.
						</dd>
					</dl>
				</div>
			</div>
			<div class="hpanel hgreen">
				<div class="panel-body">
					<img class="m-b" src="../Images/icons8-customer-64.png" runat="server" id="imgProfile" />
					<button id="btnAddProfileImage" class="btnProfilePhoto btn btn-info" runat="server"	>Upload Profile Image</button>
					<h3>
						<asp:Literal ID="litName" runat="server"></asp:Literal>
					</h3>
					<div class="text-muted font-bold m-b-xs">
						<asp:Literal ID="litVolunteerApplicationCompletedOn" runat="server"></asp:Literal>
					</div>
					<div class="text-muted font-bold m-b-xs">
						<asp:Literal ID="litVolunteerStatus" runat="server"></asp:Literal>
					</div>
					<dl class="dl-horizontal">
						<dt>
							Zello Handle
						</dt>
						<dd>
							<asp:Label id="lblZelloHandle" runat="server"></asp:Label>
						</dd>
						<dt>
							Title
						</dt>
						<dd>
							<asp:Label id="lblTitle" runat="server"></asp:Label>
						</dd>
						<dt>
							Address
						</dt>
						<dd id="ddAddress" runat="server" visible="false">
							<asp:Label id="lblAddress" runat="server"></asp:Label>
						</dd>
						<dd>
							<asp:Label id="lblCityStateZip" runat="server"></asp:Label>
						</dd>
						<dt runat="server" id="dtPhone" visible="false">
							Phone Number
						</dt>
						<dd runat="server" id="ddPhone" visible="false">
							<asp:Label id="lblPhoneNumber" runat="server"></asp:Label>
						</dd>
						<dt runat="server" id="dtEmail" visible="false">
							Email Address
						</dt>
						<dd runat="server" id="ddEmail" visible="false">
							<asp:Label id="lblEmailAddress" runat="server"></asp:Label>
						</dd>
						<dt runat="server" id="dt2">
							Username
						</dt>
						<dd runat="server" id="dd3">
							<asp:Label id="lblUsername" runat="server"></asp:Label>
						</dd>
					</dl>
				</div>
			</div>

		</div>
		<div class="col-lg-8">
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					My Current Member Profile Details. Information such as Communities, Skills, Availability and Team
				</div>
				<div class="panel-body">
					<dl class="dl-horizontal">
						<dt>
							Community Portal
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lblDisasters" runat="server"></asp:Label>
						</dd>
						<dt>
							My Team
						</dt>
						<dd class="m-b-sm">
							<asp:Literal id="litNonProfits" runat="server"></asp:Literal>
							<asp:LinkButton ID="btnAddToWebsite" CssClass="btn btn-success impactoidButton" runat="server" Text="Add to Website" OnClick="btnAddToWebsite_Click"></asp:LinkButton>
						</dd>
						<dt>
							My Skills
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lblSkills" runat="server"></asp:Label>
						</dd>
						<dt>
							My Resources
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lblResources" runat="server"></asp:Label>
						</dd>
						<dt>
							Dates Available
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lblDatesAvailable" runat="server"></asp:Label>
						</dd>
						<dt>
							Number of Days Available
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lblNumberOfDaysAvailable" runat="server"></asp:Label>
						</dd>
						<dt>
							Member Description
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lblVolunteerDescription" runat="server"></asp:Label>
						</dd>
					</dl>
				</div>
			</div>
			<div class="hpanel hgreen m-b-lg" runat="server" id="divAdminTools" visible="false">
				<div class="panel-heading">
					Administrative User Tools
				</div>
			
				<div class="panel-body m-b-lg" id="divResetPassword" runat="server" visible="false">
					<h3>Change Users Password</h3>

					<asp:TextBox ID="txtPassword" runat="server" CssClass="form-control"></asp:TextBox>
					<div>
						<asp:Button ID="btnChangePassword" CausesValidation="False" CssClass="btn btn-sm btn-info" runat="server" Text="Change Password" OnClick="btnChangePassword_Click" />
					</div>
				</div>
			
				<div class="panel-body m-b-lg" id="divUserAccountAdministration" runat="server" visible="false">
					<div class="checkbox">	
						<h3>User Access</h3>
						<input type="checkbox" runat="server" id="chkLockedOut" class="i-checks">
						<label class="chkboxlist" for="<%=chkLockedOut.ClientID%>">Is Locked Out</label>
					</div>
					<div class="checkbox">
						<h3>Choose Roles</h3>
						<asp:CheckboxList id="chkRoles" runat="server" DataValueField="RoleId" DataTextField="RoleName" CssClass="i-checks chkboxlist"></asp:CheckboxList>
					</div>
					<div>
						<asp:Button ID="bthSubmitLockout" CausesValidation="False" CssClass="btn btn-sm btn-info" runat="server" Text="Update Users Access" OnClick="bthSubmitRoles_Click" />
					</div>
				</div>

				<div class="panel-body" id="divVolunteerStatus" runat="server">
					<div class="text-muted font-bold m-b-xs">
						Update Member Status
					</div>
					<div class="alert alert-info" id="divVolunteerVettingMessage" visible="false" runat="server">
						<asp:Label ID="lblVolunteerReviewStatus" runat="server"></asp:Label>
					</div>
					<div class="m-t" role="form">
						<div class="form-group">
							<asp:RadioButtonList id="rblUserStatus" runat="server">
							</asp:RadioButtonList>
						</div>
						<div class="form-group">
							<asp:TextBox ID="txtVettingNotes" TextMode="MultiLine" runat="server" CssClass="form-control" placeholder="Enter Vetting Notes"></asp:TextBox>
						</div>
											<div class="form-group">
						<asp:Button ID="btnSumbit" CssClass="btn btn-lg btn-info" runat="server" Text="Update Member Status" OnClick="btnSumbit_Click" />
					</div>
<div class="form-group">
    <div class="col-sm-10">
        <div class="checkbox i-checks">
            <asp:CheckBox ID="chkShowDonateButton" runat="server" CssClass="custom-checkbox" />
            <label class="chkboxlist" for="<%= chkShowDonateButton.ClientID %>">Enable Team Logo</label>
        </div>
    </div>
</div>

<div class="form-group">
    <div class="col-sm-10">
        <div class="checkbox i-checks">
            <asp:CheckBox ID="chkStabilityVerified" runat="server" CssClass="custom-checkbox" />
            <label class="chkboxlist" for="<%= chkStabilityVerified.ClientID %>">Stability Verified</label>
        </div>
    </div>
</div>

<div class="form-group">
    <asp:Button ID="Button1" CssClass="btn btn-lg btn-info" runat="server" Text="Update" OnClick="btnUpdate_Click" />
</div>

						<%--//purple--%>


               </div>
				</div>
             </div>
				<div class="panel-body no-padding m-t-lg">
					<div class="chat-discussion" style="height: auto">
							<asp:Image ID="imgNewPostLogo" runat="server" CssClass="post-logo" />
							<div class="message m-b-sm">
								<span class="message-content">
									<asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtPost" runat="server"></asp:TextBox>
								</span>
								<div style="overflow:auto;">
									<asp:Button ID="btnRebuildPost" OnClick="btnAddProfileNote_Click" CssClass="btn w-xs btn-sm btn-primary pull-right m-t-sm" runat="server" Text="Post" />
								</div>
							</div>
						<asp:Repeater ID="rptPosts" runat="server">
							<ItemTemplate>
								<div class="hpanel">
										<div class="message">
											<div class="blog-article-box">
											<%# DataBinder.Eval(Container.DataItem, "fullname") %>
											</div>
											<span class="message-date"> <%# DataBinder.Eval(Container.DataItem, "createdon", "{0:M/d/yyyy HH:mm:ss}") %> </span>
											<span class="message-content">
											<%# DataBinder.Eval(Container.DataItem, "Note1") %>
											</span>
										</div>
								</div>
							</ItemTemplate>
						</asp:Repeater>
					</div>
				</div>
			</div>
		</div>
	</div>
</div>
</asp:Content>

