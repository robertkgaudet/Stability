using System;
using System.Linq;
using System.Web.Security;
using CrowdRelief;
using System.Web.UI.WebControls;
using System.IdentityModel.Metadata;
using System.Collections.Specialized;

public partial class V1_Register : System.Web.UI.Page
{
	public string disasterDropDown	= string.Empty;
	public string nonProfitDropDown = string.Empty;
	public string preselectedDisasterJQuery = string.Empty;
	public string preselectedNonProfitJQuery = string.Empty; 
	public string eventId = string.Empty;
	public string organizationId = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		eventId = Request.QueryString["eventId"];
		organizationId = Request.QueryString["organizationId"];

		if(User.Identity.IsAuthenticated)
		{
			//Redirect();
		}

		if(!IsPostBack)
		{
			LoadDisasters();
			LoadNonProfits(organizationId, eventId);
			string memberType = Request.QueryString["type"];

			if(!String.IsNullOrEmpty(memberType))
			{
				if(memberType == "survivor")
				{
					//Check for survivor, put into the survivor role.
				}
				else if(memberType == "helper")
				{
					//Check for member, put into the helper role.
				}
				else if(memberType == "nonprofitadministrator")
				{
					//Check for member, put into the helper role.
				}
			}
		}
	}
	protected void Redirect()
	{
		string urlRedirect = "/Survivor";
		if(Roles.IsUserInRole("survivor"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=survivor";
		}
		if(Roles.IsUserInRole("nonprofitadministrator"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=nonprofit";
		}
		if(Roles.IsUserInRole("business") || Roles.IsUserInRole("contractor"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=business";
		}
		if(Roles.IsUserInRole("helper") || Roles.IsUserInRole("volunteer"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=helper";
		}

		Response.Redirect(urlRedirect);
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		MembershipCreateStatus status;
		string passwordQuestion = "What time is lunch?";
		string firstName = txtFirstName.Text;
		string lastName = txtLastName.Text;
		string password = txtPassword.Text;
		string username = txtUsername.Text;
		string phoneNumber = txtPhoneNumber.Text;
		string passwordAnswer = "12:00";
		string email = txtEmail.Text;
		string urlRedirect = string.Empty;
		string address = txtAddress.Text;
		string city = txtCity.Text;
		string state = ddlState.Value;
		string zipCode = txtZipCode.Text;
		string eventName = string.Empty;
		string eventId = hidEventId.Value;
		string organizationId = hidOrganizationId.Value;

		MembershipUser newUser = Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, true, out status);

		if (newUser == null)
		{
			litError.Text = GetErrorMessage(status);
			lblMessage.Visible = false;
			divError.Visible = true;
		}
		else
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			//Send to the add a team page.
			urlRedirect = "/V1/Administration/NonProfitNew.aspx?userActionModal=false";
			if (!String.IsNullOrEmpty(eventId))
			{
				UserEvent userEvent = new UserEvent();
				userEvent.EventId = new Guid(eventId);
				userEvent.UserEventId = Guid.NewGuid();
				userEvent.UserId = new Guid(newUser.ProviderUserKey.ToString());
				dc.UserEvents.InsertOnSubmit(userEvent);
				dc.SubmitChanges();

				var eventNameValue = (from u in dc.Events
								where u.EventId == new Guid(eventId)
								select new { u.URLFriendlyName }).SingleOrDefault();

				eventName = eventNameValue.URLFriendlyName;
			}

			if (!String.IsNullOrEmpty(organizationId))
			{
				//user is already associated with a team, go ahead and send them to that team.
				UserOrganization userOrganization = new UserOrganization();
				userOrganization.UserOrganizationId = Guid.NewGuid();
				userOrganization.OrganizationId = new Guid(organizationId);
				userOrganization.UserId = new Guid(newUser.ProviderUserKey.ToString());
				dc.UserOrganizations.InsertOnSubmit(userOrganization);
				dc.SubmitChanges();
				urlRedirect = "/V1/NonProfit/TakeAction.aspx?organizationId=" + organizationId;
			}

			Roles.AddUserToRole(username, "Helper");
			Roles.AddUserToRole(username, "Volunteer");

			//Send to their team page or send them to a page to find or create a team.
			
			//if (rdMemberTypeCaseManager.Checked)
			//{
			//	Roles.AddUserToRole(username, "CaseManager");
			//	//Send survivor to the survivor disaster page
			//	urlRedirect = eventName = "CaseManagement/Default.aspx?userType=casemanager";
			//}

			//if (rdMemberTypeSurvivor.Checked)
			//{
			//	Roles.AddUserToRole(username, "Survivor");
			//	//Send survivor to the survivor disaster page
			//	urlRedirect = "/Disaster/" + eventName;
			//}

			//if(rdMemberTypeHelper.Checked)
			//{
			//	//Send helpers to the 

			//	//Send to place for choosing their role and skillset.
			//	urlRedirect = "/V1/Profile/EditNonProfitCauses.aspx";
			//}

			//if(rdMemberTypeNonProfit.Checked)
			//{
			//	Roles.AddUserToRole(username, "Helper");
			//	Roles.AddUserToRole(username, "Volunteer");
			//	Roles.AddUserToRole(username, "NonProfitAdministrator");
			//	urlRedirect = eventName = String.IsNullOrEmpty(eventName) ? "/V1/DisasterList.aspx?userType=nonprofit" : "/" + eventName + "/NonProfit";
			//}

			//if(rdMemberTypeBusiness.Checked)
			//{
			//	Roles.AddUserToRole(username, "Business");
			//	urlRedirect = eventName = String.IsNullOrEmpty(eventName) ? "/V1/DisasterList.aspx?userType=business" : "/" + eventName + "/Business";
			//}

			Roles.AddUserToRole(username, "Member");

			//Create a profile for this user.
			Profile userProfile			= new Profile();
			userProfile.UserId			= new Guid(newUser.ProviderUserKey.ToString());
			userProfile.ProfileId		= Guid.NewGuid();
			userProfile.Firstname		= firstName;
			userProfile.Lastname		= lastName;
			userProfile.PhoneNumber		= phoneNumber;
			userProfile.Address			= address;
			userProfile.City			= city;
			userProfile.State			= state;
			userProfile.Zip				= zipCode;
			dc.Profiles.InsertOnSubmit(userProfile);
			dc.SubmitChanges();
			
			ListDictionary ldEmailBodyReplacements = new ListDictionary();
			ldEmailBodyReplacements.Add("<% RecipientsName %>", firstName);

			string error = string.Empty;
			Tools.SendEmail(
			string.Empty,
			"Welcome to Stability",
			ldEmailBodyReplacements,
			email,
			firstName,
			string.Empty,
			string.Empty,
			"~\\EmailTemplates\\CreateAccount.html",
			out error);

			// Log the user into the site
			FormsAuthentication.SetAuthCookie(username, true);
			Response.Redirect(urlRedirect);
		}
	}
	
	public void LoadNonProfits(string organizationId, string eventId)
	{
		//Only use this if the person selected Volunteer.
		//If they have chosen an event filter down the volunteer orgs working on the event otherwise show all events.
		//If a nonprofitid is sent on the QS filter down to just that nonprofit.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		if(!String.IsNullOrEmpty(organizationId))
		{
			//Filter to just this nonprofit.
			var nonProfit = (from o in dc.Organizations
							 where o.OrganizationId == new Guid(organizationId)
							 && o.IsActive == true
							 select new { o }).SingleOrDefault();

			nonProfitDropDown = nonProfitDropDown + "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + "</a></li>" + Environment.NewLine;

			preselectedDisasterJQuery = "$(\"#btn-NonProfitDropdown.nonProfit\").html('" + nonProfit.o.Name + "');";
			hidOrganizationId.Value = organizationId;
		}
		else
		{
			if(!String.IsNullOrEmpty(eventId))
			{
				//If they chose an event, filter to orgs that are added to the event.
				var nonProfits = from o in dc.Organizations
								 join oe in dc.OrganizationEvents on o.OrganizationId equals oe.OrganizationId
								 where oe.EventId == new Guid(eventId)
								&& o.IsActive == true
								 orderby o.Name
								 select new { o, oe };

				foreach (var nonProfit in nonProfits)
				{
					nonProfitDropDown = nonProfitDropDown + "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + "</a></li>" + Environment.NewLine;
				}
			}
			else
			{
				//Load all available non-profits
				//If they chose an event, filter to orgs that are added to the event.
				var nonProfits = from o in dc.Organizations
								 where o.IsActive == true
								 orderby o.Name
								 select new { o };


				foreach (var nonProfit in nonProfits)
				{
					nonProfitDropDown = nonProfitDropDown + "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + "</a></li>" + Environment.NewLine;
					organizationId = nonProfit.o.OrganizationId.ToString();
				}

				if (nonProfits.Count() == 1)
				{
					hidOrganizationId.Value = organizationId;
				}
			}
		}
	}

	public void LoadDisasters()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var disasters = from d in dc.Events
						orderby d.BeginDate descending
						where d.IsActive == true
						select new {d };

		int idNumber = 0;
		foreach(var disaster in disasters)
		{
			string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
			disasterDropDown = disasterDropDown + "<li id=\"" + disaster.d.EventId + "\"><a href=\"#\">" + disaster.d.Name + " - " + disasterDate +  " Community Portal</a></li>" + Environment.NewLine;
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
}