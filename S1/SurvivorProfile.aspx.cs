using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class S1_SurvivorProfile : BaseOrganizationWebForm
{
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	public string profileImageStyle = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadPage();
	}

	public void LoadPage()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		string _userId			= Request.QueryString["userId"]; //User is going to be administrated.
		Guid _profileUserId		= Guid.Empty;
		string profileNumber	= Request.QueryString["profileNumber"];

		if(!String.IsNullOrEmpty(profileNumber))
		{
			//Get this users userId
			var userIdFromProfileNumber = (from p in dc.Profiles
										  where p.ProfileNumber == Int32.Parse(profileNumber)
										  select new {p.UserId}).SingleOrDefault();

			_userId = userIdFromProfileNumber.UserId.ToString();
		}

		if(User.IsInRole("Administrator") || User.IsInRole("Vetting"))
		{
			//ADMIN
			//User id was sent to the page, so show the person who's id was supplied.
			if(!String.IsNullOrEmpty(_userId))
			{
				Guid userIdTemp = new Guid(_userId);

				if(userIdTemp == userId) //requested matches logged in.
				{
					//This is the LOGGED IN ADMIN USER HE CAME IN WITH HIS ID ON A URL 
					 if(new Guid(_userId) == userId)
					//ADMIN IS EDITING THEIR OWN PROFILE
					//Admin is editing their own profile! Show them the edit tools.
					_profileUserId			= userId;
					divEditScreen.Visible	= true;
					divEditProfile.Visible	= true;
					btnAddProfileImage.Visible = true;
				}
				else
				{
					//ADMIN IS EDITING A different admins roles 
					_profileUserId			   = new Guid(_userId);
					divEditScreen.Visible	   = false;
					divEditProfile.Visible	   = false;
					btnAddProfileImage.Visible = false;
				}
			}
			else
			{
				//ADMIN User is on his own profile
				_profileUserId = userId;
				divEditScreen.Visible	= true;
				divEditProfile.Visible	= true;
				btnAddProfileImage.Visible = true;
			}

			dtPhone.Visible				= true;
			ddPhone.Visible				= true;
			ddAddress.Visible			= true;
			divUserAccountAdministration.Visible = true;
			divVolunteerStatus.Visible	= true;
			hidProfileId.Value			= _profileUserId.ToString();

			dtPhone.Visible				= true;
			ddPhone.Visible				= true;
			
			//Hide from vetting users
			if(!IsPostBack)
			{
				LoadRoleCheckboxList(_profileUserId);
			}
			divAdminTools.Visible		= true;
			divUserAccountAdministration.Visible = true;
		}
		else if(String.IsNullOrEmpty(_userId))
		{
			//the person who owns the profile is on it, not an admin user.
			_profileUserId = userId;
			dtPhone.Visible				= true;
			ddPhone.Visible				= true;
			divEditScreen.Visible		= true;
			divEditProfile.Visible		= true;
		}
		else if(!String.IsNullOrEmpty(_userId))
		{
			//Viewing someone elses page.
			_profileUserId = new Guid(_userId);
			dtPhone.Visible = false;
			ddPhone.Visible = false;
			ddAddress.Visible = false;
			divEditScreen.Visible = false;
			divEditProfile.Visible = false;
		}

		var profile = (from p in dc.Profiles
						where p.UserId == _profileUserId
						select p).SingleOrDefault();
		
		hypAmazon.Visible = false;
		if(!String.IsNullOrEmpty(profile.AmazonWishListURL))
		{
			hypAmazon.Text = "Donate From My Amazon Wish List";
			hypAmazon.NavigateUrl = profile.AmazonWishListURL;
			hypAmazon.Visible = true;
		}


		lblAddress.Text = profile.Address;
		lblCityStateZip.Text = profile.City + " " + profile.State + ", " + profile.Zip;
		lblZelloHandle.Text = profile.ZelloName;
		lblTitle.Text = profile.Title;
		if(!String.IsNullOrEmpty(profile.PhoneNumber))
		{
			lblPhoneNumber.Text = Regex.Replace(profile.PhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
		}
		lblDatesAvailable.Text = profile.DatesAvailable;
		lblNumberOfDaysAvailable.Text = profile.NumberOfDaysAvailable;
		lblVolunteerDescription.Text = profile.Description;
		lblSkills.Text = GetSkills(_profileUserId);
		lblDisasters.Text = GetEvents(_profileUserId);
		litNonProfits.Text = GetNonProfits(_profileUserId);
		
		this.Master.PageTitle			= "Stability - " + profile.Firstname + " " + profile.Lastname;
		this.Master.PageDescription		= profile.Firstname + " " + profile.Lastname + " Stability profile page.";
		this.Master.FbDescription		= profile.Firstname + " " + profile.Lastname + " Stability profile page.";
		this.Master.FbImage				= "Images/HurricaneMichael.jpg";
		this.Master.FbImageType			= "image/jpg";
		this.Master.FbSite_name			= "Stability - " +  profile.Firstname + " " + profile.Lastname + " On Facebook";
		this.Master.FbURL				= Request.Url.AbsoluteUri;

		MembershipUser profileUser = Membership.GetUser(_profileUserId);
		if(!IsPostBack)
		{
			CreateVolunteerStatusRadioButtons();
			if(profileUser.IsApproved)
			{
				chkLockedOut.Checked = false;
			}
			else
			{
				chkLockedOut.Checked = true;
			}
		}

		string[] userRoles = Roles.GetRolesForUser(profileUser.UserName);
		string roleType = string.Empty;
		foreach(string role in userRoles)
		{
			roleType += role.ToString() + ", ";
		}
		litName.Text = profile.Firstname + " " +  profile.Lastname + "<br/><small>Roles: " + roleType.Substring(0, roleType.Length-2) + "</small>";



		if(profile.VolunteerApplicationDate != null)
		{
			litVolunteerApplicationCompletedOn.Text = "Volunteer application completed " + GetElapsedTime(Convert.ToDateTime(profile.VolunteerApplicationDate)) + "</p>";
		}

		string volunteerStatus = VolunteerStatus.GetVolunteerStatus(_profileUserId).Value;

		if(volunteerStatus == VolunteerStatus.ApplicationComplete.Value)
		{
			//User has completed a volunteer application.
			litVolunteerStatus.Text = "<b>APPLICATION SUBMITTED</b><p>Your volunteer application is under review. (" + VolunteerStatus.VettingStarted.Value + ")</p>";
		}
		else if(volunteerStatus == VolunteerStatus.NotYetApplied.Value)
		{
			//User has not started application process
			litVolunteerStatus.Text = "<b>VOLUNTEERS NEEDED</b><p>Volunteer using the Volunteer link in the navigation menu. (" + VolunteerStatus.NotYetApplied.Value + ")</p>";
		}
		else if(volunteerStatus == VolunteerStatus.VettingComplete_Failed.Value)
		{
			//User failed vetting
			//Have them contact us.
			litVolunteerStatus.Text = "<b>APPLICATION PENDING REVIEW</b><p>Please contact Stability regarding your volunteer application.</p>";
		}
		else if(volunteerStatus == VolunteerStatus.VettingComplete_Passed.Value)
		{
			//User passed vetting.
			litVolunteerStatus.Text = "<b>APPLICATION APPROVED</b><p>Your volunteer application has been approved, welcome aboard! You might want to print an ID card or purchase a t-shirt to show your support! (" + VolunteerStatus.VettingComplete_Passed.Value + ")</p>";
		}
		else if(volunteerStatus == VolunteerStatus.VettingStarted.Value)
		{
			//User passed vetting.
			litVolunteerStatus.Text = "<b>APPLICATION BEING VETTED</b><p>Your volunteer application is currently being reviewed. (" + VolunteerStatus.VettingStarted.Value + ")</p>";
		}

		if(profile.VettedBy != null)
		{
			Guid vettingProfileUserId = (Guid)profile.VettedBy;
			Profile vettingUsersProfile = GetUserProfileByUserId(vettingProfileUserId);

			DateTime vettingUpdatedOn = new DateTime();
			if(profile.DateVettingCompleted != null)
			{
				vettingUpdatedOn = (DateTime)profile.DateVettingCompleted;
			}
			else if(profile.DateVettingStarted != null)
			{
				vettingUpdatedOn = (DateTime)profile.DateVettingStarted;
			}
			
			lblVolunteerReviewStatus.Text = "<p>This volunteers application review was last updated by " + vettingUsersProfile.Firstname + " " + vettingUsersProfile.Lastname + " " + GetElapsedTime(vettingUpdatedOn) + ".</p>" + 
				"<p><i>Last Note:</i> " + profile.VettingNotes + "</p>";
			divVolunteerVettingMessage.Visible = true;
		}

		//If no USERID then we are looking at our own profile.

		//If there is a userID then we're looking at that users profile, so use it.
		
		//If user has a profile photo, use it.
		var profilePhoto = (from ph in dc.Photos
							join pr in dc.ProfilePhotos on ph.PhotoId equals pr.PhotoId
							where pr.UserId == _profileUserId
							orderby ph.CreatedOn descending
							select new {ph.FilenameCropped }).Take(1).SingleOrDefault();

		if(profilePhoto != null)
		{
			profileImageStyle = imgProfile.ClientID + "{width:100%;}";
			imgProfile.Src = profilePhotoFolder + profilePhoto.FilenameCropped;
			this.Master.FbImage				= profilePhotoFolder + profilePhoto.FilenameCropped;
			this.Master.FbSite_name			= "Stability - " + profile.Firstname + " " + profile.Lastname + " Profile Page";
		}
	}
	
	protected string GetSkills(Guid userId)
	{
		string skillList = string.Empty;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var skills = from us in dc.UserSkills
					 join s in dc.Skills on us.SkillId equals s.SkillId
					 where us.UserId == userId
					 orderby s.Name
					 select s;

		skillList = string.Join(", ", skills.Select(p => p.Name.ToString()));

		return skillList;
	}

	protected void LoadRoleCheckboxList(Guid profileUserId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var roles = from r in dc.aspnet_Roles
					orderby r.RoleName
					select new {r.RoleName, r.RoleId };

		chkRoles.DataSource = roles;
		chkRoles.DataBind();

		//Get this users roles.
		var userRoles = from ur in dc.aspnet_UsersInRoles
						where ur.UserId == profileUserId
						select new {ur.RoleId };

		if(userRoles.Count() > 0)
		{
			foreach(var userRole in userRoles)
			{
				for (int i = 0; i < chkRoles.Items.Count; i++)
				{
					if (chkRoles.Items[i].Value == userRole.RoleId.ToString())
					{
						chkRoles.Items[i].Selected = true;
					}
				}
			}
		}
	}

	protected string GetEvents(Guid userId)
	{
		string eventList = string.Empty;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var events = from us in dc.UserEvents
					 join s in dc.Events on us.EventId equals s.EventId
					 where us.UserId == userId
					 orderby s.Name
					 select s;


		foreach(var eventItem in events)
		{
			eventList += "<a href=\"/Disaster/" + eventItem.URLFriendlyName + "\" style=\"text-decoration:underline;\">" + eventItem.Name + "</a><br/>";
		}
		//eventList = string.Join(", ", events.Select(p => p.Name.ToString()));

		return eventList;
	}

	protected string GetNonProfits(Guid userId)
	{
		string nonProfitList = string.Empty;
		
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var nonProfits = from uo in dc.UserOrganizations
					 join s in dc.Organizations on uo.OrganizationId equals s.OrganizationId
					 where uo.UserId == userId && uo.IsEnabled == true
                         orderby s.Name
					 select s;

		foreach(var nonProfit in nonProfits)
		{
			nonProfitList += "<a href=\"/V1/NonProfit/Default.aspx?organizationId=" + nonProfit.OrganizationId.ToString() + "\" style=\"text-decoration:underline;\">" + nonProfit.Name + "</a><br/>";
		}

		return nonProfitList;
	}

	public void CreateVolunteerStatusRadioButtons()
	{
		ListItem vettingStartedLI = new ListItem("Start Vetting", VolunteerStatus.VettingStarted.Value);
		ListItem vettingPassedLI = new ListItem("Passed Vetting", VolunteerStatus.VettingComplete_Passed.Value);
		ListItem vettingFailedLI = new ListItem("Failed Vetting", VolunteerStatus.VettingComplete_Failed.Value);

		rblUserStatus.Items.Add(vettingStartedLI);
		rblUserStatus.Items.Add(vettingPassedLI);
		rblUserStatus.Items.Add(vettingFailedLI);
	}

	protected void bthSubmitRoles_Click(object sender, EventArgs e)
	{
		Guid _profileUserId = new Guid(hidProfileId.Value);	//userId of the person on the profile Page
		Guid _vettingUserId = userId;	//userId of the person doing thew work on users profile.

		//Update the vetting status.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		MembershipUser profileUser = Membership.GetUser(_profileUserId);

		if(chkLockedOut.Checked)
		{
			profileUser.IsApproved = false;
			Membership.UpdateUser(profileUser);
		}
		else
		{
			profileUser.IsApproved = true;
			Membership.UpdateUser(profileUser);
		}

		foreach (ListItem item in chkRoles.Items)
		{
			//IN THE ROLE Is this user in the this role?
			var userCheck = from r in dc.aspnet_UsersInRoles
							where r.UserId == _profileUserId &&  r.RoleId == new Guid(item.Value)
							select r;

			if (item.Selected)
			{
				//If not then add them
				if (userCheck.Count() == 0)
				{ 
					aspnet_UsersInRole userInRoles = new aspnet_UsersInRole();
					userInRoles.RoleId = new Guid(item.Value);
					userInRoles.UserId = _profileUserId;
					dc.aspnet_UsersInRoles.InsertOnSubmit(userInRoles);
					dc.SubmitChanges();
				}
			}
			else
			{
				//If item is selected then unselect it.

				//Delete any checked records
				if (userCheck.Count() > 0)
				{
					//Item is selected.
					foreach(var userChecked in userCheck)
					{
						dc.aspnet_UsersInRoles.DeleteOnSubmit(userChecked);
						dc.SubmitChanges();
					}
				}
			}
		}
	}

	protected void btnSumbit_Click(object sender, EventArgs e)
	{
		Guid _profileUserId = new Guid(hidProfileId.Value);	//userId of the person on the profile Page
		Guid _vettingUserId = userId;	//userId of the person doing thew work on users profile.

		//Update the vetting status.
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var profile = (from p in dc.Profiles
					  where p.UserId == _profileUserId
					  select p).SingleOrDefault();

		profile.VettingNotes = txtVettingNotes.Text;
		profile.VettedBy = _vettingUserId;
		
		string vettingStatus = rblUserStatus.SelectedValue;
		switch (vettingStatus)
		{
			case "VettingStarted":
				{
					profile.DateVettingStarted = DateTime.Now;
					profile.VettingActive = true;
					dc.SubmitChanges();
				}
				break;
			case "VettingComplete_Failed":
				{
					profile.DateVettingCompleted = DateTime.Now;
					profile.VettingActive = false;
					profile.VettingComplete = true;
					profile.PassedVetting = false;
					dc.SubmitChanges();
				}
				break;
			case "VettingComplete_Passed":
				{
					profile.DateVettingCompleted = DateTime.Now;
					profile.VettingActive = false;
					profile.VettingComplete = true;
					profile.PassedVetting = true;
					dc.SubmitChanges();
				}
				break;
		}
		LoadPage();
	}
}