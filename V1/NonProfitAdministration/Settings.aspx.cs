using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfitAdministration_Settings : BaseWebForm
{
	public string organizationId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		organizationId = Request.QueryString["organizationId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select new { o.Name, o.HideTeamList, o.Description, o.Logo, o.CoverImage, o.RespondToTickets }).SingleOrDefault();

		Master.PageTitle = organization.Name + " Settings on Stability";
		Master.PageDescription = organization.Description;
		Master.FbDescription = organization.Description;
		Master.FbImage = organization.CoverImage;
		Master.FbImageType = "image/jpg";
		Master.FbSite_name = organization.Name + " Setting on Stability";
		Master.FbURL = Request.Url.AbsoluteUri;

		ucTeamNavigation.PageName = "settingsPage";
		ucTeamNavigation.TeamName = organization.Name;

		string logo = string.Empty;
		if (!String.IsNullOrEmpty(organization.Logo))
		{
			logo = "/Impactoid/Images/Logos/" + organization.Logo;
		}
		else
		{
			//Use placeholder image.imgLogo.Visible = true;
			logo = "/V1/Images/Logo-Placeholder.png";
		}

		ucTeamNavigation.TeamName = organization.Name;
		ucTeamHeader.Logo = logo;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.PageName = "Settings";
		ucTeamHeader.TeamDescription = organization.Description;
		ucTeamHeader.TeamName = organization.Name;

		if(!IsPostBack)
		{
			bool respondToTickets = organization.RespondToTickets != null ? (bool)organization.RespondToTickets : false;
			bool hideTeamList = organization.HideTeamList != null ? (bool)organization.HideTeamList : false;
			chkEnableTicketing.Checked = respondToTickets;
			chkHideTeamList.Checked = hideTeamList;
		}
		else
		{ divUpdateMessage.Visible = true; }
	}

	protected void btnSubmit_Click(object sender, EventArgs e)
	{
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = (from o in dc.Organizations
						   where o.OrganizationId == new Guid(organizationId)
						   select o).SingleOrDefault();

		organization.RespondToTickets = chkEnableTicketing.Checked;
		organization.HideTeamList = chkHideTeamList.Checked;

		dc.SubmitChanges();
		divUpdateMessage.Visible = true;
	}
}