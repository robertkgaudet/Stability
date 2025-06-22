<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="ActivityDashboard.aspx.cs" Inherits="V1_NonProfit_ActivityDashboard" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">	<meta name="viewport" content="width=device-width, initial-scale=1">	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css">	<script type="text/javascript">

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
.calendar-container {
  display: flex;
  flex-direction: row;
  align-items: flex-start;
  width: 100%;
}

.chart {
  display: grid;
  grid-template-rows: repeat(7, 1fr);
  grid-auto-flow: column;
  grid-auto-columns: 1fr;
  gap: 2px;
  width: 100%;
  max-width: 100%;
}

@media (max-width: 768px) and (pointer: coarse) {
  .chart {
    display: grid;
    grid-template-rows: repeat(7, 1fr);
    grid-auto-flow: column;
    grid-auto-columns: minmax(0, 1fr);  /* Resize columns to fit */
    gap: 2px;
    width: 100%;
    max-width: 100%;
  }

  .day {
    width: 100%;
    aspect-ratio: 1 / 1;
  }

  .calendar-container {
    width: 100%;
    overflow: hidden;
  }
}


.day {
  background-color: #eee;
  position: relative;
  aspect-ratio: 1 / 1;
  cursor: pointer;
  transition: background 0.2s ease;
}

.day:hover {
  background-color: #5E2E91 !important;
}

