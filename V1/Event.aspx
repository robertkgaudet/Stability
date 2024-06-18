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
        
		function updateDefaultDisaster(eventId) {
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
			$(function () {
				$('#nonProfitsTable').footable();
			});

			$("#disasterEvent.dropdown-menu li").click(function () {
				window.location.href = "/Disaster/" + $(this).attr('name');
				});
            
			$('#<%=btnSetDefaultDisaster.ClientID%>').click(function () {
                updateDefaultDisaster('<%=eventId%>');
                $('#<%=lblDefaultEventMessage.ClientID%>').text("Your default portal has been updated.");
                $('#<%=btnSetDefaultDisaster.ClientID%>').hide();
				return false;
			})

			$('.btnRegisterNonProfit').click(function () {
				window.location.href = '<%=registerNonProfit%>';
				return false;
			})
			
			$('.volunteerPassed').click(function () {
				window.location.href = '/V1/Stream.aspx';
				return false;
			})

			$('.volunteerPending').click(function () {
				window.location.href = '/V1/Profile/Profile.aspx';
				return false;
			});

			$('.btnAddHome').click(function () {
				window.location.href = '/V1/Profile/AddNewRebuild.aspx?eventId=<%=eventId%>';
				return false;
			})
			
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

			$('.grid').masonry({
				// options
				itemSelector: '.grid-item',
				gutter: 10
			});

			$('.activeUsersGrid').masonry({
				// options
				itemSelector: '.activeUsersGrid-item',
				gutter: 5
			});
			
			initMap();
		});

		var map;

		function initMap()
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

			var labels = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
			var labelIndex = 0;

            map.data.loadGeoJson('<%=host%>/V1/Handlers/GetGeoJsonByDisaster.ashx?eventId=<%=eventId%>', null, function (features){});

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
				//infowindow.setOptions({ pixelOffset: new google.maps.Size(0, -34) });
				infowindow.open(map);
			});
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
		
		.activeUsersGrid						{min-height:200px; margin-top:20px;}
		.activeUsersGrid-item					{width:180px; }
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
		.panelSurvivor:hover, .panelHelper:hover, .panelNonProfit:hover, .panelBusiness:hover
		{
			cursor:pointer;
			border:solid white 1px;
		}
		.profileDiv {
			  height:50px;
			}
		.shortDiv {
			height:25px;
		}
		.container-fluid
		{
			padding:0px;
		}
		.LinkButton
		{
			color:#FFFFFF !important;
            font-weight:normal !important;
            text-decoration:none !important;
		}
        .tab-background
        {
			background-color:#F7F9FA;
			border-top-left-radius: 5px;  /* Adjust the value as needed */
			border-top-right-radius: 5px; /* Adjust the value as needed */
			margin-right:3px;
        }
		.tab-content
		{
			background-color:white;
			padding:20px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
		<asp:HiddenField runat="server" id="hidEventId"></asp:HiddenField>
		<asp:HiddenField runat="server" id="hidEventName"></asp:HiddenField>
			<div class="form-group col-lg-12">
				<button id="btn-dropdown" class="btn btn-outline btn-default disasterEvent dropdown-toggle dropdown-volunteer" type="button" data-toggle="dropdown">Change Community Portals <i class="fa fa-sort-down"></i> </button>
				<ul id="disasterEvent" class="dropdown-menu text-center dropdown-volunteer required">
					<%=disasterDropDown%>
				</ul>
			</div>

			<uc1:EventHeader runat="server" ID="uc1EventHeader" />

			<div class="row">
				<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="row">
						<div class="col-sm-12">
							<div class="hpanel">
								<div class="hpanel">
									<ul class="nav nav-tabs">
										<li class="tab-background active"><a data-toggle="tab" href="#tab9" class="font-bold">Deployments</a></li>
										<li class="tab-background"><a data-toggle="tab" href="#tab11" class="font-bold">Relief Teams</a></li>
										<li class="tab-background"><a data-toggle="tab" href="#tab12" class="font-bold">Events</a></li>
										<li class="tab-background"><a data-toggle="tab" href="#tab10" class="font-bold">States and Counties</a></li>
										<li class="tab-background"><a data-toggle="tab" href="#tab8" class="font-bold">Community Links</a></li>
										<li class="tab-background" id="liTab7" visible="false" runat="server"><a data-toggle="tab" href="#<%=tab7.ClientID%>" class="font-bold">Case Management</a></li>
										<li class="tab-background" runat="server" visible="false" id="liAdminTab"><a data-toggle="tab" href="#<%=tab6.ClientID%>" class="font-bold">Administration</a></li>
										<li class="tab-background"><a data-toggle="tab" href="#tab3" visible="false" runat="server" class="font-bold">Survivor Stories</a></li>
										<li class="tab-background"><a data-toggle="tab" href="#tab4" visible="false" runat="server" class="font-bold">Statistics</a></li>
										<li class="tab-background"><a data-toggle="tab" href="#tab5" visible="false" runat="server" class="font-bold">Relief Finder</a></li>
										<li class="tab-background"><a data-toggle="tab" href="#tab1">Map</a></li>
									</ul>
									<div class="tab-content">
										<div id="tab1" class="tab-pane">
											<div class="row">
												<div class="col-lg-12">
													<div id="map"></div>
												</div>
											</div>
										</div>
										<div id="tab8" class="tab-pane">
											<div class="row">
												<div class="col-lg-12">
													<uc1:Links runat="server" ID="ucLinks" />
												</div>
											</div>
										</div>
										<div id="tab9" class="tab-pane active">
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
                                                                        <asp:HyperLink id="hypViewActivities" runat="server"></asp:HyperLink>
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
				</div>

				<%--<div class="col-sm-4">
					<div class="hpanel">
						<div class="panel-heading">
							Disaster Help Requests
						</div>
						<div class="panel-body no-padding">
							<div class="list-group">
								<a class="list-group-item active bg-danger" style="background-color:#d9534f;" href="<%=btnrescue%>" target="_blank">
									<h5 class="list-group-item-heading">Emergency Citizen Dispatch</h5>
									<span class="text-muted">
										<b class="list-group-item-text" style="color:white;">Request Urgent Rescue or Assistance</b>
										<p class="list-group-item-text" style="color:white;">Call 911 first! If they cannot help you, we will try. Note, we are a volunteer rescue organization and will do our best to help you. Use this for rescues, wellness checks, remote supply deliveries and assistance clearing debris.</p>
									</span>
								</a>
								<a class="list-group-item" href="/S1/Profile/AddNewSurvivor.aspx" target="_blank">
									<h5 class="list-group-item-heading">Survivor Registration</h5>
									<span class="text-muted">
										<b class="list-group-item-text">Survivors Get Started Here</b>
										<p class="list-group-item-text">Impacted by disaster? Register here to use the Stability resources that are available to survivors.</p>
									</span>
								</a>
								<a class="list-group-item" href="/V1/Profile/AddNewRebuild.aspx?eventId=<%=eventId%>" target="_blank">
									<h5 class="list-group-item-heading">Home Rebuild Tracking</h5>
									<span class="text-muted">
										<b class="list-group-item-text">Begin Recovery</b>
										<p class="list-group-item-text">Add a home profile to track and share your families home and personal recovery.</p>
									</span>
								</a>
								<a class="list-group-item" href="<%=btnrequestsupplies%>" target="_blank">
									<h5 class="list-group-item-heading">Supply Request</h5>
									<span class="text-muted">
										<b class="list-group-item-text">Find Things You Need</b>
										<p class="list-group-item-text">Send our team requests for supplies after the disaster and we will help you locate the things you need.</p>
									</span>
								</a>
							</div>
						</div>
					</div>
				</div>--%>
			</div>
		
			<!--HEADER FOR EVENT PAGE-->
			<div class="row m-t-md" runat="server" visible="false">
				<div class="col-sm-2 m-b-sm">
					<div class="hpanel hbgyellow panelSurvivor">
						<div class="panel-body">
							<div class="text-center">
								<h3>Survivors</h3>
								<span>
									Request Help, Rebuild or Supplies
								</span>
							</div>
						</div>
					</div>
				</div>
				<div class="col-sm-2 m-b-sm">
					<div class="hpanel hbggreen panelHelper">
						<div class="panel-body">
							<div class="text-center">
								<h3>Volunteer</h3>
								<span>
									Volunteer, Send Supplies, Donate
								</span>
							</div>
						</div>
					</div>
				</div>
				<div class="col-sm-2 m-b-sm">
					<div class="hpanel hbgviolet panelNonProfit">
						<div class="panel-body">
							<div class="text-center">
								<h3>Non-profits</h3>
								<span>
									Register your Non-profit Response
								</span>
							</div>
						</div>
					</div>
				</div>
				<div class="col-sm-2 m-b-sm">
					<div class="hpanel hbgnavyblue panelBusiness">
						<div class="panel-body">
							<div class="text-center">
								<h3>Business</h3>
								<span>
									Do Business or Request Help
								</span>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="alert" runat="server" id="divRegister" visible="false">
			To use all of the features of Stability please sign in or register. <i class="fa fa-user"></i> 
			<p class="m-t-sm">
				<a href="/Register" class="btn btn-success">Register Here</a>
				<a href="/SignIn" class="btn btn-success">Sign In</a>
			</p>	
		</div>
		<br /><br /><br /><br /><br /><br /><br />
		<script src="/V1/Scripts/map-icons-master/dist/js/map-icons.js"></script>
		<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
</asp:Content>