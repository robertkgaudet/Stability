using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_ActivityDashboard : BaseWebForm
{
	public string _logo;
	public string _teamName;
	public string _teamSquareLogo;
	public string _teamDescription;
	public string _pageName;
	public string _organizationId;
	public string _nonProfitDropDown;
	public string _coverImage;
	public string organizationId = string.Empty;
	public string availableDates = string.Empty;
	public string teamCounts = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamFooter.PageName = "activityPage";
		ucTeamHeader.PageName = "Impact Dashboard";

		#region HEADER PROPERTIES
		////////////////////////
		//BEGIN HEADER PROPERTIES
		////////////////////////

		organizationId = Request.QueryString["organizationId"];
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		_coverImage = causePhotoFolder + "businesscoverimage.png";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select new { o.Name, o.LogoSquare, o.Description, o.Logo, o.CoverImage, o.URLFriendlyName }).SingleOrDefault();

		string squareLogo = string.Empty;
		if (organization != null)
		{
			if (organization.CoverImage != null)
			{
			//	_coverImage = causePhotoFolder + organization.CoverImage;
			}

			ucTeamHeader.CoverImage = _coverImage;
			ucTeamHeader.TeamDescription = organization.Description;
			ucTeamHeader._teamTitle = organization.Name;
			ucTeamHeader.URLFriendlyPageName = organization.URLFriendlyName;

			if (!String.IsNullOrEmpty(organization.LogoSquare))
			{
				squareLogo = "/Impactoid/Images/Logos/" + organization.LogoSquare;
			}
			else
			{
				squareLogo = "/V1/Images/Logo-Placeholder.png";
			}

			Master.PageTitle = organization.Name + " Programs on Stability";
			Master.PageDescription = organization.Description;
			Master.FbDescription = organization.Description;
			Master.FbImage = _coverImage;
			Master.FbSite_name = organization.Name + " Programs on Stability";
		}

		ucTeamFooter.TeamName = organization.Name;
		ucTeamFooter.OrganizationId = organizationId;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.TeamLogo = squareLogo;
		Master.FbImageType = "image/jpg";
		Master.FbURL = Request.Url.AbsoluteUri;

		//ucTeamHeader.Logo = logo;
		//ucTeamHeader.OrganizationId = organizationId;
		//ucTeamHeader.PageName = "Programs";
		//ucTeamHeader.TeamDescription = organization.Description;
		//ucTeamHeader.TeamName = organization.Name;
		//ucTeamHeader.TeamSquareLogo = squareLogo;

		bool isOwner = false;
		if (User.Identity.IsAuthenticated == true)
		{
			var userOrganizationOwner = (from uo in dc.UserOrganizations
										 join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
										 where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString()) && uo.IsEnabled == true
                                         && uo.OrganizationId == new Guid(organizationId)
										 select o).Take(1).SingleOrDefault();

			if (userOrganizationOwner != null)
			{
				if ((userOrganizationOwner.OwnerId != userId))
				{
					isOwner = true;
				}
			}
		}

		////////////////////////
		//END HEADER PROPERTIES
		////////////////////////
		#endregion





		var organizationEvents = from oe in dc.OrganizationEvents
								 where oe.OrganizationId == new Guid(organizationId)
								 select new { oe.VolunteerHourlyRate, oe.OrganizationEventId };

		decimal? totalCauseVolunteerValue = 0;
		decimal? causeVolunteerValue = 0;
		int totalVolunteerHours = 0;

		foreach (var organizationEvent in organizationEvents)
		{
			//Get each cause rate and hours.
			var totalVolunteerHour = dc.GetTotalHoursByCause(organizationEvent.OrganizationEventId).First().Column1;
			if (totalVolunteerHour != null)
			{
				totalVolunteerHours += Convert.ToInt32(totalVolunteerHour);
				causeVolunteerValue += (Convert.ToInt32(totalVolunteerHour) * organizationEvent.VolunteerHourlyRate);
				totalCauseVolunteerValue += causeVolunteerValue;
			}
		}

		CultureInfo culture = new CultureInfo("en-US");
		lblOffset.Text = string.Format(culture, "{0:C}", totalCauseVolunteerValue);
		lblHours.Text = string.Format(culture, "{0:N0}", totalVolunteerHours);

		var totalVolunteers = (from org in dc.UserOrganizations
							   where org.OrganizationId == new Guid(organizationId) && org.IsEnabled == true
                               select org).Distinct().Count();

		lblTeamCount.Text = totalVolunteers.ToString();
		var deployments = from org in dc.Organizations
						  join oe in dc.OrganizationEvents on org.OrganizationId equals oe.OrganizationId
						  where oe.OrganizationId == new Guid(organizationId)
						  &&
						  org.IsActive == true
						  &&
						  oe.IsActive == true
						  select org;

		lblCauseCount.Text = deployments.Count().ToString();

		var campaigns = from oe in dc.OrganizationEvents
						join ev in dc.Events on oe.EventId equals ev.EventId
						where oe.OrganizationId == new Guid(organizationId)
						select new { oe.OrganizationEventId, oe.URLFriendlyCampaignName, campaignName = oe.CampaignName, oe.EndDate, oe.BeginDate };

		rptActiveCampaigns.DataSource = campaigns.OrderByDescending(d => d.BeginDate).ToList();
		rptActiveCampaigns.DataBind();

		litStatesCounties.Text = CrowdRelief.Tools.GetImpactedStateCountyStringByTeam(new Guid(organizationId));

		LoadTeamCountGraph(organizationId);
	}

	public void LoadTeamCountGraph(string organizationId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		DateTime currentDate = DateTime.Now;
		var startOfCurrentWeek = currentDate.AddDays(-(int)currentDate.DayOfWeek);
		var eightWeeksLater = startOfCurrentWeek.AddDays(7 * 5);

		var query = from ur in dc.UserAvailableDates
					join uo in dc.UserOrganizations on ur.UserId equals uo.UserId
					where ur.DateAvailable >= startOfCurrentWeek && ur.DateAvailable <= eightWeeksLater
					&& uo.OrganizationId == new Guid(organizationId) && uo.IsEnabled == true
                    group ur by new
					{
						WeekStart = ur.DateAvailable.AddDays(-(int)ur.DateAvailable.DayOfWeek)
					} into g
					select new
					{
						WeekStart = g.Key.WeekStart,
						UserCount = g.Select(ur => ur.UserId).Distinct().Count()
					};

		var result = query.ToList();

		List<string> weekStarts = new List<string>();
		List<int> userCounts = new List<int>();

		foreach (var item in result)
		{
			weekStarts.Add(String.Format("\"{0}\"",
				item.WeekStart.ToString("MMMM dd")));
			userCounts.Add(item.UserCount);
		}

		string weekStartsString = String.Join(", ", weekStarts);
		string userCountsString = String.Join(", ", userCounts);

		availableDates = weekStartsString;
		teamCounts = userCountsString;
	}

	protected void rptActiveCampaigns_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		CultureInfo culture = new CultureInfo("en-US");
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;

			HyperLink hypCauseName = (HyperLink)e.Item.FindControl("hypCauseName");
			Label lblBeginDate = (Label)e.Item.FindControl("lblBeginDate");
			Label lblEndDate = (Label)e.Item.FindControl("lblEndDate");
			Label lblDeploymentLength = (Label)e.Item.FindControl("lblDeploymentLength");
			HtmlTableCell tdBeginDate = e.Item.FindControl("tdBeginDate") as HtmlTableCell;
			HtmlTableCell tdEndDate = e.Item.FindControl("tdEndDate") as HtmlTableCell;

			string campaignName = (string)DataBinder.Eval(dataItem.DataItem, "campaignName");
			string URLFriendlyCampaignName = (string)DataBinder.Eval(dataItem.DataItem, "URLFriendlyCampaignName");
			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId");

			DateTime beginDate = DateTime.Now;
			if (DataBinder.Eval(dataItem.DataItem, "BeginDate") != null)
			{
				beginDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "BeginDate");
				tdBeginDate.Attributes.Add("data-value", beginDate.ToString("yyyy-MM-dd"));
			}
			DateTime endDate = DateTime.Now;
			if (DataBinder.Eval(dataItem.DataItem, "EndDate") != null)
			{
				endDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "EndDate");
				tdEndDate.Attributes.Add("data-value", endDate.ToString("yyyy-MM-dd"));
			}
			TimeSpan deploymentTimeSpan = new TimeSpan();
			int deploymentLength = 0;
			if (beginDate != null && endDate != null)
			{
				deploymentTimeSpan = endDate - beginDate;
				deploymentLength = deploymentTimeSpan.Days;
			}

			hypCauseName.Text = campaignName;
			hypCauseName.NavigateUrl = "~/Cause/" + URLFriendlyCampaignName;
			lblBeginDate.Text = beginDate.ToShortDateString();
			lblEndDate.Text = endDate.ToShortDateString();
			lblDeploymentLength.Text = String.Format(culture, "{0:N0}", deploymentLength);
		}
	}
}