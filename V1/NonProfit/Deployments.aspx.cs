using Stability;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_Deployments : BaseWebForm
{
	public string organizationId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		organizationId = Request.QueryString["organizationId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = (from o in dc.Organizations
						   where o.OrganizationId == new Guid(organizationId)
						   select new {o.Name, o.LogoSquare, o.Description, o.Logo, o.CoverImage}).SingleOrDefault();

		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		Master.PageTitle = organization.Name + " Deployments on Stability";
		Master.PageDescription = organization.Description;
		Master.FbDescription = organization.Description;
		Master.FbImage = causePhotoFolder + organization.CoverImage;
		Master.FbImageType = "image/jpg";
		Master.FbSite_name = organization.Name + " Deployments on Stability";
		Master.FbURL = Request.Url.AbsoluteUri;

		ucTeamNavigation.PageName = "deploymentPage";
		ucTeamNavigation.TeamName = organization.Name;

		string logo = string.Empty;
		string squareLogo = string.Empty;
		if (!String.IsNullOrEmpty(organization.Logo))
		{
			logo = "/Impactoid/Images/Logos/" + organization.Logo;
		}
		else
		{
			//Use placeholder image.imgLogo.Visible = true;
			logo = "/V1/Images/Logo-Placeholder.png";
		}
		if (!String.IsNullOrEmpty(organization.LogoSquare))
		{
			squareLogo = "/Impactoid/Images/Logos/" + organization.LogoSquare;
		}

		ucTeamNavigation.TeamName = organization.Name;
		ucTeamHeader.Logo = logo;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.PageName = "Deployments";
		ucTeamHeader.TeamDescription = organization.Description;
		ucTeamHeader.TeamName = organization.Name;
		ucTeamHeader.TeamSquareLogo = squareLogo;

		ucDeploymentListCard.OrganizationId = new Guid(organizationId);
	}
}