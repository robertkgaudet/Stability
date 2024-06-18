<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="NonProfitList.aspx.cs" Inherits="Administration_NonProfitList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	<script>
	
		$(document).ready(function () {
			$(function () {
				$('#nonProfitsTable').footable();
			});

			$('.btnAddNonProfit').click(function () {
				window.location.href = '/V1/Administration/NonProfitNew.aspx';
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
							<h3>Add your new team here.</h3>
							<asp:Button id="btnAdd" runat="server" CssClass="btnAddNonProfit btn btn-primary" text="Create A New Team" />
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-lg-12 container">
					<div class="hpanel">
						<div class="panel-heading hbuilt">
							<h2 class="font-light m-b-xs">
								Teams List
							</h2>
							Find an existing team.
						</div>
						<div class="panel-body">
							Type a term to search.
							<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table"/>
							
							<asp:Repeater ID="rpNonProfit" runat="server" OnItemDataBound="dlOrganizationTable_ItemDataBound">
								<HeaderTemplate>
									<table id="nonProfitsTable" class="footable table table-bordered table-hover" data-page-size="20" data-filter="#filter">
										<thead>
											<tr>
												<th data-toggle="true">Name</th>
												<%--<th data-toggle="true">Impactoid Website</th>--%>
												<th data-toggle="true" data-hide="phone,tablet">Point of Contact</th>
												<th data-toggle="true" data-hide="phone,tablet">Phone</th>
												<th data-toggle="true" data-hide="phone">City, State</th>
												<th data-toggle="true" data-hide="phone,tablet">Purpose Mission</th>
											</tr>
										</thead>
										<tbody>
								</HeaderTemplate>
								<ItemTemplate>
										<tr>
											<td>
												<a href='/V1/NonProfit/NonProfit.aspx?organizationId=<%# DataBinder.Eval(Container.DataItem, "OrganizationId") %>' class="EventLink"><%# DataBinder.Eval(Container.DataItem, "Name") %> </a>
											</td>
											<%--<td><asp:HyperLink ID="hypImpactoidWebsite" Target="_blank" runat="server"></asp:HyperLink></td>--%>
											<td><asp:Literal ID="litPointOfContactName" runat="server"></asp:Literal></td>
											<td><asp:HyperLink ID="hypPhoneNumber" runat="server"></asp:HyperLink></td>
											<td><asp:Literal ID="litCityState" runat="server"></asp:Literal></td>
											<td><asp:Literal ID="litPurposeMission" runat="server"></asp:Literal></td>
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
						<div class="panel-footer">

						</div>
					</div>
				</div>
			</div>
		</div>
</asp:Content>