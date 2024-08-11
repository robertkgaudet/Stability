<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_Profile_Rebuild, App_Web_oetfxeqz" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script src="/V1/Homer/vendor/jquery-ui/jquery-ui.min.js"></script>
	<script src="/V1/Homer/vendor/iCheck/icheck.min.js"></script>
	<script src="/V1/Homer/vendor/chartjs/Chart.min.js"></script>
	<script src="/V1/Homer/vendor/sparkline/index.js"></script>
	<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDQTpXj82d8UpCi97wzo_nKXL7nYrd4G70"></script>

	<script type="text/javascript">

		$(document).ready(function () {

            $('.btnUpdateRebuildProgress').click(function () {
                window.location.href ='RebuildProgressUpdates.aspx?&survivorId=<%=survivorId%>&rebuildId=<%=rebuildId%>';
                return false;
            })

            $('.btnUploadPhotos').click(function () {
                window.location.href = 'UploadRebuildPhoto.aspx?rebuildId=<%=rebuildId%>&eventId=<%=eventId%>';
				return false;
			})

			// When the window has finished loading google map
<%--			google.maps.event.addDomListener(window, 'load', init);

			function init() {
				// Options for Google map
				// More info see: https://developers.google.com/maps/documentation/javascript/reference#MapOptions
				var mapOptions1 = {
					zoom: 10,
					center: new google.maps.LatLng(<%=latidude%>, <%=longitude%>),
					// Style for Google Maps
				};

				// Get all html elements for map
				var mapElement1 = document.getElementById('map1');

				// Create the Google Map using elements
				var map1 = new google.maps.Map(mapElement1, mapOptions1);

				var image = '/Images/Icon/MapIcon-Red.png';
				var beachMarker = new google.maps.Marker({
					position: {lat: <%=latidude%>, lng: <%=longitude%>},
					map: map1,
					icon: image
				});
			}--%>
        });
    </script>
	
	<style>

		#base
		{
			background: #555555;
			display: inline-block;
			height: 10px;
			margin-left: 1px;
			margin-top: 10px;
			position: relative;
			width: 15px;
		}
		#base:before
		{
		  border-bottom: 8px solid #555555;
		  border-left: 8px solid transparent;
		  border-right: 7px solid transparent;
		  content: "";
		  height: 0;
		  left: 0;
		  position: absolute;
		  top: -8px;
		  width: 0;
		}
		

		.post-logo
		{
			width:50px;
			float:left;
		}
		
		.StatusClassIceBlue {
			width: 7px;
			height: 7px;
			background: #5BC0DE;
			-moz-border-radius: 7px;
			-webkit-border-radius: 7px;
			border-radius: 7px;
		}
		.StatusClassOnDeckYellow {
			width: 7px;
			height: 7px;
			background: #F0AD4E;
			-moz-border-radius: 7px;
			-webkit-border-radius: 7px;
			border-radius: 7px;
		}
		.StatusClassActiveGreen {
			width: 7px;
			height: 7px;
			background: #5CB85C;
			-moz-border-radius: 7px;
			-webkit-border-radius: 7px;
			border-radius: 7px;
		}
		.StatusClassComplete {
			width: 7px;
			height: 7px;
			background: #428BCA;
			-moz-border-radius: 7px;
			-webkit-border-radius: 7px;
			border-radius: 7px;
		}
		.btnUpdateRebuildProgress:hover	{cursor:pointer; }
		.RebuildLink
		{
			color:#365899;
		}
		.RebuildLink:hover
		{
			color:#365899;
			text-decoration:underline;
		}
		
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		
		<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="hpanel <%=headerColor%>">
				<div class="panel-body">
					<a class="small-header-action">
						<div class="clip-header">
							<i class="fa fa-arrow-up"></i>
						</div>
					</a>
				</div>
			</div>
		</div>

		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-4">
					<div class="hpanel <%=headerColor%>">
						<div class="panel-body">
							<div class="row">
								<div class="col-sm-6">
									<h4><asp:Literal ID="litSurvivorNameProfile" runat="server"></asp:Literal></h4>
									<div class="text-muted m-b-xs">
										<asp:Literal ID="litAddress" runat="server"></asp:Literal>
										<br />
										<asp:Literal ID="litCityStatZip" runat="server"></asp:Literal>
										<br />
										<asp:Literal ID="litPhoneNumber" runat="server"></asp:Literal>
										<br />
										<asp:Literal ID="litEmail" runat="server"></asp:Literal>
									</div>
									<dl class="dl-horizontal">
										<dt>Beds:</dt> <dd><asp:Literal ID="litBeds" runat="server"></asp:Literal></dd>
										<dt>Baths:</dt> <dd><asp:Literal ID="litBaths" runat="server"></asp:Literal></dd>
										<dt>Square Feet:</dt> <dd><asp:Literal ID="litSquareFeet" runat="server"></asp:Literal></dd>
									</dl>	
								</div>
								<div class="col-sm-6 text-center">
									<div runat="server" id="divListItems" visible="false">
										<h5 class="m-b-n">Track Contents <span class="badge"><asp:Literal ID="litItemPhotoCount" runat="server"></asp:Literal></span></h5>
										<button class="btn btn-<%=color%> btn-md btnUploadPhotos center-block m-t-lg">Add Item</button>
									</div>
								</div>
							</div>
						</div>
					</div>
					<div class="hpanel <%=headerColor%>">
						<div class="panel-body">
							<div class="row">
								<div class="col-sm-8">
									<div id="divOverallProgressBar" runat="server">
										<strong>Overall Progress</strong> <%=overallProgressPercent%>% Complete
										<div class="progress m-t-xs full progress-small">
											<div style="width:<%=overallProgressPercent%>%" aria-valuemax="100" aria-valuemin="0" role="progressbar" class="progress-bar progress-bar-info">
												<span class="sr-only">10% Complete (success)</span>
											</div>
										</div>
									</div>
									<div id="divProgressBar" runat="server" class="m-b-lg">
										<h3><asp:Literal ID="litCurrentProgressLabel" runat="server"></asp:Literal></h3>
										<strong>Rebuild Progress</strong> <%=rebuildProgressPercent%>% Complete
										<div class="progress m-t-xs full progress-small">
											<div style="width:<%=rebuildProgressPercent%>%" aria-valuemax="100" aria-valuemin="0" role="progressbar" class="progress-bar progress-bar-success">
												<span class="sr-only">10% Complete (success)</span>
											</div>
										</div>
									</div>
									<div class="m-t-lg">
										<dl>
											<dt>Description of Work</dt>
											<dd>
												<asp:Literal ID="litDescription" runat="server"></asp:Literal>
											</dd>
										</dl>
									</div>
									<div class="alert alert-danger" id="divNoGo" runat="server">
										Project is on hold.
									</div>
								</div>
								<div class="col-sm-4">
									Difficulty
									<div class="border-top m-b-lg">
										<asp:Literal ID="litDifficulty" runat="server"></asp:Literal>
									</div>
									<div id="divQualifiers" runat="server" visible="false">
										Qualifiers
										<div class="border-top">
											<br />
											<asp:Repeater ID="rpQualifiers" runat="server">
												<HeaderTemplate><ul style="margin-left:-10px;"></HeaderTemplate>
												<ItemTemplate>
													<li style="margin-left:-10px;"><%# DataBinder.Eval(Container.DataItem, "Status") %></li>
												</ItemTemplate>
												<FooterTemplate></ul></FooterTemplate>
											</asp:Repeater>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>

					<div class="hpanel <%=headerColor%>" id="divTasks" runat="server" visible="false">
						<div class="panel-heading hbuilt">
							Tasks
						</div>
						<div class="panel-body no-padding">
							<ul class="list-group">
								<li class="list-group-item">
									<a href="EditRebuild.aspx">Edit Rebuild Details</a>
								</li>
								<li class="list-group-item">
									<asp:HyperLink ID="hypRebuildProgressUpdates" runat="server" Text="Update Progress"></asp:HyperLink>
								</li>
								<li class="list-group-item">
									<span class="badge badge-primary">16</span>
									Household Items needed for CrowdBuying
								</li>
								<li style="display:none;" class="list-group-item">
									<span class="badge badge-success">10</span>
									List/Photo of Household Items
								</li>
								<li class="list-group-item">
									<span class="badge badge-primary">16</span>
									Work tasks
								</li>
								<li style="display:none;" class="list-group-item">
									<span class="badge badge-success">10</span>
									Case Notes
								</li>
								<li class="list-group-item ">
									<span class="badge badge-info">12</span>
									Collaborators
								</li>
								<li class="list-group-item">
									<span class="badge badge-danger">10</span>
									Photos
								</li>
							</ul>
						</div>
					</div>
				</div>
				<div class="col-lg-8">
					<div class="hpanel">
						<div class="row">
							<div class="col-md-6">
								<div class="hpanel">
									<center>
										<h1 class="m-lg center-block font-extra-bold">
											<asp:Literal ID="litCurrentStage" runat="server"></asp:Literal>
										</h1>
									</center>
									<div class="m-b-lg alert alert-danger">
										<i class="fa fa-bolt"></i> <b>Take Action:</b> <asp:HyperLink ID="hypTrackItems" runat="server" CssClass="RebuildLink" Text="Track my home contents with photos now."></asp:HyperLink> Before the storm hits.
									</div>
									<asp:Literal ID="litStageDescription" runat="server"></asp:Literal>
									<small><asp:HyperLink ID="hypReviewStages" runat="server" Text="Review Recovery Stages" NavigateUrl="~/V1/Stages.aspx"></asp:HyperLink></small>
									<button id="btnUpdateProgress" visible="false" runat="server">Update My Current	Stage</button>
								</div>	
								<div class="hpanel hbgblue m-t-lg">
									<div class="panel-body">
										<div class="text-center">
											<h3>Overall Progress</h3>
											<p class="text-big font-bold">
												<%=overallProgressPercent%>%
											</p>
										</div>
									</div>
								</div>
								<div class="hpanel hbggreen">
									<div class="panel-body">
										<div class="text-center">
											<h3>Rebuild Progress</h3>
											<p class="text-big font-bold">
												<%=rebuildProgressPercent%>%
											</p>
											<p class="text-md font-light font-extra-bold">
												<%=rebuildTickLabel%>
											</p>
										</div>
									</div>
								</div>
								<div class="hpanel hbgyellow">
									<div class="panel-body">
										<div class="text-center">
											<h3>Volunteers Needed</h3>
											<p class="text-big font-bold">
												<asp:Literal ID="litVolunteers" runat="server"></asp:Literal>
											</p>
											<div>
											<small>
												<asp:Literal ID="litVolunteersDifficulty" runat="server"></asp:Literal>
											</small>
											</div>
										</div>
									</div>
								</div>
							</div>
							<div class="col-md-6">
								<div class="hpanel <%=headerColor%>">
									<div class="panel-body disasterPanel">
										<div class="text-center">
											<h2 class="m-b-xs">
												<asp:Literal ID="litEventName" runat="server"></asp:Literal>
											</h2>
											<p class="font-bold text-info">
												<asp:Literal ID="litDate" runat="server"></asp:Literal>
											</p>
											<div class="m">
												<%=icon%>
											</div>
											<p class="small p-sm">
												<asp:Literal ID="litEventDescription" runat="server"></asp:Literal>
											</p>
										</div>
									</div>
								</div>
								<div id="divLocationMap" runat="server" visible="false" class="hpanel <%=headerColor%>">
									<div class="panel-body">
										<dl>
											<dt>Location Map</dt>
											<dd>
												<section id="map">
													<div id="map1" style="height: 200px"></div>
												</section>
											</dd>
										</dl>
									</div>
								</div>
							</div>
						</div>
						<ul class="nav nav-tabs">
							<li class="active"><a data-toggle="tab" href="#tab-1">Posts</a></li>
						</ul>
						<div class="tab-content">
							<div id="tab-1" class="tab-pane active">
								<div class="panel-body no-padding">
									<div class="chat-discussion" style="height: auto">
										<div class="chat-message">
											<asp:Image ID="imgNewPostLogo" runat="server" CssClass="post-logo" />
											<div class="message">
												<a class="message-author" href="#">  </a>
												<span class="message-date"> Mon Jan 26 2015 - 18:39:23 </span>
												<span class="message-content">
													<asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtPost" runat="server"></asp:TextBox>
												</span>
												<div style="overflow:auto;">
													<asp:Button ID="btnRebuildPost" OnClick="btnRebuildPost_Click" CssClass="btn w-xs btn-sm btn-primary pull-right m-t-sm" runat="server" Text="Post" />
												</div>
											</div>
										</div>
										<asp:Repeater ID="rptPosts" runat="server">
											<ItemTemplate>
												<div class="hpanel">
													<div class="panel-body">
														<div class="message">
															<div class="blog-article-box">
															<%# DataBinder.Eval(Container.DataItem, "fullname") %>
															</div>
															<span class="message-date"> <%# DataBinder.Eval(Container.DataItem, "createdon", "{0:M/d/yyyy HH:mm:ss}") %> </span>
															<span class="message-content">
															<%# DataBinder.Eval(Container.DataItem, "Post") %>
															</span>
														</div>
													</div>
												</div>
											</ItemTemplate>
										</asp:Repeater>
									</div>
								</div>
							</div>
							<div id="tab-2" class="tab-pane">
								<div class="panel-body">
									<strong>Lorem ipsum dolor sit amet, consectetuer adipiscing</strong>

									<p>A wonderful serenity has taken possession of my entire soul, like these sweet mornings of spring which I enjoy with my whole heart. I am alone, and feel the charm of
										existence in this spot, which was created for the bliss of souls like mine.</p>

									<div class="table-responsive">
										
									</div>
								</div>
							</div>
						</div>


					</div>
				</div>
			</div>
			<div class="row" runat="server" visible="false">
				<div class="col-lg-6">
					<div class="hpanel">
						<div class="panel-heading">
							Etta's Rebuild Progress
						</div>
						<div class="panel-body">
							<div>
								<canvas id="lineOptionsRebuild" height="140"></canvas>
							</div>
						</div>
					</div>
				</div>
				<div class="col-lg-6">
					<div class="hpanel">
						<div class="panel-heading">
							Overall Progress
						</div>
						<div class="panel-body">
							<div>
								<canvas id="lineOptionsOverall" height="140"></canvas>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>

