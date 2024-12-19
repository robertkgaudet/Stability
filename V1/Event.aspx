<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Event.aspx.cs" Inherits="V1_Event" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<%@ Register Src="~/V1/UserControls/EventHeader.ascx" TagPrefix="uc1" TagName="EventHeader" %>
<%@ Register Src="~/V1/UserControls/DeploymentListCard.ascx" TagPrefix="uc1" TagName="DeploymentListCard" %>
<%@ Register Src="~/V1/UserControls/Links.ascx" TagPrefix="uc1" TagName="Links" %>
<%@ Register Src="~/V1/UserControls/ImpactedCommunities.ascx" TagPrefix="uc1" TagName="ImpactedCommunities" %>
<%@ Register Src="~/S1/UserControls/DisasterSurvivorStoriesByDisaster.ascx" TagPrefix="uc1" TagName="DisasterSurvivorStoriesByDisaster" %>
<%@ Register Src="~/V1/UserControls/TimeBoard.ascx" TagPrefix="uc1" TagName="TimeBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<link rel="stylesheet" href="/V1/Scripts/map-icons-master/dist/css/map-icons.css" />
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
    

	<script type="text/javascript">
        
		function updateDefaultDisaster(eventId)
		{
			$.ajax(
			{
				type: "GET",
				url: "/V1/Handlers/UpdateDefaultDisaster.ashx?userId=<%=userId%>&eventId=" + eventId,
                data: "",
                contentType: "text/plain; charset=utf-8",
                dataType: "html",
                success: function (data) {
                    if (data != "") {
                        $('#console-event').html(data);
                    }
                },
                error: function (request, status, error) {
                    request.statusText + ' - ' + error + ' - ' + status;
                }
            });
        }

		$(document).ready(function () {

			initMap("All");

			$("#ddlMapFilter.dropdown-menu li").click(function (event) {
				$("#btn-mapDropDown.ddlMapFilter").html($(this).text());
				var mapFilterType = $(this).attr('id');
				initMap(mapFilterType);
				event.preventDefault();
			});

			//Redirect to another portal using the ddl
			$("#disasterEvent.dropdown-menu li").click(function () {
				window.location.href = "/Disaster/" + $(this).attr('name');
			});




			//Update this users default portal.
			$('#<%=btnSetDefaultDisaster.ClientID%>').click(function () {
				updateDefaultDisaster('<%=eventId%>');
				$('#<%=lblDefaultEventMessage.ClientID%>').text("Your default portal has been updated.");
				$('#<%=btnSetDefaultDisaster.ClientID%>').hide();
				return false;
			});





			$('.deleteLinkArticle').click(function () {
				swal({
					title: "Are you sure?",
					text: "Your will not be able to recover this imaginary file!",
					type: "warning",
					showCancelButton: true,
					confirmButtonColor: "#DD6B55",
					confirmButtonText: "Yes, delete it",
					cancelButtonText: "No, cancel please.",
					closeOnConfirm: false,
					closeOnCancel: false
				},
					function (isConfirm) {
						if (isConfirm) {
							swal("Deleted!", "Your entry has been deleted.", "success");
						} else {
							swal("Cancelled", "Your entry is safe :)", "error");
						}
					});
			});

			$('.grid').each(function () {
				// Initialize Masonry for each grid individually
				$(this).masonry({
					itemSelector: '.grid-item',
					gutter: 20,
					columnWidth: '.grid-item',
					percentPosition: true
				});
			});



			//$('.grid').masonry({
			//	// options
			//	itemSelector: '.grid-item',
			//	gutter: 10
			//});



			// Function for collapse hpanel
			$('.showhide').on('click', function (event) {
				event.preventDefault();
				var hpanel = $(this).closest('div.hpanel');
				var icon = $(this).find('i:first');
				var body = hpanel.find('div.panel-body');
				var footer = hpanel.find('div.panel-footer');
				body.slideToggle(300);
				footer.slideToggle(200);

				// Toggle icon from up to down
				icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
				hpanel.toggleClass('').toggleClass('panel-collapse');
				setTimeout(function () {
					hpanel.resize();
					hpanel.find('[id^=map-]').resize();
				}, 50);
			});

			// Function for close hpanel
			$('.closebox').on('click', function (event) {
				event.preventDefault();
				var hpanel = $(this).closest('div.hpanel');
				hpanel.remove();
				if ($('body').hasClass('fullscreen-panel-mode')) { $('body').removeClass('fullscreen-panel-mode'); }
			});

			// Fullscreen for fullscreen hpanel
			$('.fullscreen').on('click', function () {
				var hpanel = $(this).closest('div.hpanel');
				var icon = $(this).find('i:first');
				$('body').toggleClass('fullscreen-panel-mode');
				icon.toggleClass('fa-expand').toggleClass('fa-compress');
				hpanel.toggleClass('fullscreen');
				setTimeout(function () {
					$(window).trigger('resize');
				}, 100);
			});
		});

		var map;

		function initMap(mapFilterType)
		{
			var infowindow = new google.maps.InfoWindow();

			map = new google.maps.Map(document.getElementById('map'), {
				zoom: <%=zoom%>,
				center: { lat: <%=latitude%>, lng: <%=longitude%> },
				styles: [{ "stylers": [{ "saturation": -11 }] }]
			});

			map.data.setStyle(function (feature) {
				return {
					icon: feature.getProperty('icon')
				};
			});

			var mapsURL = '<%=host%>/V1/Handlers/GetGeoJsonByDisaster.ashx?keyId=<%=eventId%>&mapFilterType=' + mapFilterType;
			
			map.data.loadGeoJson(mapsURL, null, function (features) { });

			switch (mapFilterType) {
				case "Community":
					// Code to execute if expression === value1

					map.data.addListener('click', function (event) {

						var feat = event.feature;

						var formattedAddress = feat.getProperty('Address');
						var isActive = feat.getProperty('IsActive');
						var campaignName = feat.getProperty('CampaignName');
						//var logoFileName = feat.getProperty('LogoFileName');
						var URLFriendlyCampaignName = feat.getProperty('URLFriendlyCampaignName');
						//var missionPurpose = feat.getProperty('MissionPurpose');
						var pointOfContactName = feat.getProperty('PointOfContactName');
						var phoneNumber = feat.getProperty('PhoneNumber');

						var html = "<div class='col-sm-12'>test</div>";

						infowindow.setContent(html);
						infowindow.setPosition(event.latLng);
						infowindow.open(map);
					});
					break;

				case "All":
					// Code to execute if expression === value2

					map.data.addListener('click', function (event) {
						var feat = event.feature;
						var html = "<div class='col-sm-12'> <b><a style='text-decoration:underline;' target='_blank' href='/V1/Location.aspx?locationProfileId=" + feat.getProperty('locationProfileId') + "'>"
							+ feat.getProperty('Location Name') + "</a></b></br>" 
							+ feat.getProperty('Address') + "</br></br>"
							+ "<b>" + feat.getProperty('Location Type') + "</b></br>"
							+ feat.getProperty('Description') + "</b></br></br>"
							+ "Seeking Volunteers: " + feat.getProperty('SeekingVolunteers') + "</br>"
							+ "Allows Pets: " + feat.getProperty('AllowsPets') + "</br>"
							+ "Provides Medical Help: " + feat.getProperty('ProvidesMedicalHelp') + "</br>"
							+ "Capacity: " + feat.getProperty('Capacity') + "</br></br>"
						if (feat.getProperty('donationURL'))
						{ html += "<a style='text-decoration:underline;' class='btn btn-success btn-sm pull-left m-r-sm' target='_blank' href='" + feat.getProperty('donationURL') + "'>Donate To This Location</a>"; }
						html += " <a class='btn btn-info btn-sm pull-left' target='_blank' href='/V1/Location.aspx?locationProfileId=" + feat.getProperty('locationProfileId') + "'>Visit this Location</a>";
						html += "</div>";

						infowindow.setContent(html);
						infowindow.setPosition(event.latLng);
						infowindow.open(map);
					});
					break;

				case "Critical":
					// Code to execute if expression === value2
					break;

				case "VOAD":
					// Code to execute if expression === value2
					break;

				case "Professional":
					// Code to execute if expression === value2
					break;

				// Add more cases as needed
				default:
				// Code to execute if no matching case
			}
		}
	</script>
	<script src="https://maps.googleapis.com/maps/api/js?key=<%=mapApiKey%>"></script>
	 <style>
       /* Set the size of the div element that contains the map */
      #map {
        height: 800px;  /* The height is 400 pixels */
        width: 100%;  /* The width is the width of the web page */
       }
	  a{
		   outline:none !important;
		   border: none !important;
		}
	   .map-icon-label .map-icon {
			font-size: 24px;
			color: #FFFFFF;
			line-height: 48px;
			text-align: center;
			white-space: nowrap;
		}
		
