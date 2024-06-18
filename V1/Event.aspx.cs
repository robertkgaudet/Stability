using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Data.Linq.SqlClient;
using System.Xml.Linq;
using System.Security.Policy;
using System.IdentityModel.Metadata;
using Stability;

public partial class V1_Event : BaseOrganizationWebForm
{
    public string headerColor = string.Empty;
    public string color = string.Empty;
    public string icon = string.Empty;
    public string eventName = HttpContext.Current.Request.QueryString["eventName"];
    public string _eventId = HttpContext.Current.Request.QueryString["eventId"];
    public string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
    public string overallProgressSliderId = ConfigurationManager.AppSettings["overallProgressSliderId"].ToString();
	public string mapDomain = ConfigurationManager.AppSettings["mapDomain"].ToString(); 
	public string mapApiKey = ConfigurationManager.AppSettings["mapApiKey"].ToString(); 
	public string registerNonProfit = "/V1/Profile/NonProfitNew.aspx";
	public string btnRebuild = string.Empty;
	public string btnrescue = string.Empty;
	public string btnrequestsupplies = string.Empty;
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	string profileImagePlaceHolderAndPath = System.Configuration.ConfigurationManager.AppSettings["profileImagePlaceHolderAndPath"].ToString();

	public string mapURL = string.Empty;
	public Guid eventId = Guid.NewGuid();
    public string btnSurvivorPage = string.Empty;
    public string btnBusinessPage = string.Empty;
    public string btnNonProfitPage = string.Empty;
    public string btnHelperPage = string.Empty;
	public string btnCaseManagementPage = string.Empty;

	public string latitude = string.Empty;
	public string longitude = string.Empty;
	public string zoom = string.Empty;

	public string _todayVolunteerCount = "0";
	public string _todayVolunteerHours = "0";
	public string _todayVolunteerValue = "0";

	public string _totalVolunteerCount = "0";
	public string _totalVolunteerHours = "0";
	public string _totalVolunteerValue = "0";
	public string _volunteerHourlyRate = "";
	public int teamCounter;
	public string disasterDropDown	= string.Empty;

	public string host = HttpContext.Current.Request.Url.Host; //HttpContext.Current.Request.Url.Host;

	protected void LoadImpactMetrics(Guid organizationEventId, decimal volunteerHourlyRate)
	{
		//count the number of volunteers today and total.

		//TODAY volunteer count.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var totalVolunteersToday = (from uoe in dc.UserOrganizationEvents
									join t in dc.Timesheets on uoe.UserId equals t.UserId
									where uoe.OrganizationEventId == organizationEventId
									&& t.TimeIn.Date == DateTime.Today.Date
									select new { uoe.UserId }).Distinct().Count();

		_todayVolunteerCount = totalVolunteersToday.ToString();

		//TODAYS volunteer hours
		var todaysVolunteerHours = dc.GetTotalHoursByCauseByDay(organizationEventId).First().Column1;
		if (todaysVolunteerHours != null)
		{
			_todayVolunteerHours = todaysVolunteerHours;
			_todayVolunteerValue = (Convert.ToInt32(todaysVolunteerHours) * volunteerHourlyRate).ToString("C");
		}

		//TOTAL volunteer hours
		var totalVolunteerHours = dc.GetTotalHoursByCause(organizationEventId).First().Column1;
		if (totalVolunteerHours != null)
		{
			_totalVolunteerHours = totalVolunteerHours;
			_totalVolunteerValue = (Convert.ToInt32(totalVolunteerHours) * volunteerHourlyRate).ToString("C");
		}


		var totalVolunteers = (from uoe in dc.UserOrganizationEvents
							   join t in dc.Timesheets on uoe.UserId equals t.UserId
							   where uoe.OrganizationEventId == organizationEventId
							   select new { uoe.UserId }).Distinct().Count();

