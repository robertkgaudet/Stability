<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="RebuildProgressUpdates.aspx.cs" Inherits="V1_Profile_RebuildProgressUpdates" %>

<asp:Content ID="Content1" ContentPlaceholderID="head" Runat="Server">
	<script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-slider/9.8.0/bootstrap-slider.min.js"></script>
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-slider/9.8.0/css/bootstrap-slider.min.css" />
	<script type="text/javascript">
		$(document).ready(function ()
		{
			<%=javascriptSliderCode%>

			function updateRebuildProgressSlider(tickValue, sliderId, rebuildId, survivorId, userId) {
				//sending commentId will cause a delete.

				$.ajax(
				{
					type: "GET",
					url: "/V1/Handlers/RebuildProgressSliderUpdate.ashx",
					data: "tickValue=" + tickValue + "&sliderId=" + sliderId + "&rebuildId=" + rebuildId + "&survivorId=" + survivorId + "&userId=" + userId,
					contentType: "text/plain; charset=utf-8",
					dataType: "html",
					success: function (data) {
						if (data != "") {
						}
					},
					error: function (request, status, error) {
						request.statusText + ' - ' + error + ' - ' + status;
					}
				});
			}

		});
	</script>

	<style>
		.slider.slider-horizontal
		{
			width:100%;
			font-size:smaller;
		}
	</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceholderID="ContentPlaceHolder1" Runat="Server">
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="hpanel">
				<div class="panel-body">
					<h2 class="font-light m-b-xs">
						Update My Rebuild/Recovery
					</h2>
					<small>Use the tools on this page to update information about your recovery progress.</small>
				
					<div class="m-t-lg pull-right">
						<asp:HyperLink ID="hypRebuildProgressUpdates" CssClass="btn w-xs btn-info" runat="server" Text="Update Complete"></asp:HyperLink>
					</div>
				</div>
			</div>

			<div class="hpanel hblue">
				<div class="panel-body">
					<div class="center-block p-l m-sm m-b-xl">
						<div class="m-b-lg">
							<h3>Recovery Stage</h3>
							Select the stage of recovery you are currently in.
						</div>
						<input type="text" data-slider-id="GC" id="RecoveryStage" class="RecoveryStage" />
					</div>
				</div>
			</div>

			<div class="hpanel hblue">
				<div class="panel-body">
					<div class="center-block p-l m-sm m-b-xl">
						<div class="m-b-lg">
							<h3>Estimate Overall Recovery</h3>
							Estimate where you feel you are in the rebuild process. Update as frequently as you like.
						</div>
						<input type="text" data-slider-id="GC" id="EstimatedRebuildProgressSlider" class="EstimatedRebuildProgressSlider" />
					</div>
				</div>
			</div>

			<div class="hpanel hgreen">
				<div class="panel-body">
					<div class="center-block p-l m-md m-b-xl">
						<div class="m-b-lg">
							<h3>Rebuild Progress</h3>
							The progress slider below can help you keep track of where you are in the rebuild process. Track major progress on your home. Update the slider each time you complete a step.
						</div>
						<input type="text" id="RebuildProgressSlider" class="RebuildProgressSlider" />
					</div>
				</div>
			</div>
		</div>
</asp:Content>