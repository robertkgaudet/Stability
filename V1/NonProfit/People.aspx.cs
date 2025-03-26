using CrowdRelief;
using GoogleMapsAPI.Places;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class V1_NonProfit_People : BaseOrganizationWebForm
{
    public string _logo;
    public string _teamName;
    public string _teamSquareLogo;
    public string _teamDescription;
    public string _pageName;
    public string _organizationId;
    public string _nonProfitDropDown;
    public string _coverImage;
    public string organizationId = string.Empty;
    public string signedInUserFullName = string.Empty;
    public bool isUserOnTeam = false;
    public Guid organizationOwnerId = Guid.Empty;
    public string teamName = string.Empty;
    public string skillId = string.Empty;
    public string resourceId = string.Empty;
    public bool userIsOwner = false;
    public bool hideTeamList = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        organizationId = Request.QueryString["organizationId"];
        skillId = Request.QueryString["skillId"];
        resourceId = Request.QueryString["resourceId"];

        ucTeamFooter.PageName = "peoplePage";
        ucTeamHeader.PageName = "Team Members";
        #region HEADER PROPERTIES
        ////////////////////////
        //BEGIN HEADER PROPERTIES
        ////////////////////////


        string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
        _coverImage = causePhotoFolder + "businesscoverimage.png";

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var organization = (from o in dc.Organizations
                            where o.OrganizationId == new Guid(organizationId)
                            select new { o.Name, o.LogoSquare, o.HideTeamList, o.OwnerId, o.Description, o.Logo, o.CoverImage, o.URLFriendlyName, o.EnableTeamMemberVerification }).SingleOrDefault();
        hiddenManageShowDonateButtonn.Value = organization.EnableTeamMemberVerification == true? "1" : "0";
        string squareLogo = string.Empty;
        if (organization != null)
        {
            if (organization.CoverImage != null)
            {
                //	_coverImage = causePhotoFolder + organization.CoverImage;
            }

            ucTeamHeader.CoverImage = _coverImage;
            ucTeamHeader.TeamDescription = organization.Description;
            ucTeamHeader._teamTitle = organization.Name;

            if (!String.IsNullOrEmpty(organization.LogoSquare))
            {
                squareLogo = "/Impactoid/Images/Logos/" + organization.LogoSquare;
            }
            else
            {
                squareLogo = "/V1/Images/Logo-Placeholder.png";
            }

            Master.PageTitle = organization.Name + " Programs on Stability";
            Master.PageDescription = organization.Description;
            Master.FbDescription = organization.Description;
            Master.FbImage = _coverImage;
            Master.FbSite_name = organization.Name + " Programs on Stability";
            ucTeamHeader.URLFriendlyPageName = organization.URLFriendlyName;
        }

        ucTeamFooter.TeamName = organization.Name;
        ucTeamFooter.OrganizationId = organizationId;
        ucTeamHeader.OrganizationId = organizationId;
        ucTeamHeader.TeamLogo = squareLogo;
        Master.FbImageType = "image/jpg";
        Master.FbURL = Request.Url.AbsoluteUri;

        //ucTeamHeader.Logo = logo;
        //ucTeamHeader.OrganizationId = organizationId;
        //ucTeamHeader.PageName = "Programs";
        //ucTeamHeader.TeamDescription = organization.Description;
        //ucTeamHeader.TeamName = organization.Name;
        //ucTeamHeader.TeamSquareLogo = squareLogo;

        bool isOwner = false;
        if (User.Identity.IsAuthenticated == true)
        {
            var userOrganizationOwner = (from uo in dc.UserOrganizations
                                         join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
                                         where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
                                         && uo.OrganizationId == new Guid(organizationId)
                                         select o).Take(1).SingleOrDefault();

            if (userOrganizationOwner != null)
            {
                if ((userOrganizationOwner.OwnerId != userId))
                {
                    isOwner = true;
                }
            }
        }

        ////////////////////////
        //END HEADER PROPERTIES
        ////////////////////////
        #endregion

        hideTeamList = organization.HideTeamList != null ? (bool)organization.HideTeamList : false;

        if (User.Identity.IsAuthenticated)
        {
            //Is logged in user on this team?

            var userCheck = (from uo in dc.UserOrganizations
                             where uo.UserId == userId
                             && uo.OrganizationId == new Guid(organizationId)
                             select uo).Take(1).SingleOrDefault();

            Profile profile = GetUserProfileByUserId(userId);
            signedInUserFullName = profile.Firstname + " " + profile.Lastname;

            if (userCheck != null)
            {
                //User is on this team.
                isUserOnTeam = true;
                hpanelJoin.Visible = false;
                hpanelMembers.Visible = true;
                hypInviteTeamMembers.Visible = true;
                hypInviteTeamMembers.NavigateUrl = "/V1/NonProfitAdministration/InviteTeam.aspx?organizationId=" + organizationId;
            }

            teamName = organization.Name;
            organizationOwnerId = organization.OwnerId != null ? (Guid)organization.OwnerId : Guid.Empty;
            userIsOwner = organization.OwnerId == userId ? true : false;


            if (hideTeamList && !HttpContext.Current.User.IsInRole("Administrator") && !userIsOwner)
            {
                //HideTeamList is managed by the owner in settings.
                //Hide team list from nonadmin and nonowner
                hpanelMembers.Visible = false;
                divUpdateMessage.Visible = true;
                divFilterMessage.Visible = false;
                litMessage.Text = "<i class=\"fa fa-2x fa-exclamation-circle\"></i><hr>Contact team administrator to see the team list.";
            }
            else if (isUserOnTeam || User.IsInRole("Administrator") || userIsOwner)
            {
                //If user is on team or an admin or the owner they can see this team.
                hypPrintableTeamList.Visible = true;
                hypPrintableTeamList.NavigateUrl = "/V1/NonProfitAdministration/PrintableTeamList.aspx?organizationId=" + organizationId + "&skillId=" + skillId + "&resourceId=" + resourceId;

                //ADMIN SEES ALL USERS



                if (!String.IsNullOrEmpty(skillId))
                {
                    var skillName = (from s in dc.Skills
                                     where s.SkillId == new Guid(skillId)
                                     select new { s.Name }).SingleOrDefault();

                    divFilterMessage.Visible = true;
                    litFilterMessage.Text = "<i class=\"fa fa-2x fa-hand-pointer-o\"></i><hr>Showing team members with the '" + skillName.Name + "' skillset.";
                }
                if (!String.IsNullOrEmpty(resourceId))
                {
                    var resouceName = (from r in dc.Resources
                                       where r.ResourceId == new Guid(resourceId)
                                       select new { r.Name }).SingleOrDefault();

                    divFilterMessage.Visible = true;
                    litFilterMessage.Text = "<i class=\"fa fa-2x fa-truck\"></i><hr>Showing team members with a '" + resouceName.Name + "' as an available resource.";
                }
                hpanelMembers.Visible = true;
                rptVolunteers.DataSource = new List<PeopleList>();
                rptVolunteers.DataBind();
            }
            else
            {
                //User not on team, not an admin so they cannot see the list.
                divUpdateMessage.Visible = true;
                litMessage.Text = "<i class=\"fa fa-2x fa-exclamation-circle\"></i><hr>You must be on this team to see the team members.";
            }
        }
        else
        {
            //User must be signed in to see the list.
            divUpdateMessage.Visible = true;
            litMessage.Text = "<i class=\"fa fa-2x fa-exclamation-circle\"></i><hr><a href=\"\\signin\">Sign in</a> to see the list of team members.";
        }
        if (!IsPostBack)
        {      
            string type = Request.QueryString["type"];
            bool isVisible = (type == "email" || type == "sms");

            // "Select All" checkbox
            chkSelectAll.Visible = isVisible;
            if (type == "email")
            {
                divEmail.Visible = true;
                divSms.Visible = false;
            }
            else if (type == "sms")
            {
                divEmail.Visible = false;
                divSms.Visible = true;
            }
            else
            {
                divEmail.Visible = false;
                divSms.Visible = false;
            }

            LoadEvents();
            LoadDropdowns();
            //SearchButton_Click(SearchButton, EventArgs.Empty); // Calls the search function
            ScriptManager.RegisterStartupScript(this, this.GetType(), "UpdatePanelTrigger", "setTimeout(function() { __doPostBack('" + SearchButton.UniqueID + "', ''); }, 100);", true);

        }
    }
  
    [WebMethod]
    public static string sendSms(string selectedUserIds, string smsMessage)
    {   
        if (!string.IsNullOrEmpty(selectedUserIds) && !string.IsNullOrEmpty(smsMessage))
        {
            string[] userIds = selectedUserIds.Split(',');
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                foreach (string userId in userIds)
                {
                    Guid userGuid = new Guid(userId);
                    var phoneNumber = (from p in dc.Profiles
                                       where p.UserId == userGuid
                                       select p.PhoneNumber).FirstOrDefault();
                    if (!string.IsNullOrEmpty(phoneNumber))
                    {
                        string accountSid = ConfigurationManager.AppSettings["twilioAccountSID"].ToString();
                        string authToken = ConfigurationManager.AppSettings["twilioAuthToken"].ToString();
                        string fromNumber = ConfigurationManager.AppSettings["twilioPhoneNumber"].ToString();
                        var tools = new Tools(accountSid, authToken, fromNumber);
                        tools.SendSms(smsMessage, new string[] { phoneNumber });
                    }
                }
            }
        }
        return "Sms sent successfully!";
    }


    [WebMethod]
    public static string SendEmail(string selectedUserIds, string userMessage)
    {
        if (!string.IsNullOrEmpty(selectedUserIds))
        {
            string[] userIds = selectedUserIds.Split(',');
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                foreach (string userId in userIds)
                {
                    Guid userGuid = new Guid(userId);
                    var userEmail = (from m in dc.aspnet_Memberships
                                     where m.UserId == userGuid
                                     select m.Email).FirstOrDefault();

                    if (!string.IsNullOrEmpty(userEmail))
                    {
                        ListDictionary ldEmailBodyReplacements = new ListDictionary
                        {
                            { "<% UserId %>", userId.ToString() },
                            { "<% Message %>", userMessage },
                            {"<% Email %>",userEmail }
                        };
                        string error = string.Empty;
                        Tools.SendEmail(
                            userMessage,
                            "You Have a New Message on Stability",
                            ldEmailBodyReplacements,
                            "robertkgaudet@gmail.com",
                            "Stability User Alert",
                            string.Empty,
                            string.Empty,
                            "~\\EmailTemplates\\TeamMemberMessage.html",
                            out error
                        );
                    }
                  
                }
            }
       
        }


        return "Emails sent successfully!";

    }

    protected void rptVolunteers_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {

            RepeaterItem dataItem = (RepeaterItem)e.Item;



            Guid userId = Guid.Empty;
            if (DataBinder.Eval(dataItem.DataItem, "UserId") != null)
            {
                userId = (Guid)DataBinder.Eval(dataItem.DataItem, "UserId");
            }
            String firstname = (String)DataBinder.Eval(dataItem.DataItem, "Firstname");
            String lastname = (String)DataBinder.Eval(dataItem.DataItem, "Lastname");
            String zelloName = (String)DataBinder.Eval(dataItem.DataItem, "ZelloName");
            String title = (String)DataBinder.Eval(dataItem.DataItem, "Title");
            String loweredEmail = (String)DataBinder.Eval(dataItem.DataItem, "LoweredEmail");
            String phoneNUmber = (String)DataBinder.Eval(dataItem.DataItem, "phoneNUmber");
            String description = (String)DataBinder.Eval(dataItem.DataItem, "Description");
            V1_UserControls_TeamLogo ucTeamLogo = (V1_UserControls_TeamLogo)e.Item.FindControl("ucUserNameWithBadges");
            if (ucTeamLogo != null)
            {
                ucTeamLogo.UserId = userId;
                ucTeamLogo.LoadNameWithBadges();
            }
            MembershipUser profileUser = Membership.GetUser(userId);
            bool isLockedOut = false;
            HtmlGenericControl divFooter = (HtmlGenericControl)e.Item.FindControl("divFooter");
            Button btnContact = (Button)e.Item.FindControl("btnContact");
            HtmlGenericControl h5Container = (HtmlGenericControl)e.Item.FindControl("h5Container");
            Button btnManage = (Button)e.Item.FindControl("btnManage");
            Literal litVettingInfo = (Literal)e.Item.FindControl("litVettingInfo");
            Literal litActiveDate = (Literal)e.Item.FindControl("litActiveDate");
            if (User.IsInRole("Administrator") || userIsOwner || User.IsInRole("Team Administrator"))
            {
                bool? vettingComplete = (bool?)DataBinder.Eval(dataItem.DataItem, "VettingComplete");
                bool? passedVetting = (bool?)DataBinder.Eval(dataItem.DataItem, "PassedVetting");
                bool? vettingActive = (bool?)DataBinder.Eval(dataItem.DataItem, "VettingActive");
                //bool? isLockedOut = (bool?)DataBinder.Eval(dataItem.DataItem, "IsLockedOut");
                String vettingNotes = (String)DataBinder.Eval(dataItem.DataItem, "VettingNotes");
                DateTime? dateVettingCompleted = (DateTime?)DataBinder.Eval(dataItem.DataItem, "DateVettingCompleted");
                DateTime? dateVettingStarted = (DateTime?)DataBinder.Eval(dataItem.DataItem, "DateVettingStarted");
                DateTime? lastActivityDate = (DateTime?)DataBinder.Eval(dataItem.DataItem, "LastLoginDate");
                divFooter.Visible = true;
                btnContact.Visible = true;
                if (btnContact.Visible || btnManage.Visible)
                {
                    h5Container.Style["display"] = "flex";
                }
                else
                {
                    h5Container.Style.Remove("display");
                }
                btnManage.Visible = true;
                isLockedOut = !profileUser.IsApproved;
                vettingComplete = vettingComplete == null ? false : vettingComplete;
                passedVetting = passedVetting == null ? false : passedVetting;
                vettingActive = vettingActive == null ? false : vettingActive;
                DateTime dateVettingCompletedString = dateVettingCompleted == null ? DateTime.MinValue : (DateTime)dateVettingCompleted;
                DateTime dateVettingStartedString = dateVettingStarted == null ? DateTime.MinValue : (DateTime)dateVettingStarted;
                DateTime lastActivityDateString = lastActivityDate == null ? DateTime.MinValue : (DateTime)lastActivityDate;
                String dateVettingComplete = dateVettingCompletedString == DateTime.MinValue ? "Not Complete" : dateVettingCompletedString.ToLongDateString();
                String dateVettingStarts = dateVettingStartedString == DateTime.MinValue ? "Not Started" : dateVettingStartedString.ToLongDateString();
                String lastActivitysDate = lastActivityDateString == DateTime.MinValue ? "Not Started" : lastActivityDateString.ToShortDateString() + " " + lastActivityDateString.ToLongDateString() + " at " + lastActivityDateString.ToLongTimeString();
                string vettingCompleted = (bool)vettingComplete ? "VETTING COMPLETE: " + ((bool)passedVetting ? "<span style='color:yellowgreen'>PASSED</span>" : "<span style='color:orange'>FAILED</span>") : (bool)vettingActive ? "VETTING: PENDING" : "VETTING: NO ACTION TAKEN";
                vettingCompleted += ((bool)isLockedOut ? "<br><span style='color:orange'>LOCKED OUT</span>" : "<br><span style='color:yellowgreen'>HAS ACCESS</span>") + ("<br>Notes:" + vettingNotes + "<br>Date Started: " + dateVettingStarts + "<br>Date Completed: " + dateVettingComplete + "<br>Last Activity Date: " + lastActivitysDate + "<br>Phone Number: " + phoneNUmber + "<br>Email: " + loweredEmail);

                litVettingInfo.Text = vettingCompleted;
            }
            HyperLink hypName = (HyperLink)e.Item.FindControl("hypName");
            Literal litMemberInfo = (Literal)e.Item.FindControl("litMemberInfo");
            Literal litDescription = (Literal)e.Item.FindControl("litDescription");
            Literal litSkills = (Literal)e.Item.FindControl("litSkills");
            Literal litResources = (Literal)e.Item.FindControl("litResources");
            btnContact.Attributes.Add("data-name", firstname + " " + lastname);
            btnContact.Attributes.Add("data-email", loweredEmail);
            btnContact.Attributes.Add("onclick", "return btnClick(this);");

            title = !String.IsNullOrEmpty(title) ? title + "</br>" : "";
            //zelloName = !String.IsNullOrEmpty(zelloName) ? " Zello: " + zelloName + "</br>" : "";
            description = !String.IsNullOrEmpty(description) ? description + "</br>" : "";

            litMemberInfo.Text = title;
            string skills = GetSkills(userId);
            string resources = GetResources(userId);
            litSkills.Text = !String.IsNullOrEmpty(skills) ? "<h6>Skills:</h6> " + skills + "</br>" : "";
            litResources.Text = !String.IsNullOrEmpty(resources) ? "<h6>Resources:</h6> " + resources + "</br>" : "";
            litDescription.Text = description;

        }
    }
    protected string GetResources(Guid userId)
    {
        string resourceList = string.Empty;

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var resources = from ur in dc.UserResources
                        join r in dc.Resources on ur.ResourceId equals r.ResourceId
                        where ur.UserId == userId
                        select r;


        foreach (var resource in resources)
        {
            string resourceName = resource.Name;
            string resourceIdLocal = resource.ResourceId.ToString();

            string btnColor = "btn-default";
            if (!String.IsNullOrEmpty(resourceId))
            {
                if (resourceId.Equals(resourceIdLocal, StringComparison.OrdinalIgnoreCase))
                {
                    //Change button color.
                    btnColor = "btn-info";
                }
            }

            resourceList += "<button type=\"button\" id=\"button\" onclick=\"window.location.href='/V1/NonProfit/People.aspx?organizationId=" + organizationId + "&resourceId=" + resourceIdLocal + "'\" class=\"btn btn-xs " + btnColor + " m-xs\">" + resourceName + "</button>";
        }

        return resourceList;
    }
    protected string GetSkills(Guid userId)
    {
        string skillList = string.Empty;

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var skills = from us in dc.UserSkills
                     join s in dc.Skills on us.SkillId equals s.SkillId
                     where us.UserId == userId
                     select s;

        foreach (var skill in skills)
        {
            string skillName = skill.Name;
            string skillIdLocal = skill.SkillId.ToString();

            string btnColor = "btn-default";
            if (!String.IsNullOrEmpty(skillId))
            {
                if (skillId.Equals(skillIdLocal, StringComparison.OrdinalIgnoreCase))
                {
                    //Change button color.
                    btnColor = "btn-info";
                }
            }

            skillList += "<button type=\"button\" id=\"button\" onclick=\"window.location.href='/V1/NonProfit/People.aspx?organizationId=" + organizationId + "&skillId=" + skillIdLocal + "'\" class=\"btn btn-xs " + btnColor + " m-xs\">" + skillName + "</button>";
        }

        return skillList;
    }
    private List<string> GetSelectedValues(ListBox listBox)
    {
        return listBox.Items.Cast<ListItem>()
            .Where(i => i.Selected)
            .Select(i => i.Value)
            .ToList();
    }
    //private void LoadDropdowns()
    //{
    //    using (var dc = new CrowdReliefDBDataContext())
    //    {
    //        var skills = dc.Skills.Select(s => new { s.SkillId, s.Name }).ToList();
    //        ddlSkills.DataTextField = "Name";
    //        ddlSkills.DataValueField = "SkillId";
    //        ddlSkills.DataSource = skills;
    //        ddlSkills.DataBind();
    //        var resources = dc.Resources.Select(r => new { r.ResourceId, r.Name }).ToList();
    //        ddlResources.DataTextField = "Name";
    //        ddlResources.DataValueField = "ResourceId";
    //        ddlResources.DataSource = resources;
    //        ddlResources.DataBind();
    //    }

    //}
    private void LoadDropdowns()
    {
        using (var dc = new CrowdReliefDBDataContext())
        {
            var skills = dc.Skills.Select(s => new { s.SkillId, s.Name }).ToList();
            ddlSkills.DataTextField = "Name";
            ddlSkills.DataValueField = "SkillId";
            ddlSkills.DataSource = skills;
            ddlSkills.DataBind();
            var resources = dc.Resources.Select(r => new { r.ResourceId, r.Name }).ToList();
            ddlResources.DataTextField = "Name";
            ddlResources.DataValueField = "ResourceId";
            ddlResources.DataSource = resources;
            ddlResources.DataBind();
            Guid orgId;
            if (Guid.TryParse(organizationId, out orgId))
            {
                var positions = (from pos in dc.Positions
                                 where pos.OrganizationId == orgId
                                 select new
                                 {
                                     pos.PositionId,
                                     pos.Name
                                 }).ToList();
                ddlTraining.DataSource = positions;
                ddlTraining.DataTextField = "Name";
                ddlTraining.DataValueField = "PositionId";
                ddlTraining.DataBind();
            }
            ddlTraining.Items.Insert(0, new ListItem("Select Training", ""));
        }
    }
    private void LoadEvents()
    {
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var events = (from ev in dc.Events
                          where ev.IsActive == true
                          select new
                          {
                              ev.EventId,
                              ev.Name
                          }).ToList();
            ddlEvent.DataSource = events;
            ddlEvent.DataTextField = "Name";
            ddlEvent.DataValueField = "EventId";
            ddlEvent.DataBind();
            ddlEvent.Items.Insert(0, new ListItem("  Select Location ", ""));

        }

    }

    private DateTime? ParseDate(string dateText)
    {
        DateTime parsedDate;
        string[] expectedFormats = { "MM/dd/yyyy" };

        if (DateTime.TryParseExact(dateText, expectedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            return parsedDate.Date;
        }

        return null;
    }
    protected void SearchButton_Click(object sender, EventArgs e)
    {
        if (isUserOnTeam || User.IsInRole("Administrator") || userIsOwner)
        {
            string selectedTraining = ddlTraining.SelectedValue;
            int selectedIndex = ddlTraining.SelectedIndex;

            //If user is on team or an admin or the owner they can see this team.
            List<string> selectedSkills = GetSelectedValues(ddlSkills);
            List<string> selectedResources = GetSelectedValues(ddlResources);
            string selectedLocation = Request.Form["selectedEvent"] ?? ddlEvent.SelectedValue;
            int selectedRadius = 0;
            int radius;
            if (int.TryParse(Request.Form["radiusSlider"], out radius))

            {
                selectedRadius = radius;
            }

            bool emailConnected = txtEmailconnect.Checked;
            bool isVerified = txtIsVerified.Checked;
            bool isVetted = txtIsVetted.Checked;
            bool optedSMS = txtOptedSMS.Checked;

            string startDateText = Request.Form[StartDate.UniqueID];
            string endDateText = Request.Form[EndDate.UniqueID];

            DateTime? startDate = ParseDate(startDateText);
            DateTime? endDate = ParseDate(endDateText);
            string nameSearchTerm = filter.Text.Trim().ToLower();


            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                double eventLatitude = 0, eventLongitude = 0;
                Guid? eventGuid = null;
                Guid parsedEventGuid;
                if (!string.IsNullOrEmpty(selectedLocation) && Guid.TryParse(selectedLocation, out parsedEventGuid))
                {
                    eventGuid = parsedEventGuid;
                    var selectedEvent = dc.Events.FirstOrDefault(ea => ea.EventId == eventGuid.Value);
                    if (selectedEvent != null)
                    {
                        double.TryParse(selectedEvent.Latitude, out eventLatitude);
                        double.TryParse(selectedEvent.Longitude, out eventLongitude);
                    }
                }

                var selectedSkillsParam = !selectedSkills.Any() ? skillId == null ? "" : skillId : string.Join(",", selectedSkills);
                var selectedResourcesParam = !selectedResources.Any() ? resourceId == null ? "" : resourceId : string.Join(",", selectedResources);
                var nameSearchTermParam = string.IsNullOrEmpty(nameSearchTerm) ? "" : nameSearchTerm;

                // Execute stored procedure and return mapped results
                dc.CommandTimeout = 300;
                var result = dc.ExecuteQuery<PeopleList>(
                    "EXEC GetPeopleList {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}", organizationId, startDate == null ? "" : startDate.Value.ToString("yyyy-MM-dd"), endDate == null ? "" : endDate.Value.ToString("yyyy-MM-dd"), selectedSkillsParam, selectedResourcesParam, nameSearchTermParam, selectedTraining, eventLatitude, eventLongitude, selectedRadius, emailConnected, isVetted, optedSMS).ToList();				

                // Show Filter Message if Any Filter Applied
                divFilterMessage.Visible = selectedSkills.Any() || selectedResources.Any() || emailConnected || isVerified || isVetted || optedSMS;
                litFilterMessage.Text = divFilterMessage.Visible ? "<i class='fa fa-2x fa-filter'></i><hr>Filtered by selected options." : "";

                if (isUserOnTeam && !User.IsInRole("Administrator") && !userIsOwner)
                {
                    //User is on team but it's not the admin or team owner so limit what they can see.
                    //Hide unapproved users and unvetted users
                    result = result.Where(x => x.IsApproved == true && x.PassedVetting == true).ToList();
                }

                // Bind Data.
                currentPageValue.Value = currentPageValue.Value == "" ? "1" : currentPageValue.Value;
                var pNumber = Convert.ToInt32(currentPageValue.Value);
                totalPageValue.Value = Convert.ToString(Math.Ceiling((double)result.Count / 20));
                rptVolunteers.DataSource = result.Skip(20 * (pNumber - 1)).Take(20);
                rptVolunteers.DataBind();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CallMyFunction", "updatePagination();", true);
            }
        }

    }


    protected void ClearButton_Click(object sender, EventArgs e)
    {
        ddlSkills.SelectedIndex = -1;
        ddlResources.SelectedIndex = -1;
        StartDate.Text = string.Empty;
        EndDate.Text = string.Empty;
        txtIsVetted.Checked = false;
        txtOptedSMS.Checked = false;
        txtEmailconnect.Checked = false;
        txtIsVerified.Checked = false;
        rptVolunteers.DataSource = null;
        divFilterMessage.Visible = false;
        litFilterMessage.Text = string.Empty;
        filter.Text = string.Empty;
        ddlTraining.Text = string.Empty;
        ddlEvent.Text = string.Empty;
    }
}

