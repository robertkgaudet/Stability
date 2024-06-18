using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrowdRelief;

public partial class V1_Victim_CreateTicket : System.Web.UI.Page
{
	public string disasterDropDown = string.Empty;
	public string housingDropDown = string.Empty;
	public string recoveryDropDown = string.Empty;

	string eventId = HttpContext.Current.Request.QueryString["eventId"];
	string victimId = HttpContext.Current.Request.QueryString["victimId"];
	public string preselectedDisasterJQuery = string.Empty;
	bool userIsAssociatedWithNonProfit = false;
	public string disasterSurvivorStoryCategoryId	= ConfigurationManager.AppSettings["disasterSurvivorStoryCategoryId"].ToString();

	//If user is coming from the registration page they need to be put into the victim role.

	protected void Page_Load(object sender, EventArgs e)
	{
		if (ExistingTicketCheck(victimId))
		{
			//Already has a ticket for this disaster, send to their ticket.
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var survivor = (from p in dc.Profiles
							where p.UserId == new Guid(victimId)
							select p).SingleOrDefault();

			Response.Redirect("/ticket/" + survivor.ProfileNumber + "/" + survivor.Firstname + "-" + survivor.Lastname);
		}

		litEventName.Text = "Create a Help Request";

		if (!IsPostBack)
		{
			LoadDisasters();
			LoadHousing();
			LoadQualifiers();
			LoadBasicNeeds();
			LoadRecoveryStages();
			litStatesCounties.Text = LoadStatesCounties();
		}
	}

	public bool ExistingTicketCheck(string victimId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		bool hasExistingTicket = false;
		var userEvents = from ue in dc.UserEvents
						 join e in dc.Events on ue.EventId equals e.EventId
						 where ue.UserId == new Guid(victimId)
						 && ue.IsVictim == true
						 select new {e.Name, e.URLFriendlyName};

		//If user has events, show them and let the case manager choose those events.

		if(userEvents.Count() > 0)
		{
			string pastCases = string.Empty;
			hasExistingTicket = true;
			//Create a list of the events.
			foreach(var userEvent in userEvents)
			{
				pastCases += "<h6>" + userEvent.Name  + "</h6>";
			}
			litPastCases.Text = "<b>Past Events</b>" + pastCases;
		}
		else
		{
			hasExistingTicket = false;
		}

		return hasExistingTicket;
	}

	public void LoadHousing()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var housing = from h in dc.Housings
					  orderby h.Housing1
					  select new { h };

