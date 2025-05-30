using System;
using System.Linq;
using System.Web.Security;
using CrowdRelief;
using System.Web.UI.WebControls;
using System.IdentityModel.Metadata;

public partial class V1_Cajun_Navy_Page_Register : BaseWebForm
{
	public string disasterDropDown	= string.Empty;
	public string nonProfitDropDown = string.Empty;
	public string preselectedDisasterJQuery = string.Empty;
	public string preselectedNonProfitJQuery = string.Empty; 
	public string eventId = string.Empty;
	public string organizationId = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		Master.PageTitle = "Create Your Own Cajun Navy Disaster Relief Community";
		Master.PageDescription = "Gather your friends and family together to help your community to recovery from natural disasters.";
		Master.FbImage = "V1/Images/StabilityFBMetaTagImage.png";
		Master.FbImageType = ".jpg";

		if(User.Identity.IsAuthenticated)
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			//note, user might have more than org... 
			var userOrganizationOwner = (from uo in dc.UserOrganizations
										 join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
										 where o.OwnerId == userId && uo.Status== (int)RequestStatus.Approved
                                         orderby o.CreatedOn descending
										 select new { uo.OrganizationId }).Take(1).SingleOrDefault();

			if (userOrganizationOwner != null)
			{
				Response.Redirect("/V1/NonProfit/Default.aspx?OrganizationId=" + userOrganizationOwner.OrganizationId.ToString());
			}
		}

		if(!IsPostBack)
		{
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

			Roles.AddUserToRole(username, "Member");
			Roles.AddUserToRole(username, "Helper");
			Roles.AddUserToRole(username, "Volunteer");
			Roles.AddUserToRole(username, "NonProfitAdministrator");

			urlRedirect = "/V1/NonProfit/NonProfitNew.aspx";
			//Tools.SendEmail(firstName, lastName, "~\\EmailTemplates\\CreateAccount.html", "Welcome to Crowd Relief", email, this);

			// Log the user into the site
			FormsAuthentication.SetAuthCookie(username, true);
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