<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="CoronavirusHospitalLocations.aspx.cs" Inherits="V1_CoronavirusHospitalLocations" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />

<script type="text/javascript">

    $(document).ready(function () {
        $(function () {
            $('#locationTable').footable();
        });

        $(".btnAddLocation").click(function () {
            location.href = "/V1/Administration/Location/AddLocation.aspx";
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
								Covid-19 Pandemic Response Locations in the New Orleans Area
								<label runat="server" id="lblAddLocation" Class="btn btn-primary btn-lg pull-right m-r-lg btnAddLocation">Add Location</label>
							</h2>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-12 container m-l-10">

					<div class="hpanel">
						<div class="panel-heading hbuilt">
							Locations
						</div>
						<div class="panel-body">
							<div class="p-l-lg">
								To filter type what you are looking for.
								<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table"/>
							
								<asp:Repeater ID="rpHealthFacilities" runat="server" OnItemDataBound="rpHealthFacilities_ItemDataBound">
									<HeaderTemplate>
										<table id="locationTable" class="footable table table-bordered table-hover" data-page-size="50" data-filter="#filter">
											<thead>
												<tr>
													<th data-toggle="true">Name</th>
													<th data-toggle="true">Address</th>
													<th data-toggle="true" data-hide="phone,tablet">State</th>
													<th data-toggle="true" data-hide="phone,tablet">Services Provided</th>
													<th data-toggle="true">Type</th>
												</tr>
											</thead>
											<tbody>
									</HeaderTemplate>
									<ItemTemplate>
											<tr>
												<td>
													<a href='/V1/Location.aspx?locationProfileId=<%# DataBinder.Eval(Container.DataItem, "LocationProfileId") %>' class="EventLink"><%# DataBinder.Eval(Container.DataItem, "Name") %> </a>
												</td>
												<td><asp:Literal ID="litAddress" runat="server"></asp:Literal></td>
												<td><asp:Literal ID="litState" runat="server"></asp:Literal></td>
												<td><asp:Literal ID="litServices" runat="server"></asp:Literal></td>
												<td><asp:Literal ID="litType" runat="server"></asp:Literal></td>
											</tr>
									</ItemTemplate>
									<FooterTemplate>
											</tbody>
											<tfoot>
												<tr>
													<td colspan="7">
														<ul class="pagination pull-right"></ul>
													</td>
												</tr>
											</tfoot>
										</table>
									</FooterTemplate>
								</asp:Repeater>
							</div>
						</div>
									
						<div class="panel-footer">
							<div class="row">
								<div class="col-sm-6">
									<asp:HyperLink CssClass="EventLink" ID="hypRebuildingHomesCount" runat="server"></asp:HyperLink>
								</div>
								<div class="col-sm-6">
									<asp:Literal ID="litFollowingHomesCount" runat="server"></asp:Literal>
								</div>
							</div>
						</div>
					</div>

				</div>
			</div>
		</div>
</asp:Content>