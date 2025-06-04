using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Deployments : BaseWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		Guid prioritizedId = new Guid("79305f85-3816-46a8-911f-0d7e3e227c32");
        string searchTerm = Request.QueryString["searchTerm"];

        // First, get the most recent event for the prioritized organization


        var otherEvents = dc.ExecuteQuery<SearchResponse.OrganizationEventResult>(
"EXEC SearchFillter {0}, {1}",
"Deployments",
string.IsNullOrWhiteSpace(searchTerm) ? "" : searchTerm);

        var combinedEvents = new List<object>();

		combinedEvents.AddRange(otherEvents.ToList());

		// Bind the combined list to the repeater
		rptDeployments.DataSource = combinedEvents;
		rptDeployments.DataBind();

		Master.PageName = "Stability Deployments";
		litCount.Text = combinedEvents.Count().ToString() + " Deployments";
		if (User.Identity.IsAuthenticated)
		{
			ucMemberNavigation.UserId = userId.ToString();
		}

		litPageName.Text = "Recent Deployments";

		string deployment = Request.QueryString["deployment"];
		if (!String.IsNullOrEmpty(deployment))
		{
			//Show a alert to pick a team.
			divJoinDeploymentMessage.Visible = true;
		}
	}

	protected void rptDeployments_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			string campaignImageFolder = System.Configuration.ConfigurationManager.AppSettings["CampaignImageFolder"].ToString();

			Image imgLogo = (Image)e.Item.FindControl("imgLogo");
			HyperLink hypDeploymentName = (HyperLink)e.Item.FindControl("hypDeploymentName");
			Label lblDescription = (Label)e.Item.FindControl("lblDescription");
			Literal litOrganizationName = (Literal)e.Item.FindControl("litOrganizationName");
			HyperLink hypEventName = (HyperLink)e.Item.FindControl("hypEventName");
			Literal litDates = (Literal)e.Item.FindControl("litDates"); 

			RepeaterItem dataItem = (RepeaterItem)e.Item;

			//Total count of items and total cost.
			string deploymentName = (string)DataBinder.Eval(dataItem.DataItem, "CampaignName");
			string organizationName = (string)DataBinder.Eval(dataItem.DataItem, "Name");
			string description = (string)DataBinder.Eval(dataItem.DataItem, "MissionPurpose");
			string logoSquare = (string)DataBinder.Eval(dataItem.DataItem, "Logo");
			object beginDate = DataBinder.Eval(dataItem.DataItem, "BeginDate");
			object endDate = DataBinder.Eval(dataItem.DataItem, "EndDate");
			Guid organizationId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationId");
			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId");

			//TODO
			//If user is team lead, they can add a deployment.
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var isOwner = (from o in dc.Organizations
						   where o.OwnerId == userId && o.OrganizationId == organizationId
						   select o).Take(1).SingleOrDefault();

			var disaster = (from ev in dc.Events
						   join oe in dc.OrganizationEvents on ev.EventId equals oe.EventId
						   where oe.OrganizationEventId == organizationEventId
						   select new { ev.Name, ev.URLFriendlyName }).Take(1).SingleOrDefault();

			hypEventName.Text  = "Disaster: " + disaster.Name;
			hypEventName.NavigateUrl = "/Maps/" + disaster.URLFriendlyName;

			string beginDateText = "Date not available.";
			string endDateText = string.Empty;

			if (beginDate != null)
			{
				// Cast it to a nullable DateTime
				DateTime? _beginDate = (DateTime?)beginDate;
				if (_beginDate.Value < DateTime.Now)
				{
					// + " (Past Event)";
					beginDateText = _beginDate.Value.ToString("MM/dd/yyyy");
				}
				else
				{
					beginDateText = _beginDate.Value.ToString("MM/dd/yyyy");
				}
			}

			//if (endDate != null)
			//{
			//	// Cast it to a nullable DateTime
			//	DateTime? _endDate = (DateTime?)endDate;
			//	if (_endDate.Value < DateTime.Now)
			//	{
			//		endDateText = _endDate.Value.ToString("MM/dd/yyyy") + " (Past Event)";
			//	}
			//	else
			//	{
			//		endDateText = _endDate.Value.ToString("MM/dd/yyyy");
			//	}
			//}
			litDates.Text = "Begin Date: " + beginDateText;



			if (isOwner != null)
			{ 
				hypAddDeployment.Visible = true;
				hypAddDeployment.NavigateUrl = "/V1/NonProfitAdministration/RespondToEvent.aspx?organizationId=" + organizationId;
			}

			if (!String.IsNullOrEmpty(logoSquare))
			{
				logoSquare = "/Impactoid/Images/Logos/" + logoSquare;
				imgLogo.Width = 100;
			}
			else
			{
				logoSquare = "/V1/Images/Logo-Placeholder.png";
				imgLogo.Width = 100;
			}

			litOrganizationName.Text = "Team: " + organizationName;
			imgLogo.ImageUrl = logoSquare;
			hypDeploymentName.Text = deploymentName;
			hypDeploymentName.NavigateUrl = "/V1/NonProfit/NonProfitCampaign.aspx?organizationEventId=" + organizationEventId.ToString();
			//lblDescription.Text = description;
		}
	}
 
}