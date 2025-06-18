using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_EditNonProfitCampaign : BaseOrganizationWebForm
{
	public string _eventId = string.Empty;
	public string organizationEventId = string.Empty;
	public string causePhotoCount = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (Request.QueryString["OrganizationEventId"] == null)
		{
			litEventName.Text = "No OrganizationEventId provided.";
		}
		else
		{
			organizationEventId = Request.QueryString["OrganizationEventId"];
			if(!IsPostBack)
			{
				organizationEventId = Request.QueryString["OrganizationEventId"];
				loadForm(new Guid(organizationEventId));
			}
		}
	}

	protected void loadForm(Guid organizationEventId)
	{
		//Load the states list
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		//If org and eventid already exist, then don't allow them here.
		var organizationEvent = (from oe in dc.OrganizationEvents
								 join ev in dc.Events on oe.EventId equals ev.EventId
								 where oe.OrganizationEventId == organizationEventId
								 select new { ev, oe }).SingleOrDefault();

		txtVolunteerHourValue.Value = organizationEvent.oe.VolunteerHourlyRate != null ?  String.Format("{0:0.00}", organizationEvent.oe.VolunteerHourlyRate) : string.Empty;
		_eventId = organizationEvent.ev.EventId.ToString();
		txtPurposeMission.Value = organizationEvent.oe.MissionPurpose;
		txtCampaignName.Value = organizationEvent.oe.CampaignName;
		txtURLFriendlyCampaignName.Value = organizationEvent.oe.URLFriendlyCampaignName;
		chkVoad.Checked = (bool)organizationEvent.oe.IsVoadMember;
		chkAcceptsVolunteers.Checked = (bool)organizationEvent.oe.AcceptsVolunteers;
		chkIsActive.Checked = (bool)organizationEvent.oe.IsActive;
		txtVolunteerInstructions.Value = organizationEvent.oe.VolunteerInstructions;
		txtAddress.Value = organizationEvent.oe.StagingAddress;
		txtCity.Value = organizationEvent.oe.StagingCity;
		string beginDate = string.Empty;
		string endDate = string.Empty;
		if (organizationEvent.oe.BeginDate != null)
		{
			beginDate = Convert.ToDateTime(organizationEvent.oe.BeginDate).ToShortDateString();
		}
		if (organizationEvent.oe.EndDate != null)
		{
			endDate = Convert.ToDateTime(organizationEvent.oe.EndDate).ToShortDateString();
		}
		hidDeploymentBeginDate.Value = beginDate;
		hidDeploymentEndDate.Value = endDate;

		var organizationEventPhoto = from oep in dc.OrganizationEventPhotos
									 where oep.OrganizationEventId == organizationEventId
									 select oep;

		if (organizationEventPhoto != null)
		{
			causePhotoCount = organizationEventPhoto.Count().ToString();
			//Show delete button instead of the add button.
		}

		if (!string.IsNullOrEmpty(organizationEvent.oe.StagingStateId.ToString() ))
		{
			//Load the county dropdownlist and select the right value.

			var counties = from c in dc.Counties
						   //join ec in dc.EventCounties on c.CountyId equals ec.CountyId
						   where 
						   //ec.EventId == new Guid(_eventId) &&
						   c.StateId == organizationEvent.oe.StagingStateId
						   orderby c.Name
						   select new { Value = c.CountyId, Text = c.Name };

			ddlCounties.DataSource = counties;
			ddlCounties.DataBind();

			ddlCounties.Enabled = true;
			ddlCounties.SelectedValue = Convert.ToString(organizationEvent.oe.StagingCountyId);
			hidCountyId.Value = Convert.ToString(organizationEvent.oe.StagingCountyId);
		}
		
		txtZipCode.Value = organizationEvent.oe.StagingZipCode;
		txtPOCFullname.Value = organizationEvent.oe.PointOfContactName;
		txtPhoneNumber.Value = organizationEvent.oe.PhoneNumber;
		txtZelloChannel.Value = organizationEvent.oe.ZelloChannel;
		txtEmailAddress.Value = organizationEvent.oe.Email;
		txtDonationLink.Value = organizationEvent.oe.DonationURL;
		txtVolunteerLink.Value = organizationEvent.oe.VolunteerURL;
		txtHelpLink.Value = organizationEvent.oe.HelpURL;
		txtWebsite.Value = organizationEvent.oe.Website;
		txtblogURL.Value = organizationEvent.oe.BlogURL;
		txtFacebook.Value = organizationEvent.oe.FacebookPage;
		txtFacebookGroup.Value = organizationEvent.oe.FacebookGroup;


		//The state/county selected for deployment does not need to be a part of the deployment.
		var states = from s in dc.USStates
					 //join es in dc.EventStates on s.StatesId equals es.StatesId
					 //where es.EventId == organizationEvent.ev.EventId
					 orderby s.Name
					 select new { s.StatesId, s.Name };

		ddlState.DataSource = states;
		ddlState.DataBind();

		ddlState.Items.Insert(0, new ListItem("Choose a State", ""));

		ddlState.SelectedValue = organizationEvent.oe.StagingStateId.ToString();

		var disaster = (from ev in dc.Events
						where ev.EventId == organizationEvent.oe.EventId
						select new { ev.Name }).SingleOrDefault();

		litEventName.Text = disaster.Name;
		//Hide form and show message and link to the campaign.
		divMessage.Visible = true;
		divForm.Visible = true;
		hypLinkToCampaign.NavigateUrl = "/Cause/" + organizationEvent.oe.URLFriendlyCampaignName;
		hypLinkToCampaign.Text = " Visit your campaign page " + organizationEvent.oe.CampaignName;
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
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
		string phoneNumber = txtPhoneNumber.Value;
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
		string beginDate = hidDeploymentBeginDate.Value;
		string endDate = hidDeploymentEndDate.Value;

		Guid organizationEventId = new Guid(Request.QueryString["OrganizationEventId"]);

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organizationEvent = (from oe in dc.OrganizationEvents
								 where oe.OrganizationEventId == organizationEventId
								 select oe).SingleOrDefault();

		if(!String.IsNullOrEmpty(beginDate))
		{
			organizationEvent.BeginDate = Convert.ToDateTime(beginDate);
		}

		if (!String.IsNullOrEmpty(endDate))
		{
			organizationEvent.EndDate = Convert.ToDateTime(endDate);
		}

		organizationEvent.PointOfContactName = pointOfContactName;
		organizationEvent.AcceptsVolunteers = acceptsVolunteers;
		organizationEvent.MissionPurpose = Server.HtmlEncode(purposeMission);
		organizationEvent.CampaignName = campaignName;
		organizationEvent.URLFriendlyCampaignName = URLFriendlyCampaignName;
		organizationEvent.VolunteerHourlyRate = String.IsNullOrEmpty(volunteerHourlyValue) ? 0 : Convert.ToDecimal(volunteerHourlyValue);
		organizationEvent.VolunteerInstructions = volunteerInstructions;
		organizationEvent.AcceptsVolunteers = acceptsVolunteers;
		organizationEvent.IsActive = isActive;
		organizationEvent.PointOfContactName = POCName;
		organizationEvent.PhoneNumber = phoneNumber;
		organizationEvent.ZelloChannel = zelloChannel;
		organizationEvent.Email = emailAddress;
		organizationEvent.DonationURL = string.IsNullOrEmpty(donationLink) ? null : donationLink;
		organizationEvent.HelpURL = string.IsNullOrEmpty(helpLink) ? null : helpLink;
		organizationEvent.VolunteerURL = string.IsNullOrEmpty(volunteerLink) ? null : volunteerLink;
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
		organizationEvent.IsVoadMember = isVoad;
		dc.SubmitChanges();

		Response.Redirect("/Cause/" + URLFriendlyCampaignName);
	}

	protected void btnSubmit_Cancel(object sender, EventArgs e)
	{
		Response.Redirect("/V1/Administration/NonProfitList.aspx");
	}
}