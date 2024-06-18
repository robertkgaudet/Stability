using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_EditNonProfitCauses : BaseOrganizationWebForm
{
	protected void Page_Load(object sender, EventArgs e)
	{
		//Make sure the user is volunteering for a nonprofit first.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		string organizationEventId = Request.QueryString["organizationEventId"];

		if (!String.IsNullOrEmpty(organizationEventId))
		{
			var organizationEvent = (from oe in dc.OrganizationEvents
									 join ev in dc.Events on oe.EventId equals ev.EventId
									where oe.OrganizationEventId == new Guid(organizationEventId)
									select new { ev.URLFriendlyName }).SingleOrDefault();

			//Automatically set this users nonprofit to this id.
			UpdateUsersNonProfitCause(Membership.GetUser().ProviderUserKey.ToString(), organizationEventId, organizationEvent.URLFriendlyName);
		}

		if(!IsPostBack)
		{
			var organizationEvents = from oe in dc.OrganizationEvents
									 join ev in dc.Events
									 on oe.EventId equals ev.EventId
									 where oe.IsActive == true
									 orderby ev.BeginDate descending
									select new {name = ev.Name + " - " + oe.CampaignName, ev.URLFriendlyName, oe.OrganizationEventId };

			rblOrganizationEvents.DataSource = organizationEvents;
			rblOrganizationEvents.DataBind();

			var userOrganizationEvent = (from uoe in dc.UserOrganizationEvents
								   where uoe.UserId == userId
								   select uoe).Take(1).SingleOrDefault();
			
			//If the user has multiple orgs selected, only choose CNGF for them.

			//Preselect the orgs for this user
			if(userOrganizationEvent != null)
			{
				ListItem listItemToSelect = rblOrganizationEvents.Items.FindByValue(userOrganizationEvent.OrganizationEventId.ToString());

				if (listItemToSelect != null)
				{
					listItemToSelect.Selected = true;
				}
			}
		}
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("Profile.aspx");
	}

	protected void UpdateUsersNonProfitCause(string userId, string organizationEventId, string urlFriendlyName)
	{
		divMessage.Visible = true;
		lblMessage.Text = "Your affilicated non-profit cause has been updated.";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		//Get the most recent record that doesn't have a deactived on date.
		var userChecks = (from p in dc.UserOrganizationEvents
							where p.UserId == new Guid(userId) && p.DeactivatedOn == null
							orderby p.CreatedOn descending
							select p).Take(1).SingleOrDefault();

		//Set the date deactivated 
		if (userChecks != null)
		{
			userChecks.DeactivatedOn = DateTime.Now;
			dc.SubmitChanges();
		}

		//insert the checked item.
		UserOrganizationEvent userOrganizationEvent = new UserOrganizationEvent();
		userOrganizationEvent.OrganizationEventId = new Guid(organizationEventId);
		userOrganizationEvent.CreatedOn = DateTime.Now;
		userOrganizationEvent.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
		userOrganizationEvent.UserOrganizationEventId = Guid.NewGuid();
		dc.UserOrganizationEvents.InsertOnSubmit(userOrganizationEvent);
		dc.SubmitChanges();

		Response.Redirect("/Disaster/" + urlFriendlyName);
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		string userId = Membership.GetUser().ProviderUserKey.ToString();
		string organizationEventId = string.Empty;

		if (rblOrganizationEvents.SelectedIndex > -1)
		{
			ListItem selectedOrganization = rblOrganizationEvents.SelectedItem;
			organizationEventId = selectedOrganization.Value;

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var organizationEvent = (from oe in dc.OrganizationEvents
									 join ev in dc.Events on oe.EventId equals ev.EventId
									 where oe.OrganizationEventId == new Guid(organizationEventId)
									 select new { ev.URLFriendlyName }).SingleOrDefault();

			UpdateUsersNonProfitCause(userId, organizationEventId, organizationEvent.URLFriendlyName);
		}
		else
		{
			//No item selected
		}
	}
}