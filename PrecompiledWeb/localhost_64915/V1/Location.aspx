<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="V1_Location, App_Web_mjkl5wor" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style>
.map
{
	height:600px;
}
#divMap
{
	height:600px;
}
</style>
	<script type="text/javascript">
		$(document).ready(function () {

			$('.btnEditLocationProfile').click(function () {
				window.location.href = '/V1/Administration/Location/EditLocation.aspx?addressId=<%=addressId%>';
				return false;
			});
			$('.btnEditLocationRelationships').click(function () {
				window.location.href = '/V1/Administration/Location/EditLocationRelationships.aspx?addressId=<%=addressId%>&locationProfileId=<%=locationProfileId%>';
				return false;
			});
			$('.btnAddLocation').click(function () {
				window.location.href = '/V1/Administration/Location/AddLocation.aspx';
				return false;
			});

			$('.fa-dollar').click(function () {
				window.location.href = '<%=donationURL%>';
				return false;
			});

			$('.fa-facebook').click(function () {
				window.location.href = '<%=facebookURL%>';
				return false;
			});

			$('.fa-twitter').click(function () {
				window.location.href = 'https://www.twitter.com/<%=twitterUsername%>';
				return false;
			});
			$('.fa-globe').click(function () {
				window.location.href = '<%=websiteURL%>';
				return false;
			});
			$('.fa-instagram').click(function () {
				window.location.href = 'https://www.instagram.com/<%=instagramUsername%>';
				return false;
			});
			$('.fa-youtube').click(function () {
				window.location.href = '<%=youTubeURL%>';
				return false;
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<a class="small-header-action">
								<div class="clip-header">
									<i class="fa fa-arrow-up"></i>
								</div>
							</a>
							<h2 class="font-light m-b-xs">
								<asp:Literal id="litName" runat="server"></asp:Literal> - <asp:Literal id="litEvent" runat="server"></asp:Literal>
							</h2>
							<small>
								<asp:Literal id="litDescription" runat="server"></asp:Literal>
							</small>
						<div class="btn-group m-t-sm pull-right" runat="server" id="divAdministrationButtons" visible="false">
							<span class="btn btn-sm btnAddLocation btn-success m-r-sm" id="iAddLocation" runat="server" visible="false"> Add New Location</span>
							<span class="btn btn-sm btnEditLocationProfile btn-info m-r-sm" id="iEditLocationProfile" runat="server" visible="false"> Edit Location Profile</span>
							<span class="btn btn-sm btnEditLocationRelationships btn-info" id="iEditLocationRelationships" runat="server" visible="false"> Edit Location Relationships</span>
						</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			
		<div class="row">
			<div class="col-lg-5">
				<div class="hpanel hgreen">
					<div class="border-right border-left" style="height:300px;" runat="server" id="divMap">
						<iframe width="100%" height="100%" frameborder="0" style="border:0" src="https://www.google.com/maps/embed/v1/place?q=place_id:<%=googlePlacesId%>&key=<%=mapApiKey%>" allowfullscreen="true"></iframe>
					</div>
					<div class="panel-body">	
						<div class="text-muted">
							<dl>
								<dt>
									<asp:Literal id="litLocationName" runat="server"></asp:Literal>
								</dt>
								<dd class="m-b-md">
									<asp:Label CssClass="font-normal" id="lblStreetAddress" runat="server"></asp:Label>
									<br />
									<asp:Label CssClass="font-normal" id="lblCityStateZip" runat="server"></asp:Label>
									<br />
									<asp:Label CssClass="font-normal" id="lblCounty" runat="server"></asp:Label>
									<br />
									<asp:Label CssClass="font-normal" id="lblCountry" runat="server"></asp:Label>
									<br />
									<strong>Location Type: <asp:Literal id="litLocationType" runat="server"></asp:Literal></strong>
									<br />
									<strong>Currently: <asp:Literal id="litActivity" runat="server"></asp:Literal></strong>
									<br />
									<strong>Volunteers: <asp:Literal id="litVolunteers" runat="server"></asp:Literal></strong>
								</dd>
							</dl>
						</div>
						<div class="pull-right text-right">
							<div class="btn-group">
								<i class="fa fa-dollar btn btn-success btn-md" runat="server" id="liDonation"> DONATE TO THIS LOCATION</i>
								<i class="fa fa-facebook btn btn-default btn-md" runat="server" id="iFacebook"></i>
								<i class="fa fa-twitter btn btn-default btn-md" runat="server" id="iTwitter"></i>
								<i class="fa fa-instagram btn btn-default btn-md" runat="server" id="iInstagram"></i>
								<i class="fa fa-youtube btn btn-default btn-md" runat="server" id="iYouTube"></i>
								<i class="fa fa-globe btn btn-default btn-md" runat="server" id="iGlobe"></i>
							</div>
						</div>
					</div>	
					<div class="panel-footer contact-footer">
						<div class="row">
							<div class="col-xs-4 border-right">
								<div class="contact-stat"><span>Medical Help</span> <strong><asp:Literal id="litMedical" runat="server"></asp:Literal></strong></div>
							</div>
							<div class="col-xs-3 border-right">
								<div class="contact-stat"><span>Animals</span> <strong><asp:Literal id="litPets" runat="server"></asp:Literal></strong></div>
							</div>
							<div class="col-xs-3 border-right">
								<div class="contact-stat text-nowrap"><span>Status</span> <strong><asp:Literal id="litIsActve" runat="server"></asp:Literal></strong></div>
							</div>
							<div class="col-xs-2">
								<div class="contact-stat"><span>Capacity </span> <strong><asp:Literal id="litCapacity" runat="server"></asp:Literal></strong></div>
							</div>
						</div>
						<dl class="dl-horizontal">
							<dt>Services Provided</dt>
							<dd><asp:Literal id="litServicesProvided" runat="server"></asp:Literal></dd>
							<dt runat="server" id="dtEvent">Event</dt>
							<dd>
								<asp:Label CssClass="font-normal" runat="server" id="lblEvent">
									<asp:HyperLink Font-Underline="true" id="hypEvent" runat="server"></asp:HyperLink>
								</asp:Label>
							</dd>
							<dt>Description</dt>
							<dd>
								<asp:Label CssClass="font-normal" runat="server" id="divLocationDescription">
									<asp:Literal id="litLocationDescription" runat="server"></asp:Literal>
								</asp:Label>
							</dd>
							<dt><asp:Literal id="litEventType" runat="server"></asp:Literal> Begin Date</dt>
							<dd><asp:Literal id="litEventBeginDate" runat="server"></asp:Literal></dd>
							<dt>End Date</dt>
							<dd><asp:Literal id="litEventEndDate" runat="server"></asp:Literal></dd>
							<dt>Date Opened</dt>
							<dd><asp:Literal id="litDateOpened" runat="server"></asp:Literal></dd>
							<dt>Date Closed</dt>
							<dd><asp:Literal id="litDateClosed" runat="server"></asp:Literal></dd>
						</dl>
					</div>
					<div id="divHiddenMap" class="p-sm" runat="server" visible="false">
						<label class="text-info text-capitalize text-center center-block">MAP UNAVAILABLE</label>
					</div>
					<div class="panel-footer contact-footer m-b-lg">
						<div class="row">
							<div class="col-xs-3 border-right">
								<div class="contact-stat text-nowrap">
									<b>Lat:</b> <asp:Literal id="litLatitude" runat="server"></asp:Literal><br />
									<b>Lon:</b> <asp:Literal id="litLongitude" runat="server"></asp:Literal>
								</div>
							</div>
							<div class="col-xs-4 border-right">
								<div class="contact-stat">
									<b>Added By:</b> <asp:Literal id="litCreatedBy" runat="server"></asp:Literal><br />
									<b>Added On:</b> <asp:Literal id="litCreatedOn" runat="server"></asp:Literal>
								</div>
							</div>
							<div class="col-xs-5">
								<div class="contact-stat">
									<b>Updated By:</b> <asp:Literal id="litUpdatedBy" runat="server"></asp:Literal><br />
									<b>Updated On:</b> <asp:Literal id="litUpdatedOn" runat="server"></asp:Literal>
								</div>
							</div>
						</div>
					</div>
					<div class="panel-body">
						<dl class="dl-horizontal">
                    
								<dt>Point Of Contact</dt>
									<dd><asp:Label CssClass="font-normal" id="lblPointOfContactName" runat="server"></asp:Label></dd>
									<dd><asp:HyperLink Font-Underline="true" CssClass="font-normal" id="hypPointOfContactPhone" runat="server"></asp:HyperLink></dd>
									<dd class="m-b-md"><asp:HyperLink Font-Underline="true" CssClass="font-normal" id="hypPointOfContactEamil" runat="server"></asp:HyperLink></dd>
								<dt>Contact Information</dt>
									<dd><asp:HyperLink Font-Underline="true" CssClass="font-normal" id="hypPhone" runat="server"></asp:HyperLink></dd>
									<dd class="m-b-md"><asp:HyperLink Font-Underline="true" CssClass="font-normal" id="hypEmail" runat="server"></asp:HyperLink></dd>
						</dl>
					</div>
				</div>
			</div>
			<div class="col-lg-7">
				<div class="hpanel">
					<div class="hpanel">
						<ul class="nav nav-tabs">
							<li class="active"><a data-toggle="tab" href="#tab-1">Messages</a></li>
							<li><a data-toggle="tab" href="#tab-2">Events</a></li>
							<li><a data-toggle="tab" href="#tab-3">Activity History</a></li>
						</ul>
						<div class="tab-content">
							<div id="tab-1" class="tab-pane active">
								<div class="panel-body no-padding">

								
									<div class="panel-body no-padding m-t-lg">
										<div class="chat-discussion" style="height: auto">
												<asp:Image ID="imgNewPostLogo" runat="server" CssClass="post-logo" />
												<div class="message m-b-sm">
													<span class="message-content">
														<asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtPost" runat="server" placeholder="Post your comment here."></asp:TextBox>
													</span>
													<div style="overflow:auto;">
														<asp:Button ID="btnRebuildPost" OnClick="btnAddProfileNote_Click" CssClass="btn w-xs btn-sm btn-primary pull-right m-t-sm" runat="server" Text="Post" />
													</div>
												</div>
											<asp:Repeater ID="rptPosts" runat="server" OnItemDataBound="rptPosts_ItemDataBound">
												<ItemTemplate>
													<div class="hpanel media social-profile">
														 <a class="pull-left">
															<asp:Image id="imgProfilePhoto" runat="server"></asp:Image>
														</a>
															<div class="message">
																<div class="blog-article-box">
																<span class="message-date pull-right">
																	<asp:Literal id="litCreatedOn" runat="server"></asp:Literal>

																</span>
																<asp:HyperLink runat="server" id="hypFullname"></asp:HyperLink>
																</div>
																<span class="message-content m-t-sm">
																<%# DataBinder.Eval(Container.DataItem, "Note1") %>
																</span>
															</div>
													</div>
												</ItemTemplate>
											</asp:Repeater>
										</div>
									</div>

									<%--<div class="chat-discussion" style="height: auto">
										<div class="chat-message">
											<div class="social-form">
												<input class="form-control" placeholder="Your comment">
											</div>
										</div>
										<div class="chat-message">
											<img class="message-avatar" src="images/a1.jpg" alt="" >
											<div class="message">
												<a class="message-author" href="#"> Michael Smith </a>
												<span class="message-date"> Mon Jan 26 2015 - 18:39:23 </span>
														<span class="message-content">
														Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.
														</span>

												<div class="m-t-md">
													<a class="btn btn-xs btn-default"><i class="fa fa-thumbs-up"></i> Thank </a>
													<a class="btn btn-xs btn-default"><i class="fa fa-heart"></i> Donate</a>
													<a class="btn btn-xs btn-default"><i class="fa fa-handshake-o"></i> Offer Help</a>
												</div>
											</div>
										</div>
										<div class="chat-message">
											<img class="message-avatar" src="images/a4.jpg" alt="" >
											<div class="message">
												<a class="message-author" href="#"> Karl Jordan </a>
												<span class="message-date">  Fri Jan 25 2015 - 11:12:36 </span>
														<span class="message-content">
														Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover.
														</span>
												<div class="m-t-md">
													<a class="btn btn-xs btn-default"><i class="fa fa-thumbs-up"></i> Thank </a>
													<a class="btn btn-xs btn-default"><i class="fa fa-heart"></i> Donate</a>
													<a class="btn btn-xs btn-default"><i class="fa fa-handshake-o"></i> Offer Help</a>
												</div>
											</div>
										</div>
									</div>--%>
								</div>
							</div>
							<div id="tab-2" class="tab-pane">
								<div class="panel-body no-padding">
									<div class="chat-discussion" style="height: auto">
										<%=eventList%>
									</div>
								</div>
							</div>
							<div id="tab-3" class="tab-pane">
								<div class="panel-body">
									<div class="table-responsive">
										<table class="table table-striped">
											<thead>
												<tr>
													<th>Activity Status</th>
													<th>Updated By</th>
													<th>Updated On</th>
												</tr>
											</thead>
											<tbody>
												<%=activityStatus%>
											</tbody>
										</table>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>