<%@ page title="" language="C#" validaterequest="false" masterpagefile="~/CaseManagement/MasterPages/CaseManagement.master" autoeventwireup="true" inherits="CaseManagment_Case, App_Web_omisrjym" %>
<%@ MasterType VirtualPath="~/CaseManagement/MasterPages/CaseManagement.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote.css" />
    <link rel="stylesheet" href="/Homer/vendor/summernote/dist/summernote-bs3.css" />
	<script src="/Homer/vendor/summernote/dist/summernote.min.js"></script>
	<script>

		$(function () {

			$('.summernote').summernote({
				height: 150,
				toolbar: [

					['style', ['bold', 'italic', 'underline']],
					['alignment', ['ul', 'ol', 'paragraph']],
					['insert', ['link']],
					['misc', ['codeview']],
				]
			});
		});

	</script>

    <link rel="stylesheet" href="/Homer/vendor/sweetalert/lib/sweet-alert.css" />
	<script src="/Homer/vendor/iCheck/icheck.min.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.btnProfilePhoto').click(function () {
				window.location.href = '/V1/Profile/ProfilePhotoUpload.aspx?userId=<%=userId%>';
				return false;
			})
		});
	</script>

	<style>
		#<%=profileImageStyle%>

		.chkboxlist td 
		{
			font-size:medium;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:HiddenField id="hidProfileId" runat="server"></asp:HiddenField>
<div class="content">
	<div class="row">
		<div class="col-lg-12">
			<div class="hpanel">
				<div class="panel-body">
					<h2 class="font-light m-t-xs">
						<asp:Literal ID="litTitleName" runat="server"></asp:Literal>
					</h2>
					<div class="row">
                        <div class="col-lg-6 col-md-8 col-sm-12 col-xs-12">
							<div class="row">
								<div class="col-sm-2">
									<div class="project-label">CASE ID</div>
									<small class="font-extra-bold"><asp:Literal ID="litSmallProfileNumber" runat="server"></asp:Literal></small>
								</div>
								<div class="col-sm-2">
									<div class="project-label">DISASTER</div>
									<small class="font-extra-bold"><asp:Literal ID="litDisaster" runat="server"></asp:Literal></small>
								</div>
								<div class="col-sm-2">
									<div class="project-label">STAGE</div>
									<small class="font-extra-bold">Rebuild</small>
								</div>
								<div class="col-sm-2">
									<div class="project-label">CREATED</div>
									<small class="font-extra-bold">12.06.2015</small>
								</div>
								<div class="col-sm-2">
									<div class="project-label">HOME PROGRESS</div>
									<div class="progress m-t-xs full progress-small">
										<div style="width: 12%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="12" role="progressbar" class=" progress-bar progress-bar-success">
										</div>
									</div>
								</div>
								<div class="col-sm-2">
									<div class="project-label">HOUSEHOLD PROGRESS</div>
									<div class="progress m-t-xs full progress-small">
										<div style="width: 12%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="12" role="progressbar" class=" progress-bar progress-bar-success">
										</div>
									</div>
								</div>
							</div>
                        </div>
                        <div class="col-lg-6 col-md-4">
                        </div>
                    </div>
        		</div>
				<div class="panel-footer">
					<div class="row">
						<div class="col-xs-6">
							<div id="hbreadcrumb" class="pull-left">
								<ol class="hbreadcrumb breadcrumb">
									<li><a href="/CaseManagement/Default.aspx">My Dashboard</a></li>
									<li class="active">
										<span><asp:Literal ID="litCaseName" runat="server"></asp:Literal></span>
									</li>
								</ol>
							</div>
						</div>
						<div class="col-xs-6">
							<asp:HyperLink ID="hypWorkTicket" CssClass="btn-success btn btn-sm" runat="server">Create Work Ticket</asp:HyperLink>
							<asp:HyperLink ID="hypDisasterRegistrySurvey" CssClass="btn-success btn btn-sm" runat="server">Create an Items Wishlist</asp:HyperLink>
							<asp:HyperLink ID="hypSurvivorPhotos" CssClass="btn-success btn btn-sm" Text="Manage Photos" runat="server"></asp:HyperLink>
							<asp:HyperLink ID="hypmanageDocuments" CssClass="btn-success btn btn-sm" Text="Manage Documents" runat="server"></asp:HyperLink>
							<asp:HyperLink ID="hypAmazon" CssClass="btn-success btn btn-sm" runat="server" Text="View Amazon Wishlist"></asp:HyperLink>
						</div>
					</div>
				</div>
			</div>
		</div>
	
		<div class="col-lg-5">
			<div class="hpanel hblue">
				<div class="panel-heading hbuilt">
					Client Profile
				</div>
				<div class="panel-body">
					<ul class="nav nav-tabs">
						<li class="active"><a data-toggle="tab" href="#profile">Profile</a></li>
						<li><a data-toggle="tab" href="#vulnerabilities">Vulnerabilities</a></li>
						<li><a data-toggle="tab" href="#home">Home Data</a></li>
						<li><a data-toggle="tab" href="#photos">Photos</a></li>
						<li><a data-toggle="tab" href="#documents">Documents</a></li>
					</ul>
					<div class="tab-content">
						<div id="profile" class="tab-pane fade in active m-t-md">
							<dl class="dl-horizontal m-l-xs">
								<dt class="m-l-xs">
									Client Name
								</dt>
								<dd>
									<asp:Literal ID="litName" runat="server"></asp:Literal>
								</dd>
								<dt class="m-l-xs">
									Case ID
								</dt>
								<dd>
									<literal id="litProfileNumber" runat="server"></literal>
								</dd>
								<dt class="m-l-xs" runat="server" id="dtPhone" visible="false">
									Phone Number
								</dt>
								<dd runat="server" id="ddPhone" visible="false">
									<asp:Label id="lblPhoneNumber" runat="server"></asp:Label>
								</dd>
								<dt class="m-l-xs" runat="server" id="dtEmail" visible="false">
									Email Address
								</dt>
								<dd runat="server" id="ddEmail" visible="false">
									<asp:Label id="lblEmailAddress" runat="server"></asp:Label>
								</dd>
								<dt class="m-l-xs" runat="server" id="dt2">
									Username
								</dt>
								<dd runat="server" id="dd3">
									<asp:Label id="lblUsername" runat="server"></asp:Label>
								</dd>
								<dt class="m-l-xs">
									Address
								</dt>
								<dd id="ddAddress" runat="server">
									<asp:HyperLink ID="hypAddHome" CssClass="btn-info btn-sm" runat="server" Text="Add Home"></asp:HyperLink>
									<asp:Label id="lblAddress" runat="server"></asp:Label>
								</dd>
								<dd>
									<asp:Label id="lblCityStateZip" runat="server"></asp:Label>
									<br />
									<asp:Button ID="hypRemoveHome" onClientClick="return confirm('Remove this home and all related information?')" CssClass="btn-danger btn-xs" runat="server" Text="Remove Home" OnClick="btnRemoveHome_Click" />
								</dd>
							</dl>
							<div class="pull-right">
								<asp:Literal ID="litRoles" runat="server"></asp:Literal>
							</div>
						</div>
						<div id="photos" class="tab-pane fade in">
							<img class="m-b" src="/CaseManagement/Images/crowdrelief-profile-placeholder.png" runat="server" id="imgProfile" />
							<button id="btnAddProfileImage" class="btnProfilePhoto btn btn-info" runat="server"	>Upload Profile Image</button>
						</div>
						<div id="vulnerabilities" class="tab-pane fade">
							<div class="hpanel m-t-md">
								<div class="panel-heading bg-light-soft">
									<span class="text-muted m-l-md">QUALIFIERS</span>
								</div>
								<div class="panel-body">
									<div class="row">
										<div class="col-xs-6">
											<ul>
												<b>Vulnerabilities</b>	
												<asp:Repeater ID="rptqualifiers" runat="server">
													<ItemTemplate>
														<li><%# DataBinder.Eval(Container.DataItem, "Status") %></li>
													</ItemTemplate>
												</asp:Repeater>
											</ul>
										</div>
										<div class="col-xs-6">
											<ul>
												<b>Basic Needs</b>
												<asp:Repeater ID="rptBasicNeeds" runat="server">
													<ItemTemplate>
														<li><%# DataBinder.Eval(Container.DataItem, "Status") %></li>
													</ItemTemplate>
												</asp:Repeater>
											</ul>
										</div>
									</div>
								</div>
								<div class="panel-footer">
									Update Info
								</div>
							</div>
						</div>
						<div id="home" class="tab-pane fade" style="height: 600px;">
							<div id="divAddHome" class="alert alert-success" runat="server" visible="false">
								<i class="fa fa-bolt"></i> Add a Home to view Housing Status.
							</div>	
							<div id="divHomeInfo" class="hpanel m-t-md" runat="server" visible="false">
								
								<div class="panel-heading bg-light-soft">
									<span class="text-muted m-l-md">HOUSING</span>
								</div>
								<div class="panel-body">
									<div class="row">
										<div class="col-xs-6">
											<ul>
												<b>Homeowner / Renter Resources</b>	
												<asp:Repeater ID="rptHomeResources" runat="server">
													<ItemTemplate>
														<li><%# DataBinder.Eval(Container.DataItem, "Status") %></li>
													</ItemTemplate>
												</asp:Repeater>
											</ul>
										</div>
										<div class="col-xs-6">
											<ul>
												<b>Construction Needs</b>
												<asp:Repeater ID="rptConstructionNeeds" runat="server">
													<ItemTemplate>
														<li><%# DataBinder.Eval(Container.DataItem, "Status") %></li>
													</ItemTemplate>
												</asp:Repeater>
											</ul>
										</div>
									</div>
								</div>
								<div class="panel-footer">
									Select to edit home information.
									<div class="pull-right">
										<asp:HyperLink ID="hypEditHomeInfo" CssClass="btn-success btn btn-sm" runat="server">Edit Home Information</asp:HyperLink>
									</div>
								</div>
							</div>
						</div>
						<div id="documents" class="tab-pane fade" style="height: 600px;">
							<div id="Div1" class="alert alert-success">Documents Here</div>
						</div>
					</div>
				</div>
				<div class="panel-footer">
					<div id="litAddAddress" class="alert alert-success" runat="server" visible="true">
						<i class="fa fa-bolt"></i> Add a home to view the clients map.
					</div>
					<div class="border-right border-left" style="height:300px;" runat="server" visible="false" id="divMap">
						<iframe width="100%" height="100%" frameborder="0" style="border:0" src="https://www.google.com/maps/embed/v1/place?q=place_id:<%=googlePlacesId%>&key=<%=mapApiKey%>" allowfullscreen></iframe>
					</div>
				</div>
			</div>
		</div>

		<div class="col-lg-7">
			<div class="hpanel hblue">
				<div class="panel-heading hbuilt">
					Client Progress
				</div>
				<div class="panel-body">
					<ul class="nav nav-tabs">
						<li class="active"><a data-toggle="tab" href="#casenotes">Case Notes</a></li>
						<li><a data-toggle="tab" href="#cleanup">Cleanup</a></li>
						<li><a data-toggle="tab" href="#tahomerepair">Home Repair</a></li>
						<li><a data-toggle="tab" href="#resourcemapping">Resource Mapping</a></li>
						<li><a data-toggle="tab" href="#itemreplacement">Household Items</a></li>
						<li><a data-toggle="tab" href="#timeline">Timeline</a></li>
					</ul>
					<div class="tab-content">
						<div id="casenotes" class="tab-pane fade in active">
							<div class="chat-discussion" style="height: auto">
								<div class="chat-message">
									<div class="message">
										<span class="message-date pull-left">Enter case management notes below.  <asp:Literal ID="litDateTime" runat="server"></asp:Literal> </span>
										<br />	
										<span class="message-content">
											<asp:TextBox TextMode="MultiLine" runat="server" ID="txtPost" CssClass="summernote"></asp:TextBox>
										</span>
										<div style="overflow:auto;">
											<asp:Button ID="btnRebuildPost" OnClick="btnAddProfileNote_Click" CssClass="btn w-xs btn-sm btn-primary pull-right m-t-sm" runat="server" Text="Post" />
										</div>
									</div>
								</div>
								<asp:Repeater ID="rptPosts" runat="server">
									<ItemTemplate>
										<div class="hpanel">
											<div class="panel-body">
												<div class="message">
													<span class="message-content">
													<%# DataBinder.Eval(Container.DataItem, "Note1") %>
													</span>
													<div class="blog-article-box pull-right">
														<span class="message-date pull-right">
														<%# DataBinder.Eval(Container.DataItem, "createdon", "{0:M/d/yyyy h:mm tt}") %> 
														By: <%# DataBinder.Eval(Container.DataItem, "fullname") %></span>
													</div>
												</div>
											</div>
										</div>
									</ItemTemplate>
								</asp:Repeater>
							</div>
						</div>
						<div id="timeline" class="tab-pane">
							<div class="v-timeline vertical-container animate-panel"  data-child="vertical-timeline-block" data-delay="1">
								<div class="vertical-timeline-block">
									<div class="vertical-timeline-icon navy-bg">
										<i class="fa fa-calendar"></i>
									</div>
									<div class="vertical-timeline-content">
										<div class="p-sm">
											<span class="vertical-date pull-right"> Saturday <br/> <small>12:17:43 PM</small> </span>

											<h2>The standard chunk of Lorem Ipsum</h2>
											<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to
											</p>
										</div>
										<div class="panel-footer">
											It is a long established fact that
										</div>
									</div>
								</div>
								<div class="vertical-timeline-block">
									<div class="vertical-timeline-icon navy-bg">
										<i class="fa fa-calendar"></i>
									</div>
									<div class="vertical-timeline-content">
										<div class="p-sm">
											<span class="vertical-date pull-right"> Saturday <br/> <small>12:17:43 PM</small> </span>

											<h2>There are many variations</h2>
											<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to
											</p>
										</div>
										<div class="panel-footer">
											It is a long established fact that
										</div>
									</div>
								</div>
								<div class="vertical-timeline-block">
									<div class="vertical-timeline-icon navy-bg">
										<i class="fa fa-calendar"></i>
									</div>
									<div class="vertical-timeline-content">
										<div class="p-sm">
											<span class="vertical-date pull-right"> Saturday <br/> <small>12:17:43 PM</small> </span>

											<h2>Contrary to popular belief</h2>
											<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to
											</p>
										</div>
										<div class="panel-footer">
											It is a long established fact that
										</div>
									</div>
								</div>
								<div class="vertical-timeline-block">
									<div class="vertical-timeline-icon navy-bg">
										<i class="fa fa-calendar"></i>
									</div>
									<div class="vertical-timeline-content">
										<div class="p-sm">
											<span class="vertical-date pull-right"> Saturday <br/> <small>12:17:43 PM</small> </span>

											<h2>The generated Lorem Ipsum</h2>
											<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to
											</p>
										</div>
										<div class="panel-footer">
											It is a long established fact that
										</div>
									</div>
								</div>
								<div class="vertical-timeline-block">
									<div class="vertical-timeline-icon navy-bg">
										<i class="fa fa-calendar"></i>
									</div>
									<div class="vertical-timeline-content">
										<div class="p-sm">
											<span class="vertical-date pull-right"> Saturday <br/> <small>12:17:43 PM</small> </span>

											<h2>The standard chunk</h2>
											<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to
											</p>
										</div>
										<div class="panel-footer">
											It is a long established fact that
										</div>
									</div>
								</div>
							</div>
						</div>
						<div id="tahomerepair" class="tab-pane">
							<div id="Div1" class="alert alert-success">Add a home to track it's repair progress here.</div>
						</div>
						<div id="resourcemapping" class="tab-pane">
							<div id="Div1" class="alert alert-success">Resource Allocation Here</div>
						</div>
						<div id="cleanup" class="tab-pane">
							<div id="Div1" class="alert alert-success">Home Cleanup and Debris Removal Here</div>
						</div>
						<div id="itemreplacement" class="tab-pane">
							<div id="Div1" class="alert alert-success">Item Replacement Here</div>
						</div>
					</div>
				</div>
				<div class="panel-footer">
				</div>
			</div>
		</div>
	</div>
	
<div class="row" runat="server" id="divAdminTools" visible="false">
    <div class="col-lg-12">
		<div class="hpanel hgreen m-b-lg">
				<div class="panel-heading hbuilt">
					Client Security Information
				</div>
			
				<div class="panel-body" id="divResetPassword" runat="server" visible="false">
					<h3>Change Clients Password</h3>
					<asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" Width="200"></asp:TextBox> <asp:Button ID="btnChangePassword" CausesValidation="False" CssClass="btn btn-sm btn-info" runat="server" Text="Change Password" OnClick="btnChangePassword_Click" />
				</div>
				<div class="panel-footer">
				</div>
			</div>
			<div class="hpanel hgreen m-b-lg">
				<div class="panel-heading hbuilt">
					Administrative User Tools
				</div>
				<div class="panel-body" id="divUserAccountAdministration" runat="server" visible="false">
					<div class="checkbox">	
						<h3>User Access</h3>
						<input type="checkbox" runat="server" id="chkLockedOut" class="i-checks">
						<label class="chkboxlist" for="<%=chkLockedOut.ClientID%>">Is Locked Out</label>
					</div>
					<div class="checkbox">
						<h3>Choose Roles</h3>
						<asp:CheckboxList id="chkRoles" runat="server" DataValueField="RoleId" DataTextField="RoleName" CssClass="i-checks chkboxlist"></asp:CheckboxList>
					</div>
					<div>
						<asp:Button ID="bthSubmitLockout" CausesValidation="False" CssClass="btn btn-sm btn-info" runat="server" Text="Update Users Access" OnClick="bthSubmitRoles_Click" />
					</div>
				</div>
				<div class="panel-footer">
				</div>
			</div>
	</div>
</div>
</div>
</asp:Content>

