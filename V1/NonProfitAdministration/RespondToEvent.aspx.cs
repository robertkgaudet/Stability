using System;
using System.Linq;
using System.Web.ClientServices.Providers;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_RespondToEvent : BaseOrganizationWebForm
{
	public string disasterDropDown = string.Empty;
	public string preselectedDisasterJQuery = string.Empty;
	public string eventId = string.Empty;
	public string organizationId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		eventId = Request.QueryString["eventId"];
		organizationId = Request.QueryString["organizationId"];
		//If org and eventid already exist, then don't allow them here.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		if (!IsPostBack)
		{
			if (String.IsNullOrEmpty(eventId))
			{
				//No event id was sent.
				divCreateCause.Visible = false;
				divSelectEvent.Visible = true;
				LoadDisasters();
			}
			else
			{
				Guid guidEventId = new Guid(eventId);
				divCreateCause.Visible = true;
				divSelectEvent.Visible = false;
				var states = from s in dc.USStates
							 join es in dc.EventStates on s.StatesId equals es.StatesId
							 where es.EventId == guidEventId
							 select new { es.StatesId, s.Name };

				ddlState.DataSource = states;
				ddlState.DataBind();
				ddlState.Items.Insert(0, new ListItem("Choose a State", ""));

				var disaster = (from ev in dc.Events
								where ev.EventId == guidEventId
								select new { ev.Name }).SingleOrDefault();

				litEventName.Text = disaster.Name;
			}
		}
	}

	public void LoadDisasters()
	{
		eventId = Request.QueryString["eventId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disasters = from d in dc.Events
						orderby d.BeginDate descending
						where d.IsActive == true
						select new { d };

		int idNumber = 0;
		foreach (var disaster in disasters)
		{
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			disasterDropDown = disasterDropDown + "<li id=\"" + disaster.d.EventId + "\"><a href=\"#\">" + disaster.d.Name + " - " + disasterDate + "</a></li>" + Environment.NewLine;
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
		}
	}
	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		Guid eventId = new Guid(Request.QueryString["eventId"]);
		organizationId = Request.QueryString["organizationId"];

		string pointOfContactName = txtPOCFullname.Value;
		string purposeMission = txtPurposeMission.Value;
		string campaignName = txtCampaignName.Value;
		string URLFriendlyCampaignName = txtURLFriendlyCampaignName.Value.Replace(" ", "").Replace("'", "").Replace("\"", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(",", "").Replace("!", "").Replace("-", "").Replace(":", "").Replace("+", "").Replace("&", "").Replace("*", "");
		bool? isVoad = chkVoad.Checked;
		bool acceptsVolunteers = chkAcceptsVolunteers.Checked;
		bool isActive = chkIsActive.Checked;
		string volunteerInstructions = txtVolunteerInstructions.Value;
		string address = txtAddress.Value;
		string city = txtCity.Value;
		string stateId = ddlState.SelectedValue;
		string zip = txtZipCode.Value;
		string POCName = txtPOCFullname.Value;
		string phoneNumber = txtPhonenumber.Value;
		string zelloChannel = txtZelloChannel.Value;
		string emailAddress = txtEmailAddress.Value;
		string donationLink = txtDonationLink.Value;
		string volunteerLink = txtVolunteerLink.Value;
		string helpLink = txtHelpLink.Value;
		string website = txtWebsite.Value;
		string blogURL = txtblogURL.Value;
		string facebook = txtFacebook.Value;
		string facebookGroup = txtFacebookGroup.Value;
		string stagingCountyId = hidCountyId.Value;
		string volunteerHourlyValue = txtVolunteerHourValue.Value;

		OrganizationEvent organizationEvent = new OrganizationEvent();
		if (!String.IsNullOrEmpty(hidDeploymentBeginDate.Value))
		{
			organizationEvent.BeginDate = Convert.ToDateTime(hidDeploymentBeginDate.Value);
		}
		if (!String.IsNullOrEmpty(hidDeploymentBeginDate.Value))
		{
			organizationEvent.EndDate = Convert.ToDateTime(hidDeploymentEndDate.Value);
		}
		organizationEvent.OrganizationEventId = Guid.NewGuid();
		organizationEvent.EventId = eventId;
		organizationEvent.OrganizationId = new Guid(organizationId);
		organizationEvent.PointOfContactName = pointOfContactName;
		organizationEvent.AcceptsVolunteers = acceptsVolunteers;
		organizationEvent.IsActive = isActive;
		organizationEvent.MissionPurpose = purposeMission;
		organizationEvent.CampaignName = campaignName;
		organizationEvent.URLFriendlyCampaignName = URLFriendlyCampaignName;
		organizationEvent.VolunteerHourlyRate = String.IsNullOrEmpty(volunteerHourlyValue) ? 0 : Convert.ToDecimal(volunteerHourlyValue);
		organizationEvent.VolunteerInstructions = volunteerInstructions;
		organizationEvent.AcceptsVolunteers = acceptsVolunteers;
		organizationEvent.PointOfContactName = POCName;
		organizationEvent.PhoneNumber = phoneNumber;
		organizationEvent.ZelloChannel = zelloChannel;
		organizationEvent.Email = emailAddress;
		organizationEvent.DonationURL = string.IsNullOrEmpty(donationLink) ? null : donationLink;
		organizationEvent.VolunteerURL = string.IsNullOrEmpty(volunteerLink) ? null : volunteerLink;
		organizationEvent.HelpURL = string.IsNullOrEmpty(helpLink) ? null : helpLink;
		organizationEvent.Website = string.IsNullOrEmpty(website) ? null : website;
		organizationEvent.BlogURL = string.IsNullOrEmpty(blogURL) ? null : blogURL;
		organizationEvent.FacebookPage = string.IsNullOrEmpty(facebook) ? null : facebook;
		organizationEvent.FacebookGroup = string.IsNullOrEmpty(facebookGroup) ? null : facebookGroup;
		organizationEvent.StagingAddress = address;
		organizationEvent.StagingCity = city;
		if (!String.IsNullOrEmpty(stagingCountyId))
		{
			organizationEvent.StagingCountyId = new Guid(stagingCountyId);
		}
		organizationEvent.StagingStateId = new Guid(stateId);
		organizationEvent.StagingZipCode = zip;
		organizationEvent.Createdby = userId;
		organizationEvent.CreatedOn = DateTime.Now;
		organizationEvent.IsVoadMember = isVoad;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		dc.OrganizationEvents.InsertOnSubmit(organizationEvent);
		dc.SubmitChanges();

		var userEventCheck = from ue in dc.UserEvents
							 where ue.EventId == eventId && ue.UserId == userId
							 select ue;

		if(userEventCheck.Count() == 0 )
		{
			UserEvent userEvent = new UserEvent();
			userEvent.UserId = userId;
			userEvent.EventId = eventId;
			userEvent.UserEventId = Guid.NewGuid();
			userEvent.IsVictim = false;
			dc.UserEvents.InsertOnSubmit(userEvent);
			dc.SubmitChanges();
		}

		Response.Redirect("/Cause/" + URLFriendlyCampaignName);

	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Administration/NonProfitList.aspx");
	}
}