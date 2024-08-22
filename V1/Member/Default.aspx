<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="V1_Member_Default" %>
<%@ Register Src="~/V1/UserControls/MemberNavigation.ascx" TagPrefix="uc1" TagName="MemberNavigation" %>
<%@ Register Src="~/V1/UserControls/MemberHeader.ascx" TagPrefix="uc1" TagName="MemberHeader" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">	<script type="text/javascript">
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
					<div class="alert <%=availabilityStyle%> member-panel-body">
						<button Class="btn btn-success pull-right" onclick="updateCalendar(); return false;" runat="server" id="btnUpdateCalendar" ClientIDMode="static" Visible="false"><b>Update My Calendar</b><br />Add dates you want to volunteer to your calendar. <i class='fa fa-long-arrow-right'></i></button>
						<h4>Availability</h4>
						<asp:Literal ID="litDatesAvailable" runat="server"></asp:Literal>
					</div>
				</div>
				<div class="hpanel">
					<div class="panel-body member-panel-body">
						<h4>Skills & Resources</h4>
						<hr />
						<p style="font-size:16px;">
							<asp:Literal ID="litSkills" runat="server"></asp:Literal>
						</p>
						<hr />
						<p style="font-size:16px;">
							<asp:Literal ID="litResources" runat="server"></asp:Literal>
						</p>
					</div>
				</div>
				<div class="hpanel">
					<div class="panel-body member-panel-body">
						<h4>Deployment History</h4>
							<div class="hpanel">
								<div class="hpanel">
									<ul class="nav nav-tabs">
										<li class="active"><a data-toggle="tab" href="#tab-1">Deployments</a></li>
										<li class=""><a data-toggle="tab" href="#tab-2">Portals</a></li>
									</ul>
									<div class="tab-content">
										<div id="tab-1" class="tab-pane active">
											<div class="panel-body">
												<strong>DEPLOYMENTS</strong>

												<p>A wonderful serenity has taken possession of my entire soul, like these sweet mornings of spring which I enjoy with my whole heart. I am alone, and feel the charm of
													existence in this spot, which was created for the bliss of souls like mine.</p>

												<div class="table-responsive">
													<table class="table table-striped">
														<thead>
														<tr>
															<th>#</th>
															<th>Project </th>
															<th>Name </th>
															<th>Phone </th>
															<th>Company </th>
															<th>Completed </th>
															<th>Task</th>
															<th>Date</th>
															<th>Action</th>
														</tr>
														</thead>
														<tbody>
														<tr>
															<td>1</td>
															<td>Project <small>This is example of project</small></td>
															<td>Patrick Smith</td>
															<td>0800 051213</td>
															<td>Inceptos Hymenaeos Ltd</td>
															<td><span class="pie">2/45</span></td>
															<td>20%</td>
															<td>Jul 14, 2013</td>
															<td><a href="#"><i class="fa fa-check text-success"></i></a></td>
														</tr>
														<tr>
															<td>2</td>
															<td>Alpha project</td>
															<td>Alice Jackson</td>
															<td>0500 780909</td>
															<td>Nec Euismod In Company</td>
															<td><span class="pie">1/5</span></td>
															<td>40%</td>
															<td>Jul 16, 2013</td>
															<td><a href="#"><i class="fa fa-check text-success"></i></a></td>
														</tr>
														</tbody>
													</table>
												</div>
											</div>
										</div>
										<div id="tab-2" class="tab-pane">
											<div class="panel-body">
												<strong>PORTALS</strong>

												<p>A wonderful serenity has taken possession of my entire soul, like these sweet mornings of spring which I enjoy with my whole heart. I am alone, and feel the charm of
													existence in this spot, which was created for the bliss of souls like mine.</p>

												<div class="table-responsive">
													<table class="table table-striped">
														<thead>
														<tr>

															<th>#</th>
															<th>Project </th>
															<th>Name </th>
															<th>Phone </th>
															<th>Company </th>
															<th>Completed </th>
															<th>Task</th>
															<th>Date</th>
															<th>Action</th>
														</tr>
														</thead>
														<tbody>
														<tr>
															<td>1</td>
															<td>Project <small>This is example of project</small></td>
															<td>Patrick Smith</td>
															<td>0800 051213</td>
															<td>Inceptos Hymenaeos Ltd</td>
															<td><span class="pie">2/45</span></td>
															<td>20%</td>
															<td>Jul 14, 2013</td>
															<td><a href="#"><i class="fa fa-check text-success"></i></a></td>
														</tr>
														<tr>
															<td>2</td>
															<td>Alpha project</td>
															<td>Alice Jackson</td>
															<td>0500 780909</td>
															<td>Nec Euismod In Company</td>
															<td><span class="pie">1/5</span></td>
															<td>40%</td>
															<td>Jul 16, 2013</td>
															<td><a href="#"><i class="fa fa-check text-success"></i></a></td>
														</tr>
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


			<!--NAVIGATION-->
			<div class="col-sm-4 col-lg-3">
				<uc1:MemberNavigation runat="server" ID="ucMemberNavigation" />
			</div>
		</div>
	</div>

</asp:Content>

