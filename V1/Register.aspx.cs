using System;
using System.Linq;
using System.Web.Security;
using CrowdRelief;
using System.Web.UI.WebControls;
using System.IdentityModel.Metadata;
using System.Collections.Specialized;
using System.Web.Routing;
using Twilio.TwiML.Voice;
using System.Web.Services;

public partial class V1_Register : System.Web.UI.Page
{
	public string disasterDropDown = string.Empty;
	public string nonProfitDropDown = string.Empty;
	public string preselectedDisasterJQuery = string.Empty;
	public string preselectedNonProfitJQuery = string.Empty;
	public string eventId = string.Empty;
	public string organizationId = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (User.Identity.IsAuthenticated)
		{
			Response.Redirect("/V1/Profile/CommunityLandingPage.aspx");
		}

		eventId = Request.QueryString["eventId"];
		organizationId = Request.QueryString["organizationId"];

		if (!IsPostBack)
		{
			//LoadDisasters();
			LoadNonProfits(organizationId, eventId);
			string memberType = Request.QueryString["type"];

			if (!String.IsNullOrEmpty(memberType))
			{
				if (memberType == "survivor")
				{
					//Check for survivor, put into the survivor role.
				}
				else if (memberType == "helper")
				{
					//Check for member, put into the helper role.
				}
				else if (memberType == "nonprofitadministrator")
				{
					//Check for member, put into the helper role.
				}
			}
		}
	}

	protected void Redirect()
	{
		string urlRedirect = "/V1/Profile/CommunityLandingPage.aspx";
		if (Roles.IsUserInRole("survivor"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=survivor";
		}
		if (Roles.IsUserInRole("nonprofitadministrator"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=nonprofit";
		}
		if (Roles.IsUserInRole("business") || Roles.IsUserInRole("contractor"))
		{
			urlRedirect = "/V1/DisasterList.aspx?userType=business";
		}
		if (Roles.IsUserInRole("helper") || Roles.IsUserInRole("volunteer"))
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
		string eventName = string.Empty;
		Boolean deploymentSMS = chkMessageOptIn.Checked;
		//string eventId = hidEventId.Value;
		string organizationId = hidOrganizationId.Value;

		MembershipUser newUser = Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, true, out status);

		if (newUser == null)
		{
			litError.Text = GetErrorMessage(status);
			divError.Visible = true;
		}
		else
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			//Send to the add a team page.
			//urlRedirect = "/V1/Member/Default.aspx"; // " / V1/Administration/TeamName.aspx?userActionModal=false";

			if (!String.IsNullOrEmpty(organizationId))
			{
				//user is already associated with a team, go ahead and send them to that team.
				UserOrganization userOrganization = new UserOrganization();
				userOrganization.UserOrganizationId = Guid.NewGuid();
				userOrganization.UserId = new Guid(newUser.ProviderUserKey.ToString());
				userOrganization.OrganizationId = new Guid(organizationId);
				userOrganization.ShowTeamLogo = false;
				userOrganization.TeamVerifiedDate = null;
				userOrganization.IsPrimary = true;
				userOrganization.IsPreviousOwner = false;
				userOrganization.IsTeamAdministrator = false;
				userOrganization.IsOwner = false;
                userOrganization.Status = (int)RequestStatus.Pending;
                dc.UserOrganizations.InsertOnSubmit(userOrganization);
				dc.SubmitChanges();
				urlRedirect = "/V1/Profile/EditSkills.aspx?register=true";
			}
			else
			{
				urlRedirect = "/V1/Profile/EditSkills.aspx?register=true";
			}
			if (Request.QueryString["transactionId"] != null)
			{
				string transactionId = Request.QueryString["transactionId"].ToString();
				var donationDetail = dc.Donations.FirstOrDefault(x => x.TransactionId == transactionId);
				if (donationDetail != null && string.IsNullOrEmpty(donationDetail.FirstName) && string.IsNullOrEmpty(donationDetail.LastName) && string.IsNullOrEmpty(donationDetail.EmailAddress) && string.IsNullOrEmpty(donationDetail.PhoneNumber))
				{
					donationDetail.FirstName = firstName;
					donationDetail.LastName = lastName;
					donationDetail.EmailAddress = email;
					donationDetail.PhoneNumber = phoneNumber;
					donationDetail.UserId = new Guid(newUser.ProviderUserKey.ToString());
					dc.SubmitChanges();
				}

			}
			string role = Request.QueryString["role"];
			if (!string.IsNullOrEmpty(role) && role.ToLower() == "donor")
			{
				Roles.AddUserToRole(username, "Donor");
			}
			else
			{
				Roles.AddUserToRole(username, "Helper");
				Roles.AddUserToRole(username, "Volunteer");
				Roles.AddUserToRole(username, "Member");
			}

			Guid newUserId = Guid.NewGuid();
			//Create a profile for this user.
			Profile userProfile = new Profile();
			userProfile.UserId = new Guid(newUser.ProviderUserKey.ToString());
			userProfile.ProfileId = newUserId;
			userProfile.Firstname = firstName;
			userProfile.Lastname = lastName;
			userProfile.PhoneNumber = phoneNumber;
			userProfile.VettingActive = false;
			userProfile.VettingComplete = true;
			userProfile.PassedVetting = true;
			userProfile.DateVettingCompleted = DateTime.Now;
			userProfile.City = hfCityName.Value;
			userProfile.State = hfStateName.Value;
			userProfile.ReceiveDeploymentSMS = deploymentSMS;
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
			"~\\EmailTemplates\\NewUser.html",
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

		if (!String.IsNullOrEmpty(organizationId))
		{
			//Filter to just this nonprofit.
			var nonProfit = (from o in dc.Organizations
							 where o.OrganizationId == new Guid(organizationId)
							 && o.IsActive == true
							 select new { o }).SingleOrDefault();

			nonProfitDropDown = nonProfitDropDown + "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + "</a></li>" + Environment.NewLine;

			preselectedNonProfitJQuery = "$(\"#btn-NonProfitDropdown.nonProfit\").html('" + nonProfit.o.Name + "');";
			hidOrganizationId.Value = organizationId;
		}
		else
		{
			if (!String.IsNullOrEmpty(eventId))
			{
				//If they chose an event, filter to orgs that are added to the event.
				var nonProfits = from o in dc.Organizations
								 join oe in dc.OrganizationEvents on o.OrganizationId equals oe.OrganizationId
								 where oe.EventId == new Guid(eventId)
								&& o.IsActive == true
								 orderby o.Name
								 select new { o, oe };

				nonProfitDropDown = nonProfitDropDown + "<li><a href=\"#\">------ None ------</a></li>" + Environment.NewLine;
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


				nonProfitDropDown = nonProfitDropDown + "<li><a href=\"#\">------ None ------</a></li>" + Environment.NewLine;
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