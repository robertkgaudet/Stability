using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;

public partial class S1_MasterPages_Homer : System.Web.UI.MasterPage
{
	public string _pageTitle = string.Empty;
	public string _pageDescription = string.Empty;
	public string _fbImage = string.Empty;
	public string _fbURL = string.Empty;
	public string _fbImageType = string.Empty;
	public string _fbSite_name = string.Empty;
	public string _fbDescription = string.Empty;
	public bool _hideCategoryList = false;
	public bool _hideHeader = false;
	public bool _hideFooter = false;
	public bool _hideMenu = false;
	public bool _boxedBody = false;
	public string bodyTag = string.Empty;
	public string boxedWrapperOpen = string.Empty;
	public string boxedWrapperClosed = string.Empty;
	public string profileURL = "'/V1/Profile/Profile.aspx'";

	protected void Page_Load(object sender, EventArgs e)
	{
		form1.Action = HttpContext.Current.Request.RawUrl;
		divLogin.Visible = true;
		divSettings.Visible = false;
		litVolunteerPending.Text = " Volunteer";
		litVolunteerIcon.Text = "<i class=\"fa fa-heart\"></i>";

		title.Text = PageTitle;
		description.Attributes.Add("content", PageTitle);

		string fixedFooter = "fixed-footer";
		string fixedHeader = "fixed-header";
		string boxedBody = string.Empty;
		
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
		if(_boxedBody)
		{
			fixedHeader = string.Empty;
			boxedBody = "boxed";
			boxedWrapperOpen = "<div class=\"boxed-wrapper\">";
			boxedWrapperClosed = "</div>";
		}

		bodyTag = "class=\"sidebar-scroll fixed-navbar " + boxedBody + " " + fixedHeader + " " + fixedFooter + " bodyBGColor\"";

		fbTitle.Attributes.Add("content", PageTitle);
		fbImage.Attributes.Add("content", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + FbImage);
		fbURL.Attributes.Add("content", FbURL);
		fbImageType.Attributes.Add("content", FbImageType);// content="image/jpeg" content="image/png"
		fbSite_name.Attributes.Add("content", FbSite_name);
		fbDescription.Attributes.Add("content", FbDescription);

		if(HttpContext.Current.User.Identity.IsAuthenticated)
		{
			liNewPost.Visible = true;
			string profilePhotoFolder	= System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
			divSettings.Visible = true;
			divLogin.Visible = false;

			litNewSurvivor.Visible = true;
			litMySuvivors.Visible = true;

			string[] userRoles = Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);
			
			string roleType = string.Empty;
			foreach(string role in userRoles)
			{
				roleType += role.ToString() + "<br/>";
            }


            litRoleType.Text = "<small class=\"text - muted\">" + roleType + "</small>";

            if (HttpContext.Current.User.IsInRole("administrator"))
			{
				hrefAddStory.NavigateUrl = "/S1/Profile/HelperOrSurvivor.aspx";
			}
			
			if(HttpContext.Current.User.IsInRole("survivor") && !HttpContext.Current.User.IsInRole("helper") && !HttpContext.Current.User.IsInRole("volunteer"))
			{
				litNewSurvivor.Visible = false;
				litMySuvivors.Visible = false;
				profileURL = "'/S1/Profile/Default.aspx'";
			}
			else if(HttpContext.Current.User.IsInRole("volunteer"))
			{
				profileURL = "'/V1/Profile/Profile.aspx'";
				//roleType = "Volunteer";
			}
			else if(HttpContext.Current.User.IsInRole("helper"))
			{
				profileURL = "'/V1/Profile/Profile.aspx'";
				//roleType = "Helper";
			}
			else
			{
				profileURL = "'/V1/Profile/Profile.aspx'";
				//roleType = "Member";
			}

			//Get the users information.
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			Guid userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());

			var profile = (from p in dc.Profiles
						  where p.UserId == userId
						  select new {p }).SingleOrDefault();

