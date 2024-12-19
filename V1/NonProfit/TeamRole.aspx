<%@ Page Title="" Language="C#" MasterPageFile="~/V1/MasterPages/1-Column-Child.master" AutoEventWireup="true" CodeFile="TeamRole.aspx.cs" Inherits="V1_NonProfit_TeamRole" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
	<style>
		.video-container {
			padding-bottom: 56.25%; /* 16:9 aspect ratio (divide height by width) */
			height: 0;
			max-width: 100%;
			background: #000;
		}

		.video-container iframe {
			position: absolute;
			top: 0;
			left: 0;
			width: 100%;
			height: 100%;
		}
	</style>
	<style>
	  #notes {
		max-height: 300px;
		overflow-y: auto;
	  }

	  /* Custom scrollbar styling */
	  #notes::-webkit-scrollbar {
		width: 8px; /* Adjust the width to make it smaller */
	  }

	  #notes::-webkit-scrollbar-thumb {
		background-color: #cccccc; /* You can change this color as needed */
		border-radius: 4px; /* Optional: round the edges */
	  }

	  #notes::-webkit-scrollbar-track {
		background: #f1f1f1; /* Optional: color for the scrollbar track */
	  }
	</style>
    <script>
		function printChecklist(divId) {
			// Get the content of the section to print
			var printContent = document.getElementById(divId).innerHTML;
			// Create a new window or document
			var originalContents = document.body.innerHTML;
			document.body.innerHTML = printContent;
			window.print();
			document.body.innerHTML = originalContents;
			//location.reload(); // Reload to restore event listeners if needed
		}

		$(document).ready(function () {
			$("body").tooltip({ selector: '[data-toggle=tooltip]' });
		});
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-md-3">
            <div class="hpanel panel-group">
                <div class="panel-body">
					<asp:HyperLink id="hypLogo" runat="server">
					<asp:Image runat="server" id="imgLogo"></asp:Image>
					</asp:HyperLink>
                    <div class="text-center text-muted font-bold">
						<asp:Label ID="lblProgramName" runat="server" Text="Team Training and Role Information"></asp:Label>
					</div>
                </div>
                <div class="panel-section">

                    <div class="input-group">
						<asp:HyperLink ID="hypProgramRoles" runat="server"></asp:HyperLink><br />
						<a href="/V1/NonProfit/TeamRoles.aspx?organizationId=<%=organizationId%>">All Team Roles</a>
						
                    </div>
                    <button type="button" data-toggle="collapse" data-target="#notes" class="btn-sm visible-xs visible-sm collapsed btn-default btn btn-block m-t-sm">
                        All Roles <i class="fa fa-angle-down"></i>
                    </button>
                </div>

                <div id="notes" class="collapse" style="max-height: 600px; overflow-y: auto;">
					<asp:Repeater ID="dlPositions" runat="server" OnItemDataBound="dlPositions_ItemDataBound">
						<ItemTemplate>
							<div class="panel-body note-link">
								<asp:HyperLink ID="hypRoles" runat="server">
									<h5 class="m-b-xs">
										<asp:Literal ID="litRoleName" runat="server"></asp:Literal>
										<i runat="server" id="iVideo" class="fa fa-youtube-play pull-right" visible="false" data-toggle="tooltip" data-placement="top" title="Has Training Video"></i>
										<i runat="server" id="iChecklist" class="fa fa-check-square pull-right" visible="false" data-toggle="tooltip" data-placement="top" title="Has Associated Checklist"></i>
									</h5>
									<small class="text-muted">
										<asp:Literal ID="litDeploymentStatus" runat="server"></asp:Literal>
									</small>
								</asp:HyperLink>
							</div>
						</ItemTemplate>
					</asp:Repeater>
                </div>
            </div>
        </div>

        <div class="col-md-9">
			<div class="btn-group" runat="server" id="divEditAdd" visible="false">
				<button class="btn btn-sm btn-default" runat="server" id="btnEdit"><i class='fa fa-edit'></i> Edit</button>
				<button class="btn btn-sm btn-default" runat="server" id="btnAddRole"><i class='fa fa-vcard'></i> Add Role</button>
			</div>
			<h2><asp:Literal ID="litRoleName" runat="server"></asp:Literal></h2>
			<asp:Literal ID="litPositionSummary" runat="server"></asp:Literal>
			<ul class="nav nav-tabs">
				<li class="active"><a data-toggle="tab" href="#tab-3">Training</a></li>
				<li class=""><a data-toggle="tab" href="#tab-1">Role Description</a></li>
				<li class=""><a data-toggle="tab" href="#tab-4">Checklist</a></li>
				<li class=""><a data-toggle="tab" href="#tab-2">Programs</a></li>
				<li class=""><a data-toggle="tab" href="#tab-5">History</a></li>
			</ul>
            <div class="tab-content">
				<div id="tab-3" class="tab-pane active">
					<div class="panel-body">
						<div class="hpanel" id="divTrainingVideo" runat="server" visible="false">
							<div class="panel-body">
								<div class="video-container">
									<iframe width="560" height="315" src="https://www.youtube.com/embed/<%=videoId%>"
										title="YouTube video player"
										frameborder="0"
										allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
										referrerpolicy="strict-origin-when-cross-origin"
										allowfullscreen></iframe>
								</div>
							</div>
							<hr />
						</div>
						<div class="hpanel">
						<button class="btn btn-success pull-right" onclick="printChecklist('divTraining')">Print Training</button>
						<br /><br />
							<div class="panel-body">
								<div id="divTraining">
									<h3><%=positionName%> Training</h3>
									<asp:Literal ID="litTrainingText" runat="server"></asp:Literal>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div id="tab-1" class="tab-pane">
					<div class="hpanel">
						<div class="panel-body">
							<button class="btn btn-success pull-right" onclick="printChecklist('divDescription')">Print Description</button>
							<br />
							<div id="divDescription">
								<h3><%=positionName%> Description</h3>
								<div class="text-muted">
									<ul>
									<asp:Literal ID="litDeploymentStatus" runat="server"></asp:Literal>
									<asp:Literal ID="litRequiresTraininig" runat="server"></asp:Literal>
									<asp:Literal ID="litRequiresCertification" runat="server"></asp:Literal>
									<asp:Literal ID="litSharedOrTeam" runat="server"></asp:Literal>
									</ul>
								</div>
								<hr/>
								<div class="note-content">
									<asp:Literal ID="litRoleDescription" runat="server"></asp:Literal>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div id="tab-2" class="tab-pane">
					<div class="panel-body">
						<div class="hpanel">
							<div class="panel-body">
								<div id="divPrograms">
									<h3><i><%=positionName%></i> Programs</h3>
									<asp:Repeater ID="ProgramRepeater" runat="server">
										<ItemTemplate>
											<tr>
												<td>
													<h5><%# Eval("ProgramName") %></h5>
													<%# Eval("ProgramDescription") %>
												</td>
											</tr>
										</ItemTemplate>
									</asp:Repeater>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div id="tab-4" class="tab-pane">
					<div class="panel-body">
						<div>
							<button class="btn btn-success pull-right" onclick="printChecklist('divChecklist')">Print Checklist</button>
							<div class="modal fade" id="divChecklist" tabindex="-1" role="dialog" aria-hidden="true">
								<div class="modal-dialog">
									<div class="modal-content">
										<div class="color-line"></div>
										<div class="modal-header text-center">
											<h4 class="modal-title">
												<asp:Literal id="litChecklistPostionName" runat="server"></asp:Literal>
											</h4>
											<small class="font-bold">
												<asp:Literal id="litChecklistPositionSummary" runat="server"></asp:Literal>
											</small>
										</div>
										<div class="modal-body">
											<p>
												<asp:Literal id="litChecklistPrintable" runat="server"></asp:Literal>
											</p>
										</div>
									</div>
								</div>
							</div>
							<br />
							<asp:Literal ID="litChecklist" runat="server"></asp:Literal>
						</div>
					</div>
				</div>
				<div id="tab-5" class="tab-pane">
					<div class="hpanel">
						<div class="panel-body">
							<h6>Number of Days Volunteered</h6>
							<asp:Repeater ID="UserCountRepeater" runat="server">
								<ItemTemplate>
										<%# Eval("Firstname") %> <%# Eval("Lastname") %> - <strong><%# Eval("Count") %></strong>, 
								</ItemTemplate>
							</asp:Repeater>
							<div class="v-timeline vertical-container animate-panel"  data-child="vertical-timeline-block" data-delay="1">
								<asp:Repeater ID="rptEvent" runat="server">
									<ItemTemplate>
										<div class="vertical-timeline-block">
											<div class="vertical-timeline-icon navy-bg">
												<i class="fa fa-calendar"></i>
											</div>
											<div class="vertical-timeline-content">
												<div class="p-sm">
													<span class="vertical-date pull-right">
														<%# Eval("EventName") %>
													</span>
													<h2>
														<%# Eval("DeploymentDate") %>
													</h2>
													<p>
														<asp:Repeater ID="UserRepeater" runat="server" DataSource='<%# Eval("Users") %>'>
															<ItemTemplate>
																<div>
																	<a href="/V1/Member/Default.aspx?userid=<%# Eval("UserId") %>">
																		<%# Eval("Firstname") %> <%# Eval("Lastname") %>
																	</a>
																</div>
															</ItemTemplate>
														</asp:Repeater>
													</p>
												</div>
											</div>
										</div>
									</ItemTemplate>
								</asp:Repeater>
							</div>
						</div>
					</div>
				</div>
            </div>
        </div>

</asp:Content>

