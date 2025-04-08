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
			Response.Redirect("/V1/Member/Default.aspx");
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
		string urlRedirect = "/Survivor";
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
		string addressData = hidAddressData.Value;
		string passwordAnswer = "12:00";
		string email = txtEmail.Text;
		string urlRedirect = string.Empty;
		string address = txtAddress.Text;
		string city = txtCity.Text;
		string state = ddlState.Value;
		string zipCode = txtZipCode.Text;
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
				userOrganization.OrganizationId = new Guid(organizationId);
				userOrganization.UserId = new Guid(newUser.ProviderUserKey.ToString());
				dc.UserOrganizations.InsertOnSubmit(userOrganization);
				dc.SubmitChanges();
				urlRedirect = "/V1/NonProfit/Default.aspx?organizationId=" + organizationId;
			}
			else
			{
				//Send to the team list page and ask that they choose a team.
				urlRedirect = "/V1/NonProfit/TeamList.aspx?team=false";
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

			//Create a profile for this user.
			Profile userProfile = new Profile();
			userProfile.UserId = new Guid(newUser.ProviderUserKey.ToString());
			userProfile.ProfileId = Guid.NewGuid();
			userProfile.Firstname = firstName;
			userProfile.Lastname = lastName;
			userProfile.PhoneNumber = phoneNumber;
			userProfile.Address = address;
			userProfile.City = city;
			userProfile.State = state;
			userProfile.Zip = zipCode;
			userProfile.ReceiveDeploymentSMS = deploymentSMS;
			dc.Profiles.InsertOnSubmit(userProfile);
			dc.SubmitChanges();


			#region save address 
			var addressList = addressData.Split('|');
			var duplicateAddress = dc.Addresses.FirstOrDefault(f => f.GooglePlaceId == addressList[10]);
			if (duplicateAddress == null)
			{
				var duplicateCounty = dc.Counties.FirstOrDefault(f => f.Name == addressList[9]); 
				var countyId = Guid.NewGuid();
				if (duplicateCounty == null)
				{
					var states = dc.USStates.FirstOrDefault(f => f.Code == state);
					County county = new County();
					county.CountyId = countyId;
					county.Name = addressList[9];
					county.StateId = states != null ? states.StatesId : new Guid();
					county.State = state;
					dc.Counties.InsertOnSubmit(county);
					dc.SubmitChanges();
				}
				else
				{
					countyId = duplicateCounty.CountyId;
				}

				var duplicateCity = dc.Cities.FirstOrDefault(f => f.City1 == city);
				var cityId = Guid.NewGuid();
				if (duplicateCity == null)
				{
					City cit = new City();
					cit.CityId = cityId;
					cit.City1 = city;
					cit.Code = addressList[14];
					dc.Cities.InsertOnSubmit(cit);
					dc.SubmitChanges();
				}
				else
				{
					cityId = duplicateCity.CityId;
				}

				Address adres = new Address();
				adres.AddressId = Guid.NewGuid();
				adres.GooglePlaceId = addressList[10];
				adres.FormattedAddress = addressList[11];
				adres.StreetNumber = addressList[3];
				adres.StreetName = addressList[4];
				adres.Address1 = address;
				adres.City = city;
				adres.State = state;
				adres.Zip = zipCode;
				adres.Country = addressList[7];
				adres.County = addressList[9];
				adres.Latitude = addressList[1];
				adres.Longitude = addressList[2];
				adres.IsActive = true;
				adres.CreatedOn = DateTime.Now;
				adres.CreatedBy = userProfile.UserId;
				adres.CountyId = countyId;
				adres.CityId = cityId;
				adres.LocationType = addressList[13];
				dc.Addresses.InsertOnSubmit(adres);
				dc.SubmitChanges();

				ProfileAddress proadres = new ProfileAddress();
				proadres.ProfileAddressId = Guid.NewGuid();
				proadres.ProfileId = userProfile.ProfileId;
				proadres.AddressId = adres.AddressId;
				proadres.HomeTypeId = new Guid("C478785A-014D-4DFF-86FE-3693E6F33FAC");
				proadres.HomeRelationshipOwnRentTypeId = new Guid("58526C73-5469-4B5E-81B1-831476784C56");
				proadres.IsPrimaryResidence = false;
				proadres.HasFloodInsurance = false;
				proadres.HasHomeownersInsurance = false;
				proadres.ShowOnAgencyMap = false;
				proadres.ShowOnCleanupMap = false;
				proadres.IsMultistory = false;
				proadres.HasBasement = false;
				proadres.HasGarage = false;
				proadres.HasCarport = false;
				proadres.HasCrawlspace = false;
				dc.ProfileAddresses.InsertOnSubmit(proadres);
				dc.SubmitChanges();
			}
			#endregion
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

	//public void LoadDisasters()
	//{
	//	CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
	//	var disasters = from d in dc.Events
	//					orderby d.BeginDate descending
	//					where d.IsActive == true
	//					select new {d };

	//	int idNumber = 0;
	//	foreach(var disaster in disasters)
	//	{
	//		string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
	//		disasterDropDown = disasterDropDown + "<li id=\"" + disaster.d.EventId + "\"><a href=\"#\">" + disaster.d.Name + " - " + disasterDate +  " Community Portal</a></li>" + Environment.NewLine;
	//		idNumber = idNumber + 1;
	//	}
	//	if(!String.IsNullOrEmpty(eventId))
	//	{
	//		//Hide the Dropdown and show the selected disaster
	//		var disaster = (from d in dc.Events
	//						where d.EventId == new Guid(eventId)
	//						orderby d.BeginDate descending
	//						select new {d}).SingleOrDefault();

	//		string disasterDate = String.Format("{0:Y}", disaster.d.BeginDate);
	//		preselectedDisasterJQuery = "$(\"#btn-dropdown.disasterEvent\").html('" + disaster.d.Name + " - " + disasterDate + "');";
	//		hidEventId.Value = eventId;
	//	}
	//}
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