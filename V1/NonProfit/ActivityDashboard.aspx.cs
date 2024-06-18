using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_ActivityDashboard : System.Web.UI.Page
{
	public string organizationId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamNavigation.PageName = "activityPage";
		organizationId = Request.QueryString["organizationId"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select o).SingleOrDefault();

		ucTeamNavigation.TeamName = organization.Name;

		Master.PageTitle = organization.Name + " Activity Dashboard on Stability";
		Master.PageDescription = organization.Description;
		Master.FbDescription = organization.Description;
		Master.FbImage = organization.CoverImage;
		Master.FbImageType = "image/jpg";
		Master.FbSite_name = organization.Name + " Activity Dashboard on Stability";
		Master.FbURL = Request.Url.AbsoluteUri;


		string logo = string.Empty;
		if (!String.IsNullOrEmpty(organization.Logo))
		{
			logo = "/Impactoid/Images/Logos/" + organization.Logo;
		}
		else
		{
			//Use placeholder image.imgLogo.Visible = true;
			logo = "/V1/Images/Logo-Placeholder.png";
		}

		ucTeamHeader.Logo = logo;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.PageName = "Activity Dashboard";
		ucTeamHeader.TeamDescription = organization.Description;
		ucTeamHeader.TeamName = organization.Name;


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
	}
}