<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Portal.master" AutoEventWireup="true"
    CodeFile="Map.aspx.cs" Inherits="V1_Deployments_Map" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Portal.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            $("#btn-dropdown.disasterEvent").html('<%=_eventName%>');
           var currentMapFilterType = "Critical";
           var selectedLocationType = "";
           var selectedParentType = "";
           var selectedStatus = "";
           $("#disasterEvent.dropdown-menu li").click(function (event) {
               window.location.href = "/Maps/" + $(this).attr('name');
               event.preventDefault();
           });
           initMap("Critical"); 

           // Map filter type dropdown handler
           $("#ddlMapFilter.dropdown-menu li").click(function (event) {
               $("#btn-mapDropDown.ddlMapFilter").html($(this).text());
               currentMapFilterType = $(this).attr('id');
               applyFilters();
               event.preventDefault();
           });

           // Filter dropdown handlers
           $("#locationTypeFilter.dropdown-menu li").click(function (event) {
               selectedLocationType = $(this).attr('id') || "";
               $(this).closest('.dropdown-menu').siblings('.dropdown-toggle')
                   .html($(this).text() + ' <i class="fa fa-sort-down"></i>');
               applyFilters();
               event.preventDefault();
           });

           $("#parentTypeFilter.dropdown-menu li").click(function (event) {
               selectedParentType = $(this).attr('id') || "";
               $(this).closest('.dropdown-menu').siblings('.dropdown-toggle')
                   .html($(this).text() + ' <i class="fa fa-sort-down"></i>');
               applyFilters();
               event.preventDefault();
           });

           $("#statusFilter.dropdown-menu li").click(function (event) {
               selectedStatus = $(this).attr('id') || "";
               $(this).closest('.dropdown-menu').siblings('.dropdown-toggle')
                   .html($(this).text() + ' <i class="fa fa-sort-down"></i>');
               applyFilters();
               event.preventDefault();
           });

           var map;
           let GEOJsonPath = '<%=_mapGEOJsonPath%>';

           function initMap(mapFilterType) {
               currentMapFilterType = mapFilterType;
               applyFilters();
           }

           function applyFilters() {
               $('#list').empty();
               var infowindow = new google.maps.InfoWindow();

               // Clear existing map if it exists
               if (map) {
                   $('#map').empty();
               }

               map = new google.maps.Map(document.getElementById('map'), {
                   zoom: <%=_zoom%>,
                center: { lat: <%=_latitude%>, lng: <%=_longitude%> },
                styles: [{ "stylers": [{ "saturation": -11 }] }]
            });

            map.data.setStyle(function (feature) {
                const icon = feature.getProperty('icon');
                return {
                    icon: {
                        url: icon,
                        scaledSize: new google.maps.Size(35, 35)
                    }
                };
            });

            if ('<%=_mapGEOJsonPath%>') {
                map.data.loadGeoJson('<%=_mapGEOJsonPath%>');
            }

            // Build the URL with all current filters
            var mapsURL = '<%=host%>/V1/Handlers/GetGeoJsonByDisaster.ashx?keyId=<%=_eventId%>&mapFilterType=' + currentMapFilterType;

               if (selectedLocationType) {
                   mapsURL += '&locationType=' + selectedLocationType;
               }
               if (selectedParentType) {
                   mapsURL += '&parentType=' + selectedParentType;
               }
               if (selectedStatus) {
                   mapsURL += '&status=' + selectedStatus;
               }

               // Clear existing data and load with new filters
               map.data.forEach(function (feature) {
                   map.data.remove(feature);
               });

               // Load the filtered data
               map.data.loadGeoJson(mapsURL, null, function (features) {
                   const listContainer = document.getElementById('list');

                   features.forEach(function (feature, index) {
                       const address = feature.getProperty('Address');
                       const icon = '<img src="' + feature.getProperty('icon') + '" class="pull-left" style="width:35px; margin-right:10px;">';
                       var campaignName = "";
                       var contact = "";
                       var organizationName = "";
                       var label1 = "";

                       if (currentMapFilterType === 'All' || currentMapFilterType === 'Critical') {
                           campaignName = feature.getProperty('Location Name');
                           if (feature.getProperty('PointOfContactName') != null) {
                               contact = '<p><strong>Contact:</strong> ' + feature.getProperty('PointOfContactName') +
                                   '</br><strong>Phone:</strong> ' + feature.getProperty('PhoneNumber') + '</p>';
                           }
                           organizationName = feature.getProperty('Location Type');
                           label1 = "Location Type";
                       } else {
                           campaignName = feature.getProperty('CampaignName');
                           contact = '<p><strong>Contact:</strong> ' + feature.getProperty('PointOfContactName') +
                               '</br><strong>Phone:</strong> ' + feature.getProperty('PhoneNumber') + '</p>';
                           organizationName = feature.getProperty('OrganizationName');
                           label1 = "Organization";
                       }

                       const coordinates = feature.getGeometry().get();
                       const listItem = document.createElement('div');
                       listItem.className = 'list-item';
                       listItem.innerHTML = ` ${icon}
                        <h4>${campaignName}</h4>
                        <p><strong>${label1}</strong> ${organizationName}</p>
                        <p><strong>Address:</strong> ${address}</p>
                        ${contact}`;
                       listContainer.appendChild(listItem);

                       listItem.addEventListener('click', () => {
                           map.setCenter(coordinates);
                           map.setZoom(15);
                       });
                   });
               });

               // Info window handlers based on filter type
               switch (currentMapFilterType) {
                   case "Cases":
                       map.data.addListener('click', function (event) {
                           var feat = event.feature;
                           var html = buildCaseInfoWindowHtml(feat);
                           infowindow.setContent(html);
                           infowindow.setPosition(event.latLng);
                           infowindow.open(map);
                       });
                       break;

                   case "Community":
                       map.data.addListener('click', function (event) {
                           var feat = event.feature;
                           var html = buildCommunityInfoWindowHtml(feat);
                           infowindow.setContent(html);
                           infowindow.setPosition(event.latLng);
                           infowindow.open(map);
                       });
                       break;

                   case "All":
                   case "Critical":
                       map.data.addListener('click', function (event) {
                           var feat = event.feature;
                           var html = buildLocationInfoWindowHtml(feat, currentMapFilterType);
                           infowindow.setContent(html);
                           infowindow.setPosition(event.latLng);
                           infowindow.open(map);
                       });
                       break;
               }
           }

           // Helper functions for info window content
           function buildCaseInfoWindowHtml(feat) {
               var formattedAddress = feat.getProperty('Address');
               var campaignName = feat.getProperty('CampaignName');
               var URLFriendlyCampaignName = feat.getProperty('URLFriendlyCampaignName');
               var pointOfContactName = feat.getProperty('PointOfContactName');
               var phoneNumber = feat.getProperty('PhoneNumber');
               var organizationId = feat.getProperty('OrganizationId');
               var organizationName = feat.getProperty('OrganizationName');
               var btnSignUp = "<a target='_blank' href='/SignUp/" + URLFriendlyCampaignName + "'>View Open Positions</a>";

               return `<div class='col-sm-12'>
                <h4 style='margin:0px;'><a href='/Cause/${URLFriendlyCampaignName}'>${campaignName}</a></h4>
                <b><a target='_blank' href='/V1/NonProfit/Default.aspx?organizationId=${organizationId}'>${organizationName}</a></b></br>
                <b>Address: </b>${formattedAddress}</br>
                <b>Contact:</b> ${pointOfContactName}</b></br>
                <b>Phone: </b> ${phoneNumber}</br>
                ${btnSignUp}
            </div>`;
           }

           function buildCommunityInfoWindowHtml(feat) {
               // Similar to buildCaseInfoWindowHtml but with community-specific content
               // Implementation would mirror your existing community case
           }

           function buildLocationInfoWindowHtml(feat, filterType) {
               var locationName = feat.getProperty('Location Name');
               var address = feat.getProperty('Address');
               var locationType = feat.getProperty('Location Type');
               var description = feat.getProperty('Description');
               var seekingVolunteers = feat.getProperty('SeekingVolunteers');
               var allowsPets = feat.getProperty('AllowsPets');
               var providesMedicalHelp = feat.getProperty('ProvidesMedicalHelp');
               var capacity = feat.getProperty('Capacity');
               var donationURL = feat.getProperty('donationURL');
               var locationProfileId = feat.getProperty('locationProfileId');

               var html = `<div class='col-sm-12'>
                <b><a style='text-decoration:underline;' target='_blank' href='/V1/Location.aspx?locationProfileId=${locationProfileId}'>
                ${locationName}</a></b></br>
                ${address}</br></br>
                <b>${locationType}</b></br>
                ${description}</b></br></br>
                Seeking Volunteers: ${seekingVolunteers}</br>
                Allows Pets: ${allowsPets}</br>
                Provides Medical Help: ${providesMedicalHelp}</br>
                Capacity: ${capacity}</br></br>`;

               if (donationURL) {
                   html += `<a style='text-decoration:underline;' class='btn btn-success btn-sm pull-left m-r-sm' target='_blank' href='${donationURL}'>Donate To This Location</a>`;
               }
               html += ` <a class='btn btn-info btn-sm pull-left' target='_blank' href='/V1/Location.aspx?locationProfileId=${locationProfileId}'>Visit this Location</a>`;
               html += "</div>";

               return html;
           }
       });
    </script>
    <script src="https://maps.googleapis.com/maps/api/js?key=<%=mapApiKey%>"></script>
    <style>
        /* Set the size of the div element that contains the map */
        #map {
            width: 100%;
            height: 700px;
            margin-top: 0px !important;
            float: left;
        }

        #list {
            height: 700px;
            width: 100%;
            min-width: 200px;
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

        a {
            outline: none !important;
            border: none !important;
        }

        .map-icon-label .map-icon {
            font-size: 24px;
            color: #FFFFFF;
            line-height: 48px;
            text-align: center;
            white-space: nowrap;
        }

        .form-group {
            padding: 20px;
            background-color: white;
            margin-bottom: 0px !important;
        }

        .hpanel {
            margin-bottom: 0px !important;
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
        /* Improved dropdown styling */
        .filter-container {
            display: flex;
            flex-wrap: wrap;
            align-items: center;
            gap: 8px;
            padding: 10px 0;
        }

       .filter-dropdown {
    position: relative;
    display: inline-block;
}

.filter-dropdown .dropdown-menu {
    max-height: 300px; /* Set a max height to prevent overflow */
    overflow-y: auto; /* Enable scroll if too many items */
    min-width: 250px; /* Adjust width as needed */
    white-space: nowrap; /* Prevent text wrapping */
    background-color: #fff; /* Ensure background color */
    border: 1px solid #ccc; /* Add a border for clarity */
    box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1); /* Add slight shadow */
}

