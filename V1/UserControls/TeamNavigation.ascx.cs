using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_TeamNavigation : System.Web.UI.UserControl
{
	public string _pageName;
	public string _teamName; 
	public string _teamPageActive;
	public string _reportPageActive;
	public string _deploymentPageActive;
	public string _activityPageActive;
	public string _programsPageActive;
	public string _peoplePageActive;
	public string _websitePageActive;
	public string _ticketPageActive;
	public string _settingsPageActive;
	public string _supportPageActive;

	protected void Page_Load(object sender, EventArgs e)
	{
		string organizationId = Request.QueryString["organizationId"];
		hypPrograms.NavigateUrl = "/V1/NonProfit/Programs.aspx?organizationId=" + organizationId;
		hypDeployments.NavigateUrl = "/V1/NonProfit/Deployments.aspx?organizationId=" + organizationId;
		hypTeamName.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + organizationId;
		hypActivity.NavigateUrl = "/V1/NonProfit/ActivityDashboard.aspx?organizationId=" + organizationId;
		hypPeople.NavigateUrl = "/V1/NonProfit/People.aspx?organizationId=" + organizationId;
		hypSettings.NavigateUrl = "/V1/NonProfitAdministration/Settings.aspx?organizationId=" + organizationId;
		hypSupport.NavigateUrl = "/V1/NonProfit/Support.aspx?organizationId=" + organizationId;
		hypTickets.NavigateUrl = "/V1/NonProfitAdministration/Tickets.aspx?organizationId=" + organizationId;
		hypReports.NavigateUrl = "/V1/NonProfitAdministration/Reports.aspx?organizationId=" + organizationId;
		hypWebsite.NavigateUrl = "/Impactoid/CommunityPage.aspx?organizationId=" + organizationId;
		litTeamName.Text = _teamName;

		switch (PageName)
		{
			case "teamPage":
				_teamPageActive = "class=\"active\"";
				break;
			case "reportPage":
				_reportPageActive = "class=\"active\"";
				break;
			case "deploymentPage":
				_deploymentPageActive = "class=\"active\"";
				break;
			case "activityPage":
				_activityPageActive = "class=\"active\"";
				break;
			case "programsPage":
				_programsPageActive = "class=\"active\"";
				break;
			case "peoplePage":
				_peoplePageActive = "class=\"active\"";
				break;
			case "websitePage":
				_websitePageActive = "class=\"active\"";
				break;
			case "ticketPage":
				_ticketPageActive = "class=\"active\"";
				break;
			case "settingsPage":
				_settingsPageActive = "class=\"active\"";
				break;
			case "supportPage":
				_supportPageActive = "class=\"active\"";
				break;
			default:
				break;
		}
		hypPeople.Visible = false;
		if (HttpContext.Current.User.Identity.IsAuthenticated)
		{
			if(HttpContext.Current.User.IsInRole("Administrator"))
			{
				ulAdmin.Visible = true;
				hrAdmin.Visible = true;
				hypPeople.Visible = true;
			}
		}

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
}