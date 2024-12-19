<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="PrintableTeamList.aspx.cs" Inherits="V1_NonProfitAdministration_PrintableTeamList" %>

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
							<div id="divFilterMessage" runat="server" class="alert alert-info text-center" style="margin-bottom:20px;" visible="false">
								<asp:Literal ID="litFilterMessage" runat="server"></asp:Literal>
							</div>
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
							
							<table id="tblVolunteers" class="footable table table-stripped toggle-arrow-tiny" data-page-size="5000" data-filter=#filter>
								<thead>
									<tr>
										<th data-toggle="true">ID</th>
									</tr>
								</thead>
								<tbody>
									<asp:Repeater ID="dlVolunteers" runat="server" OnItemDataBound="dlVolunteers_ItemDataBound">
										<ItemTemplate>
											<tr>
												<td><asp:HyperLink ID="hypID" runat="server"></asp:HyperLink></td>
												<td><asp:Literal ID="litPhone" runat="server"></asp:Literal></td>
												<td><asp:Literal ID="litEmail" runat="server"></asp:Literal></td>
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