using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_TeamFooter2 : System.Web.UI.UserControl
{
	public string _pageName;
	public string _teamName;
	public string _organizationId;
	public string _URLFriendlyPageName = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamNavigation.PageName = _pageName;
		ucTeamNavigation.TeamName = _teamName;
		ucTeamNavigation.organizationId = _organizationId;
	}
	public string TeamName
	{
		get { return _teamName; }
		set { _teamName = value; }
	}
	public string PageName
	{
		get { return _pageName; }
		set { _pageName = value; }
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
}