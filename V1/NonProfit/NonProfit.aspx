<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="NonProfit.aspx.cs" Inherits="V1_NonProfit_NonProfit" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">	<script type="text/javascript">

		$(document).ready(function () {
			$('.donateButton').click(function () {
				window.location.href = '<%=donateLink%>';
				return false;
            });
			
			$('.activityPage').click(function () {
				window.location.href = '<%=activityPageLink%>';
				return false;
			});
			$('.viewTeamMembers').click(function () {
				window.location.href = '<%=teamMembersLink%>';
				return false;
			});

            $('.coverUploadButton').click(function () {
                window.location.href = '/V1/NonProfitAdministration/CoverImage1600x600.aspx?organizationId=<%=organizationId%>';
                return false;
            }); 

            $('.inviteButton').click(function () {
                window.location.href = '/V1/NonProfitAdministration/InviteTeam.aspx?organizationId=<%=organizationId%>';
                return false;
            }); 

            $('.managePhotosButton').click(function () {
                window.location.href = '/V1/NonProfitAdministration/ManagePhotos.aspx?organizationId=<%=organizationId%>';
                return false;
            }); 

            $('.logoUploadButton').click(function () {
                window.location.href = '/V1/NonProfit/LogoUpload.aspx?organizationId=<%=organizationId%>';
                return false;
            });

            $('.volunteerButton').click(function () {
                window.location.href = '<%=volunteerLink%>';
                return false;
			});

			$("#nonProfit.dropdown-menu li").click(function () {
				window.location.href = "/V1/NonProfit/Default.aspx?organizationId=" + $(this).attr('id');
			});
		});	
