<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="TeamAvailabilityCalendar.aspx.cs" Inherits="V1_NonProfit_TeamAvailabilityCalendar" %>
<%@ Register Src="~/V1/UserControls/TeamNavigation.ascx" TagPrefix="uc1" TagName="TeamNavigation" %>
<%@ Register Src="~/V1/UserControls/TeamHeader.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<link rel="stylesheet" href="/Homer/vendor/fullcalendar/dist/fullcalendar.print.css" media='print'/>
    <link rel="stylesheet" href="/Homer/vendor/fullcalendar/dist/fullcalendar.min.css" />
	<style>
		.highlight
		{
			background-color: #ffcccb !important;
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
                        <div class="font-normal">
							<div class="pull-right">
								<asp:HyperLink ID="hypMyCalendar" runat="server" Text="Update My Calendar" CssClass="font-normal btn btn-sm btn-info"></asp:HyperLink>
							</div>
							<h1 class="m-b-none"> <i class="fa fa-calendar"></i> Team Calendar</h1>
							<small class="text-muted">Team members available for deployment.</small>
                        </div>
                    </div>
					<!--PAGE CONTENT-->
					<div class="m-t-md">
						<div id="divUpdateMessage" runat="server" class="alert alert-warning text-center" style="margin-bottom:20px;" visible="false">
							<asp:Literal ID="litMessage" runat="server"></asp:Literal>
						</div>
						<div class="row" id="divCalendarRow" runat="server">
							<div class="col-lg-12">
								<div class="row">
									<div class="col-xs-12 col-sm-12 col-md-6">
										<div class="hpanel stats">
											<div class="panel-body h-200 list">
												<div id="calendar"></div>
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
								</div>
							</div>
						</div>
					</div>
					<!--PAGE FOOTER-->
                </div>
            </div>
        </div>
    </div>
	
	<script src="/Homer/scripts/charts.js"></script>
	<script src="/Homer/vendor/chartjs/Chart.min.js"></script>
	<script src="/Homer/vendor/sparkline/index.js"></script>
	
	<script>

		$(document).ready(function () {
			function fetchDates(organizationId) {
				$.ajax({
					url: '/V1/NonProfit/TeamAvailabilityCalendar.aspx/LoadCalendar',
					method: 'POST',
					contentType: 'application/json; charset=utf-8',
					dataType: 'json',
					data: JSON.stringify({ organizationId: organizationId }),
					success: function (response) {
						var events = JSON.parse(response.d);

						$('#calendar').fullCalendar('destroy'); // Destroy the existing calendar
						$('#calendar').fullCalendar({
							header: {
								left: 'prev,next today',
								center: 'title',
								right: 'month,agendaWeek,agendaDay'
							},
							format: 'mm/dd/yyyy',
							defaultView: 'month',
							editable: false,
							//eventLimit: true, // allow "more" link when too many events
							//todayHighlight: true,
							//beforeShowDay: function (date) {
							//	// Highlight weekends
							//	var day = date.getDay();
							//	if (day === 0 || day === 6) {
							//		return {
							//			classes: 'highlight'
							//		};
							//	}
							//	return;
							//},
							events: events
						});
					},
					error: function (error) {
						console.error('Error loading events:', error);
					}
				});
			}

			// Initialize the calendar with no events
			$('#calendar').fullCalendar({
				header: {
					left: 'prev,next today',
					center: 'title',
					right: 'month,agendaWeek,agendaDay'
				},
				format: 'mm/dd/yyyy',
				defaultView: 'month',
				editable: false,
				//eventLimit: true,
				//beforeShowDay: function (date) {
				//	// Highlight weekends
				//	var day = date.getDay();
				//	if (day === 0 || day === 6) {
				//		return {
				//			classes: 'highlight'
				//		};
				//	}
				//	return;
				//}
			});

			fetchDates('<%=organizationId%>');
		});


	</script>
	
	<script>
		$(document).ready(function () {

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

	<script src="/Homer/vendor/moment/min/moment.min.js"></script>
	<script src="/Homer/vendor/fullcalendar/dist/fullcalendar.min.js"></script>
</asp:Content>