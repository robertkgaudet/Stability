<%@ page title="" language="C#" masterpagefile="~/S1/MasterPages/Homer.master" autoeventwireup="true" inherits="S1_HelperProfile, App_Web_avkw21jy" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/Homer.master"%>

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
<div class="normalheader ">
    <div class="hpanel">
        <div class="panel-body">
            <a class="small-header-action" href="">
                <div class="clip-header">
                    <i class="fa fa-arrow-up"></i>
                </div>
            </a>

            <div id="hbreadcrumb" class="pull-right m-t-lg">
                <ol class="hbreadcrumb breadcrumb">
                    <li><a href="index.html">Dashboard</a></li>
                    <li>
                        <span>App views</span>
                    </li>
                    <li class="active">
                        <span>Survivor Profile </span>
                    </li>
                </ol>
            </div>
            <h2 class="font-light m-b-xs">
                Survivor Profile
            </h2>
            <small>Welcome to your survivor profile page.</small>
        </div>
    </div>
</div>

<div class="content">

<div class="row">
    <div class="col-lg-4">
        <div class="hpanel hgreen" id="divEditScreen" runat="server" visible="false">
			<div class="panel-heading hbuilt">
				Update My Survivor Profile
			</div>
            <div class="panel-body">
				<dl class="dl-horizontal">
					<dt>
						<asp:HyperLink runat="server" id="hypProfile" Text="Edit Profile" NavigateURL="~/V1/Profile/ProfileEdit.aspx"></asp:HyperLink>
					</dt>
					<dd>
						Update your personal profile information.
					</dd>
					<dt>
						<asp:HyperLink runat="server" id="hypNonProfits" Text="Select Non-Profits" NavigateURL="~/V1/Profile/EditNonProfits.aspx"></asp:HyperLink>
					</dt>
					<dd>
						Which non-profits are you working with?
					</dd>
					<dt>
						<asp:HyperLink runat="server" id="hypDisasters" Text="Choose My Disasters" NavigateURL="~/V1/Profile/EditDisasters.aspx"></asp:HyperLink>
					</dt>
					<dd>
						Choose the disaster you are working.
					</dd>
					<dt>
						<asp:HyperLink runat="server" id="hypSkills" Text="Edit My Skills" NavigateURL="~/V1/Profile/EditSkills.aspx"></asp:HyperLink>
					</dt>
					<dd>
						By selecting your skills, you will be matched with non-profits who need your help.
					</dd>
					<dt>
						<asp:HyperLink runat="server" id="hypIDCard" Text="Print ID Card" NavigateURL="/IDCard"></asp:HyperLink>
					</dt>
					<dd>
						Print an ID card to show when requesting services.
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
						<asp:HyperLink runat="server" id="HyperLink4" Text="Change Password" NavigateURL="~/V1/Profile/EditPassword.aspx"></asp:HyperLink>
					</dt>
					<dd>
						Update your password.
					</dd>
					<dt>
						<asp:HyperLink runat="server" id="HyperLink3" Text="Change Email" NavigateURL="~/V1/Profile/EditEmail.aspx"></asp:HyperLink>
					</dt>
					<dd>
						Modify your email address.
					</dd>
					<dt>
						<asp:HyperLink runat="server" id="HyperLink5" Text="Change Username" NavigateURL="~/V1/Profile/Username.aspx"></asp:HyperLink>
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
				<asp:HyperLink ID="hypAmazon" CssClass="btn-success btn m-t-lg m-b-lg btn-block" runat="server"></asp:HyperLink>
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
				</dl>
            </div>
        </div>

    </div>
    <div class="col-lg-8">
        <div class="hpanel hgreen">
            <div class="panel-body">
				<dl class="dl-horizontal">
					<dt>
						My Disasters
					</dt>
					<dd class="m-b-sm">
						<asp:Label id="lblDisasters" runat="server"></asp:Label>
					</dd>
					<dt>
						My Non-Profits
					</dt>
					<dd class="m-b-sm">
						<asp:Literal id="litNonProfits" runat="server"></asp:Literal>
					</dd>
					<dt>
						My Skills
					</dt>
					<dd class="m-b-sm">
						<asp:Label id="lblSkills" runat="server"></asp:Label>
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
						Volunteer Description
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
						<asp:Button ID="bthSubmitLockout" CausesValidation="False" CssClass="btn btn-lg btn-info" runat="server" Text="Update Users Access" OnClick="bthSubmitRoles_Click" />
					</div>
			</div>

            <div class="panel-body" id="divVolunteerStatus" runat="server">
                <div class="text-muted font-bold m-b-xs">
					Update Volunteer Status
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
						<asp:Button ID="btnSumbit" CssClass="btn btn-lg btn-info" runat="server" Text="Update Volunteer/Helper Status" OnClick="btnSumbit_Click" />
					</div>
				</div>
			</div>
		</div>
    </div>
</div>

</div>
</asp:Content>