.filter-dropdown .dropdown-toggle {
    width: auto; /* Ensure button width adjusts dynamically */
    min-width: 200px; /* Set a reasonable width */
}

.filter-dropdown .dropdown-menu a {
    padding: 3px; /* Adjust padding for better spacing */
    display: block;
    color: #333; /* Adjust text color */
    text-decoration: none;
}

.filter-dropdown .dropdown-menu a:hover {
    background-color: #f0f0f0; /* Highlight on hover */
}

        .dropdown-toggle::after {
            position: absolute;
            right: 12px;
            top: 50%;
            transform: translateY(-50%);
        }

        .map-filter-dropdown {
            min-width: 180px;
        }

        .portal-dropdown {
            min-width: 200px;
        }

        .deployment-section {
            margin-left: auto;
            text-align: right;
        }

        @media (max-width: 992px) {
            .filter-container {
                gap: 5px;
            }

            .filter-dropdown {
                min-width: 100%;
            }
        }
        .deployment-section {
    display: flex;
    align-items: center; /* Vertically center-aligns the items */
    justify-content: flex-start; /* Aligns items to the left */
    gap: 10px; /* Optional: Adds space between elements */
}

.deployment-text {
    margin-right: 20px; /* Adjust space between the text and the buttons */
}

.deployment-text b {
    font-weight: bold;
}

