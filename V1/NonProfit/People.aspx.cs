using CrowdRelief;
using GoogleMapsAPI.Places;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using Stripe.Climate;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Twilio.TwiML.Voice;
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
    public static string organizationId = string.Empty;
    public string signedInUserFullName = string.Empty;
    public Guid organizationOwnerId = Guid.Empty;
    public string teamName = string.Empty;
    public string skillId = string.Empty;
    public static Guid pevUserId = Guid.Empty;
    public static Guid newUserId = Guid.Empty;
    public static Guid RemoveTeamMemberId = Guid.Empty;
    public string resourceId = string.Empty;
    public bool hideTeamList = false;
    private int pageSize = 50;
    private int pageNumber = 1;

	public bool isSiteAdministrator = false;
	public bool isTeamAdministrator = false;
	public bool isTeamOwner = false;
	public bool isTeamMember = false;
	protected void Page_Load(object sender, EventArgs e)
	{
		//DETERMINE ONE OF THREE ROLES ON THIS PAGE
		//DETERMINE IF TEAM OWNER HAS DECIDED TO HIDE THE TEAM LIST IN THE TEAM SETTINGS
		//DETERMINE IF THIS IS THE USERS PRIMARY TEAM

		// isOwner
		//Use TeamOwner / miller69 and CREATE a team called Team Member Test Team
		//Team Owner - OwnerId on the UserOrganization table.

		// isTeamMember
		//Use TeamMember / miller69 and JOIN a team called Team Member Test Team
		//Team Member - UserId on the UserOrganization table.
		//TeamMember / miller69

		// isTeamAdministrator
		//Use TeamAdministrator / miller69 and BECOME the ADMIN of a team called Team Member Test Team
		//Team Administrator -IsTeamAdministrator on the UserOrganization table.
		//TeamAdministrator / miller69

		// Determine isPrimaryTeam

		string squareLogo = "/V1/Images/Logo-Placeholder.png";
		txtsms.Attributes["maxlength"]	= "450";
        organizationId					= Request.QueryString["organizationId"];
        skillId							= Request.QueryString["skillId"];
        resourceId						= Request.QueryString["resourceId"];
        ucTeamFooter.PageName			= "peoplePage";
        ucTeamHeader.PageName			= "Team Members";

        #region HEADER PROPERTIES
        ////////////////////////
        //BEGIN HEADER PROPERTIES
        ////////////////////////
        string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();


		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select new { o.Name, o.LogoSquare, o.HideTeamList, o.OwnerId, o.Description, o.Logo, o.CoverImage, o.URLFriendlyName, o.EnableTeamMemberVerification }).SingleOrDefault();


		phAdminControls.Visible = false;
		hiddenAdminRole.Value = "0";
		btnRemoveTeamMember.Visible = false;

        if (organization != null)
        {
			//USERS HERE DO NOT HAVE TO BE SIGNED IN

            //SET THE COVER IMAGE
			_coverImage = !string.IsNullOrEmpty(organization.CoverImage) ? causePhotoFolder + organization.CoverImage : causePhotoFolder + "businesscoverimage.png";

            ucTeamHeader.CoverImage = _coverImage;
            ucTeamHeader.TeamDescription = organization.Description;
            ucTeamHeader._teamTitle = organization.Name;
            if (!string.IsNullOrEmpty(organization.LogoSquare))
            {
                string virtualPath_square = "/Impactoid/Images/Logos/" + organization.LogoSquare;
                string physicalPath_square = Server.MapPath(virtualPath_square);

                if (System.IO.File.Exists(physicalPath_square))
                {
                    squareLogo = virtualPath_square;
                }
            }

            Master.PageTitle				= organization.Name + " Team Members on Stability";
            Master.PageDescription			= organization.Description;
            Master.FbDescription			= organization.Description;
            Master.FbImage					= _coverImage;
            Master.FbSite_name				= organization.Name + " Team Members on Stability";
			Master.FbImageType				= "image/jpg";
			Master.FbURL					= Request.Url.AbsoluteUri;

            ucTeamHeader.URLFriendlyPageName = organization.URLFriendlyName;
			ucTeamHeader.OrganizationId		= organizationId;
			ucTeamHeader.TeamLogo			= squareLogo;
			ucTeamFooter.TeamName			= organization.Name;
			ucTeamFooter.OrganizationId		= organizationId;
			teamName						= organization.Name;

			//ucTeamHeader.Logo = logo;
			//ucTeamHeader.OrganizationId = organizationId;
			//ucTeamHeader.PageName = "Programs";
			//ucTeamHeader.TeamDescription = organization.Description;
			//ucTeamHeader.TeamName = organization.Name;
			//ucTeamHeader.TeamSquareLogo = squareLogo;  

			////////////////////////
			//END HEADER PROPERTIES
			////////////////////////
			#endregion

			if (User.Identity.IsAuthenticated == true)
			{
				//GET CURRENT USERS NAME FOR EMAILS BEING SENT
				Profile profile = GetUserProfileByUserId(userId);
				signedInUserFullName = profile.Firstname + " " + profile.Lastname;

				//IS TEAM HIDDEN BY ADMINS?
				hideTeamList = organization.HideTeamList != null ? (bool)organization.HideTeamList : false;

				//FIRST SET IsSiteAdministrator, IsTeamMember, IsTeamOwner and IsTeamAdministrator

				isSiteAdministrator = User.IsInRole("Administrator");

				//SET AS TEAM MEMBER
				isTeamMember = dc.UserOrganizations.Any(uo => uo.OrganizationId == new Guid(organizationId) && uo.UserId == userId && (uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending));

				if(isTeamMember)
				{
					//IsTeamMember
					isTeamOwner = dc.Organizations.Any(o => o.OrganizationId == new Guid(organizationId) && o.OwnerId == userId);
					if (isTeamOwner)
					{
						//IsTeamOwner
						organizationOwnerId = userId;
						isTeamMember = true;
						btnteamOwner.Visible = true;
					}

					//IsTeamAdministrator
					isTeamAdministrator = dc.UserOrganizations.Any(uo => uo.OrganizationId == new Guid(organizationId) && uo.UserId == userId && uo.IsTeamAdministrator == true
					&& (uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending));
				
					if (isTeamAdministrator || isTeamOwner || isSiteAdministrator)
					{
						if (organization.EnableTeamMemberVerification == false)
						{
							//THIS IS FOR THE VERIFIED BADGES ON THE TEAM ADMIN PANELS
							//'TEAM VERIFIED'
							//ENABLE FEATURE THAT LET'S ADMINS APPROVE TEAM MEMBERS
							lblTeamVerified.Visible = false;
							cbTeamVerified.Visible = false;
						}

						btnRemoveTeamMember.Visible = true;
						phAdminControls.Visible = true;
						hiddenAdminRole.Value = "1";
					}

					//SHOWS THE TEAM VERIFIED ICON
					hiddenShowTeamVerified.Value = cbTeamVerified.Visible ? "1" : "0";

					//hpanelJoin.Visible = false;
					hypInviteTeamMembers.Visible = true;
					hypInviteTeamMembers.NavigateUrl = "/V1/NonProfitAdministration/InviteTeam.aspx?organizationId=" + organizationId;
				}

				//SHOW TEAM LIST TO ALL SIGNED IN USERS UNLESS TEAM ADMIN HAS HIDDEN IT.
				if (!hideTeamList || isSiteAdministrator || isTeamOwner || isTeamAdministrator)
				{
					//SHOW TEAM LIST
					LoadTeamList();
				}
				else
				{
					//HIDE TEAM LIST
					divTeamListMessage.Visible = true;
					divFilterMessage.Visible = false;
					litMessage.Text = "<i class=\"fa fa-2x fa-exclamation-circle\"></i><hr>Contact a team administrator to see the team list.";
					LoadTeamList();
				}
			}
			else
			{
				//User must be signed in to see the list.
				divTeamListMessage.Visible = true;
				litMessage.Text = "<i class=\"fa fa-2x fa-exclamation-circle\"></i><hr><a href=\"\\signin\">Sign in</a> to see the list of team members.";
			}

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

	public void LoadTeamList()
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		//SHOW TEAM LIST
		if (isSiteAdministrator || isTeamOwner || isTeamAdministrator)
		{
			//USERS WHO CAN PRINT TEAM LIST
			hypPrintableTeamList.Visible = true;
			hypPrintableTeamList.NavigateUrl = "/V1/NonProfitAdministration/PrintableTeamList.aspx?organizationId=" + organizationId + "&skillId=" + skillId + "&resourceId=" + resourceId;
		}

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

		hpanelSearchMembers.Visible = true;
		rptVolunteers.DataSource = new List<PeopleList>();
		rptVolunteers.DataBind();

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
    [WebMethod]
    public static string MakeTeamOwner(Guid selectedUser)
    {
        string orgId = organizationId;
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var organization = dc.Organizations.FirstOrDefault(o => o.OrganizationId == new Guid(orgId));
            Guid oldOwnerId = Guid.Empty;
            if (organization.OwnerId != null)
            {
                oldOwnerId = organization.OwnerId.Value;
                organization.OwnerId = selectedUser;
                dc.SubmitChanges();
                var oldUserOrg = dc.UserOrganizations
                    .FirstOrDefault(x => x.UserId == oldOwnerId && x.OrganizationId == organization.OrganizationId && x.Status == (int)RequestStatus.Approved);

                var newUserOrg = dc.UserOrganizations
                 .FirstOrDefault(x => x.UserId == selectedUser && x.OrganizationId == organization.OrganizationId && x.Status == (int)RequestStatus.Approved);
                var previousOwners = dc.UserOrganizations
               .Where(x => x.OrganizationId == organization.OrganizationId && x.IsPreviousOwner == true && x.Status == (int)RequestStatus.Approved)
               .FirstOrDefault();
                if (previousOwners != null)
                {
                    previousOwners.IsPreviousOwner = false;
                }

                if (oldUserOrg != null)
                {
                    oldUserOrg.IsOwner = false;
                    oldUserOrg.IsPreviousOwner = true;
                }

                if (newUserOrg != null)
                {
                    newUserOrg.IsOwner = true;
                }
                dc.SubmitChanges();
                pevUserId = oldOwnerId;
                newUserId = selectedUser;
                SendTeamOwnerChangeEmail(pevUserId, newUserId, new Guid(organizationId));
            }
            else
            {
                organization.OwnerId = selectedUser;
                dc.SubmitChanges();
                var newUserOrg = dc.UserOrganizations
                .FirstOrDefault(x => x.UserId == selectedUser && x.OrganizationId == organization.OrganizationId && x.Status == (int)RequestStatus.Approved);
                newUserOrg.IsOwner = true;
                dc.SubmitChanges();
            }

        }
        return "Team owner updated successfully.";
    }
    [WebMethod]
    public static string RemoveTeamMember(Guid selectedUser)
    {
        string orgId = organizationId;
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            var userOrg = dc.UserOrganizations
                        .FirstOrDefault(uo => uo.OrganizationId == new Guid(orgId) && uo.UserId == selectedUser);
            var userHistory = dc.UserOrganizationHistories
        .FirstOrDefault(uh => uh.UserOrganizationId == userOrg.UserOrganizationId);
            if (userOrg != null)
            {
                if (userOrg != null)
                {
                    int previousStatus = userOrg.Status;
                    if (userHistory != null)
                    {

                        userHistory.PreviousStatus = previousStatus;
                        userHistory.StatusChangedOn = DateTime.Now;

                    }
                    else
                    {
                        UserOrganizationHistory history = new UserOrganizationHistory
                        {
                            UserOrganizationHistoryId = Guid.NewGuid(),
                            UserOrganizationId = userOrg.UserOrganizationId,
                            UserId = selectedUser,
                            PreviousStatus = previousStatus,
                            StatusChangedOn = DateTime.Now,
                            DateToReApply = null
                        };
                    }


                    userOrg.Status = (int)RequestStatus.RemovedByAdmin;
                    dc.SubmitChanges();
                }
                RemoveTeamMemberId = selectedUser;
                SendRemoveTeamMemberEmail(RemoveTeamMemberId, new Guid(organizationId));
            }
        }
        return "Team member remove successfully.";
    }
    public static void SendRemoveTeamMemberEmail(Guid RemoveTeamMemberId, Guid organizationId)
    {
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            string orgName = dc.Organizations.Where(o => o.OrganizationId == organizationId).Select(o => o.Name).FirstOrDefault();
            var username = dc.Profiles.Where(p => p.UserId == RemoveTeamMemberId).FirstOrDefault();
            var ownerId = dc.Organizations
                  .Where(o => o.OrganizationId == organizationId)
                  .Select(o => o.OwnerId)
                  .FirstOrDefault();
            var ownername = dc.Profiles.Where(p => p.UserId == ownerId).FirstOrDefault();
            string useremail = "support@stability.org";
            ListDictionary ldEmailBodyReplacements = new ListDictionary
            {
               { "##OrganizationName##", orgName },
               { "##RemovedMemberFirstName##", username.Firstname },
               { "##RemovedMemberLastName##", username.Lastname },
               { "##RemovedMemberEmail##", useremail },
               { "##TeamOwnerFirstName##", ownername.Firstname},
               { "##TeamOwnerLastName##", ownername.Lastname}

            };
            string error = string.Empty;
            Tools.SendEmail(
                "User has been removed from the team - " + orgName,
                ". This is to inform you that a user have been removed from the team " + orgName +
                ". This user no longer has access to team resources or participation privileges.",
                ldEmailBodyReplacements,
                useremail,
				username.Firstname + " " + username.Lastname + " Removed From " + orgName,
                string.Empty,
                string.Empty,
                "~/EmailTemplates/TeamMemberRemove.html",
                out error
            );
        }
    }

    public static void SendTeamOwnerChangeEmail(Guid pevUserId, Guid newUserId, Guid organizationId)
    {
        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            string orgName = dc.Organizations.Where(o => o.OrganizationId == organizationId).Select(o => o.Name).FirstOrDefault();
            var oldusername = dc.Profiles.Where(p => p.UserId == pevUserId).FirstOrDefault();
            var newusername = dc.Profiles.Where(p => p.UserId == newUserId).FirstOrDefault();
            string oldemail = dc.aspnet_Memberships.Where(am => am.UserId == pevUserId).Select(am => am.Email).FirstOrDefault();
            string newemail = dc.aspnet_Memberships.Where(am => am.UserId == newUserId).Select(am => am.Email).FirstOrDefault();
            ListDictionary ldEmailBodyReplacements = new ListDictionary
            {
                {"##OrganizationName##",orgName },
                {"##OldOwnerFirstName##",oldusername.Firstname},
                {"##OldOwnerLastName##",oldusername.Lastname},
                {"##NewOwnerFirstName##",newusername.Firstname},
                {"##NewOwnerLastName##",newusername.Lastname},
                {"##NewOwnerEmail##",newemail}
            };
            string error = string.Empty;
            // Send email to old owner
            Tools.SendEmail(
                           "Team Owner Change for " + orgName,
                           "You have been removed as the team owner of " + orgName,
                           ldEmailBodyReplacements,
                           oldemail,
                           "Team Ownership Change",
                           string.Empty,
                           string.Empty,
                           "~/EmailTemplates/TeamOwnerChanged.html",
                           out error
                       );
            // Send email to new owner
            Tools.SendEmail(
                "Team Owner Change for " + orgName,
                "You are now the team owner of " + orgName,
                ldEmailBodyReplacements,
                newemail,
                "Team Ownership Change",
                "", "",
                "~/EmailTemplates/TeamOwnerChanged.html",
                out error
            );
        }

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
            bool receiveSMSNotifications = DataBinder.Eval(dataItem.DataItem, "ReceiveSMSNotifications") != DBNull.Value &&
                                    (bool)DataBinder.Eval(dataItem.DataItem, "ReceiveSMSNotifications");

            object rankObj = DataBinder.Eval(dataItem.DataItem, "RankPosition");
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
            RadioButtonList rblManageUserStatus = (RadioButtonList)e.Item.FindControl("rblManageUserStatus");
            HtmlGenericControl rankBadge = (HtmlGenericControl)e.Item.FindControl("rankBadge");
            if (rankObj == DBNull.Value || rankObj == null)
            {
                rankBadge.Visible = false;
            }
            else
            {
                rankBadge.InnerText = "RankPosition: " + rankObj.ToString();
                rankBadge.Visible = true;
            }
            if (User.IsInRole("Administrator") || isTeamOwner || isTeamAdministrator)
            {
                bool? vettingComplete = (bool?)DataBinder.Eval(dataItem.DataItem, "VettingComplete");
                bool? passedVetting = (bool?)DataBinder.Eval(dataItem.DataItem, "PassedVetting");
                bool? vettingActive = (bool?)DataBinder.Eval(dataItem.DataItem, "VettingActive");
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
                if (divEmail.Visible == true || divSms.Visible == true)
                {
                    btnContact.Visible = false;
                }
                if (ucTeamLogo != null)
                {
                    ucTeamLogo.UserId = userId;
                    if (btnContact.Visible == false)
                    {
                        ucTeamLogo.ShowPhoneNumber = true;
                        ucTeamLogo.ShowEmail = true;
                    }
                    else
                    {
                        ucTeamLogo.ShowPhoneNumber = false;
                        ucTeamLogo.ShowEmail = false;
                    }
                    ucTeamLogo.LoadNameWithBadges();
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
                // Set up the radio button list for vetting status
                if (rblManageUserStatus != null)
                {
                    rblManageUserStatus.Items.Clear();
                    rblManageUserStatus.Items.Add(new ListItem("Start Vetting", VolunteerStatus.VettingStarted.Value));
                    rblManageUserStatus.Items.Add(new ListItem("Passed Vetting", VolunteerStatus.VettingComplete_Passed.Value));
                    rblManageUserStatus.Items.Add(new ListItem("Failed Vetting", VolunteerStatus.VettingComplete_Failed.Value));

                    // Set the selected value based on current status
                    if (vettingComplete == true)
                    {
                        rblManageUserStatus.SelectedValue = passedVetting == true ?
                            VolunteerStatus.VettingComplete_Passed.Value :
                            VolunteerStatus.VettingComplete_Failed.Value;
                    }
                    else if (vettingActive == true)
                    {
                        rblManageUserStatus.SelectedValue = VolunteerStatus.VettingStarted.Value;
                    }
                }
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

        List<string> selectedResourceIds = new List<string>();
        foreach (ListItem item in ddlResources.Items)
        {
            if (item.Selected)
            {
                selectedResourceIds.Add(item.Value);
            }
        }
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
            if (selectedResourceIds.Contains(resourceIdLocal))
            {
                btnColor = "btn-info";
            }

            resourceList += string.Format(
                "<button type='button' class='btn btn-xs {0} m-xs' data-resource-id='{1}' onclick='onResourceClick(\"{1}\")'>{2}</button>",
                btnColor,
                resourceIdLocal,
                resourceName
            );
        }

        return resourceList;
    }


    protected string GetSkills(Guid userId)
    {
        string skillList = string.Empty;

        List<string> selectedSkillIds = new List<string>();
        foreach (ListItem item in ddlSkills.Items)
        {
            if (item.Selected)
            {
                selectedSkillIds.Add(item.Value);
            }
        }

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

            if (selectedSkillIds.Contains(skillIdLocal))
            {
                btnColor = "btn-info";
            }

            skillList += string.Format(
                "<button type='button' class='btn btn-xs {0} m-xs skill-btn' data-skill-id='{1}' onclick='onSkillClick(\"{1}\")'>{2}</button>",
                btnColor,
                skillIdLocal,
                skillName);
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
            var skills = dc.Skills
                .OrderBy(s => s.Name)
                .Select(s => new { s.SkillId, s.Name })
                .ToList();
            ddlSkills.DataTextField = "Name";
            ddlSkills.DataValueField = "SkillId";
            ddlSkills.DataSource = skills;
            ddlSkills.DataBind();
            var resources = dc.Resources
                              .OrderBy(r => r.Name)
                              .Select(r => new { r.ResourceId, r.Name })
                              .ToList();
            ddlResources.DataTextField = "Name";
            ddlResources.DataValueField = "ResourceId";
            ddlResources.DataSource = resources;
            ddlResources.DataBind();
            Guid orgId;
            if (Guid.TryParse(organizationId, out orgId))
            {
                var positions = (from pos in dc.Positions
                                 where pos.OrganizationId == orgId
                                 orderby pos.Name
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
                          orderby ev.Name
                          select new
                          {
                              ev.EventId,
                              ev.Name
                          }).ToList();

            ddlEvent.DataSource = events;
            ddlEvent.DataTextField = "Name";
            ddlEvent.DataValueField = "EventId";
            ddlEvent.DataBind();
            ddlEvent.Items.Insert(0, new ListItem("Select Portal", ""));

        }

    }
    public void CreateManageUserStatusRadioButtons()
    {
        ListItem activeLI = new ListItem("Active", "Active");
        ListItem inactiveLI = new ListItem("Inactive", "Inactive");
        ListItem pendingLI = new ListItem("Pending", "Pending");

        rblManageUserStatus.Items.Add(activeLI);
        rblManageUserStatus.Items.Add(inactiveLI);
        rblManageUserStatus.Items.Add(pendingLI);
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
        if (isTeamMember || isSiteAdministrator || isTeamOwner)
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
            bool isVetted = txtIsVetted.Checked;
            bool optedSMS = txtOptedSMS.Checked;
            bool teamVerified = txtTeamVerified.Checked;
            bool stabilityVerified = txtStabilityVerified.Checked;
            bool teamAdministrator = txtTeamAdministrator.Checked;
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
                currentPageValue.Value = currentPageValue.Value == "" ? "1" : currentPageValue.Value;

                // Execute stored procedure and return mapped results
                dc.CommandTimeout = 300;
                //var result = dc.ExecuteQuery<PeopleList>(
                //   "EXEC GetPeopleList {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}", organizationId, startDate == null ? "" : startDate.Value.ToString("yyyy-MM-dd"), endDate == null ? "" : endDate.Value.ToString("yyyy-MM-dd"), selectedSkillsParam, selectedResourcesParam, nameSearchTermParam, selectedTraining, eventLatitude, eventLongitude, selectedRadius, emailConnected, isVetted, optedSMS, teamVerified,
				

				var result = dc.ExecuteQuery<PeopleList>(
                   "EXEC GetPeopleList {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14},{15},{16},{17}", 
				   organizationId, 
				   startDate == null ? "" : startDate.Value.ToString("yyyy-MM-dd"), 
				   endDate == null ? "" : endDate.Value.ToString("yyyy-MM-dd"), 
				   selectedSkillsParam, 
				   selectedResourcesParam, 
				   nameSearchTermParam, 
				   selectedTraining, 
				   eventLatitude, 
				   eventLongitude, 
				   selectedRadius, 
				   emailConnected, 
				   isVetted, 
				   optedSMS, 
				   teamVerified, 
				   stabilityVerified, 
				   teamAdministrator, 
				   currentPageValue.Value, 
				   pageSize).Distinct().ToList();

                var totalCount = result.Any() ? result.First().TotalCount : 0;

                // Show Filter Message if Any Filter Applied
                divFilterMessage.Visible = selectedSkills.Any() || selectedResources.Any() || emailConnected || isVetted || optedSMS || teamVerified || stabilityVerified || teamAdministrator;
                litFilterMessage.Text = divFilterMessage.Visible ? "<i class='fa fa-2x fa-filter'></i><hr>Filtered by selected options." : "";

                //if (isTeamMember && !User.IsInRole("Administrator") && !isTeamOwner)
                //{
                //    result = result.Where(x => x.IsApproved == true).ToList();
                //}

                // Bind Data.
                //var pNumber = Convert.ToInt32(currentPageValue.Value);

                totalPageValue.Value = Convert.ToString(Math.Ceiling((double)totalCount / 50));

                rptVolunteers.DataSource = result;
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
        txtTeamVerified.Checked = false;
        txtStabilityVerified.Checked = false;
        txtTeamAdministrator.Checked = false;
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
    public bool? ReceiveSMSNotifications { get; set; }
    public int TotalCount { get; set; }
    public int? RankPosition { get; set; }
    public PeopleList(string firstname, DateTime createDate, string description, string loweredEmail, string phoneNumber, string lastname, Guid userId, DateTime? dateVettingCompleted, DateTime? dateVettingStarted, string vettingNotes, bool? vettingActive, bool? vettingComplete, bool? passedVetting, string title, string zelloName, DateTime lastLoginDate, DateTime lastActivityDate, bool? isApproved, bool? receiveSMSNotifications, int? rankPosition)
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
        ReceiveSMSNotifications = receiveSMSNotifications;
        RankPosition = rankPosition;
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
                IsApproved == other.IsApproved &&
                // Compare new fields
                ReceiveSMSNotifications == other.ReceiveSMSNotifications &&
                RankPosition == other.RankPosition;
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
        hashCode = hashCode * -1521134295 + ReceiveSMSNotifications.GetHashCode();
        hashCode = hashCode * -1521134295 + RankPosition.GetHashCode();
        return hashCode;
    }
}