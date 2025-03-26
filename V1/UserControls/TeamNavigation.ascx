<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TeamNavigation.ascx.cs" Inherits="V1_UserControls_TeamNavigation" %>

<script type="text/javascript">
    $(document).ready(function () {

        var panelNav = $('.panelNav');
        var caret = panelNav.find('.caret');
        var collapseElement = $('#collapseTeamNavigation');

        // Remove touchstart and touchend handlers to prevent menu from closing while scrolling
        var touchStartY = 0;
        var touchEndY = 0;

        panelNav.on('click', function () {
            collapseElement.collapse('toggle');
        });

        // Function to automatically collapse the div on small screens
        function checkWindowSize() {
            if ($(window).width() < 768) {
                collapseElement.collapse({ 'toggle': true }).collapse('hide');
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

        $('#<%=divWebsite.ClientID%>').click(function () {
            window.location.href = '/Impactoid/CommunityPage.aspx?organizationId=<%=organizationId%>';
            return false;
        });
        $('#<%=divDeployment.ClientID%>').click(function () {
            window.location.href = '/V1/NonProfitAdministration/RespondToEvent.aspx?organizationId=<%=organizationId%>';
            return false;
        });
        $('#<%=donatenow.ClientID%>').click(function () {
            window.location.href = '/V1/NonProfit/Donation.aspx?organizationId=<%=organizationId%>';
            return false;
        });
    });
</script>

<style>
    .deployment:hover {
        background-color: #D6F0CC;
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
<div class="hpanel">
    <div class="panel-body">
        <asp:HyperLink ID="hypMyProfile" runat="server" NavigateUrl="/V1/Member/Default.aspx"><i class="fa fa-id-badge"></i> My Profile</asp:HyperLink><br />
        <asp:HyperLink runat="server" ID="hypGetHelp" CssClass="btn btn-danger btn-block"><i class='fa fa-check'></i> Get Help From This Team</asp:HyperLink>
        <button class="btn panelNav btn-block" type="button" data-bs-toggle="collapseTeam"
            data-bs-target="#collapseTeamNavigation" aria-expanded="false" aria-controls="collapseTeamNavigation">
            <span class="caret"></span>Team Navigation 
        </button>
        <div class="collapseTeam mt-2" id="collapseTeamNavigation">
            <ul class="mailbox-list">
                <hr runat="server" id="hr2"></hr>
                <li><b>Response Tools</b></li>
                <li <%=_streamActive%>>
                    <asp:HyperLink runat="server" ID="hypStream"><i class="fa fa-home"></i> Posts</asp:HyperLink>
                </li>
                <li <%=_deploymentPageActive%>>
                    <asp:HyperLink runat="server" ID="hypDeployments"><i class="fa fa-street-view"></i> Deployments</asp:HyperLink>
                </li>
                <li <%=_programsPageActive%>>
                    <asp:HyperLink runat="server" ID="hypPrograms"><i class="fa fa-superpowers"></i> Programs</asp:HyperLink>
                </li>
                <li <%=_teamCalendarActive%>>
                    <asp:HyperLink runat="server" ID="hypTeamCalendar"><i class="fa fa-calendar"></i> Calendar</asp:HyperLink>
                </li>
                <li <%=_activityPageActive%>>
                    <asp:HyperLink runat="server" ID="hypActivity"><i class="fa fa-rocket"></i> Impact Dashboard</asp:HyperLink>
                </li>
                <li <%=_teamPageActive%>>
                    <asp:HyperLink runat="server" ID="hypTeamName"><i class="fa fa-th-large"></i> About</asp:HyperLink>
                </li>
            </ul>
            <hr runat="server" id="hr1"></hr>
            <ul class="mailbox-list" runat="server" id="ul1">
                <li><b>Team Resources</b></li>
                <li <%=_peoplePageActive%>>
                    <asp:HyperLink runat="server" ID="hypPeople"><i class="fa fa-vcard"></i> Team Members</asp:HyperLink>
                </li>
                <li <%=_teamRolesActive%>>
                    <asp:HyperLink runat="server" ID="hypTeamRoles"><i class="fa fa-vcard"></i> Training</asp:HyperLink>
                </li>
                <li <%=_deploymentTeamActive%>>
                    <asp:HyperLink runat="server" ID="hypDeploymentTeam"><i class="fa fa-users"></i> Find Open Positions</asp:HyperLink>
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
                <li><b>Administrative Tools</b></li>
                <li>
                    <asp:HyperLink runat="server" ID="hypInviteTeam" CssClass="inviteButton"><i class="fa fa-users text-success"></i> Invite Team Members
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" ID="hypDonationDashboard" CssClass="donationDashboard"> <i class="fa fa-tachometer text-primary"></i> Donations Dashboard
                    </asp:HyperLink>
                </li>
                <hr runat="server" id="hr5"></hr>
                <li>Image Manager</li>
                <li>
                    <asp:HyperLink runat="server" ID="hypSquareLogoUpload"  CssClass="squareLogoUploadButton">  <i class="fa fa-upload text-primary"></i>Upload Team Logo
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" ID="hypLogoUpload" CssClass="logoUploadButton"> <i class="fa fa-upload text-primary"></i> Upload Website Logo
                    </asp:HyperLink>
                </li>         
                <li>
                    <asp:HyperLink runat="server" ID="hypCoverImageUpload" CssClass="coverUploadButton">  <i class="fa fa-image text-primary"></i> Upload Cover Image
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" ID="hypManagePhotos" CssClass="managePhotosButton"> <i class="fa fa-camera text-primary"></i> Manage Photos
                    </asp:HyperLink>
                </li>
                <hr runat="server" id="hr4"></hr>
                <li>Message All Team Members</li>
                <li <%=_teamMember%>>
                <asp:HyperLink runat="server" ID="hypMail" Visible="true"><i class="fa fa-envelope"></i>Email Team Members </asp:HyperLink>
                </li>
                <li <%=_teamMember%>>
                <asp:HyperLink runat="server" ID="hypSms" Visible="true"><i class="fa fa-file-text text-warning"></i>Text Team Members</asp:HyperLink>
                </li>
                <hr runat="server" id="hr3"></hr>

                <li <%=_ticketPageActive%>>
                    <asp:HyperLink runat="server" ID="hypTickets" Visible="false"><i class="fa fa-clipboard text-danger"></i> Tickets</asp:HyperLink>
                </li>
                <li <%=_reportPageActive%>>
                    <asp:HyperLink runat="server" ID="hypReports"><i class="fa fa-file-text text-warning"></i> Reports</asp:HyperLink>
                </li>
                <li <%=_settingsPageActive%>>
                    <asp:HyperLink runat="server" ID="hypSettings"><i class="fa fa-cog"></i> Settings</asp:HyperLink>
                </li>
                <li>
                    <asp:HyperLink runat="server" ID="hypUpdateTeamInfo" CssClass="editButton"><i class="fa fa-pencil text-warning"></i> Update Team Information
                    </asp:HyperLink>
                </li>
                <li>
                    <asp:LinkButton ID="btnDeactivatePage" runat="server" Visible="false" CssClass="deactivateButton" OnClick="btnChangePageStatus_Click"> <i class="fa fa-ban text-danger"></i> De-activate This Team
                    </asp:LinkButton>
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
    <div id="donatenow" runat="server" class="alert alert-success text-center deployment m-b-xs"
        visible="true">
        <h5 class="v1"><i class="fa fa-globe pe-2x"></i><b>Donate Now</b></h5>
    </div>
</div>