</script>
	<style>
		.website
		{
			background-color:#5E2E91;
			padding:17px;
			color:white;
		}
		.website:hover
		{
			background-color:#902F91;
			color:white;
			cursor:pointer;
		}
		.modal-dialog
		{
			margin-top:100px;
		}
		.impactoidButton:hover{
			background-color:#902F91;
			color:white;
			cursor:pointer;
		}
		.impactoidButton{
			background-color:#5E2E91;
			color:white;
		}
		.btn
		{
			margin-bottom:5px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="content">
	<div class="row">
		<div class="form-group col-lg-12">
			<button id="btn-NonProfitDropdown" class="btn btn-outline btn-default nonProfit dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Select Another Team Profile Page <i class="fa fa-sort-down"></i></button>
			<ul id="nonProfit" class="dropdown-menu text-center required">
				<%=nonProfitDropDown%>
			</ul>
		</div>
	</div>
	<div class="row">
		<div class="col-sm-12">
			<div class="hpanel">
				<div class="alert alert-success" runat="server" id="divAlertPageMessage" visible="false">
					<i class="fa fa-bolt"></i> This team page is de-activated.
				</div>
			<%--	<div id="divLogo" runat="server" class="col-sm-12 bg-gray text-left m-t-n-lg" style="z-index:100;">
				</div>--%>
				<div class="panel-body">
					<div class="pull-left m-r-md m-b-sm" style="background-color:white; display:inline-block; padding:10px; border:solid 2px #ccc;">
						<asp:HyperLink ID="hypLogo" runat="server">
							<asp:Image ID="imgLogo" runat="server" />
						</asp:HyperLink>
					</div>
					<h2 class="m-b-xs m-t-xs">
						<asp:Literal id="litOrganizationName" runat="server"></asp:Literal>
					</h2>
					<p>
						Founded <asp:Literal id="litYearFounded" runat="server"></asp:Literal>
					</p>
					<b>Description</b>
					<p>
						<asp:Literal id="litDescription" runat="server"></asp:Literal>
					</p>
					<b>Mission/Purpose</b>
					<p>
						<asp:Literal id="litMission" runat="server"></asp:Literal>
					</p>
						<div class="col-md-2 pull-right" id="divSubscribeToWebsite" runat="server" visible="false">
							<div class="text-center website" data-toggle="modal" data-target="#subscribeModal">
								<i class="pe-7s-global fa-3x"></i>
								<h4 class="m-xs">Open Stability Team Website</h4>
								<small>Share your Stability webpage, grow your team.</small>
							</div>
							<div class="modal fade" id="subscribeModal" tabindex="-1" role="dialog" aria-hidden="true">
								<div class="modal-dialog">
									<div class="modal-content">
										<div class="color-line"></div>
										<div class="modal-header text-center">
											<h4 class="modal-title">Activate Your Stability Website</h4>
											<small class="font-bold">Subscribe now to activate your public Stability website.</small>
										</div>
										<div class="modal-body">
											<p>
												<asp:HyperLink ID="hypDemoWebsite" runat="server" Text="View Your Website - Example Only Opens in New Window" Target="_blank"></asp:HyperLink>
												<br />
												<strong>Enter your payment information here to subscribe and activate your organizations website.</strong>
												With a website, you can share your team information on your social media pages and with members you want to join you.
											</p>
										</div>
										<div class="modal-footer">
											<button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
											<button type="button" class="btn btn-primary">Save changes</button>
										</div>
									</div>
								</div>
							</div>
						</div>
					<p>
						<asp:LinkButton id="lbVolunteer" runat="server" CssClass="btn btn-success volunteerButton" Text="Join This Team" visible="false"></asp:LinkButton>
						<asp:LinkButton id="lbDonate" runat="server" CssClass="btn btn-success donateButton" Text="Donate" visible="false"></asp:LinkButton>
						<asp:Button ID="btnActiveVolunteer" runat="server" CssClass="btn btn-light" Visible="false" />
						<hr />
						<asp:HyperLink id="hyp" runat="server" CssClass="btn btn-success activityPage" Text="<i class='pe-7s-paint'></i> Open Team Activity Page"></asp:HyperLink>
						<asp:HyperLink id="lbImpactoidWebsite" Target="_blank" runat="server" CssClass="btn impactoidButton" Text="<i class='pe-7s-global'></i> Open Stability Team Website" visible="false"></asp:HyperLink>
					
					</p>
				</div>
				<div class="alert alert-success" runat="server" id="divAlertMessage" visible="false">
					<i class="fa fa-bolt"></i> <asp:Literal id="litAlertMessage" runat="server"></asp:Literal>
				</div>
			</div>
		</div>
	</div>
</div>
<div class="content" runat="server" visible="false" id="divUploadLogoCover">
    <div class="row">
        <div class="col-lg-12">
            <div class="hpanel hblue">
                <div class="panel-heading hbuilt">
                    Your Page Administation Tools
                </div>
                <div class="panel-body">
					<div class="form-group">
						<div class="pull-left">
							<asp:Button id="btnViewTeamMembers" runat="server" Visible="false" CssClass="btn btn-success viewTeamMembers" Text="View Team Members" />
							<asp:Button id="btnInviteTeamMembers" runat="server" Visible="false" CssClass="btn btn-success inviteButton" Text="Invite Team Members" />
							<asp:Button id="btnEditMyGroup" runat="server" Visible="false" CssClass="btn btn-primary editButton" Text="Edit My Group" />
							<asp:Button id="btnUploadLogo" runat="server" Visible="false" CssClass="btn btn-primary logoUploadButton" Text="Upload Logo" />
							<asp:Button id="btnUploadCoverImage" runat="server" Visible="false" CssClass="btn btn-primary coverUploadButton" Text="Upload Cover Image" />
							<asp:Button id="btnManagePhotos" runat="server" Visible="false" CssClass="btn btn-primary managePhotosButton" Text="Manage Photos" />
							<asp:Button id="btnDeactivatePage" runat="server" CssClass="btn btn-primary" Text="De-activate This Team" />
                            <br />List of members who have been invited.
						</div>
					</div>
                </div>
                <div class="panel-footer">
                    <i class="fa fa-lock"></i> These tools are only visible to the page administator.
                </div>
            </div>
        </div>
    </div>
</div>
<div class="content">
	<div class="row">
		<div class="col-lg-3">
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					NonProfit Details
				</div>
				<div class="panel-body">
					<dl class="dl-verticle">
						<dd>
							<asp:Label id="lblOrgName" runat="server"></asp:Label>
						</dd>
						<dd id="ddAddress" runat="server" class="m-b-sm">
							<asp:HyperLink id="hypAddress" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dd id="ddPublicPhone" runat="server" visible="false">
							<asp:HyperLink id="hyoPublicPhoneNumber" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dd id="ddPublicEmail" runat="server" visible="false">
							<asp:HyperLink id="hypPublicEmailAddress" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dd id="ddBlog" runat="server" visible="false">
							<asp:HyperLink id="hypBlog" runat="server" target="new"></asp:HyperLink>
						</dd>	
						<dd id="ddWebsite" runat="server" visible="false">
							<asp:HyperLink id="hypWebsite" runat="server" target="new"></asp:HyperLink>
						</dd>	
					</dl>
					<dl class="dl-horizontal">
						<dt>
							VOAD Member
						</dt>
						<dd>
							<asp:Label id="lblVoadMember" runat="server"></asp:Label>
						</dd>
						<dt>
							501c3
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lbl501c3" runat="server"></asp:Label>
						</dd>
					</dl>
				</div>
			</div>
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Social Media Links
				</div>
				<div class="panel-body">
					<dl class="dl-vertical">
						<dt>
							Facebook Page
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypFacebookPage" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							Facebook Group
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypFacebookGroup" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							Instagram
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypInstagram" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							YouTube Channel
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypYouTube" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt>
							Twitter
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypTwitter" runat="server" target="new"></asp:HyperLink>
						</dd>
					</dl>
				</div>
                <div class="panel-footer">
                </div>
			</div>
			<div class="hpanel hred" id="divUnpublishedInformation" runat="server" visible="false">
				<div class="panel-heading hbuilt">
					Unpublished Contact Information (Admin Only)
				</div>
				<div class="panel-body">
					<dl class="dl-verticle">

						<dt>
							Point of Contact (Unpublished)
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lblPointOfContactPerson" runat="server"></asp:Label>
						</dd>

						<dt>
							Point of Contact Phone (Unpublished)
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypPointOfContactPhone" runat="server"></asp:HyperLink>
						</dd>
						
						<dt>
							Point of Contact Email (Unpublished)
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypPointOfContactEmail" runat="server"></asp:HyperLink>
						</dd>

						<dt id="dtPrimaryPhone" runat="server">
							Primary Phone (Unpublished)
						</dt>
						<dd id="ddPrimaryPhone" runat="server" class="m-b-sm">
							<asp:HyperLink id="hypPrimaryPhone" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt id="dtSecondaryPhone" runat="server">
							Secondary Phone (Unpublished)
						</dt>
						<dd id="ddSecondaryPhone" runat="server" visible="false" class="m-b-sm">
							<asp:HyperLink id="hypSecondaryPhone" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dt runat="server" id="dtEIN" visible="false">
							EIN
						</dt>
						<dd runat="server" id="ddEIN" visible="false" class="m-b-sm">
							<asp:Label id="lblEIN" runat="server"></asp:Label>
						</dd>
					</dl>
				</div>
                <div class="panel-footer">
                </div>
			</div>
		</div>
		<div class="col-lg-5">
            <div class="hpanel hgreen">
                <div class="panel-heading hbuilt">
                    Active Deployments
                </div>
                <div class="panel-body">
                    <small>Choose a deployment to join or donate.</small>
					<asp:Repeater ID="rptActiveCampaigns" runat="server" OnItemDataBound="rptActiveCampaigns_ItemDataBound">
						<ItemTemplate>
						    <dl class="dl-vertical">
                                <dt>
                                    <asp:HyperLink ID="hypCauseName" runat="server"></asp:HyperLink> <span class="font-light">(<asp:HyperLink ID="hypDisasterName" runat="server"></asp:HyperLink>)</span>
                                </dt>
                                <dd>
                                    <asp:label ID="lblCauseMission" runat="server"></asp:label>
                                    <asp:label CssClass="text-danger" Text="Cause is Set To Inactive" id="lblInActiveFlag" runat="server" Visible="false"></asp:label>
                                    <asp:HyperLink CssClass="text-success" Text="Edit Deployment" id="hypEdit" runat="server" Visible="false"></asp:HyperLink>
                                </dd>
						    </dl>
						</ItemTemplate>
					</asp:Repeater>
				    <asp:HyperLink id="hypCause" runat="server" Text="Add a Deployment" Visible="false"></asp:HyperLink>
                </div>
                <div class="panel-footer">
                </div>
            </div>
		</div>
		<div class="col-lg-4">
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Team
				</div>
				<div class="panel-body">
                    <small>Team members recently active for this organization.  
                        <asp:HyperLink runat="server" ID="hypViewAllVolunteers" Text="View All" Visible="false"></asp:HyperLink>
                    </small>
					<asp:Repeater ID="rpNonProfitPeople" runat="server" OnItemDataBound="rpNonProfitPeople_ItemDataBound">
						<ItemTemplate>
						    <asp:Literal id="lblInfo" runat="server"></asp:Literal>
						</ItemTemplate>
					</asp:Repeater>
				</div>
                <div class="panel-footer">
                </div>
			</div>
		</div>
	</div>
</div>
</asp:Content>

