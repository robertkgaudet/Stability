<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="VolunteerList.aspx.cs" Inherits="Administration_VolunteerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
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
								Volunteer Management
							</h2>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-12 container">
					<div class="hpanel">
						<div class="panel-heading">
							<div class="panel-tools">
								<a class="showhide"><i class="fa fa-chevron-up"></i></a>
								<a class="closebox"><i class="fa fa-times"></i></a>
							</div>
							Sort, page, search to find volunteers.
						</div>
						<div class="panel-body">
							<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table">
							<table id="tblVolunteers" class="footable table table-stripped toggle-arrow-tiny" data-page-size="50" data-filter=#filter>
								<thead>
									<tr>
										<th data-toggle="true">Name</th>
										<th>Location</th>
										<th>Phone</th>
										<th>Email</th>
										<th>Applied</th>
										<th>Status</th>
										<th>Disaster</th>
										<th>Skills</th>
										<th data-hide="all">Review Status</th>
									</tr>
								</thead>
								<tbody>
									<asp:Repeater ID="dlVolunteers" runat="server" OnItemDataBound="dlVolunteers_ItemDataBound">
										<ItemTemplate>
											<tr>
												<td><asp:HyperLink ID="hypFullName" runat="server"></asp:HyperLink></td>
												<td><asp:Label ID="lblLocation" runat="server"></asp:Label></td>
												<td><asp:Label ID="lblPhonenumber" runat="server"></asp:Label></td>
												<td><asp:Label ID="lblEmail" runat="server"></asp:Label></td>
												<td><asp:Label ID="litVolunteerApplicationCompletedOn" runat="server"></asp:Label></td>
												<td><asp:Label ID="litVolunteerStatus" runat="server"></asp:Label></td>
												<td><asp:Label ID="lblDisaster" runat="server"></asp:Label></td>
												<td><asp:Label ID="lblSkills" runat="server"></asp:Label>
													<p>
														<asp:Label ID="lblVolunteerSkills" runat="server"></asp:Label>
													</p>
												</td>
												<td><asp:Label ID="lblVolunteerReviewStatus" runat="server"></asp:Label></td>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</tbody>
								<tfoot>
									<tr>
										<td colspan="5">
											<ul class="pagination pull-right"></ul>
										</td>
									</tr>
								</tfoot>
							</table>
						</div>
					</div>
				</div>
			</div>
		</div>
<script>

	$(function () {

		// Initialize Example 1
		$('#tblVolunteers').footable();
	});

</script>
</asp:Content>