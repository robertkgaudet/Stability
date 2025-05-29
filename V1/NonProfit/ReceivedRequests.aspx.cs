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

        string squareLogo = "/V1/Images/Logo-Placeholder.png";
        if (organization != null)
        {
            Guid orgid = new Guid(organizationId);
            var senderIds = dc.ReceivedRequests
         .Where(r => r.ReceiverId == orgid && r.IsActive==true)
         .Select(r => r.SenderId)
         .ToList();

            var matchingNotifications = dc.Notifications
     .Where(n => n.SenderUserId.HasValue
                 && senderIds.Contains(n.SenderUserId.Value)
                 && n.FeatureTypeId == 27
                 && n.OrganizationId == orgid)
     .Select(n => new
     {
         SenderId = n.SenderUserId.Value,
         Message = n.Description
     })
     .ToList();
            rptRequests.DataSource = matchingNotifications;
            rptRequests.DataBind();
            ucTeamHeader.CoverImage = _coverImage;

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
        {
            UserOrganization userOrganization = new UserOrganization();
            userOrganization.OrganizationId = organizationId;
            userOrganization.UserId = senderId;
            userOrganization.UserOrganizationId = Guid.NewGuid();
            dc.UserOrganizations.InsertOnSubmit(userOrganization);
            userOrganization.IsEnabled = true;
            var request = dc.ReceivedRequests.FirstOrDefault(r =>
           r.SenderId == senderId && r.ReceiverId == organizationId);
            request.Status = (int)RequestStatus.Approved;
           request.IsActive = false;
            dc.SubmitChanges();
        }

        return "User successfully added to the team.";
        
    }
    [WebMethod]
    public static string RejectRequest(Guid senderId, Guid organizationId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var request = dc.ReceivedRequests.FirstOrDefault(r =>
            r.SenderId == senderId && r.ReceiverId == organizationId);

        if (request != null)
        {
            request.Status = (int)RequestStatus.Rejected;
            request.IsActive = false;
            dc.SubmitChanges();
            return "Request has been rejected.";
        }

        return "Request not found.";
    }

}

#endregion