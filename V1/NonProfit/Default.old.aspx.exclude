<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="V1_NonProfit_Default" %>
<%@ Register Src="~/V1/UserControls/TeamNavigation.ascx" TagPrefix="uc1" TagName="TeamNavigation" %>
<%@ Register Src="~/V1/UserControls/TeamHeader.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
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

            $('.volunteerButton').click(function () {
                window.location.href = '<%=volunteerLink%>';
                return false;
			});
			$('.editButton').click(function () {
				window.location.href = '<%=editLink%>';
				return false;
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

	<div class="content">
        <div class="row">
            <div class="col-md-3">
				<uc1:TeamNavigation runat="server" ID="ucTeamNavigation" />
            </div>
            <div class="col-md-9">
				<!--PAGE HEADER-->
                <div class="hpanel ">
                    <div class="panel-heading hbuilt">
						<div class="pull-right">
							<asp:LinkButton id="lbVolunteer" runat="server" CssClass="btn btn-success volunteerButton" Text="Join This Team" visible="false"></asp:LinkButton>
							<asp:LinkButton id="lbDonate" runat="server" CssClass="btn btn-success donateButton" Text="Donate" visible="false"></asp:LinkButton>
							<asp:Button ID="btnActiveVolunteer" runat="server" CssClass="btn btn-light" Visible="false" />
						</div>
                        <div class="font-normal">
							<h1 class="m-b-none"><i class="fa fa-th-large"></i> <asp:Literal ID="litTeamName" runat="server"></asp:Literal></h1>
							<small class="text-muted">Team Details</small>
                        </div>
                    </div>
					<!--PAGE CONTENT-->
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
					<!--PAGE FOOTER-->
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
								<asp:Button id="btnInviteTeamMembers" runat="server" Visible="false" CssClass="btn btn-success inviteButton" Text="Invite Team Members" />
								<asp:Button id="btnEditMyGroup" runat="server" Visible="false" CssClass="btn btn-warning editButton" Text="Update Team Information" />
								<asp:Button id="btnUploadLogo" runat="server" Visible="false" CssClass="btn btn-primary logoUploadButton" Text="Upload Logo" />
								<asp:Button id="btnUploadSquare" runat="server" Visible="false" CssClass="btn btn-primary squareLogoUploadButton" Text="Upload A Square Logo" />
								<asp:Button id="btnUploadCoverImage" runat="server" Visible="false" CssClass="btn btn-primary coverUploadButton" Text="Upload Cover Image" />
								<asp:Button id="btnManagePhotos" runat="server" Visible="false" CssClass="btn btn-primary managePhotosButton" Text="Manage Photos" />
								<asp:Button id="btnDeactivatePage" OnClick="btnChangePageStatus_Click" runat="server" CssClass="btn btn-primary" Text="De-activate This Team" />
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
</asp:Content>