internal class PeopleList
{
    public string Firstname { get; set; }
    public DateTime CreateDate { get; set; }
    public string Description { get; set; }
    public string LoweredEmail { get; set; }
    public string PhoneNumber { get; set; }
    public string Lastname { get; set; }
    public Guid UserId { get; set; }
    public DateTime? DateVettingCompleted { get; set; }
    public DateTime? DateVettingStarted { get; set; }
    public string VettingNotes { get; set; }
    public bool? VettingActive { get; set; }
    public bool? VettingComplete { get; set; }
    public bool? PassedVetting { get; set; }
    public string Title { get; set; }
    public string ZelloName { get; set; }
    public DateTime LastLoginDate { get; set; }
    public DateTime LastActivityDate { get; set; }
    public bool? IsApproved { get; set; }

    public PeopleList(string firstname, DateTime createDate, string description, string loweredEmail, string phoneNumber, string lastname, Guid userId, DateTime? dateVettingCompleted, DateTime? dateVettingStarted, string vettingNotes, bool? vettingActive, bool? vettingComplete, bool? passedVetting, string title, string zelloName, DateTime lastLoginDate, DateTime lastActivityDate, bool? isApproved)
    {
        Firstname = firstname;
        CreateDate = createDate;
        Description = description;
        LoweredEmail = loweredEmail;
        PhoneNumber = phoneNumber;
        Lastname = lastname;
        UserId = userId;
        DateVettingCompleted = dateVettingCompleted;
        DateVettingStarted = dateVettingStarted;
        VettingNotes = vettingNotes;
        VettingActive = vettingActive;
        VettingComplete = vettingComplete;
        PassedVetting = passedVetting;
        Title = title;
        ZelloName = zelloName;
        LastLoginDate = lastLoginDate;
        LastActivityDate = lastActivityDate;
        IsApproved = isApproved;
    }

