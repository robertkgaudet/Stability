<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/2-Column-Child.master" AutoEventWireup="true" CodeFile="Deployments.aspx.cs" Inherits="V1_Deployments" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/2-Column-Child.master"%>
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
						<asp:HyperLink Text="Create A Disaster Deployment" runat="server" visible="false" id="hypAddDeployment" class="btn btn-info btn-large pull-right"></asp:HyperLink>
						<h4><asp:Literal ID="litPageName" runat="server"></asp:Literal></h4>
						Find an active disaster deployment to join or <a href="/V1/Administration/TeamName.aspx?userActionModal=false">create a team</a> to start your own deployment.
						<asp:HyperLink ID="hypMapView" runat="server" CssClass="pull-right" Text="Map View <i class='fa fa-map m-t-xs'></i>" NavigateUrl="/MapZone"></asp:HyperLink>
						<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search Deployments">
						<p>
						<span style="font-size:14px; font-weight:normal;" class="text-muted">
							<asp:Literal ID="litCount" runat="server"></asp:Literal>
						</span>
						</p>
						<div class="alert alert-success" id="divJoinDeploymentMessage" runat="server" visible="false">
							<h4><i class="fa fa-users"></i> Choose A Deployment or Create One</h4>
						</div>
						<table id="tblDeployments" class="footable table toggle-arrow-tiny table-hover table-bordered table-striped" data-page-size="500" data-filter="#filter">
							<%--<thead>
								<tr>
									<th><h4>Non-Profits, Groups and Teams</h4>
									</th>
								</tr>
							</thead>--%>
							<tbody>
								<asp:Repeater ID="rptDeployments" runat="server" OnItemDataBound="rptDeployments_ItemDataBound">
									<ItemTemplate>
										<tr>
											<td>
												<div style="height:50px;">
													<div class="row">
														<div class="col-xs-12 col-lg-6">
															<asp:Image ID="imgLogo" CssClass="m-r-md" runat="server" />
															<asp:HyperLink ID="hypDeploymentName" Font-Bold="true" runat="server"></asp:HyperLink><br />
															<asp:Label ID="lblDescription" runat="server"></asp:Label>
														</div>
														<div class="col-xs-12 col-lg-6">
															<div class="pull-left text-muted">
																<small>
																	<asp:HyperLink ID="hypEventName" runat="server"></asp:HyperLink>
																	<br />
																	<asp:Literal ID="litOrganizationName" runat="server"></asp:Literal>
																	<br />
																	<asp:Literal ID="litDates" runat="server"></asp:Literal>
																</small>
															</div>
														</div>
													</div>
												</div>
											</td>
										</tr>
									</ItemTemplate>
								</asp:Repeater>
							</tbody>
							<tfoot>
								<tr>
									<td colspan="5">
										<ul class="pagination pull-right"></ul>
									</td>
								</tr>
							</tfoot>
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