using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_ActivityDashboard : System.Web.UI.Page
{
	public string organizationId = string.Empty;
	public string availableDates = string.Empty;
	public string teamCounts = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamNavigation.PageName = "activityPage";
		organizationId = Request.QueryString["organizationId"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select o).SingleOrDefault();

		ucTeamNavigation.TeamName = organization.Name;

		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		Master.PageTitle = organization.Name + " Activity Dashboard on Stability";
		Master.PageDescription = organization.Description;
		Master.FbDescription = organization.Description;
		Master.FbImage = causePhotoFolder + organization.CoverImage;
		Master.FbImageType = "image/jpg";
		Master.FbSite_name = organization.Name + " Activity Dashboard on Stability";
		Master.FbURL = Request.Url.AbsoluteUri;


		string logo = string.Empty;
		string squareLogo = string.Empty;
		if (!String.IsNullOrEmpty(organization.Logo))
		{
			logo = "/Impactoid/Images/Logos/" + organization.Logo;
		}
		else
		{
			//Use placeholder image.imgLogo.Visible = true;
			logo = "/V1/Images/Logo-Placeholder.png";
		}
		if (!String.IsNullOrEmpty(organization.LogoSquare))
		{
			squareLogo = "/Impactoid/Images/Logos/" + organization.LogoSquare;
		}

		ucTeamHeader.Logo = logo;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.PageName = "Activity Dashboard";
		ucTeamHeader.TeamDescription = organization.Description;
		ucTeamHeader.TeamName = organization.Name;
		ucTeamHeader.TeamSquareLogo = squareLogo;


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
							   where org.OrganizationId == new Guid(organizationId)
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
						orderby ev.CreatedOn descending
						select new { oe.OrganizationEventId, oe.URLFriendlyCampaignName, campaignName = oe.CampaignName, oe.EndDate, oe.BeginDate };

		rptActiveCampaigns.DataSource = campaigns;
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
					&& uo.OrganizationId == new Guid(organizationId)
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

			string campaignName = (string)DataBinder.Eval(dataItem.DataItem, "campaignName");
			string URLFriendlyCampaignName = (string)DataBinder.Eval(dataItem.DataItem, "URLFriendlyCampaignName"); 
			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId");

			DateTime beginDate = DateTime.Now;
			if (DataBinder.Eval(dataItem.DataItem, "BeginDate") != null)
			{ 
				beginDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "BeginDate");
			}
			DateTime endDate = DateTime.Now;
			if (DataBinder.Eval(dataItem.DataItem, "EndDate") != null)
			{
				endDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "EndDate");
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