    public PeopleList()
    {

    }
    public override bool Equals(object obj)
    {
        PeopleList other = obj as PeopleList;
        return !ReferenceEquals(other, null) &&
               Firstname == other.Firstname &&
               CreateDate == other.CreateDate &&
               Description == other.Description &&
               LoweredEmail == other.LoweredEmail &&
               Lastname == other.Lastname &&
               UserId.Equals(other.UserId) &&
               DateVettingCompleted == other.DateVettingCompleted &&
               DateVettingStarted == other.DateVettingStarted &&
               VettingNotes == other.VettingNotes &&
               VettingActive == other.VettingActive &&
               VettingComplete == other.VettingComplete &&
               PassedVetting == other.PassedVetting &&
               Title == other.Title &&
               ZelloName == other.ZelloName &&
               LastLoginDate == other.LastLoginDate &&
               LastActivityDate == other.LastActivityDate &&
               IsApproved == other.IsApproved;
    }

    public override int GetHashCode()
    {
        int hashCode = -770893105;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Firstname);
        hashCode = hashCode * -1521134295 + CreateDate.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LoweredEmail);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Lastname);
        hashCode = hashCode * -1521134295 + UserId.GetHashCode();
        hashCode = hashCode * -1521134295 + DateVettingCompleted.GetHashCode();
        hashCode = hashCode * -1521134295 + DateVettingStarted.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VettingNotes);
        hashCode = hashCode * -1521134295 + VettingActive.GetHashCode();
        hashCode = hashCode * -1521134295 + VettingComplete.GetHashCode();
        hashCode = hashCode * -1521134295 + PassedVetting.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ZelloName);
        hashCode = hashCode * -1521134295 + LastLoginDate.GetHashCode();
        hashCode = hashCode * -1521134295 + LastActivityDate.GetHashCode();
        hashCode = hashCode * -1521134295 + IsApproved.GetHashCode();
        return hashCode;
    }
}