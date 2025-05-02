using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_UserControls_MemberNavigation : System.Web.UI.UserControl
{
	public string _userId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
        hypDeployment.Attributes["data-toggle"] = "tooltip";
        hypDeployment.Attributes["title"] = "Choose a team deployment to join";

        hypPortals.Attributes["data-toggle"] = "tooltip";
        hypPortals.Attributes["title"] = "Select a community portal";

        hypTimeSheet.Attributes["data-toggle"] = "tooltip";
        hypTimeSheet.Attributes["title"] = "View my time sheet";
        hypMyProfile.Attributes["data-toggle"] = "tooltip";
        hypMyProfile.Attributes["title"] = "Go to your profile page";

        hypMyTeam.Attributes["data-toggle"] = "tooltip";
        hypMyTeam.Attributes["title"] = "Go to your team's page";

        hypCalendar.Attributes["data-toggle"] = "tooltip";
        hypCalendar.Attributes["title"] = "Select the dates you are available to volunteer";

        hypSkills.Attributes["data-toggle"] = "tooltip";
        hypSkills.Attributes["title"] = "Choose your skills";

        hypResources.Attributes["data-toggle"] = "tooltip";
        hypResources.Attributes["title"] = "Choose the types of resources you can contribute";

        hypIDCard.Attributes["data-toggle"] = "tooltip";
        hypIDCard.Attributes["title"] = "Get your printable ID card";
        hypMyTeam.Visible = false;
		hypDeployments.Visible = false;
		hypMyProfile.Visible = false;
		hypPositions.Visible = false;
		divMemberNavigation.Visible = false;
		if (!String.IsNullOrEmpty(_userId))
		{
			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var orgUser = (from o in dc.Organizations
						   join uo in dc.UserOrganizations on o.OrganizationId equals uo.OrganizationId
						   where uo.UserId == new Guid(_userId)
						   orderby o.CreatedOn descending
						   select o).Take(1).SingleOrDefault();

			if (orgUser != null)
			{
				//Is in a team or not.
				hypPositions.Visible = true;
				hypDeployments.Visible = true;
				hypDeployments.NavigateUrl = "/V1/NonProfit/DeploymentTeams.aspx?organizationId=" + orgUser.OrganizationId.ToString();

				hypMyTeam.Visible = true;
				hypMyTeam.NavigateUrl = "/V1/NonProfit/Default.aspx?organizationId=" + orgUser.OrganizationId.ToString();
			}
			divMemberNavigation.Visible = true;
			hypMyProfile.Visible = true;
		}
	}

	public string UserId
	{
		get { return _userId; }
		set { _userId = value; }
	}
}