<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/2-Column-Child.master" AutoEventWireup="true" CodeFile="DeploymentTeams.aspx.cs" Inherits="V1_NonProfit_DeploymentTeams" %>
<%@ Register Src="~/V1/UserControls/MemberNavigation.ascx" TagPrefix="uc1" TagName="MemberNavigation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-xs-12">
				<div class="hpanel">
					<div class="panel-body">
						<h4>
							<asp:Literal ID="litPageName" runat="server"></asp:Literal>
						</h4>
	
						<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search Deployments">
						<table id="tblDeploymentTeams" class="footable table table-stripped toggle-arrow-tiny" data-page-size="8" data-filter="#filter">
							<thead>
								<tr>
									<th data-toggle="true">Deployment</th>
									<th>Dates</th>
									<th data-hide="phone">Location</th>
									<th data-hide="phone">Openings</th>
								</tr>
							</thead>
							<tbody>
								<asp:Repeater ID="rptDeploymentTeams" runat="server" OnItemDataBound="rptDeploymentTeams_ItemDataBound">
									<ItemTemplate>
										<tr>
											<td>
												<asp:HyperLink ID="hypCampaignName" runat="server"></asp:HyperLink>
												<br />
												<asp:HyperLink ID="hypPositions" Target="_blank" CssClass="btn btn-success m-t-md" runat="server" Text="See Open Positions"></asp:HyperLink>
											</td>
											<td>
												<asp:Label ID="lblEarliestDeploymentDate" runat="server"></asp:Label> to <br />
												<asp:Label ID="lbLatestDeploymentDate" runat="server"></asp:Label>
											</td>
											<td>
												<asp:Label ID="lblDeploymentLocation" runat="server"></asp:Label>
											</td>
											<td>
												<asp:Label ID="lblPositionCount" runat="server"></asp:Label>
											</td>
										</tr>
									</ItemTemplate>
								</asp:Repeater>
							</tbody>
						</table>
					</div>
				</div>
			</div>
			<!--NAVIGATION-->
			<div class="col-sm-4 col-lg-3">
			</div>
		</div>
		<script type="text/javascript">

			$(window).on('load', function () {
				$('.footable').footable();
			});
		</script>
		<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
	<uc1:MemberNavigation runat="server" ID="ucMemberNavigation" />
</asp:Content>