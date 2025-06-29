using CrowdRelief;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Linq;
using System.EnterpriseServices.Internal;
using System.IdentityModel.Metadata;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_Default : BaseWebForm
{
    public string _logo;
    public string _teamName;
    public string _teamSquareLogo;
    public string _teamDescription;
    public string _pageName;
    public string _organizationId;
    public string _nonProfitDropDown;
    public string _coverImage;
    public string urlFriendlyName = string.Empty;
    public string organizationId = string.Empty;
    public string action = string.Empty;
    public string volunteerLink = string.Empty;
    public string donateLink = string.Empty;
    public string impactoidLink = string.Empty;
    public string activityPageLink = string.Empty;
    public string nonProfitDropDown = string.Empty;
    public string createChapterLink = string.Empty;
    public string editLink = string.Empty;
    public string DefaultCampaignId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ucTeamFooter.PageName = "teamPage";
        ucTeamHeader.PageName = "Team Page";

        #region HEADER PROPERTIES
        ////////////////////////
        //BEGIN HEADER PROPERTIES
        ////////////////////////

        urlFriendlyName = Request.QueryString["urlFriendlyName"];
        organizationId = Request.QueryString["organizationId"];
        action= Request.QueryString["action"];
        string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
        _coverImage = causePhotoFolder + "businesscoverimage.png";

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        if (!String.IsNullOrEmpty(urlFriendlyName))
        {
            //Get and set the org id.
            var organizationIdCheck = (from o in dc.Organizations
                                       where o.URLFriendlyName == urlFriendlyName
                                       select new { o.OrganizationId }).SingleOrDefault();

            if (organizationIdCheck.OrganizationId != Guid.Empty)
            {
                organizationId = organizationIdCheck.OrganizationId.ToString();
            }
        }

        ucTeamHeader.OrganizationId = organizationId;

            if (String.IsNullOrEmpty(organizationId))
        {
            if (!User.Identity.IsAuthenticated)
            {
                //Have the user signin
                Response.Redirect("/SignIn");
            }
            else
            {
                //Get this users team, no team? Send them to pick a team.
                var userPrimaryOrganization = (from uo in dc.UserOrganizations
                                               where uo.UserId == userId && uo.IsPrimary == true
                                               select new { uo.OrganizationId }).Take(1).SingleOrDefault();

                if (userPrimaryOrganization == null)
                {
                    var userOrganization = (from uo in dc.UserOrganizations
                                            where uo.UserId == userId
                                            select new { uo.OrganizationId }).Take(1).SingleOrDefault();

                    if (userOrganization == null)
                    {
                        Response.Redirect("/V1/NonProfit/TeamList.aspx?team=false");
                    }
                    else
                    {
                        organizationId = userOrganization.OrganizationId.ToString();
                    }
                }
                else
                {
                    organizationId = userPrimaryOrganization.OrganizationId.ToString();
                }
            }
        }

        var organization = (from o in dc.Organizations
                            where o.OrganizationId == new Guid(organizationId) && o.IsActive == true
                            select new
                            {
                                o.PurposeMission,
                                o.YearFounded,
                                o.City,
                                o.State,
                                o._501c3Status,
                                o.Address,
                                o.Zip,
                                o.PointOfContactName,
                                o.PointOfContactPhoneNumber,
                                o.PointOfContactEmail,
                                o.FacebookGroupURL,
                                o.FacebookURL,
                                o.TwitterURL,
                                o.InstagramURL,
                                o.YouTubeURL,
                                o.EIN,
                                o.PrimaryPhone,
                                o.SecondaryPhone,
                                o.PublicEmail,
                                o.PublicPhoneNumber,
                                o.Website,
                                o.BlogURL,
                                o.DonationURL,
                                o.IsActive,
                                o.Name,
                                o.LogoSquare,
                                o.Description,
                                o.Logo,
                                o.CoverImage,
                                o.URLFriendlyName,
                                o.ParentOrganizationId
                            }).SingleOrDefault();


        string squareLogo = "/V1/Images/Logo-Placeholder.png";
       var waiver = dc.OrganizationWaivers
                  .FirstOrDefault(w => w.OrganizationId == new Guid(organizationId) && w.IsRequired == true);
        if (Session["ShowModal"] != null && (bool)Session["ShowModal"] == true)
        {
            litWaiverText.Text = waiver.WaiverText.Replace("\n", "<br />");
            hiddenShowModal.Value = "true";
            Session["ShowModal"] = null;
        }
        if (organization != null)
        {
            BindChapterOrganizations(organizationId);


            if (!string.IsNullOrEmpty(organization.CoverImage))
            {
                //Let's the user change the cover image.
                _coverImage = causePhotoFolder + organization.CoverImage;
            }

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

        TeamRoles teamRoles = TeamRoleService.GetTeamRoles(userId, new Guid(organizationId), User.IsInRole("Administrator"));
        if (teamRoles.IsTeamOwner || teamRoles.IsTeamAdministrator || teamRoles.IsSiteAdministrator)
        {
            lbCreateChapter.Visible = true;
            createChapterLink = "/V1/NonProfit/NonProfitNew.aspx?parentOrganizationId=" + organizationId;
        }

        ////////////////////////
        //END HEADER PROPERTIES
        ////////////////////////
        #endregion


        //		litTeamName.Text = organization.Name;
        lblOrgName.Text = organization.Name;
        litMission.Text = organization.PurposeMission;
        litDescription.Text = organization.Description;
        litYearFounded.Text = organization.YearFounded;
        hypAddress.Text = organization.Address + "<br/>" + organization.City + ", " + organization.State + " " + organization.Zip;
        //		hypAddress.NavigateUrl = "http://maps.google.com/maps?q=" + organization.Address.Replace(" ", "+") + "," + organization.City.Replace(" ", "+") + "," + organization.State.Replace(" ", "+") + "," + organization.Zip;
        //		lblVoadMember.Text = organization.IsVoadMember.ToString();
        lbl501c3.Text = organization._501c3Status.ToString();

        lbVolunteer.Visible = true;
        lbleave.Visible = false;
        lbprimary.Visible = false;
        if (User.Identity.IsAuthenticated)
        {

            var userOrganization = from uo in dc.UserOrganizations
                                   where uo.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
                                   && uo.OrganizationId == new Guid(organizationId) && (uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
                                   select uo;


            UserOrganization request = dc.UserOrganizations.FirstOrDefault(rr => rr.UserId == userId && rr.OrganizationId == new Guid(organizationId));

            int? userStatus = null;

            if (request != null)
            {
                userStatus = request.Status;
            }

            var userOrg = dc.UserOrganizations.FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == new Guid(organizationId));

            UserOrganizationHistory userHistory = null;

            if (userOrg != null)
            {
                userHistory = dc.UserOrganizationHistories
                                .FirstOrDefault(uh => uh.UserOrganizationId == userOrg.UserOrganizationId);
            }
            DateTime currentRequestTime = DateTime.Now;
            if (userStatus != null)
            {
                if (userStatus == (int)RequestStatus.Pending)
                {
                    btnActiveVolunteer.Text = "Request Pending";
                    btnActiveVolunteer.Attributes["data-toggle"] = "tooltip";
                    btnActiveVolunteer.Attributes["title"] = "Your request is pending for approval";
                    btnActiveVolunteer.Visible = true;
                    btnActiveVolunteer.Enabled = false;
                    btnActiveVolunteer.Style.Add("background-color", "lightgray");
                    btnActiveVolunteer.Style.Add("color", "black");
                    lbVolunteer.Visible = false;
                }
                else if (userStatus == (int)RequestStatus.Denied && userHistory.DateToReApply >= currentRequestTime)
                {
                    btnActiveVolunteer.Text = "Request Denied";
                    btnActiveVolunteer.Attributes["data-toggle"] = "tooltip";
                    btnActiveVolunteer.Attributes["title"] = string.Format("Your request was denied. You can reapply after {0:dd MMM yyyy}.", userHistory.DateToReApply);
                    btnActiveVolunteer.Visible = true;
                    btnActiveVolunteer.Enabled = false;
                    btnActiveVolunteer.Style.Add("background-color", "lightgray");
                    btnActiveVolunteer.Style.Add("color", "black");
                    lbVolunteer.Visible = false;
                }

                else if (userStatus == (int)RequestStatus.Blocked)
                {
                    btnActiveVolunteer.Text = "Blocked";
                    btnActiveVolunteer.Attributes["data-toggle"] = "tooltip";
                    btnActiveVolunteer.Attributes["title"] = "You have been blocked from joining this team. Please contact the administrator for more details.";
                    btnActiveVolunteer.Visible = true;
                    btnActiveVolunteer.Enabled = false;
                    btnActiveVolunteer.Style.Add("background-color", "lightgray");
                    btnActiveVolunteer.Style.Add("color", "black");
                    lbVolunteer.Visible = false;
                }
                else if (userStatus == (int)RequestStatus.RemovedByUser)
                {
                    lbVolunteer.Visible = true;
                }
                else if (userOrg.IsPrimary == true && (userStatus == (int)RequestStatus.Approved || userStatus == (int)RequestStatus.Pending))
                {
                    lbleave.Visible = true;
                    lbVolunteer.Visible = false;
                    lbprimary.Visible = false;
                }
                else if (userStatus == (int)RequestStatus.Approved)
                {
                    lbVolunteer.Visible = false;
                    lbleave.Visible = true;
                    lbprimary.Visible = true;
                }

            }
            else if (userOrg != null)
            {
                if (userOrg.IsPrimary == true)
                {

                    lbleave.Visible = true;
                    lbVolunteer.Visible = false;
                    lbprimary.Visible = false;

                }
                else
                {
                    lbVolunteer.Visible = false;
                    lbleave.Visible = true;
                    lbprimary.Visible = true;
                }
            }

            if (teamRoles.IsTeamOwner)
            {
                lbleave.Visible = true;
                lbleave.CssClass = "btn btn-secondary  btn-large pull-right m-l-md disabled";
                lbleave.Attributes["data-toggle"] = "tooltip";
                lbleave.Attributes["title"] = "Transfer ownership of this team before leaving it.";
                lbleave.Attributes["data-disabled"] = "true";
            }
            if(userOrganization.Count()==0)
            {
                if (action == "join" && !string.IsNullOrEmpty(organizationId))
                {
                    ProcessJoinTeam();
                }
            }
        }
        else
        {
            volunteerLink = "/Register/" + organizationId;
        }



        lblPointOfContactPerson.Text = organization.PointOfContactName;
        if (!String.IsNullOrEmpty(organization.PointOfContactPhoneNumber))
        {
            hypPointOfContactPhone.Text = Regex.Replace(organization.PointOfContactPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
            hypPointOfContactPhone.NavigateUrl = "tel:" + organization.PointOfContactPhoneNumber;
            hypPointOfContactPhone.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organization.PointOfContactEmail))
        {
            hypPointOfContactEmail.Text = organization.PointOfContactEmail;
            hypPointOfContactEmail.NavigateUrl = "mailto:" + organization.PointOfContactEmail;
            hypPointOfContactEmail.Font.Underline = true;
        }


        if (!String.IsNullOrEmpty(organization.FacebookURL))
        {
            hypFacebookPage.Text = organization.Name + " Facebook Page";
            hypFacebookPage.NavigateUrl = organization.FacebookURL;
            hypFacebookPage.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organization.FacebookGroupURL))
        {
            hypFacebookGroup.Text = organization.Name + " Facebook Group";
            hypFacebookGroup.NavigateUrl = organization.FacebookGroupURL;
            hypFacebookGroup.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organization.TwitterURL))
        {
            hypTwitter.Text = "Visit " + organization.TwitterURL;
            hypTwitter.NavigateUrl = "https://www.Twitter.com/" + organization.TwitterURL;
            hypTwitter.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organization.InstagramURL))
        {
            hypInstagram.Text = "Instagram";
            hypInstagram.NavigateUrl = "https://www.Instagram.com/" + organization.InstagramURL;
            hypInstagram.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organization.YouTubeURL))
        {
            hypYouTube.Text = organization.Name + " YouTube Channel";
            hypYouTube.NavigateUrl = organization.YouTubeURL;
            hypYouTube.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organization.EIN))
        {
            lblEIN.Text = organization.EIN;
            dtEIN.Visible = true;
            ddEIN.Visible = true;
        }

        if (!String.IsNullOrEmpty(organization.PrimaryPhone))
        {
            dtPrimaryPhone.Visible = true;
            ddPrimaryPhone.Visible = true;
            hypPrimaryPhone.Text = Regex.Replace(organization.PrimaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
            hypPrimaryPhone.NavigateUrl = "tel:" + organization.PrimaryPhone;
            hypPrimaryPhone.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organization.SecondaryPhone))
        {
            dtSecondaryPhone.Visible = true;
            ddSecondaryPhone.Visible = true;
            hypSecondaryPhone.Text = Regex.Replace(organization.SecondaryPhone, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
            hypSecondaryPhone.NavigateUrl = "tel:" + organization.SecondaryPhone;
            hypSecondaryPhone.Font.Underline = true;
        }


        if (!String.IsNullOrEmpty(organization.PublicPhoneNumber))
        {
            ddPublicPhone.Visible = true;
            hyoPublicPhoneNumber.Text = Regex.Replace(organization.PublicPhoneNumber, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");
            hyoPublicPhoneNumber.NavigateUrl = "tel:" + organization.PublicPhoneNumber;
            hyoPublicPhoneNumber.Font.Underline = true;
        }
        if (!String.IsNullOrEmpty(organization.PublicEmail))
        {
            ddPublicEmail.Visible = true;
            hypPublicEmailAddress.Text = organization.PublicEmail;
            hypPublicEmailAddress.NavigateUrl = "mailto:" + organization.PublicEmail;
            hypPublicEmailAddress.Font.Underline = true;
        }
        if (!String.IsNullOrEmpty(organization.Website))
        {
            ddWebsite.Visible = true;
            hypWebsite.Text = organization.Website;
            hypWebsite.NavigateUrl = organization.Website;
            hypWebsite.Font.Underline = true;
        }

        if (!String.IsNullOrEmpty(organization.BlogURL))
        {
            ddBlog.Visible = true;
            hypBlog.Text = organization.BlogURL;
            hypBlog.NavigateUrl = organization.BlogURL;
            hypBlog.Font.Underline = true;
        }
        DefaultCampaignId = dc.DonationCampaigns.Where(x => x.IsDefault && x.OrganizationId == new Guid(organizationId)).Select(x => x.DonationCampaignId.ToString()).FirstOrDefault();
        if (!String.IsNullOrEmpty(DefaultCampaignId))
        {
            lbDonate.Visible = true;
            lbDonate.PostBackUrl = string.Format("/V1/NonProfit/Donation.aspx?organizationId={0}", organizationId);
            donateLink = lbDonate.PostBackUrl;
        }
        
    }
    protected void lbleave_Click(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["organizationId"];
        urlFriendlyName = Request.QueryString["urlFriendlyName"];
        if (User.Identity.IsAuthenticated)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                if (!String.IsNullOrEmpty(urlFriendlyName) && organizationId == null)
                {
                    var organizationIdCheck = (from o in dc.Organizations
                                               where o.URLFriendlyName == urlFriendlyName
                                               select new { o.OrganizationId }).SingleOrDefault();

                    if (organizationIdCheck.OrganizationId != Guid.Empty)
                    {
                        organizationId = organizationIdCheck.OrganizationId.ToString();
                    }
                }
                var userOrg = dc.UserOrganizations.FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == new Guid(organizationId));

                var userHistory = dc.UserOrganizationHistories.FirstOrDefault(uh => uh.UserOrganizationId == userOrg.UserOrganizationId);

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
                            UserId = userId,
                            PreviousStatus = previousStatus,
                            StatusChangedOn = DateTime.Now,
                            DateToReApply = null
                        };
                    }
                    userOrg.Status = (int)RequestStatus.RemovedByUser;
                    dc.SubmitChanges();

                }
            }

        }
        Response.Redirect(Request.RawUrl);
    }
    protected void lbprimary_Click(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["organizationId"];
        urlFriendlyName = Request.QueryString["urlFriendlyName"];

        if (User.Identity.IsAuthenticated)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                if (!String.IsNullOrEmpty(urlFriendlyName) && organizationId == null)
                {
                    var organizationIdCheck = (from o in dc.Organizations
                                               where o.URLFriendlyName == urlFriendlyName
                                               select new { o.OrganizationId }).SingleOrDefault();

                    if (organizationIdCheck.OrganizationId != Guid.Empty)
                    {
                        organizationId = organizationIdCheck.OrganizationId.ToString();
                    }
                }
                var currentPrimary = dc.UserOrganizations
                                       .FirstOrDefault(uo => uo.UserId == userId && uo.IsPrimary == true && (uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending));
                if (currentPrimary != null)
                {
                    currentPrimary.IsPrimary = false;
                }

                var newPrimary = dc.UserOrganizations
                                   .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == new Guid(organizationId));
                if (newPrimary != null)
                {
                    newPrimary.IsPrimary = true;
                    dc.SubmitChanges();
                }
            }
        }
        Response.Redirect(Request.RawUrl);
    }
    protected void jointheteam_Click(object sender, EventArgs e)
    {
        ProcessJoinTeam(); 
    }

    public void ProcessJoinTeam()
    {
        if (!User.Identity.IsAuthenticated)
        {
            string returnUrl = Server.UrlEncode(Request.RawUrl);
            Response.Redirect("~/SignIn.aspx?ReturnUrl=" + returnUrl);
            return;
        }

        string organizationId = Request.QueryString["organizationId"];
        string urlFriendlyName = Request.QueryString["urlFriendlyName"];

        OrganizationWaiver waiver = null;

        using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
        {
            if (organizationId != null)
            {
                waiver = dc.OrganizationWaivers
                    .FirstOrDefault(w => w.OrganizationId == new Guid(organizationId) && w.IsRequired == true);
            }

            if (!string.IsNullOrEmpty(organizationId) || urlFriendlyName != null)
            {
                Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                UserOrganizationHistory userHistory = null;

                if (!String.IsNullOrEmpty(urlFriendlyName) && organizationId == null)
                {
                    var organizationIdCheck = (from o in dc.Organizations
                                               where o.URLFriendlyName == urlFriendlyName
                                               select new { o.OrganizationId }).SingleOrDefault();

                    if (organizationIdCheck != null && organizationIdCheck.OrganizationId != Guid.Empty)
                    {
                        organizationId = organizationIdCheck.OrganizationId.ToString();
                        waiver = dc.OrganizationWaivers
                            .FirstOrDefault(w => w.OrganizationId == new Guid(organizationId) && w.IsRequired == true);
                    }
                }

                if (waiver == null)
                {
                    var userOrg = dc.UserOrganizations
                                .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == new Guid(organizationId));
                    int? previousStatus = null;

                    if (userOrg == null)
                    {
                        userOrg = new UserOrganization
                        {
                            UserOrganizationId = Guid.NewGuid(),
                            UserId = userId,
                            OrganizationId = new Guid(organizationId),
                            ShowTeamLogo = false,
                            TeamVerifiedDate = DateTime.Now,
                            IsPrimary = false,
                            IsPreviousOwner = false,
                            IsTeamAdministrator = false,
                            IsOwner = false,
                            Status = (int)RequestStatus.Pending
                        };

                        dc.UserOrganizations.InsertOnSubmit(userOrg);
                    }
                    else
                    {
                        previousStatus = userOrg.Status;
                        userOrg.Status = (int)RequestStatus.Pending;
                        userHistory = dc.UserOrganizationHistories
                                  .FirstOrDefault(uh => uh.UserOrganizationId == userOrg.UserOrganizationId);
                    }

                    if (userHistory == null)
                    {
                        UserOrganizationHistory history = new UserOrganizationHistory
                        {
                            UserOrganizationHistoryId = Guid.NewGuid(),
                            UserOrganizationId = userOrg.UserOrganizationId,
                            UserId = userId,
                            PreviousStatus = (int)RequestStatus.Pending,
                            StatusChangedOn = DateTime.Now,
                            DateToReApply = null
                        };

                        dc.UserOrganizationHistories.InsertOnSubmit(history);
                    }
                    else
                    {
                        userHistory.PreviousStatus = previousStatus ?? 0;
                        userHistory.StatusChangedOn = DateTime.Now;
                    }

                    dc.SubmitChanges();

                    AddNotificationsAndSendEmail(null, EventArgs.Empty);
                }
                else
                {
                    litWaiverText.Text = waiver.WaiverText.Replace("\n", "<br />");
                    Session["ShowModal"] = true;
                }
            }
        }

        string url = Request.RawUrl;
        if (url.Contains("action="))
        {
            var newUrl = Regex.Replace(url, @"([&?])action=[^&]*(&?)", "$1").TrimEnd('?', '&');

            Response.Redirect(newUrl);
        }
        else
        {
            Response.Redirect(url);
        }
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["organizationId"];
        urlFriendlyName = Request.QueryString["urlFriendlyName"];
        if (!string.IsNullOrEmpty(organizationId) || urlFriendlyName != null)
        {
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
                UserOrganizationHistory userHistory = null;
                if (!String.IsNullOrEmpty(urlFriendlyName) && organizationId == null)
                {
                    var organizationIdCheck = (from o in dc.Organizations
                                               where o.URLFriendlyName == urlFriendlyName
                                               select new { o.OrganizationId }).SingleOrDefault();

                    if (organizationIdCheck.OrganizationId != Guid.Empty)
                    {
                        organizationId = organizationIdCheck.OrganizationId.ToString();
                    }
                }
                var userOrg = dc.UserOrganizations
                            .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == new Guid(organizationId));
                int? previousStatus = null;
                WaiverSignature signature = new WaiverSignature
                {
                    SignatureId = Guid.NewGuid(),
                    OrganizationId = new Guid(organizationId),
                    UserId = userId,
                    SignatureName = txtFullName.Text,
                    SignedOn = DateTime.Now
                };

                dc.WaiverSignatures.InsertOnSubmit(signature);
                dc.SubmitChanges();
                if (userOrg == null)
                {
                    userOrg = new UserOrganization
                    {
                        UserOrganizationId = Guid.NewGuid(),
                        UserId = userId,
                        OrganizationId = new Guid(organizationId),
                        ShowTeamLogo = false,
                        TeamVerifiedDate = DateTime.Now,
                        IsPrimary = false,
                        IsPreviousOwner = false,
                        IsTeamAdministrator = false,
                        IsOwner = false,
                        Status = (int)RequestStatus.Pending
                    };

                    dc.UserOrganizations.InsertOnSubmit(userOrg);
                }
                else
                {
                    previousStatus = userOrg.Status;
                    userOrg.Status = (int)RequestStatus.Pending;
                    userHistory = dc.UserOrganizationHistories
                              .FirstOrDefault(uh => uh.UserOrganizationId == userOrg.UserOrganizationId);
                }


                if (userHistory == null)
                {
                    UserOrganizationHistory history = new UserOrganizationHistory
                    {
                        UserOrganizationHistoryId = Guid.NewGuid(),
                        UserOrganizationId = userOrg.UserOrganizationId,
                        UserId = userId,
                        PreviousStatus = (int)RequestStatus.Pending,
                        StatusChangedOn = DateTime.Now,
                        DateToReApply = null
                    };

                    dc.UserOrganizationHistories.InsertOnSubmit(history);
                }
                else
                {
                    userHistory.PreviousStatus = previousStatus ?? 0;
                    userHistory.StatusChangedOn = DateTime.Now;

                }
                dc.SubmitChanges();
            
                AddNotificationsAndSendEmail(null, EventArgs.Empty);
            }
                
        }
        string url = Request.RawUrl;
        if (url.Contains("action="))
        {
            var newUrl = Regex.Replace(url, @"([&?])action=[^&]*(&?)", "$1").TrimEnd('?', '&');

            Response.Redirect(newUrl);
        }
        else
        {
            Response.Redirect(url);
        }
    }


    protected void AddNotificationsAndSendEmail(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["organizationId"];
        urlFriendlyName = Request.QueryString["urlFriendlyName"];

        if (User.Identity.IsAuthenticated)
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                if (!String.IsNullOrEmpty(urlFriendlyName) && organizationId == null)
                {
                    var organizationIdCheck = (from o in dc.Organizations
                                               where o.URLFriendlyName == urlFriendlyName
                                               select new { o.OrganizationId }).SingleOrDefault();

                    if (organizationIdCheck.OrganizationId != Guid.Empty)
                    {
                        organizationId = organizationIdCheck.OrganizationId.ToString();
                    }
                }
                var adminOwners = dc.UserOrganizations
               .Where(uo =>
                uo.OrganizationId == new Guid(organizationId) &&
               (uo.Status == (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending) &&
            (uo.IsTeamAdministrator == true || uo.IsOwner == true)
            )
          .Select(uo => uo.UserId)
          .ToList();

                string orgName = dc.Organizations
                  .Where(o => o.OrganizationId == new Guid(organizationId))
                  .Select(o => o.Name)
                  .FirstOrDefault();
                string userName = dc.Profiles
                  .Where(p => p.UserId == userId)
                  .Select(p => p.Firstname + " " + p.Lastname)
                  .FirstOrDefault();
                string message = "<a href='/V1/Member/Default.aspx?userId=" + userId + "'>" + userName + "</a> has requested to join your team " + orgName + ".";
                foreach (var adminUserId in adminOwners)
                {
                    var userEmail = (from m in dc.aspnet_Memberships
                                     where m.UserId == adminUserId
                                     select m.Email).FirstOrDefault();
                    BaseWebForm.AddNotifications(
                        NotificationType.Like,
                        FeatureTypeEnum.TeamRequest,
                        "Team Join Request",
                         message,
                        adminUserId,
                        true,
                        organizationId,
                        new Guid(organizationId)
                    );
                    ListDictionary ldEmailBodyReplacements = new ListDictionary
                {
                    { "##UserName##", userName },
                    { "##OrganizationName##", orgName },
                    { "##OrganizationId##", organizationId.ToString() },

                  };
                    string error = string.Empty;
                    Tools.SendEmail(
                        userEmail,
                        "New Team Join Request",
                        ldEmailBodyReplacements,
                        userEmail,
                        "Stability Team Alert",
                        string.Empty,
                        string.Empty,
                        "~/EmailTemplates/TeamJoinRequest.html",
                        out error
                    );

                }

            }
        }
    }
    private void BindChapterOrganizations(string parentOrganizationId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var organizations = (from org in dc.Organizations
                             where org.IsActive == true && org.ParentOrganizationId == new Guid(parentOrganizationId)
                             && org.OrganizationId != org.ParentOrganizationId
                             orderby org.Name
                             select new
                             {
                                 org.OrganizationId,
                                 org.Name
                             }).ToList();

        rptOrganizations.DataSource = organizations;
        rptOrganizations.DataBind();
    }
    protected void rptOrganizations_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RepeaterItem dataItem = (RepeaterItem)e.Item;

            HyperLink lnk = (HyperLink)e.Item.FindControl("lnkOrg");
            if (lnk != null)
            {
                lnk.Text = (string)DataBinder.Eval(dataItem.DataItem, "Name");
                lnk.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + DataBinder.Eval(dataItem.DataItem, "OrganizationId");
            }
        }
    }
    protected void btnDonationsDashboard_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/V1/NonProfit/DonationDashboard.aspx?organizationId=" + organizationId);
    }


}