            roleType = string.Empty;
            if (profile != null && profile.p.DefaultEventId != null)
            {
                //Get the disaster
                var disasterEvent = (from d in dc.Events
                                     where d.EventId == profile.p.DefaultEventId
                                     select new { d.Name, d.URLFriendlyName }).SingleOrDefault();

                if (Roles.IsUserInRole("survivor"))
                {
                    //We know their disaster, send them there.
                    roleType = "Survivor";
                }
                if (Roles.IsUserInRole("nonprofitadministrator"))
                {
                    roleType = "Nonprofit";
                }
                if (Roles.IsUserInRole("business") || Roles.IsUserInRole("contractor"))
                {
                    roleType = "Business";
                }
                if (Roles.IsUserInRole("helper") || Roles.IsUserInRole("volunteer"))
                {
                    roleType = "Helper";
                }

                hypDefaultDisaster.Text = "<b>" + disasterEvent.Name +"</b>";
                hypDefaultDisaster.NavigateUrl = "/Disaster/" + disasterEvent.URLFriendlyName + "/" + roleType;
                litDefaultDisaster.Text = "<li style=\"background-color:#FFD86E;\"><a href='/Disaster/" + disasterEvent.URLFriendlyName + "'>" + disasterEvent.Name + "</a></li>";
            }

            //If the user started a non-profit or created a response to one, then show it. If they are affiliated with more than one, then show the list.
            lblNonProfitDescription.Text = "Stability is the Official Disaster Recovery Platform of the ";
            hypNonProfit.NavigateUrl = "http://www.CajunRelief.org";
            hypNonProfit.Text = "<img class=\"right\" src=\"/S1/Images/CajunNavySmallLogo.png\" /> Cajun Navy";

            var orgUser = (from o in dc.Organizations
                           where o.OwnerId == userId
                           orderby o.Name descending
                           select o).SingleOrDefault();

            //Need to be able to assign someone as the nonprofit owner.
            if (orgUser != null)
            {
                lblNonProfitDescription.Visible = false;
                hypNonProfit.Text = orgUser.Name;
                hypNonProfit.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + orgUser.OrganizationId;
            }
            else
            {
                hypNonProfit.Target = "_blank";
            }

            var orgOrgUsers = from oe in dc.OrganizationEvents
                              where oe.PointOfContactUserId == userId
                              orderby oe.CreatedOn descending
                              select oe;
           
            if (orgOrgUsers.Count() > 0)
            {
                liDisasterCampaigns.Visible = true;
                string myCampaignList = string.Empty;
                foreach (var orgOrgUser in orgOrgUsers)
                {
                    myCampaignList += "<li><a href=\"/V1/NonProfit/Default.aspx?organizationId=" + orgUser.OrganizationId + "\">" + orgUser.Name + "</a></li>";
                }
                litMyCampaigns.Text = myCampaignList;
            }

            var userOrganizations = from uo in dc.UserOrganizations
                                    join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
                                    where uo.UserId == userId && uo.Status== (int)RequestStatus.Approved
                                    select new { o.Name, o.OrganizationId };

            if (userOrganizations.Count() > 0)
            {
                liMyNonProfits.Visible = true;
                string myOrganizationList = string.Empty;
                foreach (var userOrganization in userOrganizations)
                {
                    myOrganizationList += "<li><a href=\"/V1/NonProfit/Default.aspx?organizationId=" + userOrganization.OrganizationId + "\">" + userOrganization.Name + "</a></li>";
                }
                litMyNonProfits.Text = myOrganizationList;
            }

            var disasters = from ev in dc.Events
                            join ue in dc.UserEvents on ev.EventId equals ue.EventId
                            where ue.UserId == userId
                            orderby ev.BeginDate descending
                            select new { ev.EventId, ev.Name, ev.URLFriendlyName };

            if (disasters.Count() > 0)
            {
                liMyDisasters.Visible = true;
                string myDisasterList = string.Empty;
                foreach (var disaster in disasters)
                {
                    myDisasterList += "<li><a href=\"/Disaster/" + disaster.URLFriendlyName + "\">" + disaster.Name + "</a></li>";
                }
                litMyDisasters.Text = myDisasterList;
            }

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


			BaseOrganizationWebForm baseWebForm = new BaseOrganizationWebForm();

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
						//litAddHome.Text= "class=\"active\"";
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
						//litStages.Text= "class=\"active\"";
					}
					break;
				default:
					{
					}
					break;
			}
		}
	}

	public bool BoxedBody
	{
		get
		{
			return _boxedBody;
		}
		set
		{
			_boxedBody = value;
		}
	}
	public bool HideCategoryList
	{
		get
		{
			return _hideCategoryList;
		}
		set
		{
			_hideCategoryList = value;
		}
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