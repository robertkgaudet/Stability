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

	protected void Page_Load(object sender, EventArgs e)
	{
		imgTeamLogo.ImageUrl = _teamLogo;
		litTitle.Text = _teamTitle;
		litMemberDescription.Text = _teamDescription;
		litPageName.Text = _pageName;

		ucTeamNavigation.PageName = _pageName;
		ucTeamNavigation.TeamName = _teamName;
		ucTeamNavigation.organizationId = _organizationId;
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
	public string URLFrijggjjPageName
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
		set { _teamTitle =value; }
	}
	public string TeamDescription
	{
		get { return _teamDescription; }
		set { _teamDescription = value; }
	}
}
