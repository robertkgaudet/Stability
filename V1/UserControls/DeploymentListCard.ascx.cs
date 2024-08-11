using Stability;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_DeploymentListCard : System.Web.UI.UserControl
{
	Guid _eventId = Guid.Empty;
	Guid _organizationId = Guid.Empty;
	public int _deploymentCount = 0;
	protected void Page_Load(object sender, EventArgs e)
	{
		int deploymentCount = 0;
		litDeployments.Text = GetDeployments(_eventId, _organizationId, out deploymentCount);
		_deploymentCount = deploymentCount;
		Session["deploymentCount"] = deploymentCount;
	}

	protected string GetDeployments(Guid eventId, Guid organizationId, out int deploymentCount)
	{
		deploymentCount = 0;
		string deploymentPanel = string.Empty;
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (organizationId != Guid.Empty)
		{
			var deployments = from org in dc.Organizations
							  join oe in dc.OrganizationEvents on org.OrganizationId equals oe.OrganizationId
							  join s in dc.USStates on oe.StagingStateId equals s.StatesId
							  join c in dc.Counties on oe.StagingCountyId equals c.CountyId
							  where oe.OrganizationId == organizationId
							  &&
							  org.IsActive == true
							  &&
							  oe.IsActive == true
							  orderby oe.CreatedOn descending
							  select new { oe.BeginDate, oe.EndDate, State = s.Name, County = c.Name, org.IsVoadMember, org.Logo, oe.VolunteerHourlyRate, org.URLFriendlyName, oe.IsActive, oe.OrganizationEventId, org.OrganizationId, DeploymentName = oe.CampaignName, oe.URLFriendlyCampaignName, OrganizationName = org.Name };

			deploymentCount = deployments.Count();

			foreach (var deployment in deployments)
			{
				int teamMembersAvailable = 0;
				string rebuildTickLabel = string.Empty;
				//string volunteersNeeded = CalculateVolunteersNeeded(Guid.Empty, 0, false, disaster.EventId);

				string beginDate = deployment.BeginDate != null ? Convert.ToDateTime(deployment.BeginDate).ToShortDateString() : string.Empty;
				string endDate = deployment.EndDate != null ? Convert.ToDateTime(deployment.EndDate).ToShortDateString() : "NA";

				TimeSpan deploymentTimeSpan = new TimeSpan();
				int deploymentLength = 0;
				if (deployment.BeginDate != null && deployment.EndDate != null)
				{
					deploymentTimeSpan = (DateTime)deployment.EndDate - (DateTime)deployment.BeginDate;
					deploymentLength = deploymentTimeSpan.Days;

					List<DateTime> allDates = GetDatesBetween((DateTime)deployment.BeginDate, (DateTime)deployment.EndDate);
					//Loop through all dates between the two ranges and count team members who are available

					foreach (DateTime date in allDates)
					{
						//Check to see how many team members are available for these dates?
						//In the future also check for matching skills.
						var teamMemberDates = (from uo in dc.UserOrganizations
											  join oe in dc.OrganizationEvents on uo.OrganizationId equals oe.OrganizationId
											  join uad in dc.UserAvailableDates on uo.UserId equals uad.UserId
											  where oe.OrganizationId == organizationId
											  && uad.DateAvailable == date
											  && oe.OrganizationEventId == deployment.OrganizationEventId
											   select uo).Count();

						teamMembersAvailable = teamMemberDates;
					}
				}

				string dateRange = String.IsNullOrEmpty(beginDate) ? "Dates Unknown" : beginDate + " to " + endDate;
				dateRange = (dateRange == "NA to NA" ? "Dates Unknown" : dateRange);

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

				string countyName = string.Empty;
				string countyTerm = "County";
				if (deployment.State == "Louisiana")
				{
					countyTerm = "Parish";
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
														"<p class=\"m-b-xs\"><small>" + deployment.OrganizationName + "</small></p>" + Environment.NewLine +
													"</div>" + Environment.NewLine +
												"</div>" + Environment.NewLine +
												"<div class=\"row\" style=\"padding:0px 10px;\">" + Environment.NewLine +
													"<div class=\"col\">" + Environment.NewLine +
														"<div><h5 class=\"font-bold\">" + deployment.DeploymentName + "</h5>" + deployment.County + " " + countyTerm + ", " + deployment.State + "</div>" + Environment.NewLine +
													"</div>" + Environment.NewLine +
												"</div>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
											"<div class=\"panel-footer\">" + Environment.NewLine +
												"<div class=\"row\"><div class=\"col-xs-8 m-s-n5\"><small>" + dateRange + "<br>" + teamMembersAvailable + " Team Members</small></div><div class=\"col-xs-4 m-s-n5\"><small>" + deploymentLength + " Days</small></div></div>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
										"</div>" + Environment.NewLine +
									"</div>" + Environment.NewLine + Environment.NewLine;

			}
		}

		if (eventId != Guid.Empty)
		{
			var deployments = from org in dc.Organizations
							  join oe in dc.OrganizationEvents on org.OrganizationId equals oe.OrganizationId
							  join s in dc.USStates on oe.StagingStateId equals s.StatesId
							  join c in dc.Counties on oe.StagingCountyId equals c.CountyId
							  where oe.EventId == eventId
							  &&
							  org.IsActive == true
							  &&
							  oe.IsActive == true
							  orderby c.Name ascending
							  select new { oe.BeginDate, oe.EndDate, State = s.Name, County = c.Name, org.IsVoadMember, org.Logo, oe.VolunteerHourlyRate, org.URLFriendlyName, oe.IsActive, oe.OrganizationEventId, org.OrganizationId, DeploymentName = oe.CampaignName, oe.URLFriendlyCampaignName, OrganizationName = org.Name };

			deploymentCount = deployments.Count();

			foreach (var deployment in deployments)
			{
				//string eventDate = String.Format("{0:Y}", deployment.date);

				string rebuildTickLabel = string.Empty;
				//string volunteersNeeded = CalculateVolunteersNeeded(Guid.Empty, 0, false, disaster.EventId);

				string beginDate = deployment.BeginDate != null ? Convert.ToDateTime(deployment.BeginDate).ToShortDateString() : "NA";
				string endDate = deployment.EndDate != null ? Convert.ToDateTime(deployment.EndDate).ToShortDateString() : "NA";

				string dateRange = String.IsNullOrEmpty(beginDate) ? "Dates Unknown" : beginDate + " to " + endDate;
				dateRange = dateRange == "NA to NA" ? "Dates Unknown" : dateRange;

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

				string countyName = string.Empty;
				string countyTerm = "County";
				if (deployment.State == "Louisiana")
				{
					countyTerm = "Parish";
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
														"<p class=\"m-b-xs\"><small>" + deployment.OrganizationName + "</small></p>" + Environment.NewLine +
													"</div>" + Environment.NewLine +
												"</div>" + Environment.NewLine +
												"<div class=\"row\" style=\"padding:0px 10px;\">" + Environment.NewLine +
													"<div class=\"col\">" + Environment.NewLine +
														"<div><h5 class=\"font-bold\">" + deployment.DeploymentName + "</h5></div>" + Environment.NewLine +
													"</div>" + Environment.NewLine +
												"</div>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
											"<div class=\"panel-footer\">" + Environment.NewLine +
												"<div class=\"row\"><div class=\"col-xs-6\"><small>" + deployment.County + " " + countyTerm + ", " + deployment.State + "</small></div><div class=\"col-xs-6\"><small>" + dateRange + "</small></div></div>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
										"</div>" + Environment.NewLine +
									"</div>" + Environment.NewLine + Environment.NewLine;

			}
		}

		return deploymentPanel;
	}

	static List<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
	{
		List<DateTime> dates = new List<DateTime>();

		for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
		{
			dates.Add(date);
		}

		return dates;
	}

	public Guid OrganizationId
	{
		get { return _organizationId; }
		set { _organizationId = value; }
	}
	public Guid EventId
	{
		get { return _eventId; }
		set { _eventId = value; }
	}
	public int DeploymentCount
	{
		get { return _deploymentCount; }
		set { _deploymentCount = value; }
	}
}