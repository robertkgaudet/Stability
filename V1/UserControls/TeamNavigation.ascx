<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamNavigation.ascx.cs"
    Inherits="V1_UserControls_TeamNavigation" %>

<style>
    .deployment:hover {
        background-color: darkseagreen;
        cursor: pointer;
    }

    .website:hover {
        background-color: #BCE8F1;
        cursor: pointer;
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

    .panelNav:hover {
        background-color: #E8D3FE;
    }

    .text-website {
        color: white;
    }

    .v1 {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-left: 20px;
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {

		$('#<%=donatenow.ClientID%>').click(function () {
			window.location.href = '<%=donationLink%>'
			return false;
		});


        $('#<%=divTeamConfiguration.ClientID%>').click(function () {
            window.location.href = '/V1/DeploymentDirectorSplash.html';
            return false;
        });

        $('#<%=divDeployment.ClientID%>').click(function () {
            window.location.href = '/V1/NonProfitAdministration/RespondToEvent.aspx?organizationId=<%=organizationId%>';
            return false;
		});

		// Change button text and caret direction based on collapse state
		$('#hpanelCollapse').on('shown.bs.collapse', function () {
			$('#toggleText').text('Close Navigation');
			$('#toggleCaret').addClass('caret-up').removeClass('caret');
		});

		$('#hpanelCollapse').on('hidden.bs.collapse', function () {
			$('#toggleText').text('Open Navigation');
			$('#toggleCaret').addClass('caret').removeClass('caret-up');
		});

		function checkWindowSize() {
			if ($(window).width() < 768) {
				$('#hpanelCollapse').addClass('collapse'); // Collapse on small screens
			} else {
				$('#hpanelCollapse').removeClass('collapse'); // Expand on larger screens
			}
		}

		// Check window size on page load and window resize
		$(window).on('resize', checkWindowSize);
		checkWindowSize(); // Initial check on page load
	});
</script>

<div class="hpanel">
	    <!-- Toggle button for mobile -->
    <asp:HyperLink runat="server" ID="hypGetHelp" CssClass="btn btn-danger btn-block"><i class='fa fa-check'></i> Get Help From This Team</asp:HyperLink>
    <button id="toggleButton" class="btn btn-primary btn-block visible-xs" type="button" data-toggle="collapse" data-target="#hpanelCollapse" aria-expanded="false" aria-controls="hpanelCollapse">
        <span id="toggleText">Open Navigation</span>   <i id="toggleCaret" class="caret"></i>
    </button>
    <div id="hpanelCollapse" class="panel-body">
		<div id="donatenow" runat="server" class="alert alert-success text-center deployment m-t-sm" visible="true">
			<h5 class="v1"><i class="fa fa-heart pe-2x"></i><b>Donate To This Team</b></h5>
		</div>
        <div id="memNavigation">
            <hr runat="server" id="hr2"></hr>
            <ul class="mailbox-list">
                <li <%=_deploymentTeamActive%>>
                    <asp:HyperLink runat="server" ID="hypDeploymentTeam"><i class="fa fa-life-ring font-bold"></i> Volunteer Opportunities</asp:HyperLink>
                </li>
				<li <%=_activityPageActive%>>
					<asp:HyperLink runat="server" ID="hypActivity"><i class="fa fa-rocket"></i> Team Impact</asp:HyperLink>
				</li>
                <li <%=_peoplePageActive%>>
                    <asp:HyperLink runat="server" ID="hypPeople"><i class="fa fa-sitemap"></i> All Team Members</asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" Target="_blank" ID="hypTeamWebsite"><i class="fa fa-globe font-bold"></i> Open Website</asp:HyperLink>
                </li>
                <hr runat="server" id="hrAdmin"></hr>
                <li>Coordinate Your Team</li>
                <li <%=_deploymentPageActive%>>
                    <asp:HyperLink runat="server" ID="hypDeployments"><i class="fa fa-street-view"></i> Deployments</asp:HyperLink>
                </li>
				<li <%=_programsPageActive%>>
					<asp:HyperLink runat="server" ID="hypPrograms"><i class="fa fa-superpowers"></i> Programs</asp:HyperLink>
				</li>
                <li <%=_teamRolesActive%>>
                    <asp:HyperLink runat="server" ID="hypTeamRoles"><i class="fa fa-graduation-cap"></i> Training</asp:HyperLink>
                </li>
                <li <%=_skillsPageActive%>>
                    <asp:HyperLink runat="server" ID="hypSkillsets"><i class="fa fa-user-md"></i> Skillsets</asp:HyperLink>
                </li>
                <li <%=_resourcesPageActive%>>
                    <asp:HyperLink runat="server" ID="hypResources"><i class="fa fa-truck"></i> Equipment</asp:HyperLink>
                </li>
				<li <%=_teamPageActive%>>
					<asp:HyperLink runat="server" ID="hypTeamName"><i class="fa fa-th-large"></i> About</asp:HyperLink>
				</li>
                <li <%=_streamActive%>>
                    <asp:HyperLink runat="server" ID="hypStream"><i class="fa fa-home"></i> Posts</asp:HyperLink>
                </li>
                <li <%=_teamCalendarActive%>>
                    <asp:HyperLink runat="server" ID="hypTeamCalendar"><i class="fa fa-calendar"></i> Calendar</asp:HyperLink>
                </li>
            </ul>
            <hr runat="server" id="hr1"></hr>
            <ul class="mailbox-list" runat="server" id="ulAdmin" visible="false">
                <li><b>
                    <asp:HyperLink runat="server" ID="hypTeamManagement" Visible="false"><i class="fa fa-users text-success"></i> Team Management
                    </asp:HyperLink></b>
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
<div id="desktopNavigation">
    <div id="desktopNavigation" class="m-b-lg">
        <div id="divTeamConfiguration" runat="server" class="alert alert-info text-center deployment">
            <h5 class="v1"><i class="fa fa-user pe-2x"></i><b>Launch Deployment Director Training</b>
            </h5>
            Team, Website, and Deployment Management
        </div>
    </div>
    <div id="divDeployment" runat="server" class="alert alert-success text-center deployment m-b-xs"
        visible="false">
        <h5 class="v1"><i class="fa fa-street-view pe-2x"></i><b>Create A New Deployment</b>
        </h5>
    </div>
</div>
