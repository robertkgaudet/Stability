using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_AvailableResources : BaseWebForm
{
	public string _logo;
	public string _teamName;
	public string _teamSquareLogo;
	public string _teamDescription;
	public string _pageName;
	public string _organizationId;
	public string _nonProfitDropDown;
	public string _coverImage;
	public string organizationId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamFooter.PageName = "availableResourcesPage";
		ucTeamHeader.PageName = "Available Resources";

		#region HEADER PROPERTIES
		////////////////////////
		//BEGIN HEADER PROPERTIES
		////////////////////////
		///
		organizationId = Request.QueryString["organizationId"];
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		_coverImage = causePhotoFolder + "businesscoverimage.png";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select new { o.Name, o.LogoSquare, o.Description, o.Logo, o.CoverImage, o.URLFriendlyName }).SingleOrDefault();

		string squareLogo = string.Empty;
		if (organization != null)
		{
			if (organization.CoverImage != null)
			{
			//	_coverImage = causePhotoFolder + organization.CoverImage;
			}

			ucTeamHeader.CoverImage = _coverImage;
			ucTeamHeader.TeamDescription = organization.Description;
			ucTeamHeader._teamTitle = organization.Name;
			ucTeamHeader.URLFriendlyPageName = organization.URLFriendlyName;

			if (!String.IsNullOrEmpty(organization.LogoSquare))
			{
				squareLogo = "/Impactoid/Images/Logos/" + organization.LogoSquare;
			}
			else
			{
				squareLogo = "/V1/Images/Logo-Placeholder.png";
			}

			Master.PageTitle = organization.Name + " Programs on Stability";
			Master.PageDescription = organization.Description;
			Master.FbDescription = organization.Description;
			Master.FbImage = _coverImage;
			Master.FbSite_name = organization.Name + " Programs on Stability";
		}

		ucTeamFooter.TeamName = organization.Name;
		ucTeamFooter.OrganizationId = organizationId;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.TeamLogo = squareLogo;
		Master.FbImageType = "image/jpg";
		Master.FbURL = Request.Url.AbsoluteUri;

		//ucTeamHeader.Logo = logo;
		//ucTeamHeader.OrganizationId = organizationId;
		//ucTeamHeader.PageName = "Programs";
		//ucTeamHeader.TeamDescription = organization.Description;
		//ucTeamHeader.TeamName = organization.Name;
		//ucTeamHeader.TeamSquareLogo = squareLogo;

		bool isOwner = false;
		if (User.Identity.IsAuthenticated == true)
		{
			var userOrganizationOwner = (from uo in dc.UserOrganizations
										 join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
										 where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
										 && uo.OrganizationId == new Guid(organizationId) && uo.IsEnabled == true
                                         select o).Take(1).SingleOrDefault();

			if (userOrganizationOwner != null)
			{
				if ((userOrganizationOwner.OwnerId != userId))
				{
					isOwner = true;
				}
			}
		}

		////////////////////////
		//END HEADER PROPERTIES
		////////////////////////
		#endregion







		string resourceList = string.Empty;

		var resources = (from us in dc.UserResources
						 join s in dc.Resources on us.ResourceId equals s.ResourceId
						 join uo in dc.UserOrganizations on us.UserId equals uo.UserId
						 where uo.OrganizationId == new Guid(organizationId) && uo.IsEnabled == true
                         orderby s.Type, s.Name
						 group s by s.Name + "|" + s.ResourceId + "| (" + s.Type + ")" into resourceGroup
						 select new { Name = resourceGroup.Key, ResourceCount = resourceGroup.Count() }).Distinct();

		if (resources.Count() > 0)
		{
			foreach (var resource in resources)
			{
				if (!String.IsNullOrEmpty(resource.Name))
				{
					string[] resourceValues = resource.Name.Split('|');
					string resourceName = resourceValues[0];
					string resourceId = resourceValues[1];
					string resourceType = resourceValues[2];

					resourceList += "<button type=\"button\" id=\"skillButton\" onclick=\"window.location.href='/V1/NonProfit/People.aspx?organizationId=" + organizationId + "&resourceId=" + resourceId + "'\" class=\"btn btn-default m-sm\">" + resourceName + " " + resourceType + " (" + resource.ResourceCount + " Members Matched) </button> </br>";
				}
			}
		}

		litAvailableResources.Text = resourceList;
	}
}