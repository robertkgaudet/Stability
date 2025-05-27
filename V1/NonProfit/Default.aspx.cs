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
    public string organizationId = string.Empty;
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

        organizationId = Request.QueryString["organizationId"];
        string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
        _coverImage = causePhotoFolder + "businesscoverimage.png";

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
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
                var userOrganization = (from uo in dc.UserOrganizations
                                        where uo.UserId == userId && uo.IsEnabled == true
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

        BindChapterOrganizations(organizationId);

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

        bool isOwner = false;
        if (User.Identity.IsAuthenticated == true)
        {
            var userOrganizationOwners = (from uo in dc.UserOrganizations
                                          join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
                                          where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString()) && uo.IsEnabled == true
                                          && uo.OrganizationId == new Guid(organizationId)
                                          select o).Take(1).SingleOrDefault();

            if (userOrganizationOwners != null)
            {

                if ((userOrganizationOwners.OwnerId == userId))
                {
                    isOwner = true;
                }
            }
        }

        if (isOwner || User.IsInRole("Team Administrator"))
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

            //If the user is logged in and not in a nonprofit already then send to choose a nonprofit.
            var userOrganization = from uo in dc.UserOrganizations
                                   where uo.UserId == new Guid(Membership.GetUser().ProviderUserKey.ToString()) && uo.IsEnabled == true
                                   && uo.OrganizationId == new Guid(organizationId)
                                   select uo;

            ReceivedRequest request = dc.ReceivedRequests
            .FirstOrDefault(rr => rr.SenderId == userId && rr.ReceiverId == new Guid(organizationId));

            int? status = null;

            if (request != null)
            {
                status = request.Status;
            }
            var userOrganizationOwner = (from uo in dc.UserOrganizations
                                         join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
                                         where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString()) && uo.IsEnabled == true && uo.IsEnabled == true
                                         && uo.OrganizationId == new Guid(organizationId)
                                         select o).Take(1).SingleOrDefault();
            var userOrg = dc.UserOrganizations
                                 .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == new Guid(organizationId));
            if (status != null)
            {
                if (userOrganization.Count() == 0 && status == 0)
                {
                    btnActiveVolunteer.Text = "Request Pending";
                    btnActiveVolunteer.Attributes["data-toggle"] = "tooltip";
                    btnActiveVolunteer.Attributes["title"] = "Your request is complete. You joined the team";
                    btnActiveVolunteer.Visible = true;
                    lbVolunteer.Visible = false;
                }
                else if (userOrg != null && userOrg.IsPrimary == true && status == 1)
                {
                    lbleave.Visible = true;
                    lbVolunteer.Visible = false;
                    lbprimary.Visible = false;
                }
                else if (status == 1)
                {
                    lbVolunteer.Visible = false;
                    lbleave.Visible = true;
                    lbprimary.Visible = true;
                }
                else if (status == 2)
                {
                    btnActiveVolunteer.Text = "Request Rejected";
                    btnActiveVolunteer.Attributes["data-toggle"] = "tooltip";
                    btnActiveVolunteer.Attributes["title"] = "Your request was rejected by the team administrators.";
                    btnActiveVolunteer.Visible = true;
                    lbVolunteer.Visible = false;
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
            if (userOrganizationOwner != null && userOrg != null)
            {
                lbleave.Visible = true;
                lbleave.CssClass = "btn btn-secondary  btn-large pull-right m-l-md disabled";
                lbleave.Attributes["data-toggle"] = "tooltip";
                lbleave.Attributes["title"] = "Transfer ownership of this team before leaving it.";
                lbleave.Attributes["data-disabled"] = "true";
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
        organizationId = Request.QueryString["organizationId"];

        if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(organizationId))
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            Guid orgId = new Guid(organizationId);

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                var userOrg = dc.UserOrganizations
                                .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == orgId && uo.IsEnabled == true);

                if (userOrg != null)
                {
                    dc.UserOrganizations.DeleteOnSubmit(userOrg);
                    dc.SubmitChanges();

                }
            }

        }
        Response.Redirect(Request.RawUrl);
    }


    protected void lbprimary_Click(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["organizationId"];

        if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(organizationId))
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            Guid orgId = new Guid(organizationId);

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                var currentPrimary = dc.UserOrganizations
                                       .FirstOrDefault(uo => uo.UserId == userId && uo.IsPrimary == true && uo.IsEnabled == true && uo.IsEnabled == true);
                if (currentPrimary != null)
                {
                    currentPrimary.IsPrimary = false;
                }

                var newPrimary = dc.UserOrganizations
                                   .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == orgId);
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
        string organizationId = Request.QueryString["organizationId"];

        if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(organizationId))
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            Guid orgId = new Guid(organizationId);

            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                ReceivedRequest request = new ReceivedRequest
                {
                    Id = Guid.NewGuid(),
                    SenderId = userId,
                    ReceiverId = orgId,
                    Status = (int)RequestStatus.Pending,
                    RequestDate = DateTime.Now,
                    IsActive = true
                };

                dc.ReceivedRequests.InsertOnSubmit(request);
                dc.SubmitChanges();
                AddNotificationsAndSendEmail(null, EventArgs.Empty);
            }
        }
        Response.Redirect(Request.RawUrl);
    }
    protected void AddNotificationsAndSendEmail(object sender, EventArgs e)
    {
        string organizationId = Request.QueryString["organizationId"];

        if (User.Identity.IsAuthenticated && !string.IsNullOrEmpty(organizationId))
        {
            Guid userId = (Guid)Membership.GetUser().ProviderUserKey;
            Guid orgId = new Guid(organizationId);
            using (CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext())
            {
                var adminOwners = dc.UserOrganizations
               .Where(uo =>
                uo.OrganizationId == orgId &&
               uo.IsEnabled == true &&
            (uo.IsTeamAdministrator == true || uo.IsOwner == true)
            )
          .Select(uo => uo.UserId)
          .ToList();

                string orgName = dc.Organizations
                  .Where(o => o.OrganizationId == orgId)
                  .Select(o => o.Name)
                  .FirstOrDefault();
                string userName = dc.Profiles
                  .Where(p => p.UserId == userId)
                  .Select(p => p.Firstname + " " + p.Lastname)
                  .FirstOrDefault();
                string message = userName + " has requested to join your team " + orgName + ".";
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
                        orgId
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