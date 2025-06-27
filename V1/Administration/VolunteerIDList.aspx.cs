using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_Administration_VolunteerIDList : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e)
	{
		CreateVolunteerList();
	}

	public void CreateVolunteerList()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var profiles =	from s in dc.Profiles
						join m in dc.aspnet_Memberships on s.UserId equals m.UserId
						orderby m.CreateDate descending
						select new { s.ProfileNumber, s.UserId, s.Firstname, s.Lastname, m.CreateDate, m.LastLoginDate, m.LoweredEmail };

		dlVolunteers.DataSource = profiles.Take(3000);
		dlVolunteers.DataBind();
	}

	protected string GetSkills(Guid userId)
	{
		string skillList = string.Empty;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var skills = from us in dc.UserSkills
					 join s in dc.Skills on us.SkillId equals s.SkillId
					 where us.UserId == userId
					 select s;

		skillList = string.Join(", ", skills.Select(p => p.Name.ToString()));

		return skillList;
	}

	protected string GetEvents(Guid userId)
	{
		string eventList = string.Empty;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var events = from us in dc.UserEvents
					 join s in dc.Events on us.EventId equals s.EventId
					 where us.UserId == userId
					 select s;

		eventList = string.Join(", ", events.Select(p => p.Name.ToString()));

		return eventList;
	}

	protected void dlVolunteers_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			string campaignImageFolder = System.Configuration.ConfigurationManager.AppSettings["CampaignImageFolder"].ToString();

			RepeaterItem dataItem = (RepeaterItem)e.Item;
			HyperLink hypID					= (HyperLink)e.Item.FindControl("hypID");
			HyperLink hypDelete				= (HyperLink)e.Item.FindControl("hypDelete");
			Literal litCreateDate = (Literal)e.Item.FindControl("litCreateDate");
			Literal litLastLoginDate = (Literal)e.Item.FindControl("litLastLoginDate");
			Literal litEmail = (Literal)e.Item.FindControl("litEmail");
			Literal litTeam = (Literal)e.Item.FindControl("litTeam");
			System.Web.UI.HtmlControls.HtmlTableCell tdDelete = (System.Web.UI.HtmlControls.HtmlTableCell)e.Item.FindControl("tdDelete");

			//Total count of items and total cost.
			Guid userId				= (Guid)DataBinder.Eval(dataItem.DataItem, "userId");
			int profileNumber		= (int)DataBinder.Eval(dataItem.DataItem, "ProfileNumber");
			string firstname		= (string)DataBinder.Eval(dataItem.DataItem, "firstname");
			string lastname			= (string)DataBinder.Eval(dataItem.DataItem, "lastname");
			DateTime createDate		= (DateTime)DataBinder.Eval(dataItem.DataItem, "createDate");
			DateTime lastLoginDate	= (DateTime)DataBinder.Eval(dataItem.DataItem, "lastLoginDate");
			string email			= (string)DataBinder.Eval(dataItem.DataItem, "LoweredEmail");

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var userOrganization = (from uo in dc.UserOrganizations
									join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
									where uo.UserId == userId
									select new { o.Name }).Take(1).SingleOrDefault();

			if (userOrganization != null)
			{
				litTeam.Text = userOrganization.Name;
			}

			if (!User.IsInRole("DeleteAdministrator"))
			{
				hypDelete.Visible = false;
			}
			else
			{
				tdDelete.Visible = true;
				thControl.Visible = true;
				hypDelete.Visible = true;
				hypDelete.NavigateUrl = "/V1/Administration/Delete/DeleteUser.aspx?userId=" + userId;
			}

			hypID.Text				= "Id: " + profileNumber + " - " + firstname + " " + lastname;
			hypID.NavigateUrl		= "/V1/Profile/Profile.aspx?userId=" + userId;
			litCreateDate.Text = createDate.ToLongDateString();
			
			litLastLoginDate.Text = lastLoginDate.ToFileTimeUtc() + " " + lastLoginDate.Year + "/" + lastLoginDate.Month + "/" + lastLoginDate.Day + " " + lastLoginDate.ToLongDateString();
			litEmail.Text			= email;

		}
	}
}