.level-1 { background-color: #c6e48b; }
.level-2 { background-color: #7bc96f; }
.level-3 { background-color: #239a3b; }
.level-4 { background-color: #196127; }

.past-day {
  background-image: repeating-linear-gradient(
    45deg,
    rgba(60, 60, 60, 0.3),
    rgba(60, 60, 60, 0.3) 2px,
    transparent 2px,
    transparent 4px
  ) !important;
}

.today {
  background-color: #fff !important;
  position: relative;
}

.today-pinwheel {
	width: 16px;
	height: 16px;
	z-index: 1000;
	position: absolute;
	top: 50%;
	left: 50%;
	transform: translate(-50%, -50%);
	pointer-events: none;
	animation: spin-reverse 20s linear infinite; /* Slow spin: 12 seconds per rotation */
}

@keyframes spin-reverse {
  from { transform: translate(-50%, -50%) rotate(0deg); }
  to { transform: translate(-50%, -50%) rotate(-360deg); }
}

.availability-count {
  color: #fff;
  font-size: 12px;
  font-weight: bold;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  pointer-events: none;
}

.months {
  display: flex;
  gap: 10px;
  font-size: 12px;
  margin: 10px 0;
}

.months span {
  width: 48px;
  text-align: center;
}

.weekdays div {
  font-size: 10px;
}

.chart-wrapper-outer {
  width: 100%;
}

.chart-labels-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 4px;
}

.chart-label-start {
  text-align: left;
  font-size: 13px;
  color: #555;
  margin: 6px 4px;
}

.chart-label-end {
  text-align: right;
  font-size: 13px;
  color: #555;
  margin: 6px 4px;
}

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />
					
				<h2>Daily Situation Reports
					<br /><small style="font-size:14px;">Choose a cell for the report you want to view.</small>
				</h2>
				<div class="chart-wrapper-outer">
					<div class="block right-align">
						<div class="chart-labels-row pull-left">
							<div class="chart-label-start" id="chart-start-date"></div>
						</div>
					
						<button id="load-previous" type="button" class="btn btn-sm btn-secondary pull-right" style="margin-bottom: 10px;">
						  ⬅️ Prior Year
						</button>
						<button id="back-to-current" type="button" class="btn btn-sm btn-secondary pull-right m-r-sm" style="margin-bottom: 10px; display: none;">
						  🔄 Back
						</button>
					</div>
					<div class="chart" id="activity-chart"></div>

					<div class="chart-label-end" id="chart-end-date"></div>
				</div>

<script type="text/javascript">

	const urlFriendlyName = (() => {
		const qsValue = new URLSearchParams(window.location.search).get('urlfriendlyName');
		if (qsValue) return qsValue;

		const pathParts = window.location.pathname.split('/');
		const teamIndex = pathParts.findIndex(p =>
			p.toLowerCase() === 'team' || p.toLowerCase() === 'v1' || p.toLowerCase() === 'nonprofit'
		);

		if (teamIndex !== -1 && pathParts.length > teamIndex + 1) {
			return pathParts[teamIndex + 1];
		}

		return 'GroundForceHumanitarianAid'; // fallback default
	})();

	var availability = <%= AvailabilityByDateJson %>; // JSON from server

	const chart = document.getElementById('activity-chart');
	const today = new Date();
	today.setHours(0, 0, 0, 0);

	const urlParams = new URLSearchParams(window.location.search);
	const startParam = urlParams.get('start');

	let startDate;
	let totalDays;

	if (window.innerWidth < 768) {
		if (startParam) {
			startDate = new Date(startParam);
		} else {
			startDate = new Date(today);
			startDate.setDate(today.getDate() - 7);
		}
		totalDays = 161;
	} else {
		if (startParam) {
			startDate = new Date(startParam);
		} else {
			startDate = new Date(today);
			startDate.setMonth(today.getMonth() - 1);
		}
		totalDays = 371;
	}



	// 🟨 Add this global reference for click handler
	const initialStartDate = new Date(startDate);

	document.addEventListener("DOMContentLoaded", function () {
		const backToCurrentBtn = document.getElementById('back-to-current');
		const loadPrevBtn = document.getElementById('load-previous');

		// Always pull the latest start param here
		const urlParams = new URLSearchParams(window.location.search);
		const startParam = urlParams.get('start');

		if (startParam) {
			backToCurrentBtn.style.display = 'inline-block';
		}

		loadPrevBtn.addEventListener('click', () => {
			let baseDate = startParam ? new Date(startParam) : new Date();
			baseDate.setHours(0, 0, 0, 0);

			if (!startParam) {
				baseDate.setMonth(baseDate.getMonth() - 1); // fallback if no param
			}

			baseDate.setDate(baseDate.getDate() - 371);
			const iso = baseDate.toISOString().split('T')[0];

			const newUrl = `/V1/NonProfit/ActivityDashboard.aspx?urlfriendlyName=${encodeURIComponent(urlFriendlyName)}&start=${iso}`;

			// Force full reload to avoid caching issues on mobile
			window.location.replace(newUrl);
		});

		backToCurrentBtn.addEventListener('click', () => {
			window.location.href = `/V1/NonProfit/ActivityDashboard.aspx?urlfriendlyName=${encodeURIComponent(urlFriendlyName)}`;
		});

	});




	renderChart();

	// Add start/end labels
	const formatLabel = date => date.toLocaleDateString(undefined, {
		month: 'short', day: 'numeric', year: 'numeric'
	});

	document.getElementById('chart-start-date').textContent = 'From: ' + formatLabel(startDate);

	const endDate = new Date(startDate);
	endDate.setDate(startDate.getDate() + totalDays - 1);
	document.getElementById('chart-end-date').textContent = 'To: ' + formatLabel(endDate);

	$(document).ready(() => {
		
		$('[data-toggle="tooltip"]').tooltip();
	});

	function renderChart() {
		chart.innerHTML = '';

		for (let i = 0; i < totalDays; i++) {
			const date = new Date(startDate);
			date.setDate(startDate.getDate() + i);

			const iso = date.toISOString().split('T')[0];
			const count = availability[iso] || 0;

			const day = document.createElement('div');
			day.className = 'day';
			day.setAttribute('data-date', iso);

			if (count > 0) {
				let levelClass = '';
				if (count <= 2) levelClass = 'level-1';
				else if (count <= 7) levelClass = 'level-2';
				else if (count <= 11) levelClass = 'level-3';
				else levelClass = 'level-4';
				day.classList.add(levelClass);
			}

			const cellDate = new Date(date);
			cellDate.setHours(0, 0, 0, 0);

			if (cellDate.getTime() === today.getTime()) {
				day.classList.add('today');

				const img = document.createElement('img');
				img.src = '/V1/Images/pinwheel.png';
				img.alt = 'Today';
				img.className = 'today-pinwheel';
				day.appendChild(img);
			} else if (cellDate < today) {
				day.classList.add('past-day');
			} else if (count > 0) {
				const countLabel = document.createElement('span');
				countLabel.className = 'availability-count';
				countLabel.textContent = count;
				day.appendChild(countLabel);
			}

			const formattedDate = date.toLocaleDateString(undefined, {
				year: 'numeric', month: 'long', day: 'numeric'
			});

			const tooltipText = (count > 0)
				? `${formattedDate} – ${count} team members available`
				: `${formattedDate} – no availability data`;

			day.setAttribute('title', tooltipText);
			day.setAttribute('data-toggle', 'tooltip');

			day.addEventListener('click', () => {
				window.location.href = `/SituationReport/${encodeURIComponent(urlFriendlyName)}/${iso}`;
			});

			chart.appendChild(day);
		}

		// Update start/end labels
		const formatLabel = date => date.toLocaleDateString(undefined, {
			month: 'short', day: 'numeric', year: 'numeric'
		});

		document.getElementById('chart-start-date').textContent = 'From: ' + formatLabel(startDate);

		const endDate = new Date(startDate);
		endDate.setDate(startDate.getDate() + totalDays - 1);
		document.getElementById('chart-end-date').textContent = 'To: ' + formatLabel(endDate);

		$('[data-toggle="tooltip"]').tooltip(); // reinit tooltips
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