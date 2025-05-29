using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_ReceivedRequests : BaseWebForm
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
    public string jsonEvents = string.Empty;
    public string availableDates = string.Empty;
    public string teamCounts = string.Empty;
    public string programId = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        ucTeamFooter.PageName = "teamRolesPage";
        ucTeamHeader.PageName = "";

        #region HEADER PROPERTIES
        organizationId = Request.QueryString["organizationId"];
        programId = Request.QueryString["programId"];

        string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
        _coverImage = causePhotoFolder + "businesscoverimage.png";

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var organization = (from o in dc.Organizations
                            where o.OrganizationId == new Guid(organizationId)
                            select new { o.Name, o.LogoSquare, o.Description, o.Logo, o.CoverImage, o.URLFriendlyName }).SingleOrDefault();
        MembershipUser user = Membership.GetUser();
        Guid currentUserId = Guid.Empty;
        if (user != null && user.ProviderUserKey != null)
        {
            currentUserId = (Guid)user.ProviderUserKey;
        }
        string squareLogo = string.Empty;
        if (organization != null)
        {
            Guid orgid = new Guid(organizationId);
            var senderfalse = dc.UserOrganizations
         .Where(r => r.OrganizationId == orgid && r.IsEnabled == false && r.TeamJoinStatus == 0)
         .Select(r => r.UserId)
         .ToList();
            if (senderfalse != null)
            {

                var matchingNotificationsfalse = dc.Notifications
    .Where(n =>
        n.SenderUserId.HasValue &&
        senderfalse.Contains(n.SenderUserId.Value) &&
        n.FeatureTypeId == 27 &&
        n.RecipientUserId == currentUserId &&
        n.OrganizationId == orgid
    )
    .GroupBy(n => new { n.SenderUserId, n.Description })
    .Select(g => g.OrderByDescending(n => n.CreatedOn).FirstOrDefault())
    .Select(n => new
    {
        SenderId = n.SenderUserId.Value,
        Message = n.Description
    })
    .ToList();
                ;

                rptRequests.DataSource = matchingNotificationsfalse;
                rptRequests.DataBind();
            }
            ucTeamHeader.CoverImage = _coverImage;

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

        ucTeamHeader._teamTitle = organization.Name;
        ucTeamHeader.TeamDescription = organization.Description;
        ucTeamFooter.OrganizationId = organizationId;
        ucTeamHeader.OrganizationId = organizationId;
        ucTeamHeader.TeamLogo = squareLogo;
        Master.FbImageType = "image/jpg";
        Master.FbURL = Request.Url.AbsoluteUri;
    }

    [WebMethod]
    public static string JoinTeam(Guid senderId, Guid organizationId)
    {

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        Guid userOrgId = dc.UserOrganizations
            .Where(rr => rr.UserId == senderId && rr.OrganizationId == organizationId)
            .Select(rr => rr.UserOrganizationId)
            .FirstOrDefault();

        if (userOrgId != Guid.Empty)
        {
            UserOrganization userOrg = dc.UserOrganizations
                .FirstOrDefault(u => u.UserOrganizationId == userOrgId);

            if (userOrg != null)
            {
                userOrg.TeamJoinStatus = (int)RequestStatus.Approved; // or your desired enum value
                userOrg.IsEnabled = true;
                dc.SubmitChanges();
            }
        }

        return "User successfully added to the team.";

    }
    [WebMethod]
    public static string RejectRequest(Guid senderId, Guid organizationId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        Guid userOrgId = dc.UserOrganizations
            .Where(rr => rr.UserId == senderId && rr.OrganizationId == organizationId)
            .Select(rr => rr.UserOrganizationId)
            .FirstOrDefault();

        if (userOrgId != Guid.Empty)
        {
            UserOrganization userOrg = dc.UserOrganizations
                .FirstOrDefault(u => u.UserOrganizationId == userOrgId);

            if (userOrg != null)
            {
                userOrg.TeamJoinStatus = (int)RequestStatus.Rejected;
                dc.SubmitChanges();
            }
        }


        return "Request not found.";
    }

}

#endregion