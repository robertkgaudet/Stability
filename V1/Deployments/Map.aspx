<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Portal.master" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="V1_Deployments_Map" %>

<%@ MasterType VirtualPath="~/V1/MasterPages/Portal.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        var ddlChange = true;
        $(document).ready(function () {

            // Initialize with stored filter or default to Critical
            const storedFilter = "Critical";
            updateFilterStates(storedFilter);

            // Handle map filter changes
            $("#ddlMapFilter.dropdown-menu li").click(function () {
                const mapFilterType = $(this).attr('id');
                updateFilterStates(mapFilterType);
            });

            function updateFilterStates(mapFilterType) {
                // Always keep dropdown enabled
                $("#btn-mapDropDown").removeClass("disabled").prop("disabled", false);

                // No longer disable additional filters based on selection
                // So we remove this block entirely:
                // $(".additional-filter")
                //     .toggleClass("disabled", !isCritical)
                //     .find("button")
                //     .prop("disabled", !isCritical);

                // Just update the button text
                const $selectedItem = $(`#ddlMapFilter.dropdown-menu li[id="${mapFilterType}"]`);
                if ($selectedItem.length) {
                    $("#btn-mapDropDown").html($selectedItem.text() + ' <i class="fa fa-sort-down"></i>');
                }
            }


            // V4: State management constants
            const STORAGE_KEYS = {
                MAP_FILTER: 'mapFilterType',
                LOCATION_TYPE: 'locationTypeId',
                PARENT_TYPE: 'parentTypeId',
                STATUS: 'statusId'
            };
            const DEFAULT_FILTER = "Critical";

            // Initialize UI
            $("#btn-dropdown.disasterEvent").html('<%=_eventName%>');

        // Redirect handler remains unchanged
            $("#disasterEvent.dropdown-menu li").click(function (event) {
                var locationval = null;
                var parentval = null;
                var statusval = null;
                window.location.href = "/Maps/" + $(this).attr('name') +
                    "?parentlocationtype=" + encodeURIComponent(parentval || '') +
                    "&locationtype=" + encodeURIComponent(locationval || '') +
                    "&status=" + encodeURIComponent(statusval || '');
                event.preventDefault();
        });

        // Initialize with stored state or defaults
        initMap(DEFAULT_FILTER);
        updateDropdownDisplays();
       
        // Map filter handler with state management
        $("#ddlMapFilter.dropdown-menu li").click(function (event) {
            const mapFilterType = $(this).attr('id');
            $("#btn-mapDropDown.ddlMapFilter").html($(this).text());

            // Apply filters only for Critical view
            initMapWithFilters(mapFilterType, null, null, null);

            event.preventDefault();
        });
             
        function updateDropdownDisplays() {
            // Update main filter display
            const currentFilter = DEFAULT_FILTER;
            $(`#ddlMapFilter.dropdown-menu li[id="${currentFilter}"]`).trigger('click');

            
            
        }
            
         $(document).on('click', '#ddlLocationType.dropdown-menu li', function (event) {
            handleFilterClick(STORAGE_KEYS.LOCATION_TYPE, '#btn-locationType.locationTypeFilter').call(this, event);
        });

        $(document).on('click', '#ddlParentType.dropdown-menu li', function (event) {
            handleFilterClick(STORAGE_KEYS.PARENT_TYPE, '#btn-parentType.parentTypeFilter').call(this, event);
        });

        $(document).on('click', '#ddlStatus.dropdown-menu li', function (event) {
            handleFilterClick(STORAGE_KEYS.STATUS, '#btn-status.statusFilter').call(this, event);
        });
        function handleFilterClick(storageKey, buttonSelector) {
            ddlChange = false;
            return function (event) {
                var filterValue = $(this).attr('id') == 'null' ? '': $(this).attr('id');
                $(buttonSelector).attr('selData', filterValue);
                $(buttonSelector).html($(this).text());

                var ltId = $('#btn-locationType.locationTypeFilter').attr('selData');
                var pltId = $('#btn-parentType.parentTypeFilter').attr('selData');
                var lsId = $('#btn-status.statusFilter').attr('selData');
                // Only refresh if in Critical mode
                if (DEFAULT_FILTER === "Critical") {
                    initMapWithFilters(
                        DEFAULT_FILTER,
                        ltId == ''? null : ltId,
                        pltId == ''? null : pltId, 
                        lsId == ''? null : lsId
                    );
                }
                event.preventDefault();
            };
        }

        // Existing map functions remain exactly the same below this point
        var map;
        let GEOJsonPath = '<%=_mapGEOJsonPath%>';

        function initMap(mapFilterType) {
            // Clear non-Critical filters when switching modes
            if (mapFilterType !== "Critical") {
                [STORAGE_KEYS.LOCATION_TYPE, STORAGE_KEYS.PARENT_TYPE, STORAGE_KEYS.STATUS].forEach(key => {
                    sessionStorage.removeItem(key);
                });
            }
            initMapWithFilters(
                mapFilterType,
                null,
                null,
                null
            );
        }


        function initMapWithFilters(mapFilterType, locationTypeId, parentTypeId, statusId) {
            if (mapFilterType !== "Critical") {
                locationTypeId = null;
                parentTypeId = null;
                statusId = null;
            }

            $('#list').empty();
            var infowindow = new google.maps.InfoWindow();

            // Clear existing features if map already exists
            if (map) {
                map.data.forEach(function (feature) {
                    map.data.remove(feature);
                });
            } else {
                // Initialize map if it doesn't exist
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
            }

            // Build the GeoJSON URL with filters
            var mapsURL = '/V1/Handlers/GetGeoJsonByDisaster.ashx?keyId=<%=_eventId%>&mapFilterType=' + (mapFilterType || "Critical");

            // Only add additional parameters if in Critical mode
            if (mapFilterType === "Critical") {
                if (locationTypeId) mapsURL += '&locationTypeId=' + locationTypeId;
                if (parentTypeId) mapsURL += '&parentTypeId=' + parentTypeId;
                if (statusId) mapsURL += '&statusId=' + statusId;
            }

            // Load GeoJSON data from your .ashx handler
            map.data.loadGeoJson(mapsURL, null, function (features) {
                // Once the data is loaded, generate the list
                const listContainer = document.getElementById('list');
                var locationTypeDdl = '';
                var locationTypeIds = [];
                var parentLocationTypeDdl = '';
                var parentLocationTypeIds = [];
                var statusDdl = '';
                var statusIds = [];
                //Create the list 
                // Iterate through each feature in the GeoJSON data
                features.forEach(function (feature) {
                    // Get properties with fallbacks
                    const address = feature.getProperty('Address') || 'Address not available';
                    const icon = feature.getProperty('icon')
                        ? '<img src="' + feature.getProperty('icon') + '" class="pull-left" style="width:35px; margin-right:10px;">'
                        : '';

                    // FIXED: Always use LocationName/LocationType (your data structure)
                    const campaignName = feature.getProperty('LocationName') || 'Location name not available';
                    const organizationName = feature.getProperty('LocationType') || 'Type not specified';
                    const label1 = "Location Type";

                    // FIXED: Only show contact if data exists
                    var contact = '';
                    const contactName = feature.getProperty('PointOfContactName');
                    const phoneNumber = feature.getProperty('PhoneNumber');
                    if (contactName) {
                        contact = '<p><strong>Contact:</strong> ' + contactName;
                        if (phoneNumber) contact += '</br><strong>Phone:</strong> ' + phoneNumber;
                        contact += '</p>';
                    }
                    const pltId = feature.getProperty('LocationParentTypeId') || '';
                    const pltName = feature.getProperty('LocationType') || '';
                    const ltId = feature.getProperty('LocationTypeId') || '';
                    const ltName = feature.getProperty('LocationTypeName') || '';
                    const lsName = feature.getProperty('Status') || 'All';
                    const lsId = feature.getProperty('LocationStatusId') || 'null';
                    if(pltId != '' && pltName != '') {
                        if(!parentLocationTypeIds.includes(pltId)) {
                            parentLocationTypeIds.push(pltId);
                            parentLocationTypeDdl += "<li id='" + pltId + "'><a href='#'>" + pltName + "</a></li>";
                        }
                        
                    }
                    if(ltId != '' && ltName != '') {
                        if(!locationTypeIds.includes(ltId)) {
                            locationTypeIds.push(ltId);
                            locationTypeDdl += "<li id='" + ltId + "'><a href='#'>" + ltName + "</a></li>";
                        }
 
                    }
                     if(lsId != '' && lsName != '') {
                         if(!statusIds.includes(lsId)) {
                             statusIds.push(lsId);
                             statusDdl += "<li id='" + lsId + "'><a href='#'>" + lsName + "</a></li>";
                         }
 
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


                    listItem.addEventListener('click', () => {
                        map.setCenter(coordinates);
                        map.setZoom(15);
                    });
                });
                if(ddlChange) {
                    $('#ddlParentType').empty();
                    $('#ddlParentType').append(parentLocationTypeDdl);
                
                    $('#ddlLocationType').empty();
                    $('#ddlLocationType').append(locationTypeDdl);

                    $('#ddlStatus').empty();
                    $('#ddlStatus').append(statusDdl);
                }
                
                
            });

            //Load the info window when a user clicks on it.
            switch (mapFilterType) {
                
                case "Cases":
                    map.data.addListener('click', function (event) {
                        var feat = event.feature;
                        var formattedAddress = feat.getProperty('Address');
                        var isActive = feat.getProperty('IsActive');
                        var campaignName = feat.getProperty('CampaignName');
                        var URLFriendlyCampaignName = feat.getProperty('URLFriendlyCampaignName');
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
                            "</div >";

                        infowindow.setContent(html);
                        infowindow.setPosition(event.latLng);
                        infowindow.open(map);
                    });
                    break;
                case "Community":
                    map.data.addListener('click', function (event) {
                        var feat = event.feature;
                        var formattedAddress = feat.getProperty('Address');
                        var isActive = feat.getProperty('IsActive');
                        var campaignName = feat.getProperty('CampaignName');
                        var URLFriendlyCampaignName = feat.getProperty('URLFriendlyCampaignName');
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
                            "</div >";

                        infowindow.setContent(html);
                        infowindow.setPosition(event.latLng);
                        infowindow.open(map);
                    });
                    break;
                case "All":
                    map.data.addListener('click', function (event) {
                        var feat = event.feature;
                        var html = "<div class='col-sm-12'> <b><a style='text-decoration:underline;' target='_blank' href='/V1/Location.aspx?locationProfileId=" + feat.getProperty('locationProfileId') + "'>"
                            + feat.getProperty('LocationName') + "</a></b></br>"
                            + feat.getProperty('Address') + "</br></br>"
                            + "<b>" + feat.getProperty('LocationType') + "</b></br>"
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
                            + feat.getProperty('LocationName') + "</a></b></br>"
                            + feat.getProperty('Address') + "</br></br>"
                            + "<b>" + feat.getProperty('LocationType') + "</b></br>"
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
                    break;
                case "Professional":
                    break;
                default:
            }
        }
    });
        $(document).ready(function () {
            $(".additional-filter").show();
            $("#ddlMapFilter li").on("click", function (e) {
                e.preventDefault();
                var selectedId = $(this).attr("id");
                $("#btn-mapDropDown").html($(this).text() + ' <i class="fa fa-sort-down"></i>');
                if (selectedId === "Critical") {
                    $(".additional-filter").show();
                } else {
                    $(".additional-filter").hide();
                }
            });
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

        /* Smooth transitions */
        .filter-dropdown {
            transition: all 0.3s ease;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row form-group no-gutter">
        <div class="col-xs-12">
            <div class="filter-container">
                <div class="filter-dropdown map-filter-dropdown">
                    <button id="btn-mapDropDown" class="btn btn-outline btn-default ddlMapFilter dropdown-toggle dropdown-map-filter" type="button" data-toggle="dropdown">Modify Map Filter <i class="fa fa-sort-down"></i></button>
                    <ul id="ddlMapFilter" class="dropdown-menu text-center dropdown-map-filter">
                        <li id="All" selected="True"><a href="#">All Locations</a></li>
                        <li id="Community"><a href="#">Stability.org Deployments</a></li>
                        <li id="Critical"><a href="#">Critical Facilities</a></li>
                        <%=liCases%>
                    </ul>
                </div>
                 <div class="filter-dropdown">
                     <button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Change Community Portals <i class="fa fa-sort-down"></i></button>
                     <ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
                         <%=disasterDropDown%>
                     </ul>
                 </div>

                <!-- New Parent Type Filter -->
                <div class="filter-dropdown additional-filter">
                    <button id="btn-parentType" selData="" class="btn btn-outline btn-default parentTypeFilter dropdown-toggle dropdown-map-filter" type="button" data-toggle="dropdown">Parent Types <i class="fa fa-sort-down"></i></button>
                    <ul id="ddlParentType" class="dropdown-menu text-center dropdown-map-filter">
                        
                    </ul>
                </div>

                <!-- New Location Type Filter -->
                <div class="filter-dropdown additional-filter">
                    <button id="btn-locationType" selData="" class="btn btn-outline btn-default locationTypeFilter dropdown-toggle dropdown-map-filter" type="button" data-toggle="dropdown">Location Types <i class="fa fa-sort-down"></i></button>
                    <ul id="ddlLocationType" class="dropdown-menu text-center dropdown-map-filter">
                        
                    </ul>
                </div>


                <!-- New Status Filter -->
                <div class="filter-dropdown additional-filter">
                    <button id="btn-status" selData="" class="btn btn-outline btn-default statusFilter dropdown-toggle dropdown-map-filter" type="button" data-toggle="dropdown">Status <i class="fa fa-sort-down"></i></button>
                    <ul id="ddlStatus" class="dropdown-menu text-center dropdown-map-filter">
                        
                    </ul>
                </div>

               
                <div class="deployment-section">

                    <b>Share Your Team Deployment</b>

                    <asp:HyperLink ID="hypCreateDeployment" CssClass="btn m-l-md btn-info btn-md pull-right" runat="server"></asp:HyperLink>
                    <asp:HyperLink ID="hypSignIn" CssClass="btn btn-info btn-md pull-right" runat="server" Text="Sign In" NavigateUrl="/SignIn" Visible="false"></asp:HyperLink>
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
