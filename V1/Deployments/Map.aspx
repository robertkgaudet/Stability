<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Portal.master" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="V1_Deployments_Map" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Portal.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	
	<script type="text/javascript">
        $(document).ready(function () {
            $("#btn-dropdown.disasterEvent").html('<%=_eventName%>');

        // Handle disaster event dropdown click
        $("#disasterEvent.dropdown-menu li").click(function (event) {
            window.location.href = "/Maps/" + $(this).attr('name');
            event.preventDefault();
        });

       
        function fetchNearbyPlaces(map, latitude, longitude) {
            const categories = [
                'church',
                'school',
                'synagogue',
                'primary_school',
                'hindu_temple',
                'secondary_school',
                'mosque',
                'university',
                'local_government_office',
                'gas_station',
                'fire_station',
                'pharmacy',
                'police',
                'drugstore',
                'airport',
                'hospital'
            ];

            // Loop through each category and fetch nearby places
            categories.forEach(category => {
                const request = {
                    location: { lat: latitude, lng: longitude },
                    radius: 5000, // Search within 5 km
                    type: category // Specify the category
                };

                // Use Google Places API to fetch nearby places
                const service = new google.maps.places.PlacesService(map);
                service.nearbySearch(request, (results, status) => {
                    if (status === google.maps.places.PlacesServiceStatus.OK) {
                        // Add markers for each place
                        results.forEach(place => {
                            createPlaceMarker(map, place, category);
                        });
                    }
                });
            });
        }

        // Function to create a marker for a place
        function createPlaceMarker(map, place, category) {
            const marker = new google.maps.Marker({
                map: map,
                position: place.geometry.location,
                title: place.name,
                icon: getIconForCategory(category) // Use custom icons for each category
            });

            // Add an info window to display place details
            const infowindow = new google.maps.InfoWindow({
                content: `<strong>${place.name}</strong><br>${place.vicinity}`
            });

            // Open the info window when the marker is clicked
            marker.addListener('click', () => {
                infowindow.open(map, marker);
            });
        }


            function getIconForCategory(category) {
                const iconBase = '/Impactoid/Images/Icons/'; // Local or server path
                const iconSize = new google.maps.Size(40, 40); // Set the size of the icon (width, height)

                switch (category) {
                    case 'church': return { url: iconBase + 'churchimg.png', scaledSize: iconSize };
                    case 'school': return { url: iconBase + 'Schoolimg.png', scaledSize: iconSize };
                    case 'hospital': return { url: iconBase + 'hospitalimg.png', scaledSize: iconSize };
                    case 'synagogue': return { url: iconBase + 'synagogue.jpg', scaledSize: iconSize };
                    case 'primary_school': return { url: iconBase + 'primary_schoolimg.jpg', scaledSize: iconSize };
                    case 'hindu_temple': return { url: iconBase + 'hindu_templeimg.jpg', scaledSize: iconSize };
                    case 'secondary_school': return { url: iconBase + 'secondary_schoolimg.webp', scaledSize: iconSize };
                    case 'mosque': return { url: iconBase + 'mosqueimg.png', scaledSize: iconSize };
                    case 'university': return { url: iconBase + 'universityimg.avif', scaledSize: iconSize };
                    case 'local_government_office': return { url: iconBase + 'local_government_officeimg.webp', scaledSize: iconSize };
                    case 'gas_station': return { url: iconBase + 'gas_stationimg.png', scaledSize: iconSize };
                    case 'pharmacy': return { url: iconBase + 'primary_schoolimg.jpg', scaledSize: iconSize };
                    case 'police': return { url: iconBase + 'policeimg.png', scaledSize: iconSize };
                    case 'drugstore': return { url: iconBase + 'drugstoreimg.png', scaledSize: iconSize };
                    default: return { url: iconBase + 'info-i_maps.png', scaledSize: iconSize }; // Default icon
                }
            }


        // Initialize the map
        function initMap(mapFilterType) {
            $('#list').empty();
            const infowindow = new google.maps.InfoWindow();

            // Create the map centered at the portal's location
            const map = new google.maps.Map(document.getElementById('map'), {
                zoom: <%=_zoom%>,
                center: { lat: <%=_latitude%>, lng: <%=_longitude%> },
                styles: [{ "stylers": [{ "saturation": -11 }] }]
            });

            // Fetch and display nearby places
            fetchNearbyPlaces(map, <%=_latitude%>, <%=_longitude%>);

            // Load GeoJSON data if available
            if ('<%=_mapGEOJsonPath%>') {
                map.data.loadGeoJson('<%=_mapGEOJsonPath%>');
            }

            // Load additional GeoJSON data based on the map filter type
            const mapsURL = '<%=host%>/V1/Handlers/GetGeoJsonByDisaster.ashx?keyId=<%=_eventId%>&mapFilterType=' + mapFilterType;
            map.data.loadGeoJson(mapsURL, null, function (features) {
                const listContainer = document.getElementById('list');
                features.forEach(function (feature, index) {
                    const address = feature.getProperty('Address');
                    const icon = `<img src="${feature.getProperty('icon')}" class="pull-left" style="width:35px; margin-right:10px;">`;
                    const campaignName = feature.getProperty('Location Name') || feature.getProperty('CampaignName');
                    const contact = feature.getProperty('PointOfContactName') ? `<p><strong>Contact:</strong> ${feature.getProperty('PointOfContactName')}<br><strong>Phone:</strong> ${feature.getProperty('PhoneNumber')}</p>` : '';
                    const organizationName = feature.getProperty('Location Type') || feature.getProperty('OrganizationName');
                    const label1 = mapFilterType === 'All' || mapFilterType === 'Critical' ? 'Location Type' : 'Organization';

                    const listItem = document.createElement('div');
                    listItem.className = 'list-item';
                    listItem.innerHTML = `${icon}
                        <h4>${campaignName}</h4>
                        <p><strong>${label1}:</strong> ${organizationName}</p>
                        <p><strong>Address:</strong> ${address}</p>
                        ${contact}`;
                    listContainer.appendChild(listItem);

                    listItem.addEventListener('click', () => {
                        map.setCenter(feature.getGeometry().get());
                        map.setZoom(15);
                    });
                });
            });

            // Add click listeners for map features based on the filter type
            map.data.addListener('click', function (event) {
                const feat = event.feature;
                let html = '';

                switch (mapFilterType) {
                    case 'Cases':
                    case 'Community':
                        html = `<div class='col-sm-12'>
                            <h4 style='margin:0px;'><a href='/Cause/${feat.getProperty('URLFriendlyCampaignName')}'>${feat.getProperty('CampaignName')}</a></h4>
                            <b><a target='_blank' href='/V1/NonProfit/Default.aspx?organizationId=${feat.getProperty('OrganizationId')}'>${feat.getProperty('OrganizationName')}</a></b><br>
                            <b>Address:</b> ${feat.getProperty('Address')}<br>
                            <b>Contact:</b> ${feat.getProperty('PointOfContactName')}<br>
                            <b>Phone:</b> ${feat.getProperty('PhoneNumber')}<br>
                            <a target='_blank' href='/SignUp/${feat.getProperty('URLFriendlyCampaignName')}'>View Open Positions</a>
                        </div>`;
                        break;

                    case 'All':
                    case 'Critical':
                        html = `<div class='col-sm-12'>
                            <b><a style='text-decoration:underline;' target='_blank' href='/V1/Location.aspx?locationProfileId=${feat.getProperty('locationProfileId')}'>${feat.getProperty('Location Name')}</a></b><br>
                            ${feat.getProperty('Address')}<br><br>
                            <b>${feat.getProperty('Location Type')}</b><br>
                            ${feat.getProperty('Description')}<br><br>
                            Seeking Volunteers: ${feat.getProperty('SeekingVolunteers')}<br>
                            Allows Pets: ${feat.getProperty('AllowsPets')}<br>
                            Provides Medical Help: ${feat.getProperty('ProvidesMedicalHelp')}<br>
                            Capacity: ${feat.getProperty('Capacity')}<br><br>
                            ${feat.getProperty('donationURL') ? `<a style='text-decoration:underline;' class='btn btn-success btn-sm pull-left m-r-sm' target='_blank' href='${feat.getProperty('donationURL')}'>Donate To This Location</a>` : ''}
                            <a class='btn btn-info btn-sm pull-left' target='_blank' href='/V1/Location.aspx?locationProfileId=${feat.getProperty('locationProfileId')}'>Visit this Location</a>
                        </div>`;
                        break;
                }

                infowindow.setContent(html);
                infowindow.setPosition(event.latLng);
                infowindow.open(map);
            });
        }

        // Default map initialization
        initMap("Critical");

        // Dropdown filter change handler
        $("#ddlMapFilter.dropdown-menu li").click(function (event) {
            $("#btn-mapDropDown.ddlMapFilter").html($(this).text());
            const mapFilterType = $(this).attr('id');
            initMap(mapFilterType);
            event.preventDefault();
        });
    });
    </script>
	  <script src="https://maps.googleapis.com/maps/api/js?key=<%=mapApiKey%>&libraries=places"></script>
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