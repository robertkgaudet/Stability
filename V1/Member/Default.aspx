<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="V1_Member_Default" %>
<%@ Register Src="~/V1/UserControls/MemberNavigation.ascx" TagPrefix="uc1" TagName="MemberNavigation" %>
<%@ Register Src="~/V1/UserControls/MemberHeader.ascx" TagPrefix="uc1" TagName="MemberHeader" %>
<%@ Register Src="~/V1/UserControls/DeploymentListCard.ascx" TagPrefix="uc1" TagName="DeploymentListCard" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script type="text/javascript">
		function updateCalendar() {
			window.location.href = "/V1/Profile/AvailableDates.aspx?userActionModal=false";
		}
		function updateSkillsets() {
			window.location.href = "/V1/Profile/EditSkills.aspx?userActionModal=false";
		}
		function updateEquipment() {
			window.location.href = "/V1/Profile/EditResources.aspx?userActionModal=false";
		}

		$(document).ready(function ()
		{
			$('.btnFriend').click(function () {
				RequestConnection('<%=_receiverUserId%>', '<%=_sendingUserId%>');
				return false;
			});

			function RequestConnection(receiverUserIdString, requestorUserIdString) {
				$.ajax({
					url: '/V1/Member/Default.aspx/RequestConnection',
					method: 'POST',
					contentType: 'application/json; charset=utf-8',
					dataType: 'json',
					data: JSON.stringify({ receiverUserIdString: receiverUserIdString, requestorUserIdString: requestorUserIdString }),
					success: function (response) {

						//Update friend button style.
						$('.btnFriend').text("Connection Request Sent").removeClass("btnFriend").removeClass("btn-primary").addClass("btn-default").attr("id", "btnUpdated");
							
					},
					error: function (error) {
						console.error('Error loading events:', error);
					}
				});
			}

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
	</script>
	<style>
		.hpanel
		{
			margin-bottom:5px !important;
		}
		.member-panel-body
		{
			border-radius: 10px !important;
			margin-bottom: 0px !important;
		}
		.member-panel-body
		{
			border-radius: 10px !important;
			margin-bottom: 0px !important;

		}
		.calendar-month-day:hover, .calendar-month:hover{
			cursor:pointer;
			background-color:#F7F9FA;
		}
		.calendar-month-day
		{
			border: 1px solid #63CB31;  /* Light border */
            background-color: white;      /* White background */
            padding: 2px;                /* Add some padding */
			font-size:12px;
		}
		.calendar-year
		{
            background-color: #63CB31;      /* White background */
			color:white;
            padding: 2px;                /* Add some padding */
			margin-bottom:3px;
			font-size:12px;
		}
		.calendar-date-of-month{
			font-size:17px;
			font-weight:bold;
			color:#63CB31;
		}
		/* Custom gutter class for rows */
		.row.no-gutter {
			margin-left: 0;
			margin-right: 0;
		}

		.row.no-gutter [class*="col-"] {
			padding-left: 1px;  /* Reduced gutter padding */
			padding-right: 1px;
		}
		.tab-pane .panel-body {
			overflow: auto;
			height:1000px;
		}
	    .badge {
            background-color: #b08e4f!important;
        }
		.member-panel-body h3 {
		    color: black !important;
	    }
		.font-normal {
	        color: black !important;
        }
	</style>
	<style>
		.skill-pill {
			display: inline-block;
			margin: 2px 4px;
			padding: 4px 10px;
			border: 1px solid #ccc;
			border-radius: 15px;
			background-color: transparent;
			color: #555;
			font-size: 13px;
			transition: all 0.2s ease-in-out;
		}

		.skill-pill:hover {
			border-color: #5bc0de;
			color: #5bc0de;
			cursor: pointer;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="container" style="padding-bottom:100px !important;">
		<div class="row justify-content-center" style="margin-top:40px;">
			<!--MAIN CONTENT CONTAINER-->
			<div class="col-sm-8 col-lg-9">
				<div class="content text-center">
					<uc1:MemberHeader runat="server" ID="ucMemberHeader" />
				</div>
				<div class="hpanel">
					<div class="panel-body alert <%=availabilityStyle%> member-panel-body">
						<h3>When You’re Ready to Help
							<small style="font-size:15px;">
								<br />
								These are the dates this user has marked as available to serve. A team members availability is more than a calendar entry — it’s a signal to your community that you’re ready to step in when it matters most.
							</small>
						</h3>
						<asp:Literal ID="litDatesAvailable" runat="server"></asp:Literal>
						<div class="panel panel-default" id="divNoDates" runat="server" visible="false">
						  <div class="panel-body text-center">
							<p class="lead text-muted">
							  This member hasn’t marked any dates as available to serve just yet.
							</p>
							<p>
							  Every moment of availability matters — even one day can make a difference during a response.
							</p>
							<p>
							  <em>If this is your profile</em>, consider adding the days you’re open to help.
							  It’s a small step that makes coordinated, community-powered support possible.
							</p>
							<button id="btnUpdateCal" runat="server" onclick="updateCalendar(); return false;" class="btn btn-info btn-sm mt-2">
							  <i class="fa fa-calendar-plus-o"></i> Update Your Availability
							</button>
						  </div>
						</div>
						<button Class="btn btn-default btn-outline pull-right m-t-lg" onclick="updateCalendar(); return false;" runat="server" id="btnUpdateCalendar" ClientIDMode="static" Visible="false"><b>Need to Make A Change?</b><br />Update your availability. <i class='fa fa-long-arrow-right'></i></button>
						
					</div>
				</div>
				<div class="hpanel">
					<div class="panel-body alert <%=availabilityStyle%> member-panel-body">
						<h3>Skills & Equipment
							<small style="font-size:15px;">
								<br />
								This is more than a list — it’s a lifeline. This members skills and equipment represent real-world capacity to show up for neighbors in need. From tech support to tree clearing, food trucks to flatbeds, every item and ability listed here helps extend the reach of your community’s care and resilience.
								<br />
								<b>Think of this as your outreach toolkit</b> — ready to activate whenever a response is needed.
							</small>

						</h3>
						<div class="container-fluid">
							<div class="row">
								<div class="col-md-6 mb-3">
									<div class="text-wrap">
									<h5 class="text-primary">Skillsets</h5>
									<asp:Literal ID="litSkills" runat="server"></asp:Literal>
									<button Class="btn btn-default btn-outline pull-right m-t-lg" onclick="updateSkillsets(); return false;" runat="server" id="btnUpdateSkills" ClientIDMode="static" Visible="false"><b>Aquire New Skills?</b><br />Update your skillsets. <i class='fa fa-long-arrow-right'></i></button>
									</div>
								</div>
								<div class="col-md-6 mb-3">
									<div class="text-wrap">
									<h5 class="text-primary">Equipment</h5>
									<asp:Literal ID="litResources" runat="server"></asp:Literal>
									<button Class="btn btn-default btn-outline pull-right m-t-lg" onclick="updateEquipment(); return false;" runat="server" id="btnUpdateEquipment" ClientIDMode="static" Visible="false"><b>Equipment Changes?</b><br />Update your equipment. <i class='fa fa-long-arrow-right'></i></button>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div class="hpanel collapsed">
					<div class="panel-heading hbuilt member-panel-body">
						<div class="panel-tools">
							<a class="showhide"><i class="fa fa-chevron-up"></i></a>
						</div>
						<h3>Activity</h3>
						<span class="font-normal"> Deployments and positions signed up for.</span>
					</div>
					<div class="panel-body member-panel-body">
						<div class="hpanel">
							<ul class="nav nav-tabs">
								<li class="active"><a data-toggle="tab" href="#tab-1">Active Deployments</a></li>
								<li class=""><a data-toggle="tab" href="#tab-2">Upcoming Deployments</a></li>
								<li class=""><a data-toggle="tab" href="#tab-3">Past Deployments</a></li>
							</ul>
							<div class="tab-content">
								<div id="tab-1" class="tab-pane active">
									<div class="panel-body">
										<uc1:DeploymentListCard runat="server" ID="ucDeploymentListCard" IsActive="true" />
									</div>
								</div>
								<div id="tab-2" class="tab-pane">
									<div class="panel-body">
									</div>
								</div>
								<div id="tab-3" class="tab-pane">
									<div class="panel-body">
										<uc1:DeploymentListCard runat="server" ID="DeploymentListCard1" IsActive="false" />
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>


			<!--NAVIGATION-->
			<div class="col-sm-4 col-lg-3">
				<uc1:MemberNavigation runat="server" ID="ucMemberNavigation" />
			</div>
		</div>
	</div>
	
	<script src="/v1/Scripts/masonry.pkgd.min.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('.grid').each(function () {
				// Initialize Masonry for each grid individually
				$(this).masonry({
					itemSelector: '.grid-item',
					gutter: 20,
					columnWidth: '.grid-item',
					percentPosition: true
				});
			});
		});


	</script>
</asp:Content>

