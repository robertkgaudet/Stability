<%@ page title="" language="C#" masterpagefile="~/V1/MasterPages/Homer.master" autoeventwireup="true" inherits="Administration_Rebuilds, App_Web_ijye2wuz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	<script>
	
		$(document).ready(function () {
			$(function () {

				// Initialize Example 2
				$('#rebuildsTable').footable();
			});
		});
	</script>
	<style>
		.enableRebuildActiveGreen {
			width: 7px;
			height: 7px;
			background: #5CB85C;
			-moz-border-radius: 7px;
			-webkit-border-radius: 7px;
			border-radius: 7px;
			}
		.enableRebuildNoGoRed {
			width: 7px;
			height: 7px;
			background: #D9534F;
			-moz-border-radius: 7px;
			-webkit-border-radius: 7px;
			border-radius: 7px;
			}
		.StatusClassIceBlue {
			width: 7px;
			height: 7px;
			background: #5BC0DE;
			-moz-border-radius: 7px;
			-webkit-border-radius: 7px;
			border-radius: 7px;
			}
		.StatusClassOnDeckYellow {
			width: 7px;
			height: 7px;
			background: #F0AD4E;
			-moz-border-radius: 7px;
			-webkit-border-radius: 7px;
			border-radius: 7px;
			}
		.StatusClassActiveGreen {
			width: 7px;
			height: 7px;
			background: #5CB85C;
			-moz-border-radius: 7px;
			-webkit-border-radius: 7px;
			border-radius: 7px;
			}
		.StatusClassComplete {
			width: 7px;
			height: 7px;
			background: #428BCA;
			-moz-border-radius: 7px;
			-webkit-border-radius: 7px;
			border-radius: 7px;
			}
		
		.EventLink
		{
			color:#3498DB;
			font-weight:bold;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="row">
				<div class="col-md-6">
					<div class="hpanel">
						<div class="panel-body">
							<h2 class="font-light m-b-xs">
								All Rebuilds
							</h2>
							<h4>Rebuild Progress (Average)</h4>
							<div class="progress m-t-xs full progress-small">
								<div style="width:<%=rebuildProgressPercent%>%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="<%=rebuildProgressPercent%>" role="progressbar" class="progress-bar progress-bar-success">
									<span class="sr-only"><%=rebuildProgressPercent%>% Complete (success)</span>
								</div>
							</div>
							<h4>Overall Progress (Average)</h4>
							<div class="progress m-t-xs full progress-small">
								<div style="width:<%=overallProgressPercent%>%" aria-valuemax="100" aria-valuemin="0" aria-valuenow="<%=overallProgressPercent%>" role="progressbar" class="progress-bar progress-bar-info">
									<span class="sr-only"><%=overallProgressPercent%>% Complete (success)</span>
								</div>
							</div>
							<a class="small-header-action" href="">
								<div class="clip-header">
									<i class="fa fa-arrow-up"></i>
								</div>
							</a>
						</div>
					</div>
				</div>
				<div class="col-md-2">
					<div class="hpanel hbggreen m-t">
						<div class="panel-body">
							<div class="text-center">
								<h5>Average<br />Rebuild Progress</h5>
								<p class="text-big font-light">
									<%=rebuildProgressPercent%>%
								</p>
							</div>
						</div>
					</div>
				</div>
				<div class="col-md-2">
					<div class="hpanel hbggreen m-t">
						<div class="panel-body">
							<div class="text-center">
								<h5>Overall<br />Rebuild Progress</h4>
								<p class="text-big font-light">
									<%=overallProgressPercent%>%
								</p>
							</div>
						</div>
					</div>
				</div>
				<div class="col-md-2">
					<div class="hpanel hbgyellow m-t">
						<div class="panel-body">
							<div class="text-center">
								<h5>Total<br />Volunteers Needed</h5>
								<p class="text-big font-light">
									<asp:Literal ID="litVolunteers" runat="server"></asp:Literal>
								</p>
								<div>
								<small>
									<asp:Literal ID="litVolunteersDifficulty" runat="server"></asp:Literal>
								</small>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>
		<div class="content animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="hpanel">
				<div class="panel-body">
					To filter, type a term. Statue: hold, clear Stage: icebox, on deck, active, complete
					<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table"/>

				
					<asp:Repeater ID="dlRebuildTable" runat="server" OnItemDataBound="dlRebuildTable_ItemDataBound">
						<HeaderTemplate>
							<table id="rebuildsTable" class="footable table table-bordered table-hover" data-page-size="200" data-filter="#filter">
								<thead>
									<tr>
										<th data-toggle="true">Name</th>
										<th data-toggle="true" data-hide="phone">Address</th>
										<th data-toggle="true" data-hide="phone,tablet">Rebuild Stage</th>
										<th data-toggle="true" data-hide="phone,tablet">Rebuild Progress %</th>
										<th data-toggle="true" data-hide="phone,tablet">Overall Progress %</th>
										<th data-toggle="true" data-hide="phone,tablet">Volunteers Needed</th>
										<th data-toggle="true">Contact</th>
									</tr>
								</thead>
								<tbody>
						</HeaderTemplate>
						<ItemTemplate>
								<tr>
									<td>
										<a href='/V1/Profile/Rebuild.aspx?rebuildId=<%# DataBinder.Eval(Container.DataItem, "RebuildId") %>' class="EventLink"><%# DataBinder.Eval(Container.DataItem, "Firstname") %> <%# DataBinder.Eval(Container.DataItem, "Lastname") %></a>
									</td>
									<td><%# DataBinder.Eval(Container.DataItem, "Address1") %> <%# DataBinder.Eval(Container.DataItem, "City") %></td>
									<td><asp:Literal ID="litRebuildStage" runat="server"></asp:Literal></td>
									<td><asp:Literal ID="litProgress" runat="server"></asp:Literal></td>
									<td><asp:Literal ID="litOverallProgress" runat="server"></asp:Literal></td>
									<td><asp:Literal ID="litVolunteersNeeded" runat="server"></asp:Literal></td>
									<td><asp:HyperLink ID="hypPhone" runat="server"></asp:HyperLink></td>
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
		</div>
</asp:Content>