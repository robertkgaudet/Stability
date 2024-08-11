<%@ page title="" language="C#" masterpagefile="~/CaseManagement/MasterPages/CaseManagement.master" autoeventwireup="true" inherits="CaseManagement_Time, App_Web_5cuksoem" %>

<%@ Register Src="~/V1/UserControls/TimeControls.ascx" TagPrefix="uc1" TagName="TimeControls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.margin-top-20
		{
			margin:20px;
		}

		.table, .table-bordered
		{
			margin:0px;
		}

		.no-padding {
		  padding:0px;
		  margin:0px;
		}
		ul {
				padding:0px;
				margin:0px;
				list-style-type: none;
				display:block;
			}
		.row{padding:3px;}
		.container-fluid{margin:0px;}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
		<div class="row">
			<div class="col-lg-12">
				<div class="normalheader animate-panel" data-child="hpanel" data-effect="fadeInDown">
					<div class="hpanel">
						<div class="panel-body">
							<h2 class="font-light m-b-xs">
								Volunteer Time Tracking
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
							Volunteer Time Clock
						</div>
						<div class="panel-body">
							

							<uc1:TimeControls runat="server" ID="TimeControls" />


						</div>
						<div class="panel-footer">
						</div>
					</div>

				</div>
			</div>
			<div class="row">
				<div class="col-lg-12 container">

					<div class="hpanel">
						<div class="panel-heading hbuilt">
							Volunteer Time Reporting
						</div>
						<div class="panel-body">
							

							<div class="container-fluid" id="divFormFields" runat="server">
								<div class="row">
									<div class="col-xs-12 col-sm-6">
										<div class="center-block">
											<h5>My Timesheet</h5>
											<table class="table table-bordered table-responsive">
												<thead style="font-weight:bold;">
													<tr>
														<td style="width:40%">
															Task - Description
														</td>
														<td style="width:40%">
															<ul>
																<li class="de">Time In</li>
																<li>Time Out</li>
																<li>Total Time</li>
															</ul>
														</td>
														<td style="width:20%">
															Points
														</td>
													</tr>
												</thead>
												<tbody>
											<asp:DataList ID="dlTimesheet" runat="server" RepeatLayout="Flow" OnItemDataBound="dlTimesheet_ItemDataBound">
												<ItemTemplate>
													<tr>
														<td>
															<asp:Label ID="lblTaskName" runat="server"></asp:Label> - <asp:Label ID="lblDescription" runat="server"></asp:Label>
														</td>
														<td>
															<table>
																<tr>
																	<td>
																		<asp:Label ID="lblDate" runat="server"></asp:Label>
																	</td>
																</tr>
																<tr>
																	<td>
																		<asp:Label ID="lblTimein" runat="server"></asp:Label>
																	</td>
																</tr>
																<tr>
																	<td>
																		<asp:Label ID="lblTimeOut" runat="server"></asp:Label>
																	</td>
																</tr>
																<tr>
																	<td>
																		<asp:Label ID="lblTime" runat="server"></asp:Label>
																	</td>
																</tr>
															</table>
														</td>
														<td>
															<asp:Label ID="lblPoints" runat="server"></asp:Label>
														</td>
													</tr>
												</ItemTemplate>
											</asp:DataList>
												</tbody>
											</table>
										</div>
									</div>
								</div>
							</div>



						</div>
									
						<div class="panel-footer">
						</div>
					</div>

				</div>
			</div>
		</div>
</asp:Content>