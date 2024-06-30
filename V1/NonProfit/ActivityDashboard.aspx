<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="ActivityDashboard.aspx.cs" Inherits="V1_NonProfit_ActivityDashboard" %>
<%@ Register Src="~/V1/UserControls/TeamNavigation.ascx" TagPrefix="uc1" TagName="TeamNavigation" %>
<%@ Register Src="~/V1/UserControls/TeamHeader.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                        <div class="font-normal">
							<h1 class="m-b-none">Impact Dashboard</h1>
							<small class="text-muted">Deployments and impact from this team.</small>
                        </div>
                    </div>
					<!--PAGE CONTENT-->
					<div class="m-t-md">
						<div class="animate-panel">
							<div class="row">
								<div class="col-xs-6 col-md-3">
									<div class="hpanel">
										<div class="panel-body">
											<div class="stats-title pull-left">
												<h4>Deployments</h4>
											</div>
											<div class="stats-icon pull-right">
												<i class="fa fa-street-view fa-4x text-success"></i>
											</div>
											<div class="m-t-xl">
												<h1 class="text-success"><asp:Label id="lblCauseCount" runat="server"></asp:Label></h1>
												<small>
													Deployments are the first step to helping your community. <strong>Team members engage</strong> and all activity happen when you are deployed.
												</small>
											</div>
										</div>
									</div>
								</div>
								<div class="col-xs-6 col-sm-3">
									<div class="hpanel stats">
										<div class="panel-body h-200">
											<div class="stats-title pull-left">
												<h4>Team Members</h4>
											</div>
											<div class="stats-icon pull-right">
												<i class="fa fa-id-badge text-success fa-4x"></i>
											</div>
											<div class="clearfix"></div>
											<div class="flot-chart">
												<div class="flot-chart-content" id="flot-team-chart"></div>
											</div>
											<div class="m-t-xs">
												<div class="row">
													<div class="col-xs-12">
														<small class="stat-label">Team Member Count</small>
														<h4><asp:Label id="lblTeamCount" runat="server"></asp:Label></h4>
													</div>
												</div>
											</div>
										</div>
									</div>
								</div>
								<div class="col-xs-12 col-sm-6">
									<div class="hpanel stats">
										<div class="panel-body h-200 list">
											<div class="stats-title pull-left">
												<h4>Team Hours and Impact</h4>
											</div>
											<div class="stats-icon pull-right">
												<i class="fa fa-bolt text-success fa-4x"></i>
											</div>
											<div class="m-t-xl">
												<span class="font-bold no-margins">
													Why Track Time?
												</span>
												<br/>
												<small>
													It's important to track every minute of donated time because your time can help your local community to be reimbursed for public projects by FEMA.
													<b>The more qualified time you donate, the more your community can earn.</b>
												</small>
											</div>
											<div class="row m-t-sm">
												<div class="col-lg-6">
													<h3 class="no-margins font-extra-bold text-success"><asp:Label id="lblHours" runat="server"></asp:Label></h3>
													<div class="font-bold"> <i class="fa fa-level-up text-success"></i> HOURS WORKED</div>
												</div>
												<div class="col-lg-6">
													<h3 class="no-margins font-extra-bold text-success"><asp:Label id="lblOffset" runat="server"></asp:Label></h3>
													<div class="font-bold">$ POTENTIAL OFFSET <i class="fa fa-level-up text-success"></i></div>
												</div>
											</div>
										</div>
									</div>
								</div>
							</div>
							<div class="row">
								<div class="col-xs-12 col-sm-12 col-md-6">
									<div class="hpanel">
										<div class="panel-body">
											<div class="table-responsive">
												<table class="table table-striped">
													<thead>
														<tr>
															<th>Deployments</th>
															<th>Begin</th>
															<th>End</th>
															<th>Days</th>
														</tr>
													</thead>
													<tbody>
														<asp:Repeater ID="rptActiveCampaigns" runat="server" OnItemDataBound="rptActiveCampaigns_ItemDataBound">
															<ItemTemplate>
																<tr class="">
																	<td>
																		<span class="text-success"><asp:HyperLink CssClass="text-muted" ID="hypCauseName" runat="server"></asp:HyperLink></span>
																	</td>
																	<td><asp:label CssClass="text-muted" ID="lblBeginDate" runat="server"></asp:label></td>
																	<td><asp:label CssClass="text-muted" ID="lblEndDate" runat="server"></asp:label></td>
																	<td><asp:label CssClass="text-muted" ID="lblDeploymentLength" runat="server"></asp:label></td>
																</tr>
															</ItemTemplate>
														</asp:Repeater>
													</tbody>
												</table>
											</div>
										</div>
									</div>
								</div>
								<div class="col-xs-12 col-sm-6">
									<div class="hpanel stats">
										<div class="panel-body h-200 list">
											<div class="stats-title pull-left">
												<h4>Deployed Communities</h4>
											</div>
											<div class="stats-icon pull-right">
												<i class="fa fa-dot-circle-o text-success fa-4x"></i>
											</div>
											<div class="m-t-xl">
												<span class="font-bold no-margins">
													States and Counties
												</span>
												<br/>
												<small>
													<asp:Literal ID="litStatesCounties" runat="server"></asp:Literal>
												</small>
											</div>
										</div>
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
	<script src="/Homer/vendor/jquery-flot/jquery.flot.js"></script>
	<script src="/Homer/vendor/jquery-flot/jquery.flot.resize.js"></script>
	<script src="/Homer/vendor/jquery-flot/jquery.flot.pie.js"></script>
	<script src="/Homer/scripts/charts.js"></script>
	<script type="text/javascript">
		$(function () {

			/**
			 * Flot charts line data and options
			 */
			var chartIncomeData = [
				{
					label: "line",
					data: [[1, 10], [2, 26], [3, 16], [4, 36], [5, 32], [6, 51]]
				}
			];

			var chartIncomeOptions = {
				series: {
					lines: {
						show: true,
						lineWidth: 0,
						fill: true,
						fillColor: "#64cc34"

					}
				},
				colors: ["#62cb31"],
				grid: {
					show: false
				},
				legend: {
					show: false
				}
			};

			$.plot($("#flot-team-chart"), chartIncomeData, chartIncomeOptions);


			/**
			 * Line Chart Data and Options
			 */

			var lineChartData = [
				{
					label: "line",
					data: [[1, 24], [2, 15], [3, 29], [4, 34], [5, 30], [6, 40], [7, 23], [8, 27], [9, 40]]
				}
			];

			var lineChartOptions = {
				series: {
					lines: {
						show: true,
						lineWidth: 1,
						fill: true,
						fillColor: {
							colors: [{ opacity: 1 }, { opacity: 1 }
							]
						}
					}
				},
				xaxis: {
					tickDecimals: 0
				},
				colors: ["#62cb31"],
				grid: {
					tickColor: "#e4e5e7",
					borderWidth: 1,
					borderColor: '#e4e5e7',
					color: '#6a6c6f'
				},
				legend: {
					show: false
				},
				tooltip: true,
				tooltipOpts: {
					content: "x: %x, y: %y"
				}
			};

			$.plot($("#flot-line-chart"), lineChartData, lineChartOptions);
		});
	</script>
</asp:Content>