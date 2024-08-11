<%@ page title="" language="C#" masterpagefile="~/CaseManagement/MasterPages/CaseManagement.master" autoeventwireup="true" inherits="CaseManagement_Default, App_Web_5cuksoem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/jquery-flot/jquery.flot.js"></script>
	<script src="/Homer/vendor/jquery-flot/jquery.flot.resize.js"></script>
	<script src="/Homer/vendor/jquery-flot/jquery.flot.pie.js"></script>
	<script src="/Homer/vendor/flot.curvedlines/curvedLines.js"></script>
	<script src="/Homer/vendor/jquery.flot.spline/index.js"></script>
	
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<link rel="stylesheet" href="/V1/Scripts/map-icons-master/dist/css/map-icons.css" />
	<script>

		$(document).ready(function () {
			$(function () {

				// Initialize Example 2
				$('#survivorTable').footable();
			});

			<%=preselectedDisasterJQuery%>

			$("#disasterEvent.dropdown-menu li").click(function () {
				var urlFriendlyName = $(this).attr('urlName');
				window.location.replace(urlFriendlyName);
			});


			$(function () {

				/**
				 * Flot charts data and options
				 */
				var data1 = [[0, 55], [1, 48], [2, 40], [3, 36], [4, 40], [5, 60], [6, 50], [7, 51]];
				var data2 = [[0, 56], [1, 49], [2, 41], [3, 38], [4, 46], [5, 67], [6, 57], [7, 59]];

				var chartUsersOptions = {
					series: {
						splines: {
							show: true,
							tension: 0.4,
							lineWidth: 1,
							fill: 0.4
						},
					},
					grid: {
						tickColor: "#f0f0f0",
						borderWidth: 1,
						borderColor: 'f0f0f0',
						color: '#6a6c6f'
					},
					colors: ["#62cb31", "#efefef"],
				};

				$.plot($("#flot-line-chart"), [data1, data2], chartUsersOptions);
			});
			initMap();
		});

		var map;

		function initMap() {

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

			var labels = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
			var labelIndex = 0;

			map.data.loadGeoJson('<%=host%>/V1/Handlers/GetGeoJsonByDisasterForAgencyMap.ashx?eventId=<%=eventId%>', null, function (features) { });

			map.data.addListener('click', function (event) {
				var feat = event.feature;
				var html = "<div class='hpanel m-xs hgreen'><div class='panel-body m-xs'><div class='col-sm-6' style='width:200px;'>"
					+ "<b>" + feat.getProperty('FullName') + "</b></br>"
					+ "<b><i>" + feat.getProperty('Sentence') + " " + feat.getProperty('Housing') + "</i></b></br></br>"
					+ feat.getProperty('Address') + "</br>"
					+ feat.getProperty('City') + "</br>"
					+ feat.getProperty('State') + " " + feat.getProperty('Zip') + " (" + feat.getProperty('County') + "</br></br>"
					+ "<a style='border:none;' href='/case/" + feat.getProperty(' ProfileNumber') + "/" + feat.getProperty('FullName') + "' target = '_blank' > View Details</a >"
					+ "</div><div class='col-sm-6' style='width:100px;'>" + feat.getProperty('Note').replace("u003cpu003e", "").replace("u003c/pu003e","") + "</div></div></div>";

				infowindow.setContent(html);
				infowindow.setPosition(event.latLng);
				infowindow.setOptions({ pixelOffset: new google.maps.Size(0, -34) });
				infowindow.open(map);
			});
		}
		function unicodeToChar(text) {
			return text.replace(/\\u[\dA-F]{4}/gi,
				function (match) {
					return String.fromCharCode(parseInt(match.replace(/\\u/g, ''), 16));
				});
		}
		function removeTags(str) {
			if ((str === null) || (str === ''))
				return false;
			else
				str = str.toString();

			// Regular expression to identify HTML tags in 
			// the input string. Replacing the identified 
			// HTML tag with a null string.
			return str.replace(/(<([^>]+)>)/ig, '');
		}
	</script>
	<script src="https://maps.googleapis.com/maps/api/js?key=<%=mapApiKey%>"></script>
	 <style>
       /* Set the size of the div element that contains the map */
      #map {
        height: 600px;  /* The height is 400 pixels */
        width: 100%;  /* The width is the width of the web page */
       }

	   .map-icon-label .map-icon {
			font-size: 24px;
			color: #FFFFFF;
			line-height: 48px;
			text-align: center;
			white-space: nowrap;
		}
	   a{
		   outline:none !important;
		   border: none !important;
		}
	</style>
	<asp:HiddenField runat="server" id="hidEventId"></asp:HiddenField>
	
	<div class="row m-xs" data-role="page">
		<div class="col-sm-12">
			<div class="hpanel m-xs">
				<div class="panel-body m-xs">
					<h2 class="font-light m-xs">
						<div id="div1" class="dropdown m-b-md btn" runat="server">
							<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Choose The Disaster <i class="fa fa-sort-down"></i></button>
							<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
								<%=disasterDropDown%>
							</ul>
							<asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" Text="Add Survivor" OnClick="btnNewClientIntakeSubmit_Click" />
						</div>
					</h2>
				</div>
			</div>
		</div>
	</div>
	<div class="row m-xs" data-role="page">
		<div class="col-sm-4">
			<div class="animate-panel" data-child="hpanel" data-effect="fadeInDown">
				<div class="hpanel" runat="server" id="divAddSurvivorButton" visible="false">
					<h3>No survivors yet.</h3>
					Click here to start helping someone affected by disaster. <a href="AddCase.aspx">Add Survivior</a>
				</div>
				<div class="hpanel" runat="server" id="divSurvivorList" visible="false">
					<div class="panel-body">
						Filter list.
						<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table"/>
						<asp:Repeater ID="rpSurvivorListTable" runat="server" OnItemDataBound="rpSurvivorListTable_ItemDataBound">
							<HeaderTemplate>
								<table id="survivorTable" class="footable table table-bordered table-hover toggle-arrow-tiny" data-page-size="6" data-filter="#filter">
									<thead>
										<tr>
											<th data-toggle="true">Case ID</th>
											<th data-toggle="true">Name</th>
											<th data-toggle="true">Address</th>
											<th data-toggle="true" data-hide="all">Living Situation</th>
											<th data-toggle="true" data-hide="all">Qualifiers</th>
											<th data-toggle="true" data-hide="all">Recovery Stage</th>
											<th data-toggle="true" data-hide="all">Phone Number</th>
											<th data-toggle="true" data-hide="all">Email Address</th>
											<th data-toggle="true" data-hide="all">Latest Note</th>
											<th data-toggle="true">Added By</th>
										</tr>
									</thead>
									<tbody>
							</HeaderTemplate>
							<ItemTemplate>
										<tr>
											<td><asp:Literal ID="litProfileNumber" runat="server"></asp:Literal></td>
											<td><asp:HyperLink ID="hypName" runat="server"></asp:HyperLink></td>
											<td><asp:HyperLink ID="hypAddress" runat="server"></asp:HyperLink></td>
											<td><asp:Literal ID="litLivingSituation" runat="server"></asp:Literal></td>
											<td class="text-muted">
												<asp:Literal ID="litSurvivorQualifiers" runat="server"></asp:Literal>
											</td>
											<td><asp:Literal ID="litRecoveryStage" runat="server"></asp:Literal></td>
											<td><asp:HyperLink ID="hypPhone" runat="server"></asp:HyperLink></td>
											<td><asp:HyperLink ID="hypEmailAddress" runat="server"></asp:HyperLink></td>
											<td><asp:Literal ID="litNote" runat="server"></asp:Literal></td>
											<td><asp:Literal ID="litAddedBy" runat="server"></asp:Literal></td>
										</tr>
							</ItemTemplate>
							<FooterTemplate>
									</tbody>
									<tfoot>
										<tr>
											<td colspan="8">
												<ul class="pagination pull-right"></ul>
											</td>
										</tr>
									</tfoot>
								</table>
							</FooterTemplate>
						</asp:Repeater>
					</div>
				</div>
			</div>
		</div>
		<div class="col-sm-8" data-role="page">
			<div class="hpanel" data-role="page">
				<div class="panel-body">
					<div id="map"></div>
				</div>
			</div>
		</div>
	</div>
    <div class="row">
        <div class="col-lg-12">
			<div class="hpanel">
				<div class="panel-heading">
					<div class="panel-tools">
						<a class="showhide"><i class="fa fa-chevron-up"></i></a>
						<a class="closebox"><i class="fa fa-times"></i></a>
					</div>
					Dashboard information and statistics
				</div>
				<div class="panel-body">
					<div class="row">
						<div class="col-md-2 text-center">
							<div class="small">
								<i class="fa fa-clock-o"></i> Hurricane Ida (<asp:Literal ID="litEventDate3" runat="server"></asp:Literal>)
							</div>
							<div>
								<h1 class="font-extra-bold m-t-xl m-b-xs">
									<asp:Literal ID="litEventTimeLapse" runat="server"></asp:Literal>
								</h1>
								<small><asp:Literal ID="litEventActive" runat="server"></asp:Literal></small>
							</div>
							<div class="small m-t-xl">
								<i class="fa fa-clock-o"></i> Last update 
							</div>
						</div>
						<div class="col-md-2 text-center">
							<div class="small">
								<i class="fa fa-bolt"></i> Total Cases
							</div>
							<div>
								<h1 class="font-extra-bold m-t-xl m-b-xs">
									<asp:Literal ID="litTotalCases" runat="server"></asp:Literal>
								</h1>
							</div>
							<div class="small m-t-xl">
								<i class="fa fa-clock-o"></i> Data since <asp:Literal ID="litEventDate1" runat="server"></asp:Literal>
							</div>
						</div>
						<div class="col-md-2 text-center">
							<div class="small">
								<i class="fa fa-bolt"></i> Critical Cases
							</div>
							<div>
								<h1 class="font-extra-bold m-t-xl m-b-xs">
									<asp:Literal ID="litCriticalCases" runat="server"></asp:Literal>
								</h1>
								<small>Individuals who are homeless or living in a tent, car, shelter or damanged home.</small>
							</div>
							<div class="small m-t-xl">
								<i class="fa fa-clock-o"></i> Data since <asp:Literal ID="litEventDate2" runat="server"></asp:Literal>
							</div>
						</div>
						<div class="col-md-6">
							<div class="text-center small">
								<i class="fa fa-laptop"></i> Client Intake Trends by Month
							</div>
							<div class="flot-chart" style="height: 160px">
								<div class="flot-chart-content" id="flot-line-chart"></div>
							</div>
						</div>
					</div>
				</div>
				<div class="panel-footer">
				</div>
			</div>
		</div>
	</div>
	<div class="row">
        <div class="col-lg-3">
            <div class="hpanel">
                <div class="panel-body text-center h-200">
                    <i class="pe-7s-compass fa-4x"></i>
                     <h1 class="font-extra-bold no-margins">36 Agencies</h1>
                    <small>Total responding non-profts and NGO's</small>
                </div>
                <div class="panel-footer">
                    2 new agencies today. 4 new agencies this week. 
                </div>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="hpanel stats">
                <div class="panel-body h-200">
                    <div class="stats-title pull-left">
                        <h4>Impacted Homes</h4>
                    </div>
                    <div class="stats-icon pull-right">
                        <i class="pe-7s-home fa-4x"></i>
                    </div>
                    <div class="m-t-xl">
                        <h1 class="text-warning font-extra-bold">14,431</h1>
						<span class="font-bold no-margins">
							FEMA REPORTED
						</span>
                        <br/>
                        <small>
                            Last update provided via the regional VOAD call.
                        </small>
                    </div>
                </div>
                <div class="panel-footer">
                    56 homes reported today. 372 homes reported this week. 
                </div>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="hpanel stats">
                <div class="panel-body h-200">
                    <div class="stats-title pull-left">
                        <h4>Recovery Progress</h4>
                    </div>
                    <div class="stats-icon pull-right">
                        <i class="pe-7s-share fa-4x"></i>
                    </div>
                    <div class="m-t-xl">
                        <h3 class="m-b-xs">210</h3>
						<span class="font-bold no-margins">
							Case Updates
						</span>
                        <div class="progress m-t-xs full progress-small">
                            <div style="width: 55%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="55"
                                    role="progressbar" class=" progress-bar progress-bar-success">
                                <span class="sr-only">35% Complete (success)</span>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6">
                                <small class="stats-label">Open Cases</small>
                                <h4>871</h4>
                            </div>

                            <div class="col-xs-6">
                                <small class="stats-label">Closed Cases</small>
                                <h4>766</h4>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    This is standard panel footer
                </div>
            </div>
        </div>
        <div class="col-lg-3">
            <div class="hpanel stats">
                <div class="panel-body h-200">
                    <div class="stats-title pull-left">
                        <h4>Resources Identified</h4>
                    </div>
                    <div class="stats-icon pull-right">
                        <i class="pe-7s-help2 fa-4x"></i>
                    </div>
                    <div class="clearfix"></div>
                    <div class="flot-chart">
                        <div class="flot-chart-content" id="flot-income-chart"></div>
                    </div>
                    <div class="m-t-xs">

                        <div class="row">
                            <div class="col-xs-5">
                                <small class="stat-label">Today</small>
                                <h4>$230,00 </h4>
                            </div>
                            <div class="col-xs-7">
                                <small class="stat-label">Last week</small>
                                <h4>$7 980,60 <i class="fa fa-level-up text-success"></i></h4>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="panel-footer">
                    This is standard panel footer
                </div>
            </div>
        </div>
    </div>
	<div class="row h-600"></div>
	<script src="/V1/Scripts/map-icons-master/dist/js/map-icons.js"></script>
</asp:Content>