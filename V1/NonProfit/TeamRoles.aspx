<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/Homer.master" AutoEventWireup="true" CodeFile="TeamRoles.aspx.cs" Inherits="V1_NonProfit_TeamRoles" %>
<%@ Register Src="~/V1/UserControls/TeamHeader2.ascx" TagPrefix="uc1" TagName="TeamHeader" %>
<%@ Register Src="~/V1/UserControls/TeamFooter2.ascx" TagPrefix="uc1" TagName="TeamFooter" %>
<%@ MasterType VirtualPath="~/V1/MasterPages/Homer.master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script type="text/javascript">

		$(document).ready(function () {

			$('.btnAddRole').click(function () {
				window.location.href = '/V1/NonProfitAdministration/AddEditTeamRole.aspx?organizationId=<%=organizationId%>';
				return false;
			});

		});
	</script>
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
				<uc1:TeamHeader runat="server" ID="ucTeamHeader" />
					<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
						<div class="row">
							<div class="col-lg-12 container">
								<div class="hpanel">
									<div class="panel-body">
										<asp:Button ID="btnAddRole" runat="server" CssClass="btn btn-success pull-right m-b btnAddRole" Text="New Role" />
										Sort, page, search to find roles
										<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table">
							
										<table id="tblPositions" class="footable table table-stripped toggle-arrow-tiny" data-page-size="5000" data-filter=#filter>
											<thead>
												<tr>
													<th data-toggle="true">ID</th>
												</tr>
											</thead>
											<tbody>
												<asp:Repeater ID="dlPositions" runat="server" OnItemDataBound="dlPositions_ItemDataBound">
													<ItemTemplate>
														<tr>
															<td><asp:HyperLink ID="hypPositionId" Target="_blank" runat="server"></asp:HyperLink></td>
															<td><asp:HyperLink ID="hypGetTrained" Target="_blank" runat="server"></asp:HyperLink> <i class="fa fa-book"></i></td>
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
				<div class="m-t-md">
					<asp:Literal ID="litTeamRoles" runat="server"></asp:Literal>
				</div>
				<uc1:TeamFooter runat="server" ID="ucTeamFooter" />
<script>

	$(function () {

		// Initialize Example 1
		$('#tblPositions').footable();
	});

</script>
</asp:Content>

