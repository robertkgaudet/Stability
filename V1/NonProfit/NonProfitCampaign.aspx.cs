using CrowdRelief;
using Stability;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twilio.Types;

public partial class V1_NonProfit_NonProfitCampaign : BaseOrganizationWebForm
{
    public string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
    public string donateLink = string.Empty;
    public string volunteerLink = string.Empty;
    public string getHelpLink = string.Empty;
    public string icon = string.Empty;
    public string editCampaignLink = string.Empty;
    public string editPositionsLink = string.Empty;

    public string _todayVolunteerCount = "0";
    public string _todayVolunteerHours = "0";
    public string _todayVolunteerValue = "0";

    public string _totalVolunteerCount = "0";
    public string _totalVolunteerHours = "0";
    public string _totalVolunteerValue = "0";
    public string _volunteerHourlyRate = "";
    string pageName = "Details";
    public bool userIsInOrganization = false;

    protected void LoadImpactMetrics(Guid organizationEventId, decimal volunteerHourlyRate)
    {
        //count the number of volunteers today and total.

        //TODAY volunteer count.
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var totalVolunteersToday = (from uoe in dc.UserOrganizationEvents
                                    join t in dc.Timesheets on uoe.UserId equals t.UserId
                                    where uoe.OrganizationEventId == organizationEventId
                                    && t.TimeIn.Date == DateTime.Today.Date
                                    select new { uoe.UserId }).Distinct().Count();

        _todayVolunteerCount = totalVolunteersToday.ToString();

        //TODAYS volunteer hours
        var todaysVolunteerHours = dc.GetTotalHoursByCauseByDay(organizationEventId).First().Column1;
        if (todaysVolunteerHours != null)
        {
            _todayVolunteerHours = todaysVolunteerHours;
            _todayVolunteerValue = (Convert.ToInt32(todaysVolunteerHours) * volunteerHourlyRate).ToString("C");
        }

        //TOTAL volunteer hours
        var totalVolunteerHours = dc.GetTotalHoursByCause(organizationEventId).First().Column1;
        if (totalVolunteerHours != null)
        {
            _totalVolunteerHours = totalVolunteerHours;
            _totalVolunteerValue = (Convert.ToInt32(totalVolunteerHours) * volunteerHourlyRate).ToString("C");
        }


        var totalVolunteers = (from uoe in dc.UserOrganizationEvents
                               join t in dc.Timesheets on uoe.UserId equals t.UserId
                               where uoe.OrganizationEventId == organizationEventId
                               select new { uoe.UserId }).Distinct().Count();

        _totalVolunteerCount = totalVolunteers.ToString();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        txtsms.Attributes["maxlength"] = "450";
        string organizationEventId = string.Empty;
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        if (String.IsNullOrEmpty(Request.QueryString["organizationEventId"]) && String.IsNullOrEmpty(Request.QueryString["organizationEventFriendlyURLName"]))
        {
            Response.Write("No organizationEventId or organizationEventFriendlyURLName provided.");
            Response.End();
        }
        else
        {
            if (!String.IsNullOrEmpty(Request.QueryString["organizationEventFriendlyURLName"]))
            {
                //Get the org id.

                var organizationIdOnly = (from oe in dc.OrganizationEvents
                                          where oe.URLFriendlyCampaignName == Request.QueryString["organizationEventFriendlyURLName"]
                                          select new { oe.OrganizationEventId }).SingleOrDefault();

                organizationEventId = organizationIdOnly.OrganizationEventId.ToString();
            }
            else
            {
                organizationEventId = Request.QueryString["organizationEventId"];
            }
        }

        ucPostionNavigation.PageName = pageName;
        ucPostionNavigation.OrganizationEventId = organizationEventId;

        var organizationEvent = (from oe in dc.OrganizationEvents
                                 join ev in dc.Events on oe.EventId equals ev.EventId
                                 join o in dc.Organizations on oe.OrganizationId equals o.OrganizationId
                                 where oe.OrganizationEventId == new Guid(organizationEventId)
                                 select new { oe, ev, o }).Take(1).SingleOrDefault();

        if (organizationEvent == null)
        {
            Response.Redirect("/V1/Member/Default.aspx");
        }

        hypAddCase.NavigateUrl = "/CaseManagement/AddCase.aspx";
        hypViewCases.NavigateUrl = "/CaseManagement/Default.aspx?eventName=" + organizationEvent.ev.URLFriendlyName;
        lbVolunteer.Visible = true;
        lbVolunteer.NavigateUrl = "/SignUp/" + organizationEvent.oe.URLFriendlyCampaignName;

        volunteerLink = "/SignUp/" + organizationEvent.oe.URLFriendlyCampaignName;
        ucPostionNavigation.URLFriendlyName = organizationEvent.oe.URLFriendlyCampaignName;
        decimal volunteerRate = organizationEvent.oe.VolunteerHourlyRate != null ? Convert.ToDecimal(organizationEvent.oe.VolunteerHourlyRate) : 0;
        _volunteerHourlyRate = volunteerRate.ToString("C");
        LoadImpactMetrics(organizationEvent.oe.OrganizationEventId, volunteerRate);

        bool isActive = (bool)organizationEvent.oe.IsActive;
        //litOrganizationName.Text = organizationEvent.o.Name;

        ucPostionNavigation.CampaignName = organizationEvent.oe.CampaignName;
        ucPostionNavigation.OrganizationName = organizationEvent.o.Name;
        ucPostionNavigation.PortalName = organizationEvent.ev.Name;
        ucPostionNavigation.PortalId = organizationEvent.ev.EventId.ToString();
        ucPostionNavigation.OrganizationId = organizationEvent.o.OrganizationId.ToString();
        ucPostionNavigation.Description = organizationEvent.oe.MissionPurpose;
        hypCauseIsNotActive.Visible = !isActive;
        organizationId = organizationEvent.oe.OrganizationId;
        linkDeploymentMap.NavigateUrl = "/Maps/" + organizationEvent.ev.URLFriendlyName;

        //FILTER TO PEOPLE ON THIS NON-PROFIT CAMPAIGN
        var peopleList = from uo in dc.UserOrganizationEvents
                         join p in dc.Profiles on uo.UserId equals p.UserId
                         join net in dc.aspnet_Memberships on p.UserId equals net.UserId
                         join u in dc.aspnet_Users on p.UserId equals u.UserId
                         where uo.OrganizationEventId == organizationEvent.oe.OrganizationEventId
                         && uo.DeactivatedOn == null
                         && p.PassedVetting == true
                         && net.IsApproved == true
                         && net.LastLoginDate > DateTime.Now.AddDays(-30)
                         orderby net.LastLoginDate descending
                         select new { p.Firstname, p.Lastname, p.UserId, p.Title, p.ZelloName, LastLoginDate = u.LastActivityDate };

        //ucTimeBoard.organizationEventId = organizationEvent.oe.OrganizationEventId;

        rpNonProfitPeople.DataSource = peopleList;
        rpNonProfitPeople.DataBind();

        //Master.PageTitle = organizationEvent.oe.CampaignName + " by " + organizationEvent.o.Name + " - Stability";
        //Master.PageDescription = organizationEvent.oe.MissionPurpose;
        //Master.FbDescription = organizationEvent.oe.MissionPurpose;
        //Master.FbImage = "/V1/Images/" + organizationEvent.ev.ImageFileName;
        //Master.FbImageType = "image/jpg";
        //Master.FbSite_name = organizationEvent.oe.CampaignName + " by " + organizationEvent.o.Name + " - Stability";
        //Master.FbURL = Request.Url.AbsoluteUri;

        litCampaignMission.Text = organizationEvent.oe.MissionPurpose;
        lblParentOrgName.Text = organizationEvent.o.Name;
        hypParentAddress.Text = organizationEvent.o.Address + "<br/>" + organizationEvent.o.City + ", " + organizationEvent.o.State + " " + organizationEvent.o.Zip;
        //hypParentAddress.NavigateUrl		= "http://maps.google.com/maps?q=" + organizationEvent.o.Address.Replace(" ","+") + "," + organizationEvent.o.City.Replace(" ","+") + "," + organizationEvent.o.State.Replace(" ","+") + "," + organizationEvent.o.Zip;
        lblVoadMember.Text = organizationEvent.o.IsVoadMember.ToString();
        lbl501c3.Text = organizationEvent.o._501c3Status.ToString();
        Master.PageName = organizationEvent.o.Name + " " + organizationEvent.oe.CampaignName + " Deployment for " + organizationEvent.ev.Name;

        lblPointOfContactPerson.Text = organizationEvent.o.PointOfContactName;
        if (!String.IsNullOrEmpty(organizationEvent.o.PointOfContactPhoneNumber))
        {
            hypPointOfContactPhone.Text = Regex.Replace(organizationEvent.o.PointOfContactPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
            hypPointOfContactPhone.NavigateUrl = "tel:" + organizationEvent.o.PointOfContactPhoneNumber;
            hypPointOfContactPhone.Font.Underline = true;
        }

        if (userId == organizationEvent.o.OwnerId || Roles.IsUserInRole("Administrator"))
        {
            hypViewCases.Visible = true;
            hypAddCase.Visible = true;
            ucPostionNavigation.IsTeamOwner = true;
            editCampaignLink = "/V1/NonProfitAdministration/EditNonProfitCampaign.aspx?OrganizationEventId=" + organizationEvent.oe.OrganizationEventId;
            editPositionsLink = "/V1/NonProfitAdministration/PositionsNeeded.aspx?OrganizationEventId=" + organizationEvent.oe.OrganizationEventId;
        }

        if (!String.IsNullOrEmpty(organizationEvent.oe.BlogURL))
        {
            hypBlog.NavigateUrl = organizationEvent.oe.BlogURL;
            hypBlog.Text = organizationEvent.oe.BlogURL;
            hypBlog.Font.Underline = true;
        }
        if (!String.IsNullOrEmpty(organizationEvent.oe.ZelloChannel))
        {
            dtZelloChannel.Visible = true;
            ddZelloChannel.Visible = true;
            lblZelloChannel.Text = organizationEvent.oe.ZelloChannel;
        }

        if (!String.IsNullOrEmpty(organizationEvent.o.PointOfContactEmail))
        {
            hypPointOfContactEmail.Text = organizationEvent.o.PointOfContactEmail;
            hypPointOfContactEmail.NavigateUrl = "mailto:" + organizationEvent.o.PointOfContactEmail;
            hypPointOfContactEmail.Font.Underline = true;
        }


        if (!String.IsNullOrEmpty(organizationEvent.o.FacebookURL))
        {
            hypFacebookPage.Text = organizationEvent.o.Name + " Facebook Page";
            hypFacebookPage.NavigateUrl = organizationEvent.o.FacebookURL;
            hypFacebookPage.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organizationEvent.o.FacebookGroupURL))
        {
            hypFacebookGroup.Text = organizationEvent.o.Name + " Facebook Group";
            hypFacebookGroup.NavigateUrl = organizationEvent.o.FacebookGroupURL;
            hypFacebookGroup.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organizationEvent.o.TwitterURL))
        {
            hypTwitter.Text = "Visit " + organizationEvent.o.TwitterURL;
            hypTwitter.NavigateUrl = "https://www.Twitter.com/" + organizationEvent.o.TwitterURL;
            hypTwitter.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organizationEvent.o.YouTubeURL))
        {
            hypYouTube.Text = organizationEvent.o.Name + " YouTube Channel";
            hypYouTube.NavigateUrl = organizationEvent.o.YouTubeURL;
            hypYouTube.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organizationEvent.o.PrimaryPhone))
        {
            dtPrimaryPhone.Visible = true;
            ddPrimaryPhone.Visible = true;
            hypPrimaryPhone.Text = Regex.Replace(organizationEvent.o.PrimaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
            hypPrimaryPhone.NavigateUrl = "tel:" + organizationEvent.o.PrimaryPhone;
            hypPrimaryPhone.Font.Underline = true;
        }


        if (!String.IsNullOrEmpty(organizationEvent.o.PublicPhoneNumber))
        {
            ddParentPhone.Visible = true;
            hypParentPhone.Text = Regex.Replace(organizationEvent.o.PublicPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
            hypParentPhone.NavigateUrl = "tel:" + organizationEvent.o.PublicPhoneNumber;
            hypParentPhone.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organizationEvent.o.PublicEmail))
        {

            ddParentEmail.Visible = true;
            hypParentEmail.Text = organizationEvent.o.PublicEmail;
            hypParentEmail.NavigateUrl = "mailto:" + organizationEvent.o.PublicEmail;
            hypParentEmail.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organizationEvent.o.Website))
        {
            dtWebsite.Visible = true;
            ddWebsite.Visible = true;
            hypWebsite.Text = organizationEvent.o.Website;
            hypWebsite.NavigateUrl = organizationEvent.o.Website;
            hypWebsite.Font.Underline = true;
        }

        //Is user on this team?
        var userOrganization = from uo in dc.UserOrganizations
                               where uo.UserId == userId && uo.OrganizationId == organizationId && uo.Status== (int)RequestStatus.Approved && uo.Status == (int)RequestStatus.Pending
                               select uo;

        if (userOrganization != null)
        {
            //user is in organization
            userIsInOrganization = true;
        }

        if (userIsInOrganization && User.IsInRole("CaseManager"))
        {
            hypViewCases.Visible = true;
            hypAddCase.Visible = true;
        }

        if (!isActive)
        {
            //Campaign is no longer active, hide all volunteer links.
            hypCauseIsNotActive.Visible = true;
            hypCauseIsNotActive.Text = "This deployment is no longer active.";
        }
        else
        {
            lbVolunteer.Visible = true;
            lbVolunteer.NavigateUrl = "/SignUp/" + organizationEvent.oe.URLFriendlyCampaignName;

            if (User.Identity.IsAuthenticated)
            {
                //If the user is logged in and not in a nonprofit already then send to choose a nonprofit.
                //var userOrganizationEvent = from uoe in dc.UserOrganizationEvents
                //					   where uoe.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
                //					   && uoe.OrganizationEventId == organizationEvent.oe.OrganizationEventId
                //					   && uoe.DeactivatedOn == null
                //					   orderby uoe.CreatedOn descending
                //						select uoe;

                //if (userOrganizationEvent.Count() == 0)
                //{
                //	//Tell the user they can choose this nonprofit to volunteer with, not volunteering for this.
                //	volunteerLink = "/V1/Profile/EditNonProfitCauses.aspx?userOrganizationEvent=true&organizationEventId=" + organizationEvent.oe.OrganizationEventId;
                //}
                //else
                //{
                //	//User is already volunteering for this nonprofit, show that message and disable the volunteer button.
                //	lbVolunteer.Visible = false;
                //	btnActiveVolunteer.Text = "You are volunteering for this deployment.";
                //	btnActiveVolunteer.Visible = true;
                //	btnActiveVolunteer.Enabled = false;
                //}

            }
            else
            {
                volunteerLink = "/Register/" + organizationId;
            }

            string CampaignId = dc.DonationCampaigns.Where(x => x.OrganizationEventId == new Guid(organizationEventId)).Select(x => x.DonationCampaignId.ToString()).FirstOrDefault();

            if (!String.IsNullOrEmpty(CampaignId))
            {
                lbDonate.Visible = true;
                lbDonate.PostBackUrl = string.Format("/V1/NonProfit/DonationDetails.aspx?organizationId={0}&donationCampaignId={1}", organizationId, CampaignId);
                donateLink = lbDonate.PostBackUrl;
            }
            else //if (!String.IsNullOrEmpty(organizationEvent.o.DonationURL))
            {
                lbDonate.Visible = true;
                lbDonate.PostBackUrl = string.Format("/V1/NonProfit/Donation.aspx?organizationId={0}", organizationId);
                donateLink = lbDonate.PostBackUrl;
                //donateLink = organizationEvent.o.DonationURL;
            }


            if (!String.IsNullOrEmpty(organizationEvent.oe.HelpURL))
            {
                lbGetHelp.Visible = true;
                getHelpLink = organizationEvent.oe.HelpURL;
            }
        }
    }
    protected void rpNonProfitPeople_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RepeaterItem dataItem = (RepeaterItem)e.Item;
            Guid userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
            String firstname = (String)DataBinder.Eval(dataItem.DataItem, "Firstname");
            String lastname = (String)DataBinder.Eval(dataItem.DataItem, "Lastname");
            String zelloName = (String)DataBinder.Eval(dataItem.DataItem, "ZelloName");
            String title = (String)DataBinder.Eval(dataItem.DataItem, "Title");
            DateTime lastOnlineActiveDate = (DateTime)DataBinder.Eval(dataItem.DataItem, "LastLoginDate");

            Literal lblInfo = (Literal)e.Item.FindControl("lblInfo");

            DateTime lastLoginDate = DateTime.Now;
            double timeSpanHoursInt = 0;
            double timeSpanMinutesInt = 0;
            double timeSpanDaysInt = 0;
            string signedInInfo = string.Empty;

            string activedMessage = string.Empty;
            string deActivedMessage = string.Empty;

            string activeColorClass = string.Empty;
            string totalTimeToday = string.Empty;

            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            //Is the user signed in?
            var timesheet = (from t in dc.Timesheets
                             join tt in dc.TaskTypes on t.TaskTypeId equals tt.TaskTypeId into timeJoin
                             from time in timeJoin.DefaultIfEmpty()
                             where t.UserId == userId && t.TimeIn != null
                             orderby t.TimeIn descending
                             select new { t.TimeIn, t.TimeOut, t.Description, taskname = time.Name }).Take(1).SingleOrDefault();

            if (timesheet != null)
            {
                //If user has no timeout and more than 8 hours has passed then...
                TimeSpan? spanToday = (DateTime.Now - timesheet.TimeIn);
                timeSpanDaysInt = spanToday.Value.TotalDays;
                timeSpanHoursInt = spanToday.Value.TotalHours;
                timeSpanMinutesInt = spanToday.Value.Minutes;

                activedMessage = "<span class='" + activeColorClass + "'>Active at: " + timesheet.TimeIn.ToLongTimeString() + " " + timesheet.TimeIn.ToLongDateString() + "</span>";

                if (timesheet.TimeOut == null)
                {
                    //User is either signed in or they forgot to sign out.
                    if (timeSpanHoursInt > 8)
                    {
                        signedInInfo = "<span class='text-danger'>Forgot to sign out.</span><br>";
                        //PROBLEM - User signed in more than 8 hours ago and did not sign out, show how many days/hours.
                        activeColorClass = "text-danger";
                        deActivedMessage = "<br>De-Activated at: <i>User has been Active for more than 8 hours.</i>";
                        totalTimeToday = "<span class='text-danger'>User did not De-Activate. They have been Active for more than 8 hours.</span>";
                    }
                    else if (timeSpanHoursInt <= 8)
                    {
                        signedInInfo = "<span class='text-success'>Currently Tracking Time</span><br>";
                        //User recently signed in.//GOOD ZONE USER IS SIGNED IN AND HAS BEEN SO FOR LESS THAN 8 HOURS
                        //User signed in for today and has been active for less than 8 hours.
                        deActivedMessage = "<br>De-Activated at: <i><small>User Activated less than 8 hours ago.</small></i>";
                        activeColorClass = "text-success";
                        totalTimeToday = "<span class='text-success'>" + (spanToday.Value.Hours > 0 ? spanToday.Value.Hours + " hours " + timeSpanMinutesInt + " minutes " : timeSpanMinutesInt + " minutes ") + "</span>";
                    }
                }
                else
                {
                    //User has a sign out that matches their sign in.
                    signedInInfo = "<span>Not Tracking Time</span><br>";
                    spanToday = (timesheet.TimeOut - timesheet.TimeIn);
                    activeColorClass = string.Empty;
                    deActivedMessage = "<br>De-Activated at: " + timesheet.TimeOut.Value.ToLongTimeString() + " " + timesheet.TimeOut.Value.ToLongDateString();
                    totalTimeToday = "<span class='text-success'>" + (spanToday.Value.Hours > 0 ? spanToday.Value.Hours + " hours " + spanToday.Value.Minutes + " minutes " : spanToday.Value.Minutes + " minutes ") + "</span>";
                }

                signedInInfo = signedInInfo + activedMessage + deActivedMessage;
            }

            zelloName = String.IsNullOrEmpty(zelloName) ? "none" : zelloName;
            title = String.IsNullOrEmpty(title) ? "none" : title;
            string active = (GetElapsedTime(lastOnlineActiveDate).Substring(0, 1) == "-" ? "<span class='text-success'>Online Now</span> <i class=\"fa fa-wifi text-success\"></i>" : "Last Online: " + GetElapsedTime(lastOnlineActiveDate) + " <i class=\"fa fa-wifi text-muted\"></i>");
            lblInfo.Text = "<dl class=\"dl-vertical\"><dt><b><a href=\"/V1/Member/Default.aspx?userId=" + userId.ToString() + "\"  style=\"text-decoration:underline;\">" + firstname + " " + lastname + "</a></b> <small>" + active + "</small></dt><dd><small>" + title + "</small></dd><dd><small>" + signedInInfo + "</small></dd><dd><small>Last Active Total: " + totalTimeToday + "</small></dd></dl>";









            //HyperLink hypMakeOwner = (HyperLink)e.Item.FindControl("hypMakeOwner");
            //hypMakeOwner.NavigateUrl = "NonProfit.aspx?organizationId=" + Request.QueryString["organizationId"] +"&ownerId=" + userId.ToString();
            //hypMakeOwner.Text = "Set '" + firstname + "' As Owner";
            //if (User.IsInRole("Administrator"))
            //{
            //    hypMakeOwner.Visible = true;
            //}
        }
    }

    [WebMethod]

    public static int SendSms(string eventId, string smsMessage)
    {
        smsMessage = Regex.Replace(smsMessage, "<.*?>", string.Empty);
        try
        {
            using (var dc = new CrowdReliefDBDataContext())
            {
                Guid parsedEventId;
                if (!Guid.TryParse(eventId, out parsedEventId))
                    return 0;
                var phoneNumbers = (
                    from u in dc.UserOrganizationEvents
                    join p in dc.Profiles on u.UserId equals p.UserId
                    where u.OrganizationEventId == parsedEventId && p.ReceiveSMSNotifications == true
                    select p.PhoneNumber
                ).Distinct().ToList();

                if (!phoneNumbers.Any())
                    return 0;

                // Read Twilio config
                string accountSid = ConfigurationManager.AppSettings["twilioAccountSID"];
                string authToken = ConfigurationManager.AppSettings["twilioAuthToken"];
                string fromNumber = ConfigurationManager.AppSettings["twilioPhoneNumber"];

                var tools = new Tools(accountSid, authToken, fromNumber);

                foreach (var phoneNumber in phoneNumbers)
                {
                    tools.SendSms(smsMessage, new string[] { phoneNumber });
                }

                return phoneNumbers.Count;

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    [System.Web.Services.WebMethod]

    public static int SendEmail(string eventId, string smsMessage)
    {
        int sentCount = 0;
        try
        {
            using (var dc = new CrowdReliefDBDataContext())
            {

                var userEmails = (
                    from u in dc.UserOrganizationEvents
                    join m in dc.aspnet_Memberships on u.UserId equals m.UserId
                    join p in dc.Profiles on u.UserId equals p.UserId
                    where u.OrganizationEventId == Guid.Parse(eventId) && p.ReceiveEmailNotifications == true
                    select m.Email
                ).Distinct().ToList();

                foreach (var userEmail in userEmails)
                {
                    if (string.IsNullOrWhiteSpace(userEmail))
                        continue;

                    var emailBodyReplacements = new ListDictionary
                {
                    { "<% Message %>", smsMessage }
                };

                    string error;
                    Tools.SendEmail(
                        smsMessage,
                        "You're Invited! Join Us for the Upcoming Event",
                        emailBodyReplacements,
                        userEmail,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        "~/EmailTemplates/InviteMemberMessage.html",
                        out error
                    );
                    sentCount++;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return sentCount;

    }
}