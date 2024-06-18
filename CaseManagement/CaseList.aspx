<%@ Page Title="" Language="C#" MasterPageFile="~/CaseManagement/MasterPages/CaseManagement.master" AutoEventWireup="true" CodeFile="CaseList.aspx.cs" Inherits="CaseManagement_CaseList" %>
<%@ MasterType VirtualPath="~/S1/MasterPages/Homer.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
	<link rel="stylesheet" href="/Homer/vendor/fooTable/css/footable.core.min.css" />
	<script>
	
		$(document).ready(function () {
			$(function () {

				// Initialize Example 2
				$('#survivorTable').footable();
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="hpanel">
        <div class="panel-body">
            <a class="small-header-action" href="">
                <div class="clip-header">
                    <i class="fa fa-arrow-up"></i>
                </div>
            </a>
            <h2 class="font-light m-b-xs">
               My Survivors
            </h2>
            <small>This is the list of survivors you are currently helping.</small>
        </div>
    </div>
	<div class="animate-panel" data-child="hpanel" data-effect="fadeInDown">
			<div class="hpanel" runat="server" id="divAddSurvivorButton" visible="false">
				<h3>No survivors yet.</h3>
				Click here to start helping someone affected by disaster. <a href="AddNewSurvivor.aspx">Add A Survivor</a>
			</div>
			<div class="hpanel" runat="server" id="divSurvivorList" visible="false">
				<div class="panel-body">
					To filter, type a term.
					<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search in table"/>
					<asp:Repeater ID="rpSurvivorListTable" runat="server" OnItemDataBound="rpSurvivorListTable_ItemDataBound">
						<HeaderTemplate>
							<table id="survivorTable" class="footable table table-bordered table-hover" data-page-size="20" data-filter="#filter">
								<thead>
									<tr>
										<th data-toggle="true">Name</th>
										<th data-toggle="true">Case Number</th>
										<th data-toggle="true">Phone Number</th>
										<th data-toggle="true" data-hide="phone,tablet">Email Address</th>
										<th data-toggle="true" data-hide="phone,tablet">Living Situation</th>
										<th data-toggle="true" data-hide="phone,tablet">Recovery Stage</th>
									</tr>
								</thead>
								<tbody>
						</HeaderTemplate>
						<ItemTemplate>
									<tr>
										<td><asp:HyperLink ID="hypName" runat="server"></asp:HyperLink></td>
										<td><asp:Literal ID="litProfileNumber" runat="server"></asp:Literal></td>
										<td><asp:HyperLink ID="hypPhone" runat="server"></asp:HyperLink></td>
										<td><asp:HyperLink ID="hypEmailAddress" runat="server"></asp:HyperLink></td>
										<td><asp:Literal ID="litLivingSituation" runat="server"></asp:Literal></td>
										<td><asp:Literal ID="litRecoveryStage" runat="server"></asp:Literal></td>
									</tr>
						</ItemTemplate>
						<FooterTemplate>
								</tbody>
								<tfoot>
									<tr>
										<td colspan="8">
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

