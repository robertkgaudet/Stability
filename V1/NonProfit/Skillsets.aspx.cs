using Stability;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_Skillsets : BaseWebForm
{
	public string organizationId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		organizationId = Request.QueryString["organizationId"];
		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();

		var organization = (from o in dc.Organizations
						   where o.OrganizationId == new Guid(organizationId)
						   select new {o.Name, o.Description, o.Logo, o.CoverImage}).SingleOrDefault();

		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		Master.PageTitle = organization.Name + " List of Skillsets on Stability";
		Master.PageDescription = organization.Description;
		Master.FbDescription = organization.Description;
		Master.FbImage = causePhotoFolder + organization.CoverImage;
		Master.FbImageType = "image/jpg";
		Master.FbSite_name = organization.Name + " List of Skillsets on Stability";
		Master.FbURL = Request.Url.AbsoluteUri;

		ucTeamNavigation.PageName = "skillsetsPage";
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
		ucTeamHeader.PageName = "Skillsets";
		ucTeamHeader.TeamDescription = organization.Description;
		ucTeamHeader.TeamName = organization.Name;


		string skillList = string.Empty;

		var skills = (from us in dc.UserSkills
					 join s in dc.Skills on us.SkillId equals s.SkillId
					 join uo in dc.UserOrganizations on us.UserId equals uo.UserId
					 where uo.OrganizationId == new Guid(organizationId)
					 orderby s.Name
					group s by s.Name +"|"+ s.SkillId into resourceGroup
					select new { Name = resourceGroup.Key, ResourceCount = resourceGroup.Count() }).Distinct();

		foreach(var skill in skills)
		{
			string[] skillValues = skill.Name.Split('|');
			string skillName = skillValues[0];
			string skillId = skillValues[1];

			skillList += "<button type=\"button\" id=\"skillButton\" onclick=\"window.location.href='/V1/NonProfit/People.aspx?organizationId=" + organizationId + "&skillId=" + skillId + "'\" class=\"btn btn-default m-sm\">" + skillName + " " + skill.ResourceCount + "</button>";
			
		}

		litSkillsets.Text = skillList;
	}

}