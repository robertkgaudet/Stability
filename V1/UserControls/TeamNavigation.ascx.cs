using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_TeamNavigation : System.Web.UI.UserControl
{
    public string _pageName;
    public string _teamRolesActive;
    public string _teamName;
    public string _teamPageActive;
    public string _teamCalendarActive;
    public string _reportPageActive;
    public string _deploymentPageActive;
    public string _activityPageActive;
    public string _programsPageActive;
    public string _peoplePageActive;
    public string _websitePageActive;
    public string _ticketPageActive;
    public string _settingsPageActive;
    public string _supportPageActive;
    public string _skillsPageActive;
    public string _resourcesPageActive;
    public string _streamActive;
    public string _deploymentTeamActive;
    public string organizationId;
    public string _teamMember;
    public bool isUserOnTeam = false;
	public string donationLink = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
    {
        hypPeople.Attributes["data-toggle"] = "tooltip";
        hypPeople.Attributes["title"] = "View members of this team";

        hypTeamRoles.Attributes["data-toggle"] = "tooltip";
        hypTeamRoles.Attributes["title"] = "Enter your training portal";

        hypDeploymentTeam.Attributes["data-toggle"] = "tooltip";
        hypDeploymentTeam.Attributes["title"] = "Search this team's open positions";

        hypSkillsets.Attributes["data-toggle"] = "tooltip";
        hypSkillsets.Attributes["title"] = "View this team's skillsets";

        hypResources.Attributes["data-toggle"] = "tooltip";
        hypResources.Attributes["title"] = "View this team's equipment";

        hypStream.Attributes["data-toggle"] = "tooltip";
        hypStream.Attributes["title"] = "View this team's posts";

        hypDeployments.Attributes["data-toggle"] = "tooltip";
        hypDeployments.Attributes["title"] = "View this team's deployments";

        hypPrograms.Attributes["data-toggle"] = "tooltip";
        hypPrograms.Attributes["title"] = "View this team's programs";

        hypTeamCalendar.Attributes["data-toggle"] = "tooltip";
        hypTeamCalendar.Attributes["title"] = "View this team's calendar";

		hypTeamName.Attributes["data-toggle"] = "tooltip";
		hypTeamName.Attributes["title"] = "View details about this team";

		hypActivity.Attributes["data-toggle"] = "tooltip";
		hypActivity.Attributes["title"] = "View this teams overall impact";


		hypSupport.Attributes["data-toggle"] = "tooltip";
		hypSupport.Attributes["title"] = "Having problems? Request help here.";

		hypTeamWebsite.Attributes["data-toggle"] = "tooltip";
		hypTeamWebsite.Attributes["title"] = "Visit the team website.";


		//organizationId = Request.QueryString["organizationId"];
		hypStream.NavigateUrl = "/V1/NonProfit/Stream.aspx?organizationId=" + organizationId;
		hypTeamWebsite.NavigateUrl = "/Impactoid/CommunityPage.aspx?organizationId=" + organizationId;
		hypTeamRoles.NavigateUrl = "/V1/NonProfit/TeamRoles.aspx?organizationId=" + organizationId;
        hypPrograms.NavigateUrl = "/V1/NonProfit/Programs.aspx?organizationId=" + organizationId;
        hypDeployments.NavigateUrl = "/V1/NonProfit/Deployments.aspx?organizationId=" + organizationId;
        hypTeamName.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + organizationId;
        hypTeamCalendar.NavigateUrl = "/V1/NonProfit/TeamAvailabilityCalendar.aspx?organizationId=" + organizationId;
        hypPeople.NavigateUrl = "/V1/NonProfit/People.aspx?organizationId=" + organizationId;
        hypSupport.NavigateUrl = "/V1/NonProfit/Support.aspx?organizationId=" + organizationId;
        hypSkillsets.NavigateUrl = "/V1/NonProfit/Skillsets.aspx?organizationId=" + organizationId;
        hypResources.NavigateUrl = "/V1/NonProfit/AvailableResources.aspx?organizationId=" + organizationId;
        hypDeploymentTeam.NavigateUrl = "/V1/NonProfit/DeploymentTeams.aspx?organizationId=" + organizationId;
        hypTeamManagement.NavigateUrl = "/V1/NonProfit/TeamManagement.aspx?organizationId=" + organizationId;
        //hypRequest.NavigateUrl = "/V1/NonProfit/ReceivedRequests.aspx?organizationId=" + organizationId;

        switch (PageName)
        {
            case "teamRolesPage":
                _teamRolesActive = "class=\"active\"";
                break;
			case "streamPage":
                _streamActive = "class=\"active\"";
                break;
            case "deploymentTeamPage":
                _deploymentTeamActive = "class=\"active\"";
                break;
            case "skillsetsPage":
                _skillsPageActive = "class=\"active\"";
                break;
            case "availableResourcesPage":
                _resourcesPageActive = "class=\"active\"";
                break;
            case "teamCalendarPage":
                _teamCalendarActive = "class=\"active\"";
                break;
            case "teamPage":
                _teamPageActive = "class=\"active\"";
                break;
            case "reportPage":
                _reportPageActive = "class=\"active\"";
                break;
            case "deploymentPage":
                _deploymentPageActive = "class=\"active\"";
                break;
            case "activityPage":
                _activityPageActive = "class=\"active\"";
                break;
            case "programsPage":
                _programsPageActive = "class=\"active\"";
                break;
            case "peoplePage":
                _peoplePageActive = "class=\"active\"";
                break;
            case "websitePage":
                _websitePageActive = "class=\"active\"";
                break;
            case "ticketPage":
                _ticketPageActive = "class=\"active\"";
                break;
            case "settingsPage":
                _settingsPageActive = "class=\"active\"";
                break;
            case "supportPage":
                _supportPageActive = "class=\"active\"";
                break;
            default:
                break;
        }
        hypGetHelp.NavigateUrl = "/V1/VictimAccount.aspx?organizationId=" + organizationId;
        //hypPeople.Visible = false;

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var organization = (from o in dc.Organizations
                            where o.OrganizationId == new Guid(organizationId)
                            select o).SingleOrDefault();


		hypActivity.NavigateUrl = "/Team/" + organization.URLFriendlyName;

		//Set the teams donation link.
		if (!String.IsNullOrEmpty(organization.DonationURL))
		{
			//The team has it's own donation link setup.
			donationLink = organization.DonationURL;
		}
		else
		{
			//If user has setup their donation dashboard then show that link.

			var donationCamapaign = (from d in dc.DonationCampaigns
									 where d.OrganizationId == new Guid(organizationId)
									 select d).FirstOrDefault();
			if(donationCamapaign != null)
			{
				donationLink = "/V1/NonProfit/Donation.aspx?organizationId=" + organizationId;
			}
			else
			{
				//If no donation campaign hide the donate button.
				donatenow.Visible = false;
			}
		}

        bool hidTeamList = organization.HideTeamList != null ? (bool)organization.HideTeamList : false;
        bool respondToTickets = false;
        hypGetHelp.Visible = false;

        respondToTickets = organization.RespondToTickets != null ? (bool)organization.RespondToTickets : false;
        if (respondToTickets)
        {
            hypGetHelp.Visible = true;
            //hypTickets.Visible = true;
        }





        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            hypJoinTeam.NavigateUrl = "/V1/Profile/EditNonProfits.aspx?organizationId=" + organizationId;

            Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            Guid teamAdministratorRoleId = new Guid("E48E49D7-392B-4C3B-A53A-62B7B2537BBF");

			//Is user the owner?
            var isOrganizationOwner = (from uo in dc.UserOrganizations
                                         join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
                                         where o.OwnerId == userId && uo.UserId == userId
										 && uo.OrganizationId == new Guid(organizationId)
                                         select o).Take(1).SingleOrDefault();

			bool isOwner = false;
			bool userTeamAdministrator = false;

			if (isOrganizationOwner != null)
			{
				if (isOrganizationOwner.OwnerId == userId)
				{
					isOwner = true;
					isUserOnTeam = true;

					hypJoinTeam.Enabled = false;
					hypJoinTeam.Text = "You Own This Team";
					hypJoinTeam.CssClass = "btn btn-primary btn-block";

					hypTeamManagement.Visible = true;
					hypTeamManagement.Attributes["data-toggle"] = "tooltip";
					hypTeamManagement.Attributes["title"] = "View this team's TeamManagement";
				}
			}
            else
            {
				//Is user on this team?
				var userCheck = (from uo in dc.UserOrganizations
								 where uo.UserId == userId && (uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
								 && uo.OrganizationId == new Guid(organizationId)
								 select uo).Take(1).SingleOrDefault();

				if (userCheck != null)
				{
					//User is on this team.
					isUserOnTeam = true;

					hypJoinTeam.Enabled = false;
					hypJoinTeam.Text = "You're On This Team";
					hypJoinTeam.CssClass = "btn btn-default btn-block";


					//Check the roleid
					userTeamAdministrator = dc.aspnet_UsersInRoles
					.Any(ur => ur.UserId == userId && ur.RoleId == teamAdministratorRoleId);

					//Check the UserOrganization table assignement too.
					bool isTeamAdministratorExists = dc.UserOrganizations
					.Any(uo => uo.OrganizationId == new Guid(organizationId) 
					&& uo.UserId == userId 
					&& uo.IsTeamAdministrator == true);

					if (isTeamAdministratorExists == true || userTeamAdministrator == true)
					{
						//User is an team admin.
						hypTeamManagement.Visible = true;
						hypTeamManagement.Attributes["data-toggle"] = "tooltip";
						hypTeamManagement.Attributes["title"] = "View this team's TeamManagement";
						//hypRequest.Visible = true;
						//hypRequest.Attributes["data-toggle"] = "tooltip";
						//hypRequest.Attributes["title"] = "View this team's Request";
					}

				}
            }

            if (HttpContext.Current.User.IsInRole("Administrator") || isOwner || isUserOnTeam)
            {
                //Show team list details to admin, owner and team members.
                //hypPeople.Visible = true;
                if (HttpContext.Current.User.IsInRole("Administrator") || isOwner || HttpContext.Current.User.IsInRole("Team Administrator"))
                {                
                    ulAdmin.Visible = true;
                    divDeployment.Visible = true;
                }
            }
        }
        else
        {
            //Not signed in, send to registration page.
            hypJoinTeam.NavigateUrl = "/V1/Register.aspx?organizationId=" + organizationId;
        }
    }
    protected void btnChangePageStatus_Click(object sender, EventArgs e)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        bool updateActiveStatus = true;

        var organization = (from o in dc.Organizations
                            where o.OrganizationId == new Guid(organizationId)
                            select o).SingleOrDefault();

        //if (organization != null)
        //{

        //    btnDeactivatePage.Text = "<i class='fa fa-ban text-danger'></i> De-activate This Team";

        //    if (organization.IsActive == true)
        //    {
        //        updateActiveStatus = false;

        //        btnDeactivatePage.Text = "<i class='fa fa-ban text-danger'></i> Re-activate This Team";
        //    }

        //    organization.IsActive = updateActiveStatus;
        //    dc.SubmitChanges();
        //}
    }
    public string TeamName
    {
        get { return _teamName; }
        set { _teamName = value; }
    }
    public string PageName
    {
        get { return _pageName; }
        set { _pageName = value; }
    }
    public string OrganizationId
    {
        get { return organizationId; }
        set { organizationId = value; }
    }
}