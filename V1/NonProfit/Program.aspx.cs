using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_Program : BaseWebForm
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
	public string programId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamFooter.PageName = "programsPage";
		ucTeamHeader.PageName = "Programs";

		#region HEADER PROPERTIES
		////////////////////////
		//BEGIN HEADER PROPERTIES
		////////////////////////

		organizationId = Request.QueryString["organizationId"];
		programId = Request.QueryString["programId"];
		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		_coverImage = causePhotoFolder + "businesscoverimage.png";
		hypAllPrograms.NavigateUrl = "/V1/NonProfit/Programs.aspx?organizationId=" + organizationId;

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
										 where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString()) && uo.IsEnabled == true
                                         && uo.OrganizationId == new Guid(organizationId)
										 select o).Take(1).SingleOrDefault();

			if (userOrganizationOwner != null)
			{
				if ((userOrganizationOwner.OwnerId == userId))
				{
					isOwner = true;
				}
			}
		}

		////////////////////////
		//END HEADER PROPERTIES
		////////////////////////
		#endregion

		var program = (from p in dc.Programs
					   join op in dc.OrganizationPrograms on p.ProgramId equals op.ProgramId
					   join o in dc.Organizations on op.OrganizationId equals o.OrganizationId
					   where p.ProgramId == new Guid(programId)
					   orderby p.Order
					   select new { p.Name, p.IsShared, p.Description, o.OrganizationId, o.Logo, p.IsDeploymentRequired, p.IsRemoteOnly, p.IsTrainingRequired, p.ProgramId }).SingleOrDefault();



		string sharedIcon = string.Empty;
		litSharedPrivate.Text = "<span class=\"label label-primary pull-right\">TEAM PROGRAM</span>";
		if (Convert.ToBoolean(program.IsShared))
		{
			sharedIcon = "<i class=\"fa fa-paper-plane\"></i>";
			litSharedPrivate.Text = "<span class=\"label label-success pull-right\">GLOBAL PROGRAM</span>";
		}

		litProgramDescription.Text = program.Description;
		hypProgramName.Text = program.Name + " " + sharedIcon;
		hypProgramName.NavigateUrl = "/V1/NonProfit/Program.aspx?programId=" + programId + "&organizationId=79305f85-3816-46a8-911f-0d7e3e227c32";
		litRemoteWork.Text = program.IsRemoteOnly ? "Remote work requred." : "No remote work.";
		litRequiresDeployment.Text = program.IsDeploymentRequired ? "Requires deployment." : "Deployment not required.";
		litRequiresTraining.Text = program.IsTrainingRequired ? "Specialized training is required." : "Training not required.";
		imgLogo.ImageUrl = "/Impactoid/Images/Logos/" + program.Logo;

		if (User.IsInRole("Administrator") || isOwner)
		{
			hypEditPrograms.Visible = true;
			hypEditPrograms.NavigateUrl = "/V1/NonProfitAdministration/EditNonProfitProgram.aspx?organizationId=" + organizationId + "&programId=" + programId;
		}


		var positions = from p in dc.Positions
						join pp in dc.ProgramPositions on p.PositionId equals pp.PositionId
						where p.OrganizationId == program.OrganizationId
						&& p.IsDeleted == false
						&& pp.ProgramId == new Guid(programId)
						orderby p.Name
						select new { p.Name, p.PositionId, pp.ProgramId, p.OrganizationId };

		dlPositions.DataSource = positions;
		dlPositions.DataBind();

		if (User.IsInRole("Administrator") || isOwner)
		{
			hypAddPrograms.Visible = true;
			hypAddPrograms.NavigateUrl = "/V1/NonProfitAdministration/EditNonProfitProgram.aspx?organizationId=" + organizationId;
		}
	}

	protected void rptPrograms_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			RepeaterItem dataItem = (RepeaterItem)e.Item;
			string programName = (string)DataBinder.Eval(dataItem.DataItem, "Name");
			string programDescription = (string)DataBinder.Eval(dataItem.DataItem, "Description");
			string logo = (string)DataBinder.Eval(dataItem.DataItem, "Logo");
			bool isDeploymentRequired = (bool)DataBinder.Eval(dataItem.DataItem, "IsDeploymentRequired");
			bool isRemoteOnly = (bool)DataBinder.Eval(dataItem.DataItem, "IsRemoteOnly");
			bool isTrainingRequired = (bool)DataBinder.Eval(dataItem.DataItem, "IsTrainingRequired");
			bool isShared = (bool)DataBinder.Eval(dataItem.DataItem, "IsShared");
			Guid programId = (Guid)DataBinder.Eval(dataItem.DataItem, "ProgramId");
			Guid organizationId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationId");

			HyperLink HyperLink = (HyperLink)e.Item.FindControl("hypProgramName");
			Literal litProgramDescription = (Literal)e.Item.FindControl("litProgramDescription");
			Literal litRemoteWork = (Literal)e.Item.FindControl("litRemoteWork");
			Literal litRequiresDeployment = (Literal)e.Item.FindControl("litRequiresDeployment");
			Literal litRequiresTraining = (Literal)e.Item.FindControl("litRequiresTraining");
			Literal litSharedPrivate = (Literal)e.Item.FindControl("litSharedPrivate");
			Image imgLogo = (Image)e.Item.FindControl("imgLogo");
			Repeater dlPositions = (Repeater)e.Item.FindControl("dlPositions");

			string sharedIcon = string.Empty;
			litSharedPrivate.Text = "<span class=\"label label-primary pull-right\">TEAM PROGRAM</span>";
			if (isShared)
			{
				sharedIcon = "<i class=\"fa fa-paper-plane\"></i>";
				litSharedPrivate.Text = "<span class=\"label label-success pull-right\">GLOBAL PROGRAM</span>";
			}
			HyperLink hypEditPrograms = (HyperLink)e.Item.FindControl("hypEditPrograms");

			litProgramDescription.Text = programDescription;
			HyperLink.Text = programName + " " + sharedIcon;
			HyperLink.NavigateUrl = "/V1/NonProfit/Programs.aspx?programId=" + programId + "&organizationId=79305f85-3816-46a8-911f-0d7e3e227c32";
			litRemoteWork.Text = isRemoteOnly ? "Remote work requred." : "No remote work.";
			litRequiresDeployment.Text = isDeploymentRequired ? "Requires deployment." : "Deployment not required.";
			litRequiresTraining.Text = isTrainingRequired ? "Specialized training is required." : "Training not required.";
			imgLogo.ImageUrl = "/Impactoid/Images/Logos/" + logo;

			if (User.IsInRole("Administrator"))
			{
				hypEditPrograms.Visible = true;
				hypEditPrograms.NavigateUrl = "/V1/NonProfitAdministration/EditNonProfitProgram.aspx?organizationId=" + organizationId + "&programId=" + programId;
			}


			CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
			var positions = from p in dc.Positions
							join pp in dc.ProgramPositions on p.PositionId equals pp.PositionId
							where p.OrganizationId == organizationId
							&& p.IsDeleted == false
							&& pp.ProgramId == programId
							orderby p.Name
							select new { p.Name, p.PositionId, pp.ProgramId, p.OrganizationId };

			dlPositions.DataSource = positions;
			dlPositions.DataBind();
		}
	}

	protected void dlPositions_ItemDataBound(object sender, RepeaterItemEventArgs e)
	{
		if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
		{
			HyperLink hypPosition = (HyperLink)e.Item.FindControl("hypPosition");

			RepeaterItem dataItem = (RepeaterItem)e.Item;

			//Total count of items and total cost.
			string positionName = (string)DataBinder.Eval(dataItem.DataItem, "Name");
			Guid positionId = (Guid)DataBinder.Eval(dataItem.DataItem, "PositionId");
			Guid programId = (Guid)DataBinder.Eval(dataItem.DataItem, "ProgramId");
			Guid organizationId = (Guid)DataBinder.Eval(dataItem.DataItem, "OrganizationId");

			hypPosition.NavigateUrl = "/V1/NonProfit/TeamRole.aspx?programId=" + programId + "&organizationId=" + organizationId + "&positionId=" + positionId.ToString();
			hypPosition.Text = positionName;
		}
	}
	protected void imgLogo_Click(object sender, ImageClickEventArgs e)
	{
		ImageButton button = (ImageButton)sender;
		string parameter = button.CommandArgument;

		Response.Redirect("/V1/NonProfit/Default.aspx?organizationId=" + parameter);
	}
}