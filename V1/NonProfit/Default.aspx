<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="V1_NonProfit_Default" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script type="text/javascript">

		$(document).ready(function () {
			$('.donateButton').click(function () {
				window.location.href = '<%=donateLink%>';
				return false;
            });
			
			$('.activityPage').click(function () {
				window.location.href = '<%=activityPageLink%>';
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

			$('.squareLogoUploadButton').click(function () {
				window.location.href = '/V1/NonProfit/SquareLogoUpload.aspx?organizationId=<%=organizationId%>';
				return false;
			});

            $('.logoUploadButton').click(function () {
                window.location.href = '/V1/NonProfit/LogoUpload.aspx?organizationId=<%=organizationId%>';
                return false;
			});
            $('.donationDashboard').click(function () {
				window.location.href = '/V1/NonProfit/DonationDashboard.aspx?organizationId=<%=organizationId%>';
                 return false;
             });

            $('.volunteerButton').click(function () {
                window.location.href = '<%=volunteerLink%>';
                return false;
			});
			$('.editButton').click(function () {
				window.location.href = '<%=editLink%>';
				return false;
			}); 

			// Function for collapse hpanel
			$('.showhide').on('click', function (event) {
				event.preventDefault();
				var hpanel = $(this).closest('div.hpanel');
				var icon = $(this).find('i:first');
				var body = hpanel.find('div.panel-body');
				var footer = hpanel.find('div.panel-footer');
				body.slideToggle(300);
				footer.slideToggle(200);

				// Toggle icon from up to down
				icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
				hpanel.toggleClass('').toggleClass('panel-collapse');
				setTimeout(function () {
					hpanel.resize();
					hpanel.find('[id^=map-]').resize();
				}, 50);
			});

			// Function for close hpanel
			$('.closebox').on('click', function (event) {
				event.preventDefault();
				var hpanel = $(this).closest('div.hpanel');
				hpanel.remove();
				if ($('body').hasClass('fullscreen-panel-mode')) { $('body').removeClass('fullscreen-panel-mode'); }
			});

			// Fullscreen for fullscreen hpanel
			$('.fullscreen').on('click', function () {
				var hpanel = $(this).closest('div.hpanel');
				var icon = $(this).find('i:first');
				$('body').toggleClass('fullscreen-panel-mode');
				icon.toggleClass('fa-expand').toggleClass('fa-compress');
				hpanel.toggleClass('fullscreen');
				setTimeout(function () {
					$(window).trigger('resize');
				}, 100);
			});






		});	
</script>
	<style>
		.modal-dialog
		{
			margin-top:100px;
		}
		.btn
		{
			margin-bottom:5px;
		}
		.content
		{
			bottom:0px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />
					<div class="row">
						<div class="col-xs-12">
							<asp:LinkButton id="lbVolunteer" runat="server" CssClass="btn btn-success btn-large volunteerButton pull-right m-l-md" Text="Join This Team" visible="false"></asp:LinkButton>
							<asp:Button ID="btnActiveVolunteer" runat="server" CssClass="btn btn-light btn-large pull-right m-l-md" Visible="false" />
							<asp:LinkButton id="lbDonate" runat="server" CssClass="btn btn-success pull-right donateButton" Text="Donate" visible="false"></asp:LinkButton>
						</div>
					</div>
	
					
					<div class="panel-body">
						<div class="alert alert-success" runat="server" id="divAlertPageMessage" visible="false">
							<i class="fa fa-bolt"></i> This team page is de-activated.
						</div>
						<p>
							<b>Founded</b> <asp:Literal id="litYearFounded" runat="server"></asp:Literal>
						</p>
						<b>Description</b>
						<p>
							<asp:Literal id="litDescription" runat="server"></asp:Literal>
						</p>
						<b>Mission/Purpose</b>
						<p>
							<asp:Literal id="litMission" runat="server"></asp:Literal>
						</p>
						
						<div class="row">
							<div class="col-lg-6">
								<div class="hpanel hgreen">
									<div class="panel-heading hbuilt">
										Details
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
							</div>
							<div class="col-lg-6">
								<div class="hpanel hgreen">
									<div class="panel-heading hbuilt">
										Social Media
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
								</div>
							</div>
							<div class="col-lg-6">
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
								</div>
							</div>
						</div>
					</div>
				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />
</asp:Content>