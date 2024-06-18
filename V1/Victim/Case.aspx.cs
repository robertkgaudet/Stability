using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Configuration;

public partial class CaseManagment_Case : BaseOrganizationWebForm
{
	public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
	public string profileImageStyle = string.Empty;
	public string googlePlacesId = string.Empty;
	public string addressId = string.Empty;

	public string _userId = String.Empty; //User that is going to be administrated in case management.
	public Guid _profileUserId = Guid.Empty;
	public string profileNumber = String.Empty;
	public string mapDomain = ConfigurationManager.AppSettings["mapDomain"].ToString();
	public string mapApiKey = ConfigurationManager.AppSettings["mapApiKey"].ToString();

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadPage();
	}

	public void LoadPage()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		_userId				= Request.QueryString["userId"]; //User is going to be administrated.
		profileNumber		= Request.QueryString["profileNumber"];
		litDateTime.Text	= DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
		
		//Make sure that only team members with case manager role or the person who requeted help can see this ticket.

		if (!String.IsNullOrEmpty(profileNumber))
		{
			//Get this users userId
			var userIdFromProfileNumber = (from p in dc.Profiles
										  where p.ProfileNumber == Int32.Parse(profileNumber)
										  select new {p.UserId}).SingleOrDefault();

			_userId = userIdFromProfileNumber.UserId.ToString();
		}

		hypAddHome.Visible = true;
		hypRemoveHome.Visible = false;

		//Get the disaster impacted
		var userEvent = (from ue in dc.UserEvents
						 join e in dc.Events on ue.EventId equals e.EventId
						 where ue.UserId == new Guid(_userId)
						 select new { e.Name }).Take(1).SingleOrDefault();
		if(userEvent != null)
		{ 
			litDisaster.Text = userEvent.Name;
		}
		var profileAddress = (from pa in dc.ProfileAddresses
							 join a in dc.Addresses on pa.AddressId equals a.AddressId
							  where pa.ProfileId == Guid.Parse(_userId)
							  select new { pa.AddressId, a.Address1, a.City, a.State, a.Zip, a.GooglePlaceId }).Take(1).SingleOrDefault();

		divAddHome.Visible = true;
		if (profileAddress != null)
		{
			addressId = profileAddress.AddressId.ToString();
			hypEditHomeInfo.NavigateUrl = "/CaseManagement/EditHome.aspx?userId=" + _userId + "&addressId=" + addressId;
			divAddHome.Visible = false;
			divHomeInfo.Visible = true;
			divMap.Visible = true;
			litAddAddress.Visible = false;
			googlePlacesId = profileAddress.GooglePlaceId;
			hypAddHome.Visible = false;
			hypRemoveHome.Visible = true;
			ddAddress.Visible = true;
			lblAddress.Text = profileAddress.Address1;
			lblCityStateZip.Text = profileAddress.City + ", " + profileAddress.State + " " + profileAddress.Zip;
		}

		litProfileNumber.InnerText = profileNumber;
		litSmallProfileNumber.Text = profileNumber;
		hypAddHome.NavigateUrl = "~/CaseManagement/AddLocation.aspx?userId=" + _userId;

		hypSurvivorPhotos.NavigateUrl = "~/S1/Profile/ItemSubTypePersonSurveyPhoto.aspx?userId=" + _userId;
        if (User.IsInRole("Administrator") || User.IsInRole("Vetting"))
		{
            hypDisasterRegistrySurvey.NavigateUrl = "~/V1/Surveys/Survey-Items.aspx?userId=" + _userId;
            
            //User id was sent to the page, so show the person who's id was supplied.
            if (!String.IsNullOrEmpty(_userId))
			{
				Guid userIdTemp = new Guid(_userId);

				if(userIdTemp == userId) //userId is the logged in user. _userId is the user being administrated.
				{
                    //This is the LOGGED IN ADMIN USER HE CAME IN WITH HIS ID ON A URL 
                    hypSurvivorPhotos.Visible = true;

                     if (new Guid(_userId) == userId)
					//ADMIN IS EDITING THEIR OWN PROFILE
					//Admin is editing their own profile! Show them the edit tools.
					_profileUserId			= userId;
					btnAddProfileImage.Visible = true;
                    divResetPassword.Visible = true;
                }
				else
				{
                    //ADMIN IS EDITING A different admins roles 
                    hypSurvivorPhotos.Visible = true;
                    _profileUserId			   = new Guid(_userId);
					btnAddProfileImage.Visible = false;
                    divResetPassword.Visible = true;
                }
			}
			else
			{
                //ADMIN User is on his own profile
                hypSurvivorPhotos.Visible = true;
                _profileUserId = userId;
				btnAddProfileImage.Visible = true;
                divResetPassword.Visible = true;
            }

			dtPhone.Visible				= true;
			ddPhone.Visible				= true;
			ddAddress.Visible			= true;
            ddEmail.Visible             = true;
            dtEmail.Visible             = true;
			divUserAccountAdministration.Visible = true;
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
            //USER IS the person who owns the profile is on it, not an admin user.
            hypSurvivorPhotos.Visible = true;
            _profileUserId = userId;
			dtPhone.Visible				= true;
			ddPhone.Visible				= true;
		}
		else if(!String.IsNullOrEmpty(_userId))
		{
			//A USER IS Viewing someone elses page.
			_profileUserId = new Guid(_userId);
			dtPhone.Visible = false;
			ddPhone.Visible = false;
			ddAddress.Visible = false;
		}

