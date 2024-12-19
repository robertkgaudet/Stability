<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/2-Column-Child.master" AutoEventWireup="true" CodeFile="Positions.aspx.cs" Inherits="V1_Member_Positions" %>
<%@ Register Src="~/V1/UserControls/MemberNavigation.ascx" TagPrefix="uc1" TagName="MemberNavigation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-xs-12">
				<div class="hpanel">
					<div class="panel-body">
						<h4>
							<asp:Literal ID="litPageName" runat="server"></asp:Literal>
						</h4>
	
						<input type="text" class="form-control input-sm m-b-md" id="filter" placeholder="Search Deployments">
						<table id="tblMyPositions" class="footable table table-stripped toggle-arrow-tiny" data-page-size="8" data-filter="#filter">
							<thead>
								<tr>
									<th>Date</th>
									<th>Arrival Time</th>
									<th data-toggle="true">Position</th>
								</tr>
							</thead>
							<tbody>
								<asp:Repeater ID="rptMyPositions" runat="server" OnItemDataBound="rptMyPositions_ItemDataBound">
									<ItemTemplate>
										<tr>
											<td>
												<asp:Label ID="lblDate" runat="server"></asp:Label>
											</td>
											<td>
												<asp:Label ID="lblArrivalTime" runat="server"></asp:Label>
											</td>
											<td style="font-weight:bold; font-size:15px;">
												<asp:Label  ID="lblPosition" runat="server"></asp:Label><br />
												<asp:HyperLink ID="hypGetTrained" Font-Underline="true" Target="_blank" runat="server" Text="Get Trained"></asp:HyperLink>
												<i class="fa fa-book"></i>
											</td>
										</tr>
										<tr>
											<td colspan="3">
												<table style="width:100%; margin-bottom:100px;">
													<tr>
														<td style="vertical-align:top; padding:10px; width:50%;">
															<asp:Label ID="lblLocation" runat="server"></asp:Label>
														</td>
														<td style="vertical-align:top; padding:10px; width:50%;">
															<asp:Label ID="lblDetails" runat="server"></asp:Label>
														</td>
													</tr>
												</table>
											</td>
										</tr>
									</ItemTemplate>
								</asp:Repeater>
							</tbody>
						</table>
					</div>
				</div>
			</div>
			<!--NAVIGATION-->
			<div class="col-sm-4 col-lg-3">
			</div>
		</div>
		<script type="text/javascript">

			$(window).on('load', function () {
				$('.footable').footable();
			});
		</script>
		<script src="/Homer/vendor/fooTable/dist/footable.all.min.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
	<uc1:MemberNavigation runat="server" ID="ucMemberNavigation" />
</asp:Content>