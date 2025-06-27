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
						   where uo.UserId == new Guid(_userId) && (uo.Status== (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
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