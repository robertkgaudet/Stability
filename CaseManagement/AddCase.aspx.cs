using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrowdRelief;

public partial class CaseManagement_AddCase : System.Web.UI.Page
{
	public string disasterDropDown	= string.Empty;
	public string housingDropDown	= string.Empty;
	public string recoveryDropDown	= string.Empty;

	string eventId = HttpContext.Current.Request.QueryString["eventId"];
	public string preselectedDisasterJQuery = string.Empty;
	bool userIsAssociatedWithNonProfit = false;
	public string disasterSurvivorStoryCategoryId	= ConfigurationManager.AppSettings["disasterSurvivorStoryCategoryId"].ToString();
	public Guid organizationId	= new Guid(ConfigurationManager.AppSettings["cajunNavyFoundationId"].ToString());//Cajun Navy Foundation.

	protected void Page_Load(object sender, EventArgs e)
	{
		this.Master.HideCategoryList = true;
		

		litEventName.Text = "Add a Disaster Survivor";
		if(!String.IsNullOrEmpty(Request.QueryString["e"]))
		{
			litEventName.Text = "Before adding a story, please add a disaster survivor.";
		}

		if (!IsPostBack)
		{
			LoadDisasters();
			LoadHousing();
			LoadQualifiers();
			LoadBasicNeeds();
			LoadRecoveryStages();
		}

		//If user is a survivor, we don't need to get their information again.
		if(User.IsInRole("Survivor") && !User.IsInRole("Administrator"))
		{
			divPersonalInformation.Visible = false;
			litEventName.Text = "Update the Following Information";
		}

		//BEGIN FACEBOOK META TAGS
		Master.PageTitle		= "Add a Disater Survivor";
		Master.FbURL			= Request.Url.AbsoluteUri;
		Master.FbDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.PageDescription	= "Don't let disaster survivors be forgotten, if you are a helper or volunteer you can write stories on their behalf.";
		Master.FbImage			= "/S1/Images/CrowdReliefFBPost.jpg";
		Master.FbImageType		= "image/jpg";
		Master.FbSite_name		= "Stability Disaster Aid Platform";
		//END FACEBOOK META TAGS
	}
	
	public void LoadHousing()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var housing = from h in dc.Housings
					  orderby h.Housing1
					  select new {h };
		
		foreach(var houseType in housing)
		{
			housingDropDown = housingDropDown + "<li id=\"" + houseType.h.HousingId + "\"><a href=\"#\">" + houseType.h.Housing1 +  "</a></li>" + Environment.NewLine;
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
		ddlQualifiers.Attributes.Add("style", "width:100%;");
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
		ddlBasicNeeds.Attributes.Add("style", "width:100%;");
	}
	protected void LoadRecoveryStages()
	{
		string recoveryStageId = ConfigurationManager.AppSettings["recoveryStageId"].ToString();
	
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var recoveryStages = from s in dc.SliderTicks
					 where s.SliderId == new Guid(recoveryStageId)
					 orderby s.Tick
					 select s;
		
		foreach(var recoveryStage in recoveryStages)
		{
			recoveryDropDown = recoveryDropDown + "<li id=\"" + recoveryStage.SliderTickId + "\"><a href=\"#\">" + recoveryStage.Lable +  "</a></li>" + Environment.NewLine;
		}
	}

	public void LoadDisasters()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disasters = from d in dc.Events
						where d.IsActive == true
						orderby d.BeginDate descending
						select new {d };

		int idNumber = 0;
		foreach(var disaster in disasters)
		{
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			disasterDropDown = disasterDropDown + "<li id=\"" + disaster.d.EventId + "\"><a href=\"#\">" + disaster.d.Name + " - " + disasterDate +  "</a></li>" + Environment.NewLine;
			idNumber = idNumber + 1;
		}
		if(!String.IsNullOrEmpty(eventId))
		{
			//Hide the Dropdown and show the selected disaster
			var disaster = (from d in dc.Events
							where d.EventId == new Guid(eventId)
							orderby d.BeginDate descending
							select new {d}).SingleOrDefault();
			
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			preselectedDisasterJQuery = "$(\"#btn-dropdown.disasterEvent\").html('" + disaster.d.Name + " - " + disasterDate + "');";
			hidEventId.Value = eventId;
			
			var eventDetails = (from ev in dc.Events
						   where ev.EventId == new Guid(eventId)
						   select ev).SingleOrDefault();

			if(eventDetails != null)
			{
				litEventName.Text = "Add " + eventDetails.Name + "Disaster Survivor";
			}
		}
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		string firstname = txtFirstname.Value;
		string lastname = txtLastname.Value;
		string email = txtEmailAddress.Value;
		string username = txtUsername.Text;
		string password = txtPassword.Value;
		string telephone = txtPhonenumber.Value;
		string amazonWishList = txtAmazonWishlist.Value;
		string eventId = hidEventId.Value;
		string housingId = hidHousingId.Value;
		string recoveryStageId = hidRecoveryStage.Value;

		Guid EventId = Guid.Empty;
		Guid HousingId = Guid.Empty;
		Guid createdUserId = Guid.Empty;
		Guid RecoveryStageId = Guid.Empty;
		bool createdUserAddSuccess = true;
		
		MembershipCreateStatus status;
		//Membership.CreateUser(username, password, email); //Create the new user
		MembershipUser createdUser = Membership.CreateUser(username, password, email, "What is your first name?", txtFirstname.Value, true, out status);

