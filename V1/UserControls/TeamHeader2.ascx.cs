using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
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
            var teamownerr = "<a href='/V1/Member/Default.aspx?userId=" + teamowner + "'>" + teamownerfullname + "</a> <span class='badge badge-secondary' runat='server' visible='false'>Team Owner</span>";
            string profilePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["profilePhotoFolder"].ToString();
            var profilePhoto = (from p in dc.Photos
                                join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                where ph.UserId == teamowner
                                orderby p.CreatedOn descending
                                select p).Take(1).SingleOrDefault();
            lbTeamOwner.Text = teamownerr;
            if (profilePhoto != null)
            {
                imgTeamOwner.ImageUrl = profilePhotoFolder + profilePhoto.FilenameCropped;
            }
            else
            {
                imgTeamOwner.ImageUrl = profilePhotoFolder + "profilepicture.png";
            }
            var teamAdministratorRoleId = dc.aspnet_Roles
        .Where(r => r.RoleName == "Team Administrator")
        .Select(r => r.RoleId)
      .FirstOrDefault();
            var teamAdministratoUserId = (from ur in dc.aspnet_UsersInRoles
                                          join uo in dc.UserOrganizations on ur.UserId equals uo.UserId
                                          where ur.RoleId == teamAdministratorRoleId
                                          && uo.OrganizationId == new Guid(organizationId)
                                          select ur.UserId).FirstOrDefault();
            var teamAdministratorfullname = dc.Profiles.Where(p => p.UserId == teamAdministratoUserId).Select(p => p.Firstname + " " + p.Lastname)
                      .FirstOrDefault();
            var teamAdministrator = "<a href='/V1/Member/Default.aspx?userId=" + teamAdministratoUserId + "'>" + teamAdministratorfullname + "</a> <span class='badge badge-secondary' runat='server' visible='false'>Team Administrator</span>";
            var teamAdministratoprofilePhoto = (from p in dc.Photos
                                                join ph in dc.ProfilePhotos on p.PhotoId equals ph.PhotoId
                                                where ph.UserId == teamAdministratoUserId
                                                orderby p.CreatedOn descending
                                                select p).Take(1).SingleOrDefault();
            lbTeamAdministrator.Text = teamAdministrator;
            if (teamAdministratoprofilePhoto != null)
            {
                imgTeamAdministrator.ImageUrl = profilePhotoFolder + teamAdministratoprofilePhoto.FilenameCropped;
            }
            else
            {
                imgTeamAdministrator.ImageUrl = teamAdministratoprofilePhoto + "profilepicture.png";
            }
        }
        if (!String.IsNullOrEmpty(_organizationId))
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

			var parentOrganization = (from o in dc.Organizations
									  where o.OrganizationId == new Guid(_organizationId)
									  select new { o.Name, o.ParentOrganizationId }).SingleOrDefault();

			if(parentOrganization != null)
			{
				if(parentOrganization.ParentOrganizationId != new Guid(_organizationId))
				{
					var parentOrganizationName = (from o in dc.Organizations
												where o.OrganizationId == parentOrganization.ParentOrganizationId
												  select new { o.Name }).SingleOrDefault();
					if(parentOrganizationName != null )
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
