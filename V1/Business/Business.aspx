<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Business.aspx.cs" Inherits="V1_Business_Business" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.img-rounded{max-width:100px;}
	</style>	<script type="text/javascript">

		$(document).ready(function () {
			$('.donateButton').click(function () {
				window.location.href = '<%=donateLink%>';
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
				<div class="col-sm-12">
					<h1 class="font-light m-b-xs">
						<asp:Literal id="litBusinessName" runat="server"></asp:Literal>
					</h1>
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
					<p>
						<asp:LinkButton id="lbDonate" runat="server" CssClass="btn btn-success donateButton" Text="Donate" visible="false"></asp:LinkButton>
					</p>
					<div class="form-group" runat="server" visible="false" id="divUploadLogoCover">
						<div class="pull-right">
							<asp:Button id="btnUploadLogo" runat="server" CssClass="btn btn-primary" Text="Upload Logo" />
							<asp:Button id="btnUploadCoverImage" runat="server" CssClass="btn btn-primary" Text="Upload Cover Image" />
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
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Business Details
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
				</div>
			</div>
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Social Media Links
				</div>
				<div class="panel-body">
					<dl class="dl-horizontal">
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
			<div class="hpanel hgreen">
				<div class="panel-heading hbuilt">
					Volunteers
				</div>
				<div class="panel-body">
					<asp:Repeater ID="rpNonProfitPeople" runat="server" OnItemDataBound="rpNonProfitPeople_ItemDataBound">
						<HeaderTemplate>
							<table id="nonProfitsTable" class="footable table table-bordered table-hover" data-page-size="20" data-filter="#filter">
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
											<asp:Image CssClass="pull-left m-r-md img-rounded" ID="imgProfilePhoto" runat="server"></asp:Image>
										</div>
										<asp:Literal id="lblInfo" runat="server"></asp:Literal>
									</td>
									<td><asp:Label ID="lblSkills" runat="server"></asp:Label></td>
								</tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:HyperLink id="hypMakeOwner" runat="server" CssClass="btn btn-info" visible="false"></asp:HyperLink>
                                    </td>
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