		foreach (var houseType in housing)
		{
			housingDropDown = housingDropDown + "<li id=\"" + houseType.h.HousingId + "\"><a href=\"#\" class=\"font-bold\" style=\"font-size:15px;\">" + houseType.h.Housing1 + "</a></li>" + Environment.NewLine;
		}
	}

	protected void LoadQualifiers()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var qualifiers = from o in dc.RebuildStatus
						 where o.StatusType == 2
						 orderby o.Status
						 select o;

		ddlQualifiers.DataSource = qualifiers;
		ddlQualifiers.DataBind();
		ddlQualifiers.Attributes.Add("multiple", "");
		ddlQualifiers.Attributes.Add("style", "width:100%; font-size:15px;");
	}

	protected void LoadBasicNeeds()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var qualifiers = from o in dc.RebuildStatus
						 where o.StatusType == 3
						 orderby o.Status
						 select o;

		ddlBasicNeeds.DataSource = qualifiers;
		ddlBasicNeeds.DataBind();
		ddlBasicNeeds.Attributes.Add("multiple", "");
		ddlBasicNeeds.Attributes.Add("style", "width:100%; font-size:15px;");
	}
	protected void LoadRecoveryStages()
	{
		string recoveryStageId = ConfigurationManager.AppSettings["recoveryStageId"].ToString();

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var recoveryStages = from s in dc.SliderTicks
							 where s.SliderId == new Guid(recoveryStageId)
							 orderby s.Tick
							 select s;

		foreach (var recoveryStage in recoveryStages)
		{
			recoveryDropDown = recoveryDropDown + "<li id=\"" + recoveryStage.SliderTickId + "\"><a href=\"#\" class=\"font-bold\" style=\"font-size:15px;\">" + recoveryStage.Lable + "</a></li>" + Environment.NewLine;
		}
	}

	public string LoadStatesCounties()
	{
		return CrowdRelief.Tools.GetImpactedStateCountyString();
	}

	public void LoadDisasters()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disasters = from d in dc.Events
						where d.IsActive == true
						orderby d.BeginDate descending
						select new { d };

		int idNumber = 0;
		disasterDropDown = "<li id=\"\"><a href=\"#\" class=\"font-bold\" style=\"font-size:15px;\"> - My Community Disaster Is Not Listed</a></li>" + Environment.NewLine;
		foreach (var disaster in disasters)
		{
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			disasterDropDown += "<li id=\"" + disaster.d.EventId + "\"><a href=\"#\" class=\"font-bold\" style=\"font-size:15px;\">" + disasterDate + " - " + disaster.d.Name + "</a></li>" + Environment.NewLine;
			idNumber = idNumber + 1;
		}
		if (!String.IsNullOrEmpty(eventId))
		{
			//Hide the Dropdown and show the selected disaster
			var disaster = (from d in dc.Events
							where d.EventId == new Guid(eventId)
							orderby d.BeginDate descending
							select new { d }).SingleOrDefault();

			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			preselectedDisasterJQuery = "$(\"#btn-dropdown.disasterEvent\").html('" + disaster.d.Name + " - " + disasterDate + "');";
			hidEventId.Value = eventId;

			var eventDetails = (from ev in dc.Events
								where ev.EventId == new Guid(eventId)
								&& ev.IsActive == true
								select ev).SingleOrDefault();

			if (eventDetails != null)
			{
				litEventName.Text = "Add " + eventDetails.Name + "Disaster Survivor";
			}
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		string eventId = hidEventId.Value;
		string housingId = hidHousingId.Value;
		string recoveryStageId = hidRecoveryStage.Value;
		Guid victimIdGuid = new Guid(victimId);

		Guid EventId = Guid.Empty;
		Guid HousingId = Guid.Empty;
		Guid RecoveryStageId = Guid.Empty;

		Guid userUserRelationshipId = new Guid(System.Configuration.ConfigurationManager.AppSettings["userUserRelationshipId"]);

		//ASSOCIATE THIS USER WITH THIS EVENT AND SET FLAG THAT THEY HAVE A CASE/TICKET
		if(!string.IsNullOrEmpty(eventId))
		{ 
			var userEvent = (from ue in dc.UserEvents
							where ue.UserId == victimIdGuid
							&& ue.EventId == new Guid(eventId)
							select ue).SingleOrDefault();

			if(userEvent == null)
			{
				//User has never been associated with this event, add them and mark as a victim.
				UserEvent newUserEvent = new UserEvent();
				newUserEvent.UserEventId = Guid.NewGuid();
				newUserEvent.UserId = victimIdGuid;
				newUserEvent.EventId = new Guid(eventId);
				newUserEvent.IsVictim = true;
				dc.UserEvents.InsertOnSubmit(newUserEvent);
				dc.SubmitChanges();
			}
			else
			{
				userEvent.IsVictim = true;
				dc.SubmitChanges();
			}
		}

		if (!String.IsNullOrEmpty(housingId))
		{
			HousingId = new Guid(housingId);

			UserHousing userHousing = new UserHousing();
			userHousing.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
			userHousing.CreatedOn = DateTime.Now;
			userHousing.HousingId = HousingId;
			userHousing.UserHousingId = Guid.NewGuid();
			userHousing.UserId = victimIdGuid;
			dc.UserHousings.InsertOnSubmit(userHousing);
			dc.SubmitChanges();
		}

		if (!String.IsNullOrEmpty(recoveryStageId))
		{
			string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
			RecoveryStageId = new Guid(recoveryStageId);

			UserSliderTick userSliderTick = new UserSliderTick();
			userSliderTick.CreatedOn = DateTime.Now;
			userSliderTick.SliderId = new Guid(rebuildProgressSliderId);
			userSliderTick.SliderTickId = RecoveryStageId;
			userSliderTick.SurvivorId = victimIdGuid;
			userSliderTick.UserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
			userSliderTick.UserSliderTickId = Guid.NewGuid();
			dc.UserSliderTicks.InsertOnSubmit(userSliderTick);
			dc.SubmitChanges();
		}

		foreach (ListItem listItem in ddlQualifiers.Items)
		{
			if (listItem.Selected == true)
			{
				Guid qualifierId = new Guid(listItem.Value);

				UserQualifier userQualifier = new UserQualifier();
				userQualifier.QualifierId = qualifierId;
				userQualifier.UserId = victimIdGuid;
				userQualifier.UserQualifierId = Guid.NewGuid();
				dc.UserQualifiers.InsertOnSubmit(userQualifier);
				dc.SubmitChanges();
			}
		}

		foreach (ListItem listItem in ddlBasicNeeds.Items)
		{
			if (listItem.Selected == true)
			{
				Guid basicNeedId = new Guid(listItem.Value);

				UserBasicNeed userBasicNeed = new UserBasicNeed();
				userBasicNeed.RebuildStatusId = basicNeedId;
				userBasicNeed.UserId = victimIdGuid;
				userBasicNeed.UserBasicNeedId = Guid.NewGuid();
				dc.UserBasicNeeds.InsertOnSubmit(userBasicNeed);
				dc.SubmitChanges();
			}
		}

		var survivor = (from p in dc.Profiles
						where p.UserId == victimIdGuid
						select p).SingleOrDefault();
		
		Response.Redirect("/ticket/" + survivor.ProfileNumber + "/" + survivor.Firstname + "-" + survivor.Lastname);

	}

	public string GetErrorMessage(MembershipCreateStatus status)
	{
		switch (status)
		{
			case MembershipCreateStatus.DuplicateUserName:
				return "Username already exists. Please enter a different user name.";

			case MembershipCreateStatus.DuplicateEmail:
				return "That e-mail address already exists, do you need to sign in?<br><a class=\"alert-link\" href=\"Login.aspx\">Sign In</a>";

			case MembershipCreateStatus.InvalidPassword:
				return "The password provided is invalid. Please enter a password with 8 characters that includes a character and a number.";

			case MembershipCreateStatus.InvalidEmail:
				return "The e-mail address provided is invalid. Please check the value and try again.";

			case MembershipCreateStatus.InvalidAnswer:
				return "The password retrieval answer provided is invalid. Please check the value and try again.";

			case MembershipCreateStatus.InvalidQuestion:
				return "The password retrieval question provided is invalid. Please check the value and try again.";

			case MembershipCreateStatus.InvalidUserName:
				return "The user name provided is invalid. Please check the value and try again.";

			case MembershipCreateStatus.ProviderError:
				return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

			case MembershipCreateStatus.UserRejected:
				return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

			default:
				return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
		}
	}
}