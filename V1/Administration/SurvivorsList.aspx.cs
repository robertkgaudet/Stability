using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Administration_SurvivorList : BaseOrganizationWebForm
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
						join ur in dc.aspnet_UsersInRoles on m.UserId equals ur.UserId
						join r in dc.aspnet_Roles on ur.RoleId equals r.RoleId
						where r.RoleName == "Survivor"
						orderby s.ProfileNumber descending
						select new { s.Firstname, s.Lastname, s.ProfileNumber, s.Address, s.City, m.Email, s.Description, s.State, s.Zip, s.PhoneNumber, s.ProfileId, s.UserId, m.CreateDate };

		dlVictims.DataSource = profiles.Take(100).Distinct();
		dlVictims.DataBind();
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

	protected void dlVictims_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			string campaignImageFolder = System.Configuration.ConfigurationManager.AppSettings["CampaignImageFolder"].ToString();

			RepeaterItem dataItem = (RepeaterItem)e.Item;
			HtmlGenericControl divGridClick = (HtmlGenericControl)e.Item.FindControl("divGridClick");
			HyperLink hypFullName						= (HyperLink)e.Item.FindControl("hypFullName");
			Label lblPhonenumber						= (Label)e.Item.FindControl("lblPhonenumber");
			Label lblSkills								= (Label)e.Item.FindControl("lblSkills");
			Label lblDisaster							= (Label)e.Item.FindControl("lblDisaster");
			Label litVolunteerApplicationCompletedOn	= (Label)e.Item.FindControl("litVolunteerApplicationCompletedOn");
			Label litVolunteerStatus					= (Label)e.Item.FindControl("litVolunteerStatus");
			Label lblEmail								= (Label)e.Item.FindControl("lblEmail");
			Label lblVolunteerReviewStatus				= (Label)e.Item.FindControl("lblVolunteerReviewStatus");
			Label lblLocation							= (Label)e.Item.FindControl("lblLocation");
			Label lblVolunteerSkills					= (Label)e.Item.FindControl("lblVolunteerSkills");
			Label lblID									= (Label)e.Item.FindControl("lblId");

			//Total count of items and total cost.
			String firstname							= (String)DataBinder.Eval(dataItem.DataItem, "firstname");
			Guid userId									= (Guid)DataBinder.Eval(dataItem.DataItem, "userId");
			String lastname								= (String)DataBinder.Eval(dataItem.DataItem, "lastname");
			String phonenumber							= (String)DataBinder.Eval(dataItem.DataItem, "phonenumber");
			DateTime createDate							= (DateTime)DataBinder.Eval(dataItem.DataItem, "CreateDate");
			String email								= (String)DataBinder.Eval(dataItem.DataItem, "email");
			String address								= (String)DataBinder.Eval(dataItem.DataItem, "address");
			String city									= (String)DataBinder.Eval(dataItem.DataItem, "city");
			String state								= (String)DataBinder.Eval(dataItem.DataItem, "state");
			String zip									= (String)DataBinder.Eval(dataItem.DataItem, "zip");
			String description							= (String)DataBinder.Eval(dataItem.DataItem, "Description");
			int profileNumber							= (int)DataBinder.Eval(dataItem.DataItem, "ProfileNumber");

			lblID.Text									= profileNumber.ToString();
			hypFullName.Text							= firstname + " " + lastname + " ID " + profileNumber;
			hypFullName.NavigateUrl						= "/V1/Profile/Profile.aspx?userId=" + userId;
			lblPhonenumber.Text							= phonenumber;
			lblSkills.Text								= GetSkills(userId);
			lblDisaster.Text							= GetEvents(userId);
			lblEmail.Text								= email;
			litVolunteerApplicationCompletedOn.Text		= createDate.ToShortDateString();
			lblLocation.Text							= address + "</br>" + city + " " + state + ", " + zip;
			lblVolunteerSkills.Text						= "<b>Description:</b> " + description;
			
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var profile = (from p in dc.Profiles
						  where p.UserId == userId
						  select p).SingleOrDefault();

			string volunteerStatus = VolunteerStatus.GetVolunteerStatus(userId).Value;

			if(volunteerStatus == VolunteerStatus.ApplicationComplete.Value)
			{
				//User has completed a volunteer application.
				litVolunteerStatus.Text = "<b>APPLICATION SUBMITTED</b><p>Your volunteer application is under review. (" + VolunteerStatus.VettingStarted.Value + ")</p>";
			}
			else if(volunteerStatus == VolunteerStatus.NotYetApplied.Value)
			{
				//User has not started application process
				litVolunteerStatus.Text = "<b>VOLUNTEERS NEEDED</b><p>Volunteer using the Volunteer link in the navigation menu. (" + VolunteerStatus.NotYetApplied.Value + ")</p>";
			}
			else if(volunteerStatus == VolunteerStatus.VettingComplete_Failed.Value)
			{
				//User failed vetting
				//Have them contact us.
				litVolunteerStatus.Text = "<b>APPLICATION PENDING REVIEW</b><p>Please contact Stability regarding your volunteer application.</p>";
			}
			else if(volunteerStatus == VolunteerStatus.VettingComplete_Passed.Value)
			{
				//User passed vetting.
				litVolunteerStatus.Text = "<b>APPLICATION APPROVED</b><p>Your volunteer application has been approved, welcome aboard! You might want to print an ID card or purchase a t-shirt to show your support! (" + VolunteerStatus.VettingComplete_Passed.Value + ")</p>";
			}
			else if(volunteerStatus == VolunteerStatus.VettingStarted.Value)
			{
				//User passed vetting.
				litVolunteerStatus.Text = "<b>APPLICATION BEING VETTED</b><p>Your volunteer application is currently being reviewed. (" + VolunteerStatus.VettingStarted.Value + ")</p>";
			}

			if(profile.VettedBy != null)
			{
				Guid vettingProfileUserId = (Guid)profile.VettedBy;
				Profile vettingUsersProfile = GetUserProfileByUserId(vettingProfileUserId);

				DateTime vettingUpdatedOn = new DateTime();
				if(profile.DateVettingCompleted != null)
				{
					vettingUpdatedOn = (DateTime)profile.DateVettingCompleted;
				}
				else if(profile.DateVettingStarted != null)
				{
					vettingUpdatedOn = (DateTime)profile.DateVettingStarted;
				}
			
				lblVolunteerReviewStatus.Text = "<p>This volunteers application review was last updated by " + vettingUsersProfile.Firstname + " " + vettingUsersProfile.Lastname + " " + GetElapsedTime(vettingUpdatedOn) + ".</p>" + 
					"<p><i>Last Note:</i> " + profile.VettingNotes + "</p>";
			}










		}
	}
}