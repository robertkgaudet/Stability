using CrowdRelief;
using Microsoft.SqlServer.Server;
//using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
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

        organizationId = Request.QueryString["organizationId"];
        if (organizationId != null)
        {
            string path = Request.Url.AbsolutePath.ToLower();
            CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
            MembershipUser user = Membership.GetUser();
            Guid currentUserId = Guid.Empty;
            if (user != null && user.ProviderUserKey != null)
            {
                 currentUserId = (Guid)user.ProviderUserKey;
            }
            bool isPrimary = dc.UserOrganizations
             .Where(uo => uo.UserId == currentUserId
              && uo.OrganizationId == new Guid(organizationId) && uo.IsEnabled == true) 
              .Select(uo => uo.IsPrimary ?? false)
              .FirstOrDefault();

            if (isPrimary == true)
            {
                isprimaryteam.Visible = true;
            }
            else
            {
                isprimaryteam.Visible = false;
            }
            if (path.Contains("/v1/nonprofit/people.aspx") || path.Contains("/v1/nonprofit/default.aspx"))
            {
                var teamowner = dc.Organizations.Where(o => o.OrganizationId == new Guid(organizationId)).Select(o => o.OwnerId)
                .FirstOrDefault();
                var teamownerfullname = dc.Profiles.Where(p => p.UserId == teamowner).Select(p => p.Firstname + " " + p.Lastname)
                          .FirstOrDefault();
                var teamownerAddress = dc.Profiles.Where(p => p.UserId == teamowner).Select(p => p.State + ", " + p.City)
                          .FirstOrDefault();
                string icon = "";
                bool stabiltyverfiy = dc.Profiles
               .Any(p => p.UserId == teamowner && p.IsDisasterReadyCertified);
                if (stabiltyverfiy == true)
                {
                    icon = "<a href='/V1/Member/Default.aspx?userId=" + teamowner + "'>" +
                                      "<img src='/Impactoid/Images/Logos/purplebadge.png' style='width: 20px; height: 20px; margin-left:-2px; ' data-toggle='tooltip' title='Stability Verified'/>" +
                                    "</a>";
                }


                string teamlogo = "";
                string teamlogo1 = "";
                string teamlogo2 = "";

                var isprimaryorg = (from o in dc.Organizations
                                    join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                                    where uo.UserId == teamowner && uo.IsPrimary == true && uo.IsEnabled == true
                                    select new
                                    {
                                        o.LogoSquare,
                                        o.OrganizationId,
                                        o.Name,
                                        uo.ShowTeamLogo,
                                    }).FirstOrDefault();

                if (isprimaryorg != null)
                {
                    string primarytitle = isprimaryorg.Name + " Verified";
                    teamlogo = "<a href='/V1/NonProfit/Default.aspx?organizationId=" + isprimaryorg.OrganizationId + "'>" +
                               "<img src='" +
                               (!string.IsNullOrEmpty(isprimaryorg.LogoSquare)
                                   ? "/Impactoid/Images/Logos/" + isprimaryorg.LogoSquare
                                   : "/V1/Images/DefaultLogo.png") +
                               "' style='width: 20px; height: 20px; margin-left:-2px;' data-toggle='tooltip' title='" + primarytitle + "' />" +
                               "</a>";
                }

                Guid orgId;
                if (Guid.TryParse(organizationId, out orgId))
                {
                    var userOrg = dc.UserOrganizations
                                    .FirstOrDefault(uo => uo.UserId == currentUserId && uo.OrganizationId == orgId && uo.IsEnabled == true);

                    if (userOrg != null)
                    {
                        var orgUser = dc.Organizations
                                        .Where(o => o.OrganizationId == orgId)
                                        .Select(o => new
                                        {
                                            o.LogoSquare,
                                            o.OrganizationId,
                                            o.Name,
                                            userOrg.ShowTeamLogo
                                        }).FirstOrDefault();

                        if (orgUser != null && orgUser.ShowTeamLogo == true)
                        {
                            string primarytitle1 = orgUser.Name + " Verified";
                            teamlogo1 = "<a href='/V1/NonProfit/Default.aspx?organizationId=" + orgUser.OrganizationId + "'>" +
                                        "<img src='" +
                                        (!string.IsNullOrEmpty(orgUser.LogoSquare)
                                            ? "/Impactoid/Images/Logos/" + orgUser.LogoSquare
                                            : "/V1/Images/DefaultLogo.png") +
                                        "' style='width: 20px; height: 20px; margin-left:-2px;' data-toggle='tooltip' title='" + primarytitle1 + "' />" +
                                        "</a>";
                        }
                    }
                }
                else
                {
                    var orgUserr = (from o in dc.Organizations
                                    join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                                    where uo.UserId == currentUserId && uo.ShowTeamLogo == true && uo.IsEnabled == true
                                    orderby o.CreatedOn descending
                                    select new
                                    {
                                        o.LogoSquare,
                                        o.OrganizationId,
                                        o.Name,
                                        uo.ShowTeamLogo,
                                    }).FirstOrDefault();

                    if (orgUserr != null && orgUserr.ShowTeamLogo == true)
                    {
                        string primarytitle2 = orgUserr.Name + " Verified";
                        teamlogo2 = "<a href='/V1/NonProfit/Default.aspx?organizationId=" + orgUserr.OrganizationId + "'>" +
                                    "<img src='" +
                                    (!string.IsNullOrEmpty(orgUserr.LogoSquare)
                                        ? "/Impactoid/Images/Logos/" + orgUserr.LogoSquare
                                        : "/V1/Images/DefaultLogo.png") +
                                    "' style='width: 20px; height: 20px; margin-left:-2px;' data-toggle='tooltip' title='" + primarytitle2 + "' />" +
                                    "</a>";
                    }
                }

                string finalTeamLogo = !string.IsNullOrEmpty(teamlogo) ? teamlogo :
                                       !string.IsNullOrEmpty(teamlogo1) ? teamlogo1 :
                                       teamlogo2;

                string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
                var profilePhoto = (from p in dc.Photos
                                    join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                    where ph.UserId == teamowner
                                    orderby p.CreatedOn descending
                                    select p).Take(1).SingleOrDefault();
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
                string statushtml = "";
                if (teamowner != currentUserId)
                {
                    var UserUser = (from uu in dc.UserUsers
                                    join uus in dc.UserUserStatus on uu.UserUserStatusId equals uus.UserUserStatusId
                                    where
                                    (uu.RequestingUserId == currentUserId &&
                                    uu.AcceptingUserId == teamowner)
                                    ||
                                    (uu.RequestingUserId == teamowner &&
                                    uu.AcceptingUserId == currentUserId)
                                    select new { uus.Status }).Take(1).SingleOrDefault();
                    if (UserUser != null)
                    {
                        if (UserUser.Status == "Connected")
                        {
                            statushtml = "Friends";
                        }
                        else if (UserUser.Status == "Pending")
                        {
                            statushtml = "Connection Pending";
                        }
                        else if (UserUser.Status == "Delete")
                        {
                            statushtml = "Connection Delete";
                        }
                        else if (UserUser.Status == "Blocked")
                        {
                            statushtml = "Connection Blocked";
                        }
                    }
                }
                string photourl = VirtualPathUtility.ToAbsolute(virtualPathh);
                string connectionHtml = "";
                string cardHtml = "";
                if (teamowner != currentUserId && statushtml == "")
                {
                    connectionHtml = "<p class='connection'>" +
                              "<a href=''id='btnSendRequest' data-userid='" + teamowner + "' data-senderid='" + currentUserId + "'>Send Connection Request</a>" +
                            "</p>";
                }
                if (teamowner != null)
                {
                    if (connectionHtml != "" && statushtml == "")
                    {
                        cardHtml = "<div class='card'>" +
           "<img src='" + photourl + "' class='avatar' style='width: 40px; height: 40px;margin-bottom:30px' />" +
           "<div class='info'>" +
               "<div class='name-row'>" +
                   "<h2><a href='/V1/Member/Default.aspx?userId=" + teamowner + "' style='color: black; font-size: 15px;'>" + teamownerfullname + "</a></h2>" +
                   "<span class='badgge'>" + icon + "</span>" +
                   "<span class='badgge'>" + finalTeamLogo + "</span>" +
               "</div>" +
               "<p class='location'>" + teamownerAddress + "</p>" +
                 connectionHtml +
               "<span class='badge badge-secondary'>Team Owner</span>" +
           "</div>" +
         "</div>";
                    }
                    else if (statushtml != null && teamowner != currentUserId)
                    {
                        cardHtml = "<div class='card'>" +
           "<img src='" + photourl + "' class='avatar' style='width: 40px; height: 40px;margin-bottom:30px' />" +
           "<div class='info'>" +
               "<div class='name-row'>" +
                   "<h2><a href='/V1/Member/Default.aspx?userId=" + teamowner + "' style='color: black; font-size: 15px;'>" + teamownerfullname + "</a></h2>" +
                   "<span class='badgge'>" + icon + "</span>" +
                   "<span class='badgge'>" + finalTeamLogo + "</span>" +
               "</div>" +
               "<p class='location'>" + teamownerAddress + "</p>" +
               "<p class='connection'>" +
               "<strong>" + statushtml + "</strong>" +
                "</p>" +
               "<span class='badge badge-secondary'style='bottom:10px;'>Team Owner</span>" +
           "</div>" +
         "</div>";
                    }
                    else
                    {
                        cardHtml = "<div class='card'>" +
          "<img src='" + photourl + "' class='avatar' style='width: 40px; height: 40px;' />" +
          "<div class='info'>" +
              "<div class='name-row'>" +
                  "<h2><a href='/V1/Member/Default.aspx?userId=" + teamowner + "' style='color: black; font-size: 15px;'>" + teamownerfullname + "</a></h2>" +
                  "<span class='badgge'>" + icon + "</span>" +
                  "<span class='badgge'>" + finalTeamLogo + "</span>" +
              "</div>" +
              "<p class='location'>" + teamownerAddress + "</p>" +
              "<p class='connection'>" +
               "</p>" +
              "<span class='badge badge-secondary'style='bottom:10px;'>Team Owner</span>" +
          "</div>" +
           "</div>";
                    }
                }
                int teamAdministratorCount = 0;
                if (teamowner !=null)
                {
                     teamAdministratorCount = 1;
                }
                var teamAdministratoUserId = (from uo in dc.UserOrganizations
                                              join p in dc.Profiles on uo.UserId equals p.UserId
                                              where uo.OrganizationId == new Guid(organizationId)
                                                    && uo.IsTeamAdministrator == true
                                                    && uo.IsEnabled == true
                                              orderby p.Firstname + " " + p.Lastname
                                              select uo.UserId).ToList();

                string allTeamAdminCards = "";
                string connectionHtml1 = "";

                foreach (var userId in teamAdministratoUserId)
                {
                    string teamlogoo = "";
                    string teamlogoo1 = "";
                    string teamlogoo2 = "";
                    string icons = "";
                    var teamAdministratorfullname = dc.Profiles
                        .Where(p => p.UserId == userId)
                        .Select(p => p.Firstname + " " + p.Lastname)
                        .FirstOrDefault();

                    var teamAdministratorAddress = dc.Profiles
                        .Where(p => p.UserId == userId)
                        .Select(p => p.State + "," + p.City)
                        .FirstOrDefault();

                    var teamAdministratoprofilePhoto = (from p in dc.Photos
                                                        join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                                        where ph.UserId == userId
                                                        orderby p.CreatedOn descending
                                                        select p).Take(1).SingleOrDefault();
                    bool stabiltyverfiyy = dc.Profiles
                   .Any(p => p.UserId == userId && p.IsDisasterReadyCertified);
                    if (stabiltyverfiyy == true)
                    {
                        icons = "<a href='/V1/Member/Default.aspx?userId=" + userId + "'>" +
                                          "<img src='/Impactoid/Images/Logos/purplebadge.png' style='width: 20px; height: 20px; margin-left:-2px; ' data-toggle='tooltip' title='Stability Verified'/>" +
                                        "</a>";
                    }
                    var isprimaryorgg = (from o in dc.Organizations
                                         join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                                         where uo.UserId == userId && uo.IsPrimary == true && uo.IsEnabled == true
                                         select new
                                         {
                                             o.LogoSquare,
                                             o.OrganizationId,
                                             o.Name,
                                             uo.ShowTeamLogo,
                                         }).FirstOrDefault();

                    if (isprimaryorgg != null)
                    {
                        string primarytitle = isprimaryorgg.Name + " Verified";
                        teamlogoo = "<a href='/V1/NonProfit/Default.aspx?organizationId=" + isprimaryorgg.OrganizationId + "'>" +
                                   "<img src='" +
                                   (!string.IsNullOrEmpty(isprimaryorgg.LogoSquare)
                                       ? "/Impactoid/Images/Logos/" + isprimaryorgg.LogoSquare
                                       : "/V1/Images/DefaultLogo.png") +
                                   "' style='width: 20px; height: 20px; margin-left:-2px;' data-toggle='tooltip' title='" + primarytitle + "' />" +
                                   "</a>";
                    }

                    Guid orggId;
                    if (Guid.TryParse(organizationId, out orggId))
                    {
                        var userOrg = dc.UserOrganizations
                                        .FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == orgId && uo.IsEnabled == true);

                        if (userOrg != null)
                        {
                            var orgUser = dc.Organizations
                                            .Where(o => o.OrganizationId == orgId)
                                            .Select(o => new
                                            {
                                                o.LogoSquare,
                                                o.OrganizationId,
                                                o.Name,
                                                userOrg.ShowTeamLogo
                                            }).FirstOrDefault();

                            if (orgUser != null && orgUser.ShowTeamLogo == true)
                            {
                                string primarytitle1 = orgUser.Name + " Verified";
                                teamlogoo1 = "<a href='/V1/NonProfit/Default.aspx?organizationId=" + orgUser.OrganizationId + "'>" +
                                            "<img src='" +
                                            (!string.IsNullOrEmpty(orgUser.LogoSquare)
                                                ? "/Impactoid/Images/Logos/" + orgUser.LogoSquare
                                                : "/V1/Images/DefaultLogo.png") +
                                            "' style='width: 20px; height: 20px; margin-left:-2px;' data-toggle='tooltip' title='" + primarytitle1 + "' />" +
                                            "</a>";
                            }
                        }
                    }
                    else
                    {
                        var orgUserr = (from o in dc.Organizations
                                        join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
                                        where uo.UserId == userId && uo.ShowTeamLogo == true && uo.IsEnabled == true
                                        orderby o.CreatedOn descending
                                        select new
                                        {
                                            o.LogoSquare,
                                            o.OrganizationId,
                                            o.Name,
                                            uo.ShowTeamLogo,
                                        }).FirstOrDefault();

                        if (orgUserr != null && orgUserr.ShowTeamLogo == true)
                        {
                            string primarytitle2 = orgUserr.Name + " Verified";
                            teamlogoo2 = "<a href='/V1/NonProfit/Default.aspx?organizationId=" + orgUserr.OrganizationId + "'>" +
                                        "<img src='" +
                                        (!string.IsNullOrEmpty(orgUserr.LogoSquare)
                                            ? "/Impactoid/Images/Logos/" + orgUserr.LogoSquare
                                            : "/V1/Images/DefaultLogo.png") +
                                        "' style='width: 20px; height: 20px; margin-left:-2px;' data-toggle='tooltip' title='" + primarytitle2 + "' />" +
                                        "</a>";
                        }
                    }

                    string finalTeamLogoo = !string.IsNullOrEmpty(teamlogoo) ? teamlogoo :
                                           !string.IsNullOrEmpty(teamlogoo1) ? teamlogoo1 :
                                           teamlogoo2;

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
                    string statushtml1 = "";
                    if (userId != currentUserId)
                    {
                        var UserUser = (from uu in dc.UserUsers
                                        join uus in dc.UserUserStatus on uu.UserUserStatusId equals uus.UserUserStatusId
                                        where
                                        (uu.RequestingUserId == currentUserId &&
                                        uu.AcceptingUserId == userId)
                                        ||
                                        (uu.RequestingUserId == userId &&
                                        uu.AcceptingUserId == currentUserId)
                                        select new { uus.Status }).Take(1).SingleOrDefault();
                        if (UserUser != null)
                        {
                            if (UserUser.Status == "Connected")
                            {
                                statushtml1 = "Friends";
                            }
                            else if (UserUser.Status == "Pending")
                            {
                                statushtml1 = " Connection Pending";
                            }
                            else if (UserUser.Status == "Delete")
                            {
                                statushtml1 = "Connection Delete";
                            }
                            else if (UserUser.Status == "Blocked")
                            {
                                statushtml1 = "Connection Blocked";
                            }
                        }
                    }
                    string photoUrl = VirtualPathUtility.ToAbsolute(virtualPath);
                    if (userId != currentUserId && statushtml1 == "")
                    {
                        connectionHtml1 = "<p class='connection'>" +
                                 "<a href=''id='btnSendRequest' data-userid='" + userId + "' data-senderid='" + currentUserId + "'>Send Connection Request</a>" +
                               "</p>";
                    }
                    if (connectionHtml1 != "" && statushtml1 == "")
                    {
                        allTeamAdminCards +=
            "<div class='card'>" +
             "<img src='" + photoUrl + "' class='avatar' style='width: 40px; height: 40px;margin-bottom:30px' />" +
             "<div class='info'>" +
                 "<div class='name-row'>" +
                     "<h2><a href='/V1/Member/Default.aspx?userId=" + userId + "' style='color: black; font-size: 15px;'>" + teamAdministratorfullname + "</a></h2>" +
                      "<span class='badgge'>" + icons + "</span>" +
                       "<span class='badgge'>" + finalTeamLogoo + "</span>" +
                 "</div>" +
                 "<p class='location'>" + teamAdministratorAddress + "</p>" +
                 connectionHtml1 +
             "</div>" +
         "</div>";
                    }
                    else if (statushtml1 != null && userId != currentUserId)
                    {
                        allTeamAdminCards +=
"<div class='card'>" +
"<img src='" + photoUrl + "' class='avatar' style='width: 40px; height: 40px;margin-bottom:30px' />" +
"<div class='info'>" +
    "<div class='name-row'>" +
        "<h2><a href='/V1/Member/Default.aspx?userId=" + userId + "' style='color: black; font-size: 15px;'>" + teamAdministratorfullname + "</a></h2>" +
         "<span class='badgge'>" + icons + "</span>" +
          "<span class='badgge'>" + finalTeamLogoo + "</span>" +
    "</div>" +
    "<p class='location'>" + teamAdministratorAddress + "</p>" +
           "<p class='connection'>" +
"<strong>" + statushtml1 + "</strong>" +
"</p>" +
"</div>" +
"</div>";
                    }
                    else
                    {
                        allTeamAdminCards +=
                              "<div class='card'>" +
                               "<img src='" + photoUrl + "' class='avatar' style='width: 40px; height: 40px;' />" +
                               "<div class='info'>" +
                                   "<div class='name-row'>" +
                                       "<h2><a href='/V1/Member/Default.aspx?userId=" + userId + "' style='color: black; font-size: 15px;'>" + teamAdministratorfullname + "</a></h2>" +
                                        "<span class='badgge'>" + icons + "</span>" +
                                         "<span class='badgge'>" + finalTeamLogoo + "</span>" +
                                   "</div>" +
                                   "<p class='location'>" + teamAdministratorAddress + "</p>" +
                                          "<p class='connection'>" +
                              "</p>" +
                               "</div>" +
                           "</div>";
                    }
                    teamAdministratorCount++;
                }

                ltTeamAdministrators.Text = "<div class='card-container'>" + cardHtml + allTeamAdminCards + "</div>";
                hfTeamAdminCount.Value = teamAdministratorCount.ToString();
            }
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
