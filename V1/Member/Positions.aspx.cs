using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Member_Positions : BaseWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if(!User.Identity.IsAuthenticated)
		{
			Response.Redirect("/SignIn");
		}

		if (!IsPostBack)
		{
			litPageName.Text = "My Scheduled Positions";
			ucMemberNavigation.UserId = userId.ToString();
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var positions = (from uoep in dc.UserOrganizationEventPositions
							 join oep in dc.OrganizationEventPositions on uoep.OrganizationEventPositionId equals oep.OrganizationEventPositionId
							 join po in dc.Positions on oep.PositionId equals po.PositionId
							 join oe in dc.OrganizationEvents on oep.OrganizationEventId equals oe.OrganizationEventId
							 join county in dc.Counties on oe.StagingCountyId equals county.CountyId
							 where uoep.UserId == userId
							 && oep.DeploymentDate >= DateTime.Today
							 orderby oep.DeploymentDate
							 select new
							 {
								 oep.ArrivalTime,
								 oep.DepartureTime,
								 oep.DeploymentDate,
								 Position = po.Name,
								 oe.CampaignName,
								 oe.MissionPurpose,
								 County = county.Name,
								 State = county.State,
								 oe.StagingCity,
								 oe.StagingAddress,
								 oe.PhoneNumber,
								 oe.PointOfContactName,
								 po.PositionId,
								 oe.OrganizationId
							 }).Distinct().ToList();

			rptMyPositions.DataSource = positions;
			rptMyPositions.DataBind();

		}
	}

	protected void rptMyPositions_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		//Get the positions.
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;

			Label lblPosition = (Label)e.Item.FindControl("lblPosition");
			Label lblDate = (Label)e.Item.FindControl("lblDate");
			Label lblArrivalTime = (Label)e.Item.FindControl("lblArrivalTime");
			Label lblLocation = (Label)e.Item.FindControl("lblLocation");
			Label lblDetails = (Label)e.Item.FindControl("lblDetails");
			HyperLink hypGetTrained = (HyperLink)e.Item.FindControl("hypGetTrained");

			TimeSpan arrivalTime = (TimeSpan)DataBinder.Eval(dataItem.DataItem, "ArrivalTime");
			TimeSpan departureTime = (TimeSpan)DataBinder.Eval(dataItem.DataItem, "DepartureTime");
			DateTime deploymentDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "DeploymentDate");
			String position = (String)DataBinder.Eval(dataItem.DataItem, "Position");
			String county = (String)DataBinder.Eval(dataItem.DataItem, "County");
			String state = (String)DataBinder.Eval(dataItem.DataItem, "State");
			String city = (String)DataBinder.Eval(dataItem.DataItem, "StagingCity");
			String stagingAddress = (String)DataBinder.Eval(dataItem.DataItem, "StagingAddress");
			String phoneNumber = (String)DataBinder.Eval(dataItem.DataItem, "PhoneNumber");
			String pointOfContactName = (String)DataBinder.Eval(dataItem.DataItem, "PointOfContactName");
			String campaignName = (String)DataBinder.Eval(dataItem.DataItem, "CampaignName");
			String missionPurpose = (String)DataBinder.Eval(dataItem.DataItem, "MissionPurpose");
			Guid organizationId = (Guid)DataBinder.Eval(dataItem.DataItem, "organizationId");
			Guid positionId = (Guid)DataBinder.Eval(dataItem.DataItem, "positionId");

			hypGetTrained.NavigateUrl = "/V1/NonProfit/TeamRole.aspx?organizationId=" + organizationId + "&positionId=" + positionId;





			//--------- BEGIN GET THE TIME INFO --------------
			DateTime arrivalDateTime = DateTime.MinValue;
			DateTime dpeartDateTime = DateTime.MinValue;
			if (arrivalTime != TimeSpan.Zero)
			{
				arrivalDateTime = DateTime.Today.Add(arrivalTime);
			}
			if (arrivalTime != TimeSpan.Zero)
			{
				dpeartDateTime = DateTime.Today.Add(departureTime);
			}

			// Format to 12-hour clock with AM/PM
			string formattedArrivalTime = arrivalDateTime != DateTime.MinValue ? arrivalDateTime.ToString("h:mmtt") : string.Empty;
			string formattedDepartureTime = dpeartDateTime != DateTime.MinValue ? dpeartDateTime.ToString("h:mmtt") : string.Empty;
			string time = string.Empty;
			if (formattedArrivalTime != string.Empty && formattedDepartureTime != string.Empty)
			{
				time = formattedArrivalTime + " to (approx) " + formattedDepartureTime;
			}
			else if
			(!String.IsNullOrEmpty(formattedArrivalTime))
			{
				time = "Arrive at " + formattedArrivalTime;
			}
			else
			{
				time = "Arrive anytime";
			}

			lblArrivalTime.Text = time;
			//--------- END GET THE TIME INFO --------------


			lblPosition.Text = position;
			lblDate.Text = deploymentDate.ToLongDateString();
			lblLocation.Text = "<b>Location</b><br>" + stagingAddress + " <br>" + city + ", " + state + "<br>(" + county +")";
			lblDetails.Text = "<b>Point of Contact</b><br>Contact Name: " + pointOfContactName + " <br>Phone: " + phoneNumber;
		}
	}
}