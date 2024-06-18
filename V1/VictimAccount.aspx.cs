using System;
using System.Linq;
using System.Web.Security;
using CrowdRelief;
using System.Web.UI.WebControls;
using System.IdentityModel.Metadata;
using System.Collections.Specialized;
using System.Activities;

public partial class V1_VictimAccount : BaseWebForm
{
	public string eventId = string.Empty;
	public string organizationId = string.Empty;
	public string caseManagerId = string.Empty;
	public string urlRedirect = string.Empty;
	public string passwordGenerated = string.Empty;
	public bool isVictim = true;

	protected void Page_Load(object sender, EventArgs e)
	{
		btnSubmit.Text = "Create A Help Ticket";
		if (User.Identity.IsAuthenticated)
		{
			//Make sure they are a case manager or administrator otherwise send away.
			if(User.IsInRole("Administrator") || User.IsInRole("CaseManager"))
			{
				isVictim = false;
				//Redirect to the ticket create page but with the credentials of the logged in user and not the new one.
				caseManagerId = userId.ToString();
				divCaseManagerMessage.Visible = true;
				btnSubmit.Text = "Create Ticket";
				litSignInMessage.Text = "";
				divPassword.Visible = false;
				passwordGenerated = CreateRandomPassword("abcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()_+=`][{}");
			}
			else
			{
				//User is not allowed to add a person.
				Response.Redirect("/Error.aspx?ErrorType=VictimNoAccess");
			}
		}

		//USER CLICKED FROM AN ACTIVE COMMUNITY PORTAL.
		eventId = Request.QueryString["eventId"];

		//USER CAME FROM AN ORGANIZATION PAGE.
		organizationId = Request.QueryString["organizationId"];
	}
	private string CreateRandomPassword(string allowedCharacters)
	{
		Random random = new Random();

		char[] chars = new char[allowedCharacters.Length];

		for (int i = 0; i < allowedCharacters.Length; i++)
		{
			chars[i] = allowedCharacters[random.Next(0, allowedCharacters.Length)];
		}

		return new string(chars);
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		MembershipCreateStatus status;
		string passwordQuestion = "What time is lunch?";
		string firstName = txtFirstName.Text;
		string lastName = txtLastName.Text;
		string password = !String.IsNullOrEmpty(passwordGenerated) ? passwordGenerated : txtPassword.Text;
		string username = txtUsername.Text;
		string phoneNumber = txtPhoneNumber.Text;
		string passwordAnswer = "12:00";
		string email = txtEmail.Text;

		MembershipUser newUser = Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, true, out status);

		if (newUser == null)
		{
			litError.Text = GetErrorMessage(status);
			lblMessage.Visible = false;
			divError.Visible = true;
		}
		else
		{
			urlRedirect = "/V1/Victim/CreateTicket.aspx?organizationId=" + organizationId + "&eventId=" + eventId + "&isVictim=" + isVictim + "&caseManagerId=" + caseManagerId + "&victimId=" + newUser.ProviderUserKey.ToString();
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			if (!String.IsNullOrEmpty(eventId))
			{
				//Associate this user with their event
				UserEvent userEvent = new UserEvent();
				userEvent.EventId = new Guid(eventId);
				userEvent.UserEventId = Guid.NewGuid();
				userEvent.UserId = new Guid(newUser.ProviderUserKey.ToString());
				dc.UserEvents.InsertOnSubmit(userEvent);
				dc.SubmitChanges();
			}

			if (!String.IsNullOrEmpty(organizationId))
			{
				//Associate this user with their team
				UserOrganization userOrganization = new UserOrganization();
				userOrganization.UserOrganizationId = Guid.NewGuid();
				userOrganization.OrganizationId = new Guid(organizationId);
				userOrganization.UserId = new Guid(newUser.ProviderUserKey.ToString());
				dc.UserOrganizations.InsertOnSubmit(userOrganization);
				dc.SubmitChanges();
			}

			Roles.AddUserToRole(username, "Survivor");
			Roles.AddUserToRole(username, "Member");
			Guid newUserProfileId = Guid.NewGuid();
			//Create a profile for this user.
			Profile userProfile			= new Profile();
			userProfile.UserId			= new Guid(newUser.ProviderUserKey.ToString());
			userProfile.ProfileId		= newUserProfileId;
			userProfile.Firstname		= firstName;
			userProfile.Lastname		= lastName;
			userProfile.PhoneNumber		= phoneNumber;
			dc.Profiles.InsertOnSubmit(userProfile);
			dc.SubmitChanges();
			
			//Add user to user relationship for the person that added the survivor.
			if(!string.IsNullOrEmpty(caseManagerId))
			{
				var userUserRelationship = (from uur in dc.UserUserRelationships
											select new { uur.UserUserRelationshipId }).SingleOrDefault(); ;

				UserUser userUser = new UserUser();
				userUser.UserUserId = Guid.NewGuid();
				userUser.AcceptingUserId = new Guid(newUser.ProviderUserKey.ToString());
				userUser.RequestingUserId = new Guid(caseManagerId);
				userUser.IsActive = true;
				userUser.UserUserRelationshipId = userUserRelationship.UserUserRelationshipId;
				userUser.RequestedOn = DateTime.Now;
				userUser.AcceptedOn = DateTime.Now;
				dc.UserUsers.InsertOnSubmit(userUser);
				dc.SubmitChanges();
			}

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
			"~\\EmailTemplates\\CreateVictimAccount.html",
			out error);

			//Do not log user in if it's created by an advocate/case manager
			if(String.IsNullOrEmpty(caseManagerId))
			{ 
				// Log the user into the site
				FormsAuthentication.SetAuthCookie(username, true);
			}
			Response.Redirect(urlRedirect);
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