using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_TakeAction : BaseOrganizationWebForm
{
	public string takeAction = string.Empty;
	public Guid organizationOwnerId = Guid.Empty;
	public string organizationId = string.Empty;
	public bool isUserOnTeam = false;
	public string teamName = string.Empty;
	public string signedInUserFullName = string.Empty;
	public string nonProfitDropDown	= string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		Response.Redirect("/SignIn");

		organizationId = Request.QueryString["organizationId"]; 
		string inviteSent = Request.QueryString["invite"];

		LoadNonProfits();
		lbGetHelp.NavigateUrl = "/V1/VictimAccount.aspx?organizationId=" + organizationId;
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		if (User.Identity.IsAuthenticated)
		{
			//Is logged in user on this team?

			var userCheck = (from uo in dc.UserOrganizations
							 where uo.UserId == userId && uo.Status== (int)RequestStatus.Approved
                             && uo.OrganizationId == new Guid(organizationId)
							 select uo).Take(1).SingleOrDefault();

			Profile profile = GetUserProfileByUserId(userId);
			signedInUserFullName = profile.Firstname + " " + profile.Lastname;

			if (userCheck != null)
			{
				//User is on this team.
				isUserOnTeam = true;
				hpanelJoin.Visible = false;
				hpanelMembers.Visible = true;
				hypInviteTeamMembers.Visible = true;
				hypInviteTeamMembers.NavigateUrl = "/V1/NonProfitAdministration/InviteTeam.aspx?organizationId=" + organizationId;
			}
		}

		if (!String.IsNullOrEmpty(inviteSent))
		{
			//Show a toast that says email has been sent.
			divShowInviteAlert.Visible = true;
		}

		ucDeploymentListCard.OrganizationId = new Guid(organizationId);
		ucDeploymentListCard.IsActive = true;
		//LoadCauses(organizationId, inviteSent);
		hypWebsite.NavigateUrl = "/Impactoid/CommunityPage.aspx?organizationId=" + organizationId;
		hypDemoWebsite.NavigateUrl = "/Impactoid/CommunityPage.aspx?organizationId=" + organizationId;

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select o).SingleOrDefault();
		bool userIsOwner = false;
		if (organization != null)
		{

			hypLogo.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + organizationId;
			if (!String.IsNullOrEmpty(organization.Logo))
			{
				imgLogo.Visible = true;
				imgLogo.ImageUrl = "/Impactoid/Images/Logos/" + organization.Logo;
				imgLogo.AlternateText = organization.Name + " Logo";
			}
			else
			{
				//Use placeholder image.imgLogo.Visible = true;
				imgLogo.ImageUrl = "/V1/Images/Logo-Placeholder.png";
				imgLogo.AlternateText = organization.Name + " Logo";
			}
			if ((bool)!organization.IsActive)
			{
				Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=79305f85-3816-46a8-911f-0d7e3e227c32");
			}
			organizationOwnerId = !string.IsNullOrEmpty(organization.OwnerId.ToString() ) ? (Guid)organization.OwnerId : Guid.Empty;
			userIsOwner = organization.OwnerId == userId ? true : false;
			string contactInfo = string.Empty;
			if(User.IsInRole("Administrator") || organization.OwnerId == userId)
			{
				//User is the group owner.
				hypCreateCause.NavigateUrl = "/V1/NonProfitAdministration/RespondToEvent.aspx?userActionModal=false&organizationId=" + organizationId;
				hypCreateCause.Visible = true;
				contactInfo = "<br/>EM:" + organization.PointOfContactEmail + "<br/>PH:" + organization.PointOfContactPhoneNumber;
			}

			teamName = organization.Name;
			hypOrgName.Text = organization.Name;
			hypOrgName.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + organizationId;
			lblFounded.Text = organization.CreatedOn.ToShortDateString();
			lblLocation.Text = organization.City + ", " + organization.State;
			hypTeamPointOfContact.Text = organization.PointOfContactName + contactInfo;
			hypTeamPointOfContact.NavigateUrl = "/V1/Profile/Profile.aspx?profileId=" + organization.CreatedBy.ToString();

			if (Convert.ToBoolean(organization.IsWebsiteActive))
			{	
				divWorkingWebsite.Visible = true;
				divSubscribeToWebsite.Visible = false;
			}
			else
			{
				divWorkingWebsite.Visible = false;
				divSubscribeToWebsite.Visible = true;
			}
		}
		//var campaigns = from org in dc.Organizations
		//							 join oe in dc.OrganizationEvents on org.OrganizationId equals oe.OrganizationId
		//							 join s in dc.USStates on oe.StagingStateId equals s.StatesId
		//							 where oe.OrganizationId == new Guid(organizationId)
		//							 orderby oe.IsActive descending, org.Name ascending
		//							 select new { org.IsVoadMember, oe.VolunteerHourlyRate, org.URLFriendlyName, oe.IsActive, oe.OrganizationEventId, org.OrganizationId, oe.HelpURL, oe.VolunteerURL, org.DonationURL, oe.CampaignName, oe.VolunteerInstructions, oe.MissionPurpose, oe.URLFriendlyCampaignName, oe.StagingCity, StagingState = s.Name, org.Name, org.Description };
		//if (campaigns.Count() > 0)
		//{

		//}

		var organizationEvents = from oe in dc.OrganizationEvents
								 where oe.OrganizationId == new Guid(organizationId)
								 select new { oe.VolunteerHourlyRate, oe.OrganizationEventId };

		decimal? totalCauseVolunteerValue = 0;
		decimal? causeVolunteerValue = 0;
		int totalVolunteerHours = 0;

		foreach (var organizationEvent in organizationEvents)
		{
			//Get each cause rate and hours.
			var totalVolunteerHour = dc.GetTotalHoursByCause(organizationEvent.OrganizationEventId).First().Column1;
			if (totalVolunteerHour != null)
			{
				totalVolunteerHours += Convert.ToInt32(totalVolunteerHour);
				causeVolunteerValue += (Convert.ToInt32(totalVolunteerHour) * organizationEvent.VolunteerHourlyRate);
				totalCauseVolunteerValue += causeVolunteerValue;
			}
		}
		CultureInfo culture = new CultureInfo("en-US");
		lblOffset.Text = string.Format(culture, "{0:C}", totalCauseVolunteerValue);
		lblHours.Text = string.Format(culture, "{0:N0}", totalVolunteerHours);

		//decimal volunteerRate = organizationEvent.oe.VolunteerHourlyRate != null ? Convert.ToDecimal(organizationEvent.oe.VolunteerHourlyRate) : 0;

		var totalVolunteers = (from org in dc.UserOrganizations
							   where org.OrganizationId == new Guid(organizationId) && org.Status== (int)RequestStatus.Approved
                               select org).Distinct().Count();

		lblTeamCount.Text = totalVolunteers.ToString();
		lblEvents.Text = "N/A";

		if(isUserOnTeam && !User.IsInRole("Administrator") && !userIsOwner)
		{
			var peopleList = from uo in dc.UserOrganizations
							 join p in dc.Profiles on uo.UserId equals p.UserId
							 join net in dc.aspnet_Memberships on p.UserId equals net.UserId
							 join u in dc.aspnet_Users on p.UserId equals u.UserId
							 where uo.OrganizationId == new Guid(organizationId) && uo.Status== (int)RequestStatus.Approved
                             && net.IsApproved == true && p.PassedVetting == true
							 orderby net.LastLoginDate descending
							 select new { p.Firstname, net.CreateDate, p.Description, net.LoweredEmail, p.Lastname, p.UserId, p.DateVettingCompleted, p.DateVettingStarted, p.VettingNotes, p.VettingActive, p.VettingComplete, p.PassedVetting, p.Title, p.ZelloName, LastLoginDate = u.LastActivityDate };

			hpanelMembers.Visible = true;
			rptVolunteers.DataSource = peopleList;
			rptVolunteers.DataBind();
		}
		else if (User.IsInRole("Administrator") || userIsOwner)
		{
			//ADMIN SEES ALL USERS
			var peopleList = from uo in dc.UserOrganizations
							 join p in dc.Profiles on uo.UserId equals p.UserId
							 join net in dc.aspnet_Memberships on p.UserId equals net.UserId
							 join u in dc.aspnet_Users on p.UserId equals u.UserId
							 where uo.OrganizationId == new Guid(organizationId) && uo.Status== (int)RequestStatus.Approved
                             orderby net.LastLoginDate descending
							 select new { p.Firstname, net.CreateDate, p.Description, net.LoweredEmail, p.Lastname, p.UserId, p.DateVettingCompleted, p.DateVettingStarted, p.VettingNotes, p.VettingActive, p.VettingComplete, p.PassedVetting, p.Title, p.ZelloName, LastLoginDate = u.LastActivityDate, net.IsLockedOut };

			rptVolunteers.DataSource = peopleList;
			rptVolunteers.DataBind();

			hpanelMembers.Visible = true;

			//Show message recommending user join this team.
		}
		lblCauseCount.Text = Convert.ToString(Session["deploymentCount"]);
	}

	public void LoadNonProfits()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var nonProfits = from o in dc.Organizations
						 where o.IsActive == true
						 orderby o.Name
						 select new { o };

		foreach (var nonProfit in nonProfits)
		{
			nonProfitDropDown += "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + "</a></li>" + Environment.NewLine;
		}
	}
	protected void rptVolunteers_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
			String firstname = (String)DataBinder.Eval(dataItem.DataItem, "Firstname");
			String lastname = (String)DataBinder.Eval(dataItem.DataItem, "Lastname");
			String zelloName = (String)DataBinder.Eval(dataItem.DataItem, "ZelloName");
			String title = (String)DataBinder.Eval(dataItem.DataItem, "Title");
			String loweredEmail = (String)DataBinder.Eval(dataItem.DataItem, "LoweredEmail");
			String description = (String)DataBinder.Eval(dataItem.DataItem, "Description");
			bool? vettingComplete = (bool?)DataBinder.Eval(dataItem.DataItem, "VettingComplete");
			bool? passedVetting = (bool?)DataBinder.Eval(dataItem.DataItem, "PassedVetting");
			bool? vettingActive = (bool?)DataBinder.Eval(dataItem.DataItem, "VettingActive");
			//bool? isLockedOut = (bool?)DataBinder.Eval(dataItem.DataItem, "IsLockedOut");
			String vettingNotes = (String)DataBinder.Eval(dataItem.DataItem, "VettingNotes");
			DateTime? dateVettingCompleted = (DateTime?)DataBinder.Eval(dataItem.DataItem, "DateVettingCompleted");
			DateTime? dateVettingStarted = (DateTime?)DataBinder.Eval(dataItem.DataItem, "DateVettingStarted");
			DateTime ? lastActivityDate = (DateTime?)DataBinder.Eval(dataItem.DataItem, "LastLoginDate");

			MembershipUser profileUser = Membership.GetUser(userId);

			bool isLockedOut = !profileUser.IsApproved;
			//isLockedOut
			vettingComplete = vettingComplete == null ? false : vettingComplete;
			passedVetting = passedVetting == null ? false : passedVetting;
			vettingActive = vettingActive == null ? false : vettingActive;
			DateTime dateVettingCompletedString = dateVettingCompleted == null ? DateTime.MinValue : (DateTime)dateVettingCompleted;
			DateTime dateVettingStartedString = dateVettingStarted == null ? DateTime.MinValue : (DateTime)dateVettingStarted;
			DateTime lastActivityDateString = lastActivityDate == null ? DateTime.MinValue : (DateTime)lastActivityDate;

			String dateVettingComplete = dateVettingCompletedString == DateTime.MinValue ? "Not Complete" : dateVettingCompletedString.ToLongDateString();
			String dateVettingStarts = dateVettingStartedString == DateTime.MinValue ? "Not Started" : dateVettingStartedString.ToLongDateString();
			String lastActivitysDate = lastActivityDateString == DateTime.MinValue ? "Not Started" : lastActivityDateString.ToShortDateString() + " " + lastActivityDateString.ToLongDateString() + " at " +lastActivityDateString.ToLongTimeString();

			string vettingCompleted = (bool)vettingComplete ? "VETTING COMPLETE: " + ((bool)passedVetting ? "<span style='color:yellowgreen'>PASSED</span>" : "<span style='color:orange'>FAILED</span>") : (bool)vettingActive ? "VETTING: PENDING" : "VETTING: NO ACTION TAKEN";
			vettingCompleted += ((bool)isLockedOut ? "<br><span style='color:orange'>LOCKED OUT</span>" : "<br><span style='color:yellowgreen'>HAS ACCESS</span>") + ("<br>Notes:" + vettingNotes + "<br>Date Started: " +  dateVettingStarts + "<br>Date Completed: " + dateVettingComplete + "<br>Last Activity Date: " + lastActivitysDate);

			HtmlGenericControl divFooter = (HtmlGenericControl)e.Item.FindControl("divFooter");
			HyperLink hypName = (HyperLink)e.Item.FindControl("hypName");
			Literal litMemberInfo = (Literal)e.Item.FindControl("litMemberInfo");
			Literal litDescription = (Literal)e.Item.FindControl("litDescription"); 
			Literal litSkills = (Literal)e.Item.FindControl("litSkills");
			Literal litActiveDate = (Literal)e.Item.FindControl("litActiveDate");
			Literal litVettingInfo = (Literal)e.Item.FindControl("litVettingInfo"); 
			Button btnContact = (Button)e.Item.FindControl("btnContact");
			hypName.Text = firstname + " " + lastname;
			hypName.NavigateUrl = "/V1/Profile/Profile.aspx?userId=" + userId;

			btnContact.Attributes.Add("data-name", firstname + " " + lastname);
			btnContact.Attributes.Add("data-email", loweredEmail);
			btnContact.Attributes.Add("onclick", "return btnClick(this);");

			title = !String.IsNullOrEmpty(title) ? " TITLE: " + title + "</br>" : "";
			zelloName = !String.IsNullOrEmpty(zelloName) ? " ZELLO: " + zelloName + "</br>" : "";
			description = !String.IsNullOrEmpty(description) ? " ABOUT ME: " + description + "</br>" : "";

			litVettingInfo.Text = vettingCompleted;

			litMemberInfo.Text = title + " " + zelloName;
			string skills = GetSkills(userId);
			litSkills.Text = !String.IsNullOrEmpty(skills) ? "SKILLS: " + skills + "</br>" : "";
			litDescription.Text = description;

			if (User.IsInRole("Administrator") || organizationOwnerId == userId)
			{
				divFooter.Visible = true;
				//Show the message users button.
				btnContact.Visible = true;
			}
		}
	}

	protected string GetSkills(Guid userId)
	{
		string skillList = string.Empty;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var skills = from us in dc.UserSkills
					 join s in dc.Skills on us.SkillId equals s.SkillId
					 where us.UserId == userId
					 select s;

		skillList = string.Join(" ", skills.Select(p => "<button type=\"button\" class=\"btn btn-default\">" + p.Name.ToString() + "</button>"));

		return skillList;
	}

	//protected void LoadCauses(string organizationId, string inviteSent)
	//{
	//	CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
	//	var campaigns = from oe in dc.OrganizationEvents
	//					join ev in dc.Events on oe.EventId equals ev.EventId
	//					where oe.OrganizationId == new Guid(organizationId)
	//					&& oe.IsActive == true
	//					orderby ev.BeginDate descending
	//					select new { oe.OrganizationEventId, oe.IsActive, oe.URLFriendlyCampaignName, ev.URLFriendlyName, oe.MissionPurpose, campaignName = oe.CampaignName, disasterName = ev.Name };

	//	lblCauseCount.Text = campaigns.Distinct().Count().ToString() + " Active";

	//	if(campaigns.Count() ==0 && String.IsNullOrEmpty(inviteSent))
	//	{ divNoCause.Visible = true; }
	//	else
	//	{
	//		divNoCause.Visible = false;
	//		rptActiveCampaigns.DataSource = campaigns;
	//		rptActiveCampaigns.DataBind();
	//	}
	//}
	protected void rptActiveCampaigns_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;

			HyperLink hypCauseName = (HyperLink)e.Item.FindControl("hypCauseName");
			HyperLink hypEdit = (HyperLink)e.Item.FindControl("hypEdit");
			Label lblDisasterName = (Label)e.Item.FindControl("lblDisasterName");
			Label lblCauseMission = (Label)e.Item.FindControl("lblCauseMission");
			Label lblInActiveFlag = (Label)e.Item.FindControl("lblInActiveFlag");

			string campaignName = (string)DataBinder.Eval(dataItem.DataItem, "campaignName");
			Guid organizationEventId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationEventId");
			string disasterName = (string)DataBinder.Eval(dataItem.DataItem, "disasterName");
			string URLFriendlyName = (string)DataBinder.Eval(dataItem.DataItem, "URLFriendlyName");
			string missionPurpose = (string)DataBinder.Eval(dataItem.DataItem, "missionPurpose");
			string URLFriendlyCampaignName = (string)DataBinder.Eval(dataItem.DataItem, "URLFriendlyCampaignName");
			bool isActive = (bool)DataBinder.Eval(dataItem.DataItem, "IsActive");

			hypCauseName.Text = campaignName;
			hypCauseName.NavigateUrl = "~/Cause/" + URLFriendlyCampaignName;
			lblDisasterName.Text = disasterName;
			lblCauseMission.Text = missionPurpose;

			if (User.IsInRole("Administrator"))
			{
				hypEdit.Visible = true;
				hypEdit.NavigateUrl = "/V1/NonProfitAdministration/EditNonProfitCampaign.aspx?OrganizationEventId=" + organizationEventId;
				if (!isActive)
				{
					lblInActiveFlag.Visible = true;
				}
			}
		}
	}

	protected void SendEmailInvitations(string emailAddress, string OrganizationId, string TeamName)
	{
		//Make sure and update the email sent information.
		try
		{
			string emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
			string emailFromDisplayName = ConfigurationManager.AppSettings["emailFromDisplayName"].ToString();

			ListDictionary ldEmailBodyReplacements = new ListDictionary();
			ldEmailBodyReplacements.Add("<% TeamName %>", TeamName);
			ldEmailBodyReplacements.Add("<% OrganizationId %>", OrganizationId);

			MailAddress fromAddress = new MailAddress(emailFrom, emailFromDisplayName);

			MailDefinition mailDefinition = new MailDefinition();

			mailDefinition.BodyFileName = HttpContext.Current.Server.MapPath("~\\EmailTemplates\\CreateACause.html");
			mailDefinition.Subject = "You've Been Invited To Create a Cause on The Stability Community Action Portal";
			mailDefinition.IsBodyHtml = true;

			MailMessage newUserMailMessage = mailDefinition.CreateMailMessage(emailAddress, ldEmailBodyReplacements, this);

			MailAddress bcc = new MailAddress("robgaudet@gocajunnavy.org");
			newUserMailMessage.Bcc.Add(bcc);

			SmtpClient smtp = new SmtpClient();
			//smtp.EnableSsl = true;
			smtp.Send(newUserMailMessage);
		}
		catch (Exception ex)
		{ }
	}

	protected void btnEmailTeam_Click(object sender, EventArgs e)
	{
		string organizationId = Request.QueryString["organizationId"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select o).SingleOrDefault();

		SendEmailInvitations(organization.PointOfContactEmail, organizationId, organization.Name);
		Response.Redirect("/V1/NonProfit/Default.aspx?invite=sent&organizationId=" + organizationId);
	}
}