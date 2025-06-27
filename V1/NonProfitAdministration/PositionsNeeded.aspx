<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="PositionsNeeded.aspx.cs" Inherits="V1_NonProfitAdministration_PositionsNeeded" %>
<%@ Register Src="~/V1/UserControls/PositionNavigation.ascx" TagPrefix="uc1" TagName="PostionNavigation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="/Homer/vendor/clockpicker/dist/bootstrap-clockpicker.min.css" />
	<link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
	<link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
	<link rel="stylesheet" href="/Homer/vendor/bootstrap-datepicker-master/dist/css/bootstrap-datepicker3.min.css" />
	<link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

	<style>
	 .child-master-container
	{
		padding:0px;
	}
	h4
	{color:#9B59B6;}
	h3
	{color:#5E2E91;}
        .circle {
            display: inline-block;
            padding: 0px;
			background-color:#9B59B6;
			color: white;
			font-family:Georgia;
            border-radius: 50%; /* Creates the circular shape */
            text-align: center; /* Horizontally center the text */
            font-size: 25px; /* Font size of the number */
            width: 40px; /* Circle width */
            height: 40px; /* Circle height */
            line-height: 35px; /* Vertically center the text */
			margin-right:10px;
        }
		.position-details
		{
			border:double 3px #f1f1f1;
			padding:20px 20px 0px 20px;
			border-radius:10px;
			background-color:#fafafa;
			margin-bottom:10px;
			position: relative;
		}
		.positionPanel .panel-body{
			border:none;
			padding:0px;
		}
		.input-group-addon
		{
			font-size:16px !important;
		}
		.delete-icon {
            position: absolute;
            top: 5px;
            right: 10px;
            color: darkgrey;
            cursor: pointer;
			font-size:15px;
        }
		input[type="checkbox"] {
            transform: scale(1.5); /* Increase the size by 1.5 times */
            -webkit-transform: scale(1.5); /* For older versions of WebKit browsers */
            -moz-transform: scale(1.5); /* For older versions of Firefox */
        }

	</style>
	<script>
		var userId = '<%=_userId%>';
		var customItems = []; // To store new terms entered by the user
		var selectedItems = [];

		$(document).ready(function () {
			$('.arrival-time').clockpicker({autoclose: true,twelvehour: true,align: 'top'});

			$('.departure-time').clockpicker({
				autoclose: true,
				twelvehour: true,
				align: 'top'
			});

			// Store a reference to the original div
			var originalDiv = $('#positionDetails').clone();

			// Clone the original div each time the button is clicked
			$('#duplicateButton').on('click', function () {
				// Clone the stored original div, not the dynamically added ones
				var clonedDiv = originalDiv.clone();
				// Clear the input fields in the cloned div

				// Set default times for the newly cloned clockpickers
				clonedDiv.find('.arrival-time').val('09:00');
				clonedDiv.find('.departure-time').val('17:00');
				clonedDiv.find('.input-positions-needed').val('1');

				// Append the cloned div before the button
				clonedDiv.insertBefore('#duplicateButton');

				// Initialize ClockPicker for the newly added clockpicker inputs
				clonedDiv.find('.arrival-time').clockpicker({ autoclose: true, twelvehour: true, align: 'top' });
				clonedDiv.find('.departure-time').clockpicker({ autoclose: true, twelvehour: true, align: 'top' });

				// Attach the delete functionality to the new delete icon
				clonedDiv.find('.delete-icon').on('click', function () {
					$(this).closest('.position-details').remove(); // Remove the closest parent div
				});

				clonedDiv.find('.autocomplete-input').removeData('ui-autocomplete');

				// Optionally, you can clear the input value
				clonedDiv.find('.autocomplete-input').val('');

				// Append the cloned div to your desired location
				//$('#positionDetails').after(originalDiv);

				// Reinitialize autocomplete for the new cloned input field
				initializeAutocomplete(clonedDiv.find('.autocomplete-input'));

				clonedDiv.find('.autocomplete-input').focus();
			});



			// Attach the delete functionality to the original delete icon
			$('#positionDetails').find('.delete-icon').on('click', function () {
				$(this).closest('.position-details').remove(); // Remove the closest parent div
			});



			$('#<%=txtMessage.ClientID%>').summernote({
				toolbar: [
					['style', ['bold', 'italic', 'underline']],
					['alignment', ['ul', 'ol', 'paragraph']]
				],

				height: 125
			});



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
			$('#<%=hiddenDeployedDates.ClientID%>').val(JSON.stringify(hiddenDates));


			// Create HTML structure for grouped dates
			for (const monthYear in groupedDates) 
			{
				const monthDiv = $('<div class="month-group"></div>').append('<h4>' + monthYear + '</h4>');
				for (const week in groupedDates[monthYear])
				{
					const weekDiv = $('<div class="week-group"></div>').append('<h5>Week ' + week + '</h5>');
					groupedDates[monthYear][week].forEach(function (dateObj)
					{
						const dateDiv = $('<div class="date-div"></div>').text(dateObj.date);
						if (dateObj.isGreen)
						{
							dateDiv.css('background-color', 'green');
						}
						weekDiv.append(dateDiv);
					});
					monthDiv.append(weekDiv);
				}
				$('#selected-dates').append(monthDiv);
			}
		});

			initializeAutocomplete($('.autocomplete-input'));

			// Preload dates
			const preloadedDates = JSON.parse($('#<%=hiddenDeployedDates.ClientID%>').val());
			const dateObjects = preloadedDates.map(dateString => new Date(dateString));
			$('#datepicker').datepicker('setDates', dateObjects);
			displayDates(dateObjects);


			//// Add new input field on button click
			//$('#add-another').on('click', function () {
			//	// Clone the existing autocomplete-wrapper div
			//	var newAutocompleteWrapper = $('.autocomplete-wrapper').first().clone();

			//	// Clear the input value in the cloned element
			//	newAutocompleteWrapper.find('.autocomplete-input').val('');

			//	// Append the new div to the container
			//	$('#container').append(newAutocompleteWrapper);

			//	// Initialize Autocomplete for the new input element
			//	initializeAutocomplete(newAutocompleteWrapper.find('.autocomplete-input'));
			//});
		});

		function initializeAutocomplete(element) {
			$(element).autocomplete({
				source: function (request, response) {
					$.ajax({
						url: "/V1/Handlers/PositionAutoComplete.ashx",
						type: "GET",
						dataType: "json",
						data: {
							method: "GetSuggestions",
							term: request.term,
							userId: userId
						},
						success: function (data) {
							try {
								//var jsonData = JSON.parse(data);  // Ensure it's valid JSON
								////console.log(jsonData);
								//alert(jsonData);
								//response(data);

								var filteredData = $.grep(data, function (item) {
									return $.inArray(item, selectedItems) === -1;
								});
								response(filteredData);



							} catch (e) {
								console.error("JSON parsing STABILITY error:", e);
							}
						},
						error: function (xhr, status, error) {
							console.error("Autocomplete error: ", error);
						}
					});
				},
				minLength: 1,
				delay: 300,
				select: function (event, ui) {

					// Check if the selected item is already in selectedItems
					//if ($.inArray(ui.item.value, selectedItems) !== -1) {
					//	// Find the nearest #positionDetails container and show the duplicate message
					//	var positionDetailsContainer = $(this).closest('#positionDetails');
					//	var newPositionLabel = positionDetailsContainer.find('#newPositionLabel');

					//	newPositionLabel.text("You've already added this position00000.");
					//	newPositionLabel.show();

					//	$(this).val(''); // Clear the input
					//	return false; // Prevent adding the duplicate
					//}

					// Add the selected item to the global selectedItems array
					selectedItems.push(ui.item.value);

					//// Display a message indicating a new position has been added
					//var positionDetailsContainer = $(this).closest('#positionDetails');
					//var newPositionLabel = positionDetailsContainer.find('#newPositionLabel');
					//newPositionLabel.text("Position added.");
					//newPositionLabel.show();

					//return false;
				}
			});

			// Add custom item when user types something that is not in the autocomplete
			$(element).on('blur', function () {
				var currentInputValue = $(this).val().trim();

				// Check if the value is not empty and not already in selectedItems or customItems
				if (currentInputValue !== "" &&
					$.inArray(currentInputValue, selectedItems) === -1 &&
					$.inArray(currentInputValue, customItems) === -1) {

					// Add it to customItems array
					customItems.push(currentInputValue);

					// Optionally, add it to selectedItems to prevent duplicates in autocomplete
					selectedItems.push(currentInputValue);

					// Find the nearest #positionDetails container and show the label
					var positionDetailsContainer = $(this).closest('#positionDetails');
					var newPositionLabel = positionDetailsContainer.find('#newPositionLabel');

					// Display the label if it was hidden
					newPositionLabel.html("<i class=\"fa fa-exclamation-circle\"></i> " + currentInputValue + " is a new position, it will be added to your database.");
					newPositionLabel.show();

					console.log("New item added: " + currentInputValue);
					console.log("Custom items: ", customItems);
				}
			});


			// Add custom item when user types something new
			//$(element).on('autocompletechange', function (event, ui) {
			//	var currentInputValue = $(this).val().trim();

			//	// Check if the value is not empty and not already in selectedItems
			//	if (currentInputValue !== "" && $.inArray(currentInputValue, selectedItems) === -1) {

			//		// Add it to customItems and selectedItems array
			//		customItems.push(currentInputValue);
			//		selectedItems.push(currentInputValue);

			//		// Clear the input field
			//		//$(this).val('');

			//		// Display the label for a new position
			//		var positionDetailsContainer = $(this).closest('#positionDetails');
			//		var newPositionLabel = positionDetailsContainer.find('#newPositionLabel');

			//		newPositionLabel.text("This is a new position that will be added to your database.");
			//		newPositionLabel.show();

			//		console.log("New custom item added: " + currentInputValue);
			//		console.log("Custom items: ", customItems);

			//	} else if (currentInputValue !== "") {
			//		// Show a message that this position is already added
			//		var positionDetailsContainer = $(this).closest('#positionDetails');
			//		var newPositionLabel = positionDetailsContainer.find('#newPositionLabel');

			//		newPositionLabel.text("You've already added this position.");
			//		newPositionLabel.show();

			//		$(this).val(''); // Clear the input
			//		return; // Prevent further action
			//	}
			//});
		}

		//BEGIN DATE FUNCTIONS
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
    </script>
	<style>
		.date-div {
			background-color: #B490DA; /* Bootstrap warning color */
			color: white;
			padding: 7px;
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
			font-size: 2.0rem; /* Increase the font size of the entire datepicker */
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
		.hbuilt
		{border:none !important;}
		.panel-heading
		{font-size:18px;}
		</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<uc1:PostionNavigation runat="server" ID="ucPostionNavigation" />
	<div class="child-master-container">
		<div class="row">
			<div class="col-xs-12">
				<h3><asp:Literal ID="litEventName" runat="server"></asp:Literal></h3>
				<asp:HyperLink ID="btnPreviewPositions" Target="_blank" runat="server" Text="Preview Positions" CssClass="btn btn-primary2 pull-right previewPositions"></asp:HyperLink>
				<h4>Add Postions</h4>
				Build your deployment team. Enter the positions you think you will need to help you operate a successful deployment. Once your positions are created, your team will be able to easily claim an open position.
				<br /><br />
				To start, enter a position name like "Deployment Team Lead" or "Cook". Click 'Preview Positions' at any time to view your team, this will open a new window so it will not interrupt what you are building on this page.
				<hr />
			</div>
		</div>

		<div class="hpanel position-information positionPanel">
			<div class="panel-heading">
				<span class="circle">1</span> Enter Positions Needed
			</div>
			<div class="panel-body">
				<span class="text-muted">Add position, number of persons and arrival times. Select <span class="font-bold">' + Add Another'</span> to add more positions. New positions will be added to the database.</span>
				<div class="position-details" id="positionDetails">
					<i class="fa fa-remove delete-icon"></i>
					<div class="row">
						<div class="col-xs-12 col-sm-1" style="min-width:300px !important;">
							<div class="input-group m-b" id="container">
								<span class="input-group-addon">Postion: </span>
								<div class="autocomplete-wrapper">
									<input type="text" name="position[]" required class="form-control input-position-name autocomplete-input" style="width:225px !important;" placeholder="EX:Cook, Deliver Supplies, Chainsaw">
								</div>
							</div>
						</div>
						<div class="col-xs-12 col-sm-1">
							<div class="input-group m-b">
								<span class="input-group-addon"># Needed:</span>
								<input type="text" name="positionCount[]" required maxlength="2" class="form-control input-positions-needed" value="1" style="width:60px !important;">
							</div>
						</div>
					</div>
					<div class="row">
						<div class="col-xs-6 col-sm-1" style="min-width:178px !important;max-width:178px !important;">
							<div class="form-group">
								<div class="input-group clockpicker arrival-time" data-autoclose="true">
									<span class="input-group-addon">
										<span class="fa fa-clock-o"></span> Arrive:
									</span>
									<input type="text" name="arrivalTime[]" class="form-control" value="09:00AM" style="width:95px !important;">
								</div>
							</div>
						</div>
						<div class="col-xs-6 col-sm-1">
							<div class="form-group">
								<div class="input-group clockpicker departure-time" data-autoclose="true">
									<span class="input-group-addon">
										<span class="fa fa-clock-o"></span> Leave:
									</span>
									<input type="text" name="leaveTime[]" class="form-control" value="03:00PM" style="width:95px !important;">
								</div>
							</div>
						</div>
					</div>
					<div class="row">
						<div class="col-xs-12">
							<div class="form-group">
								<div class="checkbox checkbox-info" style="margin-top:0px !important; margin-left:20px !important;">
									<input id="checkbox4" name="isRemote[]" value="1" type="checkbox" style="width:20px !important;">
									<input type="hidden" name="isRemote[]" value="0">
									<label for="checkbox4" style="padding:0px 0px 0px 5px !important; font-size:16px;">
										This is a remote position.
									</label>
								</div>
								<!-- Label for new positions -->
								<div id="newPositionLabel" style="display:none;" class="text-info">
								</div>
							</div>	
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="row" style="margin-bottom:40px;">
			<div class="col-xs-12">
				<button type="button" class="btn btn-primary pull-right" id="duplicateButton"> + Add Another Position Form</button>
			</div>
		</div>

		<div class="hpanel position-calendar positionPanel">
			<div class="panel-heading">
				<span class="circle pull-left">2</span> What days do you need positions filled?
				<div class="text-muted" style="font-size:14px; font-weight:normal;">If you're unsure, dates can be selected later.</div>
			</div>
			
			<div class="panel-body">

				<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="row">
						<div class="col-lg-12">
							<div class="hpanel form-horizontal">
								<div class="panel-body p-lg">
									<div class="form-group">
										<div class="row">
											<div class="col-xs-12 col-md-4">
												<div id="datepicker"></div>
												<asp:HiddenField ID="hiddenDeployedDates" runat="server" />

											</div>
											<div class="col-xs-12 col-md-8">
												<div id="selected-dates" class="d-flex flex-wrap mt-3"></div>
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

		<div class="hpanel position-message positionPanel">
			<div class="panel-heading">
				<span class="circle">3</span> Enter any other details
			</div>
			<div class="panel-body">
				<div class="form-group">
					<div class="col-sm-12">
						<span class="text-muted">Enter extra details such as job descriptions, instructions, links to maps or videos.
							<br />Details here will be automatically included in confirmation and reminder messages.</span>
							<asp:TextBox ID="txtMessage" CssClass="form-control" runat="server" TextMode="MultiLine" Rows="10" ClientIDMode="Static"></asp:TextBox>
									
					</div>
				</div>
			</div>
		</div>
		
		<div class="form-group">
			<div class="col-sm-2" style="padding:10px;">
				<div class="pull-left">
					<asp:LinkButton id="btnCancel" CausesValidation="false" runat="server" OnClick="btnSubmit_Cancel" CssClass="btn btn-default m-r-sm" Text="Cancel" />
				</div>
			</div>
			<div class="col-sm-8" style=""></div>
			<div class="col-sm-2" style="padding:10px; padding-right:20px;">
				<div class="pull-right">
					<asp:Button id="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-primary" Text="Add Positions" />
				</div>
			</div>
		</div>
	</div>
	<script src="https://code.jquery.com/ui/1.14.0/jquery-ui.js"></script>
	<script src="/Homer/vendor/bootstrap-datepicker-master/dist/js/bootstrap-datepicker.min.js"></script>
	<script src="/Homer/vendor/clockpicker/dist/bootstrap-clockpicker.min.js"></script>
	<script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
</asp:Content>