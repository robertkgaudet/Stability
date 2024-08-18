<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamNavigation.ascx.cs" Inherits="V1_UserControls_TeamNavigation" %>

				<script type="text/javascript">
					$(document).ready(function () {

						var panelNav = $('.panelNav');
						var caret = panelNav.find('.caret');
						var collapseElement = $('#collapseTeamNavigation');

						var touchStartY = 0;
						var touchEndY = 0;

						// Toggle caret direction on panelNav click
						panelNav.on('click', function () {
							collapseElement.collapse('toggle');
						});

						// Function to automatically collapse the div on small screens
						function checkWindowSize() {
							if ($(window).width() < 768) {
								collapseElement.collapse({'toggle': true }).collapse('hide');
							}
						}

						// Check window size on page load and window resize
						$(window).on('resize', checkWindowSize);

						// Adjust caret direction based on collapse state
						collapseElement.on('hidden.bs.collapse', function () {
							caret.removeClass('caret-up');
						});
						collapseElement.on('shown.bs.collapse', function () {
							caret.addClass('caret-up');
						});
						checkWindowSize();  // Initial check on page load

						// Prevent toggling while scrolling
						panelNav.on('touchstart', function (e) {
							touchStartY = e.originalEvent.touches[0].clientY;
						});

						panelNav.on('touchend', function (e) {
							touchEndY = e.originalEvent.changedTouches[0].clientY;
							if (Math.abs(touchStartY - touchEndY) < 10) { // Adjust the threshold as needed
								$(this).click();
							}
						});

						$('#<%=divDeployment.ClientID%>').click(function () {
							window.location.href = '/V1/NonProfitAdministration/RespondToEvent.aspx?organizationId=<%=organizationId%>';
							return false;
						});
					});


				</script>

				<style>
					.deployment:hover
					{
						background-color:#D6F0CC;
						cursor:pointer;
					}

					.caret {
						border-top: 4px solid;
						border-right: 4px solid transparent;
						border-left: 4px solid transparent;
						display: inline-block;
						width: 0;
						height: 0;
						vertical-align: middle;
						transition: transform 0.3s ease-in-out;
					}
					.caret-up {
						transform: rotate(180deg);
					}

					.panelNav:hover
					{
						background-color:#E8D3FE;
					}
					.text-website
					{
						color:white;
					}
			</style>

				<div id="divDeployment" runat="server" class="alert alert-success text-center deployment m-b-xs" visible="false">
					<h5><i class="fa fa-street-view pe-2x pull-left"></i><b>Create A New Deployment</b></h5>
				</div>
                <div class="hpanel">
                    <div class="panel-body">
						<asp:HyperLink runat="server" ID="hypGetHelp" CssClass="btn btn-danger btn-block"><i class='fa fa-check'></i> Get Help From This Team</asp:HyperLink>
						<button class="btn panelNav btn-block" type="button" data-bs-toggle="collapseTeam" data-bs-target="#collapseTeamNavigation" aria-expanded="false" aria-controls="collapseTeamNavigation">
							<span class="caret"></span> Team Navigation
						</button>
						<div class="collapseTeam mt-2" id="collapseTeamNavigation">
							<ul class="mailbox-list">
								<li <%=_streamActive%>>
									<asp:HyperLink runat="server" ID="hypStream"><i class="fa fa-home"></i> Home</asp:HyperLink>
								</li>
								<li <%=_teamPageActive%>>
									<asp:HyperLink runat="server" ID="hypTeamName"><i class="fa fa-th-large"></i> About</asp:HyperLink>
								</li>
								<li <%=_teamCalendarActive%>>
									<asp:HyperLink runat="server" ID="hypTeamCalendar"><i class="fa fa-calendar"></i> Team Calendar</asp:HyperLink>
								</li>
								<li <%=_activityPageActive%>>
									<asp:HyperLink runat="server" ID="hypActivity"><i class="fa fa-rocket"></i> Impact Dashboard</asp:HyperLink>
								</li>
								<li <%=_deploymentPageActive%>>
									<asp:HyperLink runat="server" ID="hypDeployments"><i class="fa fa-street-view"></i> Deployments</asp:HyperLink>
								</li>
								<li <%=_programsPageActive%>>
									<asp:HyperLink runat="server" ID="hypPrograms"><i class="fa fa-superpowers"></i> Programs</asp:HyperLink>
								</li>
								<li <%=_peoplePageActive%>>
									<asp:HyperLink runat="server" ID="hypPeople"><i class="fa fa-vcard"></i> People</asp:HyperLink>
								</li>
								<li <%=_skillsPageActive%>>
									<asp:HyperLink runat="server" ID="hypSkillsets"><i class="fa fa-hand-pointer-o"></i> Skillsets</asp:HyperLink>
								</li>
								<li <%=_resourcesPageActive%>>
									<asp:HyperLink runat="server" ID="hypResources"><i class="fa fa-truck"></i> Resources</asp:HyperLink>
								</li>
							</ul>
							<hr runat="server" id="hrAdmin" visible="false"></hr>
							<ul class="mailbox-list" runat="server" id="ulAdmin" visible="false">
								<li <%=_ticketPageActive%>>
									<asp:HyperLink runat="server" ID="hypTickets" Visible="false"><i class="fa fa-clipboard text-danger"></i> Tickets</asp:HyperLink>
								</li>
								<li <%=_reportPageActive%>>
									<asp:HyperLink runat="server" ID="hypReports"><i class="fa fa-file-text text-warning"></i> Reports</asp:HyperLink>
								</li>
								<li <%=_settingsPageActive%>>
									<asp:HyperLink runat="server" ID="hypSettings"><i class="fa fa-cog"></i> Settings</asp:HyperLink>
								</li>
							</ul>
							<hr>
							<ul class="mailbox-list">
								<li <%=_supportPageActive%>>
									<asp:HyperLink runat="server" ID="hypSupport"><i class="fa fa-info-circle"></i> Support</asp:HyperLink>
								</li>
							</ul>
							<asp:HyperLink runat="server" ID="hypJoinTeam" CssClass="btn btn-success btn-block"><i class='fa fa-check'></i> Request To Join This Team</asp:HyperLink>
						</div>
                    </div>
                </div>