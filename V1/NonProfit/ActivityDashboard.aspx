<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="ActivityDashboard.aspx.cs" Inherits="V1_NonProfit_ActivityDashboard" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">	<script type="text/javascript">

		$(document).ready(function () {
			$('.table').footable({
				columns: [
					{ name: "BeginDate", type: "date" },
					{ name: "EndDate", type: "date" }
				]
			});
		});	
</script>


  <style>

    .container {
      display: flex;
      flex-direction: row;
    }

    .weekdays {
      display: flex;
      flex-direction: column;
      justify-content: space-between;
      margin-right: 8px;
      font-size: 10px;
      height: 40px; /* 7 days * 14px + gaps */
    }

    .weekdays div:nth-child(even) {
      visibility: hidden;
    }

    .chart-container {
      overflow-x: auto;
    }

    .months {
      display: flow;
      margin-bottom: 4px;
      margin-left: 42px;
      font-size: 10px;
    }

    .months span {
      flex: 1;
      min-width: 10px;
      text-align: left;
      margin-left: 4.3px;
    }

    .chart {
      display: grid;
      grid-template-columns: repeat(53, 5px);
      grid-template-rows: repeat(7, 9px);
      gap: 3px;
    }

    .day {
      width: 5px;
      height: 9px;
      background-color: #ebedf0;
      border-radius: 2px;
      transition: background-color 0.2s;
    }

    .level-1 { background-color: #c6e48b; }
    .level-2 { background-color: #7bc96f; }
    .level-3 { background-color: #239a3b; }
    .level-4 { background-color: #196127; }

    @media (max-width: 400px) {

      .months {
        font-size: 10px;
      }

      .weekdays {
        font-size: 10px;
      }
    }
  </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />
					
				  <h2>Team Members Availability</h2>

				  <div class="months" id="months"></div>

				  <div class="container">
					<div class="weekdays">
					  <div>Mon</div>
					  <div>Tue</div>
					  <div>Wed</div>
					  <div>Thu</div>
					  <div>Fri</div>
					  <div>Sat</div>
					</div>
					<div class="chart-container">
					  <div class="chart" id="activity-chart"></div>
					</div>
				  </div>

				  <script>
					  const chart = document.getElementById('activity-chart');
					  const months = document.getElementById('months');
					  const weeks = 53;
					  const days = 7;
					  const today = new Date();
					  const yearStart = new Date(today.getFullYear(), 0, 1);

					  // Fill chart with dummy data
					  for (let w = 0; w < weeks; w++) {
						  for (let d = 0; d < days; d++) {
							  const day = document.createElement('div');
							  day.classList.add('day');

							  // Random level for demonstration
							  const level = Math.floor(Math.random() * 5);
							  if (level > 0) day.classList.add(`level-${level}`);

							  chart.appendChild(day);
						  }
					  }

					  // Add month labels (approximate)
					  const monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
					  for (let i = 0; i < weeks; i++) {

						  //Loop through weeks
						  const weekStart = new Date(yearStart);
						  weekStart.setDate(yearStart.getDate() + i * 7);
						  const month = weekStart.getMonth();
						  const label = document.createElement('span');

						  // Only label the first week of a new month
						  if (i === 0 || weekStart.getDate() <= 7) {
							  label.textContent = monthNames[month];
						  } else {
							  label.textContent = '';
						  }

						  months.appendChild(label);
					  }
				  </script>


						<div class="m-t-md">
						<div class="animate-panel">
							<div class="row">
								<div class="col-xs-6 col-md-3">
									<div class="hpanel">
										<div class="panel-body">
											<div class="stats-title pull-left">
												<h4>Active Deployments</h4>
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
															<th>All Deployments</th>
															<th data-name="BeginDate">Begin</th>
															<th data-name="EndDate">End</th>
															<th>Days</th>
														</tr>
													</thead>
													<tbody>
														<asp:Repeater ID="rptActiveCampaigns" runat="server" OnItemDataBound="rptActiveCampaigns_ItemDataBound">
															<ItemTemplate>
																<tr>
																	<td>
																		<span class="text-success"><asp:HyperLink CssClass="text-muted" ID="hypCauseName" runat="server"></asp:HyperLink></span>
																	</td>
																	<td runat="server" id="tdBeginDate"><asp:label CssClass="text-muted" ID="lblBeginDate" runat="server"></asp:label></td>
																	<td runat="server" id="tdEndDate"><asp:label CssClass="text-muted" ID="lblEndDate" runat="server"></asp:label></td>
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
								<div class="col-xs-12 col-sm-12 col-md-6">
									<div class="hpanel stats">
										<div class="panel-body h-200 list">
											<div class="stats-title pull-left">
												<h4>Future Scheduling</h4>
											</div>
											<div class="stats-icon pull-right">
												<i class="fa fa-calendar-check-o text-success fa-4x"></i>
											</div>
											<div class="m-t-xl">
												<span class="font-bold no-margins">
													Team Members Available Over Next 6 Weeks
												</span>
												<br/>
												<div class="hpanel">
													<div class="panel-body">
														<div>
															<canvas id="lineOptions" height="140"></canvas>
														</div>
													</div>
												</div>
											</div>
										</div>
									</div>
								</div>
								<div class="col-xs-12 col-sm-12 col-md-6">
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
							<div class="row">
							</div>
						</div>
					</div>
	
	<script src="/Homer/vendor/jquery-ui/jquery-ui.min.js"></script>
	<script src="/Homer/vendor/jquery-flot/jquery.flot.js"></script>
	<script src="/Homer/vendor/jquery-flot/jquery.flot.resize.js"></script>
	<script src="/Homer/vendor/jquery-flot/jquery.flot.pie.js"></script>
	<script src="/Homer/scripts/charts.js"></script>
	<script src="/Homer/vendor/chartjs/Chart.min.js"></script>
	<script src="/Homer/vendor/sparkline/index.js"></script>

	<script>
		$(document).ready(function () {

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



			var lineData = {
				labels: [<%=availableDates%>],
				datasets: [

					{
						label: "Team Member Count",
						backgroundColor: 'rgba(98,203,49, 0.5)',
						pointBorderWidth: 1,
						pointBackgroundColor: "rgba(98,203,49,1)",
						pointRadius: 3,
						pointBorderColor: '#ffffff',
						borderWidth: 1,
						data: [<%=teamCounts%>]
					},
					{
						label: "Deplyment Members Needed",
						backgroundColor: 'rgba(220,220,220,0.5)',
						pointBorderWidth: 1,
						pointBackgroundColor: "rgba(98,203,49,1)",
						pointRadius: 3,
						pointBorderColor: '#ffffff',
						borderWidth: 1,
						data: [22, 44, 67, 43, 76, 45]
					}
					//,
					//{
					//	label: "Dt 2",
					//	backgroundColor: 'rgba(220,220,220,0.5)',
					//	borderColor: "rgba(220,220,220,0.7)",
					//	pointBorderWidth: 1,
					//	pointBackgroundColor: "rgba(220,220,220,1)",
					//	pointRadius: 3,
					//	pointBorderColor: '#ffffff',
					//	borderWidth: 1,
					//	data: [22, 44, 67, 43, 76, 45, 12]
					//}
				]
			};

			var lineOptions = {
				responsive: true
			};

			var ctx = document.getElementById("lineOptions").getContext("2d");
			new Chart(ctx, { type: 'line', data: lineData, options: lineOptions });
		});
	</script>









				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />


</asp:Content>