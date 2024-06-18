<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="BusinessList.aspx.cs" Inherits="Administration_BusinessList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	<script>
	
		$(document).ready(function () {
			$(function () {

				// Initialize Example 2
				$('#businessTable').footable();
			});

			$('.btnAddBusiness').click(function () {
				window.location.href = '/V1/Administration/BusinessNew.aspx';
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
								Business Partners
							<asp:Button id="btnAdd" runat="server" CssClass="pull-right btnAddBusiness btn btn-primary" text="Add New Non Profit" />
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
						<div class="panel-heading hbuilt">
							All Businesses -  Choose one to edit.
						</div>
						<div class="panel-body">
							To filter, type a term. Statue: hold, clear Stage: icebox, on deck, active, complete
							<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table"/>
							
							<asp:Repeater ID="rpBusiness" runat="server" OnItemDataBound="dlBusinessTable_ItemDataBound">
								<HeaderTemplate>
									<table id="businessTable" class="footable table table-bordered table-hover" data-page-size="20" data-filter="#filter">
										<thead>
											<tr>
												<th data-toggle="true">Name</th>
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
												<a href='/V1/Business/Business.aspx?businessId=<%# DataBinder.Eval(Container.DataItem, "BusinessId") %>' class="EventLink"><%# DataBinder.Eval(Container.DataItem, "Name") %> </a>
											</td>
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