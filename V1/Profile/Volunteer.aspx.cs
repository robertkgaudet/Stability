using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class V1_Profile_Volunteer : BaseOrganizationWebForm
{
	public string headerColor = string.Empty;
	public string color = string.Empty;
	public string icon = string.Empty;
	public Guid eventId = new Guid(HttpContext.Current.Request.QueryString["eventId"]);

	protected void Page_Load(object sender, EventArgs e)
	{
		if(!IsPostBack)
		{
			//Volunteer application is already filled out.
			string volunteerStatus = VolunteerStatus.GetVolunteerStatus(userId).Value;
			if(volunteerStatus == VolunteerStatus.ApplicationComplete.Value)
			{
				Response.Redirect("/V1/Member/Default.aspx");
			}
			else if(volunteerStatus == VolunteerStatus.VettingComplete_Failed.Value)
			{
				Response.Redirect("/V1/Member/Default.aspx");
			}
			else if(volunteerStatus == VolunteerStatus.VettingComplete_Passed.Value)
			{
				Response.Redirect("/V1/Member/Default.aspx");
			}
			else if(volunteerStatus == VolunteerStatus.VettingStarted.Value)
			{
				Response.Redirect("/V1/Member/Default.aspx");
			}

			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var disaster = (from ev in dc.Events
							where ev.EventId == eventId
							select ev).SingleOrDefault();

			if(disaster.Icon != null)
			{
				icon = disaster.Icon.Replace("COLOR", "btn-" + disaster.Color + " btn-outline");
			}
			litDate.Text = String.Format("{0:Y}", disaster.BeginDate);

			if(disaster.Color != null)
			{
				color = disaster.Color;
				headerColor = CrowdRelief.Tools.GetColor(disaster.Color);
			}

			litEventName.Text = "Volunteer for " + disaster.Name;
			litEventDescription.Text = disaster.Description;

			Page.Title = "Volunteer for Stability - Case workers, photographers and writers needed to help Louisiana flood victims recover";

			var events = from c in dc.Events
							where c.IsActive == true && c.IsDisaster == true
							select c;
			
			chkBoxListDisasters.DataSource = events;
			chkBoxListDisasters.DataBind();

			foreach (ListItem item in chkBoxListDisasters.Items)
			{
				item.Attributes.Add("class", "checkBoxFix");
			}

			var organizations = from c in dc.Organizations
								where c.IsActive == true
								orderby c.Name
								select c;

			chkBoxOrganizations.DataSource = organizations;
			chkBoxOrganizations.DataBind();

			var skills = from c in dc.Skills
							orderby c.Name
								select c;

			chkBoxListSkills.DataSource = skills;
			chkBoxListSkills.DataBind();
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		if (User.Identity.IsAuthenticated)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			string description = txtDescription.Text;
			if (!String.IsNullOrEmpty(description))
			{
				description = description.Replace(System.Environment.NewLine, "<br>");
			}

			string previousExperience = txtPreviousExperience.Text;
			if (!String.IsNullOrEmpty(previousExperience))
			{
				previousExperience = previousExperience.Replace(System.Environment.NewLine, "<br>");
			}

			var profile = (from p in dc.Profiles
							where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
							select p).SingleOrDefault();

			if(profile != null)
			{

				string profileNumber = profile.ProfileNumber.ToString();
				profile.DatesAvailable = txtDatesAvailable.Text;
				profile.Description = description;
				profile.NumberOfDaysAvailable = txtDaysAvailable.Text;
				profile.PreviousVolunteerExperience = previousExperience;
				profile.VolunteerApplicationIsComplete = true; //Set volunteer application complete to true
				dc.SubmitChanges();

				foreach (ListItem item in chkBoxListDisasters.Items)
				{
					if (item.Selected)
					{
						var userCheck = from p in dc.UserEvents
								where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
								&& p.EventId == new Guid(item.Value)
								select p;

						if (userCheck.Count() == 0)
						{ 
							UserEvent userEvent = new UserEvent();
							userEvent.EventId = new Guid(item.Value);
							userEvent.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
							userEvent.UserEventId = Guid.NewGuid();
							dc.UserEvents.InsertOnSubmit(userEvent);
							dc.SubmitChanges();
						}
					}
				}

				foreach (ListItem item in chkBoxListSkills.Items)
				{
					if (item.Selected)
					{
						var userCheck = from p in dc.UserSkills
										where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
										&& p.SkillId == new Guid(item.Value)
										select p;

						if (userCheck.Count() == 0)
						{
							UserSkill userSkill = new UserSkill();
							userSkill.SkillId = new Guid(item.Value);
							userSkill.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
							userSkill.UserSkillId = Guid.NewGuid();
							dc.UserSkills.InsertOnSubmit(userSkill);
							dc.SubmitChanges();
						}
					}
				}

				foreach (ListItem item in chkBoxOrganizations.Items)
				{
					if (item.Selected)
					{
						var userCheck = from p in dc.UserOrganizations
										where p.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
										&& p.OrganizationId == new Guid(item.Value)
										select p;

						if (userCheck.Count() == 0)
						{
							UserOrganization userOrganization = new UserOrganization();
							userOrganization.OrganizationId = new Guid(item.Value);
							userOrganization.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
							userOrganization.UserOrganizationId = Guid.NewGuid();
							dc.UserOrganizations.InsertOnSubmit(userOrganization);
							dc.SubmitChanges();
						}
					}
				}

				divFormFields.Visible = false;
				divResults.Visible = true;

				lblNumber.Text = profileNumber;
				lblResults.Text = "Welcome aboard " + profile.Firstname + "! We will be in touch with you soon about your request to volunteer!";
			}
		}
		Response.Redirect("/V1/Member/Default.aspx");
	}
}