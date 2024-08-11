using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class V1_NonProfit_Programs : BaseWebForm
{
	public string organizationId = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
		ucTeamNavigation.PageName = "programsPage";
		organizationId = Request.QueryString["organizationId"];

		CrowdReliefDBDataContext dc = new CrowdReliefDBDataContext();
		var programs = from p in dc.Programs
					   join op in dc.OrganizationPrograms on p.ProgramId equals op.ProgramId
					   join o in dc.Organizations on op.OrganizationId equals o.OrganizationId
					   where op.OrganizationId == new Guid(organizationId)
					   orderby p.Order
					   select new { p.Name, p.Description, o.Logo, p.IsDeploymentRequired, p.IsRemoteOnly, p.IsTrainingRequired, p.IsShared, p.ProgramId };

		rptPrograms.DataSource = programs;
		rptPrograms.DataBind();

		var organization = (from o in dc.Organizations
							where o.OrganizationId == new Guid(organizationId)
							select new { o.Name, o.LogoSquare, o.Description, o.Logo, o.CoverImage }).SingleOrDefault();

		ucTeamNavigation.TeamName = organization.Name;

		string causePhotoFolder = System.Configuration.ConfigurationManager.AppSettings["causePhotoFolder"].ToString();
		Master.PageTitle = organization.Name + " Programs on Stability";
		Master.PageDescription = organization.Description;
		Master.FbDescription = organization.Description;
		Master.FbImage = causePhotoFolder  +organization.CoverImage;
		Master.FbImageType = "image/jpg";
		Master.FbSite_name = organization.Name + " Programs on Stability";
		Master.FbURL = Request.Url.AbsoluteUri;


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

		ucTeamHeader.Logo = logo;
		ucTeamHeader.OrganizationId = organizationId;
		ucTeamHeader.PageName = "Programs";
		ucTeamHeader.TeamDescription = organization.Description;
		ucTeamHeader.TeamName = organization.Name;
		ucTeamHeader.TeamSquareLogo = squareLogo;

		bool isOwner = false;
		if (User.Identity.IsAuthenticated == true)
		{
			var userOrganizationOwner = (from uo in dc.UserOrganizations
										 join o in dc.Organizations on uo.OrganizationId equals o.OrganizationId
										 where o.OwnerId == new Guid(Membership.GetUser().ProviderUserKey.ToString())
										 && uo.OrganizationId == new Guid(organizationId)
										 select o).Take(1).SingleOrDefault();

			if (userOrganizationOwner != null)
			{
				if ((userOrganizationOwner.OwnerId != userId))
				{
					isOwner = true;
				}
			}
		}
		if (User.IsInRole("Administrator") || isOwner)
		{
			hypEditPrograms.Visible = true;
			hypEditPrograms.NavigateUrl = "/V1/NonProfitAdministration/EditNonProfitProgram.aspx?organizationId=" + organizationId;
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

			Literal litProgramName = (Literal)e.Item.FindControl("litProgramName");
			Literal litProgramDescription = (Literal)e.Item.FindControl("litProgramDescription");
			Literal litRemoteWork = (Literal)e.Item.FindControl("litRemoteWork");
			Literal litRequiresDeployment = (Literal)e.Item.FindControl("litRequiresDeployment");
			Literal litRequiresTraining = (Literal)e.Item.FindControl("litRequiresTraining");
			Literal litSharedPrivate =  (Literal)e.Item.FindControl("litSharedPrivate");
			Image imgLogo = (Image)e.Item.FindControl("imgLogo");

			string sharedIcon = string.Empty;
			litSharedPrivate.Text = "<span class=\"label label-primary pull-right\">TEAM PROGRAM</span>";
			if (isShared)
			{
				sharedIcon = "<i class=\"fa fa-paper-plane\"></i>";
				litSharedPrivate.Text = "<span class=\"label label-success pull-right\">GLOBAL PROGRAM</span>";
			}
			HyperLink hypEditPrograms = (HyperLink)e.Item.FindControl("hypEditPrograms");

			litProgramDescription.Text = programDescription;
			litProgramName.Text = programName + " " + sharedIcon;
			litRemoteWork.Text = isRemoteOnly ? "Remote work requred." : "No remote work.";
			litRequiresDeployment.Text = isDeploymentRequired ? "Requires deployment." : "Deployment not required.";
			litRequiresTraining.Text = isTrainingRequired ? "Specialized training is required." : "Training not required.";
			imgLogo.ImageUrl = "/Impactoid/Images/Logos/" + logo;

			if (User.IsInRole("Administrator"))
			{
				hypEditPrograms.Visible = true;
				hypEditPrograms.NavigateUrl = "/V1/NonProfitAdministration/EditNonProfitProgram.aspx?organizationId=" + organizationId + "&programId=" + programId;
			}
		}
	}
}