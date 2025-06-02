using Microsoft.SqlServer.Server;
using System;
using System.Activities.Expressions;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

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
    public bool _hideMasterCover = true;
    public string bodyTag = string.Empty;
    public string profileURL = string.Empty;
    public string fixedFooter = "fixed-footer";
    public string fixedHeader = "fixed-header";
    public string _userOrganizationEventId = string.Empty;
    public string _organizationEventId = string.Empty;
    public string _organizationId = string.Empty;
    public string _volunteerTypes;
    public string _timeActive = "false";
    public string _cause = string.Empty;
    public string _noCause = "false";
    public Guid userId = Guid.NewGuid();
    public string _showUserActionModal = string.Empty;
    public string communityUpdated = "grey";
    public string skillsUpdated = "grey";
    public string resourcesUpdated = "grey";
    public string teamUpdated = "grey";
    public string deploymentUpdated = "grey";
    public string calendarUpdated = "grey";
    public string _masterCoverImage = "";
    public string FeatureTypeCounterTitle = string.Empty;
	public string adminHeaderStyle = "{background-color:#5e2e91;height:58px;}";
    public string TimeAgo { get; set; }
    public string notificationCounting { get; set; }
    public class FeatureTypeCounter
    {
        public int Counter { get; set; }
        public string FeatureKey { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        form1.Action = HttpContext.Current.Request.RawUrl;
        divLogin.Visible = true;
        divSettings.Visible = false;
        litVolunteerPending.Text = " Volunteer";
        litVolunteerIcon.Text = "<i class=\"fa fa-heart\"></i>";
		
        PlaceHolder PlaceHolderContent = (PlaceHolder)FindControl("PlaceHolderContent");

        if (PlaceHolderContent != null)
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            // Specify the page you want to render, e.g., a different ASPX page
            string pageToRender = "/V1/MasterPages/ReusableNotification.aspx";

            // Render the page to the StringWriter
            Server.Execute(pageToRender, htw);

            // Create a LiteralControl with the rendered content
            LiteralControl litControl = new LiteralControl(sw.ToString());

            // Add the LiteralControl to the PlaceHolder
            PlaceHolderContent.Controls.Add(litControl);
        }

        divMasterCover.Visible = false;
        if (!_hideMasterCover)
        {
            divMasterCover.Visible = true;
            _masterCoverImage = "/V1/Images/Cover/Stability_Cover_V1.jpg";
        }

        string userActionModal = Request.QueryString["userActionModal"];

        if (_hideMenu)
        {
            menu.Visible = false;
        }
        if (_hideHeader)
        {
			divSearch.Visible = false;
			divMessages.Visible = false;
			fixedHeader = string.Empty;
            header.Visible = false;
			mobileMenu.Visible = false;

		}
        if (_hideFooter)
        {
            fixedFooter = string.Empty;
            //footer.Visible = false;
        }

        FbImage = String.IsNullOrEmpty(_fbImage) ? "V1/Images/MSTEAM.png" : _fbImage;
        FbImageType = String.IsNullOrEmpty(FbImageType) ? "image/png" : _fbImageType;
        PageTitle = String.IsNullOrEmpty(PageTitle) ? System.Configuration.ConfigurationManager.AppSettings["Title"].ToString() : _pageTitle;
        PageDescription = String.IsNullOrEmpty(PageDescription) ? System.Configuration.ConfigurationManager.AppSettings["Description"].ToString() : _pageDescription;
        FbSite_name = String.IsNullOrEmpty(FbSite_name) ? System.Configuration.ConfigurationManager.AppSettings["Title"].ToString() : _fbSite_name;
        FbDescription = String.IsNullOrEmpty(FbDescription) ? System.Configuration.ConfigurationManager.AppSettings["Description"].ToString() : _fbDescription;
        FbURL = HttpContext.Current.Request.Url.ToString();
        fbTitle.Attributes.Add("content", PageTitle);
        fbImage.Attributes.Add("content", HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + FbImage);
        fbURL.Attributes.Add("content", FbURL);
        fbImageType.Attributes.Add("content", FbImageType);
        fbSite_name.Attributes.Add("content", FbSite_name);
        fbDescription.Attributes.Add("content", FbDescription);
        title.Text = PageTitle;
        description.Attributes.Add("content", PageTitle);

        //Calculating the notificationCount
        using (var dc = new CrowdReliefDBDataContext())
        {
            MembershipUser user = Membership.GetUser();

            if (user != null && user.ProviderUserKey != null)
            {
                Guid currentUserId = new Guid(user.ProviderUserKey.ToString());
                int unreadCount = dc.Notifications
                    .Count(n => !n.IsRead && n.RecipientUserId == currentUserId);
                notificationCounting = unreadCount.ToString();
                notificationCounts.Text = unreadCount > 0 ? unreadCount.ToString() : string.Empty;
            }
            else
            {
                int unreadCount = 0;
            }
        }

		divFeed.Visible = false;
		divProfile.Visible = false;
		divTeam.Visible = false;
		divSearch.Visible = false;
		divMessages.Visible = false;


		if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            bool hasTeam = false;
            bool hasDeployment = false;
            bool hasPortal = false;


			divFeed.Visible = true;
			divProfile.Visible = true;
			divTeam.Visible = true;
			if(!_hideHeader)
			divSearch.Visible =  true;
			if(!_hideHeader)
			divMessages.Visible = true;

			userId = new Guid(Membership.GetUser().ProviderUserKey.ToString());
            string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
            divSettings.Visible = true;
            divLogin.Visible = false;

			//Load users groups.

			BindUserGroups();

			//Get the users information.
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            if (HttpContext.Current.User.IsInRole("Administrator"))
            {
                //menu.Style["margin-top"] = "60px";
                adminFeatureSection.Visible = false;
				//adminHeaderStyle = "{position: fixed; top: 65px; left: 0;width: 100%;z-index: 9999; background-color: #5e2e91; height: 58px; box-shadow: 0 2px 4px rgba(0,0,0,0.1);}";

				string FeatureTypeRedirectUrl = HttpContext.Current.Request.Url.AbsolutePath;

                if (!string.IsNullOrEmpty(FeatureTypeRedirectUrl))
                {
                    var FeatreTypeCounters = dc.FeatureTypes
                        .Where(x => x.RedirectURL.Contains(FeatureTypeRedirectUrl))
                        .Select(x => new FeatureTypeCounter
                        {
                            Counter = (int)x.Counter,
                            FeatureKey = x.DisplayText
                        })
                        .FirstOrDefault();

                    if (FeatreTypeCounters != null)
                    {
                        FeatureTypeCounterTitle = FeatreTypeCounters.FeatureKey + " Page Load Count = ";
                        FeatureTypeCounterNumber.Text = " : " + FeatreTypeCounters.Counter.ToString();
                    }
                    else
                    {
                        FeatureTypeCounterTitle = "Page Not in Database";
                        FeatureTypeCounterNumber.Text = "";
                    }
                }
                else
                {
                    adminFeatureSection.Visible = false;

                }
                // Add or remove the body style based on the visibility of the adminFeatureSection
                if (adminFeatureSection.Visible)
                {
                    bodyStyle.Visible = true; // Show the style block
                }
                else
                {
                    bodyStyle.Visible = false; // Hide the style block
                }
            }



            var volunteerTypes = from vt in dc.TaskTypes
                                 orderby vt.Name
                                 select vt;

            foreach (var volunteerType in volunteerTypes)
            {
                _volunteerTypes += "<li id=\"" + volunteerType.TaskTypeId + "\"><a href=\"#\"><i class=\"pe-7s-id\"></i> " + volunteerType.Name.ToUpper() + "</a></li>" + Environment.NewLine;
            }
            litVolunteerType.Text = _volunteerTypes;
            var profile = (from p in dc.Profiles
                           where p.UserId == userId
                           select new { p }).SingleOrDefault();

            string roleType = string.Empty;
            bool hasDefaultDisaster = false;
            if (profile != null && profile.p.DefaultEventId != null)
            {
                //Get the default disaster
                var disasterEvent = (from d in dc.Events
                                     where d.EventId == profile.p.DefaultEventId
                                     select new { d.Name, d.URLFriendlyName }).SingleOrDefault();

                if (disasterEvent != null)
                {
                    hasDefaultDisaster = true;
                    communityUpdated = "yellowgreen";
                    //Put the default disaster at the top.
                    //hypMDefaultDisaster.Text = "<b>" + disasterEvent.Name + "</b>";
                    //hypMDefaultDisaster.NavigateUrl = "/Disaster/" + disasterEvent.URLFriendlyName;
                    //hypDefaultDisaster.Text = "<b>" + disasterEvent.Name + "</b>";
                    //hypDefaultDisaster.NavigateUrl = "/Disaster/" + disasterEvent.URLFriendlyName;
                    litDefaultDisaster.Text = "<li><strong><a href='/Disaster/" + disasterEvent.URLFriendlyName + "'>" + disasterEvent.Name + "</a> (Default Portal)</strong></li>";
                }
            }

            var orgUser = from o in dc.Organizations
                          join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                          where uo.UserId == userId && uo.Status== (int)RequestStatus.Approved
                          orderby o.CreatedOn descending
                          select o;

            if (orgUser.Count() > 0)
            {
                hasTeam = true;
                _organizationId = orgUser.Take(1).SingleOrDefault().OrganizationId.ToString();
                linkDeployment.HRef = "/V1/NonProfit/Deployments.aspx?organizationId=" + _organizationId;
                teamUpdated = "yellowgreen";
                hypActionPage.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId;
                //Person is owner of a non-profit.
                litNonProfitName.Visible = true;
                hypNonProfit.Text = orgUser.Take(1).SingleOrDefault().Name;
                hypNonProfit.NavigateUrl = "/V1/NonProfit/Stream.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId;
                //hypMyTeam.NavigateUrl = "/V1/NonProfit/Stream.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId;
                //hypMMyTeam.NavigateUrl = "/V1/NonProfit/Stream.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId;
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
                divNoTeamGuidance.Visible = true;
                hypInviteTeamMembers.Visible = false;
                hypNonProfit.Visible = false;
                litNonProfitName.Visible = false;
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
                foreach (var orgOrgUser in orgOrgUsers)
                {
                    myDisasterList += "<li><a href=\"/V1/NonProfit/Default.aspx?organizationId=" + orgUser.Take(1).SingleOrDefault().OrganizationId + "\">" + orgUser.Take(1).SingleOrDefault().Name + "</a></li>";
                }
                litMyCampaigns.Text = myDisasterList;
            }

            var userOrganizations = from uo in dc.UserOrganizations
                                    join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
                                    where uo.UserId == userId && uo.Status== (int)RequestStatus.Approved
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

            //Has the user entered dates to deploy?
            var userAvailableDate = from uad in dc.UserAvailableDates
                                    where uad.UserId == userId
                                    select uad;

            if (userAvailableDate.Count() > 0)
            {
                calendarUpdated = "yellowgreen";
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
                         orderby oe.CreatedOn descending
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
                divNoDeploymentGuidance.Visible = true;
                litMyCausesslabel.Visible = false;


            }
            else
            {
                hasDeployment = true;
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
                hasPortal = true;
                string myDisasterList = string.Empty;
                foreach (var disaster in disasters.Distinct().OrderByDescending(d => d.BeginDate))
                {
                    myDisasterList += "<li><a href=\"/Disaster/" + disaster.URLFriendlyName + "\">" + disaster.Name + "</a></li>";
                }
                communityUpdated = "yellowgreen";
                litMyDisasters.Text = myDisasterList;
                if (!hasDefaultDisaster)
                {
                    //hypMDefaultDisaster.Text = "<b>" + disasters.Take(1).SingleOrDefault().Name + "</b>";
                    //hypMDefaultDisaster.NavigateUrl = "/Disaster/" + disasters.Take(1).SingleOrDefault().URLFriendlyName;
                    //hypDefaultDisaster.Text = "<b>" + disasters.Take(1).SingleOrDefault().Name + "</b>";
                    //hypDefaultDisaster.NavigateUrl = "/Disaster/" + disasters.Take(1).SingleOrDefault().URLFriendlyName;
                    litDefaultDisaster.Text = "<li><strong><a href='/Disaster/" + disasters.Take(1).SingleOrDefault().URLFriendlyName + "'>" + disasters.Take(1).SingleOrDefault().Name + "</a> (Default Portal)</strong></li>";
                }
            }
            else
            {
                litDiasterLabel.Visible = false;
                divNoPortalGuidance.Visible = true;
            }
            string virtualPath;
            //List nonprofits a user volunteers for.
            var profileImage = (from ph in dc.ProfilePhotos
                                join p in dc.Photos on ph.PhotoId equals p.PhotoId
                                where ph.UserId == userId && ph.IsCurrrent == true
                                orderby p.CreatedOn descending
                                select new { p.FilenameCropped }).Take(1).SingleOrDefault();

            if (profileImage != null)
            {
                virtualPath = profilePhotoFolder + profileImage.FilenameCropped;
                string physicalPath = Server.MapPath(virtualPath);

                if (!File.Exists(physicalPath))
                {
                    virtualPath = "~/V1/Images/icons8-customer-64.png";
                }
            }
            else
            {
                virtualPath = "~/V1/Images/icons8-customer-64.png";
            }
            imgProfile.Src = virtualPath;

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
                profileURL = "'/V1/Member/Default.aspx'";
                //roleType = "Helper";
            }
            else if (HttpContext.Current.User.IsInRole("volunteer"))
            {
                profileURL = "'/V1/Member/Default.aspx'";
                //roleType = "Survivor";
            }
            else
            {
                profileURL = "'/V1/Member/Default.aspx'";
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
            if (volunteerStatus == VolunteerStatus.ApplicationComplete.Value)
            {
                //Change the Volunteer button to say volunteer pending.
                litVolunteerPending.Text = " Volunteer Pending";
                hypVolunteer.NavigateUrl = "~/V1/Member/Default.aspx";
                litVolunteerIcon.Text = "<i class=\"fa fa-exclamation-circle\"></i>";
            }
            else if (volunteerStatus == VolunteerStatus.VettingComplete_Failed.Value)
            {
                litVolunteerPending.Text = " Volunteer Pending";
                hypVolunteer.NavigateUrl = "~/V1/Member/Default.aspx";
                litVolunteerIcon.Text = "<i class=\"fa fa-exclamation-circle\"></i>";
            }
            else if (volunteerStatus == VolunteerStatus.VettingComplete_Passed.Value)
            {
                litVolunteerPending.Text = " Find Volunteer Opportunities";
                hypVolunteer.NavigateUrl = "~/V1/Stream.aspx";
                litVolunteerIcon.Text = "<i class=\"fa fa-heart\"></i>";
            }
            else if (volunteerStatus == VolunteerStatus.VettingStarted.Value)
            {
                litVolunteerPending.Text = " Volunteer Under Review";
                hypVolunteer.NavigateUrl = "~/V1/Member/Default.aspx";
                litVolunteerIcon.Text = "<i class=\"fa fa-exclamation-circle\"></i>";
            }
            else
            {
                litVolunteerPending.Text = " Volunteer";
                litVolunteerIcon.Text = "<i class=\"fa fa-heart\"></i>";
            }


            //Get the current page name and set bold in navigation.
            string pageName = Page.ToString().ToLower().Replace("_", ".").Replace("asp.v1.profile.", "").Replace("asp.v1.", "");

            switch (pageName)
            {
                case "default.aspx":
                    {
                        //If user only has one home, then bypass the list and send them to it.
                        var rebuildCount = from r in dc.Rebuilds
                                           where r.CreatedBy == userId
                                           select r;

                        if (rebuildCount.Count() == 1)
                        {
                            Response.Redirect("Rebuild.aspx?rebuildId=" + rebuildCount.First().RebuildId.ToString());
                        }
                        litTrackHome.Text = "class=\"active\"";
                    }
                    break;
                case "chooseevent.aspx":
                    {
                        litAddHome.Text = "class=\"active\"";
                        litViewDisasters.Text = "class=\"active\"";
                    }
                    break;
                case "time.aspx":
                    {
                        //litRecordTime.Text= "class=\"active\"";
                    }
                    break;
                case "stream.aspx":
                    {
                        litStream.Text = "class=\"active\"";
                    }
                    break;
                case "stages.aspx":
                    {
                        litStages.Text = "class=\"active\"";
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

                if (Session["FirstLoad"] == null)
                {
                    // If it doesn't exist, set it to true
                    Session["FirstLoad"] = true;

                    // Perform your action that should only happen on the first load
                    // Example: Prevent some specific action from taking place
                    //Show the modal

                    //Can hide it here.
                    _showUserActionModal = "$(\"#divUserActionModal\").modal('show')";
                }
                else
                {
                    // On subsequent loads, set the session value to false
                    Session["FirstLoad"] = false;
                }




            }
            if (CheckSkills(userId))
            {
                skillsUpdated = "yellowgreen";
            }
            if (CheckResources(userId))
            {
                resourcesUpdated = "yellowgreen";
            }

            if (calendarUpdated == "yellowgreen" && communityUpdated == "yellowgreen" && skillsUpdated == "yellowgreen" && resourcesUpdated == "yellowgreen" && teamUpdated == "yellowgreen" && deploymentUpdated == "yellowgreen")
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
        if (HttpContext.Current.User.IsInRole("Administrator") || HttpContext.Current.User.IsInRole("Vetting"))
        {
            divAdminLinks.Visible = true;
        }
        if (HttpContext.Current.User.IsInRole("LocationAdministrator") || HttpContext.Current.User.IsInRole("LocationManager"))
        {
            divLocationAdministration.Visible = true;
        }
    }

	protected void rptUserGroups_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;

			string UrlFriendlyTeamName = (string)DataBinder.Eval(dataItem.DataItem, "UrlFriendlyTeamName");
			int TeamStatus = (int)DataBinder.Eval(dataItem.DataItem, "TeamStatus");
			string organizationUrl = !String.IsNullOrEmpty(UrlFriendlyTeamName) ? "/Team/" + UrlFriendlyTeamName : "/V1/NonProfit/Default.aspx?organizationId=" + (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationId");

			string organizationName = (string)DataBinder.Eval(dataItem.DataItem, "OrganizationName");
			string organizationTeamLogo = (string)DataBinder.Eval(dataItem.DataItem, "imgTeam");

			string imgTeamPath = "/V1/Images/Logo-Placeholder.png";  

			if (!string.IsNullOrEmpty(organizationTeamLogo))
			{
				string virtualPath = "/Impactoid/Images/Logos/" + organizationTeamLogo;
				string physicalPath = Server.MapPath(virtualPath);

				if (System.IO.File.Exists(physicalPath))
				{
					imgTeamPath = virtualPath; 
				}
			}

			string teamLabel = " <span class='badge badge-primary' style='margin-left: 55px; margin-top:-20px;'>Primary Team</span>";
			if(TeamStatus == 0)
			{
				teamLabel = " <span class='badge badge-default' style='margin-left: 55px; margin-top:-20px;'>Pending Approval</span>";
			}

			bool isPrimary = Convert.ToBoolean(DataBinder.Eval(dataItem.DataItem, "IsPrimary") ?? false);
			Literal litPrimaryBadge = (Literal)e.Item.FindControl("litPrimaryBadge");
			litPrimaryBadge.Text = isPrimary ? teamLabel : "";
			Literal lit = (Literal)e.Item.FindControl("litGroupLink");
			lit.Text = "<a href=\"" + organizationUrl + "\">" + organizationName + "</a>";
			Image imgTeam = (Image)e.Item.FindControl("imgTeam");
			imgTeam.ImageUrl = imgTeamPath;
		}
	}


		private void BindUserGroups()
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var userGroups = (from g in dc.UserOrganizations
							  join o in dc.Organizations on g.OrganizationId equals o.OrganizationId
							  where g.UserId == userId && (g.Status == 0 || g.Status == 1)
							  orderby g.IsPrimary descending, o.Name
							  select new
							  {
								  OrganizationName = o.Name,
								  UrlFriendlyTeamName = o.URLFriendlyName,
								  OrganizationId = o.OrganizationId,
								  imgTeam = o.LogoSquare,
								  OwnerId = o.OwnerId,
								  IsPrimary = g.IsPrimary,
								  TeamStatus = g.Status
							  }).ToList();

			rptUserGroups.DataSource = userGroups;
			rptUserGroups.DataBind();
		}

		private bool CheckResources(Guid userId)
		{
			bool hasResources = false;
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var resourcesCheck = (from us in dc.UserResources
								  where us.UserId == userId
								  select us).Count();

			if (resourcesCheck > 0)
			{
				hasResources = true;
			}

			return hasResources;
		}
		private bool CheckSkills(Guid userId)
		{
			bool hasSkills = false;
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var skillCheck = (from us in dc.UserSkills
							  where us.UserId == userId
							  select us).Count();

			if (skillCheck > 0)
			{
				hasSkills = true;
			}

			return hasSkills;
		}

		public bool HideMasterCover
		{
			get
			{
				return _hideMasterCover;
			}
			set
			{
				_hideMasterCover = value;
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

		protected void btnSearchMobile_Click(object sender, EventArgs e)
		{
			string searchType = String.IsNullOrEmpty(txtSearchMobile.Text) ? hdnSearchType.Value : hymoblie.Value;
			string searchTerm = String.IsNullOrEmpty(txtSearchMobile.Text) ? txtSearchHeader.Text : txtSearchMobile.Text;
			if (searchType == "Teams")
			{
				Response.Redirect("/V1/NonProfit/TeamList.aspx?searchTerm=" + searchTerm);

			}
			else {
				Response.Redirect("/V1/Member/PeopleSearch.aspx?searchTerm=" + searchTerm);
			   }
		}

	}