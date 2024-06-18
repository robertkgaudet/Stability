using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_BusinessAdministration_RespondToEvent : BaseOrganizationWebForm
{
	public string _eventId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		Guid eventId = new Guid(Request.QueryString["eventId"]);
		_eventId = eventId.ToString();
		//Guid businessId = Guid.Empty;
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if (!IsPostBack)
		{
			var states = from s in dc.USStates
						 join es in dc.EventStates on s.StatesId equals es.StatesId
						 where es.EventId == eventId
						 orderby s.Name
						 select new { es.StatesId, s.Name };

			ddlState.DataSource = states;
			ddlState.DataBind();

			ddlState.Items.Insert(0, new ListItem("Choose a State", ""));

			var disaster = (from ev in dc.Events
							where ev.EventId == eventId
							select new { ev.Name }).SingleOrDefault();

			litEventName.Text = disaster.Name;
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		Guid eventId = new Guid(Request.QueryString["eventId"]);
		Guid businessId = Guid.Empty;

		if (!String.IsNullOrEmpty(Request.QueryString["businessId"]))
		{
			businessId = new Guid(Request.QueryString["businessId"]);
		}
		else
		{
			//Get the businessId from the checklist.
			businessId = new Guid(rblChooseParentBusiness.SelectedValue);
		}

		string pointOfContactName = txtPOCFullname.Value;
		string purposeMission = txtPurposeMission.Value;
		string campaignName = txtCampaignName.Value;
		string URLFriendlyCampaignName = txtURLFriendlyCampaignName.Value.Replace(" ", "").Replace("'", "").Replace("\"", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(",", "").Replace("!", "").Replace("-", "").Replace(":", "").Replace("+", "").Replace("&", "").Replace("*", "");
		bool acceptsVolunteers = chkAcceptsVolunteers.Checked;
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
		string website = txtWebsite.Value;
		string blogURL = txtblogURL.Value;
		string facebook = txtFacebook.Value;
		string facebookGroup = txtFacebookGroup.Value;



		BusinessEvent businessEvent = new BusinessEvent();
		businessEvent.BusinessEventId = Guid.NewGuid();
		businessEvent.EventId = eventId;
		businessEvent.BusinessId = businessId;
		businessEvent.PointOfContactName = pointOfContactName;
		businessEvent.AcceptsVolunteers = acceptsVolunteers;
		businessEvent.MissionPurpose = purposeMission;
		businessEvent.CampaignName = campaignName;
		businessEvent.URLFriendlyCampaignName = URLFriendlyCampaignName;
		businessEvent.VolunteerInstructions = volunteerInstructions;
		businessEvent.AcceptsVolunteers = acceptsVolunteers;
		businessEvent.PointOfContactName = POCName;
		businessEvent.PhoneNumber = phoneNumber;
		businessEvent.ZelloChannel = zelloChannel;
		businessEvent.Email = emailAddress;
		businessEvent.DonationURL = string.IsNullOrEmpty(donationLink) ? null : donationLink;
		businessEvent.Website = string.IsNullOrEmpty(website) ? null : website;
		businessEvent.BlogURL = string.IsNullOrEmpty(blogURL) ? null : blogURL;
		businessEvent.FacebookPage = string.IsNullOrEmpty(facebook) ? null : facebook;
		businessEvent.FacebookGroup = string.IsNullOrEmpty(facebookGroup) ? null : facebookGroup;
		businessEvent.StagingAddress = address;
		businessEvent.StagingCity = city;
		//businessEvent.StagingStateId = new Guid(stateId);
		businessEvent.StagingZipCode = zip;
		businessEvent.Createdby = userId;
		businessEvent.CreatedOn = DateTime.Now;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		dc.BusinessEvents.InsertOnSubmit(businessEvent);
		dc.SubmitChanges();

		Response.Redirect("/BusinessResponse/" + URLFriendlyCampaignName);

	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Administration/NonProfitList.aspx");
	}
}