/*
        var itemSubTypePersonSurvey = (from istps in dc.ItemSubTypePersonSurveys
                                      where istps.UserId == new Guid(_userId)
                                      select istps).Take(1).SingleOrDefault();

        if(itemSubTypePersonSurvey != null)
        {
            hypDisasterRegistrySurvey.NavigateUrl = "/S1/WishList.aspx?userId=" + _userId;
            hypDisasterRegistrySurvey.Text = "View Disaster Recovery Wishlist";
            lblSurveyWishList.Text = "Wish List";
        }
		*/

        var profile = (from p in dc.Profiles
						where p.UserId == _profileUserId
						select p).SingleOrDefault();
		
		hypAmazon.Visible = true;
		if(!String.IsNullOrEmpty(profile.AmazonWishListURL))
		{
			hypAmazon.Text = "Donate From My Amazon Wish List";
			hypAmazon.NavigateUrl = profile.AmazonWishListURL;
			hypAmazon.Visible = true;
		}

		if(!String.IsNullOrEmpty(profile.PhoneNumber))
		{
			lblPhoneNumber.Text = Regex.Replace(profile.PhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
		}
		

		MembershipUser profileUser = Membership.GetUser(_profileUserId);
        lblEmailAddress.Text = profileUser.Email;
        lblUsername.Text = profileUser.UserName;
        if (!IsPostBack)
		{
			if(profileUser.IsApproved)
			{
				chkLockedOut.Checked = false;
			}
			else
			{
				chkLockedOut.Checked = true;
			}
			LoadCaseNotes(_profileUserId);
			LoadClientData(_profileUserId);
		}

		string[] userRoles = Roles.GetRolesForUser(profileUser.UserName);
		string roleType = string.Empty;
		foreach(string role in userRoles)
		{
			roleType += role.ToString() + ", ";
		}
		litName.Text = profile.Firstname + " " +  profile.Lastname;
		litTitleName.Text = profile.Firstname + " " + profile.Lastname;
		litCaseName.Text = profile.Firstname + " " + profile.Lastname;
		litRoles.Text = "<small>Roles: " + roleType.Substring(0, roleType.Length - 2) + "</small>";

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

	public void LoadClientData(Guid userId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var qualifiers = from uq in dc.UserQualifiers
					join rs in dc.RebuildStatus on uq.QualifierId equals rs.RebuildStatusId
					where rs.StatusType == 2 && uq.UserId == userId
					orderby rs.Status descending
					select new { rs.Status };

		rptqualifiers.DataSource = qualifiers.Distinct();
		rptqualifiers.DataBind();

		var basicNeeds = from ub in dc.UserBasicNeeds
						 join rs in dc.RebuildStatus on ub.RebuildStatusId equals rs.RebuildStatusId
						 where rs.StatusType == 3 && ub.UserId == userId
						 orderby rs.Status descending
						 select new { rs.Status };

		rptBasicNeeds.DataSource = basicNeeds.Distinct();
		rptBasicNeeds.DataBind();
	}

	public void LoadCaseNotes(Guid userId)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var posts = from n in dc.Notes
					join pn in dc.ProfileNotes on n.NoteId equals pn.NoteId
					join p in dc.Profiles on n.CreatedBy equals p.UserId
					where pn.UserId == userId
					orderby n.CreatedOn descending
					select new { p.ProfileId, n.Note1, n.CreatedOn, fullname = p.Firstname + " " + p.Lastname, ProfilePhoto = (p.Photo == null ? "Avatar.png" : p.Photo) };

		rptPosts.DataSource = posts;
		rptPosts.DataBind();
	}

	protected void btnRemoveHome_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		
		var profileAddress = (from pa in dc.ProfileAddresses
								join a in dc.Addresses on pa.AddressId equals a.AddressId
								where pa.ProfileId == Guid.Parse(_userId)
								select pa).Take(1).SingleOrDefault();

		var address = (from a in dc.Addresses where a.AddressId == profileAddress.AddressId select a).SingleOrDefault();

		dc.ProfileAddresses.DeleteOnSubmit(profileAddress);
		dc.Addresses.DeleteOnSubmit(address);
		dc.SubmitChanges();
		Response.Redirect("/case/" + profileNumber);
	}

	protected void btnAddProfileNote_Click(object sender, EventArgs e)
	{
		string post = txtPost.Text;
		if (!string.IsNullOrEmpty(post))
		{
			InsertProfileNote(post, _profileUserId, new Guid(Membership.GetUser().ProviderUserKey.ToString()));
			LoadCaseNotes(_profileUserId);
			txtPost.Text = string.Empty;
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
					where r.LoweredRoleName != "deleteadministrator"
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

		var nonProfits = from us in dc.UserOrganizations
					 join s in dc.Organizations on us.OrganizationId equals s.OrganizationId
					 where us.UserId == userId
					 orderby s.Name
					 select s;

		foreach(var nonProfit in nonProfits)
		{
			nonProfitList += "<a href=\"/V1/NonProfit/NonProfit.aspx?organizationId=" + nonProfit.OrganizationId.ToString() + "\" style=\"text-decoration:underline;\">" + nonProfit.Name + "</a><br/>";
		}

		return nonProfitList;
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

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(txtPassword.Text))
        {
            Guid _profileUserId = new Guid(hidProfileId.Value);
            MembershipUser membershipUser = Membership.GetUser(_profileUserId);
            membershipUser.UnlockUser();
            membershipUser.ChangePassword(membershipUser.ResetPassword(), txtPassword.Text);
        }
    }
}