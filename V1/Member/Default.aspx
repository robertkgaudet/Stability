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
			font-size:22px;
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
				<div class="hpanel collapsed">
					<div class="panel-heading hbuilt member-panel-body">
						<div class="panel-tools">
							<a class="showhide"><i class="fa fa-chevron-up"></i></a>
						</div>
						<h3>Calendar</h3>
						<span class="font-normal"> The dates available to help.</span>
					</div>
					<div class="panel-body alert <%=availabilityStyle%> member-panel-body">
						<asp:Literal ID="litDatesAvailable" runat="server"></asp:Literal>
						<div>
							<button Class="btn btn-success pull-right m-t-lg" onclick="updateCalendar(); return false;" runat="server" id="btnUpdateCalendar" ClientIDMode="static" Visible="false"><b>Update My Calendar</b><br />Add dates you want to volunteer to your calendar. <i class='fa fa-long-arrow-right'></i></button>
						</div>
					</div>
				</div>
				<div class="hpanel collapsed">
					<div class="panel-heading hbuilt member-panel-body">
						<div class="panel-tools">
							<a class="showhide"><i class="fa fa-chevron-up"></i></a>
						</div>
						<h3>Skills & Resources</h3>
					</div>
					<div class="panel-body member-panel-body">
						<p style="font-size:16px;">
							<asp:Literal ID="litSkills" runat="server"></asp:Literal>
						</p>
						<hr />
						<p style="font-size:16px;">
							<asp:Literal ID="litResources" runat="server"></asp:Literal>
						</p>
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

