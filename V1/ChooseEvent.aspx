<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="ChooseEvent.aspx.cs" Inherits="V1_Profile_ChooseEvent" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/V1/Scripts/masonry.pkgd.min.js"></script>
	<script>
		$(document).ready(function () {

			$('.grid').masonry({
				// options
				itemSelector: '.grid-item',
				gutter: 10
			});
        });
	</script>
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
	<style>
		.grid-item					{ width:250px; }
		.disasterPanel, .activeDisasterPanel				{height:100px;}
		.disasterPanel:hover		{cursor:pointer; background-color:#FCF8E3; }
		.activeDisasterPanel:hover		{cursor:pointer; background-color:rebeccapurple; color:white; }
		.alert-success{cursor:pointer; background-color:#62CB31; color:white; }
		.titleHeight{height:60px;}

	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	



	<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
		<div class="hpanel">
			<div class="panel-body">
				<h2 class="font-light m-b-xs">
					<asp:Literal ID="litEventDescription" runat="server"></asp:Literal>
				</h2>
					After a disaster, capable citizens can help each other. Choose a community portal to collaborate and help.
                    <asp:HyperLink Visible="false" ID="hypNewDisaster" CssClass="btn btn-info" runat="server" Text="Create New Community Portal" NavigateUrl="/V1/Administration/NewDisaster.aspx"></asp:HyperLink>
			</div>
		</div>
	</div>
    
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-12 container">
					<div class="hpanel">
						<div class="panel-body">
							<b>SEARCH ALL PORTALS</b>
							<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table">
							<table id="tblVolunteers" class="footable table table-stripped toggle-arrow-tiny" data-page-size="50" data-filter=#filter>
								<thead>
									<tr>
										<th data-toggle="true">Portal Name</th>
                                        <th data-toggle="true"></th>
                                        <th data-toggle="true">States/Counties</th>
                                        <th data-toggle="true">Date</th>
                                        <th data-toggle="true">Simulation</th>
                                        <th data-toggle="true">Status</th>
									</tr>
								</thead>
								<tbody>
									<asp:Repeater ID="dlDisasterPortal" runat="server" OnItemDataBound="dlDisasterPortal_ItemDataBound">
										<ItemTemplate>
											<tr>
												<td>
													<i class="fa fa-binoculars pull-left" id="iSimulation" runat="server" visible="false"></i>
													<asp:HyperLink ID="hypName" runat="server"></asp:HyperLink>
												</td>
                                                <td><asp:HyperLink ID="hypMap" runat="server"></asp:HyperLink> <asp:HyperLink ID="hypEdit" runat="server"></asp:HyperLink></td>
												<td><asp:Literal ID="litStates" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litEventDate" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litSimulation" runat="server"></asp:Literal></td>
                                                <td><asp:Literal ID="litStatus" runat="server"></asp:Literal></td>
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