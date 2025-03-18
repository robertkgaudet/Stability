using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Stability;

public partial class V1_NonProfitAdministration_Participants : BaseWebForm
{
	string organizationEventId = string.Empty;
	string pageName = "Participants";
	protected void Page_Load(object sender, EventArgs e)
	{
		organizationEventId = Request.QueryString["organizationEventId"];
		ucPostionNavigation.PageName = pageName;
		ucPostionNavigation.OrganizationEventId = organizationEventId;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (!String.IsNullOrEmpty(organizationEventId))
		{
			var organization = (from oe in dc.OrganizationEvents
								join o in dc.Organizations on oe.OrganizationId equals o.OrganizationId
								join ev in dc.Events on oe.EventId equals ev.EventId
								where oe.OrganizationEventId == new Guid(organizationEventId)
								select new { oe, o, ev }).SingleOrDefault();

			if (organization.o.OwnerId == userId || User.IsInRole("Administrator"))
			{
				ucPostionNavigation.IsTeamOwner = true;
			}

			ucPostionNavigation.CampaignName = organization.oe.CampaignName;
			ucPostionNavigation.OrganizationName = organization.o.Name;
			ucPostionNavigation.PortalName = organization.ev.Name;
			ucPostionNavigation.OrganizationId = organization.o.OrganizationId.ToString();
			ucPostionNavigation.Description = organization.oe.MissionPurpose;

			ucPostionNavigation.URLFriendlyName = organization.oe.URLFriendlyCampaignName;

			var participants = from t0 in dc.OrganizationEventPositions
							   join t1 in dc.UserOrganizationEventPositions on t0.OrganizationEventPositionId equals t1.OrganizationEventPositionId
							   join t2 in dc.Profiles on t1.UserId equals t2.UserId
							   join t3 in dc.aspnet_Memberships on t2.UserId equals t3.UserId
							   where t0.OrganizationEventId == new Guid(organizationEventId)
							   group t0 by new
							   {
								   t0.OrganizationEventId,
								   t2.UserId,
								   t2.Firstname,
								   t2.Lastname,
								   t2.PhoneNumber,
								   t2.Address,
								   t2.City,
								   t2.State,
								   t2.Zip,
								   t2.ContactMethod,
								   t2.Title,
								   t2.Photo,
								   t2.VettingActive,
								   t3.Email,
								   t3.LastLoginDate
							   } into g
							   select new
							   {
								   g.Key.OrganizationEventId,
								   g.Key.UserId,
								   g.Key.Firstname,
								   g.Key.Lastname,
								   PositionCount = g.Select(x => x.PositionId).Distinct().Count(),
								   g.Key.PhoneNumber,
								   g.Key.Address,
								   g.Key.City,
								   g.Key.State,
								   g.Key.Zip,
								   g.Key.ContactMethod,
								   g.Key.Title,
								   g.Key.Photo,
								   g.Key.VettingActive,
								   g.Key.Email,
								   g.Key.LastLoginDate
							   };

			rptParticipants.DataSource = participants.OrderByDescending(o => o.LastLoginDate).ToList();
			rptParticipants.DataBind();
		}
	}

    protected void rptParticipants_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RepeaterItem dataItem = (RepeaterItem)e.Item;
            Literal litParticipantEmail = (Literal)e.Item.FindControl("litParticipantEmail");
            Literal litPositionCount = (Literal)e.Item.FindControl("litPositionCount");
            Literal litLastActiveDateTime = (Literal)e.Item.FindControl("litLastActiveDateTime");
            Button btnDelete = (Button)e.Item.FindControl("btnDelete");
            string email = (string)DataBinder.Eval(dataItem.DataItem, "Email");
            int positionCount = (int)DataBinder.Eval(dataItem.DataItem, "PositionCount");
            DateTime lastLoginDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "LastLoginDate");
            litParticipantEmail.Text = email;
            litLastActiveDateTime.Text = lastLoginDate.ToLongDateString();
            litPositionCount.Text = "Positions " + positionCount.ToString();
            var ucTeamLogo = (V1_UserControls_TeamLogo)e.Item.FindControl("ucTeamLogo");
            if (ucTeamLogo != null)
            {
                ucTeamLogo.UserId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
                ucTeamLogo.LoadNameWithBadges();
            }
        }
    }

    [WebMethod]
	public static object GetUserEventPositions(string userId, string organizationEventId)
	{
		//CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		using (var dc = new CrowdReliefDBDataContext()) // Replace with your actual DataContext
		{
			var results = (from ep in dc.UserOrganizationEventPositions
							join op in dc.OrganizationEventPositions on ep.OrganizationEventPositionId equals op.OrganizationEventPositionId
							join pr in dc.Profiles on ep.UserId equals pr.UserId
							join am in dc.aspnet_Memberships on pr.UserId equals am.UserId
							where op.OrganizationEventId == new Guid(organizationEventId)
							&& ep.UserId == new Guid(userId)
							select new
							{
								Fullname = pr.Firstname + " " + pr.Lastname,
								op.ArrivalTime,
								op.DepartureTime,
								op.IsRemote,
								PositionName = op.Position.Name,
								op.DeploymentDate,
								am.LastLoginDate,
								pr.PhoneNumber,
								am.Email,
								ep.UserId
							}).Distinct().OrderBy(o => o.DeploymentDate).ToList();

			// Handle formatting and null values after retrieval
			var formattedResults = results.Select(r => new
			{
				r.Fullname,
				r.ArrivalTime,
				r.DepartureTime,
				r.IsRemote,
				r.PositionName,
				DeploymentDate = r.DeploymentDate.HasValue
					? r.DeploymentDate.Value.ToString("ddd, MMM dd, yyyy")
					: "TBD", // Format the date and handle nulls
				r.LastLoginDate,
				r.PhoneNumber,
				r.Email,
				r.UserId
			}).ToList();

			return formattedResults.ToList();
		}
	}
}	