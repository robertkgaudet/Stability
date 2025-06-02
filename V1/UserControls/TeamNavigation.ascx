<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamNavigation.ascx.cs"
    Inherits="V1_UserControls_TeamNavigation" %>

<script type="text/javascript">
    $(document).ready(function () {

        var panelNav = $('.panelNav');
        var caret = panelNav.find('.caret');
        var collapseElement = $('#collapseTeamNavigation');

        // Remove touchstart and touchend handlers to prevent menu from closing while scrolling
        var touchStartY = 0;
        var touchEndY = 0;

        //panelNav.on('click', function () {
        //    collapseElement.collapse('toggle');
        //});

        // Function to automatically collapse the div on small screens
        //     function checkWindowSize() {
        //if ($(window).width() < 768) {
        //	collapseElement.removeClass('in'); // Hide collapse manually
        //	//collapseElement.collapse({ 'toggle': true }).collapse('hide');
        //         }
        //     }

        function checkWindowSize() {
            if ($(window).width() < 768) {
                collapseElement.removeClass('in'); // Hide it on small screens
            } else {
                collapseElement.addClass('in'); // Show it on larger screens
            }
        }

        // Check window size on page load and window resize
        $(window).on('resize', checkWindowSize);
        checkWindowSize();  // Initial check on page load

        // Adjust caret direction based on collapse state
        collapseElement.on('hidden.bs.collapse', function () {
            caret.removeClass('caret-up');
        });

        collapseElement.on('shown.bs.collapse', function () {
            caret.addClass('caret-up');
        });


        // Prevent toggling while scrolling – removed touch event logic to avoid interference during scroll
        // panelNav.on('touchstart', function (e) {
        //     touchStartY = e.originalEvent.touches[0].clientY;
        // });

        // panelNav.on('touchend', function (e) {
        //     touchEndY = e.originalEvent.changedTouches[0].clientY;
        //     if (Math.abs(touchStartY - touchEndY) < 10) { // Adjust the threshold as needed
        //         $(this).click();
        //     }
        // });

        $('#<%=divTeamConfiguration.ClientID%>').click(function () {
            window.location.href = '/V1/DeploymentDirectorSplash.html';
            return false;
        });

        $('#<%=divWebsite.ClientID%>').click(function () {
            window.location.href = '/Impactoid/CommunityPage.aspx?organizationId=<%=organizationId%>';
            return false;
		});

        $('#<%=divDeployment.ClientID%>').click(function () {
            window.location.href = '/V1/NonProfitAdministration/RespondToEvent.aspx?organizationId=<%=organizationId%>';
            return false;
		});

		$('#<%=donatenow.ClientID%>').click(function () {

			window.location.href = '<%=donationLink%>'
            return false;
        });
    });
</script>

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
        display: inline-block;
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
<div class="hpanel">
    <div class="panel-body">
        <div id="desktopNavigation" class="m-b-lg">
            <div id="divTeamConfiguration" runat="server" class="alert alert-info text-center deployment">
                <h5 class="v1"><i class="fa fa-user pe-2x"></i><b>Launch Deployment Director Training</b>
                </h5>
                Team, Website, and Deployment Management
            </div>
        </div>
		<div id="donatenow" runat="server" class="alert alert-success text-center deployment m-t-n-md" visible="true">
			<h5 class="v1"><i class="fa fa-heart pe-2x"></i><b>Donate To This Team</b></h5>
		</div>
		<br />
        <asp:HyperLink runat="server" ID="hypGetHelp" CssClass="btn btn-danger btn-block"><i class='fa fa-check'></i> Get Help From This Team</asp:HyperLink>
        <button class="btn panelNav btn-block"
            type="button"
            data-toggle="collapse"
            data-target="#collapseTeamNavigation"
            aria-expanded="false"
            aria-controls="collapseTeamNavigation">
            <span class="caret"></span>Team Navigation 
        </button>
        <div class="collapse in mt-2" id="collapseTeamNavigation">
            <ul class="mailbox-list">
                <hr runat="server" id="hr2"></hr>
                <ul class="mailbox-list" runat="server" id="ul1">
                    <li><b>Team Resources</b></li>
					<li <%=_activityPageActive%>>
						<asp:HyperLink runat="server" ID="hypActivity"><i class="fa fa-rocket"></i> Activity Dashboard</asp:HyperLink>
					</li>
					<li <%=_programsPageActive%>>
						<asp:HyperLink runat="server" ID="hypPrograms"><i class="fa fa-superpowers"></i> Programs</asp:HyperLink>
					</li>
                    <li <%=_peoplePageActive%>>
                        <asp:HyperLink runat="server" ID="hypPeople"><i class="fa fa-sitemap"></i> Team Members</asp:HyperLink>
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

                </ul>
                <hr runat="server" id="hrAdmin" visible="false"></hr>
                <li><b>Response Tools</b></li>
                    <li <%=_deploymentTeamActive%>>
                        <asp:HyperLink runat="server" ID="hypDeploymentTeam"><i class="fa fa-life-ring font-bold"></i> Volunteer Opportunities</asp:HyperLink>
                    </li>
                <li <%=_deploymentPageActive%>>
                    <asp:HyperLink runat="server" ID="hypDeployments"><i class="fa fa-street-view"></i> Deployments</asp:HyperLink>
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
                <%--<li><b>Team Management </b></li>--%>
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
    <div id="divWebsite" runat="server" class="alert alert-info text-center website m-b-xs">
        <h5 class="v1"><i class="fa fa-globe pe-2x"></i><b>Team Member Website</b></h5>
    </div>
    <div id="divDeployment" runat="server" class="alert alert-success text-center deployment m-b-xs"
        visible="false">
        <h5 class="v1"><i class="fa fa-street-view pe-2x"></i><b>Create A New Deployment</b>
        </h5>
    </div>
</div>
