<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="BusinessCampaign.aspx.cs" Inherits="V1_Business_BusinessCampaign" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.img-rounded{max-width:100px;}
	</style>
	<script type="text/javascript">

		$(document).ready(function () {

			$('.donateButton').click(function () {
				window.location.href = '<%=donateLink%>';
				return false;
			});

			$('.volunteerButton').click(function () {
				window.location.href = '<%=volunteerLink%>';
				return false;
			});

			$('.editCampaign').click(function () {
				window.location.href = '<%=editCampaignLink%>';
				return false;
			});

			$('.btnUploadLogo').click(function () {
				window.location.href = '<%=uploadLogoLink%>';
				return false;
			}); 
		});
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="normalheader ">
	<div class="hpanel">
		<div class="panel-body">
			<div class="row">
				<div class="col-xs-12 m-b-md">
					<img class="m-b img-rounded pull-left m-r-md" src="../Images/icons8-customer-64.png" runat="server" id="imgBusinessProfile" />
					<%=icon%> <asp:Literal ID="litEventName" runat="server"></asp:Literal>
					<h2 class="font-light m-b-xs">
						<asp:Literal id="litCampaignName" runat="server"></asp:Literal>
					</h2>
				</div>
			</div>
			<div class="row">
				<div class="col-xs-5">
					<b>Campaign Mission</b>
					<p>
						<asp:Literal id="litCampaignMission" runat="server"></asp:Literal>
					</p>
				</div>
				<div class="col-xs-7">
					<dl class="dl-horizontal">
						<dt>
							Point of Contact
						</dt>
						<dd class="m-b-sm">
							<asp:Label id="lblPointOfContactPerson" runat="server"></asp:Label>
						</dd>
						
						<dt id="dtZelloChannel" runat="server" visible="false">
							Zello Channel
						</dt>
						<dd id="ddZelloChannel" runat="server" class="m-b-sm" visible="false">
							<asp:Label id="lblZelloChannel" runat="server"></asp:Label>
						</dd>

						<dt id="dtPhone" runat="server">
							Phone Number
						</dt>
						<dd id="ddPhone" runat="server" class="m-b-sm">
							<asp:HyperLink id="hypPointOfContactPhone" runat="server"></asp:HyperLink>
						</dd>
						
						<dt id="dtEmail" runat="server">
							Email
						</dt>
						<dd id="ddEmail" runat="server" class="m-b-sm">
							<asp:HyperLink id="hypPointOfContactEmail" runat="server"></asp:HyperLink>
						</dd>

						<dt id="dtPrimaryPhone" runat="server">
							Phone
						</dt>
						<dd id="ddPrimaryPhone" runat="server" class="m-b-sm">
							<asp:HyperLink id="hypPrimaryPhone" runat="server" target="new"></asp:HyperLink>
						</dd>

					</dl>
				</div>
			</div>
			<div class="row">
				<div class="col-6-xs m-l-md">
					<div class="m-b-sm">
						<asp:LinkButton id="lbDonate" runat="server" CssClass="btn btn-success donateButton" Text="Donate" visible="false"></asp:LinkButton>
					</div>
					<div class="pull-left font-light m-r-md">
						<i>
							Campaign created by: <asp:HyperLink id="hypBusinessName" runat="server" target="_blank"></asp:HyperLink> 
							for <asp:HyperLink id="hypEventName" runat="server" target="_blank"></asp:HyperLink>
						</i>
					</div>
				</div>
				<div class="col-6-xs">
					<div class="form-group" runat="server" visible="false" id="divEditCampaign">
						<div class="pull-right m-r-lg">
							<asp:Button id="btnEdit" runat="server" CssClass="btn btn-xs btn-warning editCampaign" Text="Edit Campaign" />
							<asp:Button id="btnUploadLogo" runat="server" CssClass="btn btn-xs btn-warning btnUploadLogo" Text="Upload Logo" />
						</div>
					</div>
				</div>
			</div>
		</div>
        <div class="alert alert-success" runat="server" id="divAlertMessage" visible="false">
            <i class="fa fa-bolt"></i> <asp:Literal id="litAlertMessage" runat="server"></asp:Literal>
        </div>
	</div>
</div>

<div class="content">
	<div class="row">
		<div class="col-lg-6">
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Social Media Links
				</div>
				<div class="panel-body">
					<dl class="dl-horizontal">
						<dt runat="server" id="dtWebsite" visible="false">
							Website
						</dt>
						<dd runat="server" id="ddWebsite" visible="false" class="m-b-sm">
							<asp:HyperLink id="hypWebsite" runat="server" target="new"></asp:HyperLink>
						</dd>
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
							Blog
						</dt>
						<dd class="m-b-sm">
							<asp:HyperLink id="hypBlog" runat="server" target="new"></asp:HyperLink>
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
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					This Campaign Created By:
				</div>
				<div class="panel-body">
					<dl class="dl-verticle">
						<dd>
							<asp:Label id="lblParentOrgName" runat="server"></asp:Label>
						</dd>
						<dd id="ddParentAddress" runat="server" class="m-b-sm">
							<asp:HyperLink id="hypParentAddress" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dd id="ddParentPhone" runat="server" visible="false">
							<asp:HyperLink id="hypParentPhone" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dd id="ddParentEmail" runat="server" visible="false">
							<asp:HyperLink id="hypParentEmail" runat="server" target="new"></asp:HyperLink>
						</dd>
						<dd id="ddParentWebsite" runat="server" visible="false">
							<asp:HyperLink id="hypParentWebsite" runat="server" target="new"></asp:HyperLink>
						</dd>	
					</dl>
				</div>
			</div>
		</div>
		<div class="col-lg-6">
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Volunteers
				</div>
				<div class="panel-body">
					<p>
						<asp:LinkButton id="lbVolunteer" runat="server" CssClass="btn btn-success volunteerButton" Text="Volunteer" visible="false"></asp:LinkButton>
					</p>
					
					<p>
						<asp:label id="lblVolunteerInstructions" runat="server"></asp:label>
					</p>
					<asp:Repeater ID="rpNonProfitPeople" runat="server" OnItemDataBound="rpNonProfitPeople_ItemDataBound">
						<HeaderTemplate>
							<table id="nonProfitsTable" class="table table-bordered table-hover">
								<thead>
									<tr>
										<th data-toggle="true">Full Name</th>
										<th data-toggle="true" data-hide="phone,tablet">Skills</th>
									</tr>
								</thead>
								<tbody>
						</HeaderTemplate>
						<ItemTemplate>
								<tr>
									<td>
										<div class="profile-picture">
											<asp:Image CssClass="pull-left m-r-md img-circle" width="60px" ID="imgProfilePhoto" runat="server"></asp:Image>
										</div>
										<asp:Literal id="lblInfo" runat="server"></asp:Literal>
									</td>
									<td><asp:Label ID="lblSkills" runat="server"></asp:Label></td>
								</tr>
						</ItemTemplate>
						<FooterTemplate>
								</tbody>
								<tfoot>
									<tr>
										<td colspan="2">
											<ul class="pagination pull-right"></ul>
										</td>
									</tr>
								</tfoot>
							</table>
						</FooterTemplate>
					</asp:Repeater>
				</div>
			</div>
		</div>
	</div>
</div>
</asp:Content>

