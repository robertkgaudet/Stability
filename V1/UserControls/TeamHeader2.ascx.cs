using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_TeamHeader2 : System.Web.UI.UserControl
{
    public string _coverImage = string.Empty;
    public string _teamLogo = string.Empty;
    public string _teamTitle = string.Empty;
    public string _teamDescription = string.Empty;
    public string _pageName = string.Empty;
    public string _URLFriendlyPageName = string.Empty;
    public string _organizationId = string.Empty;
    public string _teamName = string.Empty;
    public string _streamClass = string.Empty;
    public bool _isPrimary;
    public string organizationId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        imgTeamLogo.ImageUrl = _teamLogo;
        litTitle.Text = _teamTitle;
        litMemberDescription.Text = _teamDescription;
        litPageName.Text = _pageName;
        ucTeamNavigation.PageName = _pageName;
        ucTeamNavigation.TeamName = _teamName;
        ucTeamNavigation.organizationId = _organizationId;
        if (_isPrimary == true)
        {
            isprimaryteam.Visible = true;
        }
        else
        {
            isprimaryteam.Visible = false;
        }
        organizationId = Request.QueryString["organizationId"];
        if (organizationId != null)
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            var teamowner = dc.Organizations.Where(o => o.OrganizationId == new Guid(organizationId)).Select(o => o.OwnerId)
                .FirstOrDefault();
            var teamownerfullname = dc.Profiles.Where(p => p.UserId == teamowner).Select(p => p.Firstname + " " + p.Lastname)
                      .FirstOrDefault();
            var teamownerAddress = dc.Profiles.Where(p => p.UserId == teamowner).Select(p => p.State + " " + p.City)
                      .FirstOrDefault();
            var teamownerr = "<a href='/V1/Member/Default.aspx?userId=" + teamowner + "'style='color: black;font-size: 15px;'>" + teamownerfullname + "</a>";
            string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
            var profilePhoto = (from p in dc.Photos
                                join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                where ph.UserId == teamowner
                                orderby p.CreatedOn descending
                                select p).Take(1).SingleOrDefault();

            lbTeamOwner.Text = teamownerr;
            lbteamOwnerAddress.Text = teamownerAddress;

            string virtualPathh;
            if (profilePhoto != null)
            {
                virtualPathh = profilePhotoFolder + profilePhoto.FilenameCropped;
                string physicalPath = Server.MapPath(virtualPathh);

                if (!File.Exists(physicalPath))
                {
                    virtualPathh = "~/V1/Images/icons8-customer-64.png"; 
                }
            }
            else
            {
                virtualPathh = "~/V1/Images/icons8-customer-64.png"; 
            }

            imgTeamOwner.ImageUrl = VirtualPathUtility.ToAbsolute(virtualPathh);

            var teamAdministratoUserId = (from uo in dc.UserOrganizations
                                              where uo.OrganizationId == new Guid(organizationId)
                                                    && uo.IsTeamAdministrator == true
                                              select uo.UserId).ToList();
            string allTeamAdministrator = "<div class='card-container '>";

            foreach (var userId in teamAdministratoUserId)
            {
                var teamAdministratorfullname = dc.Profiles
                    .Where(p => p.UserId == userId)
                    .Select(p => p.Firstname + " " + p.Lastname)
                    .FirstOrDefault();

                var teamAdministratorAddress = dc.Profiles
                    .Where(p => p.UserId == userId)
                    .Select(p => p.City + " " + p.State)
                    .FirstOrDefault();

                var teamAdministratoprofilePhoto = (from p in dc.Photos
                                                    join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                                    where ph.UserId == userId
                                                    orderby p.CreatedOn descending
                                                    select p).Take(1).SingleOrDefault();

                string virtualPath = "";
                if (teamAdministratoprofilePhoto != null)
                {
                    virtualPath = profilePhotoFolder + teamAdministratoprofilePhoto.FilenameCropped;
                    string physicalPath = HttpContext.Current.Server.MapPath(virtualPath);

                    if (!File.Exists(physicalPath))
                    {
                        virtualPath = "~/V1/Images/icons8-customer-64.png"; 
                    }
                }
                else
                {
                    virtualPath = "~/V1/Images/icons8-customer-64.png";
                }

                string photoUrl = VirtualPathUtility.ToAbsolute(virtualPath);

                allTeamAdministrator +=
      "<div class='card'>" +
          "<img src='" + photoUrl + "' class='avatar' style='width: 40px; height: 40px;' />" +
          "<div class='info'>" +
              "<div class='name-row'>" +
                  "<h2><a href='/V1/Member/Default.aspx?userId=" + userId + "' style='color: black; font-size: 15px;'>" + teamAdministratorfullname + "</a></h2>" +
              "</div>" +
              "<p class='location'>" + teamAdministratorAddress + "</p>" +
          "</div>" +
      "</div>";

            }
            allTeamAdministrator += "</div>";
            ltTeamAdministrators.Text = allTeamAdministrator;

        }
        if (!String.IsNullOrEmpty(_organizationId))
        {
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

            var parentOrganization = (from o in dc.Organizations
                                      where o.OrganizationId == new Guid(_organizationId)
                                      select new { o.Name, o.ParentOrganizationId }).SingleOrDefault();

            if (parentOrganization != null)
            {
                if (parentOrganization.ParentOrganizationId != new Guid(_organizationId))
                {
                    var parentOrganizationName = (from o in dc.Organizations
                                                  where o.OrganizationId == parentOrganization.ParentOrganizationId
                                                  select new { o.Name }).SingleOrDefault();
                    if (parentOrganizationName != null)
                    {
                        //Don't show if this IS the parent id.
                        hypParentOrganization.Visible = true;
                        litChapterLabel.Visible = true;
                        hypParentOrganization.Text = parentOrganizationName.Name;
                        hypParentOrganization.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + parentOrganization.ParentOrganizationId;
                    }
                }
            }
        }
    }

    public string StreamClass
    {
        get { return _streamClass; }
        set { _streamClass = value; }
    }
    public string OrganizationId
    {
        get { return _organizationId; }
        set { _organizationId = value; }
    }
    public string URLFriendlyPageName
    {
        get { return _URLFriendlyPageName; }
        set { _URLFriendlyPageName = value; }
    }
    public string PageName
    {
        get { return _pageName; }
        set { _pageName = value; }
    }
    public string TeamName
    {
        get { return _teamName; }
        set { _teamName = value; }
    }
    public bool IsPrimary
    {
        get { return _isPrimary; }
        set { _isPrimary = value; }
    }
    public string TeamLogo
    {
        get { return _teamLogo; }
        set { _teamLogo = value; }
    }
    public string CoverImage
    {
        get { return _coverImage; }
        set { _coverImage = value; }
    }
    public string TeamTitle
    {
        get { return _teamTitle; }
        set { _teamTitle = value; }
    }
    public string TeamDescription
    {
        get { return _teamDescription; }
        set { _teamDescription = value; }
    }
}
