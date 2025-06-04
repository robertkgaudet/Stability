<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="AvailableDates.aspx.cs" Inherits="V1_Profile_AvailableDates" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
	<script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>

	<script>
		function getWeekNumber(date) {
			const firstJan = new Date(date.getFullYear(), 0, 1);
			const days = Math.floor((date - firstJan) / (24 * 60 * 60 * 1000));
			const weekNumber = Math.ceil((days + firstJan.getDay() + 1) / 7);
			return weekNumber;
		}

		function isBetweenJuneAndDecember(date) {
			const month = date.getMonth(); // Months are 0-based: January is 0, June is 5, December is 11
			return month >= 5 && month <= 11;
		}

		function displayDates(dates) {
			const groupedDates = {};

			dates.forEach(function (date) {
				const weekNumber = getWeekNumber(date);
				const monthYear = date.toLocaleDateString('en-US', { month: 'long', year: 'numeric' });

				if (!groupedDates[monthYear]) {
					groupedDates[monthYear] = {};
				}

				if (!groupedDates[monthYear][weekNumber]) {
					groupedDates[monthYear][weekNumber] = [];
				}

				const formattedDate = date.toLocaleDateString('en-US', { weekday: 'long', month: 'long', day: 'numeric' });
				groupedDates[monthYear][weekNumber].push({ date: formattedDate, isGreen: isBetweenJuneAndDecember(date) });
			});

			// Create HTML structure for grouped dates
			$('#selected-dates').empty();
			for (const monthYear in groupedDates) {
				const monthDiv = $('<div class="month-group"></div>').append('<h4>' + monthYear + '</h4>');
				for (const week in groupedDates[monthYear]) {
					const weekDiv = $('<div class="week-group"></div>').append('<h5>Week ' + week + '</h5>');
					groupedDates[monthYear][week].forEach(function (dateObj) {
						const dateDiv = $('<div class="date-div"></div>').text(dateObj.date);
						if (dateObj.isGreen) {
							dateDiv.css('background-color', 'green');
						}
						weekDiv.append(dateDiv);
					});
					monthDiv.append(weekDiv);
				}
				$('#selected-dates').append(monthDiv);
			}
		}

		$(document).ready(function () {
			$('#datepicker').datepicker({
				multidate: true,
				startDate: new Date(),
				format: 'mm/dd/yyyy'
				<%=readOnlyCalendar%>
			}).on('changeDate', function (e) {
				// Clear the container
				$('#selected-dates').empty();

				// Sort the dates
				const sortedDates = e.dates.sort((a, b) => a - b);

				// Object to store grouped dates
				const groupedDates = {};
				const hiddenDates = [];

				// Iterate over selected dates
				sortedDates.forEach(function (date) {
					const weekNumber = getWeekNumber(date);
					const monthYear = date.toLocaleDateString('en-US', { month: 'long', year: 'numeric' });

					if (!groupedDates[monthYear]) {
						groupedDates[monthYear] = {};
					}

					if (!groupedDates[monthYear][weekNumber]) {
						groupedDates[monthYear][weekNumber] = [];
					}

					const formattedDate = date.toLocaleDateString('en-US', { weekday: 'long', month: 'long', day: 'numeric' });
					groupedDates[monthYear][weekNumber].push({ date: formattedDate, isGreen: isBetweenJuneAndDecember(date) });

					// Add to hidden dates array
					hiddenDates.push(date.toISOString().split('T')[0]);
				});

				// Update hidden input
				$('#<%=hiddenAvailableDates.ClientID%>').val(JSON.stringify(hiddenDates));

				// Create HTML structure for grouped dates
				for (const monthYear in groupedDates) {
					const monthDiv = $('<div class="month-group"></div>').append('<h4>' + monthYear + '</h4>');
					for (const week in groupedDates[monthYear]) {
						const weekDiv = $('<div class="week-group"></div>').append('<h5>Week ' + week + '</h5>');
						groupedDates[monthYear][week].forEach(function (dateObj) {
							const dateDiv = $('<div class="date-div"></div>').text(dateObj.date);
							if (dateObj.isGreen) {
								dateDiv.css('background-color', 'green');
							}
							weekDiv.append(dateDiv);
						});
						monthDiv.append(weekDiv);
					}
					$('#selected-dates').append(monthDiv);
				}
			});

			// Preload dates
			const preloadedDates = JSON.parse($('#<%=hiddenAvailableDates.ClientID%>').val());
			const dateObjects = preloadedDates.map(dateString => new Date(dateString));
			$('#datepicker').datepicker('setDates', dateObjects);
			displayDates(dateObjects);
		});
    </script>
	<style>
		.date-div {
			background-color: #B490DA; /* Bootstrap warning color */
			color: white;
			padding: 10px;
			margin: 5px;
			border-radius: 4px;
			display: inline-block;
		}
		#selected-dates {
			display: flex;
			flex-direction: column;
		}
		.month-group {
			margin-bottom: 20px;
		}
		.week-group {
			display: flex;
			flex-wrap: wrap;
			margin-left: 20px;
			margin-bottom: 10px;
		}


		.datepicker {
			font-size: 2.2rem; /* Increase the font size of the entire datepicker */
		}
		.datepicker-days .day, 
		.datepicker-months .month, 
		.datepicker-years .year, 
		.datepicker-decades .decade, 
		.datepicker-centuries .century {
			padding: 10px; /* Increase the padding for better spacing */
		}
		.datepicker-days .day:hover, 
		.datepicker-months .month:hover, 
		.datepicker-years .year:hover, 
		.datepicker-decades .decade:hover, 
		.datepicker-centuries .century:hover {
			background-color: #f0ad4e; /* Change the hover color */
			color: white;
		}
		.datepicker table tr td span {
			font-size: 1.2rem; /* Increase the font size of month and year view */
		}
		.centerCalendarButton{
			display:flex;
			justify-content:center;
		}
		</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<h2 class="font-light m-b-xs">
								Choose Dates to Volunteer
							</h2>
                            <small>Choose as many dates as you would like.</small>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-12 container">
					<div class="hpanel form-horizontal">
						<div class="panel-heading hbuilt">
							Select the dates you are available to volunteer.
						</div>
						<div runat="server" id="divMessage" class="alert alert-success" visible="false">
							<i class="fa fa-bolt"></i>
							<asp:Literal runat="server" id="lblMessage"></asp:Literal>
						</div>
						<div class="panel-body p-lg">
							<div class="form-group">
								<div class="row">
									<div class="col-xs-12 col-md-4">
										<div id="datepicker"></div>
										<asp:HiddenField ID="hiddenAvailableDates" runat="server" />
										
										<div class="centerCalendarButton">
											<asp:LinkButton id="btnSubmit" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default m-r-sm" Text="Cancel" />
											<asp:Button id="btnCancel" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Update My Availability" />
										</div>
									</div>
									<div class="col-xs-12 col-md-8">
										<div id="selected-dates" class="d-flex flex-wrap mt-3"></div>
									</div>
								</div>
							</div>
						</div>
						<div class="panel-footer">
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>