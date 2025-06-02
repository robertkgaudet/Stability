using Microsoft.SqlServer.Server;
using Stability;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_TeamHeader : System.Web.UI.UserControl
{
	public string _logo;
	public string _teamName;
	public string _teamSquareLogo;
	public string _teamDescription;
	public string _pageName;
	public string _organizationId;
	public string _nonProfitDropDown;
	public string _coverImage;

	protected void Page_Load(object sender, EventArgs e)
	{
		litBreadcrumbPageName.Text = _pageName;
		litTeamDescription.Text = _teamDescription;
		litTeamName.Text = _teamName;
		hypBreadcrumbTeamName.Text = _teamName;
		hypBreadcrumbTeamName.NavigateUrl = "/V1/NonProfit/Stream.aspx?organizationId=" + _organizationId;
		imgTeamLogo.ImageUrl = _teamSquareLogo;

		LoadNonProfits();

		_organizationId = Request.QueryString["organizationId"];
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = (from o in dc.Organizations
						   where o.OrganizationId == new Guid(_organizationId)
						   select new { o.CoverImage }).SingleOrDefault();

		hypTeamWebsite.NavigateUrl = "/Impactoid/CommunityPage.aspx?organizationId=" + _organizationId;
		hypTeamWebsite.Text = " Open Team Website";
		hypWebsite.NavigateUrl = "/Impactoid/CommunityPage.aspx?organizationId=" + _organizationId;
		_coverImage = causePhotoFolder + "/coverplaceholder.png";
		if (!string.IsNullOrEmpty(organization.CoverImage ))
		{ 
			_coverImage = causePhotoFolder + organization.CoverImage;
		}
	}

	public void LoadNonProfits()
	{

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var nonProfits = from o in dc.Organizations
						 where o.IsActive == true
						 orderby o.Name
						 select new { o };

		foreach (var nonProfit in nonProfits)
		{
			DateTime currentDate = DateTime.Now;
			TimeSpan difference = currentDate - nonProfit.o.CreatedOn;
			string newFlag = string.Empty;
			if (difference.TotalDays < 30)
			{
				newFlag = "<i class=\"label label-success pull-right inline\">NEW</i>";
			}
			_nonProfitDropDown += "<li id=\"" + nonProfit.o.OrganizationId + "\"><a href=\"#\">" + nonProfit.o.Name + " " + newFlag + "</a></li>" + Environment.NewLine;
		}
	}

	public string CoverImage
	{
		get { return _coverImage; }
		set { _coverImage = value; }
	}
	public string Logo
	{
		get { return _logo; }
		set { _logo = value; }
	}
	public string TeamSquareLogo
	{
		get { return _teamSquareLogo; }
		set { _teamSquareLogo = value; }
	}
	

	public string TeamName
	{
		get { return _teamName; }
		set { _teamName = value; }
	}

	public string TeamDescription
	{
		get { return _teamDescription; }
		set { _teamDescription = value; }
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
}