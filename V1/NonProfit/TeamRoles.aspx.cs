using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_TeamRoles : BaseWebForm
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
	public string jsonEvents = string.Empty;
	public string availableDates = string.Empty;
	public string teamCounts = string.Empty;
	public string programId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamFooter.PageName = "teamRolesPage";
		ucTeamHeader.PageName = "Team Roles";

		#region HEADER PROPERTIES
		////////////////////////
		//BEGIN HEADER PROPERTIES
		////////////////////////

		organizationId	= Request.QueryString["organizationId"];
		programId		= Request.QueryString["programId"];
		
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		_coverImage = causePhotoFolder + "businesscoverimage.png";

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select new { o.Name, o.LogoSquare, o.Description, o.Logo, o.CoverImage, o.URLFriendlyName }).SingleOrDefault();

        string squareLogo = "/V1/Images/Logo-Placeholder.png";
        if (organization != null)
		{
			if (!string.IsNullOrEmpty(organization.CoverImage))
			{
				_coverImage = causePhotoFolder + organization.CoverImage;
			}

			ucTeamHeader.CoverImage = _coverImage;
			ucTeamHeader.TeamDescription = organization.Description;
			ucTeamHeader._teamTitle = organization.Name;

            if (!string.IsNullOrEmpty(organization.LogoSquare))
            {
                string virtualPath_square = "/Impactoid/Images/Logos/" + organization.LogoSquare;
                string physicalPath_square = Server.MapPath(virtualPath_square);

                if (System.IO.File.Exists(physicalPath_square))
                {
                    squareLogo = virtualPath_square;
                }
            }

            Master.PageTitle = organization.Name + " Programs on Stability";
			Master.PageDescription = organization.Description;
			Master.FbDescription = organization.Description;
			Master.FbImage = _coverImage;
			Master.FbSite_name = organization.Name + " Programs on Stability";
			ucTeamHeader.URLFriendlyPageName = organization.URLFriendlyName;
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
										 && uo.OrganizationId == new Guid(organizationId) && (uo.Status== (int)RequestStatus.Approved || uo.Status == (int)RequestStatus.Pending)
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

		if(!String.IsNullOrEmpty(programId))
		{
			Guid parsedProgramId = new Guid(programId); // Convert programId to Guid

			// Join and filter on ProgramId
			var positions = from p in dc.Positions
							join pp in dc.ProgramPositions on p.PositionId equals pp.PositionId
							where pp.ProgramId == parsedProgramId
							select new { p.Name, p.PositionId, pp.ProgramId };

			dlPositions.DataSource = positions;
			dlPositions.DataBind();
		}
		else
		{
			var positions = from p in dc.Positions
							where p.OrganizationId == new Guid(organizationId) 
							&& p.IsDeleted == false
							orderby p.Name
							select new { p.Name, p.PositionId, ProgramId = (Guid?)null };

			dlPositions.DataSource = positions;
			dlPositions.DataBind();
		}
	}

	protected void dlPositions_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			HyperLink hypPosition = (HyperLink)e.Item.FindControl("hypPositionId");
			HyperLink hypGetTrained = (HyperLink)e.Item.FindControl("hypGetTrained");

			RepeaterItem dataItem = (RepeaterItem)e.Item;

			//Total count of items and total cost.
			string positionName =	(string)DataBinder.Eval(dataItem.DataItem, "Name");
			Guid positionId		=	(Guid)DataBinder.Eval(dataItem.DataItem, "PositionId");
			var programId = DataBinder.Eval(e.Item.DataItem, "ProgramId");

			hypGetTrained.NavigateUrl = "/V1/NonProfit/TeamRole.aspx?ProgramId=" + programId + "&organizationId=" + organizationId + "&positionId=" + positionId;
			hypGetTrained.Text = "Get Trained";

			hypPosition.NavigateUrl = "/V1/NonProfit/TeamRole.aspx?ProgramId=" + programId + "&organizationId=" + organizationId + "&positionId=" + positionId.ToString();
			hypPosition.Text = positionName;
		}
	}
}