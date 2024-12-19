<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Portal.master" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="V1_Deployments_Map" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Portal.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	
	<script type="text/javascript">

		$(document).ready(function ()
		{
			$("#btn-dropdown.disasterEvent").html('<%=_eventName%>');

			//Redirect to another portal using the ddl
			$("#disasterEvent.dropdown-menu li").click(function (event) {
				window.location.href = "/Maps/" + $(this).attr('name');
				event.preventDefault();
			});

			//Default
			initMap("Critical"); //Community

			$("#ddlMapFilter.dropdown-menu li").click(function (event) {
				$("#btn-mapDropDown.ddlMapFilter").html($(this).text());
				var mapFilterType = $(this).attr('id');
				initMap(mapFilterType);
				event.preventDefault();
			});

			var map;
			let GEOJsonPath = '<%=_mapGEOJsonPath%>';
			
			function initMap(mapFilterType)
			{
				$('#list').empty();
				var infowindow = new google.maps.InfoWindow();

				map = new google.maps.Map(document.getElementById('map'), {
					zoom: <%=_zoom%>,
					center: { lat: <%=_latitude%>, lng: <%=_longitude%> },
					styles: [{ "stylers": [{ "saturation": -11 }] }]
				});

				map.data.setStyle(function (feature) {
					const icon = feature.getProperty('icon');
					return {
						icon: {
							url: icon, // Assuming 'icon' is the URL of the image
							scaledSize: new google.maps.Size(35, 35) // Set the desired size (width, height)
						}
					};
				});

				if ('<%=_mapGEOJsonPath%>') {

					//THE FEMA COUNTY LAYER
					map.data.loadGeoJson('<%=_mapGEOJsonPath%>');

					// Apply dynamic styles based on properties in the GeoJSON
					//map.data.setStyle(function (feature) {
					//	return {
					//		fillColor: feature.getProperty('fill') || '#d79e9e',			// Default fill color if none provided
					//		fillOpacity: feature.getProperty('fill-opacity') || 0.6,		// Default opacity
					//		strokeColor: feature.getProperty('stroke') || '#6e6e6e',		// Default stroke color
					//		strokeOpacity: feature.getProperty('stroke-opacity') || 1.0,	// Default stroke opacity
					//		strokeWeight: feature.getProperty('stroke-width') || 2,			// Default stroke width
					//	};
					//});

				}

				// Overlay the OpenWeatherMap tile layer for clouds
				//var weatherLayer = new google.maps.ImageMapType({
				//	getTileUrl: function (coord, zoom) {
				//		//alert("https://api.openweathermap.org/data/3.0/onecall?lat=" + coord.x + "&lon=" + coord.y + "&appid=4d7b998ab8afe0a1649b818287b88eba");
				//		return "https://api.openweathermap.org/data/3.0/onecall?lat=" + coord.x + "&lon=" + coord.y + "&appid=4d7b998ab8afe0a1649b818287b88eba";
				//		//return weatherLayer;
				//	},
				//	tileSize: new google.maps.Size(1000, 1000),
				//	opacity: 0.6
				//});

				//// Add the weather layer to the map
				//map.overlayMapTypes.insertAt(3, weatherLayer);







				var mapsURL = '<%=host%>/V1/Handlers/GetGeoJsonByDisaster.ashx?keyId=<%=_eventId%>&mapFilterType=' + mapFilterType;

				// Load GeoJSON data from your .ashx handler
				map.data.loadGeoJson(mapsURL, null, function (features) {
					// Once the data is loaded, generate the list
					const listContainer = document.getElementById('list');

					//Create the list
					// Iterate through each feature in the GeoJSON data
					features.forEach(function (feature, index) {
						// Access the properties directly using getProperty
						const address			= feature.getProperty('Address');
						const icon				= '<img src="' + feature.getProperty('icon') + '" class="pull-left" style="width:35px; margin-right:10px;">';
						//alert(icon);
						var campaignName		= "";
						var contact				= "";
						var organizationName	= "";
						var label1				= "";
						if (mapFilterType === 'All' || mapFilterType === 'Critical') {
							//alert(icon);
							campaignName = feature.getProperty('Location Name');
							if (feature.getProperty('PointOfContactName') != null) {
								contact = '<p><strong>Contact:</strong> ' + feature.getProperty('PointOfContactName') + '</br><strong>Phone:</strong> ' + feature.getProperty('PhoneNumber') + '</p>';
							}

							organizationName = feature.getProperty('Location Type');
							label1 = "Location Type";
						} else {
							campaignName = feature.getProperty('CampaignName');
							contact = '<p><strong>Contact:</strong> ' + feature.getProperty('PointOfContactName') + '</br><strong>Phone:</strong> ' + feature.getProperty('PhoneNumber') + '</p>';
							organizationName = feature.getProperty('OrganizationName');
							label1 = "Organization";
						}

						const coordinates = feature.getGeometry().get();  // Get coordinates (Google Maps LatLng object)

						// Create a list item for each feature
						const listItem = document.createElement('div');
						listItem.className = 'list-item';
						listItem.innerHTML = ` ${icon}
							<h4>${campaignName}</h4>
							<p><strong>${label1}</strong> ${organizationName}</p>
							<p><strong>Address:</strong> ${address}</p>
							${contact}
          `;
						listContainer.appendChild(listItem);

						// Click event on list item to zoom the map to the corresponding point
						listItem.addEventListener('click', () => {
							map.setCenter(coordinates);
							map.setZoom(15);
						});
					});
				});

				//Load the info window when a user clicks on it.
				switch (mapFilterType) {
					case "Cases":

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
							var organizationId = feat.getProperty('OrganizationId');
							var organizationName = feat.getProperty('OrganizationName');
							var URLFriendlyOrganizationName = feat.getProperty('URLFriendlyOrganizationName');
							var btnSignUp = "<a target='_blank' href='/SignUp/" + URLFriendlyCampaignName + "'>View Open Positions</a>";

							var html = "<div class='col-sm-12'>" +
								"<h4 style='margin:0px;'><a href='/Cause/" + URLFriendlyCampaignName + "'>" + campaignName + "</a></h4>" +
								"<b><a target='_blank' href='/V1/NonProfit/Default.aspx?organizationId=" + organizationId + "'>" + organizationName + "</a></b></br>" +
								"<b>Address: </b>" + formattedAddress + "</br>" +
								"<b>Contact:</b> " + pointOfContactName + "</b></br>" +
								"<b>Phone: </b> " + phoneNumber + "</br > " +
								btnSignUp +
								"</div >"

							infowindow.setContent(html);
							infowindow.setPosition(event.latLng);
							infowindow.open(map);
						});
						break;
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
							var organizationId = feat.getProperty('OrganizationId');
							var organizationName = feat.getProperty('OrganizationName');
							var URLFriendlyOrganizationName = feat.getProperty('URLFriendlyOrganizationName');
							var btnSignUp = "<a target='_blank' href='/SignUp/" + URLFriendlyCampaignName + "'>View Open Positions</a>"; 

							var html = "<div class='col-sm-12'>" +
								"<h4 style='margin:0px;'><a href='/Cause/" + URLFriendlyCampaignName + "'>" + campaignName + "</a></h4>" +
								"<b><a target='_blank' href='/V1/NonProfit/Default.aspx?organizationId=" + organizationId + "'>" + organizationName + "</a></b></br>" +
								"<b>Address: </b>" + formattedAddress + "</br>" +
								"<b>Contact:</b> " + pointOfContactName + "</b></br>" +
								"<b>Phone: </b> " + phoneNumber + "</br > " +
								btnSignUp +
								"</div >"

							infowindow.setContent(html);
							infowindow.setPosition(event.latLng);
							infowindow.open(map);
						});
						break;

					case "All":

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
							if (feat.getProperty('donationURL')) { html += "<a style='text-decoration:underline;' class='btn btn-success btn-sm pull-left m-r-sm' target='_blank' href='" + feat.getProperty('donationURL') + "'>Donate To This Location</a>"; }
							html += " <a class='btn btn-info btn-sm pull-left' target='_blank' href='/V1/Location.aspx?locationProfileId=" + feat.getProperty('locationProfileId') + "'>Visit this Location</a>";
							html += "</div>";

							infowindow.setContent(html);
							infowindow.setPosition(event.latLng);
							infowindow.open(map);
						});
						break;

					case "Critical":

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
							if (feat.getProperty('donationURL')) { html += "<a style='text-decoration:underline;' class='btn btn-success btn-sm pull-left m-r-sm' target='_blank' href='" + feat.getProperty('donationURL') + "'>Donate To This Location</a>"; }
							html += " <a class='btn btn-info btn-sm pull-left' target='_blank' href='/V1/Location.aspx?locationProfileId=" + feat.getProperty('locationProfileId') + "'>Visit this Location</a>";
							html += "</div>";

							infowindow.setContent(html);
							infowindow.setPosition(event.latLng);
							infowindow.open(map);
						});
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
		})
	</script>
	<script src="https://maps.googleapis.com/maps/api/js?key=<%=mapApiKey%>"></script>
	<style>
    /* Set the size of the div element that contains the map */
    #map 
	{
		width: 100%;
		height:700px;
		margin-top:0px !important;
		float: left;
    }
	#list {
      height: 700px;
      width: 100%;
	  min-width:200px;
      float: left;
      overflow-y: scroll;
      padding: 10px;
      border-left: 2px solid #ccc;
    }
    .list-item {
      padding: 10px;
      border-bottom: 1px solid #ddd;
      cursor: pointer;
    }
    .list-item:hover {
      background-color: #f0f0f0;
    }
	a{
		outline:none !important;
		border: none !important;
	}
	.map-icon-label .map-icon 
	{
		font-size: 24px;
		color: #FFFFFF;
		line-height: 48px;
		text-align: center;
		white-space: nowrap;
	}

	.form-group
	{
		padding:20px;
		background-color:white;
		margin-bottom:0px !important;
	}
	.hpanel
	{
		margin-bottom:0px !important;
	}
	.row.no-gutter {
      margin-left: 0;
      margin-right: 0;
    }
    .row.no-gutter [class*='col-'] {
      padding-left: 0;
      padding-right: 0;
    }
    .side-by-side div {
      display: inline-block;
      width: 100%; /* Adjust the width to fit your layout */
      vertical-align: top; /* Align the divs to the top */
      background-color: lightblue;
      padding: 10px;
      margin-right: 1%;
    }
    .side-by-side div:last-child {
      margin-right: 0; /* Remove margin for the last div */
    }
	</style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="row form-group no-gutter">
		<div class="col-xs-12 col-sm-6" style="max-width:260px;">
			<button id="btn-mapDropDown" class="btn btn-outline btn-default ddlMapFilter dropdown-toggle dropdown-map-filter" type="button" data-toggle="dropdown">Modify Map Filter <i class="fa fa-sort-down"></i></button>
			<ul id="ddlMapFilter" class="dropdown-menu text-center dropdown-map-filter">
				<li id="All" Selected="True"><a href="#">All Locations</a></li>
				<li id="Community"><a href="#">Stability.org Deployments</a></li>
				<li id="Critical"><a href="#">Critical Facilities</a></li>
				<%=liCases%>
<%--			<li id="VOAD"><a href="#">VOAD Organizations</a></li>
				<li id="Professional"><a href="#">Emergency Response Companies</a></li>--%>
			</ul>
		</div>
		<div class="col-xs-12 col-sm-6">
			<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Change Community Portals <i class="fa fa-sort-down"></i> </button>
			<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
				<%=disasterDropDown%>
			</ul>
		</div>
		<div class="text-right">
		<b>Share Your Team Deployment</b>
			<br />
			<asp:HyperLink ID="hypCreateDeployment" CssClass="btn m-l-md btn-info btn-md pull-right" runat="server"></asp:HyperLink>
			<asp:HyperLink ID="hypSignIn" CssClass="btn btn-info btn-md pull-right" runat="server" Text="Sign In" NavigateUrl="/SignIn" Visible="false"></asp:HyperLink>
		</div>
	</div>
	<div class="row no-gutter">
		<div class="col-xs-12 col-sm-9">
			<div id="map"></div>
		</div>
		<div class="col-xs-12 col-sm-3">
			<div id="list"></div>
		</div>
	</div>
	<div style="height:200px;"></div>
</asp:Content>