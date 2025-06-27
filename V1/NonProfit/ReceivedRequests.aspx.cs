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
    public string jsonEvents = string.Empty;
    public string availableDates = string.Empty;
    public string teamCounts = string.Empty;
    public string programId = string.Empty;

	protected void Page_Load(object sender, EventArgs e)
    {
        ucTeamFooter.PageName = "teamRolesPage";
        ucTeamHeader.PageName = "";

		#region HEADER PROPERTIES
		_organizationId = Request.QueryString["organizationId"];
        programId = Request.QueryString["programId"];

        string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
        _coverImage = causePhotoFolder + "businesscoverimage.png";
		ucTeamHeader.CoverImage = _coverImage;

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
        var organization = (from o in dc.Organizations
                            where o.OrganizationId == new Guid(_organizationId)
                            select new { o.Name, o.LogoSquare, o.Description, o.Logo, o.CoverImage, o.URLFriendlyName }).SingleOrDefault();

        MembershipUser user = Membership.GetUser();
        Guid currentUserId = Guid.Empty;
        if (user != null && user.ProviderUserKey != null)
        {
            currentUserId = (Guid)user.ProviderUserKey;
        }

        string squareLogo = "/V1/Images/Logo-Placeholder.png";
        if (organization != null)
        {

			var pendingTeamMembers = (from ud in dc.UserOrganizations
									  join p in dc.Profiles on ud.UserId equals p.UserId
									  where ud.OrganizationId == new Guid(_organizationId) && ud.Status == (int)RequestStatus.Pending
									  select new {ud.UserId, ud.OrganizationId, ProfileName = p.Firstname + " " + p.Lastname }).ToList();

			if (pendingTeamMembers != null)
			{
				//Guid? userId = pendingTeamMembers.FirstOrDefault()?.UserId;

				//var matchingNotificationsfalse = dc.Notifications
				//.Where(n =>
				//	n.SenderUserId.HasValue &&
				//	pendingTeamMembers.Contains(n.SenderUserId.Value) &&
				//	n.FeatureTypeId == 27 &&
				//	n.RecipientUserId == currentUserId &&
				//	n.OrganizationId == organizationId
				//)
				//.GroupBy(n => new { n.SenderUserId, n.Description })
				//.Select(g => g.OrderByDescending(n => n.CreatedOn).FirstOrDefault())
				//.Select(n => new
				//{
				//	SenderId = n.SenderUserId.Value,
				//	ProfileName = n.Description
				//})
				//.ToList();
    //            ;

                rptRequests.DataSource = pendingTeamMembers;
                rptRequests.DataBind();
            }

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
        ucTeamFooter.OrganizationId = _organizationId;
        ucTeamHeader.OrganizationId = _organizationId;
        ucTeamHeader.TeamLogo = squareLogo;
        Master.FbImageType = "image/jpg";
        Master.FbURL = Request.Url.AbsoluteUri;
    }

    [WebMethod]
    public static string JoinTeam(Guid senderId, Guid organizationId)
    {

        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var userOrgData = dc.UserOrganizations
    .Where(rr => rr.UserId == senderId && rr.OrganizationId == organizationId)
    .Select(rr => new { rr.UserOrganizationId, rr.Status })
    .FirstOrDefault();

        if (userOrgData !=null)
        {
            UserOrganization userOrg = dc.UserOrganizations
                .FirstOrDefault(u => u.UserOrganizationId == userOrgData.UserOrganizationId);
            UserOrganizationHistory userHistory = dc.UserOrganizationHistories
          .FirstOrDefault(uh => uh.UserOrganizationId == userOrgData.UserOrganizationId);
            if (userOrg != null)
            {
                int previousStatus = userOrgData.Status;
                if (userHistory != null)
                {
                    userHistory.PreviousStatus = previousStatus;
                    userHistory.StatusChangedOn = DateTime.Now;
                }
                userOrg.Status = (int)RequestStatus.Approved; 
                dc.SubmitChanges();
            }
        }

        return "User successfully added to the team.";

    }

	protected void rptRequests_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			// Get the data item
			var data = e.Item.DataItem;

			// Assuming the data item has UserId (Guid) and Firstname (string)
			Guid userId = (Guid)DataBinder.Eval(data, "UserId");
			string profileName = (string)DataBinder.Eval(data, "ProfileName");

			// Find the HyperLink control
			HyperLink hypUser = (HyperLink)e.Item.FindControl("hypUser");

			if (hypUser != null)
			{
				hypUser.Text = profileName;
				hypUser.NavigateUrl = "/V1/Member/Default.aspx?userId=" + userId;
			}
		}
	}



	[WebMethod]
    public static string RejectRequest(Guid senderId, Guid organizationId)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var userOrgData = dc.UserOrganizations
    .Where(rr => rr.UserId == senderId && rr.OrganizationId == organizationId)
    .Select(rr => new { rr.UserOrganizationId, rr.Status })
    .FirstOrDefault();

        if (userOrgData != null)
        {
            UserOrganization userOrg = dc.UserOrganizations
                .FirstOrDefault(u => u.UserOrganizationId == userOrgData.UserOrganizationId);
            UserOrganizationHistory userHistory = dc.UserOrganizationHistories
          .FirstOrDefault(uh => uh.UserOrganizationId == userOrgData.UserOrganizationId);
            if (userOrg != null)
            {
                int previousStatus = userOrgData.Status;
                if (userHistory != null)
                {
                    userHistory.PreviousStatus = previousStatus;
                    userHistory.StatusChangedOn = DateTime.Now;
                }
                userOrg.Status = (int)RequestStatus.RemovedByAdmin;
                dc.SubmitChanges();
            }
        }


        return "Request not found.";
    }
    [WebMethod]
    public static string BlockUser(Guid senderId, Guid organizationId)
    {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

      var userOrgData = dc.UserOrganizations
  .Where(rr => rr.UserId == senderId && rr.OrganizationId == organizationId)
  .Select(rr => new { rr.UserOrganizationId, rr.Status })
  .FirstOrDefault();

      if (userOrgData !=null)
      {
          UserOrganization userOrg = dc.UserOrganizations
              .FirstOrDefault(u => u.UserOrganizationId == userOrgData.UserOrganizationId);
          UserOrganizationHistory userHistory = dc.UserOrganizationHistories
        .FirstOrDefault(uh => uh.UserOrganizationId == userOrgData.UserOrganizationId);
          if (userOrg != null)
          {
              int previousStatus = userOrgData.Status;
              if (userHistory != null)
              {
                  userHistory.PreviousStatus = previousStatus;
                  userHistory.StatusChangedOn = DateTime.Now;
              }
              userOrg.Status = (int)RequestStatus.Blocked; 
              dc.SubmitChanges();
          }
      }


        return "Request not found.";
    }
    [WebMethod]
    public static string DenyRequest(Guid senderId, Guid organizationId,DateTime reapplyDate)
    {
        CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

        var userOrgData = dc.UserOrganizations
    .Where(rr => rr.UserId == senderId && rr.OrganizationId == organizationId)
    .Select(rr => new { rr.UserOrganizationId, rr.Status })
    .FirstOrDefault();

        if (userOrgData != null)
        {
            UserOrganization userOrg = dc.UserOrganizations
                .FirstOrDefault(u => u.UserOrganizationId == userOrgData.UserOrganizationId);
            UserOrganizationHistory userHistory = dc.UserOrganizationHistories
          .FirstOrDefault(uh => uh.UserOrganizationId == userOrgData.UserOrganizationId);
            if (userOrg != null)
            {
                int previousStatus = userOrgData.Status;
                if (userHistory != null)
                {
                    userHistory.PreviousStatus = previousStatus;
                    userHistory.StatusChangedOn = DateTime.Now;
                    userHistory.DateToReApply = reapplyDate;
                }
                userOrg.Status = (int)RequestStatus.Denied;
                dc.SubmitChanges();
            }
        }


        return "Request not found.";
    }
}

#endregion