		_totalVolunteerCount = totalVolunteers.ToString();
	}

	public void LoadDisasters()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disasters = from d in dc.Events
						orderby d.BeginDate descending
						where d.IsActive == true
						select new { d };

		int idNumber = 0;
		foreach (var disaster in disasters)
		{
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			string disasterUrlFriendlyName = String.IsNullOrEmpty(disaster.d.URLFriendlyName) ? "HurricaneIan" : disaster.d.URLFriendlyName;
			disasterDropDown = disasterDropDown + "<li name=\"" + disasterUrlFriendlyName + "\"><a href=\"#\">" + disaster.d.Name + " - " + disasterDate + "</a></li>" + Environment.NewLine;
			idNumber = idNumber + 1;
		}
	}
	protected void Page_Load(object sender, EventArgs e)
    {
        if (host == "localhost")
        {
            host = "http://" + host + ":" + HttpContext.Current.Request.Url.Port;
        }
        else
        {
            host = "https://" + host;
        }
        string disasterPageTitle = System.Configuration.ConfigurationManager.AppSettings["DisasterPageTitle"];
		if (!User.Identity.IsAuthenticated)
        {
            btnRebuildPost.Text = "Registration/Sign In is required to post.";
            btnRebuildPost.Enabled = false;
            txtPost.Attributes.Add("placeholder","Please Register/Sign In to post.");
            txtPost.Enabled = false;
        }

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		LoadDisasters();

		if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["eventId"]))
        {
			//Sent EventId on the Query String.
            eventId = new Guid(HttpContext.Current.Request.QueryString["eventId"]);
        }
        else if(!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["eventName"]))
        {
            //Get the eventId from the eventname
            var disaster = (from ev in dc.Events
                            where ev.URLFriendlyName == HttpContext.Current.Request.QueryString["eventName"]
                            select new { ev.EventId }).SingleOrDefault();

            eventId = disaster.EventId;
        }


		if (!IsPostBack)
        {
			//LoadPosts();
			ucDeploymentListCard.EventId = eventId;
			uc1EventHeader.CauseCount = Convert.ToString(Session["deploymentCount"]);

			LoadTeams();

			hidEventId.Value = eventId.ToString();

			if (!IsPostBack)
			{
				hypVolunteer.Font.Underline = true;
				hypAddASurvivor.Font.Underline = true;
				hypAddHome.Font.Underline = true;
				hypAddNonProfit.Font.Underline = true;
				hypUpdateProgress.Font.Underline = true;
				hypUpdateRebuildProgress.Font.Underline = true;
				
				ucLinks.eventId = eventId;
				ucDisasterSurvivorStoriesByDisaster.eventId = eventId;
				ucImpactedCommunities.eventId = eventId;

				var disaster = (from ev in dc.Events
								where ev.EventId == eventId
								select ev).SingleOrDefault();


				if (disaster != null)
				{
					eventId = disaster.EventId;

					if (HttpContext.Current.User.Identity.IsAuthenticated)
					{
						var profile = (from p in dc.Profiles
									   where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
									   select p).SingleOrDefault();

						if (profile.DefaultEventId == eventId)
						{
							//Show message that this is the users default event id.
							lblDefaultEventMessage.Text = disaster.Name + " is your default portal.";
							btnSetDefaultDisaster.Visible = false;
						}
						else
						{
							lblDefaultEventMessage.Text = "Set this as your default disaster to load this page when you log in.";
						}

						if (User.IsInRole("CaseManager"))
						{
							tab7.Visible = true;
							liTab7.Visible = true;
							hypCaseManagers.NavigateUrl = "/CaseManagement/AddCase.aspx";
							hypViewCases.NavigateUrl = "/CaseManagement/" + disaster.URLFriendlyName;
						}

						//if (disaster.Icon != null)
						//{
						//	icon = disaster.Icon.Replace("COLOR", "btn-" + disaster.Color + " btn-outline");
						//}
					}
				}

				if (!HttpContext.Current.User.Identity.IsAuthenticated)
				{
					divRegister.Visible = true;
					divChooseDefaultDisaster.Visible = false;
				}
				else
				{
					divRegister.Visible = false;
					divChooseDefaultDisaster.Visible = true;
				}

				if (!string.IsNullOrEmpty(disaster.Latitude))
				{
					latitude = disaster.Latitude;
					longitude = disaster.Longitude;
					zoom = disaster.Zoom.ToString();
				}

				btnrequestsupplies = disaster.NogginRequestSuppliesLink;
				btnrescue = disaster.NogginRequestRescueLink;

				lblQuickLinks.Text = disaster.Name + " Quick Links";

				hidEventName.Value = disaster.URLFriendlyName;

				var userOrganizationOwner = (from o in dc.Organizations
									   where o.OwnerId == userId && o.IsActive == true
									   select o).SingleOrDefault();

				Guid organizationIdForOwner = Guid.Empty;
				litAddTeamMessage.Text = "If you would like to add a new deployment, first add your team.";
				hypAddTeam.Visible = true;
				if (userOrganizationOwner != null)
				{
					hypAddTeam.Visible = false;
					//User owns a nonprofit, let them create a campaign for it.
					litAddTeamMessage.Text = "By creating a deployment, you help the entire community understand your efforts and prevent duplication of work, thereby enhancing collaboration.";
					organizationIdForOwner = userOrganizationOwner.OrganizationId;
					//Show link if user does not already have a nonprofit they started.
				}

				hypAddNewNonProfit.Visible = true;
				hypAddNewNonProfit.NavigateUrl = "/V1/Administration/NonProfitNew.aspx";

				//Is this user associated with a nonprofit?
				var userOrganization = (from uo in dc.UserOrganizations
									   where uo.UserId == userId
									   select uo).Take(1).SingleOrDefault();

				if(userOrganization != null)
				{ 
					if(((organizationIdForOwner != Guid.Empty) && (userOrganization.OrganizationId == organizationIdForOwner)) || User.IsInRole("CauseSuperAdministrator"))
					{
						//Allow any CauseSuperAdministrator to add a campaign.
						//Allow a person who create a nonprofit to add a campaign to it.
						hypAddNewCampaign.Visible = true;
						hypAddNewCampaign.NavigateUrl = "/V1/NonProfitAdministration/RespondToEvent.aspx?eventId=" + eventId + "&organizationId=" + userOrganization.OrganizationId;
					}
				}
				//Show link if user has a nonprofit.

				btnSurvivorPage = "/" + disaster.URLFriendlyName + "/Survivor";
				btnBusinessPage = "/" + disaster.URLFriendlyName + "/Business";
				btnHelperPage = "/" + disaster.URLFriendlyName + "/Helper";
				btnNonProfitPage = "/" + disaster.URLFriendlyName + "/NonProfit";
				btnCaseManagementPage = "/CaseManagement";

				hypAddNonProfit.NavigateUrl = "/" + disaster.URLFriendlyName + "/NonProfit";
				hypAddASurvivor.NavigateUrl = "/S1/Profile/AddNewSurvivor.aspx?eventId=" + eventId;
				hypAddHome.NavigateUrl = "/V1/Profile/AddNewRebuild.aspx?eventId=" + eventId;
				hypVolunteer.NavigateUrl = "/" + disaster.URLFriendlyName + "/Helper";

				mapURL = disaster.MAPUrl;

				string pageDescription = disaster.Description + "<br>Survivors, helpers, volunteers, non-profits and businesses start here to begin to restore and rebuild.";

				string pageTitle = disaster.Name + disasterPageTitle;

				this.Master.PageTitle = pageTitle;
				this.Master.PageDescription = pageDescription;
				this.Master.FbDescription = pageDescription;
				this.Master.FbSite_name = pageTitle;

				uc1EventHeader.PageTitle = disasterPageTitle;
				uc1EventHeader.EventName = disaster.Name;
				uc1EventHeader.PageDescription = pageDescription;

				if (disaster.Icon != null)
				{
					icon = disaster.Icon.Replace("COLOR", "btn-" + disaster.Color + " btn-outline");
				}

				if (disaster.Color != null)
				{
					color = disaster.Color;
					headerColor = CrowdRelief.Tools.GetColor(disaster.Color);
				}

				if (User.IsInRole("Administrator"))
				{
					divAdmin.Visible = true;
					LoadLinkCategories();
					//divRequestAdmin.Visible = false;
					tab6.Visible = true;
					liAdminTab.Visible = true;
					hypEditDisaster.NavigateUrl = "/V1/Administration/NewDisaster.aspx?eventId=" + eventId;
					hypEditCounties.NavigateUrl = "/V1/Administration/DisasterCounty.aspx?eventId=" + eventId;
				}

				hypNewCategory.NavigateUrl = "/V1/Profile/AddArticle.aspx?eventId=" + eventId;

				var homeCount = from c in dc.Rebuilds
								where c.EventId == eventId
								select c;

				litHomesAdded.Text = homeCount.Count().ToString();
				homeCount = homeCount.Where(c => c.CreatedBy == userId);

				var volunteerCount = from u in dc.aspnet_Users
									 join uir in dc.aspnet_UsersInRoles on u.UserId equals uir.UserId
									 join r in dc.aspnet_Roles on uir.RoleId equals r.RoleId
									 join uev in dc.UserEvents on u.UserId equals uev.UserId
									 where r.LoweredRoleName == "volunteer" || r.LoweredRoleName == "helper"
									 && uev.EventId == eventId
									 select u;

				litVolunteerCount.Text = volunteerCount.Count().ToString();


				var survivorCount = from u in dc.aspnet_Users
									join uir in dc.aspnet_UsersInRoles on u.UserId equals uir.UserId
									join r in dc.aspnet_Roles on uir.RoleId equals r.RoleId
									join uev in dc.UserEvents on u.UserId equals uev.UserId
									where r.LoweredRoleName == "survivor"
									&& uev.EventId == eventId
									select u;

				litSurvivorCount.Text = survivorCount.Count().ToString();

				var nonProfitCount = from pe in dc.OrganizationEvents
									 join ev in dc.Events on pe.EventId equals ev.EventId
									 where ev.EventId == eventId
									 select pe;

				litNonprofitCount.Text = nonProfitCount.Count().ToString();

				litVolunteers.Text = CalculateVolunteersNeeded(Guid.Empty, 0, false, eventId);

				string rebuildTickLabel = string.Empty;
				string overallTickLabel = string.Empty;

				litRebuildProgress.Text = GetPercent(true, eventId, Guid.Empty, new Guid(rebuildProgressSliderId), out rebuildTickLabel).ToString() + "%";
				litOverallProgress.Text = GetPercent(true, eventId, Guid.Empty, new Guid(overallProgressSliderId), out overallTickLabel).ToString() + "%";

				//CALCULATE TIME AND SHOW IT ON THIS PAGE SOMEWHERE!!!!!
				var userTime = from p in dc.Profiles
							   join ev in dc.UserEvents on p.UserId equals ev.UserId
							   join t in dc.Timesheets on p.UserId equals t.UserId
							   where ev.EventId == eventId && t.TimeIn != null
							   orderby t.TimeIn descending, p.Lastname
							   select new { p.Firstname, p.Lastname, p.UserId, p.ZelloName, t.TimeIn, t.TimeOut };
			}
        }
    }

	protected string GetDeployments()
	{
		string deploymentPanel = string.Empty;
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var deployments = from org in dc.Organizations
							join oe in dc.OrganizationEvents on org.OrganizationId equals oe.OrganizationId
							join s in dc.USStates on oe.StagingStateId equals s.StatesId
							where oe.EventId == eventId && org.IsActive == true
							orderby oe.IsActive descending, org.Name ascending
							select new { org.IsVoadMember, org.Logo, oe.VolunteerHourlyRate, org.URLFriendlyName, oe.IsActive, oe.OrganizationEventId, org.OrganizationId, oe.HelpURL, oe.VolunteerURL, org.DonationURL, oe.CampaignName, oe.VolunteerInstructions, oe.MissionPurpose, oe.URLFriendlyCampaignName, oe.StagingCity, StagingState = s.Name, org.Name, org.Description };

		uc1EventHeader.CauseCount = deployments.Count().ToString();

		foreach (var deployment in deployments)
		{
			//string eventDate = String.Format("{0:Y}", deployment.date);

			string rebuildTickLabel = string.Empty;
			//string volunteersNeeded = CalculateVolunteersNeeded(Guid.Empty, 0, false, disaster.EventId);


			string logo = string.Empty;

			if (!String.IsNullOrEmpty(deployment.Logo))	
			{
				logo = "/Impactoid/Images/Logos/" + deployment.Logo;
			}
			else
			{
				//Use placeholder image.imgLogo.Visible = true;
				logo = "/V1/Images/Logo-Placeholder.png";
			}

			string logoDiv = "<div class=\"m-b-sm m-l-sm pull-right\" style=\"background-color:white; display:inline-block; padding:2px; border:solid 1px #ccc;\">" +
								"<img id=\"imgLogo\" src=\"" + logo + "\" width=\"60px\" />" +
							"</div>";

			deploymentPanel += "<div class=\"grid-item m-b-sm\" onclick=\"window.location.href='/Cause/" + deployment.URLFriendlyCampaignName + "';\">" + Environment.NewLine + Environment.NewLine +
									"<div class=\"hpanel hviolet\">" + Environment.NewLine +
										"<div class=\"panel-body deploymentPanel\">" + Environment.NewLine +
											"<div class=\"row\" style=\"padding:0px 10px;\">" + Environment.NewLine +
												"<div class=\"col\">" + Environment.NewLine +
													logoDiv + Environment.NewLine +
												"</div>" + Environment.NewLine +
												"<div class=\"col\">" + Environment.NewLine +
													"<p class=\"m-b-xs text-" + color + "\"><small>" + deployment.Name + "</small></p>" + Environment.NewLine +
												"</div>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
											"<div class=\"row\" style=\"padding:0px 10px;\">" + Environment.NewLine +
												"<div class=\"col\">" + Environment.NewLine +
													"<div><h5 class=\"font-bold\">" + deployment.CampaignName + "</h5></div>" + Environment.NewLine +
												"</div>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
										"</div>" + Environment.NewLine +
									"</div>" + Environment.NewLine +
									"<div class=\"panel-footer\">" + Environment.NewLine +
										"<small>" + "DATES HERE" + "</small>" + Environment.NewLine +
									"</div>" + Environment.NewLine +
							"</div>" + Environment.NewLine + Environment.NewLine;

		}

		return deploymentPanel;
	}

	protected void btnRebuildPost_Click(object sender, EventArgs e)
    {
        eventId = new Guid(hidEventId.Value);

        string post = txtPost.Text;
        if(!string.IsNullOrEmpty(post))
        {
            UpdateEventPost(post, userId, eventId);
            //LoadPosts();
        }
        Response.Redirect("~/" + hidEventName.Value);
    }
    
    public void LoadPosts()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var posts = from ep in dc.EventPosts
                    join p in dc.Profiles on ep.UserId equals p.UserId
                    where ep.EventId == eventId
                    orderby ep.CreatedOn descending
                    select new { p.ProfileId, ep.Post, p.UserId, ep.EventPostId, ep.CreatedOn, fullname = "<b>" + p.Firstname + " " + p.Lastname + "</b> posted an update.", ProfilePhoto = (p.Photo == null ? "Avatar.png" : p.Photo) };
        if(posts.Count() > 0)
        { 
        rptPosts.DataSource = posts;
        rptPosts.DataBind();
        }
    }

	protected void LoadTeams()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organizationEventCount = (from oe in dc.OrganizationEvents
									  join o in dc.Organizations on oe.OrganizationId equals o.OrganizationId
									  where oe.EventId == eventId && o.IsActive == true
									  select new { o.OrganizationId, o.Name, o.Description }).Distinct();

		rptTeams.DataSource = organizationEventCount;
		rptTeams.DataBind();

		uc1EventHeader.TeamCount = organizationEventCount.Count().ToString();
	}

	protected void rptNonProfitOrganizations_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		teamCounter += 1;
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem				= (RepeaterItem)e.Item;
			HyperLink hypCampaignName			= (HyperLink)e.Item.FindControl("hypCampaignName");
			Label lblOrganizationDescription	= (Label)e.Item.FindControl("lblOrganizationDescription");

			HyperLink hypDonationURL			= (HyperLink)e.Item.FindControl("hypDonationURL");
			HyperLink hypVolunteerURL			= (HyperLink)e.Item.FindControl("hypVolunteerURL");
			HyperLink hypGetHelpURL				= (HyperLink)e.Item.FindControl("hypGetHelpURL");
			Button hypCauseIsNotActive			= (Button)e.Item.FindControl("hypCauseIsNotActive"); 
			Label lblisVoadMember				= (Label)e.Item.FindControl("lblisVoadMember");
			HyperLink hypOrganizationName		= (HyperLink)e.Item.FindControl("hypOrganizationName");
			Label lblMissionPurpose				= (Label)e.Item.FindControl("lblMissionPurpose");
			Label lblURLFriendlyCampaignName	= (Label)e.Item.FindControl("lblURLFriendlyCampaignName");
			Label lblStagingCity				= (Label)e.Item.FindControl("lblStagingCity");
			Label lblStagingState				= (Label)e.Item.FindControl("lblStagingState");
			Label lblVolunteerInstructions		= (Label)e.Item.FindControl("lblVolunteerInstructions");
			Literal litBrace1					= (Literal)e.Item.FindControl("litBrace1"); 
			Literal litVolunteerValue = (Literal)e.Item.FindControl("litVolunteerValue");
			Literal litVolunteerHours = (Literal)e.Item.FindControl("litVolunteerHours"); 
			Button btnActiveVolunteer			= (Button)e.Item.FindControl("btnActiveVolunteer");
			Literal litDivQH = (Literal)e.Item.FindControl("litDivQH");
			Literal litDivQP = (Literal)e.Item.FindControl("litDivQP");
			Literal litCauseName = (Literal)e.Item.FindControl("litCauseName");

			Guid organizationId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationId");
			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId"); 
			string organizationName = (string)DataBinder.Eval(dataItem.DataItem, "Name"); 
			decimal volunteerRate = DataBinder.Eval(dataItem.DataItem, "VolunteerHourlyRate") != null ? Convert.ToDecimal(DataBinder.Eval(dataItem.DataItem, "VolunteerHourlyRate")) : 0;
			string organizationDescription = (string)DataBinder.Eval(dataItem.DataItem, "Description");
			bool isVoadMember = (bool)DataBinder.Eval(dataItem.DataItem, "isVoadMember");
			bool isActive = (bool)DataBinder.Eval(dataItem.DataItem, "isActive");
			string donationURL = (string)DataBinder.Eval(dataItem.DataItem, "DonationURL");
			string volunteerURL = (string)DataBinder.Eval(dataItem.DataItem, "VolunteerURL");
			string getHelpURL = (string)DataBinder.Eval(dataItem.DataItem, "HelpURL");
			string volunteerInstructions = (string)DataBinder.Eval(dataItem.DataItem, "VolunteerInstructions");
			string campaignName = (string)DataBinder.Eval(dataItem.DataItem, "CampaignName");
			string missionPurpose = (string)DataBinder.Eval(dataItem.DataItem, "MissionPurpose");
			string URLFriendlyCampaignName = (string)DataBinder.Eval(dataItem.DataItem, "URLFriendlyCampaignName");
			string stagingCity = (string)DataBinder.Eval(dataItem.DataItem, "StagingCity");
			string stagingState = (string)DataBinder.Eval(dataItem.DataItem, "StagingState");
			string urlFriendlyName = (string)DataBinder.Eval(dataItem.DataItem, "URLFriendlyName");

			litDivQH.Text = "<a data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#q" + teamCounter + "\" aria-expanded=\"true\">";
			litDivQP.Text = "<div id=\"q" + teamCounter + "\" runat=\"server\" class=\"panel-collapse collapse\">";
			litCauseName.Text = campaignName;

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			_volunteerHourlyRate = volunteerRate.ToString("C");
			//LoadImpactMetrics(organizationEventId, volunteerRate);

			//var totalVolunteersToday = (from uoe in dc.UserOrganizationEvents
			//							join t in dc.Timesheets on uoe.UserId equals t.UserId
			//							where uoe.OrganizationEventId == organizationEventId
			//							&& t.TimeIn.Date == DateTime.Today.Date
			//							select new { uoe.UserId }).Distinct().Count();

			//_todayVolunteerCount = totalVolunteersToday.ToString();

			//TODAYS volunteer hours
			//var todaysVolunteerHours = dc.GetTotalHoursByCauseByDay(organizationEventId).First().Column1;
			//if (todaysVolunteerHours != null)
			//{
			//	_todayVolunteerHours = todaysVolunteerHours;
			//	_todayVolunteerValue = (Convert.ToInt32(todaysVolunteerHours) * volunteerRate).ToString("C");
			//}

			//TOTAL volunteer hours
			var totalVolunteerHours = dc.GetTotalHoursByCause(organizationEventId).First().Column1;
			if (totalVolunteerHours != null)
			{
				_totalVolunteerHours = totalVolunteerHours;
				_totalVolunteerValue = (Convert.ToInt32(totalVolunteerHours) * volunteerRate).ToString("C");
				litVolunteerValue.Text = _totalVolunteerValue;
				litVolunteerHours.Text = _totalVolunteerHours;
			}

			//var totalVolunteers = (from uoe in dc.UserOrganizationEvents
			//					   join t in dc.Timesheets on uoe.UserId equals t.UserId
			//					   where uoe.OrganizationEventId == organizationEventId
			//					   select new { uoe.UserId }).Distinct().Count();
			//_totalVolunteerCount = totalVolunteers.ToString();\
			//_totalVolunteerCount = totalVolunteers.ToString();\



			string urlRoute = "/i/" + urlFriendlyName;
			string idRoute = "/Impactoid/CommunityPage.aspx?organizationId=" + organizationId.ToString();
			string impactoidURL = !string.IsNullOrEmpty(urlFriendlyName) ? urlRoute : idRoute;

			hypOrganizationName.Text = organizationName;
			hypOrganizationName.NavigateUrl = impactoidURL;
			hypCampaignName.NavigateUrl = "~/Cause/" + URLFriendlyCampaignName;
			hypCampaignName.Text = "View Cause Details";

			if (!String.IsNullOrEmpty(donationURL))
			{
				hypDonationURL.Visible = isActive;
				hypDonationURL.Text = "Donate To This Deployment";
				hypDonationURL.NavigateUrl = donationURL;
				hypDonationURL.Enabled = isActive;
			}

			if(!isActive)
			{
				hypCauseIsNotActive.Visible = true;
				hypCauseIsNotActive.Text = "This deployment is no longer active.";
			}
			else
			{
				if(!String.IsNullOrEmpty(volunteerURL))
				{
					//Nonprofit has provided their own Volunteer Link.
					hypVolunteerURL.Visible	= isActive;
					hypVolunteerURL.Text = "Volunteer For This Deployment";
					hypVolunteerURL.NavigateUrl = volunteerURL;
					hypVolunteerURL.Enabled = isActive;
				}
				else
				{

					//Nonprofit has NOT provided their own Volunteer Link.
					if (User.Identity.IsAuthenticated)
					{
						//User is logged in.

						//If the user is logged in and has not selected this campaign to volunteer for already then let them choose this campaign from here.
						var userOrganizationEvents = from uoe in dc.UserOrganizationEvents
											   where uoe.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
											   && uoe.OrganizationEventId == organizationEventId
											   && uoe.DeactivatedOn == null
											   orderby uoe.CreatedOn descending
											   select uoe;

						if (userOrganizationEvents.Count() == 0)
						{
							//Tell the user they can choose this nonprofit event to volunteer with.
							hypVolunteerURL.Visible = true;
							hypVolunteerURL.Text = "Volunteer For This Deployment";
							hypVolunteerURL.NavigateUrl = "/V1/Profile/EditNonProfitCauses.aspx?userOrganizationEvent=true&organizationEventId=" + organizationEventId;
						}
						else
						{
							//User is already volunteering for this nonprofit, show that message and disable the volunteer button.
							hypVolunteerURL.Visible = false;
							btnActiveVolunteer.Text = "You are volunteering for this cause.";
							btnActiveVolunteer.Visible = isActive;
							btnActiveVolunteer.Enabled = false;
						}
					}
					else
					{
						hypVolunteerURL.Visible = true;
						hypVolunteerURL.Text = "Volunteer For This Deployment";
						hypVolunteerURL.NavigateUrl = "/Register/" + organizationId;
					}
				}

				if (!String.IsNullOrEmpty(getHelpURL))
				{
					hypGetHelpURL.Visible = true;
					hypGetHelpURL.Text = "Get Help";
					hypGetHelpURL.NavigateUrl = getHelpURL;
				}
			}

			lblisVoadMember.Text = isVoadMember ? "VOAD Member" : "";

			lblMissionPurpose.Text = missionPurpose;
			lblStagingCity.Text = stagingCity;
			lblStagingState.Text = stagingState;
			lblVolunteerInstructions.Text = volunteerInstructions;
		}
	}


	protected void LoadLinkCategories()
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var categories = from c in dc.Categories
                            orderby c.ParentCategory, c.Category1
                            select new {c.Order, c.ParentCategory, category = c.ParentCategory + " - " + c.Category1, c.CategoryId };

        ddlCateogry.DataSource = categories;
        ddlCateogry.DataBind();
    }

    protected void rptPosts_OnItemDataBound(Object Sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        { 
            RepeaterItem dataItem = (RepeaterItem)e.Item;
            HyperLink hypFullname = (HyperLink)e.Item.FindControl("hypFullname");
            Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "userId");
            string fullname = (string)DataBinder.Eval(dataItem.DataItem, "fullname");
            hypFullname.Text = fullname;
            hypFullname.NavigateUrl = "~/V1/Profile/Profile.aspx?userId=" + userId;

            if(User.IsInRole("Administrator"))
            {
                Guid eventPostId = (Guid)DataBinder.Eval(dataItem.DataItem, "eventPostId");
                HtmlGenericControl divDelete = (HtmlGenericControl)e.Item.FindControl("divDelete");
                LinkButton lbDelete = (LinkButton)e.Item.FindControl("lbDelete");

                divDelete.Visible = true;
                lbDelete.CommandArgument = eventPostId.ToString();
            }
        }
    }
    
    protected void lbDelete_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)(sender);
        string eventPostId = btn.CommandArgument;
        
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var eventPost = (from ep in dc.EventPosts
                          where ep.EventPostId == new Guid(eventPostId)
                          select ep).SingleOrDefault();

        dc.EventPosts.DeleteOnSubmit(eventPost);
        dc.SubmitChanges();
        Response.Redirect("~/" + HttpContext.Current.Request.QueryString["eventName"]);
    }

    protected void btnAddLink_Click(object sender, EventArgs e)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
        Link link = new Link();

        link.CategoryId = new Guid(ddlCateogry.SelectedValue);
        link.Title = txtTitle.Value;
        link.Description = txtDescription.Value;
        link.LinkId = Guid.NewGuid();
        link.URL = txtUrl.Value;
        link.EventId = eventId;
        link.CreatedBy = userId;
        link.CreatedOn = DateTime.Now;

        dc.Links.InsertOnSubmit(link);
        dc.SubmitChanges();
        divAlertMessage.Visible = true;
        litMessage.Text = "New link added.";

        Response.Redirect("~/" + HttpContext.Current.Request.QueryString["eventName"]);
    }

	protected void rptTeams_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		teamCounter += 1;
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			HyperLink hypViewTeam = (HyperLink)e.Item.FindControl("hypViewTeam");
			HyperLink hypViewActivities = (HyperLink)e.Item.FindControl("hypViewActivities"); 
			Literal litTeamDescription = (Literal)e.Item.FindControl("litTeamDescription");
			Literal litTeamName = (Literal)e.Item.FindControl("litTeamName");
			Literal litDivQH = (Literal)e.Item.FindControl("litDivQH");
			Literal litDivQP = (Literal)e.Item.FindControl("litDivQP");

			litDivQH.Text = "<a data-toggle=\"collapse\" data-parent=\"#accordion\" href=\"#q" + teamCounter + "\" aria-expanded=\"true\">";
			litDivQP.Text = "<div id=\"q" + teamCounter + "\" runat=\"server\" class=\"panel-collapse collapse\">";

			Guid organizationId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationId");
			string name = (string)DataBinder.Eval(dataItem.DataItem, "Name");
			string description = (string)DataBinder.Eval(dataItem.DataItem, "Description");

			litTeamName.Text = name;
			litTeamDescription.Text = description;
			hypViewTeam.Text = "Team Profile Page";
			hypViewTeam.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + organizationId;

			hypViewActivities.Text = "Open Team Activity";
			hypViewActivities.NavigateUrl = "/V1/NonProfit/ActivityDashboard.aspx?organizationId=" + organizationId;
			
		}
	}
}