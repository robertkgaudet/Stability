using Stability;
using System;
using System.Activities;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_DeploymentListCard : System.Web.UI.UserControl
{
	Guid _eventId = Guid.Empty;
	Guid _userId = Guid.Empty;
	Boolean _isActive = true;
	Guid _organizationId = Guid.Empty;
	public int _deploymentCount = 0;
	protected void Page_Load(object sender, EventArgs e)
	{
		int deploymentCount = 0;
		litDeployments.Text = GetDeployments(_eventId, _userId, _organizationId, _isActive, out deploymentCount);
		_deploymentCount = deploymentCount;
		Session["deploymentCount"] = deploymentCount;
	}

	protected string GetDeployments(Guid eventId, Guid userId, Guid organizationId, Boolean isActive, out int deploymentCount)
	{
		deploymentCount = 0;
		string deploymentPanel = string.Empty;
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (userId != Guid.Empty)
		{
			//Deployments for USER
			var deployments = from ue in dc.UserEvents
							  join uoe in dc.UserOrganizationEvents on ue.UserId equals uoe.UserId
							  join ev in dc.Events on ue.EventId equals ev.EventId
							  join oe in dc.OrganizationEvents on uoe.OrganizationEventId equals oe.OrganizationEventId
							  join org in dc.Organizations on oe.OrganizationId equals org.OrganizationId
							  join s in dc.USStates on oe.StagingStateId equals s.StatesId
							  join c in dc.Counties on oe.StagingCountyId equals c.CountyId
							  where uoe.UserId == userId && oe.IsActive == isActive
							  select new { oe.BeginDate, oe.CreatedOn, oe.EndDate, 
								  State = s.Name, County = c.Name, org.IsVoadMember, 
								  PortalName = ev.Name,
								  org.Logo, oe.VolunteerHourlyRate, org.URLFriendlyName, 
								  oe.IsActive, oe.OrganizationEventId, org.OrganizationId, 
								  DeploymentName = oe.CampaignName, oe.URLFriendlyCampaignName, 
								  OrganizationName = org.Name };
			
			deployments = deployments.Distinct().OrderByDescending(d => d.CreatedOn);
			deploymentCount = deployments.Count();
			//DeploymentCount = deployments.Count();

			foreach (var deployment in deployments.ToList())
			{
				string rebuildTickLabel = string.Empty;
				//string volunteersNeeded = CalculateVolunteersNeeded(Guid.Empty, 0, false, disaster.EventId);

				string beginDate = deployment.BeginDate != null ? Convert.ToDateTime(deployment.BeginDate).ToShortDateString() : string.Empty;
				string endDate = deployment.EndDate != null ? Convert.ToDateTime(deployment.EndDate).ToShortDateString() : "NA";
				string portalName = deployment.PortalName;

				TimeSpan deploymentTimeSpan = new TimeSpan();
				int deploymentLength = 0;
				if (deployment.BeginDate != null && deployment.EndDate != null)
				{
					deploymentTimeSpan = (DateTime)deployment.EndDate - (DateTime)deployment.BeginDate;
					deploymentLength = deploymentTimeSpan.Days;

					List<DateTime> allDates = GetDatesBetween((DateTime)deployment.BeginDate, (DateTime)deployment.EndDate);
					//Loop through all dates between the two ranges and count team members who are available

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

				string logoDiv = "<div id=\"logoFooter\" style=\"background-color:white; display:inline-block; padding:2px; border:solid 1px #ccc;\">" +
									"<img id=\"imgLogo\" src=\"" + logo + "\" width=\"80px\" />" +
								"</div>";

				deploymentPanel += "<div class=\"grid-item m-b-lg\" onclick=\"window.location.href='/V1/NonProfit/NonProfitCampaign.aspx?organizationEventId=" + deployment.OrganizationEventId + "';\">" + Environment.NewLine + Environment.NewLine +
										"<div class=\"hpanel\">" + Environment.NewLine +
											"<div class=\"panel-body deploymentPanel\" style=\"position: relative;\">" + Environment.NewLine +
												"<div class=\"row\" style=\"padding:0px 10px;\">" + Environment.NewLine +
													"<div class=\"col\">" + Environment.NewLine +
														"<small>" + portalName + "</small>" + Environment.NewLine +
													"</div>" + Environment.NewLine +
													"<div class=\"col\">" + Environment.NewLine +
														"<div><h4 class=\"font-bold\">" + deployment.DeploymentName + "</h4>" + deployment.County + " " + countyTerm + ", " + deployment.State + "</div>" + Environment.NewLine +
													"</div>" + Environment.NewLine +
													"<div class=\"row\"><div class=\"col-xs-8 m-s-n5\"><small>" + dateRange + "</small></div><div class=\"col-xs-4 m-s-n5\"><small>" + deploymentLength + " Days</small></div></div>" + Environment.NewLine +
												"</div>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
											"<div class=\"panel-footer\" style=\"text-align: center;\">" + Environment.NewLine +
														logoDiv + Environment.NewLine +
											"</div>" + Environment.NewLine +
										"</div>" + Environment.NewLine +
									"</div>" + Environment.NewLine + Environment.NewLine;

			}
		}

		if (organizationId != Guid.Empty)
		{
			//Deployments FOR TEAM
			var deployments = from org in dc.Organizations
							  join oe in dc.OrganizationEvents on org.OrganizationId equals oe.OrganizationId
							  join ev in dc.Events on oe.EventId equals ev.EventId
							  join s in dc.USStates on oe.StagingStateId equals s.StatesId
							  join c in dc.Counties on oe.StagingCountyId equals c.CountyId
							  where oe.OrganizationId == organizationId
							  &&
							  org.IsActive == true
							  &&
							  oe.IsActive == isActive
							  select new { oe.BeginDate, PortalName = ev.Name, oe.EndDate, oe.CreatedOn, State = s.Name, County = c.Name, org.IsVoadMember, org.Logo, oe.VolunteerHourlyRate, org.URLFriendlyName, oe.IsActive, oe.OrganizationEventId, org.OrganizationId, DeploymentName = oe.CampaignName, oe.URLFriendlyCampaignName, OrganizationName = org.Name };

			deploymentCount = deployments.Count();

			foreach (var deployment in deployments.OrderByDescending(d => d.CreatedOn).ToList())
			{
				string rebuildTickLabel = string.Empty;
				//string volunteersNeeded = CalculateVolunteersNeeded(Guid.Empty, 0, false, disaster.EventId);

				string beginDate = deployment.BeginDate != null ? Convert.ToDateTime(deployment.BeginDate).ToShortDateString() : string.Empty;
				string endDate = deployment.EndDate != null ? Convert.ToDateTime(deployment.EndDate).ToShortDateString() : "NA";
				string portalName = deployment.PortalName;

				TimeSpan deploymentTimeSpan = new TimeSpan();
				int deploymentLength = 0;
				if (deployment.BeginDate != null && deployment.EndDate != null)
				{
					deploymentTimeSpan = (DateTime)deployment.EndDate - (DateTime)deployment.BeginDate;
					deploymentLength = deploymentTimeSpan.Days;

					List<DateTime> allDates = GetDatesBetween((DateTime)deployment.BeginDate, (DateTime)deployment.EndDate);
					//Loop through all dates between the two ranges and count team members who are available

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

				string logoDiv = "<div id=\"logoFooter\" style=\"background-color:white; display:inline-block; padding:2px; border:solid 1px #ccc;\">" +
									"<img id=\"imgLogo\" src=\"" + logo + "\" width=\"80px\" />" +
								"</div>";

				deploymentPanel += "<div class=\"grid-item m-b-lg\" onclick=\"window.location.href='/V1/NonProfit/NonProfitCampaign.aspx?organizationEventId=" + deployment.OrganizationEventId + "';\">" + Environment.NewLine + Environment.NewLine +
										"<div class=\"hpanel\">" + Environment.NewLine +
											"<div class=\"panel-body deploymentPanel\" style=\"position: relative;\">" + Environment.NewLine +
												"<div class=\"row\" style=\"padding:0px 10px;\">" + Environment.NewLine +
													"<div class=\"col\">" + Environment.NewLine +
														"<small>" + portalName + "</small>" + Environment.NewLine +
													"</div>" + Environment.NewLine +
													"<div class=\"col\">" + Environment.NewLine +
														"<div><h4 class=\"font-bold\">" + deployment.DeploymentName + "</h4>" + deployment.County + " " + countyTerm + ", " + deployment.State + "</div>" + Environment.NewLine +
													"</div>" + Environment.NewLine +
												"<div class=\"row\"><div class=\"col-xs-8 m-s-n5\"><small>" + dateRange + "</small></div><div class=\"col-xs-4 m-s-n5\"><small>" + deploymentLength + " Days</small></div></div>" + Environment.NewLine +
												"</div>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
											"<div class=\"panel-footer\" style=\"text-align: center;\">" + Environment.NewLine +
														logoDiv + Environment.NewLine +
											"</div>" + Environment.NewLine +
										"</div>" + Environment.NewLine +
									"</div>" + Environment.NewLine + Environment.NewLine;

			}
		}

		if (eventId != Guid.Empty)
		{
			//Deployments FOR EVENT
			var deployments = from org in dc.Organizations
							  join oe in dc.OrganizationEvents on org.OrganizationId equals oe.OrganizationId
							  join ev in dc.Events on oe.EventId equals ev.EventId
							  join s in dc.USStates on oe.StagingStateId equals s.StatesId
							  join c in dc.Counties on oe.StagingCountyId equals c.CountyId
							  where oe.EventId == eventId
							  &&
							  org.IsActive == true
							  &&
							  oe.IsActive == isActive
							  select new { oe.BeginDate, PortalName = ev.Name, oe.EndDate, oe.CreatedOn, State = s.Name, County = c.Name, org.IsVoadMember, org.Logo, oe.VolunteerHourlyRate, org.URLFriendlyName, oe.IsActive, oe.OrganizationEventId, org.OrganizationId, DeploymentName = oe.CampaignName, oe.URLFriendlyCampaignName, OrganizationName = org.Name };

			deploymentCount = deployments.Count();

			foreach (var deployment in deployments.OrderByDescending(d => d.CreatedOn).ToList())
			{
				//string eventDate = String.Format("{0:Y}", deployment.date);

				string rebuildTickLabel = string.Empty;
				//string volunteersNeeded = CalculateVolunteersNeeded(Guid.Empty, 0, false, disaster.EventId);

				string beginDate = deployment.BeginDate != null ? Convert.ToDateTime(deployment.BeginDate).ToShortDateString() : "NA";
				string endDate = deployment.EndDate != null ? Convert.ToDateTime(deployment.EndDate).ToShortDateString() : "NA";
				string portalName = deployment.PortalName;

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

				string logoDiv = "<div id=\"logoFooter\" style=\"background-color:white; display:inline-block; padding:2px; border:solid 1px #ccc;\">" +
									"<img id=\"imgLogo\" src=\"" + logo + "\" width=\"80px\" />" +
								"</div>";

				deploymentPanel += "<div class=\"grid-item m-b-lg\" onclick=\"window.location.href='/V1/NonProfit/NonProfitCampaign.aspx?organizationEventId=" + deployment.OrganizationEventId + "';\">" + Environment.NewLine + Environment.NewLine +
										"<div class=\"hpanel\">" + Environment.NewLine +
											"<div class=\"panel-body deploymentPanel\" style=\"position: relative;\">" + Environment.NewLine +
												"<div class=\"row\" style=\"padding:0px 10px;\">" + Environment.NewLine +
													"<div class=\"col\">" + Environment.NewLine +
														"<small>" + portalName + "</small>" + Environment.NewLine +
													"</div>" + Environment.NewLine +
													"<div class=\"col\">" + Environment.NewLine +
														"<div><h4 class=\"font-bold\">" + deployment.DeploymentName + "</h4></div>" + Environment.NewLine +
													"</div>" + Environment.NewLine +
												"<div class=\"row\"><div class=\"col-xs-6\"><small>" + deployment.County + " " + countyTerm + ", " + deployment.State + "</small></div><div class=\"col-xs-6\"><small>" + dateRange + "</small></div></div>" + Environment.NewLine +
												"</div>" + Environment.NewLine +
											"</div>" + Environment.NewLine +
											"<div class=\"panel-footer\" style=\"text-align: center;\">" + Environment.NewLine +
														logoDiv + Environment.NewLine +
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

	public Guid UserId
	{
		get { return _userId; }
		set { _userId = value; }
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
	public Boolean IsActive
	{
		get { return _isActive; }
		set { _isActive = value; }
	}
}