.disasterEvent-dropdown {
    position: relative;
    display: inline-block;
}  
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row form-group no-gutter">
        <div class="col-xs-12">
          <div class="filter-container">
    <!-- Map Filter Dropdown -->
    <div class="filter-dropdown map-filter-dropdown">
        <button id="btn-mapDropDown" class="btn btn-outline btn-default dropdown-toggle"
            type="button" data-toggle="dropdown">
            Modify Map Filter <i class="fa fa-sort-down"></i>
        </button>
        <ul id="ddlMapFilter" class="dropdown-menu">
            <li id="All" selected="True"><a href="#">All Locations</a></li>
            <li id="Community"><a href="#">Stability.org Deployments</a></li>
            <li id="Critical"><a href="#">Critical Facilities</a></li>
            <%=liCases%>
        </ul>
    </div>

    <!-- Location Type Dropdown -->
    <div class="filter-dropdown">
        <button id="btn-locationType" class="btn btn-outline btn-default dropdown-toggle"
            type="button" data-toggle="dropdown">
            Location Type <i class="fa fa-sort-down"></i>
        </button>
        <ul id="locationTypeFilter" class="dropdown-menu">
            <li id=""><a href="#">All Location Types</a></li>
            <%=locationTypeDropDown%>
        </ul>
    </div>

    <!-- Parent Type Dropdown -->
    <div class="filter-dropdown">
        <button id="btn-parentType" class="btn btn-outline btn-default dropdown-toggle"
            type="button" data-toggle="dropdown">
            Location Parent Type <i class="fa fa-sort-down"></i>
        </button>
        <ul id="parentTypeFilter" class="dropdown-menu">
            <li id=""><a href="#">All Parent Types</a></li>
            <%=locationParentTypeDropDown%>
        </ul>
    </div>

    <!-- Status Dropdown -->
    <div class="filter-dropdown">
        <button id="btn-status" class="btn btn-outline btn-default dropdown-toggle"
            type="button" data-toggle="dropdown">
            Location Status <i class="fa fa-sort-down"></i>
        </button>
        <ul id="statusFilter" class="dropdown-menu">
            <li id=""><a href="#">All Statuses</a></li>
            <%=locationStatusDropDown%>
        </ul>
    </div>

    <!-- Community Portal Dropdown -->
    <div class="filter-dropdown">
        <button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer"
            type="button" data-toggle="dropdown">Change Community Portals <i class="fa fa-sort-down"></i>
        </button>
        <ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
            <%=disasterDropDown%>
        </ul>
    </div>

    <div class="deployment-section">
        <div class="deployment-text">
            <b>Share Your Team Deployment</b>
        </div>
        <asp:HyperLink ID="hypCreateDeployment" CssClass="btn btn-info btn-md" 
            runat="server" Text="Add Deployment"></asp:HyperLink>
        <asp:HyperLink ID="hypSignIn" CssClass="btn btn-info btn-md" runat="server"
            Text="Sign In" NavigateUrl="/SignIn" Visible="false"></asp:HyperLink>
    </div>
</div>
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
    <div style="height: 200px;"></div>
</asp:Content>
