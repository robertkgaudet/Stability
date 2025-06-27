using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;

public partial class CaseManagement_MasterPage : System.Web.UI.MasterPage
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
	public string fixedFooter = "fixed-footer";
	public string fixedHeader = "fixed-header";
	public string profileURL = string.Empty;
	public Guid userId = Guid.NewGuid();
	public string urlFriendlyName = string.Empty;
	public string userOrganizationName = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
	{

		form1.Action = HttpContext.Current.Request.RawUrl;
		divLogin.Visible = true;
		divSettings.Visible = false;

		title.Text = "Disaster Case Management - " + PageTitle;
		description.Attributes.Add("content", PageTitle);


		if (_hideMenu)
		{
			menu.Visible = false;
		}
		if (_hideHeader)
		{
			fixedHeader = string.Empty;
			header.Visible = false;
		}
		if (_hideFooter)
		{
			fixedFooter = string.Empty;
			footer.Visible = false;
		}

		fbTitle.Attributes.Add("content", PageTitle);
		fbImage.Attributes.Add("content", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + FbImage);
		fbURL.Attributes.Add("content", FbURL);
		fbImageType.Attributes.Add("content", FbImageType);// content="image/jpeg" content="image/png"
		fbSite_name.Attributes.Add("content", FbSite_name);
		fbDescription.Attributes.Add("content", FbDescription);

		//activeStatus.Attributes.Add("checked", "checked");
		if (HttpContext.Current.User.Identity.IsAuthenticated)
		{
			if(!HttpContext.Current.User.IsInRole("CaseManager"))
			{
				Response.Redirect("/Error.aspx?ErrorType=VictimNoAccess");
			}

			userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
			string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
			divSettings.Visible = true;
			divLogin.Visible = false;

			//Get the users information.
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var timesheet = (from t in dc.Timesheets
							 where t.UserId == userId
							 orderby t.TimeIn descending
							 select t).Take(1).SingleOrDefault();

			if (timesheet != null)
			{
				if (timesheet.TimeIn.Date > DateTime.Today && timesheet.TimeIn == null)
				{
					//user forgot to logout, log them out at midnight on the day they forgot.
					TimeSpan midnight = new TimeSpan(23, 59, 0);
					string missingDateTime = timesheet.TimeIn.Date.ToLongDateString();
					timesheet.TimeOut = timesheet.TimeIn.Date + midnight;
				}

				//is user logged in.
				if (timesheet.TimeOut == null && timesheet.TimeIn > DateTime.Now.AddDays(-1))
				{
					//User is logged in.
					activeStatus.Attributes.Add("checked", "checked");
				}
			}

			var profile = (from p in dc.Profiles
						   where p.UserId == userId
						   select new { p }).SingleOrDefault();

			string roleType = string.Empty;
			if (profile != null && !string.IsNullOrEmpty(profile.p.DefaultEventId.ToString()))
			{
				//Get the disaster
				var disasterEvent = (from d in dc.Events
									 where d.EventId == profile.p.DefaultEventId
									 select new { d.Name, d.URLFriendlyName }).SingleOrDefault();

				//           if (Roles.IsUserInRole("survivor"))
				//           {
				//               //We know their disaster, send them there.
				//               roleType = "Survivor";
				//           }
				//           if (Roles.IsUserInRole("nonprofitadministrator"))
				//           {
				//               roleType = "Nonprofit";
				//           }
				//           if (Roles.IsUserInRole("business") || Roles.IsUserInRole("contractor"))
				//           {
				//               roleType = "Business";
				//           }
				//           if (Roles.IsUserInRole("helper") || Roles.IsUserInRole("volunteer"))
				//           {
				//if (!CheckSkills(userId) && HttpContext.Current.Request.RawUrl.IndexOf("EditSkills.aspx") == 0)
				//{
				//	Response.Redirect("~/V1/Profile/EditSkills.aspx");
				//}
				//roleType = "Helper";
				//           }
				if (disasterEvent != null)
				{
					urlFriendlyName = disasterEvent.URLFriendlyName;
					hypDefaultDisaster.Text = "<b>" + disasterEvent.Name + "</b>";
					hypDefaultDisaster.NavigateUrl = "/Disaster/" + disasterEvent.URLFriendlyName;// + "/" + roleType;

					litDefaultDisaster.Text = "<li style=\"background-color:#FFD86E;\"><a href='/" + disasterEvent.URLFriendlyName + "'>" + disasterEvent.Name + "</a></li>";
				}
			}

			var userOrganizations = (from uo in dc.UserOrganizations
									 join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
									 where uo.UserId == userId && o.IsActive == true && (uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
                                     select new { o.Name, o.OrganizationId }).Take(1).SingleOrDefault() ;

			if (userOrganizations != null)
			{
				hypUserOrganizatioName.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + userOrganizations.OrganizationId;
				hypUserOrganizatioName.Text = userOrganizations.Name;
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
					myDisasterList += "<li><a href=\"/" + disaster.URLFriendlyName + "\">" + disaster.Name + "</a></li>";
				}
				litMyDisasters.Text = myDisasterList;
			}
			//List nonprofits a user volunteers for.

			var profileImage = (from ph in dc.ProfilePhotos
								join p in dc.Photos on ph.PhotoId equals p.PhotoId
								where ph.UserId == userId && ph.IsCurrrent == true
								orderby p.CreatedOn descending
								select new { p.FilenameCropped }).Take(1).SingleOrDefault();

			if (profileImage != null)
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
			if (HttpContext.Current.User.IsInRole("survivor"))
			{
				profileURL = "'/S1/Profile/Default.aspx'";
				//roleType = "Survivor";
			}
			else if (HttpContext.Current.User.IsInRole("helper"))
			{
				profileURL = "'/V1/Profile/Profile.aspx'";
				//roleType = "Helper";
			}
			else if (HttpContext.Current.User.IsInRole("volunteer"))
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

			//litRoleType.Text = "<small>" + roleType + "</small>";
			if (profile == null)
			{
				System.Web.Security.FormsAuthentication.SignOut();
				Session.Abandon();
				Response.Redirect("/SignIn");
			}
			litUsername.Text = profile.p.Firstname + " " + profile.p.Lastname + "<br/>";


			//Get the current page name and set bold in navigation.
			string pageName = Page.ToString().ToLower().Replace("_", ".").Replace("asp.v1.profile.", "").Replace("asp.v1.", "");
		}
		else
		{
			activeStatus.Attributes.Add("disabled", "disabled");
		}
		if (HttpContext.Current.User.IsInRole("Administrator") || HttpContext.Current.User.IsInRole("Vetting"))
		{
			divAdminLinks.Visible = true;
		}
		if (HttpContext.Current.User.IsInRole("LocationAdministrator") || HttpContext.Current.User.IsInRole("LocationManager"))
		{
			divLocationAdministration.Visible = true;
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