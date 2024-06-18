using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Activities.Expressions;
using System.Web.Caching;

public partial class MasterPages_Homer : System.Web.UI.MasterPage
{
	public string _pageTitle = string.Empty;
	public string _pageDescription = string.Empty;
	public string _fbImage = string.Empty;
	public string _fbURL = string.Empty;
	public string _fbImageType = string.Empty;
	public string _fbSite_name = string.Empty;
	public string _fbDescription = string.Empty;
	public bool _hideHeader = false;
	public bool _hideFooter = false;
	public bool _hideMenu = false;
	public string bodyTag = string.Empty;
	public string profileURL = string.Empty;
	public string fixedFooter = "fixed-footer";
	public string fixedHeader = "fixed-header";
	public string _userOrganizationEventId = string.Empty;
	public string _organizationEventId = string.Empty;
	public string _volunteerTypes;
	public string _timeActive = "false";
	public string _cause = string.Empty;
	public string _noCause = "false";
	public Guid userId = Guid.NewGuid();
	public string _showUserActionModal = string.Empty;
	public string communityUpdated		= "grey";
	public string skillsUpdated			= "grey";
	public string teamUpdated			= "grey";
	public string deploymentUpdated		= "grey";

	protected void Page_Load(object sender, EventArgs e)
	{
		form1.Action = HttpContext.Current.Request.RawUrl;
		divLogin.Visible = true;
		divSettings.Visible = false;
		litVolunteerPending.Text = " Volunteer";
		litVolunteerIcon.Text = "<i class=\"fa fa-heart\"></i>";

		string userActionModal = Request.QueryString["userActionModal"];
		
		if(_hideMenu)
		{
			menu.Visible = false;
		}
		if(_hideHeader)
		{
			fixedHeader = string.Empty;
			header.Visible = false;
		}
		if(_hideFooter)
		{
			fixedFooter = string.Empty;
			footer.Visible = false;
		}

		FbImage = String.IsNullOrEmpty(_fbImage) ? "V1/Images/MSTEAM.png" : _fbImage;
		FbImageType = String.IsNullOrEmpty(FbImageType) ? "image/png" : _fbImageType;
		PageTitle = String.IsNullOrEmpty(PageTitle) ? System.Configuration.ConfigurationManager.AppSettings["Title"].ToString() : _pageTitle;
		PageDescription = String.IsNullOrEmpty(PageDescription) ? System.Configuration.ConfigurationManager.AppSettings["Description"].ToString() : _pageDescription;
		FbSite_name = String.IsNullOrEmpty(FbSite_name) ? System.Configuration.ConfigurationManager.AppSettings["Title"].ToString() : _fbSite_name;
		FbDescription = String.IsNullOrEmpty(FbDescription) ? System.Configuration.ConfigurationManager.AppSettings["Description"].ToString() : _fbDescription;

		fbTitle.Attributes.Add("content", PageTitle);
		fbImage.Attributes.Add("content", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + FbImage);
		fbURL.Attributes.Add("content", FbURL);
		fbImageType.Attributes.Add("content", FbImageType);
		fbSite_name.Attributes.Add("content", FbSite_name);
		fbDescription.Attributes.Add("content", FbDescription);
		title.Text = PageTitle;
		description.Attributes.Add("content", PageTitle);

		//activeStatus.Attributes.Add("checked", "checked");
		if (HttpContext.Current.User.Identity.IsAuthenticated)
		{
			userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
			string profilePhotoFolder	= System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
			divSettings.Visible = true;
			divLogin.Visible = false;

			//Get the users information.
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var volunteerTypes = from vt in dc.TaskTypes
								orderby vt.Name
								select vt;

			foreach(var volunteerType in volunteerTypes)
			{
				_volunteerTypes += "<li id=\"" + volunteerType.TaskTypeId + "\"><a href=\"#\"><i class=\"pe-7s-id\"></i> " + volunteerType.Name.ToUpper() + "</a></li>" + Environment.NewLine;
			}
			litVolunteerType.Text = _volunteerTypes;
			var profile = (from p in dc.Profiles
						where p.UserId == userId
						select new {p }).SingleOrDefault();

            string roleType = string.Empty;
			bool hasDefaultDisaster = false;
            if (profile != null && profile.p.DefaultEventId != null)
            {
                //Get the default disaster
                var disasterEvent = (from d in dc.Events
                                     where d.EventId == profile.p.DefaultEventId
                                     select new { d.Name, d.URLFriendlyName }).SingleOrDefault();

				if(disasterEvent != null)
				{
					hasDefaultDisaster = true;
					communityUpdated = "yellowgreen";
					//Put the default disaster at the top.
					hypMDefaultDisaster.Text = "<b>" + disasterEvent.Name + "</b>";
					hypMDefaultDisaster.NavigateUrl = "/Disaster/" + disasterEvent.URLFriendlyName;
					hypDefaultDisaster.Text = "<b>" + disasterEvent.Name + "</b>";
					hypDefaultDisaster.NavigateUrl = "/Disaster/" + disasterEvent.URLFriendlyName;
					litDefaultDisaster.Text = "<li><strong><a href='/Disaster/" + disasterEvent.URLFriendlyName + "'>" + disasterEvent.Name + "</a> (Default Portal)</strong></li>";
				}
            }

            var orgUser = from o in dc.Organizations
						   join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                            where uo.UserId == userId
                            orderby o.CreatedOn descending
                            select o;

			if (orgUser.Count() > 0)
			{
				teamUpdated = "yellowgreen";
				hypActionPage.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId;
				//Person is owner of a non-profit.
				litNonProfitName.Visible = true;
                hypNonProfit.Text = orgUser.Take(1).SingleOrDefault().Name;
                hypNonProfit.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId;
				hypMyTeam.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId;
				hypMMyTeam.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId;
				liDeployment.Attributes.Add("data-url", "/V1/NonProfitAdministration/RespondToEvent.aspx?userActionModal=false&organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId);

				hypInviteTeamMembers.NavigateUrl = "/V1/NonProfitAdministration/InviteTeam.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId;

				var myDisasterCampaigns = from uoe in dc.UserOrganizationEvents
										  join oe in dc.OrganizationEvents on uoe.OrganizationEventId equals oe.OrganizationEventId
										  where oe.OrganizationId == orgUser.Take(1).SingleOrDefault().OrganizationId && uoe.UserId == userId
										  select new { oe };
				
				liMyCampaigns.Visible = false;
				string myDisasterCampaignList = string.Empty;
				foreach (var myDisasterCampaign in myDisasterCampaigns)
				{
					myDisasterCampaignList += "<li><a href=\"/Cause/" + myDisasterCampaign.oe.URLFriendlyCampaignName + "\">" + myDisasterCampaign.oe.CampaignName + "</a></li>";
				}
				litMyDisasterCampaigns.Text = myDisasterCampaignList;
			}
			else
            {
				hypInviteTeamMembers.Visible =	false;
				hypNonProfit.Visible = false;
				hypNonProfit.Target = "_blank";
            }

            var orgOrgUsers = from oe in dc.OrganizationEvents
                              where oe.PointOfContactUserId == userId
                              orderby oe.CreatedOn descending
                              select oe;

            if (orgOrgUsers.Count() > 0)
            {
                liDisasterCampaigns.Visible = true;
                string myDisasterList = string.Empty;
                foreach(var orgOrgUser in orgOrgUsers)
                {
                    myDisasterList += "<li><a href=\"/V1/NonProfit/Default.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId + "\">" + orgUser.Take(1).SingleOrDefault().Name + "</a></li>";
                }
                litMyCampaigns.Text = myDisasterList;
            }

			var userOrganizations = from uo in dc.UserOrganizations
                                    join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
                                    where uo.UserId == userId
                                    select new { o.Name, o.OrganizationId };
            
            if (userOrganizations.Count() > 0)
			{
				liMyNonProfits.Visible = false;
                string myOrganizationList = string.Empty;
                foreach (var userOrganization in userOrganizations)
                {
                    myOrganizationList += "<li><a href=\"/V1/NonProfit/Default.aspx?organizationId=" + userOrganization.OrganizationId + "\">" + userOrganization.Name + "</a></li>";
                }
                litMyNonProfits.Text = myOrganizationList;
            }

			//Has the user selected a cause?
			var userOrganizationEvents = from uoe in dc.UserOrganizationEvents
										 join oe in dc.OrganizationEvents on uoe.OrganizationEventId equals oe.OrganizationEventId
										 join c in dc.Counties on oe.StagingCountyId equals c.CountyId
										where uoe.UserId == userId && uoe.DeactivatedOn == null
										orderby uoe.CreatedOn descending
										select new { uoe, oe, c };

			var causes = from oe in dc.OrganizationEvents
							where oe.IsActive == true
							orderby oe.CampaignName
							select oe;

			foreach (var cause in causes)
			{
				deploymentUpdated = "yellowgreen";
				_cause += "<li id=\"" + cause.OrganizationEventId + "\"><a href=\"#\"><i class=\"fa fa-globe\"></i> " + cause.CampaignName + "</a></li>" + Environment.NewLine;
			}
			litCause.Text = _cause;
			litCauseB.Text = _cause;

			lblCause.Text = "Choose a Deployment to track your time.";
			string myCauseList = string.Empty;
			if (userOrganizationEvents.Count() == 0)
			{
				//NO CAUSE
				_noCause = "true";
			}
			else
			{
				//TIMESHEET
				deploymentUpdated = "yellowgreen";
				lblCause.Text = "<strong>Your Deployment: </strong>" + userOrganizationEvents.Take(1).SingleOrDefault().oe.CampaignName;
				_userOrganizationEventId = userOrganizationEvents.Take(1).SingleOrDefault().uoe.UserOrganizationEventId.ToString();
				lblCountyState.Text = userOrganizationEvents.Take(1).SingleOrDefault().c.State + ", (" + userOrganizationEvents.Take(1).SingleOrDefault().c.Name + " County)";
				litCountyHeader.Text = userOrganizationEvents.Take(1).SingleOrDefault().c.Name + " County";

				foreach (var userOrganizationEvent in userOrganizationEvents)
				{
					//List their causes on the Assistant Button.
					myCauseList += "<li><a href=\"/Cause/" + userOrganizationEvent.oe.URLFriendlyCampaignName + "\">" + userOrganizationEvent.oe.CampaignName + "</a></li>" + Environment.NewLine;
				}
			}
			litMyCauses.Text = myCauseList;

			var timesheet = (from t in dc.Timesheets
			where t.UserId == userId
			orderby t.TimeIn descending
								select t).Take(1).SingleOrDefault();

			if (timesheet != null)
			{
				//Update the Active Status button.
				if (timesheet.TimeIn.Date > DateTime.Today && timesheet.TimeOut == null)
				{
					//user forgot to logout, log them out at midnight on the day they forgot.
					TimeSpan midnight = new TimeSpan(23, 59, 0);
					string missingDateTime = timesheet.TimeIn.Date.ToLongDateString();
					timesheet.TimeOut = timesheet.TimeIn.Date + midnight;
					dc.SubmitChanges();
				}

				//is user logged in.
				if (timesheet.TimeOut == null && timesheet.TimeIn > DateTime.Now.AddDays(-1))
				{
					//show all of the sign out info;
					_timeActive = "true";
				}
			}

			var disasters = from ev in dc.Events
							join ue in dc.UserEvents on ev.EventId equals ue.EventId
							where ue.UserId == userId
							orderby ev.BeginDate
							select new { ev.EventId, ev.Name, ev.URLFriendlyName, ev.BeginDate };

			if (disasters.Count() > 0)
			{
				string myDisasterList = string.Empty;
				foreach (var disaster in disasters.Distinct().OrderByDescending(d => d.BeginDate))
				{
					myDisasterList += "<li><a href=\"/Disaster/" + disaster.URLFriendlyName + "\">" +disaster.Name + "</a></li>";
				}
				communityUpdated = "yellowgreen";
				litMyDisasters.Text = myDisasterList;
				if(!hasDefaultDisaster)
				{
					hypMDefaultDisaster.Text = "<b>" + disasters.Take(1).SingleOrDefault().Name + "</b>";
					hypMDefaultDisaster.NavigateUrl = "/Disaster/" + disasters.Take(1).SingleOrDefault().URLFriendlyName;
					hypDefaultDisaster.Text = "<b>" + disasters.Take(1).SingleOrDefault().Name + "</b>";
					hypDefaultDisaster.NavigateUrl = "/Disaster/" + disasters.Take(1).SingleOrDefault().URLFriendlyName;
					litDefaultDisaster.Text = "<li><strong><a href='/Disaster/" + disasters.Take(1).SingleOrDefault().URLFriendlyName + "'>" + disasters.Take(1).SingleOrDefault().Name + "</a> (Default Portal)</strong></li>";
				}
			}

			//List nonprofits a user volunteers for.
			var profileImage = (from ph in dc.ProfilePhotos
			join p in dc.Photos on ph.PhotoId equals p.PhotoId
			where ph.UserId == userId && ph.IsCurrrent == true
			orderby p.CreatedOn descending
			select new { p.FilenameCropped }).Take(1).SingleOrDefault();

			if(profileImage != null)
			{
				//Get the users profile image
				imgProfile.Src = profilePhotoFolder + profileImage.FilenameCropped;
			}

   //         string[] userRoles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);

   //         roleType = string.Empty;
   //         foreach (string role in userRoles)
			//{
			//	roleType += role.ToString() + "<br/>";
			//}
			if(HttpContext.Current.User.IsInRole("survivor"))
			{
				profileURL = "'/S1/Profile/Default.aspx'";
				//roleType = "Survivor";
			}
			else if(HttpContext.Current.User.IsInRole("helper"))
			{
				profileURL = "'/V1/Profile/Profile.aspx'";
				//roleType = "Helper";
			}
			else if(HttpContext.Current.User.IsInRole("volunteer"))
			{
				profileURL = "'/V1/Profile/Profile.aspx'";
				//roleType = "Survivor";
			}
			else
			{
				profileURL = "'/V1/Profile/Profile.aspx'";
				//roleType = "Member";
			}

			BaseOrganizationWebForm baseWebForm = new BaseOrganizationWebForm();

			if (profile == null)
			{
				System.Web.Security.FormsAuthentication.SignOut();
				Session.Abandon();
				Response.Redirect("/SignIn");
			}
            litUsername.Text = profile.p.Firstname + " " + profile.p.Lastname + "<br/>";

			string volunteerStatus = VolunteerStatus.GetVolunteerStatus(userId).Value;
			if(volunteerStatus == VolunteerStatus.ApplicationComplete.Value)
			{
				//Change the Volunteer button to say volunteer pending.
				litVolunteerPending.Text = " Volunteer Pending";
				hypVolunteer.NavigateUrl = "~/V1/Profile/Profile.aspx";
				litVolunteerIcon.Text = "<i class=\"fa fa-exclamation-circle\"></i>";
			}
			else if(volunteerStatus == VolunteerStatus.VettingComplete_Failed.Value)
			{
				litVolunteerPending.Text = " Volunteer Pending";
				hypVolunteer.NavigateUrl = "~/V1/Profile/Profile.aspx";
				litVolunteerIcon.Text = "<i class=\"fa fa-exclamation-circle\"></i>";
			}
			else if(volunteerStatus == VolunteerStatus.VettingComplete_Passed.Value)
			{
				litVolunteerPending.Text = " Find Volunteer Opportunities";
				hypVolunteer.NavigateUrl = "~/V1/Stream.aspx";
				litVolunteerIcon.Text = "<i class=\"fa fa-heart\"></i>";
				liPrintIdCard.Visible = true;
			}
			else if(volunteerStatus == VolunteerStatus.VettingStarted.Value)
			{
				litVolunteerPending.Text = " Volunteer Under Review";
				hypVolunteer.NavigateUrl = "~/V1/Profile/Profile.aspx";
				litVolunteerIcon.Text = "<i class=\"fa fa-exclamation-circle\"></i>";
			}
			else
			{
				litVolunteerPending.Text = " Volunteer";
				litVolunteerIcon.Text = "<i class=\"fa fa-heart\"></i>";
			}


			//Get the current page name and set bold in navigation.
			string pageName = Page.ToString().ToLower().Replace("_",".").Replace("asp.v1.profile.","").Replace("asp.v1.","");

			switch (pageName)
			{
				case "default.aspx":
					{
						//If user only has one home, then bypass the list and send them to it.
						var rebuildCount = from r in dc.Rebuilds
										   where r.CreatedBy == userId
										   select r;

						if(rebuildCount.Count() == 1)
						{
							Response.Redirect("Rebuild.aspx?rebuildId=" + rebuildCount.First().RebuildId.ToString());
						}
						litTrackHome.Text = "class=\"active\"";
					}
					break;
				case "chooseevent.aspx":
					{
						litAddHome.Text= "class=\"active\"";
						litViewDisasters.Text= "class=\"active\"";
					}
					break;
				case "time.aspx":
					{
						//litRecordTime.Text= "class=\"active\"";
					}
					break;
				case "stream.aspx":
					{
						litStream.Text= "class=\"active\"";
					}
					break;
				case "stages.aspx":
					{
						litStages.Text= "class=\"active\"";
					}
					break;
				default:
					{
					}
					break;
			}

			if (userActionModal == "false" && !IsPostBack)
			{
				//Hide the user action modal.
				_showUserActionModal = "";
			}
			else
			{
				_showUserActionModal = "$(\"#divUserActionModal\").modal('show')";
			}
			if (CheckSkills(userId))
			{
				skillsUpdated = "yellowgreen";
			}

			if (communityUpdated == "yellowgreen" && skillsUpdated == "yellowgreen" && teamUpdated == "yellowgreen" && deploymentUpdated == "yellowgreen")
			{
				_showUserActionModal = "";
			}
		}
		else
		{
			//User is not signed in.
			_showUserActionModal = "";
			timeButtons.Visible = false;
			myDisastersButton.Visible = false;
		}
		if(HttpContext.Current.User.IsInRole("Administrator") || HttpContext.Current.User.IsInRole("Vetting"))
		{
			divAdminLinks.Visible = true;
		}
		if (HttpContext.Current.User.IsInRole("LocationAdministrator") || HttpContext.Current.User.IsInRole("LocationManager"))
		{
			divLocationAdministration.Visible = true;
		}
	}

	private bool CheckSkills(Guid userId)
	{
		bool hasSkills = false;
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var skillCheck = (from us in dc.UserSkills
						 where us.UserId == userId
						 select us).Count();

		if(skillCheck > 0)
		{
			hasSkills = true;
		}

		return hasSkills;
	}
	
	public bool HideMenu
	{
		get
		{
			return _hideMenu;
		}
		set
		{
			_hideMenu = value;
		}
	}
	public bool HideFooter
	{
		get
		{
			return _hideFooter;
		}
		set
		{
			_hideFooter = value;
		}
	}
	public bool HideHeader
	{
		get
		{
			return _hideHeader;
		}
		set
		{
			_hideHeader = value;
		}
	}

	public string PageTitle
	{
		get
		{
			return _pageTitle;
		}
		set
		{
			_pageTitle = value;
		}
	}
	public string PageDescription
	{
		get
		{
			return _pageDescription;
		}
		set
		{
			_pageDescription = value;
		}
	}
	public string FbImage
	{
		get
		{
			return _fbImage;
		}
		set
		{
			_fbImage = value;
		}
	}
	public string FbURL
	{
		get
		{
			return _fbURL;
		}
		set
		{
			_fbURL = value;
		}
	}
	public string FbImageType
	{
		get
		{
			return _fbImageType;
		}
		set
		{
			_fbImageType = value;
		}
	}
	public string FbSite_name
	{
		get
		{
			return _fbSite_name;
		}
		set
		{
			_fbSite_name = value;
		}
	}
	public string showUserActionModal
	{
		get
		{
			return _showUserActionModal;
		}
		set
		{
			_showUserActionModal = value;
		}
	}
	public string FbDescription
	{
		get
		{
			return _fbDescription;
		}
		set
		{
			_fbDescription = value;
		}
	}
}