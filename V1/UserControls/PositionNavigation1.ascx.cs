using System;
using System.Activities.Debugger;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_PositionNavigation1 : System.Web.UI.UserControl
{
	public string _pageName;
	public string _detailsActive;
	public string _positionsActive;
	public string _participantsActive;
	public string _messagesActive;
	public string _reportsActive;
	public string _invitesActive;
	public string _settingsActive;
	public string _moneyActive;
	public string _organizationEventId;
	public string _viewPositionsActive;
	public string _urlFriendlyName;
	public bool _isTeamOwner;
	public string _organizationName;
	public string _organizationId;
	public string _campaignName;
	public string _portalName;
	public string _portalId;
	public string _description;

	protected void Page_Load(object sender, EventArgs e)
	{
		switch (PageName)
		{
			case "ActivePositions":
				_viewPositionsActive = "class=\"activeLink\"";
			break;
			case "Details":
				_detailsActive = "class=\"activeLink\"";
				break;
			case "Positions":
				_positionsActive = "class=\"activeLink\"";
				break;
			case "Participants":
				_participantsActive = "class=\"activeLink\"";
				break;
			case "Messages":
				_messagesActive = "class=\"activeLink\"";
				break;
			case "Reports":
				_reportsActive = "class=\"activeLink\"";
				break;
			case "Invites":
				_invitesActive = "class=\"activeLink\"";
				break;
			case "Settings":
				_settingsActive = "class=\"activeLink\"";
				break;
			case "Money":
				_moneyActive = "class=\"activeLink\"";
				break;
		}

		if(IsTeamOwner || HttpContext.Current.User.IsInRole("Administrator"))
		{
			divNavigation.Visible = true;
		}
		else
		{
			if(PageName != "Details" && PageName != "Positions")
			{
				Response.Redirect("/Cause/" + URLFriendlyName);
			}
		}

		litCampaignName.Text = CampaignName;
		hypEventName.Text = PortalName;
		hypOrganization.Text = OrganizationName;
		hypEventName.NavigateUrl = "/MapZone/" + URLFriendlyName;
		hypOrganization.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + OrganizationId;
	}

	public string OrganizationEventId
	{
		get { return _organizationEventId; }
		set { _organizationEventId = value; }
	}
	public bool IsTeamOwner
	{
		get { return _isTeamOwner; }
		set { _isTeamOwner = value; }
	}
	public string PageName
	{
		get { return _pageName; }
		set { _pageName = value; }
	}
	public string URLFriendlyName
	{
		get { return _urlFriendlyName; }
		set { _urlFriendlyName = value; }
	}
	public string OrganizationName
	{
		get { return _organizationName; }
		set { _organizationName = value; }
	}
	public string OrganizationId
	{
		get { return _organizationId; }
		set { _organizationId = value; }
	}
	public string CampaignName
	{
		get { return _campaignName; }
		set { _campaignName = value; }
	}
	public string PortalName
	{
		get { return _portalName; }
		set { _portalName = value; }
	}
	public string PortalId
	{
		get { return _portalId; }
		set { _portalId = value; }
	}
	public string Description
	{
		get { return _description; }
		set { _description = value; }
	}
}