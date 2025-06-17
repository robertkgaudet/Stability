<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" Async="true" AutoEventWireup="true" CodeFile="CommunityLandingPage.aspx.cs" Inherits="V1_Profile_CommunityLandingPage" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/1-Column-Child.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<%--    <style>
        .section { margin-bottom: 20px; }
        .weather-box, .narrative-box { border: 1px solid #ddd; padding: 15px; background-color: #fff; border-radius: 5px; }
        .weather-label { font-size: 18px; font-weight: bold; }
        .narrative-text { font-size: 15px; color: #333; margin-top: 10px; white-space: pre-wrap; }
    </style>--%>

<style type="text/css">	

	.panel-body.member-panel-body {
		padding: 0px !important; /* or whatever value you want */
		margin: 0px !important; /* or whatever value you want */
		color: #4a4a4a; !important /* Ensures font color overrides default ASP.NET theme */
	}
	.panel-body {
		color: #4a4a4a !important; /* Ensures font color overrides default ASP.NET theme */
		font-size: 12px !important;
	}
	.civic-feed-wrapper {
		background: linear-gradient(to bottom, #e7fbee, #d5c7f1);
		padding: 60px 0px;
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 16px;
	}


	.card {
		background: #f2f2f2; /* soft plum or replace with #3F76E0 for blue weather card */
		border-radius: 16px;
		padding: 20px 20px 50px 20px;
		margin-bottom:50px;
		box-shadow: 0 4px 10px rgba(0, 0, 0, 0.05);
		width: calc(100% - 40px);
		max-width: 480px;
		box-sizing: border-box;
		color: #4a4a4a; /* Ensures font color overrides default ASP.NET theme */
		font-size: 18px; /* Global base size for card content */
		line-height: 1.6;
	}
	
	.modal-body
	{
		font-size:13px;
	}
	.modal-footer
	{
		font-size:13px;
	}
	.modal-title
	{
		font-size:14px;
		color: #4a4a4a; /* Ensures font color overrides default ASP.NET theme */
	}

	.list-group-item-action, .list-group-item
	{
		font-size:13px !important;
		line-height: 1.6 !important;
	}

	.card.weather-card {
		background-color: #3F76E0;
		color: white;
		text-align: center; /* ⬅️ Center all content */
	}

	.weather-temp {
		font-size: 48px;     /* ⬅️ Bigger temperature */
		font-weight: bold;
		line-height: 1.2;
		margin-top: 8px;
		margin-bottom: 8px;
		display: block;      /* Ensures it's on its own line */
	}

	.weather-icon {
		font-size: 40px;
		display: block;
		margin-bottom: 4px;
	}
	.forecast-row {
		display: flex;
		justify-content: space-between;
		flex-wrap: nowrap;
		gap: 12px;
		width: 100%;
		margin-top: 20px;
		overflow: hidden; /* disables scroll */
	}


	.forecast-item {
		flex: 1 1 0;
		min-width: 0;
		text-align: center;
		font-size: 14px;
		color: white;
		padding: 4px;
	}


	.forecast-icon {
		font-size: 24px;
	}

	.forecast-day {
		font-weight: bold;
		margin-top: 4px;
	}

	.forecast-temp {
		margin-top: 2px;
		font-size: 16px;
	}

	.forecast-text {
		font-size: 12px;
		opacity: 0.9;
	}



	.label-title {
		font-weight: bold;
		font-size: 18px;
		margin-bottom: 6px;
		color: inherit; /* Inherit from card’s text color */
	}

	.label-value {
		font-size: 20px;
		font-weight: 500;
		color: inherit;
	}
	
	.label-value-demographics
	{
		line-height: 1;
		font-size: 40px;
		font-weight: 700;
		color: inherit;
	}
	.label-title-demographics
	{
		line-height: 1.6;
		font-size: 15px;
		font-weight: 500;
		color: inherit;
	}

	.label-value-hello
	{
		line-height: 1;
		font-size: 35px;
		font-weight: 900;
		color: inherit;
	}

	.narrative-textbox {
		font-size: 18px;
	}

	.cta-button-row {
		display: flex;
		justify-content: center;
		gap: 20px;
		margin-top: 24px;
		flex-wrap: wrap;
	}

	.cta-button {
		background-color: #f5e3b3; /* warm gold */
		color: #4a4a4a;
		text-decoration: none;
		border: none;
		border-radius: 24px;
		padding: 12px 24px;
		font-size: 16px;
		font-weight: 600;
		cursor: pointer;
		transition: background-color 0.2s ease;
		box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
		display: inline-block;
		text-align: center;
		min-width: 140px;
	}

	.cta-button:hover {
		background-color: #e0d0a4;
	}


	/* Adjust card width on very small devices */
	@media (min-width: 768px) {
		.card {
			margin-left: auto;
			margin-right: auto;
		}
	}
	@media (max-width: 480px) {
    .forecast-item {
        font-size: 12px;
		}
	}
	.panel-warning > .panel-heading {
        background-color: #e2b007;
        color: white;
    }

</style>
	<script type="text/javascript">
	function toggleDetails(link) {
		var details = link.parentElement.nextElementSibling;
		var visible = details.style.display === "block";
		details.style.display = visible ? "none" : "block";
		link.innerText = visible ? "Show More" : "Hide Details";
	}
	</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

		<div class="civic-feed-wrapper">
				
			<asp:Label CssClass="label-value-hello" ID="lblHello" runat="server" />
			<small>
				<asp:Label ID="lblCity" CssClass="label-value" runat="server" />
			</small>
			<div class="card">
				<asp:Label ID="txtNarrative" CssClass="narrative-textbox" runat="server" /> 
			</div>

			<div class="card weather-card">
				<asp:Label ID="lblWeatherIcon" runat="server" CssClass="weather-icon" />
				<div class="label-title">Temperature</div>
				<asp:Label ID="lblTemp" CssClass="weather-temp" runat="server" />
				<div class="forecast-row">
					<asp:Repeater ID="rptForecast" runat="server">
						<ItemTemplate>
							<div class="forecast-item">
								<div class="forecast-icon"><%# Eval("Icon") %></div>
								<div class="forecast-day"><%# Eval("Day") %></div>
								<div class="forecast-temp"><%# Eval("Temp") %>°</div>
								<div class="forecast-text"><%# Eval("Condition") %></div>
							</div>
						</ItemTemplate>
					</asp:Repeater>
				</div>
			</div>

			<div class="card">
				<asp:Repeater ID="rptDisasterCards" runat="server">
					<ItemTemplate>
						<div class='panel <%# GetCardCssClass(Eval("Severity").ToString(), Eval("Urgency").ToString()) %>' style='margin-bottom: 15px;'>
							<div class="panel-heading">
								<strong><%# Eval("Event") %> - <%# Eval("Severity") %></strong><br />
								<small><%# Eval("Headline") %></small>
							</div>
							<div class="panel-body">
								<p><b>Description:</b> <%# Eval("Description") %></p>
								<p><b>What You Can Do:</b><br /><%# Eval("GptMessage") %></p>
								<p><b>Affected Area:</b> <%# Eval("AreaDesc") %></p>
								<p>
									<a href='javascript:void(0)' onclick='toggleDetails(this)' style='text-decoration:underline;'>Show More</a>
								</p>
								<div class='alert-details' style='display:none;'>
									<p><b>Sender:</b> <%# Eval("SenderName") %></p>
									<p><b>Status:</b> <%# Eval("Status") %> | <b>Type:</b> <%# Eval("MessageType") %></p>
									<p><b>Certainty:</b> <%# Eval("Certainty") %> | <b>Urgency:</b> <%# Eval("Urgency") %> | <b>Category:</b> <%# Eval("Category") %></p>
									<p><b>Issued:</b> <%# ((DateTime)Eval("Sent")).ToString("MMM dd, h:mm tt") %><br />
									   <b>Effective:</b> <%# ((DateTime)Eval("Sent")).ToString("MMM dd, h:mm tt") %><br />
									   <b>Expires:</b> <%# ((DateTime)Eval("Expires")).ToString("MMM dd, h:mm tt") %><br />
									   <b>Ends:</b> <%# ((DateTime)Eval("Ends")).ToString("MMM dd, h:mm tt") %></p>
								</div>
								<a href='<%# Eval("Url") %>' target='_blank' class='btn btn-xs btn-warning'>View NWS Official Alert</a>
							</div>
						</div>
					</ItemTemplate>
				</asp:Repeater>

			</div>
			

			<div class="card">
				<!-- List container -->
				<div class="list-group weather-advisory-list">
					<asp:Repeater ID="rptDisasterEvents" runat="server">
						<ItemTemplate>
							<!-- List item -->
							<a href="#" class="list-group-item list-group-item-action" data-toggle="modal" data-target='<%# "#modal_" + Eval("Title").ToString().GetHashCode() %>'>
								<strong><%# Eval("Title") %></strong><br />
								<small><b>Affected:</b> <%# Eval("Location") %></small>
								<span class="pull-right text-muted"><%# Eval("Date", "{0:MMM dd, yyyy}") %></span>
							</a>

							<!-- Modal per item -->
							<div class="modal fade" id='<%# "modal_" + Eval("Title").ToString().GetHashCode() %>' tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
								<div class="modal-dialog" role="document">
									<div class="modal-content">

										<div class="modal-header bg-warning text-white">
											<h5 class="modal-title"><%# Eval("Title") %></h5>
											<button type="button" class="close" data-dismiss="modal" aria-label="Close">
												<span aria-hidden="true">&times;</span>
											</button>
										</div>

										<div class="modal-body">
											<p><%# Eval("Summary") %></p>
											<p><b>Source:</b> <%# Eval("Source") %><br />
											   <b>Affected Area:</b> <%# Eval("Location") %><br />
											   <b>Date:</b> <%# Eval("Date", "{0:MMM dd, yyyy}") %></p>
										</div>

										<div class="modal-footer">
											<a href='<%# Eval("Url") %>' target="_blank" class="btn btn-warning">More Info</a>
											<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
										</div>

									</div>
								</div>
							</div>
						</ItemTemplate>
					</asp:Repeater>
				</div>

				</div>


			<div class="card demographics-card">
				<H5>Local Senior Snapshot: <asp:Literal ID="lblZip" runat="server"></asp:Literal> </H5>
				<div class="label-title-demographics">Residents 60+</div>
				<asp:Label ID="lblElderlyCount" CssClass="label-value-demographics" runat="server" />

				<div class="label-title-demographics">Total Population</div>
				<asp:Label ID="lblTotalPop" CssClass="label-value-demographics" runat="server" />

				<div class="label-title-demographics">Percent Elderly</div>
				<asp:Label ID="lblElderlyPct" CssClass="label-value-demographics" runat="server" />
			</div>
			<div class="cta-button-row">
				<a href="/V1/Member/Default.aspx" class="cta-button">👤 View Profile</a>
				<asp:HyperLink ID="hypTeam" runat="server" CssClass="cta-button" Text="🤝 Visit Team"></asp:HyperLink>
			</div>
		</div>

</asp:Content>