		if (createdUser == null)
		{
			lblErrorMessage.Text = GetErrorMessage(status);
			lblErrorMessage.Visible = true;
			createdUserAddSuccess = false;
		}
		else
		{
			createdUserId = new Guid(createdUser.ProviderUserKey.ToString()); //Get the newly added users new userId
			
			Profile profile = new Profile();
			profile.Firstname = firstname;
			profile.Lastname = lastname;
			profile.PhoneNumber = telephone;
			profile.ProfileId = createdUserId; //Just making the profile Id the same as the userId
			profile.UserId = createdUserId;
			if(!string.IsNullOrEmpty(amazonWishList))
			{
				profile.AmazonWishListURL = amazonWishList;
			}
			profile.VettingComplete = false;

			dc.Profiles.InsertOnSubmit(profile);
			dc.SubmitChanges();

			Roles.AddUserToRole(username, "Survivor");
			Roles.AddUserToRole(username, "Member");
			createdUserAddSuccess = true;

			//Tools.SendEmail(firstname, lastname, "~\\EmailTemplates\\CreateAccount.html", "Welcome to Stability", email, this);
		}

		if(createdUserAddSuccess)
		{
			Guid userUserRelationshipId = new Guid(System.Configuration.ConfigurationManager.AppSettings["userUserRelationshipId"]);

			//Create a relationship between the helper and the recipient.
			UserUser userUser = new UserUser();
			userUser.UserUserId = Guid.NewGuid();
			userUser.AcceptedOn = DateTime.Now;
			userUser.IsActive = true;
			userUser.RequestedOn = DateTime.Now;

			//Survivor
			userUser.AcceptingUserId = createdUserId; //SURVIVOR

			//HELPER
			userUser.RequestingUserId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

			//RELATIONSHIP TYPE 
			userUser.UserUserRelationshipId = userUserRelationshipId;
			dc.UserUsers.InsertOnSubmit(userUser);
			dc.SubmitChanges();

			//userUser is the relationship between the survivor and the Case Manager.
			//userEvent is the relationship between the survivor and the event.

			//Get a list of suvivorId's that belong to this case manager
			//Then use that list to find out which ones are in the userEvent table using the suviviorId
			//Join on userUser and userEvent

			var UserOrganization = (from uo in dc.UserOrganizations
								   where uo.IsPrimary == true && uo.IsEnabled == true &&

                                   uo.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
								   select uo).Take(1).SingleOrDefault();

			OrganizationCase organizationCase = new OrganizationCase();
			organizationCase.OrganizationId = UserOrganization.OrganizationId;
			organizationCase.UserId = createdUserId;
			organizationCase.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
			organizationCase.CreatedOn = DateTime.Now;
			organizationCase.IsActive = true;
			dc.OrganizationCases.InsertOnSubmit(organizationCase);
			dc.SubmitChanges();

			if (!String.IsNullOrEmpty(eventId))
			{
				EventId = new Guid(eventId);

				UserEvent userEvent = new UserEvent();
				userEvent.EventId = EventId;
				userEvent.UserId = createdUserId;
				userEvent.UserEventId = Guid.NewGuid();
				userEvent.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
				userEvent.CreatedOn = DateTime.Now;
				dc.UserEvents.InsertOnSubmit(userEvent);
				dc.SubmitChanges();
			}

			if(!String.IsNullOrEmpty(housingId))
			{
				HousingId = new Guid(housingId);

				UserHousing userHousing = new UserHousing();
				userHousing.CreatedBy = new Guid(Membership.GetUser().ProviderUserKey.ToString());
				userHousing.CreatedOn = DateTime.Now;
				userHousing.HousingId = HousingId;
				userHousing.UserHousingId = Guid.NewGuid();
				userHousing.UserId = createdUserId;
				dc.UserHousings.InsertOnSubmit(userHousing);
				dc.SubmitChanges();
			}

			if(!String.IsNullOrEmpty(recoveryStageId))
			{
				string rebuildProgressSliderId = ConfigurationManager.AppSettings["rebuildProgressSliderId"].ToString();
				RecoveryStageId = new Guid(recoveryStageId);

				UserSliderTick userSliderTick = new UserSliderTick();
				userSliderTick.CreatedOn = DateTime.Now;
				userSliderTick.SliderId = new Guid(rebuildProgressSliderId);
				userSliderTick.SliderTickId = RecoveryStageId;
				userSliderTick.SurvivorId = createdUserId;
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
					userQualifier.UserId = createdUserId;
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
					userBasicNeed.UserId = createdUserId;
					userBasicNeed.UserBasicNeedId = Guid.NewGuid();
					dc.UserBasicNeeds.InsertOnSubmit(userBasicNeed);
					dc.SubmitChanges();
				}
			}
			//}
			//else
			//{
			//	//user already exists get their userid
			//	MembershipUser newSurvivorUser = Membership.GetUser(createdUserNameCheck); // Get the user object
			//	createdUserId = new Guid(newSurvivorUser.ProviderUserKey.ToString()); //Get the users new userId
			//}

			var survivor = (from p in dc.Profiles
                       where p.UserId == createdUserId
                       select p).SingleOrDefault();

            Response.Redirect("/case/" + survivor.ProfileNumber + "/" + survivor.Firstname + "-" + survivor.Lastname);
		
		}
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

	private string CreateRandomUsername(string allowedCharacters)
	{
		Random random = new Random();
		
		char[] chars = new char[allowedCharacters.Length];

		for (int i = 0; i < allowedCharacters.Length; i++)
		{
			chars[i] = allowedCharacters[random.Next(0, allowedCharacters.Length)];
		}

		return new string(chars);
	}
}