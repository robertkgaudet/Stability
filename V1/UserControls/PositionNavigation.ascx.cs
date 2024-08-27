using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_PositionNavigation : System.Web.UI.UserControl
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
	protected void Page_Load(object sender, EventArgs e)
	{
		switch (PageName)
		{
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
	}
	
	public string OrganizationEventId
	{
		get { return _organizationEventId; }
		set { _organizationEventId = value; }
	}
	public string PageName
	{
		get { return _pageName; }
		set { _pageName = value; }
	}
}