/*		.activeUsersGrid						{min-height:200px; margin-top:20px;}
		.activeUsersGrid-item					{width:180px; }*/
		.alert-success							{cursor:pointer; background-color:#62CB31; color:white; }

		.EventLink
		{
			color:#365899;
		}
		.row-fluid .col-xs-3 .col-xs-9
		{
			padding:0px;
		}
		.EventLink:hover
		{
			color:#365899;
			text-decoration:underline;
		}
		.LinkButton
		{
			color:#FFFFFF !important;
            font-weight:normal !important;
            text-decoration:none !important;
		}
        .tab-background
        {
			background-color:#5E2E91;
			color:#5E2E91;
			border-top-left-radius: 5px;  /* Adjust the value as needed */
			border-top-right-radius: 5px; /* Adjust the value as needed */
			margin-right:7px;
        }
		li a{
			color:white;
		}
		.nav-tabs
		{
			height:40px;
		}
	 	.tab-content
		 {
	 		background-color: white;
	 		padding: 20px !important;
	 	}
		 .mapFilter
		 {
	 		background-color: white;
	 		padding: 10px 20px 0px 20px !important;
	 	}
		.map-tab
		{
	 		background-color: white;
	 		padding: 0px !important;
		}
		.panel-heading
		{
			border-radius:10px !important;
		}
		.panel-body
		{
			border-radius:10px !important;
		}
		.ddlMapFilter
		{width:250px;
		 text-align:left;
		}
	</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
		<asp:HiddenField runat="server" id="hidEventId"></asp:HiddenField>
		<asp:HiddenField runat="server" id="hidEventName"></asp:HiddenField>
			<div class="hpanel collapsed">
				<div class="panel-heading hbuilt member-panel-body">
					<div class="panel-tools"><a class="showhide"><i class="fa fa-chevron-up"></i></a></div>
					<h4><%=eventName%></h4>
				</div>
				<div class="panel-body member-panel-body">
					<uc1:EventHeader runat="server" ID="uc1EventHeader" />
				</div>
			</div>


			<div class="row m-t-lg" style="margin-bottom:200px;">
				<div class="col-sm-12">
					<div class="hpanel">
						<div class="hpanel">
							<ul class="nav nav-tabs">
								<li class="tab-background active"><a data-toggle="tab" href="#tab1">Map</a></li>
								<li class="tab-background"><a data-toggle="tab" href="#tab9" class="font-bold">Deployments</a></li>
								<li class="tab-background"><a data-toggle="tab" href="#tab11" class="font-bold">Relief Teams</a></li>
								<li class="tab-background"><a data-toggle="tab" href="#tab10" class="font-bold">States and Counties</a></li>
								<li class="tab-background"><a data-toggle="tab" href="#tab8" class="font-bold">Community Links</a></li>
								<li class="tab-background" id="liTab7" visible="false" runat="server"><a data-toggle="tab" href="#<%=tab7.ClientID%>" class="font-bold">Case Management</a></li>
								<li class="tab-background" runat="server" visible="false" id="liAdminTab"><a data-toggle="tab" href="#<%=tab6.ClientID%>" class="font-bold">Administration</a></li>
								<li class="tab-background"><a data-toggle="tab" href="#tab3" visible="false" runat="server" class="font-bold">Survivor Stories</a></li>
								<li class="tab-background"><a data-toggle="tab" href="#tab4" visible="false" runat="server" class="font-bold">Statistics</a></li>
								<li class="tab-background"><a data-toggle="tab" href="#tab5" visible="false" runat="server" class="font-bold">Relief Finder</a></li>
								<li class="pull-right">
									<div class="form-group">
										<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Change Community Portals <i class="fa fa-sort-down"></i> </button>
										<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
											<%=disasterDropDown%>
										</ul>
									</div>
								</li>
							</ul>
							<div class="tab-content map-tab">
								<div id="tab1" class="tab-pane active">
									<div class="form-group" id="divMapFilter">
										<div id="divChangeMapFilter" class="dropdown m-b-md mapFilter" runat="server">
											<button id="btn-mapDropDown" class="btn btn-outline btn-default ddlMapFilter dropdown-toggle dropdown-map-filter" type="button" data-toggle="dropdown">Modify Map Filter <i class="fa fa-sort-down"></i></button>
											<ul id="ddlMapFilter" class="dropdown-menu text-center dropdown-map-filter">
												<li id="All" Selected="True"><a href="#">All Locations</a></li>
												<li id="Community"><a href="#">Stability.org Deployments</a></li>
												<li id="Critical"><a href="#">Critical Facilities</a></li>
												<li id="VOAD"><a href="#">VOAD Organizations</a></li>
												<li id="Professional"><a href="#">Emergency Response Companies</a></li>
											</ul>
										</div>
									</div>
									<div id="map"></div>
								</div>
								<div id="tab8" class="tab-pane">
									<div class="row">
										<div class="col-lg-12">
											<uc1:Links runat="server" ID="ucLinks" />
										</div>
									</div>
								</div>
								<div id="tab9" class="tab-pane">
									<uc1:DeploymentListCard runat="server" ID="ucDeploymentListCard" />
									<div class="row">
										<div class="col-lg-8">
                                            <div class="content" style="background-color:white;">
                                                <asp:HyperLink ID="hypAddNewCampaign" CssClass="btn btn-info pull-left m-r-lg" runat="server" Text="<i class='fa fa-map'></i> Add A Deployment" Visible="false"></asp:HyperLink>
                                                <asp:Literal ID="litAddTeamMessage" runat="server"></asp:Literal> 
                                                <asp:HyperLink ID="hypAddTeam" Visible="false" Text="Create Your Team, Organization or Business" runat="server" NavigateUrl="/V1/Administration/NonProfitNew.aspx"></asp:HyperLink>
                                            </div>
                                        </div>
                                    </div>
								</div>
								<div id="tab11" class="tab-pane">
									<div class="row">
										<div class="col-lg-12">
                                            <div class="content">
                                                <div>
                                                    <asp:HyperLink ID="hypAddNewNonProfit" CssClass="btn btn-info pull-left m-r-lg" runat="server" Text="<i class='fa fa-group'></i> Create A Team" Visible="false"></asp:HyperLink>
                                                    Teams can be any kind of friend group, business, non-profit, essentially any kind of group that wants to help. Share how your group will help by adding a team.
                                                    Teams can have many roles: some serve food, others distribute supplies, assist with clean-up, or support physical and mental health.
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="hpanel panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                                                <div class="panel-body">
                                                    <h4 class="m-t-none m-b-none">Relief Teams</h4>
                                                    <small class="text-muted">
                                                        Teams appear below after they have added a deployment to the relief portal.
                                                    </small>
                                                </div>
                                                <asp:Repeater ID="rptTeams" runat="server" OnItemDataBound="rptTeams_ItemDataBound">
                                                    <ItemTemplate>
                                                        <div class="panel-body">
                                                            <asp:Literal ID="litDivQH" runat="server"></asp:Literal>
                                                                <i class="fa fa-chevron-down pull-right text-muted"></i>
                                                                <asp:Literal id="litTeamName" runat="server"></asp:Literal>
                                                            </a>
                                                            <asp:Literal ID="litDivQP" runat="server"></asp:Literal>
                                                                <p>
                                                                    <asp:Literal id="litTeamDescription" runat="server"></asp:Literal>
                                                                </p>
                                                                <asp:HyperLink id="hypViewTeam" runat="server"></asp:HyperLink> | 
                                                                <asp:HyperLink id="hypViewActivities" runat="server"></asp:HyperLink> | 
                                                                <asp:HyperLink id="hypTeamWebsite" Target="_blank" runat="server"></asp:HyperLink> | 
                                                                <asp:HyperLink id="hypTeamCalendar" runat="server"></asp:HyperLink>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                        </div>
                                    </div>
								</div>
								<div id="tab4" class="tab-pane" visible="false" runat="server">
									<div class="row">
										<div class="col-lg-12">
											<div class="hpanel hgreen" runat="server">
												<div class="panel-heading hbuilt">
													<asp:Label id="lblQuickLinks" runat="server"></asp:Label>
												</div>
												<div class="panel-body">
													<div class="row m-b">
														<div class="col-xs-4">
															<b> Total Survivors</b>
														</div>
														<div class="col-xs-3">
															<asp:Literal ID="litSurvivorCount" runat="server"></asp:Literal>
														</div>
														<div class="col-xs-5">
															<asp:HyperLink id="hypAddASurvivor" runat="server" text="Add Survivor"></asp:HyperLink>
														</div>
													</div>
													<div class="row m-b">
														<div class="col-xs-4">
															<b>Home Rebuilds</b>
														</div>
														<div class="col-xs-3">
															<asp:Literal ID="litHomesAdded" runat="server"></asp:Literal>
														</div>
														<div class="col-xs-5">
															<asp:HyperLink id="hypAddHome" runat="server" text="Add Home"></asp:HyperLink>
														</div>
													</div>
													<div class="row m-b">
														<div class="col-xs-4">
															<b> Total Volunteers</b>
														</div>
														<div class="col-xs-3">
															<asp:Literal ID="litVolunteerCount" runat="server"></asp:Literal>
														</div>
														<div class="col-xs-5">
														</div>
													</div>
													<div class="row m-b">
														<div class="col-xs-4">
															<b>Volunteers Needed</b>
														</div>
														<div class="col-xs-3">
															<asp:Literal ID="litVolunteers" runat="server"></asp:Literal>
														</div>
														<div class="col-xs-5">
															<asp:HyperLink id="hypVolunteer" runat="server" text="Volunteer"></asp:HyperLink>
														</div>
													</div>
													<div class="row m-b">
														<div class="col-xs-4">
															<b> Total NonProfits</b>
														</div>
														<div class="col-xs-3">
															<asp:Literal ID="litNonprofitCount" runat="server"></asp:Literal>
														</div>
														<div class="col-xs-5">
															<asp:HyperLink id="hypAddNonProfit" runat="server" text="Add Nonprofit"></asp:HyperLink>
														</div>
													</div>
													<div class="row m-b">
														<div class="col-xs-4">
															<b>Rebuild Progress</b>
														</div>
														<div class="col-xs-3">
															<asp:Literal ID="litRebuildProgress" runat="server"></asp:Literal>
														</div>
														<div class="col-xs-5">
															<asp:HyperLink id="hypUpdateRebuildProgress" runat="server" text="Update Rebuild" NavigateURL="/V1/Profile/Default.aspx"></asp:HyperLink>
														</div>
													</div>
													<div class="row m-b">
														<div class="col-xs-4">
															<b>Overall Progress</b>
														</div>
														<div class="col-xs-3">
															<asp:Literal ID="litOverallProgress" runat="server"></asp:Literal>
														</div>
														<div class="col-xs-5">
															<asp:HyperLink id="hypUpdateProgress" runat="server" text="Update Overall Progress" NavigateURL="/V1/Profile/Default.aspx"></asp:HyperLink>
														</div>
													</div>
												</div>
											</div>
											<uc1:TimeBoard runat="server" ID="ucTimeBoard" />
										</div>
									</div>
								</div>	
								<div id="tab10" class="tab-pane m-b-lg">
									<div class="row m-b-lg">
										<div class="col-lg-12 m-b-lg">
                                            <div class="content m-b-lg">
                                                <p class="m-t-lg m-b-lg">
                                                    <h2>States and Counties</h2>
                                                    This list of impacted states and counties will evolve as emergency managers publically report damage assessments.
                                                </p>
                                                <br />
												<uc1:ImpactedCommunities runat="server" ID="ucImpactedCommunities" />
                                                        
											</div>
										</div>
									</div>
								</div>
								<div id="tab7" class="tab-pane" runat="server" visible="false">
									<div class="row">
										<div class="col-lg-12">
											<div class="panel-body no-padding">
												<div class="hpanel hgreen" runat="server">
													<div class="panel-heading hbuilt">
														Case Management
													</div>
													<div class="panel-body">
														<asp:HyperLink ID="hypCaseManagers" Font-Underline="true" CssClass="btn btn-sm btn-info" runat="server" Text="Create a New Case"></asp:HyperLink>
														<asp:HyperLink ID="hypViewCases" Font-Underline="true" CssClass="btn btn-sm btn-info" runat="server" Text="View Cases"></asp:HyperLink>
													</div>
												</div>
											</div>
										</div>
									</div>
								</div>
								<div id="tab5" class="tab-pane" visible="false" runat="server">
									<div class="row">
										<div class="col-lg-12">
											<div class="panel-body no-padding">
												<div class="chat-discussion" style="height: auto">
													<h2>Post your needs or how you can help.</h2>
													<asp:Image ID="imgNewPostLogo" runat="server" CssClass="post-logo" />
													<div class="message">
														<a class="message-author" href="#">  </a>
														<span class="message-content">
															<asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtPost" runat="server"></asp:TextBox>
														</span>
														<div style="overflow:auto;">
															<asp:LinkButton ID="btnRebuildPost" CausesValidation="false" OnClick="btnRebuildPost_Click" CssClass="btn w-xs btn-sm btn-primary pull-right m-t-sm" runat="server" Text="Post" />
														</div>
													</div>
													<asp:Repeater ID="rptPosts" runat="server" OnItemDataBound="rptPosts_OnItemDataBound">
														<ItemTemplate>
															<div class="hpanel">
																<div class="panel-body">
																	<div class="message">
																		<div class="blog-article-box">
																			<asp:HyperLink id="hypFullname" runat="server"></asp:HyperLink>
																		</div>
																		<span class="message-date">
																			<%# DataBinder.Eval(Container.DataItem, "createdon", "{0:M/d/yyyy HH:mm:ss}") %>
																		</span>
																		<span class="message-content">
																			<%# DataBinder.Eval(Container.DataItem, "Post") %>
																			<div class="pull-right" runat="server" id="divDelete" visible="false">
																				<asp:LinkButton id="lbDelete" runat="server" OnClick="lbDelete_Click" text="Delete"></asp:LinkButton>
																			</div>
																		</span>
																	</div>
																</div>
															</div>
														</ItemTemplate>
													</asp:Repeater>
												</div>
											</div>
										</div>
									</div>
								</div>
								<div id="tab3" class="tab-pane" visible="false" runat="server">
									<div class="row">
										<div class="col-lg-12">
											<uc1:DisasterSurvivorStoriesByDisaster runat="server" ID="ucDisasterSurvivorStoriesByDisaster" />
										</div>
									</div>
								</div>
								<div id="tab6" class="tab-pane" runat="server">
									<div class="row">
										<div class="col-lg-12">
											<div class="hpanel hgreen" runat="server" visible="false" id="divAdmin">
												<div class="panel-tools m-sm">
													<a class="showhide"><i class="fa fa-chevron-up"></i></a>
													<a class="closebox"><i class="fa fa-times"></i></a>
												</div>
												<div class="alert alert-secondary">
													<i class="fa fa-lock"></i> Administrative Tools 
                                                    | <asp:HyperLink ID="hypEditDisaster" runat="server" Text="Edit Portal"></asp:HyperLink>
                                                    | <asp:HyperLink ID="hypEditCounties" runat="server" Text="Edit Counties"></asp:HyperLink>
												</div>
												<div class="panel-body">
													<div>
														<div class="row">
															<div class="col-sm-12 well">
																<div class="hpanel panel-body">
																	<div class="panel-heading">
																	<h2>Add A New Link</h2>
																		<asp:HyperLink ID="hypNewCategory" CssClass="EventLink" runat="server" Text="Add A New Article"></asp:HyperLink>
																	</div>
																	<div class="alert alert-success m-b" runat="server" id="divAlertMessage" visible="false"><asp:Literal ID="litMessage" runat="server"></asp:Literal></div>
																	<div class="form-group m-t-md">
																		<div class="input-group">
																			<div class="input-group-addon">
																				Choose Link Category
																			</div>
																			<asp:DropDownList CssClass="form-control" ID="ddlCateogry" runat="server" DataTextField="category" DataValueField="CategoryId" ></asp:DropDownList>
																		</div>
																	</div>
																	<div class="form-group ">
																		<div class="input-group">
																			<div class="input-group-addon">
																				Link URL
																			</div>
																			<input class="form-control" id="txtUrl" runat="server" name="txtUrl" type="url" placeholder="URL" required/>
																		</div>
																	</div>
																	<div class="form-group ">
																		<div class="input-group">
																			<div class="input-group-addon">
																				Link Title
																			</div>
																			<input class="form-control" id="txtTitle" runat="server" name="txtTitle" type="text" placeholder="Link Title" required/>
																		</div>
																	</div>
																	<div class="form-group ">
																		<div class="input-group">
																			<div class="input-group-addon">
																				Link Description
																			</div>
																			<textarea class="form-control" cols="40" rows="10" id="txtDescription" runat="server" name="txtDescription" type="text" placeholder="Link Description" required></textarea>
																		</div>
																	</div>
																	<div class="form-group ">
																		<div class="input-group pull-right">
																			<asp:Button ID="btnAddLink" runat="server" Text="Add Link" CssClass="btn btn-primary" OnClick="btnAddLink_Click" />
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
							</div>
							<div class="panel-footer">
								<div runat="server" id="divChooseDefaultDisaster" visible="false">
									<asp:Button ID="btnSetDefaultDisaster" CssClass="btn" CausesValidation="false" runat="server" Text="Set as Default Portal" />
									<asp:Label ID="lblDefaultEventMessage" runat="server"></asp:Label>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
			<div style="height:200px; width:100%;"></div>
			<div class="alert" runat="server" id="divRegister" visible="false">
				To use all of the features of Stability please sign in or register. <i class="fa fa-user"></i> 
				<p class="m-t-sm">
					<a href="/Register" class="btn btn-success">Register Here</a>
					<a href="/SignIn" class="btn btn-success">Sign In</a>
				</p>	
			</div>
		</div>
		<script src="/v1/Scripts/masonry.pkgd.min.js"></script>
		<script src="/V1/Scripts/map-icons-master/dist/js/map-icons.js"></script>
		<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
</asp:Content>